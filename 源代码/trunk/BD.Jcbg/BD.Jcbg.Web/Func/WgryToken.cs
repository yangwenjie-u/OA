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
    public class WgryToken
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

        #region 获取务工人员平台token
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
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
                string sql = "select * from h_tokenconfig where sfyx=1 and lx='WGRY'";
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
                            string savedtoken = tokeninfo["token"].GetSafeString();
                            DateTime expireTime = tokeninfo["expireTime"].GetSafeDate();

                            // token 未过期, 直接拿来用
                            if (expireTime  > DateTime.Now)
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
                        string appID = configs[0]["appid"].GetSafeString();
                        string appSecret = configs[0]["appsecret"].GetSafeString();
                        string urldata = string.Format(
                            "appID={0}&appSecret={1}",
                            HttpUtility.UrlEncode(appID),
                            HttpUtility.UrlEncode(appSecret)
                        );
                        url = url + (url.IndexOf("?") > -1 ? "&" : "?" ) + urldata;
                        SysLog4.WriteError(url);
                        string retstring = MyHttp.SendDataByGET(url);

                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                        if (retdata != null)
                        {
                            int code = retdata["code"].GetSafeInt();
                            // 已经获取到token
                            if (code == 0 )
                            {
                                
                                string expireTime = retdata["expireTime"].GetSafeString();
                                string wgrytoken = retdata["data"].GetSafeString();
                                // 获取到新的token之后，存起来
                                if (wgrytoken != "")
                                {
                                    token = wgrytoken;
                                    tokenjson = jss.Serialize(new Dictionary<string, object>() {
                                            { "expireTime", expireTime},
                                            { "token", wgrytoken}
                                        });

                                    sql = string.Format("update h_tokenconfig set tokenjson='{0}' where sfyx=1 and lx='WGRY'", tokenjson);
                                    CommonService.Execsql(sql);
                                }
                            }
                            else
                            {
                                SysLog4.WriteError("获取务工人员平台token失败：\r\n" + retdata["msg"].GetSafeString() );
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

        #endregion
    }
}