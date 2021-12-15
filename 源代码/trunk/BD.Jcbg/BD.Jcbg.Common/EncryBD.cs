using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BD.Jcbg.Common
{
    public class EncryBD
    {
        private const string KEY_64 = "8zzsjd95";
        private const string IV_64 = "fcb95eze";

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //编号 1-》空格  0-》tab
        public static string Encode2(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            long mark = data.GetSafeLong();
            string a = System.Convert.ToString(mark, 2);
            string str = a.Replace("0", "\t").Replace("1", " ");
            return str;
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decode2(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            var a = data.Replace("\t", "0");
            var b = a.Replace(" ", "1");
            string str = data.Replace("\t", "0").Replace(" ", "1");
            long aa = System.Convert.ToInt64(str, 2);// d为string类型 以“1010”为例，输出为10
            str = aa.GetSafeString();
            return str;
        }

        /// <summary>
        /// 将字符串转成二进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string bianma(string s)
        {
            byte[] data = Encoding.Unicode.GetBytes(s);
            StringBuilder result = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }


        /// <summary>
        /// 将二进制转成字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string jiema(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
            System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        private const string s = "8401326597427530819649710235680683725149674012958317549280635610982437246739801542518730963519640872";
        public static string LtoS(string content)
        {
            string val = "";
            int total = 0;
            for (int i = content.Length - 1; i >= 0; i--)
            {
                var j = Convert.ToInt32(content[i].ToString());
                var b = (5 + i + Convert.ToInt32(s[((i + total) * 10 + j) % 100])) * 9 % 10;
                val += b;
                total += j;
            }
            val = Convert.ToString(Convert.ToInt64(val), 2);
            if (val.Length < 28)
            {
                val = val.PadLeft(28, '0');
            }
            else
            {
                var len = val.Length % 4 == 0 ? val.Length / 4 : val.Length / 4 + 1;
                val = val.PadLeft(4 * len, '0');
            }
            return val;
        }


        #region 使用 RNGCryptoServiceProvider 生成强随机字符串

        public static string GetRNGCryptoServiceProviderRandom()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng
            = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            string str = (Math.Abs(BitConverter.ToInt32(bytes, 0))).GetSafeString();
            return str;
        }


        private static RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();

        public static string GetRandomString(int stringlength)
        {
            return GetRandomString(null, stringlength);
        }

        //获得长度为stringLength的随机字符串，以key为字母表
        public static string GetRandomString(string key, int stringLength)
        {
            if (key == null || key.Length < 8)
            {
                key = "1234567890";
                // key = "abcdefghijklmnopqrstuvwxyz1234567890";
            }

            int length = key.Length;
            StringBuilder randomString = new StringBuilder(length);
            for (int i = 0; i < stringLength; ++i)
            {
                randomString.Append(key[SetRandomSeeds(length)]);
            }

            return randomString.ToString();
        }

        private static int SetRandomSeeds(int length)
        {
            decimal maxValue = (decimal)int.MaxValue;
            byte[] array = new byte[4];
            _random.GetBytes(array);

            return (int)(Math.Abs(BitConverter.ToInt32(array, 0)) / maxValue * length);
        }
        #endregion


    }
}
