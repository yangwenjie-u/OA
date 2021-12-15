using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using SysLog4 = BD.Jcbg.Common.SysLog4;
using System.Web.Http;
using System.Web;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.Web.Func;
using System.Net;
using System.Net.Http.Headers;
using TGTextSharp.text;
using System.Threading;

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 现场检测api调用接口
    /// </summary>
    public class ApiXcjcController : ApiController
    {
        #region 服务
        private static IJcService _jcService = null;
        private static IJcService JcService
        {
            get
            {
                if (_jcService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                }
                return _jcService;
            }
        }
        private static ICommonService _commonService = null;
        private static ICommonService CommonService
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

        private static IApiSessionService _apiSessionService = null;
        private static IApiSessionService ApiSessionService
        {
            get
            {
                if (_apiSessionService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _apiSessionService = webApplicationContext.GetObject("ApiSessionService") as IApiSessionService;
                }
                return _apiSessionService;
            }
        }

        private static ISxtptService _sxtptService = null;
        private static ISxtptService SxtptService
        {
            get
            {
                if (_sxtptService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _sxtptService = webApplicationContext.GetObject("SxtptService") as ISxtptService;
                }
                return _sxtptService;
            }
        }

        #endregion

        #region 现场检测对外调用

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Login()
        {
            var httpRequest = HttpContext.Current.Request;

            var request = new VTransXcjcReqLogin 
            { 
                username = httpRequest["username"].GetSafeString(),
                password = httpRequest["password"].GetSafeString()
            };

            string ret = "";
            VTransXcjcRespLogin resp = new VTransXcjcRespLogin() { code = VTransXcjcRespBase.Success, upsimcode = false, msg = "", sessionid="", gcs = new List<IDictionary<string,string>>() };
            try
            {
                string msg = "", usercode = "", realname = "";
                bool code = UserService.CheckLogin(request.username, request.password, out usercode, out realname, out msg);
                resp.msg = msg;
                if (!code)
                    resp.code = VTransXcjcRespBase.ErrorUserLoginError;
                else
                {
                    string rylx = "";
                    if (!JcService.GetVirtualRylx(request.username, out msg))
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetUserType;
                        resp.msg = msg;
                    }
                    else
                    {
                        rylx = msg;
                        if (!ApiSessionService.SetSession(request.username, request.password, usercode, realname, out msg))
                        {
                            resp.code = VTransXcjcRespBase.ErrorSaveSession;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.sessionid = msg;
                            resp.upsimcode = !JcService.HasBindPhoneSim(usercode);
                            resp.jcrjzh = JcService.GetJcrjzh(usercode);
                            resp.usertype = rylx;
                            resp.username = request.username;
                            resp.realname = realname;
                            resp.gcs = JcService.GetGcAndRylx(request.username, out code, out msg);

                            if (!code)
                            {
                                resp.code = VTransXcjcRespBase.ErrorException;
                                resp.msg = msg;
                            }
                        }
                    }
                }

                if (resp.code == VTransXcjcRespBase.Success)
                {
                    var files = httpRequest.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        byte[] fileBytes = new byte[files[i].ContentLength];
                        Stream MyStream = files[i].InputStream;
                        MyStream.Read(fileBytes, 0, files[i].ContentLength);
                        MyStream.Close();

                        var img = new BD.Jcbg.Common.MyImage(fileBytes);

                        if (img.IsImage())
                        {
                            JcService.SysLogPicSave(new SysLogPic
                            {
                                UserCode = usercode,
                                PicName = files[i].FileName,
                                PicContent = fileBytes,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                }

                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 登录接口2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Login2()
        {
            var httpRequest = HttpContext.Current.Request;

            var request = new VTransXcjcReqLogin
            {
                username = httpRequest["username"].GetSafeString(),
                password = httpRequest["password"].GetSafeString()
            };

            string ret = "";
            VTransXcjcRespLogin2 resp = new VTransXcjcRespLogin2() { code = VTransXcjcRespBase.Success, upsimcode = false, msg = "", sessionid = "" };
            try
            {
                string msg = "", usercode = "", realname = "";
                bool code = UserService.CheckLogin(request.username, request.password, out usercode, out realname, out msg);
                resp.msg = msg;
                if (!code)
                    resp.code = VTransXcjcRespBase.ErrorUserLoginError;
                else
                {
                    string rylx = "";
                    if (!JcService.GetVirtualRylx(request.username, out msg))
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetUserType;
                        resp.msg = msg;
                    }
                    else
                    {
                        rylx = msg;
                        if (!ApiSessionService.SetSession(request.username, request.password, usercode, realname, out msg))
                        {
                            resp.code = VTransXcjcRespBase.ErrorSaveSession;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.sessionid = msg;
                            resp.upsimcode = !JcService.HasBindPhoneSim(usercode);
                            resp.jcrjzh = JcService.GetJcrjzh(usercode);
                            resp.usertype = rylx;
                        }
                    }
                }

                if (resp.code == VTransXcjcRespBase.Success)
                {
                    var files = httpRequest.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        byte[] fileBytes = new byte[files[i].ContentLength];
                        Stream MyStream = files[i].InputStream;
                        MyStream.Read(fileBytes, 0, files[i].ContentLength);
                        MyStream.Close();

                        var img = new BD.Jcbg.Common.MyImage(fileBytes);

                        if (img.IsImage())
                        {
                            JcService.SysLogPicSave(new SysLogPic
                            {
                                UserCode = usercode,
                                PicName = files[i].FileName,
                                PicContent = fileBytes,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                }

                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        [HttpGet]
        public void ShowPic(string userCode)
        {
            try
            {
                var files = JcService.SysLogPicGets(userCode);

                if (files.Count > 0)
                {
                    var file = files[0];
                    string mime = MimeMapping.GetMimeMapping(file.PicName);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = mime;
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(file.PicName));
                    HttpContext.Current.Response.BinaryWrite(file.PicContent);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        /// <summary>
        /// 获取人员对应的工程信息和人员类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetGcRylx([FromBody]VTransXcjcReqGcRylx request)
        {
            string ret = "";
            bool code = false;
            VTransXcjcRespGcRylx resp = new VTransXcjcRespGcRylx() { code = VTransXcjcRespGcRylx.Success, msg = "", gcs = new List<IDictionary<string, string>>() };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);
                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else 
                {
                    resp.username = request.UserName;
                    resp.realname = session.RealName;
                    resp.gcs = JcService.GetGcAndRylx(request.UserName, out code, out msg);

                    if (!code)
                    {
                        resp.code = VTransXcjcRespBase.ErrorException;
                        resp.msg = msg;
                    }
                }

                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 绑定sim卡号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage BindSimCode([FromBody]VTransXcjcReqBindSmsCode request)
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid,out msg);
                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else if (JcService.HasSimUsed(session.UserCode, request.simcode))
                {
                    resp.code = VTransXcjcRespBase.ErrorSimHasUse;
                }
                else
                {
                    if (!JcService.BindPhoneSim(session.UserCode, request.simcode, out msg))
                        resp.code = VTransXcjcRespBase.ErrorBindSimCode;
                    resp.msg = msg;
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取分页的试验项目列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyxmList([FromBody]VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetXcjcSyxmList(qybh, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetSyxmList;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 获取分页的试验列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyList([FromBody]VTransXcjcReqSyList request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetXcjcSybhList(qybh, request.syxmbh, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetWtdList;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 获取分页的试验设备列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyDevList([FromBody] VTransXcjcReqSyDevList request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetXcjcSyDevList(qybh, request.ptbh, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetWtdDevList;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 获取分页的试验同检人员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyrList([FromBody] VTransXcjcReqSyrList request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetXcjcSyrList(qybh, session.UserCode, request.ptbh, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetWtdDevList;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 根据平台编号获取委托单详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyDetail([FromBody]VTransXcjcReqSyDetail request)
        {
            string ret = "";
            VTransXcjcRespSyDetail resp = new VTransXcjcRespSyDetail() { code = VTransXcjcRespBase.Success, msg = "", records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    IDictionary<string, object> dict = JcService.GetXcjcSyDetail(request.ptbh, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetWtdList;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.records = dict;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 获取分页试验部位列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSybwList([FromBody]VTransXcjcReqSybw request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    int totalcount = 0;
                    IList<IDictionary<string, string>> dt = JcService.GetXcjcSybwList(request.syxmbh, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetSybwList;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.totalcount = totalcount;
                        resp.records = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 获取摄像头列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSxtList([FromBody]VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetXcjcSxtList(qybh, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetSxtList;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        
        /// <summary>
        /// 校验摄像头是否在线
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CheckSxtOnline([FromBody]VTransXcjcReqCheckSxt request)
        {
            string ret = "";
            VTransXcjcRespCheckSxt resp = new VTransXcjcRespCheckSxt() { code = VTransXcjcRespBase.Success, msg = "", sxtmc="" , sxtwyh= "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string sxtmc = "";
                    string sxtwyh = "";
                    bool online = SxtptService.QueryOnline(request.sxtbh, out sxtwyh, out sxtmc, out msg);
                    resp.sxtmc = sxtmc;
                    resp.sxtwyh = sxtwyh;
                    if (online)
                        resp.code = VTransXcjcRespBase.Success;
                    else
                    {
                        if (msg == "")
                            resp.code = VTransXcjcRespBase.CodeSxtNotOnline;
                        else
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetSxtOnline;
                            resp.msg = msg;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 开始试验
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage StartExperment()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };

            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string sxtlist = HttpContext.Current.Request["sxtlist"].GetSafeString();
                string longitude = HttpContext.Current.Request["longitude"].GetSafeString();
                string latitude = HttpContext.Current.Request["latitude"].GetSafeString();
                //流水号
                int lsh = HttpContext.Current.Request["lsh"].GetSafeInt();
                //桩(组)号
                string zh = HttpContext.Current.Request["zh"].GetSafeString();
                //桩类型
                string zlx = HttpContext.Current.Request["zlx"].GetSafeString();
                //设备编号
                string sbbh = HttpContext.Current.Request["sbbh"].GetSafeString();
                //同检人员
                string tjry = HttpContext.Current.Request["tjry"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + longitude + latitude + sxtlist), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    IList<VTransXcjcReqStartItem> useSxts = new List<VTransXcjcReqStartItem>();
                    if (sxtlist != null && sxtlist.Count() > 0)
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        useSxts = jss.Deserialize<IList<VTransXcjcReqStartItem>>(sxtlist);
                    }
                    IList<byte[]> upFiles = new List<byte[]>();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    //SysLog4.WriteError(String.Format("Controller调试：委托单唯一号：{0}上传图片数：{1}", wtdwyh, files.Count));
                    foreach (string key in files.AllKeys)
                    {
                        HttpPostedFile file = files[key];
                        //SysLog4.WriteError(String.Format("委托单唯一号：{0}上传图片大小：{1}", wtdwyh, file.ContentLength));
                        if (file.ContentLength > 0)
                        {
                            byte[] bytes = new byte[file.ContentLength];
                            using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                                bytes = reader.ReadBytes(bytes.Length);
                            upFiles.Add(bytes);
                        }
                    }
                    bool code = JcService.XcjcStartExperment(wtdwyh, lsh, zh, zlx, session.UserCode, session.RealName, longitude, latitude, useSxts, upFiles, out msg);
                    if (!code)
                    {
                        resp.msg = msg;
                        resp.code = VTransXcjcRespBase.ErrorStartExperment;
                    }
                    else
                    {
                        //开启现场试验
                        if (GlobalVariableConfig.GLOBAL_CAMERASNAP_AUTO)
                        {
                            SysLog4.WriteLog("开启现场试验：" + JsonSerializer.Serialize(useSxts) + " " + wtdwyh + " " + zh);
                            SxtptService.ThreadStartSxt(useSxts, session.UserCode, session.RealName, wtdwyh, zh);
                        }
                        else
                        {
                            SysLog4.WriteLog("未开启现场试验图片链：" + useSxts + " " + wtdwyh + " " + zh);
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }


        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadPicture()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string imagetype = HttpContext.Current.Request["imagetype"].GetSafeString();
                //组号
                string zh = HttpContext.Current.Request["zh"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + imagetype), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {

                    IList<byte[]> upFiles = new List<byte[]>();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    foreach (string key in files.AllKeys)
                    {
                        HttpPostedFile file = files[key];
                        if (file.ContentLength > 0)
                        {
                            byte[] bytes = new byte[file.ContentLength];
                            using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                                bytes = reader.ReadBytes(bytes.Length);
                            upFiles.Add(bytes);
                        }
                    }
                    bool code = JcService.XcjcUpImage(wtdwyh, zh, upFiles, imagetype.GetSafeInt(), out msg);

                    if (!code)
                    {
                        resp.msg = msg;
                        resp.code = VTransXcjcRespBase.ErrorUploadImage;
                    }

                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 上传视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadVideo([FromBody]VTransXcjcReqVideoInfo request)
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);


                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    VTransXcjcReqVideoInfoItem[] videos = jsonSerializer.Deserialize<VTransXcjcReqVideoInfoItem[]>(request.datajson);

                    if (videos == null || videos.Count() == 0)
                        resp.code = VTransXcjcRespBase.ErrorVideoNull;
                    else
                    {
                        bool code = JcService.XcjcUploadVideo(request.ptbh, request.videotype, videos, out msg);

                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }

                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 停止试验
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage StopExperment()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                //桩(组)号
                string zh = HttpContext.Current.Request["zh"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    IList<byte[]> upFiles = new List<byte[]>();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    foreach (string key in files.AllKeys)
                    {
                        HttpPostedFile file = files[key];
                        if (file.ContentLength > 0)
                        {
                            byte[] bytes = new byte[file.ContentLength];
                            using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                                bytes = reader.ReadBytes(bytes.Length);
                            upFiles.Add(bytes);
                        }
                    }
                    bool code = JcService.XcjcStopExperment(wtdwyh, zh, upFiles, out msg);
                    if (!code)
                    {
                        resp.msg = msg;
                        resp.code = VTransXcjcRespBase.ErrorStopExperment;
                    }
                    else
                    {
                        //自动抓拍结束
                        if (GlobalVariableConfig.GLOBAL_CAMERASNAP_AUTO)
                        {
                            SysLog4.WriteLog("自动抓拍结束：" + wtdwyh + " " + zh);
                            SxtptService.DropSxtThread(wtdwyh, zh);
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取分页的正在试验列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetInSyList([FromBody]VTransXcjcReqInSyList request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetXcjcInSybhList(qybh, session.UserCode, request.syxmbh, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetInSybh;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 上传人员签名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadSign()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    
                    byte[] signFile = null;
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        HttpPostedFile file = files[0];
                        signFile = new byte[file.ContentLength];
                        using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                            signFile = reader.ReadBytes(signFile.Length);
                        if (signFile != null)
                        {
                            BD.Jcbg.Common.MyImage img = new BD.Jcbg.Common.MyImage(signFile);
                            if (!img.IsImage())
                            {
                                resp.code = VTransXcjcRespBase.ErrorInvalidImage;
                                resp.msg = "无效的图像文件，支持的签名文件格式为：" + img.GetValidImageDesc();
                            }
                            else
                            {
                                // 上传到用户系统
                                byte[] content = img.ConvertToJpg(Configs.SignMaxWidth, Configs.SignMaxHeight);
                                string sign64 = content.EncodeBase64();
                                bool code = Remote.UserService.SetUserSign(session.UserName, sign64, out msg);
                                if (!code)
                                    resp.code = VTransXcjcRespBase.ErrorUploadToUserSystem;
                                else
                                {
                                    // 上传到录入界面
                                    code = JcService.XcjcSetRySign(session.UserName, file.FileName, content, out msg);
                                    if (!code)
                                    {
                                        resp.code = VTransXcjcRespBase.ErrorSaveSign;
                                        resp.msg = msg;
                                    }
                                }
                            }
                            

                            
                        }

                    }


                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取委托单现场检测图片内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ShowWtdXcjcImage()
        {
            byte[] ret = null;
            try
            {
                ret = JcService.GetXcjcUpImage(HttpContext.Current.Request["id"].GetSafeString());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally
            {

            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ret)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return resp;
        }
        #endregion

        #region 见证取样对外调用
        /// <summary>
        /// 获取分页的所有实验项目编号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqySyxmList([FromBody]VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    int totalcount = 0;
                    IList<IDictionary<string, string>> dt = JcService.GetPageBaseSyxmList(request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetSyxmList;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.totalcount = totalcount;
                        resp.records = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取见证人工程列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzrGcList([FromBody]VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetPageJzrGcList(session.UserName, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetJzryGclb;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取收样人工程列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyrGcList([FromBody]VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.GetPageSyrGcList(qybh, request.key, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetSyryGclb;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取见证人试验列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzrsyList([FromBody]VTransJzqyReqJzrysyList request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.JzqyGetJzrSybhList(qybh, request.gcbh, request.syxmbh, request.key, request.zt, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetJzrySybh;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取见证人试验列表,包含详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzrsyxqList([FromBody]VTransJzqyReqJzrysyxqList request)
        {
            string ret = "";
            VTransXcjcRespMultiRowInfo resp = new VTransXcjcRespMultiRowInfo() { code = VTransXcjcRespBase.Success, msg = "", records=null};
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);
                
                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        string url = ClientInfo.GetXcjcImageUrl(HttpContext.Current.Request, "ShowWtdImage?id={0}");
                        IList<IDictionary<string, object>> dt = JcService.JzqyGetJzrSybhListWithDetail(session.UserName, request.gcbh, request.syxmbh, request.key, request.zt, request.tprq1, request.tprq2, url, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetJzrySybh;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取送样人试验列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyrsyList([FromBody]VTransJzqyReqSyrysyList request)
        {
            string ret = "";
            VTransXcjcRespBasePage resp = new VTransXcjcRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", totalcount = 0, records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        int totalcount = 0;
                        IList<IDictionary<string, string>> dt = JcService.JzqyGetSyrSybhList(qybh, request.gcbh, request.syxmbh, request.key, request.zt, request.pagesize, request.pageindex, out totalcount, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetSyrySybh;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.totalcount = totalcount;
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取送样人试验列表，包含详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyrsyxqList([FromBody]VTransJzqyReqSyrysyxqList request)
        {
            string ret = "";
            VTransXcjcRespMultiRowInfo resp = new VTransXcjcRespMultiRowInfo() { code = VTransXcjcRespBase.Success, msg = "",  records = null };
            try
            {
                string msg = "";

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else
                {
                    string url = ClientInfo.GetXcjcImageUrl(HttpContext.Current.Request, "ShowWtdImage?id={0}");
                    IList<IDictionary<string, object>> dt = JcService.JzqyGetSyrSybhListWithDetail(request.qybh, request.gcbh, request.syxmbh, request.key, request.zt, request.tprq1,request.tprq2,url, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetSyrySybh;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.records = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 上传见证取样图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadJzqyPicture()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string imagetype = HttpContext.Current.Request["imagetype"].GetSafeString();
                string ewm = HttpContext.Current.Request["ewm"].GetSafeString();
                string lon = HttpContext.Current.Request["lon"].GetSafeString();
                string lat = HttpContext.Current.Request["lat"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + imagetype + ewm + lon+lat +zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        IList<byte[]> upFiles = new List<byte[]>();
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        for (int i=0; i<files.Count; i++)
                        {
                            HttpPostedFile file = files[i];
                            if (file.ContentLength > 0)
                            {
                                byte[] bytes = new byte[file.ContentLength];
                                using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                                    bytes = reader.ReadBytes(bytes.Length);
                                upFiles.Add(bytes);
                            }
                        }
                        bool code = JcService.JzqyUpImage(wtdwyh, qybh, session.UserCode, session.RealName, upFiles, ewm.GetSafeRequest(), lon.GetSafeRequest(), lat.GetSafeRequest(), zh.GetSafeRequest(), imagetype, out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }
                    

                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 上传见证取样图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadJzqyPicture2()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string imagetype = HttpContext.Current.Request["imagetype"].GetSafeString();
                string ewm = HttpContext.Current.Request["ewm"].GetSafeString();
                string lon = HttpContext.Current.Request["lon"].GetSafeString();
                string lat = HttpContext.Current.Request["lat"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();
                string file = HttpContext.Current.Request["file"].GetSafeString();

                //日志
                SysLog4.WriteLog("UploadJzqyPicture2调试数据接收：wtdwyh：" + wtdwyh + "，imagetype：" + imagetype + "，ewm：" + ewm + "，zh：" + zh +
                                 "，file：" + file);

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + imagetype + ewm + lon + lat + zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        var jsSerializer = new JavaScriptSerializer();
                        jsSerializer.MaxJsonLength = Int32.MaxValue;
                        //json转成字典
                        IList<IDictionary<string, string>> files = jsSerializer.Deserialize<IList<IDictionary<string, string>>>(file); ;  //[{\"imageType\":\"A\",\"fileStr\":\"B\"},{\"imageType\":\"AA\",\"fileStr\":\"BB\"}]
                        //文件数组
                        IList<IDictionary<string, byte[]>> fileList = new List<IDictionary<string, byte[]>>();
                        //文件项
                        IDictionary<string, byte[]> fileItem = null;
                        string fileStr = "";
                        for (int i = 0; i < files.Count; i++)
                        {
                            //获取文件字符串
                            fileStr = files[i]["fileStr"].GetSafeString();
                            //判断文件是否存在
                            if(fileStr == "")
                                continue;;
                            //获取文件字节流
                            byte[] bytes = Convert.FromBase64String(fileStr);
                            //添加文件项
                            fileItem = new Dictionary<string, byte[]>();
                            fileItem.Add(files[i]["imageType"].GetSafeString(), bytes);
                            //添加项目
                            fileList.Add(fileItem);
                        }
                        bool code = JcService.JzqyUpImage2(wtdwyh, qybh, session.UserCode, session.RealName, fileList, ewm.GetSafeRequest(), lon.GetSafeRequest(), lat.GetSafeRequest(), zh.GetSafeRequest(), out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }


                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            SysLog4.WriteLog("UploadJzqyPicture2调试数据输出：" + ret);
            return GetResponse(ret);
        }

         /// <summary>
        /// 上传见证取样收样图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadJzqysyPicture()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string realname = HttpContext.Current.Request["realname"].GetSafeString();
                string qybh = HttpContext.Current.Request["qybh"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string imagetype = HttpContext.Current.Request["imagetype"].GetSafeString();
                string ewm = HttpContext.Current.Request["ewm"].GetSafeString();
                string lon = HttpContext.Current.Request["lon"].GetSafeString();
                string lat = HttpContext.Current.Request["lat"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();

                string msg = "";


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + qybh + realname + wtdwyh + imagetype + ewm + lon + lat + zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;

                else
                {
                    IList<byte[]> upFiles = new List<byte[]>();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        if (file.ContentLength > 0)
                        {
                            byte[] bytes = new byte[file.ContentLength];
                            using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                                bytes = reader.ReadBytes(bytes.Length);
                            upFiles.Add(bytes);
                        }
                    }
                    bool code = JcService.JzqyUpImageNotSafe(wtdwyh, qybh, realname, upFiles, ewm.GetSafeRequest(), lon.GetSafeRequest(), lat.GetSafeRequest(), zh.GetSafeRequest(), out msg);
                    if (!code)
                    {
                        resp.msg = msg;
                        resp.code = VTransXcjcRespBase.ErrorUploadImage;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取委托单见证信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetWtdJzInfo([FromBody]VTransJzqyReqGetWtdInfo request)
        {
            string ret = "";
            VTransXcjcRespMultiRowInfo resp = new VTransXcjcRespMultiRowInfo() { code = VTransXcjcRespBase.Success, msg = "", records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        string url = ClientInfo.GetXcjcImageUrl(HttpContext.Current.Request, "ShowWtdImage?id={0}");
                        string viewWtdUrl = ClientInfo.GetCurDnsWithPort(HttpContext.Current.Request)+  "/jc/viewwtd?id="+request.ptbh;
                        IList<IDictionary<string, object>> dt = JcService.JzqyGetWtdJzxq(url, request.ptbh, viewWtdUrl, out msg);
                        if (msg != "")
                        {
                            resp.code = VTransXcjcRespBase.ErrorGetWtdJzInfo;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.records = dt;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 获取委托单见证图片内容
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ShowWtdImage()
        {
            byte[] ret = null;
            try
            {
                ret = JcService.GetWtdUpImage(HttpContext.Current.Request["id"].GetSafeString());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally
            {
                
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ret)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return resp;
        }
        /// <summary>
        /// 设置委托单见证结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetWtdJzResult([FromBody]VTransJzqyReqSetZt request)
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = ""};
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.ptbh))
                        {
                            string[] arr = request.ptbh.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            StringBuilder sb = new StringBuilder();
                            foreach (string str in arr)
                            {
                                bool code = JcService.JzqySetStatus(qybh, session.UserCode, session.RealName, str, request.zt == 1, out msg);
                                if (!code)
                                {
                                    if (string.IsNullOrEmpty(msg))
                                        msg = VTransXcjcRespBase.GetErrorInfo(VTransXcjcRespBase.ErrorSetJzqyzt);
                                    sb.Append(request.ptbh + "设置失败：" + msg + "。");
                                }
                            }
                            resp.msg = sb.ToString();
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 设置样品单组完成标志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetDzwcStatus()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                //获取参数数据
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();
                //判断会话
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);

                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        bool code = JcService.JzqySetDzwcStatus(wtdwyh,zh, out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 设置工程坐标
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetGczb()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                //获取参数数据
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                //工程编号
                string gcbh = HttpContext.Current.Request["gcbh"].GetSafeString();
                //经度
                string longitude = HttpContext.Current.Request["longitude"].GetSafeString();
                //纬度
                string latitude = HttpContext.Current.Request["latitude"].GetSafeString();
                //判断会话
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);

                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + gcbh + longitude + latitude), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        bool code = JcService.JzqySetGczb(gcbh, longitude, latitude, out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 获取试验信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqySyInfo([FromBody]VTransJzqyReqSyInfo request)
        {
            string ret = "";
            VTransXcjcRespMultiRowInfo resp = new VTransXcjcRespMultiRowInfo() { code = VTransXcjcRespBase.Success, msg = "", records = null };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string url = ClientInfo.GetXcjcImageUrl(HttpContext.Current.Request, "ShowWtdImage?id={0}");
                    string viewWtdUrl = ClientInfo.GetCurDnsWithPort(HttpContext.Current.Request)+  "/jc/viewwtd?id="+request.ptbh;
                    IList<IDictionary<string, object>> dt = JcService.JzqyGetSyInfo(request.ptbh, viewWtdUrl, url, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetJzqySyInfo;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.records = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

         /// <summary>
        /// 根据二维码获取试验摘要信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqySyInfoByEwm([FromBody]VTransJzqyReqSyInfoByEwm request)
        {
            string ret = "";
            VTransXcjcRespBaseInfo resp = new VTransXcjcRespBaseInfo() { code = VTransXcjcRespBase.Success, msg = "", record = null };
            try
            {
                string msg = "";
                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else
                {
                    IDictionary<string, object> dt = JcService.JzqyGetSyInfoByEwm(request.ewmbh, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetJzqySyInfo;
                        resp.msg = msg;
                    }
                    else
                    {
                        //2019-03-27：杨鑫钢
                        //手持机端，回传是否启用"收样图片"控制符,1表示启用,0表示不启用
                        dt.Add("sytp", JcService.SysUseSytp() ? "1" : "0");
                        resp.record = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 根据二维码获取见证取样图片信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqyImagesByEwm([FromBody]VTransJzqyReqSyInfoByEwm request)
        {
            string ret = "";
            VTransXcjcRespBaseInfo resp = new VTransXcjcRespBaseInfo() { code = VTransXcjcRespBase.Success, msg = "", record = null };
            try
            {
                string msg = "";
                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else
                {
                    string url = ClientInfo.GetXcjcImageUrl(HttpContext.Current.Request, "ShowWtdImage?id={0}");
                    IDictionary<string, object> dt = JcService.JzqyGetImagesByEwm(url, request.ewmbh, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetJzqySyInfo;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.record = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        /// <summary>
        /// 根据平台编号，组号获取见证取样图片信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqyImagesByZh([FromBody]VTransJzqyReqSyInfoByZh request)
        {
            string ret = "";
            VTransXcjcRespBaseInfo resp = new VTransXcjcRespBaseInfo() { code = VTransXcjcRespBase.Success, msg = "", record = null };
            try
            {
                string msg = "";
                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else
                {
                    string url = ClientInfo.GetXcjcImageUrl(HttpContext.Current.Request, "ShowWtdImage?id={0}");
                    IDictionary<string, object> dt = JcService.JzqyGetImagesByZh(url, request.ptbh, request.zh, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetJzqySyInfo;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.record = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 删除关联的委托单二维码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelJzqyEwm([FromBody]VTransJzqyReqDelEwm request)
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    bool code = JcService.JzqyDelEwm(request.ewmbh, out msg);

                        resp.code = code?0:1;
                        resp.msg = msg;
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        
        #endregion

        #region 内部函数
        HttpResponseMessage GetResponse(string msg)
        {
            return new HttpResponseMessage { Content = new StringContent(msg, System.Text.Encoding.UTF8, "application/json") };
        }

        #endregion

        #region 现场检测设备服务调用接口
        /// <summary>
        /// 设置现场检测厂家试验编号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetXcjcCjsybh([FromBody]VTransXcjcReqSetCjsbbh request)
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = ""};
            try
            {
                if (request.NeedDeal)
                {
                    if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                        resp.code = VTransXcjcRespBase.ErrorParamCheck;
                    else
                    {
                        string msg = "";

                        if (!JcService.XcjcSetCjsybh(request.wtdwyh, request.zhuanghao, request.cjsybh, out msg))
                        {
                            resp.code = VTransXcjcRespBase.ErrorUpdateCjsybh;
                            resp.msg = msg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }
        #endregion

        #region 见证取样对外调用(自用)
        /// <summary>
        /// 获取微信扫一扫配置参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqyWxConfig([FromBody]VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransJzqyRespBasePage resp = new VTransJzqyRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", data = null };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    //获取微信信息
                    string timestamp = WeiXinHelper.CreateTimestamp();
                    string noncestr = WeiXinHelper.CreateNonceStr();
                    string url = Request.RequestUri.ToString().ToString();
                    string signature = WeiXinHelper.GetSignature(timestamp, noncestr, url);
                    IDictionary<string, string> dt = new Dictionary<string, string>();
                    //AppID
                    dt.Add("appId", Configs.WxAppid);
                    //时间戳
                    dt.Add("timestamp", timestamp);
                    //随机数
                    dt.Add("noncestr", noncestr);
                    //验证码
                    dt.Add("signature", signature);
                    resp.data = dt;
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 上传见证取样图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadJzqyPicture3()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                
                string lon = HttpContext.Current.Request["lon"].GetSafeString();
                string lat = HttpContext.Current.Request["lat"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();
                string file = HttpContext.Current.Request["file"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);

                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + lon + lat + zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        //二维码图片值由图片解析出来
                        byte[] ewmByte = null;
                        //解析数据包
                        var jsSerializer = new JavaScriptSerializer();
                        jsSerializer.MaxJsonLength = Int32.MaxValue;
                        //json转成字典
                        IList<IDictionary<string, string>> files = jsSerializer.Deserialize<IList<IDictionary<string, string>>>(file); ;  //[{\"imageType\":\"A\",\"fileStr\":\"B\"},{\"imageType\":\"AA\",\"fileStr\":\"BB\"}]
                        //文件数组
                        IList<IDictionary<string, byte[]>> fileList = new List<IDictionary<string, byte[]>>();
                        //文件项
                        IDictionary<string, byte[]> fileItem = null;
                        string fileStr = "";
                        for (int i = 0; i < files.Count; i++)
                        {
                            //获取文件字符串
                            fileStr = files[i]["fileStr"].GetSafeString();
                            //判断文件是否存在
                            if (fileStr == "")
                                continue; ;
                            //获取文件字节流
                            byte[] bytes = Convert.FromBase64String(fileStr);
                            //判断上传的图片类型是否为"二维码图片",如果为二维码图片,则存起内容
                            if (ewmByte == null && files[i]["imageType"].GetSafeString() == "ewmtp")
                            {
                                //记录二维码图片内容
                                ewmByte = bytes;
                            }
                            //添加文件项
                            fileItem = new Dictionary<string, byte[]>();
                            fileItem.Add(files[i]["imageType"].GetSafeString(), bytes);
                            //添加项目
                            fileList.Add(fileItem);
                        }
                        bool code = JcService.JzqyUpImage3(wtdwyh, qybh, session.UserCode, session.RealName, fileList, ewmByte, lon.GetSafeRequest(), lat.GetSafeRequest(), zh.GetSafeRequest(), out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }
            return GetResponse(ret);
        }

        /// <summary>
        /// 上传见证取样图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadJzqyPicture4()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string imagetype = HttpContext.Current.Request["imagetype"].GetSafeString();
                string ewm = HttpContext.Current.Request["ewm"].GetSafeString();
                string lon = HttpContext.Current.Request["lon"].GetSafeString();
                string lat = HttpContext.Current.Request["lat"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();
                string file = HttpContext.Current.Request["file"].GetSafeString();


                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + imagetype + ewm + lon + lat + zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        var jsSerializer = new JavaScriptSerializer();
                        jsSerializer.MaxJsonLength = Int32.MaxValue;
                        //json转成字典
                        IList<IDictionary<string, string>> files = jsSerializer.Deserialize<IList<IDictionary<string, string>>>(file); ;  //[{\"imageType\":\"A\",\"fileStr\":\"B\"},{\"imageType\":\"AA\",\"fileStr\":\"BB\"}]
                        //文件数组
                        IList<IDictionary<string, byte[]>> fileList = new List<IDictionary<string, byte[]>>();
                        //文件项
                        IDictionary<string, byte[]> fileItem = null;
                        string fileStr = "";
                        for (int i = 0; i < files.Count; i++)
                        {
                            //获取文件字符串
                            fileStr = files[i]["fileStr"].GetSafeString();
                            //判断文件是否存在
                            if (fileStr == "")
                                continue; ;
                            //获取文件字节流
                            byte[] bytes = Convert.FromBase64String(fileStr);
                            //添加文件项
                            fileItem = new Dictionary<string, byte[]>();
                            fileItem.Add(files[i]["imageType"].GetSafeString(), bytes);
                            //添加项目
                            fileList.Add(fileItem);
                        }
                        bool code = JcService.JzqyUpImage4(wtdwyh, qybh, session.UserCode, session.RealName, fileList, ewm.GetSafeRequest(), lon.GetSafeRequest(), lat.GetSafeRequest(), zh.GetSafeRequest(), out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }


                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            SysLog4.WriteLog("UploadJzqyPicture4调试数据输出：" + ret);
            return GetResponse(ret);
        }

        /// <summary>
        /// 标点APP上传见证图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadJzqyPicture5()
        {
            string ret = "";
            VTransXcjcRespBase resp = new VTransXcjcRespBase() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                string wtdwyh = HttpContext.Current.Request["ptbh"].GetSafeString();
                string imagetype = HttpContext.Current.Request["imagetype"].GetSafeString();
                string ewm = HttpContext.Current.Request["ewm"].GetSafeString();
                string lon = HttpContext.Current.Request["lon"].GetSafeString();
                string lat = HttpContext.Current.Request["lat"].GetSafeString();
                string zh = HttpContext.Current.Request["zh"].GetSafeString();
                string tplx = HttpContext.Current.Request["tplx"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + wtdwyh + ewm + lon + lat + zh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    string qybh = JcService.GetRyQybh(session.UserName, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetCompanyCode;
                        resp.msg = msg;
                    }
                    else
                    {
                        IList<byte[]> upFiles = new List<byte[]>();
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFile file = files[i];
                            if (file.ContentLength > 0)
                            {
                                byte[] bytes = new byte[file.ContentLength];
                                using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                                    bytes = reader.ReadBytes(bytes.Length);
                                upFiles.Add(bytes);
                            }
                        }
                        bool code = JcService.JzqyUpImage5(wtdwyh, qybh, session.UserCode, session.RealName, upFiles, ewm.GetSafeRequest(), lon.GetSafeRequest(), lat.GetSafeRequest(), zh.GetSafeRequest(), tplx, out msg);
                        if (!code)
                        {
                            resp.msg = msg;
                            resp.code = VTransXcjcRespBase.ErrorUploadImage;
                        }
                    }


                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Logout()
        {
            string ret = "";
            VTransJzqyRespBasePage resp = new VTransJzqyRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", data = null };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);
                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.Success;
                    resp.msg = msg;
                }
                else
                {
                    bool code = ApiSessionService.DelSession(sessionid, out msg);
                    if (!code)
                    {
                        resp.msg = msg;
                        resp.code = VTransXcjcRespBase.ErrorUploadImage;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }
            return GetResponse(ret);
        }

        /// <summary>
        /// 根据试验项目获取图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetJzqySyxmTpType([FromBody] VTransXcjcReqBasePage request)
        {
            string ret = "";
            VTransJzqyRespBasePage resp = new VTransJzqyRespBasePage() { code = VTransXcjcRespBase.Success, msg = "", data = null };
            try
            {
                string sessionid = HttpContext.Current.Request["sessionid"].GetSafeString();
                string checkcode = HttpContext.Current.Request["checkcode"].GetSafeString();
                //试验项目编号
                string syxmbh = HttpContext.Current.Request["syxmbh"].GetSafeString();

                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!checkcode.Equals(MD5Util.GetCommonMD5(GlobalVariable.GetApiEncryptKey() + sessionid + syxmbh), StringComparison.OrdinalIgnoreCase))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    IDictionary<string, object> dt = JcService.JzqyGetSyxmTpType(syxmbh, out msg);
                    if (msg != "")
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetSyrySybh;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.data = dt;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }
            return GetResponse(ret);
        }
        #endregion

        #region 同步合同 同步委托单

        [HttpPost]
        public HttpResponseMessage SyncJcjgHt([FromBody]VTransReqSyncJcjgHt request)
        {
            string ret = "";
            var resp = new VTransRespSyncJcjgHt() { code = VTransXcjcRespBase.Success, msg = "", data = "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    var result = JcService.SyncJcjgHt(request.htjson, session.UserCode, session.RealName);

                    if (!result.success)
                    {
                        resp.code = VTransXcjcRespBase.ErrorException;
                        resp.msg = msg;
                    }
                    else
                    {
                        resp.data = result.data as string;
                    }
                }

                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        }

        [HttpPost]
        public HttpResponseMessage SyncJcjgWtd([FromBody]VTransReqSyncJcjgWtd request)
        {
            string ret = "";
            var resp = new VTransRespSyncJcjgWtd() { code = VTransXcjcRespBase.Success, msg = "", data = "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    if (string.IsNullOrEmpty(request.mjson)
                    || string.IsNullOrEmpty(request.sjson))
                    {
                        resp.code = VTransXcjcRespBase.ErrorException;
                        resp.msg = "传入的参数不能为空";
                    }
                    else
                    {
                        var result = JcService.SyncJcjgWtd(request.mjson, request.sjson, session.UserCode, session.UserName);

                        if (!result.success)
                        {
                            resp.code = VTransXcjcRespBase.ErrorException;
                            resp.msg = msg;
                        }
                        else
                        {
                            resp.data = result.data as string;
                        }
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        } 
        #endregion

        #region 获取所有试验项目
        /// <summary>
        /// 获取所有试验项目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSyxms([FromBody]VTransXcjcReqGetSyxms request)
        {
            string ret = "";
            VTransXcjcRespGetSyxms resp = new VTransXcjcRespGetSyxms() { code = VTransXcjcRespBase.Success, msg = "" };
            try
            {
                string msg = "";
                SysSession session = ApiSessionService.GetSessionUser(request.sessionid, out msg);

                if (!request.IsValid(GlobalVariable.GetApiEncryptKey()))
                    resp.code = VTransXcjcRespBase.ErrorParamCheck;
                else if (session == null)
                {
                    resp.code = VTransXcjcRespBase.ErrorUserNotLogin;
                    resp.msg = msg;
                }
                else
                {
                    IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
                    var code = JcService.GetSyxmsByGcbh(request.gcbh, out records, out msg);

                    if (code)
                    {
                        resp.syxms = records;
                    }
                    else
                    {
                        resp.code = VTransXcjcRespBase.ErrorGetSyxmList;
                        resp.msg = msg;
                    }
                }
                if (resp.msg == "")
                    resp.msg = VTransXcjcRespBase.GetErrorInfo(resp.code);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                resp.code = VTransXcjcRespBase.ErrorException;
                resp.msg = ex.Message;
            }
            finally
            {
                ret = (new JavaScriptSerializer()).Serialize(resp);
            }

            return GetResponse(ret);
        } 
        #endregion
    }
}
