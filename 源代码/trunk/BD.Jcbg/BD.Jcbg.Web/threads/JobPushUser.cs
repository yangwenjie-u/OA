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
    public class JobPushUser : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobPushUser");
            while (true)
            {
                try
                {
                    string msg = "";
                    string topnum = Configs.GetConfigItem("pushusertopnum").GetSafeString();
                    string sql = "select top " + topnum + " * from i_m_ry where (issync is null or issync=0) ";
                    IList<IDictionary<string, object>> rylist = CommonService.GetDataTable2(sql);
                    if (rylist.Count > 0)
                    {
                        foreach (var ry in rylist)
                        {
                            string rybh = ry["rybh"].GetSafeString();
                            string ryxm = ry["ryxm"].GetSafeString();
                            string sfzhm = ry["sfzhm"].GetSafeString();
                            string zh = ry["zh"].GetSafeString();
                            string sjhm = ry["sjhm"].GetSafeString();
                            string yhzh = "";
                            if (rybh !="" && zh!="")
                            {
                                sql = string.Format("select top 1 * from i_m_qyzh where qybh='{0}' and sfqyzzh=0", rybh);
                                IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(sql);
                                if (ddt.Count > 0)
                                {
                                    yhzh = ddt[0]["yhzh"].GetSafeString();
                                    if (yhzh !="")
                                    {
                                        string url = Configs.GetConfigItem("pushuserurl").GetSafeString();
                                        if (url !="")
                                        {
                                            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            string sign = String.Format("timestring={0}&secret={1}", timestring, "sync_user");
                                            sign = MD5Util.StringToMD5Hash(sign, true);
                                            string postdata = string.Format("ryxm={0}&sfzhm={1}&zh={2}&sjhm={3}&yhzh={4}&timestring={5}&sign={6}",
                                                    HttpUtility.UrlEncode(ryxm),
                                                    HttpUtility.UrlEncode(sfzhm),
                                                    HttpUtility.UrlEncode(zh),
                                                    HttpUtility.UrlEncode(sjhm),
                                                    HttpUtility.UrlEncode(yhzh),
                                                    HttpUtility.UrlEncode(timestring),
                                                    HttpUtility.UrlEncode(sign)
                                                );
                                            string result = MyHttp.SendDataByPost(url, postdata);
                                            SysLog4.WriteError(result);
                                            if (result !="")
                                            {
                                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                                jss.MaxJsonLength = int.MaxValue;
                                                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                                                if (retdata !=null)
                                                {
                                                    
                                                    sql = string.Format("update i_m_ry set issync=1 where rybh='{0}'", rybh);
                                                    CommonService.Execsql(sql);
                                                    //string code = retdata["code"].GetSafeString();
                                                    // 推送成功之后更新状态
                                                    //if (code == "0")
                                                    //{
                                                    //    sql = string.Format("update i_m_ry set issync=1 where rybh='{0}'", rybh);
                                                    //    CommonService.Execsql(sql);
                                                    //}
                                                    //else
                                                    //{
                                                    //    SysLog4.WriteError("推送用户失败：" + retdata["msg"].GetSafeString());
                                                    //}
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SysLog4.WriteError("无法获取pushuserurl");
                                        }
                                        
                                    }
                                    else
                                    {
                                        SysLog4.WriteError(string.Format("同步人员失败：姓名[{0}], 无法获取yhzh", ryxm));
                                    }

                                }
                                else
                                {
                                    SysLog4.WriteError(string.Format("同步人员失败：姓名[{0}], 无法获取yhzh", ryxm));
                                }
                            }
                            else
                            {
                                SysLog4.WriteError(string.Format("同步人员失败：姓名[{0}], rybh或者zh为空", ryxm));
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

        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }


    }
}