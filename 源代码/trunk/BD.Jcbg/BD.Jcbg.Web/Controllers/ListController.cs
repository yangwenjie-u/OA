using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;

namespace BD.Jcbg.Web.Controllers
{
    public class ListController : Controller
    {
		#region 服务
		private ICommonService _commonService = null;
		private ICommonService CommonService
		{
			get
			{
				if (_commonService == null)
				{
					IApplicationContext webApplicationContext = ContextRegistry.GetContext();
					_commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
				}
				return _commonService;
			}
		}
		private IExcelService _excelService = null;
		private IExcelService ExcelService
		{
			get
			{
				if (_excelService == null)
				{
					IApplicationContext webApplicationContext = ContextRegistry.GetContext();
					_excelService = webApplicationContext.GetObject("ExcelService") as IExcelService;
				}
				return _excelService;
			}
		}
		private ISystemService _systemService = null;
		private ISystemService SystemService
		{
			get
			{
				if (_systemService == null)
				{
					IApplicationContext webApplicationContext = ContextRegistry.GetContext();
					_systemService = webApplicationContext.GetObject("SystemService") as ISystemService;
				}
				return _systemService;
			}
		}
		#endregion

		#region 列表删除操作
		/// <summary>
		/// 删除桌面项
		/// </summary>
		public void DeleteHelpDesktopItem()
		{
			bool ret = true;

			try
			{
				ret = CommonService.Delete("HelpDesktopItem", "RECID", Request["id"].GetSafeString());
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			finally
			{
				
				Response.Write(JsonFormat.GetRetString(ret));
			}
		}
		/// <summary>
		/// 删除印章
		/// </summary>
		public void DeleteHelpSeal()
		{
			bool ret = true;

			try
			{
				ret = CommonService.Delete("HelpSeal", "RECID", Request["id"].GetSafeString());
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret));
			}
		}
		/// <summary>
		/// 删除标准
		/// </summary>
		public void DeleteHelpStandard()
		{
			bool ret = true;

			try
			{
				ret = CommonService.Delete("HelpStandard", "RECID", Request["id"].GetSafeString());
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret));
			}
		}
		/// <summary>
		/// 删除归档目录
		/// </summary>
		public void DeleteHelpDocumentCategory()
		{
			bool ret = true;

			try
			{
				ret = CommonService.Delete("HelpDocumentCategory", "RECID", Request["id"].GetSafeString());
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret));
			}
		}
		/// <summary>
		/// 删除员工信息
		/// </summary>
		public void DeleteInfo()
		{
			bool ret = true;

			try
			{
				string table = Request["tablename"].GetSafeString();
				string field = Request["fieldname"].GetSafeString();
				string fvalue = Request["fieldvalue"].GetSafeString();
				ret = CommonService.Delete(table, field, fvalue);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret));
			}
		}
		/// <summary>
		/// 删除规章制度
		/// </summary>
		public void DeleteCompanyRule()
		{
			bool ret = true;

			try
			{
				string recid = Request["recid"].GetSafeString();
				string files = Request["files"].GetSafeString();
				ret = CommonService.Delete("CompanyRule", "RECID", recid);
				if (ret)
				{
					string[] arrfiles = files.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string strfile in arrfiles)
					{
						string[] arrtmp = strfile.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						if (arrtmp.Length < 2)
							continue;
						ret = CommonService.Delete("DataEntryFile", "fileid", arrtmp[0]); 
					}
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret));
			}
		}
		#endregion

		#region 导入工资
		[Authorize]
		public void ImportWage()
		{
			

			bool ret = true;
			string msg = "";

			try
			{
				int year = Request["import_year"].GetSafeInt();
				int month = Request["import_month"].GetSafeInt();

				HttpPostedFileBase file = Request.Files["import_file"];

				if (year == 0)
				{
					ret = false;
					msg += " 年份不正确";
				}
				if (month == 0)
				{
					ret = false;
					msg += " 月份不正确";
				}
				if (file == null || file.ContentLength == 0)
				{
					ret = false;
					msg += " 文件为空";
				}
				else if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) &&
					!file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
				{
					ret = false;
					msg += " 文件格式不正确，只能是excel文件";
				}
				if (ret)
				{
					string filepath = Server.MapPath("/tempfiles/" + CurrentUser.UserName + "_" + DateTime.Now.ToString("yyyMMdd_HHmmss_ff") + Path.GetExtension(file.FileName));
					file.SaveAs(filepath);

					IList<KeyValuePair<string,string>> users = new List<KeyValuePair<string,string>>();
					foreach (RemoteUserService.VUser vuser in Remote.UserService.Users)
						users.Add(new KeyValuePair<string,string>(vuser.USERNAME, vuser.REALNAME));

					ret = ExcelService.ImportWage(filepath, year, month, CurrentUser.UserName, CurrentUser.RealName, users, out msg);

				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
				msg = e.Message;
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret, msg));
			}
		}
		#endregion

		#region 导入加班工资
		[Authorize]
		public void ImportExtraWage()
		{


			bool ret = true;
			string msg = "";

			try
			{
				int year = Request["importe_year"].GetSafeInt();
				int month = Request["importe_month"].GetSafeInt();
				string mark = Request["importe_mark"].GetSafeString();

				HttpPostedFileBase file = Request.Files["importe_file"];

				if (year == 0)
				{
					ret = false;
					msg += " 年份不正确";
				}
				if (month == 0)
				{
					ret = false;
					msg += " 月份不正确";
				}
				if (file == null || file.ContentLength == 0)
				{
					ret = false;
					msg += " 文件为空";
				}
				else if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) &&
					!file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
				{
					ret = false;
					msg += " 文件格式不正确，只能是excel文件";
				}
				if (ret)
				{
					string filepath = Server.MapPath("/tempfiles/" + CurrentUser.UserName + "_" + DateTime.Now.ToString("yyyMMdd_HHmmss_ff") + Path.GetExtension(file.FileName));
					file.SaveAs(filepath);

					IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
					foreach (RemoteUserService.VUser vuser in Remote.UserService.Users)
						users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

					ret = ExcelService.ImportExtraWage(filepath, year, month, mark, CurrentUser.UserName, CurrentUser.RealName, users, out msg);

				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
				msg = e.Message;
			}
			finally
			{

				Response.Write(JsonFormat.GetRetString(ret, msg));
			}
		}
		#endregion

		#region 查看文件
		/// <summary>
		/// 查看文件页面
		/// </summary>
		/// <returns></returns>
		public void FileView()
		{
			string filename = "";
			long filesize = 0;
			byte[] ret = null;
			string fileid = Request["id"].GetSafeString();
			try
			{

				bool code = SystemService.GetFileDetail(fileid, out filename, out ret);
				if (code)
				{
					filesize = ret.Length;

					string mime = BD.WorkFlow.Common.MimeMapping.GetMimeMapping(filename);
					Response.Clear();
					Response.ContentType = mime;
					Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
					Response.AddHeader("Content-Length", filesize.ToString());
					Response.BinaryWrite(ret);
					Response.Flush();
					Response.End();
				}
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{

			}
		}
		#endregion

	}
}
