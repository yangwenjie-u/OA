using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;
using Bd.jcbg.Common;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using BD.IDataInputBll;
using NHibernate;
using System.Data;
using BD.Jcbg.Web.Common;

namespace BD.Jcbg.Web.Controllers
{
    public class UserController : Controller
    {

        #region 用户权限认证并跳转链接

        /// <summary>
        ///  解析内容,认证登录用户，并跳转相应的链接
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2016-11-24 15:45:39
        public ActionResult AuthorizeUrl(string content)
        {
            string result = Base64Func.DecodeBase64(content);
            //第一个为username，第二个为password(已加密，需要解密)
            string[] results = result.Split('|');
            string userName = CryptFun.Decode(results[0]), passWord = CryptFun.Decode(results[1]);
            string msg;
            string realname = "";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qymc from I_M_QY where ZH='" + userName + "'");
            if (dt.Count > 0)
            {
                realname = dt[0]["qymc"];
            }
            if (UserService.Login(userName, realname, passWord, out msg))
            {
                // 设置日志系统用户
                BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                // 设置流程模块用户
                BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                    new WorkFlow.Common.SessionUser()
                    {
                        CompanyId = CurrentUser.CurUser.CompanyId,
                        CompanyName = CurrentUser.CurUser.CompanyName,
                        DepartmentId = CurrentUser.CurUser.DepartmentId,
                        DepartmentName = CurrentUser.CurUser.DepartmentName,
                        DutyLevel = CurrentUser.CurUser.DutyLevel,
                        RealName = CurrentUser.CurUser.RealName,
                        UserName = CurrentUser.CurUser.UserName
                    }, null);
                // 设置录入界面用户
                Session["USERCODE"] = CurrentUser.UserCode;
                Session["USERNAME"] = CurrentUser.UserName;
                Session["REALNAME"] = CurrentUser.RealName;
                Session["CPCODE"] = CurrentUser.CompanyCode;
                Session["CPNAME"] = CurrentUser.CompanyName;
                Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                //Session["MenuCode"] = "QYGL_QYBA";
                CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                // 设置用户桌面项
                bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out msg);
                if (!status)
                    SysLog4.WriteLog(msg);
                return Redirect("/User/Main");
            }
            return Redirect("http://139.129.167.50:8013/");
        }

        #endregion

        #region 服务
        private ISystemService _systemService = null;
        private ISystemService SystemService
        {
            get
            {
                try
                {
                    if (_systemService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _systemService = webApplicationContext.GetObject("SystemService") as ISystemService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _systemService;
            }
        }
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                try
                {
                    if (_commonService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _commonService;
            }
        }
        private BD.Log.IBll.ILogService _logService = null;
        private BD.Log.IBll.ILogService LogService
        {
            get
            {
                if (_logService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _logService = webApplicationContext.GetObject("LogService") as BD.Log.IBll.ILogService;
                }
                return _logService;
            }
        }
        private BD.WorkFlow.Bll.RemoteUserService _remoteUserService = null;
        private BD.WorkFlow.Bll.RemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as BD.WorkFlow.Bll.RemoteUserService;
                }
                return _remoteUserService;
            }
        }

        private IJcService _jcService = null;
        private IJcService JcService
        {
            get
            {
                try
                {
                    if (_jcService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _jcService;
            }
        }
        private ISmsService _smsService = null;
        private ISmsService SmsService
        {
            get
            {
                try
                {
                    if (_smsService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _smsService = webApplicationContext.GetObject("SmsService") as ISmsService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _smsService;
            }
        }
        private ISelfService _selfService = null;
        private ISelfService SelfService
        {
            get
            {
                try
                {
                    if (_selfService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _selfService = webApplicationContext.GetObject("SelfService") as ISelfService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _selfService;
            }
        }

        private IDataInputService _dataInputService = null;
        private IDataInputService DataInputService
        {
            get
            {
                if (_dataInputService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataInputService = webApplicationContext.GetObject("DataInputService") as IDataInputService;
                }
                return _dataInputService;
            }
        }
        #endregion

        #region 页面

        /// <summary>
        /// 黑色登陆页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        
        /// <summary>
        /// 新改的蓝色背景
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginV3()
        {
            return View();
        }
        /// <summary>
        /// 检测集团版
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginV4()
        {
            return View();
        }
        /// <summary>
        /// 登录成功后主界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Main()
        {
			/*
			if (CurrentUser.CurUser.DepartmentId.Equals("DP201405000001") || CurrentUser.CurUser.DepartmentId.Equals("DP201802000003")) //信访/政府部门
            {
                ViewData["Welcome"] = "/welcome/welcomezf";
                // ViewData["Welcome"] = "welcomeqy";
                // ViewData["Welcome"] = "welcomegc";
            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("DP201802000002")) //五方主体
            {
                ViewData["Welcome"] = "/welcome/welcomeqy";             
            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("DP201802000001")) //劳务部门
            {
                ViewData["Welcome"] = "/welcome/welcomegc";
            }
            else
            {
                ViewData["Welcome"] = "/user/welcome";
            }*/
            //string welcome = GlobalVariable.GetSysSettingValue(true, "GLOBAL_PAGE_MAIN_URL");
            //if (string.IsNullOrEmpty(welcome))
            string welcome = "/user/welcome";
            ViewBag.url = welcome;
            ViewBag.Realname = CurrentUser.RealName;
            return View();
        }

        [LoginAuthorize]
        public ActionResult MainNew()
        {
            string welcome = "/user/welcome";
            ViewBag.url = welcome;
            ViewBag.Realname = CurrentUser.RealName;
            return View();
        }
        [Authorize]
        public ActionResult MainJcjt()
        {
            string welcome = "/user/welcome";
            ViewBag.url = welcome;
            ViewBag.Realname = CurrentUser.RealName;
            return View();
        }
        [Authorize]
        public ActionResult MainV3()
        {
            string welcome = "/user/welcome";
            ViewBag.url = welcome;
            ViewBag.Realname = CurrentUser.RealName;
            return View();
        }
        [Authorize]
        public ActionResult WelcomeJg()
        {
            return View();
        }
        /// <summary>
        /// 空欢迎页面，管理员用
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeNull()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult WelcomeBg()
        {
            return View();
        }
        /// <summary>
        /// 监管3屏网页第一版
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeJgV1()
        {
            return View();
        }

        /// <summary>
        /// 绍兴检测监管首页演示
        /// </summary>
        /// <returns></returns>
        public ActionResult WelcomeJgV2()
        {
            return View();
        }

        /// <summary>
        /// 临沂检测监管首页演示
        /// </summary>
        /// <returns></returns>
        public ActionResult WelcomeJgV3()
        {
            return View();
        }
        /// <summary>
        /// 办公3屏网页第一版
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeBgV1()
        {
            return View();
        }
        [Authorize]
        public ActionResult WelcomeRyV1()
        {
            return View();
        }
        [Authorize]
        public ActionResult WelcomeQyV1()
        {
            return View();
        }
		[Authorize]
        public ActionResult MainWz()
        {
            if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201405000001") || CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000003") || CurrentUser.CurUser.DepartmentId.Equals("DP201707000005")) //信访/政府部门
            {
                ViewData["Welcome"] = "/WzWgry/index";
                ViewData["yjstyle"] = "";
                // ViewData["Welcome"] = "welcomeqy";
                // ViewData["Welcome"] = "welcomegc";
            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
            {
                ViewData["Welcome"] = "/WzWgry/qyIndex";
                ViewData["yjstyle"] = "display:none";
            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
            {
                ViewData["Welcome"] = "/WzWgry/gcIndex";
                ViewData["yjstyle"] = "display:none";

            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000004")) //银行
            {
                ViewData["Welcome"] = "/WzWgry/bank";
                ViewData["yjstyle"] = "";
            }
            else
            {
                ViewData["Welcome"] = "/WzWgry/gcIndex";
                ViewData["yjstyle"] = "display:none";
            }
            return View();
        }



        /// <summary>
        /// 桌面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Desktop()
        {
            return View();
        }
        /// <summary>
        /// 重新加载流程的用户角色信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult InitVars()
        {
            return View();
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangeInfo()
        {
            string usercode = CurrentUser.UserCode;
            string username = CurrentUser.RealUserName.GetSafeString();
            string realname = CurrentUser.RealName.GetSafeString();
            string phone = "";
            string sfzhm = "";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm,sfzhm from view_i_m_zh where zh='" + CurrentUser.RealUserName + "' ");
            if (dt.Count > 0)
            {
                phone = dt[0]["sjhm"];
                if (GlobalVariable.RySfzEncode)
                    sfzhm =CryptFun.LrDecode(dt[0]["sfzhm"]);
            }
            else
            {
                IList<IDictionary<string, string>> dtt = CommonService.GetDataTable("select phone,idno from userinfo where username='" + CurrentUser.RealUserName + "' ");
                if (dtt.Count > 0){
                    phone = dtt[0]["phone"];
                    sfzhm = dtt[0]["idno"];
                }
            }
            ViewData["usercode"] = usercode;
            ViewData["username"] = username;
            ViewData["realname"] = realname;
            ViewData["phone"] = phone;
            ViewData["sfzhm"] = sfzhm;
            return View();
        }
        /// <summary>
        /// 修改登陆名，不带短信验证
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangeInfo2()
        {
            string usercode = CurrentUser.UserCode;
            string username = CurrentUser.RealUserName.GetSafeString();
            string realname = CurrentUser.RealName.GetSafeString();
            string phone = "";
            string sfzhm = "";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm,sfzhm from view_i_m_zh where zh='" + CurrentUser.RealUserName + "' ");
            if (dt.Count > 0)
            {
                phone = dt[0]["sjhm"];
                sfzhm = dt[0]["sfzhm"];
            }
            else
            {
                IList<IDictionary<string, string>> dtt = CommonService.GetDataTable("select phone,idno from userinfo where username='" + CurrentUser.RealUserName + "' ");
                if (dtt.Count > 0)
                {
                    phone = dtt[0]["phone"];
                    sfzhm = dt[0]["idno"];
                }
            }
            ViewData["usercode"] = usercode;
            ViewData["username"] = username;
            ViewData["realname"] = realname;
            ViewData["phone"] = phone;
            ViewData["sfzhm"] = sfzhm;
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePass()
        {
            string username = CurrentUser.RealUserName.GetSafeString();
            string phone = "";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm from view_i_m_zh where zh='" + CurrentUser.RealUserName + "' ");
            if (dt.Count > 0)
            {
                phone = dt[0]["sjhm"];
            }
            else
            {
                IList<IDictionary<string, string>> dtt = CommonService.GetDataTable("select phone from userinfo where username='" + CurrentUser.RealUserName + "' ");
                if (dtt.Count > 0)
                    phone = dtt[0]["phone"];
            }
            ViewData["username"] = username;
            ViewData["phone"] = phone;

            return View();
        }
        [Authorize]
        public ActionResult ChangePass2()
        {
            return View();
        }
        public void CheckChangePass()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["CHANGEPASS_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["CHANGEPASS_VERIFY_CODE"] = null;
                        code = msg == "";
                    }
                }
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


        public void CheckChangeInfo()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["CHANGEINFO_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["CHANGEINFO_VERIFY_CODE"] = null;
                        code = msg == "";
                    }
                }
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

        public ActionResult ResetPass()
        {
            string username = Request["username"].GetSafeString();
            string sjhm = "";

            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select sjhm from VIEW_I_M_ZH where zh='" + username + "'");
            if (dt.Count > 0)
            {
                sjhm = dt[0]["sjhm"].GetSafeString();
            }
            else
            {
                IList<IDictionary<string, string>> ddt = CommonService.GetDataTable("select phone from userinfo where username='" + username + "'");
                if (ddt.Count > 0)
                {
                    sjhm = ddt[0]["phone"].GetSafeString();
                }
            }
            ViewBag.username = username;
            ViewBag.phone = sjhm;
            return View();
        }

        public ActionResult ResetPassFirstStep()
        {
            ViewBag.resettype = Request["type"].GetSafeRequest();
            ViewBag.realname = Request["xm"].GetSafeRequest();
            ViewBag.msg = Request["msg"].GetSafeRequest();
            return View();
        }

        public JsonResult CheckResetPassUsername(string username)
        {
            string msg = "";
            bool code = false;
            try
            {
                string zh = "";
                string sjhm = "";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zh, sjhm from VIEW_I_M_ZH where zh='" + username + "'");
                if (dt.Count > 0)
                {
                    zh = dt[0]["zh"].GetSafeString();
                    sjhm = dt[0]["sjhm"].GetSafeString();
                }
                else
                {
                    IList<IDictionary<string, string>> ddt = CommonService.GetDataTable("select username, phone from userinfo where username='" + username + "'");
                    if (ddt.Count > 0)
                    {
                        zh = ddt[0]["username"].GetSafeString();
                        sjhm = ddt[0]["phone"].GetSafeString();
                    }

                }
                if (zh == "")
                {
                    msg = "用户名不存在，请联系窗口工作人员！";
                }
                else if (sjhm == "")
                {
                    msg = "手机号码未登记，请先到窗口办理登记！";
                }


                code = msg == "";
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                //Response.Write(JsonFormat.GetRetString(code, msg));
            }
            return Json(new { code = code ? "0" : "1", msg = msg });

        }

        public void CheckResetPass()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"].GetSafeString();
                string phone = Request["phone"].GetSafeString();
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["RESETPASS_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["CHANGEPASS_VERIFY_CODE"] = null;

                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from VIEW_I_M_ZH where zh='" + username + "' and sjhm='" + phone + "' ");
                        if (dt[0]["sum"].GetSafeInt() == 0)
                        {
                            IList<IDictionary<string, string>> ddt = CommonService.GetDataTable("select count(*) as sum from userinfo where username='" + username + "' and phone='" + phone + "' ");
                            if (ddt[0]["sum"].GetSafeInt() == 0)
                            {
                                msg = "账户或者手机号码与注册时不一致，请重新填写！";
                            }
                        }

                        code = msg == "";
                    }
                }
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


        /// <summary>
        /// 日程安排
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Calendar()
        {
            ViewData["ul"] = CurrentUser.CurUser.DutyLevel;
            return View();
        }
        /// <summary>
        /// 修改日程安排
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CalendarEdit()
        {
            ViewData["date"] = Request["date"].GetSafeString();
            ViewData["id"] = Request["id"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 工作托管
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Hosting()
        {
            ViewData["username"] = CurrentUser.UserName;
            return View();
        }
        /// <summary>
        /// 设置签名
        /// </summary>
        [Authorize]
        public ActionResult SetSign()
        {
            return View();
        }
        /// <summary>
        /// 欢迎页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Welcome()
        {
            /*
            string page1, page2;
            //考勤管理部门
            if (CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("kqglbm")))
            {
                page1 = "display:none;";
                page2 = "";
            }
            else
            {
                page1 = "";
                page2 = "display:none;";
            }
            // 注册企业/注册人员，用于显示邮件还是今日工作
            string pageMail, pageTask;
            if (CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("zcqybm")) || CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("zcrybm")))
            {
                pageTask = "display:none;";
                pageMail = "";
            }
            else
            {
                pageTask = "";
                pageMail = "display:none;";
            }

            ViewData["Page1"] = page1;
            ViewData["Page2"] = page2;
            ViewData["pageTask"] = pageTask;
            ViewData["pageMail"] = pageMail;*/
            ViewData["Page1"] = "display:none;";
            ViewData["Page2"] = "";
            return View();
        }
        /// <summary>
        /// 菜单页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Menus()
        {
            ViewData["groupid"] = Request["groupid"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 上传签名
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UploadSignature()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Partner()
        {
            return View();
        }


        public ActionResult Downs()
        {
            return View();
        }

        public ActionResult DownIe()
        {
            return View();
        }
        public ActionResult DownsV3()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            ViewBag.UserName = CurrentUser.RealUserName;
            ViewBag.RealName = CurrentUser.RealName;
            string sql = "select qymc from i_m_qy where qybh in (select qybh from i_m_ry where rybh=(select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'))";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
                ViewBag.CompanyName = dt[0]["qymc"].GetSafeString();
            else
                ViewBag.CompanyName = "";
            return View();
        }
        public ActionResult Agreement()
        {
            return View();
        }
        [Authorize]
        public ActionResult UserOut()
        {
            ViewBag.UserName = CurrentUser.RealUserName;
            ViewBag.RealName = CurrentUser.RealName;

            var qybh = CommonService.GetSingleData("select qybh from i_m_ry where rybh = (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')").GetSafeString();

            if (!string.IsNullOrEmpty(qybh))
            {
                var qymc = CommonService.GetSingleData("select qymc from i_m_qy where qybh = '" + qybh + "'").GetSafeString();

                if (!string.IsNullOrEmpty(qymc))
                    ViewBag.CompanyName = qymc;
                else
                    ViewBag.CompanyName = qybh;
            }
            else
            {
                ViewBag.CompanyName = "";
            }

            return View();
        }


        [Authorize]
        public ActionResult AddKqjUser()
        {
            return View();
        }

        [Authorize]
        public ActionResult ZJModifyUserPhone()
        {
            string rybh = Request["rybh"].GetSafeString();
            string ryxm = Request["ryxm"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();
            ViewBag.rybh = rybh;
            ViewBag.ryxm = ryxm;
            ViewBag.sjhm = sjhm;
            return View();
        }


        [Authorize]
        public ActionResult ZJModifyQYPhone()
        {
            string qybh = Request["qybh"].GetSafeString();
            string lxrxm = Request["lxrxm"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();
            ViewBag.qybh = qybh;
            ViewBag.lxrxm = lxrxm;
            ViewBag.sjhm = sjhm;
            return View();
        }
        public ActionResult YC_Login()
        {
            bool code = false;
            string msg = "";
            string err = "";
            string url = Request["url"].GetSafeString();
            try
            {
                //时间戳
                string timestring = Request["timestring"].GetSafeString();
                //校验码
                string sign = Request["sign"].GetSafeString();
                string signstr = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
                string username = Request["username"].GetSafeString();
                if (sign == MD5Util.StringToMD5Hash(signstr, true))
                {
                    bool ret = Remote.UserService.LoginWithOutPassWord(username,Configs.AppId, out err);

                    // 登录成功
                    if (ret)
                    {
                        // 设置日志系统用户
                        BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                        // 设置流程模块用户
                        BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                            new WorkFlow.Common.SessionUser()
                            {
                                CompanyId = CurrentUser.CurUser.CompanyId,
                                CompanyName = CurrentUser.CurUser.CompanyName,
                                DepartmentId = CurrentUser.CurUser.DepartmentId,
                                DepartmentName = CurrentUser.CurUser.DepartmentName,
                                DutyLevel = CurrentUser.CurUser.DutyLevel,
                                RealName = CurrentUser.CurUser.RealName,
                                UserName = CurrentUser.CurUser.UserName
                            }, null);
                        // 设置录入界面用户
                        Session["USERCODE"] = CurrentUser.UserCode;
                        Session["USERNAME"] = CurrentUser.UserName;
                        Session["REALNAME"] = CurrentUser.RealName;
                        Session["CPCODE"] = CurrentUser.CompanyCode;
                        Session["CPNAME"] = CurrentUser.CompanyName;
                        Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                        Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                        Session["SJHM"] = SystemService.GetUserMobile(CurrentUser.UserCode);
                        // 企业及个人用户企业编号
                        Session["USERQYBH"] = JcService.GetQybh(CurrentUser.UserCode);
                        //Session["MenuCode"] = "QYGL_QYBA";
					    //设置当前登录劳资账号所在工程的jdzch
                        SetJDZCH(CurrentUser.UserName);
                        CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                        // 设置用户桌面项
                        bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                        if (!status)
                            SysLog4.WriteLog(err);
                    
                        // 获取页面跳转类型
                        IList<IDictionary<string, string>>  dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                        if (dt.Count > 0)
                            CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                        else
                            CurrentUser.CurUser.UrlJumpType = "SYS";
                    }

                
                    BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                    {
                        ClientType = LogConst.ClientType,
                        Ip = ClientInfo.Ip,
                        LogTime = DateTime.Now,
                        ModuleName = LogConst.ModuleUser,
                        Operation = LogConst.UserOpLogin,
                        UserName = username,
                        RealName = ret ? CurrentUser.RealName : "",
                        Remark = "",
                        Result = ret
                    };
                    LogService.SaveLog(log);
                }
                else
                {
                    url = "/oa/error";
                }
                
            }
            catch(Exception e)
            {  }
            url = HttpUtility.UrlEncode(url);         
            return new RedirectResult("/user/ifr?url=" + url);
        }

        [Authorize]
        public ActionResult ifr()
        {
            ViewBag.url = Request["url"].GetSafeString();
            return View();
        }


        #endregion
        #region 获取各种列表
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        [Authorize]
        public void GetMenus()
        {
            List<MenuItem> menus = new List<MenuItem>();
            try
            {
                menus = CurrentUser.Menus;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(menus));
                Response.End();
            }
        }
        [Authorize]
        public JsonResult GetMenusV2()
        {
            VMenuRetV2 ret = new VMenuRetV2();

            try
            {
                ret.user_pic = SkinManager.GetImagePath("Web-Icons1_03.png");
                ret.user_name = CurrentUser.RealName;
                ret.one_caidan = new List<VMenuRetV2Item1>();

                List<MenuItem> menus = CurrentUser.Menus;
                foreach (MenuItem item in menus)
                {
                    if (!item.IsGroup)
                        continue;
                    VMenuRetV2Item1 menu1 = new VMenuRetV2Item1();

                    menu1.one_caidan_pic_class = item.ImageUrl;
                    menu1.one_caidan_name = item.MenuName;
                    menu1.one_caidan_english = "";
                    #region 丁力
                    menu1.MenuId = item.MenuCode;
                    menu1.MenuUrl = item.MenuUrl;
                    #endregion
                    menu1.two_caidan = new List<VMenuRetV2Item2>();
                    int count = 0;
                    foreach (MenuItem subitem in menus)
                    {
                        if (subitem.ParentCode == item.MenuCode && !subitem.IsGroup)
                        {
                            VMenuRetV2Item2 menu2 = new VMenuRetV2Item2();
                            menu1.two_caidan.Add(menu2);
                            menu2.two_caidan_name = subitem.MenuName;
                            menu2.two_caidan_pic_class = subitem.ImageUrl;
                            menu2.MenuId = subitem.MenuCode;
                            menu2.MenuUrl = subitem.MenuUrl;
                            menu2.two_caidan_three = "false";
                            menu2.IsOut = ((count++) == 0 ) ? "true" : "false";
                        }
                    }

                    if (menu1.two_caidan.Count > 0 || item.Memo =="1")
                        ret.one_caidan.Add(menu1);

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取桌面项
        /// </summary>
        [Authorize]
        public void GetDesktopItems()
        {
            IList<ViewSelfDesktopItem> items = new List<ViewSelfDesktopItem>();
            try
            {
                items = SystemService.GetUserDesktopItems(CurrentUser.UserName);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(items));
                Response.End();
            }
        }
        /// <summary>
        /// 获取日程安排数据
        /// </summary>
        [Authorize]
        public void GetCalendarInfo()
        {
            IList<VUserCalendar> items = new List<VUserCalendar>();
            try
            {
                string username = CurrentUser.UserName;
                // 有单位外出记录权限的，或者岗位为4的的获取所有外出记录，有管理部门的获取管理部门外出记录，其他获取自己的
                if (CurrentUser.CurUser.DutyLevel != "1" && !Request["self"].GetSafeBool())
                {
                    IList<RemoteUserService.VUser> users = new List<RemoteUserService.VUser>();
                    if (CurrentUser.CurUser.DutyLevel == "4" || CurrentUser.HasRight("JCBGM0504"))
                        users = UserService.Users;
                    else
                    {
                        var q = from e in UserService.Users where CurrentUser.CurUser.ManageDep.IndexOf(e.DEPCODE) > -1 select e;
                        users = q.ToList<RemoteUserService.VUser>();
                    }
                    foreach (RemoteUserService.VUser user in users)
                        username += "," + user.USERNAME;
                }
                items = SystemService.GetUserCalendar(CurrentUser.UserName,
                    username,
                    new DateTime(1970, 1, 1).AddHours(8).AddSeconds(Request["start"].GetSafeInt()),
                    new DateTime(1970, 1, 1).AddHours(8).AddSeconds(Request["end"].GetSafeInt()));

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(items));
                Response.End();
            }
        }
        /// <summary>
        /// 获取某个日程安排数据
        /// </summary>
        [Authorize]
        public void GetCalendarDetail()
        {
            VUserCalendar ret = null;
            try
            {
                ret = SystemService.GetCalendar(Request["id"].GetSafeInt(), CurrentUser.UserName);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret != null ? 0 : 1, ret));
                Response.End();
            }
        }

        /// <summary>
        /// 获取树状规章制度
        /// </summary>
        [Authorize]
        public void GetCompanyRules()
        {
            StringBuilder ret = new StringBuilder();
            try
            {
                IList<IDictionary<string, string>> categorys = CommonService.GetDataTable("select ItemKey,ItemName from HelpRuleCategory order by DisplayOrder");
                IList<IDictionary<string, string>> rules = CommonService.GetDataTable("select * from CompanyRule order by RuleCategory,DisplayOrder");
                ret.Append("[");
                foreach (IDictionary<string, string> category in categorys)
                {
                    if (ret.Length > 1)
                        ret.Append(",");
                    ret.Append("{\"id\":\"c_" + category["itemkey"] + "\",");
                    ret.Append("\"text\":\"" + category["itemname"] + "\",");
                    //ret.Append("\"state\":\"closed\",");

                    ret.Append("\"children\": [");
                    bool isfirst1 = true;
                    foreach (IDictionary<string, string> rule in rules)
                    {
                        if (rule["rulecategory"] != category["itemkey"])
                            continue;
                        if (!isfirst1)
                            ret.Append(",");
                        isfirst1 = false;
                        string[] arrrulefiles = rule["rulefile"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        ret.Append("{");
                        //没有文件
                        if (rule["rulefile"] == "")
                        {
                            ret.Append("\"id\":\"null_" + rule["recid"] + "\",");
                            ret.Append("\"text\":\"" + rule["rulename"] + "（无文件）\"");
                        }
                        // 只有一个文件直接显示文件
                        else if (arrrulefiles.Length == 1)
                        {
                            ret.Append("\"id\":\"" + arrrulefiles[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0] + "\",");
                            ret.Append("\"text\":\"" + rule["rulename"] + "\"");
                        }
                        // 有多个文件显示规章制度名称，再显示文件
                        else
                        {
                            ret.Append("\"id\":\"r_" + rule["recid"] + "\",");
                            ret.Append("\"text\":\"" + rule["rulename"] + "\",");
                            ret.Append("\"state\":\"open\",");
                            ret.Append("\"children\": [");
                            bool isfirst2 = true;
                            foreach (string strfile in arrrulefiles)
                            {
                                string[] arrtmp = strfile.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (arrtmp.Length < 2)
                                    continue;
                                if (!isfirst2)
                                    ret.Append(",");
                                isfirst2 = false;

                                ret.Append("{");
                                ret.Append("\"id\":\"" + arrtmp[0] + "\",");
                                ret.Append("\"text\":\"" + arrtmp[1] + "\"");
                                ret.Append("}");
                            }
                            ret.Append("]");
                        }
                        ret.Append("}");
                    }
                    ret.Append("]}");
                }
                ret.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(ret.ToString());
                Response.End();
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [Authorize]
        public void GetUsers()
        {
            IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {

                foreach (RemoteUserService.VUser vuser in Remote.UserService.Users)
                    users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(users));
                Response.End();
            }
        }
        /// <summary>
        /// 获取内网用户
        /// </summary>
        [Authorize]
        public void GetInnerUsers()
        {
            IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {
                foreach (RemoteUserService.VUser vuser in Remote.UserService.FileShareUsers)
                    users.Add(new KeyValuePair<string, string>(vuser.USERCODE, vuser.REALNAME));

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(users));
                Response.End();
            }
        }
        /// <summary>
        /// 获取托管工作给我的用户 
        /// </summary>
        [Authorize]
        public void GetHostedUsers()
        {
            IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {

                IList<string> hosteds = SystemService.GetHostedUsers(CurrentUser.UserName);
                foreach (string username in hosteds)
                {
                    var q = from e in Remote.UserService.Users where e.USERNAME.Equals(username, StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() > 0)
                    {
                        RemoteUserService.VUser user = q.First();
                        users.Add(new KeyValuePair<string, string>(user.USERNAME, user.REALNAME));
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(users));
                Response.End();
            }
        }


        /// <summary>
        /// 获取内网用户
        /// </summary>
        [Authorize]
        public void GetKqjNotInUsers()
        {
            IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {
                foreach (RemoteUserService.VUser vuser in Remote.UserService.FileShareUsers)
                {
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from userinfo where usercode='" + vuser.USERCODE + "' ");
                    if (dt[0]["sum"].GetSafeInt() == 0)
                    {
                        users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(users));
                Response.End();
            }
        }

        /// <summary>
        /// 获取我托管工作的用户
        /// </summary>
        [Authorize]
        public void GetHostingUser()
        {
            string user = "";
            try
            {
                user = SystemService.GetHostingUser(CurrentUser.UserName);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(user != "", user));
                Response.End();
            }
        }

        [Authorize]
        public void ShowSign()
        {
            byte[] ret = null;
            try
            {
                string sign = "";
                if (Remote.UserService.GetUserSign(CurrentUser.RealUserName, out sign))
                {
                    ret = sign.DecodeBase64Array();

                    string filename = "sign.jpg";
                    string mime = MimeMapping.GetMimeMapping(filename);
                    Response.Clear();
                    Response.ContentType = mime;
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }


        public ActionResult GetUserSign()
        {
            byte[] ret = null;
            try
            {
                string sign = "";
                string username = Remote.UserService.GetUserName(Request["username"].GetSafeString(""));
                if (Remote.UserService.GetUserSign(username, out sign))
                {
                    ret = sign.DecodeBase64Array();
                    /*
                    string filename = "sign.jpg";
                    string mime = MimeMapping.GetMimeMapping(filename);
                    Response.Clear();
                    Response.ContentType = mime;
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();*/
                }
                else
                {
                    Response.StatusCode = 404;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return File(ret, "image/jpeg", "sign.jpg");
        }


        /// <summary>
        /// 获取用户设置项
        /// </summary>
        [Authorize]
        public void GetSettingValue()
        {
            string wtdw = "";
            try
            {
                string key = RouteData.Values["id"].GetSafeString();
                UserSetting item = SystemService.GetUserSetting(CurrentUser.UserName, key);
                if (item != null)
                    wtdw = item.SettingValue.GetSafeString();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(wtdw));
                Response.End();
            }
        }
        /// <summary>
        /// 获取当前用户是否保存个人信息
        /// </summary>
        [Authorize]
        public void GetUserInfoSum()
        {
            bool code = false;
            string msg = "0";
            try
            {

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from userinfo where username='" + CurrentUser.UserName + "' ");

                if (dt.Count > 0)
                    msg = dt[0]["sum"];
                code = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetCurUserInfo()
        {
            bool code = true;
            string msg = "";
            string username = "";
            string realname = "";

            try
            {
                if (CurrentUser.IsLogin)
                {
                    username = CurrentUser.UserName;
                    realname = CurrentUser.RealName;
                }
                else
                {
                    code = false;
                    msg = "当前用户没有登录";
                }
            }
            catch (Exception ex)
            {                
                code = false;
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            finally
            {
                
            }

            IDictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("code", code?"0":"1");
            ret.Add("msg", msg);
            ret.Add("username", username);
            ret.Add("realname", realname);
            return Json(ret);
        }
        #endregion
        #region 页面操作
        /// <summary>
        /// 登录操作
        /// </summary>
        public void DoLogin()
        {
            string err = "";
            bool ret = false;
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();
                string verifyCode = Request["verifycode"].GetSafeString();

                //if (!IsVerifyCodeRight(username, verifyCode, Request))
                //{
                //    err = "短信验证码错误";
                //}
                //else
                {
                    string realname = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qymc from I_M_QY where ZH='" + username + "'");
                    if (dt.Count > 0)
                    {
                        realname = dt[0]["qymc"];
                    }      
                    ret = Remote.UserService.Login(username, realname, password, out err);
                    // 登录成功
                    if (ret)
                    {
                        // 设置日志系统用户
                        BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                        // 设置流程模块用户
                        BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                            new WorkFlow.Common.SessionUser()
                            {
                                CompanyId = CurrentUser.CurUser.CompanyId,
                                CompanyName = CurrentUser.CurUser.CompanyName,
                                DepartmentId = CurrentUser.CurUser.DepartmentId,
                                DepartmentName = CurrentUser.CurUser.DepartmentName,
                                DutyLevel = CurrentUser.CurUser.DutyLevel,
                                RealName = CurrentUser.CurUser.RealName,
                                UserName = CurrentUser.CurUser.UserName
                            }, null);
						Session["UserPowerList"] = null;
                        // 设置录入界面用户
						Session["USERCODE"] = CurrentUser.UserCode;
						Session["USERNAME"] = CurrentUser.UserName;
						Session["REALNAME"] = CurrentUser.RealName;
						Session["CPCODE"] = CurrentUser.CompanyCode;
						Session["CPNAME"] = CurrentUser.CompanyName;
						Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
						Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
						Session["MANAGEDEP"] = CurrentUser.CurUser.ManageDep;
						Session["SJHM"] = SystemService.GetUserMobile(CurrentUser.UserCode);
                        // 企业及个人用户所属企业编号
                        Session["USERQYBH"] = JcService.GetQybh(CurrentUser.UserCode);
                        Session["USERQYMC"] = JcService.GetQymc(Session["USERQYBH"].ToString());
                        // 登录的账号
                        Session["USERBH"] = JcService.GetUserbh(CurrentUser.UserCode);
                        //Session["MenuCode"] = "QYGL_QYBA";
                        //设置当前登录劳资账号所在工程的jdzch
                        //SetJDZCH(CurrentUser.UserName);
                        CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                        // 设置用户桌面项
                        bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                        if (!status)
                            SysLog4.WriteLog(err);

                        // 获取页面跳转类型
                        dt = CommonService.GetDataTable("select zhlx,qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");

                        var qyzhbh = string.Empty;
                        
                        if (dt.Count > 0)
                        {
                            CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                            qyzhbh = dt[0]["qybh"];
                            SetCurQybh(dt[0]["zhlx"], dt[0]["qybh"]);
                            //判断是否为企业账号
                            if (dt[0]["zhlx"].GetSafeString().ToUpper() == "Q")
                            {
                                //企业类型编号
                                Session["LXBH"] = JcService.GetLxbh(qyzhbh);
                            }
                        }
                        else
                            CurrentUser.CurUser.UrlJumpType = "SYS";

                        //账号对应的业务系统编号
                        Session["QYZHBH"] = qyzhbh;
                        dt = CommonService.GetDataTable("select qymc from I_M_QY where qybh = '" + Session["USERQYBH"] + "' ");
                        if (dt.Count > 0)
                            Session["USERQYMC"] = dt[0]["qymc"].GetSafeString();
                        //所属质监站编号
                        dt = CommonService.GetDataTable("select top 1 zjzbh from i_m_nbry where zh='" + CurrentUser.RealUserName + "'");
                        if (dt.Count > 0)
                            Session["ZJZBH"] = dt[0]["zjzbh"];
                        else
                            Session["ZJZBH"] = "";

                        //用户所属角色
                        Session["ROLES"] = UserService.GetRoles(Configs.AppId, CurrentUser.RealUserName);
                    }

                    // 记录登陆日志
                    BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                    {
                        ClientType = LogConst.ClientType,
                        Ip = ClientInfo.Ip,
                        LogTime = DateTime.Now,
                        ModuleName = LogConst.ModuleUser,
                        Operation = LogConst.UserOpLogin,
                        UserName = CurrentUser.UserName,
                        RealName = ret ? CurrentUser.RealName : "",
                        Remark = "",
                        Result = ret
                    };
                    LogService.SaveLog(log);
                }

                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";
                if (err == "")
                {
                    if (ret)
                        err = JsonFormat.GetRetString(ret, CurrentUser.RealName);
                    else
                        err = JsonFormat.GetRetString(ret);
                }
                else
                    err = JsonFormat.GetRetString(false, err);

                Response.Write(err);
                Response.End();
            }
        }


        /// <summary>
        /// 手机登录操作
        /// </summary>
        public void DoLoginForPhone()
        {
            string err = "";
            bool ret = false;
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();


                ret = Remote.UserService.Login(username, password, out err);
                // 登录成功
                if (ret)
                {
                    // 设置日志系统用户
                    BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                    // 设置流程模块用户
                    BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                        new WorkFlow.Common.SessionUser()
                        {
                            CompanyId = CurrentUser.CurUser.CompanyId,
                            CompanyName = CurrentUser.CurUser.CompanyName,
                            DepartmentId = CurrentUser.CurUser.DepartmentId,
                            DepartmentName = CurrentUser.CurUser.DepartmentName,
                            DutyLevel = CurrentUser.CurUser.DutyLevel,
                            RealName = CurrentUser.CurUser.RealName,
                            UserName = CurrentUser.CurUser.UserName
                        }, null);
                    // 设置录入界面用户
                    Session["USERCODE"] = CurrentUser.UserCode;
                    Session["USERNAME"] = CurrentUser.UserName;
                    Session["REALNAME"] = CurrentUser.RealName;
                    Session["CPCODE"] = CurrentUser.CompanyCode;
                    Session["CPNAME"] = CurrentUser.CompanyName;
                    Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                    Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                    //Session["MenuCode"] = "QYGL_QYBA";

                    CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                    // 设置用户桌面项
                    bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                    if (!status)
                        SysLog4.WriteLog(err);
                }

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = LogConst.ClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = LogConst.ModuleUser,
                    Operation = LogConst.UserOpLogin,
                    UserName = username,
                    RealName = ret ? CurrentUser.RealName : "",
                    Remark = "",
                    Result = ret
                };
                LogService.SaveLog(log);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";
                
                if (err == "" && ret)
                    err = JsonFormat.GetRetString(ret, CurrentUser.CurUser.UserName);
                else
                    err = JsonFormat.GetRetString(false, err);

                Response.Write(err);
                Response.End();
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void DoLogout()
        {
            try
            {
                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = LogConst.ClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = LogConst.ModuleUser,
                    Operation = LogConst.UserOpLogout,
                    UserName = CurrentUser.UserName,
                    RealName = CurrentUser.RealName,
                    Remark = "",
                    Result = true
                };
                LogService.SaveLog(log);

                Remote.UserService.Logout();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString());
                Response.End();
            }
        }
        /// <summary>
        /// 初始化用户信息
        /// </summary>
        [Authorize]
        public void DoInitVars()
        {
            string err = "";
            bool ret = true;
            try
            {
                RemoteUserService.InitVars();
                BD.DataInputBll.UserSystemRemoteService.ClearWebServiceData();
                BD.WebListBll.UserSystemRemoteService.ClearWebServiceData();
                BD.Jcbg.Web.Remote.UserService.m_Users = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = false;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        [Authorize]
        public void DoChangePass()
        {
            string oldpass = Request["pass1"].GetSafeString();
            string newpass = Request["pass2"].GetSafeString();
            string err = "";
            bool ret = true;
            try
            {
                ret = Remote.UserService.ChangePass(CurrentUser.RealUserName, oldpass, newpass);
                if (ret)
                {
                    Session["USER_INFO_USERNAME"] = CurrentUser.RealUserName;
                    Session["USER_INFO_PASSWORD"] = newpass;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }


        public void DoChangeInfo()
        {
            string usercode = Request["usercode"].GetSafeString();
            string username = Request["username"].GetSafeString();
            string realname = Request["realname"].GetSafeString();
            string phone = Request["phone"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeRequest();
            if (GlobalVariable.RySfzEncode)
                sfzhm = CryptFun.LrEncode(sfzhm);
            string err = "";
            bool ret = false;
            string code = "2";  // 发生错误时返回的代码
            try
            {
                string oldusername = CurrentUser.RealUserName.GetSafeString();
                ret = ChangeRealnameByUserCode(usercode, CurrentUser.RealName.GetSafeString(), realname, oldusername, out err);

                
                if (ret)
                   ret = ChangeUsernameByUserCode(usercode, oldusername, username, out err) ;    


                if (ret)
                {
                    string sql = "";
                    string sqlt = "";
                    IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select lxsj, qybh from i_m_qy where sptg=1 and zh='" + username + "' ");
                    if (dt1.Count > 0)
                    {
                        sql = string.Format("update i_m_qy set lxsj='{0}' where sptg=1 and zh='{1}'", phone, username);
                        // 修改完手机号码之后，更新该企业所有在建工程的联系人手机号码
                        string qybh = dt1[0]["qybh"].GetSafeString();
                        if (qybh != "")
                        {
                            sqlt = string.Format("UpdateQYLXRSJHM('{0}', '{1}')", qybh, phone);
                        }
                    }
                    else
                    {
                        IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select sjhm, rybh from i_m_ry where sptg=1 and zh='" + username + "' ");
                        if (dt2.Count > 0)
                        {
                            sql = string.Format("update i_m_ry set sjhm='{0}',sfzhm='{2}' where sptg=1 and zh='{1}'", phone, username,sfzhm);
                            // 修改完手机号码之后，更新该人员所有在建工程的手机号码
                            string rybh = dt2[0]["rybh"].GetSafeString();
                            if (rybh != "")
                            {
                                sqlt = string.Format("UpdateRYXX('{0}', '{1}')", rybh, phone);
                            }
                        }
                        else
                        {
                            IList<IDictionary<string, string>> dt3 = CommonService.GetDataTable("select phone from userinfo where usercode='" + usercode + "' ");
                            if (dt3.Count > 0)
                            {
                                sql = string.Format("update userinfo set phone='{0}',username='{1}',idno='{3}' where usercode='{2}'", phone, username, usercode, sfzhm);
                            }
                            else
                            {
                                sql = string.Format("insert into userinfo (username, realname, phone,issubmit, isautosign,usercode,idno) values ( '{0}','{1}','{2}',0,1,'{3}','{4}')", username, realname, phone, usercode, sfzhm);
                            }
                        }
                    }
                    if (sql != "")
                    {
                        IList<string> lssql = new List<string>();
                        lssql.Add(sql);
                        ret = CommonService.ExecTrans(lssql);
                        err = ret ? "" : "修改失败！";
                        // 更新成功
                        if (ret)
                        {
                            // 如果需要更新在建工程的联系人手机号码
                            if (sqlt != "")
                            {
                                CommonService.ExecProc(sqlt, out err);

                            }
                            if (oldusername != username) // 如果用户名发生修改，需要提示用户重新登录
                            {
                                code = "1";
                            }
                            else  // 如果用户名没变，不需要重新登录
                            {
                                code = "0";
                            }
                        }
                    }
                    else
                    {
                        ret = false;
                        err = "用户名不存在！";
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = "2";
                err = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{ \"code\":\"{0}\", \"msg\": \"{1}\"}}", code, err));
                Response.End();
            }
        }


        /// <summary>
        /// 根据用户代码修改用户名
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="oldusername"></param>
        /// <param name="newusername"></param>
        /// <returns></returns>
        private bool ChangeUsernameByUserCode(string usercode, string oldusername, string newusername, out string err)
        {
            bool ret = true;
            err = "";
            // 用户名发生变化，需要更新用户系统中相应的信息，以及办公系统中相应的信息
            if (oldusername != newusername)
            {
                ret = Remote.UserService.ChangeUsernameByUserCode(usercode, newusername, out err);
                if (ret)
                {
                    string sql = "";
                    IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select lxsj from i_m_qy where sptg=1 and zh='" + oldusername + "' ");
                    if (dt1.Count > 0)
                    {
                        sql = string.Format("update i_m_qy set zh='{0}' where sptg=1 and zh='{1}'", newusername, oldusername);
                    }
                    else
                    {
                        IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select sjhm from i_m_ry where sptg=1 and zh='" + oldusername + "' ");
                        if (dt2.Count > 0)
                        {
                            sql = string.Format("update i_m_ry set zh='{0}' where sptg=1 and zh='{1}'", newusername, oldusername);
                        }
                        else
                        {
                            IList<IDictionary<string, string>> dt3 = CommonService.GetDataTable("select phone from userinfo where username='" + oldusername + "' ");
                            if (dt3.Count > 0)
                            {
                                sql = string.Format("update userinfo set username='{0}' where username='{1}'", newusername, oldusername);
                            }
                        }
                    }

                    if (sql != "")
                    {
                        IList<string> lssql = new List<string>();
                        lssql.Add(sql);
                        ret = CommonService.ExecTrans(lssql);
                        err = ret ? "" : "修改用户名失败！";
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 根据用户代码修改用户姓名
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="oldusername"></param>
        /// <param name="newusername"></param>
        /// <returns></returns>
        private bool ChangeRealnameByUserCode(string usercode, string oldrealname, string newrealname, string username, out string err)
        {
            bool ret = true;
            err = "";
            // 用户名发生变化，需要更新用户系统中相应的信息，以及办公系统中相应的信息
            if (oldrealname != newrealname)
            {
                ret = Remote.UserService.ChangeRealnameByUserCode(usercode, newrealname, out err);
                if (ret)
                {
                    string sql = "";
                    IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select lxsj from i_m_qy where sptg=1 and zh='" + username + "' ");
                    if (dt1.Count > 0)
                    {
                        sql = string.Format("update i_m_qy set qymc='{0}' where sptg=1 and zh='{1}'", newrealname, username);
                    }
                    else
                    {
                        IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select sjhm from i_m_ry where sptg=1 and zh='" + username + "' ");
                        if (dt2.Count > 0)
                        {
                            sql = string.Format("update i_m_ry set ryxm='{0}' where sptg=1 and zh='{1}'", newrealname, username);
                        }
                        else
                        {
                            IList<IDictionary<string, string>> dt3 = CommonService.GetDataTable("select phone from userinfo where username='" + username + "' ");
                            if (dt3.Count > 0)
                            {
                                sql = string.Format("update userinfo set realname='{0}' where username='{1}'", newrealname, username);
                            }
                        }
                    }

                    if (sql != "")
                    {
                        IList<string> lssql = new List<string>();
                        lssql.Add(sql);
                        ret = CommonService.ExecTrans(lssql);
                        err = ret ? "" : "修改用户名失败！";
                    }
                }
            }
            return ret;
        }

        public void DoResetPass()
        {
            string username = Request["username"].GetSafeString();
            string newpass = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
            string err = "";
            bool ret = false;
            try
            {
                ret = Remote.UserService.ResetPass(username, newpass, out err);

                if (ret)
                {
                    Session["USER_INFO_USERNAME"] = username;
                    Session["USER_INFO_PASSWORD"] = newpass;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }
        /// <summary>
        /// 保存某个日程安排数据
        /// </summary>
        [Authorize]
        public void SaveCalendar()
        {
            bool ret = true;
            string msg = "";
            try
            {
                bool isallday = Request["isallday"].GetSafeInt() > 0;
                bool isendtime = Request["isend"].GetSafeInt() > 0;
                string starttime = isallday ? Request["startdate"].GetSafeString() : Request["starttime"].GetSafeString();
                string endtime = isendtime ? (isallday ? Request["enddate"].GetSafeString() : Request["endtime"].GetSafeString()) : "1970-1-1";
                VUserCalendar calendar = new VUserCalendar()
                {
                    allDay = isallday,
                    canEdit = true,
                    color = Request["color"].GetSafeString(),
                    end = endtime,
                    id = Request["recid"].GetSafeString(),
                    realname = CurrentUser.RealName,
                    start = starttime,
                    title = Request["event"].GetSafeString(),
                    url = "",
                    username = CurrentUser.UserName
                };
                ret = SystemService.SaveCalendar(calendar, out msg);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        /// <summary>
        /// 删除某个日程安排数据
        /// </summary>
        [Authorize]
        public void DeleteCalendar()
        {
            bool ret = true;
            string msg = "";
            try
            {
                int id = Request["id"].GetSafeInt();
                ret = SystemService.DeleteCalendar(id, CurrentUser.UserName, out msg);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        /// <summary>
        /// 工作托管
        /// </summary>
        [Authorize]
        public void DoSaveHosting()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string username = Request["username"].GetSafeString();
                if (username != "")
                    ret = SystemService.SaveHostingUser(CurrentUser.UserName, username, out msg);
                else
                {
                    ret = false;
                    msg = "无效的托管用户";
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        /// <summary>
        /// 取消托管
        /// </summary>
        [Authorize]
        public void DoCancelHosting()
        {
            bool ret = true;
            string msg = "";
            try
            {
                ret = SystemService.CancelHostingUser(CurrentUser.UserName, out msg);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }

        public void PhoneGetHostinguser()
        {
            int ret = 1;
            string rettext = "";
            string user = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                foreach (RemoteUserService.VUser vuser in Remote.UserService.FileShareUsers)
                    users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

                user = SystemService.GetHostingUser(CurrentUser.UserName);
                ret = 0;
            }
            catch (Exception e)
            {
                user = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                rettext = string.Format("{{\"code\":\"{0}\",\"host\":\"{1}\",\"record\":{2}}}", ret.ToString(), user, jss.Serialize(users));
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
                /*
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(users));
                Response.End();*/
            }
        }

        public void PhoneSaveHosting()
        {
            bool ret = true;
            string msg = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out msg);
                string host = Request["username"].GetSafeString();
                if (username != "" && CurrentUser.UserName != "")
                    ret = SystemService.SaveHostingUser(CurrentUser.UserName, host, out msg);
                else
                {
                    ret = false;
                    msg = "无效的托管用户";
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        public void PhoneCancelHosting()
        {
            bool ret = true;
            string msg = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out msg);
                ret = SystemService.CancelHostingUser(CurrentUser.UserName, out msg);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }


        /// <summary>
        /// 工作托管
        /// </summary>
        [Authorize]
        public void DoAddKqjUser()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string username = Request["username"].GetSafeString();
                if (username != "")
                {

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from  userinfo where usercode='" + username + "' ");
                    if (dt[0]["sum"].GetSafeInt() == 0)
                    {
                        var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.USERNAME.Equals(username, StringComparison.OrdinalIgnoreCase) select e;
                        if (q.Count() > 0)
                        {
                            RemoteUserService.VUser rvu = q.First();
                            string sql = "insert into userinfo(username,Realname,Departmentid,departmentname,usercode)values ('" + rvu.USERNAME + "','" + rvu.REALNAME + "','" + rvu.DEPCODE + "','" + rvu.DEPNAME + "','" + rvu.USERCODE + "')";
                            IList<string> sqls = new List<string>();
                            sqls.Add(sql);
                            ret = CommonService.ExecTrans(sqls, out msg);
                        }
                        else
                        {
                            ret = false;
                            msg = "无效的考勤用户！";
                        }
                    }
                    else
                    {
                        ret = false;
                        msg = "该账户已经添加到考勤人员，请自己录入虹膜";
                    }
                    ret = SystemService.SaveHostingUser(CurrentUser.UserName, username, out msg);
                }
                else
                {
                    ret = false;
                    msg = "无效的考勤用户";
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }


        /// <summary>
        /// 切换帐号
        /// </summary>
        [Authorize]
        public void SetHostedUser()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string username = Request["username"].GetSafeString();
                WorkFlow.Common.SessionUser hosteduser = null;
                if (username != "")
                {
                    var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.USERNAME.Equals(username, StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() > 0)
                    {
                        BD.Jcbg.Web.RemoteUserService.VUser user = q.First();
                        hosteduser = new WorkFlow.Common.SessionUser()
                        {
                            CompanyId = user.CPCODE,
                            CompanyName = user.CPNAME,
                            DepartmentId = user.DEPCODE,
                            DepartmentName = user.DEPNAME,
                            DutyLevel = user.POSTMC,
                            RealName = user.REALNAME,
                            UserName = user.USERNAME
                        };
                    }
                }

                BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                        new WorkFlow.Common.SessionUser()
                        {
                            CompanyId = CurrentUser.CurUser.CompanyId,
                            CompanyName = CurrentUser.CurUser.CompanyName,
                            DepartmentId = CurrentUser.CurUser.DepartmentId,
                            DepartmentName = CurrentUser.CurUser.DepartmentName,
                            DutyLevel = CurrentUser.CurUser.DutyLevel,
                            RealName = CurrentUser.CurUser.RealName,
                            UserName = CurrentUser.CurUser.UserName
                        }, hosteduser);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        /// <summary>
        /// 设置签名
        /// </summary>
        [Authorize]
        public void SaveSetSign()
        {
            bool ret = true;
            string msg = "";
            try
            {
                if (Request.Files.Count > 0)
                {
                    string filename = "";
                    HttpPostedFileBase postfile = Request.Files[0];
                    if (filename == "")
                        filename = postfile.FileName;
                    // 读取文件
                    byte[] postcontent = new byte[postfile.ContentLength];
                    int readlength = 0;
                    while (readlength < postfile.ContentLength)
                    {
                        int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    MyImage img = new MyImage(postcontent);
                    if (!img.IsImage())
                    {
                        msg = "无效的图像文件，支持的签名文件格式为：" + img.GetValidImageDesc();
                        ret = false;
                    }
                    else
                    {
                        byte[] content = img.ConvertToJpg(Configs.SignMaxWidth, Configs.SignMaxHeight);
                        string sign64 = content.EncodeBase64();
                        ret = Remote.UserService.SetUserSign(CurrentUser.RealUserName, sign64, out msg);
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        /// <summary>
        /// 离职操作完成
        /// </summary>
        [Authorize]
        public void DoLeaveCompany()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string username = CurrentUser.RealUserName;
                string procstr = string.Format("DoLeaveCompany('{0}')", username);
                ret = CommonService.ExecProc(procstr, out msg);

                if (ret)
                {
                    ret = JcService.InsertRyBghj(username, CurrentUser.UserName, CurrentUser.RealName, 2, out msg);
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
		/// <summary>
        /// 设置登陆的劳资专管员的qybh和jdzch
        /// </summary>
        /// <param name="loginname"></param>
        [Authorize]
        public void SetJDZCH(string username)
        {
            string jdzch = "";
            string qybh = "";
            string gcbh = "";
            try
            {
                string sql = "select * from I_M_LZZGY_ZH where usercode='" + username + "'";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count != 0)
                {
                    jdzch = dt[0]["jdzch"];
                    qybh = dt[0]["qybh"];
                    gcbh = dt[0]["jdzch"];
                }
                else
                {
                    sql = "select * from I_M_QYZH where yhzh='" + username + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count != 0)
                    {
                        qybh = dt[0]["qybh"];
                    }
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
                Session["GC_JDZCH"] = jdzch;
                Session["GC_GCBH"] = gcbh;
                Session["GC_QYBH"] = qybh;

                CurrentUser.CurUser.Qybh=qybh;
                CurrentUser.CurUser.Jdzch = jdzch;
                CurrentUser.CurUser.GCBH = gcbh;
            }
        }

        /// <summary>
        /// 设置当前登录账号的检测机构编号
        /// </summary>
        /// <param name="zhlx"></param>
        /// <param name="qybh"></param>
        [Authorize]
        public void SetCurQybh(string zhlx,string qybh)
        {
            if(zhlx.ToLower()=="q")
            {
                CurrentUser.CurUser.Qybh = qybh;
                string sql = "select qymc from I_M_QY where qybh='" + qybh + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                    CurrentUser.CurUser.Qymc = dt[0]["qymc"];
            }
            else if (zhlx.ToLower() == "r")
            {
                string sql = "select * from I_M_NBRY_JC where usercode='" + CurrentUser.UserCode + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    CurrentUser.CurUser.Qybh = dt[0]["jcjgbh"];
                    sql = "select qymc from I_M_QY where qybh='" + dt[0]["jcjgbh"] + "'";
                    IList<IDictionary<string, string>> dtq = CommonService.GetDataTable(sql);
                    if (dtq.Count > 0)
                        CurrentUser.CurUser.Qymc = dtq[0]["qymc"];
                }

            }
          
        }
        /// <summary>
        /// 保存内部用户的虹膜模板
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult SaveInnerUserIris(string usercode, string iris)
        {

            bool ret = true;
            string msg = "";
            try
            {
                string sql = "update userinfo set irismodule='" + iris + "' where usercode='" + usercode + "'";
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
                ret = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {

            }
            return Json(new { code = ret ? "0" : "1", msg = msg });
        }
        #endregion

        #region 用户注册第一步
        public ActionResult UserRegisterFirstStep()
        {
            return View();
        }

        /// <summary>
        /// 人员注册校验
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckUserRegisterFirstStep(string realname, string sfzh, string sjhm)
        {
            string msg = "";
            bool code = false;
            try
            {
                realname = realname.GetSafeRequest().Trim();
                sfzh = sfzh.GetSafeRequest().Trim();
                sjhm = sjhm.GetSafeRequest().Trim();
                if (realname != "" && sfzh != "" && sjhm != "")
                {
                    string sql = string.Format("select rybh, ryxm, sfzhm, sjhm from i_m_ry where (sjhm='{0}' or sfzhm='{1}') and sptg = 1 ", sjhm, sfzh);

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);

                    // 没有手机或者身份证号码一样的，新注册
                    if (dt.Count == 0)
                    {
                        code = true;
                    }
                    else
                    {
                        // 姓名，手机，身份证一样，跳转到找回密码
                        var q = from e in dt where e["ryxm"] == realname && e["sfzhm"] == sfzh && e["sjhm"] == sjhm select e;
                        if (q.Count() > 0)
                        {

                            msg = "1";
                        }
                        else
                        {
                            //身份证一样（不管姓名），提示来站里办理
                            q = from e in dt where e["sfzhm"] == sfzh select e;
                            if (q.Count() > 0)
                                msg = "2";
                            else
                            {
                                // 手机号码一样（不管姓名），提示来站里办理
                                msg = "3";
                            }
                        }

                    }
                    /*
                    // 人员姓名与身份证号码相同的记录多于1条
                    if (dt.Count > 1)
                    {
                        code = true;
                        msg = "1";
                        totalcount = dt.Count;
                        data = "";
                    }
                    // 人员姓名与身份证号码相同的记录刚好有1条
                    else if (dt.Count == 1)
                    {
                        code = true;
                        msg = "2";
                        totalcount = dt.Count;
                        data = dt[0]["rybh"];

                    }
                    // 人员姓名与身份证号码相同的记录不存在
                    else if(dt.Count == 0)
                    {
                        sql = string.Format("select rybh, ryxm, sjhm from i_m_ry where ryxm='{0}' ", realname);
                        dt = CommonService.GetDataTable(sql);
                        // 人员姓名相同的记录存在
                        if (dt.Count >= 1)
                        {
                            code = true;
                            msg = "3";
                            totalcount = dt.Count;
                            data = dt[0]["ryxm"];
                        }
                        // 人员姓名相同的记录不存在
                        else if (dt.Count == 0)
                        {
                            code = true;
                            msg = "4";
                            totalcount = dt.Count;
                            data ="";
                        }
                    }*/


                }
                else
                {
                    code = false;
                    msg = "请填写完整的信息！";
                }




            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                //Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"jsondata\":\"" + data + "\"}");

            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion


        #region 用户注册第二步
        public ActionResult UserRegisterSecondStep()
        {
            string realname = Request["ryxm"].GetSafeString();
            ViewBag.realname = realname;
            return View();
        }


        public void CheckUserRegisterSecondStep()
        {
            string msg = "";
            bool code = false;
            int totalcount = 0;
            string data = "";
            try
            {
                string realname = Request["realname"].GetSafeString();
                string sjhm = Request["sjhm"].GetSafeString();
                if (realname != "" && sjhm != "")
                {
                    string sql = string.Format("select rybh, ryxm, sjhm from i_m_ry where ryxm='{0}' and sjhm='{1}' ", realname, sjhm);

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    // 人员姓名与手机号码相同的记录多于1条
                    if (dt.Count > 1)
                    {
                        code = true;
                        msg = "1";
                        totalcount = dt.Count;
                        data = "";
                    }
                    // 人员姓名与手机号码相同的记录刚好有1条
                    else if (dt.Count == 1)
                    {
                        code = true;
                        msg = "2";
                        totalcount = dt.Count;
                        data = dt[0]["rybh"];

                    }
                    // 人员姓名与手机号码相同的记录不存在
                    else if (dt.Count == 0)
                    {
                        code = true;
                        msg = "3";
                        totalcount = dt.Count;
                        data = "";
                    }


                }




            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"jsondata\":\"" + data + "\"}");
            }
        }


        public void CheckUserRegisterSecondStepYZM()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["USERREGISTERSECONDSTEP_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["USERREGISTERSECONDSTEP_VERIFY_CODE"] = null;
                        code = msg == "";
                    }
                }
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

        #region 人员备案修改手机号码
        public ActionResult ModifyUserPhone()
        {
            string rybh = Request["rybh"].GetSafeString();
            string ryxm = Request["ryxm"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();
            ViewBag.rybh = rybh;
            ViewBag.ryxm = ryxm;
            ViewBag.sjhm = sjhm;
            return View();
        }


        public void CheckRYBAModifyPhoneYZM()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["RYBAMODIFYPHONE_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["RYBAMODIFYPHONE_VERIFY_CODE"] = null;
                        code = msg == "";
                    }
                }
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

        public void DoRYBAModifyPhone()
        {
            string msg = "";
            bool code = false;
            try
            {
                string rybh = Request["rybh"].GetSafeString();
                string sjhm = Request["sjhm"].GetSafeString();
                if (rybh != "" && sjhm != "")
                {
                    string sql = string.Format("select rybh from i_m_ry where rybh='{0}'  ", rybh);

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        sql = string.Format("update i_m_ry set sjhm='{0}' where rybh='{1}'", sjhm, rybh);
                        List<string> lssql = new List<string>();
                        lssql.Add(sql);
                        code = CommonService.ExecTrans(lssql);
                        // 人员备案修改了手机号码之后，更新该人员所有在建工程的联系电话 -- 金成龙
                        if (code)
                        {
                            sql = string.Format("UpdateRYXX('{0}', '{1}')", rybh, sjhm);
                            CommonService.ExecProc(sql, out msg);
                        }
                        msg = code ? "" : "修改失败";
                    }
                    else
                    {
                        msg = "无效的人员信息！";
                    }
                }
                else
                {
                    msg = "提交的信息不完整！";
                }


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

        #region 企业备案修改手机号码
        public ActionResult ModifyQYPhone()
        {
            string qybh = Request["qybh"].GetSafeString();
            string lxrxm = Request["lxrxm"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();
            ViewBag.qybh = qybh;
            ViewBag.lxrxm = lxrxm;
            ViewBag.sjhm = sjhm;
            return View();
        }


        public void CheckQYBAModifyPhoneYZM()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["QYBAMODIFYPHONE_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["QYBAMODIFYPHONE_VERIFY_CODE"] = null;
                        code = msg == "";
                    }
                }
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

        public void DoQYBAModifyPhone()
        {
            string msg = "";
            bool code = false;
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                string lxrxm = Request["lxrxm"].GetSafeString();
                string sjhm = Request["sjhm"].GetSafeString();
                if (qybh != "" && sjhm != "")
                {
                    string sql = string.Format("select qybh from i_m_qy where qybh='{0}'  ", qybh);

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        sql = string.Format("update i_m_qy set lxsj='{0}', qyfzr='{1}' where qybh='{2}'", sjhm, lxrxm, qybh);
                        List<string> lssql = new List<string>();
                        lssql.Add(sql);
                        code = CommonService.ExecTrans(lssql);
                        // 企业备案管理，修改企业联系人和联系电话之后，更新该企业的所有在建工程的联系信息
                        if (code)
                        {
                            sql = string.Format("UpdateQYLXRXX('{0}', '{1}', '{2}')", qybh, lxrxm, sjhm);
                            CommonService.ExecProc(sql, out msg);

                        }
                        msg = code ? "" : "修改失败";
                    }
                    else
                    {
                        msg = "无效的企业信息！";
                    }
                }
                else
                {
                    msg = "提交的信息不完整！";
                }


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

        #region 用户自定义菜单

        public ActionResult Setting()
        {
            return View();
        }

        #region 登录短信验证码
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JsonResult GetLoginVerifyCode(string username, string password)
        {
            bool code = false;
            string msg = "";

            try
            {
                if (!GlobalVariable.LoginSmsVerify(Request))
                    msg = "不需要短信验证";
                else
                {
                    string key = "USER_CONTROLLER_VERIFY_CODE_TIME_" + username;
                    int validMinutes = GlobalVariable.LoginSmsValidMinutes(Request);
                    var obj = HttpRuntime.Cache.Get(key);
                    if (obj != null)
                    {
                        IDictionary<string, object> cacheItem =obj as IDictionary<string,object>;
                        int timediff = (int)(cacheItem["time"].GetSafeDate().AddMinutes(validMinutes).Subtract(DateTime.Now).TotalSeconds);
                        var timeDesc = "";
                        if (timediff > 60)
                            timeDesc = timediff / 60 + "分";
                        else
                            timeDesc = timediff + "秒";
                        msg = "无法重复发送，请" + timeDesc + "后再试";
                    }
                    else
                    {
                        string usercode, realname;
                        if (UserService.CheckLogin(username, password, out usercode, out realname, out msg))
                        {
                            if (SelfService.GetPhone(usercode, out msg))
                            {
                                string phone = msg;
                                string verifycode = RandomNumber.GetNew(RandomType.Number, GlobalVariable.LoginSmsLength(Request));
                                string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId(Request);
                                string vctemplate = GlobalVariable.LoginSmsTemplate(Request);

                                SmsRequestVerifyCode smsParam = new SmsRequestVerifyCode()
                                {
                                    invokeId = vcinvokeid,
                                    phoneNumber = phone,
                                    templateCode = vctemplate,
                                    contentVar = new SmsVarVerifyCode()
                                    {
                                        code = verifycode,
                                        name= realname,
                                        time = validMinutes+"分钟"
                                    }
                                };
                                if (SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(), new Guid().ToString(), phone, (new JavaScriptSerializer()).Serialize(smsParam), out msg))
                                {
                                    IDictionary<string, object> cacheItem = new Dictionary<string, object>();
                                    cacheItem.Add("time", DateTime.Now);
                                    cacheItem.Add("code", verifycode);
                                    HttpRuntime.Cache.Insert(key, cacheItem, null, DateTime.Now.AddMinutes(validMinutes), TimeSpan.Zero);
                                    code = true;
                                }
                            }
                        }
                        else
                            msg = "用户密码校验失败";
                    }
                }
            }
            catch (Exception ex)
            {
                code = false;
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        /// <summary>
        /// 判断请求短信验证码是否有效，如果不需要短信，返回成功
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool IsVerifyCodeRight(string username, string verifycode, HttpRequestBase request)
        {
            if (!GlobalVariable.LoginSmsVerify(request))
                return true;
            string key = "USER_CONTROLLER_VERIFY_CODE_TIME_" + username;
            var obj = HttpRuntime.Cache.Get(key);
            if (obj == null)
                return false;
            IDictionary<string, object> cacheItem = obj as IDictionary<string, object>;
            string shouldCode = cacheItem["code"].GetSafeString();
            return shouldCode == verifycode;
        }
        #endregion

        /// <summary>
        /// 保存用户自定义菜单
        /// </summary>
        [Authorize]
        public void SaveUserMenuSetting()
        {
            string msg = "";
            bool code = true;
            try
            {
                string setting = Request["setting"].GetSafeString();
                string usercode = CurrentUser.UserName;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;

                if (setting != "")
                {
                    Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(setting);
                    if (dt.Keys.Contains("menuid"))
                    {
                        string pmenud = dt["menuid"].GetSafeString();
                        IList<Dictionary<string, object>> submenus = (dt["twomenu"] as ArrayList).Cast<Dictionary<string, object>>().ToList();
                        if (submenus == null || submenus.Count == 0)
                        {
                            code = false;
                            msg = "二级菜单数据不能为空！";
                        }
                        else
                        {
                            string sql = string.Format("delete from usermenusetting where usercode='{0}' and pmenuid='{1}'", usercode, pmenud);
                            CommonService.Execsql(sql);

                            List<string> lsql = new List<string>();
                            foreach (var item in submenus)
                            {
                                string menuid = item["menuid"].GetSafeString();
                                string isout = item["isout"].GetSafeString();
                                if (menuid != "" && isout != "")
                                {
                                    string s = string.Format("insert into usermenusetting (usercode, pmenuid, menuid, isout) values ('{0}','{1}','{2}',{3})", usercode, pmenud, menuid, isout);
                                    lsql.Add(s);
                                }
                            }
                            if (lsql.Count > 0)
                            {
                                CommonService.ExecTrans(lsql);
                            }

                        }
                    }
                    else
                    {
                        code = false;
                        msg = "一级菜单数据不能为空！";
                    }

                }
                else
                {
                    msg = "自定义菜单数据不能为空！";
                }


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

        /// <summary>
        /// 获取当前用户自定义菜单
        /// </summary>
        [Authorize]
        public void GetUserMenuSetting()
        {
            string msg = "";
            bool code = true;
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                string usercode = CurrentUser.UserName;
                string sql = string.Format("select * from usermenusetting where usercode='{0}'", usercode);
                IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                if (d.Count > 0)
                {
                    // 查找所有一级菜单
                    List<string> lpmenus = d.Select(x => x["pmenuid"].GetSafeString()).Distinct().ToList();
                    // 遍历一级菜单，查找相应的二级菜单
                    foreach (var pm in lpmenus)
                    {
                        List<Dictionary<string, object>> submenus = new List<Dictionary<string, object>>();
                        var q = d.Where(x => x["pmenuid"].GetSafeString() == pm).ToList();
                        foreach (var item in q)
                        {
                            Dictionary<string, object> sub = new Dictionary<string, object>();
                            sub.Add("menuid", item["menuid"]);
                            sub.Add("isout", item["isout"]);
                            submenus.Add(sub);
                        }

                        Dictionary<string, object> info = new Dictionary<string, object>();
                        info.Add("menuid", pm);
                        info.Add("twomenu", submenus);
                        list.Add(info);
                    }
                }


            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(list)));
                Response.End();
            }
        }
        /// <summary>
        /// 同步用户
        /// 用于两个项目之间的用户同步（业务表用户数据）
        /// 前提：两个项目公用一个用户系统
        /// </summary>
        public void SyncUser()
        {
            string msg = "";
            bool code = true;
            try
            {
                string rybh = Request["rybh"].GetSafeString();
                string ryxm = Request["ryxm"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();
                string zh = Request["zh"].GetSafeString();
                string sjhm = Request["sjhm"].GetSafeString();
                string yhzh = Request["yhzh"].GetSafeString();
                string timestring = Request["timestring"].GetSafeString();
                string sign = Request["sign"].GetSafeString();
                string signstr = String.Format("timestring={0}&secret={1}", timestring, "sync_user");
                if (sign == MD5Util.StringToMD5Hash(signstr, true))
                {
                    if (yhzh != "" && zh != "")
                    {
                        // 校验yhzh是否已存在
                        string sql = string.Format("select count(*) as num from i_m_qyzh where yhzh='{0}'", yhzh);
                        IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(sql);
                        int num = 0;
                        if (ddt.Count > 0)
                        {
                            num = ddt[0]["num"].GetSafeInt();
                        }
                        if (num == 0)
                        {
                            // 校验zh是否已存在
                            sql = string.Format("select count(*) as num from i_m_ry where (zh='{0}' or sfzhm='{1}')", zh, sfzhm);
                            ddt = CommonService.GetDataTable2(sql);
                            if (ddt.Count > 0)
                            {
                                num = ddt[0]["num"].GetSafeInt();
                            }
                            if (num == 0)
                            {
                                // 生成新的人员编号
                                string newrybh = "";
                                ISession session = null;
                                ITransaction transaction = null;
                                try
                                {
                                    //初始化数据库信息
                                    session = DataInputService.GetDBSession();
                                    IDbCommand cmd = session.Connection.CreateCommand();
                                    transaction = session.BeginTransaction();
                                    transaction.Enlist(cmd);
                                    newrybh = DataInputService.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_M_RY__RYBH'|maxbhfield-ZDBH", "I_M_RY", "RYBH", null, cmd);
                                    if (newrybh == "")
                                    {
                                        transaction.Rollback();
                                        code = false;
                                        msg = "获取rybh失败！";
                                    }
                                    else
                                    {
                                        // 插入i_m_qyzh
                                        string s = "insert into i_m_qyzh (qybh, yhzh, sfqyzzh,lrsj,zhlx) " +
                                                    " values ('{0}','{1}',0,getdate(),'R')";
                                        s = string.Format(s, newrybh, yhzh);
                                        DataInputService.ExecSql(s, cmd);

                                        // 插入i_m_ry
                                        s = "insert into i_m_ry (lxbh,rybh,ryxm,sfzhm,lrsj,sptg,sfyx,zh,sjhm,tyyhxy,zlyz)" +
                                            " values ('00', '{0}','{1}','{2}',getdate(),1,1,'{3}','{4}',1,1)";
                                        s = string.Format(s, newrybh, ryxm, sfzhm, zh, sjhm);
                                        DataInputService.ExecSql(s, cmd);

                                        transaction.Commit();
                                    }

                                }
                                catch (Exception ex)
                                {

                                    if (transaction != null)
                                    {
                                        transaction.Rollback();
                                    }
                                    code = false;
                                    msg = ex.Message;
                                    SysLog4.WriteLog(ex);
                                }
                                finally
                                {
                                    if (session != null)
                                    {
                                        session.Close();
                                    }
                                }



                            }
                            else
                            {
                                code = false;
                                msg = "zh或者sfzhm已存在！";
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "yhzh已存在!";
                        }

                    }
                    else
                    {
                        code = false;
                        msg = "参数错误！";
                    }

                }
                else
                {
                    code = false;
                    msg = "校验失败";
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 登录页修改手机号码
        public ActionResult ModifyPhone()
        {
            return View();
        }
        #endregion
    }
}
