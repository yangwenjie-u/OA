using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BD.Jcbg.Common
{
    public class CryptFun
    {
        // Fields
        private const string KEY_64 = "8e5sjd86";
        private const string IV_64 = "fib85ede";

        // Methods
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
            catch(Exception)
            {
                return null;
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream(buffer3);
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }
        

        public static string Encode(string data, string key="", string iv="")
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            if (string.IsNullOrEmpty(key))
                key = KEY_64;
            if (string.IsNullOrEmpty(iv))
                iv = IV_64;
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

        // 录入界面的加密操作
        // Fields
        private const string LR_KEY_64 = "8zzsjd95";
        private const string LR_IV_64 = "fcb95eze";

        // Methods
        public static string LrDecode(string data)
        {
            byte[] buffer3;
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(LR_KEY_64);
            byte[] rgbIV = Encoding.ASCII.GetBytes(LR_IV_64);
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

        public static string LrEncode(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(LR_KEY_64);
            byte[] rgbIV = Encoding.ASCII.GetBytes(LR_IV_64);
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
