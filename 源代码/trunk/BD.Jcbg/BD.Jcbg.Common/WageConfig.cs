using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Data;

namespace BD.Jcbg.Common
{
	public class WageConfig
	{
		IList<WageEmployee> m_Employees = null;
		public IList<WageEmployee> Employees
		{
			get { return m_Employees; }
		}

		public bool IsValid = false;

		
		public WageConfig()
		{

			m_Employees = new List<WageEmployee>();
			try
			{
				XDocument document = XDocument.Load(string.Format(@"{0}\configs\wage.xml", SysEnvironment.CurPath));

				var query = from m in document.Elements("employees").Elements("employee")
							select m;

				foreach (XElement ele in query)
				{
					WageEmployee employee = new WageEmployee();
					if (!employee.Load(ele))
						continue;
					if (!employee.IsValid)
					{
						SysLog4.WriteError("无效的人员信息:" + employee.ToString());
						continue;
					}
					var q = from e in m_Employees where e.EmployeeType.Equals(employee.EmployeeType, StringComparison.OrdinalIgnoreCase) select e;
					if (q.Count() > 0)
					{
						SysLog4.WriteError("人员类型重复：" + employee.ToString());
						continue;
					}

					m_Employees.Add(employee);
				}
				var qo = from e in m_Employees orderby e.ColumnMin descending select e;
				m_Employees = qo.ToList();
				IsValid = true;
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				IsValid = false;
			}

		}

		public WageEmployee GetConfig(int rowSum, int columnSum)
		{
			WageEmployee ret = null;
			try
			{
				if (!IsValid)
					return ret;
				foreach (WageEmployee itm in m_Employees)
				{
					if (columnSum >= itm.ColumnMin && itm.RowSum==rowSum)
					{
						ret = itm;
						break;
					}
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}

	public class WageEmployee
	{
		public string EmployeeType { get; set; }
		public int RowSum { get; set; }
		public int ColumnMin { get; set; }
		public IList<WageDataItem> DatatItems { get; set; }

		public bool IsValid
		{
			get
			{
				bool ret = false;
				try
				{
					ret = EmployeeType.Length > 0 && RowSum > 0 && ColumnMin > 0 && DatatItems.Count > 0;
					if (ret)
					{
						var q = from e in DatatItems where e.FieldName.Equals("wagename", StringComparison.OrdinalIgnoreCase) select e;
						ret = q.Count() > 0;
					}
				}
				catch (Exception e)
				{
					SysLog4.WriteLog(e);
				}
				return ret;
			}
		}

		public WageDataItem Get(string itemname)
		{
			WageDataItem ret = null;
			if (!IsValid)
				return ret;
			try
			{
				var q = from e in DatatItems where e.FieldName.Equals(itemname, StringComparison.OrdinalIgnoreCase) select e;
				if (q.Count() > 0)
					ret = q.First();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public bool Load(XElement ele)
		{
			bool ret = true;
			try
			{
				EmployeeType = ele.Element("type").Value.GetSafeString();
				RowSum = ele.Element("rowsum").Value.GetSafeInt();
				ColumnMin = ele.Element("columnmin").Value.GetSafeInt();

				DatatItems = new List<WageDataItem>();

				var q = from e in ele.Elements("datas").Elements("data") select e;

				foreach (XElement ce in q)
				{
					WageDataItem itm = new WageDataItem();
					if (!itm.Load(ce))
						continue;
					if (!itm.IsValid)
					{
						SysLog4.WriteError("无效的项：" + itm.ToString());
						continue;
					}
					var q1 = from e in DatatItems where e.FieldName.Equals(itm.FieldName, StringComparison.OrdinalIgnoreCase) select e;
					if (q1.Count() > 0)
					{
						SysLog4.WriteError("字段名称重复：" + itm.ToString());
						continue;
					}
					if (itm.DataRow > RowSum)
					{
						SysLog4.WriteError("超过最大行数：" + itm.ToString());
						continue;
					}
					DatatItems.Add(itm);
				}
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
			StringBuilder ret = new StringBuilder();
			try
			{
				ret.Append(string.Format("type:{0} rowsum:{1} cloumnsum:{2} datas:[", EmployeeType, RowSum, ColumnMin));
				if (DatatItems != null)
				{
					foreach (WageDataItem itm in DatatItems)
					{
						ret.Append("{" + itm.ToString() + "}");
					}
				}
				ret.Append("]");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret.ToString();
		}
	}

	public class WageDataItem
	{
		public string FieldName { get; set; }
		public string FieldDesc { get; set; }
		public int DataRow { get; set; }

		public bool IsValid
		{
			get
			{
				bool ret = false;
				try
				{
					ret = FieldName.Length > 0 && DataRow > 0  && FieldDesc.Length>0;
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
				DataRow = ele.Attribute("row").Value.GetSafeInt();
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
				ret = string.Format("field:{0} desc:{1} row:{2} ", FieldName, FieldDesc, DataRow);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
