using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Func
{
    public class SSOCheckHelper
    {

        /// <summary>
        ///  HmacSHA256 加密
        /// </summary>
        /// <param name="secret">projectSecret</param>
        /// <param name="data">请求的JSON参数</param>
        /// <returns></returns>
        public static string GetSignature(string data, string secret)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte test in hashmessage)
                {
                    sb.Append(test.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 验证令牌并获取用户的登录信息
        /// </summary>
        /// <param name="ssotoken"></param>
        public static string doQuery(string ssotoken)
        {

            string PROJECT_SECRET = Configs.GetConfigItem("sscprojectsecret").GetSafeString();
            string PROJECT_ID = Configs.GetConfigItem("sscprojectid").GetSafeString();
            string QUERY_URL = Configs.GetConfigItem("sscqueryurl").GetSafeString();
            // 请求参数
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject.Add("token", ssotoken);
            // 获取请求参数的JSON字符串
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string parm = jss.Serialize(jsonObject);
            string signature =GetSignature(parm, PROJECT_SECRET);
            Dictionary<string, string> postHeaders = new Dictionary<string, string>();
            // 项目ID
            postHeaders.Add("x-esso-project-id", PROJECT_ID);
            // 请求参数值
            postHeaders.Add("x-esso-signature", signature);
            postHeaders.Add("Charset", "UTF-8");

            // 以POST方式请求
            string result = MyHttp.SendPOST(QUERY_URL, parm, postHeaders);
            return result;
        }
    }
}