using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using System.Threading;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.Func
{
    public class Dust
    {
        #region 服务
        private static ICommonService _commonService = null;
        private static ICommonService CommonService
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

        #region 扬尘相关操作
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string DustGetToken()
        {
            string token = "";
            try
            {
                // 是否需要重新获取token
                bool needtogettoken = true;

                // JSON 序列化和反序列化类
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;

                // 获取token配置信息
                string sql = "select * from h_tokenconfig where sfyx=1 and lx='DUSTY'";
                IList<IDictionary<string, object>> configs = CommonService.GetDataTable2(sql);
                if (configs.Count > 0)
                {
                    // 获取已经保存的token配置信息
                    string tokenjson = configs[0]["tokenjson"].GetSafeString();
                    //SysLog4.WriteError("tokenjson:" + tokenjson);
                    // 从未保存过token信息，需要重新获取
                    if (tokenjson == "")
                    {
                        needtogettoken = true;
                    }
                    // 以前保存过token信息的
                    else
                    {
                        Dictionary<string, object> tokeninfo = jss.Deserialize<Dictionary<string, object>>(tokenjson);
                        if (tokeninfo != null)
                        {
                            double unixtokentime = tokeninfo["tokentime"].GetSafeDouble();
                            string savedtoken = tokeninfo["token"].GetSafeString();
                            int expires = tokeninfo["expires"].GetSafeInt();

                            DateTime tokentime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddSeconds(unixtokentime);
                            // token 未过期, 直接拿来用
                            if (tokentime.AddSeconds(expires) > DateTime.Now)
                            {
                                token = savedtoken;
                                needtogettoken = false;
                            }
                            else // token 过期，需要重新获取                        
                            {
                                needtogettoken = true;
                            }
                        }
                    }

                    if (needtogettoken)
                    {
                        string url = configs[0]["url"].GetSafeString();
                        string username = configs[0]["username"].GetSafeString();
                        string pwd = configs[0]["pwd"].GetSafeString();
                        string secret = configs[0]["appsecret"].GetSafeString();
                        string postdata = string.Format(
                            "UserName={0}&PassWord={1}&AppSecret={2}",
                            HttpUtility.UrlEncode(username),
                            HttpUtility.UrlEncode(MD5Util.StringToMD5Hash(pwd)),
                            HttpUtility.UrlEncode(secret)
                        );
                        string retstring = MyHttp.SendDataByPost(url, postdata);
                        
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                        if (retdata != null)
                        {
                            int code = retdata["Code"].GetSafeInt();
                            // 已经获取到token
                            if (code == 0 || code == 40003)
                            {
                                Dictionary<string, object> tokeninfo = (Dictionary<string, object>)retdata["Datas"];
                                if (tokeninfo != null)
                                {
                                    double tokentime = tokeninfo["TokenTime"].GetSafeDouble();
                                    int expires = tokeninfo["Expires"].GetSafeInt();
                                    string dustytoken = tokeninfo["Token"].GetSafeString();
                                    // 获取到新的token之后，存起来
                                    if (dustytoken != "")
                                    {
                                        token = dustytoken;
                                        tokenjson = jss.Serialize(new Dictionary<string, object>() {
                                            { "tokentime", tokentime},
                                            { "expires", expires},
                                            { "token", dustytoken}
                                        });

                                        sql = string.Format("update h_tokenconfig set tokenjson='{0}' where sfyx=1 and lx='DUSTY'", tokenjson);
                                        CommonService.Execsql(sql);
                                    }
                                }
                            }


                        }
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return token;
        }

        public static long getUnixTimestamp(DateTime dt)
        {
            long ret = 0;
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            ret = (dt.ToUniversalTime().Ticks - dt1970.Ticks) / 10000000;
            return ret;
        }

        /// <summary>
        /// 根据unix时间戳获取时间
        /// </summary>
        /// <param name="unixstamp"></param>
        /// <returns></returns>
        public static DateTime getDatetimeFromUnix(double unixstamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(unixstamp);
        }

        #endregion
    }
}