using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using BD.WorkFlow.IBll;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Bd.jcbg.Common;
using BD.Jcbg.Web.Common;

namespace BD.Jcbg.Web.Controllers
{
    public class OaController : Controller
    {
		#region 服务
        private BD.Jcbg.IBll.ICommonService _commonService = null;
        private BD.Jcbg.IBll.ICommonService CommonService
		{
			get
			{
				if (_commonService == null)
				{
					IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
				}
				return _commonService;
			}
		}

        private IJcjtService _jcjtService = null;
        private IJcjtService JcjtService
        {
            get
            {
                try
                {
                    if (_jcjtService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _jcjtService = webApplicationContext.GetObject("JcjtService") as IJcjtService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _jcjtService;
            }
        }

        private IOaService _oaService = null;
		private IOaService OaService
		{
			get
			{
				if (_oaService == null)
				{
					IApplicationContext webApplicationContext = ContextRegistry.GetContext();
					_oaService = webApplicationContext.GetObject("OaService") as IOaService;
				}
				return _oaService;
			}
		}
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

        private IJcService _jcService = null;
        private IJcService JcService
        {
            get
            {
                try
                {
                    if (_jcService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _jcService;
            }
        }
		#endregion

		#region 页面

        public ActionResult error()
        {
            return View();
        }
        public ActionResult uploadFile()
        {
            return View();
        }
		/// <summary>
		/// 公告管理
		/// </summary>
		/// <returns></returns>
		public ActionResult AnnounceList1()
		{
			return View();
		}
		/// <summary>
		/// 公告查看
		/// </summary>
		/// <returns></returns>
		public ActionResult AnnounceList2()
		{
			return View();
		}
		/// <summary>
		/// 公告编辑
		/// </summary>
		/// <returns></returns>
		public ActionResult AnnounceEdit()
		{
			int id = Request["id"].GetSafeInt();
			if (id > 0)
			{
				CompanyAnnounce ann = OaService.GetAnnounce(id);
				if (ann != null)
				{
					ViewBag.Recid = id;
					ViewBag.Title = ann.Title;
					ViewBag.FileIds = ann.FileIds;
					ViewBag.Body = ann.Body;

					IList<CompanyReader> readers = OaService.GetCompanyReader(CompanyEntityType.CompanyAnnounce, id.ToString());
					StringBuilder usernames = new StringBuilder();
					foreach (CompanyReader reader in readers)
					{
						if (usernames.Length > 0)
							usernames.Append(",");
						usernames.Append(reader.UserName);
					}
					ViewBag.Users = usernames.ToString();
				}
			}
			return View();
		}
		/// <summary>
		/// 公告查看
		/// </summary>
		/// <returns></returns>
		public ActionResult AnnounceView()
		{
			int id = Request["id"].GetSafeInt();
			if (id > 0)
			{
				CompanyAnnounce ann = OaService.GetAnnounce(id);
				if (ann != null)
				{
					ViewBag.Recid = id;
					ViewBag.Title = ann.Title;
					ViewBag.FileIds = ann.FileIds;
					ViewBag.Body = ann.Body;

					IList<CompanyReader> readers = OaService.GetCompanyReader(CompanyEntityType.CompanyAnnounce, id.ToString());
					StringBuilder usernames = new StringBuilder();
					foreach (CompanyReader reader in readers)
					{
						if (usernames.Length > 0)
							usernames.Append(",");
						usernames.Append(reader.UserName);
					}
					ViewBag.Users = usernames.ToString();
					// 设置已读
					if (Request["read"].GetSafeBool())
					{
						string msg = "";
						OaService.SetAnnounceRead(id, CurrentUser.UserName, out msg);
					}
				}
			}
			return View();
		}
        /// <summary>
        /// 公告查看
        /// </summary>
        /// <returns></returns>
        public ActionResult AnnounceView1()
        {
            int id = Request["id"].GetSafeInt();
            if (id > 0)
            {
                CompanyAnnounce ann = OaService.GetAnnounce(id);
                if (ann != null)
                {
                    ViewBag.Recid = id;
                    ViewBag.Title = ann.Title;
                    ViewBag.FileIds = ann.FileIds;
                    ViewBag.Body = ann.Body;

                    IList<CompanyReader> readers = OaService.GetCompanyReader(CompanyEntityType.CompanyAnnounce, id.ToString());
                    StringBuilder usernames = new StringBuilder();
                    foreach (CompanyReader reader in readers)
                    {
                        if (usernames.Length > 0)
                            usernames.Append(",");
                        usernames.Append(reader.UserName);
                    }
                    ViewBag.Users = usernames.ToString();
                    // 设置已读
                    if (Request["read"].GetSafeBool())
                    {
                        string msg = "";
                        OaService.SetAnnounceRead(id, CurrentUser.UserName, out msg);
                    }
                }
            }
            return View();
        }
		/// <summary>
		/// 邮件列表
		/// </summary>
		/// <returns></returns>
		public ActionResult MailList()
		{
			return View();
		}
		/// <summary>
		/// 写邮件
		/// </summary>
		/// <returns></returns>
		public ActionResult MailEdit()
		{
			int id = Request["id"].GetSafeInt();
			if (id > 0)
			{
				UserMail mail = OaService.GetMail(id);
				if (mail != null)
				{
					ViewBag.Recid = id;
					ViewBag.Receiver = mail.Receiver;
					ViewBag.Title = mail.Title;
					ViewBag.Content = mail.Content;
					ViewBag.Html = mail.Content.DecodeBase64();
					ViewBag.FileIds = mail.FileIds;

				}
			}
			return View();
		}
		/// <summary>
		/// 查看邮件
		/// </summary>
		/// <returns></returns>
		public ActionResult MailView()
		{
			int id = Request["id"].GetSafeInt();
			if (id > 0)
			{
				UserMail mail = OaService.GetMail(id);
				if (mail != null)
				{
					ViewBag.Recid = id;
					ViewBag.Sender = mail.SenderRealName;
					ViewBag.Receiver = mail.ReceiverRealName;
					ViewBag.Title = mail.Title;
					ViewBag.FileIds = mail.FileIds;
					ViewBag.Content = mail.Content;
					// 设置已读
					if (Request["read"].GetSafeBool())
					{
						string msg = "";
						OaService.SetMailRead(id, CurrentUser.UserName, out msg);
					}
				}
			}
			return View();
		}
        /// <summary>
        /// 查看邮件
        /// </summary>
        /// <returns></returns>
        public ActionResult MailView1()
        {
            int id = Request["id"].GetSafeInt();
            if (id > 0)
            {
                UserMail mail = OaService.GetMail(id);
                if (mail != null)
                {
                    ViewBag.Recid = id;
                    ViewBag.Sender = mail.SenderRealName;
                    ViewBag.Receiver = mail.ReceiverRealName;
                    ViewBag.Title = mail.Title;
                    ViewBag.FileIds = mail.FileIds;
                    ViewBag.Content = mail.Content;
                    // 设置已读
                    if (Request["read"].GetSafeBool())
                    {
                        string msg = "";
                        OaService.SetMailRead(id, CurrentUser.UserName, out msg);
                    }
                }
            }
            return View();
        }
		/// <summary>
		/// 文件共享页面
		/// </summary>
		/// <returns></returns>
		public ActionResult FileShare()
		{
			return View();
		}
        /// <summary>
        /// 变更录入界面，如果有id，则不能修改
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Change()
        {
            int id = Request["id"].GetSafeInt();
            ViewBag.Recid = id;
            if (id > 0)
            {
                CompanyChange change = OaService.GetChange(id);
                if (change != null)
                {
                    ViewBag.Recid = id;
                    ViewBag.Dep = change.DepartmentName;
                    ViewBag.User = change.CreatedBy;
                    ViewBag.Date = change.CreatedOn;
                }
                else
                {
                    ViewBag.Recid = 0;
                    ViewBag.Dep = CurrentUser.CurUser.DepartmentName;
                    ViewBag.User = CurrentUser.RealName;
                    ViewBag.Date = DateTime.Now.ToShortDateString();
                }
                return View();
            }
            else
                return RedirectToAction("Changelist", "Oa");
        }
        [Authorize]
        public ActionResult ChangeTotle()
        {
            int id = Request["id"].GetSafeInt();
            string createdon = "";
            string createdby = "";
            string name = "";
            if (Request["id"].GetSafeInt() > 0)
            {
                CompanyChangeTotal changetotal = OaService.GetChangeToal(id);
                if (changetotal != null)
                {
                    id = changetotal.ChangeTotalid;
                    createdon = changetotal.CreatedOn;
                    createdby = changetotal.CreatedBy;
                    name = changetotal.TotoleName;
                }
                else
                {
                    id = 0;
                    createdby = CurrentUser.RealName;
                }
            }
            else
            {
                createdby = CurrentUser.RealName;
            }
            ViewBag.Recid = id;
            ViewBag.CreatedOn = createdon;
            ViewBag.CreatedBy = createdby;
            ViewBag.Name = name;
            return View();
            
        }
        [Authorize]
        public ActionResult ChangeTotleView()
        {
            int id = Request["id"].GetSafeInt();
            string createdon = "";
            string createdby = "";
            string name = "";
            if (Request["id"].GetSafeInt() > 0)
            {
                CompanyChangeTotal changetotal = OaService.GetChangeToal(id);
                if (changetotal != null)
                {
                    id = changetotal.ChangeTotalid;
                    createdon = changetotal.CreatedOn;
                    createdby = changetotal.CreatedBy;
                    name = changetotal.TotoleName;
                }
                else
                {
                    id = 0;
                    createdby = CurrentUser.RealName;
                }
            }
            else
            {
                createdby = CurrentUser.RealName;
            }
            ViewBag.Recid = id;
            ViewBag.CreatedOn = createdon;
            ViewBag.CreatedBy = createdby;
            ViewBag.Name = name;
            return View();

        }
        /// <summary>
        /// 变更列表，查看自己部门
        /// </summary>
        /// <returns></returns>
        public ActionResult Changelist()
        {
            BD.WebListParam.ListParam param = new WebListParam.ListParam();
            param.FormDm = "CompanyChange";
            param.FormStatus = "0";
            return RedirectToAction("Index", "WebForm", param);
        }
        /// <summary>
        /// 变更列表，所有
        /// </summary>
        /// <returns></returns>
        public ActionResult Changelist2()
        {
            BD.WebListParam.ListParam param = new WebListParam.ListParam();
            param.FormDm = "CompanyChange";
            param.FormStatus = "1";
            return RedirectToAction("Index", "WebForm", param);
        }
        /// <summary>
        /// 新增变更
        /// </summary>
        /// <returns></returns>
        public ActionResult AddChange()
        {
            //BD.DataInputBll
            BD.DataInputParam.DataParam param = new DataInputParam.DataParam();
            param.zdzdtable = "ZDZD";
            param.t1_tablename = "CompanyChange";
            param.t1_pri = "ChangeID";
            param.t1_title = "建设项目设计变更明细表";
            param.t2_tablename = "CompanyChangeItem";
            param.t2_pri = "ChangeID,ChangeItemID";
            param.t2_title = "变更明细";
            param.t2_num = "1";
            param.button = "提交|TJ|/oa/changelist||返回|HH|/oa/changelist";
            return RedirectToAction("Index", "DataEntry", param);
        }

        /// <summary>
        /// 变更列表，只能查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeTotallist()
        {
            BD.WebListParam.ListParam param = new WebListParam.ListParam();
            param.FormDm = "CompanyChangeTotal";
            param.FormStatus = "0";
            return RedirectToAction("Index", "WebForm", param);
        }
        /// <summary>
        /// 变更列表，能修改
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeTotallist2()
        {
            BD.WebListParam.ListParam param = new WebListParam.ListParam();
            param.FormDm = "CompanyChangeTotal";
            param.FormStatus = "1";
            return RedirectToAction("Index", "WebForm", param);
        }


        //页面还没有写完，保存到以后，暂时设定不能修改
		#endregion

		#region 获取信息
		/// <summary>
		/// 获取部门用户树
		/// </summary>
		[Authorize]
		public void GetUserTree()
		{
			var depcode = Request["depcode"].GetSafeString();
			var nocompany = Request["nocompany"].GetSafeString();
			StringBuilder sb = new StringBuilder();
			try
			{
				// 获取部门
                IList<RemoteUserService.VUser> users = BD.Jcbg.Web.Remote.UserService.FileShareUsers;

				List<KeyValuePair<string,string>> deps = new List<KeyValuePair<string,string>>();
                foreach (RemoteUserService.VUser user in users)
				{
					var q = from e in deps where e.Key.Equals(user.DEPCODE, StringComparison.OrdinalIgnoreCase) select e;
					if (q.Count() == 0 && (depcode == "" || user.DEPCODE == depcode))
						deps.Add(new KeyValuePair<string, string>(user.DEPCODE, user.DEPNAME));
				}
				if (nocompany == "")
					sb.Append("[{\"id\":\"" + CurrentUser.CompanyCode + "\",\"text\":\"" + CurrentUser.CompanyName + "\",\"children\":");
				sb.Append("[");
				foreach (KeyValuePair<string,string> dep in deps)
				{
					sb.Append("{\"id\":\""+dep.Key+"\",\"text\":\""+dep.Value+"\"");
                    var q = from e in users where e.DEPCODE.Equals(dep.Key, StringComparison.OrdinalIgnoreCase) orderby e.DEPNAME ascending, e.POSTDM descending, e.REALNAME ascending select e;
					if (q.Count() > 0)
					{
						sb.Append(",\"children\":[");
						foreach (RemoteUserService.VUser user in q)
						{
							if (user != q.First())
								sb.Append(",");
							sb.Append("{\"id\":\"" + user.USERCODE + "\",\"text\":\"" + user.REALNAME + "\"}");
						}
						sb.Append("]");
					}
					sb.Append("},");					
				}
				if (sb.ToString().EndsWith(","))
					sb.Remove(sb.Length - 1, 1);
				sb.Append("]");
				if (nocompany == "")
					sb.Append("}]");

			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				
				Response.Write(sb.ToString());
				Response.End();
			}
		}
		/// <summary>
		/// 获取管理的公告列表
		/// </summary>
		[Authorize]
		public void GetManageAnnounce()
		{
			IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
			int totalcount = 0;
			try
			{
				int pageindex = Request["page"].GetSafeInt(1);
				int pagesize = Request["rows"].GetSafeInt(10);
				
				datas = OaService.GetPageAnnounces("", Request["key"].GetSafeString(),
					Request["dt1"].GetSafeString(), Request["dt2"].GetSafeString(), pagesize, pageindex, out totalcount);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
			}
		}
		/// <summary>
		/// 获取公告列表
		/// </summary>
		[Authorize]
		public void GetAnnounce()
		{
			IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
			int totalcount = 0;
			try
			{
				int pageindex = Request["page"].GetSafeInt(1);
				int pagesize = Request["rows"].GetSafeInt(10);
				string hasread = Request["hasread"].GetSafeString();
				string key = Request["key"].GetSafeString();

				datas = OaService.GetPageAnnounces(CurrentUser.UserName,key, hasread, pagesize, pageindex, out totalcount);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
			}
		}
		/// <summary>
		/// 获取邮件列表
		/// </summary>
		[Authorize]
		public void GetMails()
		{
			IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
			int totalcount = 0;
			try
			{
				int pageindex = Request["page"].GetSafeInt(1);
				int pagesize = Request["rows"].GetSafeInt(10);
				string hasread = Request["hasread"].GetSafeString();
				string key = Request["key"].GetSafeString();
				string foldertype = Request["folder"].GetSafeString(MailFolderType.ReceiveBox);
				string dt1 = Request["dt1"].GetSafeString();
				string dt2 = Request["dt2"].GetSafeString();

				datas = OaService.GetPageMails( foldertype, CurrentUser.UserName, key, hasread, dt1, dt2, pagesize, pageindex, out totalcount);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
			}
		}

		/// <summary>
		/// 获取邮件夹
		/// </summary>
		[Authorize]
		public void GetMailFolders()
		{
			IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
			try
			{
				datas = OaService.GetMailFolders(CurrentUser.UserName);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(jss.Serialize(datas));
			}
		}
		/// <summary>
		/// 获取共享文件列表
		/// </summary>
		[Authorize]
		public void GetShareFile()
		{
			IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
			try
			{
				datas = OaService.GetShareFiles(
					Request["folderid"].GetSafeString(),
					Request["foldertype"].GetSafeString(ShareFolderType.All),
					CurrentUser.UserName,
					Request["key"].GetSafeString());
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(jss.Serialize(datas));
			}
		}
        /// <summary>
        /// 获取新邮件数量
        /// </summary>
        [Authorize]
        public void GetNewMailSum()
        {
            int ret = 0;
            try
            {
                ret = OaService.GetNewMailSum(CurrentUser.UserName);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(true, ret.ToString()));
            }
        }
		/// <summary>
		/// 获取文件共享对象
		/// </summary>
		[Authorize]
		public void GetShareFileReader()
		{
			IList<CompanyReader> readers = new List<CompanyReader>();
			try
			{
				string filetype = Request["filetype"].GetSafeString();
				string entity = "";
				if (filetype == ShareFileType.File)
					entity = CompanyEntityType.UserShareFile;
				else if (filetype == ShareFileType.Folder)
					entity = CompanyEntityType.UserShareFileFolder;
				readers = OaService.GetCompanyReader(entity, Request["id"].ToString());
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				JavaScriptSerializer jss = new JavaScriptSerializer();
				Response.Write(jss.Serialize(readers));
			}
		}
		/// <summary>
		/// 获取共享文件夹，如果传入文件夹，除掉该文件夹的所有子文件夹
		/// </summary>
		[Authorize]
		public void GetShareFolders()
		{
			int fileid = Request["id"].GetSafeInt();
			string filetype = Request["filetype"].GetSafeString();

			string excludeid = "";
			if (filetype == ShareFileType.Folder)
				excludeid = fileid.ToString();
			string ret = "";
			try
			{
				IList<UserShareFileFolder> folders = OaService.GetShareFolders(CurrentUser.UserName);

				TreeOrder<UserShareFileFolder> tree = new TreeOrder<UserShareFileFolder>(folders, "Recid", "ParentId", "0", excludeid, "FolderName");
				ret = tree.GetJsonTree();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			finally
			{
				Response.ContentType = "text/plain";
				Response.Write(ret);
				Response.End();
			}

		}
		#endregion

		#region 文件操作
		/// <summary>
		/// 保存文件
		/// </summary>
		public void SaveFile()
		{
			bool code = false;
			string msg = "";
			string ctrlid = Request["ctrlid"].GetSafeString();		// 客户端控件名
			string filename = Request["file_name"].GetSafeString();	// 输入的文件名
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
					SysFileImage fimage = OaService.GetFileImage(postfile.FileName, false);
					CompanyFileStorage file = new CompanyFileStorage()
					{
						CreatedTime = DateTime.Now,
						FileContent = postcontent,
						FileDesc = filename,
						FileName = postfile.FileName,
						FileSize = readlength,
						FileTypeDesc = fimage == null ? "" : fimage.FileDesc,
						ImageName = fimage == null ? "" : fimage.ImageName,
						RealName = CurrentUser.RealName,
						UserName = CurrentUser.UserName
					};
					string err = "";
					file = OaService.SaveFile(file, out err);
					code = err.Length == 0;
					if (err != "")
						msg = "文件保存失败";
					else
						msg = file.Recid.ToString();
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
		public void DeleteFile()
		{
			bool ret = false;
			string msg = "";
			try
			{
				int fileid = Request["id"].GetSafeInt();
				ret = OaService.DeleteFile(fileid, out msg);
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

		/// <summary>
		/// 查看文件页面
		/// </summary>
		/// <returns></returns>
		public void FileView()
		{
			string filename = "";
			long filesize = 0;
			byte[] ret = null;
			int fileid = Request["id"].GetSafeInt();
			try
			{
				CompanyFileStorage file = OaService.GetCompanyFileStorage(fileid);

				if (file != null)
				{
					ret = file.FileContent;
					filename = file.FileDesc;
					filesize = file.FileSize.Value;

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

		#endregion

		#region 其他操作
		/// <summary>
		/// 保存公告
		/// </summary>
		[Authorize]
		public void SaveAnnounce()
		{
			bool code = true;
			string msg = "";
			
			try
			{
				int recid = Request["recid"].GetSafeInt();
				string title = Request["title"].GetSafeString();
				string readers = Request["readers"].GetSafeString();
				string content = Request["content"].GetSafeString();
				// 读者
				IList<CompanyReader> lstreaders = new List<CompanyReader>();
				if (readers != "")
				{
					string[] arrreader = readers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string struser in arrreader)
					{
						var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.USERCODE.Equals(struser, StringComparison.OrdinalIgnoreCase) select e;
						if (q.Count() > 0)
						{
							RemoteUserService.VUser vuser = q.First();
							CompanyReader reader = new CompanyReader()
							{
								HasReader = false,
								HasDelete = false,
								ParentEntity = CompanyEntityType.CompanyAnnounce,
								UserName = vuser.USERCODE,
								RealName = vuser.REALNAME
							};
							lstreaders.Add(reader);
						}
					}
				}
				// 公告
				CompanyAnnounce ann = new CompanyAnnounce();
				if (recid > 0)
					ann = OaService.GetAnnounce(recid);
				ann.Title = title;
				ann.Body = content;
				ann.UserName = CurrentUser.UserName;
				ann.RealName = CurrentUser.RealName;
				ann.CreatedTime = DateTime.Now;
				ann.FileIds = Request["fj"].GetSafeString();

				code = OaService.SaveAnnounce(ann, lstreaders, out msg);

			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				code = false;
				msg = e.Message;
			}
			finally
			{
				Response.Write(JsonFormat.GetRetString(code, msg));
			}
		}
		/// <summary>
		/// 删除公告
		/// </summary>
		[Authorize]
		public void DeleteAnnounce()
		{
			bool code = true;
			string msg = "";
			try
			{
				code = OaService.DeleteAnnounce(Request["id"].GetSafeInt(), out msg);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				code = false;
				msg = e.Message;
			}
			finally
			{
				Response.Write(JsonFormat.GetRetString(code, msg));
			}
		}
		/// <summary>
		/// 保存公告
		/// </summary>
		[Authorize]
        public void SaveMail()
        {
            bool code = true;
            string msg = "";

            try
            {
                int recid = Request["recid"].GetSafeInt();
                string title = Request["title"].GetSafeString();
                string readers = Request["readers"].GetSafeString();
                string content = Request["content"].GetSafeString();
                // 读者
                IList<CompanyReader> lstreaders = new List<CompanyReader>();
                if (readers != "")
                {
                    string[] arrreader = readers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string struser in arrreader)
                    {
                        var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.USERCODE.Equals(struser, StringComparison.OrdinalIgnoreCase) select e;
                        if (q.Count() > 0)
                        {
                            RemoteUserService.VUser vuser = q.First();
                            CompanyReader reader = new CompanyReader()
                            {
                                HasReader = false,
                                HasDelete = false,
                                ParentEntity = CompanyEntityType.CompanyAnnounce,
                                UserName = vuser.USERCODE,//vuser.USERNAME,
                                RealName = vuser.REALNAME
                            };
                            lstreaders.Add(reader);
                        }
                    }
                }
                // 公告
                UserMail mail = new UserMail();
                if (recid > 0)
                    mail = OaService.GetMail(recid);
                mail.Sender = CurrentUser.UserName;
                mail.SenderRealName = CurrentUser.RealName;
                mail.Receiver = readers;
                mail.ReceiverRealName = BD.Jcbg.Web.Remote.UserService.GetUserRealName(readers);
                mail.Folderid = (Request["ls"].GetSafeInt() == 1 ? MailFolderType.DraftBox : MailFolderType.SendBox).GetSafeInt();
                mail.Title = title;
                mail.Content = content;
                mail.SendTime = DateTime.Now;
                mail.ContentSize = 0;
                mail.HasSend = mail.Folderid.GetSafeString() == MailFolderType.SendBox;
                mail.HasDelete = false;
                mail.FileIds = Request["fj"].GetSafeString();

                code = OaService.SaveMail(mail, lstreaders, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
		/// <summary>
		/// 删除公告
		/// </summary>
		[Authorize]
		public void DeleteMail()
		{
			bool code = true;
			string msg = "";
			try
			{
				code = OaService.DeleteMail(Request["id"].GetSafeInt(), 
					Request["readerid"].GetSafeInt(),
					CurrentUser.UserName, out msg);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				code = false;
				msg = e.Message;
			}
			finally
			{
				Response.Write(JsonFormat.GetRetString(code, msg));
			}
		}

		/// <summary>
		/// 创建共享文件夹
		/// </summary>
		[Authorize]
		public void AddFolder()
		{
			string msg = "";
			UserShareFileFolder folder = null;
			try
			{
				folder = new UserShareFileFolder()
				{
					CreatedTime = DateTime.Now,
					FolderName = Request["foldername"].GetSafeString(),
					ParentId = Request["parentid"].GetSafeInt(),
					RealName = CurrentUser.RealName,
					UserName = CurrentUser.UserName
				};
				folder = OaService.SaveFileFolder(folder, out msg);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				msg = e.Message;
			}
			finally
			{
				if (folder != null && folder.Recid>0)
					Response.Write(JsonFormat.GetRetString(0, OaService.GetShareFileRow(true, folder.Recid)));
				else
					Response.Write(JsonFormat.GetRetString(1, msg));
			}
		}
		/// <summary>
		/// 上传文件
		/// </summary>
		[Authorize]
		public void AddShareFile()
		{
			string msg = "";
			UserShareFile sfile = null;
			CompanyFileStorage file = null;
			try
			{
				if (Request.Files.Count > 0)
				{
					string filename = "";
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
					SysFileImage fimage = OaService.GetFileImage(postfile.FileName, false);
					file = new CompanyFileStorage()
					{
						CreatedTime = DateTime.Now,
						FileContent = postcontent,
						FileDesc = filename,
						FileName = postfile.FileName,
						FileSize = readlength,
						FileTypeDesc = fimage == null ? "" : fimage.FileDesc,
						ImageName = fimage == null ? "" : fimage.ImageName,
						RealName = CurrentUser.RealName,
						UserName = CurrentUser.UserName
					};
					file = OaService.SaveFile(file, out msg);
					if (file.Recid > 0)
					{
						sfile = new UserShareFile()
						{
							FolderId = Request["parentid"].GetSafeInt(),
							FileId = file.Recid
						};
						sfile = OaService.SaveFile(sfile, out msg);
					}
				}

				
				
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				msg = e.Message;
			}
			finally
			{
				if (sfile != null && sfile.Recid > 0)
					Response.Write(JsonFormat.GetRetString(0, OaService.GetShareFileRow(false, sfile.Recid)));
				else
					Response.Write(JsonFormat.GetRetString(1, msg));
			}
		}
		/// <summary>
		/// 删除文件
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public void DeleteShareFile()
		{
			bool ret = true;
			string msg = "";
			try
			{
				ret = OaService.DeleteFile(Request["id"].GetSafeInt(), CurrentUser.UserName, out msg);
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
		/// <summary>
		/// 删除文件夹
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public void DeleteShareFolder()
		{
			bool ret = true;
			string msg = "";
			try
			{
				int folderid = Request["id"].GetSafeInt();
				if (OaService.GetShareFiles(folderid.ToString(), ShareFolderType.Normal, CurrentUser.UserName, "").Count > 0)
				{
					ret = false;
					msg = "当前文件夹包含文件，请先删除文件";
				}
				else
					ret = OaService.DeleteFileFolder(folderid, CurrentUser.UserName, out msg);
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
		/// <summary>
		/// 文件重命名
		/// </summary>
		[Authorize]
		public void RenameShareFile()
		{
			bool ret = true;
			string msg = "";
			try
			{
				int fileid = Request["id"].GetSafeInt();
				string filename = Request["filename"].GetSafeString();
				string filetype = Request["filetype"].GetSafeString();
				if (filename == "")
				{
					ret = false;
					msg = "文件名不能为空";
				}
				else
				{
					if (filetype == ShareFileType.File)
						ret = OaService.RenameShareFileName(fileid, filename, CurrentUser.UserName, out msg);
					else if (filetype == ShareFileType.Folder)
						ret = OaService.RenameShareFileFolderName(fileid, filename, CurrentUser.UserName, out msg);
					else
					{
						ret = false;
						msg = "无效的文件类型";
					}
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
		/// <summary>
		/// 文件共享
		/// </summary>
		[Authorize]
		public void SaveShareFileReader()
		{
			bool code = true;
			string msg = "";

			try
			{
				int recid = Request["fileshare_recid"].GetSafeInt();
				string filetype = Request["fileshare_filetype"].GetSafeString();
				string readers = Request["fileshare_readers"].GetSafeString();
				// 读者
				IList<CompanyReader> lstreaders = new List<CompanyReader>();
				if (readers != "")
				{
					string[] arrreader = readers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string struser in arrreader)
					{
						var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.USERCODE.Equals(struser, StringComparison.OrdinalIgnoreCase) select e;
						if (q.Count() > 0)
						{
							RemoteUserService.VUser vuser = q.First();
							CompanyReader reader = new CompanyReader()
							{
								HasReader = false,
								HasDelete = false,
								ParentEntity = CompanyEntityType.CompanyAnnounce,
								UserName = vuser.USERCODE,
								RealName = vuser.REALNAME
							};
							lstreaders.Add(reader);
						}
					}
				}

				if (filetype == ShareFileType.File)
					code = OaService.ShareFile(recid, lstreaders, out msg);
				else if (filetype == ShareFileType.Folder)
					code = OaService.ShareFileFolder(recid, lstreaders, out msg);
				else
				{
					code = false;
					msg = "无效的文件类型";
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
				Response.Write(JsonFormat.GetRetString(code, msg));
			}
		}
		/// <summary>
		/// 设置父目录
		/// </summary>
		[Authorize]
		public void SaveShareFileParent()
		{
			bool code = true;
			string msg = "";

			try
			{
				int recid = Request["fileshare_moveto_recid"].GetSafeInt();
				string filetype = Request["fileshare_moveto_filetype"].GetSafeString();
				int folderid = Request["fileshare_folder_tree"].GetSafeInt();

				if (filetype == ShareFileType.File)
					code = OaService.SaveShareFileFileFolder(recid, folderid, CurrentUser.UserName, out msg);
				else if (filetype == ShareFileType.Folder)
					code = OaService.SaveShareFileFolderParent(recid, folderid, CurrentUser.UserName, out msg);
				else
				{
					code = false;
					msg = "无效的文件类型";
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
				Response.Write(JsonFormat.GetRetString(code, msg));
			}
		}
		#endregion

        #region 变更公示操作
        /// <summary>
        /// 变更显示
        /// </summary>
        public void GetChangeItem()
        {
            IList<CompanyChangeItem> readers = new List<CompanyChangeItem>();
            try
            {
                
                //readers = OaService.GetCompanyReader(entity, Request["id"].ToString());
                readers = OaService.GetCompanyChangeItem(Request["id"].GetSafeInt());
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(readers));
            }
        }

        /// <summary>
        /// 导出变更excel
        /// </summary>
        public void GetChangeItemExcel()
        {
            string excelText = "";
            int id = Request["id"].GetSafeInt();
            ViewBag.Recid = id;
            try
            {
                CompanyChange change = OaService.GetChange(id);
                if (change != null)
                {
                    int count_c_no = 0;
                    decimal count_c_money = 0;
                    int count_b_no = 0;
                    decimal count_b_money = 0;
                    int count_a_no = 0;
                    decimal count_a_money = 0;
                    int count_big_no = 0;
                    decimal count_big_money = 0;
                    int count_all_no = 0;
                    decimal count_all_money = 0;

                    string c_text = "";
                    string b_text = "";
                    string a_text = "";
                    string big_text = "";
                    IList<CompanyChangeItem> readers = new List<CompanyChangeItem>();
                    readers = OaService.GetCompanyChangeItem(Request["id"].GetSafeInt());
                    //编号 发生时间 变更地点、原因和主要内容 主要工程量 变更金额 审批情况 备注
                    foreach (CompanyChangeItem item in readers)
                    {
                        if (item.ChangeType.Value == 1)
                        {
                            c_text += "<tr><td>" + item.ChangeNo + "</td><td>" + item.ChangeDate + "</td><td>" + item.ChangeContent + "</td><td>" + item.ChangeWork + "</td><td>" + item.ChangeMoney.Value.ToString() + "</td><td>" + item.ChangeApprove + "</td><td>" + item.ChangeRemark + "</td></tr>";
                            count_c_no++;
                            count_c_money = count_c_money + item.ChangeMoney.Value.GetSafeDecimal();
                        }
                        else if (item.ChangeType.Value == 2)
                        {
                            b_text += "<tr><td>" + item.ChangeNo + "</td><td>" + item.ChangeDate + "</td><td>" + item.ChangeContent + "</td><td>" + item.ChangeWork + "</td><td>" + item.ChangeMoney.Value.ToString() + "</td><td>" + item.ChangeApprove + "</td><td>" + item.ChangeRemark + "</td></tr>";
                            count_b_no++;
                            count_b_money = count_b_money + item.ChangeMoney.Value.GetSafeDecimal();
                        }
                        else if (item.ChangeType.Value == 3)
                        {
                            a_text += "<tr><td>" + item.ChangeNo + "</td><td>" + item.ChangeDate + "</td><td>" + item.ChangeContent + "</td><td>" + item.ChangeWork + "</td><td>" + item.ChangeMoney.Value.ToString() + "</td><td>" + item.ChangeApprove + "</td><td>" + item.ChangeRemark + "</td></tr>";
                            count_a_no++;
                            count_a_money = count_a_money + item.ChangeMoney.Value.GetSafeDecimal();
                        }
                        else
                        {
                            big_text += "<tr><td>" + item.ChangeNo + "</td><td>" + item.ChangeDate + "</td><td>" + item.ChangeContent + "</td><td>" + item.ChangeWork + "</td><td>" + item.ChangeMoney.Value.ToString() + "</td><td>" + item.ChangeApprove + "</td><td>" + item.ChangeRemark + "</td></tr>";
                            count_big_no++;
                            count_big_money = count_big_money + item.ChangeMoney.Value.GetSafeDecimal();
                        }
                        count_all_no++;
                        count_all_money = count_all_money + item.ChangeMoney.Value.GetSafeDecimal();
                    }
                    if (c_text != "")
                    {
                        c_text = c_text.Remove(0, 4);
                        c_text = "<tr><td rowspan='" + (count_c_no + 1).ToString() + "' style='width: 85px;'>C类一般设计变更</td>" + c_text;
                        c_text += "<tr><td style='height: 19px;'>合计:" + count_c_no.ToString() + "</td><td></td><td></td><td></td><td>" + count_c_money.ToString() + "</td><td></td><td></td></tr>";
                    }
                    if (b_text != "")
                    {
                        b_text = b_text.Remove(0, 4);
                        b_text = "<tr><td rowspan='" + (count_b_no + 1).ToString() + "' style='width: 85px;'>B类一般设计变更</td>" + b_text;
                        b_text += "<tr><td style='height: 19px;'>合计:" + count_b_no.ToString() + "</td><td></td><td></td><td></td><td>" + count_b_money.ToString() + "</td><td></td><td></td></tr>";
                    }
                    if (a_text != "")
                    {
                        a_text = a_text.Remove(0, 4);
                        a_text = "<tr><td rowspan='" + (count_a_no + 1).ToString() + "' style='width: 85px;'>A类一般设计变更</td>" + a_text;
                        a_text += "<tr><td style='height: 19px;'>合计:" + count_a_no.ToString() + "</td><td></td><td></td><td></td><td>" + count_a_money.ToString() + "</td><td></td><td></td></tr>";
                    }
                    if (big_text != "")
                    {
                        big_text = big_text.Remove(0, 4);
                        big_text = "<tr><td rowspan='" + (count_big_no + 1).ToString() + "' style='width: 85px;'>重大设计变更</td>" + big_text;
                        big_text += "<tr><td style='height: 19px;'>合计:" + count_big_no.ToString() + "</td><td></td><td></td><td></td><td>" + count_big_money.ToString() + "</td><td></td><td></td></tr>";
                    }

                    excelText = "<table border='1' cellpadding='0' cellspacing='0' style='width:1013px; text-align:center; border:'><tbody> <tr> <td colspan='8' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>建设项目设计变更明细表</td> </tr> <tr align='left'> <td colspan='5' style='height: 28px;'>合同段名称：" + change.DepartmentName + "</td> <td>填报人：" + change.CreatedBy + "</td> <td>填报时间：</td> <td>" + change.CreatedOn + "</td> </tr> <tr> <td style='width: 85px; height: 190px;'></td> <td>编号</td> <td>发生时间</td> <td>变更地点、原因和主要内容</td> <td>主要工程量</td> <td>变更金额</td> <td>审批情况</td> <td>备注</td> </tr>";
                    excelText = excelText + c_text + b_text + a_text + big_text;
                    excelText += "<tr> <td style='height: 19px;'>合计</td> <td>" + count_all_no.ToString() + "</td> <td> </td> <td> </td> <td> </td> <td>" + count_all_money.ToString() + "</td> <td> </td> <td> </td></tr></tbody> </table><span style='color:rgb(255, 0, 0)'> 说明：新增设计变更请在备注栏标明。</span> ";


                }
                else
                {
                    excelText = "<table border='1' cellpadding='0' cellspacing='0' style='width:1013px; text-align:center; border:'><tbody> <tr> <td colspan='8' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>建设项目设计变更明细表</td> </tr> <tr align='left'> <td colspan='4' style='height: 28px;'>合同段名称：</td> <td></td> <td>填报人：</td> <td>填报时间：</td> <td></td> </tr> <tr> <td style='width: 85px; height: 190px;'></td> <td>编号</td> <td>发生时间</td> <td>变更地点、原因和主要内容</td> <td>主要工程量</td> <td>变更金额</td> <td>审批情况</td> <td>备注</td> </tr>";
                    excelText += "<tr> <td colspan='8' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>数据出错，记录不存在或者已删除！</td> </tr><tr> <td style='height: 19px;'>合计</td> <td></td> <td> </td> <td> </td> <td> </td> <td></td> <td> </td> <td> </td></tr></tbody> </table><span style='color:rgb(255, 0, 0)'> 说明：新增设计变更请在备注栏标明。</span> ";

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                excelText = "<table border='1' cellpadding='0' cellspacing='0' style='width:1013px; text-align:center; border:'><tbody> <tr> <td colspan='8' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>建设项目设计变更明细表</td> </tr> <tr align='left'> <td colspan='4' style='height: 28px;'>合同段名称：</td> <td></td> <td>填报人：</td> <td>填报时间：</td> <td></td> </tr> <tr> <td style='width: 85px; height: 190px;'></td> <td>编号</td> <td>发生时间</td> <td>变更地点、原因和主要内容</td> <td>主要工程量</td> <td>变更金额</td> <td>审批情况</td> <td>备注</td> </tr>";
                excelText += "<tr> <td colspan='8' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>数据出错，" + e.Message + "！</td> </tr><tr> <td style='height: 19px;'>合计</td> <td></td> <td> </td> <td> </td> <td> </td> <td></td> <td> </td> <td> </td></tr></tbody> </table><span style='color:rgb(255, 0, 0)'> 说明：新增设计变更请在备注栏标明。</span> ";

            }
            
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToShortTimeString() + ".xls");
            Response.Write(excelText);
            Response.End();
        }

        /// <summary>
        /// 导出变更汇总excel
        /// </summary>
        public void GetChangeTotalItemExcel()
        {
            string excelText = "";
            int id = Request["id"].GetSafeInt();
            ViewBag.Recid = id;
            try
            {
                CompanyChangeTotal change = OaService.GetChangeToal(id);
                if (change != null)
                {
                    excelText = "<table border='1' cellpadding='0' cellspacing='0' style='width:1041px'><tbody><tr><td colspan='16' style='width: 1041px; height: 42px;text-align:center; font-size:x-large; font-weight:bolde'>建设项目设计变更汇总表</td></tr><tr><td style='height: 28px;'>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan='2'>填报人：" + change.CreatedBy + "</td><td colspan='3'>填报时间：" + change.CreatedOn + "</td></tr><tr><td rowspan='2' style='height: 38px;'>序号</td><td rowspan='2' style='width: 45px;'>工程类别</td><td rowspan='2'>合同段</td><td rowspan='2' style='width: 81px;'>原合同总价（万元）</td><td rowspan='2' style='width: 85px;'>暂列金（万元）</td><td rowspan='2' style='width: 59px;'>时间段</td><td colspan='2'>C类一般设计变更</td><td colspan='2'>B类一般设计变更</td><td colspan='2'>A类一般设计变更</td><td colspan='2'>重大设计变更</td><td colspan='2'>合计</td></tr><tr><td style='height: 19px;'>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td></tr>";
                    IList<CompanyChangeTotalItem> readers = OaService.GetCompanyChangeTotalItem(change.ChangeTotalid);
                    int countno=0;
                    foreach (CompanyChangeTotalItem item in readers)
                    {
                        countno++;
                        excelText += "<tr><td rowspan='2'>" + countno.ToString() + "</td><td rowspan='2' style='width: 45px;'>" + item.ProjectType + "</td><td rowspan='2'>" + item.DepartmentName + "</td><td rowspan='2'>" + item.OldMoney + "</td><td rowspan='2'>" + item.TempMoney + "</td><td>本季度</td><td>" + item.Cno.Value.GetSafeString() + "</td><td>" + item.Cmoney.Value.GetSafeString() + "</td><td>" + item.BigNo.Value.GetSafeString() + "</td><td>" + item.Bmoney.Value.GetSafeString() + "</td><td>" + item.Ano.Value.GetSafeString() + "</td><td>" + item.Amoney.Value.GetSafeString() + "</td><td>" + item.BigNo.Value.GetSafeString() + "</td><td>" + item.BigMoney.Value.GetSafeString() + "</td><td>" + item.AlllNo.Value.ToString() + "</td><td>" + item.AllMoney.Value.GetSafeString() + "</td></tr><tr><td style='height: 20px;'>累计</td><td>" + item.CtotalNo.Value.GetSafeString() + "</td><td>" + item.CtotalMoney.Value.GetSafeString() + "</td><td>" + item.BtotalNo.Value.GetSafeString() + "</td><td>" + item.BtotalMoney.Value.GetSafeString() + "</td><td>" + item.AtotalNo.Value.GetSafeString() + "</td><td>" + item.AtotalMoney.Value.GetSafeString() + "</td><td>" + item.BigTotalNo.Value.ToString() + "</td><td>" + item.BigTotalMoney.Value.ToString() + "</td><td>" + item.TotalNo.Value.GetSafeString() + "</td><td>" + item.TotalMoney.Value.GetSafeString() + "</td></tr>";
                    }
                    excelText += "<tr><td rowspan='2' colspan='2' style='width: 45px;'>项目合计</td><td rowspan='2'></td><td rowspan='2'>" + change.OldMoney + "</td><td rowspan='2'>" + change.TempMoney + "</td><td>本季度</td><td>" + change.Cno.Value.GetSafeString() + "</td><td>" + change.Cmoney.Value.GetSafeString() + "</td><td>" + change.BigNo.Value.GetSafeString() + "</td><td>" + change.Bmoney.Value.GetSafeString() + "</td><td>" + change.Ano.Value.GetSafeString() + "</td><td>" + change.Amoney.Value.GetSafeString() + "</td><td>" + change.BigNo.Value.GetSafeString() + "</td><td>" + change.BigMoney.Value.GetSafeString() + "</td><td>" + change.AlllNo.Value.ToString() + "</td><td>" + change.AllMoney.Value.GetSafeString() + "</td></tr><tr><td style='height: 20px;'>累计</td><td>" + change.CtotalNo.Value.GetSafeString() + "</td><td>" + change.CtotalMoney.Value.GetSafeString() + "</td><td>" + change.BtotalNo.Value.GetSafeString() + "</td><td>" + change.BtotalMoney.Value.GetSafeString() + "</td><td>" + change.AtotalNo.Value.GetSafeString() + "</td><td>" + change.AtotalMoney.Value.GetSafeString() + "</td><td>" + change.BigTotalNo.Value.ToString() + "</td><td>" + change.BigTotalMoney.Value.ToString() + "</td><td>" + change.TotalNo.Value.GetSafeString() + "</td><td>" + change.TotalMoney.Value.GetSafeString() + "</td></tr>";
                    excelText += "<span style='color:rgb(255, 0, 0)'> 说明：1、累计项为自开工累计数量。</span>";
                    
                }
                else
                {
                    excelText = "<table border='1' cellpadding='0' cellspacing='0' style='width:1041px'><tbody><tr><td colspan='16' style='width: 1041px; height: 42px;text-align:center; font-size:x-large; font-weight:bolde'>建设项目设计变更汇总表</td></tr><tr><td style='height: 28px;'>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan='2'>填报人：</td><td>&nbsp;</td><td colspan='2'>填报时间：</td></tr><tr><td rowspan='2' style='height: 38px;'>序号</td><td rowspan='2' style='width: 45px;'>工程类别</td><td rowspan='2'>合同段</td><td rowspan='2' style='width: 81px;'>原合同总价（万元）</td><td rowspan='2' style='width: 85px;'>暂列金（万元）</td><td rowspan='2' style='width: 59px;'>时间段</td><td colspan='2'>C类一般设计变更</td><td colspan='2'>B类一般设计变更</td><td colspan='2'>A类一般设计变更</td><td colspan='2'>重大设计变更</td><td colspan='2'>合计</td></tr><tr><td style='height: 19px;'>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td></tr>";
                    excelText += "<tr> <td colspan='16' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>数据出错，记录不存在或者已删除！</td> </tr></tbody> </table><span style='color:rgb(255, 0, 0)'> 说明：1、累计项为自开工累计数量。</span> ";

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                excelText = "<table border='1' cellpadding='0' cellspacing='0' style='width:1041px'><tbody><tr><td colspan='16' style='width: 1041px; height: 42px;text-align:center; font-size:x-large; font-weight:bolde'>建设项目设计变更汇总表</td></tr><tr><td style='height: 28px;'>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan='2'>填报人：</td><td>&nbsp;</td><td colspan='2'>填报时间：</td></tr><tr><td rowspan='2' style='height: 38px;'>序号</td><td rowspan='2' style='width: 45px;'>工程类别</td><td rowspan='2'>合同段</td><td rowspan='2' style='width: 81px;'>原合同总价（万元）</td><td rowspan='2' style='width: 85px;'>暂列金（万元）</td><td rowspan='2' style='width: 59px;'>时间段</td><td colspan='2'>C类一般设计变更</td><td colspan='2'>B类一般设计变更</td><td colspan='2'>A类一般设计变更</td><td colspan='2'>重大设计变更</td><td colspan='2'>合计</td></tr><tr><td style='height: 19px;'>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td><td>项数</td><td>金额（万元）</td></tr>";
                excelText += "<tr> <td colspan='16' style='width: 1013px; height: 42px; text-align:center; font-size:x-large; font-weight:bolder'>数据出错，" + e.Message + "！</td> </tr></tbody> </table><span style='color:rgb(255, 0, 0)'> 说明：1、累计项为自开工累计数量。</span> ";

            }

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToShortTimeString() + ".xls");
            Response.Write(excelText);
            Response.End();
        }
        /// <summary>
        /// 获取变更汇总详细
        /// </summary>
        public void GetChangeTotal()
        {
            IList<CompanyChangeTotalItem> readers = new List<CompanyChangeTotalItem>();
            try
            {

                //readers = OaService.GetCompanyReader(entity, Request["id"].ToString());
                readers = OaService.GetCompanyChangeTotalItem(Request["id"].GetSafeInt());
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(readers));
            }
        }
        #endregion

        public void GetChangeSum()
        {
            string start = Request["start"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd");
            string end = Request["end"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd");
            IList<CompanyChangeTotalItem> readers = new List<CompanyChangeTotalItem>();
            try
            {
                IList<CompanyChange> changes=new List<CompanyChange>();
                changes = OaService.GetCompanyChange(start, end);
                foreach (CompanyChange change in changes)
                {
                    int tempno = 0;
                    decimal tempmoney = 0;
                    int allno = 0;
                    decimal allmoney = 0;
                    int totalno = 0;
                    decimal totalmoney = 0;
                    CompanyChangeTotalItem totalitem = new CompanyChangeTotalItem();
                    totalitem.ProjectType = "路面";
                    totalitem.DepartmentName = change.DepartmentName;
                    totalitem.DepartmentId = change.DepartmentId;
                    totalitem.OldMoney = "0";
                    totalitem.TempMoney = "0";

                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeSum(change.Changeid, 1, out tempno, out tempmoney);
                    totalitem.Cno = tempno;
                    totalitem.Cmoney = tempmoney;
                    allno = allno + tempno;
                    allmoney = allmoney + tempmoney;
                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeAllSum(change.DepartmentId, 1, out tempno, out tempmoney);
                    totalitem.CtotalNo = tempno;
                    totalitem.CtotalMoney = tempmoney;
                    totalno = totalno + tempno;
                    totalmoney = totalmoney + tempmoney;

                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeSum(change.Changeid, 2, out tempno, out tempmoney);
                    totalitem.Bno = tempno;
                    totalitem.Bmoney = tempmoney;
                    allno = allno + tempno;
                    allmoney = allmoney + tempmoney;
                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeAllSum(change.DepartmentId, 2, out tempno, out tempmoney);
                    totalitem.BtotalNo = tempno;
                    totalitem.BtotalMoney = tempmoney;
                    totalno = totalno + tempno;
                    totalmoney = totalmoney + tempmoney;

                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeSum(change.Changeid, 3, out tempno, out tempmoney);
                    totalitem.Ano = tempno;
                    totalitem.Amoney = tempmoney;
                    allno = allno + tempno;
                    allmoney = allmoney + tempmoney;
                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeAllSum(change.DepartmentId, 3, out tempno, out tempmoney);
                    totalitem.AtotalNo = tempno;
                    totalitem.AtotalMoney = tempmoney;
                    totalno = totalno + tempno;
                    totalmoney = totalmoney + tempmoney;

                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeSum(change.Changeid, 4, out tempno, out tempmoney);
                    totalitem.BigNo = tempno;
                    totalitem.BigMoney = tempmoney;
                    allno = allno + tempno;
                    allmoney = allmoney + tempmoney;
                    tempno = 0;
                    tempmoney = 0;
                    OaService.GetCompanyChangeAllSum(change.DepartmentId, 4, out tempno, out tempmoney);
                    totalitem.BigTotalNo = tempno;
                    totalitem.BigTotalMoney = tempmoney;
                    totalno = totalno + tempno;
                    totalmoney = totalmoney + tempmoney;


                    totalitem.AlllNo = allno;
                    totalitem.AllMoney = allmoney;
                    totalitem.TotalNo = totalno;
                    totalitem.TotalMoney = totalmoney;

                    readers.Add(totalitem);


                }
 
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(readers));
            }
        }

        public void SaveChangeSum()
        {
            bool code = false;
            string msg = "";
            try
            {
                int id = Request["id"].GetSafeInt();
                int isAnnounce = Request["isAnnounce"].GetSafeInt();
                string updated = Request.Form["updated"].GetSafeString();
                string createdby = Request.Form["createdby"].GetSafeString();
                string title = Request.Form["title"].GetSafeString();
                string createdon = Request.Form["createdon"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                IList<CompanyChangeTotalItem> readers = jss.Deserialize<IList<CompanyChangeTotalItem>>(updated);
                int totalid = 0;
                code = OaService.SaveCompanyChangeAllSum(id, out  totalid, title, createdby, createdon, readers, out msg);

                if (isAnnounce == 1 && code)
                {
                    IList<CompanyReader> lstreaders = new List<CompanyReader>();
                    foreach (RemoteUserService.VUser vuser in BD.Jcbg.Web.Remote.UserService.Users)
                    {
                        CompanyReader reader = new CompanyReader()
                        {
                            HasReader = false,
                            HasDelete = false,
                            ParentEntity = CompanyEntityType.CompanyAnnounce,
                            UserName = vuser.USERCODE,
                            RealName = vuser.REALNAME
                        };
                        lstreaders.Add(reader);
                    }

                    string content = "<a href=\"/oa/changetotleview?id=" + totalid + "\" target=\"_self\">点击查看</a>";
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    CompanyAnnounce ann = new CompanyAnnounce();
                    ann.Title = title;
                    ann.Body = Convert.ToBase64String(bytes);
                    ann.UserName = CurrentUser.UserName;
                    ann.RealName = CurrentUser.RealName;
                    ann.CreatedTime = DateTime.Now;

                    OaService.SaveAnnounce(ann, lstreaders, out msg);
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
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

            
        }

        /// <summary>
        /// 获取部门用户树
        /// </summary>
        [Authorize]
        public void GetDeps()
        {
            var depcode = Request["depcode"].GetSafeString();
            var nocompany = Request["nocompany"].GetSafeString();
            StringBuilder sb = new StringBuilder();
            try
            {
                // 获取部门
                IList<RemoteUserService.VUser> users = BD.Jcbg.Web.Remote.UserService.FileShareUsers;

                List<KeyValuePair<string, string>> deps = new List<KeyValuePair<string, string>>();
                foreach (RemoteUserService.VUser user in users)
                {
                    var q = from e in deps where e.Key.Equals(user.DEPCODE, StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() == 0 && (depcode == "" || user.DEPCODE == depcode))
                        deps.Add(new KeyValuePair<string, string>(user.DEPCODE, user.DEPNAME));
                }
                if (nocompany == "")
                    sb.Append("[{\"id\":\"" + CurrentUser.CompanyCode + "\",\"text\":\"" + CurrentUser.CompanyName + "\",\"children\":");
                sb.Append("[");
                foreach (KeyValuePair<string, string> dep in deps)
                {
                    sb.Append("{\"id\":\"" + dep.Key + "\",\"text\":\"" + dep.Value + "\"");

                    sb.Append("},");
                }
                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                if (nocompany == "")
                    sb.Append("}]");

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

                Response.Write(sb.ToString());
                Response.End();
            }
        }

        #region 手机操作

        public void PhoneGetMailFolders()
        {
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                datas = OaService.GetMailFolders(CurrentUser.UserName);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(datas));
            }
        }

        public void phonegetmail()
        {
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            int totalcount = 0;
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                int pageindex = Request["page"].GetSafeInt(1);
                int pagesize = Request["rows"].GetSafeInt(10);
                string hasread = Request["hasread"].GetSafeString();
                string key = Request["key"].GetSafeString();
                string foldertype = Request["folder"].GetSafeString(MailFolderType.ReceiveBox);
                string dt1 = Request["dt1"].GetSafeString();
                string dt2 = Request["dt2"].GetSafeString();

                datas = OaService.GetPageMails(foldertype, CurrentUser.UserName, key, hasread, dt1, dt2, pagesize, pageindex, out totalcount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        public void phonedeletemail()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                string deleteids = Request["ids"].GetSafeString();
                string[] item = deleteids.Split(',');
                for (int i = 0; i < item.Length; i++)
                {
                    string[] ids = item[i].Split('|');
                    if (ids.Length == 2)
                    {
                        ret = OaService.DeleteMail(ids[0].GetSafeInt(),
                            ids[1].GetSafeInt(),
                            CurrentUser.UserName, out err);
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = false;
            }
            finally
            {
                rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }

        public void viewmail()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            UserMail mail = null;
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                int id = Request["id"].GetSafeInt();
                mail = OaService.GetMail(id);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                err = jss.Serialize(mail);
                ret = true;
                string msg = "";
                OaService.SetMailRead(id, CurrentUser.UserName, out msg);
            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }

        public void getUpPorblem()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);

            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }


        }

        
        public void getProblemList()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);

                int currno = Request["countsms"].GetSafeInt();

                int page = (currno / 20) + 1;
                string gcbh = Request["gcbh"].GetSafeString();

                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                string sql = "select * from I_S_GC_Problem where (status=3 or username='" + CurrentUser.UserName + "') order by time desc";

                if (gcbh != "")
                    sql = "select * from I_S_GC_Problem where gcbh='" + gcbh + "' and (status=3 or username='" + CurrentUser.UserName + "') order by time desc";
                int totalcount = 0;
                datas = CommonService.GetPageData(sql, 20, page, out totalcount);
                string datatext = "";
                foreach (IDictionary<string, string> item in datas)
                {
                    if (datatext != "")
                        datatext += ",";
                    string detailtext = "";

                    IList<IDictionary<string, string>> details = CommonService.GetDataTable("select * from I_S_GC_ProblemDetail where ProblemId='" + item["problemid"].GetSafeString() + "'");
                    foreach (IDictionary<string, string> itm in details)
                    {
                        if (detailtext != "")
                            detailtext += ",";
                        detailtext += "{\"attach\":\"" + itm["recid"].GetSafeString() + "\",\"name\":\"" + itm["attachment"].GetSafeString() + "\",\"type\":\"" + itm["type"].GetSafeString() + "\",\"comment\":\"" + itm["comment"].GetSafeString() + "\"}";
                    }
                    datatext += "{\"problemid\":\"" + item["recid"].GetSafeString() + "\",\"address\":\"\",\"comment\":\"" + item["comment"].GetSafeString() + "\",\"projectid\":\"" + item["gcbh"].GetSafeString() + "\",\"projectname\":\"" + item["gcmc"].GetSafeString() + "\",\"time\":\"" + item["time"].GetSafeString() + "\",\"title\":\"" + item["title"].GetSafeString() + "\",\"username\":\"" + item["realname"].GetSafeString() + "\",\"data\":[" + detailtext + "]}";


                }
                int countsms = 0;
                if (page * 20 >= totalcount)
                    countsms = totalcount;
                else
                    countsms = page * 20;
                ret = true;
                rettext = "{\"success\":" + ret.ToString().ToLower() + ",\"countsms\":\"" + countsms.ToString() + "\",\"time\":\"" + DateTime.Now.ToShortDateString() + "\",\"data\":[" + datatext + "]}";


            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                //rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }


        public void getMapProblemList()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string datatext = "";
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);

                string zoom = Request["zoom"].GetSafeString();
                string GCMC = Request["projectname"].GetSafeString();
                string lon = Request["lon"].GetSafeDouble().ToString();
                string lat = Request["lat"].GetSafeDouble().ToString();
                DateTime date1 = Request["date1"].GetSafeDate(DateTime.MinValue);
                DateTime date2 = Request["date2"].GetSafeDate(DateTime.MinValue);
                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                string sql = "select top 10 RECID,GCMC,GCBH,title,RealName,lon,lat,comment,problemid from I_S_GC_Problem  where (ABS(CONVERT(DECIMAL(12,6),lon)-CONVERT(DECIMAL(12,6),'" + lon + "'))+ABS(CONVERT(DECIMAL(12,6),lat)-CONVERT(DECIMAL(12,6),'" + lat + "')))<0.01 and (status=3 or username='" + CurrentUser.UserName + "')";

                if (date1 != DateTime.MinValue)
                {
                    sql += " and convert(datetime,time)>='"+date1.ToString("yyyy-MM-dd")+"'";
                }
                if (date2 != DateTime.MinValue)
                {
                    sql += " and convert(datetime,time)<'" + date2.AddDays(1).ToString("yyyy-MM-dd") + "'";
                }
                if (GCMC != "")
                {
                    sql += " and GCMC like '%" + GCMC + "%'";
                }

                datas = CommonService.GetDataTable(sql);
                
                foreach (IDictionary<string, string> item in datas)
                {
                    if (datatext != "")
                        datatext += ",";

                    int totlecount=0;
                    IList<IDictionary<string, string>> dt = CommonService.GetPageData("select RECID from I_S_GC_ProblemDetail where type='img' and ProblemId='" + item["problemid"] + "'", 1, 1, out totlecount);
                    string tempid = "";
                    if (dt.Count > 0)
                        tempid = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/oa/DloadImg?id=" + dt[0]["recid"].GetSafeString();

                    datatext += "{\"title\":\"" + item["title"] + "\",\"lon\":\"" + item["lon"] + "\",\"lat\":\"" + item["lat"] + "\",\"usercode\":\"" + item["realname"] + "\",\"comment\":\"" + item["comment"] + "\",\"problemid\":\"" + item["recid"] + "\",\"count\":\"" + totlecount.ToString() + "\",\"attach\":\"" + tempid + "\"}";
                }
                ret = true;


            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                //rettext = JsonFormat.GetRetString(ret, err);

                rettext = "{\"success\":" + ret.ToString().ToLower() + ",\"msg\":\"" + err + "\",\"data\":[" + datatext + "]}";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }

        public void DloadImg()
        {
            byte[] ret = null;
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                /*
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);*/
                int id = Request["id"].GetSafeInt();


                //int fileid = 0;
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2("select thumbattachment,attachment from I_S_GC_ProblemDetail where RECID=" + id);
                if (dt.Count > 0)
                {
                    ret = dt[0]["thumbattachment"] as byte[];
                    string filename = dt[0]["attachment"].GetSafeString();
                    Response.Clear();
                    Response.ContentType = "image/jpeg jpeg jpg jpe";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
            }
        }

        public void DloadVoice()
        {
            byte[] ret = null;
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                /*
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);*/
                int id = Request["id"].GetSafeInt();


                //int fileid = 0;
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2("select thumbattachment,attachment from I_S_GC_ProblemDetail where RECID=" + id);
                if (dt.Count > 0)
                {
                    ret = dt[0]["thumbattachment"] as byte[];
                    string filename = dt[0]["attachment"].GetSafeString();
                    Response.Clear();
                    Response.ContentType = "video/mpeg4";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
            }
        }

        public void DloadBigImg()
        {
            byte[] ret = null;
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                /*
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);*/
                int id = Request["id"].GetSafeInt();

                int fileid = 0;
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2("select RealFileID from I_S_GC_ProblemDetail where RECID=" + id);
                if (dt.Count > 0)
                {
                    fileid = dt[0]["realfileid"].GetSafeInt();
                    StFile file = WorkFlowService.GetFile(fileid);

                    if (file != null)
                    {
                        ret = file.FileContent;
                        string filename = file.FileOrgName;

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
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
            }
        }



        public void uploadProblem()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);

                if (username != "")
                {
                    
                    string GCBH = Request["projectid"].GetSafeString();
                    string GCMC = Request["projectname"].GetSafeString();
                    string ProblemId = Guid.NewGuid().ToString();
                    string lon = Request["longitude"].GetSafeDouble().ToString();
                    string lat = Request["latitude"].GetSafeDouble().ToString();
                    string comment = Request["comment"].GetSafeString(); ;
                    string time = Request["time"].GetSafeString(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    string title = Request["title"].GetSafeString(); ;
                    string UserName = CurrentUser.UserName;
                    string RealName = CurrentUser.RealName;
                    DateTime CreatedOn = DateTime.Now;
                    int Status = Request["qxselect"].GetSafeInt(1);
                    string serial = Request["serial"].GetSafeString(); ;

                    if (GCBH == "-001")
                    {
                        ret = true;
                    }
                    else
                    {
                        string sql = "INSERT INTO I_S_GC_Problem(GCBH,GCMC,ProblemId,lon,lat,comment,time,title,UserName,RealName,CreatedOn,Status,WORKSERIAL)VALUES('" + GCBH + "','" + GCMC + "','" + ProblemId + "','" + lon + "','" + lat + "','" + comment + "','" + time + "','" + title + "','" + UserName + "','" + RealName + "','" + CreatedOn.ToString("yyyy-MM-dd HH:mm") + "'," + Status + ",'" + serial + "')";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        ret = CommonService.ExecTrans(sqls, out err);
                        if (ret)
                        {
                            //Request.Files
                            if (title.Trim() != "")
                            {
                                string sqlstr = "INSERT INTO I_S_GC_ProblemDetail (ProblemId,width,height,comment,type,RealFileID,Status)VALUES(@ProblemId,@width,@height,@comment,@type,@RealFileID,@Status)";
                                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                IDataParameter sqlparam = new SqlParameter("@ProblemId", ProblemId);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@width", "0");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@height", "0");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@comment", title);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@type", "txt");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@RealFileID", "");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@Status", Status);
                                sqlparams.Add(sqlparam);

                                CommonService.ExecTrans(sqlstr, sqlparams, out err);
                            }
                            if (comment.Trim() != "")
                            {
                                string sqlstr = "INSERT INTO I_S_GC_ProblemDetail (ProblemId,width,height,comment,type,RealFileID,Status)VALUES(@ProblemId,@width,@height,@comment,@type,@RealFileID,@Status)";
                                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                IDataParameter sqlparam = new SqlParameter("@ProblemId", ProblemId);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@width", "0");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@height", "0");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@comment", comment);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@type", "txt");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@RealFileID", "");
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@Status", Status);
                                sqlparams.Add(sqlparam);

                                CommonService.ExecTrans(sqlstr, sqlparams, out err);
                            }


                            for (int i = 0; i < Request.Files.Count; i++)
                            {
                                HttpPostedFileBase postfile = Request.Files[i];
                                string filename = "";
                                if (filename == "")
                                    filename = postfile.FileName;

                                /*string filepath = Server.MapPath("/pi/" + filename);

                                postfile.SaveAs(filepath);*/

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
                                    Formid = 0
                                };
                                file = WorkFlowService.SaveFile(file);
                                if (file != null)
                                {
                                    string width = "0";
                                    string height = "0";
                                    string attachment = filename;
                                    byte[] thumbattachment = null;
                                    string type = "";
                                    int RealFileID = file.Fileid;

                                    MyImage img = new MyImage(postcontent);

                                    if (!img.IsImage())
                                    {
                                        type = "voice";
                                        thumbattachment = postcontent;
                                    }
                                    else
                                    {
                                        /*
                                        System.Drawing.Image tempimage = System.Drawing.Image.FromStream(postfile.InputStream, true);
                                        width = tempimage.Width.ToString();//宽
                                        height = tempimage.Height.ToString();//高
                                        thumbattachment = img.ConvertToJpg(400, 400);*/
                                        thumbattachment = img.GetThumbnail();
                                        type = "img";
                                    }

                                    string sqlstr = "INSERT INTO I_S_GC_ProblemDetail (ProblemId,width,height,attachment,thumbattachment,type,RealFileID,Status)VALUES(@ProblemId,@width,@height,@attachment,@thumbattachment,@type,@RealFileID,@Status)";
                                    IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                    IDataParameter sqlparam = new SqlParameter("@ProblemId", ProblemId);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@width", width);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@height", height);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@attachment", attachment);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@thumbattachment", thumbattachment);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@type", type);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@RealFileID", RealFileID);
                                    sqlparams.Add(sqlparam);
                                    sqlparam = new SqlParameter("@Status", Status);
                                    sqlparams.Add(sqlparam);

                                    CommonService.ExecTrans(sqlstr, sqlparams, out err);
                                }



                            }
                        }
                    }
                }
                else
                {
                    ret = false;
                    err = "用户丢失，请重新登录以后上传";
                }

            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {

                rettext = "{\"success\":" + ret.ToString().ToLower() + ",\"msg\":\"" + err + "\"}";
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }






        public void getProblemDetail()
        {
            string rettext = "";
            //string ret = "";
            string err = "";
            string datatext = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                /*
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);*/
                int id = Request["problemid"].GetSafeInt();

                string sql = "";
                string mode = Request["mode"].GetSafeString();

                if (mode == "right")
                {
                    sql = "select top 1 * from I_S_GC_Problem where RECID<" + id;
                }
                else if (mode == "right")
                {
                    sql = "select top 1 * from I_S_GC_Problem where RECID>" + id;
                }
                else
                {
                    sql = "select * from I_S_GC_Problem where RECID=" + id;
                }

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string detailtext = "";

                    IList<IDictionary<string, string>> details = CommonService.GetDataTable("select * from I_S_GC_ProblemDetail where ProblemId='" + dt[0]["problemid"].GetSafeString() + "'");
                    foreach (IDictionary<string, string> itm in details)
                    {
                        if (detailtext != "")
                            detailtext += ",";
                        detailtext += "{\"attach\":\"" + itm["recid"].GetSafeString() + "\",\"name\":\"" + itm["attachment"].GetSafeString() + "\",\"type\":\"" + itm["type"].GetSafeString() + "\"}";
                    }
                    datatext += "{\"problemid\":\"" + dt[0]["recid"].GetSafeString() + "\",\"address\":\"\",\"comment\":\"" + dt[0]["comment"].GetSafeString() + "\",\"projectid\":\"" + dt[0]["gcbh"].GetSafeString() + "\",\"projectname\":\"" + dt[0]["gcmc"].GetSafeString() + "\",\"time\":\"" + dt[0]["time"].GetSafeString() + "\",\"title\":\"" + dt[0]["title"].GetSafeString() + "\",\"username\":\"" + dt[0]["realname"].GetSafeString() + "\",\"lon\":\"" + dt[0]["lon"].GetSafeString() + "\",\"lat\":\"" + dt[0]["lat"].GetSafeString() + "\",\"data\":[" + detailtext + "]}";


                    rettext = "{\"success\":true,\"problem\":" + datatext + "}";
                }
                else
                {
                    rettext="{\"success\":false,\"msg\":\"问题已到顶！\"}";
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
                rettext="{\"success\":false,\"msg\":\""+err+"！\"}";
            }
            finally
            {

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }



        public void getProjectList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            if (!CurrentUser.IsLogin)
                Remote.UserService.Login(username, password, out msg);
            if (CurrentUser.UserName == "")
                Remote.UserService.Login(username, password, out msg);

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";
                string sql = string.Format("select zhlx from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string zhlx = dt[0]["zhlx"];
                    if (zhlx.Equals("Q", StringComparison.OrdinalIgnoreCase))
                    {
                        strwhere += string.Format(" and gcbh in (select gcbh from view_gc_qy_zh where zhdm='{0}') ", CurrentUser.UserName);
                    }
                    else if (zhlx.Equals("R", StringComparison.OrdinalIgnoreCase))
                    {
                        strwhere += string.Format(" and gcbh in (select gcbh from view_gc_ry_zh where zhdm='{0}') ", CurrentUser.UserName);
                    }
                }
                if (key != "")
                {
                    strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                }
                sql = " from View_I_M_GC where ZT not in ('LR') " + strwhere + " order by gcbh desc";
                if (type == "lxgc")
                {
                    sql = "select GCBH,GCMC,ZJDJH " + sql;
                    CommonService.GetPageData(sql, 1, 1, out totalcount);
                    pagesize = totalcount;

                }
                else
                {
                    sql = "select * " + sql;
                }
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }



        public void getProjectDetail()
        {
            bool ret = false;
            string rettext = "";
            string err = "";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();


            try
            {

                int id = Request["id"].GetSafeInt();

                datas = CommonService.GetDataTable("select * from View_I_M_GC where RECID=" + id.ToString());
                if (datas.Count > 0)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    IDictionary<string, string> data = datas[0];
                    string gcbh = data["gcbh"].GetSafeString();
                    IList<IDictionary<string, string>> items = new List<IDictionary<string, string>>();
                    items = CommonService.GetDataTable("select * from I_S_GC_FGC where GCBH='" + gcbh + "'");
                    data.Add("fgclist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_JSDW where GCBH='" + gcbh + "'");
                    data.Add("jsdwlist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_JSRY where GCBH='" + gcbh + "'");
                    data.Add("jsrylist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_KCDW where GCBH='" + gcbh + "'");
                    data.Add("kcdwlist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_KCRY where GCBH='" + gcbh + "'");
                    data.Add("kcrylist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_SJDW where GCBH='" + gcbh + "'");
                    data.Add("sjdwlist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_SJRY where GCBH='" + gcbh + "'");
                    data.Add("sjrylist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_SGDW where GCBH='" + gcbh + "'");
                    data.Add("sgdwlist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_SGRY where GCBH='" + gcbh + "'");
                    data.Add("sgrylist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_JLDW where GCBH='" + gcbh + "'");
                    data.Add("jldwlist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_JLRY where GCBH='" + gcbh + "'");
                    data.Add("jlrylist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_TSDW where GCBH='" + gcbh + "'");
                    data.Add("tsdwlist", jss.Serialize(items));
                    items = CommonService.GetDataTable("select * from I_S_GC_TSRY where GCBH='" + gcbh + "'");
                    data.Add("tsrylist", jss.Serialize(items));


                    err = jss.Serialize(data);
                    ret = true;
                }
                else
                {
                    ret = false;
                    err = "记录不存在！";
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }

        
        public void getReportList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";

            if (!CurrentUser.IsLogin)
                Remote.UserService.Login(username, password, out msg);
            if (CurrentUser.UserName == "")
                Remote.UserService.Login(username, password, out msg);

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string strwhere = "";
                string sql = string.Format("select zhlx from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string zhlx = dt[0]["zhlx"];
                    if (zhlx.Equals("Q", StringComparison.OrdinalIgnoreCase))
                    {
                        strwhere += string.Format(" and gcbh in (select gcbh from view_gc_qy_zh where zhdm='{0}') ", CurrentUser.UserName);
                    }
                    else if (zhlx.Equals("R", StringComparison.OrdinalIgnoreCase))
                    {
                        strwhere += string.Format(" and gcbh in (select gcbh from view_gc_ry_zh where zhdm='{0}') ", CurrentUser.UserName);
                    }
                }
                if (gcbh != "")
                {
                    strwhere += " and GCBH='" + gcbh + "'";
                }
                if (key != "")
                {
                    strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or ExtraInfo1 like '%" + key + "%' or ExtraInfo2 like '%" + key + "%' or ExtraInfo3 like '%" + key + "%' or ExtraInfo4 like '%" + key + "%' or ExtraInfo5 like '%" + key + "%')"; 
                    if(type == "JGYSSQ")
                    {
                        strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%')";
                    }
                }
                sql = " select * from dbo.View_JDBG_JDJL where LX='" + type + "' " + strwhere + " order by SY_LRSJ desc";
                if (type == "JGYSSQ")
                {
                    sql = " select * from dbo.view_jdbg_yssqjl where YSSQLX='" + type + "' " + strwhere + " order by gcyssj desc";
                }
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }


        public void getReportDetail()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string type = Request["type"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                int id = Request["id"].GetSafeInt();
                if (type == "JGYSSQ")
                {
                    datas = CommonService.GetDataTable("select * from dbo.view_jdbg_yssqjl where RECID=" + id.ToString());
                }
                else
                {
                    datas = CommonService.GetDataTable("select * from dbo.View_JDBG_JDJL where RECID=" + id.ToString());
                }
                
                if (datas.Count > 0)
                {
                    IDictionary<string, string> data = datas[0];
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    err = jss.Serialize(data);
                    ret = true;
                }
                else
                {
                    ret = false;
                    err = "记录不存在！";
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }



        public ActionResult getReportDown()
        {
             IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            string filename="";
            string serial="";
            string lx = "";
            //int id = Request["id"].GetSafeInt();
            string qstring = Request["id"].GetSafeString();
            string[] t = qstring.Split('$');
            int id = t[0].GetSafeInt();
            bool canprint = true;
            try
            {
                string type = Request["type"].GetSafeString();
                if (type == "JGYSSQ")
                {
                    datas = CommonService.GetDataTable("select * from dbo.view_jdbg_yssqjl where RECID=" + id.ToString());
                    if (datas.Count > 0)
                    {
                        IDictionary<string, string> data = datas[0];
                        filename = data["reportfile"];
                        serial = data["workserial"];
                        lx ="JGYSSQJL";
                    }
                }
                else
                {
                    datas = CommonService.GetDataTable("select * from dbo.View_JDBG_JDJL where RECID=" + id.ToString());
                    if (datas.Count > 0)
                    {
                        IDictionary<string, string> data = datas[0];
                        filename = data["reportfile"];
                        serial = data["workserial"];
                        lx = data["lx"];
                        // 监督报告与整改单是否需要签章
                        if (lx == "JDBG")
                        {
                            string extrainfo8 = data["extrainfo8"].GetSafeString();
                            canprint = extrainfo8 == "1";
                        }
                        else if (lx == "ZGD")
                        {
                            string lrrxm = data["lrrxm"].GetSafeString();
                            canprint = lrrxm!="";
                        }
                    }
                }
                
                if (t.Length > 1)
                {
                    if (t[1].GetSafeString() != "")
                    {
                        filename = t[1].GetSafeString();
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return new RedirectResult("/jdbg/FlowReport?reportfile=" + filename + "&serial=" + serial + "&type=download" + "&jdjlid="+ id + "&reporttype=" + lx + "&print=" + (canprint?"1":"0"));
            //return RedirectToAction("FlowReportDown", "jdbg", new { reportfile = "%E7%9B%91%E7%9D%A3%E6%96%B9%E6%A1%88v1", serial = 20170116009 });

        }

        /// <summary>
        /// 手机端流程中查看报表
        /// </summary>
        /// <returns></returns>
        public ActionResult getReportDown2()
        {
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            string filename = "";
            string serial = "";
            string reporttype = "";
            string reportfile = "";
            int print = 0;
            int id = 0;

            try
            {
                serial = Request["serial"].GetSafeString();
                reporttype = Request["reporttype"].GetSafeString();
                reportfile = Request["reportfile"].GetSafeString();
                print = Request["print"].GetSafeInt(1);
                string[] t = serial.Split(new char[] { '*' });
                serial = t[0];
                datas = CommonService.GetDataTable("select * from dbo.View_JDBG_JDJL where workserial='" + serial + "'");
                if (datas.Count > 0)
                {
                    IDictionary<string, string> data = datas[0];
                    filename = data["reportfile"];
                    serial = data["workserial"];
                    id = data["recid"].GetSafeInt();
                    if (reportfile !="")
                    {
                        filename = DataFormat.DecodeBase64(reportfile);
                    }
                }
                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return new RedirectResult("/jdbg/FlowReport?reportfile=" + filename + "&serial=" + serial + "&type=download" + "&jdjlid=" + id + "&reporttype=" + reporttype + "&print=" + print.ToString());
            //return RedirectToAction("FlowReportDown", "jdbg", new { reportfile = "%E7%9B%91%E7%9D%A3%E6%96%B9%E6%A1%88v1", serial = 20170116009 });
        }

        public void getJCReportList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            DateTime dt1 = Request["dt1"].GetSafeDate(DateTime.MinValue);
            DateTime dt2 = Request["dt2"].GetSafeDate(DateTime.MinValue);
            string gcbh = Request["gcbh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string err = "";

            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);

                if (isQYZH(username))
                {
                    err = "企业账户不提供免费检测报告查看！";
                }
                else
                {

                    string strwhere = "";

                    if (gcbh != "")
                    {
                        strwhere += " and GCBH='" + gcbh + "'";
                    }
                    else
                    {
                        strwhere += " and GCBH in(select gcbh from  View_I_M_GC a where (a.tjjdy='" + username + "' or a.azjdy='" + username + "' or a.jdgcs='" + username + "' or exists(select * from View_GC_QY_ZH x where x.gcbh=a.gcbh and x.zh='" + username + "')or exists(select * from View_GC_RY_ZH x where x.gcbh=a.gcbh and x.zh='" + username + "')))";
                    }

                    if (key != "")
                    {
                        strwhere += " and (BGBH like '%" + key + "%' or SYDWMC like '%" + key + "%' or SYXMMC like '%" + key + "%' or GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or JCJGMS like '%" + key + "%' or WTDBH like '%" + key + "%')";
                    }
                    if (dt1 != DateTime.MinValue)
                    {
                        strwhere += " and SYRQ>='" + dt1.ToString("yyyy-MM-dd") + "'";
                    }
                    if (dt2 != DateTime.MinValue)
                    {
                        strwhere += " and SYRQ<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "'";
                    }

                    string sql = " select * from dbo.View_Sysj_Bgsj where 1=1 " + strwhere + " order by gcbh desc";
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1},\"msg\":\"{2}\"}}", totalcount, jss.Serialize(datas), err));
                Response.End();
            }
        }



        public void getJCReport()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();



            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                string id = Request["id"].GetSafeString();

                datas = CommonService.GetDataTable("select * from dbo.View_Sysj_Bgsj where BGWYH='" + id + "'");
                if (datas.Count > 0)
                {
                    IDictionary<string, string> data = datas[0];                   
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    err = jss.Serialize(data);
                    ret = true;
                }
                else
                {
                    ret = false;
                    err = "记录不存在！";
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                ret = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                rettext = JsonFormat.GetRetString(ret, err);
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(rettext);
                Response.End();
            }
        }


        public void GetReportAbs()
        {

            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string id = Request["id"].GetSafeString();
            bool code = false;
            string bgwyh = "";
            string msg = "";
            string sxh = "";
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
                if (id != "")
                    code = JcService.GetReportAbs(id, out bgwyh, out sxh, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("code", code ? "0" : "1");
                row.Add("msg", msg);
                row.Add("bgwyh", bgwyh);
                row.Add("sxh", sxh);
                Response.Write(jss.Serialize(row));
            }
            //return RedirectToAction("FlowReportDown", "jdbg", new { reportfile = "%E7%9B%91%E7%9D%A3%E6%96%B9%E6%A1%88v1", serial = 20170116009 });
        }


        public void GetJCReportFile()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string key = Request["id"].GetSafeString();
                string[] arr = key.Split(new char[] { '_' });
                string bgwyh = arr[0];
                string sxh = arr[1];
                if (bgwyh != "")
                {
                    code = JcService.GetReportFile(bgwyh, sxh, out file, out msg);
                    if (code && file != null)
                    {
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "application/pdf";
                        Response.BinaryWrite(file);
                    }
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


        public void getkqlist()
        {
            string ret = "";
            string userName = Request["username"].GetSafeString();
            string password = Request["password"].GetSafeString();

            int year = Request["year"].GetSafeInt(DateTime.Now.Year);
            int month = Request["month"].GetSafeInt(DateTime.Now.Month);

            DateTime date1 = DateTime.Parse(year + "-" + month + "-1").GetSafeDate(DateTime.Now);
            DateTime date2 = DateTime.Parse(year + "-" + month + "-1").AddMonths(1).GetSafeDate(DateTime.Now);

            string sql = "select LogDate from dbo.KqjUserLog where UserId=(select sfzhm from i_m_wgry where rybh='" + userName + "') and LogDate>='" + date1.ToString("yyyy-MM-dd") + "' and LogDate<='" + date2.ToString("yyyy-MM-dd") + "'";
            IList<IDictionary<string, string>> datas = CommonService.GetDataTable(sql);
            if (datas.Count > 0)
            for (int i = 0; i < datas.Count; i++)
            {
                if (ret != "")
                    ret += ",";
                DateTime logdate = datas[i]["logdate"].GetSafeDate(DateTime.MinValue);
                ret += "{'title': '" + logdate.ToString("HH:mm") + "签到','start': '" + logdate.ToString("yyyy-MM-dd") + "',color:''}";
            }
            ret = "[" + ret + "]";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(ret);
            //Response.Flush();
            Response.End();
        }



        public void getkqlist2()
        {
            string ret = "";
            string userName = Request["username"].GetSafeString();
            string password = Request["password"].GetSafeString();

            int page = Request["page"].GetSafeInt(1);
            int totle = 0;
            string sql = "select Recid,LogDate, IsConfirm,QYMC,GCMC,SY_LogDate from dbo.ViewKqjUserLog where UserId=(select sfzhm from i_m_wgry where rybh='" + userName + "')  order by logdate desc";
            IList<IDictionary<string, string>> datas = CommonService.GetPageData(sql, 20, page, out totle); //CommonService.GetDataTable(sql);
            if (datas.Count > 0)
                for (int i = 0; i < datas.Count; i++)
                {
                    if (ret != "")
                        ret += ",";
                    string temmc = datas[i]["gcmc"].GetSafeString();
                    if (temmc == "")
                        temmc = datas[i]["qymc"].GetSafeString();
                    int confirmint = 0;
                    string confirmtext = "未确认";
                    if (datas[i]["isconfirm"].GetSafeBool())
                    {
                        confirmint = 1;
                        confirmtext = "已确认";
                    }
                    ret += "{'title': '" + temmc + "签到','start': '" + datas[i]["sy_logdate"].GetSafeString() + "',color:'','recid':'" + datas[i]["recid"].GetSafeInt(0) + "','confirm':'" + confirmint.ToString() + "','confirmtext':'" + confirmtext + "'}";
                }
            ret = "[" + ret + "]";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(ret);
            //Response.Flush();
            Response.End();
        }

        /// <summary>
        /// 考勤确认。需要区分五大员和务工人员，需要完善
        /// </summary>
        public void kqconfirm()
        {
            bool code = true;
            string ret = "";
            string userName = Request["username"].GetSafeString();
            string password = Request["password"].GetSafeString();
            try
            {
                int recid = Request["id"].GetSafeInt(0);
                double lat=Request["lat"].GetSafeDouble();
                double lng = Request["lng"].GetSafeDouble();

                int confirmtime = Configs.GetConfigItem("confirmtime").GetSafeInt(10);
                IList<IDictionary<string, string>> datas = CommonService.GetDataTable("select * from KqjUserLog where UserId=(select sfzhm from i_m_wgry where rybh='" + userName + "') and Recid=" + recid.ToString() + " and DateAdd(mi," + confirmtime.ToString() + ",logdate)>=getdate() ");
                if (datas.Count == 0)
                {
                    code = false;
                    ret = "确认失败！请在" + confirmtime.ToString() + "分钟内确认。";
                }
                else
                {
                    IList<string> sqls = new List<string>();
                    sqls.Add("update KqjUserLog set IsConfirm=1,lat=" + lat + ", lng=" + lng + " where UserId=(select sfzhm from i_m_wgry where rybh='" + userName + "') and Recid=" + recid.ToString());
                    code = CommonService.ExecTrans(sqls);
                    ret = "确认成功！";
                }
            }
            catch (Exception ex)
            {
                ret = "确认失败！" + ex.Message;
            }


            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(JsonFormat.GetRetString(code, ret));
            //Response.Flush();
            Response.End();
        }


        public bool isQYZH(string username)
        {
            bool ret = true;
            try
            {
                IList<IDictionary<string, string>> datas = CommonService.GetDataTable("select * from I_M_QYZH where YHZH='" + username + "'");
                if (datas.Count == 0)
                    ret = false;
            }
             catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }


            return ret;
        }

		
        /// <summary>
        /// 获取手机提醒列表
        /// </summary>
        public void getPhoneAlertList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                //string strwhere = "";
                string sql = " select * from PhoneAlert2 where Reader='" + username + "' order by Recid desc";
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 删除手机提醒列表
        /// </summary>
        public void DeletePhoneAlertList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            bool code = true;
            string msg = "";
            try
            {
                int id = Request["id"].GetSafeInt(0);
                IList<string> sqls = new List<string>();
                sqls.Add("delete from PhoneAlert2 where Reader='" + username + "' and Recid= " + id.ToString());
                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        #endregion
        #region 数据提供接口
        public void PageProjectList()
        {
            string zjzbh = Request["zjzbh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string lasttime = Request["lasttime"].GetSafeString();
            string key = Request["key"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                if (zjzbh != "")
                {
                    strwhere += " and ZJZBH='" + zjzbh + "'";
                }
                if (key != "")
                {
                    strwhere += " and (gcmc like '%" + key + "%' or gcbh like '%" + key + "%' or zjdjh like '%" + key + "%')";
                }
                if (!string.IsNullOrEmpty(lasttime))
                    strwhere += " and lastupdatetime is not null and lastupdatetime>=convert(datetime,'" + lasttime + "') ";
                string sql = " from View_I_M_GC_For_DownLoad where (lsgc=1) or ( ZT not in ('LR','YT') " + strwhere + " ) order by case when zjdjh like 'L%' then 0 else 1 end desc ,gcbh desc";
                sql = "select * " + sql;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    //工程基本信息
                    compexRow.Add("工程信息", date);
                    string gcbh = date["gcbh"];
                    //分工程
                    IList<IDictionary<string, string>> tbIsgcfgc = CommonService.GetDataTable("select * from i_s_gc_fgc where gcbh='" + gcbh + "'");
                    compexRow.Add("分工程", tbIsgcfgc);
                    // 监理单位
                    IList<IDictionary<string, string>> tbIsgcjldw = CommonService.GetDataTable("select * from I_S_GC_JLDW where gcbh='" + gcbh + "'");
                    // 监理人员
                    IList<IDictionary<string, string>> tbIsgcjlry = CommonService.GetDataTable("select * from I_S_GC_JLRY where gcbh='" + gcbh + "'");
                    IList<IDictionary<string, object>> jldws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcjldw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> jldwRow = new Dictionary<string, object>();
                        jldwRow.Add("单位", row);
                        var q = from e in tbIsgcjlry where e["qybh"].Equals(row["gcqybh"]) select e;
                        jldwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        jldws.Add(jldwRow);
                    }
                    compexRow.Add("监理单位", jldws);

                    // 施工单位
                    IList<IDictionary<string, string>> tbIsgcsgdw = CommonService.GetDataTable("select * from I_S_GC_SGDW where gcbh='" + gcbh + "'");
                    // 施工人员
                    IList<IDictionary<string, string>> tbIsgcsgry = CommonService.GetDataTable("select * from I_S_GC_SGRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> sgdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcsgdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> sgdwRow = new Dictionary<string, object>();
                        sgdwRow.Add("单位", row);
                        var q = from e in tbIsgcsgry where e["qybh"].Equals(row["gcqybh"]) select e;
                        sgdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        sgdws.Add(sgdwRow);
                    }
                    compexRow.Add("施工单位", sgdws);




                    // 建设单位
                    IList<IDictionary<string, string>> tbIsgcjsdw = CommonService.GetDataTable("select * from I_S_GC_JSDW where gcbh='" + gcbh + "'");
                    // 建设人员
                    IList<IDictionary<string, string>> tbIsgcjsry = CommonService.GetDataTable("select * from I_S_GC_JSRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> jsdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcjsdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> jsdwRow = new Dictionary<string, object>();
                        jsdwRow.Add("单位", row);
                        var q = from e in tbIsgcjsry where e["qybh"].Equals(row["gcqybh"]) select e;
                        jsdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        jsdws.Add(jsdwRow);
                    }
                    compexRow.Add("建设单位", jsdws);



                    // 勘察单位
                    IList<IDictionary<string, string>> tbIsgckcdw = CommonService.GetDataTable("select * from I_S_GC_KCDW where gcbh='" + gcbh + "'");
                    // 勘察人员
                    IList<IDictionary<string, string>> tbIsgckcry = CommonService.GetDataTable("select * from I_S_GC_KCRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> kcdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgckcdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> kcdwRow = new Dictionary<string, object>();
                        kcdwRow.Add("单位", row);
                        var q = from e in tbIsgckcry where e["qybh"].Equals(row["gcqybh"]) select e;
                        kcdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        kcdws.Add(kcdwRow);
                    }
                    compexRow.Add("勘察单位", kcdws);



                    // 设计单位
                    IList<IDictionary<string, string>> tbIsgcsjdw = CommonService.GetDataTable("select * from I_S_GC_SJDW where gcbh='" + gcbh + "'");
                    // 设计人员
                    IList<IDictionary<string, string>> tbIsgcsjry = CommonService.GetDataTable("select * from I_S_GC_SJRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> sjdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcsjdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> sjdwRow = new Dictionary<string, object>();
                        sjdwRow.Add("单位", row);
                        var q = from e in tbIsgcsjry where e["qybh"].Equals(row["gcqybh"]) select e;
                        sjdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        sjdws.Add(sjdwRow);
                    }
                    compexRow.Add("设计单位", sjdws);


                    //图审单位
                    IList<IDictionary<string, string>> tbIsgctsdw = CommonService.GetDataTable("select * from I_S_GC_TSDW where gcbh='" + gcbh + "'");
                    compexRow.Add("图审单位", tbIsgcsjdw);

                    //有问题，人员都没有出来。明天排查下
                    records.Add(compexRow);

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void PageQYList()
        {
            string zjzbh = Request["zjzbh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string key = Request["key"].GetSafeString();
            string lasttime = Request["lasttime"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                if (zjzbh != "")
                {
                    strwhere += " and ZJZBH='" + zjzbh + "'";
                }
                if (key != "")
                {
                    strwhere += " and ( qybh='" + key + "' or qymc like '%" + key + "%' )";
                }
                if (!string.IsNullOrEmpty(lasttime))
                    strwhere += " and lastupdatetime is not null and lastupdatetime>=convert(datetime,'" + lasttime + "') ";
                string sql = " from I_M_QY where sptg=1 and sfyx=1 " + strwhere + " order by qybh desc";
                sql = "select * " + sql;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("企业信息", date);
                    string qybh = date["qybh"];
                    //分工程
                    sql = "select * from View_I_S_QY_QYZZ_For_DownLoad where sptg=1 and sfyx=1 and qybh='" + qybh + "'";
                    
                    IList<IDictionary<string, string>> tbIsqyzz = CommonService.GetDataTable(sql);
                    compexRow.Add("企业资质", tbIsqyzz);

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void PageRyList()
        {

            string zjzbh = Request["zjzbh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string key = Request["key"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                /*
                if (zjzbh != "")
                {
                    strwhere += " and ZJZBH='" + zjzbh + "'";
                }*/
                if (key != "")
                {
                    strwhere += " and ( rybh='" + key + "' or ryxm like '%" + key + "%' )";
                }
                string sql = " from I_M_RY where sptg=1 and sfyx=1 " + strwhere + " order by rybh desc";
                sql = "select * " + sql;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("人员信息", date);
                    string rybh = date["rybh"];
                    //分工程
                    IList<IDictionary<string, string>> tbIsqyzz = CommonService.GetDataTable("select * from View_I_S_RY_RYZZ_For_DownLoad where sptg=1 and sfyx=1 and rybh='" + rybh + "'");
                    compexRow.Add("人员资质", tbIsqyzz);

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }

        public void PageZhList()
        {

            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string key = Request["key"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                if (key != "")
                {
                    strwhere += " and qybh like '%" + key + "%' ";
                }
                string sql = " from I_M_QYZH where 1=1 " + strwhere;
                sql = "select * " + sql;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> data in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("账号信息", data);
                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }

        public void PageNBRyList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string key = Request["key"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                if (key != "")
                {
                    strwhere += " and ( rybh='" + key + "' or ryxm like '%" + key + "%' or zh='" + key + "' )";
                }
                string sql = " from I_M_NBRY where sptg=1 and sfyx=1 " + strwhere + " order by rybh desc";
                sql = "select * " + sql;
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("内部人员信息", date);
                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void AllProjectList()
        {

            string zjzbh = Request["zjzbh"].GetSafeString();
            string key = Request["key"].GetSafeString();

            
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                if (zjzbh != "")
                {
                    strwhere += " and ZJZBH='" + zjzbh + "'";
                }
                if (key != "")
                {
                    strwhere += " and gcbh='" + key + "'";
                }
                string sql = " from View_I_M_GC_For_DownLoad where ZT not in ('LR','YT') " + strwhere + " order by gcbh desc";
                sql = "select * " + sql;
                datas = CommonService.GetDataTable(sql);
                totalcount = datas.Count;
                foreach (IDictionary<string, string> date in datas)
                {

                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    //工程基本信息
                    compexRow.Add("工程信息", date);
                    string gcbh = date["gcbh"];
                    //分工程
                    IList<IDictionary<string, string>> tbIsgcfgc = CommonService.GetDataTable("select * from i_s_gc_fgc where gcbh='" + gcbh + "'");
                    compexRow.Add("分工程", tbIsgcfgc);
                    // 监理单位
                    IList<IDictionary<string, string>> tbIsgcjldw = CommonService.GetDataTable("select * from I_S_GC_JLDW where gcbh='" + gcbh + "'");
                    // 监理人员
                    IList<IDictionary<string, string>> tbIsgcjlry = CommonService.GetDataTable("select * from I_S_GC_JLRY where gcbh='" + gcbh + "'");
                    IList<IDictionary<string, object>> jldws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcjldw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> jldwRow = new Dictionary<string, object>();
                        jldwRow.Add("单位", row);
                        var q = from e in tbIsgcjlry where e["qybh"].Equals(row["gcqybh"]) select e;
                        jldwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        jldws.Add(jldwRow);
                    }
                    compexRow.Add("监理单位", jldws);

                    // 施工单位
                    IList<IDictionary<string, string>> tbIsgcsgdw = CommonService.GetDataTable("select * from I_S_GC_SGDW where gcbh='" + gcbh + "'");
                    // 施工人员
                    IList<IDictionary<string, string>> tbIsgcsgry = CommonService.GetDataTable("select * from I_S_GC_SGRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> sgdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcsgdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> sgdwRow = new Dictionary<string, object>();
                        sgdwRow.Add("单位", row);
                        var q = from e in tbIsgcsgry where e["qybh"].Equals(row["gcqybh"]) select e;
                        sgdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        sgdws.Add(sgdwRow);
                    }
                    compexRow.Add("施工单位", sgdws);




                    // 建设单位
                    IList<IDictionary<string, string>> tbIsgcjsdw = CommonService.GetDataTable("select * from I_S_GC_JSDW where gcbh='" + gcbh + "'");
                    // 建设人员
                    IList<IDictionary<string, string>> tbIsgcjsry = CommonService.GetDataTable("select * from I_S_GC_JSRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> jsdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcjsdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> jsdwRow = new Dictionary<string, object>();
                        jsdwRow.Add("单位", row);
                        var q = from e in tbIsgcjsry where e["qybh"].Equals(row["gcqybh"]) select e;
                        jsdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        jsdws.Add(jsdwRow);
                    }
                    compexRow.Add("建设单位", jsdws);



                    // 勘察单位
                    IList<IDictionary<string, string>> tbIsgckcdw = CommonService.GetDataTable("select * from I_S_GC_KCDW where gcbh='" + gcbh + "'");
                    // 勘察人员
                    IList<IDictionary<string, string>> tbIsgckcry = CommonService.GetDataTable("select * from I_S_GC_KCRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> kcdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgckcdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> kcdwRow = new Dictionary<string, object>();
                        kcdwRow.Add("单位", row);
                        var q = from e in tbIsgckcry where e["qybh"].Equals(row["gcqybh"]) select e;
                        kcdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        kcdws.Add(kcdwRow);
                    }
                    compexRow.Add("勘察单位", kcdws);



                    // 设计单位
                    IList<IDictionary<string, string>> tbIsgcsjdw = CommonService.GetDataTable("select * from I_S_GC_SJDW where gcbh='" + gcbh + "'");
                    // 设计人员
                    IList<IDictionary<string, string>> tbIsgcsjry = CommonService.GetDataTable("select * from I_S_GC_SJRY where gcbh='" + gcbh + "'");

                    IList<IDictionary<string, object>> sjdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcsjdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> sjdwRow = new Dictionary<string, object>();
                        sjdwRow.Add("单位", row);
                        var q = from e in tbIsgcsjry where e["qybh"].Equals(row["gcqybh"]) select e;
                        sjdwRow.Add("人员", q.ToList<IDictionary<string, string>>());
                        sjdws.Add(sjdwRow);
                    }
                    compexRow.Add("设计单位", sjdws);


                    //图审单位
                    IList<IDictionary<string, string>> tbIsgctsdw = CommonService.GetDataTable("select * from I_S_GC_TSDW where gcbh='" + gcbh + "'");
                    compexRow.Add("图审单位", tbIsgcsjdw);

                    //有问题，人员都没有出来。明天排查下
                    records.Add(compexRow);

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void AllQYList()
        {

            string zjzbh = Request["zjzbh"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                if (zjzbh != "")
                {
                    strwhere += " and ZJZBH='" + zjzbh + "'";
                }
                string sql = " from I_M_QY where sptg=1 and sfyx=1 " + strwhere + " order by qybh desc";
                sql = "select * " + sql;
                datas = CommonService.GetDataTable(sql);
                totalcount = datas.Count;
                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("企业信息", date);
                    string qybh = date["qybh"];
                    //分工程
                    IList<IDictionary<string, string>> tbIsqyzz = CommonService.GetDataTable("select * from View_I_S_QY_QYZZ_For_DownLoad where sptg=1 and sfyx=1 and qybh='" + qybh + "'");
                    compexRow.Add("企业资质", tbIsqyzz);

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void AllRyList()
        {

            string zjzbh = Request["zjzbh"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                /*
                if (zjzbh != "")
                {
                    strwhere += " and ZJZBH='" + zjzbh + "'";
                }*/
                string sql = " from I_M_RY where sptg=1 and sfyx=1 " + strwhere + " order by rybh desc";
                sql = "select * " + sql;
                datas = CommonService.GetDataTable(sql);
                totalcount = datas.Count;
                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("人员信息", date);
                    string rybh = date["rybh"];
                    //分工程
                    IList<IDictionary<string, string>> tbIsqyzz = CommonService.GetDataTable("select * from View_I_S_RY_RYZZ_For_DownLoad where sptg=1 and sfyx=1 and rybh='" + rybh + "'");
                    compexRow.Add("人员资质", tbIsqyzz);

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }

        public void AllNBRyList()
        {

            int totalcount = 0;
            string key = Request["key"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (key != "")
                {
                    strwhere += " and ( rybh='" + key + "' or ryxm like '%" + key + "%' or zh='" + key + "' )";
                }
                string sql = " from I_M_NBRY where sptg=1 and sfyx=1 " + strwhere + " order by rybh desc";
                sql = "select * " + sql;
                datas = CommonService.GetDataTable(sql);
                totalcount = datas.Count;
                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("人员信息", date);

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void AllZhList()
        {
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            List<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";

                string sql = " from I_M_QYZH where 1=1 " + strwhere ;
                sql = "select * " + sql;
                datas = CommonService.GetDataTable(sql);
                totalcount = datas.Count;
                foreach (IDictionary<string, string> data in datas)
                {
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("账号信息", data);

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }


        public void getFile()
        {
            string fname = "";
            long filesize = 0;
            byte[] ret = null;
            string fileid = Request["id"].GetSafeString().Replace("'", "");
            string filetype = Request["type"].GetSafeString();
            try
            {
                bool isbig = filetype == "big";
                fname = isbig ? "FILECONTENT" : "SMALLCONTENT";
                string furlname = isbig ? "fileurl" : "smallurl";
                string sql = string.Format("select {0} as thumbattachment,FILENAME,storagetype, {2} as furl from DATAFILE where [FILEID]='{1}'", fname, fileid,furlname);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (dt.Count > 0)
                {
                    ret = dt[0]["thumbattachment"] as byte[];
                    string filename = dt[0]["filename"].GetSafeString();
                    string storagetype = dt[0]["storagetype"].GetSafeString();
                    string furl = dt[0]["furl"].GetSafeString();
                    if (ret == null || ret.Length == 0)
                    {
                        if (storagetype.Equals("oss", StringComparison.OrdinalIgnoreCase) && (furl!=""))
                        {
                            ret = OssCdnUtil.DownFile(furl);
                        }
                    }


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


        #endregion
        #region 务工人员手机接口
        /// <summary>
        /// 获取人员信息,在岗，在册，当前
        /// </summary>
        public void getRYGCList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            bool code = true;
            string err = "";
            string ryxm = Request["ryxm"].GetSafeString();
            string type = Request["type"].GetSafeString("1");
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                //if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string strwhere = "";
                    string where = " where 1=1 ";
                    string sql = "";
                    #region 根据政府账号查询管辖区域
                    string zfsql = "select szsf,szcs,szxq,szjd from View_H_ZFZH_XQ  where usercode='" + CurrentUser.UserName + "' ";
                    IList<IDictionary<string, string>> zf_dqlist = CommonService.GetDataTable(zfsql);
                    string szsf = "", szcs = "", szxq = "", szjd = "";

                    for (int i = 0; i < zf_dqlist.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(zf_dqlist[i]["szsf"]))
                            szsf += zf_dqlist[i]["szsf"] + ",";
                        if (!string.IsNullOrEmpty(zf_dqlist[i]["szcs"]))
                            szcs += zf_dqlist[i]["szcs"] + ",";
                        if (!string.IsNullOrEmpty(zf_dqlist[i]["szxq"]))
                            szxq += zf_dqlist[i]["szxq"] + ",";
                        if (!string.IsNullOrEmpty(zf_dqlist[i]["szjd"]))
                            szjd += zf_dqlist[i]["szjd"] + ",";
                    }
                    if (szsf != "")
                    {
                        szsf = szsf.FormatSQLInStr(); ;
                        where += " and e.szsf in (" + szsf + ")";
                    }
                    if (szcs != "")
                    {
                        szcs = szcs.FormatSQLInStr(); ;
                        where += " and e.szcs in (" + szcs + ")";
                    }
                    if (szxq != "")
                    {
                        szxq = szxq.FormatSQLInStr(); ;
                        where += " and e.szxq in (" + szxq + ")";
                    }
                    if (szjd != "")
                    {
                        szjd = szjd.FormatSQLInStr(); ;
                        where += " and e.szjd in (" + szjd + ")";
                    }
                    /////////////////////////
                    #endregion
                    if (type != "3" && ryxm != "")
                    {
                        strwhere += " and  a.ryxm  like '%" + ryxm + "%'";
                    }
                    if (type == "2") //在岗
                    {
                        strwhere += " and  a.hasdelete=0";
                    }
                    if (type == "3") //当前
                    {
                        if (ryxm != "")
                        {
                            strwhere += " and  a.realname  like '%" + ryxm + "%'";
                        }
                        sql = "select a.realname as ryxm,a.userid as sfzhm,'' as sfbzfzr,b.xb,b.gcmc,b.dh from kqjuserdaylog a left join I_M_WGRY b on a.UserId = b.SFZHM and a.ProjectId = b.JDZCH ";

                        sql += "where 1=1 " + strwhere + " and datediff(dd,LogDay,getdate())=0 and jdzch in (select gcbh from i_m_gc e " + where + ") group by userid,realname,b.xb,b.gcmc,b.dh order by a.gcmc ";
                    }
                    if (sql == "")
                        sql = "select a.ryxm,a.sfzhm,'' as sfbzfzr,a.xb,a.gcmc,a.dh from i_m_wgry a where 1=1 " + strwhere + " and jdzch in (select gcbh from i_m_gc e " + where + ") group by a.ryxm,a.sfzhm,a.xb,a.gcmc,a.dh order by gcmc";
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 获取某个人员的考勤记录
        /// </summary>
        public void getRYKQList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            bool code = true;
            string err = "";
            string sfzhm = Request["sfzhm"].GetSafeString();
            //string sfbzfzr = Request["sfbzfzr"].GetSafeString();
            //string ryxm=Request["ryxm"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            string date1 = Request["date1"].GetSafeString();
            string date2 = Request["date2"].GetSafeString();

            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string tjsum = "0";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string strwhere = "";
                    if (sfzhm == "")
                    {

                    }
                    else
                    {
                        //if (sfbzfzr == "是")
                        //{
                        //    if (sfzhm != "")
                        //        strwhere += " and  bzfzr  ='" + sfzhm + "'";
                        //    if(ryxm!="")
                        //        strwhere += " and  realname like '%" + ryxm + "%'";
                        //}
                        //else
                        {
                            if (sfzhm != "")
                            {
                                strwhere += " and  userid  ='" + sfzhm + "'";
                            }
                        }
                        if (gcmc != "")
                            strwhere += " and  projectname like '%" + gcmc + "%'";
                        if (date1 != "")
                        {
                            strwhere += " and  datediff(dd,LogDay,'" + date1 + "')<=0";
                        }
                        if (date2 != "")
                        {
                            strwhere += " and  datediff(dd,LogDay,'" + date2 + "')>=0";
                        }
                        string sql = "select userid,RealName,projectname,CONVERT(VARCHAR(24),LogDay,111) as LogDay,sum(RealSum) as totalSum ,gz,gw from ViewKqjUserDayLog where 1=1 " + strwhere;
                        sql += " group by userid,RealName,LogDay,projectname,gz,gw order by LogDay desc";
                        datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                        sql = "select userid,sum(RealSum) as tjsum from KqjUserDayLog where 1=1 " + strwhere;
                        sql += " group by userid order by userid";
                        IList<IDictionary<string, string>> sumdata = CommonService.GetDataTable(sql);
                        if (sumdata.Count != 0)
                        {
                            tjsum = sumdata[0]["tjsum"];
                        }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1},\"tjsum\":{2}}}", totalcount, jss.Serialize(datas), tjsum));
                Response.End();
            }
        }
        /// <summary>
        /// 获取某个人员的考勤记录
        /// </summary>
        public void getGCRYPayList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            bool code = true;
            string err = "";
            string gcbh = Request["gcbh"].GetSafeString();
            string year = Request["year"].GetSafeString();
            string month = Request["month"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeString();
            string ryxm = Request["ryxm"].GetSafeString();

            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string strwhere = "";
                    if (gcbh == "")
                    {

                    }
                    else
                    {
                        if (gcbh != "")
                            strwhere += " and  jdzch = '" + gcbh + "'";
                        if (year != "")
                        {
                            strwhere += " and  logyear='" + year + "'";
                        }
                        //else
                        //{
                        //    strwhere += " and  logyear='" + DateTime.Now.Year + "'";
                        //}
                        if (month != "")
                        {
                            strwhere += " and  logmonth='" + month + "'";
                        }
                        //else
                        //    strwhere += " and  logmonth='" + DateTime.Now.Month + "'";
                        if (sfzhm != "")
                        {
                            strwhere += " and  userid='" + sfzhm + "' ";

                        }
                        if (ryxm != "")
                        {
                            strwhere += " and  ryxm like '%" + ryxm + "%' ";

                        }

                        string sql = "select * from KqjUserMonthPay where 1=1 " + strwhere;
                        sql += "order by logyear desc,logmonth desc";
                        datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 获取班组长下的工程
        /// </summary>
        public void getBZGCList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            bool code = true;
            string err = "";
            string sfzhm = Request["sfzhm"].GetSafeString();

            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string tjsum = "0";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string strwhere = "";
                    if (sfzhm == "")
                    {

                    }
                    else
                    {
                        if (sfzhm != "")
                            strwhere += " and  sfzhm  ='" + sfzhm + "'";

                        string sql = "select gcmc  from i_m_wgry where 1=1 " + strwhere;
                        datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 获取班组长下的各人员
        /// </summary>
        public void getBZRYList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            bool code = true;
            string err = "";
            string sfzhm = Request["sfzhm"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            string date1 = Request["date1"].GetSafeString();
            string date2 = Request["date2"].GetSafeString();

            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string tjsum = "0";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string strwhere = "";
                    if (sfzhm == "")
                    {

                    }
                    else
                    {
                        if (sfzhm != "")
                            strwhere += " and  bzfzr  ='" + sfzhm + "'";
                        if (gcmc != "")
                            strwhere += " and  projectname like '%" + gcmc + "%'";
                        if (date1 != "")
                        {
                            strwhere += " and  datediff(dd,LogDay,'" + date1 + "')<=0";
                        }
                        if (date2 != "")
                        {
                            strwhere += " and  datediff(dd,LogDay,'" + date2 + "')>=0";
                        }
                        //string sql = "select userid,RealName,projectname,CONVERT(VARCHAR(24),LogDay,111) as LogDay,sum(RealSum) as totalSum ,gz,gw from ViewKqjUserDayLog where 1=1 " + strwhere;
                        //sql += " group by userid,RealName,LogDay,projectname,gz,gw order by realname";
                        //datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                        string sql = "select userid,RealName,sum(RealSum) as tjsum from KqjUserDayLog where 1=1 " + strwhere;
                        sql += " group by userid,RealName order by realname";
                        datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 获取人员预警信息
        /// </summary>
        public void getYJRYList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            string ryxm = Request["ryxm"].GetSafeString();
            string logyear = Request["logyear"].GetSafeString();
            string logmonth = Request["logmonth"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string tjsum = "0";
            bool code = true;
            string err = "";


            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string where = "";
                    if (gcmc != "")
                    {
                        where += " and  gcmc like '%" + gcmc + "%'";
                    }
                    if (ryxm != "")
                    {
                        where += " and  ryxm like '%" + ryxm + "%'";
                    }
                    if (logyear != "")
                    {
                        where += " and  logyear = '" + logyear + "'";
                    }
                    if (logmonth != "")
                    {
                        where += " and  logmonth = '" + logmonth + "'";
                    }

                    string sql = "select userid,ryxm,jdzch as gcbh ,gcmc,logyear,logmonth,tx_yjzt,ff_yjzt from ViewKqjUserMonthPay where (TX_YJZT=1 or FF_YJZT=1) " + where;
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        public void getGCRYList()
        {

        }
        /// <summary>
        /// 获取薪资预警信息
        /// </summary>
        public void getYJXZList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            string logyear = Request["logyear"].GetSafeString();
            string logmonth = Request["logmonth"].GetSafeString();
            bool code = true;
            string err = "";
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string tjsum = "0";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string where = "";
                    if (gcmc != "")
                    {
                        where += " and  gcmc like '%" + gcmc + "%'";
                    }
                    if (logyear != "")
                    {
                        where += " and  logyear = '" + logyear + "'";
                    }
                    if (logmonth != "")
                    {
                        where += " and  logmonth = '" + logmonth + "'";
                    }
                    string sql = "select * from INFO_YJ_XZ where (xzdw=0 or xzze=0) " + where;
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion
		#region 务工人员微信公众号
        /// <summary>
        /// 微信校验及绑定
        /// </summary>
        public void WXdologin()
        {
            string sfzhm = Request["sfzhm"].GetSafeString();
            string ryxm = Request["ryxm"].GetSafeString();
            string wxkey = Request["wxkey"].GetSafeString();
            string content = Request["content"].GetSafeString();
            WXJSON wxjson = new WXJSON();
            wxjson.Datas = null;
            bool code = false;
        
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (content == "")
                {
                    if (sfzhm == "" || ryxm == "" || wxkey == "")
                    {
                        wxjson.Msg = "校验失败1";
                        wxjson.Status = "error";
                        code = false;
                    }
                    else
                        code = true;
                }
                else
                {
                    if (wxkey == "")
                    {
                        wxjson.Msg = "校验失败2";
                        wxjson.Status = "error";
                        code = false;
                    }
                    else
                    {
                        content =EncryUtil.Decode(content.DecodeBase64());
                        string[] con_list = content.Split('_');
                        if (con_list.Length != 2)
                        {
                            wxjson.Msg = "用户信息有误";
                            wxjson.Status = "error";
                            code = false;
                        }
                        else
                        {
                            ryxm = con_list[0];
                            sfzhm = con_list[1];
                            code = true;
                        }
                    }

                }

                if (code)
                {
                    string strwhere = "";
                    if (sfzhm != "")
                    {
                        strwhere += " and  sfzhm ='" + sfzhm + "'";
                    }
                    if (ryxm != "")
                    {
                        strwhere += " and  ryxm = '" + ryxm + "'";
                    }
                    string sql = "select * from i_m_ry_info where 1=1 " + strwhere;

                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count == 0)
                    {
                        wxjson.Msg = "身份证与姓名不符合";
                        wxjson.Status = "error";
                    }
                    else
                    {
                        // sql = "select * from info_wxbd where wxkey='" + wxkey + "'";
                        // IList<IDictionary<string, string>> wxdatas = CommonService.GetDataTable(sql);
                        if (string.IsNullOrEmpty(datas[0]["wxkey"]))
                        {
                            //sql = "insert into info_wxbd (wxkey,ryxm,sfzhm) values('" + wxkey + "','" + ryxm + "','" + sfzhm + "')";
                            sql = "update i_m_ry_info set wxkey='" + wxkey + "' where sfzhm='" + sfzhm + "'";
                            if (CommonService.Execsql(sql))
                            {
                                wxjson.Status = "success";
                                wxjson.Msg = "绑定成功";
                            }
                        }
                        else
                        {
                            string wxsfzhm = datas[0]["sfzhm"];
                            if (wxsfzhm != sfzhm)
                            {
                                wxjson.Status = "failure";
                                wxjson.Msg = "当前微信已经绑定其他账号";
                            }
                            else
                            {
                                wxjson.Status = "failure";
                                wxjson.Msg = "当前身份证号已经绑定";
                            }
                        }
                    }
                }              
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {               
                JavaScriptSerializer jss = new JavaScriptSerializer();
                //Response.Headers.Add("Access-Control-Allow-Origin","*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                //Response.Write(string.Format("{{\"Status\":\"{0}\",\"Msg\":\"{1}\",\"Datas\":\"{2}\"}}", status, msg,""));
                Response.End();
            }
        }
        /// <summary>
        /// 微信解绑
        /// </summary>
        public void WXoutLogin()
        {        
            string wxkey = Request["wxkey"].GetSafeString();
            WXJSON wxjson = new WXJSON();
            wxjson.Datas = null;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else
                {
                   // string sql = "select * from info_wxbd where wxkey='" + wxkey + "'";
                    string sql = "select * from i_m_ry_info where wxkey='" + wxkey + "'";
                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count == 0)
                    {
                        wxjson.Msg = "该微信号没有绑定";
                        wxjson.Status = "failure";
                    }
                    else
                    {
                        string sfzhm = datas[0]["sfzhm"];
                        //sql = "delete from info_wxbd where wxkey='" + wxkey + "'";
                        sql = "update i_m_ry_info set wxkey='' where sfzhm='" + sfzhm + "'";
                        if(CommonService.Execsql(sql))
                        {
                            wxjson.Status = "success";
                            wxjson.Msg = "解绑成功";
                        }
                        else
                        {
                            wxjson.Status = "error";
                            wxjson.Msg = "解绑失败";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {

                JavaScriptSerializer jss = new JavaScriptSerializer();
                //Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                //Response.Write(string.Format("{{\"Status\":\"{0}\",\"Msg\":\"{1}\",\"Datas\":\"{2}\"}}", status, msg,""));
                Response.End();
            }
        }
        /// <summary>
        /// 获取人员的工程表
        /// </summary>
        public void WXgetRYGCList()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string jdzch = Request["jdzch"].GetSafeString();
            //string msg = "";
            //string status = "";
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else
                {
                    string where ="";
                    if (jdzch != "")
                        where += " and jdzch='" + jdzch + "' ";
                    //string sql = "select jdzch as gcbh,gcmc from i_m_wgry where sfzhm=(select sfzhm from info_wxbd where wxkey='" + wxkey + "') and gcmc <>''";
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    IList<IDictionary<string, string>> rydata = new List<IDictionary<string, string>>();
                    rydata = CommonService.GetDataTable(sql);
                    if (rydata.Count>0)
                    {
                        sql = "select jdzch as gcbh,gcmc from i_m_wgry where sfzhm=(select sfzhm from i_m_ry_info where wxkey='" + wxkey + "') and gcmc <>'' " + where;
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            wxjson.Msg = "";
                            wxjson.Status = "success";
                            wxjson.Datas = datas;
                        }
                        else
                        {
                            wxjson.Msg = "没有找到所属工程";
                            wxjson.Status = "notingc"; //没有在该工程，要先录入
                        }
                    }                
                    else
                    {
                        wxjson.Msg = "没有绑定";
                        wxjson.Status = "notbind"; //没有绑定
                    }

                }            
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
               // Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
               // Response.Write(string.Format("{{\"Status\":\"{0}\",\"Msg\":\"{1}\",\"Datas\":\"{2}\"}}", status, msg, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 根据微信号和工程ID(自增字段)，查询对应的考勤信息（上一年度的有效考勤次数和当月的有效考勤次数）
        /// </summary>
        public void WXgetRYGCKQList()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" || gcbh=="")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    //string sql = "select sfzhm from info_wxbd where wxkey='" + wxkey + "'";
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    string str_datas = "";
                    if (sfzdatas.Count > 0)
                    {
                        IDictionary<string, string> row = new Dictionary<string, string>();

                        string sfzhm = sfzdatas[0]["sfzhm"];
                        DateTime dt = DateTime.Now ;
                        string year = dt.Year.ToString();
                        string month = dt.Month.ToString();
                        sql = "select workday,ryxm,gcmc from ViewKqjUserMonthPay where jdzch='" + gcbh + "' and userid='" + sfzhm + "' and logyear='" + year + "' and logmonth='" + month + "'";
                        IList<IDictionary<string, string>> paydatas= CommonService.GetDataTable(sql);
                        if(paydatas.Count>0)
                        {
                            string workday = paydatas[0]["workday"];
                            string ryxm = paydatas[0]["ryxm"];
                            string gcmc = paydatas[0]["gcmc"];
                            str_datas += "考勤人员:" + ryxm+"\n";
                            str_datas += "工程名称:" + gcmc + "\n";
                            str_datas += "考勤信息:" + year + "年" + month + "月有效考勤天数为" + workday + ";";                              
                        }
                        else
                        {
                            sql = "select ryxm,gcmc from i_m_wgry where jdzch='" + gcbh + "' and sfzhm='" + sfzhm + "'";
                            IList<IDictionary<string, string>> rygcdatas = CommonService.GetDataTable(sql);
                            if (rygcdatas.Count>0)
                            {
                                string ryxm = rygcdatas[0]["ryxm"];
                                string gcmc = rygcdatas[0]["gcmc"];
                                str_datas += "考勤人员:" + ryxm + "\n";
                                str_datas += "工程名称:" + gcmc + "\n";
                                str_datas += "考勤信息:" + year + "年" + month + "月有效考勤天数为0;";
                            }
                            else
                            {
                                wxjson.Msg = "没有找到该人员";
                                wxjson.Status = "failure";

                            }
                       
                        }
                        if (str_datas!="")
                        {
                            sql = "select sum(workday)as totalday from KqjUserMonthPay where jdzch='" + gcbh + "' and userid='" + sfzhm + "' and logyear='" + (dt.Year - 1).ToString() + "' ";
                            IList<IDictionary<string, string>> daydatas = CommonService.GetDataTable(sql);
                            if (paydatas.Count > 0)
                            {
                                string totalday = daydatas[0]["totalday"];
                                str_datas += (dt.Year - 1).ToString() + "年有效考勤总天数为" + totalday + ";";
                            }
                            else
                            {
                                str_datas += (dt.Year - 1).ToString() + "年有效考勤总天数为0;";
                            }
                      
                        }                       
                        byte[] b = System.Text.Encoding.UTF8.GetBytes(str_datas);      
                        //转成 Base64 形式的 System.String  
                        str_datas = Convert.ToBase64String(b); 

                        row.Add("str", str_datas);
                        datas.Insert(0, row);
                        wxjson.Datas = datas;
                        wxjson.Msg = "";
                        wxjson.Status = "success";
                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                //Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));              
                Response.End();
            }
        }

        /// <summary>
        /// 获取工程下的务工人员考勤信息,根据工程ID和考勤月份，获取务工人员对应月份的考勤信息
        /// </summary>
        public void WXgetGC_RYKQList()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string year = Request["year"].GetSafeString();
            string month = Request["month"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey==""||year == "" || gcbh == "" || month == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    //string sql = "select sfzhm from info_wxbd where wxkey='" + wxkey + "'";
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    if (sfzdatas.Count > 0)
                    {
                        IDictionary<string, string> row = new Dictionary<string, string>();

                        string sfzhm = sfzdatas[0]["sfzhm"];
                        string strwhere=" jdzch='" + gcbh + "' and userid='" + sfzhm + "' and logyear='" + year + "' and logmonth='" + month + "'";
                        sql = "select * from ViewKqjUserMonthPay where 1=1 and " + strwhere;               
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            wxjson.Datas = datas;
                            wxjson.Msg = "";
                            wxjson.Status = "success";
                        }
                        else
                        {
                            wxjson.Msg = "没有考勤数据";
                            wxjson.Status = "failure";
                        }                  
                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                //Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }
        }

        /// <summary>
        /// 根据微信号和工程ID(自增字段)，查询对应的薪资信息（上一年度的薪资（应发/实发）和当月的薪资（应发/实发））
        /// </summary>
        public void WXgetRYXZ()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" ||  gcbh == "" )
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                   // string sql = "select sfzhm from info_wxbd where wxkey='" + wxkey + "'";
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    string str_datas = "";
                    if (sfzdatas.Count > 0)
                    {
                        IDictionary<string, string> row = new Dictionary<string, string>();
                        DateTime dt = DateTime.Now;
                        string year = dt.Year.ToString();
                        string month = dt.Month.ToString();
                        string sfzhm = sfzdatas[0]["sfzhm"];
                        string strwhere = " jdzch='" + gcbh + "' and userid='" + sfzhm + "' and logyear='" + year + "' and logmonth='" + month + "'";
                        sql = "select havepay,notpay,bankpay from ViewKqjUserMonthPay where 1=1 and " + strwhere;
                        IList<IDictionary<string, string>> M_paydatas  = CommonService.GetDataTable(sql);
                        if (M_paydatas.Count > 0)
                        {
                            string havepay = M_paydatas[0]["havepay"];
                            string bankpay = M_paydatas[0]["bankpay"];
                            str_datas += year + "年" + month + "月应发工资为" + havepay + "/实发工资为" + bankpay+";";
                        }
                        else
                        {
                            str_datas += year + "年" + month + "月应发工资为0/实发工资为0;";
                        }
                        strwhere = " jdzch='" + gcbh + "' and userid='" + sfzhm + "' and logyear='" + year + "'";
                        sql = "select sum(havepay) as havepay ,sum(notpay) as notpay ,sum(bankpay) as bankpay from ViewKqjUserMonthPay where 1=1 and " + strwhere;
                        IList<IDictionary<string, string>> Y_paydatas = CommonService.GetDataTable(sql);
                        if (Y_paydatas.Count > 0)
                        {
                            string havepay = Y_paydatas[0]["havepay"]==""?"0":Y_paydatas[0]["havepay"];
                            string bankpay = Y_paydatas[0]["bankpay"] == "" ? "0" : Y_paydatas[0]["bankpay"];
                            str_datas += year + "年应发总工资为" + havepay + "/实发总工资为" + bankpay;
                        }
                        if (str_datas!="")
                        {
                            row.Add("str", str_datas);
                            datas.Insert(0, row);
                            wxjson.Datas = datas;
                            wxjson.Msg = "";
                            wxjson.Status = "success";
                        }
                        else
                        {
                            wxjson.Status = "failure";
                            wxjson.Msg = "没有数据";
                        }
                      
                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
               // Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }
        }
        /// <summary>
        /// 2.根据微信号,工程ID和薪资年份，获取务工人员对应的薪资信息
        /// </summary>
        public void WXgetRYXZDetial()
        {
            string wxkey = Request["wxkey"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            string year = Request["year"].GetSafeString();
            WXJSON wxjson = new WXJSON();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (wxkey == "" || gcbh == "")
                {
                    wxjson.Msg = "校验失败";
                    wxjson.Status = "error";
                }
                else
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    //string sql = "select sfzhm from info_wxbd where wxkey='" + wxkey + "'";
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    string str_datas = "";
                    if (sfzdatas.Count > 0)
                    {                    
                        string sfzhm = sfzdatas[0]["sfzhm"];
                        string strwhere = " jdzch='" + gcbh + "' and userid='" + sfzhm + "' and logyear='" + year + "'";
                        sql = "select shouldpay,havepay,notpay,bankpay,yzpay,logmonth from ViewKqjUserMonthPay where 1=1 and " + strwhere;
                        sql+=" order by logmonth asc";
                        IList<IDictionary<string, string>> M_paydatas = CommonService.GetDataTable(sql);
                        for (int i = 1 ;i<=12;i++)
                        {
                            bool havmonth=false;
                            IDictionary<string, string> row = new Dictionary<string, string>();
                            for(int j=0;j<M_paydatas.Count;j++)
                            {
                                if(M_paydatas[j]["logmonth"]==i.ToString())
                                {
                                    havmonth=true;
                                    string shouldpay = M_paydatas[j]["shouldpay"] == "" ? "0" : M_paydatas[j]["shouldpay"];
                                    string havepay = M_paydatas[j]["havepay"] == "" ? "0" : M_paydatas[j]["havepay"];
                                    string bankpay = M_paydatas[j]["bankpay"] == "" ? "0" : M_paydatas[j]["bankpay"];
                                    string notpay = M_paydatas[j]["notpay"] == "" ? "0" : M_paydatas[j]["notpay"];
                                    string yzpay = M_paydatas[j]["yzpay"] == "" ? "0" : M_paydatas[j]["yzpay"];

                                    row.Add("shouldpay" + i.ToString(), shouldpay);
                                    row.Add("havepay" + i.ToString(), havepay);
                                    row.Add("bankpay" + i.ToString(), bankpay);
                                    row.Add("notpay" + i.ToString(), notpay);
                                    row.Add("yzpay" + i.ToString(), yzpay);
                                    row.Add("month" + i.ToString(), i.ToString()); 
                                    break;
                                }
                            }
                            if(!havmonth)
                            {
                                row.Add("shouldpay" + i.ToString(), "0");
                                row.Add("havepay" + i.ToString(), "0");
                                row.Add("bankpay" + i.ToString(), "0");
                                row.Add("notpay" + i.ToString(), "0");
                                row.Add("yzpay" + i.ToString(), "0");
                                row.Add("month" + i.ToString(), i.ToString());
                            }
                            datas.Insert(i-1, row);
                        }
                        wxjson.Datas = datas;
                        wxjson.Msg = "";
                        wxjson.Status = "success";
                    
                    }
                    else
                    {
                        wxjson.Status = "failure";
                        wxjson.Msg = "当前微信没有绑定账号";
                    }

                }
            }
            catch (Exception e)
            {
                wxjson.Status = "error";
                wxjson.Msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
               // Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(wxjson));
                Response.End();
            }
        }
        #endregion


		public void DloadImgBySfzh()
        {
            byte[] ret = null;
            string err = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {

                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);

                string sfzhm = Request["sfzhm"].GetSafeString();


                //int fileid = 0;
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2("SELECT TOP 1 zp FROM I_M_WGRY WHERE SFZHM = '" + sfzhm + "'");
                if (dt.Count > 0)
                {
                    string data = "xcuivosfoamfodamf;mzxcvl;" + dt[0]["zp"].ToString();
                    ret = Convert.FromBase64String(dt[0]["zp"].ToString());
                    string filename = sfzhm + ".jpg";
                    Response.Clear();
                    Response.ContentType = "image/jpeg jpeg jpg jpe";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
            }
        }

        #region 务工人员上层下载
        public void PageWgryList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string lasttime = Request["lasttime"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (!string.IsNullOrEmpty(lasttime))
                    strwhere += " and (yhkh is not null or yhkh <> '') and (yhhh is not null or yhhh <> '') and lastupdatetime is not null and lastupdatetime>=convert(datetime,'" + lasttime + "') ";
                string sql = " from I_M_WGRY where 1=1 " + strwhere + " order by jdzch desc";
                sql = "select ryxm,sfzhm,sjhm,YHKH,YHHH " + sql;

                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> date in datas)
                {
                    IDictionary<string, object> dstdate = new Dictionary<string, object>();
                    string sfzhm = CryptFun.Encode(date["sfzhm"]);
                    string yhkh = CryptFun.Encode(date["yhkh"]);

                    dstdate.Add("ryxm", date["ryxm"]);
                    dstdate.Add("sfzhm", sfzhm);
                    dstdate.Add("sjhm", date["sjhm"]);
                    dstdate.Add("yhkh", yhkh);
                    dstdate.Add("yhhh", date["yhhh"]);
                    records.Add(dstdate);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(records)));
                Response.End();
            }
        }
        /// <summary>
        /// 劳务公司获取
        /// </summary>
        public void PageLWGSList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string lasttime = Request["lasttime"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (!string.IsNullOrEmpty(lasttime))
                    strwhere += " and lastupdatetime is not null and lastupdatetime>=convert(datetime,'" + lasttime + "') ";
                string sql = " from I_M_LWGS where 1=1 " + strwhere ;
                sql = "select * " + sql;

                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
             
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        /// <summary>
        /// 支付公司
        /// </summary>
        public void PageZFGSList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string lasttime = Request["lasttime"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            IList<IDictionary<string, object>> records = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (!string.IsNullOrEmpty(lasttime))
                    strwhere += " and lastupdatetime is not null and lastupdatetime>=convert(datetime,'" + lasttime + "') ";
                string sql = " from View_I_M_GC_ZFGS where gcbh is not null and qybh is not null and zfgsbh is not null " + strwhere + " order by gcbh desc";
                sql = "select * " + sql;

                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                jss.MaxJsonLength = Int32.MaxValue;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion


        #region 方法-角色管理
        #region 用户系统交互

        private string umsurl = Configs.GetConfigItem("umsurl");



        public JsonResult GetUserList()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            StringBuilder sb = new StringBuilder();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            int totalcount = 0;
            //IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {
                string companyid = Request["companyid"].GetSafeString();
                string realname = Request["text"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();

                int pageindex = Request["page"].GetSafeInt(1);
                int pagesize = Request["rows"].GetSafeInt(20);

                sb.Append("[");

                string sql = "select a.zh as name,b.jcjgmc as cpname,a.Rybh as id,a.ryxm as text,case when a.sfyx=1 then 1 else 0 end as sfyx from i_m_nbry_jc a, h_jcjg b where a.jcjgbh=b.jcjgbh";
                if (companyid != "")
                    sql += " and a.jcjgbh='" + companyid + "'";
                if (realname != "")
                    sql += " and a.ryxm  like '%" + realname + "%'";
                if (sfzhm != "")
                    sql += " and a.sfzhm like '%" + sfzhm + "%'";
                //if (CurrentUser.Qybh != "")
                //    sql += " and a.jcjgbh='" + CurrentUser.Qybh + "'";

                dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = msg, totalCount = totalcount, data = dt }, JsonRequestBehavior.AllowGet);
        }


        #region  获取检测人员关联的试验项目
        public JsonResult GetJCRYSyxm()
        {
            IList<JcjtJcjgZZ> datas = new List<JcjtJcjgZZ>();
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeString();
                datas = JcjtService.GetJCRYZZxx_Byrybh(rybh);
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = "", datas = datas }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 变更人员信息
        public JsonResult SetJCRYxx()
        {
            bool code = true;
            string msg = "";
            string ret = "";
            try
            {
                string rybh = Request["rybh"].GetSafeString();
                string json = Request["jsondata"].GetSafeString();
                //    code=JcjtService.SaveJCRYSYXM(rybh, json);
                string usercode = Request["usercode"].GetSafeString();
                string rolecodelist = Request["rolecodelist"].GetSafeString();
                ret = JcjtService.ModifyUserRoleByUsercodeAndRolecodeList(usercode, rolecodelist);

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, ret = ret }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置检测人员角色关联的试验项目
        /// </summary>
        /// <returns></returns>
        public JsonResult SetJCRYSYXM()
        {
            bool code = true;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string json = Request["jsondata"].GetSafeString();
                code = JcjtService.SaveJCRYSYXM(usercode, json);
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //获取人员的角色项目
        [LoginAuthorize]
        public JsonResult GetJCRYRoleSYXM()
        {
            bool code = true;
            string msg = "";
            string ret = "";
            //List<JcRoles> datas = new List<JcRoles>();
           var datas = new List<string>();
            try
            {
                //    string usercode = Request["usercode"].GetSafeString();
                //    //string rolecode = Request["rolecode"].GetSafeString();
                //    string method = Request["method"].GetSafeString();
                //    string opt = Request["opt"].GetSafeString();
                //    if (method.ToLower() == "role" && opt.ToLower() == "getrolelistbyusercode") //获取用户角色
                //    {
                //        string page = Request["page"].GetSafeString("1");
                //        string rows = Request["rows"].GetSafeString("1000");
                //        string cpcode = Request["cpcode"].GetSafeString();
                //        IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                //        if (dt.Count > 0)
                //        {
                //            cpcode = dt[0]["cpcode"].GetSafeString(); //"CP201611000001";//
                //        }
                //        string procode = Configs.AppId;// "WZJDBG";
                //                                       //   ret = JcjtService.GetRoleListByUsercode(page, rows, usercode, cpcode, procode, "");
                //        datas = JcjtService.GetJCRYRoleSYXM(usercode, cpcode, procode, page, rows);
                //    }

                //    //datas = JcjtService.GetJCRYRoleSYXM(usercode, rolecode);
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, datas = datas }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        #region 获取当前用户按钮权限
        [LoginAuthorize]
        public JsonResult GetUserBtnRoleListByLogoner()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            object datas = null;
            StringBuilder sb = new StringBuilder();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //string btnrolesStr = CurrentUser.ButtonRole;
                //if (string.IsNullOrEmpty(btnrolesStr))
                //{
                //    string sql = $"select b.* from VIEW_MENU_BUTTON b where b.isdisabled = 1 or btncode in (select btncode from M_MENU_BUTTON_ROLE where ROLECODE in ({CurrentUser.Roles.FormatSQLInStr()}))";
                //    dt = CommonService.GetDataTable(sql);
                //    datas = dt;
                //    CurrentUser.CurUser.ButtonRole = JsonSerializer.Serialize(dt);
                //}
                //else
                //    datas = jss.Deserialize<IList<IDictionary<string, string>>>(btnrolesStr);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return Json(new { code = code ? "0" : "1", msg = msg, data = datas }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion



      
    }
}
