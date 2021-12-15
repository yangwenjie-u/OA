using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using System.Reflection;


namespace BD.Jcbg.Common
{
	/// <summary>
	/// 基础转换
	/// </summary>
	public static class Extensions
	{
		#region base64转换
		/// <summary>
		/// base64编码
		/// </summary>
		/// <param name="source"></param>
		/// <param name="encode"></param>
		/// <returns></returns>
		public static string EncodeBase64(this string source, Encoding encode)
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
		/// <summary>
		/// base64编码
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string EncodeBase64(this string source)
		{
			return source.EncodeBase64(Encoding.UTF8);
		}
		/// <summary>
		/// base64编码
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string EncodeBase64(this byte[] source)
		{
			string ret = "";
			try
			{
				ret = Convert.ToBase64String(source);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// base64解码
		/// </summary>
		/// <param name="source"></param>
		/// <param name="encode"></param>
		/// <returns></returns>
		public static string DecodeBase64(this string source, Encoding encode)
		{
			string decode = "";
			byte[] bytes = Convert.FromBase64String(source);
			try
			{
				decode = encode.GetString(bytes);
			}
			catch
			{
				decode = source;
			}
			return decode;
		}
		/// <summary>
		/// base64解码
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string DecodeBase64(this string source)
		{
			return source.DecodeBase64(Encoding.UTF8);
		}
		/// <summary>
		/// base64解码
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] DecodeBase64Array(this string source)
		{
			byte[] ret = null;
			try
			{
				ret = Convert.FromBase64String(source);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		#endregion

		#region Des加解密
		private const string KEY_64 = "tanked98";
		private const string IV_64 = "1381kack";
        

        private static byte[] IV_64_JK = { 0, 0, 0, 0, 0, 0, 0, 0 };
		/// <summary>
		/// Des解密
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
        public static string DecodeDes(this string data, string key = KEY_64, string iv = IV_64)
		{
			byte[] buffer3;
			if (string.IsNullOrEmpty(data))
			{
				return data;
			}
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            byte[] rgbIV = Encoding.ASCII.GetBytes(iv);
			try
			{
				buffer3 = Convert.FromBase64String(data);
			}
			catch
			{
				return null;
			}
			DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
			MemoryStream stream = new MemoryStream(buffer3);
			CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read);
			StreamReader reader = new StreamReader(stream2);
			return reader.ReadToEnd();
		}
		/// <summary>
		/// Des加密
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string EncodeDes(this string data, string key=KEY_64, string iv=IV_64)
		{
			if (string.IsNullOrEmpty(data))
			{
				return data;
			}
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            byte[] rgbIV = Encoding.ASCII.GetBytes(iv);
			DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
			int keySize = provider.KeySize;
			MemoryStream stream = new MemoryStream();
			CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
			StreamWriter writer = new StreamWriter(stream2);
			writer.Write(data);
			writer.Flush();
			stream2.FlushFinalBlock();
			writer.Flush();
			return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
		}

        /// <summary>
        /// webservice中的des解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DecodeDesJk(this string data, string key = KEY_64)
        {
            string ret = "";
            if (data == "")
                return ret;
            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();


                des.Key = ASCIIEncoding.UTF8.GetBytes(key);
                des.IV = IV_64_JK;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// webservice中的des加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncodeDesJk(this string data, string key = KEY_64)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            string ret = "";
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);


                des.Key = ASCIIEncoding.UTF8.GetBytes(key);
                des.IV = IV_64_JK;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                ret = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
		#endregion

		#region 数据类型转换
		public static int GetSafeInt(this object obj, int def = 0)
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
		public static uint GetSafeUint(this object obj, uint def = 0)
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
		public static decimal GetSafeDecimal(this object obj, decimal def = 0)
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
		public static double GetSafeDouble(this object obj, double def = 0)
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

		public static long GetSafeLong(this object obj, long def = 0)
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

		public static DateTime GetSafeDate(this object obj, DateTime def)
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

		public static DateTime GetSafeDate(this object obj)
		{
			return obj.GetSafeDate(new DateTime(1900, 1, 1));
		}

		public static bool GetSafeBool(this object obj, bool def = false)
		{
			bool ret = def;
			try
			{
				if (obj.GetSafeString() == "1")
					ret = true;
				else if (obj.GetSafeString() == "0")
					ret = false;
				else if (obj != null)
					ret = Convert.ToBoolean(obj);
			}
			catch { }
			return ret;
		}

		public static string GetSafeString(this object obj, string def = "")
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
        /// <summary>
        /// 字符串根据分隔符转换成list，并移除重复
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splits"></param>
        /// <param name="removeRepeat"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        public static IList<string> StringToList(this string str, char[] splits, bool removeRepeat = true, IList<string> exists = null)
        {
            IList<string> ret = new List<string>();
            try
            {
                
                if (exists != null)
                {
                    for (int j = 0; j < exists.Count; j++)
                    {
                        var q = from e in ret where e.Equals(exists[j], StringComparison.OrdinalIgnoreCase) select e;
                        if (q.Count() == 0 || !removeRepeat)
                        {
                            ret.Add(exists[j]);
                        }
                    }
                }
                string[] arrItems = str.Split(splits);
                for (int j = 0; j < arrItems.Length; j++)
                {
                    var q = from e in ret where e.Equals(arrItems[j], StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() == 0 || !removeRepeat)
                    {
                        ret.Add(arrItems[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 一个list是否包含另一个list
        /// </summary>
        /// <param name="bigList"></param>
        /// <param name="smallList"></param>
        /// <returns></returns>
        public static bool ListContains(this IList<string> bigList, IList<string> smallList)
        {
            bool ret = true;
            try
            {
                foreach (string str in smallList)
                {
                    var q = from e in bigList where e.Equals(str) select e;
                    if (q.Count() == 0)
                    {
                        ret = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret = false;
            }
            return ret;
        }

        public static DateTime GetTimeFormUtcMs(this long total)
        {
            int ms = (int)(total % 1000);
            total = total / 1000;
            int s = (int)(total % 60);
            total = total / 60;
            int m = (int)(total % 60);
            total = total / 60;
            int h = (int)(total % 24);
            total = total / 24;
            int d = (int)total;
            TimeSpan sp = new TimeSpan(d, h, m, s, ms);
            return new DateTime(1970, 1, 1).Add(sp).AddHours(8);
        }
		#endregion

		#region 数据库类型转换
		public static string FormatSQLInStr(this string str)
		{
			str = str.Trim(new char[] { ',' });
			str = str.Replace(",", "','");
			str = "'" + str + "'";
			return str;
		}

        public static string FormatSQLInt(this string str)
        {
            str = str.Trim(new char[] { ',' });
            return str;
        }
		#endregion

		#region 类序列化
		/// <summary>
		/// 类序列化成xml字符串
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string Serialize(this object obj)
		{
			if (obj == null)
				return "";
			return new JavaScriptSerializer().Serialize(obj);
		}
		#endregion

		#region 反射
		/// <summary>
		/// 获取属性值
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="fieldname"></param>
		/// <returns></returns>
		public static string MyGetObjectProperty(this object obj, string fieldname)
		{
			Type type = obj.GetType();

			object ovalue = type.GetProperty(fieldname).GetValue(obj, null);
			string svalue = ovalue.GetSafeString();
			if (string.IsNullOrEmpty(svalue)) return null;
			return svalue;
		}
		#endregion

        #region 比较相等
        /// <summary>
        /// 比较数据是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool IsSameArray(this object o1, object o2)
        {
            byte[] obj1 = o1 as byte[];
            byte[] obj2 = o2 as byte[];
            try
            {
                if (obj1 == null && obj2 == null)
                    return true;
                if (obj1 == null || obj2 == null)
                    return false;
                if (obj1.Length != obj2.Length)
                    return false;
                for (int i = 0; i < obj1.Length; i++)
                    if (obj1[i] != obj2[i])
                        return false;
            }
            catch { }
            return true;
        }
        #endregion

        #region 请求参数安全化
        public static string GetSafeRequest(this string obj)
        {
            return obj.GetSafeString().Replace("'", "");
        }
        #endregion

        #region 正则表达式验证
        public static bool IsMobile(this string obj)
        {
            if (obj == null)
                return false;
            if (obj.Length < 11)
                return false;
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(obj);

        }
        #endregion

		#region 单元格
        /// <summary>
        /// 获取单元格值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <returns></returns>
        public static string GetCellValue(this ISheet sheet, int rowNum, int colNum)
        {
            return sheet.GetRow(rowNum-1).GetCell(colNum - 1).ToString();
        }
        #endregion
        #region json处理
        public static String ToJson(this String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                        /*
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;*/
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                        /*
                    case '\t':
                        sb.Append("\\t");
                        break;*/
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
        #endregion
        
        #region 数据库写入格式化
        public static string GetSafeDbValue(this string obj)
        {
            return obj.GetSafeString().Replace("'", "''");
        }
        #endregion

        #region 动态创建对象
        public static object CreateObject(this string typeName, string dllpath)
        {
            object obj = null;
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllpath);
                Type objType = assembly.GetType(typeName);
                obj = Activator.CreateInstance(objType);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return obj;
        }
        #endregion

        #region url编码
        public static string UrlEncode(this string txt, Encoding e = null)
        {
            if (e == null)
            {
                e = Encoding.UTF8;
            }
            return System.Web.HttpUtility.UrlEncode(txt, e);
        }

        #endregion

        #region 支付平台无效数据替换
        public static string GetValidPayString(this string obj)
        {
            if (obj == null)
                return "";
            return obj.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }
        #endregion


        #region 
        /// <summary>
        /// （1）是不允许出现空格。2）把所有标点符号由英文换成中文全角。3）英文字母小写全部换成大写。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetFormateString(this string obj)
        {
            if(obj!="")
            {
                obj = obj.Replace(" ", "");
                obj = ToSBC(obj);
                obj = obj.ToUpper();
            }       
            return obj;
        }

        public static  bool IsNatural_Number(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }
        /// 转全角的函数(SBC case)
        ///
        ///任意字符串
        ///全角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public static String ToSBC(String input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (IsNatural_Number(c[i].ToString()))
                {
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        /**/
        // /
        // / 转半角的函数(DBC case)
        // /
        // /任意字符串
        // /半角字符串
        // /
        // /全角空格为12288，半角空格为32
        // /其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        // /
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
        #endregion

    }
}
