using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace BD.JC.JS.Common
{
	/// <summary>
	/// 配置类
	/// </summary>
	public static class Configs
	{
		/// <summary>
		/// 获取配置项值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetConfigItem(string name)
		{
			XDocument document = XDocument.Load(string.Format(@"{0}\configs\configs.xml", SysEnvironment.CurPath));

			var query = from m in document.Elements("configs").Elements(name)
						select m;
			return query.First<XElement>().Value;

		}

		public static string UmsUrl
		{
            get { return GetConfigItem("umsurl"); }
		}

        public static string AppId
        {
            get { return GetConfigItem("appid"); }
        }
	}
}
