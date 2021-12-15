using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BD.Jcbg.Common
{
    public class EncryUtil  //列表界面加密类
    {
        // Fields
        private const string KEY_64 = "8zzsjd95";
        private const string IV_64 = "fcb95eze";

        // Methods
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decode(string data)
        {
            byte[] buffer3;
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(KEY_64);
            byte[] rgbIV = Encoding.ASCII.GetBytes(IV_64);
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
        /// 编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encode(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(KEY_64);
            byte[] rgbIV = Encoding.ASCII.GetBytes(IV_64);
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
    }
}
