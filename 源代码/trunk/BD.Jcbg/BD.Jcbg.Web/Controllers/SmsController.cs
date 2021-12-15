using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Func;


namespace BD.Jcbg.Web.Controllers
{
    public class SmsController : Controller
    {
        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                }
                return _commonService;
            }
        }
        private ISmsService _smsService = null;
        private ISmsService SmsService
        {
            get
            {
                if (_smsService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsService = webApplicationContext.GetObject("SmsService") as ISmsService;
                }
                return _smsService;
            }
        }

        private ISmsServiceWzzjz _smsServiceWzzjz = null;
        private ISmsServiceWzzjz SmsServiceWzzjz
        {
            get
            {
                if (_smsServiceWzzjz == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsServiceWzzjz = webApplicationContext.GetObject("SmsServiceWzzjz") as ISmsServiceWzzjz;
                }
                return _smsServiceWzzjz;
            }
        }
        #endregion

        #region 页面
        public ActionResult Message()
        {
            ViewBag.phones = Request["phones"].GetSafeString();
            return View();
        }

        #endregion
        #region 获取变量
        public void GetVerifyCodeSpan()
        {
            string msg = "";
            bool code = true;
            try
            {
                msg = (GlobalVariable.GetSmsRegisterVerifyCodeMinSpan(Request) * 60).ToString();
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion
        #region 发送短信
        public void DoSendVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId(Request);
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode(Request);
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength(Request);
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds(Request);
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan(Request);
                    bool isSpanMin = false;

                    string oldValue = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID(Request);
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(Request), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        if (code)
                            Session["REGISTER_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        /// <summary>
        /// 忘记密码是的验证码
        /// </summary>
        public void DoSendVerifyCode2()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                string sjhmlx = Request["sjhmlx"].GetSafeString();
                string qyzh = Request["qyzh"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";


                if (!GetQYSJHM(receiver, qyzh))
                {
                    msg = "该手机号与该单位账号不对应";
                }
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId(Request);
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode(Request);
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength(Request);
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds(Request);
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan(Request);
                    bool isSpanMin = false;

                    string oldValue = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(Request), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        if (code)
                            Session["REGISTER_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        public bool GetQYSJHM(string sjhm, string qyzh)
        {
            bool ret = false;
            string msg = "";
            try
            {
                string tablename = "I_M_QY";

                string sql = "select * from " + tablename + " where lxsj='" + sjhm + "' and zh='" + qyzh + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    ret = true;
                }
                else
                    ret = false;
            }
            catch (Exception e)
            {

            }
            return ret;
        }

        public void DoSendChangePassVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["CHANGEPASS_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }


                        if (code)
                            Session["CHANGEPASS_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendChangeInfoVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["CHANGEINFO_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["CHANGEINFO_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendResetPassVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["RESETPASS_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["RESETPASS_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendValidateReportUserVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["VALIDATEREPORTUSER_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["VALIDATEREPORTUSER_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendValidateReportQFRVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["VALIDATEREPORTQFR_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["VALIDATEREPORTQFR_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendUserRegisterSecondStepVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["USERREGISTERSECONDSTEP_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["USERREGISTERSECONDSTEP_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendRYBAModifyPhoneVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["RYBAMODIFYPHONE_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["RYBAMODIFYPHONE_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendQYBAModifyPhoneVerifyCode()
        {
            bool code = false;
            string msg = "";
            try
            {
                string receiver = Request["receiver"].GetSafeString();
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCode();
                    int vclen = GlobalVariable.GetSmsRegisterVerifyCodeLength();
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    int vcspan = GlobalVariable.GetSmsRegisterVerifyCodeMinSpan();
                    bool isSpanMin = false;

                    string oldValue = Session["QYBAMODIFYPHONE_VERIFY_CODE"] as string;// 验证码,时间
                    if (oldValue != null)
                    {
                        string[] arr = oldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime timeOld = arr[1].GetSafeDate();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes < vcspan)
                            isSpanMin = true;
                    }
                    if (isSpanMin)
                        msg = "验证码发送最小时间间隔为" + vcspan + "分种，现在不能重发，请稍后";
                    else
                    {
                        string verifycode = RandomNumber.GetNew(RandomType.Number, vclen);

                        SmsRequestRegister objRegister = new SmsRequestRegister()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarRegister()
                            {
                                code = verifycode,
                                minute = vcminutes.ToString()
                            }
                        };

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objRegister);
                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${code}", verifycode);
                            paramlist.Add("${minute}", vcminutes.ToString());
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("VERIFY_CODE");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                            msg = code ? "" : msg;
                        }

                        if (code)
                            Session["QYBAMODIFYPHONE_VERIFY_CODE"] = verifycode + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        public void DoSendUserInfo()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm from View_I_M_ZH where id='" + usercode + "'");
                string receiver = "";
                if (dt.Count > 0)
                    receiver = dt[0]["sjhm"];
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsRegisterTemplateCodeUserInfo();

                    string username = Session["USER_INFO_USERNAME"].GetSafeString();
                    string password = Session["USER_INFO_PASSWORD"].GetSafeString();

                    SmsRequestUserInfo objRegister = new SmsRequestUserInfo()
                        {
                            invokeId = vcinvokeid,
                            phoneNumber = receiver,
                            templateCode = vctemplate,
                            contentVar = new SmsVarUserInfo()
                            {
                                username = username,
                                password = password
                            }
                        };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(objRegister);
                    int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                    // 百度--短信模块
                    if (MessageSenderID == 0)
                    {
                        code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                    }
                    // 温州市建设工程质量监督站--短信模块
                    else if (MessageSenderID == 1)
                    {
                        Dictionary<string, string> paramlist = new Dictionary<string, string>();
                        paramlist.Add("${username}", username);
                        paramlist.Add("${password}", password);
                        string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("REGISTER_SUCCESS");
                        contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                        code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        msg = code ? "" : msg;
                    }
                    // 温州市建设工程质量监督站--新短信模块
                    else if (MessageSenderID == 2)
                    {
                        Dictionary<string, string> paramlist = new Dictionary<string, string>();
                        paramlist.Add("${username}", username);
                        paramlist.Add("${password}", password);
                        string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("REGISTER_SUCCESS");
                        contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                        code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        msg = code ? "" : msg;
                    }




                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendChangePassUserInfo()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm from View_I_M_ZH where zh='" + usercode + "'");
                string receiver = "";
                if (dt.Count > 0)
                    receiver = dt[0]["sjhm"];
                else
                {
                    IList<IDictionary<string, string>> dtt = CommonService.GetDataTable("select phone from userinfo where username='" + usercode + "'");
                    if (dtt.Count > 0)
                        receiver = dtt[0]["phone"];
                }
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {

                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsPasswordChangeTemplateCodeUserInfo();

                    string username = Session["USER_INFO_USERNAME"].GetSafeString();
                    string password = Session["USER_INFO_PASSWORD"].GetSafeString();

                    SmsRequestUserInfo objRegister = new SmsRequestUserInfo()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = receiver,
                        templateCode = vctemplate,
                        contentVar = new SmsVarUserInfo()
                        {
                            username = username,
                            password = password
                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(objRegister);

                    int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                    // 百度--短信模块
                    if (MessageSenderID == 0)
                    {
                        code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                    }
                    // 温州市建设工程质量监督站--短信模块
                    else if (MessageSenderID == 1)
                    {
                        Dictionary<string, string> paramlist = new Dictionary<string, string>();
                        paramlist.Add("${username}", username);
                        paramlist.Add("${password}", password);
                        string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("CHANGEPASS_SUCCESS");
                        contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                        code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        msg = code ? "" : msg;
                    }
                    // 温州市建设工程质量监督站--新短信模块
                    else if (MessageSenderID == 2)
                    {
                        Dictionary<string, string> paramlist = new Dictionary<string, string>();
                        paramlist.Add("${username}", username);
                        paramlist.Add("${password}", password);
                        string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("CHANGEPASS_SUCCESS");
                        contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                        code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        msg = code ? "" : msg;
                    }




                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendResetPassUserInfo()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string receiver = Request["phone"].GetSafeString();
                if (receiver == "") {
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm from View_I_M_ZH where zh='" + usercode + "'");

                    if (dt.Count > 0)
                        receiver = dt[0]["sjhm"];
                    else
                    {
                        IList<IDictionary<string, string>> dtt = CommonService.GetDataTable("select phone from userinfo where username='" + usercode + "'");
                        if (dtt.Count > 0)
                            receiver = dtt[0]["phone"];
                    }
                }
                
                if (receiver == "")
                    msg = "请输入手机号码";
                else if (!receiver.IsMobile())
                    msg = "请输入正确的手机号码";
                else
                {
                    string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string vctemplate = GlobalVariable.GetSmsPasswordResetTemplateCodeUserInfo();

                    string username = Session["USER_INFO_USERNAME"].GetSafeString();
                    string password = Session["USER_INFO_PASSWORD"].GetSafeString();

                    SmsRequestUserInfo objRegister = new SmsRequestUserInfo()
                    {
                        invokeId = vcinvokeid,
                        phoneNumber = receiver,
                        templateCode = vctemplate,
                        contentVar = new SmsVarUserInfo()
                        {
                            username = username,
                            password = password
                        }
                    };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string contents = jss.Serialize(objRegister);
                    int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                    // 百度--短信模块
                    if (MessageSenderID == 0)
                    {
                        code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                    }
                    // 温州市建设工程质量监督站--短信模块
                    else if (MessageSenderID == 1)
                    {
                        Dictionary<string, string> paramlist = new Dictionary<string, string>();
                        paramlist.Add("${username}", username);
                        paramlist.Add("${password}", password);
                        string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("RESETPASS_SUCCESS");
                        contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                        code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        msg = code ? "" : msg;
                    }
                    // 温州市建设工程质量监督站--新短信模块
                    else if (MessageSenderID == 2)
                    {
                        Dictionary<string, string> paramlist = new Dictionary<string, string>();
                        paramlist.Add("${username}", username);
                        paramlist.Add("${password}", password);
                        string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("RESETPASS_SUCCESS");
                        contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                        code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        msg = code ? "" : msg;
                    }



                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoSendMessage()
        {
            bool code = false;
            string msg = "";
            try
            {
                string phones = Request["phones"].GetSafeString();
                string content = Request["content"].GetSafeString();

                if (phones == "")
                {
                    msg = "手机号码不能为空";
                }
                else
                {
                    string[] arrPhones = phones.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string invokeid = GlobalVariable.GetSmsBaseInvokeId();
                    string templateid = GlobalVariable.GetSmsMessageTemplateCode();
                    string companyname = GlobalVariable.GetSmsMessageCompanyName();

                    foreach (string strPhone in arrPhones)
                    {
                        if (!strPhone.IsMobile())
                            continue;
                        SmsRequestMessage objMsg = new SmsRequestMessage()
                        {
                            invokeId = invokeid,
                            phoneNumber = strPhone,
                            templateCode = templateid,
                            contentVar = new SmsVarMessage()
                            {
                                client = companyname,
                                info = content
                            }
                        };
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objMsg);

                        int MessageSenderID = GlobalVariable.GetDefaultMessageSenderID();
                        // 百度--短信模块
                        if (MessageSenderID == 0)
                        {
                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), strPhone, contents, out msg);
                        }
                        // 温州市建设工程质量监督站--短信模块
                        else if (MessageSenderID == 1)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${client}", companyname);
                            paramlist.Add("${info}", content);
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("INFORMATION");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessage(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), strPhone, contents, out msg);
                            msg = code ? "" : msg;
                        }
                        // 温州市建设工程质量监督站--新短信模块
                        else if (MessageSenderID == 2)
                        {
                            Dictionary<string, string> paramlist = new Dictionary<string, string>();
                            paramlist.Add("${client}", companyname);
                            paramlist.Add("${info}", content);
                            string templatecontent = GlobalVariable.GetWzzjzSmsTemplate("INFORMATION");
                            contents = ReplaceSmsTemplateContent(paramlist, templatecontent);
                            code = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), strPhone, contents, out msg);
                            msg = code ? "" : msg;
                        }

                    }
                    msg = "";
                    code = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 短信通知
        /// </summary>
        public void DoSendMessageTZ()
        {
            bool code = false;
            string msg = "";
            try
            {
                string smstype = Request["smstype"].GetSafeString();
                string phones = "";
                string content = "";
                string sql="";
                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                IDataParameter sqlparam = null;
                if (smstype == "qyzzsp")
                {
                    phones = Configs.GetConfigItem("qyzzspsj");
                    content = "有企业申请企业资质需审批，";
                    sql = "Insert INTO INFO_SMS ([guid],[Phone],[Message],[HasDeal],[LX]) values (NEWID(),@lxdhs,@msg,0,'qyzzsp')";
                    sqlparams = new List<IDataParameter>();
                    sqlparam = new SqlParameter("@lxdhs", phones);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@msg", content);
                    sqlparams.Add(sqlparam);
                }
                if (smstype == "qyzzsptg")
                {
                    phones = Configs.GetConfigItem("qyzzspsj");
                    content = "你的企业资质（类型）申请已通过，";
                    sql = "Insert INTO INFO_SMS ([guid],[Phone],[Message],[HasDeal],[LX]) values (NEWID(),@lxdhs,@msg,0,'qyzzsp')";
                    sqlparams = new List<IDataParameter>();
                    sqlparam = new SqlParameter("@lxdhs", phones);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@msg", content);
                    sqlparams.Add(sqlparam);
                }
                if(sql!="")
                    code=CommonService.ExecTrans(sql, sqlparams, out msg);
     
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 替换短信模板中的参数
        /// 金成龙--2017-08-31
        /// </summary>
        /// <param name="paramlist"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private string ReplaceSmsTemplateContent(Dictionary<string, string> paramlist, string template)
        {
            string ret = template;
            foreach (var param in paramlist)
            {
                ret = ret.Replace(param.Key, param.Value);
            }
            return ret;
        }
        #endregion

        #region 通用短信发送接口
        /// <summary>
        /// 支付平台短信
        /// </summary>       
        public JsonResult DoSendCommonMessage(string key, string messagetype, string receiver, string jsonbody)
        {
            bool code = false;
            string msg = "";
            try
            {
                string realKey = MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + messagetype + receiver);
                if (!realKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    msg = "验证错误";
                }
                else if (string.IsNullOrEmpty(receiver) || !receiver.IsMobile())
                {
                    msg = "手机号码格式错误";
                }
                else if (string.IsNullOrEmpty(jsonbody))
                {
                    msg = "消息内容不能为空";
                }
                else
                {
                    // 支付短信
                    if (messagetype == "1001")
                    {
                        SmsVarPayVerify varobj = new JavaScriptSerializer().Deserialize<SmsVarPayVerify>(jsonbody);
                        if (varobj == null)
                        {
                            msg = "消息内容反序列化失败";
                        }
                        else
                        {
                            string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId(Request);
                            string vctemplate = GlobalVariable.GetSmsPayVerifyCodeTemplate(Request);
                            SmsRequestPayVerify smsObj = new SmsRequestPayVerify()
                            {
                                invokeId = vcinvokeid,
                                phoneNumber = receiver,
                                templateCode = vctemplate,
                                contentVar = varobj
                            };

                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            string contents = jss.Serialize(smsObj);

                            code = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(Request), Guid.NewGuid().ToString(), receiver, contents, out msg);
                        }
                    }
                    else
                    {
                        msg = "消息类型错误";
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion
    }
}