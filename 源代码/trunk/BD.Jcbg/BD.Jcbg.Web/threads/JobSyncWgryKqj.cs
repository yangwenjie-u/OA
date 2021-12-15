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
    /// 从务工人员平台获取工程的考勤机数量
    /// </summary>
    public class JobSyncWgryKqj:ISchedulerJob
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
            SysLog4.WriteError("开始线程JobSyncWgryKqj");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    int topnum = Configs.GetConfigItem("SyncWgryKqjTopnum").GetSafeInt(1);
                    int hour = Configs.GetConfigItem("SyncWgryKqjLastHour").GetSafeInt(8);
                    string sql = $"select top {topnum} recid,gcbh from i_m_gc where kqjslsynctime is null or dateadd(hour,{hour},kqjslsynctime) < getdate() order by kqjslsynctime";
                    IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(sql);

                    if (ddt.Count > 0)
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;
                        foreach (var gc in ddt)
                        {
                            int recid = gc["recid"].GetSafeInt();
                            string gcbh = gc["gcbh"].GetSafeString();
                            string wgptbh = "ZJBG";
                            string url = Configs.GetConfigItem("SyncWgryKqjUrl").GetSafeString();
                            if (url !="")
                            {
                                string token = WgryToken.GetToken();
                                if (token !="")
                                {
                                    string postdata = string.Format("token={0}&gcbh_yc={1}&wgptbh={2}",
                                            HttpUtility.UrlEncode(token),
                                            HttpUtility.UrlEncode(gcbh),
                                            HttpUtility.UrlEncode(wgptbh)
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
                                            int kqjsl = 0;
                                            // 推送成功
                                            if (code == "0")
                                            {
                                                ArrayList kqjlist = retdata["data"] as ArrayList;
                                                if (kqjlist != null )
                                                {
                                                    kqjsl = kqjlist.Count;
                                                }
                                                
                                            }
                                            else
                                            {
                                                SysLog4.WriteError( retdata["msg"].GetSafeString());
                                                
                                            }

                                            nsql = string.Format("update i_m_gc set kqjsl={0},kqjslsynctime=getdate() where recid={1}", kqjsl.ToString(),recid.ToString());

                                            if (nsql !="")
                                            {
                                                CommonService.Execsql(nsql);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    SysLog4.WriteError("务工人员平台获取token失败！");
                                }
                            }
                            else
                            {
                                SysLog4.WriteError("务工人员平台获取工程考勤机列表URL为空！");
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