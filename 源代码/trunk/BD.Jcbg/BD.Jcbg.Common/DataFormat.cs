using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BD.Jcbg.Common
{
    public static class DataFormat
    {
        public static short GetSafeShort(object obj, short def = 0)
        {
            short ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToInt16(obj);
            }
            catch { }
            return ret;
        }
        public static int GetSafeInt(object obj, int def = 0)
        {
            int ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToInt32(obj);
            }
            catch { }
            return ret;
        }
        public static uint GetSafeUint(object obj, uint def = 0)
        {
            uint ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToUInt32(obj);
            }
            catch { }
            return ret;
        }
        public static decimal GetSafeDecimal(object obj, decimal def = 0)
        {
            decimal ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToDecimal(obj);
            }
            catch { }
            return ret;
        }
        public static double GetSafeDouble(object obj, double def = 0)
        {
            double ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToDouble(obj);
            }
            catch { }
            return ret;
        }

        public static long GetSafeLong(object obj, long def = 0)
        {
            long ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToInt64(obj);
            }
            catch { }
            return ret;
        }

        public static DateTime GetSafeDate(object obj, DateTime def)
        {
            DateTime ret = def;
            try
            {
                string str = obj.ToString();
                str = str.Replace("年", "-");
                str = str.Replace("月", "-");
                ret = Convert.ToDateTime(obj);
            }
            catch { }
            return ret;
        }

        public static DateTime GetSafeDate(object obj)
        {
            return GetSafeDate(obj, new DateTime(1900, 1, 1));
        }

        public static bool GetSafeBool(object obj, bool def = false)
        {
            bool ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToBoolean(obj);
            }
            catch { }
            return ret;
        }

        public static string GetSafeString(object obj, string def = "")
        {
            string ret = def;
            try
            {
                if (obj != null)
                    ret = Convert.ToString(obj);
            }
            catch { }
            return ret;
        }

        public static string FormatSQLInStr(string str)
        {
            str = str.Trim(new char[] { ',' });
            str = str.Replace(",", "','");
            str = "'" + str + "'";
            return str;
        }
        public static string FormatSQLInStr(IList<string> strs)
        {
            string ret = "";
            try
            {
                foreach (string str in strs)
                {
                    ret += str + ",";
                }
                ret = FormatSQLInStr(ret);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 从str1中查找str2是否在字符串中，split 为分隔符
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static bool IsStrIn(string str1, string str2, string split)
        {
            return (split + str1 + split).IndexOf(split + str2 + split) > -1;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            string code = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                code = Convert.ToBase64String(bytes);
            }
            catch
            {
                code = source;
            }
            return code;
        }

        public static string EncodeBase64(byte[] source)
        {
            string code = "";
            try
            {
                code = Convert.ToBase64String(source);
            }
            catch
            {
                code = "";
            }
            return code;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            if (result == "")
                return decode;
            result = result.Replace(' ', '+');
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetSubString(string str, int len)
        {
            string strOrg = str;
            if (str.Length < len)
                return str;
            if (len <= 0)
                return str;
            int curByteLen = 0;
            int curIndex = 0;
            int retByteLen = len * 2;

            while (curByteLen < retByteLen && curIndex < str.Length)
            {
                if ((int)str[curIndex] > 128)
                    curByteLen++;
                curIndex++;
                curByteLen++;
            }
            if (curByteLen > retByteLen)
                curIndex--;
            str = str.Substring(0, curIndex);
            if (str != strOrg)
                str += "..";
            return str;
        }

        /// <summary>
        /// url跟returnurl封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="returnurl"></param>
        /// <returns></returns>
        public static string FormatReturnUrl(string url, string returnurl)
        {
            if (!url.StartsWith("/"))
                url = "/" + url;
            if (url.IndexOf("?") == -1)
                url += "?";
            else
                url += "&";
            return url + "ReturnUrl=" + System.Web.HttpUtility.UrlEncode(returnurl);
        }

        public static string ArrToString(IList<string> arrs)
        {
            string ret = "";
            if (arrs == null)
                return ret;
            foreach (string str in arrs)
            {
                if (ret != "")
                    ret += ",";
                ret += str;
            }
            return ret;
        }
        /// <summary>
        /// 数据库类型转换成dbtype
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DbType GetDbType(string str)
        {
            DbType ret = DbType.Object;
            str = str.ToLower();
            if (str == "image" || str == "binary" || str == "varbinary")
                ret = DbType.Binary;
            else if (str == "text" || str == "ntext" || str == "varchar" || str == "char" || str == "nvarchar" || str == "nchar")
                ret = DbType.String;
            else if (str == "tinyint")
                ret = DbType.Byte;
            else if (str == "smallint")
                ret = DbType.Int16;
            else if (str == "int")
                ret = DbType.Int32;
            else if (str == "bigint")
                ret = DbType.Int64;
            else if (str == "smalldatetime" || str == "datetime" || str == "timestamp")
                ret = DbType.DateTime;
            else if (str == "real" || str == "numeric")
                ret = DbType.Double;
            else if (str == "money" || str == "smallmoney")
                ret = DbType.Currency;
            else if (str == "float")
                ret = DbType.Single;
            else if (str == "bit")
                ret = DbType.Boolean;
            else if (str == "decimal")
                ret = DbType.Decimal;
            return ret;
        }

        public static byte[] StringToByte(string str)
        {
            byte[] by = new byte[1];
            try
            {
                if (str.Length > 0)
                {
                    by = Encoding.UTF8.GetBytes(str);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return by;
        }

        public static string ByteToString(byte[] by)
        {
            string str = "";
            try
            {
                if (by != null)
                {
                    str = Encoding.UTF8.GetString(by);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return str;
        }

        public static string GB2312ToUTF8(string str)
        {
            string ret = "";
            try
            {
                ret = Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        public static string GetCnWeekday(DayOfWeek day)
        {
            return new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" }[(int)day];

        }
    }
}
