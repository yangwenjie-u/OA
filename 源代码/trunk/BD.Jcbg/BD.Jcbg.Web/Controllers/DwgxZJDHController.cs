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
using System.Web.UI;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using System.Text.RegularExpressions;
using Spring.Transaction.Interceptor;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using BD.Jcbg.Web.Func;
using System.Net;
using NPOI.XSSF.UserModel;
using CryptFun = BD.Jcbg.Common.CryptFun;
using BD.Jcbg.Web.Remote;
using Newtonsoft.Json;
using NHibernate;
using BD.IDataInputBll;
using System.Collections;
using System.ServiceModel;
using NPOI.XWPF.UserModel;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Data.SqlClient;
using ReportPrint.Common;



namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 浙江大和个性化控制器
    /// </summary>
    public class DwgxZJDHController : Controller
    {
        #region 服务
        private IUpBgsdscService _iUpBgsdscService = null;
        private IUpBgsdscService UpBgsdscService
        {
            get
            {
                if (_iUpBgsdscService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _iUpBgsdscService = webApplicationContext.GetObject("UpBgsdscService") as IUpBgsdscService;
                }
                return _iUpBgsdscService;
            }
        }


        private static ISystemService _systemService = null;
        private static ISystemService SystemService
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

        private IJdbgService _jdbgService = null;
        private IJdbgService JdbgService
        {
            get
            {
                if (_jdbgService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jdbgService = webApplicationContext.GetObject("JdbgService") as IJdbgService;
                }
                return _jdbgService;
            }
        }

        private IWorkFlowService _workflowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workflowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workflowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workflowService;
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

        private IDataInputService _dataInputService = null;
        private IDataInputService DataInputService
        {
            get
            {
                if (_dataInputService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataInputService = webApplicationContext.GetObject("DataInputService") as IDataInputService;
                }
                return _dataInputService;
            }
        }
        private INewsArtcleService _newsArtcleService = null;
        private INewsArtcleService NewsArtcleService
        {
            get
            {
                if (_newsArtcleService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _newsArtcleService = webApplicationContext.GetObject("NewsArtcleService") as INewsArtcleService;
                }
                return _newsArtcleService;
            }
        }

        private INewsAttachService _newsAttachService = null;
        private INewsAttachService NewsAttachService
        {
            get
            {
                if (_newsAttachService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _newsAttachService = webApplicationContext.GetObject("NewsAttachService") as INewsAttachService;
                }
                return _newsAttachService;
            }
        }

        private IGclrWxyFileService _gclrWxyFileService = null;
        private IGclrWxyFileService GclrWxyFileService
        {
            get
            {
                if (_gclrWxyFileService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _gclrWxyFileService = webApplicationContext.GetObject("GclrWxyFileService") as IGclrWxyFileService;
                }
                return _gclrWxyFileService;
            }
        }
        private IAlertService _alertService = null;
        private IAlertService AlertService
        {
            get
            {
                if (_alertService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _alertService = webApplicationContext.GetObject("AlertService") as IAlertService;
                }
                return _alertService;
            }
        }
        private IReportWWGZfileService _reportWWGZfileService = null;
        private IReportWWGZfileService ReportWWGZfileService
        {
            get
            {
                if (_reportWWGZfileService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _reportWWGZfileService = webApplicationContext.GetObject("ReportWWGZfileService") as IReportWWGZfileService;
                }
                return _reportWWGZfileService;
            }
        }

        private IReportWWGZService _reportWWGZService = null;
        private IReportWWGZService ReportWWGZService
        {
            get
            {
                if (_reportWWGZService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _reportWWGZService = webApplicationContext.GetObject("ReportWWGZService") as IReportWWGZService;
                }
                return _reportWWGZService;
            }
        }
        #endregion

        #region 页面
        [Authorize]
        public ActionResult index()
        {
            return View();
        }
        #endregion

        #region 手动上传报告
        [Authorize]
        public ActionResult UploadReportSD()
        {
            return View();
        }


        [Authorize]
        public void DoUploadReportSD()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> dt = new Dictionary<string, object>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase postfile = files[i];
                    // 文件名
                    string strSaveName = postfile.FileName;
                    if (strSaveName.LastIndexOf("\\") > -1)
                        strSaveName = strSaveName.Substring(strSaveName.LastIndexOf("\\") + 1);

                    // 扩展名
                    string ext = "";
                    if (strSaveName.IndexOf(".") > 0)
                    {
                        ext = strSaveName.Substring(strSaveName.LastIndexOf('.'), strSaveName.Length - strSaveName.LastIndexOf('.'));
                    }
                    List<string> fileexts = new List<string>()
                    {
                        ".docx",
                        ".doc"
                    };
                    if (!fileexts.Contains(ext.ToLower()))
                    {
                        ret = false;
                        msg = "文件类型不符合！";
                    }
                    else
                    {
                        byte[] postcontent = new byte[postfile.ContentLength];
                        int readlength = 0;
                        while (readlength < postfile.ContentLength)
                        {
                            int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                            readlength += tmplen;
                        }
                        if (postcontent.Length > 0)
                        {

                            #region 提取报告信息
                            dt = GetBGInfo(ext, postcontent);
                            #endregion
                            #region 上传报告的信息存储
                            UpBgsdscService.GetReportFileBySlt(strSaveName, postcontent, ext, dt);
                            #endregion
                        }
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

        [Authorize]
        public ActionResult AddUploadReportSD()
        {
            return View();
        }

        [Authorize]
        public void AddDoUploadReportSD()
        {
            bool ret = true;
            string msg = "";
            string bglx = Request["bglx"].GetSafeString();
            string bgbh = Request["bgbh"].GetSafeString();
            string wtdw = Request["wtdw"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            string syxm = Request["syxm"].GetSafeString();
            string jclb = Request["jclb"].GetSafeString();
            string bgscfs = "手动添加报告";
            Dictionary<string, object> dt = new Dictionary<string, object>();
            try
            {
                if (!string.IsNullOrEmpty(bgbh))
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase postfile = files[i];
                        // 文件名
                        string strSaveName = postfile.FileName;
                        if (strSaveName.LastIndexOf("\\") > -1)
                            strSaveName = strSaveName.Substring(strSaveName.LastIndexOf("\\") + 1);

                        // 扩展名
                        string ext = "";
                        if (strSaveName.IndexOf(".") > 0)
                        {
                            ext = strSaveName.Substring(strSaveName.LastIndexOf('.'), strSaveName.Length - strSaveName.LastIndexOf('.'));
                        }
                        List<string> fileexts = new List<string>()
                    {
                        ".docx",
                        ".doc"
                    };
                        if (!fileexts.Contains(ext.ToLower()))
                        {
                            ret = false;
                            msg = "文件类型不符合！";
                        }
                        else
                        {
                            byte[] postcontent = new byte[postfile.ContentLength];
                            int readlength = 0;
                            while (readlength < postfile.ContentLength)
                            {
                                int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                                readlength += tmplen;
                            }
                            if (postcontent.Length > 0)
                            {
                                #region 添加报告信息
                                dt.Add("bglx", bglx);
                                dt.Add("bgbh", bgbh);
                                dt.Add("wtdw", wtdw);
                                dt.Add("gcmc", gcmc);
                                dt.Add("syxm", syxm);
                                dt.Add("jclb", jclb);
                                dt.Add("bgscfs", bgscfs);
                                //dt = GetBGInfo(ext, postcontent);
                                #endregion
                                #region 上传报告的信息存储
                                UpBgsdscService.GetReportFileBySlt(strSaveName, postcontent, ext, dt);
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    ret = false;
                    msg = "报告编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

        public ActionResult ViewReport()
        {
            string id = Request["id"].GetSafeString();
            ViewBag.fid = id;
            return View();
        }

        public void GetReportFile()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string pdfid = Request["id"].GetSafeString();
                if (pdfid != "")
                {
                    code = UpBgsdscService.GetReportFile(pdfid, out file, out msg);
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

        public void GetReportFileByEWM()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, string> dt = new Dictionary<string, string>();
            try
            {
                string ewm = Request["id"].GetSafeString();
                string thumbfileid = "";
                string pdfid = "";
                if (ewm != "")
                {
                    code = UpBgsdscService.GetReportFileByEWM(ewm, out thumbfileid, out pdfid, out msg);
                    if (code)
                    {
                        if (thumbfileid != "" && pdfid != "")
                        {
                            dt.Add("picurl", "/DataInput/FileService?method=DownloadFile&fileid=" + thumbfileid);
                            dt.Add("imgurl", "/dwgxzjdh/viewreport?id=" + pdfid);
                        }
                        else
                        {
                            code = false;
                            msg = "报告信息不全";
                        }
                    }

                }
                else
                {
                    code = false;
                    msg = "提交的参数错误！";
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        // 根据文件类型，从文件中提取报告内容
        private Dictionary<string, object> GetBGInfo(string fileext, byte[] postcontent)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            try
            {
                if (fileext == ".docx" || fileext == ".doc")
                {
                    byte[] contents = postcontent;
                    if (fileext == ".doc")
                    {
                        string outstr = "";
                        string msg = "";
                        if (new OfficeConvert().ConvertDocToDocxStr(Convert.ToBase64String(contents), out outstr, out msg))
                        {
                            contents = Convert.FromBase64String(outstr);
                        }
                    }
                    Stream st = new MemoryStream(contents);
                    // 加载报告
                    StringBuilder sb = new StringBuilder();
                    XWPFDocument doc = new XWPFDocument(st);


                    // 获取报告字段配置项
                    string sdscbgconfig = Configs.GetConfigItem("sdscbg", "sdscbgconfigs.xml").GetSafeString();
                    if (sdscbgconfig != "")
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = Int32.MaxValue;
                        ArrayList ar = jss.Deserialize<ArrayList>(sdscbgconfig);
                        //foreach (var e in ar)
                        //{
                        var bgc = (Dictionary<string, object>)ar[0];
                        if (bgc != null && bgc.Count > 0)
                        {
                            //string bglx = bgc["bglx"].GetSafeString();
                            //int bglxposition = bgc["bglxposition"].GetSafeInt();
                            //string t = doc.Paragraphs[bglxposition].ParagraphText;
                            string t = "";
                            // 找到了
                            //if (t.Trim().Replace(" ", "") == bglx)
                            //{
                            //info.Add("bglx", bglx);
                            ArrayList bgfields = (ArrayList)bgc["bgfields"];
                            if (bgfields != null && bgfields.Count > 0)
                            {
                                foreach (Dictionary<string, object> f in bgfields)
                                {
                                    string fieldname = f["name"].GetSafeString();
                                    int position = f["position"].GetSafeInt();
                                    string prefix = f["prefix"].GetSafeString();
                                    t = doc.Paragraphs[position].ParagraphText.Trim();
                                    if (prefix != "")
                                    {
                                        t = t.Replace(" ", "").Replace(prefix, "");
                                    }
                                    if (info.ContainsKey(fieldname))
                                    {
                                        info[fieldname] = t;
                                    }
                                    else
                                    {
                                        info.Add(fieldname, t);
                                    }

                                }
                                info.Add("bgscfs", "上传报告");
                                //}
                                //break;
                            }
                            else
                            {
                                //continue;
                            }
                            // }
                        }
                    }

                    for (int i = 0; i < doc.Paragraphs.Count; i++)
                    {
                        XWPFParagraph p = doc.Paragraphs[i];
                        var text = p.ParagraphText;
                        sb.AppendLine(string.Format("p{0}:{1}", i.ToString(), text));

                    }

                    st.Close();
                    SysLog4.WriteError(sb.ToString());
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return info;
        }


        [Authorize]
        public JsonResult DoDeleteReport(string id, string filename)
        {
            bool code = true;
            string msg = "";
            try
            {

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        [Authorize]
        public JsonResult DeleteReport(string id)
        {
            bool code = true;
            string msg = "";
            try
            {
                string sql = string.Format(" delete from up_bgsdsc where recid={0}", id);
                CommonService.Execsql(sql);
                string bm = "up_bgsdsc";
                if (DelRecord(bm, id, out msg))
                { }
                else
                {
                    throw new Exception(msg);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion

        #region
        /// <summary>
        /// 记录删除日志
        /// </summary>
        /// <returns></returns>
        public bool DelRecord(string bm, string id, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = string.Format("insert into up_user_record(ZLID,SCBM,NAME,SCSJ) values('{0}','{1}','{2}','{3}')", id, bm, CurrentUser.UserCode, DateTime.Now);
                CommonService.Execsql(sql);
                code = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            return code;
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePass()
        {
            string username = CurrentUser.RealUserName.GetSafeString();
            ViewData["username"] = username;
            return View();
        }
        #endregion

        #region 防伪报告首页
        public void GetReportFileByEWMV2()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, string> dt = new Dictionary<string, string>();
            try
            {
                string ewm = Request["id"].GetSafeString();
                string thumbfileid = "";
                string pdfid = "";
                if (ewm != "")
                {
                    code = UpBgsdscService.GetReportFileByEWM(ewm, out thumbfileid, out pdfid, out msg);
                    if (code)
                    {
                        if (thumbfileid != "" && pdfid != "")
                        {
                            dt.Add("picurl", "/DataInput/FileService?method=DownloadFile&fileid=" + thumbfileid);
                            dt.Add("imgurl", "/dwgxzjdh/viewreportv2?id=" + ewm);
                        }
                        else
                        {
                            code = false;
                            msg = "报告信息不全";
                        }
                    }

                }
                else
                {
                    code = false;
                    msg = "提交的参数错误！";
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        public ActionResult ViewReportV2()
        {
            string id = Request["id"].GetSafeString();
            ViewBag.fid = id;
            return View();
        }

        public void GetReportFileV2()
        {
            bool code = false;
            string msg = "";
            try
            {
                string ewm = Request["id"].GetSafeString();
                string reportfile = Request["reportfile"].GetSafeString();
                if (ewm != "")
                {
                    var g = new ReportPrint.GenerateGuid();
                    var c = g.Get();
                    c.type = ReportPrint.EnumType.Word;
                    c.libType = ReportPrint.LibType.OpenXmlSdk;
                    c.openType = ReportPrint.OpenType.PDF;
                    c.fileindex = "0";
                    c.table = "view_up_bgsdsc";
                    c.filename = reportfile;
                    c.where = "ewm='" + ewm + "'";
                    c.signindex = 0;
                    c.AllowVisitNum = 1;
                    byte[] pdfbytes = null;
                    code = g.GetFile(c, out pdfbytes, out msg);
                    if (code && pdfbytes != null)
                    {
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "application/pdf";
                        Response.BinaryWrite(pdfbytes);
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
        #endregion

        #region 按照新的规则，重新生成二维码
        public void ReGenerateReport()
        {
            bool ret = true;
            string msg = "";
            string fileid = Request["fileid"].GetSafeString();
            try
            {
                ret = UpBgsdscService.ReGenerateReport(fileid, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }


        }

        public void GetJDCCLXD()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string fileid = Request["fileid"].GetSafeString();
                if (fileid != "")
                {
                    code = UpBgsdscService.GetReportFile(fileid, out file, out msg);
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
        #endregion
    }
}