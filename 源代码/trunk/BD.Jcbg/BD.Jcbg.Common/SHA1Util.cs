using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BD.Jcbg.Common
{
    public class SHA1Util
    {
        public static string StringToSHA1Hash(string inputString, bool isUtf8 = true)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] encryptedBytes = null;
            if (isUtf8)
            {
                encryptedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
            else
            {
                encryptedBytes = sha1.ComputeHash(Encoding.Default.GetBytes(inputString));
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        public static string GetCommonSHA1(string inputString, bool isUtf8 = true)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] encryptedBytes = null;
            if (isUtf8)
            {
                encryptedBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
            else
            {
                encryptedBytes = sha1.ComputeHash(Encoding.Default.GetBytes(inputString));
            }
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }
    }
}
