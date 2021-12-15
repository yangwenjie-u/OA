using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;

namespace BD.Jcbg.Web.Func
{
    public class ZJWGRY
    {

        private static CookieContainer cookie = new CookieContainer();
        private static string UserName = "zf0011";
        private static string UserPwd = "wgry6688";
        //公共属性#region 公共属性 
        public static string userpwd
        {
            set { UserPwd = value; }
        }
        public static string username
        {
            set { UserName = value; }
        }


        public static string GetWgryTJ(string url, string date)
        {
            if (Login())
            {
                Uri d = new Uri(url);
                var p = string.Format(date);
                var res = Post(d, p, cookie, "");
                return res.ToString();
            }
            else
            {
                return "";
            }
        }


        private static bool Login()
        {
            var url = "http://120.27.218.55:8001/LoginJump/Index?clientkey={0}&ul=";
            url = string.Format(url, GetToken(UserName, "", UserPwd));
            Uri d = new Uri(url);
            var res = GET(d, cookie, "");
            d = new Uri("http://120.27.218.55:8001/user/getmenusv2");
            res = GETLocation(d, cookie, "");
            if (string.IsNullOrEmpty(res))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <param name="d">目标地址</param>
        /// <param name="postData">POST数据</param>
        /// <param name="cookie">Cookie容器</param>
        /// <param name="refer">来源</param>
        /// <param name="isUtf8">返回的编码是否是UTF8</param>
        /// <returns></returns>
        private static string Post(Uri d, string postData, CookieContainer cookie, string refer)
        {
            try
            {
                HttpWebRequest req1 = (HttpWebRequest)HttpWebRequest.Create(d);
                byte[] bs = null;

                bs = Encoding.UTF8.GetBytes(postData);
                req1.Accept = "application/json, text/javascript,text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req1.ContentLength = bs.Length;
                req1.ContentType = "application/x-www-form-urlencoded";
                req1.CookieContainer = cookie;
                req1.Headers.Add("Cache-control", "no-cache");
                req1.KeepAlive = true;
                req1.ServicePoint.Expect100Continue = false;
                req1.Method = "POST";
                req1.Referer = refer;
                req1.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36 BDBrowser";
                using (Stream rs = req1.GetRequestStream())
                {
                    rs.Write(bs, 0, bs.Length);
                }
                using (WebResponse wr = req1.GetResponse())
                {
                    StreamReader sr;
                    sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// GET提交数据
        /// </summary>
        /// <param name="d">目标地址</param>
        /// <param name="cookie">Cookie容器</param>
        /// <param name="refer">来源</param>
        /// <param name="isGb2312">是否是GB2312</param>
        /// <returns></returns>
        private static string GET(Uri d, CookieContainer cookie, string refer)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(d);
                req.Accept = "application/json,text/javascript,text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.CookieContainer = cookie;
                req.KeepAlive = true;
                req.Headers.Add("Cache-control", "no-cache");
                req.Method = "GET";
                req.Referer = refer;
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36 BDBrowser";
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader sr;
                    sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// GET提交数据
        /// </summary>
        /// <param name="d">目标地址</param>
        /// <param name="cookie">Cookie容器</param>
        /// <param name="refer">来源</param>
        /// <param name="isGb2312">是否是GB2312</param>
        /// <returns></returns>
        public static string GETLocation(Uri d, CookieContainer cookie, string refer)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(d);
                req.Accept = "application/json,text/javascript,text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.CookieContainer = cookie;
                req.KeepAlive = true;
                req.Headers.Add("Cache-control", "no-cache");
                req.Method = "GET";
                req.Referer = refer;
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36 BDBrowser";
                using (WebResponse wr = req.GetResponse())
                {
                    if (wr.Headers["location"] == null)
                    {
                        return "";
                    }
                    else
                    {
                        return wr.Headers["location"];
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private static string GetToken(string UserName, string ID, string UserPwd)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(UserName.ToLower())) + "|" + time + "|" + GetMd5Hash(UserName, time) + "|" + ID + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(UserPwd));
            token = EnCrypt(token);
            return token;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string StringToMD5Hash(string inputString, bool isUtf8 = true)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = null;
            if (isUtf8)
            {
                encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
            else
            {
                encryptedBytes = md5.ComputeHash(Encoding.Default.GetBytes(inputString));
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string GetMd5Hash(string UserName, string time)
        {
            return StringToMD5Hash(UserName + "&" + time + "&" + "~!@#$%^&*()_+?");
        }

        private static string EnCrypt(string str)
        {
            RSACryptoServiceProvider rsaencrype = CreateRSAEncryptProvider("", "");

            String text = str;

            int size = rsaencrype.KeySize;

            byte[] data = new UTF8Encoding().GetBytes(text);

            if (data.Length <= size / 8 - 11)
            {
                byte[] endata = rsaencrype.Encrypt(data, false);

                return ToHexString(endata);
            }
            else
            {
                var result = "";
                var maxsize = size / 8 - 11;
                for (int i = 0; i < (data.Length + maxsize - 1) / maxsize; i++)
                {
                    if (data.Length >= (i * maxsize + maxsize))
                    {
                        byte[] myByte = new byte[maxsize];
                        for (int j = 0; j < maxsize; j++)
                        {
                            myByte[j] = data[i * maxsize + j];
                        }
                        result += ToHexString(rsaencrype.Encrypt(myByte, false));
                    }
                    else
                    {
                        byte[] myByte = new byte[data.Length - i * maxsize];
                        for (int j = 0; j < data.Length - i * maxsize; j++)
                        {
                            myByte[j] = data[i * maxsize + j];
                        }
                        result += ToHexString(rsaencrype.Encrypt(myByte, false));
                    }
                }
                return result;
            }
        }

        private static RSACryptoServiceProvider CreateRSAEncryptProvider(string Modulus, string Exponent)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();
            if (String.IsNullOrEmpty(Modulus))
            {
                parameters1.Modulus = hexToBytes("C58B3C310D39E68CD95E93DA07570D9F06E4E4670BEF59D4005C385F407D8D62892EA7A8C73CC7B4C7E88C1F25365247CB9E0C5F1469A035ADF546AB7874440DD42C64F88207863283A9D0C670D4E0D20621858427BC3567BE422E99D4417F27F629BFCC5256F644C46B9B0BEB471D25BC8F38988F325D9B420194F0C15FF0ED");
            }
            else
            {
                parameters1.Modulus = hexToBytes(Modulus);
            }
            if (String.IsNullOrEmpty(Exponent))
            {
                parameters1.Exponent = hexToBytes("010001");
            }
            else
            {
                parameters1.Exponent = hexToBytes(Exponent);
            }
            CspParameters parameters2 = new CspParameters();
            parameters2.Flags = CspProviderFlags.UseDefaultKeyContainer;
            RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider();
            provider1.ImportParameters(parameters1);
            return provider1;
        }

        private static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        private static byte[] hexToBytes(String src)
        {
            int l = src.Length / 2;
            String str;
            byte[] ret = new byte[l];

            for (int i = 0; i < l; i++)
            {
                str = src.Substring(i * 2, 2);
                ret[i] = Convert.ToByte(str, 16);
            }
            return ret;
        }

   
    }
}