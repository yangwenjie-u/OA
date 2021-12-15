using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Func
{
    public  class PersonnelSocialSecurity
    {
        /// <summary>
        /// 缓存的秘钥信息
        /// </summary>
        private static Dictionary<string,string> tokenInfo= null;

        /// <summary>
        /// 获取个人社保信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="idcard"></param>
        /// <param name="areaAK"></param>
        /// <param name="msg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool GetPersonnelSocialSecurity(string name, string idcard, string areaAK, out string msg, out string result)
        {
            bool ret = true;
            result = "";
            msg = "";
            try
            {
                string secret = "";
                if(GetRequsetSecret(out msg, out secret))
                {
                    
                    long requesttime = GetTimeToken();

                    string sign = MD5Util.StringToMD5Hash(appKey + secret + requesttime.ToString()).ToLower();
                    //string url = "http://172.23.31.13:80/gateway/api/001008006007011/dataSharing/1defS0Vb1lLiD9de.htm";
                    string url = Configs.GetConfigItem("pssdataurl");
                    string postdata = "requestTime=" + requesttime.ToString() +
                                    "&sign=" + sign +
                                    "&appKey=" + appKey +
                                    "&name=" + name +
                                    "&idcard=" + idcard +
                                    "&areaAK=" + areaAK;
                    result = MyHttp.SendDataByPost(url, postdata);
                }
                else
                {
                    ret = false;
                    result = "获取AppKey失败";
                }
                
               
            }
            catch (Exception e)
            {
                ret = false;
                result = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据appKey, appSecret获取秘钥信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool RefreshTokenByKey(out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                long requesttime = GetTimeToken();
                string sign = MD5Util.StringToMD5Hash(appKey + appSecret + requesttime.ToString()).ToLower();
                string postdata = "requestTime=" + requesttime.ToString() +
                                "&sign=" + sign +
                                "&appKey=" + appKey;

                //string url = "http://172.23.31.13:80/gateway/app/refreshTokenByKey.htm";
                string url = Configs.GetConfigItem("pssrefreshtokenbykeyurl");
                string result = MyHttp.SendDataByPost(url, postdata);
                if (result != "")
                {
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                    if (retdata != null)
                    {
                        string code = retdata["code"].GetSafeString();
                        // 调用成功
                        if (code == "00")
                        {
                            Dictionary < string, object> dt= (Dictionary<string, object>)retdata["datas"];
                            if (dt != null)
                            {
                                if (tokenInfo == null)
                                {
                                    tokenInfo = new Dictionary<string, string>();
                                }
                                tokenInfo["refreshSecret"] =dt["refreshSecret"].GetSafeString();
                                tokenInfo["refreshSecretEndTime"] = dt["refreshSecretEndTime"].GetSafeString();
                                tokenInfo["requestSecret"] = dt["requestSecret"].GetSafeString();
                                tokenInfo["requestSecretEndTime"] = dt["requestSecretEndTime"].GetSafeString();
                            }
                            else
                            {
                                ret = false;
                                msg = "解析数据失败！";

                            }

                        }
                        // 调用失败
                        else
                        {
                            ret = false;
                            msg =retdata["msg"].GetSafeString();
                        }
                    }
                }
                else
                {
                    ret = false;
                    msg = "获取秘钥失败！";
                }


            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据appKey, refreshSecret获取秘钥信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool refreshTokenBySec(out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string refreshSecret = tokenInfo["refreshSecret"];
                long requesttime = GetTimeToken();
                string sign = MD5Util.StringToMD5Hash(appKey + refreshSecret + requesttime.ToString()).ToLower();
                string postdata = "requestTime=" + requesttime.ToString() +
                                "&sign=" + sign +
                                "&appKey=" + appKey;

                //string url = "http://172.23.31.13:80/gateway/app/refreshTokenBySec.htm";
                string url = Configs.GetConfigItem("pssrefreshtokenbysecurl");
                string result = MyHttp.SendDataByPost(url, postdata);
                if (result != "")
                {
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                    if (retdata != null)
                    {
                        string code = Convert.ToString(retdata["code"]);
                        // 调用成功
                        if (code == "00")
                        {
                            string info = Convert.ToString(retdata["datas"]);
                            if (info != "")
                            {
                                Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(info);
                                if (dt != null)
                                {
                                    tokenInfo["refreshSecret"] = dt["refreshSecret"].GetSafeString();
                                    tokenInfo["refreshSecretEndTime"] = dt["refreshSecretEndTime"].GetSafeString();
                                    tokenInfo["requestSecret"] = dt["requestSecret"].GetSafeString();
                                    tokenInfo["requestSecretEndTime"] = dt["requestSecretEndTime"].GetSafeString();
                                }
                            }
                            else
                            {
                                ret = false;
                                msg = "解析数据失败！";

                            }

                        }
                        // 调用失败
                        else
                        {
                            ret = false;
                            msg = Convert.ToString(retdata["msg"]);
                        }
                    }
                }
                else
                {
                    ret = false;
                    msg = "获取秘钥失败！";
                }


            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取请求秘钥
        /// </summary>
        /// <returns></returns>
        private static bool GetRequsetSecret(out string msg, out string Secret)
        {
            bool ret = true;
            msg = "";
            Secret = "";
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                if (RefreshTokenByKey(out msg))
                {
                    Secret = tokenInfo["requestSecret"];
                }
                else
                {
                    ret = false;
                }

                // 系统中已缓存秘钥信息
                //if (tokenInfo != null)
                //{
                //    string refreshSecret = tokenInfo["refreshSecret"];
                //    long refreshSecretEndTime = Convert.ToInt64(tokenInfo["refreshSecretEndTime"]);
                //    string requestSecret = tokenInfo["requestSecret"];
                //    long requestSecretEndTime = Convert.ToInt64(tokenInfo["requestSecretEndTime"]);
                //    long currentTime = GetTimeToken();
                //    // 请求秘钥已过期
                //    if (currentTime >= requestSecretEndTime)
                //    {
                //        // 刷新秘钥已过期,使用APP秘钥获取最新的秘钥信息
                //        if (currentTime >= refreshSecretEndTime)
                //        {
                //            if (RefreshTokenByKey(out msg))
                //            {
                //                Secret = tokenInfo["requestSecret"];
                //            }
                //            else
                //            {
                //                ret = false;
                //            }
                //        }
                //        // 刷新秘钥未过期，使用刷新秘钥去获取最新的秘钥信息
                //        else
                //        {
                //            if (refreshTokenBySec(out msg))
                //            {
                //                Secret = tokenInfo["requestSecret"];
                //            }
                //            else
                //            {
                //                ret = false;
                //            }
                //        }

                //    }
                //    // 请求秘钥未过期，直接使用
                //    else
                //    {
                //        Secret = requestSecret;
                //    }

                //}
                //// 未缓存秘钥信息，需要重新获取
                //else
                //{
                //    if (RefreshTokenByKey(out msg))
                //    {
                //        Secret = tokenInfo["requestSecret"];
                //    }
                //    else
                //    {
                //        ret = false;
                //    }
                //}

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取当前时间戳-13位数字
        /// </summary>
        /// <returns></returns>
        private static long GetTimeToken()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// appKey
        /// </summary>
        //private static string appKey = "0cfa7ece925c4c0a9d961e132e3b13fd";

        private static string appKey
        {
            get { return Configs.GetConfigItem("pssappkey"); }
        }

        /// <summary> 
        /// appSecret
        /// </summary>
        //private static string appSecret = "627e70ee38d743bc8e6533c19480d18b";


        private static string appSecret
        {
            get { return Configs.GetConfigItem("pssappsecret"); }
        }



    }
}
