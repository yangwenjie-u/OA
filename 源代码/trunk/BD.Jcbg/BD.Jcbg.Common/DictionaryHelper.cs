using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class DictionaryHelper
    {
        /// <summary>
        /// 获取键值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(IDictionary<string, string> dict, string key)
        { 
            string value = string.Empty;

            if (dict == null)
                return value;

            if (dict.ContainsKey(key))
                value = dict[key];

            return value;
        }

        /// <summary>
        /// 获取键值(key不区分大小写)
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueIgnoreCase(IDictionary<string, string> dict, string key)
        {
            string value = string.Empty;

            if (dict == null)
                return value;

            if (dict.ContainsKey(key))
                value = dict[key];
            else if (dict.ContainsKey(key.ToUpper()))
                value = dict[key.ToUpper()];
            else if (dict.ContainsKey(key.ToLower()))
                value = dict[key.ToLower()];

            return value;
        }

        /// <summary>
        /// 赋值键值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void SetValue(IDictionary<string, string> dict, string key, string value)
        {
            if (dict != null)
            {
                if (dict.ContainsKey(key))
                    dict[key] = value;
                else
                    dict.Add(key, value);
            }
        }
    }
}
