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
    /// 诸暨市智慧建管平台
    /// 推送工程形象进度和工程状态到务工人员平台
    /// </summary>
    public class JobSyncWgryGCZT:ISchedulerJob
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
            SysLog4.WriteError("开始线程JobSyncWgryGCZT");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    int topnum = Configs.GetConfigItem("SyncWgryGcTopnum").GetSafeInt(1);
                    
                    string sql = string.Format("select top {0} * from jdbg_gc_wgry_sync where LastSyncTime is null order by recid", topnum.ToString());
                    IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(sql);

                    if (ddt.Count > 0)
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;
                        foreach (var gc in ddt)
                        {
                            int recid = gc["recid"].GetSafeInt();
                            string gcbh = gc["gcbh"].GetSafeString();
                            string xxjd = gc["xxjd"].GetSafeString();
                            int gczt = gc["gczt"].GetSafeInt();
                            string jgysrq = gc["jgysrq"].GetSafeString();
                            string wgptbh = "ZJBG";
                            string url = Configs.GetConfigItem("SyncWgryGcUrl").GetSafeString();
                            if (url !="")
                            {
                                string token = WgryToken.GetToken();
                                if (token !="")
                                {
                                    string postdata = string.Format("token={0}&gcbh_yc={1}&gczt={2}&xxjd={3}&wgptbh={4}",
                                            HttpUtility.UrlEncode(token),
                                            HttpUtility.UrlEncode(gcbh),
                                            HttpUtility.UrlEncode(gczt.ToString()),
                                            HttpUtility.UrlEncode(xxjd),
                                            HttpUtility.UrlEncode(wgptbh),
                                            HttpUtility.UrlEncode(jgysrq)
                                        );
                                    string retstring = MyHttp.SendDataByPost(url, postdata);
                                    SysLog4.WriteError(retstring);
                                    if (retstring !="")
                                    {
                                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                                        if (retdata != null)
                                        {
                                            string nsql = "";
                                            string code = retdata["code"].GetSafeString();
                                            // 推送成功
                                            if (code == "0")
                                            {
                                                nsql = string.Format("delete from jdbg_gc_wgry_sync where recid={0}", recid.ToString());
                                            }
                                            else
                                            {
                                                msg = retdata["msg"].GetSafeString();
                                                nsql = string.Format(" update jdbg_gc_wgry_sync set lastsynctime=getdate(), syncerror='{0}' where recid={1}", msg, recid.ToString());
                                            }
                                            if (nsql !="")
                                            {
                                                CommonService.Execsql(nsql);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    SysLog4.WriteError("务工人员平台工程状态同步获取token失败！");
                                }
                            }
                            else
                            {
                                SysLog4.WriteError("务工人员平台工程状态同步URL为空！");
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