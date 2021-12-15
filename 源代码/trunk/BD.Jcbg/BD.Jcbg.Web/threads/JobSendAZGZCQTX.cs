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

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 诸暨智慧建管云平台
    /// 安装告知超过一个月之后未办理使用登记，通知安监站
    /// </summary>
    public class JobSendAZGZCQTX:ISchedulerJob
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
            SysLog4.WriteError("开始线程JobSendAZGZCQTX");
            while (true)
            {
                try
                {
                    string procstr = "SendMailAZGZCQ()";
                    string msg = "";
                    if(!CommonService.ExecProc(procstr, out msg))
                    {
                        SysLog4.WriteError(msg);
                    }
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