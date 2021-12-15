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
using BD.WorkFlow.Common;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using BD.WorkFlow.IBll;
using System.Web.UI;
namespace BD.WorkFlow.Web.Controllers
{
    public class WorkFlowController : Controller
    {
        #region 服务
        private IWorkFlowService _workFlowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workFlowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workFlowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workFlowService;
            }
        }
        private IRemoteUserService _remoteUserService = null;
        private IRemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as IRemoteUserService;
                }
                return _remoteUserService;
            }
        }
        private BD.Log.IBll.ILogService _logService = null;
        private BD.Log.IBll.ILogService LogService
        {
            get
            {
                if (_logService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _logService = webApplicationContext.GetObject("LogService") as BD.Log.IBll.ILogService;
                }
                return _logService;
            }
        }
        private BD.IDataInputBll.IDataInputService _dataentryService = null;
        private BD.IDataInputBll.IDataInputService DataEntryService
        {
            get
            {
                if (_dataentryService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataentryService = webApplicationContext.GetObject("DataInputService") as BD.IDataInputBll.IDataInputService;
                }
                return _dataentryService;
            }
        }

        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("WCommonService") as ICommonService;
                }
                return _commonService;
            }
        }
        #endregion
        #region 页面
        /// <summary>
        /// 测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Main()
        {
            return View();
        }
        /// <summary>
        /// 流程分组
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Groups()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpGroups,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogRemarkOpenList,
                Result = true
            };
            LogService.SaveLog(log);

            return View();

        }
        /// <summary>
        /// 表单模板
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Templates()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpTemplates,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogRemarkOpenList,
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// HTNL表单编辑
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TemplateHtmlEdit()
        {
            int templateid = DataFormat.GetSafeInt(Request["id"]);
            if (templateid > 0)
            {
                StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
                if (template != null)
                    ViewBag.Html = Server.HtmlEncode(DataFormat.GetSafeString(template.ContentHtml));
            }
            return View();
        }
        /// <summary>
        /// HTML表单查看
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TemplateHtmlView()
        {
            int templateid = DataFormat.GetSafeInt(Request["id"]);
            if (templateid > 0)
            {
                StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
                if (template != null)
                    ViewBag.Html = DataFormat.GetSafeString(template.ContentHtml);
            }
            return View();
        }

        [Authorize]
        public ActionResult TemplateIWebofficeEdit()
        {
            int templateid = DataFormat.GetSafeInt(Request["id"]);
            string url = "/workflow/GetTemplateOffileFile?templateid=" + templateid;

            PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
            pc.ID = "PageOfficeCtrl1";
            pc.SaveFilePage = "/WorkFlow/SaveTemplateCotentWord?templateid=" + templateid;
            pc.ServerPage = "/pageoffice/server.aspx";
            pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
            pc.Titlebar = false; //隐藏标题栏
            pc.Menubar = false; //隐藏菜单栏
            pc.OfficeToolbars = true; //隐藏Office工具栏
            pc.CustomToolbar = false; //隐藏自定义工具栏

            StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
            FormTemplateType templateType = new FormTemplateType(template.TemplateType);

            System.Web.UI.Page page = new System.Web.UI.Page();
            PageOffice.OpenModeType openMode = PageOffice.OpenModeType.docAdmin;
            if (templateType.HasExcel)
                openMode = PageOffice.OpenModeType.xlsNormalEdit;
            pc.WebOpen(url, openMode, WorkFlowUser.UserName);
            page.Controls.Add(pc);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Server.Execute(page, htw, false);
                    ViewBag.EditorHtml = sb.ToString();
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult TemplateIWebofficeView()
        {
            int templateid = DataFormat.GetSafeInt(Request["id"]);
            if (templateid > 0)
            {
                StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
                if (template != null)
                    ViewBag.Html = DataFormat.GetSafeString(template.ContentHtml);
            }
            return View();
        }
        /// <summary>
        /// 流程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Process()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpProcess,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogRemarkOpenList,
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// 用户能够发起的流程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Workcancreate()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpProcess,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogRemarkOpenList,
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// 流程报表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Reports()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpReport,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogRemarkOpenList,
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// 流水号管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Serials()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpSerial,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogRemarkOpenList,
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// 流程测试
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ProcessTest()
        {
            int processid = DataFormat.GetSafeInt(Request["id"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);
            if (processid > 0)
            {
                ViewStProcess process = WorkFlowService.GetProcess(processid);
                if (process != null)
                {
                    StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(process.FormTemplateid));
                    if (template != null)
                    {
                        ViewBag.Html = WorkFlowService.GetFormatedForm(template.Templateid, serial, 0, "",
                            RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                            "",
                            processid,
                            WorkFlowUser.RealTaskUser, null);
                    }
                }
            }
            return View();
        }
        /// <summary>
        /// 我发起的工作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WorkTodo()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpWorkTodoList,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = "",
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// 我发起的工作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WorkCreate()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpWorkCreateList,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = "",
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        /// <summary>
        /// 我参与的工作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WorkRelate()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpWorkRelateList,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = "",
                Result = true
            };
            LogService.SaveLog(log);
            ViewBag.Username = WorkFlowUser.RealSignUser.UserName;

            return View();
        }
        /// <summary>
        /// 工作托管
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Host()
        {
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpWorkTodoList,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = "",
                Result = true
            };
            LogService.SaveLog(log);

            return View();
        }
        [Authorize]
        public ActionResult ShowImage()
        {
            ViewBag.url = DataFormat.GetSafeString(Request["url"]);
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取菜单
        /// </summary>
        [Authorize]
        public void GetMenus()
        {
            string urls = "[{\"Title\":\"流程分组管理\",\"Url\":\"/workflow/groups\"}," +
                            "{\"Title\":\"表单管理\",\"Url\":\"/workflow/templates\"}," +
                            "{\"Title\":\"报表管理\",\"Url\":\"/workflow/reports\"}," +
                           "{\"Title\":\"流程管理\",\"Url\":\"/workflow/process\"}," +
                           "{\"Title\":\"流水号管理\",\"Url\":\"/workflow/serials\"}," +
                           "{\"Title\":\"发起工作\",\"Url\":\"/workflow/workcancreate\"}," +
                           "{\"Title\":\"待办工作\",\"Url\":\"/workflow/worktodo\"}," +
                           "{\"Title\":\"工作托管\",\"Url\":\"/workflow/host\"}," +
                           "{\"Title\":\"我发起的工作\",\"Url\":\"/workflow/workcreate\"}," +
                           "{\"Title\":\"我参与的工作\",\"Url\":\"/workflow/workrelate\"}," +
                           "{\"Title\":\"日志管理\",\"Url\":\"/log/list\"}]";
            Response.Write(urls);
        }
        /// <summary>
        /// 获取单位人员叔
        /// </summary>
        public void GetUserTree()
        {
            StringBuilder ret = new StringBuilder();

            try
            {
                IList<VCompany> companys = RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager);
                IList<VCompany> deps = RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager);
                IList<VUser> users = RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager);

                ret.Append("[");

                bool firstCompany = true;
                foreach (VCompany company in companys)
                {
                    if (firstCompany)
                        firstCompany = false;
                    else
                        ret.Append(",");
                    ret.Append("{\"id\":\"" + company.CompanyId + "\",\"text\":\"" + company.CompanyName + "\",\"children\":[");
                    bool firstDep = true;
                    foreach (VCompany dep in deps)
                    {
                        if (dep.ParentId != company.CompanyId || dep.CompanyId == company.CompanyId)
                            continue;

                        if (firstDep)
                            firstDep = false;
                        else
                            ret.Append(",");
                        ret.Append("\r\n\t{\"id\":\"" + dep.CompanyId + "\",\"text\":\"" + dep.CompanyName + "\",\"children\":[");

                        bool firstUser = true;
                        foreach (VUser user in users)
                        {
                            if (user.DepartmentId != dep.CompanyId)
                                continue;
                            if (firstUser)
                                firstUser = false;
                            else
                                ret.Append(",");

                            ret.Append("\r\n\t\t{\"id\":\"" + user.UserId + "\",\"text\":\"" + user.UserRealName + "\"}");
                        }
                        ret.Append("]}");
                    }
                    ret.Append("]}");
                }
                ret.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.Clear();
                ret.Append("[]");
            }
            finally
            {
                Response.Write(ret);
            }

        }
        /// <summary>
        /// 获取流程分组
        /// </summary>
        [Authorize]
        public void GetGroups()
        {
            IList<StProcessGroup> items = new List<StProcessGroup>();
            int totalcount = 0;
            try
            {
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
                string companyid = DataFormat.GetSafeString(Request["companyid"]);
                if (companyid == "")
                    companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
                string groupname = DataFormat.GetSafeString(Request["groupname"]);
                string inuse = DataFormat.GetSafeString(Request["inuse"]);


                items = WorkFlowService.GetProcessGroups(companyid, groupname, inuse, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
            }
        }
        /// <summary>
        /// 获取单位的流程分组
        /// </summary>
        [Authorize]
        public void GetCompanyGroups()
        {
            IList<StProcessGroup> groups = new List<StProcessGroup>();
            try
            {
                string companyid = DataFormat.GetSafeString(Request["companyid"]);
                if (companyid == "")
                    companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
                groups = WorkFlowService.GetCompanyProcessGroups(companyid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(groups));
            }
        }
        /// <summary>
        /// 获取单位
        /// </summary>
        [Authorize]
        public void GetCompanys()
        {
            IList<VCompany> companys = new List<VCompany>();
            try
            {
                companys = RemoteUserService.GetFlowCompanys(WorkFlowUser.UserName);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(companys));
            }
        }
        /// <summary>
        /// 获取表单模板
        /// </summary>
        [Authorize]
        public void GetTemplates()
        {
            int totalcount = 0;
            IList<StFormTemplate> items = new List<StFormTemplate>();
            try
            {
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
                string companyid = DataFormat.GetSafeString(Request["companyid"]);
                if (companyid == "")
                    companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
                string templatetype = DataFormat.GetSafeString(Request["templatetype"]);
                string templatename = DataFormat.GetSafeString(Request["templatename"]);


                items = WorkFlowService.GetFormTemplates(companyid, templatetype, templatename, pagesize, pageindex, out totalcount);

                if (items != null)
                {
                    foreach (StFormTemplate item in items)
                    {
                        item.ContentHtml = "";
                        item.ContentWord = null;
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
            }
        }
        /// <summary>
        /// 获取单位的流程模板
        /// </summary>
        [Authorize]
        public void GetCompanyTemplates()
        {
            IList<StFormTemplate> items = new List<StFormTemplate>();
            try
            {
                string companyid = DataFormat.GetSafeString(Request["companyid"]);
                if (companyid == "")
                    companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
                items = WorkFlowService.GetCompanyFormTemplates(companyid);
                if (items != null)
                {
                    foreach (StFormTemplate item in items)
                    {
                        item.ContentHtml = "";
                        item.ContentWord = null;
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(items));
            }
        }
        /// <summary>
        /// 获取html模板
        /// </summary>
        [Authorize]
        public void GetTemplateHtml()
        {
            string ret = "";
            try
            {
                int templateid = DataFormat.GetSafeInt(Request["templateid"]);
                StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
                ret = template == null ? "" : DataFormat.GetSafeString(template.ContentHtml);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取office模板文件
        /// </summary>
        [Authorize]
        public void GetTemplateOffileFile()
        {
            try
            {
                int templateid = DataFormat.GetSafeInt(Request["templateid"]);
                StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(templateid));
                byte[] fileContent = null;
                if (template != null)
                {
                    if (template.ContentWord != null)
                    {
                        fileContent = template.ContentWord;
                    }

                }
                FormTemplateType templateType = new FormTemplateType(template.TemplateType);
                if (fileContent == null)
                {
                    if (templateType.HasWord)
                        fileContent = System.IO.File.ReadAllBytes(Server.MapPath("/workflowtemplates/default.doc"));
                    else
                        fileContent = System.IO.File.ReadAllBytes(Server.MapPath("/workflowtemplates/default.xls"));
                }
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office" + templateType.Ext);
                Response.Charset = "UTF-8";
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(fileContent);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        /// <summary>
        /// 获取Word模板
        /// </summary>
        [Authorize]
        public void TemplateIWebOffice()
        {
            /*
			try
			{
				iMsgServer2015 MsgObj = new iMsgServer2015();
				MsgObj.setSendType("JSON");
				MsgObj.Load(HttpContext.ApplicationInstance.Context.Request);
				string option = MsgObj.GetMsgByName("OPTION");
				string username = MsgObj.GetMsgByName("USERNAME");
				string recordid = MsgObj.GetMsgByName("RECORDID");			
				if (option.Equals("LOADFILE", StringComparison.OrdinalIgnoreCase))
				{
							
					MsgObj.MsgTextClear();
					StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(recordid));
					if (template != null)
					{
						string ext = template.TemplateType == "WORD模板" ? ".doc":".xls";
						bool isLoad = false;
						if ((isLoad = MsgObj.MsgFileLoad(template.ContentWord)) != true)
						{
							isLoad = MsgObj.MsgFileLoad(Server.MapPath("/workflowtemplates/default"+ext));
							
						}
						if (isLoad)
							MsgObj.Send(HttpContext.ApplicationInstance.Context.Response);
					}
				}
				if (option.Equals("SAVEFILE", StringComparison.OrdinalIgnoreCase))
				{
					MsgObj.MsgTextClear();
					StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(recordid));
					if (template != null)
					{
						template.ContentWord = MsgObj.MsgFileBody();

						template = WorkFlowService.SaveFormTemplate(template);

						template.ContentHtml = "";
						template.ContentWord = null;

						BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
						{
							ClientType = WorkFlowConst.LogClientType,
							Ip = ClientInfo.Ip,
							LogTime = DateTime.Now,
							ModuleName = WorkFlowConst.LogModuleName,
							Operation = WorkFlowConst.LogOpTemplateUpdate,
							UserName = WorkFlowUser.UserName,
							RealName = WorkFlowUser.RealName,
							Remark = (new JavaScriptSerializer().Serialize(template)),
							Result = true
						};
						LogService.SaveLog(log);

						Response.Write(JsonClient.GetRetString(0, template));
					}
				}

			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}*/
        }
        /// <summary>
        /// 获取流程
        /// </summary>
        [Authorize]
        public void GetProcess()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string companyid = DataFormat.GetSafeString(Request["companyid"]);
            if (companyid == "")
                companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            string processname = DataFormat.GetSafeString(Request["processname"]);
            string groupid = DataFormat.GetSafeString(Request["groupid"]);
            string zdzdprocess = DataFormat.GetSafeString(Request["zdzdprocess"]);
            string zdzdkey = DataFormat.GetSafeString(Request["zdzdkey"]);
            string inuse = DataFormat.GetSafeString(Request["inuse"]);

            int totalcount = 0;
            IList<ViewStProcess> items = WorkFlowService.GetProcesses(companyid, groupid, processname, zdzdprocess, zdzdkey, inuse, pagesize, pageindex, out totalcount);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
        }
        /// <summary>
        /// 能够发起的流程
        /// </summary>
        [Authorize]
        public void GetMyProcess()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string companyid = DataFormat.GetSafeString(Request["companyid"]);
            if (companyid == "")
                companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            string processname = DataFormat.GetSafeString(Request["processname"]);
            string groupid = DataFormat.GetSafeString(Request["groupid"]);
            string zdzdprocess = DataFormat.GetSafeString(Request["zdzdprocess"]);
            string zdzdkey = DataFormat.GetSafeString(Request["zdzdkey"]);
            string inuse = DataFormat.GetSafeString(Request["inuse"]);

            int totalcount = 0;
            IList<VUserRole> userroles = RemoteUserService.GetUserRoles(null);
            VUser user = RemoteUserService.GetUser(WorkFlowUser.RealSignUser.UserName);
            IList<ViewStProcess> items = WorkFlowService.GetMyProcesses(companyid, groupid, processname, zdzdprocess, zdzdkey, inuse, pagesize, pageindex, out totalcount, user, userroles);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
        }

        /// <summary>
        /// 获取流程报表
        /// </summary>
        [Authorize]
        public void GetReports()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string companyid = DataFormat.GetSafeString(Request["companyid"]);
            if (companyid == "")
                companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            string reportname = DataFormat.GetSafeString(Request["reportname"]);
            string reporturl = DataFormat.GetSafeString(Request["reporturl"]);
            string inuse = DataFormat.GetSafeString(Request["inuse"]);

            int totalcount = 0;
            IList<StReport> items = WorkFlowService.GetReports(companyid, reportname, reporturl, inuse, pagesize, pageindex, out totalcount);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
        }

        /// <summary>
        /// 获取流程流水号
        /// </summary>
        [Authorize]
        public void GetSerials()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string companyid = DataFormat.GetSafeString(Request["companyid"]);
            if (companyid == "")
                companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            string key = DataFormat.GetSafeString(Request["key"]);
            string memo = DataFormat.GetSafeString(Request["memo"]);

            int totalcount = 0;
            IList<StSerial> items = WorkFlowService.GetSerials(companyid, key, memo, pagesize, pageindex, out totalcount);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
        }
        /// <summary>
        /// 获取待办工作列表
        /// </summary>
        [Authorize]
        public void GetWorkTodoList()
        {
            IList<ViewTodoTask> todotasks = new List<ViewTodoTask>();
            int totalcount = 0;
            try
            {
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
                string key = DataFormat.GetSafeString(Request["key"]);
                todotasks = WorkFlowService.GetTodoTasks(WorkFlowUser.RealTaskUser.UserName, key, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(todotasks)));
            }
        }
        /// <summary>
        /// 获取待办工作数量
        /// </summary>
        [Authorize]
        public void GetWorkTodoCount()
        {
            int count = 0;
            try
            {
                count = WorkFlowService.GetTodoTaskCount(WorkFlowUser.RealTaskUser.UserName);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"count\":{0}}}", count));
            }
        }
        /// <summary>
        /// 获取我创建的工作列表
        /// </summary>
        [Authorize]
        public void GetWorkCreateList()
        {
            IList<StForm> createWorks = new List<StForm>();
            int totalcount = 0;
            try
            {
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
                string key = DataFormat.GetSafeString(Request["key"]);
                string state = DataFormat.GetSafeString(Request["state"]);
                createWorks = WorkFlowService.GetMyCreateWorks(WorkFlowUser.RealTaskUser.UserName, state, key, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(createWorks)));
            }
        }
        /// <summary>
        /// 获取我相关的工作列表
        /// </summary>
        [Authorize]
        public void GetWorkRelateList()
        {
            IList<IDictionary<string, string>> createWorks = new List<IDictionary<string, string>>();
            int totalcount = 0;
            try
            {
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
                string key = DataFormat.GetSafeString(Request["key"]);
                string state = DataFormat.GetSafeString(Request["state"]);
                createWorks = WorkFlowService.GetMyRelateWorks(WorkFlowUser.RealTaskUser.UserName, state, key, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(createWorks)));
            }
        }
        /// <summary>
        /// 获取某个任务完成的节点
        /// </summary>
        [Authorize]
        public void GetDoneTasks()
        {
            IList<StDoneTasks> tasks = new List<StDoneTasks>();
            int totalcount = 0;
            try
            {
                string serial = DataFormat.GetSafeString(Request["serial"]);
                tasks = WorkFlowService.GetDoneTasks(serial);
                totalcount = tasks.Count;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(tasks)));
            }
        }
        /// <summary>
        /// 获取待办任务
        /// </summary>
        [Authorize]
        public void GetTodoTasks()
        {
            IList<StToDoTasks> tasks = new List<StToDoTasks>();
            int totalcount = 0;
            try
            {
                string serial = DataFormat.GetSafeString(Request["serial"]);
                tasks = WorkFlowService.GetTodoTasks(serial);
                totalcount = tasks.Count;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(tasks)));
            }
        }
        /// <summary>
        /// 获取协助办理的已完成任务
        /// </summary>
        [Authorize]
        public void GetAssistDoneTasks()
        {
            IList<StDoneTasks> tasks = new List<StDoneTasks>();
            int totalcount = 0;
            try
            {
                string serial = DataFormat.GetSafeString(Request["serial"]);
                tasks = WorkFlowService.GetSubDoneTasks(serial);
                totalcount = tasks.Count;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(tasks)));
            }
        }
        /// <summary>
        /// 获取协助办理的待办任务
        /// </summary>
        [Authorize]
        public void GetAssistTodoTasks()
        {
            IList<StToDoTasks> tasks = new List<StToDoTasks>();
            int totalcount = 0;
            try
            {
                string serial = DataFormat.GetSafeString(Request["serial"]);
                tasks = WorkFlowService.GetSubTodoTasks(serial);
                totalcount = tasks.Count;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(tasks)));
            }
        }

        #endregion

        #region 数据操作
        public void SetLoginUser()
        {
            try
            {
                //Request.ContentEncoding = Encoding.UTF8;
                //string username = DataFormat.GB2312ToUTF8(DataFormat.GetSafeString(Request["username"]));
                System.Collections.Specialized.NameValueCollection parames = HttpUtility.ParseQueryString(Request.Url.Query, Encoding.UTF8);
                string username = parames["username"];
                //string username = DataFormat.GetSafeString(Request["username"]);
                VUser vuser = RemoteUserService.GetUser(username);
                if (vuser != null)
                {
                    VCompany company = RemoteUserService.GetDepartment(vuser.CompanyId);
                    VCompany dep = RemoteUserService.GetDepartment(vuser.DepartmentId);
                    SessionUser user = new SessionUser()
                    {
                        UserName = vuser.UserId,
                        RealName = vuser.UserRealName,
                        CompanyId = vuser.CompanyId,
                        CompanyName = company.CompanyName,
                        DepartmentId = vuser.DepartmentId,
                        DepartmentName = dep.CompanyName,
                        DutyLevel = "2",
                    };

                    BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(user, null);
                    /*
                    UserManager.UserMgr.USERCODE = vuser.UserCode;
                    UserManager.UserMgr.USERNAME = user.UserName;
                    UserManager.UserMgr.REALNAME = user.RealName;
                    UserManager.UserMgr.CPCODE = user.CompanyId;
                    UserManager.UserMgr.CPNAME = user.CompanyName;
                    UserManager.UserMgr.DEPCODE = user.DepartmentId;
                    UserManager.UserMgr.DEPNAME = user.DepartmentName;*/
                    // 设置录入界面用户
                    Session["USERCODE"] = vuser.UserCode;
                    Session["USERNAME"] = user.UserName;
                    Session["REALNAME"] = user.RealName;
                    Session["CPCODE"] = user.CompanyId;
                    Session["CPNAME"] = user.CompanyName;
                    Session["DEPCODE"] = user.DepartmentId;
                    Session["DEPNAME"] = user.DepartmentName;

                    System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName, false);
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonClient.GetRetString());
            }
        }

        /// <summary>
        /// 删除流程分组
        /// </summary>		
        [Authorize]
        public void DeleteGroup()
        {
            int groupid = DataFormat.GetSafeInt(Request["id"]);
            StProcessGroup group = WorkFlowService.GetProcessGroup(groupid);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpGroupsDel,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(group)),
                Result = true
            };
            LogService.SaveLog(log);

            WorkFlowService.DeleteProcessGroup(groupid);
            Response.Write(JsonClient.GetRetString(true));
        }
        /// <summary>
        /// 保存分组
        /// </summary>
        [Authorize]
        public void SaveGroup()
        {
            bool isnew = true;
            StProcessGroup group = new StProcessGroup();
            group.Groupid = DataFormat.GetSafeInt(Request["groupid"]);
            if (group.Groupid > 0)
            {
                group = WorkFlowService.GetProcessGroup(group.Groupid);
                isnew = false;
            }
            group.GroupName = DataFormat.GetSafeString(Request["groupname"]);
            group.CompanyId = DataFormat.GetSafeString(Request["companyid"]);
            group.CompanyName = DataFormat.GetSafeString(Request["companyname"]);
            group.InUse = DataFormat.GetSafeBool(Request["inuse"]);
            if (isnew)
            {
                group.UserId = WorkFlowUser.UserName;
                group.UserRealName = WorkFlowUser.RealName;
                group.CreateTime = DateTime.Now;
            }

            group = WorkFlowService.SaveProcessGroup(group);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = isnew ? WorkFlowConst.LogOpGroupAdd : WorkFlowConst.LogOpGroupUpdate,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(group)),
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString(0, group));
        }

        /// <summary>
        /// 删除表单模板
        /// </summary>		
        [Authorize]
        public void DeleteTemplate()
        {
            int templateid = DataFormat.GetSafeInt(Request["id"]);
            StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);

            template.ContentHtml = "";
            template.ContentWord = null;
            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpTemplateDel,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(template)),
                Result = true
            };
            LogService.SaveLog(log);

            WorkFlowService.DeleteFormTemplate(templateid);
            Response.Write(JsonClient.GetRetString(true));
        }
        /// <summary>
        /// 保存表单模板
        /// </summary>
        [Authorize]
        public void SaveTemplate()
        {
            bool isnew = true;
            StFormTemplate template = new StFormTemplate();
            template.Templateid = DataFormat.GetSafeInt(Request["workflow_templates_edit_info_id"]);
            if (template.Templateid > 0)
            {
                template = WorkFlowService.GetFormTemplate(template.Templateid);
                isnew = false;
            }
            template.TemplateType = DataFormat.GetSafeString(Request["workflow_templates_edit_info_html"]) + "," +
                DataFormat.GetSafeString(Request["workflow_templates_edit_info_office"]);
            template.TemplateName = DataFormat.GetSafeString(Request["workflow_templates_edit_info_templatename"]);
            template.CompanyId = DataFormat.GetSafeString(Request["workflow_templates_edit_info_companyid"]);
            template.CompanyName = DataFormat.GetSafeString(Request["workflow_templates_edit_info_companyname"]);
            template.DisplayStepType = DataFormat.GetSafeString(Request["workflow_templates_edit_info_displaysteptype"]);
            if (isnew)
            {
                template.UserId = WorkFlowUser.UserName;
                template.UserRealName = WorkFlowUser.RealName;
                template.CreateTime = DateTime.Now;
            }

            template = WorkFlowService.SaveFormTemplate(template);

            template.ContentHtml = "";
            template.ContentWord = null;

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = isnew ? WorkFlowConst.LogOpTemplateAdd : WorkFlowConst.LogOpTemplateUpdate,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(template)),
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString(0, template));
        }
        /// <summary>
        /// 保存html模板
        /// </summary>
        [Authorize]
        public void SaveTemplateContentHtml()
        {
            int templateid = DataFormat.GetSafeInt(Request["workflow_templates_edit_id"]);
            string html = DataFormat.GetSafeString(Request["workflow_templates_edit_html"]);

            StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
            template.ContentHtml = DataFormat.DecodeBase64(Encoding.GetEncoding("GB2312"), html);

            WorkFlowService.SaveFormTemplate(template);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpTemplateUpdate,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = WorkFlowConst.LogOpTemplateRemarkSaveHtml,
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString());
        }
        /// <summary>
        /// 保存word模板
        /// </summary>
        [Authorize]
        public void SaveTemplateCotentWord()
        {
            string msg = "";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {
                int templateid = DataFormat.GetSafeInt(Request["templateid"]);


                byte[] file = fs.FileBytes;

                StFormTemplate template = WorkFlowService.GetFormTemplate(templateid);
                template.ContentWord = file;

                WorkFlowService.SaveFormTemplate(template);

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpTemplateUpdate,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = WorkFlowConst.LogOpTemplateRemarkSaveWord,
                    Result = true
                };
                LogService.SaveLog(log);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = "保存异常，异常信息：" + e.Message;
            }
            finally
            {
                fs.CustomSaveResult = msg;
                fs.Close();
            }
        }
        /// <summary>
        /// 删除流程
        /// </summary>		
        [Authorize]
        public void DeleteProcess()
        {
            int processid = DataFormat.GetSafeInt(Request["id"]);
            ViewStProcess process = WorkFlowService.GetProcess(processid);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpProcessDel,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(process)),
                Result = true
            };
            LogService.SaveLog(log);

            WorkFlowService.DeleteProcess(processid);
            Response.Write(JsonClient.GetRetString(true));
        }
        /// <summary>
        /// 保存流程
        /// </summary>
        [Authorize]
        public void SaveProcess()
        {
            bool isnew = true;
            StProcess process = new StProcess();
            process.Processid = DataFormat.GetSafeInt(Request["processid"]);
            isnew = process.Processid == 0;
            process.ProcessName = DataFormat.GetSafeString(Request["processname"]);
            process.ProcessDesc = DataFormat.GetSafeString(Request["processdesc"]);
            process.ProcessType = DataFormat.GetSafeInt(Request["processtype"]);
            process.Groupid = DataFormat.GetSafeInt(Request["groupid"]);
            process.FormTemplateid = DataFormat.GetSafeInt(Request["formtemplateid"]);
            process.UniqueType = 0;// DataFormat.GetSafeInt(Request["uniquetype"]);
            process.DispCompState = DataFormat.GetSafeBool(Request["dispcompstate"]);
            process.UseInPhone = DataFormat.GetSafeInt(Request["useinphone"]);
            process.PhoneTemplateid = DataFormat.GetSafeInt(Request["phonetemplateid"]);
            process.CreateInPhone = DataFormat.GetSafeInt(Request["createinphone"]);
            process.BusynessInfo1 = DataFormat.GetSafeString(Request["busynessinfo1"]);
            process.BusynessInfo2 = DataFormat.GetSafeString(Request["busynessinfo2"]);
            process.CompanyId = DataFormat.GetSafeString(Request["companyid"]);
            process.InUse = DataFormat.GetSafeBool(Request["inuse"]);
            process.PreListUrl = DataFormat.GetSafeString(Request["prelisturl"]);
            process.PreWhere = DataFormat.GetSafeString(Request["prewhere"]);
            process.CompanyName = DataFormat.GetSafeString(Request["companyname"]);
            process.ZdzdProcess = DataFormat.GetSafeBool(Request["zdzdprocess"]);
            process.ZdzdKey = DataFormat.GetSafeString(Request["zdzdkey"]);
            process.SubProcess = DataFormat.GetSafeBool(Request["subprocess"]);
            process.FixProcess = DataFormat.GetSafeBool(Request["fixprocess"]);
            process.AfterPostFunc = DataFormat.GetSafeString(Request["afterpostfunc"]);
            process.BeforeCancelProc = DataFormat.GetSafeString(Request["beforecancelproc"]);
            if (isnew)
            {
                process.UserId = WorkFlowUser.UserName;
                process.UserRealName = WorkFlowUser.RealName;
                process.CreateTime = DateTime.Now;
            }
            else
            {
                ViewStProcess view = WorkFlowService.GetProcess(process.Processid);
                process.UserId = view.UserId;
                process.UserRealName = view.UserRealName;
                process.CreateTime = view.CreateTime;
            }


            ViewStProcess vprocess = WorkFlowService.SaveProcess(process);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = isnew ? WorkFlowConst.LogOpProcessAdd : WorkFlowConst.LogOpProcessUpdate,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(vprocess)),
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString(0, vprocess));
        }

        /// <summary>
        /// 删除流程报表
        /// </summary>		
        [Authorize]
        public void DeleteReport()
        {
            int reportid = DataFormat.GetSafeInt(Request["id"]);
            StReport report = WorkFlowService.GetReport(reportid);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpReportDel,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(report)),
                Result = true
            };
            LogService.SaveLog(log);

            WorkFlowService.DeleteReport(reportid);
            Response.Write(JsonClient.GetRetString(true));
        }
        /// <summary>
        /// 保存分组
        /// </summary>
        [Authorize]
        public void SaveReport()
        {
            bool isnew = true;
            StReport report = new StReport();
            report.ReportId = DataFormat.GetSafeInt(Request["reportid"]);
            if (report.ReportId > 0)
            {
                report = WorkFlowService.GetReport(report.ReportId);
                isnew = false;
            }
            report.ReportName = DataFormat.GetSafeString(Request["reportname"]);
            report.ReportUrl = DataFormat.GetSafeString(Request["reporturl"]);
            report.CompanyId = DataFormat.GetSafeString(Request["companyid"]);
            report.CompanyName = DataFormat.GetSafeString(Request["companyname"]);
            report.InUse = DataFormat.GetSafeBool(Request["inuse"]);
            if (isnew)
            {
                report.UserId = WorkFlowUser.UserName;
                report.UserRealName = WorkFlowUser.RealName;
                report.CreateTime = DateTime.Now;
            }

            report = WorkFlowService.SaveReport(report);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = isnew ? WorkFlowConst.LogOpReportAdd : WorkFlowConst.LogOpReportUpdate,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(report)),
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString(0, report));
        }
        /// <summary>
        /// 删除流程流水号
        /// </summary>		
        [Authorize]
        public void DeleteSerial()
        {
            int recid = DataFormat.GetSafeInt(Request["id"]);
            StSerial serial = WorkFlowService.GetSerial(recid);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpSerialDel,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(serial)),
                Result = true
            };
            LogService.SaveLog(log);

            WorkFlowService.DeleteSerial(recid);
            Response.Write(JsonClient.GetRetString(true));
        }
        /// <summary>
        /// 保存分组
        /// </summary>
        [Authorize]
        public void SaveSerial()
        {
            bool isnew = true;
            StSerial serial = new StSerial();
            serial.Recid = DataFormat.GetSafeInt(Request["recid"]);
            if (serial.Recid > 0)
            {
                serial = WorkFlowService.GetSerial(serial.Recid);
                isnew = false;
            }
            else
                serial.CurMaxSerial = "";
            serial.SerialKey = DataFormat.GetSafeString(Request["serialkey"]);
            serial.SerialModule = DataFormat.GetSafeString(Request["SerialModule"]);
            serial.CompanyId = DataFormat.GetSafeString(Request["companyid"]);
            serial.CompanyName = DataFormat.GetSafeString(Request["companyname"]);
            serial.Memo = DataFormat.GetSafeString(Request["Memo"]);

            serial = WorkFlowService.SaveSerial(serial);

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = isnew ? WorkFlowConst.LogOpSerialAdd : WorkFlowConst.LogOpSerialUpdate,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(serial)),
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString(0, serial));
        }

        /// <summary>
        /// 复制流程
        /// </summary>
        [Authorize]
        public void CopyProcess()
        {
            int processid = DataFormat.GetSafeInt(Request["id"]);
            string processName = DataFormat.GetSafeString(Request["processname"]);
            ViewStProcess vprocess = WorkFlowService.CopyProcess(processid, processName, WorkFlowUser.UserName, WorkFlowUser.RealName);


            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpProcessCopy,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = (new JavaScriptSerializer().Serialize(vprocess)),
                Result = true
            };
            LogService.SaveLog(log);

            Response.Write(JsonClient.GetRetString(0, vprocess));
        }

        /// <summary>
        /// 撤销工作
        /// </summary>		
        [Authorize]
        public void CancelTask()
        {
            string serial = DataFormat.GetSafeString(Request["serial"]);


            string err = "";
            bool ret = false;
            try
            {
                ret = WorkFlowService.CancelWork(serial, WorkFlowUser.RealTaskUser, out err);

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpCancelWork,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = "撤销任务:" + serial + "，结果：" + ret + "，其他信息：" + err,
                    Result = true
                };
                LogService.SaveLog(log);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, err));
            }
        }
        /// <summary>
        /// 自动撤销任务，用于催办撤销接口
        /// </summary>
        [Authorize]
        public void AutoCancelTask()
        {
            string serial = DataFormat.GetSafeString(Request["serial"]);


            string err = "";
            bool ret = false;
            try
            {
                ret = WorkFlowService.CancelWork(serial, out err);

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpCancelWork,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = "自动撤销任务:" + serial + "，结果：" + ret + "，其他信息：" + err,
                    Result = true
                };
                LogService.SaveLog(log);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, err));
            }
        }
        /// <summary>
        /// 撤销
        /// </summary>		
        [Authorize]
        public void RollTask()
        {

            int taskid = DataFormat.GetSafeInt(Request["taskid"], 0);



            string err = "";
            bool ret = WorkFlowService.RollWork(taskid, WorkFlowUser.RealTaskUser, out err);
            try
            {
                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpCancelWork,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = "回退任务:" + taskid + "，结果：" + ret + "，其他信息：" + err,
                    Result = true
                };
                LogService.SaveLog(log);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, err));
            }
        }


        /// <summary>
        /// 获取用户相关节点
        /// </summary>
        [Authorize]
        public void GetUserActivity()
        {
            string ret = "";
            try
            {

                IList<VUserRole> userroles = RemoteUserService.GetUserRoles(null);
                VUser user = RemoteUserService.GetUser(WorkFlowUser.RealSignUser.UserName);
                string companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
                IList<ViewStProcess> processes = WorkFlowService.GetCompanyProcesses(companyid);
                foreach (ViewStProcess process in processes)
                {
                    string tempret = "";
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(process.Processid);
                    IList<StActivity> activities = flow.ActivityCol;
                    foreach (StActivity activity in activities)
                    {
                        if (activity.Equals(flow.RealStartActivity))
                            continue;

                        string tempret2 = "";
                        IList<StAssignRule> assignRules = flow.GetActivityAssignRule(activity.Activityid);
                        foreach (StAssignRule rule in assignRules)
                        {

                            if (DataFormat.GetSafeInt(rule.BasedOn) == (int)EnumExecutorBasedOn.DEPT_BASED)
                            {
                                if (WorkFlowUser.DepartmentId.Equals(rule.Executor, StringComparison.OrdinalIgnoreCase))
                                {
                                    tempret2 = "{\"id\":\"" + activity.Activityid + "\",\"text\":\"" + activity.ActivityName + "\",\"leaf\":\"true\"}";
                                }
                            }
                            else if (DataFormat.GetSafeInt(rule.BasedOn) == (int)EnumExecutorBasedOn.USER_BASED)
                            {
                                if (WorkFlowUser.RealSignUser.UserName.Equals(rule.Executor, StringComparison.OrdinalIgnoreCase))
                                {
                                    tempret2 = "{\"id\":\"" + activity.Activityid + "\",\"text\":\"" + activity.ActivityName + "\",\"leaf\":\"true\"}";
                                }
                            }
                            else if (DataFormat.GetSafeInt(rule.BasedOn) == (int)EnumExecutorBasedOn.ROLE_BASED)
                            {
                                var q = from e in userroles where e.RoleCode.Equals(rule.Executor, StringComparison.OrdinalIgnoreCase) select e.UserCode;
                                IList<string> usercodes = q.ToList<string>();
                                if (usercodes.Contains(user.UserCode))
                                {
                                    tempret2 = "{\"id\":\"" + activity.Activityid + "\",\"text\":\"" + activity.ActivityName + "\",\"leaf\":\"true\"}";
                                    //考虑这里改成treegrid 的数据，参考样式{}
                                }
                            }
                            if (tempret2 != "")
                                break;
                        }
                        if (tempret2 != "")
                        {
                            if (tempret != "")
                                tempret += ",";
                            tempret += tempret2;
                            //break;
                        }
                    }
                    if (tempret != "")
                    {
                        if (ret != "")
                            ret += ",";
                        ret = ret + "{\"id\":\"p" + process.Processid + "\",\"text\":\"" + process.ProcessName + "\",\"leaf\":\"false\",\"children\":[" + tempret + "]}";
                    }

                }
                /*
                string teproles = "";
                IList<VUserRole> roles = RemoteUserService.GetUserRoles(null);
                foreach (VUserRole role in roles)
                {
                    VUser user=RemoteUserService.GetUser(WorkFlowUser.RealSignUser.UserName);
                    if (role.UserCode == user.UserCode)
                    {
                        if (teproles != "")
                            teproles += ",";
                        teproles += "'" + role.RoleCode + "'";
                    }
                }
                */

                //IList<StActivity> activities = WorkFlowService.GetActivitiesByUser(WorkFlowUser.RealSignUser.UserName, teproles, WorkFlowUser.c.DepartmentId);
                //ret = JsonClient.GetRetString(0, activities);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                ret = "[" + ret + "]";
                Response.Write(ret);
            }
        }

        /// <summary>
        /// 是否托管中
        /// </summary>
        [Authorize]
        public void CheckHost()
        {
            int activityid = DataFormat.GetSafeInt(Request["activityid"], 0);
            string ret = "";
            try
            {
                StHost host = WorkFlowService.GetActivityHost(activityid, WorkFlowUser.RealSignUser.UserName);
                if (host != null)
                    ret = "{\"date1\":\"" + host.StartTime.Value.ToString() + "\",\"date2\":\"" + host.EndTime.Value.ToString() + "\",\"person\":\"" + host.Hosted + "\",\"type\":\"2\"}";
                else
                    ret = "{\"date1\":\"\",\"date2\":\"\",\"person\":\"\",\"type\":\"1\"}";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        [Authorize]
        public void SetHost()
        {


            int activityid = DataFormat.GetSafeInt(Request["activityid"], 0);
            DateTime starttime = DataFormat.GetSafeDate(Request["date1"], DateTime.Now);
            DateTime endtime = DataFormat.GetSafeDate(Request["date2"], DateTime.Now);
            string hostedusername = DataFormat.GetSafeString(Request["hosted"], "");
            string ret = "";
            try
            {
                StActivity activity = WorkFlowService.GetActivity(activityid);
                VUser hosted = RemoteUserService.GetUser(hostedusername);
                WorkFlowService.SetActivityHost(activity, hosted, starttime, endtime);
                ret = JsonClient.GetRetString(true);

            }
            catch (Exception e)
            {
                ret = JsonClient.GetRetString(false, e.Message);
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        [Authorize]
        public void CancelHost()
        {
            int activityid = DataFormat.GetSafeInt(Request["activityid"], 0);
            string ret = "";
            try
            {
                WorkFlowService.CancelHost(activityid, WorkFlowUser.RealSignUser.UserName);
                ret = JsonClient.GetRetString(true);

            }
            catch (Exception e)
            {
                ret = JsonClient.GetRetString(false, e.Message);
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }

        #endregion

        #region 流程设计器
        /// <summary>
        /// 获取流程定义
        /// </summary>
        public void GetFlowDefine()
        {
            string key = DataFormat.GetSafeString(Request["key"]);
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            try
            {
                int processid = DataFormat.GetSafeInt(key);
                VOrderFlow orderFlow = WorkFlowService.GetOrderFlow(processid);
                if (orderFlow.IsValid)
                {
                    // 根元素
                    XElement eleProcess = new XElement("processes",
                        new XAttribute("key", orderFlow.Process.Processid),
                        new XAttribute("name", orderFlow.Process.ProcessName),
                        new XAttribute("version", "1.0"),
                        new XAttribute("description", orderFlow.Process.ProcessDesc));
                    // 开始节点
                    if (orderFlow.StartActivity != null && orderFlow.StartActivity.ActivityId > 0)
                    {
                        XElement eleStart = new XElement("start",
                            new XAttribute("g", orderFlow.StartActivity.Position),
                            new XAttribute("name", orderFlow.StartActivity.ActivityName),
                            new XAttribute("id", orderFlow.StartActivity.ActivityGraphId),
                            new XAttribute("startConProcName", ""));
                        eleProcess.Add(eleStart);
                        // 到真正开始节点的线条
                        StActivity realStartActivity = orderFlow.RealStartActivity;
                        if (realStartActivity != null)
                        {
                            XElement eleToRealStart = new XElement("transition",
                                new XAttribute("name", orderFlow.StartActivity.TransGraphName),
                                new XAttribute("g", orderFlow.StartActivity.TransPosition),
                                new XAttribute("id", orderFlow.StartActivity.TransGraphId),
                                new XAttribute("to", realStartActivity.ActivityGraphId));
                            eleStart.Add(eleToRealStart);
                        }
                    }
                    // region 其他节点
                    int activityNum = 1;
                    foreach (StActivity activity in orderFlow.ActivityCol)
                    {
                        // 经办人信息
                        string roles = "", users = "", deps = "";
                        IList<StAssignRule> assignRules = orderFlow.GetActivityAssignRule(activity.Activityid);
                        foreach (StAssignRule assignRule in assignRules)
                        {
                            if (assignRule.BasedOn == (int)EnumExecutorBasedOn.DEPT_BASED)
                            {
                                if (deps != "")
                                    deps += "|";
                                deps += assignRule.Executor + "$" + assignRule.ExecutorName;
                            }
                            else if (assignRule.BasedOn == (int)EnumExecutorBasedOn.ROLE_BASED)
                            {
                                if (roles != "")
                                    roles += "|";
                                roles += assignRule.Executor + "$" + assignRule.ExecutorName;
                            }
                            else if (assignRule.BasedOn == (int)EnumExecutorBasedOn.USER_BASED)
                            {
                                if (users != "")
                                    users += "|";
                                users += assignRule.Executor + "$" + assignRule.ExecutorName;
                            }
                        }
                        // 活动信息
                        XElement ele = new XElement("task",
                            new XAttribute("activityid", activity.Activityid),
                            new XAttribute("id", activity.ActivityGraphId),
                            new XAttribute("name", activity.ActivityName),
                            new XAttribute("autoExecFlag", activity.ActivityType == (int)EnumActivityType.COMPLETION ? "true" : "false"),
                            new XAttribute("sameDeptNextAssignee", DataFormat.GetSafeBool(activity.LimitedSameGroup).ToString().ToLower()),
                            new XAttribute("nextAssignee", DataFormat.GetSafeBool(activity.PermitSelectUser).ToString().ToLower()),
                            new XAttribute("userExecType", activity.ExecuteType == (int)EnumExecutorType.ANY ? "1" : "2"),
                            new XAttribute("print", DataFormat.GetSafeBool(activity.IsPrint).ToString().ToLower()),
                            new XAttribute("printContent", activity.ReportName),
                            new XAttribute("conProcName", activity.PreEnterFunc),
                            new XAttribute("submitConProcName", activity.PrePostFunc),
                            new XAttribute("execProcNames", activity.AfterPostFunc),
                            new XAttribute("execType", "2"),
                            new XAttribute("execSqls", ""),
                            new XAttribute("newTask", DataFormat.GetSafeBool(activity.IsCreateStream).ToString().ToLower()),
                            new XAttribute("subProcessId", activity.Streamid),
                            new XAttribute("subProcessTaskName", activity.StreamSelect),
                            new XAttribute("hideSubProcess", DataFormat.GetSafeBool(activity.StreamHidden).ToString().ToLower()),
                            new XAttribute("execAllTask", DataFormat.GetSafeBool(activity.NextStepAllExec).ToString().ToLower()),
                            new XAttribute("alias", activity.AliasName),
                            new XAttribute("order", activityNum++.ToString()),
                            new XAttribute("hideApproval", DataFormat.GetSafeBool(activity.HiddenOpinion).ToString().ToLower()),
                            new XAttribute("hideNextSelect", DataFormat.GetSafeBool(activity.HiddenNextStep).ToString().ToLower()),
                            new XAttribute("nextDefaultUser", activity.DefaultUser),
                            new XAttribute("form", activity.GotoUrl),
                            new XAttribute("append", DataFormat.GetSafeBool(activity.UploadAttach).ToString().ToLower()),
                            new XAttribute("requestAppend", DataFormat.GetSafeBool(activity.MustUploadAttach).ToString().ToLower()),
                            new XAttribute("g", activity.Position),
                            new XAttribute("candidateUsers", users),
                            new XAttribute("candidateDepts", deps),
                            new XAttribute("candidateRoles", roles),
                            new XAttribute("workorder", DataFormat.GetSafeInt(activity.WorkOrder)),
                            new XAttribute("formUrl", DataFormat.GetSafeString(activity.FormUrl)),
                            new XAttribute("deadLine", DataFormat.GetSafeBool(activity.Deadline).ToString().ToLower()),
                            new XAttribute("deadLineCancel", DataFormat.GetSafeBool(activity.Deadlinecancel).ToString().ToLower()),
                            new XAttribute("deadLineAlert", DataFormat.GetSafeBool(activity.DeadlineAlert).ToString().ToLower()),
                            new XAttribute("deadLineTime", DataFormat.GetSafeDecimal(activity.DeadlineTime, 12))
                            );

                        // 到下一个节点的线条
                        IList<StRoutingRule> routingRules = orderFlow.GetActivityRoutingRule(activity.Activityid);
                        if (routingRules != null)
                        {
                            foreach (StRoutingRule routingRule in routingRules)
                            {
                                if (routingRule.TransPosition.Length == 0 ||
                                    routingRule.TransGraphId.Length == 0 ||
                                    routingRule.TransGraphName.Length == 0 ||
                                    routingRule.NextActivityidList.Length == 0)
                                    continue;
                                string[] arrTransPosition = routingRule.TransPosition.Split(new char[] { '|' });
                                string[] arrTransGraphid = routingRule.TransGraphId.Split(new char[] { '|' });
                                string[] arrTransGraphname = routingRule.TransGraphName.Split(new char[] { '|' });
                                string[] arrNextIds = routingRule.NextActivityidList.Split(new char[] { ',' });
                                for (int j = 0; j < arrTransPosition.Length && j < arrTransGraphid.Length && j < arrTransGraphname.Length && j < arrNextIds.Length; j++)
                                {
                                    StActivity tmpNextActivity = orderFlow.GetActivity(DataFormat.GetSafeInt(arrNextIds[j]));
                                    XElement eleTrans = new XElement("transition",
                                        new XAttribute("id", arrTransGraphid[j]),
                                        new XAttribute("name", arrTransGraphname[j]),
                                        new XAttribute("g", arrTransPosition[j]),
                                        new XAttribute("to", tmpNextActivity == null ? "" : tmpNextActivity.ActivityGraphId));
                                    ele.Add(eleTrans);
                                }
                            }
                        }
                        // 添加到process节
                        eleProcess.Add(ele);

                    }
                    // 加载到xml文件
                    doc.Add(eleProcess);
                }
                doc.Save(Response.OutputStream);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(doc.ToString());
                SysLog4.WriteLog(e);
            }


        }
        /// <summary>
        /// 保存流程定义
        /// </summary>
        public void SaveFlowDefine()
        {
            XDocument doc = XDocument.Load(Request.InputStream);

            string ret = "OK";
            StringBuilder sb = new StringBuilder();
            try
            {
                XElement root = doc.Root;
                int processid = DataFormat.GetSafeInt(root.Attribute("key").Value);
                if (processid > 0)
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);

                    string firstActivityId = "";
                    // 解析开始节点
                    XElement startnode = root.Element("start");
                    StStartActivity startActivity = flow.StartActivity;	// 开始节点
                    if (startActivity == null)
                        startActivity = new StStartActivity();
                    startActivity.Processid = processid;
                    startActivity.ActivityName = DataFormat.GetSafeString(startnode.Attribute("name").Value);
                    startActivity.Position = DataFormat.GetSafeString(startnode.Attribute("g").Value);
                    startActivity.ActivityGraphId = DataFormat.GetSafeString(startnode.Attribute("id").Value);
                    startActivity.TransPosition = "";
                    startActivity.TransGraphId = "";
                    XElement transNode = startnode.Element("transition");
                    if (transNode != null)
                    {
                        startActivity.TransPosition = DataFormat.GetSafeString(transNode.Attribute("g").Value);
                        startActivity.TransGraphId = DataFormat.GetSafeString(transNode.Attribute("id").Value);
                        startActivity.TransGraphName = DataFormat.GetSafeString(transNode.Attribute("name").Value);
                        firstActivityId = DataFormat.GetSafeString(transNode.Attribute("to").Value);
                    }
                    // 其他节点
                    IList<VStActivity> activitys = new List<VStActivity>();
                    foreach (XElement ele in root.Elements())
                    {
                        // 去掉开始节点
                        if (!ele.Name.LocalName.Equals("task", StringComparison.OrdinalIgnoreCase))
                            continue;
                        string graphyid = DataFormat.GetSafeString(ele.Attribute("id").Value);
                        StActivity activity = flow.GetActivity(graphyid);
                        if (activity == null)
                            activity = new StActivity();
                        //StActivity activity = new StActivity();

                        activity.Processid = processid;
                        activity.ActivityName = DataFormat.GetSafeString(ele.Attribute("name").Value);
                        activity.TimeAllowed = 0;
                        activity.RuleApplied = 0;
                        activity.ExPreRuleFunction = "";
                        activity.ExPostRuleFunction = "";
                        activity.ActivityType = DataFormat.GetSafeBool(ele.Attribute("autoExecFlag").Value) ? (int)EnumActivityType.COMPLETION : (int)EnumActivityType.INTER_ACTION;
                        activity.OrMergeFlag = 0;
                        activity.NumVotesNeeded = 0;
                        activity.AutoExecutive = 0;
                        activity.ActivityDesc = "";
                        activity.PermitSelectUser = DataFormat.GetSafeBool(ele.Attribute("nextAssignee").Value);
                        activity.LimitedSameGroup = DataFormat.GetSafeBool(ele.Attribute("sameDeptNextAssignee").Value);
                        activity.Method = 1;
                        activity.ExecuteType = DataFormat.GetSafeInt(ele.Attribute("userExecType").Value) == (int)EnumExecutorType.ANY ? (int)EnumExecutorType.ANY : (int)EnumExecutorType.ALL_BRANCH;
                        activity.IsPrint = DataFormat.GetSafeBool(ele.Attribute("print").Value);
                        activity.ReportName = DataFormat.GetSafeString(ele.Attribute("printContent").Value);
                        activity.PreEnterFunc = DataFormat.GetSafeString(ele.Attribute("conProcName").Value);
                        activity.PrePostFunc = DataFormat.GetSafeString(ele.Attribute("submitConProcName").Value);
                        activity.AfterPostFunc = DataFormat.GetSafeString(ele.Attribute("execProcNames").Value);
                        activity.IsCreateStream = DataFormat.GetSafeBool(ele.Attribute("newTask").Value);
                        activity.Streamid = DataFormat.GetSafeString(ele.Attribute("subProcessId").Value);
                        activity.StreamSelect = DataFormat.GetSafeString(ele.Attribute("subProcessTaskName").Value);
                        activity.StreamHidden = DataFormat.GetSafeBool(ele.Attribute("hideSubProcess").Value);
                        activity.NextStepAllExec = DataFormat.GetSafeBool(ele.Attribute("execAllTask").Value);
                        activity.AliasName = DataFormat.GetSafeString(ele.Attribute("alias").Value);
                        activity.HiddenOpinion = DataFormat.GetSafeBool(ele.Attribute("hideApproval").Value);
                        activity.HiddenNextStep = DataFormat.GetSafeBool(ele.Attribute("hideNextSelect").Value);
                        activity.DefaultUser = DataFormat.GetSafeString(ele.Attribute("nextDefaultUser").Value);
                        activity.GotoUrl = DataFormat.GetSafeString(ele.Attribute("form").Value);
                        activity.UploadAttach = DataFormat.GetSafeBool(ele.Attribute("append").Value);
                        activity.MustUploadAttach = DataFormat.GetSafeBool(ele.Attribute("requestAppend").Value);
                        activity.Position = DataFormat.GetSafeString(ele.Attribute("g").Value);
                        activity.ActivityGraphId = DataFormat.GetSafeString(ele.Attribute("id").Value);
                        activity.WorkOrder = DataFormat.GetSafeShort(ele.Attribute("workorder").Value);
                        activity.FormUrl = DataFormat.GetSafeString(ele.Attribute("formUrl").Value);
                        activity.Deadline = DataFormat.GetSafeBool(ele.Attribute("deadLine").Value);
                        activity.DeadlineAlert = DataFormat.GetSafeBool(ele.Attribute("deadLineCancel").Value);
                        activity.Deadlinecancel = DataFormat.GetSafeBool(ele.Attribute("deadLineAlert").Value);
                        activity.DeadlineTime = DataFormat.GetSafeDecimal(ele.Attribute("deadLineTime").Value, 12);

                        VStActivity vactivity = new VStActivity(activity);
                        activitys.Add(vactivity);
                        // 保存节点的用户角色部门
                        IList<StAssignRule> assignRules = new List<StAssignRule>();
                        // 角色
                        string strRoles = DataFormat.GetSafeString(ele.Attribute("candidateRoles").Value);
                        if (strRoles.Length > 0)
                        {
                            string[] arrRoles = strRoles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < arrRoles.Length; j++)
                            {
                                string[] tmpArr = arrRoles[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                string strId = DataFormat.GetSafeString(tmpArr[0]);
                                if (strId.Length == 0)
                                    continue;
                                StAssignRule tmpassignRule = new StAssignRule();
                                tmpassignRule.BasedOn = (int)EnumExecutorBasedOn.ROLE_BASED;
                                tmpassignRule.Executor = strId;
                                tmpassignRule.ExecutorName = RemoteUserService.GetRole(strId).RoleName;
                                vactivity.AssignRuleCol.Add(tmpassignRule);
                            }
                        }
                        // 部门
                        string strDeps = DataFormat.GetSafeString(ele.Attribute("candidateDepts").Value);
                        if (strDeps.Length > 0)
                        {
                            string[] arr = strDeps.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < arr.Length; j++)
                            {
                                string[] tmpArr = arr[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                string strId = DataFormat.GetSafeString(tmpArr[0]);
                                if (strId.Length == 0)
                                    continue;
                                StAssignRule tmpassignRule = new StAssignRule();
                                tmpassignRule.BasedOn = (int)EnumExecutorBasedOn.DEPT_BASED;
                                tmpassignRule.Executor = strId;
                                tmpassignRule.ExecutorName = RemoteUserService.GetDepartment(strId).CompanyName;
                                vactivity.AssignRuleCol.Add(tmpassignRule);
                            }
                        }
                        // 用户
                        string strUsers = DataFormat.GetSafeString(ele.Attribute("candidateUsers").Value);
                        if (strUsers.Length > 0)
                        {
                            string[] arr = strUsers.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < arr.Length; j++)
                            {
                                string[] tmpArr = arr[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                string strId = DataFormat.GetSafeString(tmpArr[0]);
                                if (strId.Length == 0)
                                    continue;
                                StAssignRule tmpassignRule = new StAssignRule();
                                tmpassignRule.BasedOn = (int)EnumExecutorBasedOn.USER_BASED;
                                tmpassignRule.Executor = strId;
                                tmpassignRule.ExecutorName = RemoteUserService.GetUser(strId).UserRealName;
                                vactivity.AssignRuleCol.Add(tmpassignRule);
                            }
                        }
                        // 保存节点的流程规则
                        string transPosition = "";
                        string transGrphid = "";
                        string transGraphName = "";
                        string transTo = "";
                        foreach (XElement chileEle in ele.Elements())
                        {
                            transPosition += DataFormat.GetSafeString(chileEle.Attribute("g").Value) + "|";
                            transGrphid += DataFormat.GetSafeString(chileEle.Attribute("id").Value) + "|";
                            string tempGraphName = DataFormat.GetSafeString(chileEle.Attribute("name").Value, "");
                            if (tempGraphName == "")
                                tempGraphName = DataFormat.EncodeBase64("1==1");

                            transGraphName += DataFormat.GetSafeString(tempGraphName) + "|";
                            transTo += DataFormat.GetSafeString(chileEle.Attribute("to").Value) + "|";
                        }
                        StRoutingRule routinRule = new StRoutingRule();
                        routinRule.PreActivityid = activity.ActivityGraphId.Equals(firstActivityId, StringComparison.OrdinalIgnoreCase) ? 0 : -1;
                        routinRule.CompletionFlag = 1;
                        routinRule.NextActivityidList = transTo;
                        routinRule.PreDependentSet = "";
                        routinRule.TransPosition = transPosition;
                        routinRule.TransGraphId = transGrphid;
                        routinRule.TransGraphName = transGraphName;
                        vactivity.RoutingRuleCol.Add(routinRule);
                    }

                    bool code = WorkFlowService.SaveFlowActivitys(startActivity, activitys, flow.ActivityCol);
                    ret = code ? "OK" : "ERROR";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = "ERROR|" + e.Message;
            }
            Response.Write(ret);
        }
        /// <summary>
        /// 获取流程的部门
        /// </summary>
        public void GetFlowDepartments()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement eleDeps = new XElement("depts");
            doc.Add(eleDeps);
            IList<VCompany> deps = RemoteUserService.GetFlowDepartments(WorkFlowUser.UserName);
            foreach (VCompany dep in deps)
            {
                XElement eleDep = new XElement("dept",
                    new XElement("id", dep.CompanyId),
                    new XElement("name", dep.CompanyName),
                    new XElement("selected", false));
                eleDeps.Add(eleDep);
            }

            doc.Save(Response.OutputStream);
        }
        /// <summary>
        /// 获取流程报表
        /// </summary>
        public void GetFlowReports()
        {
            string companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            IList<StReport> reports = WorkFlowService.GetCompanyReports(companyid);

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement eleReports = new XElement("prints");
            doc.Add(eleReports);
            foreach (StReport report in reports)
            {
                XElement eleReport = new XElement("print",
                    new XElement("id", report.ReportUrl),
                    new XElement("name", report.ReportName),
                    new XElement("selected", false));
                eleReports.Add(eleReport);
            }

            doc.Save(Response.OutputStream);

        }
        /// <summary>
        /// 获取流程列表
        /// </summary>
        public void GetFlowProcess()
        {
            string companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            IList<ViewStProcess> processes = WorkFlowService.GetCompanyProcesses(companyid);

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement eleProcesses = new XElement("processes");
            doc.Add(eleProcesses);
            foreach (ViewStProcess process in processes)
            {
                XElement eleProcess = new XElement("process",
                    new XElement("key", process.Processid),
                    new XElement("name", process.ProcessName));
                eleProcesses.Add(eleProcess);
            }

            doc.Save(Response.OutputStream);
        }
        /// <summary>
        /// 获取流程角色
        /// </summary>
        public void GetFlowRoles()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement eleRoles = new XElement("roles");
            doc.Add(eleRoles);
            IList<VRole> roles = RemoteUserService.GetFlowRoles(WorkFlowUser.UserName);
            foreach (VRole role in roles)
            {
                XElement eleRole = new XElement("role",
                    new XElement("id", role.RoleId),
                    new XElement("name", role.RoleName),
                    new XElement("selected", false));
                eleRoles.Add(eleRole);
            }

            doc.Save(Response.OutputStream);
        }
        /// <summary>
        /// 获取流程用户
        /// </summary>
        public void GetFlowUsers()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement eleUsers = new XElement("users");
            doc.Add(eleUsers);
            IList<VUser> users = RemoteUserService.GetFlowUsers(WorkFlowUser.UserName);
            foreach (VUser user in users)
            {
                XElement eleUser = new XElement("user",
                    new XElement("id", user.UserId),
                    new XElement("name", user.UserRealName),
                    new XElement("selected", false));
                eleUsers.Add(eleUser);
            }

            doc.Save(Response.OutputStream);
        }
        /// <summary>
        /// 获取用户角色树
        /// </summary>
        public void GetflowUserRoleTree()
        {
            IList<VCompany> companys = RemoteUserService.GetFlowCompanys(WorkFlowUser.UserName);
            IList<VCompany> deps = RemoteUserService.GetFlowDepartments(WorkFlowUser.UserName);
            IList<VUser> users = RemoteUserService.GetFlowUsers(WorkFlowUser.UserName);
            IList<VRole> roles = RemoteUserService.GetFlowRoles(WorkFlowUser.UserName);

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement eleCompanys = new XElement("node",
                    new XAttribute("name", "单位列表"),
                    new XAttribute("type", "company"),
                    new XAttribute("state", "0"),
                    new XAttribute("id", "0")
                );
            doc.Add(eleCompanys);
            foreach (VCompany company in companys)
            {
                XElement eleCompany = new XElement("node",
                    new XAttribute("name", company.CompanyName),
                    new XAttribute("type", "branch"),
                    new XAttribute("state", "0"),
                    new XAttribute("id", company.CompanyId)
                );
                eleCompanys.Add(eleCompany);
                foreach (VCompany dep in deps)
                {
                    if (dep.ParentId != company.CompanyId || dep.CompanyId == company.CompanyId)
                        continue;
                    XElement eleDep = new XElement("node",
                        new XAttribute("name", dep.CompanyName),
                        new XAttribute("type", "dept"),
                        new XAttribute("state", "0"),
                        new XAttribute("id", dep.CompanyId)
                    );
                    eleCompany.Add(eleDep);
                    foreach (VUser user in users)
                    {
                        if (user.DepartmentId != dep.CompanyId)
                            continue;

                        XElement eleUser = new XElement("node",
                            new XAttribute("name", user.UserRealName),
                            new XAttribute("type", "user"),
                            new XAttribute("state", "0"),
                            new XAttribute("id", user.UserId)
                        );
                        eleDep.Add(eleUser);
                    }
                }
                foreach (VRole role in roles)
                {
                    if (role.CompanyId != company.CompanyId)
                        continue;
                    XElement eleRole = new XElement("node",
                            new XAttribute("name", role.RoleName),
                            new XAttribute("type", "role"),
                            new XAttribute("state", "0"),
                            new XAttribute("id", role.RoleId)
                    );
                    eleCompany.Add(eleRole);
                }
                foreach (VUser user in users)
                {
                    if (user.DepartmentId != company.CompanyId)
                        continue;
                    XElement eleUser = new XElement("node",
                            new XAttribute("name", user.UserRealName),
                            new XAttribute("type", "user"),
                            new XAttribute("state", "0"),
                            new XAttribute("id", user.UserId)
                    );
                    eleCompany.Add(eleUser);
                }
            }

            doc.Save(Response.OutputStream);
        }

        #endregion

        #region 流程流转页面
        [Authorize]
        public ActionResult StartWork()
        {
            string formUrl = "";
            string prelisturl = "";
            bool zdzdProcess = false;
            ParamOpenCreateWork param = new ParamOpenCreateWork(Request);

            try
            {
                if (param.ProcessId == 0)
                    ViewBag.Error = "无效流程编号";
                else
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(param.ProcessId);
                    if (flow == null || flow.Process == null)
                        ViewBag.Error = "无效流程编号";
                    else
                    {
                        if (flow.RealStartActivity != null)
                        {
                            formUrl = DataFormat.DecodeBase64(DataFormat.GetSafeString(flow.RealStartActivity.FormUrl));
                        }
                        if (formUrl == "")
                        {
                            if (flow.Process.PreListUrl != "" && !param.PreUrlDone && param.ParentSerial == "")
                                prelisturl = flow.Process.PreListUrl;
                            else
                            {
                                zdzdProcess = DataFormat.GetSafeBool(flow.Process.ZdzdProcess);
                                if (!zdzdProcess)
                                {
                                    StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(flow.Process.FormTemplateid));

                                    ViewBag.HasHtml = template.TemplateType.IndexOf("1") > -1 ? "1" : "0";
                                    ViewBag.HasOffice = (template.TemplateType.IndexOf("2") > -1 || template.TemplateType.IndexOf("3") > -1) ? "1" : "0";
                                    ViewBag.TemplateId = template.Templateid;

                                    ViewBag.ProcessId = param.ProcessId;
                                    ViewBag.ParentSerial = param.ParentSerial;
                                    ViewBag.ExtraInfo = DataFormat.EncodeBase64(param.ExtraInfo);
                                    ViewBag.ExtraInfo2 = DataFormat.EncodeBase64(param.ExtraInfo2);
                                    ViewBag.ExtraInfo3 = DataFormat.EncodeBase64(param.ExtraInfo3);
                                    ViewBag.ExtraInfo4 = DataFormat.EncodeBase64(param.ExtraInfo4);
                                    ViewBag.ExtraInfo5 = DataFormat.EncodeBase64(param.ExtraInfo5);
                                    ViewBag.ExtraInfo6 = DataFormat.EncodeBase64(param.ExtraInfo6);
                                    ViewBag.ReturnUrl = param.ReturnUrl;
                                    ViewBag.Serial = param.Serial;
                                    ViewBag.IsCopy = param.IsCopy;
                                    ViewBag.Params = DataFormat.EncodeBase64(param.ParamStr);
                                    ViewBag.CallBackJs = param.callbackjs;

                                    string err = "";

                                    if ((!WorkFlowService.CheckBeforeEnter(flow.RealStartActivity, param.ExtraInfo, null, WorkFlowUser.UserName, "",
                                        WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                                        RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                                        RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err,
                                        param.ExtraInfo2, param.ExtraInfo, param.ExtraInfo4, param.ExtraInfo5, param.ExtraInfo6
                                        )) && DataFormat.GetSafeBool(flow.Process.FixProcess))
                                        ViewBag.Error = err;
                                    else
                                    {
                                        if (template != null)
                                        {
                                            ViewBag.Html = WorkFlowService.GetFormatedForm(template.Templateid,
                                                param.Serial, 0, param.ParentSerial,
                                                RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                                RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                                                RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                                                param.ExtraInfo,
                                                param.ProcessId, WorkFlowUser.RealTaskUser, param);
                                        }
                                        ViewBag.Error = "";
                                    }
                                }
                            }
                        }
                    }
                }


                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpWorkCreate,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = string.Format("流程id:{0},父工作流水号:{1},额外表信息:{2},返回页面:{3}", ViewBag.ProcessId, ViewBag.ParentSerial, ViewBag.ExtraInfo, ViewBag.ReturnUrl),
                    Result = true
                };
                LogService.SaveLog(log);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            ActionResult view = View();
            if (formUrl != "")
                view = new RedirectResult(param.AppendParams("/workflow/startworkn"));
            else if (prelisturl != "")
                view = new RedirectResult(param.FormatNextUrl(prelisturl, "preurldone=true"));
            else if (zdzdProcess)
                view = new RedirectResult(param.AppendParams("/workflow/startworkz"));

            return view;
        }


        public ActionResult PhoneStartWork()
        {
            string formUrl = "";
            string prelisturl = "";
            bool zdzdProcess = false;
            ParamOpenCreateWork param = new ParamOpenCreateWork(Request);

            try
            {
                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);

                if (!WorkFlowUser.IsLogin)
                    PhoneSetUser(username);
                if (param.ProcessId == 0)
                    ViewBag.Error = "无效流程编号";
                else
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(param.ProcessId);
                    if (flow == null || flow.Process == null)
                        ViewBag.Error = "无效流程编号";
                    else
                    {
                        if (flow.RealStartActivity != null)
                        {
                            formUrl = DataFormat.DecodeBase64(DataFormat.GetSafeString(flow.RealStartActivity.FormUrl));
                        }
                        if (formUrl == "")
                        {
                            if (flow.Process.PreListUrl != "" && !param.PreUrlDone)
                                prelisturl = flow.Process.PreListUrl;
                            else
                            {
                                zdzdProcess = DataFormat.GetSafeBool(flow.Process.ZdzdProcess);
                                if (!zdzdProcess)
                                {
                                    StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(flow.Process.FormTemplateid));

                                    ViewBag.HasHtml = template.TemplateType.IndexOf("1") > -1 ? "1" : "0";
                                    ViewBag.HasOffice = (template.TemplateType.IndexOf("2") > -1 || template.TemplateType.IndexOf("3") > -1) ? "1" : "0";
                                    ViewBag.TemplateId = template.Templateid;

                                    ViewBag.ProcessId = param.ProcessId;
                                    ViewBag.ParentSerial = param.ParentSerial;
                                    ViewBag.ExtraInfo = DataFormat.EncodeBase64(param.ExtraInfo);
                                    ViewBag.ExtraInfo2 = DataFormat.EncodeBase64(param.ExtraInfo2);
                                    ViewBag.ExtraInfo3 = DataFormat.EncodeBase64(param.ExtraInfo3);
                                    ViewBag.ExtraInfo4 = DataFormat.EncodeBase64(param.ExtraInfo4);
                                    ViewBag.ExtraInfo5 = DataFormat.EncodeBase64(param.ExtraInfo5);
                                    ViewBag.ExtraInfo6 = DataFormat.EncodeBase64(param.ExtraInfo6);
                                    ViewBag.ReturnUrl = param.ReturnUrl;
                                    ViewBag.Serial = param.Serial;
                                    ViewBag.IsCopy = param.IsCopy;
                                    ViewBag.Params = DataFormat.EncodeBase64(param.ParamStr);
                                    ViewBag.CallBackJs = param.callbackjs;

                                    string err = "";

                                    if ((!WorkFlowService.CheckBeforeEnter(flow.RealStartActivity, param.ExtraInfo, null, WorkFlowUser.UserName, "",
                                        WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                                        RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                                        RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err,
                                        param.ExtraInfo2, param.ExtraInfo, param.ExtraInfo4, param.ExtraInfo5, param.ExtraInfo6
                                        )) && DataFormat.GetSafeBool(flow.Process.FixProcess))
                                        ViewBag.Error = err;
                                    else
                                    {
                                        if (template != null)
                                        {
                                            ViewBag.Html = WorkFlowService.GetFormatedForm(template.Templateid,
                                                param.Serial, 0, param.ParentSerial,
                                                RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                                RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                                                RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                                                param.ExtraInfo,
                                                param.ProcessId, WorkFlowUser.RealTaskUser, param);
                                        }
                                        ViewBag.Error = "";
                                    }
                                }
                            }
                        }
                    }
                }


                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpWorkCreate,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = string.Format("流程id:{0},父工作流水号:{1},额外表信息:{2},返回页面:{3}", ViewBag.ProcessId, ViewBag.ParentSerial, ViewBag.ExtraInfo, ViewBag.ReturnUrl),
                    Result = true
                };
                LogService.SaveLog(log);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            ActionResult view = View();
            if (formUrl != "")
                view = new RedirectResult(param.AppendParams("/workflow/startworkn"));
            else if (prelisturl != "")
                view = new RedirectResult(param.FormatNextUrl(prelisturl, "preurldone=true"));
            else if (zdzdProcess)
                view = new RedirectResult(param.AppendParams("/workflow/startworkz"));

            return view;
        }
        /// <summary>
        /// 直接转向某个url
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult StartWorkn()
        {
            ParamOpenCreateWork param = new ParamOpenCreateWork(Request);

            try
            {
                if (param.ProcessId == 0)
                    ViewBag.Error = "无效流程编号";
                else
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(param.ProcessId);
                    if (flow == null || flow.Process == null)
                        ViewBag.Error = "无效流程编号";
                    else
                    {
                        ViewBag.ProcessId = param.ProcessId;
                        ViewBag.ParentSerial = param.ParentSerial;
                        ViewBag.ExtraInfo = DataFormat.EncodeBase64(param.ExtraInfo);
                        ViewBag.ExtraInfo2 = DataFormat.EncodeBase64(param.ExtraInfo2);
                        ViewBag.ExtraInfo3 = DataFormat.EncodeBase64(param.ExtraInfo3);
                        ViewBag.ExtraInfo4 = DataFormat.EncodeBase64(param.ExtraInfo4);
                        ViewBag.ExtraInfo5 = DataFormat.EncodeBase64(param.ExtraInfo5);
                        ViewBag.ExtraInfo6 = DataFormat.EncodeBase64(param.ExtraInfo6);
                        ViewBag.ReturnUrl = param.ReturnUrl;
                        ViewBag.Serial = param.Serial;
                        ViewBag.IsCopy = param.IsCopy;
                        ViewBag.FormUrl = param.AppendParams(DataFormat.DecodeBase64(flow.RealStartActivity.FormUrl));
                        ViewBag.Params = DataFormat.EncodeBase64(param.ParamStr);
                        ViewBag.CallBackJs = param.callbackjs;

                        string err = "";
                        if (!WorkFlowService.CheckBeforeEnter(flow.RealStartActivity, param.ExtraInfo, null, WorkFlowUser.UserName, "",
                            WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                            RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err,
                            param.ExtraInfo2, param.ExtraInfo, param.ExtraInfo4, param.ExtraInfo5, param.ExtraInfo6
                            ))
                            ViewBag.Error = err;
                        else
                            ViewBag.Error = "";
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return View();
        }
        /// <summary>
        /// ZDZD流程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult StartWorkz()
        {
            ParamOpenCreateWork param = new ParamOpenCreateWork(Request);

            try
            {
                if (param.ProcessId == 0)
                    ViewBag.Error = "无效流程编号";
                else
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(param.ProcessId);
                    if (flow == null || flow.Process == null)
                        ViewBag.Error = "无效流程编号";
                    else
                    {
                        ViewBag.ProcessId = param.ProcessId;
                        ViewBag.ParentSerial = param.ParentSerial;
                        ViewBag.ExtraInfo = DataFormat.EncodeBase64(param.ExtraInfo);
                        ViewBag.ExtraInfo2 = DataFormat.EncodeBase64(param.ExtraInfo2);
                        ViewBag.ExtraInfo3 = DataFormat.EncodeBase64(param.ExtraInfo3);
                        ViewBag.ExtraInfo4 = DataFormat.EncodeBase64(param.ExtraInfo4);
                        ViewBag.ExtraInfo5 = DataFormat.EncodeBase64(param.ExtraInfo5);
                        ViewBag.ExtraInfo6 = DataFormat.EncodeBase64(param.ExtraInfo6);
                        ViewBag.ReturnUrl = param.ReturnUrl;
                        ViewBag.Serial = param.Serial;
                        ViewBag.IsCopy = param.IsCopy;
                        ViewBag.Params = DataFormat.EncodeBase64(param.ParamStr);
                        ViewBag.CallBackJs = param.callbackjs;

                        string err = "";
                        if (!WorkFlowService.CheckBeforeEnter(flow.RealStartActivity, param.ExtraInfo, null, WorkFlowUser.UserName, "",
                            WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                            RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err,
                            param.ExtraInfo2, param.ExtraInfo, param.ExtraInfo4, param.ExtraInfo5, param.ExtraInfo6
                            ))
                            ViewBag.Error = err;
                        else
                            ViewBag.Error = "";
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return View();
        }
        /// <summary>
        /// OFFICE流程控件
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult StartWorkoFrame()
        {
            int templateId = DataFormat.GetSafeInt(Request["templateid"]);
            int taskId = DataFormat.GetSafeInt(Request["taskid"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);
            try
            {
                string url = "";

                string fileType = "";
                if (taskId > 0)
                {
                    StToDoTasks task = WorkFlowService.GetTodoTask(taskId);
                    StForm form = WorkFlowService.GetForm(task.SerialNo);
                    fileType = form.TemplateType;
                    url = "/workflow/GetFormOffileFile?taskid=" + taskId;

                }
                else if (serial != "")
                {
                    StForm form = WorkFlowService.GetForm(serial);
                    fileType = form.TemplateType;
                    url = "/workflow/GetFormOffileFile?serial=" + serial;
                }
                else
                {
                    StFormTemplate template = WorkFlowService.GetFormTemplate(templateId);
                    fileType = template.TemplateType;
                    url = "/workflow/GetTemplateOffileFile?templateid=" + templateId;

                }

                PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
                pc.ID = "PageOfficeCtrl1";
                pc.SaveFilePage = "/WorkFlow/SaveFormOfficeFile";
                pc.ServerPage = "/pageoffice/server.aspx";
                pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
                pc.Titlebar = false; //隐藏标题栏
                pc.Menubar = false; //隐藏菜单栏
                pc.OfficeToolbars = true; //隐藏Office工具栏
                pc.CustomToolbar = false; //隐藏自定义工具栏

                System.Web.UI.Page page = new System.Web.UI.Page();
                PageOffice.OpenModeType openMode = PageOffice.OpenModeType.docAdmin;
                FormTemplateType templateType = new FormTemplateType(fileType);
                if (templateType.HasExcel)
                    openMode = PageOffice.OpenModeType.xlsNormalEdit;
                pc.WebOpen(url, openMode, WorkFlowUser.UserName);
                page.Controls.Add(pc);
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        Server.Execute(page, htw, false);
                        ViewBag.EditorHtml = sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return View();
        }
        [Authorize]
        public ActionResult CheckWork()
        {
            ParamOpenCheckWork param = new ParamOpenCheckWork(Request);

            int taskid = param.TaskId;
            if (taskid == 0)
            {
                ViewBag.Error = "无效的任务id";
                return View();
            }
            StToDoTasks task = WorkFlowService.GetTodoTask(taskid);
            if (task == null)
            {
                ViewBag.Error = "无效的工作，可能是其他人已经完成了该工作，或者工作已撤销";
                return View();
            }

            StActivity activity = WorkFlowService.GetActivity(task.CurActivityid.Value);
            //这里整个逻辑要换过，因为根据activity不能获取processid，
            StForm form = WorkFlowService.GetForm(task.SerialNo);
            ViewStProcess process = WorkFlowService.GetProcess(form.Processid.Value);// new StProcess(form.Processid.Value);
            if (process.FixProcess.Value == false)
            {
                activity = new StActivity();
                activity.Activityid = 0;
                activity.Processid = process.Processid;
                activity.ActivityName = "办理";
                activity.HiddenNextStep = false;
                activity.PermitSelectUser = true;
                activity.HiddenOpinion = true;
            }
            if (activity == null && task.CurActivityid > 0)
            {
                ViewBag.Error = "无效的流程步骤id，可能是流程已更改";
                return View();
            }
            string formUrl = "";
            if (activity != null)
                formUrl = DataFormat.DecodeBase64(DataFormat.GetSafeString(activity.FormUrl));
            bool zdzdProcess = false;
            if (formUrl == "")
            {
                VOrderFlow flow = WorkFlowService.GetOrderFlow(form.Processid.Value);
                zdzdProcess = DataFormat.GetSafeBool(flow.Process.ZdzdProcess);

                if (!zdzdProcess)
                {
                    ViewBag.TaskStatus = DataFormat.GetSafeInt(task.TaskStatus);

                    VOrderWork work = WorkFlowService.GetOrderWork(task.SerialNo, "", flow);


                    StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(work.Form.FormTemplateId));

                    ViewBag.HasHtml = template.TemplateType.IndexOf("1") > -1 ? "1" : "0";
                    ViewBag.HasOffice = (template.TemplateType.IndexOf("2") > -1 || template.TemplateType.IndexOf("3") > -1) ? "1" : "0";

                    ViewBag.ProcessId = activity.Processid;
                    ViewBag.ReturnUrl = param.ReturnUrl;
                    ViewBag.TaskId = taskid;
                    ViewBag.ActivityId = activity.Activityid;
                    ViewBag.Serial = task.SerialNo;
                    ViewBag.ParentDlgId = param.DlgId;

                    string err = "";


                    ViewBag.SerialNo = work.Form.SerialNo;

                    if (!WorkFlowService.CheckBeforeEnter(activity, "", work, WorkFlowUser.UserName, "",
                        WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                                RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                                RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err))
                    {
                        ViewBag.Error = err;
                        return View();
                    }

                    if (template != null)
                    {
                        string extrainfo = "";
                        if (work != null && work.Form != null)
                            extrainfo = work.Form.ExtraInfo1;
                        ViewBag.Html = WorkFlowService.GetFormatedForm(template.Templateid,
                            task.SerialNo, activity.Activityid, "",
                            RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                            extrainfo,
                            activity.Processid.Value, WorkFlowUser.RealTaskUser, param);
                    }
                }
            }

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpWorkCheck,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = string.Format("流程id:{0},工作id:{1},工作名称:{2}", activity.Processid, taskid, activity.ActivityName),
                Result = true
            };
            ActionResult view = View();
            if (formUrl != "")
                view = new RedirectResult(param.AppendParams("/workflow/checkworkn"));
            else if (zdzdProcess)
                view = new RedirectResult(param.AppendParams("/workflow/checkworkz"));
            return view;
        }

        public ActionResult PhoneCheckWork()
        {
            string username = DataFormat.GetSafeString(Request["login_name"]);
            string password = DataFormat.GetSafeString(Request["login_pwd"]);

            if (!WorkFlowUser.IsLogin)
                PhoneSetUser(username);

            ParamOpenCheckWork param = new ParamOpenCheckWork(Request);

            int taskid = param.TaskId;
            if (taskid == 0)
            {
                ViewBag.Error = "无效的任务id";
                return View();
            }
            StToDoTasks task = WorkFlowService.GetTodoTask(taskid);
            if (task == null)
            {
                ViewBag.Error = "无效的工作，可能是其他人已经完成了该工作，或者工作已撤销";
                return View();
            }

            StActivity activity = WorkFlowService.GetActivity(task.CurActivityid.Value);
            //这里整个逻辑要换过，因为根据activity不能获取processid，
            StForm form = WorkFlowService.GetForm(task.SerialNo);
            ViewStProcess process = WorkFlowService.GetProcess(form.Processid.Value);// new StProcess(form.Processid.Value);
            if (process.FixProcess.Value == false)
            {
                activity = new StActivity();
                activity.Activityid = 0;
                activity.Processid = process.Processid;
                activity.ActivityName = "办理";
                activity.HiddenNextStep = false;
                activity.PermitSelectUser = true;
                activity.HiddenOpinion = true;
            }
            if (activity == null && task.CurActivityid > 0)
            {
                ViewBag.Error = "无效的流程步骤id，可能是流程已更改";
                return View();
            }
            string formUrl = "";
            if (activity != null)
                formUrl = DataFormat.DecodeBase64(DataFormat.GetSafeString(activity.FormUrl));
            bool zdzdProcess = false;
            if (formUrl == "")
            {
                VOrderFlow flow = WorkFlowService.GetOrderFlow(form.Processid.Value);
                zdzdProcess = DataFormat.GetSafeBool(flow.Process.ZdzdProcess);

                if (!zdzdProcess)
                {
                    ViewBag.TaskStatus = DataFormat.GetSafeInt(task.TaskStatus);

                    VOrderWork work = WorkFlowService.GetOrderWork(task.SerialNo, "", flow);


                    StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(work.Form.FormTemplateId));

                    ViewBag.HasHtml = template.TemplateType.IndexOf("1") > -1 ? "1" : "0";
                    ViewBag.HasOffice = (template.TemplateType.IndexOf("2") > -1 || template.TemplateType.IndexOf("3") > -1) ? "1" : "0";

                    ViewBag.ProcessId = activity.Processid;
                    ViewBag.ReturnUrl = param.ReturnUrl;
                    ViewBag.TaskId = taskid;
                    ViewBag.ActivityId = activity.Activityid;
                    ViewBag.Serial = task.SerialNo;
                    ViewBag.ParentDlgId = param.DlgId;

                    string err = "";


                    ViewBag.SerialNo = work.Form.SerialNo;

                    if (!WorkFlowService.CheckBeforeEnter(activity, "", work, WorkFlowUser.UserName, "",
                        WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                                RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                                RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err))
                    {
                        ViewBag.Error = err;
                        return View();
                    }

                    if (template != null)
                    {
                        string extrainfo = "";
                        if (work != null && work.Form != null)
                            extrainfo = work.Form.ExtraInfo1;
                        ViewBag.Html = WorkFlowService.GetFormatedForm(template.Templateid,
                            task.SerialNo, activity.Activityid, "",
                            RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                            extrainfo,
                            activity.Processid.Value, WorkFlowUser.RealTaskUser, param);
                    }
                }
            }

            BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
            {
                ClientType = WorkFlowConst.LogClientType,
                Ip = ClientInfo.Ip,
                LogTime = DateTime.Now,
                ModuleName = WorkFlowConst.LogModuleName,
                Operation = WorkFlowConst.LogOpWorkCheck,
                UserName = WorkFlowUser.UserName,
                RealName = WorkFlowUser.RealName,
                Remark = string.Format("流程id:{0},工作id:{1},工作名称:{2}", activity.Processid, taskid, activity.ActivityName),
                Result = true
            };
            ActionResult view = View();
            if (formUrl != "")
                view = new RedirectResult(param.AppendParams("/workflow/checkworkn"));
            else if (zdzdProcess)
                view = new RedirectResult(param.AppendParams("/workflow/checkworkz"));
            return view;
        }
        [Authorize]
        public ActionResult CheckWorkn()
        {
            ParamOpenCheckWork param = new ParamOpenCheckWork(Request);

            int taskid = param.TaskId;
            if (taskid == 0)
            {
                ViewBag.Error = "无效的任务id";
                return View();
            }
            StToDoTasks task = WorkFlowService.GetTodoTask(taskid);
            if (task == null)
            {
                ViewBag.Error = "无效的工作，可能是其他人已经完成了该工作，或者工作已撤销";
                return View();
            }

            StActivity activity = WorkFlowService.GetActivity(task.CurActivityid.Value);
            StForm form = WorkFlowService.GetForm(task.SerialNo);
            ViewStProcess process = WorkFlowService.GetProcess(form.Processid.Value);// new StProcess(form.Processid.Value);
            if (process.FixProcess.Value == false)
            {
                activity = new StActivity();
                activity.Activityid = 0;
                activity.Processid = process.Processid;
                activity.ActivityName = "办理";
                activity.HiddenNextStep = false;
                activity.PermitSelectUser = true;
                activity.HiddenOpinion = true;
            }
            if (activity == null && task.CurActivityid > 0)
            {
                ViewBag.Error = "无效的流程步骤id，可能是流程已更改";
                return View();
            }
            ViewBag.ProcessId = activity.Processid;
            ViewBag.ReturnUrl = param.ReturnUrl;
            ViewBag.TaskId = taskid;
            ViewBag.ActivityId = activity.Activityid;
            ViewBag.Serial = task.SerialNo;
            ViewBag.ParentDlgId = param.DlgId;
            // 附加了工作流水号
            ViewBag.FormUrl = param.AppendParams(DataFormat.DecodeBase64(activity.FormUrl), "serial=" + task.SerialNo);

            VOrderFlow flow = WorkFlowService.GetOrderFlow(activity.Processid.Value);

            string err = "";
            VOrderWork work = WorkFlowService.GetOrderWork(task.SerialNo, "", flow);
            if (!WorkFlowService.CheckBeforeEnter(activity, "", work, WorkFlowUser.UserName, "",
                WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                        RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err))
            {
                ViewBag.Error = err;
                return View();
            }


            return View();
        }
        [Authorize]
        public ActionResult CheckWorkz()
        {
            ParamOpenCheckWork param = new ParamOpenCheckWork(Request);

            int taskid = param.TaskId;
            if (taskid == 0)
            {
                ViewBag.Error = "无效的任务id";
                return View();
            }
            StToDoTasks task = WorkFlowService.GetTodoTask(taskid);
            if (task == null)
            {
                ViewBag.Error = "无效的工作，可能是其他人已经完成了该工作，或者工作已撤销";
                return View();
            }

            StActivity activity = WorkFlowService.GetActivity(task.CurActivityid.Value);
            StForm form = WorkFlowService.GetForm(task.SerialNo);
            ViewStProcess process = WorkFlowService.GetProcess(form.Processid.Value);// new StProcess(form.Processid.Value);
            if (process.FixProcess.Value == false)
            {
                activity = new StActivity();
                activity.Activityid = 0;
                activity.Processid = process.Processid;
                activity.ActivityName = "办理";
                activity.HiddenNextStep = false;
                activity.PermitSelectUser = true;
                activity.HiddenOpinion = true;
            }
            if (activity == null && task.CurActivityid > 0)
            {
                ViewBag.Error = "无效的流程步骤id，可能是流程已更改";
                return View();
            }
            ViewBag.ProcessId = activity.Processid;
            ViewBag.ReturnUrl = param.ReturnUrl;
            ViewBag.TaskId = taskid;
            ViewBag.ActivityId = activity.Activityid;
            ViewBag.Serial = task.SerialNo;
            ViewBag.ParentDlgId = param.DlgId;
            // 附加了工作流水号

            VOrderFlow flow = WorkFlowService.GetOrderFlow(activity.Processid.Value);


            string err = "";
            VOrderWork work = WorkFlowService.GetOrderWork(task.SerialNo, "", flow);

            ViewBag.Params = work.Form.Params;
            ViewBag.EntryKey = work.Form.EntryKey;

            if (!WorkFlowService.CheckBeforeEnter(activity, "", work, WorkFlowUser.UserName, "",
                WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                        RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err))
            {
                ViewBag.Error = err;
                return View();
            }


            return View();
        }
        [Authorize]
        public ActionResult TodoWorkList()
        {
            return View();
        }
        [Authorize]
        public ActionResult AllWorkList()
        {
            return View();
        }
        [Authorize]
        public ActionResult UnsubmitWorkList()
        {
            return View();
        }

        #endregion

        #region 流程流转函数
        /// <summary>
        /// 获取流程定义
        /// </summary>
        [Authorize]
        public void GetProcessInfo()
        {
            string ret = JsonClient.GetRetString(false);
            try
            {
                int processid = DataFormat.GetSafeInt(Request["id"]);
                if (processid > 0)
                {
                    //这里没写完，如果自由流程的话，怎么返回
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);
                    ret = JsonClient.GetRetString(0, flow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取下一个有效步骤
        /// </summary>
        [Authorize]
        public void GetNextSteps()
        {
            int activityid = DataFormat.GetSafeInt(Request["id"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);

            IList<StActivity> activitys = new List<StActivity>();
            try
            {
                if (activityid == 0)
                {
                    StActivity tempactivity = new StActivity();
                    tempactivity.Activityid = 0;
                    tempactivity.Processid = 0;
                    tempactivity.ActivityName = "办理";
                    tempactivity.HiddenNextStep = false;
                    tempactivity.PermitSelectUser = true;
                    tempactivity.HiddenOpinion = true;
                    activitys.Add(tempactivity);
                    tempactivity = new StActivity();
                    tempactivity.Activityid = -1;
                    tempactivity.Processid = 0;
                    tempactivity.ActivityType = 8;
                    tempactivity.ActivityName = "完成";
                    tempactivity.HiddenNextStep = false;
                    tempactivity.PermitSelectUser = true;
                    tempactivity.HiddenOpinion = true;
                    tempactivity.PermitSelectUser = false;
                    activitys.Add(tempactivity);
                }
                else
                    activitys = WorkFlowService.GetValidNextActivitys(activityid, WorkFlowUser.RealTaskUser.UserName, WorkFlowUser.RealTaskUser.DepartmentId, WorkFlowUser.DutyLevel, serial, null);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(activitys));
            }
        }
        /// <summary>
        /// 获取某个步骤执行用户
        /// </summary>
        [Authorize]
        public void GetActivityUsers()
        {
            int activityid = DataFormat.GetSafeInt(Request["id"]);
            int nextactivityid = DataFormat.GetSafeInt(Request["nextid"]);
            int processid = DataFormat.GetSafeInt(Request["processid"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);

            StringBuilder sb = new StringBuilder();
            IList<VUser> users = new List<VUser>();
            try
            {
                if (activityid == 0 && nextactivityid == 0)
                {
                    users = RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager);
                }
                else if (activityid == 0 && nextactivityid == -1)
                {
                    VUser tempuser = new VUser();
                    tempuser.UserId = "0";
                    tempuser.UserRealName = "自动办理";
                    users.Add(tempuser);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    sb.Append(jss.Serialize(users));

                }
                else
                {
                    //IList<VCompany> companys = RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager);
                    IList<VCompany> deps1 = RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager);
                    IList<VCompany> deps = null;
                    users = WorkFlowService.GetNextActivityUsers(processid, serial, activityid, nextactivityid,
                        RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                        WorkFlowUser.RealTaskUser.UserName);
                    sb.Append("[");
                    deps = (from VCompany t in deps1

                            orderby t.CompanyId descending // 按照Name属性排序 

                            select t).ToList();

                    bool firstDep = true;
                    foreach (VCompany dep in deps)
                    {
                        StringBuilder ret2 = new StringBuilder();




                        bool firstUser = true;
                        foreach (VUser user in users)
                        {
                            if (user.DepartmentId != dep.CompanyId)
                                continue;
                            if (firstUser)
                                firstUser = false;
                            else
                                ret2.Append(",");

                            ret2.Append("\r\n\t\t{\"id\":\"" + user.UserId + "\",\"text\":\"" + user.UserRealName + "\"}");
                        }
                        if (ret2.ToString() != "")
                        {
                            if (firstDep)
                                firstDep = false;
                            else
                                sb.Append(",");
                            sb.Append("{\"id\":\"" + dep.CompanyId + "\",\"text\":\"" + dep.CompanyName + "\",\"children\":[");
                            sb.Append(ret2.ToString());
                            sb.Append("]}");
                        }
                    }
                    sb.Append("]");
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                //JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(sb.ToString());
            }
        }
        /// <summary>
        /// 获取默认用户
        /// </summary>
        [Authorize]
        public void GetNextStepDefaultUser()
        {
            int activityid = DataFormat.GetSafeInt(Request["activityid"]);
            int processid = DataFormat.GetSafeInt(Request["processid"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);
            string entrykey = DataFormat.GetSafeString(Request["entrykey"]);

            IList<string> userids = new List<string>();

            try
            {
                if (activityid <= 0)
                {
                    userids.Add("0");
                }
                else
                {
                    IList<VUser> users = RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager);
                    userids = WorkFlowService.GetNextStepDefaultUser(processid, activityid, WorkFlowUser.RealTaskUser.UserName, WorkFlowUser.RealTaskUser.DepartmentId, serial, users, entrykey);

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(userids));
            }
        }
        /// <summary>
        /// 获取可以创建的新流程
        /// </summary>
        [Authorize]
        public void GetNewProcess()
        {
            IList<ViewStProcess> ret = new List<ViewStProcess>();
            try
            {
                int activityid = DataFormat.GetSafeInt(Request["id"]);
                ret = WorkFlowService.GetNewProcess(activityid);
                var q = from e in ret where !DataFormat.GetSafeBool(e.SubProcess) select e;
                ret = q.ToList<ViewStProcess>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(ret));
            }
        }
        /// <summary>
        /// 获取协办人员
        /// </summary>
        [Authorize]
        public void GetAssistUsers()
        {
            /*
			List<VUser> ret = new List<VUser>();
			try
			{
				int activityid = DataFormat.GetSafeInt(Request["id"]);
				StActivity activity = WorkFlowService.GetActivity(activityid);
				ViewStProcess curProcess = WorkFlowService.GetProcess(DataFormat.GetSafeInt(activity.Processid));
				IList<ViewStProcess> processes = new List<ViewStProcess>();
				if (DataFormat.GetSafeBool(curProcess.SubProcess))
					processes.Add(curProcess);
				else
				{
					processes = WorkFlowService.GetNewProcess(activityid);
					if (processes != null)
						processes = processes.Where(tmppro => DataFormat.GetSafeBool(tmppro.SubProcess)).ToList();
					else
						processes = new List<ViewStProcess>();
				}
				foreach (ViewStProcess process in processes)
					ret.AddRange(WorkFlowService.GetProcessAllUsers(process.Processid, WorkFlowUser.UserName,
						RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
						RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager))));
				ret = ret.Distinct().ToList();
				var q = from e in ret where !e.UserId.Equals(WorkFlowUser.UserName) select e;
                //这里要改，返回部门加用户格式，如果没有用户，就不要返回单位，还是参照首页的吧。索性全部重写过吧


				ret = q.ToList<VUser>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(jss.Serialize(ret));
			}
            */
            StringBuilder ret = new StringBuilder();

            try
            {
                IList<VCompany> companys = RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager);
                IList<VCompany> deps = RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager);
                List<VUser> users = new List<VUser>();


                int activityid = DataFormat.GetSafeInt(Request["id"]);
                StActivity activity = WorkFlowService.GetActivity(activityid);
                ViewStProcess curProcess = WorkFlowService.GetProcess(DataFormat.GetSafeInt(activity.Processid));
                IList<ViewStProcess> processes = new List<ViewStProcess>();
                if (DataFormat.GetSafeBool(curProcess.SubProcess))
                    processes.Add(curProcess);
                else
                {
                    processes = WorkFlowService.GetNewProcess(activityid);
                    if (processes != null)
                        processes = processes.Where(tmppro => DataFormat.GetSafeBool(tmppro.SubProcess)).ToList();
                    else
                        processes = new List<ViewStProcess>();
                }
                foreach (ViewStProcess process in processes)
                    users.AddRange(WorkFlowService.GetProcessAllUsers(process.Processid, WorkFlowUser.UserName,
                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager))));
                users = users.Distinct().ToList();
                var q = from e in users where !e.UserId.Equals(WorkFlowUser.UserName) select e;


                users = q.ToList<VUser>();


                ret.Append("[");

                bool firstDep = true;
                foreach (VCompany dep in deps)
                {
                    StringBuilder ret2 = new StringBuilder();




                    bool firstUser = true;
                    foreach (VUser user in users)
                    {
                        if (user.DepartmentId != dep.CompanyId)
                            continue;
                        if (firstUser)
                            firstUser = false;
                        else
                            ret2.Append(",");

                        ret2.Append("\r\n\t\t{\"id\":\"" + user.UserId + "\",\"text\":\"" + user.UserRealName + "\"}");
                    }
                    if (ret2.ToString() != "")
                    {
                        if (firstDep)
                            firstDep = false;
                        else
                            ret.Append(",");
                        ret.Append("{\"id\":\"" + dep.CompanyId + "\",\"text\":\"" + dep.CompanyName + "\",\"children\":[");
                        ret.Append(ret2.ToString());
                        ret.Append("]}");
                    }
                }
                ret.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret.Clear();
                ret.Append("[]");
            }
            finally
            {
                Response.Write(ret);
            }


        }
        /// <summary>
        /// 工作协办，挂起原工作，工作转交给协办人
        /// </summary>
        public void DoAskAssistWork()
        {
            bool ret = false;
            string msg = "";
            try
            {
                int taskid = DataFormat.GetSafeInt(Request["taskid"]);
                string assistUserName = DataFormat.GetSafeString(Request["assistuser"]);
                VUser user = RemoteUserService.GetUser(assistUserName);

                ret = WorkFlowService.AskAssistTask(
                    WorkFlowUser.UserName, WorkFlowUser.RealName,
                    user.UserId, user.UserRealName, taskid,
                    RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                    out msg);


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, msg));
            }
        }

        /// <summary>
        /// 取消工作协办
        /// </summary>
        public void DoCancelAssistWork()
        {
            bool ret = false;
            string msg = "";
            try
            {
                int taskid = DataFormat.GetSafeInt(Request["taskid"]);

                ret = WorkFlowService.CancelAssistTask(
                    taskid,
                    WorkFlowUser.RealTaskUser,
                    out msg);


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, msg));
            }
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        [Authorize]
        public void DoCreateWork()
        {
            bool ret = true;
            string msg = "";
            string serial = "";
            try
            {
                ParamReqCreateWork param = new ParamReqCreateWork(Request);
                ret = WorkFlowService.CompleteFirstTask(param, GetFormItems(), WorkFlowUser.RealTaskUser, WorkFlowUser.RealSignUser,
                    RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                    out msg);
                serial = param.Serial;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, msg, serial));
            }
        }
        /// <summary>
        /// 审批任务
        /// </summary>
        [Authorize]
        public void DoCheckWork()
        {
            bool ret = false;
            string msg = "";
            string serial = "";
            try
            {
                ParamReqCheckWork param = new ParamReqCheckWork(Request);
                StToDoTasks todotask = WorkFlowService.GetTodoTask(param.TaskId);
                ret = WorkFlowService.CompleteCheckTask(param, GetFormItems(), WorkFlowUser.RealTaskUser, WorkFlowUser.RealSignUser,
                    RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                    out msg);

                serial = todotask.SerialNo;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, msg, serial));
            }
        }
        /// <summary>
        /// 从请求表单获取内容
        /// </summary>
        /// <returns></returns>
        [Authorize]
        private IList<StFormItem> GetFormItems()
        {
            IList<StFormItem> ret = new List<StFormItem>();
            try
            {
                if (Request.Form != null && Request.Form.AllKeys != null)
                {
                    foreach (string key in Request.Form.AllKeys)
                    {
                        if (key == null)
                            continue;
                        if (key.StartsWith("FixField", StringComparison.OrdinalIgnoreCase))
                            continue;
                        StFormItem itm = new StFormItem() { Formid = 0, ItemName = key, Recid = 0, ItemValue = "" };
                        if (key.StartsWith("@"))	// 编号字段，编号在保存时生成
                            ret.Add(itm);
                        else
                        {
                            itm.ItemValue = DataFormat.GetSafeString(Request[key]);
                            ret.Add(itm);
                        }
                    }
                }
                if (Request.QueryString != null && Request.QueryString.AllKeys != null)
                {
                    foreach (string key in Request.QueryString.AllKeys)
                    {
                        if (key == null)
                            continue;
                        if (key.StartsWith("FixField", StringComparison.OrdinalIgnoreCase))
                            continue;
                        var q = from e in ret where e.ItemName.Equals(key, StringComparison.OrdinalIgnoreCase) select e;
                        if (q.Count() > 0)
                            continue;
                        StFormItem itm = new StFormItem() { Formid = 0, ItemName = key, Recid = 0, ItemValue = "" };
                        if (key.StartsWith("@"))	// 编号字段，编号在保存时生成
                            ret.Add(itm);
                        else
                        {
                            itm.ItemValue = DataFormat.GetSafeString(Request[key]);
                            ret.Add(itm);
                        }
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
        /// 获取某个工作OFFICE文件
        /// </summary>
        [Authorize]
        public void GetFormOffileFile()
        {
            try
            {
                int taskId = DataFormat.GetSafeInt(Request["taskid"]);
                string serial = DataFormat.GetSafeString(Request["serial"]);

                StForm form = null;

                if (taskId > 0)
                {
                    StToDoTasks task = WorkFlowService.GetTodoTask(taskId);
                    form = WorkFlowService.GetForm(task.SerialNo);
                }
                else if (serial != "")
                {
                    form = WorkFlowService.GetForm(serial);
                }

                byte[] fileContent = null;
                string ext = "";
                if (form != null)
                {
                    fileContent = form.FormTemplate;
                    ext = form.TemplateType;
                }
                FormTemplateType templateType = new FormTemplateType(ext);
                ext = templateType.HasWord ? ".doc" : ".xls";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office" + ext);
                Response.Charset = "UTF-8";
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(fileContent);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        /// <summary>
        /// 保存流程流转的OFFICE文件
        /// </summary>
        [Authorize]
        public void SaveFormOfficeFile()
        {
            string msg = "";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {
                string serial = DataFormat.GetSafeString(fs.GetFormField("serial"));

                byte[] file = fs.FileBytes;

                bool ret = WorkFlowService.UpdateFormContent(serial, file, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = "保存OFFICE文件异常，异常信息：" + e.Message;
            }
            finally
            {
                fs.CustomSaveResult = msg;
                fs.Close();
            }
        }

        [Authorize]
        public void TemplateWebOfficeStartWork()
        {
            /*
			try
			{
				iMsgServer2015 MsgObj = new iMsgServer2015();
				MsgObj.setSendType("JSON");
				MsgObj.Load(HttpContext.ApplicationInstance.Context.Request);
				string option = MsgObj.GetMsgByName("OPTION");
				string username = MsgObj.GetMsgByName("USERNAME");
				string recordid = MsgObj.GetMsgByName("RECORDID");
				if (option.Equals("LOADFILE", StringComparison.OrdinalIgnoreCase))
				{

					MsgObj.MsgTextClear();
					if (!recordid.StartsWith("S", StringComparison.OrdinalIgnoreCase))
					{
						StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(recordid));
						if (template != null)
						{
							string ext = template.TemplateType == "WORD模板" ? ".doc" : ".xls";
							bool isLoad = false;
							if ((isLoad = MsgObj.MsgFileLoad(template.ContentWord)) != true)
							{
								isLoad = MsgObj.MsgFileLoad(Server.MapPath("/workflowtemplates/default" + ext));

							}
							if (isLoad)
								MsgObj.Send(HttpContext.ApplicationInstance.Context.Response);
						}
					}
					else
					{
						recordid = recordid.Substring(1);
						StForm form = WorkFlowService.GetForm(recordid);
						string ext = form.TemplateType == "WORD模板" ? ".doc" : ".xls";
						bool isLoad = false;
						if ((isLoad = MsgObj.MsgFileLoad(form.FormTemplate)) != true)
						{
							isLoad = MsgObj.MsgFileLoad(Server.MapPath("/workflowtemplates/default" + ext));

						}
						if (isLoad)
							MsgObj.Send(HttpContext.ApplicationInstance.Context.Response);
					}
				}
				if (option.Equals("SAVEFILE", StringComparison.OrdinalIgnoreCase))
				{
					string seiralno = MsgObj.GetMsgByName("SERIALNO");
					MsgObj.MsgTextClear();
					string message = "";
					bool ret = WorkFlowService.UpdateFormContent(seiralno, MsgObj.MsgFileBody(), out message);


					Response.Write(JsonClient.GetRetString(ret, message));
				}

			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}*/
        }
        /// <summary>
        /// 返回某个工作的formitem列表
        /// </summary>
        /// <param name="serialno"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetWorkDatas(string serialno)
        {
            IList<StFormItem> items = new List<StFormItem>();
            try
            {
                items = WorkFlowService.GetFormItems(serialno);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(items);
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 保存文件
        /// </summary>
        [Authorize]
        public void SaveFile()
        {
            bool code = false;
            string msg = "";
            string ctrlid = DataFormat.GetSafeString(Request["ctrlid"]);		// 客户端控件名
            string filename = DataFormat.GetSafeString(Request["file_name"]);	// 输入的文件名
            string osstype = DataFormat.GetSafeString(Request["osstype"]);	// OSS：上传到OSS，其他值：以全局配置为准
            try
            {

                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postfile = Request.Files[0];
                    if (filename == "")
                        filename = postfile.FileName;
                    // 读取文件
                    byte[] postcontent = new byte[postfile.ContentLength];
                    int readlength = 0;
                    while (readlength < postfile.ContentLength)
                    {
                        int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    // 保存文件到数据库
                    StFile file = new StFile()
                    {
                        Activityid = 0,
                        FileContent = postcontent,
                        Fileid = 0,
                        FileNewName = filename,
                        FileOrgName = postfile.FileName,
                        FileSize = readlength,
                        Formid = 0,
                        StorageType = osstype.Equals("OSS", StringComparison.OrdinalIgnoreCase) ? "OSS" : ""
                    };
                    file = WorkFlowService.SaveFile(file);
                    code = file != null;
                    if (file == null)
                        msg = "文件保存失败";
                    else
                        msg = file.Fileid.ToString();
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write((code ? "0" : "1") + "|" + ctrlid + "|" + msg);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        [Authorize]
        public void DeleteFile()
        {
            bool ret = false;
            string msg = "";
            try
            {
                int fileid = DataFormat.GetSafeInt(Request["id"]);
                ret = WorkFlowService.DeleteFile(fileid);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, msg));
            }
        }

        /// <summary>
        /// 查看文件页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void FileView()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int fileid = DataFormat.GetSafeInt(Request["id"]);
            try
            {
                StFile file = WorkFlowService.GetFile(fileid);

                if (file != null)
                {
                    ret = file.FileContent;
                    filename = file.FileOrgName;
                    filesize = DataFormat.GetSafeLong(file.FileSize);

                    string mime = MimeMapping.GetMimeMapping(filename);
                    Response.Clear();
                    Response.ContentType = mime;
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
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

        /// <summary>
        /// 图片附件大图
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetWorkFlowAttachBig()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int fileid = DataFormat.GetSafeInt(RouteData.Values["id"]);
            try
            {
                StFile file = WorkFlowService.GetFile(fileid);

                if (file != null)
                {
                    MyImage img = new MyImage(file.FileContent);
                    if (img.IsImage())
                        ret = file.FileContent;
                    else
                        ret = null;

                    filename = file.FileOrgName;
                    filesize = DataFormat.GetSafeLong(file.FileSize);

                    string mime = MimeMapping.GetMimeMapping(filename);
                    Response.Clear();
                    Response.ContentType = mime;
                    Response.Charset = "UTF-8";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
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
        /// <summary>
        /// 图片附件小图
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetWorkFlowAttachSmall()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int fileid = DataFormat.GetSafeInt(RouteData.Values["id"]);
            try
            {

                //IList<IDictionary<string, object>> dt = CommonService.GetDataTable2("select fileorgname,filesize,filethumbnail from dbo.STFile where fileid=" + fileid);

                //if (dt.Count > 0)
                //{
                //    ret = dt[0]["filethumbnail"] as byte[];
                //    filename = DataFormat.GetSafeString(dt[0]["fileorgname"]);
                //    filesize = DataFormat.GetSafeLong(dt[0]["filesize"]);
                //}

                //if (ret == null)
                //{
                //    StFile file = WorkFlowService.GetFile(fileid);
                //    MyImage img = new MyImage(file.FileContent);
                //    if (img.IsImage())
                //    {
                //        ret = img.GetThumbnail();
                //        file.FileThumbnail = ret;
                //        WorkFlowService.SaveFile(file);
                //    }
                //    else
                //        ret = null;
                //}
                StFile file = WorkFlowService.GetFileThumbnail(fileid);
                if (file != null)
                {
                    filename = file.FileOrgName;
                    filesize = DataFormat.GetSafeLong(file.FileSize);
                    ret = file.FileThumbnail;
                    if (ret != null)
                    {
                        MyImage img = new MyImage(ret);
                        if (!img.IsImage())
                        {
                            ret = null;
                        }
                    }
                }
                string mime = MimeMapping.GetMimeMapping(filename);
                Response.Clear();
                Response.ContentType = mime;
                Response.Charset = "UTF-8";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                //Response.AddHeader("Content-Length", filesize.ToString());
                Response.BinaryWrite(ret);
                Response.Flush();
                Response.End();
                /*
                StFile file = WorkFlowService.GetFile(fileid);

                if (file != null)
                {
                   
                    if (file.FileThumbnail != null)
                        ret = file.FileThumbnail;
                    else
                    {
                        MyImage img = new MyImage(file.FileContent);
                        if (img.IsImage())
                        {
                            ret = img.GetThumbnail();
                            file.FileThumbnail = ret;
                            WorkFlowService.SaveFile(file);
                        }
                        else
                            ret = null;
                    }
                    filename = file.FileOrgName;
                    filesize = ret.Length;

                    string mime = MimeMapping.GetMimeMapping(filename);
                    Response.Clear();
                    Response.ContentType = mime;
                    Response.Charset = "UTF-8";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                }*/
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

        #region 测试
        public void GetProcessPreActivity()
        {
            try
            {
                int processid = DataFormat.GetSafeInt(Request["processid"]);
                int activityid = DataFormat.GetSafeInt(Request["activityid"]);
                VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);
                StActivity activity = flow.GetPreActivity(activityid);
                Response.Write(activity == null ? "null" : activity.Activityid.ToString());
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        #endregion

        #region 手机任务流转

        /// <summary>
        /// 用户登录，防止用户信息丢失
        /// </summary>
        /// <param name="username"></param>
        public void PhoneSetUser(string username)
        {
            VUser vuser = RemoteUserService.GetUser(username);
            if (vuser != null)
            {
                VCompany company = RemoteUserService.GetDepartment(vuser.CompanyId);
                VCompany dep = RemoteUserService.GetDepartment(vuser.DepartmentId);
                SessionUser user = new SessionUser()
                {
                    UserName = vuser.UserCode,
                    RealName = vuser.UserRealName,
                    CompanyId = vuser.CompanyId,
                    CompanyName = company.CompanyName,
                    DepartmentId = vuser.DepartmentId,
                    DepartmentName = dep.CompanyName,
                    DutyLevel = "2",
                };

                BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(user, null);
                /*
                UserManager.UserMgr.USERCODE = vuser.UserCode;
                UserManager.UserMgr.USERNAME = user.UserName;
                UserManager.UserMgr.REALNAME = user.RealName;
                UserManager.UserMgr.CPCODE = user.CompanyId;
                UserManager.UserMgr.CPNAME = user.CompanyName;
                UserManager.UserMgr.DEPCODE = user.DepartmentId;
                UserManager.UserMgr.DEPNAME = user.DepartmentName;*/
                // 设置录入界面用户
                Session["USERCODE"] = vuser.UserCode;
                Session["USERNAME"] = user.UserName;
                Session["REALNAME"] = user.RealName;
                Session["CPCODE"] = user.CompanyId;
                Session["CPNAME"] = user.CompanyName;
                Session["DEPCODE"] = user.DepartmentId;
                Session["DEPNAME"] = user.DepartmentName;

                System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName, false);
            }
        }

        public void GetPhoneWorkTodoList()
        {


            string username = DataFormat.GetSafeString(Request["login_name"]);
            string password = DataFormat.GetSafeString(Request["login_pwd"]);

            if (!WorkFlowUser.IsLogin)
                PhoneSetUser(username);


            IList<ViewTodoTask> todotasks = new List<ViewTodoTask>();
            int totalcount = 0;
            try
            {
                int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
                int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
                string key = DataFormat.GetSafeString(Request["key"]);
                todotasks = WorkFlowService.PhoneGetTodoTasks(WorkFlowUser.RealTaskUser.UserName, key, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(todotasks)));
            }
        }


        public void PhoneCreattaskDetail()
        {
            string err = "";
            int ret = 1;
            string rettext = "";
            string Params = "", HasHtml = "", HasOffice = "", ProcessId = "", SerialNo = "", Html = "", ActivityId = "", Activityname = "", from = "", hasoption = "", nextstep = "", defaultusers = "", isshowuser = "";
            try
            {

                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                string ExtraInfo = DataFormat.DecodeBase64(DataFormat.GetSafeString(Request["ExtraInfo"]));
                string Serial = DataFormat.GetSafeString(Request["Serial"]);
                string ParentSerial = DataFormat.GetSafeString(Request["ParentSerial"]);
                if (!WorkFlowUser.IsLogin)
                    PhoneSetUser(username);
                ParamOpenCreateWork param = new ParamOpenCreateWork(Request);
                Params = param.ParamStr;

                int processid = DataFormat.GetSafeInt(Request["processid"]);
                if (processid == 0)
                {
                    err = "无效的任务id";
                    throw new Exception(err);

                }
                string formUrl = "";
                VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);
                ViewStProcess process = WorkFlowService.GetProcess(processid);
                if (DataFormat.GetSafeInt(process.UseInPhone.Value, 0) == 0)
                {
                    err = "当前任务不能通过手机办理，请使用PC端办理";
                    throw new Exception(err);
                }
                if (DataFormat.GetSafeInt(process.PhoneTemplateid.Value, 0) == 0)
                {
                    err = "当前任务没有手机模板，请联系系统管理人员！";
                    throw new Exception(err);
                }
                if (flow == null || flow.Process == null)
                {
                    err = "无效的任务id";
                    throw new Exception(err);
                }
                else
                {
                    if (flow.RealStartActivity != null)
                    {
                        formUrl = DataFormat.DecodeBase64(DataFormat.GetSafeString(flow.RealStartActivity.FormUrl));
                    }
                    if (formUrl == "")
                    { }
                }


                bool zdzdProcess = false;
                if (formUrl == "")
                {
                    zdzdProcess = DataFormat.GetSafeBool(flow.Process.ZdzdProcess);

                    if (!zdzdProcess)
                    {

                        StFormTemplate template = WorkFlowService.GetFormTemplate(BD.WorkFlow.Common.DataFormat.GetSafeInt(process.FormTemplateid));
                        HasHtml = template.TemplateType.IndexOf("1") > -1 ? "1" : "0";
                        HasOffice = (template.TemplateType.IndexOf("2") > -1 || template.TemplateType.IndexOf("3") > -1) ? "1" : "0";

                        StFormTemplate phonetemplate = WorkFlowService.GetFormTemplate(BD.WorkFlow.Common.DataFormat.GetSafeInt(process.PhoneTemplateid));

                        StActivity activity = flow.RealStartActivity;
                        ProcessId = processid.ToString();
                        hasoption = activity.HiddenOpinion.Value ? "0" : "1";
                        //ViewBag.TaskId = taskid;
                        ActivityId = activity.Activityid.ToString();
                        //ViewBag.Serial = task.SerialNo;
                        Activityname = activity.ActivityName;
                        isshowuser = activity.PermitSelectUser.Value ? "1" : "0";

                        err = "";

                        SerialNo = "";

                        IList<string> userids = new List<string>();
                        if (activity.Activityid <= 0)
                        {
                            userids.Add("0");
                        }
                        else
                        {
                            IList<VUser> users = RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager);
                            userids = WorkFlowService.GetNextStepDefaultUser(activity.Processid.Value, activity.Activityid, WorkFlowUser.RealTaskUser.UserName, WorkFlowUser.RealTaskUser.DepartmentId, SerialNo, users);
                        }


                        IList<StActivity> activitys = new List<StActivity>();
                        if (!activity.HiddenNextStep.Value)
                        {
                            if (activity.Activityid == 0)
                            {
                                StActivity tempactivity = new StActivity();
                                tempactivity.Activityid = 0;
                                tempactivity.Processid = 0;
                                tempactivity.ActivityName = "办理";
                                tempactivity.HiddenNextStep = false;
                                tempactivity.PermitSelectUser = true;
                                tempactivity.HiddenOpinion = true;
                                activitys.Add(tempactivity);
                                tempactivity = new StActivity();
                                tempactivity.Activityid = -1;
                                tempactivity.Processid = 0;
                                tempactivity.ActivityType = 8;
                                tempactivity.ActivityName = "完成";
                                tempactivity.HiddenNextStep = false;
                                tempactivity.PermitSelectUser = true;
                                tempactivity.HiddenOpinion = true;
                                tempactivity.PermitSelectUser = false;
                                activitys.Add(tempactivity);
                            }
                            else
                                activitys = WorkFlowService.GetValidNextActivitys(activity.Activityid, WorkFlowUser.RealTaskUser.UserName, WorkFlowUser.RealTaskUser.RealName, WorkFlowUser.DutyLevel, SerialNo, null);
                        }
                        if ((!WorkFlowService.CheckBeforeEnter(flow.RealStartActivity, ExtraInfo, null, WorkFlowUser.UserName, "",
                                       WorkFlowUser.CompanyId, WorkFlowUser.DepartmentId, "",
                                       RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                                       RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                       RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager), out err)) && DataFormat.GetSafeBool(flow.Process.FixProcess))
                        {
                            throw new Exception(err);
                        }

                        if (phonetemplate != null)
                        {
                            Html = WorkFlowService.GetFormatedForm(phonetemplate.Templateid,
                                                Serial, 0, ParentSerial,
                                                RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                                RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                                                RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                                                ExtraInfo,
                                                processid, WorkFlowUser.RealTaskUser, param);
                        }
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        defaultusers = jss.Serialize(userids);
                        nextstep = jss.Serialize(activitys);
                    }
                }

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpWorkCreate,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = string.Format("流程id:{0},父工作流水号:{1},额外表信息:{2},返回页面:{3}", processid, ParentSerial, ExtraInfo, "手机客户端页面"),
                    Result = true
                };
                LogService.SaveLog(log);
                if (formUrl != "")
                {
                    err = "当前任务手机不能办理！";
                    throw new Exception(err);
                }
                else if (zdzdProcess)
                {
                    err = "当前任务手机不能办理！";
                    throw new Exception(err);
                }



                ret = 0;
            }
            catch (Exception e)
            {
                err = e.Message;
                ret = 1;
                SysLog4.WriteLog(e);
            }
            finally
            {

                //基本完成，需要传递参数没有传完，还有很多
                byte[] bytes = Encoding.UTF8.GetBytes(Html);
                string str = Convert.ToBase64String(bytes);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"code\":\"" + ret.ToString() + "\",\"msg\":\"" + err + "\",\"hashtml\":\"" + HasHtml + "\",\"hasoffice\":\"" + HasOffice + "\",\"serialno\":\"" + SerialNo + "\",\"nextselect\":\"\",\"processid\":\"" + ProcessId + "\",\"html\":\"" + str + "\",\"activityname\":\"" + Activityname + "\"");
                sb.Append(",\"from\":\"" + from + "\"");
                sb.Append(",\"hasoption\":\"" + hasoption + "\"");
                bytes = Encoding.UTF8.GetBytes(nextstep);
                str = Convert.ToBase64String(bytes);
                sb.Append(",\"nextstep\":\"" + str + "\"");
                bytes = Encoding.UTF8.GetBytes(defaultusers);
                str = Convert.ToBase64String(bytes);
                sb.Append(",\"defaultusers\":\"" + str + "\"");
                sb.Append(",\"isshowuser\":\"" + isshowuser + "\"");
                sb.Append(",\"activityid\":\"" + ActivityId + "\"");
                sb.Append(",\"params\":\"" + Params + "\"");

                sb.Append("}");
                rettext = sb.ToString();
                rettext = sb.ToString();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }
        //这里应该没有问题了，创建提交还没有做，这个要确认下
        public void PhoneGettaskDetail()
        {
            string err = "";
            int ret = 1;
            string rettext = "";
            string TaskStatus = "", HasHtml = "", HasOffice = "", ProcessId = "", SerialNo = "", Html = "", ActivityId = "", Activityname = "", from = "", hasoption = "", nextstep = "", defaultusers = "", isshowuser = "";
            try
            {

                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                if (!WorkFlowUser.IsLogin)
                    PhoneSetUser(username);



                int taskid = DataFormat.GetSafeInt(Request["taskid"]);
                if (taskid == 0)
                {
                    err = "无效的任务id";
                    throw new Exception(err);

                }
                StToDoTasks task = WorkFlowService.GetTodoTask(taskid);
                if (task == null)
                {
                    err = "无效的工作，可能是其他人已经完成了该工作，或者工作已撤销";
                    throw new Exception(err);
                }

                from = "来自：" + task.PreUserRealName;

                StActivity activity = WorkFlowService.GetActivity(task.CurActivityid.Value);
                //这里整个逻辑要换过，因为根据activity不能获取processid，
                StForm form = WorkFlowService.GetForm(task.SerialNo);
                ViewStProcess process = WorkFlowService.GetProcess(form.Processid.Value);// new StProcess(form.Processid.Value);
                if (process.FixProcess.Value == false)
                {
                    activity = new StActivity();
                    activity.Activityid = 0;
                    activity.Processid = process.Processid;
                    activity.ActivityName = "办理";
                    activity.HiddenNextStep = false;
                    activity.PermitSelectUser = true;
                    activity.HiddenOpinion = true;
                }
                if (activity == null && task.CurActivityid > 0)
                {
                    err = "无效的流程步骤id，可能是流程已更改";
                    throw new Exception(err);
                }
                if (DataFormat.GetSafeInt(process.UseInPhone.Value, 0) == 0)
                {
                    err = "当前任务不能通过手机办理，请使用PC端办理";
                    throw new Exception(err);
                }
                if (DataFormat.GetSafeInt(process.PhoneTemplateid.Value, 0) == 0)
                {
                    err = "当前任务没有手机模板，请联系系统管理人员！";
                    throw new Exception(err);
                }



                string formUrl = "";
                if (activity != null)
                    formUrl = BD.WorkFlow.Common.DataFormat.DecodeBase64(BD.WorkFlow.Common.DataFormat.GetSafeString(activity.FormUrl));
                bool zdzdProcess = false;
                if (formUrl == "")
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(form.Processid.Value);
                    zdzdProcess = BD.WorkFlow.Common.DataFormat.GetSafeBool(flow.Process.ZdzdProcess);

                    if (!zdzdProcess)
                    {
                        TaskStatus = BD.WorkFlow.Common.DataFormat.GetSafeInt(task.TaskStatus).ToString();

                        VOrderWork work = WorkFlowService.GetOrderWork(task.SerialNo, "", flow);

                        StFormTemplate template = WorkFlowService.GetFormTemplate(BD.WorkFlow.Common.DataFormat.GetSafeInt(work.Form.FormTemplateId));
                        HasHtml = template.TemplateType.IndexOf("1") > -1 ? "1" : "0";
                        HasOffice = (template.TemplateType.IndexOf("2") > -1 || template.TemplateType.IndexOf("3") > -1) ? "1" : "0";

                        StFormTemplate phonetemplate = WorkFlowService.GetFormTemplate(BD.WorkFlow.Common.DataFormat.GetSafeInt(process.PhoneTemplateid));


                        ProcessId = activity.Processid.ToString();
                        hasoption = activity.HiddenOpinion.Value ? "0" : "1";
                        //ViewBag.TaskId = taskid;
                        ActivityId = activity.Activityid.ToString();
                        //ViewBag.Serial = task.SerialNo;
                        Activityname = task.TaskName;
                        isshowuser = activity.PermitSelectUser.Value ? "1" : "0";

                        err = "";

                        SerialNo = work.Form.SerialNo;

                        IList<string> userids = new List<string>();
                        if (activity.Activityid <= 0)
                        {
                            userids.Add("0");
                        }
                        else
                        {
                            IList<VUser> users = RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager);
                            userids = WorkFlowService.GetNextStepDefaultUser(activity.Processid.Value, activity.Activityid, WorkFlowUser.RealTaskUser.UserName, WorkFlowUser.RealTaskUser.DepartmentId, SerialNo, users);
                        }


                        IList<StActivity> activitys = new List<StActivity>();
                        if (!activity.HiddenNextStep.Value)
                        {
                            if (activity.Activityid == 0)
                            {
                                StActivity tempactivity = new StActivity();
                                tempactivity.Activityid = 0;
                                tempactivity.Processid = 0;
                                tempactivity.ActivityName = "办理";
                                tempactivity.HiddenNextStep = false;
                                tempactivity.PermitSelectUser = true;
                                tempactivity.HiddenOpinion = true;
                                activitys.Add(tempactivity);
                                tempactivity = new StActivity();
                                tempactivity.Activityid = -1;
                                tempactivity.Processid = 0;
                                tempactivity.ActivityType = 8;
                                tempactivity.ActivityName = "完成";
                                tempactivity.HiddenNextStep = false;
                                tempactivity.PermitSelectUser = true;
                                tempactivity.HiddenOpinion = true;
                                tempactivity.PermitSelectUser = false;
                                activitys.Add(tempactivity);
                            }
                            else
                                activitys = WorkFlowService.GetValidNextActivitys(activity.Activityid, WorkFlowUser.RealTaskUser.UserName, WorkFlowUser.RealTaskUser.RealName, WorkFlowUser.DutyLevel, SerialNo, null);
                        }
                        if (!WorkFlowService.CheckBeforeEnter(activity, "", work, BD.WorkFlow.Common.WorkFlowUser.UserName, "",
                            BD.WorkFlow.Common.WorkFlowUser.CompanyId, BD.WorkFlow.Common.WorkFlowUser.DepartmentId, "",
                                     RemoteUserService.GetFlowCompanys(BD.WorkFlow.Common.WorkFlowConfig.FlowManager),
                                    RemoteUserService.GetFlowDepartments(BD.WorkFlow.Common.WorkFlowConfig.FlowManager),
                                    RemoteUserService.GetFlowUsers(BD.WorkFlow.Common.WorkFlowConfig.FlowManager), out err))
                        {
                            throw new Exception(err);
                        }

                        if (template != null)
                        {
                            string extrainfo = "";
                            if (work != null && work.Form != null)
                                extrainfo = work.Form.ExtraInfo1;
                            Html = WorkFlowService.GetFormatedForm(phonetemplate.Templateid,
                                task.SerialNo, activity.Activityid, "",
                                RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                                RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                                RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                                extrainfo,
                                activity.Processid.Value, WorkFlowUser.RealTaskUser, null);
                        }
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        defaultusers = jss.Serialize(userids);
                        nextstep = jss.Serialize(activitys);
                    }
                }

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = WorkFlowConst.LogClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = WorkFlowConst.LogModuleName,
                    Operation = WorkFlowConst.LogOpWorkCheck,
                    UserName = WorkFlowUser.UserName,
                    RealName = WorkFlowUser.RealName,
                    Remark = string.Format("流程id:{0},工作id:{1},工作名称:{2}", activity.Processid, taskid, activity.ActivityName),
                    Result = true
                };
                if (formUrl != "")
                {
                    err = "当前任务手机不能办理！";
                    throw new Exception(err);
                }
                else if (zdzdProcess)
                {
                    err = "当前任务手机不能办理！";
                    throw new Exception(err);
                }



                ret = 0;
            }
            catch (Exception e)
            {
                err = e.Message;
                ret = 1;
                SysLog4.WriteLog(e);
            }
            finally
            {

                //基本完成，需要传递参数没有传完，还有很多
                byte[] bytes = Encoding.UTF8.GetBytes(Html);
                string str = Convert.ToBase64String(bytes);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"code\":\"" + ret.ToString() + "\",\"msg\":\"" + err + "\",\"hashtml\":\"" + HasHtml + "\",\"hasoffice\":\"" + HasOffice + "\",\"serialno\":\"" + SerialNo + "\",\"nextselect\":\"\",\"processid\":\"" + ProcessId + "\",\"html\":\"" + str + "\",\"activityname\":\"" + Activityname + "\"");
                sb.Append(",\"from\":\"" + from + "\"");
                sb.Append(",\"hasoption\":\"" + hasoption + "\"");
                bytes = Encoding.UTF8.GetBytes(nextstep);
                str = Convert.ToBase64String(bytes);
                sb.Append(",\"nextstep\":\"" + str + "\"");
                bytes = Encoding.UTF8.GetBytes(defaultusers);
                str = Convert.ToBase64String(bytes);
                sb.Append(",\"defaultusers\":\"" + str + "\"");
                sb.Append(",\"isshowuser\":\"" + isshowuser + "\"");
                sb.Append(",\"activityid\":\"" + ActivityId + "\"");


                sb.Append("}");
                rettext = sb.ToString();
                rettext = sb.ToString();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }

        public void PhoneGetNextUser()
        {
            string err = ""; int activityid = DataFormat.GetSafeInt(Request["id"]);
            int nextactivityid = DataFormat.GetSafeInt(Request["nextid"]);
            int processid = DataFormat.GetSafeInt(Request["processid"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);
            IList<VUser> users = new List<VUser>();
            try
            {

                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                if (!WorkFlowUser.IsLogin)
                    PhoneSetUser(username);

                if (activityid == 0 && nextactivityid == 0)
                {
                    users = RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager);
                }
                else if (activityid == 0 && nextactivityid == -1)
                {
                    VUser tempuser = new VUser();
                    tempuser.UserId = "0";
                    tempuser.UserRealName = "自动办理";
                    users.Add(tempuser);
                }
                else
                {
                    users = WorkFlowService.GetNextActivityUsers(processid, serial, activityid, nextactivityid,
                        RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                        RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                        WorkFlowUser.RealTaskUser.UserName);
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {

                JavaScriptSerializer jss = new JavaScriptSerializer();

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(users));
                Response.End();
            }

        }

        public void phonegetoffice()
        {
            try
            {
                //string err = "";
                /*
                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                if (!WorkFlowUser.IsLogin)
                    BD.Jcbg.Web.Remote.UserService.Login(username, password, out err);
                */
                int taskId = DataFormat.GetSafeInt(Request["taskid"]);
                string serial = DataFormat.GetSafeString(Request["serial"]);

                StForm form = null;

                if (taskId > 0)
                {
                    StToDoTasks task = WorkFlowService.GetTodoTask(taskId);
                    form = WorkFlowService.GetForm(task.SerialNo);
                }
                else if (serial != "")
                {
                    form = WorkFlowService.GetForm(serial);
                }

                byte[] fileContent = null;
                string ext = "";
                if (form != null)
                {
                    fileContent = form.FormTemplate;
                    ext = form.TemplateType;
                }
                WorkFlow.Common.FormTemplateType templateType = new WorkFlow.Common.FormTemplateType(ext);
                ext = templateType.HasWord ? ".doc" : ".xls";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office" + ext);
                Response.Charset = "UTF-8";
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(fileContent);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }


        public void PhoneSubmitCreateTask()
        {
            bool ret = false;
            string msg = "";
            string serial = "";
            try
            {
                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                if (!WorkFlowUser.IsLogin)
                    PhoneSetUser(username);


                ParamReqCreateWork param = new ParamReqCreateWork(Request);
                ret = WorkFlowService.CompleteFirstTask(param, GetFormItems(), WorkFlowUser.RealTaskUser, WorkFlowUser.RealSignUser,
                    RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                    out msg);
                serial = param.Serial;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonClient.GetRetString(ret, msg, serial));
                Response.End();

            }
        }

        public void PhoneSubmitTask()
        {
            bool ret = false;
            string msg = "";
            string serial = "";
            try
            {
                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                if (!WorkFlowUser.IsLogin)
                    PhoneSetUser(username);


                ParamReqCheckWork param = new ParamReqCheckWork(Request);
                StToDoTasks todotask = WorkFlowService.GetTodoTask(param.TaskId);
                ret = WorkFlowService.CompleteCheckTask(param, GetFormItems(), WorkFlowUser.RealTaskUser, WorkFlowUser.RealSignUser,
                    RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                    out msg);

                serial = todotask.SerialNo;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonClient.GetRetString(ret, msg, serial));
                Response.End();

            }
        }


        public void PhoneGetProcess()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string companyid = DataFormat.GetSafeString(Request["companyid"]);
            if (companyid == "")
                companyid = RemoteUserService.GetFlowCompanyIds(WorkFlowUser.UserName);
            string processname = DataFormat.GetSafeString(Request["processname"]);
            string groupid = DataFormat.GetSafeString(Request["groupid"]);
            string zdzdprocess = DataFormat.GetSafeString(Request["zdzdprocess"]);
            string zdzdkey = DataFormat.GetSafeString(Request["zdzdkey"]);
            string inuse = DataFormat.GetSafeString(Request["inuse"]);

            int totalcount = 0;
            IList<VUserRole> userroles = RemoteUserService.GetUserRoles(null);
            VUser user = RemoteUserService.GetUser(WorkFlowUser.RealSignUser.UserName);
            IList<ViewStProcess> items = WorkFlowService.GetPhoneProcesses(companyid, groupid, processname, zdzdprocess, zdzdkey, inuse, pagesize, pageindex, out totalcount, user, userroles);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(items)));
        }

        #endregion

        #region 个人语句
        [Authorize]
        public void AddSelfWords()
        {
            bool code = false;
            string msg = "";
            try
            {
                IList<string> sqls = new List<string>();
                foreach (string str in Request.Form.Keys)
                {
                    string strword = DataFormat.GetSafeString(Request[str]);
                    if (strword != "")
                    {
                        sqls.Add("insert into h_cyyj(ryzh,ryxm,yjnr,nrlx) values('" + WorkFlowUser.UserName + "','" + WorkFlowUser.RealName + "','" + strword + "','ysjl')");
                    }
                }
                if (sqls.Count > 0)
                {
                    CommonService.ExecTrans(sqls);
                }
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            Response.Write(JsonClient.GetRetString(code, msg));
        }

        [Authorize]
        public JsonResult GetSelfWords()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = CommonService.GetDataTable("select recid,yjnr from h_cyyj where ryzh='" + WorkFlowUser.UserName + "' order by yjnr asc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);

            }
            return Json(ret);
        }
        #endregion

        #region 流程定义导入导出
        /// <summary>
        /// 导出流程定义
        /// </summary>
        public void ExportFlowDefine()
        {
            string key = DataFormat.GetSafeString(Request["exportprocessid"]);
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            try
            {
                int processid = DataFormat.GetSafeInt(key);
                VOrderFlow orderFlow = WorkFlowService.GetOrderFlow(processid);
                if (orderFlow.IsValid)
                {
                    // 根元素
                    XElement eleProcess = new XElement("processes",
                        new XAttribute("key", orderFlow.Process.Processid),
                        new XAttribute("name", orderFlow.Process.ProcessName),
                        new XAttribute("version", "1.0"),
                        new XAttribute("description", orderFlow.Process.ProcessDesc));
                    // 开始节点
                    if (orderFlow.StartActivity != null && orderFlow.StartActivity.ActivityId > 0)
                    {
                        XElement eleStart = new XElement("start",
                            new XAttribute("g", orderFlow.StartActivity.Position),
                            new XAttribute("name", orderFlow.StartActivity.ActivityName),
                            new XAttribute("id", orderFlow.StartActivity.ActivityGraphId),
                            new XAttribute("startConProcName", ""));
                        eleProcess.Add(eleStart);
                        // 到真正开始节点的线条
                        StActivity realStartActivity = orderFlow.RealStartActivity;
                        if (realStartActivity != null)
                        {
                            XElement eleToRealStart = new XElement("transition",
                                new XAttribute("name", orderFlow.StartActivity.TransGraphName),
                                new XAttribute("g", orderFlow.StartActivity.TransPosition),
                                new XAttribute("id", orderFlow.StartActivity.TransGraphId),
                                new XAttribute("to", realStartActivity.ActivityGraphId));
                            eleStart.Add(eleToRealStart);
                        }
                    }
                    // region 其他节点
                    int activityNum = 1;
                    foreach (StActivity activity in orderFlow.ActivityCol)
                    {
                        // 经办人信息
                        string roles = "", users = "", deps = "";
                        IList<StAssignRule> assignRules = orderFlow.GetActivityAssignRule(activity.Activityid);
                        foreach (StAssignRule assignRule in assignRules)
                        {
                            if (assignRule.BasedOn == (int)EnumExecutorBasedOn.DEPT_BASED)
                            {
                                if (deps != "")
                                    deps += "|";
                                deps += assignRule.Executor + "$" + assignRule.ExecutorName;
                            }
                            else if (assignRule.BasedOn == (int)EnumExecutorBasedOn.ROLE_BASED)
                            {
                                if (roles != "")
                                    roles += "|";
                                roles += assignRule.Executor + "$" + assignRule.ExecutorName;
                            }
                            else if (assignRule.BasedOn == (int)EnumExecutorBasedOn.USER_BASED)
                            {
                                if (users != "")
                                    users += "|";
                                users += assignRule.Executor + "$" + assignRule.ExecutorName;
                            }
                        }
                        // 活动信息
                        XElement ele = new XElement("task",
                            new XAttribute("activityid", activity.Activityid),
                            new XAttribute("id", activity.ActivityGraphId),
                            new XAttribute("name", activity.ActivityName),
                            new XAttribute("autoExecFlag", activity.ActivityType == (int)EnumActivityType.COMPLETION ? "true" : "false"),
                            new XAttribute("sameDeptNextAssignee", DataFormat.GetSafeBool(activity.LimitedSameGroup).ToString().ToLower()),
                            new XAttribute("nextAssignee", DataFormat.GetSafeBool(activity.PermitSelectUser).ToString().ToLower()),
                            new XAttribute("userExecType", activity.ExecuteType == (int)EnumExecutorType.ANY ? "1" : "2"),
                            new XAttribute("print", DataFormat.GetSafeBool(activity.IsPrint).ToString().ToLower()),
                            new XAttribute("printContent", activity.ReportName),
                            new XAttribute("conProcName", activity.PreEnterFunc),
                            new XAttribute("submitConProcName", activity.PrePostFunc),
                            new XAttribute("execProcNames", activity.AfterPostFunc),
                            new XAttribute("execType", "2"),
                            new XAttribute("execSqls", ""),
                            new XAttribute("newTask", DataFormat.GetSafeBool(activity.IsCreateStream).ToString().ToLower()),
                            new XAttribute("subProcessId", activity.Streamid),
                            new XAttribute("subProcessTaskName", activity.StreamSelect),
                            new XAttribute("hideSubProcess", DataFormat.GetSafeBool(activity.StreamHidden).ToString().ToLower()),
                            new XAttribute("execAllTask", DataFormat.GetSafeBool(activity.NextStepAllExec).ToString().ToLower()),
                            new XAttribute("alias", activity.AliasName),
                            new XAttribute("order", activityNum++.ToString()),
                            new XAttribute("hideApproval", DataFormat.GetSafeBool(activity.HiddenOpinion).ToString().ToLower()),
                            new XAttribute("hideNextSelect", DataFormat.GetSafeBool(activity.HiddenNextStep).ToString().ToLower()),
                            new XAttribute("nextDefaultUser", activity.DefaultUser),
                            new XAttribute("form", activity.GotoUrl),
                            new XAttribute("append", DataFormat.GetSafeBool(activity.UploadAttach).ToString().ToLower()),
                            new XAttribute("requestAppend", DataFormat.GetSafeBool(activity.MustUploadAttach).ToString().ToLower()),
                            new XAttribute("g", activity.Position),
                            new XAttribute("candidateUsers", users),
                            new XAttribute("candidateDepts", deps),
                            new XAttribute("candidateRoles", roles),
                            new XAttribute("workorder", DataFormat.GetSafeInt(activity.WorkOrder)),
                            new XAttribute("formUrl", DataFormat.GetSafeString(activity.FormUrl)),
                            new XAttribute("deadLine", DataFormat.GetSafeBool(activity.Deadline).ToString().ToLower()),
                            new XAttribute("deadLineCancel", DataFormat.GetSafeBool(activity.Deadlinecancel).ToString().ToLower()),
                            new XAttribute("deadLineAlert", DataFormat.GetSafeBool(activity.DeadlineAlert).ToString().ToLower()),
                            new XAttribute("deadLineTime", DataFormat.GetSafeDecimal(activity.DeadlineTime, 12))
                            );

                        // 到下一个节点的线条
                        IList<StRoutingRule> routingRules = orderFlow.GetActivityRoutingRule(activity.Activityid);
                        if (routingRules != null)
                        {
                            foreach (StRoutingRule routingRule in routingRules)
                            {
                                if (routingRule.TransPosition.Length == 0 ||
                                    routingRule.TransGraphId.Length == 0 ||
                                    routingRule.TransGraphName.Length == 0 ||
                                    routingRule.NextActivityidList.Length == 0)
                                    continue;
                                string[] arrTransPosition = routingRule.TransPosition.Split(new char[] { '|' });
                                string[] arrTransGraphid = routingRule.TransGraphId.Split(new char[] { '|' });
                                string[] arrTransGraphname = routingRule.TransGraphName.Split(new char[] { '|' });
                                string[] arrNextIds = routingRule.NextActivityidList.Split(new char[] { ',' });
                                for (int j = 0; j < arrTransPosition.Length && j < arrTransGraphid.Length && j < arrTransGraphname.Length && j < arrNextIds.Length; j++)
                                {
                                    StActivity tmpNextActivity = orderFlow.GetActivity(DataFormat.GetSafeInt(arrNextIds[j]));
                                    XElement eleTrans = new XElement("transition",
                                        new XAttribute("id", arrTransGraphid[j]),
                                        new XAttribute("name", arrTransGraphname[j]),
                                        new XAttribute("g", arrTransPosition[j]),
                                        new XAttribute("to", tmpNextActivity == null ? "" : tmpNextActivity.ActivityGraphId));
                                    ele.Add(eleTrans);
                                }
                            }
                        }
                        // 添加到process节
                        eleProcess.Add(ele);

                    }
                    // 加载到xml文件
                    doc.Add(eleProcess);
                }

                //doc.Save(Response.OutputStream);
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                resp.Clear();
                resp.Buffer = true;
                resp.Charset = "UTF-8";
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("process-" + processid, System.Text.Encoding.UTF8).ToString() + ".xml");
                resp.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                resp.ContentType = "text/xml";//设置输出文件类型为xml文件。
                resp.Write(doc.ToString());
                resp.End();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(doc.ToString());
                SysLog4.WriteLog(e);
            }


        }
        /// <summary>
        /// 导入流程定义
        /// </summary>
        public void ImportFlowDefine()
        {

            string msg = "";
            bool code = true;
            try
            {
                string key = DataFormat.GetSafeString(Request["importprocessid"]);
                int processid = DataFormat.GetSafeInt(key);
                XDocument doc = null;
                if (processid > 0)
                {

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase postfile = Request.Files[0];
                        doc = XDocument.Load(postfile.InputStream);
                        XElement root = doc.Root;
                        VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);

                        string firstActivityId = "";
                        // 解析开始节点
                        XElement startnode = root.Element("start");
                        StStartActivity startActivity = flow.StartActivity; // 开始节点
                        if (startActivity == null)
                            startActivity = new StStartActivity();
                        startActivity.Processid = processid;
                        startActivity.ActivityName = DataFormat.GetSafeString(startnode.Attribute("name").Value);
                        startActivity.Position = DataFormat.GetSafeString(startnode.Attribute("g").Value);
                        startActivity.ActivityGraphId = DataFormat.GetSafeString(startnode.Attribute("id").Value);
                        startActivity.TransPosition = "";
                        startActivity.TransGraphId = "";
                        XElement transNode = startnode.Element("transition");
                        if (transNode != null)
                        {
                            startActivity.TransPosition = DataFormat.GetSafeString(transNode.Attribute("g").Value);
                            startActivity.TransGraphId = DataFormat.GetSafeString(transNode.Attribute("id").Value);
                            startActivity.TransGraphName = DataFormat.GetSafeString(transNode.Attribute("name").Value);
                            firstActivityId = DataFormat.GetSafeString(transNode.Attribute("to").Value);
                        }
                        // 其他节点
                        IList<VStActivity> activitys = new List<VStActivity>();
                        foreach (XElement ele in root.Elements())
                        {
                            // 去掉开始节点
                            if (!ele.Name.LocalName.Equals("task", StringComparison.OrdinalIgnoreCase))
                                continue;
                            string graphyid = DataFormat.GetSafeString(ele.Attribute("id").Value);
                            StActivity activity = flow.GetActivity(graphyid);
                            if (activity == null)
                                activity = new StActivity();
                            //StActivity activity = new StActivity();

                            activity.Processid = processid;
                            activity.ActivityName = DataFormat.GetSafeString(ele.Attribute("name").Value);
                            activity.TimeAllowed = 0;
                            activity.RuleApplied = 0;
                            activity.ExPreRuleFunction = "";
                            activity.ExPostRuleFunction = "";
                            activity.ActivityType = DataFormat.GetSafeBool(ele.Attribute("autoExecFlag").Value) ? (int)EnumActivityType.COMPLETION : (int)EnumActivityType.INTER_ACTION;
                            activity.OrMergeFlag = 0;
                            activity.NumVotesNeeded = 0;
                            activity.AutoExecutive = 0;
                            activity.ActivityDesc = "";
                            activity.PermitSelectUser = DataFormat.GetSafeBool(ele.Attribute("nextAssignee").Value);
                            activity.LimitedSameGroup = DataFormat.GetSafeBool(ele.Attribute("sameDeptNextAssignee").Value);
                            activity.Method = 1;
                            activity.ExecuteType = DataFormat.GetSafeInt(ele.Attribute("userExecType").Value) == (int)EnumExecutorType.ANY ? (int)EnumExecutorType.ANY : (int)EnumExecutorType.ALL_BRANCH;
                            activity.IsPrint = DataFormat.GetSafeBool(ele.Attribute("print").Value);
                            activity.ReportName = DataFormat.GetSafeString(ele.Attribute("printContent").Value);
                            activity.PreEnterFunc = DataFormat.GetSafeString(ele.Attribute("conProcName").Value);
                            activity.PrePostFunc = DataFormat.GetSafeString(ele.Attribute("submitConProcName").Value);
                            activity.AfterPostFunc = DataFormat.GetSafeString(ele.Attribute("execProcNames").Value);
                            activity.IsCreateStream = DataFormat.GetSafeBool(ele.Attribute("newTask").Value);
                            activity.Streamid = DataFormat.GetSafeString(ele.Attribute("subProcessId").Value);
                            activity.StreamSelect = DataFormat.GetSafeString(ele.Attribute("subProcessTaskName").Value);
                            activity.StreamHidden = DataFormat.GetSafeBool(ele.Attribute("hideSubProcess").Value);
                            activity.NextStepAllExec = DataFormat.GetSafeBool(ele.Attribute("execAllTask").Value);
                            activity.AliasName = DataFormat.GetSafeString(ele.Attribute("alias").Value);
                            activity.HiddenOpinion = DataFormat.GetSafeBool(ele.Attribute("hideApproval").Value);
                            activity.HiddenNextStep = DataFormat.GetSafeBool(ele.Attribute("hideNextSelect").Value);
                            activity.DefaultUser = DataFormat.GetSafeString(ele.Attribute("nextDefaultUser").Value);
                            activity.GotoUrl = DataFormat.GetSafeString(ele.Attribute("form").Value);
                            activity.UploadAttach = DataFormat.GetSafeBool(ele.Attribute("append").Value);
                            activity.MustUploadAttach = DataFormat.GetSafeBool(ele.Attribute("requestAppend").Value);
                            activity.Position = DataFormat.GetSafeString(ele.Attribute("g").Value);
                            activity.ActivityGraphId = DataFormat.GetSafeString(ele.Attribute("id").Value);
                            activity.WorkOrder = DataFormat.GetSafeShort(ele.Attribute("workorder").Value);
                            activity.FormUrl = DataFormat.GetSafeString(ele.Attribute("formUrl").Value);
                            activity.Deadline = DataFormat.GetSafeBool(ele.Attribute("deadLine").Value);
                            activity.DeadlineAlert = DataFormat.GetSafeBool(ele.Attribute("deadLineCancel").Value);
                            activity.Deadlinecancel = DataFormat.GetSafeBool(ele.Attribute("deadLineAlert").Value);
                            activity.DeadlineTime = DataFormat.GetSafeDecimal(ele.Attribute("deadLineTime").Value, 12);

                            VStActivity vactivity = new VStActivity(activity);
                            activitys.Add(vactivity);
                            // 保存节点的用户角色部门
                            IList<StAssignRule> assignRules = new List<StAssignRule>();
                            // 角色
                            string strRoles = DataFormat.GetSafeString(ele.Attribute("candidateRoles").Value);
                            if (strRoles.Length > 0)
                            {
                                string[] arrRoles = strRoles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int j = 0; j < arrRoles.Length; j++)
                                {
                                    string[] tmpArr = arrRoles[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                    string strId = DataFormat.GetSafeString(tmpArr[0]);
                                    if (strId.Length == 0)
                                        continue;
                                    StAssignRule tmpassignRule = new StAssignRule();
                                    tmpassignRule.BasedOn = (int)EnumExecutorBasedOn.ROLE_BASED;
                                    tmpassignRule.Executor = strId;
                                    tmpassignRule.ExecutorName = RemoteUserService.GetRole(strId).RoleName;
                                    vactivity.AssignRuleCol.Add(tmpassignRule);
                                }
                            }
                            // 部门
                            string strDeps = DataFormat.GetSafeString(ele.Attribute("candidateDepts").Value);
                            if (strDeps.Length > 0)
                            {
                                string[] arr = strDeps.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int j = 0; j < arr.Length; j++)
                                {
                                    string[] tmpArr = arr[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                    string strId = DataFormat.GetSafeString(tmpArr[0]);
                                    if (strId.Length == 0)
                                        continue;
                                    StAssignRule tmpassignRule = new StAssignRule();
                                    tmpassignRule.BasedOn = (int)EnumExecutorBasedOn.DEPT_BASED;
                                    tmpassignRule.Executor = strId;
                                    tmpassignRule.ExecutorName = RemoteUserService.GetDepartment(strId).CompanyName;
                                    vactivity.AssignRuleCol.Add(tmpassignRule);
                                }
                            }
                            // 用户
                            string strUsers = DataFormat.GetSafeString(ele.Attribute("candidateUsers").Value);
                            if (strUsers.Length > 0)
                            {
                                string[] arr = strUsers.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int j = 0; j < arr.Length; j++)
                                {
                                    string[] tmpArr = arr[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                    string strId = DataFormat.GetSafeString(tmpArr[0]);
                                    if (strId.Length == 0)
                                        continue;
                                    StAssignRule tmpassignRule = new StAssignRule();
                                    tmpassignRule.BasedOn = (int)EnumExecutorBasedOn.USER_BASED;
                                    tmpassignRule.Executor = strId;
                                    tmpassignRule.ExecutorName = RemoteUserService.GetUser(strId).UserRealName;
                                    vactivity.AssignRuleCol.Add(tmpassignRule);
                                }
                            }
                            // 保存节点的流程规则
                            string transPosition = "";
                            string transGrphid = "";
                            string transGraphName = "";
                            string transTo = "";
                            foreach (XElement chileEle in ele.Elements())
                            {
                                transPosition += DataFormat.GetSafeString(chileEle.Attribute("g").Value) + "|";
                                transGrphid += DataFormat.GetSafeString(chileEle.Attribute("id").Value) + "|";
                                string tempGraphName = DataFormat.GetSafeString(chileEle.Attribute("name").Value, "");
                                if (tempGraphName == "")
                                    tempGraphName = DataFormat.EncodeBase64("1==1");

                                transGraphName += DataFormat.GetSafeString(tempGraphName) + "|";
                                transTo += DataFormat.GetSafeString(chileEle.Attribute("to").Value) + "|";
                            }
                            StRoutingRule routinRule = new StRoutingRule();
                            routinRule.PreActivityid = activity.ActivityGraphId.Equals(firstActivityId, StringComparison.OrdinalIgnoreCase) ? 0 : -1;
                            routinRule.CompletionFlag = 1;
                            routinRule.NextActivityidList = transTo;
                            routinRule.PreDependentSet = "";
                            routinRule.TransPosition = transPosition;
                            routinRule.TransGraphId = transGrphid;
                            routinRule.TransGraphName = transGraphName;
                            vactivity.RoutingRuleCol.Add(routinRule);
                        }

                        code = WorkFlowService.SaveFlowActivitys(startActivity, activitys, flow.ActivityCol);
                        msg = code ? "" : "导出流程定义失败！";
                    }
                    else
                    {
                        code = false;
                        msg = "流程定义文件不能为空！";
                    }

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 导入流程定义页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportFlow()
        {
            return View();
        }


        #endregion

        #region 直接修改流程表单
        /// <summary>
        /// 流程修改完成的记录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ModifyWork()
        {
            int Step = DataFormat.GetSafeInt(Request["Step"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);
            StForm form = WorkFlowService.GetForm(serial);
            int processid = DataFormat.GetSafeInt(form.Processid);
            string postproc = DataFormat.GetSafeString(Request["postproc"]);
            if (processid > 0)
            {
                ViewStProcess process = WorkFlowService.GetProcess(processid);
                if (process != null)
                {
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);
                    StActivity activity = flow.GetActivityByStep(Step);
                    int activityid = 0;
                    if (activity != null)
                    {
                        activityid = activity.Activityid;
                    }
                    StFormTemplate template = WorkFlowService.GetFormTemplate(DataFormat.GetSafeInt(process.FormTemplateid));
                    if (template != null)
                    {
                        ViewBag.Html = WorkFlowService.GetFormatedForm(template.Templateid, serial, activityid, "",
                            RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                            RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                            "",
                            processid,
                            WorkFlowUser.RealTaskUser, null);
                    }
                }
            }
            ViewBag.Step = Step.ToString();
            ViewBag.ReturnUrl = DataFormat.GetSafeString(Request["ReturnUrl"]);
            ViewBag.Serial = serial;
            ViewBag.postproc = postproc;
            return View();
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        [Authorize]
        public void DoModifyWork()
        {
            bool ret = false;
            string msg = "";
            int step = DataFormat.GetSafeInt(Request["step"]);
            string serial = DataFormat.GetSafeString(Request["serial"]);
            string postproc = DataFormat.GetSafeString(Request["postproc"]);
            try
            {
                ret = WorkFlowService.ModifyCompleteedTask(serial, step, postproc, GetFormItems(), WorkFlowUser.RealTaskUser, WorkFlowUser.RealSignUser,
                    RemoteUserService.GetFlowCompanys(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowDepartments(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetFlowRoles(WorkFlowConfig.FlowManager),
                    RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(WorkFlowConfig.FlowManager)),
                    out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonClient.GetRetString(ret, msg, serial));
            }
        }
        #endregion

        #region 用户签名
        public ActionResult GetUserSign()
        {
            string username = DataFormat.GetSafeString(Request["userid"]);
            return new RedirectResult("/user/getusersign?username=" + username);
        }
        #endregion


    }
}
