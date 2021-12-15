using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using System.Data;
using System.Data.OleDb;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace BD.Jcbg.Bll
{
	public class ExcelService : IExcelService
	{
		#region 数据库对象
		public ICommonDao CommonDao { get; set; }
		#endregion
		#region 服务
		/// <summary>
		/// 导入工资
		/// </summary>
		/// <param name="filepath"></param>
		public bool ImportWage(string filepath, int year, int month, string curusername, string currealname,
			IList<KeyValuePair<string,string>>users, out string msg)
		{
			bool ret = true;
			msg = "";
			try
			{
				WageConfig wconfig = new WageConfig();
				if (!wconfig.IsValid)
				{
					msg = "加载配置文件失败";
					ret = false;
					return ret;
				}
				DataSet ds = GetDatas(filepath);

				if (ds.Tables.Count == 0)
				{
					msg = "没有数据表格";
					return false;
				}

				DataTable table = ds.Tables[0];
				for (int i=0; i<table.Rows.Count; i++)
				{
					if (IsNullRow(table.Rows[i]))				// 跳过空行
						continue;
					int startindex = i;							// 数据开始行
					int rowsum = GetRowSum(table, startindex);	// 数据行数
					int columnsum = GetColumnSum(table, startindex, rowsum);	// 数据列数

					// 根据行列数匹配配置项
					WageEmployee configItem = wconfig.GetConfig(rowsum, columnsum);
					if (configItem != null)
					{
						// 查找工资条姓名配置项
						WageDataItem itm = configItem.Get("wagename");
						// 获取工资条用户姓名
						string realname = GetCellValue(table, columnsum, startindex, itm.DataRow, itm.FieldDesc);
						// 获用户帐户姓名
						var quser = from e in users where e.Value.Equals(realname, StringComparison.OrdinalIgnoreCase) select e;

						KeyValuePair<string, string> useritem = new KeyValuePair<string, string>("", "");
						if (quser.Count() > 0)
							useritem = quser.First();
						StringBuilder sbfields = new StringBuilder();
						StringBuilder sbvalues = new StringBuilder();
						foreach (WageDataItem data in configItem.DatatItems)
						{
							sbfields.Append("," + data.FieldName);
							sbvalues.Append(",'" + GetCellValue(table, columnsum, startindex, data.DataRow, data.FieldDesc) + "'");
						}
						string sql = "insert into userwage (username,realname,wagetype,wageyear,wagemonth,lrr,lrrxm,lrsj" + sbfields +
							") values('" + useritem.Key + "','" + useritem.Value + "','" + configItem.EmployeeType + "'," + year + "," + month + ",'" + curusername + "','" +
							currealname + "',getdate()" + sbvalues + ")";
						ret = CommonDao.ExecCommand(sql, CommandType.Text);
					}
					else
					{
						msg += string.Format("数据找不到配置项，从{0}行开始，行数{1}，列数{2}。", startindex, rowsum, columnsum);
					}
					i += rowsum - 1;

				}

			}
			catch (Exception e)
			{
				msg = e.Message;
				SysLog4.WriteLog(e);
				ret = false;
			}
			return ret;
		}

		/// <summary>
		/// 导入加班费
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="extrainfo"></param>
		/// <param name="curusername"></param>
		/// <param name="currealname"></param>
		/// <param name="users"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool ImportExtraWage(string filepath, int year, int month, string extrainfo, string curusername, string currealname,
			IList<KeyValuePair<string, string>> users, out string msg)
		{
			bool ret = true;
			msg = "";
			try
			{
				WageExtraConfig wconfig = new WageExtraConfig();
				if (!wconfig.IsValid)
				{
					msg = "加载配置文件失败";
					ret = false;
					return ret;
				}
				DataSet ds = GetDatas(filepath);

				if (ds.Tables.Count == 0)
				{
					msg = "没有数据表格";
					return false;
				}

				for (int x = 0; x < ds.Tables.Count; x++)
				{
					DataTable table = ds.Tables[x];
					int titleIndex = GetTitleIndex(table, wconfig);
					if (titleIndex == -1)
					{
						SysLog4.WriteError("第" + x + "个表格找不到标题行");
						continue;
					}
					for (int i = titleIndex+1; i < table.Rows.Count; i++)
					{

						IList<IDictionary<string, string>> rowDatas = GetRowDatas(table, i, titleIndex, wconfig);

						for (int j = 0; j < rowDatas.Count; j++)
						{
							IDictionary<string, string> rowData = rowDatas[j];
							// 获取工资条用户姓名
							var q = from e in rowData where e.Key.Equals("wagename", StringComparison.OrdinalIgnoreCase) select e;
							if (q.Count() == 0)
							{
								SysLog4.WriteLog("找不到wagename列");
								continue;
							}
							
							string realname = q.First().Value;
							if (realname == "")
								continue;
							// 获用户帐户姓名
							var quser = from e in users where e.Value.Equals(realname, StringComparison.OrdinalIgnoreCase) select e;
							if (quser.Count() == 0)
							{
								SysLog4.WriteLog("找不到" + realname + "");
								continue;
							}
							KeyValuePair<string, string> useritem = quser.First();

							StringBuilder sbfields = new StringBuilder();
							StringBuilder sbvalues = new StringBuilder();
							
							for (int k=0;k<rowData.Keys.Count;k++)
							{
								sbfields.Append("," + rowData.Keys.ElementAt(k));
								sbvalues.Append(",'" + rowData[rowData.Keys.ElementAt(k)] + "'");
							}
							string sql = "insert into UserExtraWage(username,realname,wageyear,wagemonth,lrr,lrrxm,lrsj,Mark" + sbfields +
								") values('" + useritem.Key + "','" + useritem.Value + "'," + year + "," + month + ",'" + curusername + "','" +
								currealname + "',getdate(),'"+extrainfo+"'" + sbvalues + ")";
							ret = CommonDao.ExecCommand(sql, CommandType.Text);
						}

					}
				}

			}
			catch (Exception e)
			{
				msg = e.Message;
				SysLog4.WriteLog(e);
				ret = false;
			}
			return ret;
		}
        /// <summary>
        /// 把excel得第一个sheet解析成表格数据
        /// </summary>
        /// <param name="filecontent"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string,string>> ParseExcel(Stream content, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                IWorkbook workbook = new XSSFWorkbook(content);
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    // 遍历，获取特殊需要替换的单元格
                    ExcelPrintCellCollection pcells = new ExcelPrintCellCollection();
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                            continue;
                        IDictionary<string, string> retRow = new Dictionary<string, string>();
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell == null)
                                continue;
                            retRow.Add(j.ToString(), cell.ToString());
                        }
                        ret.Add(retRow);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
		#endregion
		#region 导入工资条其他函数
		/// <summary>
		/// 读取excel数据
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private DataSet GetDatas(string filePath)
		{
			string connStr = "";
			string fileType = System.IO.Path.GetExtension(filePath);
			if (string.IsNullOrEmpty(fileType)) return null;

			if (fileType == ".xls")
				connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
			else
				connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";
			string sql_F = "Select * FROM [{0}]";

			OleDbConnection conn = null;
			OleDbDataAdapter da = null;
			DataTable dtSheetName = null;

			DataSet ds = new DataSet();
			try
			{
				// 初始化连接，并打开
				conn = new OleDbConnection(connStr);
				conn.Open();

				// 获取数据源的表定义元数据                        
				string SheetName = "";
				dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

				// 初始化适配器
				da = new OleDbDataAdapter();
				for (int i = 0; i < dtSheetName.Rows.Count; i++)
				{
					SheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];

					if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
					{
						continue;
					}

					da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
					DataSet dsItem = new DataSet();
					da.Fill(dsItem, SheetName);

					ds.Tables.Add(dsItem.Tables[0].Copy());
				}
			}
			catch (Exception ex)
			{
				SysLog4.WriteLog(ex);
			}
			finally
			{
				// 关闭连接
				if (conn.State == ConnectionState.Open)
				{
					conn.Close();
					da.Dispose();
					conn.Dispose();
				}
			}
			return ds;
		}
		/// <summary>
		/// 判断一行是否是空行
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		private bool IsNullRow(DataRow row)
		{
			bool ret = true;
			try
			{
				object[] colums = row.ItemArray;
				for (int j = 0; j < colums.Count(); j++)
				{
					if (colums[j].GetSafeString().Trim() != "")
					{
						ret = false;
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
		/// <summary>
		/// 计算行数，根据空行计算
		/// </summary>
		/// <param name="table"></param>
		/// <param name="startrow"></param>
		/// <returns></returns>
		private int GetRowSum(DataTable table, int startrow)
		{
			int ret = 0;
			try
			{
				int endrow = startrow;

				while (endrow < table.Rows.Count && !IsNullRow(table.Rows[endrow]))
					endrow++;

				ret = endrow - startrow;
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 所有行中最大列数
		/// </summary>
		/// <param name="table"></param>
		/// <param name="startrow"></param>
		/// <param name="rowsum"></param>
		/// <returns></returns>
		private int GetColumnSum(DataTable table, int startrow, int rowsum)
		{
			int ret = 0;
			try
			{
				for (int i = startrow; i < startrow + rowsum; i++)
				{
					object[] colums = table.Rows[i].ItemArray;
					int columnindex = colums.Count() - 1;
					do
					{
						if (colums[columnindex].GetSafeString().Trim() != "")
							break;
					} while (columnindex-- >= 0);
					columnindex += 1;

					ret = Math.Max(ret, columnindex);
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		/// <summary>
		/// 获取单元格值
		/// </summary>
		/// <param name="table"></param>
		/// <param name="startrow"></param>
		/// <param name="fieldrow"></param>
		/// <param name="fieldcolumn"></param>
		/// <returns></returns>
		private string GetCellValue(DataTable table,int maxColumnSum, int startrow, int fieldrow, string fielddesc)
		{
			string ret = "";
			try
			{
				int realFieldRow = startrow + fieldrow - 1;
				int realDescRow = realFieldRow - 1;
				int columnIndex = 0;
				for (; columnIndex < maxColumnSum; columnIndex++)
				{
					string desc = table.Rows[realDescRow].ItemArray[columnIndex].GetSafeString().Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
					if (desc.Equals(fielddesc, StringComparison.OrdinalIgnoreCase))
					{
						ret = table.Rows[realFieldRow].ItemArray[columnIndex].GetSafeString();
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
		#endregion
		#region 导入加班费其他函数
		private int GetTitleIndex(DataTable dt, WageExtraConfig config)
		{
			int ret = -1;
			try
			{
				if (!config.IsValid)
					return ret;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					bool allFind = true;
					for (int j = 0; j < config.DataItems.Count; j++)
					{
						string configName = config.DataItems[j].FieldDesc;
						int k = 0;
						for (; k < dt.Columns.Count; k++)
						{
							string columnValue = dt.Rows[i][k].GetSafeString().Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
							if (configName.Equals(columnValue, StringComparison.OrdinalIgnoreCase))
							{
								break;
							}
						}
						if (k == dt.Columns.Count)
						{
							allFind = false;
							break;
						}
					}
					if (allFind)
					{
						ret = i;
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
		private IList<IDictionary<string, string>> GetRowDatas(DataTable dt, int rowIndex, int titleIndex, WageExtraConfig config)
		{
			IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
			try
			{
				
				IList<KeyValuePair<WageExtraDataItem, IList<int>>> fieldColumns = GetFieldColumns(dt.Rows[titleIndex], config);
				if (fieldColumns.Count == 0)
					return ret;
				for (int i = 0; i < fieldColumns[0].Value.Count; i++)
				{
					IDictionary<string, string> row = new Dictionary<string, string>();
					bool tooLong = false;
					int extraIndex = 5;//Wage5开始为额外扣款
					for (int j = 0; j < fieldColumns.Count; j++)
					{
						KeyValuePair<WageExtraDataItem, IList<int>> fieldColumn = fieldColumns[j];
						if (i >= fieldColumn.Value.Count)
						{
							tooLong = true;
							SysLog4.WriteError("字段" + fieldColumn.Key.FieldDesc + "重复" + fieldColumn.Value.Count + "次，小于第一个字段" + fieldColumn.Value.Count + "次");
							break;
						}
						// 正常字段
						string fieldName = fieldColumn.Key.FieldName;
						if (fieldName == "")
							fieldName = "Wage" + (extraIndex++).ToString();

						row.Add(fieldName, dt.Rows[rowIndex][fieldColumn.Value[i]].GetSafeString());
					}
					if (tooLong)
						break;
					ret.Add(row);
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		private IList<KeyValuePair<WageExtraDataItem, IList<int>>> GetFieldColumns(DataRow titleRow, WageExtraConfig config)
		{
			IList<KeyValuePair<WageExtraDataItem, IList<int>>> ret = new List<KeyValuePair<WageExtraDataItem, IList<int>>>();
			try
			{
				for (int i = 0; i < titleRow.Table.Columns.Count; i++)
				{
					string fieldDesc = titleRow[i].GetSafeString().Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");

					var qi = from e in config.DataItems where e.FieldDesc.Equals(fieldDesc, StringComparison.OrdinalIgnoreCase) select e;
					var qe = from e in config.ExcludeItems where e.FieldDesc.Equals(fieldDesc, StringComparison.OrdinalIgnoreCase) select e;
					// 申明的字段
					if (qi.Count() > 0)
					{
						WageExtraDataItem configItem = qi.First();
						var q = from e in ret where e.Key.FieldName.Equals(configItem.FieldName) && e.Key.FieldDesc.Equals(configItem.FieldDesc) select e;
						if (q.Count() == 0)
						{
							IList<int> indexs = new List<int>();
							indexs.Add(i);
							ret.Add(new KeyValuePair<WageExtraDataItem, IList<int>>(configItem, indexs));
						}
						else
							q.First().Value.Add(i);
					}
					// 非排除的字段，即可变字段
					else if (qe.Count() == 0)
					{
						WageExtraDataItem itm = new WageExtraDataItem() { FieldName = "", FieldDesc = fieldDesc };
						var q = from e in ret where e.Key.FieldName.Length == 0 && e.Key.FieldDesc.Equals(fieldDesc) select e;
						if (q.Count() == 0)
						{
							
							IList<int> indexs = new List<int>();
							indexs.Add(i);
							ret.Add(new KeyValuePair<WageExtraDataItem, IList<int>>(itm, indexs));
						}
						else
							q.First().Value.Add(i);
					}
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
		#endregion

		#region 解析excel
        public ISheet jxExcel(byte[] bytes)
        {
 
            IWorkbook workbook = null;
            ISheet sheet = null;
            Stream fstream = new MemoryStream(bytes);
            workbook = new HSSFWorkbook(fstream);
            sheet = workbook.GetSheetAt(0);
          //  string AB = sheet.GetCellValue(2, 2);
            return sheet;

        }
        #endregion
	}
}
