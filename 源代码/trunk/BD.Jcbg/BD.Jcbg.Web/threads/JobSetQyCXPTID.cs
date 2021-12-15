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

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 从全国建筑市场监管公共服务平台
    /// 获取企业的平台ID
    /// </summary>
    public class JobSetQyCXPTID : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobSetQyCXPTID");
            while (true)
            {
                try
                {
                    string msg = "";
                    IList<string> lsql  =  new List<string>();

                    string sql = "select qybh, qymc, zzjgdm from i_m_qy " +
                        " where (qybh is not null and qybh<>'') " +
                        " and (zzjgdm is not null and zzjgdm<>'')" +
                        " and (qgcxptid is null or qgcxptid='')";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        foreach (var row in dt)
                        {
                            string qybh = row["qybh"].GetSafeString();
                            string zzjgdm = row["zzjgdm"].GetSafeString();
                            string qymc = row["qymc"].GetSafeString();
                            if (zzjgdm!="")
                            {
                                string qyid = "";
                                if (QGJZSCJGGGFWPT.GetQyid(zzjgdm, out msg, out qyid))
                                {
                                    string s = string.Format("update i_m_qy set qgcxptid='{0}' where qybh='{1}'", qyid, qybh);
                                    lsql.Add(s);
                                }
                                else
                                {
                                    SysLog4.WriteError(string.Format("无法获取企业[{0}]的平台ID,详细错误：\r\n{1}", qymc, msg));
                                }
                                
                            }
                        }

                    }

                    if (lsql.Count > 0)
                    {
                        CommonService.ExecTrans(lsql);
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