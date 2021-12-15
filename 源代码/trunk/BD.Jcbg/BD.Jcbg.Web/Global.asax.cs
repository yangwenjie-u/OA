using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BD.Jcbg.Common;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.IBll;
using System.Threading;
using BD.Jcbg.Web.threads;
using BD.Common;
using System.Web.Http;

namespace BD.Jcbg.Web
{
	// 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
	// 请访问 http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "ProblemVoice",   // 媒体路由
                "jdbg/p-{id}.mp3",
                new { controller = "Jdbg", action = "GetProblemVoice", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "ProblemImageBig",   // 媒体路由
                "jdbg/p-b{id}.jpg",
                new { controller = "Jdbg", action = "GetProblemImageBig", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "ProblemImageSmall",   // 媒体路由
                "jdbg/p-s{id}.jpg",
                new { controller = "Jdbg", action = "GetProblemImageSmall", id = UrlParameter.Optional }
            );
            //整改附件大图片
            routes.MapRoute(
                "WorkflowImageBig", // 路由名称
                "workflow/p-b{id}.jpg", // 带有参数的 URL
                new { controller = "workflow", action = "GetWorkFlowAttachBig", id = UrlParameter.Optional } // 参数默认值
            );
            //整改附件小图片
            routes.MapRoute(
                "WorkflowImageSmall", // 路由名称
                "workflow/p-s{id}.jpg", // 带有参数的 URL
                new { controller = "workflow", action = "GetWorkFlowAttachSmall", id = UrlParameter.Optional } // 参数默认值
            );
            //routes.MapRoute(
            //    "Default", // 路由名称
            //    "{controller}/{action}/{id}", // 带有参数的 URL
            //    new { controller = "xwwzUser", action = "Index", id = UrlParameter.Optional } // 参数默认值
            //);
            //routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            routes.MapRoute(
				"Default", // 路由名称
				"{controller}/{action}/{id}", // 带有参数的 URL
				new { controller = "User", action = "Login", id = UrlParameter.Optional } // 参数默认值
			);
            

		}

        protected void Application_Start()
        {
            InitLoginJump();


            SysEnvironment.IsWeb = true;
            InitOfficeService();

            BD.Log.Common.SysLog4.SetConfig();
            SysLog4.SetConfig();
            BD.WorkFlow.Common.SysLog4.SetConfig();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Func.Zhwx.StartSend();
            //Func.BHZ.StartGet();

            Func.Phone.StartSend();
            Func.YSSQAutoFinish.StartSend();
            //短信通知
            Func.SmsSend.StartSend();
            //考勤时间
            Func.KqjThread.StartSendMsg();

            //Func.WebSocket.InitSocket();

            // 启动线程
            ThreadConfig tconfig = new ThreadConfig();
            IList<ThreadConfigItem> threads = tconfig.Load();
            string dllpath = string.Format(@"{0}\bin\BD.Jcbg.Web.dll", SysEnvironment.CurPath);
            foreach (ThreadConfigItem titem in threads)
            {
                if (!titem.ItemStart)
                    continue;
                object obj = titem.ItemClass.CreateObject(dllpath);
                if (obj != null)
                {
                    ISchedulerJob job = (ISchedulerJob)obj;
                    StartThread(job, titem.ItemInterval);
                }
            string msg = "";
            IDCardOperation.Init(SysEnvironment.CurPath + @"\skins\default\fonts",
                SysEnvironment.CurPath + @"\skins\default\images\正面.jpg",
                SysEnvironment.CurPath + @"\skins\default\images\背面.jpg", out msg);

            

            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Func.BHZ.StopGet();
            Func.Phone.StopSend();
            Func.YSSQAutoFinish.StopSend();
            Func.SmsSend.StopSend();
            Func.KqjThread.StopSendMsg();
            //Func.WebSocket.StopSocket();
            // websocket
            //WebSocket.StopSocket();

            foreach (Thread thread in m_Threads)
            {
                thread.Abort();
            }
        }


        protected List<Thread> m_Threads = new List<Thread>();
        protected void StartThread(ISchedulerJob job, int Interval)
        {
            job.SetInterval(Interval);
            System.Threading.ThreadStart start = new System.Threading.ThreadStart(job.Execute);
            Thread thread = new System.Threading.Thread(start);
            thread.Start();
            m_Threads.Add(thread);
        }

        protected void InitOfficeService()
        {
            ReportPrint.InitReportPrint r = new ReportPrint.InitReportPrint();
            r.ConfigsPath = "~/configs";
            r.WebPath = "~/ReportPrint";
            r.ViewEngines = ViewEngines.Engines;
            r.Server = Server;
            r.encoding = Encoding.UTF8;
            r.webConfigPath = "~/Views/Web.config";
            var err = "";
            var flag = r.Initialize(out err);
            if (!flag)
            {
                SysLog4.WriteLog("初始化office报表服务失败：" + err);
            }
            r.AddTempPath(Server.MapPath(@"~\report\jdbg"));
            r.AddTempPath(Server.MapPath(@"~\report\jc\wts"));
            r.AddTempPath(Server.MapPath(@"~\report\pdftemp"));

            r.AddSignPath(@"http://wz.jzyglxt.com/user/GetUserSign?username={0}");
            r.AddSignPath(@"http://wz.jzyglxt.com/report/1.jpg");
            r.AddSignPath(@"http://zjzhjg.jzyglxt.com/user/GetUserSign?username={0}");
            r.SetAuthorize(() =>
            {
                return CurrentUser.IsLogin;
            });
            r.SetDebug(true);

        }


        #region LoginJump

        protected void InitLoginJump()
        {
            LoginJump.InitializeClient.LoginUrl = "/User/Login";
            LoginJump.InitializeClient.Login = new LoginJump.InitializeClient.LoginDelegate(Login);
        }


        internal bool Login(string username, string userpwd, HttpResponseBase Response, System.Web.HttpSessionStateBase Session, out string err)
        {
            err = "";
            try
            {
                bool ret = Remote.UserService.Login(username.GetSafeString(), userpwd.GetSafeString(), out err, Session);
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
                    ISystemService SystemService = ContextRegistry.GetContext().GetObject("SystemService") as ISystemService;
                    bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                    if (!status)
                    {
                        SysLog4.WriteLog(err);
                        //return false;
                    }
                    // 获取页面跳转类型
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                        CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                    else
                        CurrentUser.CurUser.UrlJumpType = "SYS";

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
                    BD.Log.IBll.ILogService LogService = ContextRegistry.GetContext().GetObject("LogService") as BD.Log.IBll.ILogService;
                    LogService.SaveLog(log);
                    return true;
                }
                else
                {
                    if (err=="")
                    {
                        err = "用户名或者密码错误，登录失败！";
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                err = ex.Message;
                return false;
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

        /// <summary>
        /// 设置登陆的劳资专管员的qybh和jdzch
        /// </summary>
        /// <param name="loginname"></param>
        [System.Web.Mvc.Authorize]
        public void SetJDZCH(string loginname, System.Web.HttpSessionStateBase Session)
        {
            string jdzch = "";
            string qybh = "";
            string gcbh = "";
            try
            {
                string sql = "select * from I_M_LZZGY_ZH where zh='" + loginname + "'";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count != 0)
                {
                    jdzch = dt[0]["jdzch"];
                    qybh = dt[0]["qybh"];
                    gcbh = dt[0]["gcbh"];
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                Session["GC_JDZCH"] = jdzch;
                Session["GC_GCBH"] = gcbh;
                Session["GC_QYBH"] = qybh;

                CurrentUser.CurUser.Qybh = qybh;
                CurrentUser.CurUser.Jdzch = jdzch;
                CurrentUser.CurUser.GCBH = gcbh;
            }
        }
        #endregion

    }
}