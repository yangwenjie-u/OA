using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Data;

namespace BD.Jcbg.Common
{
	public class WageExtraConfig
	{
		IList<WageExtraDataItem> m_DataItems = null;
		public IList<WageExtraDataItem> DataItems
		{
			get { return m_DataItems; }
		}

		IList<WageExtraDataItem> m_ExcludeItems = null;
		public IList<WageExtraDataItem> ExcludeItems
		{
			get { return m_ExcludeItems; }
		}

		public bool IsValid = false;


		public WageExtraConfig()
		{

			m_DataItems = new List<WageExtraDataItem>();
			m_ExcludeItems = new List<WageExtraDataItem>();
			try
			{
				XDocument document = XDocument.Load(string.Format(@"{0}\configs\wageow.xml", SysEnvironment.CurPath));

				var query = from m in document.Elements("extraworkwage").Elements("datas").Elements("data")
							select m;

				foreach (XElement ele in query)
				{
					WageExtraDataItem item = new WageExtraDataItem();
					if (!item.Load(ele))
						continue;
					if (!item.IsValid)
					{
						SysLog4.WriteError("无效的配置项:" + item.ToString());
						continue;
					}
					var q = from e in m_DataItems where e.FieldName.Equals(item.FieldName, StringComparison.OrdinalIgnoreCase) select e;
					if (q.Count() > 0)
					{
						SysLog4.WriteError("字段名重复：" + item.ToString());
						continue;
					}

					m_DataItems.Add(item);
				}

				query = from m in document.Elements("extraworkwage").Elements("excludes").Elements("data")
						select m;

				foreach (XElement ele in query)
				{
					WageExtraDataItem item = new WageExtraDataItem();
					if (!item.Load(ele))
						continue;
					var q = from e in m_ExcludeItems where e.FieldDesc.Equals(item.FieldDesc, StringComparison.OrdinalIgnoreCase) select e;
					if (q.Count() > 0)
					{
						SysLog4.WriteError("排除字段描述重复：" + item.ToString());
						continue;
					}

					m_ExcludeItems.Add(item);
				}

				IsValid = true;
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				IsValid = false;
			}

		}

	}

	public class WageExtraDataItem
	{
		public string FieldName { get; set; }
		public string FieldDesc { get; set; }

		public bool IsValid
		{
			get
			{
				bool ret = false;
				try
				{
					ret = FieldName.Length > 0 && FieldDesc.Length>0;
				}
				catch (Exception e)
				{
					SysLog4.WriteLog(e);
				}
				return ret;
			}
		}
		public bool Load(XElement ele)
		{
			bool ret = true;
			try
			{
				FieldName = ele.Attribute("field").Value.GetSafeString();
				FieldDesc = ele.Attribute("desc").Value.GetSafeString();
			}
			catch (Exception e)
			{
				ret = false;
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		override public string ToString()
		{
			string ret = "";
			try
			{
				ret = string.Format("field:{0} desc:{1} ", FieldName, FieldDesc);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
