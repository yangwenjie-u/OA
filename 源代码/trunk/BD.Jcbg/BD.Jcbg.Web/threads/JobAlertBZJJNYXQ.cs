using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;
using System.IO;
using ReportPrint.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BD.Jcbg.Web.Func.SCXPT;
using System.Collections;
using BD.Jcbg.Web.Func;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 诸暨市智慧建管平台
    /// 保证金有效期到期提醒
    /// </summary>
    public class JobAlertBZJJNYXQ : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
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
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobAlertBZJJNYXQ");
            while (true)
            {
                try
                {
                    string msg = "";
                    string procstr = "FlowAlertBZJJNYXQ()";
                    CommonService.ExecProc(procstr, out msg);

                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }

        }
    }
}