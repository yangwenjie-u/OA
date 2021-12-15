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
    /// 向温州市建设工程质量监督站推动监督抽查联系单确认状态
    /// </summary>
    public class JobPushJdcclxdQrzt : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobPushJdcclxdQrzt");
            while (true)
            {
                try
                {
                    string msg = "";
                    string topnum = Configs.GetConfigItem("pushjdcclxdtopnum").GetSafeString();
                    string sql = "select top " + topnum + " * from jg_jdbg_jdccrwwtjl where isconfirmed=1 and (isdeal is null or isdeal=0) ";
                    IList<IDictionary<string, object>> lxdlist = CommonService.GetDataTable2(sql);
                    if (lxdlist.Count > 0)
                    {
                        foreach (var lxd in lxdlist)
                        {
                            string recid = lxd["recid"].GetSafeString();
                            string workserial = lxd["workserial"].GetSafeString();
                            string url = Configs.GetConfigItem("pushjdcclxdurl").GetSafeString();
                            if (url != "")
                            {
                                string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                string sign = String.Format("timestring={0}&secret={1}", timestring, "sync_jdcclxd");
                                sign = MD5Util.StringToMD5Hash(sign, true);
                                string postdata = string.Format("workserial={0}&timestring={1}&sign={2}",
                                        HttpUtility.UrlEncode(workserial),
                                        HttpUtility.UrlEncode(timestring),
                                        HttpUtility.UrlEncode(sign)
                                    );
                                string result = MyHttp.SendDataByPost(url, postdata);
                                SysLog4.WriteError(result);
                                if (result != "")
                                {
                                    JavaScriptSerializer jss = new JavaScriptSerializer();
                                    jss.MaxJsonLength = int.MaxValue;
                                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                                    if (retdata != null)
                                    {
                                        string code = retdata["code"].GetSafeString();
                                        // 推送成功之后更新状态
                                        if (code == "0")
                                        {
                                            sql = string.Format("update jg_jdbg_jdccrwwtjl set isdeal=1 where recid={0}", recid);
                                            CommonService.Execsql(sql);
                                        }
                                        else
                                        {
                                            SysLog4.WriteError("推送监督抽查联系单确认状态失败：" + retdata["msg"].GetSafeString());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                SysLog4.WriteError("无法获取pushjdcclxdurl");
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