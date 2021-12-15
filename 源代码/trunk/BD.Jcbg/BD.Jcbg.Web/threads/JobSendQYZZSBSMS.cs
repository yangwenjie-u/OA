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
    /// 绍兴市建筑业企业资质管理系统
    /// 发送短信
    /// </summary>
    public class JobSendQYZZSBSMS : ISchedulerJob
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

        private ISmsService _smsService = null;
        private ISmsService SmsService
        {
            get
            {
                if (_smsService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsService = webApplicationContext.GetObject("SmsService") as ISmsService;
                }
                return _smsService;
            }
        }
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobSendQYZZSBSMS");
            while (true)
            {
                try
                {
                    bool success = true;
                    string msg = "";
                    string sql = "select top 20 * from jdbg_qyzzsb_sms where (issend is null or issend=0) order by recid ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string vcinvokeid = GlobalVariable.GetSmsBaseInvokeId(null);
                        foreach (var item in dt)
                        {
                            int recid = item["recid"].GetSafeInt();
                            string zzid = item["zzid"].GetSafeString();
                            string sjhm = item["sjhm"].GetSafeString();
                            string smstpl = item["smstpl"].GetSafeString();
                            string fieldlist = item["fieldlist"].GetSafeString();
                            
                            Dictionary<string, string> contentVars = new Dictionary<string, string>();
                            // 获取模板中的变量值
                            if (zzid !="")
                            {
                                string s = string.Format("select top 1 * from view_jdbg_qyzzsb where id='{0}'", zzid);
                                IList<IDictionary<string, string>> zzlist = CommonService.GetDataTable(s);
                                if (zzlist.Count > 0)
                                {
                                    var zzinfo = zzlist[0];
                                    if (fieldlist!="")
                                    {
                                        string[] fields = fieldlist.Split(new char[] { ',' });
                                        foreach (var f in fields)
                                        {
                                            var v = zzinfo[f].GetSafeString();
                                            contentVars.Add(f, v);
                                        }
                                    }
                                }
                            }

                            // 生成发送的内容对象
                            Dictionary<string, object> contentObj = new Dictionary<string, object>(){
                                {"invokeId", vcinvokeid},
                                {"phoneNumber", sjhm },
                                {"templateCode", smstpl },
                                { "contentVar", contentVars}
                            };
                            string contents = jss.Serialize(contentObj);
                            success = SmsService.SendMessage(GlobalVariable.GetSmsBaseAppId(null), Guid.NewGuid().ToString(), sjhm, contents, out msg);
                            if (success)
                            {
                                sql = string.Format(" update jdbg_qyzzsb_sms set issend=1 where recid={0}", recid.ToString());
                                CommonService.Execsql(sql);
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