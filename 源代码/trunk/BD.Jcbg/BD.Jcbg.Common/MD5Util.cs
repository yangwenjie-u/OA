using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BD.Jcbg.Common
{
    public class MD5Util
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string StringToMD5Hash(string inputString, bool isUtf8 = true)
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

        public static string GetCommonMD5(string inputString, bool isUtf8 = true)
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
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }
    }
}