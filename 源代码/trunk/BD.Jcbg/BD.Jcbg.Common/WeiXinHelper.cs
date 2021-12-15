using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace BD.Jcbg.Common
{
    public class WeiXinHelper
    {
        /// <summary>
        /// tokenURL
        /// </summary>
        private static readonly string tokenUrl = "https://api.weixin.qq.com/cgi-bin/token";

        /// <summary>
        /// tickURL
        /// </summary>
        private static readonly string ticketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";

        /// <summary>
        /// 获取媒体图片
        /// </summary>
        public static readonly string mediaUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";

        /// <summary>
        /// 访问数据包
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpResponse(string url)
        {
            string retString = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";// "textml;charset=UTF-8";
            request.UserAgent = null;
            // request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                StreamReader myStreamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                stream.Close();
            }
            return retString;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public static string getAccessToken()
        {
            string token = "";
            string url = String.Format("{0}?grant_type=client_credential&appid={1}&secret={2}", tokenUrl, Configs.WxAppid,Configs.WxSecret);
          
            if (HttpRuntime.Cache.Get("AccessToken") == null)
            {
                string data = GetHttpResponse(url);
                //获取数据包
                var result = JObject.Parse(data);
                if (result["access_token"] != null && result["access_token"].Value<string>() != string.Empty)
                {
                    token = result["access_token"].Value<string>();
                    HttpRuntime.Cache.Insert("AccessToken", token, null, DateTime.Now.AddSeconds(3600), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            else
            {
                token = HttpRuntime.Cache.Get("AccessToken").ToString();  
            }
            return token;
        }

        /// <summary>
        /// 获取ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetTicket(string accessToken)
        {
            string ticket = "";
            string url = String.Format("{0}?type=jsapi&access_token={1}", ticketUrl, accessToken);

            if (HttpRuntime.Cache.Get("Ticket") == null)
            {
                string data = GetHttpResponse(url);
                //获取数据包
                var result = JObject.Parse(data);
                if (result["ticket"] != null && result["ticket"].Value<string>() != string.Empty)
                {
                    ticket = result["ticket"].Value<string>();
                    HttpRuntime.Cache.Insert("Ticket", ticket, null, DateTime.Now.AddSeconds(3600), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            else
            {
                ticket = HttpRuntime.Cache.Get("Ticket").ToString();
            }
            return ticket;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string noncestr, string url)
        {
            string string1 = "jsapi_ticket=" + GetTicket(getAccessToken()) + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            //使用sha1加密这个字符串
            return SHA1(string1);
        }
        #region 工具类
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            var rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToString();
            return str.ToLower();
        }

        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <returns></returns>
        public static string CreateTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            return timestamp;
        }
        #endregion
    }
}
