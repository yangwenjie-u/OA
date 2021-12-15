using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BD.JC.JS.Common
{
    public static class Extensions
    {
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
                ret = false;
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
        public static string EncodeDes(this string data, string key = KEY_64, string iv = IV_64)
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
            }
            return ret;
        }
        #endregion
    }
}
