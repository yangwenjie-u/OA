using System;
using System.Text;

namespace Bd.jcbg.Common
{
    public class Base64Func
    {
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return source;
            }
        }

        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        public static string DecodeBase64(Encoding encode, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                return encode.GetString(bytes);
            }
            catch
            {
                return result;
            }
        }

        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

    }
}
