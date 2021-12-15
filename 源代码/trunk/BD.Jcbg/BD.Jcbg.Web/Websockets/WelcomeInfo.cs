using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using BD.Jcbg.Common;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.IBll;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;

namespace BD.Jcbg.Web.Websockets
{
    public class WelcomeInfo : WebSocketBehavior
    {
        #region 服务
        private BD.Jcbg.IBll.ICommonService _commonService = null;
        private BD.Jcbg.IBll.ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
                }
                return _commonService;
            }
        }
        private IOaService _oaService = null;
        private IOaService OaService
        {
            get
            {
                if (_oaService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _oaService = webApplicationContext.GetObject("OaService") as IOaService;
                }
                return _oaService;
            }
        }
        private IWorkFlowService _workFlowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workFlowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workFlowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workFlowService;
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
        #endregion
        private Timer timer = null;

        protected override void OnOpen()
        {
            try
            {
                string configs = Context.QueryString["configs"].GetSafeString();
                if (configs != "")
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    // 获取配置项
                    Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(configs);
                    //Send(configs);
                    //Send(dt["refresh"].GetSafeString());
                    SendMsg(dt);
                    timer = new Timer((obj) => { SendMsg(dt); }, null, 1000, dt["refresh"].GetSafeInt(5000));
                }

            }
            catch (Exception e)
            {
                Send(e.StackTrace);
            }



        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data + "-- from server !");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            Send(string.Format("server error: {0}", e.Message));
        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
        }

        private void SendMsg(Dictionary<string, object> configs)
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string,object>();
            try
            {
                // 用户信息配置项
                Dictionary<string, object> userinfo = configs["userinfo"] as Dictionary<string, object>;
                // 内部邮件配置项
                Dictionary<string, object> nbyj = configs["nbyj"] as Dictionary<string, object>;
                //待办工作配置项
                Dictionary<string, object> dbgz = configs["dbgz"] as Dictionary<string, object>;
                // 用户名
                string username = userinfo["username"].GetSafeString();

                //内部邮件数据
                int pageindex = nbyj["page"].GetSafeInt(1);
                int pagesize = nbyj["rows"].GetSafeInt(10);
                string hasread = nbyj["hasread"].GetSafeString();
                int totalcount = 0;
                IList<IDictionary<string, string>> datas = OaService.GetPageMails(MailFolderType.ReceiveBox, username, "", hasread, "", "", pagesize, pageindex, out totalcount);
                data.Add("nbyj", new Dictionary<string, object>() {
                    { "total", totalcount},
                    { "rows",datas}
                });

                //待办工作
                IList<ViewTodoTask> todotasks = new List<ViewTodoTask>();
                pageindex = dbgz["page"].GetSafeInt(1);
                pagesize = dbgz["rows"].GetSafeInt(10);
                string key = dbgz["key"].GetSafeString();
                todotasks = WorkFlowService.GetTodoTasks(username, key, pagesize, pageindex, out totalcount);
                data.Add("dbgz", new Dictionary<string, object>() {
                    { "total", totalcount},
                    { "rows",todotasks}
                });
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.ToString();
                
            }
            finally
            {
                Send(GetJson(ret, msg, data));
            }
        }

        private string GetJson(bool code, string msg, object data)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = int.MaxValue;
            return string.Format("{{\"code\": \"{0}\", \"msg\": \"{1}\", \"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(data));

        }
    }
}