using BD.Jcbg.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BD.Jcbg.Web.xwwz
{
    /// <summary>
    /// 配置类
    /// </summary>
    public static class XWConfigs
    {
        /// <summary>
        /// 获取配置项值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfigItem(string name)
        {
            XDocument document = XDocument.Load(string.Format(@"{0}\configs\xwwz.xml", SysEnvironment.CurPath));

            var query = from m in document.Elements("configs").Elements(name)
                        select m;
            return query.First<XElement>().Value;

        }
        /// <summary>
        /// 新闻图片Url
        /// </summary>
        public static string TpUrl
        {
            get { return GetConfigItem("tpurl"); }
        }

       
    }
}