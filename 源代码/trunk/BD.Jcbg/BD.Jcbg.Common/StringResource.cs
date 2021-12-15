using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace BD.Jcbg.Common
{
	/// <summary>
	/// 资源类
	/// </summary>
	public static class StringResource
	{
		/// <summary>
		/// 获取枚举类型的释义
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string GetEnumString(object obj)
		{
			XDocument document = XDocument.Load(string.Format(@"{0}\configs\strings.xml", SysEnvironment.CurPath));
			string enumType = obj.GetType().ToString();
			int lastindex = enumType.LastIndexOf(".");
			if (lastindex > -1)
				enumType = enumType.Substring(lastindex + 1);
			var query = from m in document.Elements("enums").Elements(enumType)
						select m.Element(obj.ToString()).Value;
			return query.First<string>();

		}
        /// <summary>
        /// 获取系统定义的字符
        /// </summary>
        /// <param name="settingname"></param>
        /// <returns></returns>
        static string GetSettingString(string settingname)
        {
            XDocument document = XDocument.Load(string.Format(@"{0}\configs\Strings.xml", SysEnvironment.CurPath));

            var query = from m in document.Elements("objects").Elements("settings").Elements(settingname)
                        select m;
            if (query.Count() == 0)
                return "";
            return query.First().Value;
        }

        public static string CheckRyOut
        {
            get { return GetSettingString("ryoutday"); }
        }
	}
}
