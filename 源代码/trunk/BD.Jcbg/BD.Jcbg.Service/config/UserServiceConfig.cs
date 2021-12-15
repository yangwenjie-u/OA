using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BD.Jcbg.Common;

namespace BD.Jcbg.Service.config
{
    public class UserServiceConfig
    {
        #region 属性
        /// <summary>
        /// 用户系统的应用APPID
        /// </summary>
        public static string UmsAppId = "";

        /// <summary>
        /// 用户系统的WebService路径
        /// </summary>
        public static string UmsUrl = "";
        #endregion

        /// <summary>
        /// 根据配置文件节点名获取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfigItem(string name)
        {
            XDocument document = XDocument.Load(string.Format(@"{0}configs\UserSystemConfig.xml", SysEnvironment.CurPath));
            var query = from m in document.Elements("configs").Elements(name) select m;
            return query.First<XElement>().Value;
        }

        static UserServiceConfig()
        { 
            /// <summary>
            /// 用户系统的应用APPID
            /// </summary>
            UmsAppId = GetConfigItem("appid");

            /// <summary>
            /// 用户系统的WebService路径
            /// </summary>
            UmsUrl = GetConfigItem("url");
        }

    }
}
