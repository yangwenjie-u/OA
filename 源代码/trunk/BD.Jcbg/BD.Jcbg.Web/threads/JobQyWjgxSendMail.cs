using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;


namespace BD.Jcbg.Web.threads
{
    public class JobQyWjgxSendMail: ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒

        #region 服务
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
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }
        public void Execute()
        {
            while (true)
            {
                try
                {
                    string msg = "";
                    string proc = string.Format("FlowQYHZSendMail()");
                    CommonService.ExecProc(proc, out msg);
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }


                Thread.Sleep(Interval);
            }
        }
    }
}