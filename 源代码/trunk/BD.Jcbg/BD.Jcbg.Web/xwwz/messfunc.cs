using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BD.Jcbg.Web.xwwz
{
    public class messfunc
    {
        /// <summary>
        /// 获取key对应的值
        /// </summary>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            string ret = System.Configuration.ConfigurationManager.AppSettings[key];
            return ret;
        }
    }
}