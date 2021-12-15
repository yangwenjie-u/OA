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
    /// 往检测监管平台推送注册人员
    /// </summary>
    public class JobBhgbgSendMail : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobBhgbgSendMail");
            while (true)
            {
                try
                {
                    string msg = "";
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string urlprefix = Configs.GetConfigItem("bhgbgsendmailurl").GetSafeString();
                    int topnum = Configs.GetConfigItem("bhgbgsendmailtop").GetSafeInt(10);
                    string sql = $"select top {topnum} * from jdbg_bhgbg_tz where sendtime is null ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        foreach (var item in dt)
                        {
                            string recid = item["recid"];
                            string gcbh = item["gcbh"];
                            string bgwyh = item["bgwyh"];
                            string bgdata = item["bgdata"];
                            if (gcbh !="" && bgwyh!="" && bgdata!="")
                            {
                                Dictionary<string, object> data = jss.Deserialize<Dictionary<string, object>>(DataFormat.DecodeBase64(bgdata));
                                if (data != null && data.Count > 0)
                                {
                                    string wtdbh = data["委托单编号"].GetSafeString();
                                    string jcjgmc = data["检测单位名称"].GetSafeString();
                                    string syxmmc = data["试验项目名称"].GetSafeString();
                                    string bgbh = data["报告编号"].GetSafeString();
                                    string bgurl = urlprefix + HttpUtility.UrlEncode(bgwyh);
                                    string procstr = string.Format("GCBHGBG_SendMail('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", 
                                            gcbh,bgwyh,wtdbh,jcjgmc,syxmmc,bgbh,bgurl
                                        );
                                    CommonService.ExecProc(procstr, out msg);
                                    sql = string.Format("update jdbg_bhgbg_tz set sendtime=getdate() where recid={0}", recid);
                                    CommonService.Execsql(sql);
                                }
                            }
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


    }
}