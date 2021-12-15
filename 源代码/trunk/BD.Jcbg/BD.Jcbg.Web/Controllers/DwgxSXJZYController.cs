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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Bd.jcbg.Common;
using BD.Jcbg.Web.Func.SCXPT;


namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 绍兴市建筑业企业资质管理个性化控制器
    /// </summary>
    public class DwgxSXJZYController : Controller
    {
        #region 服务

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

        private IFormAPIService _formAPIService = null;
        private IFormAPIService FormAPIService
        {
            get
            {
                if (_formAPIService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _formAPIService = webApplicationContext.GetObject("FormAPIService") as IFormAPIService;
                }
                return _formAPIService;
            }
        }

        private IJcjgBgService _jcjgBgService = null;
        private IJcjgBgService JcjgBgService
        {
            get
            {
                if (_jcjgBgService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jcjgBgService = webApplicationContext.GetObject("JcjgBgService") as IJcjgBgService;
                }
                return _jcjgBgService;
            }
        }

        private IDwgxSxjzyService _dwgxSxjzyService = null;
        private IDwgxSxjzyService DwgxSxjzyService
        {
            get
            {
                if (_dwgxSxjzyService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dwgxSxjzyService = webApplicationContext.GetObject("DwgxSxjzyService") as IDwgxSxjzyService;
                }
                return _dwgxSxjzyService;
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

        private IDataFileService _dataFileService = null;
        private IDataFileService DataFileService
        {
            get
            {
                if (_dataFileService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataFileService = webApplicationContext.GetObject("DataFileService") as IDataFileService;
                }
                return _dataFileService;
            }
        }
        #endregion


        #region 页面


        /// <summary>
        /// 务工人员首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult index()
        {
            return View();
        }

        /// <summary>
        /// 务工人员首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult show()
        {
            return View();
        }

        /// <summary>
        /// 务工人员首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult wgryindex()
        {



            return View();
        }


        /// <summary>
        /// 务工人员工程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult wgcIndex()
        {
            
            return View();
        }
        /// <summary>
        /// 务工人员企业
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult wqyIndex()
        {



            return View();
        }

        /// <summary>
        /// 企业首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeQy()
        {
            return View();
        }
        /// <summary>
        /// 人员首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeRy()
        {
            return View();
        }
        /// <summary>
        /// 质量安全监督站首页
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult WelcomeZaj()
        {
            return View();
        }
        /// <summary>
        /// 监管处首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeJgc()
        {
            return View();
        }

        /// <summary>
        /// 建管局首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WelcomeJgj()
        {
            return View();
        }
        #endregion


        #region 数据处理
        



        /// <summary>
        /// 获取跳转列表
        /// </summary>
        [Authorize]
        public void GetJUMPList()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();

            try
            {

                string sql = "select lx, title,href,backgroupimage from sys_welcome_jump where sfyx=1 order by xssx";
                dt = CommonService.GetDataTable2(sql);


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"rows\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        public void jump()
        {
            string lx = Request["lx"].GetSafeString();
            try
            {
                if (lx != "")
                {
                    string username = "";
                    string pwd = "";
                    string jumpurl = "";
                    string redirecturl = "";
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    string sql = string.Format("select username,pwd,jumpurl,redirecturl from sys_welcome_jump where sfyx = 1 and lx='{0}'", lx);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        username = dt[0]["username"].GetSafeString();
                        pwd = dt[0]["pwd"].GetSafeString();
                        jumpurl = dt[0]["jumpurl"].GetSafeString();
                        redirecturl = dt[0]["redirecturl"].GetSafeString();

                    }
                    if (username != "" && pwd != "" && jumpurl != "")
                    {

                        string token = RSAUtil.GetToken(username, "", pwd);
                        string url = Server.UrlEncode(redirecturl);
                        Response.Redirect(jumpurl + "?ClientKey=" + token + "&ul=" + url);
                    }

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

        }
        
  


        #region 务工人员接口

        

        public string SendDataByGET(string Url)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }



        public string SendDataByPost(string Url, string datas)
        {
            string retString = "";
            try
            {
                // https请求
                if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }               
                
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.UTF8.GetBytes(datas);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    retString = reader.ReadToEnd();
                }


                /*
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Method = "POST";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();*/
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }


        public void SendDataByGO(string Url, string Url2)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Method = "POST";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                Response.Redirect(Url2);
                //myStreamReader.Close();
                //myResponseStream.Close();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            //return retString;
        }

        #endregion

        #endregion

        #region 报表打印下载
        /// <summary>
        /// 流程报表 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult FlowReport()
        {
            string url = "";
            string reportFile = Request["reportfile"].GetSafeString();
            //SysLog4.WriteError(reportFile);
            string serial = Request["serial"].GetSafeString();
            string type = Request["type"].GetSafeString();
            string templatetype = Request["templatetype"].GetSafeString("word");
            int jdjlid = Request["jdjlid"].GetSafeInt();
            int isprint = Request["print"].GetSafeInt(1);
            string opentype = Request["opentype"].GetSafeString();
            StForm form = WorkFlowService.GetForm(serial);
            int formid = 0;
            string gcbh = "";

            if (form != null)
            {
                formid = form.Formid;
                gcbh = form.ExtraInfo3;
            }



            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (templatetype == "excel")
            {
                c.type = ReportPrint.EnumType.Excel;
            }
            c.libType = ReportPrint.LibType.OpenXmlSdk;
            c.openType = ReportPrint.OpenType.PDF;
            if (opentype == "filedown")
            {
                c.openType = ReportPrint.OpenType.FileDown;
            }

            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "stformitem|view_i_m_gc|view_gc_ry|view_gc_qy|view_gc_xctp|jdbg_jdjl_xq|jdbg_jdjl|view_zgdhf_ztfj|view_zgdhf_zgtmhffj|view_zgd_zgtmfj|view_jdbg_zgdcfjl_last|view_zgdyq_ztfj|view_zgd_zgtm|view_jgys_fjmc|view_jgys_fj";
            c.filename = reportFile;
            //c.field = "formid";

            c.where = "formid=" + formid + "|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|parentid=" + jdjlid + "|recid=" + jdjlid + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid + "|workserial='" + serial + "'" + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid;

            c.signindex = 2;
            if (isprint == 1)
            {
                c.customtools = "1,|2,|12,下载";
            }

            else
                c.customtools = "2,";

            c.AllowVisitNum = 1;


            // 获取客户端传入的reporttype
            string reporttype = Request["reporttype"].GetSafeString();
            // 需要替换的字典
            Dictionary<string, object> rd = new Dictionary<string, object>()
            {
                { "formid", formid },
                { "gcbh", gcbh },
                { "parentid", jdjlid },
                { "recid", jdjlid },
                { "xformid", formid },
                { "serial", serial},
                { "reporttype", reporttype},
                { "reportfile", reportFile},
                { "isprint", isprint},
                { "type", type}
            };

            #region 根据reporttype获取table,where
            // 如果客户端传入reporttype参数，从数据库配置表HELP_REPORT获取tablename和where
            // jcl -- 2017-11--16
            if (reporttype != "")
            {
                string tables = "";
                string wheres = "";
                try
                {
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(string.Format("select * from help_reporttype where reporttype='{0}'", reporttype));
                    if (dt.Count > 0)
                    {
                        //构造数据库中配置的数据源字典
                        List<string> tlist = new List<string>();
                        List<string> wlist = new List<string>();
                        foreach (var row in dt)
                        {
                            string t = row["tablename"].GetSafeString();
                            string w = row["wherestr"].GetSafeString();
                            // 如果表名不为空（这里包含了where条件为空的情况，按需求可能需要修改）
                            if (t != "")
                            {
                                // 替换数据库配置项的值
                                foreach (var r in rd)
                                {
                                    Regex reg = new Regex("\\{" + r.Key + "\\}", RegexOptions.IgnoreCase);
                                    w = reg.Replace(w, r.Value.GetSafeString());
                                }
                                tlist.Add(t);
                                wlist.Add(w);
                            }
                        }
                        if (tlist.Count > 0 && tlist.Count == wlist.Count)
                        {
                            tables = string.Join("|", tlist.ToArray());
                            wheres = string.Join("|", wlist.ToArray());
                        }
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }


                if (tables != "" && wheres != "")
                {
                    c.table = tables;
                    c.where = wheres;
                }


            }
            #endregion

            if (type == "download")
            {
                c.openType = ReportPrint.OpenType.PDFFileDown;
            }
            var guid = g.Add(c);
            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);



        }

        [Authorize]
        public ActionResult FlowReportDownOffice()
        {
            string url = "";
            string reportFile = Request["reportfile"].GetSafeString();
            //SysLog4.WriteError(reportFile);
            string serial = Request["serial"].GetSafeString();
            string type = Request["type"].GetSafeString();
            string templatetype = Request["templatetype"].GetSafeString("word");
            int jdjlid = Request["jdjlid"].GetSafeInt();
            int isprint = Request["print"].GetSafeInt(1);
            string opentype = Request["opentype"].GetSafeString();
            string filename = Request["filename"].GetSafeString();
            StForm form = WorkFlowService.GetForm(serial);
            int formid = 0;
            string gcbh = "";

            if (form != null)
            {
                formid = form.Formid;
                gcbh = form.ExtraInfo3;
            }



            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (templatetype == "excel")
            {
                c.type = ReportPrint.EnumType.Excel;
            }
            c.libType = ReportPrint.LibType.OpenXmlSdk;
            c.openType = ReportPrint.OpenType.FileDown;
            if (filename != "")
            {
                c.filedownname = filename;
            }

            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "stformitem|view_i_m_gc|view_gc_ry|view_gc_qy|view_gc_xctp|jdbg_jdjl_xq|jdbg_jdjl|view_zgdhf_ztfj|view_zgdhf_zgtmhffj|view_zgd_zgtmfj|view_jdbg_zgdcfjl_last|view_zgdyq_ztfj|view_zgd_zgtm|view_jgys_fjmc|view_jgys_fj";
            c.filename = reportFile;
            //c.field = "formid";

            c.where = "formid=" + formid + "|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|parentid=" + jdjlid + "|recid=" + jdjlid + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid + "|workserial='" + serial + "'" + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid;

            c.signindex = 2;
            if (isprint == 1)
            {
                c.customtools = "1,|2,|12,下载";
            }

            else
                c.customtools = "2,";

            c.AllowVisitNum = 1;


            // 获取客户端传入的reporttype
            string reporttype = Request["reporttype"].GetSafeString();
            // 需要替换的字典
            Dictionary<string, object> rd = new Dictionary<string, object>()
            {
                { "formid", formid },
                { "gcbh", gcbh },
                { "parentid", jdjlid },
                { "recid", jdjlid },
                { "xformid", formid },
                { "serial", serial},
                { "reporttype", reporttype},
                { "reportfile", reportFile},
                { "isprint", isprint},
                { "type", type}
            };

            #region 根据reporttype获取table,where
            // 如果客户端传入reporttype参数，从数据库配置表HELP_REPORT获取tablename和where
            // jcl -- 2017-11--16
            if (reporttype != "")
            {
                string tables = "";
                string wheres = "";
                try
                {
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(string.Format("select * from help_reporttype where reporttype='{0}'", reporttype));
                    if (dt.Count > 0)
                    {
                        //构造数据库中配置的数据源字典
                        List<string> tlist = new List<string>();
                        List<string> wlist = new List<string>();
                        foreach (var row in dt)
                        {
                            string t = row["tablename"].GetSafeString();
                            string w = row["wherestr"].GetSafeString();
                            // 如果表名不为空（这里包含了where条件为空的情况，按需求可能需要修改）
                            if (t != "")
                            {
                                // 替换数据库配置项的值
                                foreach (var r in rd)
                                {
                                    Regex reg = new Regex("\\{" + r.Key + "\\}", RegexOptions.IgnoreCase);
                                    w = reg.Replace(w, r.Value.GetSafeString());
                                }
                                tlist.Add(t);
                                wlist.Add(w);
                            }
                        }
                        if (tlist.Count > 0 && tlist.Count == wlist.Count)
                        {
                            tables = string.Join("|", tlist.ToArray());
                            wheres = string.Join("|", wlist.ToArray());
                        }
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }


                if (tables != "" && wheres != "")
                {
                    c.table = tables;
                    c.where = wheres;
                }


            }
            #endregion
            var guid = g.Add(c);
            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);



        }
        #endregion



        #region 大屏统计
        

        #region 流程用户相关
        /// <summary>
        /// 获取有效用户
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 获取有效用户
        /// </summary>
        /// <returns></returns>
        private IList<VUser> GetValidUsers()
        {
            IList<VUser> validUsers = new List<VUser>();

            string strIncludeDeps = "";
            string strExcludeDeps = Configs.GetConfigItem("zcqybm") + "," + Configs.GetConfigItem("zcrybm");
            string strIncludeUsers = "";
            string strExcludeUsers = "bgadmin";

            try
            {
                validUsers = RemoteUserService.GetFlowUsers(BD.WorkFlow.Common.WorkFlowConfig.FlowManager);
                if (strIncludeDeps != "")
                {
                    var q = from e in validUsers where ("," + strIncludeDeps + ",").IndexOf("," + e.DepartmentId + ",") > -1 || ("," + strIncludeDeps + ",").IndexOf("," + e.CompanyId + ",") > -1 select e;
                    validUsers = q.ToList<VUser>();
                }
                if (strExcludeDeps != "")
                {
                    var q = from e in validUsers where ("," + strExcludeDeps + ",").IndexOf("," + e.DepartmentId + ",") == -1 && ("," + strIncludeDeps + ",").IndexOf("," + e.CompanyId + ",") == -1 select e;
                    validUsers = q.ToList<VUser>();
                }
                if (strIncludeUsers != "")
                {
                    var q = from e in validUsers where ("," + strIncludeUsers + ",").IndexOf("," + e.UserId + ",") > -1 select e;
                    validUsers = q.ToList<VUser>();
                }
                if (strExcludeUsers != "")
                {
                    var q = from e in validUsers where ("," + strExcludeUsers + ",").IndexOf("," + e.UserId + ",") == -1 select e;
                    validUsers = q.ToList<VUser>();
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return validUsers;
        }

        /// <summary>
        /// 获取所有科长 GetKZList
        /// </summary>
        public void GetKZList()
        {

            IList<VUser> validUsers = new List<VUser>();

            string rolecodes = Request["rolecodes"].GetSafeString();

            try
            {
                if (rolecodes != "")
                {
                    validUsers = GetValidUsers();
                    IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = Remote.UserService.GetUserListByRolecode(rolecodes);
                    List<string> usercodes = urs.Select(x => x.USERCODE).ToList();
                    validUsers = validUsers.Where(x => usercodes.Contains(x.UserCode)).ToList();
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", validUsers.Count, jss.Serialize(validUsers)));
                Response.End();
            }

        }
        #endregion



        #region 用户系统交互

        public void GetTopMenus()
        {
            string msg = "";
            try
            {
                UserService.GetMenuTopLevelList("", out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            Response.Write(msg);
            Response.End();
        }


        [Authorize]
        public JsonResult GetMenusV2()
        {
            VMenuRetV2 ret = new VMenuRetV2();

            try
            {
                ret.user_pic = SkinManager.GetImagePath("Web-Icons1_03.png");
                ret.user_name = CurrentUser.RealName;
                ret.one_caidan = new List<VMenuRetV2Item1>();

                List<MenuItem> menus = CurrentUser.Menus;
                foreach (MenuItem item in menus)
                {
                    if (!item.IsGroup)
                        continue;
                    VMenuRetV2Item1 menu1 = new VMenuRetV2Item1();

                    menu1.one_caidan_pic_class = item.ImageUrl;
                    menu1.one_caidan_name = item.MenuName;
                    menu1.one_caidan_english = "";
                    menu1.MenuId = item.MenuCode;
                    menu1.topid = item.Djcd;
                    menu1.two_caidan = new List<VMenuRetV2Item2>();
                    int count = 0;
                    foreach (MenuItem subitem in menus)
                    {
                        if (subitem.ParentCode == item.MenuCode && !subitem.IsGroup)
                        {
                            VMenuRetV2Item2 menu2 = new VMenuRetV2Item2();
                            menu1.two_caidan.Add(menu2);
                            menu2.two_caidan_name = subitem.MenuName;
                            menu2.two_caidan_pic_class = subitem.ImageUrl;
                            menu2.MenuId = subitem.MenuCode;
                            menu2.MenuUrl = subitem.MenuUrl;
                            menu2.two_caidan_three = "false";
                            menu2.IsOut = ((count++) == 0) ? "true" : "false";
                        }
                    }

                    if (menu1.two_caidan.Count > 0)
                        ret.one_caidan.Add(menu1);

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetMenusV3()
        {
            VMenuRetV2 ret = new VMenuRetV2();

            try
            {
                ret.user_pic = SkinManager.GetImagePath("Web-Icons1_03.png");
                ret.user_name = CurrentUser.RealName;
                ret.one_caidan = new List<VMenuRetV2Item1>();

                List<MenuItem> menus = GetProcodeAndMenuByCuruser(umsurl);
                foreach (MenuItem item in menus)
                {
                    if (!item.IsGroup)
                        continue;
                    VMenuRetV2Item1 menu1 = new VMenuRetV2Item1();

                    menu1.one_caidan_pic_class = item.ImageUrl;
                    menu1.one_caidan_name = item.MenuName;
                    menu1.one_caidan_english = "";
                    menu1.MenuId = item.MenuCode;
                    menu1.topid = item.Djcd;
                    menu1.two_caidan = new List<VMenuRetV2Item2>();
                    int count = 0;
                    foreach (MenuItem subitem in menus)
                    {
                        if (subitem.ParentCode == item.MenuCode && !subitem.IsGroup && subitem.Procode == item.Procode)
                        {
                            VMenuRetV2Item2 menu2 = new VMenuRetV2Item2();
                            menu1.two_caidan.Add(menu2);
                            menu2.two_caidan_name = subitem.MenuName;
                            menu2.two_caidan_pic_class = subitem.ImageUrl;
                            menu2.MenuId = subitem.MenuCode;
                            menu2.MenuUrl = subitem.MenuUrl;
                            menu2.two_caidan_three = "false";
                            menu2.IsOut = ((count++) == 0) ? "true" : "false";
                        }
                    }

                    if (menu1.two_caidan.Count > 0)
                        ret.one_caidan.Add(menu1);

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }






        private string umsurl = Configs.GetConfigItem("umsurl");

        public void UmsApiService()
        {

            string err = "";
            string ret = "";
            try
            {
                string method = Request["method"].GetSafeString();
                string opt = Request["opt"].GetSafeString();

                if (method.ToLower() == "user" && opt.ToLower() == "checkuserbysfzh")
                {
                    string sfzh = Request["sfzh"].GetSafeString();
                    ret = CheckUserBySfzh(sfzh);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "adduser")
                {
                    string username = Request["username"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    string password = Request["password"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string sfzh = Request["sfzh"].GetSafeString();
                    string depcode = "";
                    string xb = Request["xb"].GetSafeString();
                    string sjhm = Request["sjhm"].GetSafeString();
                    string rolecodelist = Request["rolecodelist"].GetSafeString();
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select depcode from H_ZJZ where ZJZBH='" + cpcode + "'");
                    if (dt.Count > 0)
                        depcode = dt[0]["depcode"].GetSafeString();


                    ret = AddUser(username, realname, password, sfzh, xb, sjhm, cpcode, depcode, "", rolecodelist);
                }

                if (method.ToLower() == "role" && opt.ToLower() == "getownerrolelistbyusercode")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string usercode = Request["usercode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();

                    if (cpcode != "")
                        cpcode += ",";
                    cpcode += "CP201402000001";
                    string procode = Configs.AppId;// "WZJDBG";
                    ret = GetOwnerRoleListByUsercode(page, rows, usercode, cpcode, procode, "");
                }
                if (method.ToLower() == "role" && opt.ToLower() == "getrolelistbyusercode")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string usercode = Request["usercode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();

                    if (cpcode != "")
                        cpcode += ",";
                    cpcode += "CP201402000001";
                    string procode = Configs.AppId;// "WZJDBG";
                    ret = GetRoleListByUsercode(page, rows, usercode, cpcode, procode, "");
                }
                if (method.ToLower() == "role" && opt.ToLower() == "getrolelist")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string usercode = Request["usercode"].GetSafeString();
                    string cpname = Request["cpname"].GetSafeString();
                    string proname = Request["proname"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    if (cpcode != "")
                        cpcode += ",";
                    cpcode += "CP201402000001";
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = GetRoleList(page, rows, usercode, cpcode, cpname, procode, proname, "", "");
                }
                if (method.ToLower() == "user" && opt.ToLower() == "modifyuserinfobyusercode")
                {
                    string username = Request["username"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    string usercode = Request["usercode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string depcode = "";
                    string postdm = "";
                    string xb = Request["xb"].GetSafeString();
                    string sjhm = Request["sjhm"].GetSafeString();
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select depcode from H_ZJZ where ZJZBH='" + cpcode + "'");
                    if (dt.Count > 0)
                        depcode = dt[0]["depcode"].GetSafeString();
                    string rolecodelist = Request["rolecodelist"].GetSafeString();
                    string procode = Configs.AppId; //"WZJDBG";
                    string clearrole = Request["clearrole"].GetSafeString("true");
                    ret = ModifyUserInfoByUsercode(username, realname, usercode, xb, sjhm, cpcode, depcode, "", procode, rolecodelist, clearrole);
                }
                if (method.ToLower() == "role" && opt.ToLower() == "addroleinfo")
                {
                    string cpcode = Request["cpcode"].GetSafeString();
                    string rolename = Request["rolename"].GetSafeString();
                    string memo = Request["memo"].GetSafeString();
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = AddRoleInfo(cpcode, procode, rolename, memo);
                }

                if (method.ToLower() == "user" && opt.ToLower() == "getowneruserlistbyrolecode")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string rolecode = Request["rolecode"].GetSafeString();
                    string username = Request["username"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = GetOwnerUserListByRolecode(page, rows, rolecode, cpcode, username, realname);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "modifyuserstatusbyusercode")
                {
                    string usercode = Request["usercode"].GetSafeString();
                    string userstatus = Request["userstatus"].GetSafeString();

                    ret = ModifyUserStatusByUsercode(usercode, userstatus);
                }

                if (method.ToLower() == "userrole" && opt.ToLower() == "modifyuserrolebyrolecodeandusercodelist")
                {
                    string rolecode = Request["rolecode"].GetSafeString();
                    string usercodelist = Request["usercodelist"].GetSafeString();

                    ret = ModifyUserRoleByRolecodeAndUsercodeList(rolecode, usercodelist);
                }


                if (method.ToLower() == "power" && opt.ToLower() == "getownerpowerlistbyrolecode")
                {
                    string rolecode = Request["rolecode"].GetSafeString();


                    ret = GetOwnerPowerListByRolecode(rolecode);
                }

                if (method.ToLower() == "power" && opt.ToLower() == "savepowerbyrolecode")
                {
                    string rolecode = Request["rolecode"].GetSafeString();
                    string menulist = Request["menulist"].GetSafeString();

                    ret = SavePowerByRolecode(rolecode, menulist);
                }


                if (method.ToLower() == "power" && opt.ToLower() == "getpowerlistbyrolecode")
                {
                    string rolecode = Request["rolecode"].GetSafeString();

                    ret = GetPowerListByRolecode(rolecode);
                }


                if (method.ToLower() == "user" && opt.ToLower() == "getuserlistbymenucode")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string menucode = Request["menucode"].GetSafeString();
                    string cpcode = "";//这里没写完

                    string procode = Configs.AppId; //"WZJDBG";
                    ret = GetUserListByMenucode(page, rows, procode, cpcode, menucode);
                }
                if (method.ToLower() == "role" && opt.ToLower() == "getrolelistbymenucode")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string menucode = Request["menucode"].GetSafeString();
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = GetRoleListByMenucode(page, rows, procode, menucode);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "getuserlistbyrolecode")
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string rolecode = Request["rolecode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    ret = GetUserListByRolecode(page, rows, rolecode, cpcode, realname);
                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }


        public string CheckUserBySfzh(string sfzhm)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=CheckUserBySfzh&sfzh=" + sfzhm + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string AddUser(string username, string realname, string password, string sfzh, string xb, string sjhm, string cpcode, string depcode, string postdm, string rolecodelist)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=AddUser&username=" + username + "&realname=" + realname + "&sfzh=" + sfzh + "&password=" + password + "&cpcode=" + cpcode + "&depcode=" + depcode + "&rolecodelist=" + rolecodelist + "&postdm=" + postdm + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                    string sql = "INSERT INTO I_M_NBRY([ZH],[ZJZBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[SFYX]) VALUES('" + username + "','" + cpcode + "','" + param["usercode"] + "','" + realname + "','" + xb + "','" + sfzh + "','" + sjhm + "',1)";
                    CommonService.ExecSql(sql, out err);
                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string ModifyUserStatusByUsercode(string usercode, string userstatus)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=ModifyUserStatusByUsercode&usercode=" + usercode + "&userstatus=" + userstatus + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    CommonService.Execsql("update i_m_nbry set SFYX=" + userstatus + " where RYBH='" + usercode + "'");

                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }



        public string GetOwnerRoleListByUsercode(string page, string rows, string usercode, string cpcode, string procode, string rolename)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetOwnerRoleListByUsercode&page=" + page + "&rows=" + rows + "&usercode=" + usercode + "&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }
        public string GetRoleListByUsercode(string page, string rows, string usercode, string cpcode, string procode, string rolename)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetRoleListByUsercode&page=" + page + "&rows=" + rows + "&usercode=" + usercode + "&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string GetRoleList(string page, string rows, string usercode, string cpcode, string cpname, string procode, string proname, string rolename, string rolecode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetRoleList&page=" + page + "&rows=" + rows + "&usercode=" + usercode + "&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&cpname=" + cpname + "&proname=" + proname + "&rolecode=" + rolecode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string AddRoleInfo(string cpcode, string procode, string rolename, string memo)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=AddRoleInfo&cpcode=" + cpcode + "&procode=" + procode + "&rolename=" + rolename + "&memo=" + memo + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetOwnerUserListByRolecode(string page, string rows, string rolecode, string cpcode, string username, string realname)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetOwnerUserListByRolecode&page=" + page + "&rows=" + rows + "&rolecode=" + rolecode + "&cpcode=" + cpcode + "&username=" + username + "&realname=" + realname + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string ModifyUserInfoByUsercode(string username, string realname, string usercode, string xb, string sjhm, string cpcode, string depcode, string postdm, string procode, string rolecodelist, string clearrole)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=ModifyUserInfoByUsercode&username=" + username + "&realname=" + realname + "&usercode=" + usercode + "&procode=" + procode + "&cpcode=" + cpcode + "&depcode=" + depcode + "&postdm=" + postdm + "&rolecodelist=" + rolecodelist + "&clearrole=" + clearrole + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    //Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                    string sql = "update I_M_NBRY set zh='" + username + "',zjzbh='" + cpcode + "',ryxm='" + realname + "',sjhm='" + sjhm + "',xb='" + xb + "' where rybh='" + usercode + "'";
                    CommonService.ExecSql(sql, out err);
                }

                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }
        public string ModifyUserRoleByRolecodeAndUsercodeList(string rolecode, string usercodelist)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=UserRole&opt=ModifyUserRoleByRolecodeAndUsercodeList&rolecode=" + rolecode + "&usercodelist=" + usercodelist + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetOwnerPowerListByRolecode(string rolecode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=GetOwnerPowerListByRolecode&rolecode=" + rolecode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        public string SavePowerByRolecode(string rolecode, string menulist)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=SavePowerByRolecode&rolecode=" + rolecode + "&menulist=" + menulist + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetPowerListByRolecode(string rolecode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Power&opt=GetPowerListByRolecode&rows=1000&rolecode=" + rolecode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }



        public string GetUserListByMenucode(string page, string rows, string procode, string cpcode, string menucode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetUserListByMenucode&page=" + page + "&rows=" + rows + "&procode=" + procode + "&menucode=" + menucode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetRoleListByMenucode(string page, string rows, string procode, string menucode)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=Role&opt=GetRoleListByMenucode&page=" + page + "&rows=" + rows + "&procode=" + procode + "&menucode=" + menucode + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public string GetUserListByRolecode(string page, string rows, string rolecode, string cpcode, string realname)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetUserListByRolecode&page=" + page + "&rows=" + rows + "&rolecode=" + rolecode + "&cpcode=" + cpcode + "&realname=" + realname + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [Authorize]
        public void GetUserList()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            //IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {
                string companyid = Request["companyid"].GetSafeString();
                string realname = Request["text"].GetSafeString();
                sb.Append("[");

                string sql = "select a.zh as name,b.zjzmc as cpname,a.Rybh as id,a.ryxm as text  from i_m_nbry a, h_zjz b where a.zjzbh=b.zjzbh";
                if (companyid != "")
                    sql += " and a.zjzbh='" + companyid + "'";
                if (realname != "")
                    sql += " and a.ryxm  like '%" + realname + "%'";
                if (CurrentUser.CompanyCode != "CP201707000004")
                    sql += " and a.zjzbh='" + CurrentUser.CompanyCode + "'";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["id"] + "\",\"name\":\"" + dt[i]["name"] + "\",\"cpname\":\"" + dt[i]["cpname"] + "\",\"text\":\"" + dt[i]["text"] + "\"},");
                }

                /*
                if (companyid != "")
                {

                    var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.CPCODE.Equals(companyid, StringComparison.OrdinalIgnoreCase) && e.REALNAME.ToUpper().Contains(realname.ToUpper()) orderby e.ORDERNO ascending, e.DEPNAME ascending, e.POSTDM descending, e.REALNAME ascending select e;
                    foreach (RemoteUserService.VUser vuser in q)
                    {
                        //users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

                        sb.Append("{\"id\":\"" + vuser.USERCODE + "\",\"name\":\"" + vuser.USERNAME + "\",\"cpname\":\"" + vuser.CPNAME + "\",\"text\":\"" + vuser.REALNAME + "\"},");

                    }
                }
                else
                {

                    string sql = "select zjzbh from h_zjz where dl=0";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    for (int i = 0; i < dt.Count; i++)
                    {
                        var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.CPCODE.Equals(dt[i]["zjzbh"], StringComparison.OrdinalIgnoreCase) && e.REALNAME.ToUpper().Contains(realname.ToUpper()) orderby e.ORDERNO ascending, e.DEPNAME ascending, e.POSTDM descending, e.REALNAME ascending select e;
                        foreach (RemoteUserService.VUser vuser in q)
                        {
                            //users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

                            sb.Append("{\"id\":\"" + vuser.USERCODE + "\",\"name\":\"" + vuser.USERNAME + "\",\"cpname\":\"" + vuser.CPNAME + "\",\"text\":\"" + vuser.REALNAME + "\"},");

                        }
                    }
                }
                */

                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(sb.ToString());
                Response.End();
            }
        }



        /// <summary>
        /// 获取用户列表
        /// </summary>
        [Authorize]
        public void GetUserList2()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            int totalcount = 0;
            //IList<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            try
            {
                string companyid = Request["companyid"].GetSafeString();
                string realname = Request["text"].GetSafeString();
                //int pageindex = Request["page"].GetSafeInt(1);
                //int pagesize = Request["pagesize"].GetSafeInt(20);

                int pageindex = Request["page"].GetSafeInt(1);
                int pagesize = Request["rows"].GetSafeInt(20);

                sb.Append("[");

                string sql = "select a.zh as name,b.zjzmc as cpname,a.Rybh as id,a.ryxm as text,case when a.sfyx=1 then 1 else 0 end as sfyx from i_m_nbry a, h_zjz b where a.zjzbh=b.zjzbh";
                if (companyid != "")
                    sql += " and a.zjzbh='" + companyid + "'";
                if (realname != "")
                    sql += " and a.ryxm  like '%" + realname + "%'";
                if (CurrentUser.CompanyCode != "CP201707000004")
                    sql += " and a.zjzbh='" + CurrentUser.CompanyCode + "'";

                //IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);

                dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                /*/
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["id"] + "\",\"name\":\"" + dt[i]["name"] + "\",\"cpname\":\"" + dt[i]["cpname"] + "\",\"text\":\"" + dt[i]["text"] + "\",\"sfyx\":\"" + dt[i]["sfyx"] + "\"},");
                }

                /*
                if (companyid != "")
                {

                    var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.CPCODE.Equals(companyid, StringComparison.OrdinalIgnoreCase) && e.REALNAME.ToUpper().Contains(realname.ToUpper()) orderby e.ORDERNO ascending, e.DEPNAME ascending, e.POSTDM descending, e.REALNAME ascending select e;
                    foreach (RemoteUserService.VUser vuser in q)
                    {
                        //users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

                        sb.Append("{\"id\":\"" + vuser.USERCODE + "\",\"name\":\"" + vuser.USERNAME + "\",\"cpname\":\"" + vuser.CPNAME + "\",\"text\":\"" + vuser.REALNAME + "\"},");

                    }
                }
                else
                {

                    string sql = "select zjzbh from h_zjz where dl=0";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    for (int i = 0; i < dt.Count; i++)
                    {
                        var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.CPCODE.Equals(dt[i]["zjzbh"], StringComparison.OrdinalIgnoreCase) && e.REALNAME.ToUpper().Contains(realname.ToUpper()) orderby e.ORDERNO ascending, e.DEPNAME ascending, e.POSTDM descending, e.REALNAME ascending select e;
                        foreach (RemoteUserService.VUser vuser in q)
                        {
                            //users.Add(new KeyValuePair<string, string>(vuser.USERNAME, vuser.REALNAME));

                            sb.Append("{\"id\":\"" + vuser.USERCODE + "\",\"name\":\"" + vuser.USERNAME + "\",\"cpname\":\"" + vuser.CPNAME + "\",\"text\":\"" + vuser.REALNAME + "\"},");

                        }
                    }
                }
                */

                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                /*
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(sb.ToString());
                Response.End();*/


                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }



        public void GetCompanys()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("[");
                string sql = "select * from h_zjz where recid!=19 order by xssx asc";
                if (CurrentUser.CompanyCode == "CP201707000004")
                    sql = "select * from h_zjz where recid!=19 order by xssx asc";
                else
                    sql = "select * from h_zjz where zjzbh='" + CurrentUser.CompanyCode + "' order by xssx asc";


                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"CompanyId\":\"" + dt[i]["zjzbh"].GetSafeString() + "\",\"CompanyName\":\"" + dt[i]["zjzmc"].GetSafeString() + "\"},");
                }

                if (sb.ToString().EndsWith(","))
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(sb.ToString());
                Response.End();
            }
        }

        public void GetCompanyRoles()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string sql = "select * from H_ZJZQX order by xssx asc ";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                sb.Append(jss.Serialize(dt));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(sb.ToString());
                Response.End();
            }
        }


        /// <summary>
        /// 注销用户
        /// </summary>
        [Authorize]
        public void DoForbidenUser()
        {
            string username = Request["username"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {

                ret = Remote.UserService.ForbidenUser(username, out err);
                CommonService.Execsql("update i_m_nbry set SFYX=0 where zh='" + username + "'");

                //BD.Jcbg.Web.Remote.UserService.m_Users = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }


        [Authorize]
        public void DoAddUser()
        {
            string username = Request["username"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {
                string usercode = UserService.GetUserCode(username);

                if (usercode != "")
                {
                    ret = false;
                    err = "用户名已经存在，请更换用户名！";
                }
                else
                {

                    //这里增加一个判断，用户是否已经存在
                    string companycode = Request["cpcode"].GetSafeString();
                    string depcode = "";

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select depcode from H_ZJZ where ZJZBH='" + companycode + "'");
                    if (dt.Count > 0)
                        depcode = dt[0]["depcode"].GetSafeString();
                    string realname = Request["name"].GetSafeString();
                    string rolecode = Request["rcode"].GetSafeString();
                    string password = Request["pw"].GetSafeString();
                    if (rolecode.StartsWith(","))
                        rolecode = rolecode.TrimStart(',');
                    ret = UserService.AddUser(companycode, depcode, username, realname, rolecode, "", password, out err);
                    IList<string> roleCodes = new List<string>();

                    string[] roles = rolecode.Split(',');
                    for (int i = 0; i < roles.Length; i++)
                    {
                        if (roles[i].GetSafeString() != "")
                            roleCodes.Add(roles[i].GetSafeString());
                    }
                    //ret = UserService.UpdateUserRole(username, roleCodes, out err);
                    ret = UserService.UpdateUserRole(username, roleCodes, out err);
                    BD.Jcbg.Web.Remote.UserService.m_Users = null;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }


        [Authorize]
        public void DoUpdateUser()
        {
            string username = Request["username"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {
                string companycode = Request["cpcode"].GetSafeString();
                string depcode = "";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select depcode from H_ZJZ where ZJZBH='" + companycode + "'");
                if (dt.Count > 0)
                    depcode = dt[0]["depcode"].GetSafeString();
                string realname = Request["name"].GetSafeString();
                string rolecode = Request["rcode"].GetSafeString();
                string password = Request["pw"].GetSafeString();
                //ret = UserService.AddUser2(companycode, depcode, username, realname, rolecode, "", password, out err);
                IList<string> roleCodes = new List<string>();

                string[] roles = rolecode.Split(',');
                for (int i = 0; i < roles.Length; i++)
                {
                    if (roles[i].GetSafeString() != "")
                        roleCodes.Add(roles[i].GetSafeString());
                }
                ret = UserService.UpdateUserRole(username, roleCodes, out err);
                BD.Jcbg.Web.Remote.UserService.m_Users = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }

        public void test()
        {
            string ret = "";
            try
            {
                string teststring = "{\"success\": true,\"compress\": false,\"msg\": \"保存成功！\", \"data\": {\"usercode\": \"A001\", \"username\": \"AAA\",\"realname\": \"测试\" }}";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet UmsRet = jss.Deserialize<UmsRet>(teststring);
                //ret = UmsRet.data.ToString();
                Dictionary<string, object> param = (Dictionary<string, object>)UmsRet.data;
                ret = UmsRet.success.ToString() + ";" + UmsRet.compress.ToString() + ";" + UmsRet.msg + ";usercode:" + param["usercode"] + ";username:" + param["username"] + ";realname:" + param["realname"];
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }


        public class UmsRet
        {
            /// <summary>
            /// 返回结果
            /// </summary>
            public bool success { get; set; }
            /// <summary>
            /// 是否压缩
            /// </summary>
            public bool compress { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 对象
            /// </summary>
            public object data { get; set; }

        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Ums()
        {
            return View();
        }


        /// <summary>
        /// 用户管理界面，列表新
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Umslist()
        {
            return View();
        }

        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UmsEdit()
        {

            string username = Request["username"].GetSafeString();

            ViewBag.UserName = username;
            ViewBag.RealName = UserService.GetUserRealName(username);
            ViewBag.Companyid = UserService.GetUserCompanyCode(username);
            if (username != "")
                ViewBag.Userroles = UserService.GetUserRole(username);
            else
                ViewBag.Userroles = "";

            //增加一个获取用户权限，然后丢回去的

            return View();
        }



        [Authorize]
        public ActionResult Userlist()
        {
            return View();
        }


        [Authorize]
        public ActionResult Rolelist()
        {
            return View();
        }


        [Authorize]
        public ActionResult Powerlist()
        {
            return View();
        }


        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UmsEdit2()
        {

            string usercode = Request["usercode"].GetSafeString();

            string sql = "select zh,zjzbh,rybh,ryxm,xb,sfzhm,sjhm from dbo.I_M_NBRY where rybh='" + usercode + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.username = dt[i]["zh"];
                ViewBag.xb = dt[i]["xb"];
                ViewBag.usercode = dt[i]["rybh"];
                ViewBag.realname = dt[i]["ryxm"];
                ViewBag.sfzh = dt[i]["sfzhm"];
                ViewBag.sjhm = dt[i]["sjhm"];
                ViewBag.cpcode = dt[i]["zjzbh"];
            }



            //增加一个获取用户权限，然后丢回去的

            return View();
        }



        /// <summary>
        /// 修改自己的信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SelfEdit()
        {

            string usercode = CurrentUser.UserCode;

            string sql = "select zh,zjzbh,rybh,ryxm,xb,sfzhm,sjhm from dbo.I_M_NBRY where rybh='" + usercode + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.username = dt[i]["zh"];
                ViewBag.xb = dt[i]["xb"];
                ViewBag.usercode = dt[i]["rybh"];
                ViewBag.realname = dt[i]["ryxm"];
                ViewBag.sfzh = dt[i]["sfzhm"];
                ViewBag.sjhm = dt[i]["sjhm"];
                ViewBag.cpcode = dt[i]["zjzbh"];
            }



            //增加一个获取用户权限，然后丢回去的

            return View();
        }


        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult umsrole()
        {

            string username = Request["username"].GetSafeString();






            //增加一个获取用户权限，然后丢回去的

            return View();
        }


        #endregion

        
        #region 项目中的页面跳转
        /// <summary>
        /// 务工人员系统页面跳转
        /// 跳转页面主要用于政府端和企业端（施工单位）
        /// </summary>
        [Authorize]       
        public void gotoPage()
        {           
            try
            {
                // 当前登录用户在多个用户系统中的唯一识别码
                // 政府端固定的唯一码         
                string sfzh = "zf0011";
                // 企业端，用组织机构代码作为唯一码
                string s1 = string.Format("select zzjgdm from i_m_qy where qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                IList<IDictionary<string, string>> zhdt = CommonService.GetDataTable(s1);
                if (zhdt.Count > 0)
                {
                    sfzh = zhdt[0]["zzjgdm"];
                    SysLog4.WriteError("账号：" + sfzh);                  
                }
                string umsurl = "";
                string username = "";
                string loginurl = "";
                string pageurl = "";
                string rooturl = "";
                int checktype = 1;
                string lx = Request["lx"].GetSafeString();
                string pageid = Request["pageid"].GetSafeString();
                string sql = string.Format("select loginurl,umsurl,rooturl,checktype from SysMenuJumpLoginUrl where lx='{0}'", lx);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    loginurl = dt[0]["loginurl"];
                    umsurl = dt[0]["umsurl"];
                    rooturl = dt[0]["rooturl"];
                    checktype = dt[0]["checktype"].GetSafeInt(1);
                }
                if (checktype == 1)
                {
                    username = GetUserListBySfzh(sfzh, umsurl);
                }
                else if (checktype == 2)
                {
                    username = GetUserListBySfzh2(sfzh, rooturl);
                }
                

                sql = string.Format("select pageurl from SysMenuJumpPageUrl where lx='{0}' and pageid='{1}'", lx, pageid);
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    pageurl = dt[0]["pageurl"];
                }

                string url = HttpUtility.UrlEncode(pageurl);
                string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //string sign=MD5Util.StringToMD5Hash(timestring, true)
                string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
                sign = MD5Util.StringToMD5Hash(sign, true);
                Response.Redirect(loginurl + "?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

        }

        

        public void gotoXYPJPage()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

           
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */

            try
            {

                string username = "tzadmin";
                string pwd = "88888";
                string loginurl = "";
                string pageurl = "";
                string lx = Request["lx"].GetSafeString();
                string pageid = Request["pageid"].GetSafeString();
                string sql = string.Format("select loginurl from SysMenuJumpLoginUrl where lx='{0}'", lx);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    loginurl = dt[0]["loginurl"];
                }

                sql = string.Format("select pageurl from SysMenuJumpPageUrl where lx='{0}' and pageid='{1}'", lx, pageid);
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    pageurl = dt[0]["pageurl"];
                }
                string token = RSAUtil.GetToken(username, "", pwd);
                string url = Server.UrlEncode(pageurl);
                Response.Redirect(loginurl + "?ClientKey=" + token + "&ul=" + url);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

        }

        /// <summary>
        /// 根据唯一识别码和用户系统地址，获取username
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <param name="umsurl"></param>
        /// <returns></returns>
        public string GetUserListBySfzh(string sfzhm, string umsurl)
        {

            string err = "";
            string ret = "";
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetUserListBySfzh&sfzh=" + sfzhm + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //SysLog4.WriteError(ret);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    //Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                    IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(umsret.data);
                    if (param.Count > 0)
                    {
                        ret = param[0]["username"].GetSafeString();
                    }
                }
                else
                {
                    ret = "";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        /// <summary>
        /// 根据唯一识别码和API接口地址，获取username
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <param name="umsurl"></param>
        /// <returns></returns>
        public string GetUserListBySfzh2(string sfzhm, string rooturl)
        {

            string err = "";
            string ret = "";
            try
            {

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //string time = new DateTime(2019,1,18,13,30,0).ToString("yyyy-MM-dd HH:mm:ss");
                string md5key = "bd_32586239";
                string s = string.Format("{0}{1}{2}", sfzhm, time, md5key+md5key);
                string sign = MD5Util.StringToMD5Hash(s);                
                string url = rooturl + "/api/getqyzh";
                string data = string.Format("zzjgdm={0}&time={1}&sign={2}", HttpUtility.UrlEncode(sfzhm), time, HttpUtility.UrlEncode(sign));
                url = url + "?" + data;                
                string retstring = SendDataByGET(url);
                //SysLog4.WriteError(s);
                //SysLog4.WriteError(sign);
                //SysLog4.WriteError(url);
                //SysLog4.WriteError(retstring);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                bool retsucess = true;
                string retmsg = "";
                if (retdata !=null)
                {
                    retsucess = retdata["isSuccess"].GetSafeBool();
                    retmsg = retdata["msg"].GetSafeString();
                    if (retsucess)
                    {
                        ret = retdata["data"].GetSafeString();
                    }
                }
                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }
            return ret;
        }


        public List<MenuItem> GetProcodeAndMenuByCuruser(string umsurl)
        {
            string err = "";
            string ret = "";
            List<MenuItem> menus = new List<MenuItem>();
            try
            {
                string timestring = GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = umsurl + "/Api/Service";
                string dates = "method=User&opt=GetProcodeAndMenuByUsercode&usercode=" + CurrentUser.UserName + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                //SysLog4.WriteError(ret);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    // 获取菜单数据
                    IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(umsret.data);
                    if (param.Count > 0)
                    {
                        foreach (var r in param)
                        {
                            // 项目代码
                            string procode = r["procode"].GetSafeString();
                            // 项目下的菜单
                            IList<Dictionary<string, object>> menulist = jss.ConvertToType<IList<Dictionary<string, object>>>(r["menulist"]);
                            List<MenuItem> tempmenus = new List<MenuItem>();
                            // 遍历菜单，生成所有菜单项
                            if (menulist.Count > 0)
                            {
                                foreach (var m in menulist)
                                {
                                    string ul = m["funurl"].GetSafeString();
                                    MenuItem menu = new MenuItem();
                                    menu.DisplayOrder = (decimal)m["pxh"].GetSafeDouble();
                                    menu.ImageUrl = m["imgurl"].GetSafeString();
                                    menu.IsGroup = m["pmenucode"].GetSafeString() == "";
                                    menu.IsMenu = m["ismenu"].GetSafeString() == "1";
                                    menu.MenuCode = m["menucode"].GetSafeString();
                                    menu.MenuName = m["menuname"].GetSafeString();
                                    menu.Procode = procode;

                                    if (procode == "ZJJCJG")
                                    {
                                        if (ul != "")
                                        {
                                            ul = "/dwgxzj/gotoJCJGPage?lx=JCJG&ul=" + HttpUtility.UrlEncode(ul);
                                        }
                                    }
                                    menu.MenuUrl = ul;

                                    menu.ParentCode = m["pmenucode"].GetSafeString();
                                    menu.Memo = m["memo"].GetSafeString();
                                    menu.Djcd = m["djcd"].GetSafeString();
                                    tempmenus.Add(menu);

                                }
                            }
                            if (tempmenus.Count > 0)
                            {
                                var q = from e in tempmenus where e.ParentCode == "" orderby e.DisplayOrder ascending select e;
                                List<MenuItem> groups = q.ToList();
                                foreach (MenuItem itm in groups)
                                {
                                    q = from e in tempmenus where e.ParentCode == itm.MenuCode && e.IsMenu orderby e.DisplayOrder ascending select e;
                                    List<MenuItem> childs = q.ToList();
                                    if (childs.Count > 0)
                                    {
                                        menus.Add(itm);
                                        foreach (var c in childs)
                                        {
                                            menus.Add(c);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    //SysLog4.WriteError(jss.Serialize(menus));

                }
                else
                {
                    ret = "";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

            }

            return menus;
        }





        public void gotoJCJGPage()
        {
            try
            {
                string username = CurrentUser.RealUserName;
                string loginurl = "";
                string pageurl = Request["ul"].GetSafeString();
                string lx = Request["lx"].GetSafeString();
                string sql = string.Format("select loginurl from SysMenuJumpLoginUrl where lx='{0}'", lx);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    loginurl = dt[0]["loginurl"];
                }


                string url = HttpUtility.UrlEncode(pageurl);
                string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //string sign=MD5Util.StringToMD5Hash(timestring, true)
                string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
                sign = MD5Util.StringToMD5Hash(sign, true);
                Response.Redirect(loginurl + "?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

        }
        #endregion
        

        #region 手机APP相关
        public void DoLoginForPhone()
        {
            string err = "";
            bool ret = false;
            string departmentid = "";
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();
                password = Base64Func.DecodeBase64(password);
                ret = Remote.UserService.Login(username, password, out err);
                // 登录成功
                if (ret)
                {
                    // 设置日志系统用户
                    BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                    // 设置流程模块用户
                    BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                        new WorkFlow.Common.SessionUser()
                        {
                            CompanyId = CurrentUser.CurUser.CompanyId,
                            CompanyName = CurrentUser.CurUser.CompanyName,
                            DepartmentId = CurrentUser.CurUser.DepartmentId,
                            DepartmentName = CurrentUser.CurUser.DepartmentName,
                            DutyLevel = CurrentUser.CurUser.DutyLevel,
                            RealName = CurrentUser.CurUser.RealName,
                            UserName = CurrentUser.CurUser.UserName
                        }, null);
                    // 设置录入界面用户
                    Session["USERCODE"] = CurrentUser.UserCode;
                    Session["USERNAME"] = CurrentUser.UserName;
                    Session["REALNAME"] = CurrentUser.RealName;
                    Session["CPCODE"] = CurrentUser.CompanyCode;
                    Session["CPNAME"] = CurrentUser.CompanyName;
                    Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                    Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                    //Session["MenuCode"] = "QYGL_QYBA";

                    CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);

                    // 获取页面跳转类型
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                        CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                    else
                        CurrentUser.CurUser.UrlJumpType = "SYS";

                    if (CurrentUser.CurUser.UrlJumpType == "N" || CurrentUser.CurUser.UrlJumpType == "P")
                    {
                        departmentid = CurrentUser.CurUser.DepartmentId;
                    }

                }

                BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                {
                    ClientType = LogConst.ClientType,
                    Ip = ClientInfo.Ip,
                    LogTime = DateTime.Now,
                    ModuleName = LogConst.ModuleUser,
                    Operation = LogConst.UserOpLogin,
                    UserName = username,
                    RealName = ret ? CurrentUser.RealName : "",
                    Remark = "",
                    Result = ret
                };

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                string info = "";
                if (err == "" && ret)
                {
                    info = string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"departmentid\":\"{2}\"}}", ret ? "0" : "1", CurrentUser.CurUser.UrlJumpType, departmentid);
                }
                else
                {
                    info = string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"departmentid\":\"{2}\"}}", "1", err, departmentid);
                }
                Response.Write(info);
                Response.End();
            }
        }

        /// <summary>
        /// 企业--获取资质申报列表
        /// </summary>
        public void PhoneGetzzsbList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string zzsblxbh = Request["zzsblxbh"].GetSafeString();
            string sqzt = Request["sqzt"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {
                    string strwhere = " 1=1 ";

                    if (zzsblxbh != "")
                    {
                        strwhere += string.Format(" and zzsblxbh='{0}' ",zzsblxbh);
                    }

                    if (sqzt !="")
                    {
                        strwhere += string.Format(" and sqzt={0} ", sqzt);
                    }
                    
                    if (CurrentUser.CurUser.UrlJumpType == "Q" )
                    {
                        strwhere += " and qybh in (select qybh from i_m_qyzh where yhzh = '" + CurrentUser.UserName + "')" ;
                        
                    }
                    string sql = "select * from view_jdbg_qyzzsb where " + strwhere;

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

        public void getQyInfo()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string name = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {
                    dt = CommonService.GetDataTable("select qybh,qymc from i_m_qy where zh='" + username + "'");

                    if (dt.Count > 0)
                    {
                        code = true;
                        msg = dt[0]["qybh"];
                        name = dt[0]["qymc"];
                    }
                    else
                    {
                        code = false;
                        msg = "获取企业信息失败";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"name\":\"{2}\"}}", code ? "0" : "1", msg, name));
                Response.End();
            }
        }

        public void PhoneGetQyInfo()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string name = "";
            Dictionary<string, object> info = new Dictionary<string, object>();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qybh,qymc from i_m_qy where zh='" + username + "'");

                    if (dt.Count > 0)
                    {
                        string qybh = dt[0]["qybh"];
                        string procstr = string.Format("GetQYInfo('{0}')", qybh);
                        IList<IDictionary<string, string>> qyinfo = CommonService.ExecDataTableProc(procstr, out msg);
                        if (qyinfo.Count > 0)
                        {
                            int gccount = qyinfo[0]["gccount"].GetSafeInt();
                            int rycount = qyinfo[0]["rycount"].GetSafeInt();
                            int zzcount = qyinfo[0]["zzcount"].GetSafeInt();
                            info.Add("gccount", gccount);
                            info.Add("rycount", rycount);
                            info.Add("zzcount", zzcount);
                        }

                    }
                    else
                    {
                        code = false;
                        msg = "获取企业信息失败";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(info)));
                Response.End();
            }
        }

        public void GetZhInfo()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string name = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {
                    name = CurrentUser.RealName;

                    /*
                    if (CurrentUser.CurUser.UrlJumpType == "Q")
                    {

                        string sql = "select qylxs from View_I_M_QY_WITH_ZZ where yhzh='" + CurrentUser.UserName + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            msg = dt[0]["qylxs"].GetSafeString();
                        }
                    }*/
                    List<MenuItem> menus = new List<MenuItem>();
                    menus = CurrentUser.Menus;
                    //二维码登记
                    var canedit = (from a in menus
                                   where a.MenuCode.Equals("SBGL_BZJGL")
                                   select a).ToList();
                    if (canedit.Count > 0)
                        msg += ",ewmzc";
                    var isazdw = (from a in menus
                                  where a.MenuCode.Equals("SBGL_BZJLR")
                                  select a).ToList();
                    if (isazdw.Count > 0)
                        msg += ",ewmzc";
                    //安装施工确认
                    var azsgqr = (from a in menus
                                  where a.MenuCode.Equals("SBGL_SGAZQR")
                                  select a).ToList();
                    if (azsgqr.Count > 0)
                        msg += ",azsgqr";
                    //安装监理确认
                    var azjlqr = (from a in menus
                                  where a.MenuCode.Equals("SBGL_ZAJL")
                                  select a).ToList();
                    if (azjlqr.Count > 0)
                        msg += ",azjlqr";
                    //安装监督站
                    var azjdz = (from a in menus
                                 where a.MenuCode.Equals("SBGL_ZAJDZ")
                                 select a).ToList();
                    if (azjdz.Count > 0)
                        msg += ",azjjdz";


                    //使用监理确认
                    var syjlqr = (from a in menus
                                  where a.MenuCode.Equals("SBGL_SYJLQR")
                                  select a).ToList();
                    if (syjlqr.Count > 0)
                        msg += ",syjlqr";
                    //使用监督站
                    var syjdz = (from a in menus
                                 where a.MenuCode.Equals("SBGL_SYJDZ")
                                 select a).ToList();
                    if (syjdz.Count > 0)
                        msg += ",syjdz";

                    //拆卸施工确认
                    var cxsgqr = (from a in menus
                                  where a.MenuCode.Equals("SBGL_SGCXQR")
                                  select a).ToList();
                    if (cxsgqr.Count > 0)
                        msg += ",cxsgqr";
                    //拆卸监理确认
                    var cxjlqr = (from a in menus
                                  where a.MenuCode.Equals("SBGL_CXJL")
                                  select a).ToList();
                    if (cxjlqr.Count > 0)
                        msg += ",cxjlqr";
                    //拆卸监督站
                    var cxjdz = (from a in menus
                                 where a.MenuCode.Equals("SBGL_CXJDZ")
                                 select a).ToList();
                    if (cxjdz.Count > 0)
                        msg += ",cxjdz";


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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"name\":\"{2}\"}}", code ? "0" : "1", msg, name));
                Response.End();
            }
        }

        public void getProjectList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {
                    string strwhere = "";

                    if (key != "")
                    {
                        strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                    }
                    string sql = "";
                    if (CurrentUser.CurUser.UrlJumpType == "Q" || CurrentUser.CurUser.UrlJumpType == "R")
                    {
                        sql += " from View_GC_QYGL where QYBH in (select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')  and ZT<>'YT' order by gcbh desc";
                    }
                    else
                    {

                        sql = " from View_I_M_GC1 a where a.zt not in ('YT','LR') " + strwhere + " order by gcbh desc";
                    }




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

        public void PhoneGetQYList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strwhere = "";

                    if (key != "")
                    {
                        //strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                        strwhere += " and qymc like '%" + key + "%'";
                    }
                    string sql = " select * from i_M_qy where 1=1 and (qymc is not null and qymc<>'')" + strwhere + "  order by qymc asc";

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

        public void PhoneGetQYDetail()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string qybh = Request["qybh"].GetSafeString();

            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> rows = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strwhere = "";


                    string sql = " select qybh,qymc,qyfzr,lxdh,qyfr,zh,lxsj,'' as zzzt,'' as qylxmcs,'' as sszjz,'' as sprxm  from View_I_M_QY where qybh='" + qybh + "'";

                    datas = CommonService.GetDataTable(sql);
                    sql = "select * from  View_I_S_QY_QYZZ where qybh='" + qybh + "'";
                    rows = CommonService.GetDataTable(sql);

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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"xx\":{2},\"zz\":{3}}}", code ? "0" : "1", msg, jss.Serialize(datas), jss.Serialize(rows)));
                Response.End();
            }
        }

        public void PhoneGetRYList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string key = Request["key"].GetSafeString();
            string qybh = Request["qybh"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strwhere = "";

                    if (key != "")
                    {
                        //strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                        strwhere += " and ryxm like '%" + key + "%'";
                    }
                    if (qybh != "")
                        strwhere += " and qybh='" + qybh + "'";
                    string sql = " select rybh,ryxm,sjhm,sfzhm from i_m_ry where 1=1 AND (ryxm is not null and ryxm<>'') " + strwhere + " ";

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

        public void PhoneGetRYDetail()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string rybh = Request["rybh"].GetSafeString();

            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> rows = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strwhere = "";


                    string sql = " select qymc,ryxm,xb,rybh,sjhm,sfzhm,'' as nfjg,'' as sy_zt,'' as sprxm,'' as spsj,'' as sszjz,'' as qylxmcs,sy_zs from View_I_M_RY where rybh='" + rybh + "'";

                    datas = CommonService.GetDataTable(sql);
                    sql = "select * from   dbo.View_I_S_RY_RYZZ_For_DownLoad where rybh='" + rybh + "'";
                    rows = CommonService.GetDataTable(sql);

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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"xx\":{2},\"zz\":{3}}}", code ? "0" : "1", msg, jss.Serialize(datas), jss.Serialize(rows)));
                Response.End();
            }
        }

        public void PhoneScan()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string key = Request["key"].GetSafeString();

            //这里做一个写法，如果说，二维码扫描过的，返回id和类型，0表示
            string type = "0";


            string msg = "该二维码不是设备二维码，二维码内容为：" + Request["key"].GetSafeString() + "！";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> rows = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    key = CryptFun.Decode(key);

                    if (key == "" || key == null)
                    {
                        code = false;
                        msg = "该二维码不是设备二维码，二维码内容为：" + Request["key"].GetSafeString() + "！";
                    }
                    else
                    {
                        string sql = " select gcmc,cqbh from SB_ReportSBSY where State!=2 and RECID in (select SBSYID from SB_BZJADD where BZJID=(select RECID from dbo.SB_BZJ where Qrcode='" + key + "')) ";

                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {

                            code = false;
                            msg = "该标准节正在[" + datas[0]["gcmc"].GetSafeString() + "]工程的[" + datas[0]["cqbh"].GetSafeString() + "]设备中使用，无法重复使用，请确认！";
                            type = "1";
                        }
                        sql = "select useenddate from dbo.SB_BZJ where Qrcode='" + key + "'";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            //这里要改，提示语句调整下，方便用户扫描下就能看到
                            msg = "";
                            if (datas[0]["useenddate"].GetSafeDate(DateTime.MinValue) == DateTime.MinValue || DateTime.Now.AddMonths(6) >= datas[0]["useenddate"].GetSafeDate(DateTime.MinValue))
                            {
                                code = false;
                                msg = "该标准节将在" + datas[0]["useenddate"].GetSafeDate(DateTime.MinValue).ToShortDateString() + "报废，系统禁止使用！";

                            }
                            type = "1";
                        }
                        else
                        {

                            sql = "select beianicp from sb_ba where Qrcode1='" + key + "'";
                            datas = CommonService.GetDataTable(sql);
                            if (datas.Count > 0)
                            {
                                msg = datas[0]["beianicp"].GetSafeString();
                                type = "2";
                            }
                            else
                            {
                                sql = "select beianicp from sb_ba where baid=(select top 1 parentbaid from dbo.SB_BA_List where Qrcode='" + key + "' )";
                                datas = CommonService.GetDataTable(sql);
                                if (datas.Count > 0)
                                {
                                    msg = datas[0]["beianicp"].GetSafeString();
                                    type = "2";
                                }
                                else
                                {
                                    code = false;
                                    msg = "该二维码尚未使用，请确认！";
                                }
                            }

                            /*
                            code = false;
                            msg = "该二维码不存在，请确认！";*/
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"type\":{2},\"key\":\"{3}\"}}", code ? "0" : "1", msg, type, key));
                Response.End();
            }
        }


        public void PhoneGetRYJFList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string key = Request["key"].GetSafeString();
            string zt = Request["zt"].GetSafeString();
            string year = Request["year"].GetSafeInt(0).ToString();
            string rybh = Request["rybh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strwhere = "";

                    if (key != "")
                    {
                        //strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                        strwhere += " and ryxm like '%" + key + "%'";
                    }
                    if (year != "0")
                    {
                        strwhere += " and JFYEAR=" + year;
                    }
                    if (zt != "")
                        strwhere += " and RYState=" + zt;
                    if (rybh != "")
                        strwhere += " and sfzhm=(select sfzhm from i_m_RY where RYBH='" + rybh + "')";
                    string sql = " select RYBH,RYXM,SFZHM,JFYEAR, CASE RYState  WHEN 1 THEN '不良行为' WHEN 2 THEN '黑名单' ELSE '记分'  END AS RYState, Total,CASE  WHEN RYState=0 THEN '' when  KSZT=-1 then '考试不通过' when KSZT=RYState then '考试通过' else '未考试' end AS KSTZ  from I_S_RY_JF_List where 1=1 " + strwhere + "  order by JFYEAR desc";

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


        public void PhoneGetRYJFXQList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string err = "";
            string key = Request["key"].GetSafeString();
            string year = Request["year"].GetSafeInt(0).ToString();
            string rybh = Request["rybh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strwhere = "";

                    if (key != "")
                    {
                        //strwhere += " and (GCMC like '%" + key + "%' or ZJDJH like '%" + key + "%' or SY_JSDWMC like '%" + key + "%' or JLDWMC like '%" + key + "%' or SGDWMC like '%" + key + "%')";
                        strwhere += " and ryxm like '%" + key + "%'";
                    }
                    if (year != "0")
                    {
                        strwhere += " and CreateYear=" + year;
                    }

                    if (rybh != "")
                        strwhere += " and sfzhm='" + rybh + "'";
                    string sql = " SELECT RECID ,RYBH ,RYXM ,GW ,SFZHM ,GCBH ,GCMC ,QYBH ,QYMC ,CreatedUser ,CreatedRealname ,CreatedDate ,CreateYear ,JDJL ,JFZ ,JFZDX ,convert(varchar(100), JFRQ, 23) as JFRQ ,SerialNo FROM dbo.I_S_RY_JF where 1=1 " + strwhere + "  order by CreateYear desc";

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

        public void phoneGetShareFile()
        {

            string err = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    Remote.UserService.Login(username, password, out err);
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

        public ActionResult FlowSBBAReportDown()
        {

            string url = "";
            string reportFile = "起重机械产权备案表";
            string recid = Request["recid"].GetSafeString();
            string type = Request["type"].GetSafeString("down"); ;


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            //c.openType = ReportPrint.OpenType.FileDown;
            if (type == "down")
                c.openType = ReportPrint.OpenType.FileDown;
            else if (type == "pic")
                c.openType = ReportPrint.OpenType.PIC;
            else
                c.openType = ReportPrint.OpenType.Print;
            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "View_SB_BA";
            c.filename = reportFile;
            //c.field = "formid";
            c.where = "RECID=" + recid;
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            c.customtools = "2,";
            var guid = g.Add(c);


            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }

        #endregion

        #region 忘记密码
        public ActionResult resetpass()
        {
            return View();
        }

        public void CheckResetPass()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"].GetSafeString();
                string phone = Request["phone"].GetSafeString();
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["RESETPASS_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    SysLog4.WriteError(arr.Length.ToString());
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["CHANGEPASS_VERIFY_CODE"] = null;

                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from VIEW_I_M_ZH where zh='" + username + "'");
                        if (dt[0]["sum"].GetSafeInt() == 0)
                        {
                            IList<IDictionary<string, string>> ddt = CommonService.GetDataTable("select count(*) as sum from i_m_nbry where zh='" + username + "'");
                            if (ddt[0]["sum"].GetSafeInt() == 0)
                            {
                                msg = "账号不存在，请重新填写！";
                            }
                        }

                        code = msg == "";
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoResetPass()
        {
            string username = Request["username"].GetSafeString();
            string newpass = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
            string err = "";
            bool ret = false;
            try
            {
                ret = Remote.UserService.ResetPass(username, newpass, out err);

                if (ret)
                {
                    Session["USER_INFO_USERNAME"] = username;
                    Session["USER_INFO_PASSWORD"] = newpass;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }
        #endregion

        #region 企业人员注册
        public void SaveQYRegister()
        {
            string msg = "";
            bool code = true;
            string qybh = "";
            try
            {
                string zh = Request["zh"].GetSafeString().Trim();
                string qymc = Request["qymc"].GetSafeString().Trim();
                string zzjgdm = Request["zzjgdm"].GetSafeString();
                string aqfzrsj = Request["aqfzrsj"].GetSafeString().Trim();
                string qylxr = Request["qylxr"].GetSafeString().Trim();
                string lxrsj = Request["lxrsj"].GetSafeString().Trim();
                string islocal = Request["islocal"].GetSafeString().Trim();

                // 根据企业名称判断企业是否存在
                string sql = string.Format("select qybh from i_m_qy where qymc='{0}' and (zh is null or zh='')", qymc);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                // 企业已存在，更新
                if (dt.Count > 0)
                {
                    qybh = dt[0]["qybh"].GetSafeString();
                    string ss = string.Format(" update i_m_qy set lxbh='00',zh='{0}', zzjgdm='{1}',aqfzrsj='{2}',qyfzr='{3}',lxsj='{4}',islocal='{6}' where qymc='{5}' and (zh is null or zh='')", zh, zzjgdm, aqfzrsj, qylxr, lxrsj, qymc, (islocal == "1" ? "1" : "0"));
                    CommonService.Execsql(ss);
                }
                // 企业不存在，新增
                else
                {
                    qybh = "";

                    ISession session = null;
                    ITransaction transaction = null;
                    try
                    {
                        //初始化数据库信息
                        session = DataInputService.GetDBSession();
                        IDbCommand cmd = session.Connection.CreateCommand();
                        transaction = session.BeginTransaction();
                        transaction.Enlist(cmd);
                        qybh = DataInputService.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_M_QY__QYBH'|maxbhfield-ZDBH", "I_M_QY", "QYBH", null, cmd);
                        if (qybh == "")
                        {
                            transaction.Rollback();
                            code = false;
                            msg = "获取编号失败！";
                        }
                        else
                        {
                            string s = string.Format("insert into i_m_qy(lxbh,qybh,qymc,zzjgdm,qyfzr,lxsj,aqfzrsj,zh,sptg,sfyx,islocal) values('00','{0}','{1}','{2}','{3}','{4}','{5}','{6}',1,1,'{7}')", qybh, qymc, zzjgdm, qylxr, lxrsj, aqfzrsj, zh, (islocal == "1" ? "1" : "0"));
                            DataInputService.ExecSql(s, cmd);
                            transaction.Commit();
                        }

                    }
                    catch (Exception ex)
                    {

                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        code = false;
                        msg = ex.Message;
                        SysLog4.WriteLog(ex);
                    }
                    finally
                    {
                        if (session != null)
                        {
                            session.Close();
                        }
                    }

                    //CommonService.Execsql(s);

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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"qybh\":\"{2}\"}}", code ? "0" : "1", msg, qybh));
                Response.End();
            }
        }

        public void SaveRYRegister()
        {
            string msg = "";
            bool code = true;
            string rybh = "";
            try
            {
                string zh = Request["zh"].GetSafeString().Trim();
                string ryxm = Request["ryxm"].GetSafeString().Trim();
                string sfzhm = Request["sfzhm"].GetSafeString().Trim();
                string sjhm = Request["sjhm"].GetSafeString().Trim();

                // 根据身份证号码判断人员是否存在
                string sql = string.Format("select rybh from i_m_ry where sfzhm='{0}' and (zh is null or zh='')", sfzhm);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                // 人员已存在，更新
                if (dt.Count > 0)
                {
                    rybh = dt[0]["rybh"].GetSafeString();
                    string ss = string.Format(" update i_m_ry set lxbh='00', zh='{0}', ryxm='{1}',sjhm='{2}' where sfzhm='{3}' and (zh is null or zh='')", zh, ryxm, sjhm, sfzhm);
                    CommonService.Execsql(ss);
                }
                // 人员不存在，新增
                else
                {
                    rybh = "";


                    ISession session = null;
                    ITransaction transaction = null;
                    try
                    {
                        //初始化数据库信息
                        session = DataInputService.GetDBSession();
                        IDbCommand cmd = session.Connection.CreateCommand();
                        transaction = session.BeginTransaction();
                        transaction.Enlist(cmd);
                        rybh = DataInputService.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_M_RY__RYBH'|maxbhfield-ZDBH", "I_M_RY", "RYBH", null, cmd);
                        if (rybh == "")
                        {
                            transaction.Rollback();
                            code = false;
                            msg = "获取编号失败！";
                        }
                        else
                        {
                            string s = string.Format("insert into i_m_ry(lxbh,rybh,ryxm,sfzhm,sjhm,zh, sptg,sfyx) values('00','{0}','{1}','{2}','{3}','{4}',1,1)", rybh, ryxm, sfzhm, sjhm, zh);
                            DataInputService.ExecSql(s, cmd);
                            transaction.Commit();
                        }

                    }
                    catch (Exception ex)
                    {

                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        code = false;
                        msg = ex.Message;
                        SysLog4.WriteLog(ex);
                    }
                    finally
                    {
                        if (session != null)
                        {
                            session.Close();
                        }
                    }
                    //CommonService.Execsql(s);

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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"rybh\":\"{2}\"}}", code ? "0" : "1", msg, rybh));
                Response.End();
            }
        }


        public void CheckRegisterQYZDY()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string yzm = Request["yzm"].GetSafeString();
                string qymc = Request["qymc"].GetSafeString();
                string yzmE = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“发送验证码”获取验证码";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["REGISTER_VERIFY_CODE"] = null;

                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from (( select zh from i_m_ry where zh='" + username + "') union all ( select zh from i_m_qy where zh='" + username + "') union all ( select yhzh from i_m_qyzh where yhzh='" + username + "')) as t1");
                        if (dt[0]["sum"].GetSafeInt() > 0)
                            msg = "登录账号已存在";
                        else
                        {
                            if (UserService.UserExists(username))
                                msg = "账号已存在";
                        }
                        dt = CommonService.GetDataTable("select zh from I_M_QY where QYMC='" + qymc + "' and (zh is not null and zh<>'')");
                        if (dt.Count > 0)
                        {
                            msg = "【" + qymc + "】系统中已经存在，企业账户：" + dt[0]["zh"].GetSafeString();
                        }

                        code = msg == "";
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void CheckRegisterRYZDY()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string idno = Request["idno"];
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“发送验证码”获取验证码";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    DateTime timeOld = arr[1].GetSafeDate();
                    string yzmEs = arr[0];
                    int vcminutes = GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                    if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                        msg = "验证码已超时，请重新获取验证码";
                    else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                        msg = "验证码错误，请输入正确的验证码";
                    else
                    {
                        Session["REGISTER_VERIFY_CODE"] = null;

                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from i_m_ry where sfzhm='" + idno + "' and (zh is not null and zh<>'')");
                        if (dt[0]["sum"].GetSafeInt() > 0)
                            msg = "身份证号码已存在";
                        else
                        {
                            dt = CommonService.GetDataTable("select count(*) as sum from (( select zh from i_m_ry where zh='" + username + "') union all ( select zh from i_m_qy where zh='" + username + "') union all ( select yhzh from i_m_qyzh where yhzh='" + username + "')) as t1");
                            if (dt[0]["sum"].GetSafeInt() > 0)
                                msg = "登录账号已存在";
                            else
                            {
                                if (UserService.UserExists(username))
                                    msg = "账号已存在";
                            }
                        }
                        code = msg == "";
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public ActionResult QYZC()
        {
            string isfromhelplink = Request["isfromhelplink"].GetSafeString("");
            ViewBag.isfromhelplink = isfromhelplink;
            return View();
        }

        public ActionResult QYZC2()
        {
            string isfromhelplink = Request["isfromhelplink"].GetSafeString("");
            ViewBag.isfromhelplink = isfromhelplink;
            return View();
        }


        public ActionResult RYZC()
        {
            string isfromhelplink = Request["isfromhelplink"].GetSafeString("");
            ViewBag.isfromhelplink = isfromhelplink;
            return View();
        }


        public JsonResult CheckUserRegisterFirstStep(string realname, string sfzh, string sjhm)
        {
            string msg = "";
            bool code = false;
            try
            {
                realname = realname.GetSafeRequest().Trim();
                sfzh = sfzh.GetSafeRequest().Trim();
                sjhm = sjhm.GetSafeRequest().Trim();
                if (realname != "" && sfzh != "" && sjhm != "")
                {
                    string sql = string.Format("select rybh from i_m_ry where sfzhm='{0}' and (zh is null or zh='') ", sfzh);
                    IList<IDictionary<string, string>> dtt = CommonService.GetDataTable(sql);
                    if (dtt.Count > 0)
                    {
                        code = true;
                    }
                    else
                    {
                        sql = string.Format("select rybh, ryxm, sfzhm, sjhm,zh from i_m_ry where sjhm='{0}' or sfzhm='{1}' ", sjhm, sfzh);
                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);

                        // 没有手机或者身份证号码一样的，新注册
                        if (dt.Count == 0)
                        {
                            code = true;
                        }
                        else
                        {
                            // 姓名，手机，身份证一样，跳转到找回密码
                            var q = from e in dt where e["ryxm"] == realname && e["sfzhm"] == sfzh && e["sjhm"] == sjhm select e;
                            if (q.Count() > 0)
                            {

                                msg = "1";
                            }
                            else
                            {
                                //身份证一样（不管姓名），提示来站里办理
                                q = from e in dt where e["sfzhm"] == sfzh && e["zh"] != "" select e;
                                if (q.Count() > 0)
                                    msg = "2";
                                else
                                {
                                    // 手机号码一样（不管姓名），提示来站里办理
                                    q = from e in dt where e["sjhm"] == sjhm select e;
                                    if (q.Count() > 0)
                                    {
                                        msg = "3";
                                    }
                                    else
                                    {
                                        code = true;
                                    }

                                }
                            }

                        }

                    }




                }
                else
                {
                    code = false;
                    msg = "请填写完整的信息！";
                }




            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                //Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"jsondata\":\"" + data + "\"}");

            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }


        public JsonResult CheckQyValid(string qymc, string qydm)
        {
            bool code = false;
            string msg = "";
            bool toreset = false;
            try
            {
                qymc = qymc.GetSafeRequest().Trim();
                qydm = qydm.GetSafeRequest().Trim().Replace("-", "");// 从00000000-0换算成000000000
                if (qydm.Length == 0)
                {
                    code = false;
                    msg = "组织机构代码或社会统一信用代码无效";
                }
                else if (qydm.Length == 9 || qydm.Length == 18)
                {
                    // 企业名称有，账号不为空，不允许注册，跳转到忘记密码
                    string sql2 = string.Format("select qymc from i_m_qy where qymc='{0}' and (zh is not null and zh<>'')", qymc);
                    IList<IDictionary<string, string>> dtt2 = CommonService.GetDataTable(sql2);
                    if (dtt2.Count > 0)
                    {
                        code = false;
                        toreset = true;
                        msg = dtt2[0]["qymc"];
                    }
                    else
                    {
                        string sql = string.Format("select qymc from i_m_qy where qymc='{0}' and (zh is null or zh='')", qymc);
                        IList<IDictionary<string, string>> dtt = CommonService.GetDataTable(sql);
                        // 企业名称有，但是账号为空，允许注册
                        if (dtt.Count > 0)
                        {
                            code = true;
                        }
                        else
                        {
                            string where = " qymc='" + qymc + "' or zzjgdm='" + qydm + "' ";
                            // 老的组织机构代码
                            if (qydm.Length == 9)
                            {
                                string qydmlong = qydm.Insert(8, "-");// 从000000000换算成00000000-0
                                where += " or zzjgdm='" + qydmlong + "' or zzjgdm like '________" + qydm + "_' ";
                            }
                            // 新的五证合一码
                            else if (qydm.Length == 18)
                            {
                                string qydmshort = qydm.Substring(8, 9);    // 组织机构代码
                                string qydmlong = qydmshort.Insert(8, "-"); // 组织机构代码带'-'
                                where += " or zzjgdm='" + qydmshort + "' or zzjgdm='" + qydmlong + "' ";
                            }
                            sql = "select qybh, qymc, zzjgdm from i_m_qy where " + where;
                            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                            if (dt.Count == 0)
                            {
                                code = true;
                            }
                            else
                            {
                                code = false;
                                toreset = true;
                                msg = dt[0]["qymc"];
                            }
                        }


                    }


                }
                // 无效的输入
                else
                {
                    msg = "组织机构代码或社会统一信用代码无效";
                    toreset = false;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(new { code = code ? "0" : "1", toreset = toreset ? "0" : "1", msg = msg });
        }
        

        #endregion

        #endregion
        
        #region 删除内部邮件
        [Authorize]
        public void DeleteMail()
        {
            bool code = true;
            string msg = "";
            try
            {
                string idlist = Request["idlist"].GetSafeString();
                if (idlist != "")
                {
                    string[]info = idlist.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                    if (info.Length > 0)
                    {
                        foreach (var item in info)
                        {
                            string[] idinfo = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (idinfo.Length == 2)
                            {
                                int id = idinfo[0].GetSafeInt();
                                int readerid = idinfo[1].GetSafeInt();
                                code = OaService.DeleteMail(id, readerid,CurrentUser.UserName, out msg);
                            }
                        }
                    }

                }
                else
                {
                    code = false;
                    msg = "参数错误！";
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

        #region 列表界面调用第三方数据源公共接口
        /// <summary>
        /// 正式调用接口
        /// </summary>
        public void GetWebListData()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                ret = FormAPIService.GetWebListData(Request, this, out total, out data, out msg);                
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
                Response.AddHeader("Content-Type", "application/json");
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("total", total);
                info.Add("rows", data);
                Response.Write(string.Format("{{\"success\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "true" : "false", msg, jss.Serialize(info)));
                Response.End();
            }
        }
        #endregion

        #region 流程过程查看
        public ActionResult showlcgc()
        {
            string serial = Request["serial"].GetSafeString();
            ViewBag.serial = serial;
            return View();
        }

        #endregion

        #region 文件共享列表

        /// <summary>
		/// 文件共享页面
		/// </summary>
		/// <returns></returns>
		public ActionResult FileShare()
        {
            return View();
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
                string folderid= Request["folderid"].GetSafeString();
                string foldertype= Request["foldertype"].GetSafeString(ShareFolderType.All);
                string username= CurrentUser.UserName;
                string key= Request["key"].GetSafeString();
                // 我的文件夹
                string sql1 = "(select top 100 percent 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.Normal + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b where b.imagetype='1' and (a.username='" + username + "' or exists(select * from i_m_qyzh where zhlx='P' and yhzh=a.username)) order by a.foldername asc)";
                // 共享给我的文件夹（父目录为0）
                string sql2 = "(select top 100 percent 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b,companyreader c where a.recid=c.ParentId and b.imagetype='1' and c.username='" + username + "' and c.ParentEntity='" + CompanyEntityType.UserShareFileFolder + "' order by a.foldername asc)";
                // 共享给我的文件夹，父目录非0）
                string sql12 = "(select top 100 percent 0 as isvirtual,0 as fileid,a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b,companyreader c where a.parentid=" + folderid + " order by a.foldername asc)";
                // 我的文件
                string sql3 = "(select top 100 percent 0 as isvirtual,b.recid as fileid,a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.Normal + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where (b.username='" + username + "' or exists(select * from i_m_qyzh where zhlx='P' and yhzh=b.username))  order by b.filedesc asc)";
                // 共享给我的文件（查找里面用）
                string sql4 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where a.recid in (select ParentId from companyreader where username='" + username + "' and ParentEntity='" + CompanyEntityType.UserShareFile + "') or a.folderid in (select ParentId from companyreader where username='" + username + "' and ParentEntity='" + CompanyEntityType.UserShareFileFolder + "') order by b.filedesc asc)";
                // 共享给我的文件（列表用，父目录为0用）
                string sql9 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where a.recid in (select ParentId from companyreader where username='" + username + "' and ParentEntity='" + CompanyEntityType.UserShareFile + "') order by b.filedesc asc)";
                // 共享给我的文件（列表用，父目录为非零用）
                string sql11 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where  a.folderid=" + folderid + " order by b.filedesc asc)";

                // 我共享给别人的文件夹
                string sql5 = "(select top 100 percent 0 as isvirtual,0 as fileid, a.recid,a.parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,a.foldername as [filename],a.username,a.realname,a.createdtime,b.imagename,b.filedesc as filetypedesc,0 as filesize from usersharefilefolder a,sysfileimage b where b.imagetype='1' and a.username='" + username + "' and exists (select * from companyreader where parentid=a.recid and ParentEntity='" + CompanyEntityType.UserShareFileFolder + "') order by a.foldername asc)";
                // 我共享给别人的文件（查找里面用）
                string sql6 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where b.username='" + username + "' and exists (select * from companyreader where parentid=a.recid and ParentEntity='" + CompanyEntityType.UserShareFile + "') order by b.filedesc asc)";
                // 我共享给别人的文件（列表用）
                string sql10 = "(select top 100 percent 0 as isvirtual,b.recid as fileid, a.recid,a.folderid as parentid,'" + ShareFileType.File + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,b.filedesc as [filename],b.username,b.realname,b.createdtime,b.imagename,b.filetypedesc,b.filesize as filesize from usersharefile a inner join companyfilestorage b on a.fileid=b.recid where b.username='" + username + "' and exists (select * from companyreader where parentid=a.recid and ParentEntity='" + CompanyEntityType.UserShareFile + "') order by b.filedesc asc)";
                // 根目录中的其他人共享给我的
                string sql7 = "(select 1 as isvirtual,0 as fileid,0 as recid,0 as parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.OtherShare + "' as sharetype,'收到的共享' as [filename],'" + username + "' as username, '' as realname, '' as createdtime,imagename,filedesc as filetypedesc,0 as filesize from sysfileimage where imagetype='3')";
                // 根目录中我共享给别人的
                string sql8 = "(select 1 as isvirtual,0 as fileid,0 as recid,0 as parentid,'" + ShareFileType.Folder + "' as filetype,'" + ShareFolderType.MyShare + "' as sharetype,'我的共享' as [filename],'" + username + "' as username,'' as realname,'' as createdtime, imagename,filedesc as filetypedesc,0 as filesize from sysfileimage where imagetype='3')";

                string sql = "";
                if (key != "")
                    sql = "select * from (" + sql1 + " union all " + sql2 + " union all " + sql3 + " union all " + sql4 + ") as t1 where ([filename] like '%" + key + "%' or [filetypedesc] like '%" + key + "%') order by filename asc";
                else
                {
                    // 正常文件夹
                    if (foldertype == ShareFolderType.Normal)
                    {
                        sql = "select * from (" + sql1 + " union all " + sql3 + ") as t1 where parentid=" + folderid + " ";
                    }
                    // 别人共享给我的
                    else if (foldertype == ShareFolderType.OtherShare)
                    {
                        //sql = "select * from (" + sql2 + " union all " + sql4 + ") as t1 where parentid=" + folderid + " ";
                        if (folderid.GetSafeInt() == 0)
                            sql = "select * from (" + sql2 + " union all " + sql9 + ") as t1 ";
                        else
                            sql = "select * from (" + sql12 + " union all " + sql11 + ") as t1 ";
                    }
                    // 我共享给别人的
                    else if (foldertype == ShareFolderType.MyShare)
                    {
                        //sql = "select * from (" + sql5 + " union all " + sql6 + ") as t1 where parentid=" + folderid+" ";
                        sql = "select * from (" + sql5 + " union all " + sql6 + ") as t1 ";
                    }
                    else if (foldertype == ShareFolderType.All)
                    {
                        sql = "select * from (" + sql7 + " union all " + sql8 + " union all " + sql1 + " union all " + sql3 + ") as t1 where parentid=0  ";
                    }
                }
                datas = CommonService.GetDataTable(sql);
                //datas = OaService.GetShareFiles(
                //    Request["folderid"].GetSafeString(),
                //    Request["foldertype"].GetSafeString(ShareFolderType.All),
                //    CurrentUser.UserName,
                //    Request["key"].GetSafeString());
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
        #endregion


        #region weblist第三方接口杂类函数

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private string DustGetToken()
        {
            string token = "";
            try
            {
                bool needtogettoken = true;
                if (Session != null && Session["dustytoken"] != null)
                {
                    Dictionary<string, object> tokeninfo = (Dictionary<string, object>)Session["dustytoken"];
                    if (tokeninfo != null)
                    {
                        double unixtokentime = (double)tokeninfo["tokentime"];
                        string savedtoken = tokeninfo["token"].GetSafeString();
                        int expires = tokeninfo["expires"].GetSafeInt();
                        DateTime tokentime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddSeconds(unixtokentime);
                        // token 未过期, 直接拿来用
                        if (tokentime.AddSeconds(expires) > DateTime.Now)
                        {
                            token = savedtoken;
                            needtogettoken = false;
                        }
                        else // token 过期，需要重新获取                        
                        {
                            needtogettoken = true;
                        }
                    }
                }
                if (needtogettoken)
                {
                    string sql = "select * from h_tokenconfig where sfyx=1 and lx='DUSTY'";
                    IList<IDictionary<string, object>> configs = CommonService.GetDataTable2(sql);
                    if (configs.Count > 0)
                    {
                        string url = configs[0]["url"].GetSafeString();
                        string username = configs[0]["username"].GetSafeString();
                        string pwd = configs[0]["pwd"].GetSafeString();
                        string secret = configs[0]["appsecret"].GetSafeString();
                        string postdata = string.Format(
                            "UserName={0}&PassWord={1}&AppSecret={2}",
                            HttpUtility.UrlEncode(username),
                            HttpUtility.UrlEncode(MD5Util.StringToMD5Hash(pwd)),
                            HttpUtility.UrlEncode(secret)
                        );
                        string retstring = SendDataByPost(url, postdata);
                        //SysLog4.WriteError(retstring);
                        // JSON 序列化和反序列化类
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);                        
                        if (retdata != null)
                        {
                            int code = retdata["Code"].GetSafeInt();
                            // 已经获取到token
                            if (code == 0 || code == 40003)
                            {
                                Dictionary<string, object> tokeninfo = (Dictionary<string, object>)retdata["Datas"];
                                if (tokeninfo != null)
                                {
                                    double tokentime = tokeninfo["TokenTime"].GetSafeDouble();
                                    int expires = tokeninfo["Expires"].GetSafeInt();
                                    string dustytoken = tokeninfo["Token"].GetSafeString();
                                    // 获取到新的token之后，存起来
                                    if (dustytoken != "")
                                    {
                                        token = dustytoken;
                                        Session["dustytoken"] = new Dictionary<string, object>() {
                                            { "tokentime", tokentime},
                                            { "expires", expires},
                                            { "token", dustytoken}
                                        };
                                    }
                                }
                            }


                        }
                    }


                }

            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return token;
        }

        /// <summary>
        /// 修改扬尘过滤参数
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ForgeDustyFilterParams(List<KeyValuePair<string, string>> paramlist)
        {
            // 开始时间和结束时间
            string startdate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.ToString("yyyy-MM-dd");
            
            var q = paramlist.Where(x => x.Key.Equals("CurrentTime", StringComparison.OrdinalIgnoreCase));
            if (q.Count() > 0)
            {
                var p = q.First();
                string v = p.Value;
                if (v !="")
                {
                    string[] vl = v.Split('~');
                    if (vl.Length ==2)
                    {
                        startdate = vl[0];
                        enddate = vl[1];
                    }
                }                
            }
            
            paramlist.Add(new KeyValuePair<string, string>("StartTime", getUnixTimestamp(DateTime.Parse(startdate + " 00:00:00")).ToString()));
            paramlist.Add(new KeyValuePair<string, string>("EndTime", getUnixTimestamp(DateTime.Parse(enddate + " 23:59:59")).ToString()));
            
            return paramlist;
        }


        /// <summary>
        /// 时间转换成unix时间戳（秒数）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private long getUnixTimestamp(DateTime dt)
        {
            long ret = 0;
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            ret = (dt.Ticks - dt1970.Ticks) / 10000000;
            return ret;
        }

        /// <summary>
        /// 根据unix时间戳获取时间
        /// </summary>
        /// <param name="unixstamp"></param>
        /// <returns></returns>
        private DateTime getDatetimeFromUnix(double unixstamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixstamp);
        }

        /// <summary>
        /// 调用本控制器中的方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object InvokeMethod(string method, object[] parameters = null)
        {
            object ret = null;
            Type type = this.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var m = type.GetMethod(method, flags);
            if (m !=null)
            {
                ret = m.Invoke(this, parameters);
            }
            return ret;
        }

        private bool checkDustySuccess(object code)
        {
            return code.GetSafeInt() == 0;
        }

        private Dictionary<string,object> extraParamGetDustyToken(List<KeyValuePair<string, string>> paramlist)
        {
            bool code = true;
            string msg = "";
            string data =DustGetToken();
            if (data == "")
            {
                code = false;
                msg = "获取token失败！";
            }
            return new Dictionary<string, object>() {
                {"code", code },
                {"msg", msg },
                { "data",string.Format("{0}={1}", "Token", HttpUtility.UrlEncode(data))}
            };
            
        }

        private bool checkJCBGSuccess(object code)
        {
            return code.GetSafeBool();
        }
        #endregion

        #region 月报表季报表删除和修改
        public void DelYBB()
        {
            string msg = "";
            bool code = true;

            try
            {
                string serial = Request["serial"].GetSafeString();
                if (serial !="")
                {
                    string proc = string.Format("FlowDeleteYBB('{0}')", serial);
                    code = CommonService.ExecProc(proc, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        public void DelJBB()
        {
            string msg = "";
            bool code = true;

            try
            {
                string serial = Request["serial"].GetSafeString();
                if (serial != "")
                {
                    string proc = string.Format("FlowDeleteJBB('{0}')", serial);
                    code = CommonService.ExecProc(proc, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        public void CheckModifyYBB()
        {
            string msg = "";
            bool code = true;

            try
            {
                string serial = Request["serial"].GetSafeString();
                if (serial != "")
                {
                    string proc = string.Format("CheckModifyYBB('{0}')", serial);
                    IList<IDictionary<string, string>> data = CommonService.ExecDataTableProc(proc, out msg);
                    if (data.Count > 0)
                    {
                        string ret = data[0]["ret"];
                        string err = data[0]["msg"];
                        if (ret == "1")
                        {
                            code = true;
                            msg = "";
                        }
                        else
                        {
                            code = false;
                            msg = err;
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "校验修改失败！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 企业相关人员删除操作
        /// <summary>
        /// 删除技术负责人
        /// </summary>
        public void DelJSFZR()
        {
            string msg = "";
            bool code = true;

            try
            {
                string rybh = Request["rybh"].GetSafeString();
                if (rybh != "")
                {
                    code = DwgxSxjzyService.DelJSFZR(rybh, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        /// <summary>
        /// 注册建造师
        /// </summary>
        public void DelZCJZS()
        {
            string msg = "";
            bool code = true;

            try
            {
                string rybh = Request["rybh"].GetSafeString();
                if (rybh != "")
                {
                    code = DwgxSxjzyService.DelZCJZS(rybh, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        ///中级以上职称人员
        /// </summary>
        public void DelZJYSZCRY()
        {
            string msg = "";
            bool code = true;

            try
            {
                string rybh = Request["rybh"].GetSafeString();
                if (rybh != "")
                {
                    code = DwgxSxjzyService.DelZJYSZCRY(rybh, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 现场管理人员
        /// </summary>
        public void DelXCGLRY()
        {
            string msg = "";
            bool code = true;

            try
            {
                string rybh = Request["rybh"].GetSafeString();
                if (rybh != "")
                {
                    code = DwgxSxjzyService.DelXCGLRY(rybh, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 删除技术工人
        /// </summary>
        public void DelJSGR()
        {
            string msg = "";
            bool code = true;

            try
            {
                string rybh = Request["rybh"].GetSafeString();
                if (rybh != "")
                {                    
                    code = DwgxSxjzyService.DelJSGR(rybh, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 删除机械设备
        /// </summary>
        public void DelJXSB()
        {
            string msg = "";
            bool code = true;

            try
            {
                int recid = Request["recid"].GetSafeInt();
                if (recid > 0)
                {
                    code = DwgxSxjzyService.DelJXSB(recid, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        public void DelGCYJ()
        {
            string msg = "";
            bool code = true;

            try
            {
                int recid = Request["recid"].GetSafeInt();
                if (recid > 0)
                {
                    code = DwgxSxjzyService.DelGCYJ(recid, out msg);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 资质申报
        /// <summary>
        /// 获取企业资质申报类型
        /// </summary>
        public void GetQyzzSblx()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                dt = DwgxSxjzyService.GetQyzzSblx();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }


        }

        /// <summary>
        /// 获取企业基本情况
        /// </summary>
        public void GetQyJbqk()
        {
            string msg = "";
            bool code = true;
            IDictionary<string, object> data = new Dictionary<string, object>();
            
            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser!=null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if(dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh !="")
                {
                    sql = string.Format("select * from i_m_qy where qybh='{0}'", qybh);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        data.Add("qyxx", dt[0]);
                    }
                    else
                    {
                        code = false;
                        msg = "无法获取企业信息！";
                    }
                    sql = string.Format("GetQYRYTJ('{0}')", qybh);
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    dtt = CommonService.ExecDataTableProc(sql, out msg);
                    if (dtt.Count > 0)
                    {
                        data.Add("rytj", dtt[0]);
                    }

                    sql = string.Format("GetQYSBTJ('{0}')", qybh);
                    dtt = CommonService.ExecDataTableProc(sql, out msg);
                    if (dtt.Count > 0)
                    {
                        data.Add("sbtj", dtt[0]);
                    }
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 获取企业技术负责人
        /// </summary>
        public void GetQYJsfzr()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from VIEW_I_S_QY_JSFZR where qybh='{0}' and (isfreeze is null or isfreeze=0)  order by ryxm", qybh);
                    data = CommonService.GetDataTable(sql);                   
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }
        /// <summary>
        /// 获取企业注册建造师
        /// </summary>
        public void GetQYZcjzs()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from VIEW_I_S_QY_ZCJZS where qybh='{0}' and (isfreeze is null or isfreeze=0)  order by ryxm", qybh);
                    data = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 获取企业中级以上职称人员
        /// </summary>
        public void GetQYZjyszcry()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from VIEW_I_S_QY_ZJYSZCRY where qybh='{0}'  and (isfreeze is null or isfreeze=0)  order by ryxm", qybh);
                    data = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 获取企业现场管理人员
        /// </summary>
        public void GetQYXcglry()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from VIEW_I_S_QY_XCGLRY where qybh='{0}'  and (isfreeze is null or isfreeze=0)  order by ryxm", qybh);
                    data = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 获取企业技术工人
        /// </summary>
        public void GetQYJsgr()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from VIEW_I_S_QY_JSGR where qybh='{0}'  and (isfreeze is null or isfreeze=0)  order by ryxm", qybh);
                    data = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 获取企业机械设备
        /// </summary>
        public void GetQYJxsb()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from I_S_QY_SB where qybh='{0}'", qybh);
                    data = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }
        /// <summary>
        /// 获取企业工程业绩
        /// </summary>
        public void GetQYGcyj()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string qybh = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                }

                if (qybh != "")
                {
                    sql = string.Format("select * from I_S_QY_GCYJ where qybh='{0}'", qybh);
                    data = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 根据人员编号获取技术负责人简历
        /// </summary>
        public void GetJsfzrjl()
        {
            string msg = "";
            bool code = true;
            IDictionary<string, object> data = new Dictionary<string, object>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string rybh = Request["rybh"].GetSafeString();

                if (rybh != "")
                {
                    sql = string.Format("select * from I_S_QY_JSFZR where rybh='{0}'", rybh);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        data = dt[0];
                        sql = string.Format("select * from I_S_JSFZR_GZJL where rybh='{0}'", rybh);
                        dt = CommonService.GetDataTable2(sql);
                        data.Add("gzjllist", dt);
                    }
                }
                else
                {
                    code = false;
                    msg = "人员编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        /// <summary>
        /// 根据 recid获取工程业绩详情
        /// </summary>
        public void GetGcyj()
        {
            string msg = "";
            bool code = true;
            IDictionary<string, object> data = new Dictionary<string, object>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string recid = Request["recid"].GetSafeString();

                if (recid != "")
                {
                    sql = string.Format("select * from I_S_QY_GCYJ where recid={0}", recid);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        data = dt[0];
                    }
                }
                else
                {
                    code = false;
                    msg = "工程业绩ID不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        /// <summary>
        ///根据申报类型获取申报资料
        /// </summary>
        public void GetSBZL()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();

            try
            {
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                string sql = "";
                string zzsblxbh = Request["zzsblxbh"];
                
                if (zzsblxbh != "")
                {
                    sql = string.Format("select cllxbh, clmc,zt,isrequired,fjurl from VIEW_H_ZZSBLX_SBCL where zzsblxbh='{0}' order by cllxbh ", zzsblxbh);
                    data = CommonService.GetDataTable2(sql);
                }
                else
                {
                    code = false;
                    msg = "申报类型编号不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }

        /// <summary>
        /// 保存资质申请--第一步
        /// </summary>
        public void SaveZZSBSQ()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string qymc = "";
                string sqrzh = "";
                string sqrxm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        sqrzh = CurrentUser.UserName;
                        sqrxm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    sqrzh = "UR201906000001";
                    sqrxm = "浙江标点信息科技有限公司";
                }

                if (qybh != "")
                {
                    sql = string.Format("select qymc from I_M_QY where qybh='{0}'", qybh);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qymc = dt[0]["qymc"];
                    }
                }
                #endregion

                #region 生成ID
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if ((!data.ContainsKey("id")) || (data["id"] == ""))
                {
                    string procstr = "GetGuid()";
                    dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        id = dt[0]["id"];
                    }
                    data["id"] = id;
                }
                else
                {
                    id = data["id"];
                }
                #endregion

                #region 保存记录
                string zzsblxbh = "";
                string zzsblxmc = "";
                string sqlx = "";
                if ((!data.ContainsKey("zzsblxbh")) || (data["zzsblxbh"] == ""))
                {
                    code = false;
                    msg = "资质申报类型编号不能为空！";
                }
                else if ((!data.ContainsKey("zzsblxmc")) || (data["zzsblxmc"] == ""))
                {
                    code = false;
                    msg = "资质申报类型名称不能为空！";
                }
                else
                {
                    zzsblxbh = data["zzsblxbh"];
                    zzsblxmc = data["zzsblxmc"];

                    if (!data.ContainsKey("sqlx") || (data["sqlx"] == ""))
                    {
                        sqlx = "1";
                    }
                    else
                    {
                        sqlx = data["sqlx"];
                    }
                    
                    // 保存资质申报主记录
                    string procstr = string.Format("SaveJDBGQYZZSB('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                        id, qybh,qymc, zzsblxbh,zzsblxmc,sqrzh,sqrxm,sqlx
                        );
                    dt = CommonService.ExecDataTableProc(procstr, out msg);
                    // 保存所有表单项
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        List<string> lssql = new List<string>();
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }

                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }
                            lssql.Add(sql);
                        }

                        foreach (var item in existeditems)
                        {
                            if (!data.Keys.Contains(item) && (
                                item.StartsWith("jsfzr_") ||
                                item.StartsWith("Qyzcjzs_") ||
                                item.StartsWith("QYZjyszcry_") ||
                                item.StartsWith("QYJsgr_")
                                ))
                            {
                                sql = string.Format(" delete from jdbg_qyzzsb_xq where parentid={0} and itemname='{1}'", recid, item);
                                lssql.Add(sql);
                            }

                            
                        }
                        CommonService.ExecTrans(lssql);

                    }

                    procstr = string.Format("SetQYZZSBXQ('{0}')", id);
                    CommonService.ExecProc(procstr, out msg);
                    if (code)
                    {
                        msg = "";
                    }

                }
                #endregion
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }

        /// <summary>
        /// 保存资质申请，申请资料上传--第二步
        /// </summary>
        public void SaveZZSBZL()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string sqzlsczh = "";
                string sqzlscxm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        sqzlsczh = CurrentUser.UserName;
                        sqzlscxm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    sqzlsczh = "UR201906000001";
                    sqzlscxm = "浙江标点信息科技有限公司";
                }
                
                #endregion

                #region 获取id，保存数据
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if (data.ContainsKey("id") &&  (data["id"] != ""))
                {
                    id = data["id"];
                    sql = string.Format("select recid from jdbg_qyzzsb where id='{0}'", id);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }
                        
                        List<string> lssql = new List<string>();
                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }
                            
                            lssql.Add(sql);
                        }
                        if (lssql.Count > 0)
                        {
                            CommonService.ExecTrans(lssql);
                        }

                        string procstr = string.Format("SaveJDBGQYZZSBZLSC('{0}','{1}','{2}')", id, sqzlsczh, sqzlscxm);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";
                        

                        
                    }
                    else
                    {
                        code = false;
                        msg = "找不到资质申请记录！";
                    }

                }
                else
                {
                    code = false;
                    msg = "id 不能为空！";
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }
        /// <summary>
        /// 根据id 获取企业资质申报申请所有表单项
        /// </summary>
        public void GetZZSBSQXQ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string id = Request["id"].GetSafeString();
                if (id!="")
                {
                    string sql = string.Format("select itemname, itemvalue from jdbg_qyzzsb_xq where parentid in (select recid from jdbg_qyzzsb where id='{0}')", id);
                    dt = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 根据id 获取资质申报已上传的资料
        /// </summary>
        public void GetZZSBSCZL()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string sql = "select itemname, itemvalue from jdbg_qyzzsb_xq ";
                    sql += " where 1=1 ";
                    sql += " and itemname like 'zl[_]%' ";
                    sql += "and (itemname like '%[_]fj' or itemname like  '%[_]zgbmshzt' or itemname like  '%[_]zgbmshyj' or itemname like  '%[_]jsjcszt'  or itemname like  '%[_]jsjcsyj' ) ";
                    sql += string.Format(" and parentid in (select recid from jdbg_qyzzsb where id='{0}')", id);
                    dt = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 删除资质申报
        /// </summary>
        public void DelZZSB()
        {
            string msg = "";
            bool code = true;
            
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("FlowDeleteJDBGQYZZSB('{0}')", id);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 企业资质上报主管部门
        /// </summary>
        public void QyzzSbzgbm()
        {
            string msg = "";
            bool code = true;
            try
            {
                string id = Request["id"].GetSafeString();
                string zgbmbh = Request["zgbmbh"].GetSafeString();
                string zgbmmc = Request["zgbmmc"].GetSafeString();
                if (id != "")
                {
                    string sqsbzh = "UR201906000001";
                    string sqsbxm = "浙江标点信息科技有限公司";
                    if (CurrentUser.CurUser != null )
                    {
                        sqsbzh = CurrentUser.UserName;
                        sqsbxm = CurrentUser.RealName;

                    }
                    string procstr = string.Format("QYZZSBZGBM('{0}','{1}','{2}','{3}','{4}')", id,sqsbzh, sqsbxm,zgbmbh,zgbmmc);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 企业资质申报撤回
        /// </summary>
        public void QyzzWithDrawSQ()
        {
            string msg = "";
            bool code = true;
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("QYZZSBWITHDRAW('{0}')", id);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        /// 企业资质申报建设行政主管部门退回
        /// </summary>
        public void QyzzReturnBackSQ()
        {
            string msg = "";
            bool code = true;
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("QYZZSBRETURNBACK('{0}')", id);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 上传申报资料
        /// </summary>
        public void DoUploadZZSBZL()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postfile = Request.Files[0];
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
                    // 获取文件二进制数据
                    byte[] postcontent = new byte[postfile.ContentLength];
                    int readlength = 0;
                    while (readlength < postfile.ContentLength)
                    {
                        int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                        readlength += tmplen;
                    }

                    // 保存上传的附件
                    string fileid = Guid.NewGuid().ToString("N");
                    code = DataFileService.SaveDataFile(fileid, strSaveName, postcontent, ext, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                    //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                    //IList<IDataParameter> sqlparams = new List<IDataParameter>();
                    //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILENAME", strSaveName);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILECONTENT", postcontent);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILEEXT", ext);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //sqlparams.Add(sqlparam);
                    if (code)
                    {
                        msg = "";
                        data.Add("id", fileid);
                        data.Add("name", strSaveName);
                    }
                    else
                    {
                        code = false;
                    }
                }
                else
                {
                    code = false;
                    msg = "上传文件不能为空";
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        /// <summary>
        /// 企业资质上报至建设局
        /// </summary>
        public void QyzzSbjsj()
        {
            string msg = "";
            bool code = true;
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string sqsbzh = "UR201906000001";
                    string sqsbxm = "浙江标点信息科技有限公司";
                    if (CurrentUser.CurUser != null)
                    {
                        sqsbzh = CurrentUser.UserName;
                        sqsbxm = CurrentUser.RealName;

                    }
                    string procstr = string.Format("QYZZSBJSJ('{0}','{1}','{2}')", id, sqsbzh, sqsbxm);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 保存行政主管部门审核意见--第三步
        /// </summary>
        public void SaveZZSBZGBMSHYJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string zh = "";
                string xm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        zh = CurrentUser.UserName;
                        xm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    zh = "UR201906000001";
                    xm = "浙江标点信息科技有限公司";
                }

                #endregion

                #region 获取id，保存数据
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if (data.ContainsKey("id") && (data["id"] != ""))
                {
                    id = data["id"];
                    sql = string.Format("select recid from jdbg_qyzzsb where id='{0}'", id);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }

                        List<string> lssql = new List<string>();
                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }

                            lssql.Add(sql);
                        }
                        if (lssql.Count > 0)
                        {
                            CommonService.ExecTrans(lssql);
                        }

                        string procstr = string.Format("SaveJDBGQYZZSBXZZGBMSHYJ('{0}','{1}','{2}')", id, zh, xm);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";



                    }
                    else
                    {
                        code = false;
                        msg = "找不到资质申请记录！";
                    }

                }
                else
                {
                    code = false;
                    msg = "id 不能为空！";
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }

        /// <summary>
        /// 企业资质申报，保存初审审批意见
        /// </summary>
        public void SaveZZSBSCYJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string jsjcszh = "";
                string jsjcsxm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        jsjcszh = CurrentUser.UserName;
                        jsjcsxm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    jsjcszh = "UR201906000001";
                    jsjcsxm = "浙江标点信息科技有限公司";
                }

                #endregion

                #region 获取id，保存数据
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if (data.ContainsKey("id") && (data["id"] != ""))
                {
                    id = data["id"];
                    sql = string.Format("select recid from jdbg_qyzzsb where id='{0}'", id);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }

                        List<string> lssql = new List<string>();
                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }

                            lssql.Add(sql);
                        }
                        if (lssql.Count > 0)
                        {
                            CommonService.ExecTrans(lssql);
                        }

                        string procstr = string.Format("SaveJDBGQYZZSBSCYJ('{0}','{1}','{2}')", id, jsjcszh, jsjcsxm);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";



                    }
                    else
                    {
                        code = false;
                        msg = "找不到资质申请记录！";
                    }

                }
                else
                {
                    code = false;
                    msg = "id 不能为空！";
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }
        /// <summary>
        /// 企业资质申报，保存复核意见
        /// </summary>
        public void SaveZZSBFHYJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string zh = "";
                string xm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        zh = CurrentUser.UserName;
                        xm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    zh = "UR201906000001";
                    xm = "浙江标点信息科技有限公司";
                }

                #endregion

                #region 获取id，保存数据
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if (data.ContainsKey("id") && (data["id"] != ""))
                {
                    id = data["id"];
                    sql = string.Format("select recid from jdbg_qyzzsb where id='{0}'", id);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }

                        List<string> lssql = new List<string>();
                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }

                            lssql.Add(sql);
                        }
                        if (lssql.Count > 0)
                        {
                            CommonService.ExecTrans(lssql);
                        }

                        string procstr = string.Format("SaveJDBGQYZZSBFHYJ('{0}','{1}','{2}')", id, zh, xm);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";



                    }
                    else
                    {
                        code = false;
                        msg = "找不到资质申请记录！";
                    }

                }
                else
                {
                    code = false;
                    msg = "id 不能为空！";
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }


        /// <summary>
        /// 企业资质申报，保存处室长审核意见
        /// </summary>
        public void SaveZZSBCSZSHYJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string zh = "";
                string xm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        zh = CurrentUser.UserName;
                        xm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    zh = "UR201906000001";
                    xm = "浙江标点信息科技有限公司";
                }

                #endregion

                #region 获取id，保存数据
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if (data.ContainsKey("id") && (data["id"] != ""))
                {
                    id = data["id"];
                    sql = string.Format("select recid from jdbg_qyzzsb where id='{0}'", id);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }

                        List<string> lssql = new List<string>();
                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }

                            lssql.Add(sql);
                        }
                        if (lssql.Count > 0)
                        {
                            CommonService.ExecTrans(lssql);
                        }

                        string procstr = string.Format("SaveJDBGQYZZSBCSZSHYJ('{0}','{1}','{2}')", id, zh, xm);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";



                    }
                    else
                    {
                        code = false;
                        msg = "找不到资质申请记录！";
                    }

                }
                else
                {
                    code = false;
                    msg = "id 不能为空！";
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }

        /// <summary>
        /// 企业资质申报，保存行政主管部门退回意见
        /// </summary>
        public void SaveZZSBTHYJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string zh = "";
                string xm = "";
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"];
                        zh = CurrentUser.UserName;
                        xm = CurrentUser.RealName;
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    zh = "UR201906000001";
                    xm = "浙江标点信息科技有限公司";
                }

                #endregion

                #region 获取id，保存数据
                string id = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if (data.ContainsKey("id") && (data["id"] != ""))
                {
                    id = data["id"];
                    sql = string.Format("select recid from jdbg_qyzzsb where id='{0}'", id);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string recid = dt[0]["recid"];
                        // 获取已经有的表单项
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        sql = string.Format("select distinct itemname from jdbg_qyzzsb_xq where parentid={0}", recid);
                        dtt = CommonService.GetDataTable(sql);
                        List<string> existeditems = new List<string>();
                        foreach (var item in dtt)
                        {
                            existeditems.Add(item["itemname"]);
                        }

                        List<string> lssql = new List<string>();
                        foreach (var item in data)
                        {
                            // 已经存在的update
                            if (existeditems.Contains(item.Key))
                            {
                                sql = string.Format("update jdbg_qyzzsb_xq set itemvalue='{0}' where parentid={1} and itemname='{2}'", item.Value, recid, item.Key);
                            }
                            else
                            {
                                sql = string.Format("insert into jdbg_qyzzsb_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                            }

                            lssql.Add(sql);
                        }
                        if (lssql.Count > 0)
                        {
                            CommonService.ExecTrans(lssql);
                        }

                        string procstr = string.Format("SaveJDBGQYZZSBTHYJ('{0}','{1}','{2}')", id, zh, xm);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";



                    }
                    else
                    {
                        code = false;
                        msg = "找不到资质申请记录！";
                    }

                }
                else
                {
                    code = false;
                    msg = "id 不能为空！";
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }
        /// <summary>
        /// 根据id获取企业资质申报时间线
        /// </summary>
        public void GetQYZZSBTimeLine()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string id = Request["id"].GetSafeString();
                string procstr = string.Format("GetQYZZSBTimeLine('{0}')", id);
                dt = CommonService.ExecDataTableProc(procstr, out msg);
                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        public void GetUrlByZzsblxbh()
        {
            string msg = "";
            bool code = true;
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string zzsblxbh = Request["zzsblxbh"].GetSafeString();
                string sql = string.Format("select url from h_zzsblx where sfyx=1 and lxbh='{0}'", zzsblxbh);
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    data.Add("url", dt[0]["url"]);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }


        public void GetQYZZSBRYSBGCYJ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string id = Request["id"].GetSafeString();
                string type = Request["type"].GetSafeString();
                if (id!="" && type!="")
                {
                    string procstr = string.Format("GetQYZZSBRYSBGCYJ('{0}','{1}')", id, type);
                    dt = CommonService.ExecDataTableProc(procstr, out msg);
                }
                else
                {
                    code = false;
                    msg = "id 或者 type 不能为空！";
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        public void downsqb()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string id = Request["id"].GetSafeString();
                string reporttype = Request["reporttype"].GetSafeString();
                string reportfile = Request["reportfile"].GetSafeString();
                string filename = Request["filename"].GetSafeString();
                if (id != "" && reporttype!="" && reportfile != "")
                {
                    code = DwgxSxjzyService.GetQyzzSqb(id, reporttype, reportfile,out msg, out file);
                    if (code && file != null)
                    {
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        //设置输出流
                        //Response.ContentType = "application/octet-stream";
                        Response.ContentType = "application/pdf";
                        //防止中文乱码
                        string fileName = HttpUtility.UrlEncode(filename + DateTime.Now.ToString("yyyyMMddHHmmss"));
                        //设置输出文件名
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
                        //输出
                        Response.Charset = "UTF-8";
                        Response.BinaryWrite(file);
                    }
                    else
                    {
                        SysLog4.WriteError(msg);
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

        [Authorize]
        /// <summary>
        /// 企业资质申请，窗口确认受理
        /// </summary>
        public void QyzzCKQRSL()
        {
            string msg = "";
            bool code = true;
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("QYZZSBCKQRSL('{0}','{1}','{2}')", id, CurrentUser.UserName, CurrentUser.RealName);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        [Authorize]
        /// <summary>
        /// 企业资质申请，窗口确认打证
        /// </summary>
        public void QyzzCKQRDZ()
        {
            string msg = "";
            bool code = true;
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("QYZZSBCKQRDZ('{0}','{1}','{2}')", id, CurrentUser.UserName, CurrentUser.RealName);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 人员社保查询接口
        /// <summary>
        /// 根据姓名，身份证号码，查询人员社保
        /// </summary>
        public void GetPersonSS()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                string name = Request["name"].GetSafeString();
                string idcard = Request["idcard"].GetSafeString();
                string areaKey = Request["areakey"].GetSafeString();
                string areaAK = Request["areaak"].GetSafeString();
                if (name !="" && idcard!="")
                {
                    if (areaAK == "")
                    {
                        ret = DwgxSxjzyService.GetAreaAK(areaKey, out msg, out areaAK);
                    }

                    
                    if (ret && areaAK != "")
                    {
                        string result = "";
                        if (PersonnelSocialSecurity.GetPersonnelSocialSecurity(name, idcard, areaAK, out msg, out result))
                        {
                            SysLog4.WriteError(result);
                            if (result != "")
                            {
                                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                                if (retdata != null)
                                {
                                    string code = retdata["code"].GetSafeString();
                                    if (code == "00")
                                    {
                                        string info = retdata["datas"].GetSafeString();
                                        if (info != "")
                                        {
                                            Dictionary<string, object> dtinfo = jss.Deserialize<Dictionary<string, object>>(info);
                                            if (dtinfo != null)
                                            {
                                                bool success = dtinfo["success"].GetSafeBool();
                                                if (success)
                                                {
                                                    data = dtinfo;
                                                }
                                                else
                                                {
                                                    ret = false;
                                                    msg = dtinfo["message"].GetSafeString();
                                                    SysLog4.WriteError(string.Format("姓名：{0}, 身份证：{1}, 错误信息：{2}", name, idcard, msg));
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        ret = false;
                                        msg = retdata["msg"].GetSafeString();
                                    }
                                }
                            }
                            else
                            {
                                ret = false;
                                msg = "人员社保查询接口返回为空！";
                            }
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                    else
                    {
                        ret = false;
                        msg = "找不到对应的地区";
                    }
                }
                else
                {
                    ret = false;
                    msg = "姓名和身份证号码不能为空！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        public void GetAreaList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                datas = DwgxSxjzyService.GetAreaList();
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.Write(jss.Serialize(new { code = ret ? "0" : "1", msg = msg, data = datas }));
            }
        }
        #endregion

        #region 专家会签页面
        /// <summary>
        /// 写入专家会签信息
        /// </summary>
        public void DoZjhq()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string idlist = Request["idlist"];
                string userlist = Request["userlist"];
                ret = DwgxSxjzyService.WriteZjhq(idlist, userlist, out msg);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(jss.Serialize(new { code = ret ? "0" : "1", msg = msg }));
                Response.End();
            }
        }

        public void Zjqrhq()
        {
            bool ret = true;
            string msg = "";
            try
            {
                int recid = Request["id"].GetSafeInt();
                string sfty = Request["sfty"].GetSafeBool() ? "1" : "0";
                string zjqryj = Request["zjqryj"].GetSafeString();
                if (recid > 0)
                {
                    ret = DwgxSxjzyService.Zjqrhq(recid, sfty, zjqryj,out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }
                

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(jss.Serialize(new { code = ret ? "0" : "1", msg = msg }));
                Response.End();
            }
        }
        #endregion

        #region 异常人员
        public void GetQyzzsbYCRY()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string id = Request["id"];
                string procstr = string.Format("GetQyzzsbYCRY('{0}')", id);
                dt = CommonService.ExecDataTableProc(procstr, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }


        }
        #endregion

        #region 导入人员信息
        [Authorize]
        public ActionResult ImportQyry()
        {
            string type = Request["type"].GetSafeString();
            ViewBag.type = type;
            return View();
        }
        [Authorize]
        public void doimportQyry()
        {
            bool ret = true;
            string msg = "";
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postfile = Request.Files[0];
                    string type = Request["type"].GetSafeString();
                    if (type == "")
                    {
                        ret = false;
                        msg = "参数错误!";
                    }
                    else
                    {
                        // 允许的扩展名
                        List<string> extensions = new List<string>() { ".xls", ".xlsx" };
                        string filename = postfile.FileName;
                        string ext = System.IO.Path.GetExtension(postfile.FileName).GetSafeString();
                        if (ext == "" || (!extensions.Contains(ext)))
                        {
                            ret = false;
                            msg = "上传的文件类型错误";
                        }
                        else
                        {
                            List<Dictionary<string, string>> dt = new List<Dictionary<string, string>>();
                            switch (ext)
                            {
                                case ".xls":
                                    dt = GetQYRYDataFromExcel(postfile.InputStream, type);
                                    break;
                                case ".xlsx":
                                    dt = GetQYRYDataFromExcel(postfile.InputStream, type,true);
                                    break;
                                default:
                                    break;
                            }
                            if (dt.Count > 0)
                            {
                                // 获取当前企业编号
                                string qybh = "";
                                string sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}' and sfqyzzh=1", CurrentUser.UserName);
                                IList<IDictionary<string, string>> qyinfo = CommonService.GetDataTable(sql);
                                if (qyinfo.Count > 0)
                                {
                                    qybh = qyinfo[0]["qybh"].GetSafeString();
                                }
                                if (qybh !="")
                                {
                                    List<string> lsql = GetRyInsertSql(dt, qybh, type);
                                    if (lsql.Count > 0)
                                    {
                                        CommonService.ExecTrans(lsql);
                                    }
                                }
                                else
                                {
                                    ret = false;
                                    msg = "获取企业信息失败";
                                }

                            }

                        }

                    }

                    
                }
                else
                {
                    ret = false;
                    msg = "上传文件不能为空！";
                }
                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }


        }

        private List<Dictionary<string, string>> GetQYRYDataFromExcel(Stream input, string type,  bool isExcel2007 = false)
        {
            List<Dictionary<string, string>> dt = new List<Dictionary<string, string>>();

            IWorkbook wb = null;
            if (isExcel2007)
            {
                wb = new XSSFWorkbook(input);
            }
            else
            {
                wb = new HSSFWorkbook(input);
            }

            ISheet sheet = wb.GetSheetAt(0);
            if (sheet != null)
            {
                //从第2行开始（第一行为标题）
                for (int j = 1; j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);
                    if (row != null)
                    {
                        Dictionary<string, string> info = new Dictionary<string, string>();
                        if (type == "JSFZR")
                        {
                            string ryxm = row.GetCell(0).GetSafeString().Trim();
                            if (ryxm != "")
                            {
                                info.Add("ryxm", ryxm);
                                info.Add("xb", row.GetCell(1).GetSafeString().Trim());
                                info.Add("birthyear", row.GetCell(2).GetSafeString().Trim());
                                info.Add("birthmonth", row.GetCell(3).GetSafeString().Trim());
                                info.Add("zc", row.GetCell(4).GetSafeString().Trim());
                                info.Add("zczy", row.GetCell(5).GetSafeString().Trim());
                                info.Add("zyzg", row.GetCell(6).GetSafeString().Trim());
                                info.Add("sfzhm", row.GetCell(7).GetSafeString().Trim());
                                info.Add("zczsbh", row.GetCell(8).GetSafeString().Trim());
                                info.Add("bysj", row.GetCell(9).GetSafeString().Trim());
                                info.Add("byxy", row.GetCell(10).GetSafeString().Trim());
                                info.Add("byzy", row.GetCell(11).GetSafeString().Trim());
                                info.Add("zgxl", row.GetCell(12).GetSafeString().Trim());
                                info.Add("gcglzl", row.GetCell(13).GetSafeString().Trim());
                                info.Add("fzzzlb", row.GetCell(14).GetSafeString().Trim());
                                dt.Add(info);                   
                            }
                        }
                        else if (type == "ZCJZS")
                        {
                            string ryxm = row.GetCell(0).GetSafeString().Trim();
                            if (ryxm != "")
                            {
                                info.Add("ryxm", ryxm);
                                info.Add("sfzhm", row.GetCell(1).GetSafeString().Trim());
                                info.Add("zy", row.GetCell(2).GetSafeString().Trim());
                                info.Add("jb", row.GetCell(3).GetSafeString().Trim());
                                info.Add("zczsbh", row.GetCell(4).GetSafeString().Trim());
                                info.Add("fzrq", row.GetCell(5).GetSafeString().Trim());
                                info.Add("zsyxq", row.GetCell(6).GetSafeString().Trim());
                                dt.Add(info);
                            }
                        }
                        else if (type == "ZJYSZCRY")
                        {
                            string ryxm = row.GetCell(0).GetSafeString().Trim();
                            if (ryxm != "")
                            {
                                info.Add("ryxm", ryxm);
                                info.Add("xl", row.GetCell(1).GetSafeString());
                                info.Add("zc", row.GetCell(2).GetSafeString());
                                info.Add("sfzhm", row.GetCell(3).GetSafeString());
                                info.Add("zczy", row.GetCell(4).GetSafeString());
                                info.Add("xlzy", row.GetCell(5).GetSafeString());
                                info.Add("sbzzlb", row.GetCell(6).GetSafeString());
                                dt.Add(info);
                            }
                        }
                        else if (type == "XCGLRY")
                        {
                            string ryxm = row.GetCell(0).GetSafeString().Trim();
                            if (ryxm != "")
                            {
                                info.Add("ryxm", ryxm);
                                info.Add("sfzhm", row.GetCell(1).GetSafeString().Trim());
                                info.Add("gwlb", row.GetCell(2).GetSafeString().Trim());
                                info.Add("gwzsbh", row.GetCell(3).GetSafeString().Trim());
                                info.Add("fzdw", row.GetCell(4).GetSafeString().Trim());
                                dt.Add(info);
                            }
                        }
                        else if (type == "JSGR")
                        {
                            string ryxm = row.GetCell(0).GetSafeString().Trim();
                            if (ryxm != "")
                            {
                                info.Add("ryxm", ryxm);
                                info.Add("sfzhm", row.GetCell(1).GetSafeString());
                                info.Add("jndj", row.GetCell(2).GetSafeString());
                                info.Add("zygz", row.GetCell(3).GetSafeString());
                                info.Add("zsbh", row.GetCell(4).GetSafeString());
                                info.Add("fzdw", row.GetCell(5).GetSafeString());
                                info.Add("sfzy", row.GetCell(6).GetSafeString());
                                info.Add("fzrq", row.GetCell(7).GetSafeString());
                                info.Add("zsyxq", row.GetCell(8).GetSafeString());
                                dt.Add(info);
                            }
                        }
                    }
                }
            }

            return dt;
        }

        private List<string> GetRyInsertSql(List<Dictionary<string, string>> datalist, string qybh, string type)
        {
            List<string> lsql = new List<string>();
            try
            {
                if (datalist.Count > 0)
                {
                    if (type == "JSFZR")
                    {
                        foreach (var row in datalist)
                        {
                            string sql = " insert into i_s_qy_jsfzr (qybh,ryxm,xb,birthyear,birthmonth,zc,zczy,zyzg,sfzhm,zczsbh,bysj,byxy,byzy,zgxl,gcglzl,fzzzlb,rybh) " +
                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}',newid())";
                            lsql.Add(string.Format(sql,
                                qybh, row["ryxm"], row["xb"], row["birthyear"], row["birthmonth"], row["zc"], row["zczy"], row["zyzg"], row["sfzhm"], row["zczsbh"], row["bysj"], row["byxy"], row["byzy"], row["zgxl"], row["gcglzl"], row["fzzzlb"]
                                ));
                        }
                    }
                    else if (type == "ZCJZS")
                    {
                        foreach (var row in datalist)
                        {
                            string sql = " insert into i_s_qy_zcjzs (qybh,ryxm,sfzhm,zy,jb,zczsbh,rybh,fzrq,zsyxq) " +
                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}',newid(),'{6}','{7}')";
                            lsql.Add(string.Format(sql,
                                qybh, row["ryxm"], row["sfzhm"], row["zy"], row["jb"], row["zczsbh"], row["fzrq"], row["zsyxq"]
                                ));
                        }
                    }
                    else if (type == "ZJYSZCRY")
                    {
                        foreach (var row in datalist)
                        {
                            string sql = " insert into i_s_qy_zjyszcry (qybh,ryxm,xl,zc,sfzhm,zczy,xlzy,sbzzlb,rybh) " +
                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',newid())";
                            lsql.Add(string.Format(sql,
                                qybh, row["ryxm"], row["xl"], row["zc"], row["sfzhm"], row["zczy"], row["xlzy"], row["sbzzlb"]
                                ));
                        }
                    }
                    else if (type == "XCGLRY")
                    {
                        foreach (var row in datalist)
                        {
                            string sql = " insert into i_s_qy_xcglry (qybh,ryxm,sfzhm,gwlb,gwzsbh,fzdw,rybh) " +
                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}',newid())";
                            lsql.Add(string.Format(sql,
                                qybh, row["ryxm"], row["sfzhm"], row["gwlb"], row["gwzsbh"], row["fzdw"]
                                ));
                        }
                    }
                    else if (type == "JSGR")
                    {
                        foreach (var row in datalist)
                        {
                            string sql = " insert into i_s_qy_jsgr (qybh,ryxm,sfzhm,jndj,zygz,zsbh,fzdw,sfzy,rybh,fzrq,zsyxq) " +
                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',newid(),'{8}','{9}')";
                            lsql.Add(string.Format(sql,
                                qybh, row["ryxm"], row["sfzhm"], row["jndj"], row["zygz"], row["zsbh"], row["fzdw"], row["sfzy"], row["fzrq"], row["zsyxq"]
                                ));
                        }
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return lsql;
        }
        #endregion

        #region 自定义登录
        /// <summary>
        /// 登录操作
        /// </summary>
        public void DoLogin()
        {
            string err = "";
            bool ret = false;
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();
                password = Base64Func.DecodeBase64(password);
                string oldcode = Session["USERLOGIN_SecurityCode"] as string;
                string yzm = Request["yzm"].GetSafeString();


                Session["USERLOGIN_SecurityCode"] = null;

                if (oldcode.GetSafeString().ToUpper() != yzm.GetSafeString().ToUpper())
                {
                    ret = false;
                    err = "验证码错误！";
                }
                else
                {
                    string realname = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qymc from I_M_QY where ZH='" + username + "'");
                    if (dt.Count > 0)
                    {
                        realname = dt[0]["qymc"];
                    }
                    ret = Remote.UserService.Login(username, realname, password, out err);
                    // 登录成功
                    if (ret)
                    {
                        // 设置日志系统用户
                        BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                        // 设置流程模块用户
                        BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                            new WorkFlow.Common.SessionUser()
                            {
                                CompanyId = CurrentUser.CurUser.CompanyId,
                                CompanyName = CurrentUser.CurUser.CompanyName,
                                DepartmentId = CurrentUser.CurUser.DepartmentId,
                                DepartmentName = CurrentUser.CurUser.DepartmentName,
                                DutyLevel = CurrentUser.CurUser.DutyLevel,
                                RealName = CurrentUser.CurUser.RealName,
                                UserName = CurrentUser.CurUser.UserName
                            }, null);
                        Session["UserPowerList"] = null;
                        // 设置录入界面用户
                        Session["USERCODE"] = CurrentUser.UserCode;
                        Session["USERNAME"] = CurrentUser.UserName;
                        Session["REALNAME"] = CurrentUser.RealName;
                        Session["CPCODE"] = CurrentUser.CompanyCode;
                        Session["CPNAME"] = CurrentUser.CompanyName;
                        Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                        Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                        Session["MANAGEDEP"] = CurrentUser.CurUser.ManageDep;
                        Session["SJHM"] = SystemService.GetUserMobile(CurrentUser.UserCode);
                        // 企业及个人用户企业编号
                        Session["USERQYBH"] = JcService.GetQybh(CurrentUser.UserCode);
                        //Session["MenuCode"] = "QYGL_QYBA";
                        //设置当前登录劳资账号所在工程的jdzch
                        //SetJDZCH(username);
                        CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                        // 设置用户桌面项
                        bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                        if (!status)
                            SysLog4.WriteLog(err);

                        // 获取页面跳转类型
                        dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                        if (dt.Count > 0)
                            CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                        else
                            CurrentUser.CurUser.UrlJumpType = "SYS";

                    }

                    // 记录登陆日志
                    BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                    {
                        ClientType = LogConst.ClientType,
                        Ip = ClientInfo.Ip,
                        LogTime = DateTime.Now,
                        ModuleName = LogConst.ModuleUser,
                        Operation = LogConst.UserOpLogin,
                        UserName = CurrentUser.UserName,
                        RealName = ret ? CurrentUser.RealName : "",
                        Remark = "",
                        Result = ret
                    };
                    LogService.SaveLog(log);
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";
                if (err == "")
                {
                    if (ret)
                        err = JsonFormat.GetRetString(ret, CurrentUser.RealName);
                    else
                        err = JsonFormat.GetRetString(ret);
                }
                else
                    err = JsonFormat.GetRetString(false, err);

                Response.Write(err);
                Response.End();
            }
        }

        public ActionResult SecurityCode()
        {
            string oldcode = Session["USERLOGIN_SecurityCode"] as string;
            string code = CreateRandomCode(4); //验证码的字符为4个
            Session["USERLOGIN_SecurityCode"] = code; //验证码存放在TempData中
            return File(CreateValidateGraphic(code), "image/Jpeg");
        }

        /// <summary>
        /// 生成随机的字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,g,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }


        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 16.0), 27);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);

                //画图片的前景干扰线
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion

        #region  全国建筑市场监管公共服务平台
        /// <summary>
        /// 获取企业在公共服务平台上的详细信息URL
        /// </summary>
        public void GetQyQueryCompDetailUrl()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                string zzjgdm = Request["zzjgdm"].GetSafeString();
                string url = "";
                string ptid = "";
                if (DwgxSxjzyService.GetQyQGCXPTID(qybh, out msg, out ptid))
                {
                    if (ptid !="")
                    {
                        url = QGJZSCJGGGFWPT.GetCompDetailUrl(ptid);
                    }
                    else
                    {
                        ret = QGJZSCJGGGFWPT.GetQueryCompDetailUrl(zzjgdm, out msg, out url);
                    }
                }
                

                data.Add("url", url);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        /// <summary>
        /// 获取企业某个资质证书在公共服务平台上的详情url
        /// </summary>
        public void GetQyQueryCompCaCertDetailUrl()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                string zzjgdm = Request["zzjgdm"].GetSafeString();
                string certno = Request["zzzsbh"].GetSafeString();
                string url = "";
                string ptid = "";
                // 先根据企业编号获取已经保存的ptid
                // 如果没有，再去查询ptid
                // 获取到ptid之后，生成证书的url
                if (DwgxSxjzyService.GetQyQGCXPTID(qybh, out msg, out ptid))
                {
                    if (ptid == "")
                    {
                        ret = QGJZSCJGGGFWPT.GetQyid(zzjgdm, out msg, out ptid);
                    }
                    if (ret)
                    {
                        url = QGJZSCJGGGFWPT.GetCompCaCertDetail(ptid,certno);
                    }
                    
                }


                data.Add("url", url);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }
        #endregion

        #region 企业资质相关
        public void DelQYZZ()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id !="")
                {
                    ret = DwgxSxjzyService.DelQYZZ(id, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();

            }
        }

        public ActionResult ViewZZRYYCXQ()
        {
            string id = Request["id"].GetSafeString();
            ViewBag.id = id;
            return View();
        }

        /// <summary>
        /// 获取企业资质异常详情
        /// </summary>
        public void GetYcQyzzInfo()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    data = DwgxSxjzyService.GetYcQyzzInfo(id);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();

            }
        }
        #endregion

        #region 获取企业资质申请表和变更表
        public void GetQyzzReportFile()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string id = Request["id"].GetSafeString();
                string reporttype = Request["reporttype"].GetSafeString();
                if (id != "" && reporttype!="")
                {
                    code = DwgxSxjzyService.GetQyzzReportFile(id,reporttype, out file, out msg);
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

        public ActionResult ViewQyzzsqFile()
        {
            string id = Request["id"].GetSafeString();
            string reporttype = Request["reporttype"].GetSafeString();
            ViewBag.id = id;
            ViewBag.reporttype = reporttype;
            return View();
        }
        #endregion

        #region 浙江省诚信平台接口获取数据

        public void TestSCXPT()
        {
            bool ret = true;
            string msg = "";
            object data = null;
            try
            {
                string CorpName = Request["CorpName"].GetSafeString();
                string SCUCode = Request["SCUCode"].GetSafeString();
                ret = ZJJGPublicData.GetUserRecordInfo(CorpName, SCUCode, out msg, out data);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();

            }
        }

        private bool checkSCXPTSuccess(object code)
        {
            return code.GetSafeInt() == 1;
        }

        /// <summary>
        /// 获取省诚信平台分页的企业信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetSCXPTBasicList(Dictionary<string, object> data)
        {

            Dictionary<string, object> ret = new Dictionary<string, object>();
            if (data != null)
            {
                if (data.ContainsKey("Data"))
                {
                    Dictionary<string, object> realdata = (Dictionary<string, object>)data["Data"];
                    if (realdata!=null)
                    {
                        if (realdata.ContainsKey("PageCount"))
                        {
                            ArrayList al = (ArrayList)realdata["PageCount"];
                            if (al!=null && al.Count > 0)
                            {
                                Dictionary<string, object> info1 = (Dictionary<string, object>)al[0];
                                if (info1.ContainsKey("RecordCount"))
                                {
                                    ret.Add("total", info1["RecordCount"].GetSafeInt());
                                }
                            }
                        }

                        if (realdata.ContainsKey("TbCorpBasicList"))
                        {
                            ArrayList al = (ArrayList)realdata["TbCorpBasicList"];
                            ret.Add("rows", al);
                        }
                    }
                }
            }

            return ret;
        }

        private Dictionary<string, object> extraParamGetBasicList(List<KeyValuePair<string, string>> paramlist)
        {
            bool code = true;
            string msg = "";
            string data = ZJJGPublicData.GetFixedParams();
            return new Dictionary<string, object>() {
                {"code", code },
                {"msg", msg },
                { "data", data }
            };

        }

        private List<KeyValuePair<string, string>> ForgeSCXPTBASICLISTFilterParams(List<KeyValuePair<string, string>> paramlist)
        {
            var q = paramlist.Where(x => x.Key.Equals("CorpName", StringComparison.OrdinalIgnoreCase));
            if (q.Count() == 0)
            {
                paramlist.Add(new KeyValuePair<string, string>("CorpName", ""));
            }

            q = paramlist.Where(x => x.Key.Equals("CorpCode", StringComparison.OrdinalIgnoreCase));
            if (q.Count() == 0)
            {
                paramlist.Add(new KeyValuePair<string, string>("CorpCode", ""));
            }

            return paramlist;
        }

        [Authorize]
        public JsonResult DownSCXPTAllInfo()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string qybh = "";
                string qymc = "";
                string zzjgdm = "";
                string sql = string.Format("select top 1 qybh, qymc, zzjgdm from i_m_qy where qybh in (select qybh from i_m_qyzh where yhzh='{0}' and sfqyzzh=1) ", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    qybh = dt[0]["qybh"].GetSafeString();
                    qymc = dt[0]["qymc"].GetSafeString();
                    zzjgdm = dt[0]["zzjgdm"].GetSafeString();
                }

                if (qybh !="" && qymc !="")
                {
                    IList<string> lsql = new List<string>();
                    string upsql = "";

                    #region 下载企业基本信息
                    object basicinfo = null;
                    // 获取企业基本信息
                    if (ZJJGPublicData.GetCorpBasicInfo(qymc, zzjgdm, out msg, out basicinfo))
                    {
                        if (basicinfo != null)
                        {
                            Dictionary<string, object> qydata = (Dictionary<string, object>)basicinfo;
                            if (qydata != null)
                            {
                                if (qydata.ContainsKey("tbCorpBasicInfo"))
                                {
                                    var qyinfolist = qydata["tbCorpBasicInfo"] as ArrayList;
                                    if (qyinfolist != null && qyinfolist.Count > 0)
                                    {
                                        Dictionary<string, object> qyinfo = (Dictionary<string, object>)qyinfolist[0];

                                        if (qyinfo != null && qyinfo.Count > 0)
                                        {
                                            #region 更新企业基本信息

                                            upsql = "update i_m_qy set " +
                                                    " XSJLSJ='{0}' ," +
                                                    " YYZZBH='{1}' ," +
                                                    " ZCZJ='{2}' ," +
                                                    " JJXZ='{3}' ," +
                                                    " ZCD1='{4}' ," +
                                                    " ZCD2='{5}' ," +
                                                    " ZCD3='{6}' ," +
                                                    " XSYB='{7}' ," +
                                                    " LXDH='{8}' ," +
                                                    " DWCZ='{9}' ," +
                                                    " LXYX='{10}' ," +
                                                    " ZCDYB='{11}' ," +
                                                    " BGDZ='{12}' ," +
                                                    " DWWZ='{13}' ," +
                                                    " QYFR='{14}' ," +
                                                    " QYFRSFZHM='{15}' ," +
                                                    " XSFRZW='{16}' ," +
                                                    " XSFRZC='{17}' ," +
                                                    " ZCD4='{18}', " +
                                                    " XSZCD1='{21}', " +
                                                    " XSZCD2='{22}', " +
                                                    " XSZCD3='{23}', " +
                                                    " SCXPTLastUpdateTime=getdate() " +
                                                    " where qybh='{19}' and qymc='{20}' ";
                                            string zcd3 = qyinfo["CountyName"].GetSafeString();
                                            if (zcd3 == "市辖区")
                                            {
                                                zcd3 = "越城区";
                                            }
                                            upsql = string.Format(upsql,
                                                    qyinfo["CorpBirthDate"].GetSafeString(),
                                                    qyinfo["LicenseNum"].GetSafeString(),
                                                    qyinfo["RegPrin"].GetSafeString(),
                                                    qyinfo["EconTypeName"].GetSafeString(),
                                                    qyinfo["ProvinceName"].GetSafeString(),
                                                    qyinfo["CityName"].GetSafeString(),
                                                    zcd3,
                                                    qyinfo["BusPostalCode"].GetSafeString(),
                                                    qyinfo["OfficePhone"].GetSafeString(),
                                                    qyinfo["Fax"].GetSafeString(),
                                                    qyinfo["EMail"].GetSafeString(),
                                                    qyinfo["PostalCode"].GetSafeString(),
                                                    qyinfo["BusAddress"].GetSafeString(),
                                                    qyinfo["Url"].GetSafeString(),
                                                    qyinfo["LegalManName"].GetSafeString(),
                                                    qyinfo["LegalManIDCard"].GetSafeString(),
                                                    qyinfo["LegalManDutyName"].GetSafeString(),
                                                    qyinfo["LegalManTitleName"].GetSafeString(),
                                                    qyinfo["Address"].GetSafeString(),
                                                    qybh, qymc,
                                                    qyinfo["ProvinceName"].GetSafeString(),
                                                    qyinfo["CityName"].GetSafeString(),
                                                    zcd3
                                                    );

                                            lsql.Add(upsql);

                                            //SysLog4.WriteError(upsql);

                                            #endregion

                                            #region 更新证书信息
                                            var zzzsinfolist = qydata["TBCorpCertInfo"] as ArrayList;
                                            var zzfwinfolist = qydata["TBCorpCertDetailInfo"] as ArrayList;
                                            if (zzzsinfolist != null && zzzsinfolist.Count > 0)
                                            {
                                                #region 提取所有资质范围
                                                List<Dictionary<string, object>> zzfwlist = new List<Dictionary<string, object>>();
                                                foreach (var zzfw in zzfwinfolist)
                                                {
                                                    Dictionary<string, object> zzfwinfo = (Dictionary<string, object>)zzfw;
                                                    if (zzfwinfo != null && zzfwinfo.Count > 0)
                                                    {
                                                        zzfwlist.Add(new Dictionary<string, object>() {
                                                                        {"ZZZSBH", zzfwinfo["CertID"].GetSafeString()},
                                                                        {"ZZFW", zzfwinfo["Mark"].GetSafeString()},
                                                                        {"JSFZR", ""}
                                                                    });
                                                    }
                                                }

                                                //SysLog4.WriteError("获取到的资质范围总数：" + zzfwlist.Count.ToString());
                                                #endregion


                                                #region 提取所有资质证书
                                                List<Dictionary<string, object>> zslist = new List<Dictionary<string, object>>();
                                                List<Dictionary<string, object>> aqsczslist = new List<Dictionary<string, object>>();

                                                foreach (var zzzs in zzzsinfolist)
                                                {
                                                    Dictionary<string, object> zzzsinfo = (Dictionary<string, object>)zzzs;
                                                    if (zzzsinfo != null && zzzsinfo.Count > 0)
                                                    {
                                                        if (zzzsinfo["CertTypeName"].GetSafeString().StartsWith("建筑业"))
                                                        {
                                                            // 获取当前资质拥有的资质范围
                                                            List<Dictionary<string, object>> ownzzfw = new List<Dictionary<string, object>>();
                                                            foreach (var fw in zzfwlist)
                                                            {
                                                                SysLog4.WriteError("zzzsbh: " + fw["ZZZSBH"].GetSafeString());
                                                                SysLog4.WriteError("CertID:" + zzzsinfo["CertID"].GetSafeString());
                                                                if (fw["ZZZSBH"].GetSafeString() == zzzsinfo["CertID"].GetSafeString())
                                                                {
                                                                    ownzzfw.Add(fw);
                                                                }
                                                            }
                                                            zslist.Add(new Dictionary<string, object>() {
                                                                            {"ZZZSBH", zzzsinfo["CertID"].GetSafeString() },
                                                                            {"FZRQ", zzzsinfo["OrganDate"].GetSafeString() },
                                                                            {"ZSYXQ", zzzsinfo["EndDate"].GetSafeString() },
                                                                            {"FZJG", zzzsinfo["OrganName"].GetSafeString() },
                                                                            { "ZZFWLIST", ownzzfw}
                                                                        });

                                                        }
                                                        else if (zzzsinfo["CertTypeName"].GetSafeString() == "安全生产许可")
                                                        {
                                                            aqsczslist.Add(new Dictionary<string, object>() {
                                                                            {"ZZZSBH", zzzsinfo["CertID"].GetSafeString() },
                                                                            {"FZRQ", zzzsinfo["OrganDate"].GetSafeString() },
                                                                            {"ZSYXQ", zzzsinfo["EndDate"].GetSafeString() },
                                                                            {"FZJG", zzzsinfo["OrganName"].GetSafeString() }
                                                                        });
                                                        }
                                                    }

                                                }
                                                #endregion

                                                #region 生成SQL
                                                if (zslist.Count > 0)
                                                {
                                                    foreach (var zs in zslist)
                                                    {
                                                        string zzzsbh = zs["ZZZSBH"].GetSafeString();
                                                        string fzrq = zs["FZRQ"].GetSafeString();
                                                        string zsyxq = zs["ZSYXQ"].GetSafeString();
                                                        string fzjg = zs["FZJG"].GetSafeString();
                                                        List<Dictionary<string, object>> fwlist = zs["ZZFWLIST"] as List<Dictionary<string, object>>;
                                                        // 根据资质证书编号删除资质范围
                                                        upsql = "delete from jdbg_qyzz_zzfw " +
                                                                " where zzid in (select id from jdbg_qyzz where qybh='{0}' and zzzsbh='{1}')";
                                                        upsql = string.Format(upsql, qybh, zzzsbh);
                                                        lsql.Add(upsql);

                                                        // 根据资质证书编号删除资质证书
                                                        upsql = string.Format(" delete from jdbg_qyzz where qybh='{0}' and zzzsbh='{1}'", qybh, zzzsbh);
                                                        lsql.Add(upsql);

                                                        // 插入资质证书信息
                                                        string id = Guid.NewGuid().ToString("N");

                                                        upsql = "insert into jdbg_qyzz (id,qybh,zzzsbh,fzrq,zsyxq,fzjg) " +
                                                                " values ('{0}','{1}','{2}','{3}','{4}','{5}')";
                                                        upsql = string.Format(upsql, id, qybh, zzzsbh, fzrq, zsyxq, fzjg);
                                                        lsql.Add(upsql);

                                                        SysLog4.WriteError("id： " + id + "资质范围总数：" + fwlist.Count);
                                                        if (fwlist != null && fwlist.Count > 0)
                                                        {
                                                            foreach (var fw in fwlist)
                                                            {
                                                                string zzfwmc = fw["ZZFW"].GetSafeString();
                                                                string jsfzr = fw["JSFZR"].GetSafeString();
                                                                // 资质范围等级名称替换，保持与保准一致
                                                                zzfwmc = zzfwmc.Replace("壹级", "一级").Replace("贰级", "二级").Replace("叁级", "三级");
                                                                if (zzfwmc != "")
                                                                {
                                                                    upsql = "insert into jdbg_qyzz_zzfw (zzid, zzfw,jsfzr) " +
                                                                            "values ('{0}','{1}','{2}')";
                                                                    upsql = string.Format(upsql, id, zzfwmc, jsfzr);
                                                                    lsql.Add(upsql);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (aqsczslist.Count > 0)
                                                {
                                                    string aqsczhbh = aqsczslist[0]["ZZZSBH"].GetSafeString();
                                                    string aqsczhyxq = aqsczslist[0]["ZSYXQ"].GetSafeString();
                                                    upsql = "update I_M_QY set AQSCXKZBH='{0}', AQSCXKZBHDQSJ='{1}' " +
                                                                            " where qybh='{2}'";
                                                    upsql = string.Format(upsql, aqsczhbh, aqsczhyxq, qybh);
                                                    lsql.Add(upsql);

                                                }
                                                #endregion

                                            }
                                            #endregion

                                            #region 更新企业附件
                                            string QyToRowguid = qyinfo["ToRowGuid"].GetSafeString();
                                            if (QyToRowguid != "")
                                            {
                                                object corpfiledata = null;
                                                if (ZJJGPublicData.GetCorpFileList(QyToRowguid, out msg, out corpfiledata))
                                                {
                                                    if (corpfiledata != null)
                                                    {
                                                        ArrayList corpfilelist = corpfiledata as ArrayList;
                                                        if (corpfilelist != null && corpfilelist.Count > 0)
                                                        {
                                                            List<string> qyfjlsql = new List<string>();
                                                            foreach (var corpfile in corpfilelist)
                                                            {
                                                                Dictionary<string, object> file = (Dictionary<string, object>)corpfile;
                                                                if (file != null && file.Count > 0)
                                                                {
                                                                    string FileGuid = file["FileGuid"].GetSafeString().Replace("'", "");
                                                                    string FileName = file["FileName"].GetSafeString().Replace("'", "");
                                                                    string FileGroupName = file["FileGroupName"].GetSafeString().Replace("'", "");
                                                                    string FileBase64String = file["FileBase64String"].GetSafeString();
                                                                    byte[] filecontent = FileBase64String.DecodeBase64Array();
                                                                    if (FileName == "")
                                                                    {
                                                                        FileName = Guid.NewGuid().ToString("N") + ".jpg";
                                                                    }
                                                                    if (FileName != "" && filecontent != null && filecontent.Length > 0)
                                                                    {
                                                                        string fileid = "";
                                                                        string ext = "";
                                                                        if (FileName.IndexOf(".") > 0)
                                                                        {
                                                                            ext = FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.'));
                                                                        }
                                                                        if (SaveFile(FileName, ext, filecontent, out fileid, out msg))
                                                                        {
                                                                            upsql = "insert into scxpt_i_s_qy_fj (fileguid,filename,filegroupname,fileid,qybh,qymc) " +
                                                                                    " values ('{0}','{1}','{2}','{3}','{4}','{5}')";
                                                                            upsql = string.Format(upsql, FileGuid, FileName, FileGroupName, fileid, qybh, qymc);
                                                                            qyfjlsql.Add(upsql);
                                                                        }
                                                                        else
                                                                        {
                                                                            SysLog4.WriteError(msg);
                                                                            SysLog4.WriteError("保存企业附件失败，附件名称：" + FileName);
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                            if (qyfjlsql.Count > 0)
                                                            {
                                                                // 删除已有的企业附件
                                                                upsql = string.Format("delete from scxpt_i_s_qy_fj where qybh='{0}'", qybh);
                                                                qyfjlsql.Insert(0, upsql);
                                                                // 删除已经保存的企业附件
                                                                upsql = string.Format("delete from datafile where fileid in (select fileid from scxpt_i_s_qy_fj where qybh='{0}')", qybh);
                                                                qyfjlsql.Insert(0, upsql);
                                                                // 更新营业执照正本
                                                                upsql = "update i_m_qy set yyzzfj=stuff((select '|'+fileid+','+filename from scxpt_i_s_qy_fj where qybh='{0}' and filegroupname='{1}' for xml path('') ),1,1,'') where qybh='{2}'";
                                                                upsql = string.Format(upsql, qybh, "企业法人营业执照正本", qybh);
                                                                qyfjlsql.Add(upsql);
                                                                // 更新组织机构代码证
                                                                upsql = "update i_m_qy set zzjgzs=stuff((select '|'+fileid+','+filename from scxpt_i_s_qy_fj where qybh='{0}' and filegroupname='{1}' for xml path('') ),1,1,'') where qybh='{2}'";
                                                                upsql = string.Format(upsql, qybh, "组织机构代码证", qybh);
                                                                qyfjlsql.Add(upsql);

                                                                foreach (var s in qyfjlsql)
                                                                {
                                                                    lsql.Add(s);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SysLog4.WriteError("下载企业附件出错\r\n企业名称：" + qymc);
                                                }
                                            }
                                            #endregion

                                            #region 执行更新
                                            if (lsql.Count > 0)
                                            {
                                                CommonService.ExecTrans(lsql);
                                            }
                                            #endregion


                                        }
                                    }


                                }

                            }

                        }

                    }
                    else
                    {
                        ret = false;
                        msg = "下载企业信息出错：企业名称[" + qymc + "]";
                    }
                    #endregion

                    #region 下载企业人员信息
                    lsql.Clear();
                    object personinfo = null;
                    if (ZJJGPublicData.GetCorpPersonInfo(qymc, zzjgdm, out msg, out personinfo))
                    {
                        if (personinfo != null)
                        {
                            ArrayList persondata = personinfo as ArrayList;
                            if (persondata != null && persondata.Count > 0)
                            {
                                List<Dictionary<string, object>> jsfzrlist = new List<Dictionary<string, object>>();
                                List<Dictionary<string, object>> zcjzslist = new List<Dictionary<string, object>>();
                                List<Dictionary<string, object>> zjyszcrylist = new List<Dictionary<string, object>>();
                                List<Dictionary<string, object>> jsgrlist = new List<Dictionary<string, object>>();
                                List<Dictionary<string, object>> xcglrylist = new List<Dictionary<string, object>>();
                                // 获取所有的人员列表
                                List<Dictionary<string, object>> allpersonlist = new List<Dictionary<string, object>>();
                                foreach (var personrow in persondata)
                                {
                                    allpersonlist.Add((Dictionary<string, object>)personrow);
                                }
                                // 下载所有人员详情
                                foreach (var item in allpersonlist)
                                {
                                    string rowguid = item["RowGuid"].GetSafeString();
                                    string userguid = item["UserGuid"].GetSafeString();
                                    object userrecord = null;
                                    if (ZJJGPublicData.GetUserRecordInfo(rowguid, userguid, out msg, out userrecord))
                                    {
                                        Dictionary<string, object> persondetail = (Dictionary<string, object>)userrecord;
                                        item.Add("PersonDetail", persondetail);
                                        if (persondetail != null && persondetail.Count > 0)
                                        {
                                            ArrayList list = persondetail["EssentialInfo"] as ArrayList;
                                            item.Add("Detail_EssentialInfo", list);
                                            if (list != null && list.Count > 0)
                                            {
                                                Dictionary<string, object> ess = (Dictionary<string, object>)list[0];
                                                if (ess != null && ess.Count > 0)
                                                {
                                                    item["ESS_NationName"] = ess["NationName"].GetSafeString();
                                                    item["ESS_EduLevelName"] = ess["EduLevelName"].GetSafeString();
                                                    item["ESS_DegreeName"] = ess["DegreeName"].GetSafeString();
                                                    item["ESS_IDCardTypeName"] = ess["IDCardTypeName"].GetSafeString();
                                                    item["ESS_IDCard"] = ess["IDCard"].GetSafeString();
                                                    item["ESS_PersonName"] = ess["PersonName"].GetSafeString();
                                                    item["ESS_SexName"] = ess["SexName"].GetSafeString();
                                                }
                                            }

                                            list = persondetail["PractisingInfo"] as ArrayList;
                                            item.Add("Detail_PractisingInfo", list);

                                            list = persondetail["PerformanceInfo"] as ArrayList;
                                            item.Add("Detail_PerformanceInfo", list);

                                            list = persondetail["PostInfo"] as ArrayList;
                                            item.Add("Detail_PostInfo", list);
                                            list = persondetail["TBPersonTechTitleInfo"] as ArrayList;
                                            item.Add("Detail_TBPersonTechTitleInfo", list);
                                        }

                                        // 生成注册人员、职称人员、现场人员、技术工人数据
                                        if (item["iszhuce"].GetSafeBool())
                                        {
                                            zcjzslist.Add(item);
                                        }

                                        if (item["iszhicheng"].GetSafeBool())
                                        {
                                            zjyszcrylist.Add(item);
                                        }

                                        if (item["isxianchang"].GetSafeBool())
                                        {
                                            xcglrylist.Add(item);
                                        }

                                        if (item["isjishugongren"].GetSafeBool())
                                        {
                                            jsgrlist.Add(item);
                                        }

                                        #region 处理人员附件
                                        string rysfzhm = item["ESS_IDCard"].GetSafeString();
                                        string ryxm = item["ESS_PersonName"].GetSafeString();
                                        if (rysfzhm != "" && rowguid != "" && userguid != "")
                                        {
                                            object personfiledata = null;
                                            if (ZJJGPublicData.GetPersonFileList(rowguid, userguid, rysfzhm, out msg, out personfiledata))
                                            {
                                                ArrayList personfilelist = personfiledata as ArrayList;
                                                if (personfilelist != null && personfilelist.Count > 0)
                                                {
                                                    List<string> ryfjlsql = new List<string>();
                                                    foreach (var personfile in personfilelist)
                                                    {
                                                        Dictionary<string, object> file = (Dictionary<string, object>)personfile;
                                                        if (file != null && file.Count > 0)
                                                        {
                                                            string FileGuid = file["FileGuid"].GetSafeString().Replace("'", "");
                                                            string FileName = file["FileName"].GetSafeString().Replace("'","");
                                                            string FileGroupName = file["FileGroupName"].GetSafeString().Replace("'", "");
                                                            string FileBase64String = file["FileBase64String"].GetSafeString();
                                                            byte[] filecontent = FileBase64String.DecodeBase64Array();
                                                            if (FileName == "")
                                                            {
                                                                FileName = Guid.NewGuid().ToString("N") + ".jpg";
                                                            }
                                                            if (FileName != "" && filecontent != null && filecontent.Length > 0)
                                                            {
                                                                string fileid = "";
                                                                string ext = "";
                                                                if (FileName.IndexOf(".") > 0)
                                                                {
                                                                    ext = FileName.Substring(FileName.LastIndexOf('.'), FileName.Length - FileName.LastIndexOf('.'));
                                                                }
                                                                if (SaveFile(FileName, ext, filecontent, out fileid, out msg))
                                                                {
                                                                    upsql = "insert into scxpt_i_s_qy_ry_fj (fileguid,filename,filegroupname,fileid,qybh,sfzhm,ryxm) " +
                                                                            " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                                                                    upsql = string.Format(upsql, FileGuid, FileName, FileGroupName, fileid, qybh, rysfzhm, ryxm);
                                                                    ryfjlsql.Add(upsql);
                                                                }
                                                                else
                                                                {
                                                                    SysLog4.WriteError(msg);
                                                                    SysLog4.WriteError("保存企业附件失败，附件名称：" + FileName);
                                                                }

                                                            }

                                                        }
                                                    }
                                                    if (ryfjlsql.Count > 0)
                                                    {
                                                        // 删除已有的企业附件
                                                        upsql = string.Format("delete from scxpt_i_s_qy_ry_fj where qybh='{0}' and sfzhm='{1}'", qybh, rysfzhm);
                                                        ryfjlsql.Insert(0, upsql);
                                                        upsql = string.Format("delete from datafile where fileid in (select fileid from scxpt_i_s_qy_ry_fj where qybh='{0}' and sfzhm='{1}')", qybh, rysfzhm);
                                                        ryfjlsql.Insert(0, upsql);
                                                        CommonService.ExecTrans(ryfjlsql);



                                                    }
                                                }
                                            }
                                            else
                                            {
                                                SysLog4.WriteError("下载人员附件出错\r\n人员姓名：" + ryxm + "\r\n身份证号码：" + rysfzhm);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        SysLog4.WriteError("获取人员详细信息失败：人员姓名[" + item["personname"].GetSafeString() + "]\r\n错误：" + msg);
                                    }
                                }

                                #region 处理注册建造师
                                List<string> allzcjzssfzlist = new List<string>();
                                foreach (var item in zcjzslist)
                                {
                                    // 从人员的执业资格信息中获取注册证书
                                    var zyzglist = item["Detail_PractisingInfo"] as ArrayList;

                                    // 获取注册建造师证书列表
                                    List<Dictionary<string, object>> zcjzszslist = new List<Dictionary<string, object>>();
                                    foreach (var zyzg in zyzglist)
                                    {
                                        Dictionary<string, object> zyzginfo = (Dictionary<string, object>)zyzg;
                                        List<string> jzszsmclist = new List<string>() {
                                                        "注册建造师（一级）",
                                                        "注册建造师（二级）"
                                                    };
                                        if (zyzginfo != null && zyzginfo.Count > 0)
                                        {
                                            if (jzszsmclist.Contains(zyzginfo["SpecialtyTypeName"].GetSafeString()))
                                            {
                                                zcjzszslist.Add(zyzginfo);
                                            }
                                        }
                                    }

                                    if (zcjzszslist.Count > 0)
                                    {
                                        // 存储本次下载的证书信息
                                        List<Dictionary<string, string>> deletingzcjzs = new List<Dictionary<string, string>>();
                                        string rysfzhm = "";
                                        foreach (var zsjzszs in zcjzszslist)
                                        {
                                            //string ryxm = zsjzszs["PersonName"].GetSafeString();
                                            string ryxm = item["ESS_PersonName"].GetSafeString();
                                            string sfzhm = zsjzszs["IDCard"].GetSafeString();
                                            string zy = zsjzszs["RegTradeTypeName"].GetSafeString();
                                            string jb = zsjzszs["SpecialtyTypeName"].GetSafeString();
                                            if (jb == "注册建造师（一级）")
                                            {
                                                jb = "一级注册建造师";
                                            }
                                            else if (jb == "注册建造师（二级）")
                                            {
                                                jb = "二级注册建造师";
                                            }

                                            string zczsbh = zsjzszs["CertNum"].GetSafeString();

                                            string fzrq = zsjzszs["AwardDate"].GetSafeString();
                                            if (fzrq != "")
                                            {
                                                DateTime tmpdt = new DateTime();
                                                if (DateTime.TryParse(fzrq, out tmpdt))
                                                {
                                                    fzrq = tmpdt.ToString("yyyy-MM-dd");
                                                }
                                            }
                                            string zsyxq = zsjzszs["EffectDate"].GetSafeString();
                                            if (zsyxq != "")
                                            {
                                                DateTime tmpdt = new DateTime();
                                                if (DateTime.TryParse(zsyxq, out tmpdt))
                                                {
                                                    zsyxq = tmpdt.ToString("yyyy-MM-dd");
                                                }
                                            }

                                            deletingzcjzs.Add(new Dictionary<string, string>() {
                                                {"zczsbh",zczsbh },
                                                { "zy", zy}
                                            });
                                            rysfzhm = sfzhm;
                                            allzcjzssfzlist.Add(sfzhm);
                                            string procstr = string.Format("DownSCXPTZCJZS('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                    qybh, ryxm, sfzhm, zy, jb, zczsbh, fzrq, zsyxq
                                                );
                                            SysLog4.WriteError(procstr);
                                            CommonService.ExecProc(procstr, out msg);

                                        }

                                        //除了本次下载的证书信息，其他的都删了
                                        if (deletingzcjzs.Count > 0 && rysfzhm != "")
                                        {
                                            string notstr = "";
                                            foreach (var dzc in deletingzcjzs)
                                            {
                                                if (notstr != "")
                                                {
                                                    notstr += " or ";
                                                }
                                                notstr += string.Format("(zy='{0}' and zczsbh='{1}')", dzc["zy"], dzc["zczsbh"]);
                                            }
                                            sql = string.Format("delete from i_s_qy_zcjzs where qybh='{0}' and sfzhm='{1}' and  not ({2})",
                                                    qybh, rysfzhm, notstr
                                                );
                                            CommonService.Execsql(sql);
                                        }

                                    }

                                }
                                // 除了本次下载的注册建造师，其他的都删了
                                if (allzcjzssfzlist.Count > 0)
                                {
                                    sql = string.Format(" delete from i_s_qy_zcjzs where qybh='{0}' and sfzhm not in({1})",
                                            qybh, string.Join(",", allzcjzssfzlist.Distinct().ToList()).FormatSQLInStr()
                                        );
                                    CommonService.Execsql(sql);
                                }

                                #endregion

                                #region 处理中级以上职称人员
                                List<string> allzjyszcrysfzlist = new List<string>();
                                foreach (var item in zjyszcrylist)
                                {
                                    var zclist = item["Detail_TBPersonTechTitleInfo"] as ArrayList;
                                    if (zclist != null && zclist.Count > 0)
                                    {
                                        string rysfzhm = "";
                                        List<Dictionary<string, string>> deletingzc = new List<Dictionary<string, string>>();
                                        foreach (var zcdata in zclist)
                                        {
                                            Dictionary<string, object> zcinfo = (Dictionary<string, object>)zcdata;
                                            if (zcinfo != null && zcinfo.Count > 0)
                                            {
                                                string ryxm = item["ESS_PersonName"].GetSafeString();
                                                string xl = item["ESS_EduLevelName"].GetSafeString();
                                                string zc = zcinfo["TechTitleName"].GetSafeString();
                                                
                                                string sfzhm = item["ESS_IDCard"].GetSafeString();
                                                string zczy = zcinfo["MajorName"].GetSafeString();
                                                string xlzy = "";
                                                deletingzc.Add(new Dictionary<string, string>() {
                                                                { "zc", zc},
                                                                { "zczy", zczy}
                                                            });
                                                rysfzhm = sfzhm;
                                                allzjyszcrysfzlist.Add(sfzhm);
                                                string procstr = string.Format("DownSCXPTZJYSZCRY('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                                        qybh, ryxm, xl, zc, sfzhm, zczy, xlzy
                                                    );
                                                CommonService.ExecProc(procstr, out msg);
                                            }
                                        }

                                        // 除了本次下载的职称，其他的全部删掉
                                        if (deletingzc.Count > 0 && rysfzhm != "")
                                        {
                                            string notstr = "";
                                            foreach (var dzc in deletingzc)
                                            {
                                                if (notstr != "")
                                                {
                                                    notstr += " or ";
                                                }
                                                notstr += string.Format("(zc='{0}' and zczy='{1}')", dzc["zc"], dzc["zczy"]);
                                            }
                                            sql = string.Format("delete from i_s_qy_zjyszcry where qybh='{0}' and sfzhm='{1}' and not ({2})",
                                                    qybh, rysfzhm, notstr
                                                );
                                            //SysLog4.WriteError(sql);
                                            CommonService.Execsql(sql);
                                        }
                                    }
                                }
                                // 除了本次下载的职称人员，其他的都删了
                                if (allzjyszcrysfzlist.Count > 0)
                                {
                                    sql = string.Format(" delete from i_s_qy_zjyszcry where qybh='{0}' and sfzhm not in({1})",
                                            qybh, string.Join(",", allzjyszcrysfzlist.Distinct().ToList()).FormatSQLInStr()
                                        );
                                    CommonService.Execsql(sql);
                                }
                                #endregion

                                #region 处理技术工人
                                // 保存本次下载的所有技术工人的身份证
                                // 本次下载之后，删除系统中所有身份证不在里面的人
                                List<string> alljsgrsfzlist = new List<string>();
                                foreach (var item in jsgrlist)
                                {
                                    var gwlist = item["Detail_PostInfo"] as ArrayList;
                                    if (gwlist != null && gwlist.Count > 0)
                                    {
                                        string rysfzhm = "";
                                        List<string> zsbhlist = new List<string>();
                                        foreach (var gwdata in gwlist)
                                        {
                                            Dictionary<string, object> gw = (Dictionary<string, object>)gwdata;
                                            if (gw != null && gw.Count > 0)
                                            {
                                                string gwname = gw["PostClassName"].GetSafeString();
                                                // 只处理技术工人
                                                List<string> allgwlist = new List<string>() {
                                                                "现场作业人员",
                                                                "特种作业人员"
                                                            };
                                                if (allgwlist.Contains(gwname))
                                                {
                                                    string ryxm = item["ESS_PersonName"].GetSafeString();
                                                    string sfzhm = item["ESS_IDCard"].GetSafeString();
                                                    string jndj = gw["CertLevelName"].GetSafeString();
                                                    string zygz = gw["PostTypeName"].GetSafeString();
                                                    string zsbh = gw["CertNum"].GetSafeString();
                                                    string fzdw = gw["OrganName"].GetSafeString();
                                                    string sfzy = "是";
                                                    string fzrq = gw["OrganDate"].GetSafeString();
                                                    string zsyxq = gw["EndDate"].GetSafeString();

                                                    rysfzhm = sfzhm;
                                                    zsbhlist.Add(zsbh);
                                                    alljsgrsfzlist.Add(sfzhm);
                                                    string procstr = string.Format("DownSCXPTJSGR('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                                                        qybh, ryxm, sfzhm, jndj, zygz, zsbh, fzdw, sfzy, fzrq, zsyxq
                                                                    );
                                                    CommonService.ExecProc(procstr, out msg);
                                                }
                                            }

                                        }

                                        // 除了本次下载的技术工人岗位，其他的都删了
                                        if (zsbhlist.Count > 0 && rysfzhm != "")
                                        {
                                            sql = string.Format("delete from i_s_qy_jsgr where qybh='{0}' and sfzhm='{1}' and zsbh not in({2})",
                                                    qybh, rysfzhm, string.Join(",", zsbhlist).FormatSQLInStr()
                                                );
                                            CommonService.Execsql(sql);
                                        }


                                    }

                                }
                                // 除了本次下载的技术工人，其他的都删了
                                if (alljsgrsfzlist.Count > 0)
                                {
                                    sql = string.Format(" delete from i_s_qy_jsgr where qybh='{0}' and sfzhm not in({1})",
                                            qybh, string.Join(",", alljsgrsfzlist.Distinct().ToList()).FormatSQLInStr()
                                        );
                                    CommonService.Execsql(sql);
                                }
                                #endregion

                                #region 更新最后处理时间
                                sql = string.Format("update i_m_qy set SCXPTRYLastUpdateTime=getdate() where qybh='{0}'", qybh);
                                CommonService.Execsql(sql);
                                #endregion
                            }
                        }
                        else
                        {
                            ret = false;
                            msg = "下载企业相关人员信息出错：企业名称[" + qymc + "]\r\n错误：返回内容为空";
                            SysLog4.WriteError("下载企业相关人员信息出错：企业名称[" + qymc + "]\r\n错误：返回内容为空");
                        }
                    }
                    else
                    {
                        ret = false;
                        msg = "下载企业相关人员信息出错：企业名称[" + qymc + "]\r\n错误：" + msg;
                        SysLog4.WriteError("下载企业相关人员信息出错：企业名称[" + qymc + "]\r\n错误：" + msg);
                    }
                    #endregion

                    #region 下载企业工程信息
                    lsql.Clear();
                    object prjlist = null;
                    if (ZJJGPublicData.GetCorpProjectList(qymc, out msg, out prjlist))
                    {
                        if (prjlist != null)
                        {
                            ArrayList prjl = prjlist as ArrayList;
                            List<Dictionary<string, object>> projectlist = new List<Dictionary<string, object>>();
                            if (prjl != null && prjl.Count > 0)
                            {
                                foreach (var item in prjl)
                                {
                                    projectlist.Add((Dictionary<string, object>)item);
                                }
                                // 遍历每个工程项目
                                if (projectlist.Count > 0)
                                {
                                    foreach (var item in projectlist)
                                    {
                                        string PrjGuid = item["PrjGuid"].GetSafeString();
                                        string prjname = item["PRJNAME"].GetSafeString();
                                        //SysLog4.WriteError("PrjGuid: " + PrjGuid);
                                        if (PrjGuid != "")
                                        {
                                            object projectinfo = null;
                                            if (ZJJGPublicData.GetProjectInfo(PrjGuid, out msg, out projectinfo))
                                            {
                                                if (projectinfo != null)
                                                {
                                                    Dictionary<string, object> projectdata = (Dictionary<string, object>)projectinfo;
                                                    if (projectdata != null && projectdata.Count > 0)
                                                    {
                                                        #region 提取工程项目详情
                                                        List<Dictionary<string, object>> TbProjectInfo = new List<Dictionary<string, object>>();
                                                        List<Dictionary<string, object>> TbTenderInfo = new List<Dictionary<string, object>>();
                                                        List<Dictionary<string, object>> TbContractRecordManage = new List<Dictionary<string, object>>();
                                                        List<Dictionary<string, object>> TbBuilderLicenceManage = new List<Dictionary<string, object>>();
                                                        List<Dictionary<string, object>> TbSubjectInfo = new List<Dictionary<string, object>>();
                                                        List<Dictionary<string, object>> TbProjectFinishManage = new List<Dictionary<string, object>>();

                                                        ArrayList list = projectdata["TbProjectInfo"] as ArrayList;
                                                        foreach (var itm in list)
                                                        {
                                                            TbProjectInfo.Add((Dictionary<string, object>)itm);
                                                        }
                                                        list = projectdata["TbTenderInfo"] as ArrayList;
                                                        foreach (var itm in list)
                                                        {
                                                            TbTenderInfo.Add((Dictionary<string, object>)itm);
                                                        }
                                                        list = projectdata["TbContractRecordManage"] as ArrayList;
                                                        foreach (var itm in list)
                                                        {
                                                            TbContractRecordManage.Add((Dictionary<string, object>)itm);
                                                        }
                                                        list = projectdata["TbBuilderLicenceManage"] as ArrayList;
                                                        foreach (var itm in list)
                                                        {
                                                            TbBuilderLicenceManage.Add((Dictionary<string, object>)itm);
                                                        }
                                                        list = projectdata["TbSubjectInfo"] as ArrayList;
                                                        foreach (var itm in list)
                                                        {
                                                            TbSubjectInfo.Add((Dictionary<string, object>)itm);
                                                        }
                                                        list = projectdata["TbProjectFinishManage"] as ArrayList;
                                                        foreach (var itm in list)
                                                        {
                                                            TbProjectFinishManage.Add((Dictionary<string, object>)itm);
                                                        }
                                                        #endregion

                                                        #region 生成sql语句
                                                        string gcmc = "";
                                                        string xmbh = "";
                                                        string gcszdsf = "";
                                                        string gcszdcs = "";
                                                        string gcszdxq = "";
                                                        string htbh = "";
                                                        string sgxkzbh = "";
                                                        string xmjl = "";
                                                        string gclb = "";
                                                        string jszb = "";
                                                        string htj = "";
                                                        string jsj = "";
                                                        string sgcbfs = "";
                                                        string sgzzfs = "";
                                                        string kgsj = "";
                                                        string jgsj = "";
                                                        string jsdw = "";
                                                        string jsdwlxr = "";
                                                        string jsdwlxdh = "";
                                                        string ysdw = "";
                                                        string ysdwlxr = "";
                                                        string ysdwlxdh = "";

                                                        // 工程基本信息
                                                        if (TbProjectInfo.Count > 0)
                                                        {
                                                            gcmc = TbProjectInfo[0]["PrjName"].GetSafeString();
                                                            xmbh = TbProjectInfo[0]["PrjNum"].GetSafeString();
                                                            gcszdsf = TbProjectInfo[0]["ProvinceName"].GetSafeString();
                                                            gcszdcs = TbProjectInfo[0]["CityName"].GetSafeString();
                                                            gcszdxq = TbProjectInfo[0]["CountyName"].GetSafeString();
                                                            gclb = TbProjectInfo[0]["PrjTypeName"].GetSafeString();
                                                            jsdw = TbProjectInfo[0]["BuildCorpName"].GetSafeString();
                                                        }
                                                        // 招投标信息
                                                        if (TbTenderInfo.Count > 0)
                                                        {
                                                            xmjl = TbTenderInfo[0]["ConsCorpLeader"].GetSafeString();
                                                        }
                                                        // 合同备案信息
                                                        if (TbContractRecordManage.Count > 0)
                                                        {
                                                            htbh = TbContractRecordManage[0]["RecordNum"].GetSafeString();
                                                            htj = TbContractRecordManage[0]["ContractMoney"].GetSafeString();
                                                            sgcbfs = TbContractRecordManage[0]["ContractTypeName"].GetSafeString();
                                                            if (sgcbfs == "施工总包")
                                                            {
                                                                sgcbfs = "施工总承包";
                                                            }

                                                        }
                                                        // 施工许可证信息
                                                        if (TbBuilderLicenceManage.Count > 0)
                                                        {
                                                            sgxkzbh = TbBuilderLicenceManage[0]["BuilderLicenceNum"].GetSafeString();
                                                            xmjl = TbBuilderLicenceManage[0]["ConsCorpLeader"].GetSafeString(); ;
                                                            gclb = TbBuilderLicenceManage[0]["PrjTypeName"].GetSafeString();
                                                        }
                                                        // 竣工验收备案信息
                                                        if (TbProjectFinishManage.Count > 0)
                                                        {
                                                            kgsj = TbProjectFinishManage[0]["BDate"].GetSafeString();
                                                            jgsj = TbProjectFinishManage[0]["EDate"].GetSafeString();
                                                            jsj = TbProjectFinishManage[0]["FactCost"].GetSafeString();
                                                        }

                                                        if (gcmc != "")
                                                        {
                                                            string procstr = string.Format("DownSCXPTGC('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')",
                                                                    qybh, PrjGuid, gcmc, xmbh, gcszdsf, gcszdcs, gcszdxq, htbh, sgxkzbh, xmjl, gclb, jszb, htj, jsj, sgcbfs, sgzzfs, kgsj, jgsj, jsdw, jsdwlxr, jsdwlxdh, ysdw, ysdwlxr, ysdwlxdh
                                                                );
                                                            //SysLog4.WriteError(procstr);
                                                            CommonService.ExecProc(procstr, out msg);
                                                        }
                                                        #endregion



                                                    }
                                                }
                                            }
                                            else
                                            {
                                                SysLog4.WriteError("下载企业工程信息出错：\r\n企业名称：" + qymc + "]\r\n工程名称：" + prjname + "\r\n项目GUID：" + PrjGuid);
                                            }
                                        }
                                    }

                                }
                                


                            }
                            #region 更新最后处理时间
                            sql = string.Format("update i_m_qy set SCXPTGCLastUpdateTime=getdate() where qybh='{0}'", qybh);
                            CommonService.Execsql(sql);
                            #endregion
                        }
                        else
                        {
                            ret = false;
                            msg = "下载企业相关工程信息出错：企业名称[" + qymc + "]\r\n错误：返回内容为空";
                            SysLog4.WriteError("下载企业相关工程信息出错：企业名称[" + qymc + "]\r\n错误：返回内容为空");
                        }
                    }
                    else
                    {
                        ret = false;
                        msg = "下载企业相关工程信息出错：企业名称[" + qymc + "]\r\n错误：" + msg;
                        SysLog4.WriteError("下载企业相关工程信息出错：企业名称[" + qymc + "]\r\n错误：" + msg);
                    }
                    #endregion
                }
                else
                {
                    ret = false;
                    msg = "无法找到企业信息！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(msg);
            }

            return Json(new { code = ret ? "0" : "1", msg = msg });
        }

        private bool SaveFile(string FileName, string ext, byte[] content, out string id, out string msg)
        {
            bool ret = true;
            msg = "";
            id = "";
            try
            {
                string fileid = Guid.NewGuid().ToString("N");
                ret = DataFileService.SaveDataFile(id, FileName, content, ext, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                //IList<IDataParameter> sqlparams = new List<IDataParameter>();
                //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                //sqlparams.Add(sqlparam);
                //sqlparam = new SqlParameter("@FILENAME", FileName);
                //sqlparams.Add(sqlparam);
                //sqlparam = new SqlParameter("@FILECONTENT", content);
                //sqlparams.Add(sqlparam);
                //sqlparam = new SqlParameter("@FILEEXT", ext);
                //sqlparams.Add(sqlparam);
                //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //sqlparams.Add(sqlparam);
                if (ret)
                {
                    msg = "";
                    id = fileid;
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }
        #endregion

        #region 企业资质核查情况记录


        /// <summary>
        /// 保存企业资质核查情况记录
        /// </summary>
        public void SaveQyzzHZQK()
        {
            string msg = "";
            bool code = true;
            try
            {
                #region 获取企业信息
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                string sql = "";
                string qybh = "";
                string qymc = "";
                
                if (CurrentUser.CurUser != null)
                {
                    sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qybh = dt[0]["qybh"].GetSafeString();
                        
                    }
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000097"; // 测试用，正式场合得废掉；
                    
                }

                if (qybh != "")
                {
                    sql = string.Format("select qymc from I_M_QY where qybh='{0}'", qybh);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        qymc = dt[0]["qymc"].GetSafeString();
                    }
                }
                #endregion

                #region 生成ID
                string id = "";
                string fj = "";
                Dictionary<string, string> data = new Dictionary<string, string>();
                System.Collections.Specialized.NameValueCollection paramlist = Request.Form;
                foreach (var k in Request.Form.AllKeys)
                {
                    data[k] = Request.Form[k];
                }
                if ((!data.ContainsKey("id")) || (data["id"] == ""))
                {
                    string proc = "GetGuid()";
                    dt = CommonService.ExecDataTableProc(proc, out msg);
                    if (dt.Count > 0)
                    {
                        id = dt[0]["id"];
                    }
                    data["id"] = id;
                }
                else
                {
                    id = data["id"];
                }

                if (data.ContainsKey("fj"))
                {
                    fj = data["fj"].GetSafeString();
                }
                #endregion

                #region 保存记录
                
                // 保存资质申报主记录
                string procstr = string.Format("SaveJDBGQYZZHCQK('{0}','{1}','{2}','{3}')",
                    id, qybh, qymc, fj
                    );
                dt = CommonService.ExecDataTableProc(procstr, out msg);
                // 保存所有表单项
                if (dt.Count > 0)
                {
                    string recid = dt[0]["recid"];
                    List<string> lssql = new List<string>();
                    sql = string.Format(" delete from jdbg_qyzz_hcqkjl_xq where parentid={0}", recid);
                    lssql.Add(sql);
                    foreach (var item in data)
                    {
                        sql = string.Format("insert into jdbg_qyzz_hcqkjl_xq (parentid, itemname, itemvalue) values ({0}, '{1}', '{2}')", recid, item.Key, item.Value);
                        lssql.Add(sql);
                    }
                    CommonService.ExecTrans(lssql);

                }

                if (code)
                {
                    msg = "";
                }

                #endregion
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        /// <summary>
        /// 获取企业资质核查情况详情
        /// </summary>
        public void GetQyzzHcqkXQ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string sql = string.Format("select itemname, itemvalue from jdbg_qyzz_hcqkjl_xq where parentid in (select recid from jdbg_qyzz_hcqkjl where id='{0}')", id);
                    dt = CommonService.GetDataTable(sql);
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        /// <summary>
        ///上传核查情况附件
        /// </summary>
        public void DoUploadHcxkZL()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postfile = Request.Files[0];
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
                    // 获取文件二进制数据
                    byte[] postcontent = new byte[postfile.ContentLength];
                    int readlength = 0;
                    while (readlength < postfile.ContentLength)
                    {
                        int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                        readlength += tmplen;
                    }

                    // 保存上传的附件
                    string fileid = Guid.NewGuid().ToString("N");
                    code = DataFileService.SaveDataFile(fileid, strSaveName, postcontent, ext, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                    //string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                    //IList<IDataParameter> sqlparams = new List<IDataParameter>();
                    //IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILENAME", strSaveName);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILECONTENT", postcontent);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@FILEEXT", ext);
                    //sqlparams.Add(sqlparam);
                    //sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //sqlparams.Add(sqlparam);
                    if (code)
                    {
                        msg = "";
                        data.Add("id", fileid);
                        data.Add("name", strSaveName);
                    }
                    else
                    {
                        code = false;
                    }
                }
                else
                {
                    code = false;
                    msg = "上传文件不能为空";
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        /// <summary>
        /// 删除资质核查情况
        /// </summary>
        public void DelZZHCQK()
        {
            string msg = "";
            bool code = true;

            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("FlowDeleteJDBGQYZZHCQK('{0}')", id);
                    IList<IDictionary<string, string>> dt = dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["msg"];
                    }
                }
                else
                {
                    code = false;
                    msg = "id不能为空！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        public void GetHCQKFile()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string id = Request["id"].GetSafeString();
                string reporttype = Request["reporttype"].GetSafeString();
                if (id != "" && reporttype != "")
                {
                    code = DwgxSxjzyService.GetHCQKFile(id, reporttype, out file, out msg);
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

        #region 查看Pdf
        public ActionResult ViewPdfFile()
        {
            string url = Request["url"].GetSafeString();
            ViewBag.url = url;
            return View();
        }
        #endregion

        #region 查看企业和人员的附件
        public ActionResult viewqyfj()
        {
            string qybh = Request["qybh"].GetSafeString();
            ViewBag.qybh = qybh;
            return View();
        }

        public ActionResult viewryfj()
        {
            string qybh = Request["qybh"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeString();
            ViewBag.qybh = qybh;
            ViewBag.sfzhm = sfzhm;
            return View();
        }

        public ActionResult viewzzsbryfj()
        {
            string qybh = "";
            string id = Request["id"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeString();
            if (id !="")
            {
                string sql = string.Format("select qybh from jdbg_qyzzsb where id='{0}'", id);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    qybh = dt[0]["qybh"].GetSafeString();
                }
            }
            ViewBag.qybh = qybh;
            ViewBag.sfzhm = sfzhm;
            return View("viewryfj");
        }

        public void GetQYFJList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                if (qybh !="")
                {
                    string procstr = string.Format("GetQYFJList('{0}')", qybh);
                    data = CommonService.ExecDataTableProc(procstr, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }
                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }


        public void GetRYFJList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                string sfzhm = Request["sfzhm"].GetSafeString();
                if (qybh != "" && sfzhm!="")
                {
                    string procstr = string.Format("GetRYFJList('{0}','{1}')", qybh, sfzhm);
                    data = CommonService.ExecDataTableProc(procstr, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }



        #endregion

        #region 从平台获取企业、人员附件（用于上传申报资料）
        public void GetPTQYFJList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = "";
                string sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    qybh = dt[0]["qybh"].GetSafeString();
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000102";
                }
                if (qybh != "")
                {
                    string procstr = string.Format("GetQYFJList('{0}')", qybh);
                    data = CommonService.ExecDataTableProc(procstr, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }

        public void GetPTRYFJList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string qybh = "";
                string sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    qybh = dt[0]["qybh"].GetSafeString();
                }
                if (qybh == "")
                {
                    qybh = "ZJQ000102";
                }

                if (qybh != "")
                {
                    string type = Request["type"].GetSafeString();
                    if (type == "")
                    {
                        ret = false;
                        msg = "人员类型不能为空！";
                    }
                    else
                    {
                        string tb = "";
                        if (type=="ZCJZS")
                        {
                            tb = "I_S_QY_ZCJSZ";
                        }
                        else if (type == "ZJYSZCRY")
                        {
                            tb = "I_S_QY_ZJYSZCRY";
                        }
                        else if (type == "JSGR")
                        {
                            tb = "I_S_QY_JSGR";
                        }
                        else if (type == "JSFZR")
                        {
                            tb = "I_S_QY_JSFZR";
                        }
                        else if (type == "XCGLRY")
                        {
                            tb = "I_S_QY_XCGLRY";
                        }

                        if (tb !="")
                        {
                            sql = string.Format("select distinct sfzhm, ryxm from {0} where qybh='{1}' order by ryxm ", tb, qybh);
                            IList<IDictionary<string, string>> dtt = CommonService.GetDataTable(sql);
                            if (dtt.Count > 0)
                            {
                               
                                foreach (var item in dtt)
                                {
                                    string sfzhm = item["sfzhm"].GetSafeString();
                                    string ryxm = item["ryxm"].GetSafeString();
                                    string procstr = string.Format("GetRYFJList('{0}','{1}')", qybh, sfzhm);
                                    var ryfj = CommonService.ExecDataTableProc(procstr, out msg);
                                    data.Add(new Dictionary<string, object>() {
                                        {"sfzhm", sfzhm },
                                        {"ryxm", ryxm },
                                        {"ryfj", ryfj }
                                    });

                                }
                            }
                        }
                        else
                        {
                            ret = false;
                            msg = "人员类型错误！";
                        }



                    }
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }
        #endregion

        #region 查看资质申报请示件

        public ActionResult viewzzsbqsj()
        {
            string id = Request["id"].GetSafeString();
            ViewBag.id = id;
            return View();
        }

        public void GetZZSBQSJ()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string,object> data = new Dictionary<string, object>();
            try
            {
                string fj = "";
                string id = Request["id"].GetSafeString();
                if (id !="")
                {
                    string sql = "select top 1 itemvalue from jdbg_qyzzsb_xq where itemname = 'qxsqsjfj' " +
                        " and parentid in (select recid from jdbg_qyzzsb where id='{0}' )";
                    sql = string.Format(sql, id);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        fj = dt[0]["itemvalue"].GetSafeString();
                    }

                    data.Add("fj", fj);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }

        #endregion

        #region 上报主管部门之前，需要校验人员社保是否异常
        public void CheckQyzzsbRYSB()
        {
            string msg = "";
            bool code = true;
            
            try
            {

                string id = Request["id"];
                string procstr = string.Format("GetQyzzsbYCRY('{0}')", id);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt != null && dt.Count > 0)
                {
                    code = false;
                    List<string> rylist = dt.Select(x => x["ryxm"].GetSafeString()).Distinct().ToList();
                    msg = string.Format("以下人员社保异常：{0}，请核对之后再上报！", string.Join("、", rylist)); 
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 获取当前操作人的姓名
        [Authorize]
        public void GetCurrentUserInfo()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string realname = CurrentUser.RealName;
                data["realname"] = realname;

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }
        #endregion

        #region 查看企业相关人员（技术负责人、注册建造师等）
        public ActionResult ViewQYRY()
        {
            string qybh = Request["qybh"].GetSafeString();
            ViewBag.qybh = qybh;
            return View();
        }
        #endregion

        #region 确认资质核查情况查看状态
        public JsonResult DoSetHCQKRead()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id !="")
                {
                    string sql = string.Format("update JDBG_QYZZ_HCQKJL set isread=1 where id='{0}'", id);
                    ret = CommonService.ExecSql(sql, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            return Json(new { code = ret ? "0" : "1", msg = msg });
        }
        #endregion

        #region 审批窗口退回
        [Authorize]
        public JsonResult CKTH()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                string ckyj = Request["ckyj"].GetSafeString();
                string procstr = string.Format("SaveQYZZSBCKYJ('{0}','{1}','{2}','{3}')", id, ckyj, CurrentUser.UserName, CurrentUser.RealName);
                CommonService.ExecProc(procstr, out msg);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            return Json(new { code = ret ? "0" : "1", msg = msg });
        }
        #endregion

        #region 浙里办APP接口
        // 获取已申报的资质列表
        public void PhoneGetQyzzsbList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string zzsblxbh = Request["zzsblxbh"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc !="")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    if (zzsblxbh !="")
                    {
                        strwhere += string.Format(" and zzsblxbh='{0}' ", zzsblxbh);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')",CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and sqzt>=3 and exists (select * from h_xzzgbm_spr  b where sprzh='{0}' and b.zgbmbh=a.zgbmbh) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and sqzt>=4 ");
                    }
                    string sql = "select * from view_jdbg_qyzzsb a where " + strwhere + "  order by recid desc ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0":"1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        // 获取企业技术负责人
        public void PhoneGetQyJSFZRList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string ryxm = Request["ryxm"].GetSafeString();
                    string sfzhm = Request["sfzhm"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc != "")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    if (ryxm != "")
                    {
                        strwhere += string.Format(" and ryxm like '%{0}%' ", ryxm);
                    }
                    if (sfzhm !="")
                    {
                        strwhere += string.Format(" and sfzhm like '%{0}%' ", ryxm);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and exists (select * from i_m_qy b where qybh=a.qybh and exists(select * from h_xzzgbm_spr where sprzh='{0}'and szsf=b.zcd1 and szcs=b.zcd2 and szxq=b.zcd3)) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and 1=1 ");
                    }
                    string sql = "select * from VIEW_I_S_QY_JSFZR a where " + strwhere + "  order by ryxm  ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        // 获取企业注册建造师
        public void PhoneGetQyZCJZSList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string ryxm = Request["ryxm"].GetSafeString();
                    string sfzhm = Request["sfzhm"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc != "")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    if (ryxm != "")
                    {
                        strwhere += string.Format(" and ryxm like '%{0}%' ", ryxm);
                    }
                    if (sfzhm != "")
                    {
                        strwhere += string.Format(" and sfzhm like '%{0}%' ", ryxm);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and exists (select * from i_m_qy b where qybh=a.qybh and exists(select * from h_xzzgbm_spr where sprzh='{0}'and szsf=b.zcd1 and szcs=b.zcd2 and szxq=b.zcd3)) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and 1=1 ");
                    }
                    string sql = "select * from VIEW_I_S_QY_ZCJZS a where " + strwhere + "  order by ryxm  ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        // 获取企业中级以上职称人员
        public void PhoneGetQyZJYSZCRYList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string ryxm = Request["ryxm"].GetSafeString();
                    string sfzhm = Request["sfzhm"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc != "")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    if (ryxm != "")
                    {
                        strwhere += string.Format(" and ryxm like '%{0}%' ", ryxm);
                    }
                    if (sfzhm != "")
                    {
                        strwhere += string.Format(" and sfzhm like '%{0}%' ", ryxm);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and exists (select * from i_m_qy b where qybh=a.qybh and exists(select * from h_xzzgbm_spr where sprzh='{0}'and szsf=b.zcd1 and szcs=b.zcd2 and szxq=b.zcd3)) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and 1=1 ");
                    }
                    string sql = "select * from VIEW_I_S_QY_ZJYSZCRY a where " + strwhere + "  order by ryxm  ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        // 获取企业技术工人
        public void PhoneGetQyJSGRList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string ryxm = Request["ryxm"].GetSafeString();
                    string sfzhm = Request["sfzhm"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc != "")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    if (ryxm != "")
                    {
                        strwhere += string.Format(" and ryxm like '%{0}%' ", ryxm);
                    }
                    if (sfzhm != "")
                    {
                        strwhere += string.Format(" and sfzhm like '%{0}%' ", ryxm);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and exists (select * from i_m_qy b where qybh=a.qybh and exists(select * from h_xzzgbm_spr where sprzh='{0}'and szsf=b.zcd1 and szcs=b.zcd2 and szxq=b.zcd3)) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and 1=1 ");
                    }
                    string sql = "select * from VIEW_I_S_QY_JSGR a where " + strwhere + "  order by ryxm  ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        // 获取企业基本信息
        public void PhoneGetQyJBXXList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc != "")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and exists (select * from i_m_qy b where qybh=a.qybh and exists(select * from h_xzzgbm_spr where sprzh='{0}'and szsf=b.zcd1 and szcs=b.zcd2 and szxq=b.zcd3)) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and 1=1 ");
                    }
                    string sql = "select * from view_i_m_qy a where " + strwhere + "  order by qymc  ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }

        // 获取企业资质
        public void PhoneGetQyzzList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            password = Base64Func.DecodeBase64(password);
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    // 获取账号类型
                    string zhlx = "";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                    if (dt.Count > 0)
                    {
                        zhlx = dt[0]["zhlx"];
                    }
                    string qymc = Request["qymc"].GetSafeString();
                    string strwhere = " 1=1 ";

                    if (qymc != "")
                    {
                        strwhere += string.Format(" and qymc like '%{0}%' ", qymc);
                    }
                    // 企业端
                    if (zhlx == "Q")
                    {
                        strwhere += string.Format(" and qybh in (select qybh from i_m_qyzh where yhzh='{0}')", CurrentUser.UserName);
                    }
                    // 各区县市
                    else if (zhlx == "N")
                    {
                        strwhere += string.Format(" and exists (select * from i_m_qy b where qybh=a.qybh and exists(select * from h_xzzgbm_spr where sprzh='{0}'and szsf=b.zcd1 and szcs=b.zcd2 and szxq=b.zcd3)) ", CurrentUser.UserName);
                    }
                    // 市建设局
                    else if (zhlx == "SYS")
                    {
                        strwhere += string.Format(" and 1=1 ");
                    }
                    string sql = "select * from view_jdbg_qyzz a where " + strwhere + "  order by qymc  ";


                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                }
                else
                {
                    if (msg == "")
                    {
                        msg = "登录失败！";
                    }
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 政务网用户单点登录校验
        public ActionResult SSOCheck()
        {
            bool success = true;
            string msg = "";
            try
            {
                // 票据 ssoticket是跨session用的，在原先链接里面加上&ssoticket=就可以实现跨session登录
                string ssoticket = Request["ssoticket"].GetSafeString();
                // 从请求的参数中获取令牌 ssotoken
                string ssotoken = Request["ssotoken"].GetSafeString();
                //从请求参数中获取具体办事事项地址（若此项有值，成功登录后请跳转此地址到具体事项，否则跳转系统首页）
                string gotoUrl = Request["goto"].GetSafeString();
                SysLog4.WriteError("----ssotoken----\r\n" + ssotoken);
                SysLog4.WriteError("----ssoticket----\r\n" + ssoticket);
                SysLog4.WriteError("----gotoUrl----\r\n" + gotoUrl);
                // 验证令牌并获取用户的登录信息
                string result = SSOCheckHelper.doQuery(ssotoken);
                SysLog4.WriteError(result);

                // 获取用户信息
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(result);
                if (retdata != null)
                {
                    string errorCode = retdata["errCode"].GetSafeString();
                    if (errorCode == "0")
                    {
                        // 企业（法人）信息
                        
                        Dictionary<string, object> companyInfo = (Dictionary < string, object>)retdata["info"];
                        if (companyInfo !=null)
                        {
                            // 企业名称
                            string qymc = companyInfo["CompanyName"].GetSafeString();
                            // 组织机构代码
                            string zzjgdm = companyInfo["OrganizationNumber"].GetSafeString();
                            // 社会统一信用代码
                            string uniscid = companyInfo["uniscid"].GetSafeString();
                            // 政务服务网账户唯一标识
                            string userId = companyInfo["userId"].GetSafeString();

                            string procstr = string.Format("SSOCheck('{0}','{1}','{2}','{3}')", qymc, zzjgdm, uniscid, userId);
                            IList<IDictionary<string, string>> ddt = CommonService.ExecDataTableProc(procstr, out msg);
                            if (ddt.Count > 0)
                            {
                                success = ddt[0]["ret"].GetSafeBool();
                                msg = ddt[0]["msg"].GetSafeString();
                                
                                // 校验成功
                                if (success)
                                {
                                    // 获取当前用户账号
                                    string username = ddt[0]["username"].GetSafeString();
                                    // 登录系统
                                    success = Remote.UserService.LoginWithOutPassWord(username, Configs.AppId, out msg);
                                    // 登录成功
                                    if (success)
                                    {
                                        // 设置日志系统用户
                                        BD.Log.Common.LogUser.SetUserInfo(CurrentUser.UserName, CurrentUser.RealName, CurrentUser.HasRight("JCBGM09041"));
                                        // 设置流程模块用户
                                        BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(
                                            new WorkFlow.Common.SessionUser()
                                            {
                                                CompanyId = CurrentUser.CurUser.CompanyId,
                                                CompanyName = CurrentUser.CurUser.CompanyName,
                                                DepartmentId = CurrentUser.CurUser.DepartmentId,
                                                DepartmentName = CurrentUser.CurUser.DepartmentName,
                                                DutyLevel = CurrentUser.CurUser.DutyLevel,
                                                RealName = CurrentUser.CurUser.RealName,
                                                UserName = CurrentUser.CurUser.UserName
                                            }, null);
                                        // 设置录入界面用户
                                        Session["USERCODE"] = CurrentUser.UserCode;
                                        Session["USERNAME"] = CurrentUser.UserName;
                                        Session["REALNAME"] = CurrentUser.RealName;
                                        Session["CPCODE"] = CurrentUser.CompanyCode;
                                        Session["CPNAME"] = CurrentUser.CompanyName;
                                        Session["DEPCODE"] = CurrentUser.CurUser.DepartmentId;
                                        Session["DEPNAME"] = CurrentUser.CurUser.DepartmentName;
                                        Session["SJHM"] = SystemService.GetUserMobile(CurrentUser.UserCode);
                                        // 企业及个人用户企业编号
                                        Session["USERQYBH"] = JcService.GetQybh(CurrentUser.UserCode);
                                        //Session["MenuCode"] = "QYGL_QYBA";
                                        
                                        CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                                        // 设置用户桌面项
                                        bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out msg);
                                        if (!status)
                                            SysLog4.WriteLog(msg);

                                        // 获取页面跳转类型
                                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                                        if (dt.Count > 0)
                                            CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                                        else
                                            CurrentUser.CurUser.UrlJumpType = "SYS";
                                    }
                                    BD.Log.DataModal.Entities.SysLog log = new BD.Log.DataModal.Entities.SysLog()
                                    {
                                        ClientType = LogConst.ClientType,
                                        Ip = ClientInfo.Ip,
                                        LogTime = DateTime.Now,
                                        ModuleName = LogConst.ModuleUser,
                                        Operation = LogConst.UserOpLogin,
                                        UserName = username,
                                        RealName = success ? CurrentUser.RealName : "",
                                        Remark = "",
                                        Result = success
                                    };
                                    LogService.SaveLog(log);
                                }
                            }
                            else
                            {
                                success = false;
                                msg = "校验用户失败！";
                            }
                            

                        }
                    }
                    else
                    {
                        success = false;
                        msg = "用户认证失败：" + retdata["msg"].GetSafeString();
                    }
                }
                


            }
            catch (Exception e )
            {
                SysLog4.WriteError(e.Message);

            }
            if (success)
            {
                return new RedirectResult("/user/main");
            }
            else
            {
                ViewBag.error = msg;
                return View();
            }
        }
        #endregion

        #region 好差评接口

        public void GetZWWPJUrl()
        {
            bool ret = true;
            string msg = "";
            string url = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("GetZWWPJData('{0}')",id);
                    IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                    
                    if (dt.Count > 0)
                    {
                        string bjbh = dt[0]["bjbh"].GetSafeString();
                        string zzjgdm = dt[0]["zzjgdm"].GetSafeString();
                        string matterid = dt[0]["matterid"].GetSafeString();
                        string ckslrxm = dt[0]["ckslrxm"].GetSafeString();
                        if (bjbh !="")
                        {
                            ret = ZWWHCP.GetPJUrl(
                                bjbh, 2, 2,
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                "51", zzjgdm, "legal", matterid,
                                ckslrxm, "330602", "绍兴市本级",
                                out url, out msg
                            );
                        }
                        else
                        {
                            ret = false;
                            msg = "办件编号为空！";
                        }
                        
                    }
                    else
                    {
                        ret = false;
                        msg = "无法获取评价相关数据";
                    }
                    
                }
                else
                {
                    ret = false;
                    msg = "id不能为空";
                }
                

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"url\":\"{2}\"}}", ret ? "0" : "1", msg, url));
            Response.End();
        }

        public JsonResult LRBJBH()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                string bjbh = Request["bjbh"].GetSafeString();
                string procstr = string.Format("SaveQYZZSBBJBH('{0}','{1}')", id, bjbh);
                CommonService.ExecProc(procstr, out msg);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            return Json(new { code = ret ? "0" : "1", msg = msg });
        }
        #endregion

        #region 获取承诺制申报资质类型
        public void GetQyzzSbCnzlb()
        {
            string msg = "";
            bool code = true;
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                string sql = "select * from H_QY_ZZFL where ISCNZ=1";
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (dt.Count > 0)
                {
                    var g = dt.GroupBy(x => x["xl"].GetSafeString());
                    foreach (var item in g)
                    {
                        Dictionary<string, object> d = new Dictionary<string, object>();
                        string xl = item.Key;
                        d.Add("xl", xl);
                        d.Add(
                            "zzfwlist", 
                            dt.Where(x => x["xl"].GetSafeString() == xl)
                            .Select(
                                x => new {
                                    bh = x["bh"].GetSafeString(),
                                    dj = x["dj"].GetSafeString(),
                                    zzfwmc = x["zzfwmc"].GetSafeString()
                                }
                            )
                        );
                        data.Add(d);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }


        }
        #endregion



    }
}