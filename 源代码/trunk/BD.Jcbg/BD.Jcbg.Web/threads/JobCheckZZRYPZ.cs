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
    /// 绍兴市建筑业资质管理系统
    /// 校验资质人员配置是否异常
    /// </summary>
    public class JobCheckZZRYPZ : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobCheckZZRYPZ");
            while (true)
            {
                try
                {
                    bool ret = true;
                    string msg = "";
                    string sql = "select top 100 * from view_jdbg_qyzz_zzfw where (lastchecktime is null or dateadd(day,2,lastchecktime) < getdate()) ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        foreach (var row in dt)
                        {
                            string zzid = row["zzid"].GetSafeString();
                            string zzfw = row["zzfw"].GetSafeString();
                            string procname = row["procname"].GetSafeString();
                            if (procname !="" && zzid!="" && zzfw!="")
                            {
                                string procstr = string.Format("{0}('{1}','{2}')", procname, zzid, zzfw);
                                if (!CommonService.ExecProc(procstr, out msg))
                                {
                                    SysLog4.WriteError(msg);
                                }
                            }
                        }
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