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
using BD.Jcbg.Web.Func;
using System.Collections;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 绍兴市建筑业资质管理系统
    /// 定时清除重复的人员附件
    /// </summary>
    public class JobClearDuplicatedRYFJ : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobCheckRYSBSH");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    string procstr = string.Format("ClearDuplicateRYFJ()");

                    success = CommonService.ExecProc(procstr, out msg);
                    if (!success)
                    {
                        SysLog4.WriteError("清除重复的人员附件出错 ： " + msg);
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