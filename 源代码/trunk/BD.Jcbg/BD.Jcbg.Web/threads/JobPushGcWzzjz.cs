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
    /// 往检测监管平台推送工程的正式监督登记号
    /// </summary>
    public class JobPushGcWzzjz :ISchedulerJob
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
            SysLog4.WriteError("开始线程JobPushGcWzzjz");
            while (true)
            {
                try
                {
                    string msg = "";
                    string topnum = Configs.GetConfigItem("pushgctopnum").GetSafeString("10");
                    string sql = "select top " + topnum + " * from jdbg_gc_push where isdeal=0 ";
                    IList<IDictionary<string, object>> gclist = CommonService.GetDataTable2(sql);
                    if (gclist.Count > 0)
                    {
                        sql = "select * from H_JCJG_JDCCLXD_CONFIG where lx='PUSHGC'";
                        IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                        string url = "";
                        string postdata = "";
                        if (dt.Count > 0)
                        {
                            url = dt[0]["url"].GetSafeString();
                            postdata = dt[0]["fixedparam"].GetSafeString();
                        }
                        if (url !="")
                        {
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            jss.MaxJsonLength = int.MaxValue;
                            foreach (var gc in gclist)
                            {
                                string recid = gc["recid"].GetSafeString();
                                string gcbh = gc["gcbh"].GetSafeString();
                                string gcmc = gc["gcmc"].GetSafeString();
                                string zjdjh = gc["zjdjh"].GetSafeString();
                                postdata = postdata +
                                    "&gcbh=" + HttpUtility.UrlEncode(gcbh) +
                                    "&zjdjh=" + HttpUtility.UrlEncode(zjdjh);

                                string retstring = MyHttp.SendDataByPost(url, postdata);

                                if (retstring != "")
                                {
                                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                                    if (retdata != null && retdata.Count > 0)
                                    {
                                        string code = retdata["code"].GetSafeString();
                                        // 调用成功
                                        if (code == "0")
                                        {
                                            sql = string.Format("update jdbg_gc_push set isdeal=1, pushtime=getdate() where recid={0}", recid);
                                            CommonService.Execsql(sql);
                                        }
                                        else
                                        {
                                            msg = retdata["msg"].GetSafeString();
                                            SysLog4.WriteError(string.Format("推送工程失败：\r\n工程名称:{0}\r\n错误信息:{1}", gcmc, msg));
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            SysLog4.WriteError("推送工程Url未配置");
                        }
                        

                    }
                }
                catch (Exception ex)
                {
                    SysLog4.WriteError(ex.Message);

                }


                Thread.Sleep(Interval);
            }

        }

        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }
    }
}