using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Log.Common;
using BD.Log.IBll;
using BD.Log.DataModal.Entities;

namespace BD.Jcbg.Web.Controllers
{
    public class LogController : Controller
    {
		#region 服务
		private ILogService _logService = null;
		private ILogService LogService
		{
			get
			{
				if (_logService == null)
				{
					IApplicationContext webApplicationContext = ContextRegistry.GetContext();
					_logService = webApplicationContext.GetObject("LogService") as ILogService;
				}
				return _logService;
			}
		}
		#endregion

		#region 页面
		/// <summary>
		/// 日志列表
		/// </summary>
		/// <returns></returns>
		public ActionResult List()
		{
			ActionResult ret = null;
			if (LogUser.HasLogManageRights)
				ret = View();

			try
			{
				SysLog log = new SysLog()
				{
					ModuleName = "日志模块",
					ClientType = "网页",
					Ip = ClientInfo.Ip,
					LogTime = DateTime.Now,
					Operation = "日志管理",
					UserName = LogUser.UserName,
					RealName = LogUser.RealName,
					Result = ret != null,
					Remark = "打开系统日志页面，" + (ret == null ? "失败，没有权限" : "成功")
				};
				LogService.SaveLog(log);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}

			return ret;
		}

		#endregion

		#region 处理函数
		/// <summary>
		/// 清除日志
		/// </summary>
		public void ClearLog()
		{
			bool ret = true;
			if (LogUser.HasLogManageRights)
				LogService.ClearLog();
			else
				ret = false;

			try
			{
				SysLog log = new SysLog()
				{
					ModuleName = "日志模块",
					ClientType = "网页",
					Ip = ClientInfo.Ip,
					LogTime = DateTime.Now,
					Operation = "日志管理",
					UserName = LogUser.UserName,
					RealName = LogUser.RealName,
					Result = ret,
					Remark = "清除日志结果，" + (ret? "成功" : "失败，没有权限")
				};
				LogService.SaveLog(log);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}

			Response.Write(JsonFormat.GetRetString(ret));
		}
		#endregion

		#region 获取列表
		/// <summary>
		/// 获取系统日志列表表
		/// </summary>
		public void GetLogs()
		{
			string str = "";
			if (!LogUser.HasLogManageRights)
				str = "{{\"total\":0,\"rows\":[]}}";
			else
			{
				string username = DataFormat.GetSafeString(Request["username"]);
				string modulename = DataFormat.GetSafeString(Request["modulename"]);
				string result = DataFormat.GetSafeString(Request["result"]);
				string ip = DataFormat.GetSafeString(Request["ip"]);
				string clienttype = DataFormat.GetSafeString(Request["clienttype"]);
				string operation = DataFormat.GetSafeString(Request["operation"]);
				string remark = DataFormat.GetSafeString(Request["remark"]);
				string time1 = DataFormat.GetSafeString(Request["time1"]);
				if (time1 != "")
					time1 += ":00";
				string time2 = DataFormat.GetSafeString(Request["time2"]);
				if (time2 != "")
					time2 += ":00";
				int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
				int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);

				int totalcount = 0;
				IList<SysLog> logs = LogService.GetLogs(username, modulename, operation, remark, result, ip, clienttype,
					time1, time2, pagesize, pageindex, out totalcount);

				JavaScriptSerializer jss = new JavaScriptSerializer();
				str = string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(logs));
			}
			Response.Write(str);
		}
		/// <summary>
		/// 获取非分页日志
		/// </summary>
		public void GetNotPageLogs()
		{
			string str = "";
			if (!LogUser.HasLogManageRights)
				str = "{[]}";
			else
			{
				string username = DataFormat.GetSafeString(Request["username"]);
				string modulename = DataFormat.GetSafeString(Request["modulename"]);
				string result = DataFormat.GetSafeString(Request["result"]);
				string ip = DataFormat.GetSafeString(Request["ip"]);
				string clienttype = DataFormat.GetSafeString(Request["clienttype"]);
				string operation = DataFormat.GetSafeString(Request["operation"]);
				string remark = DataFormat.GetSafeString(Request["remark"]);
				string time1 = DataFormat.GetSafeString(Request["time1"]);
				if (time1 != "")
					time1 += ":00";
				string time2 = DataFormat.GetSafeString(Request["time2"]);
				if (time2 != "")
					time2 += ":00";
				IList<SysLog> logs = LogService.GetLogs(username, modulename, operation, remark, result, ip, clienttype,
					time1, time2);

				JavaScriptSerializer jss = new JavaScriptSerializer();
				str = jss.Serialize(logs);
			}
			Response.Write(str);
		}
		/// <summary>
		/// 导出excel
		/// </summary>
		public void ExportExcel()
		{
			StringBuilder str = new StringBuilder();
			str.Append("时间,操作用户,所属模块,操作内容,操作详情,是否成功,客户端类型,Ip地址\r\n");
			if (LogUser.HasLogManageRights)
			{
				string username = DataFormat.GetSafeString(Request["username"]);
				string modulename = DataFormat.GetSafeString(Request["modulename"]);
				string result = DataFormat.GetSafeString(Request["result"]);
				string ip = DataFormat.GetSafeString(Request["ip"]);
				string clienttype = DataFormat.GetSafeString(Request["clienttype"]);
				string operation = DataFormat.GetSafeString(Request["operation"]);
				string remark = DataFormat.GetSafeString(Request["remark"]);
				string time1 = DataFormat.GetSafeString(Request["time1"]);
				if (time1 != "")
					time1 += ":00";
				string time2 = DataFormat.GetSafeString(Request["time2"]);
				if (time2 != "")
					time2 += ":00";
				IList<SysLog> logs = LogService.GetLogs(username, modulename, operation, remark, result, ip, clienttype,
					time1, time2);

				foreach (SysLog log in logs)
				{
					str.Append(DataFormat.GetSafeDate(log.LogTime).ToString("yyyy-MM-dd HH:mm:ss")+",");
					str.Append(log.UserName+"("+log.RealName+"),");
					str.Append(log.ModuleName+",");
					str.Append(log.Operation+",");
					str.Append(log.Remark+",");
					str.Append((DataFormat.GetSafeBool(log.Result)?"是":"否")+",");
					str.Append(log.ClientType+",");
					str.Append(log.Ip+",");
					str.Append("\r\n");
				}
			}

			Response.Clear();
			Response.Buffer = false;
			Response.AppendHeader("Content-Disposition", "attachment;filename=log.csv");
			Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			Response.ContentType = "application/vnd.ms-excel";
			Response.Write(str.ToString());
		}
		/// <summary>
		/// 获取日志模块
		/// </summary>
		public void GetLogModules()
		{
			Response.Write(new JavaScriptSerializer().Serialize(LogService.GetLogModules()));
		}
		/// <summary>
		/// 获取日志操作
		/// </summary>
		public void GetLogOperations()
		{
			Response.Write(new JavaScriptSerializer().Serialize(LogService.GetLogOperations()));
		}
		/// <summary>
		/// 获取日志客户端类型
		/// </summary>
		public void GetLogClitentTypes()
		{
			Response.Write(new JavaScriptSerializer().Serialize(LogService.GetLogClientTypes()));
		}
		#endregion


	}
}
