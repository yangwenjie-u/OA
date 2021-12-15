using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class TimeUtil
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <returns></returns>
        public static string GetDate()
        {
            return string.Format("{0:yyyy-MM-dd}", DateTime.Now);
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <returns></returns>
        public static string GetYear()
        {
            return string.Format("{0:yyyy}", DateTime.Now);
        }

        /// <summary>
        /// 日期时间
        /// </summary>
        /// <returns></returns>
        public static string GetDateTime()
        {
            return String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
        }

        /// <summary>
        /// 月
        /// </summary>
        /// <returns></returns>
        public static string GetMonth()
        {
            return String.Format("{0:yyyy-MM}", DateTime.Now);
        }
    }
}
