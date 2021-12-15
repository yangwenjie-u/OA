using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class HelpLinkHelper
    {
        public static Dictionary<string, string> HandleHelpLink(string helpLink)
        {
            if (string.IsNullOrEmpty(helpLink))
                return null;

            helpLink = helpLink.Trim();
            Dictionary<string, string> dict = new Dictionary<string, string>();

            helpLink = helpLink.Replace("helplink--", "");
            var items = helpLink.Split('|');

            foreach (var item in items)
            {
                var subItem = item.Split('-');
                if (subItem.Length == 2)
                {
                    if (!dict.ContainsKey(subItem[0]))
                        dict.Add(subItem[0], subItem[1]);
                }
            }

            return dict;
        }

        public static List<string> GetHelpLinkValue(Dictionary<string, string> dict, string key)
        {
            if (dict == null)
                return new List<string>();

            string value = string.Empty;

            if (dict.ContainsKey(key.ToLower()))
                value = dict[key.ToLower()];

            if (!string.IsNullOrEmpty(value))
            {
                return value.Split(',').ToList();
            }

            return new List<string>();
        }
    }
}
