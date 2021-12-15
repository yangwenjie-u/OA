﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.Common;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Bll;

namespace BD.Jcbg.Web.zm
{
    public class tools
    {

        private const int SEC_ = 2;

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

        /// <summary>
        /// 校验Webservice是否合法
        /// 返回false符合要求，true不符合要求
        /// </summary>
        /// <param name="timemillis"></param>
        /// <param name="MD5Data"></param>
        /// <returns></returns>
        public static bool CheckWebservice(string timemillis, string MD5Data)
        {
            bool ret = false;

            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);

            long startTime = timemillis.GetSafeLong();
            long endTime = Convert.ToInt64((DateTime.Now - DateStart).TotalMilliseconds);

            string newSecurity = MD5Util.StringToMD5Hash(timemillis + "bdsoft");
            if (!newSecurity.Equals(MD5Data) || (endTime - startTime) < 0 || (endTime - startTime) > SEC_ * 60 * 1000)
                ret = true;
            return ret;
        }


        /// <summary>  
        /// 判断输入的字符串是否是一个合法的手机号  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(input);

        }


        /// <summary>  
        /// 判断输入的字符串是否数字和字母组成 
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsZmAndNum(string input)
        {
            Regex regex = new Regex("^[A-Za-z0-9]+$");
            return regex.IsMatch(input);

        }

        /// <summary>
        /// 返回数据进行封装成Dictionary<string, string>
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static IDictionary<string, string> AnalyticParam(string Str)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            string[] StrTemp = Str.Split('&');
            for (int i = 0; i < StrTemp.Length; i++)
            {
                string[] StrTemp2 = StrTemp[i].Split('=');
                if (StrTemp2.Length == 2)
                    ret.Add(StrTemp2[0].GetSafeString().ToLower(), ("==" + StrTemp[i].GetSafeString()).Replace("==" + StrTemp2[0].GetSafeString() + "=", ""));
            }

            return ret;
        }


        /// <summary>
        /// 获取网络数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string getPost(string url, string data)
        {
            byte[] byteArray = Encoding.GetEncoding("GBK").GetBytes(data);// 要发放的数据 

            HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(url);
            objWebRequest.Method = "POST";
            objWebRequest.ContentType = "application/x-www-form-urlencoded";
            objWebRequest.ContentLength = byteArray.Length;
            Stream newStream = objWebRequest.GetRequestStream();
            // Send the data. 
            newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string ret = sr.ReadToEnd(); // 返回的数据

            return ret;
        }


        /// <summary>
        /// 用户code+固定字符串方式进行MD5加密
        /// 返回false符合要求，true不符合要求
        /// </summary>
        /// <param name="code"></param>
        /// <param name="MD5Data"></param>
        /// <returns></returns>
        public  bool CheckWebservice2(string code, string MD5Data)
        {
            try {
                string newSecurity = MD5Util.StringToMD5Hash(code + "bdsoft");
                if (code.Length < 5 || code.Equals("") || !IsZmAndNum(code))
                {
                    return true;
                }
                if (!MD5Data.Equals(newSecurity))
                {
                    return true;
                }
                string sql = "select * from checkwebcode where code = '" + code + "' and touse = 'True' ";
                IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

                ret = CommonService.GetDataTable(sql);
                if (ret.Count!=1)
                {
                    return true;
                }
            }catch(Exception ex){
                SysLog4.WriteError(ex.Message);
                return true;
            }

            return false;
        }


        public static string PostJsonData(string url, string data, string encoding = "UTF-8")
        {
            string ret = "";
            try
            {
                StringBuilder errStr = new StringBuilder();
                errStr.Append(" {");
                errStr.Append("     \"text\": \"政务云系统\",");
                errStr.Append("     \"attachments\": ");
                errStr.Append("     [");
                errStr.Append("         {");
                errStr.AppendFormat("       \"title\": \"{0}\",", "短信发送错误");
                errStr.AppendFormat("       \"text\": \"{0}\",", data);
                errStr.Append("             \"color\": \"#FF0033\"");
                errStr.Append("         }");
                errStr.Append("     ]");
                errStr.Append(" }");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                request.ContentType = "application/json";
                byte[] payload = Encoding.GetEncoding(encoding).GetBytes(errStr.ToString());
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.GetResponseStream();
                //Stream s = response.GetResponseStream();
                //string strValue = "";
                //if (s != null)
                //{
                //    StreamReader reader = new StreamReader(s, Encoding.GetEncoding(encoding));
                //    strValue = reader.ReadToEnd().Trim();
                //}
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }



    }
}