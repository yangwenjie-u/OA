using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using CryptFun = Bd.jcbg.Common.CryptFun;
using Bd.jcbg.Common;
using BD.Jcbg.Web.Remote;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ReportPrintService.OpenXmlSdk;
using System.Threading;
using System.Data;
using System.Data.OleDb;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using BD.Jcbg.Web.Func;
using Newtonsoft.Json.Linq;

namespace BD.Jcbg.Web.Controllers
{
    public class WLXFController : Controller
    {

        #region 服务
        private ISystemService _systemService = null;
        private ISystemService SystemService
        {
            get
            {
                try
                {
                    if (_systemService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _systemService = webApplicationContext.GetObject("SystemService") as ISystemService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _systemService;
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
                    _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                }
                return _commonService;
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

        private static IApiSessionService _apiSessionService = null;
        private static IApiSessionService ApiSessionService
        {
            get
            {
                if (_apiSessionService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _apiSessionService = webApplicationContext.GetObject("ApiSessionService") as IApiSessionService;
                }
                return _apiSessionService;
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

        private BD.WorkFlow.Bll.RemoteUserService _remoteUserService = null;
        private BD.WorkFlow.Bll.RemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as BD.WorkFlow.Bll.RemoteUserService;
                }
                return _remoteUserService;
            }
        }

        #endregion

        #region 页面

        [Authorize]
        public ActionResult cq()
        {

            ViewData["Page1"] = "display:none;";
            ViewData["Page2"] = "";
            return View();
        }

        [Authorize]
        public ActionResult qysh()
        {

            ViewBag.qybh = Request["qybh"].GetSafeString();
            ViewBag.zzbh = Request["zzbh"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult rysh()
        {

            ViewBag.rybh = Request["rybh"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult viewProcess()
        {
            ViewBag.recid = Request["serialno"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult shouadmin()
        {
            return View();
        }

        [Authorize]
        public ActionResult viewXmfpxq()
        {
            return View();
        }

        [Authorize]
        public ActionResult viewfjck()
        {

            ViewBag.serialno = Request["serialno"].GetSafeString();
            ViewBag.lx = Request["lx"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult viewDtjz()
        {
            ViewBag.serialno = Request["serialno"].GetSafeString();
            ViewBag.fxbaid = Request["fxbaid"].GetSafeString();
            ViewBag.typebh = Request["typebh"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult gclr()
        {
            ViewBag.fxbaid = Request["fxbaid"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult qyzz()
        {
            ViewBag.qybh = Request["qybh"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult gcList()
        {
            ViewBag.type = Request["type"].GetSafeString();
            return View();
        }

        [Authorize]
        public ActionResult sjtj()
        {
            return View();
        }

        [Authorize]
        public ActionResult setPosMap()
        {
            return View();
        }

        [Authorize]
        public ActionResult setPosMapShow()
        {
            return View();
        }

        public ActionResult datainput()
        {
            return View();
        }
     
        public ActionResult editiswt()
        {
            return View();
        }

        [Authorize]
        public ActionResult xcpd()
        {
            ViewBag.serialno = Request["serialno"].GetSafeString();
            ViewBag.fxbaid = Request["fxbaid"].GetSafeString();
            ViewBag.recid = Request["recid"].GetSafeInt();
            ViewBag.parentid = Request["parentid"].GetSafeInt();
            return View();
        }

        [Authorize]
        public ActionResult gcChooseList()
        {
            return View();
        }

        public ActionResult index()
        {
            ViewBag.realname = CurrentUser.RealName;
            return View();
        }
        #endregion

        #region 相关企业消防申请单打印
        public ActionResult FlowQyReportDown()
        {
            string url = "";
            string reportFile = Request["filename"].GetSafeString();
            string FXBAID = Request["FXBAID"].GetSafeString();
            string type = Request["type"].GetSafeString("printdow");
            string fgcids1 = Request["fgcids1"].GetSafeString();
            string fgcids2 = Request["fgcids2"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();

            string table = "view_i_m_gc_xfys|view_i_s_gc_xfys_sjdw|view_i_s_gc_xfys_sgdw|view_i_s_gc_xfys_jldw|view_i_s_gc_xfys_tsdw|view_i_s_gc_xfys_jcdw_sgz|view_i_s_gc_xfys_qyfgc1|view_i_s_gc_xfys_qyfgc2|view_i_m_gc_xfys2|view_i_s_gc_xfys_dw";
            string where = "FXBAID='" + FXBAID + "'|FXBAID='" + FXBAID + "'|FXBAID='" + FXBAID + "'|FXBAID='" + FXBAID + "'|FXBAID='" + FXBAID + "'|FXBAID='" + FXBAID + "'|RECID in (" + fgcids1.FormatSQLInStr() + ")|RECID in (" + fgcids2.FormatSQLInStr() + ")|Serialno='" + serialno + "'|Serialno='" + serialno + "'";
            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (type == "pdf")
                c.openType = ReportPrint.OpenType.PDF;
            else if (type == "print")
                c.openType = ReportPrint.OpenType.Print;
            else if (type == "printdow")
                c.openType = ReportPrint.OpenType.PDFFileDown;
            else if (type == "pic")
                c.openType = ReportPrint.OpenType.PIC;
            else
                c.openType = ReportPrint.OpenType.FileDown;
            //c.field = reportFile;
            c.fileindex = "0";
            c.table = table;
            c.filename = reportFile;
            //c.field = "formid";
            c.where = where;
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            c.customtools = "1,|2,|3,|12,下载";

            var guid = g.Add(c);

            url = "/reportPrint/Index?" + guid;
            return new RedirectResult(url);
        }
        #endregion

        #region 相关消防打印
        public ActionResult FlowReportDown()
        {
            string url = "";
            string reportFile = Request["filename"].GetSafeString();
            string Serialno = Request["Serialno"].GetSafeString();
            string FXBAID = Request["FXBAID"].GetSafeString();
            string type = Request["type"].GetSafeString("printdow");
            int fileindex = Request["fileindex"].GetSafeInt();
            string sql = "select recid,fxbaid,serialno from I_S_GC_XFYS where serialno='" + Serialno + "'";
            IList<IDictionary<string, string>> datas = CommonService.GetDataTable(sql);
            int parentid = 0;
            if (datas.Count > 0)
                parentid = datas[0]["recid"].GetSafeInt();
            string table = "view_i_m_gc_xfys2|view_i_s_gc_xfys_fgc1|view_i_s_gc_xfys_fgc2|view_i_s_gc_xfys_zl|view_i_s_gc_xfysjl|view_i_s_gc_xfjc_xcjc|view_i_s_gc_xfysjl2|view_i_s_gc_xfys_fgc3|view_i_s_gc_xfys_dw|view_i_s_gc_xfys_zlgd|view_i_s_gc_xfys_zlgd_list1|view_i_s_gc_xfys_zlgd_list2|view_i_s_gc_xfys_zlgd_list|i_s_gc_xfys_xcpditem|view_i_s_gc_xfys_fgc4|view_i_s_gc_xfys_fgc5|view_i_s_gc_xfys_fgc";
            string where = "Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "' order by xh asc|Serialno='" + Serialno + "' order by xh asc|Serialno='" + Serialno + "' order by xh asc|parentid=" + parentid+ "|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'";


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            string signedfilename = "";
            if (fileindex == 0)
                signedfilename = (reportFile + Serialno + FXBAID).Replace('|', '-');
            else
                signedfilename = reportFile;
            string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + signedfilename + ".docx";
            if (System.IO.File.Exists(filepath))
            {
                /*
                var myBytes = System.IO.File.ReadAllBytes(filepath);
                string mime = "application/pdf";
                return File(myBytes, mime, reportFile + ".pdf");
                */

                c.type = ReportPrint.EnumType.Word;
                if (type == "pdf")
                    c.openType = ReportPrint.OpenType.PDF;
                else if (type == "print")
                    c.openType = ReportPrint.OpenType.Print;
                else if (type == "printdow")
                    c.openType = ReportPrint.OpenType.PDFFileDown;
                else if (type == "pic")
                    c.openType = ReportPrint.OpenType.PIC;
                else
                    c.openType = ReportPrint.OpenType.FileDown;
                //c.field = reportFile;
                c.fileindex = "1";
                c.table = table;
                c.filename = signedfilename;
                //c.field = "formid";
                c.where = where;
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print ;

                c.AllowVisitNum = 1;
                c.customtools = "1,|2,|3,|12,下载";

            }

            else
            {

                c.type = ReportPrint.EnumType.Word;
                if (type == "pdf")
                    c.openType = ReportPrint.OpenType.PDF;
                else if (type == "print")
                    c.openType = ReportPrint.OpenType.Print;
                else if (type == "printdow")
                    c.openType = ReportPrint.OpenType.PDFFileDown;
                else if (type == "pic")
                    c.openType = ReportPrint.OpenType.PIC;
                else
                    c.openType = ReportPrint.OpenType.FileDown;
                //c.field = reportFile;
                c.fileindex = "0";
                c.table = table;
                c.filename = reportFile;
                //c.field = "formid";
                c.where = where;
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print ;

                c.AllowVisitNum = 1;
                c.customtools = "1,|2,|3,|12,下载";


            }
            var guid = g.Add(c);

            url = "/reportPrint/Index?" + guid;
            return new RedirectResult(url);
        }
        #endregion

        #region 相关记分单打印
        public ActionResult FlowJfReportDown()
        {
            string url = "";
            string reportFile = Request["filename"].GetSafeString();
            string Serialno = Request["Serialno"].GetSafeString();
            string type = Request["type"].GetSafeString("printdow");

            string table = "view_i_s_qy_jf|view_i_s_qy_jf_mx|view_i_s_qy_jf_gzd|view_i_s_ry_jf|view_i_s_ry_jf_mx|view_i_s_ry_jf_gzd";
            string where = "Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'|Serialno='" + Serialno + "'";


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();

            string signedfilename = (reportFile + Serialno).Replace('|', '-');
            string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + signedfilename + ".docx";
            if (System.IO.File.Exists(filepath))
            {
                c.type = ReportPrint.EnumType.Word;
                if (type == "pdf")
                    c.openType = ReportPrint.OpenType.PDF;
                else if (type == "print")
                    c.openType = ReportPrint.OpenType.Print;
                else if (type == "printdow")
                    c.openType = ReportPrint.OpenType.PDFFileDown;
                else if (type == "pic")
                    c.openType = ReportPrint.OpenType.PIC;
                else
                    c.openType = ReportPrint.OpenType.FileDown;
                //c.field = reportFile;
                c.fileindex = "0";
                c.table = "";
                c.filename = signedfilename;
                //c.field = "formid";
                c.where = "";
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print ;

                c.AllowVisitNum = 1;
                c.customtools = "1,|2,|3,|12,下载";

            }

            else
            {

                c.type = ReportPrint.EnumType.Word;
                if (type == "pdf")
                    c.openType = ReportPrint.OpenType.PDF;
                else if (type == "print")
                    c.openType = ReportPrint.OpenType.Print;
                else if (type == "printdow")
                    c.openType = ReportPrint.OpenType.PDFFileDown;
                else if (type == "pic")
                    c.openType = ReportPrint.OpenType.PIC;
                else
                    c.openType = ReportPrint.OpenType.FileDown;
                //c.field = reportFile;
                c.fileindex = "0";
                c.table = table;
                c.filename = reportFile;
                //c.field = "formid";
                c.where = where;
                c.signindex = 0;
                //c.openType = ReportPrint.OpenType.Print ;

                c.AllowVisitNum = 1;
                c.customtools = "1,|2,|3,|12,下载";

            }
            var guid = g.Add(c);

            url = "/reportPrint/Index?" + guid;
            return new RedirectResult(url);
        }
        #endregion

        #region 删除工程信息
        [Authorize]
        public void getXFYSDel()
        {
            bool ret = false;
            string rettext = "";
            string err = "";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> sqsl = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> htsl = new List<IDictionary<string, string>>();
            //状态位，0表示等待审核（起草），1初审，2审核，3公示，4公布，-1表示公示完毕，历史记录的
            try
            {
                string id = Request["id"].GetSafeString();

                string zt = "";
                string lxdm = "";
                string lrrzh = "";
                string lrrxm = "";
                TimeSpan nowDt = DateTime.Now.TimeOfDay;

                TimeSpan workStartDT = DateTime.Parse("0:10").TimeOfDay;
                TimeSpan workEndDT = DateTime.Parse("6:00").TimeOfDay;
                if (nowDt > workStartDT && nowDt < workEndDT)
                {
                    ret = false;
                    err = "凌晨 0：10到6：00 系统维护中，禁止操作！";
                }
                else
                {

                    datas = CommonService.GetDataTable("select zt,lrrzh,lrrxm from I_M_GC_XFYS where FXBAID='" + id + "'");
                    if (datas.Count > 0)
                    {
                        zt = datas[0]["zt"].GetSafeString();
                        lrrxm = datas[0]["lrrxm"].GetSafeString();
                        lrrzh = datas[0]["lrrzh"].GetSafeString();
                    }
                    else
                    {
                        id = "";
                    }

                    if (id == "")
                    {
                        ret = false;
                        err = "记录不存在！";
                    }
                    else
                    {
                        sqsl = CommonService.GetDataTable("select * from I_S_GC_XFYS where FXBAID='" + id + "'");
                        htsl = CommonService.GetDataTable("select * from I_S_GC_XFJC_HT where FXBAID='" + id + "'");
                        if (sqsl.Count > 0)
                        {
                            ret = false;
                            err = "该工程已发起申请不能删除！";
                        }
                        else if (htsl.Count > 0)
                        {
                            ret = false;
                            err = "该工程已有合同数据不能删除！";
                        }
                        else
                        {
                            if (zt != "YT")
                            {
                                ret = false;
                                err = "已受理的工程不能删除！";
                            }
                            else
                            {
                                string sql = "";
                                IList<string> sqls = new List<string>();
                                sql = "delete from I_M_GC_XFYS where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                sql = "delete from I_S_GC_XFYS_SJDW where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                sql = "delete from I_S_GC_XFYS_SGDW where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                sql = "delete from I_S_GC_XFYS_JLDW where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                sql = "delete from I_S_GC_XFYS_TSDW where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                sql = "delete from I_S_GC_XFYS_JCDW_SGZ where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                sql = "delete from I_S_GC_XFYS_FGC where FXBAID='" + id + "'";
                                sqls.Add(sql);
                                ret = CommonService.ExecTrans(sqls, out err);
                            }
                        }
                    }
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
        #endregion

        #region 消防检查记录

        /// <summary>
        /// 获取某次消防验收列表
        /// </summary>
        public void getXFYSJLList()
        {

            string FXBAID = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string serialno = Request["serialno"].GetSafeString();
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from I_S_GC_XFYSJL where fxbaid='" + FXBAID + "' and serialno='" + serialno + "'";
                string strwhere = "";
                sql += " order by Type1 desc";
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
        /// 新增验收记录
        /// </summary>
        public void getXFYSJLAdd()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string FXBAID = Request["fxbaid"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string type1 = Request["type1"].GetSafeString();
            string type2 = Request["type2"].GetSafeString();
            string jdjl = Request["jdjl"].GetSafeString();
            string bz = Request["bz"].GetSafeString();
            string fj = Request["fj"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();

            string iswt = Request["iswt"].GetSafeInt(0).GetSafeString();

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            string zh = Request["zh"].GetSafeString();

            string username = "";
            string realname = "";
            if (!CurrentUser.IsLogin)
            {
                username = UserService.GetUserCode(zh);
                realname = UserService.GetUserRealName(username);
            }
            else
            {
                username = CurrentUser.UserName;
                realname = CurrentUser.RealName;
            }

            //状态位，0表示等待审核（起草），1初审，2审核，3公示，4公布，-1表示公示完毕，历史记录的,5提交审核
            try
            {
                string sql = "";
                if (id != "")
                    sql = "update I_S_GC_XFYSJL set FXBAID='" + FXBAID + "',LRSJ=getdate(),LRRZH='" + username + "',LRRXM='" + realname + "',Type1='" + type1 + "',Type2='" + type2 + "',JDJL='" + jdjl + "',BZ='" + bz + "',FJ='" + fj + "',iswt='" + iswt + "' where [RECID]=" + id + " and [FXBAID]='" + FXBAID + "'";
                else
                    sql = "INSERT INTO [I_S_GC_XFYSJL] ([FXBAID] ,[LRSJ] ,[LRRZH] ,[LRRXM] ,[Type1] ,[Type2] ,[JDJL] ,[BZ] ,[FJ],Serialno,iswt) VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','" + type1 + "' ,'" + type2 + "' ,'" + jdjl + "' ,'" + bz + "' ,'" + fj + "','" + serialno + "','" + iswt + "')";
                ret = CommonService.ExecSql(sql, out err);
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

        /// <summary>
        /// 删除验收记录
        /// </summary>
        public void getXFYSJLDel()
        {
            bool ret = false;
            string rettext = "";
            string err = "";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string FXBAID = Request["fxbaid"].GetSafeString();
                string id = Request["id"].GetSafeString();


                ret = CommonService.ExecSql("delete from [I_S_GC_XFYSJL] where [RECID]=" + id + " and [FXBAID]='" + FXBAID + "'", out err);
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

        /// <summary>
        /// 获取所有消防分类
        /// </summary>
        public void getXFYSJLLXList()
        {
            string FXBAID = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string serialno = Request["serialno"].GetSafeString();
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from I_S_GC_XFYSJL_LX where fxbaid='" + FXBAID + "' and serialno='" + serialno + "'";
                sql += " order by Type1 desc";
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
        /// 新增或者删除消防分类
        /// </summary>
        public void getXFYSJLLXAdd()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string fxbaid = Request["fxbaid"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            string type1 = Request["type1"].GetSafeString();
            string type2 = Request["type2"].GetSafeString();          
            try
            {
                string sql = "";
                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                datas = CommonService.GetDataTable("select * from i_s_gc_xfysjl_lx where type1='" + type1 + "' and type2='" + type2 + "' and fxbaid='" + fxbaid + "' and serialno='" + serialno + "'");
                if (datas.Count > 0)
                    sql = "delete i_s_gc_xfysjl_lx where type1=" + type1 + " and type2='" + type2 + "' and fxbaid='" + fxbaid + "' and serialno='" + serialno + "'";
                else
                    sql = "insert into i_s_gc_xfysjl_lx (fxbaid,serialno,type1,type2,lrsj,lrrzh,lrrxm) values('" + fxbaid + "' ,'" + serialno + "','" + type1 + "' ,'" + type2 + "',getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "')";
                ret = CommonService.ExecSql(sql, out err);
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
        #endregion

        #region 消防上传资料记录

        #region 获取某次消防验收资料列表
        public void getXFYSZLList()
        {

            string FXBAID = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string serialno = Request["serialno"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from I_S_GC_XFYS_ZL  where fxbaid='" + FXBAID + "' and serialno='" + serialno + "' and TypeBh='" + typebh + "'";
                string strwhere = "";
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                if (datas.Count <= 0)
                {
                    IList<string> sqls = new List<string>();

                    sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust,MustIn,MsgInfo,TypeBh ) select '" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "',BZ,'','" + serialno + "',IsMust,MustIn,MsgInfo,typebh from H_XFYS_ZL where typebh='" + typebh + "'");


                    /*
                    if (typebh == "01")
                    {
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设工程消防设计备案申报表','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设单位的工商营业执照等合法身份证明文件复印件；','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','申请人身份证明文件（建设单位法定代表人委托他人办理事项的，应提供委托书、委托人和被委托人身份证明文件）','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防设计文件（施工图电子化联合审查前）','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','所在建筑的建筑合法性证明文件（改建工程）','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','施工许可文件复印件（新建、扩建工程）','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','施工图审查机构出具的《建设工程施工图设计文件消防审查合格书》','','" + serialno + "',0)");
                    }
                    else if(typebh == "02")
                    {
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设工程消防验收申报表','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','工程竣工验收报告','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','有关消防设施的工程竣工图纸','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防产品质量合格证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','具有防火性能要求的建筑构件、建筑材料（含建筑保温材料）、装修材料符合国家标准或者行业标准的证明文件、出厂合格证复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防设施检测合格证明文件复印件','','" + serialno + "')");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','施工、工程监理、检测单位的合法身份证明和资质等级证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设单位的工商营业执照等合法身份证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','法律、行政法规规定的其他材料','','" + serialno + "',0)");
                    }
                    else if (typebh == "03")
                    {
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设工程竣工验收消防备案申报表','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','工程竣工验收报告','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','有关消防设施的工程竣工图纸','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防产品质量合格证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','具有防火性能要求的建筑构件、建筑材料（含建筑保温材料）、装修材料符合国家标准或者行业标准的证明文件、出厂合格证复印件','','" + serialno + "',0)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防设施检测合格证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','施工、工程监理、检测单位的合法身份证明和资质等级证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设单位的工商营业执照等合法身份证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','法律、行政法规规定的其他材料','','" + serialno + "',0)");
                    }
                    else
                    {
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设工程消防验收申报表','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','工程竣工验收报告','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','有关消防设施的工程竣工图纸','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防产品质量合格证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','具有防火性能要求的建筑构件、建筑材料（含建筑保温材料）、装修材料符合国家标准或者行业标准的证明文件、出厂合格证复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','消防设施检测合格证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','施工、工程监理、检测单位的合法身份证明和资质等级证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','建设单位的工商营业执照等合法身份证明文件复印件','','" + serialno + "',1)");
                        sqls.Add("INSERT INTO [I_S_GC_XFYS_ZL]([FXBAID],[LRSJ],[LRRZH],[LRRXM],[BZ],[FJ],[Serialno],IsMust)VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','法律、行政法规规定的其他材料','','" + serialno + "',0)");
                    }*/
                    CommonService.ExecTrans(sqls);
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

        #region 新增验收记录
        public void getXFYSZLAdd()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string FXBAID = Request["fxbaid"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string bz = Request["bz"].GetSafeString();
            string fj = Request["fj"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            string yj = Request["yj"].GetSafeString();
            string fyj = Request["fyj"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            //状态位，0表示等待审核（起草），1初审，2审核，3公示，4公布，-1表示公示完毕，历史记录的,5提交审核
            try
            {
                string sql = "";
                if (id != "")
                    sql = "update I_S_GC_XFYS_ZL set FXBAID='" + FXBAID + "',LRSJ=getdate(),LRRZH='" + CurrentUser.UserName + "',LRRXM='" + CurrentUser.RealName + "',BZ='" + bz + "',FJ='" + fj + "',yj='" + yj + "',fyj='" + fyj + "',typebh='" + typebh + "' where [RECID]=" + id + " and [FXBAID]='" + FXBAID + "'";
                else
                    sql = "INSERT INTO [I_S_GC_XFYS_ZL] ([FXBAID] ,[LRSJ] ,[LRRZH] ,[LRRXM] ,[BZ] ,[FJ],Serialno,[YJ],[FYJ],[TypeBh]) VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "' ,'" + bz + "' ,'" + fj + "','" + serialno + "','" + yj + "','" + fyj + "','" + typebh + "')";
                ret = CommonService.ExecSql(sql, out err);
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
        #endregion

        #region 删除验收记录
        public void getXFYSZLDel()
        {
            bool ret = false;
            string rettext = "";
            string err = "";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string FXBAID = Request["fxbaid"].GetSafeString();
                string id = Request["id"].GetSafeString();

                ret = CommonService.ExecSql("delete from [I_S_GC_XFYS_ZL] where [RECID]=" + id + " and [FXBAID]='" + FXBAID + "'", out err);
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
        #endregion

        #region 获取某次档案资料列表
        public void getXFYSZLGDList()
        {

            string FXBAID = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string serialno = Request["serialno"].GetSafeString();
            string parentserialno = Request["parentserialno"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {

                string sql = "select a.*,b.bh from I_S_GC_XFYS_ZLGD_LIST as a left join I_S_GC_XFYS_ZLGD as b on a.serialno=b.serialno where a.fxbaid='" + FXBAID + "' and a.TypeBh='" + typebh + "' and a.serialno='" + serialno + "' and a.parentserialno='" + parentserialno + "' order by xh asc";
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
        #endregion

        #region 档案资料同步
        public void getXFYSDAZLTB()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string FXBAID = Request["fxbaid"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            string parentserialno = Request["parentserialno"].GetSafeString();
            try
            {
                IList<string> sqls = new List<string>();
                if (!string.IsNullOrEmpty(parentserialno))
                {
                    sqls.Add("INSERT INTO [I_S_GC_XFYS_ZLGD_LIST] ([FXBAID] ,[LRSJ] ,[LRRZH] ,[LRRXM] ,[DAZLMC],[SL],[FJ],[ISMUST],[Serialno],[TypeBh],[ParentSerialNO],[LX],[XH],[ParentID]) SELECT '" + FXBAID + "',getdate(),'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "' ,dazlmc,yj,fj,0,'','" + typebh + "','" + parentserialno + "',lx,(row_number() OVER (partition by fxbaid order by lx asc)) as xh,recid FROM View_I_S_GC_XFYS_ZL_LIST where Serialno='" + parentserialno + "' and isnull(ISZLGD,0)<>1 ");
                    sqls.Add("UPDATE I_S_GC_XFYS_ZL SET ISZLGD=1 WHERE Serialno='" + parentserialno + "'");
                    sqls.Add("UPDATE I_S_GC_XFYS_REPORTFILE SET ISZLGD=1 WHERE Serialno='" + parentserialno + "'");
                    sqls.Add("UPDATE I_S_GC_XFYS SET ISZLGD1=1 WHERE Serialno='" + parentserialno + "'");
                    sqls.Add("UPDATE I_S_GC_XFYS SET ISZLGD2=1 WHERE Serialno='" + parentserialno + "'");
                }

                ret = CommonService.ExecTrans(sqls);
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
        #endregion

        #region 新增档案资料
        public void getXFYSZLGDAdd()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string FXBAID = Request["fxbaid"].GetSafeString();
            string dazlmc = Request["dazlmc"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string bz = Request["bz"].GetSafeString();
            string fj = Request["fj"].GetSafeString();
            string ismust = Request["ismust"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            string sl = Request["sl"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            string parentserialno = Request["parentserialno"].GetSafeString();
            string xh = Request["xh"].GetSafeString();
            string lx = Request["lx"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "";
                if (id != "")
                    sql = "update I_S_GC_XFYS_ZLGD_LIST set FXBAID='" + FXBAID + "',LRSJ=getdate(),LRRZH='" + CurrentUser.UserName + "',LRRXM='" + CurrentUser.RealName + "',DAZLMC='" + dazlmc + "',BZ='" + bz + "',SL='" + sl + "',FJ='" + fj + "',ISMUST='" + ismust + "',typebh='" + typebh + "',xh='" + xh + "' where [RECID]=" + id + " and [FXBAID]='" + FXBAID + "'";
                else
                    sql = "INSERT INTO [I_S_GC_XFYS_ZLGD_LIST] ([FXBAID] ,[LRSJ] ,[LRRZH] ,[LRRXM] ,[DAZLMC],[BZ] ,[FJ],[ISMUST],[Serialno],[SL],[TypeBh],[ParentSerialNO],[LX],[XH]) VALUES('" + FXBAID + "' ,getdate() ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "' ,'" + dazlmc + "','" + bz + "' ,'" + fj + "','" + ismust + "','" + serialno + "','" + sl + "','" + typebh + "','" + parentserialno + "','" + lx + "','" + xh + "')";
                SysLog4.WriteError(sql);
                ret = CommonService.ExecSql(sql, out err);
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
        #endregion

        #region 删除档案资料记录
        public void getXFYSZLGDDel()
        {
            bool ret = false;
            string rettext = "";
            string err = "";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string FXBAID = Request["fxbaid"].GetSafeString();
                string id = Request["id"].GetSafeString();
                string lx = Request["lx"].GetSafeString();
                string parentid = Request["parentid"].GetSafeString();
                string xh = Request["xh"].GetSafeString();
                string typebh = Request["typebh"].GetSafeString();
                string serialno = Request["serialno"].GetSafeString();
                string parentserialno = Request["parentserialno"].GetSafeString();
                IList<string> sqls = new List<string>();
                if (lx == "0" && parentid != "")
                    sqls.Add("update i_s_gc_xfys_zl set iszlgd=0 where recid=" + parentid + " and [FXBAID]='" + FXBAID + "' ");
                else if (lx == "1" && parentid != "")
                    sqls.Add("update i_s_gc_xfys_reportfile set iszlgd=0 where recid=" + parentid + " and [FXBAID]='" + FXBAID + "'");
                else if (lx == "2" && parentid != "")
                    sqls.Add("update i_s_gc_xfys set iszlgd1=0 where recid=" + parentid + " and [FXBAID]='" + FXBAID + "'");
                else if (lx == "3" && parentid != "")
                    sqls.Add("update i_s_gc_xfys set iszlgd2=0 where recid=" + parentid + " and [FXBAID]='" + FXBAID + "'");
                sqls.Add("update I_S_GC_XFYS_ZLGD_LIST set xh=(xh-1) where xh>" + xh + " and parentserialno='" + parentserialno + "' and typebh='" + typebh + "' and serialno='" + serialno + "'");
                sqls.Add("delete from [I_S_GC_XFYS_ZLGD_LIST] where [RECID]=" + id + " and [FXBAID]='" + FXBAID + "'");
                ret = CommonService.ExecTrans(sqls);
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
        #endregion

        #endregion

        #region 获取正在整改的工程
        public void getXFYSList()
        {

            string FXBAID = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string serialno = Request["serialno"].GetSafeString();

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select recid,fxbaid,serialno,gcmc,l_typebh,l_typename,jdy,tqry from View_I_M_GC_XFYS1 where L_ZT='YSQR' and ( ','+JDY+',' like '%," + CurrentUser.UserName + ",%' or ','+TQRY+',' like '%," + CurrentUser.UserName + ",%')";
                sql += " union all select recid,fxbaid,'' as serialno,gcmc,typebh as l_typebh,'' as l_typename,'' as jdy,'' as tqry from i_s_gc_xfys_lsxcys where zt='ysqr'";
                string strwhere = "";
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
        #endregion

        #region 抽签
        public void CheckSelect()
        {

            /*
            -1 已经抽签抽中
            -2 已经抽签没抽中
            0 抽签抽中
            1 抽签没抽中
            2 没抽过
            -100 报错
             */

            string FXBAID = Request["fxbaid"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            string type = Request["type"].GetSafeString();

            int lv = 50;
            int ztint = -100;
            string msg = "";
            bool code = true;
            int result = 0;
            int[] intArr = new int[lv];
            ArrayList myList = new ArrayList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string lx = "";
            try
            {

                string sql = "select isselect,selectjson,bllx,bl from dbo.I_S_GC_XFYS where fxbaid='" + FXBAID + "' and Serialno='" + serialno + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    lx = dt[0]["bllx"].GetSafeString();

                    if (dt[0]["selectjson"].GetSafeString() != "")
                    {
                        code = false;
                        msg = "该记录已经抽签，不能重复抽签！";
                        if (dt[0]["isselect"].GetSafeInt() == 1)
                            ztint = -1;
                        else
                            ztint = -2;

                    }
                    else
                    {
                        code = true;
                        lv = dt[0]["bl"].GetSafeInt(100);
                        Random rnd = new Random();
                        while (myList.Count < lv)
                        {
                            int num = rnd.Next(1, 101);
                            if (!myList.Contains(num))
                                myList.Add(num);
                        }/*
                for (int i = 0; i < 100; i++)
                {
                    intArr[i] = (int)myList[i];
                }*/
                        result = rnd.Next(1, 101);

                        if (FXBAID == "4202ea77ee684575b1c01d86f744b523" || FXBAID == "2d2dbf87430d435cb748dd5e55636b10")
                        {
                            while (myList.Contains(result))
                            { result = rnd.Next(1, 101); }
                        }


                        if (myList.Contains(result))
                        {
                            code = true;
                            msg = "当前项目被抽中！";
                            ztint = 0;
                        }
                        else
                        {
                            code = false;
                            msg = "当前项目没有被抽中！";
                            ztint = 1;
                        }

                        IList<string> sqls = new List<string>();

                        string jsontext = string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3},\"lx\":\"{4}\",\"zt\":\"{5}\"}}", code ? "0" : "1", msg, result, jss.Serialize(myList), lx, ztint);
                        if (code)
                        {
                            sqls.Add("update I_S_GC_XFYS set isselect=1 ,selectjson='" + jsontext + "' where fxbaid='" + FXBAID + "' and Serialno='" + serialno + "'");
                            sqls.Add("INSERT INTO [STFormItem]([FormID] ,[ItemName],[ItemValue]) select formid,'CQJG','1' from stform where SerialNo='" + serialno + "'");
                        }
                        else
                        {
                            sqls.Add("update I_S_GC_XFYS set isselect=0 ,selectjson='" + jsontext + "' where fxbaid='" + FXBAID + "' and Serialno='" + serialno + "'");
                            sqls.Add("INSERT INTO [STFormItem]([FormID] ,[ItemName],[ItemValue]) select formid,'CQJG','0' from stform where SerialNo='" + serialno + "'");
                        }



                        CommonService.ExecTrans(sqls);
                    }





                }
                else
                {
                    code = false;
                    msg = "该记录不存在，请不要修改";
                }



            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3},\"lx\":\"{4}\",\"zt\":\"{5}\"}}", code ? "0" : "1", msg, result, jss.Serialize(myList), lx, ztint));
                Response.End();
            }
        }
        #endregion

        #region 抽签
        public void HaveSelect()
        {


            /*
 0 已经抽签抽中
1 已经抽签没抽中
2 没抽过
-100 报错
             */
            string FXBAID = Request["fxbaid"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            bool code = false;
            string msg = "";
            string lx = "";
            int ztint = -100;
            try
            {

                string sql = "select isselect,selectjson,bllx,bl from dbo.I_S_GC_XFYS where fxbaid='" + FXBAID + "' and Serialno='" + serialno + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    lx = dt[0]["bllx"].GetSafeString();
                    if (dt[0]["selectjson"].GetSafeString() != "")
                    {
                        code = true;
                        msg = dt[0]["selectjson"].GetSafeString();
                        if (dt[0]["isselect"].GetSafeInt() == 1)
                            ztint = -1;
                        else
                            ztint = -2;

                    }
                    else
                    {
                        ztint = 2;
                        code = false;
                        msg = dt[0]["bllx"].GetSafeString();
                    }

                }
                else
                {
                    code = false;
                    msg = "该记录不存在，请不要修改";
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                if (code)
                    Response.Write(msg);
                else
                    Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3},\"lx\":\"{4}\",\"zt\":\"{5}\"}}", code ? "0" : "1", msg, "0", "[]", lx, ztint));
                Response.End();
            }
        }
        #endregion

        #region 生成签名照
        public void GetSign()
        {
            string username = Request["username"].GetSafeString();
            byte[] ret = null;
            try
            {

                string[] t = username.Split(',');

                if (t.Length > 1)
                {
                    Bitmap[] maps = new Bitmap[t.Length];
                    for (int i = 0; i < t.Length; i++)
                    {
                        string sign = "";
                        string tuser = UserService.GetUserName(t[i]);
                        if (Remote.UserService.GetUserSign(tuser, out sign))
                        {
                            MemoryStream ms1 = new MemoryStream(sign.DecodeBase64Array());
                            maps[i] = (Bitmap)Image.FromStream(ms1);
                        }
                    }
                    int maxHeight = 0;
                    //计算图片的总数
                    int sumWidth = 0;

                    for (int i = 0; i < t.Length; i++)
                    {
                        maxHeight = Math.Max(maxHeight, maps[i].Height);
                        sumWidth += maps[i].Width;
                    }
                    //创建一个位图
                    Bitmap bgImg = new Bitmap(sumWidth, maxHeight);
                    bgImg.SetResolution(120, 120);

                    Graphics g = Graphics.FromImage(bgImg);
                    //清除画布，背景设置为白色
                    g.Clear(Color.White);

                    int gWidth = 0;
                    for (int i = 0; i < t.Length; i++)
                    {
                        gWidth = i == 0 ? 0 : gWidth + maps[i - 1].Width;
                        g.DrawImage(maps[i], gWidth, 0, maps[i].Width, maps[i].Height);
                    }
                    g.Dispose();
                    MemoryStream ms = new MemoryStream();
                    bgImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ret = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释

                    //string guid = Guid.NewGuid().ToString("N");
                    //string path = Server.MapPath(@"~\ReportPrint\tmpdoc\" + guid + ".jpg");
                    //FileStream fs = new FileStream(path, FileMode.Create);
                    //fs.Write(ret, 0, ret.Length);
                    //fs.Dispose();

                    ms.Close();


                    string filename = "sign.jpg";
                    string mime = MimeMapping.GetMimeMapping(filename);
                    Response.Clear();
                    Response.ContentType = mime;
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();

                }
                else
                {
                    string sign = "";

                    username = UserService.GetUserName(username);
                    if (Remote.UserService.GetUserSign(username, out sign))
                    {
                        ret = sign.DecodeBase64Array();


                        string filename = "sign.jpg";
                        string mime = MimeMapping.GetMimeMapping(filename);
                        Response.Clear();
                        Response.ContentType = mime;
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
                        Response.BinaryWrite(ret);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        //Response.Write("123");
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                Response.StatusCode = 404;
            }
            finally
            {

            }
        }
        #endregion

        #region 获取待整改列表
        public void getZGlist()
        {
            string FXBAID = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string serialno = Request["serialno"].GetSafeString();


            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                string sql = "select a.FXBAID,a.GCMC,a.JSDWMC,b.Typename,b.serialno,Convert(nvarchar(100),b.XMFPSJ,23) as XMFPSJ from dbo.I_M_GC_XFYS a,I_S_GC_XFYS b where a.FXBAID=b.FXBAID and b. zt='XMZG' order by b.XMFPSJ";

                if (CurrentUser.CompanyCode == "CPCOPrePwz5dd4")
                    sql = "select a.FXBAID,a.GCMC,a.JSDWMC,b.Typename,b.serialno,Convert(nvarchar(100),b.XMFPSJ,23) as XMFPSJ from dbo.I_M_GC_XFYS a,I_S_GC_XFYS b where a.FXBAID=b.FXBAID and b. zt='XMZG' and ( ','+b.JDY+',' like '%," + CurrentUser.UserName + ",%' or ','+b.TQRY+',' like '%," + CurrentUser.UserName + ",%') order by b.XMFPSJ";
                else
                    sql = "select a.FXBAID,a.GCMC,a.JSDWMC,b.Typename,b.serialno,Convert(nvarchar(100),b.XMFPSJ,23) as XMFPSJ from dbo.I_M_GC_XFYS a,I_S_GC_XFYS b where a.FXBAID=b.FXBAID and b. zt='XMZG' and (a.lrrzh='$$用户名称$$' or a.jsdwbh  in (select qybh from i_m_qyzh where yhzh='$$用户名称$$')) order by b.XMFPSJ";

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
        #endregion

        #region 登录操作
        public void DoLogin()
        {
            string err = "";
            bool ret = false;
            string RType = "";
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                int errcount = 0;
                DateTime lockdate = DateTime.Now;
                
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select updatetime,errcount,lockdate from U_UserP where UserCode='" + username + "'");
                if (dt.Count > 0)
                {
                    lockdate = dt[0]["lockdate"].GetSafeDate(DateTime.Now);
                    errcount = dt[0]["errcount"].GetSafeInt(0);
                }
                else
                {
                    CommonService.Execsql("INSERT INTO [U_UserP]([UserCode],[Updatetime])VALUES('" + username + "',getdate())");
                }

                if (errcount > 5 && lockdate.AddHours(1) >= DateTime.Now.Date)
                {
                    ret = false;
                    err = " 当前账户今天错误登陆超过5次，已经锁定，请一小时以后再试！";
                }
                else
                    ret = Remote.UserService.Login(username, password, out err);
                // 登录成功
                if (ret)
                {
                    //字符统计
                    int iNum = 0, iBtt = 0, iLtt = 0, iSym = 0;
                    foreach (char c in password)
                    {
                        if (c >= '0' && c <= '9')
                            iNum = 1;
                        else if (c >= 'a' && c <= 'z')
                            iLtt = 1;
                        else if (c >= 'A' && c <= 'Z')
                            iBtt = 1;
                        else
                            iSym = 1;
                    }

                    if (password.Length < 8 || (iNum + iLtt + iSym + iBtt) <= 2)
                    {
                        //pmsg = "-1";
                        //Remote.UserService.Logout();
                        ret = false;
                        err = "密码的长度小于8位或者复杂度小于3种！";
                        System.Web.HttpContext.Current.Session.Abandon();
                        System.Web.Security.FormsAuthentication.SignOut();
                    }
                    else
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
                        // 设置用户桌面项
                        bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                        if (!status)
                            SysLog4.WriteLog(err);

                        if (CurrentUser.CompanyCode == "CPCOPrePwz5dd4")
                        {
                            CurrentUser.CurUser.UrlJumpType = "N";
                        }
                        else
                        {
                            // 获取页面跳转类型
                            dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                            if (dt.Count > 0)
                                CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                            else
                                CurrentUser.CurUser.UrlJumpType = "R";
                        }
                        CommonService.Execsql("update [U_UserP] set errcount=0  where [UserCode]='" + username + "'");
                    }
                }
                else
                {
                    errcount = errcount + 1;
                    CommonService.Execsql("update [U_UserP] set errcount=" + errcount.ToString() + " ,lockdate=getdate() where [UserCode]='" + username + "'");
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
                LogService.SaveLog(log);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {

                if (CurrentUser.CompanyCode == "CPCOPrePwz5dd4")
                    RType = "N";
                else
                    RType = "Q";

                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("code", ret ? "0" : "1");
                if (err == "" && ret)
                    row.Add("msg", CurrentUser.RealName);
                else
                    row.Add("msg", err);
                row.Add("type", RType);
                err = jss.Serialize(row);


                Response.ContentType = "text/plain";



                Response.Write(err);
                Response.End();
            }
        }
        #endregion

        #region 登录操作
        public void DoLogin1()
        {
            string err = "";
            bool ret = false;
            string RType = "";
            try
            {
                string username = Request["loginName"].GetSafeString();
                string password = Request["loginPwd"].GetSafeString();
                //string realname = "";
                //IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select qymc from I_M_QY where ZH='" + username + "'");
                //if (dt.Count > 0)
                //{
                //    realname = dt[0]["qymc"];
                //}
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
                    // 设置用户桌面项
                    bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out err);
                    if (!status)
                        SysLog4.WriteLog(err);

                    if (CurrentUser.CompanyCode == "CPCOPrePwz5dd4")
                    {
                        CurrentUser.CurUser.UrlJumpType = "N";
                    }
                    else
                    {
                        // 获取页面跳转类型
                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                        if (dt.Count > 0)
                            CurrentUser.CurUser.UrlJumpType = dt[0]["zhlx"];
                        else
                            CurrentUser.CurUser.UrlJumpType = "R";
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
                LogService.SaveLog(log);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {             
                if (CurrentUser.CompanyCode == "CPCOPrePwz5dd4")
                    RType = "N";
                else
                    RType = "Q";

                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("code", ret ? "0" : "1");
                if (err == "" && ret)
                    row.Add("msg", CurrentUser.RealName);
                else
                    row.Add("msg", err);
                row.Add("type", RType);
                err = jss.Serialize(row);


                Response.ContentType = "text/plain";



                Response.Write(err);
                Response.End();
            }
        }
        #endregion

        #region 手机端获取用户名称
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
                                   where a.MenuCode.Equals("XFYS_QY14")
                                   select a).ToList();
                    if (canedit.Count > 0)
                        msg += ",gcsb";
                    menus = CurrentUser.Menus;
                    //二维码登记
                    var jf = (from a in menus
                              where a.MenuCode.Equals("GCZL_RYJF")
                              select a).ToList();
                    if (jf.Count > 0)
                        msg += ",jf";
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
        #endregion

        #region 获取公告栏列表
        public void getGGLList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from View_I_S_GC_XFYS_GGL";
                sql += " order by xssx asc";
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
        #endregion

        #region 获取法律法规列表
        public void getFLFGList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from View_I_S_GC_XFYS_FLFG";
                sql += " order by xssx asc";
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
        #endregion

        #region 获取消防知识列表
        public void getXFZSList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from View_I_S_GC_XFYS_XFZS";
                sql += " order by xssx asc";
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
        #endregion

        #region 获取消防知识列表
        public void getBGXZList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from View_I_S_GC_XFYS_BGXZ";
                sql += " order by xssx asc";
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
        #endregion

        #region 获取法律法规列表
        public void getJSBZList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from View_I_S_GC_XFYS_JSBZ";
                sql += " order by xssx asc";
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
        #endregion

        #region 企业资质审核
        [Authorize]
        public void CheckQyzz()
        {
            bool code = false;
            string msg = "";
            try
            {
                string zzbh = Request["zzbh"].GetSafeString();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                if (checkoption == 0)
                {
                    sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=0,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',thyy='" + reason + "' where zzbh in (" + zzbh.FormatSQLInStr() + ")");
                }
                else
                {
                    string sql = "select zh from i_m_qy where qybh in (select qybh from i_s_qy_qyzz where zzbh in (" + zzbh.FormatSQLInStr() + "))";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0 || dt[0]["zh"] == "")
                        msg = "获取企业账号失败";
                    else
                    {
                        string username = dt[0]["zh"];
                        sql = "select zhjsbh,zhdwbh,zhbmbh from h_qylx where lxbh in (select qylxbh from i_s_qy_qyzz where zzbh in (" + zzbh.FormatSQLInStr() + "))";
                        dt = CommonService.GetDataTable(sql);
                        // 需要创建角色
                        if (dt.Count > 0 && dt[0]["zhjsbh"] != "")
                        {
                            string roleid = dt[0]["zhjsbh"];
                            if (UserService.AddUserRole(username, roleid, out msg))
                            {
                                msg = "";

                            }
                            else
                                msg = "创建企业角色失败!";
                            string cpcode = dt[0]["zhdwbh"];
                            string depcode = dt[0]["zhbmbh"];
                            //if (msg ==""&& cpcode != "" && depcode != "") //更改用户单位部门(商品砼企业)
                            //{
                            //    if(UserService.ChangeCompanyDepByUsercodeAndCpcodeDepcode(username,cpcode,depcode,out msg))
                            //    {
                            //        msg = "";
                            //    }

                            //}

                            RemoteUserService.InitVars();
                            BD.DataInputBll.UserSystemRemoteService.ClearWebServiceData();
                            BD.WebListBll.UserSystemRemoteService.ClearWebServiceData();
                            BD.Jcbg.Web.Remote.UserService.m_Users = null;

                        }
                        if (msg == "")
                            sqls.Add("update i_s_qy_qyzz set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where zzbh in (" + zzbh.FormatSQLInStr() + ")");
                    }
                }
                string sqlstr = "select zzbh,qybh,qylxbh from i_s_qy_qyzz where zzbh in (" + zzbh.FormatSQLInStr() + ")";
                IList<IDictionary<string, string>> qyxxs = CommonService.GetDataTable(sqlstr);
                foreach (IDictionary<string, string> qyxx in qyxxs)
                {
                    if (qyxx["qylxbh"].GetSafeString() == "01")
                    {
                        sqls.Add("delete from i_s_qy_qyzz where qylxbh<>'01' and qybh='" + qyxx["qybh"].GetSafeString() + "'");
                    }
                    string url = "qybh=" + qyxx["qybh"].GetSafeString() + "&zzbh=" + qyxx["zzbh"].GetSafeString() + "";
                    sqls.Add("update CompanyReader set hasreader=1,readertime=getdate() where parentid in (select recid from usermail where dbo.FuncBase64Decode(content) like '%" + url + "%' and title='资质审批')");
                }
                if (msg == "")
                    code = CommonService.ExecTrans(sqls);


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 人员资质审核
        [Authorize]
        public void CheckRyzz()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybhs = Request["rybhs"].GetSafeString();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                if (checkoption == 0)
                {
                    sqls.Add("update i_m_ry set sptg=1,sfyx=0,sbsp=0,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',REMARK='" + reason + "' where rybh in (" + rybhs.FormatSQLInStr() + ")");
                }
                else
                {
                    if (msg == "")
                    {
                        sqls.Add("update i_s_ry_ryzz set sptg=1,sfyx=1 where rybh in (" + rybhs.FormatSQLInStr() + ")");
                        sqls.Add("update i_m_ry set sptg=1,sfyx=1,spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "' where rybh in (" + rybhs.FormatSQLInStr() + ")");
                    }
                }
                string sqlstr = "select a.rybh,a.ryxm,b.qybh,b.qymc from i_m_ry a left join i_m_qy as b on a.qybh=b.qybh where a.rybh in (" + rybhs.FormatSQLInStr() + ")";
                IList<IDictionary<string, string>> ryxxs = CommonService.GetDataTable(sqlstr);
                foreach (IDictionary<string, string> ryxx in ryxxs)
                {
                    string url = "rybh=" + ryxx["rybh"].GetSafeString() + "";
                    string sj = "";
                    if (checkoption == 0)
                        sj = CurrentUser.RealName + "对" + ryxx["qymc"].GetSafeString() + "的" + ryxx["ryxm"].GetSafeString() + "进行了驳回！";
                    else
                        sj = CurrentUser.RealName + "对" + ryxx["qymc"].GetSafeString() + "的" + ryxx["ryxm"].GetSafeString() + "审核通过！"; ;
                    sqls.Add("INSERT INTO I_S_GC_XFYS_JL_RECORD(SJ, RYBH, QYBH, LX, LRSJ, LRRZH, LRRXM, SY)VALUES('" + sj + "','" + ryxx["rybh"].GetSafeString() + "','" + ryxx["qybh"].GetSafeString() + "','人员信息审核',getdate(),'" + CurrentUser.UserName + "','" + CurrentUser.RealName + "','" + reason + "')");
                    sqls.Add("update CompanyReader set hasreader=1,readertime=getdate() where parentid in (select recid from usermail where dbo.FuncBase64Decode(content) like '%" + url + "%' and title='人员信息审核')");
                }
                if (msg == "")
                    code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 人员提交审核
        [Authorize]
        public void SubmitRyzz()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();

                IList<string> sqls = new List<string>();
                sqls.Add("update i_m_ry set sptg=0,sfyx=0,sbsp=1,remark=''  where rybh in (" + rybh.FormatSQLInStr() + ")");

                string sqlstr = "select a.rybh,b.qymc from i_m_ry a left join i_m_qy as b on a.qybh=b.qybh where a.rybh in (" + rybh.FormatSQLInStr() + ")";
                IList<IDictionary<string, string>> ryxxs = CommonService.GetDataTable(sqlstr);
                foreach (IDictionary<string, string> ryxx in ryxxs)
                {
                    string url = "rybh=" + ryxx["rybh"].GetSafeString() + "";
                    string qymc = ryxx["qymc"].GetSafeString();
                    string sql = "select * from companyreader where parentid in (select recid from usermail where dbo.FuncBase64Decode(content) like  '%" + url + "%' and title='人员信息审核') and hasreader=0";
                    IList<IDictionary<string, string>> sl = CommonService.GetDataTable(sql);
                    if (sl.Count == 0)
                    {
                        IList<IDictionary<string, string>> rys = CommonService.GetDataTable("select * from h_fsyjry where isfs=1");
                        foreach (IDictionary<string, string> ry in rys)
                        {
                            string mailcontent = "CONVERT(nvarchar(max), GETDATE(), 23) + ','+'" + qymc + "'+'已重新提交人员信息，请前往审核！' + '<a href=\"/wlxf/rysh?rybh=" + ryxx["rybh"].GetSafeString() + "\">点击审核</a>'";
                            sqls.Add("INSERT INTO UserMail(Sender,SenderRealName,Receiver,ReceiverRealName,FolderID, Title,[Content], SendTime, ContentSize, HasSend, HasDelete, FileIds) VALUES('', '[系统]', '" + ry["ryzh"].GetSafeString() + "', '" + ry["ryxm"].GetSafeString() + "',1, '人员信息审核', dbo.FuncBase64Encode(" + mailcontent + "), getdate(), len(dbo.FuncBase64Encode(" + mailcontent + ")), 1, 0, '')");
                            sqls.Add("INSERT INTO CompanyReader(ParentEntity,ParentId,UserName,RealName,HasReader, ReaderTime, HasDelete)VALUES('UserMail',ident_current('UserMail'), '" + ry["ryzh"].GetSafeString() + "', '" + ry["ryxm"].GetSafeString() + "', 0, null, 0)");
                        }
                    }
                }

                code = CommonService.ExecTrans(sqls);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 删除企业，企业资质
        [Authorize]
        public void DeleteQy()
        {
            bool code = true;
            string msg = "";
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_M_QY where qybh='" + qybh + "'");
                sqls.Add("delete from I_S_QY_QYZZ where qybh='" + qybh + "' ");
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

        #region 政务网用户单点登录校验
        public ActionResult zc()
        {
            bool success = true;
            string msg = "";
            try
            {

                //Remote.UserService.Logout();
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
                    msg = retdata["msg"].GetSafeString();
                    if (errorCode == "0")
                    {
                        // 企业（法人）信息
                        Dictionary<string, object> companyInfo = (Dictionary<string, object>)retdata["info"];
                        if (companyInfo != null)
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
                                string oldqybh = ddt[0]["qybh"].GetSafeString();
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
                                else
                                {
                                    string sql = "", qybh = "", username = "", realname = "", postcode = "";
                                    IList<IDictionary<string, string>> dt = null;
                                    sql = "select * from i_m_qy where qymc='" + qymc + "' and qybh in (select qybh from i_m_qyzh)";
                                    dt = CommonService.GetDataTable(sql);
                                    if (dt.Count != 0)
                                    {
                                        msg = "请补全温岭市建设工程消防审验管理系统的组织机构代码";
                                    }
                                    else
                                    {
                                        //若无账号密码给用户生成账号密码
                                        do
                                        {
                                            bool code = false;
                                            // 查找企业类型信息，获取默认单位、部门、角色                         
                                            sql = "select * from h_qylx where lxbh='13'";
                                            dt = CommonService.GetDataTable(sql);
                                            if (dt.Count == 0)
                                            {
                                                msg = "找不到企业类型记录";
                                                break;
                                            }
                                            // 不用创建账号，返回
                                            if (!dt[0]["sfcjzh"].GetSafeBool())
                                            {
                                                msg = "不用创建账号";
                                                break;
                                            }
                                            string companycode = dt[0]["zhdwbh"];
                                            string depcode = dt[0]["zhbmbh"];
                                            string rolecode = dt[0]["zhjsbh"];
                                            postcode = dt[0]["gwbh"];
                                            if (uniscid != "")
                                                username = uniscid;
                                            else username = zzjgdm;
                                            realname = qymc;

                                            //string password = RandomNumber.GetNew(RandomType.Number, GlobalVariable.GetUserPasswordLength());
                                            string password = "88888";
                                            code = UserService.AddUser(companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                                            if (!code)
                                            {
                                                msg = "生成用户账号失败！";
                                                break;
                                            }    
                                            string yhzh = msg;

                                            sql = "select right(replicate('0',6)+Convert(nvarchar,substring(zdbh,2,6)+1),6)  as qybh from PR_M_BHMS where recid=14";
                                            dt = CommonService.GetDataTable(sql);
                                            if (dt.Count > 0)
                                                qybh = "Q" + dt[0]["qybh"].ToString();
                                            //更新编号表企业编号
                                            IList<string> sqls = new List<string>();
                                            sql = "update PR_M_BHMS set zdbh='" + qybh + "' where recid=14";
                                            sqls.Add(sql);
                                            //删除旧的企业数据、更新企业资质
                                            if (oldqybh != "")
                                            {
                                                sql = "delete from i_m_qy where qybh='" + oldqybh + "'";
                                                sqls.Add(sql);
                                                sql = "update i_s_qy_qyzz set qybh='" + qybh + "' where qybh='" + oldqybh + "'";
                                                sqls.Add(sql);
                                            }
                                            //企业账号
                                            sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj,zhlx) values('" + qybh + "','" + yhzh + "',1,'','',getdate(),'Q')";
                                            sqls.Add(sql);
                                            //企业信息
                                            sql = "insert into i_m_qy(lxbh,qybh,qymc,lrsj,sptg,sfyx,zh,zzjgdm,aqscxkzyxq,zwwuserid) values('13','" + qybh + "','" + qymc + "',getdate(),'1','1','" + username + "','" + username + "','2099-01-01','" + userId + "')";
                                            sqls.Add(sql);
                                            //企业资质
                                            string zzbh = "";
                                            sql = "select right(replicate('0',6)+Convert(nvarchar,substring(zdbh,2,6)+1),6)  as zzbh from PR_M_BHMS where recid=20";
                                            dt = CommonService.GetDataTable(sql);
                                            if (dt.Count > 0)
                                                zzbh = dt[0]["zzbh"].ToString();
                                            sql = "update PR_M_BHMS set zdbh='" + zzbh + "' where recid=20";
                                            //插入企业资质
                                            sqls.Add(sql);
                                            sql = "insert into I_S_QY_QYZZ(ZZBH, QYBH, ZZMC, FZJG, YXQS, YXQZ, QYLXBH, ZZX, ZZLXBH, ZZDJBH, ZZNRBH, SFLS, XZNR, SPTG, SFYX, ZJZBH) values('" + zzbh + "', '" + qybh + "', '/', '', '1900-01-01', '2099-01-01', '13', '主项', '001', '001', '001', '非临时', '/', 1, 1, 'CP201611000001')";
                                            sqls.Add(sql);
                                            code = CommonService.ExecTrans(sqls, out msg);
                                            try
                                            {
                                                RemoteUserService.InitVars();
                                                BD.DataInputBll.UserSystemRemoteService.ClearWebServiceData();
                                                BD.WebListBll.UserSystemRemoteService.ClearWebServiceData();
                                                BD.Jcbg.Web.Remote.UserService.m_Users = null;
                                            }
                                            catch (Exception e)
                                            {
                                                SysLog4.WriteLog(e);
                                                msg = e.Message;
                                            }
                                            if (code)
                                            {
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

                                                    CurrentUser.SetSession("DEPCODE", CurrentUser.CurUser.DepartmentId);
                                                    // 设置用户桌面项
                                                    bool status = SystemService.InitUserDesktopItem(CurrentUser.UserName, CurrentUser.UserRights, out msg);
                                                    if (!status)
                                                        SysLog4.WriteLog(msg);

                                                    // 获取页面跳转类型
                                                    IList<IDictionary<string, string>> data = CommonService.GetDataTable("select zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'");
                                                    if (data.Count > 0)
                                                        CurrentUser.CurUser.UrlJumpType = data[0]["zhlx"];
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
                                            success = true;
                                        } while (false);
                                    }
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
                        msg = "用户认证失败：" + msg;
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);

            }
            if (success)
            {
                return new RedirectResult("/user/mainyh");
            }
            else
            {
                ViewBag.error = msg;
                return View();
            }
        }
        #endregion

        #region 获取数据统计信息

        #region 各申报消防类型数量
        [Authorize]
        public void getGCXXList()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string sqsj1 = Request["sqsj1"].GetSafeString();
            string sqsj2 = Request["sqsj2"].GetSafeString();
            string slsj1 = Request["slsj1"].GetSafeString();
            string slsj2 = Request["slsj2"].GetSafeString();
            string qfsj1 = Request["qfsj1"].GetSafeString();
            string qfsj2 = Request["qfsj2"].GetSafeString();
            string bjsj1 = Request["bjsj1"].GetSafeString();
            string bjsj2 = Request["bjsj2"].GetSafeString();
            string zxsyxz1 = Request["zxsyxz1"].GetSafeString();
            string zxsyxz2 = Request["zxsyxz2"].GetSafeString();
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string where = "";
                string strwhere = "";
                string syxzwhere = "";
                string sql = "";
                if (sqsj1 != "")
                    where += "and c.lrsj>='" + sqsj1.GetSafeDate() + "' ";
                if (sqsj2 != "")
                    where += "and c.lrsj<=DATEADD(SECOND,-1,DATEADD(DAY,1,'" + sqsj2.GetSafeDate() + "')) ";
                if (slsj1 != "")
                    where += "and c.ckcssj>='" + slsj1.GetSafeDate() + "' ";
                if (slsj2 != "")
                    where += "and c.ckcssj<=DATEADD(SECOND,-1,DATEADD(DAY,1,'" + slsj2.GetSafeDate() + "')) ";
                if (slsj1 != "" || slsj2 != "")
                    where += "and (select count(1) from I_S_GC_XFYS_ALERT where serialno = c.serialno)> 0 ";
                if (qfsj1 != "")
                    where += "and c.ysqfsj>='" + qfsj1.GetSafeDate() + "' ";
                if (qfsj2 != "")
                    where += "and c.ysqfsj<=DATEADD(SECOND,-1,DATEADD(DAY,1,'" + qfsj2.GetSafeDate() + "')) ";
                if (bjsj1 != "")
                    where += "and c.dysj>='" + bjsj1.GetSafeDate() + "' ";
                if (bjsj2 != "")
                    where += "and c.dysj<=DATEADD(SECOND,-1,DATEADD(DAY,1,'" + bjsj2.GetSafeDate() + "')) ";
                if (where != "")
                    strwhere = where;
                if (zxsyxz1 != "" && zxsyxz1 != "1")
                {
                    syxzwhere += " and isnull(a.zxsyxz1,'')= '" + zxsyxz1 + "' ";
                    where += " and isnull(a.zxsyxz1,'')= '" + zxsyxz1 + "' ";
                }
                if (zxsyxz2 != "" && zxsyxz2 != "1")
                {
                    syxzwhere += " and isnull(a.zxsyxz2,'')= '" + zxsyxz2 + "' ";
                    where += " and isnull(a.zxsyxz2,'')= '" + zxsyxz2 + "' ";
                }
               

                if (where == "")
                    sql = "select (select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and c.isselect=1) as czcs,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and c.isselect=0) as wczcs,isnull(count(1),0) as zgcsl,Convert(decimal(18,4),(select isnull(sum(isnull(zjzmj,0)),0)/10000 from I_M_GC_XFYS where zxgc='否'))+Convert(decimal(18,4),(select isnull(sum(Convert(decimal(18,4),case when zxmj='' then '0' else zxmj end)),0)/10000 from I_M_GC_XFYS where zxgc='是')) as zjzmj, Convert(decimal(18,4),isnull(sum(isnull(a.gctze,0)),0)/10000) as zgctze,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3) as sbsxsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='01') as sjscsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='02') as xfyssl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03') as xfbasl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='04') as gsqysl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05') as xfbafcsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and c.ishg='该工程符合建设工程消防验收有关规定') as xfbafchgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and c.ishg='该工程不符合建设工程消防验收有关规定') as xfbafcbhgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06') as xfysfcsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and c.ishg='该工程消防验收合格') as xfysfchgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and c.ishg='该工程消防验收不合格') as xfysfcbhgsl,(select count(1) from I_S_GC_XFYS where typebh='02' and ishg='该工程消防验收合格' and zt='BAQR') as xfyshg,(select count(1) from I_S_GC_XFYS where typebh='02' and ishg='该工程消防验收不合格' and zt='BAQR') as xfysbhg,(select count(1) from I_S_GC_XFYS where typebh='03' and ishg='该工程符合建设工程消防验收有关规定' and zt='BAQR') as xfbahg,(select count(1) from I_S_GC_XFYS where typebh='03' and ishg='该工程不符合建设工程消防验收有关规定' and zt='BAQR') as xfbabhg from I_M_GC_XFYS a";
                else
                    sql = "select (select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and  c.isselect=1 and 1=1 " + where + ") as czcs,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and c.isselect=0 and 1=1 " + where + ") as wczcs,isnull(count(1),0) as zgcsl,Convert(decimal(18,4),(select isnull(sum(isnull(a.zjzmj,0)),0)/10000 from I_M_GC_XFYS a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid where a.zxgc='否' " + where + "))+Convert(decimal(18,4),(select isnull(sum(Convert(decimal(18,4),case when zxmj='' then '0' else zxmj end)),0)/10000 from I_M_GC_XFYS a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid where a.zxgc='是' " + where + ")) as zjzmj, Convert(decimal(18,4),isnull(sum(isnull(a.gctze,0)),0)/10000) as zgctze,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and 1=1 " + where + ") as sbsxsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='01' and 1=1 " + where + ") as sjscsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='02' and 1=1 " + where + ") as xfyssl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and 1=1 " + where + ") as xfbasl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='04' and 1=1 " + where + ") as gsqysl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and 1=1 " + where + ") as xfbafcsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and c.ishg='该工程符合建设工程消防验收有关规定' " + where + ") as xfbafchgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and c.ishg='该工程不符合建设工程消防验收有关规定' " + where + ") as xfbafcbhgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and 1=1 " + where + ") as xfysfcsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and c.ishg='该工程消防验收合格' " + where + ") as xfysfchgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and c.ishg='该工程消防验收不合格' " + where + ") as xfysfcbhgsl,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='02' and c.ishg='该工程消防验收合格' and c.zt='BAQR' " + where + ") as xfyshg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='02' and c.ishg='该工程消防验收不合格' and c.zt='BAQR' " + where + ") as xfysbhg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and c.ishg='该工程符合建设工程消防验收有关规定' and c.zt='BAQR' " + where + ") as xfbahg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='03' and c.ishg='该工程不符合建设工程消防验收有关规定' and c.zt='BAQR' " + where + ") as xfbabhg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and c.ishg='该工程消防验收合格' and c.zt='BAQR' " + where + ") as xfysfyhg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='06' and c.ishg='该工程消防验收不合格' and c.zt='BAQR' " + where + ") as xfysfybhg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and c.ishg='该工程符合建设工程消防验收有关规定' and c.zt='BAQR' " + where + ") as xfbafchg,(select isnull(count(1),0) from i_m_gc_xfys a left join I_S_GC_XFYS c on a.fxbaid=c.fxbaid left join STForm as b on c.SerialNo=b.SerialNo where b.dostate<>3 and c.typebh='05' and c.ishg='该工程不符合建设工程消防验收有关规定' and c.zt='BAQR' " + where + ") as xfbafcbhg from I_M_GC_XFYS a where a.fxbaid in (select fxbaid from i_s_gc_xfys c where 1=1 " + strwhere + ") " + syxzwhere + "";
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
        #endregion

        #region 获取装修工程信息
        public void getZxgcList()
        {
            decimal zxmj1 = Request["zxmj1"].GetSafeDecimal();
            decimal zxmj2 = Request["zxmj2"].GetSafeDecimal();
            string l_typename = Request["l_typename"].GetSafeString();
            string qfsj1 = Request["qfsj1"].GetSafeString();
            string qfsj2 = Request["qfsj2"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(100);
            //string syxz = Request["syxz"].GetSafeString();
            string zxsyxz1 = Request["zxsyxz1"].GetSafeString();
            string zxsyxz2 = Request["zxsyxz2"].GetSafeString();
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> sqls = new Dictionary<string, object>();
            try
            {
                string where = "";
                string sql = "";
                string strsql = "";
                if (zxmj1 != 0)
                    where += " and cast(a.zxmj as decimal)>=" + zxmj1 + "";
                if (zxmj2 != 0)
                    where += " and cast(a.zxmj as decimal)<=" + zxmj2 + "";
                if (zxsyxz1 != "" && zxsyxz1 != "1") 
                    where += " and isnull(a.zxsyxz1,'')= '" + zxsyxz1 + "' ";
                if (zxsyxz2 != "" && zxsyxz2 != "1")
                    where += " and isnull(a.zxsyxz2,'')= '" + zxsyxz2 + "' ";
                if (l_typename != "")
                {
                    if (l_typename == "消防验收")
                    {
                        where += " and a.fxbaid in (select fxbaid from i_s_gc_xfys where typebh in('02') and zt in('CQQR','BAQR')";
                    }
                    else
                    {
                        where += " and a.fxbaid in (select fxbaid from i_s_gc_xfys where typebh in('03') and zt in('CQQR','BAQR')";
                    }
                    if (qfsj1 != "")
                        where += " and dysj>='" + qfsj1.GetSafeDate() + "'";
                    if (qfsj2 != "")
                        where += " and dysj<='" + qfsj2.GetSafeDate() + "'";
                    where += ")";
                }
                else
                {
                    where += " and a.fxbaid in (select fxbaid from i_s_gc_xfys where typebh in('02','03') and zt in('CQQR','BAQR')";
                    if (qfsj1 != "")
                        where += " and dysj>='" + qfsj1.GetSafeDate() + "'";
                    if (qfsj2 != "")
                        where += " and dysj<='" + qfsj2.GetSafeDate() + "'";
                    where += ")";
                }
                //if (where == "")
                //{
                sql = "select count(*) as zxsl from I_M_GC_XFYS a where a.zxgc='是' and a.fxbaid in (select fxbaid from i_s_gc_xfys where typebh in('02','03') and zt in('CQQR','BAQR')) " + where + "";
                strsql = "select *,(case when (select count(1) from i_s_gc_xfys where typebh in('02') and zt in('CQQR','BAQR') and fxbaid=a.fxbaid)>0 and (select count(1) from i_s_gc_xfys where typebh in('03') and zt in('CQQR','BAQR') and fxbaid=a.fxbaid)>0 then '消防验收,消防备案' when (select count(1) from i_s_gc_xfys where typebh in('02') and zt in('CQQR','BAQR') and fxbaid=a.fxbaid)>0 then '消防验收' else '消防备案' end) as l_typename,(case when (select count(1) from i_s_gc_xfys where typebh in('03') and zt in('CQQR','BAQR') and fxbaid=a.fxbaid and isselect=1)>0  then '抽中' when (select count(1) from i_s_gc_xfys where typebh in('03') and zt in('CQQR','BAQR') and fxbaid=a.fxbaid and isselect=0)>0 then '未抽中' else '\\' end) as iscz from I_M_GC_XFYS a where a.zxgc='是' " + where + "";
                //}
                //else
                //{
                //    sql = "select count(*) as zxsl from I_M_GC_XFYS a where a.zxgc='是' and a.fxbaid in (select fxbaid from i_s_gc_xfys where typebh in('02','03') and zt in('CQQR','BAQR')) " + where + "";
                //    strsql = "select *,(case when (select count(1) from i_s_gc_xfys where typebh in('02') and zt in('CQQR','BAQR') and fxbaid=a.fxbaid)>0 then '消防验收' else '消防备案' end) as typename  from I_M_GC_XFYS a where a.zxgc='是' and a.fxbaid in (select fxbaid from i_s_gc_xfys where typebh in('02','03') and zt in('CQQR','BAQR')) " + where + "";
                //}
                datas = CommonService.GetDataTable(sql);
                sqls.Add("zxsl", datas);
                datas = CommonService.GetPageData(strsql, pagesize, pageindex, out totalcount);
                sqls.Add("gcxx", datas);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(sqls)));
                Response.End();
            }
        }
        #endregion

        #region 获取工程坐标详情
        public void GetGczbXq()
        {
            string err = "";
            string gcmc = Request["gcmc"].GetSafeString();
            string bjsj1 = Request["bjsj1"].GetSafeString();
            string bjsj2 = Request["bjsj2"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            //string syxz = Request["syxz"].GetSafeString();
            string zxsyxz1 = Request["zxsyxz1"].GetSafeString();
            string zxsyxz2 = Request["zxsyxz2"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> gcslzbxx = new Dictionary<string, object>();
            IDictionary<string, object> gcsljdxx = new Dictionary<string, object>();
            IDictionary<string, object> gcxq = new Dictionary<string, object>();
            IList<object> gcxxlist = new List<object>();
            try
            {
                string sql = "";
                string strwhere = "";
                string strwhere1 = "";
                string strwhere2 = "";
                if (gcmc != "")
                {
                    strwhere += " and a.gcmc like '%" + gcmc + "%'";
                    strwhere1 = " and a.gcmc like '%" + gcmc + "%'";
                }
                if (zxsyxz1 != "" && zxsyxz1 != "1")
                    strwhere1 += " and isnull(a.zxsyxz1,'')= '" + zxsyxz1 + "' ";
                if (zxsyxz2 != "" && zxsyxz2 != "1")
                    strwhere1 += " and isnull(a.zxsyxz2,'')= '" + zxsyxz2 + "' ";
                if (bjsj1 != "")
                {
                    strwhere += " and b.dysj>='" + bjsj1.GetSafeDate() + "'";
                    strwhere2 += " and b.dysj>='" + bjsj1.GetSafeDate() + "'";
                }
                if (bjsj2 != "")
                {
                    strwhere += " and b.dysj<DATEADD(SECOND,-1,DATEADD(DAY,1,'" + bjsj2.GetSafeDate() + "'))";
                    strwhere2 += " and b.dysj<DATEADD(SECOND,-1,DATEADD(DAY,1,'" + bjsj2.GetSafeDate() + "'))";
                }
                if (typebh != "")
                {
                    strwhere += " and b.typebh='" + typebh.GetSafeString() + "'";
                    strwhere2 += " and b.typebh='" + typebh.GetSafeString() + "'";
                }
                if (strwhere1 != "" || strwhere2 != "")
                    sql = "select count(1) as sl from i_m_gc_xfys a where a.fxbaid in (select b.fxbaid from i_s_gc_xfys b where b.zt in('CKSJ','CQQR','BAQR')" + strwhere2 + ") " + strwhere1 + "";
                else
                    sql = "select count(1) as sl from i_m_gc_xfys a where 1=1 " + strwhere1 + "";
                datas = CommonService.GetDataTable(sql);
                int gcsl = datas[0]["sl"].GetSafeInt();
                gcslzbxx.Add("gcsl", gcsl);
                if (strwhere1 != "" || strwhere2 != "")
                    sql = "select count(1) as sl from i_m_gc_xfys a where isnull(a.gczb,'')<>'' and a.fxbaid in (select b.fxbaid from i_s_gc_xfys b where b.zt in('CKSJ','CQQR','BAQR')" + strwhere2 + ") " + strwhere1 + "";
                else
                    sql = "select count(1) as sl from i_m_gc_xfys a where isnull(a.gczb,'')<>'' " + strwhere1 + "";
                datas = CommonService.GetDataTable(sql);
                int gczbsl = datas[0]["sl"].GetSafeInt();
                gcslzbxx.Add("gczbsl", gczbsl);
                gcslzbxx.Add("gcwbzsl", gcsl - gczbsl);
                gcxxlist.Add(gcslzbxx);

                //string[] arrjdbh = new string[] { "tpjd", "cdjd", "cxjd", "cbjd", "hfjd", "zgz", "dxz", "smz", "rhz", "xhz", "stz", "bhz", "wqz", "cnz", "sqtz", "wgz", "dbxc" };
                //string[] arrjdmc = new string[] { "太平街道", "城东街道", "城西街道", "城北街道", "横峰街道", "泽国镇", "大溪镇", "松门镇", "箬横镇", "新河镇", "石塘镇", "滨海镇", "温峤镇", "城南镇", "石桥头镇", "坞根镇", "东部新区" };

                sql = "select count(case when a.szjd='太平街道' then 1 end) as tpjdsl,count(case when a.szjd='城东街道' then 1 end) as cdjdsl,count(case when a.szjd='城西街道' then 1 end) as cxjdsl,count(case when a.szjd='城北街道' then 1 end) as cbjdsl,count(case when a.szjd='横峰街道' then 1 end) as hfjdsl,count(case when a.szjd='泽国镇' then 1 end) as zgzsl";
                sql += ",count(case when a.szjd='大溪镇' then 1 end) as dxzsl,count(case when a.szjd='松门镇' then 1 end) as smzsl,count(case when a.szjd='箬横镇' then 1 end) as rhzsl,count(case when a.szjd='新河镇' then 1 end) as xhzsl,count(case when a.szjd='石塘镇' then 1 end) as stzsl,count(case when a.szjd='滨海镇' then 1 end) as bhzsl";
                sql += ",count(case when a.szjd='温峤镇' then 1 end) as wqzsl,count(case when a.szjd='城南镇' then 1 end) as cnzsl,count(case when a.szjd='石桥头镇' then 1 end) as sqtzsl,count(case when a.szjd='坞根镇' then 1 end) as wgzsl,count(case when a.szjd='东部新区' then 1 end) as dbxcsl";
                sql += " from i_m_gc_xfys a left join i_s_gc_xfys as b on a.fxbaid=b.fxbaid where b.zt in('CKSJ','CQQR','BAQR') " + strwhere1 + "";
                datas = CommonService.GetDataTable(sql + strwhere);
                gcsljdxx.Add("jdxq", datas);
                gcxxlist.Add(gcsljdxx);
                sql = "select a.recid,a.fxbaid,a.gcmc,a.gczb from i_m_gc_xfys a where isnull(a.gczb,'')<>'' and a.fxbaid in (select fxbaid from i_s_gc_xfys b where b.zt in('CKSJ','CQQR','BAQR') " + strwhere2 + ") " + strwhere1 + "";
                datas = CommonService.GetDataTable(sql);
                gcxq.Add("gcxq", datas);
                gcxxlist.Add(gcxq);
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(gcxxlist)));
                Response.End();
            }
        }
        #endregion

        #endregion

        #region 社会统一信用代码校验
        public void searchQYZZJGDM()
        {
            string zzjgdm = Request["zzjgdm"].GetSafeRequest();
            bool code = true;
            string msg = "";
            try
            {
                if (zzjgdm == "")
                {
                    code = false;
                    msg = "组织机构代码为空";
                }
                else
                {
                    string sql = "select qymc,zh from I_M_QY where zzjgdm ='" + zzjgdm + "'";

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string qy = dt[0]["qymc"].ToString();
                        string zh = dt[0]["zh"].ToString();
                        msg = "该社会统一信用代码的企业已注册，企业名称：" + qy + ";企业账号:" + zh;
                        code = false;
                    }
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }
        #endregion

        #region 注册时校验
        public void CheckRegister()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"];
                string yzm = Request["yzm"].GetSafeString();
                string qymc = Request["qymc"].GetSafeString();
                string zzjgdm = Request["zzjgdm"].GetSafeString();
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
                        dt = CommonService.GetDataTable("select zh from I_M_QY where QYMC='" + qymc + "'");
                        if (dt.Count > 0)
                        {
                            msg = "【" + qymc + "】系统中已经存在，企业账户：" + dt[0]["zh"].GetSafeString();
                        }
                        dt = CommonService.GetDataTable("select qymc from I_M_QY where ZZJGDM='" + zzjgdm + "'");
                        if (dt.Count > 0)
                        {
                            msg = "社会信用统一代码【" + zzjgdm + "】系统中已经存在，企业名称：" + dt[0]["qymc"].GetSafeString();
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
        #endregion

        #region 更新是否已打印过回馈单
        [Authorize]
        public void UpdateIsdy()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("update I_S_GC_XFJC_XCJC set ISDY=1 where RECID=" + recid + "");
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

        #region 获取消防检测附件
        [Authorize]
        public void getXFJCFJList()
        {
            string lx = Request["lx"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(100);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IList<object> urllist = new List<object>();
            try
            {
                string sql = "";
                string fj = "";
                string zgfj = "";
                if (lx == "xfjc")
                {
                    sql = "select * from I_S_GC_XFJC_XCJC where serialno='" + serialno + "'";
                    sql += " order by lrsj asc";
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                    if (datas.Count > 0)
                        fj = datas[0]["fj"].GetSafeString();
                }
                else
                {
                    sql = "select * from I_S_GC_XFYS where serialno='" + serialno + "'";
                    sql += " order by lrsj asc";
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                    if (datas.Count > 0)
                    {
                        fj = datas[0]["zghffj"].GetSafeString();
                        zgfj = datas[0]["zghffj2"].GetSafeString();
                    }
                }

                if (fj.Length > 0)
                {
                    string[] strArr = fj.Split(new[] { "||" }, StringSplitOptions.None);
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        string[] str = strArr[i].Split(new[] { "|" }, StringSplitOptions.None);
                        if (str.Length > 0)
                            urllist.Add(new Dictionary<string, string>() {
                                        {"lx","fj" },
                                        { "name",str[0].GetSafeString()},
                                        {  "id",str[1].GetSafeString()}
                                    });

                    }
                }

                if (zgfj.Length > 0)
                {
                    string[] strArr = zgfj.Split(new[] { "|" }, StringSplitOptions.None);
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        string[] str = strArr[i].Split(new[] { "," }, StringSplitOptions.None);
                        if (str.Length > 0)
                            urllist.Add(new Dictionary<string, string>() {
                                        {"lx","zgfj" },
                                        { "name",str[1].GetSafeString()},
                                        {  "id",str[0].GetSafeString()}
                                    });

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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(urllist)));
                Response.End();
            }
        }
        #endregion

        # region  消防意见书编辑html页面
        [Authorize]
        public ActionResult ReportOfficeHtmlEdit()
        {
            //获取参数
            string serialno = Request["serialno"].GetSafeString();
            string fxbaid = Request["fxbaid"].GetSafeString();
            string ishg = Request["ishg"].GetSafeString();
            string jsdwmc = Request["jsdwmc"].GetSafeString();
            string yjsbh = Request["yjsbh"].GetSafeString();
            string yjsnr = Request["yjsnr"].GetSafeString();
            string zywt = Request["zywt"].GetSafeString();
            try
            {

                string url = "/wlxf/GetReportOffileFile?serialno=" + serialno + "&fxbaid=" + fxbaid + "&ishg=" + System.Web.HttpUtility.UrlEncode(ishg) + "&jsdwmc=" + System.Web.HttpUtility.UrlEncode(jsdwmc) + "&yjsbh=" + System.Web.HttpUtility.UrlEncode(yjsbh) + "&yjsnr=" + System.Web.HttpUtility.UrlEncode(yjsnr) + "&zywt=" + System.Web.HttpUtility.UrlEncode(zywt);

                PageOffice.WordWriter.WordDocument doc = new PageOffice.WordWriter.WordDocument();
                doc.DisableWindowSelection = false;
                doc.DisableWindowRightClick = false;

                PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
                pc.ID = "PageOfficeCtrl1";
                pc.SaveFilePage = "/wlxf/SaveReportFile?serialno=" + serialno + "&fxbaid=" + fxbaid + "&ishg=" + System.Web.HttpUtility.UrlEncode(ishg) + "&jsdwmc=" + System.Web.HttpUtility.UrlEncode(jsdwmc) + "&yjsbh=" + System.Web.HttpUtility.UrlEncode(yjsbh) + "&yjsnr=" + System.Web.HttpUtility.UrlEncode(yjsnr) + "&zywt=" + System.Web.HttpUtility.UrlEncode(zywt);
                pc.ServerPage = "/pageoffice/server.aspx";
                pc.SetWriter(doc);
                pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
                pc.Titlebar = true; //隐藏标题栏
                pc.Menubar = true; //隐藏菜单栏
                pc.OfficeToolbars = true; //隐藏Office工具栏
                pc.CustomToolbar = true; //隐藏自定义工具栏
                pc.AddCustomToolButton("保存", "SaveDocument()", 1);
                pc.AddCustomToolButton("导入文件", "OpenDocument()", 3);
                pc.AddCustomToolButton("全屏切换", "SwitchFullScreen()", 4);
                pc.AddCustomToolButton("打印", "ShowPrintDlg()", 6);

                System.Web.UI.Page page = new System.Web.UI.Page();
                PageOffice.OpenModeType openMode = PageOffice.OpenModeType.docSubmitForm;


                string tempreport = "";
                string reporthelper = "";
                if (ishg == "该工程消防验收合格")
                {
                    tempreport = "特殊建设工程消防验收意见书（合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收意见书（合格）.docx";
                }
                else
                {
                    tempreport = "特殊建设工程消防验收意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收意见书（不合格）.docx";
                }

                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;

                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "YJSBH",yjsbh},
                        { "YJSNR",yjsnr},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                    dt.Add(dic);
                    data.Add("VIEW_I_M_GC_XFYS2", dt);
                    data.Add("VIEW_I_S_GC_XFYS_FGC3", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYS_FGC3 where serialno='" + serialno + "'"));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://183.131.123.107:8083/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                Newtonsoft.Json.Linq.JArray strdata = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(jss.Serialize(d.bookmark));
                                foreach (var item in strdata)
                                {
                                    doc.OpenDataRegion(item["key"].GetSafeString()).Editing = true;
                                }
                                pc.SetWriter(doc);
                            }
                        }
                    }

                }
                pc.SetWriter(doc);
                pc.WebOpen(url, openMode, CurrentUser.UserName);
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


        /// <summary>
        /// 获取office模板文件
        /// </summary>
        [Authorize]
        public void GetReportOffileFile()
        {
            try
            {
                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string ishg = Request["ishg"].GetSafeString();
                string jsdwmc = Request["jsdwmc"].GetSafeString();
                string yjsbh = Request["yjsbh"].GetSafeString();
                string yjsnr = Request["yjsnr"].GetSafeString();
                string zywt = Request["zywt"].GetSafeString();
                string tempreport = "";
                string reporthelper = "";
                if (ishg == "该工程消防验收合格")
                {
                    tempreport = "特殊建设工程消防验收意见书（合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收意见书（合格）.docx";
                }
                else
                {
                    tempreport = "特殊建设工程消防验收意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收意见书（不合格）.docx";
                }
                byte[] fileContent = null;
                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;


                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "YJSBH",yjsbh},
                        { "YJSNR",yjsnr},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                    dt.Add(dic);
                    data.Add("VIEW_I_M_GC_XFYS2", dt);
                    data.Add("VIEW_I_S_GC_XFYS_FGC3", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYS_FGC3 where serialno='" + serialno + "'"));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://183.131.123.107:8083/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                fileContent = Convert.FromBase64String(d.file as string);
                            }
                        }
                    }

                }

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office.docx");
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
        public void SaveReportFile()
        {
            string msg = "保存成功";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {

                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string ishg = Request["ishg"].GetSafeString();
                string jsdwmc = Request["jsdwmc"].GetSafeString();
                string yjsbh = Request["yjsbh"].GetSafeString();
                string yjsnr = Request["yjsnr"].GetSafeString();
                string zywt = Request["zywt"].GetSafeString();

                IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "YJSBH",yjsbh},
                        { "YJSNR",yjsnr},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                dt.Add(dic);
                data.Add("VIEW_I_M_GC_XFYS2", dt);
                data.Add("VIEW_I_S_GC_XFYS_FGC3", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYS_FGC3 where serialno='" + serialno + "'"));

                string tempreport = "";
                string filereport = "";
                if (ishg == "该工程消防验收合格")
                {
                    filereport = "特殊建设工程消防验收意见书（合格）" + serialno + fxbaid + "_v.docx";
                    tempreport = "特殊建设工程消防验收意见书（合格）.docx";
                }
                else
                {
                    filereport = "特殊建设工程消防验收意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    tempreport = "特殊建设工程消防验收意见书（不合格）.docx";
                }



                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + filereport;
                string temppath = Server.MapPath("~\\report\\jdbg") + "\\" + tempreport;
                byte[] file = fs.FileBytes;

                System.IO.File.WriteAllBytes(filepath, file);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string result = ReportHelper.ReFormatWts(Convert.ToBase64String(System.IO.File.ReadAllBytes(temppath)), Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath)), data);
                ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                if (ret != null)
                {
                    if (ret.success)
                    {
                        Dictionary<string, object> d = ret.data as Dictionary<string, object>;
                        if (d != null)
                        {
                            file = Convert.FromBase64String(d["filestr"].GetSafeString());
                            System.IO.File.WriteAllBytes(filepath, file);
                        }

                    }
                }
                CommonService.Execsql("update I_S_GC_XFYS set isbc=1 where serialno='" + serialno + "'");
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
        #endregion

        #region  联系单编辑html页面
        [Authorize]
        public ActionResult ReportOfficeHtmlEditLxd()
        {
            //获取参数
            string serialno = Request["serialno"].GetSafeString();
            string fxbaid = Request["fxbaid"].GetSafeString();
            try
            {

                string url = "/wlxf/GetReportOffileFileLxd?serialno=" + serialno + "&fxbaid=" + fxbaid;

                PageOffice.WordWriter.WordDocument doc = new PageOffice.WordWriter.WordDocument();
                doc.DisableWindowSelection = false;
                doc.DisableWindowRightClick = false;

                PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
                pc.ID = "PageOfficeCtrl1";
                pc.SaveFilePage = "/wlxf/SaveReportFileLxd?serialno=" + serialno + "&fxbaid=" + fxbaid;
                pc.ServerPage = "/pageoffice/server.aspx";
                pc.SetWriter(doc);
                pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
                pc.Titlebar = true; //隐藏标题栏
                pc.Menubar = true; //隐藏菜单栏
                pc.OfficeToolbars = true; //隐藏Office工具栏
                pc.CustomToolbar = true; //隐藏自定义工具栏
                pc.AddCustomToolButton("保存", "SaveDocument()", 1);
                pc.AddCustomToolButton("导入文件", "OpenDocument()", 3);
                pc.AddCustomToolButton("全屏切换", "SwitchFullScreen()", 4);
                pc.AddCustomToolButton("打印", "ShowPrintDlg()", 6);

                System.Web.UI.Page page = new System.Web.UI.Page();
                PageOffice.OpenModeType openMode = PageOffice.OpenModeType.docSubmitForm;


                string tempreport = "";
                string reporthelper = "";

                tempreport = "建设工程消防验收（备案抽查）问题联系单" + serialno + fxbaid + "_v.docx";
                reporthelper = "建设工程消防验收（备案抽查）问题联系单.docx";

                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;

                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    data.Add("VIEW_I_M_GC_XFYS2", CommonService.GetDataTable2("select * from VIEW_I_M_GC_XFYS2 where serialno='" + serialno + "'"));
                    data.Add("VIEW_I_S_GC_XFYSJL", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYSJL where serialno='" + serialno + "'"));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://wlxf.jzyglxt.com/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                Newtonsoft.Json.Linq.JArray strdata = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(jss.Serialize(d.bookmark));
                                foreach (var item in strdata)
                                {
                                    doc.OpenDataRegion(item["key"].GetSafeString()).Editing = true;
                                }
                                pc.SetWriter(doc);
                            }
                        }
                    }

                }
                pc.SetWriter(doc);
                pc.WebOpen(url, openMode, CurrentUser.UserName);
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


        /// <summary>
        /// 获取office模板文件
        /// </summary>
        [Authorize]
        public void GetReportOffileFileLxd()
        {
            try
            {
                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string tempreport = "";
                string reporthelper = "";
                tempreport = "建设工程消防验收（备案抽查）问题联系单" + serialno + fxbaid + "_v.docx";
                reporthelper = "建设工程消防验收（备案抽查）问题联系单.docx";

                byte[] fileContent = null;
                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;


                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    data.Add("VIEW_I_M_GC_XFYS2", CommonService.GetDataTable2("select * from VIEW_I_M_GC_XFYS2 where serialno='" + serialno + "'"));
                    data.Add("VIEW_I_S_GC_XFYSJL", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYSJL where serialno='" + serialno + "'"));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://wlxf.jzyglxt.com/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                fileContent = Convert.FromBase64String(d.file as string);
                            }
                        }
                    }

                }

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office.docx");
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
        public void SaveReportFileLxd()
        {
            string msg = "保存成功";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {

                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();

                IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                data.Add("VIEW_I_M_GC_XFYS2", CommonService.GetDataTable2("select * from VIEW_I_M_GC_XFYS2 where serialno='" + serialno + "'"));
                data.Add("VIEW_I_S_GC_XFYSJL", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYSJL where serialno='" + serialno + "'"));

                string tempreport = "";
                string filereport = "";
                filereport = "建设工程消防验收（备案抽查）问题联系单" + serialno + fxbaid + "_v.docx";
                tempreport = "建设工程消防验收（备案抽查）问题联系单.docx";


                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + filereport;
                string temppath = Server.MapPath("~\\report\\jdbg") + "\\" + tempreport;
                byte[] file = fs.FileBytes;

                System.IO.File.WriteAllBytes(filepath, file);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string result = ReportHelper.ReFormatWts(Convert.ToBase64String(System.IO.File.ReadAllBytes(temppath)), Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath)), data);
                ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                if (ret != null)
                {
                    if (ret.success)
                    {
                        Dictionary<string, object> d = ret.data as Dictionary<string, object>;
                        if (d != null)
                        {
                            file = Convert.FromBase64String(d["filestr"].GetSafeString());
                            System.IO.File.WriteAllBytes(filepath, file);
                        }

                    }
                }
                CommonService.Execsql("update I_S_GC_XFYS set isbclxd=1 where serialno='" + serialno + "'");
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
        #endregion

        #region  消防复查意见书编辑html页面
        [Authorize]
        public ActionResult ReportOfficeHtmlEditFcyjs()
        {
            //获取参数
            string serialno = Request["serialno"].GetSafeString();
            string fxbaid = Request["fxbaid"].GetSafeString();
            string ishg = Request["ishg"].GetSafeString();
            string zywt = Request["zywt"].GetSafeString();
            try
            {
                string jsdwmc = "";
                string ckcssj = "";
                string gcmc = "";
                string gcdd = "";
                decimal zjzmj = 0;
                decimal jzgd = 0;
                decimal dscs = 0;
                decimal dxcs = 0;
                string zxsyxz = "";
                string bapzwh = "";
                IList<IDictionary<string, string>> gcxx = CommonService.GetDataTable("select fxbaid,serialno,jsdwmc,ckcssj,gcmc,gcdd,zjzmj,jzgd,dscs,dxcs,zxsyxz,bapzwh from VIEW_I_M_GC_XFYS2 where fxbaid='" + fxbaid + "' and serialno='" + serialno + "'");
                if (gcxx.Count > 0)
                {
                    jsdwmc = gcxx[0]["jsdwmc"].GetSafeString();
                    ckcssj = gcxx[0]["ckcssj"].GetSafeString();
                    gcmc = gcxx[0]["gcmc"].GetSafeString();
                    gcdd = gcxx[0]["gcdd"].GetSafeString();
                    zjzmj = gcxx[0]["zjzmj"].GetSafeDecimal();
                    jzgd = gcxx[0]["jzgd"].GetSafeDecimal();
                    dscs = gcxx[0]["dscs"].GetSafeDecimal();
                    dxcs = gcxx[0]["dxcs"].GetSafeDecimal();
                    zxsyxz = gcxx[0]["zxsyxz"].GetSafeString();
                    bapzwh = gcxx[0]["bapzwh"].GetSafeString();
                }
                string url = "/wlxf/GetReportOffileFileFcyjs?serialno=" + serialno + "&fxbaid=" + fxbaid + "&ishg=" + System.Web.HttpUtility.UrlEncode(ishg) + "&jsdwmc=" + System.Web.HttpUtility.UrlEncode(jsdwmc) + "&ckcssj=" + System.Web.HttpUtility.UrlEncode(ckcssj) + "&gcmc=" + System.Web.HttpUtility.UrlEncode(gcmc) + "&zywt=" + System.Web.HttpUtility.UrlEncode(zywt) + "&gcdd=" + System.Web.HttpUtility.UrlEncode(gcdd) + "&zjzmj=" + System.Web.HttpUtility.UrlEncode(zjzmj.GetSafeString()) + "&jzgd=" + System.Web.HttpUtility.UrlEncode(jzgd.GetSafeString()) + "&dscs=" + System.Web.HttpUtility.UrlEncode(dscs.GetSafeString()) + "&dxcs=" + System.Web.HttpUtility.UrlEncode(dxcs.GetSafeString()) + "&zxsyxz=" + System.Web.HttpUtility.UrlEncode(zxsyxz) + "&bapzwh=" + System.Web.HttpUtility.UrlEncode(bapzwh);

                PageOffice.WordWriter.WordDocument doc = new PageOffice.WordWriter.WordDocument();
                doc.DisableWindowSelection = false;
                doc.DisableWindowRightClick = false;

                PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
                pc.ID = "PageOfficeCtrl1";
                pc.SaveFilePage = "/wlxf/SaveReportFileFcyjs?serialno=" + serialno + "&fxbaid=" + fxbaid + "&ishg=" + System.Web.HttpUtility.UrlEncode(ishg) + "&jsdwmc=" + System.Web.HttpUtility.UrlEncode(jsdwmc) + "&ckcssj=" + System.Web.HttpUtility.UrlEncode(ckcssj) + "&gcmc=" + System.Web.HttpUtility.UrlEncode(gcmc) + "&zywt=" + System.Web.HttpUtility.UrlEncode(zywt) + "&gcdd=" + System.Web.HttpUtility.UrlEncode(gcdd) + "&zjzmj=" + System.Web.HttpUtility.UrlEncode(zjzmj.GetSafeString()) + "&jzgd=" + System.Web.HttpUtility.UrlEncode(jzgd.GetSafeString()) + "&dscs=" + System.Web.HttpUtility.UrlEncode(dscs.GetSafeString()) + "&dxcs=" + System.Web.HttpUtility.UrlEncode(dxcs.GetSafeString()) + "&zxsyxz=" + System.Web.HttpUtility.UrlEncode(zxsyxz) + "&bapzwh=" + System.Web.HttpUtility.UrlEncode(bapzwh);
                pc.ServerPage = "/pageoffice/server.aspx";
                pc.SetWriter(doc);
                pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
                pc.Titlebar = true; //隐藏标题栏
                pc.Menubar = true; //隐藏菜单栏
                pc.OfficeToolbars = true; //隐藏Office工具栏
                pc.CustomToolbar = true; //隐藏自定义工具栏
                pc.AddCustomToolButton("保存", "SaveDocument()", 1);
                pc.AddCustomToolButton("导入文件", "OpenDocument()", 3);
                pc.AddCustomToolButton("全屏切换", "SwitchFullScreen()", 4);
                pc.AddCustomToolButton("打印", "ShowPrintDlg()", 6);

                System.Web.UI.Page page = new System.Web.UI.Page();
                PageOffice.OpenModeType openMode = PageOffice.OpenModeType.docSubmitForm;


                string tempreport = "";
                string reporthelper = "";
                if (ishg == "该工程消防验收合格")
                {
                    tempreport = "特殊建设工程消防验收复查意见书（合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收复查意见书（合格）.docx";
                }
                else
                {
                    tempreport = "特殊建设工程消防验收复查意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收复查意见书（不合格）.docx";
                }

                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;

                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "CKCSSJ",ckcssj},
                        { "GCMC",gcmc},
                        { "GCDD",gcdd},
                        { "ZJZMJ",zjzmj},
                        { "JZGD",jzgd},
                        { "DSCS",dscs},
                        { "DXCS",dxcs},
                        { "ZXSYXZ",zxsyxz},
                        { "BAPZWH",bapzwh},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "YJSBH",""},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                    dt.Add(dic);
                    data.Add("VIEW_I_M_GC_XFYS2", dt);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://183.131.123.107:8083/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                Newtonsoft.Json.Linq.JArray strdata = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(jss.Serialize(d.bookmark));
                                foreach (var item in strdata)
                                {
                                    doc.OpenDataRegion(item["key"].GetSafeString()).Editing = true;
                                }
                                pc.SetWriter(doc);
                            }
                        }
                    }

                }
                pc.SetWriter(doc);
                pc.WebOpen(url, openMode, CurrentUser.UserName);
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


        /// <summary>
        /// 获取office模板文件
        /// </summary>
        [Authorize]
        public void GetReportOffileFileFcyjs()
        {
            try
            {
                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string jsdwmc = Request["jsdwmc"].GetSafeString();
                string ckcssj = Request["ckcssj"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string gcdd = Request["gcdd"].GetSafeString();
                decimal zjzmj = Request["zjzmj"].GetSafeDecimal();
                decimal jzgd = Request["jzgd"].GetSafeDecimal();
                decimal dscs = Request["dscs"].GetSafeDecimal();
                decimal dxcs = Request["dxcs"].GetSafeDecimal();
                string zxsyxz = Request["zxsyxz"].GetSafeString();
                string bapzwh = Request["bapzwh"].GetSafeString();
                string ishg = Request["ishg"].GetSafeString();
                string zywt = Request["zywt"].GetSafeString();
                string tempreport = "";
                string reporthelper = "";
                if (ishg == "该工程消防验收合格")
                {
                    tempreport = "特殊建设工程消防验收复查意见书（合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收复查意见书（合格）.docx";
                }
                else
                {
                    tempreport = "特殊建设工程消防验收复查意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "特殊建设工程消防验收复查意见书（不合格）.docx";
                }
                byte[] fileContent = null;
                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;


                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "CKCSSJ",ckcssj},
                        { "GCMC",gcmc},
                        { "GCDD",gcdd},
                        { "ZJZMJ",zjzmj},
                        { "JZGD",jzgd},
                        { "DSCS",dscs},
                        { "DXCS",dxcs},
                        { "ZXSYXZ",zxsyxz},
                        { "BAPZWH",bapzwh},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "YJSBH",""},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                    dt.Add(dic);
                    data.Add("VIEW_I_M_GC_XFYS2", dt);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://183.131.123.107:8083/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                fileContent = Convert.FromBase64String(d.file as string);
                            }
                        }
                    }

                }

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office.docx");
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
        public void SaveReportFileFcyjs()
        {
            string msg = "保存成功";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {

                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string jsdwmc = Request["jsdwmc"].GetSafeString();
                string ckcssj = Request["ckcssj"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string gcdd = Request["gcdd"].GetSafeString();
                decimal zjzmj = Request["zjzmj"].GetSafeDecimal();
                decimal jzgd = Request["jzgd"].GetSafeDecimal();
                decimal dscs = Request["dscs"].GetSafeDecimal();
                decimal dxcs = Request["dxcs"].GetSafeDecimal();
                string zxsyxz = Request["zxsyxz"].GetSafeString();
                string bapzwh = Request["bapzwh"].GetSafeString();
                string ishg = Request["ishg"].GetSafeString();
                string zywt = Request["zywt"].GetSafeString();

                IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "CKCSSJ",ckcssj},
                        { "GCMC",gcmc},
                        { "GCDD",gcdd},
                        { "ZJZMJ",zjzmj},
                        { "JZGD",jzgd},
                        { "DSCS",dscs},
                        { "DXCS",dxcs},
                        { "ZXSYXZ",zxsyxz},
                        { "BAPZWH",bapzwh},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "YJSBH",""},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                dt.Add(dic);
                data.Add("VIEW_I_M_GC_XFYS2", dt);

                string tempreport = "";
                string filereport = "";
                if (ishg == "该工程消防验收合格")
                {
                    filereport = "特殊建设工程消防验收复查意见书（合格）" + serialno + fxbaid + "_v.docx";
                    tempreport = "特殊建设工程消防验收复查意见书（合格）.docx";
                }
                else
                {
                    filereport = "特殊建设工程消防验收复查意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    tempreport = "特殊建设工程消防验收复查意见书（不合格）.docx";
                }



                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + filereport;
                string temppath = Server.MapPath("~\\report\\jdbg") + "\\" + tempreport;
                byte[] file = fs.FileBytes;

                System.IO.File.WriteAllBytes(filepath, file);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string result = ReportHelper.ReFormatWts(Convert.ToBase64String(System.IO.File.ReadAllBytes(temppath)), Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath)), data);
                ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                if (ret != null)
                {
                    if (ret.success)
                    {
                        Dictionary<string, object> d = ret.data as Dictionary<string, object>;
                        if (d != null)
                        {
                            file = Convert.FromBase64String(d["filestr"].GetSafeString());
                            System.IO.File.WriteAllBytes(filepath, file);
                        }

                    }
                }
                CommonService.Execsql("update I_S_GC_XFYS set isbcfcyjs=1 where serialno='" + serialno + "'");
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
        #endregion

        #region  规上工业企业房屋消防验收意见书编辑html页面
        [Authorize]
        public ActionResult ReportOfficeHtmlEditGsyjs()
        {
            //获取参数
            string serialno = Request["serialno"].GetSafeString();
            string fxbaid = Request["fxbaid"].GetSafeString();
            string ishg = Request["ishg"].GetSafeString();
            string zywt = Request["zywt"].GetSafeString();
            try
            {
                string jsdwmc = "";
                string ckcssj = "";
                string gcmc = "";
                string gcdd = "";
                decimal zjzmj = 0;
                decimal jzgd = 0;
                decimal dscs = 0;
                decimal dxcs = 0;
                string zxsyxz = "";
                IList<IDictionary<string, string>> gcxx = CommonService.GetDataTable("select fxbaid,serialno,jsdwmc,ckcssj,gcmc,gcdd,zjzmj,jzgd,dscs,dxcs,zxsyxz,bapzwh from VIEW_I_M_GC_XFYS2 where fxbaid='" + fxbaid + "' and serialno='" + serialno + "'");
                if (gcxx.Count > 0)
                {
                    jsdwmc = gcxx[0]["jsdwmc"].GetSafeString();
                    ckcssj = gcxx[0]["ckcssj"].GetSafeString();
                    gcmc = gcxx[0]["gcmc"].GetSafeString();
                    gcdd = gcxx[0]["gcdd"].GetSafeString();
                    zjzmj = gcxx[0]["zjzmj"].GetSafeDecimal();
                    jzgd = gcxx[0]["jzgd"].GetSafeDecimal();
                    dscs = gcxx[0]["dscs"].GetSafeDecimal();
                    dxcs = gcxx[0]["dxcs"].GetSafeDecimal();
                    zxsyxz = gcxx[0]["zxsyxz"].GetSafeString();
                }
                string url = "/wlxf/GetReportOffileFileGsyjs?serialno=" + serialno + "&fxbaid=" + fxbaid + "&ishg=" + System.Web.HttpUtility.UrlEncode(ishg) + "&jsdwmc=" + System.Web.HttpUtility.UrlEncode(jsdwmc) + "&ckcssj=" + System.Web.HttpUtility.UrlEncode(ckcssj) + "&gcmc=" + System.Web.HttpUtility.UrlEncode(gcmc) + "&zywt=" + System.Web.HttpUtility.UrlEncode(zywt) + "&gcdd=" + System.Web.HttpUtility.UrlEncode(gcdd) + "&zjzmj=" + System.Web.HttpUtility.UrlEncode(zjzmj.GetSafeString()) + "&jzgd=" + System.Web.HttpUtility.UrlEncode(jzgd.GetSafeString()) + "&dscs=" + System.Web.HttpUtility.UrlEncode(dscs.GetSafeString()) + "&dxcs=" + System.Web.HttpUtility.UrlEncode(dxcs.GetSafeString()) + "&zxsyxz=" + System.Web.HttpUtility.UrlEncode(zxsyxz);

                PageOffice.WordWriter.WordDocument doc = new PageOffice.WordWriter.WordDocument();
                doc.DisableWindowSelection = false;
                doc.DisableWindowRightClick = false;

                PageOffice.PageOfficeCtrl pc = new PageOffice.PageOfficeCtrl();
                pc.ID = "PageOfficeCtrl1";
                pc.SaveFilePage = "/wlxf/SaveReportFileGsyjs?serialno=" + serialno + "&fxbaid=" + fxbaid + "&ishg=" + System.Web.HttpUtility.UrlEncode(ishg) + "&jsdwmc=" + System.Web.HttpUtility.UrlEncode(jsdwmc) + "&ckcssj=" + System.Web.HttpUtility.UrlEncode(ckcssj) + "&gcmc=" + System.Web.HttpUtility.UrlEncode(gcmc) + "&zywt=" + System.Web.HttpUtility.UrlEncode(zywt) + "&gcdd=" + System.Web.HttpUtility.UrlEncode(gcdd) + "&zjzmj=" + System.Web.HttpUtility.UrlEncode(zjzmj.GetSafeString()) + "&jzgd=" + System.Web.HttpUtility.UrlEncode(jzgd.GetSafeString()) + "&dscs=" + System.Web.HttpUtility.UrlEncode(dscs.GetSafeString()) + "&dxcs=" + System.Web.HttpUtility.UrlEncode(dxcs.GetSafeString()) + "&zxsyxz=" + System.Web.HttpUtility.UrlEncode(zxsyxz);
                pc.ServerPage = "/pageoffice/server.aspx";
                pc.SetWriter(doc);
                pc.JsFunction_AfterDocumentOpened = "AfterDocumentOpened()";
                pc.Titlebar = true; //隐藏标题栏
                pc.Menubar = true; //隐藏菜单栏
                pc.OfficeToolbars = true; //隐藏Office工具栏
                pc.CustomToolbar = true; //隐藏自定义工具栏
                pc.AddCustomToolButton("保存", "SaveDocument()", 1);
                pc.AddCustomToolButton("导入文件", "OpenDocument()", 3);
                pc.AddCustomToolButton("全屏切换", "SwitchFullScreen()", 4);
                pc.AddCustomToolButton("打印", "ShowPrintDlg()", 6);

                System.Web.UI.Page page = new System.Web.UI.Page();
                PageOffice.OpenModeType openMode = PageOffice.OpenModeType.docSubmitForm;


                string tempreport = "";
                string reporthelper = "";
                if (ishg == "该工程符合建设工程消防验收有关规定")
                {
                    tempreport = "建筑工程消防验收（补办证）意见书（合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "建筑工程消防验收（补办证）意见书（合格）.docx";
                }
                else
                {
                    tempreport = "建筑工程消防验收（补办证）意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "建筑工程消防验收（补办证）意见书（不合格）.docx";
                }

                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;

                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "CKCSSJ",ckcssj},
                        { "GCMC",gcmc},
                        { "GCDD",gcdd},
                        { "ZJZMJ",zjzmj},
                        { "JZGD",jzgd},
                        { "DSCS",dscs},
                        { "DXCS",dxcs},
                        { "ZXSYXZ",zxsyxz},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "YJSBH",""},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                    dt.Add(dic);
                    data.Add("VIEW_I_M_GC_XFYS2", dt);
                    data.Add("VIEW_I_S_GC_XFYS_FGC3", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYS_FGC3 where serialno='" + serialno + "'"));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://183.131.123.107:8083/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                Newtonsoft.Json.Linq.JArray strdata = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(jss.Serialize(d.bookmark));
                                foreach (var item in strdata)
                                {
                                    doc.OpenDataRegion(item["key"].GetSafeString()).Editing = true;
                                }
                                pc.SetWriter(doc);
                            }
                        }
                    }

                }
                pc.SetWriter(doc);
                pc.WebOpen(url, openMode, CurrentUser.UserName);
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


        /// <summary>
        /// 获取office模板文件
        /// </summary>
        [Authorize]
        public void GetReportOffileFileGsyjs()
        {
            try
            {
                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string jsdwmc = Request["jsdwmc"].GetSafeString();
                string ckcssj = Request["ckcssj"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string gcdd = Request["gcdd"].GetSafeString();
                decimal zjzmj = Request["zjzmj"].GetSafeDecimal();
                decimal jzgd = Request["jzgd"].GetSafeDecimal();
                decimal dscs = Request["dscs"].GetSafeDecimal();
                decimal dxcs = Request["dxcs"].GetSafeDecimal();
                string zxsyxz = Request["zxsyxz"].GetSafeString();
                string ishg = Request["ishg"].GetSafeString();
                string zywt = Request["zywt"].GetSafeString();
                string tempreport = "";
                string reporthelper = "";
                if (ishg == "该工程符合建设工程消防验收有关规定")
                {
                    tempreport = "建筑工程消防验收（补办证）意见书（合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "建筑工程消防验收（补办证）意见书（合格）.docx";
                }
                else
                {
                    tempreport = "建筑工程消防验收（补办证）意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    reporthelper = "建筑工程消防验收（补办证）意见书（不合格）.docx";
                }
                byte[] fileContent = null;
                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + tempreport;
                string tempfile = Server.MapPath("~\\report\\jdbg") + "\\" + reporthelper;


                if (System.IO.File.Exists(tempfile) || System.IO.File.Exists(filepath))
                {
                    string file = "";
                    if (System.IO.File.Exists(filepath))
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
                    else
                        file = Convert.ToBase64String(System.IO.File.ReadAllBytes(tempfile));
                    IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "CKCSSJ",ckcssj},
                        { "GCMC",gcmc},
                        { "GCDD",gcdd},
                        { "ZJZMJ",zjzmj},
                        { "JZGD",jzgd},
                        { "DSCS",dscs},
                        { "DXCS",dxcs},
                        { "ZXSYXZ",zxsyxz},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "YJSBH",""},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                    dt.Add(dic);
                    data.Add("VIEW_I_M_GC_XFYS2", dt);
                    data.Add("VIEW_I_S_GC_XFYS_FGC3", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYS_FGC3 where serialno='" + serialno + "'"));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    string result = ReportHelper.FormatWts(file, @"http://183.131.123.107:8083/wlxf/GetSign?username={0}", data, "1");
                    ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                    if (ret != null)
                    {
                        if (ret.success)
                        {
                            ReportPrintService.Common.DataFormat d = jss.Deserialize<ReportPrintService.Common.DataFormat>(ret.data as string);
                            if (d != null)
                            {
                                fileContent = Convert.FromBase64String(d.file as string);
                            }
                        }
                    }

                }

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.HeaderEncoding = System.Text.Encoding.UTF8;
                Response.AddHeader("Content-Disposition", "attachment;filename=office.docx");
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
        public void SaveReportFileGsyjs()
        {
            string msg = "保存成功";
            PageOffice.FileSaver fs = new PageOffice.FileSaver();
            try
            {

                string serialno = Request["serialno"].GetSafeString("");
                string fxbaid = Request["fxbaid"].GetSafeString();
                string jsdwmc = Request["jsdwmc"].GetSafeString();
                string ckcssj = Request["ckcssj"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string gcdd = Request["gcdd"].GetSafeString();
                decimal zjzmj = Request["zjzmj"].GetSafeDecimal();
                decimal jzgd = Request["jzgd"].GetSafeDecimal();
                decimal dscs = Request["dscs"].GetSafeDecimal();
                decimal dxcs = Request["dxcs"].GetSafeDecimal();
                string zxsyxz = Request["zxsyxz"].GetSafeString();
                string ishg = Request["ishg"].GetSafeString();
                string zywt = Request["zywt"].GetSafeString();

                IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer.OrdinalIgnoreCase);
                IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "JSDWMC",jsdwmc},
                        { "CKCSSJ",ckcssj},
                        { "GCMC",gcmc},
                        { "GCDD",gcdd},
                        { "ZJZMJ",zjzmj},
                        { "JZGD",jzgd},
                        { "DSCS",dscs},
                        { "DXCS",dxcs},
                        { "ZXSYXZ",zxsyxz},
                        { "ZYWT",zywt},
                        { "ISHG",ishg+"。"},
                        { "YJSBH",""},
                        { "JDY",""},
                        { "TQRY",""},
                        { "YSFSZH",""},
                        { "YSQFZH",""},
                        { "YSQFSJ",""}
                    };
                dt.Add(dic);
                data.Add("VIEW_I_M_GC_XFYS2", dt);
                data.Add("VIEW_I_S_GC_XFYS_FGC3", CommonService.GetDataTable2("select * from VIEW_I_S_GC_XFYS_FGC3 where serialno='" + serialno + "'"));
                string tempreport = "";
                string filereport = "";
                if (ishg == "该工程符合建设工程消防验收有关规定")
                {
                    filereport = "建筑工程消防验收（补办证）意见书（合格）" + serialno + fxbaid + "_v.docx";
                    tempreport = "建筑工程消防验收（补办证）意见书（合格）.docx";
                }
                else
                {
                    filereport = "建筑工程消防验收（补办证）意见书（不合格）" + serialno + fxbaid + "_v.docx";
                    tempreport = "建筑工程消防验收（补办证）意见书（不合格）.docx";
                }



                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + filereport;
                string temppath = Server.MapPath("~\\report\\jdbg") + "\\" + tempreport;
                byte[] file = fs.FileBytes;

                System.IO.File.WriteAllBytes(filepath, file);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string result = ReportHelper.ReFormatWts(Convert.ToBase64String(System.IO.File.ReadAllBytes(temppath)), Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath)), data);
                ReportPrintService.Common.Result ret = jss.Deserialize<ReportPrintService.Common.Result>(result);
                if (ret != null)
                {
                    if (ret.success)
                    {
                        Dictionary<string, object> d = ret.data as Dictionary<string, object>;
                        if (d != null)
                        {
                            file = Convert.FromBase64String(d["filestr"].GetSafeString());
                            System.IO.File.WriteAllBytes(filepath, file);
                        }

                    }
                }
                CommonService.Execsql("update I_S_GC_XFYS set isbcgsyjs=1 where serialno='" + serialno + "'");
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
        #endregion

        #region 获取工程信息
        [Authorize]
        public void getSysj()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int taskid = Request["taskid"].GetSafeInt(0);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string where = "";
                string sql = "";
                sql = "select sysj from I_S_GC_XFYS where serialno=(select serialno from STToDoTasks where taskid=" + taskid + ") and zt not in('BAQR','CQQR','YTTH','CKSJ')";
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
        #endregion

        #region  查看本月项目分配详情
        [Authorize]
        public void ViewFpxq()
        {
            string err = "";
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> fpxm = new Dictionary<string, object>();
            IList<object> fpxmhzlist = new List<object>();
            try
            {
                string sql = "";
                sql = "select gcmc,jdyxm,tqryxm,apsj,xmfpsj,xmfprq,gcdd from View_I_S_GC_XFYS_FPXQ order by xmfpsj desc";
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                fpxm.Add("fpxq", datas);
                sql = "select '梁朝晖' as ryxm,count(*) as zcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%梁朝晖%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)) as xbxms,(select count(*) as xbxms from i_s_gc_xfys where jdyxm like '%梁朝晖%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byzcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%梁朝晖%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byxbxms from i_s_gc_xfys where jdyxm like '%梁朝晖%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)";
                datas = CommonService.GetDataTable(sql);
                fpxmhzlist.Add(datas);
                sql = "select '陈超' as ryxm,count(*) as zcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%陈超%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)) as xbxms,(select count(*) as xbxms from i_s_gc_xfys where jdyxm like '%陈超%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byzcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%陈超%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byxbxms from i_s_gc_xfys where jdyxm like '%陈超%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)";
                datas = CommonService.GetDataTable(sql);
                fpxmhzlist.Add(datas);
                sql = "select '李安平' as ryxm,count(*) as zcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%李安平%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)) as xbxms,(select count(*) as xbxms from i_s_gc_xfys where jdyxm like '%李安平%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byzcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%李安平%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byxbxms from i_s_gc_xfys where jdyxm like '%李安平%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)";
                datas = CommonService.GetDataTable(sql);
                fpxmhzlist.Add(datas);
                sql = "select '应曦' as ryxm,count(*) as zcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%应曦%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)) as xbxms,(select count(*) as xbxms from i_s_gc_xfys where jdyxm like '%应曦%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byzcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%应曦%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byxbxms from i_s_gc_xfys where jdyxm like '%应曦%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)";
                datas = CommonService.GetDataTable(sql);
                fpxmhzlist.Add(datas);
                sql = "select '林海军' as ryxm,count(*) as zcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%林海军%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)) as xbxms,(select count(*) as xbxms from i_s_gc_xfys where jdyxm like '%林海军%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byzcbxms,(select count(*) as xbxms from i_s_gc_xfys where tqryxm like '%林海军%' and  (DATEDIFF(mm, xmfpsj, GETDATE()) = 0)) as byxbxms from i_s_gc_xfys where jdyxm like '%林海军%' and  (DATEDIFF(yy, xmfpsj, GETDATE()) = 0)";
                datas = CommonService.GetDataTable(sql);
                fpxmhzlist.Add(datas);
                fpxm.Add("fphz", fpxmhzlist);
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(fpxm)));
                Response.End();
            }
        }
        #endregion

        #region 删除申报工程信息
        [Authorize]
        public void getDelSbgc()
        {
            bool ret = false;
            string rettext = "";
            string err = "";

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string serilano = Request["Serialno"].GetSafeString();
                string fxbaid = "";
                string zt = "";
                string typebh = "";
                string sbh = "";
                int recid = 0;
                datas = CommonService.GetDataTable("select top 1 * from i_s_gc_xfys where serialno='" + serilano + "'");
                if (datas.Count > 0)
                {
                    recid = datas[0]["RECID"].GetSafeInt();
                    fxbaid = datas[0]["FXBAID"].GetSafeString();
                    zt = datas[0]["ZT"].GetSafeString();
                    typebh = datas[0]["Typebh"].GetSafeString();
                    sbh = datas[0]["SBH"].GetSafeString();
                }
                if (CurrentUser.UserCode == "URCOPusywPPcLH" && datas.Count > 0)
                {
                    IList<string> sqls = new List<string>();
                    sqls.Add("delete from i_s_gc_xfys where Serialno='" + serilano + "'");
                    sqls.Add("delete from i_s_gc_xfys_reportfile where Serialno='" + serilano + "'");
                    sqls.Add("delete from i_s_gc_xfys_zl where Serialno='" + serilano + "'");
                    sqls.Add("delete from i_s_gc_xfys_alert where Serialno='" + serilano + "'");
                    sqls.Add("delete from sttodotasks where Serialno='" + serilano + "'");
                    sqls.Add("update stform set dostate=3 where Serialno='" + serilano + "'");
                    if (!string.IsNullOrEmpty(sbh) && (typebh == "01" || typebh == "02" || typebh == "03"))
                        sqls.Add("insert into I_S_GC_XFYS_SBHZX(fxbaid,sbh,isdone) values('" + fxbaid + "','" + sbh + "',0)");
                    sqls.Add("insert into I_S_GC_XFYS_DW_RECORD(fxbaid,parentid,szbm,lrsj,lrrzh,lrrxm) values('" + fxbaid + "','" + recid + "','I_S_GC_XFYS',getdate(),'" + CurrentUser.UserCode + "','" + CurrentUser.RealName + "')");
                    ret = CommonService.ExecTrans(sqls, out err);
                }
                else
                {

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
        #endregion

        #region  现场检测拍照

        #region 获取某次现场检测照片列表
        [Authorize]
        public void getXfjcZpList()
        {
            string fxbaid = Request["fxbaid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string serialno = Request["serialno"].GetSafeString();
            int totalcount = 0;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from I_S_GC_XFJC_XCJC_XCZP where fxbaid='" + fxbaid + "' and serialno='" + serialno + "'";
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
        #endregion

        #region 新增现场检测照片记录
        [Authorize]
        public void getXfjcZpAdd()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            string fxbaid = Request["fxbaid"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            string zpms = Request["zpms"].GetSafeString();
            string fj = Request["fj"].GetSafeString();
            string jcry = Request["jcry"].GetSafeString();
            string zbd = Request["zbd"].GetSafeString();
            string dz = Request["dz"].GetSafeString();
            DateTime pzsj = Request["pzsj"].GetSafeDate();
            int ismust = Request["ismust"].GetSafeInt();
            int mustin = Request["mustin"].GetSafeInt();
            int xh = Request["xh"].GetSafeInt();
            string serialno = Request["serialno"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "";
                if (recid != "")
                    sql = "update I_S_GC_XFJC_XCJC_XCZP set FXBAID='" + fxbaid + "',ZPMS='" + zpms + "',JCRY='" + jcry + "',ZBD='" + zbd + "',DZ='" + dz + "',PZSJ='" + pzsj + "',FJ='" + fj + "',LRSJ=getdate(),IsMust='" + ismust + "',xh='" + xh + "' where [RECID]=" + recid + " and [FXBAID]='" + fxbaid + "'";
                else
                    sql = "INSERT INTO [I_S_GC_XFJC_XCJC_XCZP] ([FXBAID] ,[Serialno],[ZPMS],[JCRY],[ZBD],[DZ],[PZSJ],[FJ],[LRSJ],[IsMust],[MustIn],[XH]) VALUES('" + fxbaid + "' ,'" + serialno + "' ,'" + zpms + "' ,'" + jcry + "' ,'" + zbd + "' ,'" + dz + "','" + pzsj + "','" + fj + "',getdate(),'" + ismust + "','" + mustin + "','" + xh + "')";
                ret = CommonService.ExecSql(sql, out err);
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
        #endregion

        #region 删除现场检测照片记录
        [Authorize]
        public void getXfjcZpDel()
        {
            bool ret = false;
            string rettext = "";
            string err = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string serialno = Request["serialno"].GetSafeString();
                string recid = Request["recid"].GetSafeString();
                ret = CommonService.ExecSql("delete from [I_S_GC_XFJC_XCJC_XCZP] where [RECID]=" + recid + " and [Serialno]='" + serialno + "'", out err);
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
        #endregion
        #endregion

        #region 获取单体建筑
        [Authorize]
        public void getDtjzList()
        {
            string serialno = Request["serialno"].GetSafeString();
            string fxbaid = Request["fxbaid"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(100);
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string dtjz = "";
                string sql = "select extrainfo4 from stform where serialno='" + serialno + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                    dtjz = dt[0]["extrainfo4"].GetSafeString();
                sql = "select recid,fgcmc,(case when recid in (select str2table from strtotable('" + dtjz + "')) then 1 else 0 end) isselect  from I_S_GC_XFYS_FGC where fxbaid='" + fxbaid + "'";
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
        #endregion

        #region 保存单体建筑
        [Authorize]
        public void SaveDtjz()
        {
            string fxbaid = Request["fxbaid"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            string typebh = Request["typebh"].GetSafeString();
            string recids = Request["recids"].GetSafeString();
            string msg = "";
            bool code = true;
            Dictionary<string, string> datas = new Dictionary<string, string>();
            try
            {
                IList<string> sqls = new List<string>();
                sqls.Add("update STForm set extrainfo4='" + recids + "' where serialno='" + serialno + "'");
                sqls.Add("update I_S_GC_XFYS_REPORTFILE set isdone=0 where fxbaid='" + fxbaid + "' and serialno='" + serialno + "'");
                sqls.Add("insert into I_S_GC_XFYS_DW_RECORD(fxbaid,qybh,qymc,szbm,lrsj,lrrzh,lrrxm) values('" + fxbaid + "','" + recids + "','" + serialno + "','STForm',getdate(),'" + CurrentUser.UserCode + "','" + CurrentUser.RealName + "')"); ;
                code = CommonService.ExecTrans(sqls, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }
        #endregion

        #region 录入工程

        #region 获取建设单位
        [Authorize]
        public void getJsdwList()
        {
            string qymc = Request["qymc"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select qymc,qybh,qyfzr,lxsj,qyfr from view_i_m_qy where qymc like '%" + qymc + "%'";
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
        #endregion

        #region 获取设计单位、检测单位等
        [Authorize]
        public void getDwxxList()
        {
            string qymc = Request["qymc"].GetSafeString();
            string qylx = Request["qylx"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select QYMC,QYBH,QYFR,QYFRSJ,QYFZR,LXSJ,ZZDJ,ZZZSBH from View_I_M_QY_WITH_ZZ where qylxs like '%" + qylx + "%' and qymc like '%" + qymc + "%'";
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
        #endregion

        #region 获取企业资质
        [Authorize]
        public void getQyzzList()
        {
            string qybh = Request["qybh"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select zzdj,zzmc from view_i_s_qy_qyzz where qybh='" + qybh + "'";
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
        #endregion

        #region 获取申报工程信息
        [Authorize]
        public void getGcxx()
        {
            string fxbaid = Request["fxbaid"].GetSafeString();
            IDictionary<string, object> dt = new Dictionary<string, object>();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select RECID,FXBAID,GCMC,LXR,LXDH,GCDD,JSDWMC,JSDWBH,JSDWFZR,JSDWLXDH,ZZDJ,JSDWFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH,GCLXBH,LSXJZ,TSXFSJ,ZJZMJ,JZGD,DSCS,DXCS,HZWXX,GCTZE,Convert(nvarchar(100),KGRQ,23) as KGRQ,Convert(nvarchar(100),JGRQ,23) as JGRQ,JZGCZLJDJG,XFSJPZWH,Convert(nvarchar(100),SJPZZZRQ,23) as SJPZZZRQ,LSXJZPZWJ,JQXFSJCS,XFSS,GBYT,ZXYYYT,ZXSYXZ,ZXGC,ZXBW,ZXMJ,ZXCS,YJZMC,NHDJ,JZBW,JABWCLLB,JZBWCS,JZBWSYXZ,JZBWYYYT,QT,CGWZ,CGZRL,CGSZXS,CGCCXS,CGCCWZMC,DDCCL,DCWZMC,ISBC,ZT,BZ,SZSF,SZCS,SZXQ,SZJD,GCZB,DCCG,ZXSYXZ1,ZXSYXZ2 from I_M_GC_XFYS where fxbaid='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("gcxx", datas);
                sql = "select RECID,FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH from I_S_GC_XFYS_SJDW where fxbaid='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("sjdw", datas);
                sql = "select RECID,FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH from I_S_GC_XFYS_SGDW where fxbaid='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("sgdw", datas);
                sql = "select RECID,FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH from I_S_GC_XFYS_JLDW where fxbaid='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("jldw", datas);
                sql = "select RECID,FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH from I_S_GC_XFYS_TSDW where fxbaid='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("tsdw", datas);
                sql = "select RECID,FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH from I_S_GC_XFYS_JCDW_SGZ where fxbaid='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("jcdw", datas);
                sql = "select RECID,FXBAID,FGCMC,JGXS,SYXZ,NHDJ,DSCS,DXCS,JZGD,ZDMJ,DSJZMJ,DXJZMJ,SYXZ1,SYXZ2 from I_S_GC_XFYS_FGC where FXBAID='" + fxbaid + "'";
                datas = CommonService.GetDataTable(sql);
                dt.Add("dtjz", datas);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(dt));
                Response.End();
            }
        }
        #endregion

        #region 新增或者删除申报工程
        [Authorize]
        public void addGcxx()
        {
            string msg = "";
            bool code = true;
            try
            {
                string gcxx = Request["gcxx"].GetSafeString();
                string sjdw = Request["sjdw"].GetSafeString();
                string sgdw = Request["sgdw"].GetSafeString();
                string jldw = Request["jldw"].GetSafeString();
                string tsdw = Request["tsdw"].GetSafeString();
                string jcdw = Request["jcdw"].GetSafeString();
                string dtjz = Request["dtjz"].GetSafeString();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                IList<Dictionary<string, object>> gcxxitem = jss.Deserialize<IList<Dictionary<string, object>>>(gcxx);
                IList<Dictionary<string, object>> sjdwlist = jss.Deserialize<IList<Dictionary<string, object>>>(sjdw);
                IList<Dictionary<string, object>> sgdwlist = jss.Deserialize<IList<Dictionary<string, object>>>(sgdw);
                IList<Dictionary<string, object>> jldwlist = jss.Deserialize<IList<Dictionary<string, object>>>(jldw);
                IList<Dictionary<string, object>> tsdwlist = jss.Deserialize<IList<Dictionary<string, object>>>(tsdw);
                IList<Dictionary<string, object>> jcdwlist = jss.Deserialize<IList<Dictionary<string, object>>>(jcdw);
                IList<Dictionary<string, object>> dtjzlist = jss.Deserialize<IList<Dictionary<string, object>>>(dtjz);
                string procstr = string.Format("CheckQYISJY('{0}')", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                IList<string> sqls = new List<string>();
                string sql = "";
                if (dt.Count > 0)
                {
                    if (dt[0]["msg"].GetSafeString() == "true")
                    {
                        string guid = "";
                        //工程信息
                        if (gcxxitem.Count > 0)
                        {
                            procstr = string.Format("CheckGcmc('{0}','{1}','{2}')", gcxxitem[0]["gcmc"].GetSafeString(), gcxxitem[0]["fxbaid"].GetSafeString(), CurrentUser.UserName);
                            dt = CommonService.ExecDataTableProc(procstr, out msg);
                            if (dt[0]["msg"].GetSafeString() != "true")
                            {
                                code = false;
                                msg = dt[0]["msg"].GetSafeString();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(gcxxitem[0]["fxbaid"].GetSafeString()))
                                {
                                    guid = Guid.NewGuid().ToString("N");
                                    sql = "insert into i_m_gc_xfys(FXBAID,GCMC,LXR,LXDH,GCDD,JSDWMC,JSDWBH,JSDWFZR,JSDWLXDH,ZZDJ,JSDWFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH,GCLXBH,LSXJZ,TSXFSJ,ZJZMJ,JZGD,DSCS,DXCS,HZWXX,GCTZE,KGRQ,JGRQ,JZGCZLJDJG,XFSJPZWH,SJPZZZRQ,LSXJZPZWJ,JQXFSJCS,XFSS,GBYT,ZXYYYT,ZXSYXZ,ZXGC,ZXBW,ZXMJ,ZXCS,YJZMC,NHDJ,JZBW,JABWCLLB,JZBWCS,JZBWSYXZ,JZBWYYYT,QT,CGWZ,CGZRL,CGSZXS,CGCCXS,CGCCWZMC,DDCCL,DCWZMC,ISBC,CKQR,ZT,ZTName,LRSJ,LRRZH,LRRXM,BZ,SZSF,SZCS,SZXQ,SZJD,GCZB,DCCG,ZXSYXZ1,ZXSYXZ2)";
                                    sql += " values('" + guid + "','" + gcxxitem[0]["gcmc"].GetSafeString() + "','" + gcxxitem[0]["lxr"].GetSafeString() + "','" + gcxxitem[0]["lxdh"].GetSafeString() + "','" + gcxxitem[0]["gcdd"].GetSafeString() + "','" + gcxxitem[0]["jsdwmc"].GetSafeString() + "','" + gcxxitem[0]["jsdwbh"].GetSafeString() + "','" + gcxxitem[0]["jsdwfzr"].GetSafeString() + "','" + gcxxitem[0]["jsdwlxdh"].GetSafeString() + "','" + gcxxitem[0]["zzdj"].GetSafeString() + "','" + gcxxitem[0]["jsdwfr"].GetSafeString() + "'";
                                    sql += ",'" + gcxxitem[0]["frsfz"].GetSafeString() + "','" + gcxxitem[0]["xmfzr"].GetSafeString() + "','" + gcxxitem[0]["fzrsfz"].GetSafeString() + "','" + gcxxitem[0]["fzrlxdh"].GetSafeString() + "','" + gcxxitem[0]["gclxbh"].GetSafeString() + "','" + gcxxitem[0]["lsxjz"].GetSafeString() + "','" + gcxxitem[0]["tsxfsj"].GetSafeString() + "','" + gcxxitem[0]["zjzmj"].GetSafeDecimal() + "','" + gcxxitem[0]["jzgd"].GetSafeDecimal() + "','" + gcxxitem[0]["dscs"].GetSafeDecimal() + "','" + gcxxitem[0]["dxcs"].GetSafeDecimal() + "'";
                                    sql += ",'" + gcxxitem[0]["hzwxx"].GetSafeString() + "','" + gcxxitem[0]["gctze"].GetSafeDecimal() + "','" + gcxxitem[0]["kgrq"].GetSafeDate() + "','" + gcxxitem[0]["jgrq"].GetSafeDate() + "','" + gcxxitem[0]["jzgczljdjg"].GetSafeString() + "','" + gcxxitem[0]["xfsjpzwh"].GetSafeString() + "','" + gcxxitem[0]["sjpzzzrq"].GetSafeString() + "','" + gcxxitem[0]["lsxjzpzwj"].GetSafeString() + "','" + gcxxitem[0]["jqxfsjcs"].GetSafeString() + "','" + gcxxitem[0]["xfss"].GetSafeString() + "','" + gcxxitem[0]["gbyt"].GetSafeString() + "'";
                                    sql += ",'" + gcxxitem[0]["zxyyyt"].GetSafeString() + "','" + gcxxitem[0]["zxsyxz"].GetSafeString() + "','" + gcxxitem[0]["zxgc"].GetSafeString() + "','" + gcxxitem[0]["zxbw"].GetSafeString() + "','" + gcxxitem[0]["zxmj"].GetSafeString() + "','" + gcxxitem[0]["zxcs"].GetSafeString() + "','" + gcxxitem[0]["yjzmc"].GetSafeString() + "','" + gcxxitem[0]["nhdj"].GetSafeString() + "','" + gcxxitem[0]["jzbw"].GetSafeString() + "','" + gcxxitem[0]["jabwcllb"].GetSafeString() + "','" + gcxxitem[0]["jzbwcs"].GetSafeString() + "'";
                                    sql += ",'" + gcxxitem[0]["jzbwsyxz"].GetSafeString() + "','" + gcxxitem[0]["jzbwyyyt"].GetSafeString() + "','" + gcxxitem[0]["qt"].GetSafeString() + "','" + gcxxitem[0]["cgwz"].GetSafeString() + "','" + gcxxitem[0]["cgzrl"].GetSafeString() + "','" + gcxxitem[0]["cgszxs"].GetSafeString() + "','" + gcxxitem[0]["cgccxs"].GetSafeString() + "','" + gcxxitem[0]["cgccwzmc"].GetSafeString() + "','" + gcxxitem[0]["ddccl"].GetSafeString() + "','" + gcxxitem[0]["dcwzmc"].GetSafeString() + "','" + gcxxitem[0]["isbc"].GetSafeInt() + "','YT','YT','填报',getdate(),'" + CurrentUser.UserCode + "','" + CurrentUser.RealName + "','" + gcxxitem[0]["bz"].GetSafeString() + "'";
                                    sql += ",'" + gcxxitem[0]["szsf"].GetSafeString() + "','" + gcxxitem[0]["szcs"].GetSafeString() + "','" + gcxxitem[0]["szxq"].GetSafeString() + "','" + gcxxitem[0]["szjd"].GetSafeString() + "','" + gcxxitem[0]["gczb"].GetSafeString() + "','" + gcxxitem[0]["dccg"].GetSafeString() + "','" + gcxxitem[0]["zxsyxz1"].GetSafeString() + "','" + gcxxitem[0]["zxsyxz2"].GetSafeString() + "')";
                                    sqls.Add(sql);
                                }
                                else
                                {
                                    guid = gcxxitem[0]["fxbaid"].GetSafeString();
                                    sql = "update i_m_gc_xfys set GCMC='" + gcxxitem[0]["gcmc"].GetSafeString() + "',LXR='" + gcxxitem[0]["lxr"].GetSafeString() + "',LXDH='" + gcxxitem[0]["lxdh"].GetSafeString() + "',GCDD='" + gcxxitem[0]["gcdd"].GetSafeString() + "',JSDWMC='" + gcxxitem[0]["jsdwmc"].GetSafeString() + "',JSDWBH='" + gcxxitem[0]["jsdwbh"].GetSafeString() + "',JSDWFZR='" + gcxxitem[0]["jsdwfzr"].GetSafeString() + "',JSDWLXDH='" + gcxxitem[0]["jsdwlxdh"].GetSafeString() + "',ZZDJ='" + gcxxitem[0]["zzdj"].GetSafeString() + "'";//  where fxbaid='" + gcxxitem["fxbaid"].GetSafeString()+"'";
                                    sql += ",JSDWFR='" + gcxxitem[0]["jsdwfr"].GetSafeString() + "',FRSFZ='" + gcxxitem[0]["frsfz"].GetSafeString() + "',XMFZR='" + gcxxitem[0]["xmfzr"].GetSafeString() + "',FZRSFZ='" + gcxxitem[0]["fzrsfz"].GetSafeString() + "',FZRLXDH='" + gcxxitem[0]["fzrlxdh"].GetSafeString() + "',GCLXBH='" + gcxxitem[0]["gclxbh"].GetSafeString() + "',LSXJZ='" + gcxxitem[0]["lsxjz"].GetSafeString() + "',TSXFSJ='" + gcxxitem[0]["tsxfsj"].GetSafeString() + "',ZJZMJ='" + gcxxitem[0]["zjzmj"].GetSafeDecimal() + "',JZGD='" + gcxxitem[0]["jzgd"].GetSafeDecimal() + "'";
                                    sql += ",DSCS='" + gcxxitem[0]["dscs"].GetSafeDecimal() + "',DXCS='" + gcxxitem[0]["dxcs"].GetSafeDecimal() + "',HZWXX='" + gcxxitem[0]["hzwxx"].GetSafeString() + "',GCTZE='" + gcxxitem[0]["gctze"].GetSafeDecimal() + "',KGRQ='" + gcxxitem[0]["kgrq"].GetSafeDate() + "',JGRQ='" + gcxxitem[0]["jgrq"].GetSafeDate() + "',JZGCZLJDJG='" + gcxxitem[0]["jzgczljdjg"].GetSafeString() + "',XFSJPZWH='" + gcxxitem[0]["xfsjpzwh"].GetSafeString() + "',SJPZZZRQ='" + gcxxitem[0]["sjpzzzrq"].GetSafeString() + "',LSXJZPZWJ='" + gcxxitem[0]["lsxjzpzwj"].GetSafeString() + "'";
                                    sql += ",JQXFSJCS='" + gcxxitem[0]["jqxfsjcs"].GetSafeString() + "',XFSS='" + gcxxitem[0]["xfss"].GetSafeString() + "',GBYT='" + gcxxitem[0]["gbyt"].GetSafeString() + "',ZXYYYT='" + gcxxitem[0]["zxyyyt"].GetSafeString() + "',ZXSYXZ='" + gcxxitem[0]["zxsyxz"].GetSafeString() + "',ZXGC='" + gcxxitem[0]["zxgc"].GetSafeString() + "',ZXBW='" + gcxxitem[0]["zxbw"].GetSafeString() + "',ZXMJ='" + gcxxitem[0]["zxmj"].GetSafeString() + "',ZXCS='" + gcxxitem[0]["zxcs"].GetSafeString() + "',YJZMC='" + gcxxitem[0]["yjzmc"].GetSafeString() + "'";
                                    sql += ",NHDJ='" + gcxxitem[0]["nhdj"].GetSafeString() + "',JZBW='" + gcxxitem[0]["jzbw"].GetSafeString() + "',JABWCLLB='" + gcxxitem[0]["jabwcllb"].GetSafeString() + "',JZBWCS='" + gcxxitem[0]["jzbwcs"].GetSafeString() + "',JZBWSYXZ='" + gcxxitem[0]["jzbwsyxz"].GetSafeString() + "',JZBWYYYT='" + gcxxitem[0]["jzbwyyyt"].GetSafeString() + "',QT='" + gcxxitem[0]["qt"].GetSafeString() + "',CGWZ='" + gcxxitem[0]["cgwz"].GetSafeString() + "',CGZRL='" + gcxxitem[0]["cgzrl"].GetSafeString() + "',CGSZXS='" + gcxxitem[0]["cgszxs"].GetSafeString() + "'";
                                    sql += ",CGCCXS='" + gcxxitem[0]["cgccxs"].GetSafeString() + "',CGCCWZMC='" + gcxxitem[0]["cgccwzmc"].GetSafeString() + "',DDCCL='" + gcxxitem[0]["ddccl"].GetSafeString() + "',DCWZMC='" + gcxxitem[0]["dcwzmc"].GetSafeString() + "',ISBC='" + gcxxitem[0]["isbc"].GetSafeInt() + "',BZ='" + gcxxitem[0]["bz"].GetSafeString() + "',SZSF='" + gcxxitem[0]["szsf"].GetSafeString() + "',SZCS='" + gcxxitem[0]["szcs"].GetSafeString() + "',SZXQ='" + gcxxitem[0]["szxq"].GetSafeString() + "',SZJD='" + gcxxitem[0]["szjd"].GetSafeString() + "',GCZB='" + gcxxitem[0]["gczb"].GetSafeString() + "',DCCG='" + gcxxitem[0]["dccg"].GetSafeString() + "',ZXSYXZ1='" + gcxxitem[0]["zxsyxz1"].GetSafeString() + "',ZXSYXZ2='" + gcxxitem[0]["zxsyxz2"].GetSafeString() + "' where fxbaid='" + gcxxitem[0]["fxbaid"].GetSafeString() + "'";
                                    sqls.Add(sql);
                                }
                                //设计单位
                                if (sjdwlist.Count > 0)
                                {
                                    foreach (Dictionary<string, object> sjdwitem in sjdwlist)
                                    {
                                        if (sjdwitem["edittype"].GetSafeInt() == 0)
                                        {
                                            sql = "insert into I_S_GC_XFYS_SJDW(FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH) values('" + guid + "','" + sjdwitem["qybh"].GetSafeString() + "','" + sjdwitem["qymc"].GetSafeString() + "','" + sjdwitem["qyzz"].GetSafeString() + "','" + sjdwitem["zzzh"].GetSafeString() + "','" + sjdwitem["qyfzr"].GetSafeString() + "','" + sjdwitem["lxdh"].GetSafeString() + "','" + sjdwitem["qyfr"].GetSafeString() + "','" + sjdwitem["frsfz"].GetSafeString() + "','" + sjdwitem["xmfzr"].GetSafeString() + "','" + sjdwitem["fzrsfz"].GetSafeString() + "','" + sjdwitem["fzrlxdh"].GetSafeString() + "')";
                                            sqls.Add(sql);
                                        }
                                        else if (sjdwitem["edittype"].GetSafeInt() == 1)
                                        {
                                            sql = "update I_S_GC_XFYS_SJDW set qybh='" + sjdwitem["qybh"].GetSafeString() + "',qymc='" + sjdwitem["qymc"].GetSafeString() + "',qyzz='" + sjdwitem["qyzz"].GetSafeString() + "',zzzh='" + sjdwitem["zzzh"].GetSafeString() + "',qyfzr='" + sjdwitem["qyfzr"].GetSafeString() + "',lxdh='" + sjdwitem["lxdh"].GetSafeString() + "',qyfr='" + sjdwitem["qyfr"].GetSafeString() + "',frsfz='" + sjdwitem["frsfz"].GetSafeString() + "',xmfzr='" + sjdwitem["xmfzr"].GetSafeString() + "',fzrsfz='" + sjdwitem["fzrsfz"].GetSafeString() + "',fzrlxdh='" + sjdwitem["fzrlxdh"].GetSafeString() + "' where fxbaid='" + sjdwitem["fxbaid"].GetSafeString() + "' and recid='" + sjdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                        else
                                        {
                                            sql = "delete from I_S_GC_XFYS_SJDW where fxbaid='" + sjdwitem["fxbaid"].GetSafeString() + "' and recid='" + sjdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                    }
                                }
                                //施工单位
                                if (sgdwlist.Count > 0)
                                {
                                    foreach (Dictionary<string, object> sgdwitem in sgdwlist)
                                    {
                                        if (sgdwitem["edittype"].GetSafeInt() == 0)
                                        {
                                            sql = "insert into I_S_GC_XFYS_SGDW(FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH) values('" + guid + "','" + sgdwitem["qybh"].GetSafeString() + "','" + sgdwitem["qymc"].GetSafeString() + "','" + sgdwitem["qyzz"].GetSafeString() + "','" + sgdwitem["zzzh"].GetSafeString() + "','" + sgdwitem["qyfzr"].GetSafeString() + "','" + sgdwitem["lxdh"].GetSafeString() + "','" + sgdwitem["qyfr"].GetSafeString() + "','" + sgdwitem["frsfz"].GetSafeString() + "','" + sgdwitem["xmfzr"].GetSafeString() + "','" + sgdwitem["fzrsfz"].GetSafeString() + "','" + sgdwitem["fzrlxdh"].GetSafeString() + "')";
                                            sqls.Add(sql);
                                        }
                                        else if (sgdwitem["edittype"].GetSafeInt() == 1)
                                        {
                                            sql = "update I_S_GC_XFYS_SGDW set qybh='" + sgdwitem["qybh"].GetSafeString() + "',qymc='" + sgdwitem["qymc"].GetSafeString() + "',qyzz='" + sgdwitem["qyzz"].GetSafeString() + "',zzzh='" + sgdwitem["zzzh"].GetSafeString() + "',qyfzr='" + sgdwitem["qyfzr"].GetSafeString() + "',lxdh='" + sgdwitem["lxdh"].GetSafeString() + "',qyfr='" + sgdwitem["qyfr"].GetSafeString() + "',frsfz='" + sgdwitem["frsfz"].GetSafeString() + "',xmfzr='" + sgdwitem["xmfzr"].GetSafeString() + "',fzrsfz='" + sgdwitem["fzrsfz"].GetSafeString() + "',fzrlxdh='" + sgdwitem["fzrlxdh"].GetSafeString() + "' where fxbaid='" + sgdwitem["fxbaid"].GetSafeString() + "' and recid='" + sgdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                        else
                                        {
                                            sql = "delete from I_S_GC_XFYS_SGDW where fxbaid='" + sgdwitem["fxbaid"].GetSafeString() + "' and recid='" + sgdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                    }
                                }
                                //监理单位
                                if (jldwlist.Count > 0)
                                {
                                    foreach (Dictionary<string, object> jldwitem in jldwlist)
                                    {
                                        if (jldwitem["edittype"].GetSafeInt() == 0)
                                        {
                                            sql = "insert into I_S_GC_XFYS_JLDW(FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH) values('" + guid + "','" + jldwitem["qybh"].GetSafeString() + "','" + jldwitem["qymc"].GetSafeString() + "','" + jldwitem["qyzz"].GetSafeString() + "','" + jldwitem["zzzh"].GetSafeString() + "','" + jldwitem["qyfzr"].GetSafeString() + "','" + jldwitem["lxdh"].GetSafeString() + "','" + jldwitem["qyfr"].GetSafeString() + "','" + jldwitem["frsfz"].GetSafeString() + "','" + jldwitem["xmfzr"].GetSafeString() + "','" + jldwitem["fzrsfz"].GetSafeString() + "','" + jldwitem["fzrlxdh"].GetSafeString() + "')";
                                            sqls.Add(sql);
                                        }
                                        else if (jldwitem["edittype"].GetSafeInt() == 1)
                                        {
                                            sql = "update I_S_GC_XFYS_JLDW set qybh='" + jldwitem["qybh"].GetSafeString() + "',qymc='" + jldwitem["qymc"].GetSafeString() + "',qyzz='" + jldwitem["qyzz"].GetSafeString() + "',zzzh='" + jldwitem["zzzh"].GetSafeString() + "',qyfzr='" + jldwitem["qyfzr"].GetSafeString() + "',lxdh='" + jldwitem["lxdh"].GetSafeString() + "',qyfr='" + jldwitem["qyfr"].GetSafeString() + "',frsfz='" + jldwitem["frsfz"].GetSafeString() + "',xmfzr='" + jldwitem["xmfzr"].GetSafeString() + "',fzrsfz='" + jldwitem["fzrsfz"].GetSafeString() + "',fzrlxdh='" + jldwitem["fzrlxdh"].GetSafeString() + "' where fxbaid='" + jldwitem["fxbaid"].GetSafeString() + "' and recid='" + jldwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                        else
                                        {
                                            sql = "delete from I_S_GC_XFYS_JLDW where fxbaid='" + jldwitem["fxbaid"].GetSafeString() + "' and recid='" + jldwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                    }
                                }
                                //图审单位
                                if (tsdwlist.Count > 0)
                                {
                                    foreach (Dictionary<string, object> tsdwitem in tsdwlist)
                                    {
                                        if (tsdwitem["edittype"].GetSafeInt() == 0)
                                        {
                                            sql = "insert into I_S_GC_XFYS_TSDW(FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH) values('" + guid + "','" + tsdwitem["qybh"].GetSafeString() + "','" + tsdwitem["qymc"].GetSafeString() + "','" + tsdwitem["qyzz"].GetSafeString() + "','" + tsdwitem["zzzh"].GetSafeString() + "','" + tsdwitem["qyfzr"].GetSafeString() + "','" + tsdwitem["lxdh"].GetSafeString() + "','" + tsdwitem["qyfr"].GetSafeString() + "','" + tsdwitem["frsfz"].GetSafeString() + "','" + tsdwitem["xmfzr"].GetSafeString() + "','" + tsdwitem["fzrsfz"].GetSafeString() + "','" + tsdwitem["fzrlxdh"].GetSafeString() + "')";
                                            sqls.Add(sql);
                                        }
                                        else if (tsdwitem["edittype"].GetSafeInt() == 1)
                                        {
                                            sql = "update I_S_GC_XFYS_TSDW set qybh='" + tsdwitem["qybh"].GetSafeString() + "',qymc='" + tsdwitem["qymc"].GetSafeString() + "',qyzz='" + tsdwitem["qyzz"].GetSafeString() + "',zzzh='" + tsdwitem["zzzh"].GetSafeString() + "',qyfzr='" + tsdwitem["qyfzr"].GetSafeString() + "',lxdh='" + tsdwitem["lxdh"].GetSafeString() + "',qyfr='" + tsdwitem["qyfr"].GetSafeString() + "',frsfz='" + tsdwitem["frsfz"].GetSafeString() + "',xmfzr='" + tsdwitem["xmfzr"].GetSafeString() + "',fzrsfz='" + tsdwitem["fzrsfz"].GetSafeString() + "',fzrlxdh='" + tsdwitem["fzrlxdh"].GetSafeString() + "' where fxbaid='" + tsdwitem["fxbaid"].GetSafeString() + "' and recid='" + tsdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                        else
                                        {
                                            sql = "delete from I_S_GC_XFYS_TSDW where fxbaid='" + tsdwitem["fxbaid"].GetSafeString() + "' and recid='" + tsdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                    }
                                }
                                //检测单位
                                if (jcdwlist.Count > 0)
                                {
                                    foreach (Dictionary<string, object> jcdwitem in jcdwlist)
                                    {
                                        if (jcdwitem["edittype"].GetSafeInt() == 0)
                                        {
                                            sql = "insert into I_S_GC_XFYS_JCDW_SGZ(FXBAID,QYBH,QYMC,QYZZ,ZZZH,QYFZR,LXDH,QYFR,FRSFZ,XMFZR,FZRSFZ,FZRLXDH) values('" + guid + "','" + jcdwitem["qybh"].GetSafeString() + "','" + jcdwitem["qymc"].GetSafeString() + "','" + jcdwitem["qyzz"].GetSafeString() + "','" + jcdwitem["zzzh"].GetSafeString() + "','" + jcdwitem["qyfzr"].GetSafeString() + "','" + jcdwitem["lxdh"].GetSafeString() + "','" + jcdwitem["qyfr"].GetSafeString() + "','" + jcdwitem["frsfz"].GetSafeString() + "','" + jcdwitem["xmfzr"].GetSafeString() + "','" + jcdwitem["fzrsfz"].GetSafeString() + "','" + jcdwitem["fzrlxdh"].GetSafeString() + "')";
                                            sqls.Add(sql);
                                        }
                                        else if (jcdwitem["edittype"].GetSafeInt() == 1)
                                        {
                                            sql = "update I_S_GC_XFYS_JCDW_SGZ set qybh='" + jcdwitem["qybh"].GetSafeString() + "',qymc='" + jcdwitem["qymc"].GetSafeString() + "',qyzz='" + jcdwitem["qyzz"].GetSafeString() + "',zzzh='" + jcdwitem["zzzh"].GetSafeString() + "',qyfzr='" + jcdwitem["qyfzr"].GetSafeString() + "',lxdh='" + jcdwitem["lxdh"].GetSafeString() + "',qyfr='" + jcdwitem["qyfr"].GetSafeString() + "',frsfz='" + jcdwitem["frsfz"].GetSafeString() + "',xmfzr='" + jcdwitem["xmfzr"].GetSafeString() + "',fzrsfz='" + jcdwitem["fzrsfz"].GetSafeString() + "',fzrlxdh='" + jcdwitem["fzrlxdh"].GetSafeString() + "' where fxbaid='" + jcdwitem["fxbaid"].GetSafeString() + "' and recid='" + jcdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                        else
                                        {
                                            sql = "delete from I_S_GC_XFYS_JCDW_SGZ where fxbaid='" + jcdwitem["fxbaid"].GetSafeString() + "' and recid='" + jcdwitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                    }
                                }
                                //单体建筑
                                if (dtjzlist.Count > 0)
                                {
                                    foreach (Dictionary<string, object> dtjzitem in dtjzlist)
                                    {
                                        if (dtjzitem["edittype"].GetSafeInt() == 0)
                                        {
                                            sql = "insert into I_S_GC_XFYS_FGC(FXBAID,FGCMC,JGXS,SYXZ,NHDJ,DSCS,DXCS,JZGD,ZDMJ,DSJZMJ,DXJZMJ,SYXZ1,SYXZ2) values('" + guid + "','" + dtjzitem["fgcmc"].GetSafeString() + "','" + dtjzitem["jgxs"].GetSafeString() + "','" + dtjzitem["syxz"].GetSafeString() + "','" + dtjzitem["nhdj"].GetSafeString() + "','" + dtjzitem["dscs"].GetSafeString() + "','" + dtjzitem["dxcs"].GetSafeDecimal() + "','" + dtjzitem["jzgd"].GetSafeDecimal() + "','" + dtjzitem["zdmj"].GetSafeDecimal() + "','" + dtjzitem["dsjzmj"].GetSafeDecimal() + "','" + dtjzitem["dxjzmj"].GetSafeDecimal() + "','" + dtjzitem["syxz1"].GetSafeString() + "','" + dtjzitem["syxz2"].GetSafeString() + "')";
                                            sqls.Add(sql);
                                        }
                                        else if (dtjzitem["edittype"].GetSafeInt() == 1)
                                        {
                                            sql = "update I_S_GC_XFYS_FGC set FGCMC='" + dtjzitem["fgcmc"].GetSafeString() + "',JGXS='" + dtjzitem["jgxs"].GetSafeString() + "',SYXZ='" + dtjzitem["syxz"].GetSafeString() + "',NHDJ='" + dtjzitem["nhdj"].GetSafeString() + "',DSCS='" + dtjzitem["dscs"].GetSafeString() + "',DXCS='" + dtjzitem["dxcs"].GetSafeDecimal() + "',JZGD='" + dtjzitem["jzgd"].GetSafeDecimal() + "',ZDMJ='" + dtjzitem["zdmj"].GetSafeDecimal() + "',DSJZMJ='" + dtjzitem["dsjzmj"].GetSafeDecimal() + "',DXJZMJ='" + dtjzitem["dxjzmj"].GetSafeDecimal() + "',SYXZ1='"+ dtjzitem["syxz1"].GetSafeString() + "',SYXZ2='"+ dtjzitem["syxz2"].GetSafeString() + "'  where fxbaid='" + dtjzitem["fxbaid"].GetSafeString() + "' and recid='" + dtjzitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                        else
                                        {
                                            sql = "delete from I_S_GC_XFYS_FGC where fxbaid='" + dtjzitem["fxbaid"].GetSafeString() + "' and recid='" + dtjzitem["recid"].GetSafeInt() + "'";
                                            sqls.Add(sql);
                                        }
                                    }
                                }
                                code = CommonService.ExecTrans(sqls, out msg);
                            }
                        }
                    }
                    else
                    {
                        code = false;
                        msg = dt[0]["msg"].GetSafeString();
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }
        #endregion

        #endregion

        #region 获取现场验收记录
        [Authorize]
        public void getXcysjl()
        {
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string serialno = Request["serialno"].GetSafeString();
            int totalcount = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "";
                sql = "select recid,jdjl from I_S_GC_XFYSJL where serialno='" + serialno + "'";
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
        #endregion

        #region 获取地址 
        [Authorize]
        public void GetSxqy()
        {
            string err = "";
            string szsf = Request["szsf"].GetSafeString();
            string szcs = Request["szcs"].GetSafeString();
            string szxq = Request["szxq"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> szqy = new Dictionary<string, object>();
            IList<object> szqylist = new List<object>();
            try
            {
                string sql = "";
                if (szxq != "")
                {
                    sql = "select distinct jdid,szjd from h_city where szxq='" + szxq + "'";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szjd", datas);
                    szqylist.Add(szqy);
                }
                else if (szcs != "")
                {
                    sql = "select distinct xqid,szxq from h_city where szcs='" + szcs + "'";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szxq", datas);
                    if (datas.Count > 0)
                    {
                        szxq = datas[0]["szxq"].GetSafeString();
                    }
                    sql = "select distinct jdid,szjd from h_city where szxq='" + szxq + "'";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szjd", datas);
                    szqylist.Add(szqy);
                }
                else if (szsf != "")
                {
                    sql = "select distinct sfid,szsf from h_city";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szsf", datas);
                    sql = "select distinct csid,szcs from h_city where szsf='" + szsf + "'";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szcs", datas);
                    if (datas.Count > 0)
                    {
                        szcs = datas[0]["szcs"].GetSafeString();
                    }
                    sql = "select distinct xqid,szxq from h_city where szcs='" + szcs + "'";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szxq", datas);
                    if (datas.Count > 0)
                    {
                        szxq = datas[0]["szxq"].GetSafeString();
                    }
                    sql = "select distinct jdid,szjd from h_city where szxq='" + szxq + "'";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szjd", datas);
                    szqylist.Add(szqy);
                }
                else
                {
                    sql = "select distinct sfid,szsf from h_city";
                    datas = CommonService.GetDataTable(sql);
                    szqy.Add("szsf", datas);
                    szqylist.Add(szqy);
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(szqylist)));
                Response.End();
            }
        }
        #endregion

        #region 保存工程坐标
        [Authorize]
        public void SaveGczb()
        {
            string fxbaid = Request["fxbaid"].GetSafeString();
            string gczb = Request["gczb"].GetSafeString();
            string msg = "";
            bool code = true;
            Dictionary<string, string> datas = new Dictionary<string, string>();
            try
            {
                IList<string> sqls = new List<string>();
                sqls.Add("update I_M_GC_XFYS set GCZB='" + gczb + "' where Fxbaid='" + fxbaid + "'");
                code = CommonService.ExecTrans(sqls, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }
        #endregion

        #region 获取导入界面
        [Authorize]
        public ContentResult ImportJm()
        {

            string Import = "";
            Import = "ImportDtjz";

            StringBuilder str = new StringBuilder();

            str.Append(" <style type=\"text/css\">             ");
            str.Append(" .tableContent {                      ");
            str.Append("     width: 100%;                     ");
            str.Append("     border-collapse: separate;       ");
            str.Append("     border-spacing: 15px;            ");
            str.Append(" }                                    ");
            str.Append("                                      ");
            str.Append(" .tableContent .tableName {           ");
            str.Append("     text-align: right;               ");
            str.Append("     width: 100px;                    ");
            str.Append(" }                                    ");
            str.Append("                                      ");
            str.Append(" .tableContent .tableValue {          ");
            str.Append(" 	text-align: left;                 ");
            str.Append(" }                                    ");
            str.Append("                                      ");
            str.Append(" .tableContent .tableValue input {    ");
            str.Append("     width: 180px;                    ");
            str.Append(" }                                    ");
            str.Append("                                      ");
            str.Append(" .tableContent .tableValue textarea { ");
            str.Append("     width:550px;                     ");
            str.Append(" }                                    ");
            str.Append("                                      ");
            str.Append("  </style>                            ");

            str.Append("<div align=\"center\">");
            str.Append("<form id=\"protypemenu_importForm\" action=\"/wlxf/" + Import + "\" method=\"post\" enctype=\"multipart/form-data\">");
            str.Append("<table class=\"tableContent\">");
            str.Append("<br />");
            str.Append("<tr>");
            str.Append("<td class=\"tableName\">导入文件</td>");
            str.Append("<td class=\"tableValue\"><input type=\"file\" name=\"ZBINPUTFILE\" accept=\".xls,.xlsx\"/></td>");
            str.Append("</tr> ");
            str.Append("</table>");
            str.Append("</form>");
            str.Append("</div>");

            return Content(str.ToString());
        }
        #endregion

        #region 导入单体建筑
        [Authorize]
        public void ImportDtjz()
        {
            string fxbaid = Request["fxbaid"].GetSafeString();
            string zt = Request["zt"].GetSafeString();
            bool code = true;
            string msg = "";
            try
            {
                if (zt != "SL")
                {
                    Stream stream = null;
                    IWorkbook workbook = null;
                    ISheet sheet;
                    IList<string> sqls = new List<string>();  
                    try
                    {
                        if (Request.Files.Count <= 0)
                        {
                            code = false;
                            msg = "请选中一个excel文件上传!";
                        }
                        else
                        {
                            HttpPostedFileBase postfile = Request.Files[0];
                            if (postfile.ContentLength != 0)
                            {
                                stream = postfile.InputStream;
                                string filename = postfile.FileName;
                                if (filename.IndexOf(".xlsx") <= 0 && filename.IndexOf(".xls") <= 0)
                                {
                                    code = false;
                                    msg = "选择的不是excel文件!";
                                }

                                // 2007版本  
                                if (filename.IndexOf(".xlsx") > 0)
                                    workbook = new XSSFWorkbook(stream);
                                // 2003版本  
                                else if (filename.IndexOf(".xls") > 0)
                                    workbook = new HSSFWorkbook(stream);

                                //hssfworkbook = new HSSFWorkbook(stream);
                                //HSSFSheet ws = (HSSFSheet)hssfworkbook.GetSheetAt(0);
                                sheet = workbook.GetSheetAt(0);
                                ISheet ws = sheet;
                                if (ws != null)//工作薄中没有工作表
                                {
                                    int rowNum = ws.LastRowNum;
                                    int blanksum = 0;
                                    if (rowNum>=1)
                                    {
                                        for (int i = 1; i <= rowNum; i++)
                                        {
                                            String sq = "insert into I_S_GC_XFYS_FGC(FXBAID,FGCMC,JGXS,SYXZ,NHDJ,DSCS,DXCS,JZGD,ZDMJ,DSJZMJ,DXJZMJ) values ";
                                            IDictionary<string, string> row = new Dictionary<string, string>();
                                            IRow tprow = (IRow)ws.GetRow(i);

                                            int rfirst = sheet.FirstRowNum;
                                            int rlast = sheet.LastRowNum;

                                            int cfirst = tprow.FirstCellNum;
                                            int clast = tprow.LastCellNum;

                                            ICell cell0= (ICell)tprow.GetCell(0);
                                            if (cell0.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell1 = (ICell)tprow.GetCell(1);
                                            if (cell1.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell2 = (ICell)tprow.GetCell(2);
                                            if (cell2.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell3 = (ICell)tprow.GetCell(3);
                                            if (cell3.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell4 = (ICell)tprow.GetCell(4);
                                            if (cell4.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell5 = (ICell)tprow.GetCell(5);
                                            if (cell5.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell6 = (ICell)tprow.GetCell(6);
                                            if (cell6.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell7 = (ICell)tprow.GetCell(7);
                                            if (cell7.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell8 = (ICell)tprow.GetCell(8);
                                            if (cell8.GetSafeString().Trim() == "")
                                                blanksum++;
                                            ICell cell9 = (ICell)tprow.GetCell(9);
                                            if (cell9.GetSafeString().Trim() == "")
                                                blanksum++;
                                           

                                            sq = sq + "('" + fxbaid + "',";
                                            sq = sq + "'" + cell0.GetSafeString() + "',";
                                            sq = sq + "'" + cell1.GetSafeString() + "',";
                                            sq = sq + "'" + cell2.GetSafeString() + "',";
                                            sq = sq + "'" + cell3.GetSafeString() + "',";
                                            sq = sq + "'" + cell4.GetSafeString() + "',";
                                            sq = sq + "'" + cell5.GetSafeString() + "',";
                                            sq = sq + "'" + cell6.GetSafeString() + "',";
                                            sq = sq + "'" + cell7.GetSafeString() + "',";
                                            sq = sq + "'" + cell8.GetSafeString() + "',";
                                            sq = sq + "'" + cell9.GetSafeString() + "')";
                                            sqls.Add(sq);

                                        }
                                        workbook.Close();
                                        stream.Close();
                                        if (blanksum == 0)
                                        {
                                            code = CommonService.ExecTrans(sqls, out msg);
                                            JavaScriptSerializer jss = new JavaScriptSerializer();
                                        }
                                        else
                                        {
                                            code = false;
                                            msg = "必填项不能为空！";
                                        }
                                    }
                                    else
                                    {
                                        code = false;
                                        msg = "excel为空！";
                                    }
                                }
                                else
                                {
                                    code = false;
                                    msg = "工作薄中没有工作表！";
                                }
                            }
                            else
                            {
                                code = false;
                                msg = "请选中一个excel文件上传！";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (workbook != null)
                        {
                            workbook.Close();
                        }
                        if (stream != null)
                        {
                            stream.Close();
                        }
                        code = false;
                        msg = ex.Message;
                    }
                }
                else
                {
                    code = false;
                    msg = "该工程已经受理无法导入！";                  
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }
        #endregion

        #region 更新验收记录
        [Authorize]
        public void updatexfysjl()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recids = Request["recids"].GetSafeString();
                int iswt = Request["iswt"].GetSafeInt();
                IList<string> sqls = new List<string>();
                string[] recidlist = recids.Split(',');
                if (recidlist.Length > 0)
                {
                    foreach (string recid in recidlist)
                    {
                        string sql = "update i_s_gc_xfysjl set iswt="+iswt+" where recid="+recid.GetSafeInt()+"";
                        sqls.Add(sql);
                    }
                }               
                code = CommonService.ExecTrans(sqls,out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 录入现场评定信息

        #region 获取现场评定基础信息
        [Authorize]
        public void xcpdGcxx()
        {
            string fxbaid = Request["fxbaid"].GetSafeString();
            string serialno = Request["serialno"].GetSafeString();
            int parentid = Request["parentid"].GetSafeInt();
            int recid = Request["recid"].GetSafeInt();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(100);
            int totalcount = 0;
            int xcpdid = 0;
            string sql = "";
            IDictionary<string, object> xfyspd = new Dictionary<string, object>();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                datas = CommonService.GetDataTable("select recid from i_s_gc_xfys_xcpd where serialno='" + serialno + "' and fxbaid='" + fxbaid + "'");
                if (datas.Count > 0)
                    xcpdid = datas[0]["recid"].GetSafeInt();
                if (recid == 0 && xcpdid == 0)
                {
                    sql = "select recid,gcmc,gcdd,xcpdry,serialno from view_i_s_gc_xfys where serialno='" + serialno + "' and fxbaid='" + fxbaid + "'";
                    datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                    xfyspd.Add("xcpdxx", datas);
                    string xcpdbh = "";
                    sql = "select substring(curmaxserial,7,4) as nf,right(replicate('0',3)+Convert(nvarchar,substring(curmaxserial,11,3)+1),3) as xcpdbh from stserial where recid=49";
                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count > 0)
                    {
                        if (datas[0]["nf"].GetSafeString() == DateTime.Now.Year.GetSafeString())
                            xcpdbh = "XFYSPD" + DateTime.Now.Year.GetSafeString() + datas[0]["xcpdbh"].ToString();
                        else
                            xcpdbh = "XFYSPD" + DateTime.Now.Year.GetSafeString() + "001";
                    }
                    xfyspd.Add("xcpdbh", xcpdbh);
                    totalcount = 2;
                }
                else
                {
                    sql = "select recid,parentid,itemname,itemvalue from i_s_gc_xfys_xcpditem where parentid="+parentid+"";
                    datas = CommonService.GetDataTable(sql);
                    xfyspd.Add("xcpdxx", datas);
                    totalcount = 1;
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(xfyspd)));
                Response.End();
            }
        }
        #endregion

        #region 新增或者更新消防验收评定信息
        [Authorize]
        public void addXfyspd()
        {
            string msg = "";
            bool code = true;
            try
            {
                int recid = Request["recid"].GetSafeInt();
                int parentid = Request["parentid"].GetSafeInt();
                string serialno = Request["serialno"].GetSafeString();
                string fxbaid = Request["fxbaid"].GetSafeString();
                string xfyspd = Request["xfyspd"].GetSafeString();
                int xcpdid = 0;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                IDictionary<string, object> xfyspditem = jss.Deserialize<Dictionary<string, object>>(xfyspd);
                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                IList<string> sqls = new List<string>();
                datas = CommonService.GetDataTable("select recid from i_s_gc_xfys_xcpd where serialno='" + serialno + "' and fxbaid='" + fxbaid + "'");
                if (datas.Count > 0)
                    xcpdid = datas[0]["recid"].GetSafeInt();
                if (recid == 0 && xcpdid == 0)
                {
                    if (xfyspditem.Count > 0)
                    {
                        string sql = "select substring(curmaxserial,7,4) as nf,right(replicate('0',3)+Convert(nvarchar,substring(curmaxserial,11,3)+1),3) as xcpdbh from stserial where recid=49";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            if (datas[0]["nf"].GetSafeString() == DateTime.Now.Year.GetSafeString())
                            {
                                if (datas[0]["xcpdbh"].GetSafeString() != xfyspditem["xcpdbh"].GetSafeString().Substring(10, 3))
                                {
                                    xfyspditem["xcpdbh"] = xfyspditem["xcpdbh"].GetSafeString().Substring(0, 10) + datas[0]["xcpdbh"].GetSafeString();
                                }
                            }
                        }
                        sqls.Add("update stserial set curmaxserial='"+ xfyspditem["xcpdbh"].GetSafeString()+ "' where recid=49");
                        sqls.Add("insert into i_s_gc_xfys_xcpd(fxbaid,serialno,parentid,xcpdbh,gcmc,gcdd,xcpdjl,xcpdry,xcpdsj) values('" + fxbaid + "','" + serialno + "'," + parentid + ",'" + xfyspditem["xcpdbh"].GetSafeString() + "','" + xfyspditem["gcmc"].GetSafeString() + "','" + xfyspditem["gcdd"].GetSafeString() + "','" + xfyspditem["xcpdjl"].GetSafeString() + "','" + xfyspditem["xcpdry"].GetSafeString() + "','" + xfyspditem["xcpdsj"].GetSafeDate() + "')");
                        foreach (KeyValuePair<string, object> kvp in xfyspditem)
                        {
                            sqls.Add("insert into i_s_gc_xfys_xcpditem(parentid,itemname,itemvalue) values(" + parentid + ",'" + kvp.Key + "','" + xfyspditem[kvp.Key] + "')");
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "获取的现场评定记录为空！";
                    }
                }
                else
                {
                    if (xfyspditem.Count > 0)
                    {
                        sqls.Add("update i_s_gc_xfys_xcpd set fxbaid='" +fxbaid + "',serialno='" +serialno + "',parentid=" + parentid + ",xcpdbh='" + xfyspditem["xcpdbh"].GetSafeString() + "',gcmc='" + xfyspditem["gcmc"].GetSafeString() + "',gcdd='" + xfyspditem["gcdd"].GetSafeString() + "',xcpdjl='" + xfyspditem["xcpdjl"].GetSafeString() + "',xcpdry='" + xfyspditem["xcpdry"].GetSafeString() + "',xcpdsj='" + xfyspditem["xcpdsj"].GetSafeDate() + "' where parentid=" + parentid + " and fxbaid='" + fxbaid + "'");
                        foreach (KeyValuePair<string, object> kvp in xfyspditem)
                        {
                            sqls.Add("update i_s_gc_xfys_xcpditem set itemvalue='" + xfyspditem[kvp.Key] + "' where parentid=" + parentid + " and itemname='" + kvp.Key + "'");
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "获取的现场评定记录为空！";
                    }
                }
                code = CommonService.ExecTrans(sqls, out msg);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }
        #endregion


        #endregion

        #region 删除操作
        /// <summary>
        /// 删除临时现场验收工程
        /// </summary>
        [Authorize]
        public void CurrencyDeleteLsxcys()
        {
            bool code = true;
            string msg = "";
            try
            {
                int recid = Request["recid"].GetSafeInt();
                string fxbaid = Request["fxbaid"].GetSafeString();
                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                datas = CommonService.GetDataTable("select * from i_s_gc_xfysjl where fxbaid='"+fxbaid+"' and serialno=''");
                if (datas.Count > 0)
                {
                    code = false;
                    msg = "该临时现场验收工程已有验收记录，无法删除！";
                }
                else
                {
                    IList<string> sqls = new List<string>();
                    sqls.Add("delete from i_s_gc_xfys_lsxcys where recid=" + recid + "");
                    code = CommonService.ExecTrans(sqls);
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

        #region 从台州获取新建或者扩建的工程
        [Authorize]
        public void getAllProject()
        {
            string err = "";
            string projects = "";

            string qymc = Request["qymc"].GetSafeString();
            string sgxkzh = Request["sgxkzh"].GetSafeString();
            try
            { 
                IList<IDictionary<string, string>> datas = CommonService.GetDataTable(string.Format("select top 1 a.* from i_m_qy as a left join i_m_qyzh as b on a.qybh=b.qybh where b.yhzh='{0}'",CurrentUser.UserCode));
                if (qymc == "" && datas.Count > 0)
                    qymc = datas[0]["qymc"].GetSafeString();
                projects = Post("http://tzjzy.jsj.zjtz.gov.cn/tz/GetAllProjectListByQYMCOrSGXKZ", "zjzbh=CP201706000005&qymc=" + qymc + "&sgxkzh=" + sgxkzh + "&timestring=" + ConvertTimeToLong(DateTime.Now).ToString() + "&sign=" + Encode("CP201706000005" + qymc + sgxkzh + ConvertTimeToLong(DateTime.Now).ToString()) + "", "");
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(projects);
                Response.End();
            }
        }

        #region 保存工程信息
        [Authorize]
        public void saveTzProject()
        {
            string msg = "";
            bool code = true;
            try
            {
                string data = Request["data"].GetSafeString();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> dataitem = jss.Deserialize<Dictionary<string, object>>(data);
                IList<string> sqls = new List<string>();               
                string sql = "";
                if (dataitem.Count > 0)
                {
                    Dictionary<string, object> gcxxitem = dataitem["工程信息"] as Dictionary<string, object>;
                    if (gcxxitem.Count > 0)
                    {
                        IList<IDictionary<string, string>> gclist = CommonService.GetDataTable("select * from i_m_gc_xfys where gcbh='" + gcxxitem["gcbh"].GetSafeString() + "'");
                        if (gclist.Count > 0)
                        {
                            code = false;
                            msg = "该工程已经下载到系统，无法重复下载！";
                        }
                        else
                        {
                            string guid = Guid.NewGuid().ToString("N");
                            IList<IDictionary<string, string>> qylist = CommonService.GetDataTable(string.Format("select top 1 a.* from i_m_qy as a left join i_m_qyzh as b on a.qybh=b.qybh where b.yhzh='{0}'", CurrentUser.UserCode));
                            string jsdwmc = "";
                            string jsdwbh = "";
                            string jsdwfzr = "";
                            string jsdwsjh = "";
                            string jsdwfr = "";
                            if (qylist.Count > 0)
                            {
                                jsdwmc = qylist[0]["qymc"].GetSafeString();
                                jsdwbh = qylist[0]["qybh"].GetSafeString();
                                jsdwfzr = qylist[0]["qyfzr"].GetSafeString();
                                jsdwsjh = qylist[0]["lxsj"].GetSafeString();
                                jsdwfr = qylist[0]["qyfr"].GetSafeString();
                            }
                            sql = "insert into i_m_gc_xfys(FXBAID,GCBH,GCMC,GCDD,JSDWMC,JSDWBH,JSDWFZR,JSDWLXDH,ZZDJ,JSDWFR,GCLXBH,LSXJZ,TSXFSJ,ZJZMJ,DSCS,DXCS,GCTZE,KGRQ,JGRQ,GBYT,ISBC,CKQR,ZT,ZTName,LRSJ,LRRZH,LRRXM,SZSF,SZCS,SZXQ,SZJD,GCZB,DCCG)";
                            sql += " values('" + guid + "','" + gcxxitem["gcbh"].GetSafeString() + "','" + gcxxitem["gcmc"].GetSafeString() + "','" + gcxxitem["gcdd"].GetSafeString() + "','" + jsdwmc + "','" + jsdwbh + "','" + jsdwfzr + "','" + jsdwsjh + "','\\','" + jsdwfr + "'";
                            sql += ",'新建','否','否','" + gcxxitem["jzmj"].GetSafeDecimal() + "','" + gcxxitem["cs"].GetSafeDecimal() + "','" + gcxxitem["dxcs"].GetSafeDecimal() + "'";
                            sql += ",'" + gcxxitem["gczj"].GetSafeDecimal() + "','" + gcxxitem["jhkgrq"].GetSafeDate() + "','" + gcxxitem["jhjgrq"].GetSafeDate() + "','否'";
                            sql += ",0,'YT','YT','填报',getdate(),'" + CurrentUser.UserCode + "','" + CurrentUser.RealName + "'";
                            sql += ",'" + gcxxitem["szsf"].GetSafeString() + "','" + gcxxitem["szcs"].GetSafeString() + "','" + gcxxitem["szxq"].GetSafeString() + "','" + gcxxitem["szjd"].GetSafeString() + "','" + gcxxitem["gczb"].GetSafeString() + "','否')";
                            sqls.Add(sql);
                            ArrayList sjdwlist = dataitem["设计单位"] as ArrayList;
                            if (sjdwlist.Count>0)
                            {
                                foreach (Dictionary<string, object> sjdwitem in sjdwlist)
                                {
                                    Dictionary<string, object> sjdw = sjdwitem["单位"] as Dictionary<string, object>;
                                    qylist = CommonService.GetDataTable("select qymc,qybh,qyfr,qyfrsj,qyfzr,lxsj,zzdj,zzzsbh from View_I_M_QY_WITH_ZZ where qymc='" + sjdw["qymc"].GetSafeString() + "'");
                                    if (qylist.Count > 0)
                                    {
                                        sql = "insert into i_s_gc_xfys_sjdw(fxbaid,qybh,qymc,qyzz,zzzh,qyfzr,lxdh,qyfr) values('" + guid + "','" + qylist[0]["qybh"].GetSafeString() + "','" + qylist[0]["qymc"].GetSafeString() + "','" + qylist[0]["zzdj"].GetSafeString() + "','" + qylist[0]["zzzsbh"].GetSafeString() + "','" + qylist[0]["qyfzr"].GetSafeString() + "','" + qylist[0]["lxsj"].GetSafeString() + "','" + qylist[0]["qyfr"].GetSafeString() + "')";
                                        sqls.Add(sql);
                                    }
                                }
                            }
                            ArrayList jldwlist = dataitem["监理单位"] as ArrayList;
                            if (jldwlist.Count > 0)
                            {
                                foreach (Dictionary<string, object> jldwitem in jldwlist)
                                {
                                    Dictionary<string, object> jldw = jldwitem["单位"] as Dictionary<string, object>;
                                    qylist = CommonService.GetDataTable("select qymc,qybh,qyfr,qyfrsj,qyfzr,lxsj,zzdj,zzzsbh from View_I_M_QY_WITH_ZZ where qymc='" + jldw["qymc"].GetSafeString() + "'");
                                    if (qylist.Count > 0)
                                    {
                                        sql = "insert into i_s_gc_xfys_jldw(fxbaid,qybh,qymc,qyzz,zzzh,qyfzr,lxdh,qyfr) values('" + guid + "','" + qylist[0]["qybh"].GetSafeString() + "','" + qylist[0]["qymc"].GetSafeString() + "','" + qylist[0]["zzdj"].GetSafeString() + "','" + qylist[0]["zzzsbh"].GetSafeString() + "','" + qylist[0]["qyfzr"].GetSafeString() + "','" + qylist[0]["lxsj"].GetSafeString() + "','" + qylist[0]["qyfr"].GetSafeString() + "')";
                                        sqls.Add(sql);
                                    }
                                }
                            }
                            ArrayList sgdwlist = dataitem["施工单位"] as ArrayList;
                            if (sgdwlist.Count > 0)
                            {
                                foreach (Dictionary<string, object> sgdwitem in sgdwlist)
                                {
                                    Dictionary<string, object> sgdw = sgdwitem["单位"] as Dictionary<string, object>;
                                    qylist = CommonService.GetDataTable("select qymc,qybh,qyfr,qyfrsj,qyfzr,lxsj,zzdj,zzzsbh from View_I_M_QY_WITH_ZZ where qymc='" + sgdw["qymc"].GetSafeString() + "'");
                                    if (qylist.Count > 0)
                                    {
                                        sql = "insert into i_s_gc_xfys_sgdw(fxbaid,qybh,qymc,qyzz,zzzh,qyfzr,lxdh,qyfr) values('" + guid + "','" + qylist[0]["qybh"].GetSafeString() + "','" + qylist[0]["qymc"].GetSafeString() + "','" + qylist[0]["zzdj"].GetSafeString() + "','" + qylist[0]["zzzsbh"].GetSafeString() + "','" + qylist[0]["qyfzr"].GetSafeString() + "','" + qylist[0]["lxsj"].GetSafeString() + "','" + qylist[0]["qyfr"].GetSafeString() + "')";
                                        sqls.Add(sql);
                                    }
                                }
                            }
                            ArrayList tsdwlist = dataitem["图审单位"] as ArrayList;
                            if (tsdwlist.Count > 0)
                            {
                                foreach (Dictionary<string, object> tsdwitem in tsdwlist)
                                {
                                    qylist = CommonService.GetDataTable("select qymc,qybh,qyfr,qyfrsj,qyfzr,lxsj,zzdj,zzzsbh from View_I_M_QY_WITH_ZZ where qymc='" + tsdwitem["qymc"].GetSafeString() + "'");
                                    if (qylist.Count > 0)
                                    {
                                        sql = "insert into i_s_gc_xfys_tsdw(fxbaid,qybh,qymc,qyzz,zzzh,qyfzr,lxdh,qyfr) values('" + guid + "','" + qylist[0]["qybh"].GetSafeString() + "','" + qylist[0]["qymc"].GetSafeString() + "','" + qylist[0]["zzdj"].GetSafeString() + "','" + qylist[0]["zzzsbh"].GetSafeString() + "','" + qylist[0]["qyfzr"].GetSafeString() + "','" + qylist[0]["lxsj"].GetSafeString() + "','" + qylist[0]["qyfr"].GetSafeString() + "')";
                                        sqls.Add(sql);
                                    }
                                }
                            }
                            ArrayList dtjzlist = dataitem["分工程"] as ArrayList;
                            if (dtjzlist.Count > 0)
                            {
                                foreach (Dictionary<string, object> dtjzitem in dtjzlist)
                                {
                                    sql = "insert into I_S_GC_XFYS_FGC(FXBAID,FGCMC,JGXS,DSCS,DXCS,DSJZMJ) values('" + guid + "','" + dtjzitem["fgcmc"].GetSafeString() + "','" + dtjzitem["jgxs"].GetSafeString() + "','" + dtjzitem["cs"].GetSafeString() + "','" + dtjzitem["dxcs"].GetSafeDecimal() + "','" + dtjzitem["jzmj"].GetSafeDecimal() + "')";
                                    sqls.Add(sql);
                                }
                            }
                            code = CommonService.ExecTrans(sqls, out msg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
                SysLog4.WriteError(e.Message);
            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }
        #endregion

        #region 获取api数据       
        public static string Post(string Url, string Data, string Referer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.Referer = Referer;
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            Stream myResponseStream = request.GetRequestStream();
            myResponseStream.Write(bytes, 0, bytes.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            return retString;
        }
        #endregion

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2018/4/8 14:20
        private static long ConvertTimeToLong(DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }

        //DES加密方式
        private string KEY_64 = "7b387e7e"; //密钥
        private string IV_64 = "ee1695b4";//向量
                                          // Methods
                                          // Methods
        public string Encode(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(KEY_64);
            byte[] rgbIV = Encoding.ASCII.GetBytes(IV_64);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int keySize = provider.KeySize;
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(stream2);
            writer.Write(data);
            writer.Flush();
            stream2.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
        }
        #endregion

        #region 获取使用性质
        [Authorize]
        public void GetHelpSyxz()
        {
            string err = "";
            string dl = Request["dl"].GetSafeString();
            string zl = Request["zl"].GetSafeString();
            
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> syxz = new Dictionary<string, object>();
            IList<object> syxzlist = new List<object>();
            try
            {
                string sql = "";
                if (zl != "")
                {
                    sql = "select distinct xlid,xl from h_syxz where zl='" + zl + "'";
                    datas = CommonService.GetDataTable(sql);
                    syxz.Add("xl", datas);
                    syxzlist.Add(syxz);
                }
                else if (dl != "")
                {
                    IList<IDictionary<string, string>> zldatas = new List<IDictionary<string, string>>();
                    IDictionary<string, object> zllist = new Dictionary<string, object>();
                    IList<IDictionary<string, object>> zlhelp = new List<IDictionary<string, object>>();
                    sql = "select distinct zlid,zl from h_syxz where dl='" + dl + "'";
                    zldatas = CommonService.GetDataTable(sql);
                    for (int j = 0; j < zldatas.Count; j++)
                    {
                        IList<IDictionary<string, string>> xldatas = new List<IDictionary<string, string>>();
                        sql = "select distinct xlid,xl from h_syxz where zlid='" + zldatas[j]["zlid"].GetSafeString() + "' ";
                        xldatas = CommonService.GetDataTable(sql);
                        zllist.Add("bh", zldatas[j]["zlid"].GetSafeString());
                        zllist.Add("title", zldatas[j]["zl"].GetSafeString());
                        zllist.Add("children", xldatas);
                        zlhelp.Add(zllist);
                        zllist = new Dictionary<string, object>();
                    }
                    syxzlist.Add(zlhelp);
                }
                else
                {
                    sql = "select distinct dlid,dl from h_syxz";
                    datas = CommonService.GetDataTable(sql);
                   
                    IList<IDictionary<string, string>> zldatas = new List<IDictionary<string, string>>();
                    IDictionary<string, object> zllist = new Dictionary<string, object>();
                    IList<IDictionary<string, object>> zlhelp = new List<IDictionary<string, object>>();
                    IDictionary<string, object> dllist = new Dictionary<string, object>();
                    IList<IDictionary<string, object>> dlhelp = new List<IDictionary<string, object>>();
                    for (int i = 0;i < datas.Count; i++)
                    {
                        sql = "select distinct zlid,zl from h_syxz where dlid='" + datas[i]["dlid"].GetSafeString() + "'";
                        zldatas = CommonService.GetDataTable(sql);
                        for (int j = 0; j < zldatas.Count; j++)
                        {
                            IList<IDictionary<string, string>> xldatas = new List<IDictionary<string, string>>();
                            sql = "select distinct xlid,xl from h_syxz where zlid='" + zldatas[j]["zlid"].GetSafeString() + "' ";
                            xldatas = CommonService.GetDataTable(sql);
                            zllist.Add("bh", zldatas[j]["zlid"].GetSafeString());
                            zllist.Add("title", zldatas[j]["zl"].GetSafeString());
                            zllist.Add("children",xldatas);
                            zlhelp.Add(zllist);
                            zllist = new Dictionary<string, object>();
                        }
                        dllist.Add("bh", datas[i]["dlid"].GetSafeString());
                        dllist.Add("title", datas[i]["dl"].GetSafeString());                      
                        dllist.Add("children", zlhelp);
                        zlhelp = new List<IDictionary<string, object>>();
                        dlhelp.Add(dllist);
                        dllist = new Dictionary<string, object>();

                    }
                    syxzlist.Add(dlhelp);                 
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
                Response.Write(string.Format("{{\"rows\":{0}}}",jss.Serialize(syxzlist)));
                Response.End();
            }
        }
        #endregion

        #region 大屏综合展现接口
        /// <summary>
        /// 不合格项目分析
        /// </summary>
        [Authorize]
        public void getDpBhgxm()
        {
            string bjsj1 = Request["bjsj1"].GetSafeString();
            string bjsj2 = Request["bjsj2"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> dpzhxx = new Dictionary<string, object>();
            try
            {
                string where = "";
                if (bjsj1 != "")
                    where += "and a.dysj>='" + bjsj1.GetSafeDate() + "' ";
                if (bjsj2 != "")
                    where += "and a.dysj<=DATEADD(SECOND,-1,DATEADD(DAY,1,'" + bjsj2.GetSafeDate() + "')) ";
                string sql = "";
                //if (where !="")
                //    sql = "select count(1) as zgcsl,cast(isnull(sum(isnull(b.gctze,0)),0)/10000 as decimal(18,4)) as zgctze,cast(sum(case when b.zxgc = '否' then cast(isnull(b.zjzmj, 0) as decimal(18, 4)) else cast((case when b.zxmj = '' or b.zxmj is null then '0' else b.zxmj end) as decimal(18, 4)) end)/ 10000 as decimal(18,4)) as zjzmj from i_m_gc_xfys as b where b.fxbaid in (select a.fxbaid from i_s_gc_xfys a where a.zt in('CKSJ','CQQR','BAQR') " + where + ")";
                //else
                //    sql = "select count(1) as zgcsl,cast(isnull(sum(isnull(a.gctze,0)),0)/10000 as decimal(18,4)) as zgctze,cast(sum(case when a.zxgc = '否' then cast(isnull(a.zjzmj, 0) as decimal(18, 4)) else cast((case when a.zxmj = '' or a.zxmj is null then '0' else a.zxmj end) as decimal(18, 4)) end)/ 10000 as decimal(18,4)) as zjzmj from i_m_gc_xfys as a";
                //datas = CommonService.GetDataTable(sql);
                //dpzhxx.Add("gcxx",datas);
                sql = "select count(1) as ccxmsl,(select count(1) from i_s_gc_xfys as a where a.zt='BAQR' and a.typebh='02' and a.ishg='该工程消防验收不合格' " + where + ") as xfysbhgsl";
                sql += ",(select count(1) from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh = '03' and a.ishg = '该工程不符合建设工程消防验收有关规定' " + where + ") as xfbabhgsl";
                sql += ",(select count(1) from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh = '05' and a.ishg = '该工程不符合建设工程消防验收有关规定' " + where + ") as xfbafcbhgsl";
                sql += ",(select count(1) from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh = '06' and a.ishg = '该工程消防验收不合格' " + where + ") as xfysfybhgsl";
                sql += " from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh in('02','03','05','06') " + where + "";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("ccxm", datas);
                sql = "select b.zxsyxz1,count(1) as sl from i_s_gc_xfys as a left join i_m_gc_xfys as b on a.fxbaid=b.fxbaid where a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') and isnull(b.zxsyxz1,'')<>'' " + where + " group by b.zxsyxz1 order by b.zxsyxz1";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("syxz", datas);
                sql = "select b.szjd,count(1) as sl from i_s_gc_xfys as a left join i_m_gc_xfys as b on a.fxbaid = b.fxbaid where a.ishg like '%不%' and a.zt = 'BAQR' and a.typebh in('02','03','05','06') and isnull(b.szjd,'')<>'' " + where + " group by b.szjd order by b.szjd";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("szjd", datas);
                IDictionary<string,object> wtlist = new Dictionary<string, object>();
                IList<IDictionary<string, object>> wtjh = new List<IDictionary<string, object>>();
                IList<IDictionary<string, string>> wtlxs = CommonService.GetDataTable("select * from h_wtlx where isqy=1");
                if (wtlxs.Count>0)
                {
                    foreach (IDictionary<string,string> wt in wtlxs)
                    {
                        wtlist.Add("wt",wt["wt"].GetSafeString());
                        datas = CommonService.GetDataTable("select count(1) as sl from i_s_gc_xfysjl as b left join i_s_gc_xfys as a on b.serialno=a.serialno where a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') and b.jdjl like '%" + wt["wt"].GetSafeString() + "%' " + where + "");
                        wtlist.Add("sl", datas[0]["sl"].GetSafeString());
                        wtjh.Add(wtlist);
                        wtlist = new Dictionary<string, object>();
                    }
                    if (wtjh.Count > 5)
                    {
                        wtjh = wtjh.OrderByDescending(x => x["sl"].GetSafeInt()).Take(5).ToList();
                    }
                }
                dpzhxx.Add("wtfx", wtjh);
                sql = "select b.fxbaid,b.gcmc,b.gczb from i_s_gc_xfys as a left join i_m_gc_xfys as b on a.fxbaid=b.fxbaid where a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') " + where + "";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("gczb", datas);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"rows\":{0}}}", jss.Serialize(dpzhxx)));
                Response.End();
            }
        }

        /// <summary>
        /// 阶段性抽查分析
        /// </summary>
        [Authorize]
        public void getDpJdxcc()
        {
            string bjsj1 = Request["bjsj1"].GetSafeString();
            string bjsj2 = Request["bjsj2"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IDictionary<string, object> dpzhxx = new Dictionary<string, object>();
            try
            {
                string where = "";
                if (bjsj1 != "")
                    where += "and a.dysj>='" + bjsj1.GetSafeDate() + "' ";
                if (bjsj2 != "")
                    where += "and a.dysj<=DATEADD(SECOND,-1,DATEADD(DAY,1,'" + bjsj2.GetSafeDate() + "')) ";
                string sql = "";
                //if (where !="")
                //    sql = "select count(1) as zgcsl,cast(isnull(sum(isnull(b.gctze,0)),0)/10000 as decimal(18,4)) as zgctze,cast(sum(case when b.zxgc = '否' then cast(isnull(b.zjzmj, 0) as decimal(18, 4)) else cast((case when b.zxmj = '' or b.zxmj is null then '0' else b.zxmj end) as decimal(18, 4)) end)/ 10000 as decimal(18,4)) as zjzmj from i_m_gc_xfys as b where b.fxbaid in (select a.fxbaid from i_s_gc_xfys a where a.zt in('CKSJ','CQQR','BAQR') " + where + ")";
                //else
                //    sql = "select count(1) as zgcsl,cast(isnull(sum(isnull(a.gctze,0)),0)/10000 as decimal(18,4)) as zgctze,cast(sum(case when a.zxgc = '否' then cast(isnull(a.zjzmj, 0) as decimal(18, 4)) else cast((case when a.zxmj = '' or a.zxmj is null then '0' else a.zxmj end) as decimal(18, 4)) end)/ 10000 as decimal(18,4)) as zjzmj from i_m_gc_xfys as a";
                //datas = CommonService.GetDataTable(sql);
                //dpzhxx.Add("gcxx",datas);
                sql = "select count(1) as ccxmsl,(select count(1) from i_s_gc_xfys as a where a.zt='BAQR' and a.typebh='02' and a.ishg='该工程消防验收不合格' " + where + ") as xfysbhgsl";
                sql += ",(select count(1) from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh = '03' and a.ishg = '该工程不符合建设工程消防验收有关规定' " + where + ") as xfbabhgsl";
                sql += ",(select count(1) from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh = '05' and a.ishg = '该工程不符合建设工程消防验收有关规定' " + where + ") as xfbafcbhgsl";
                sql += ",(select count(1) from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh = '06' and a.ishg = '该工程消防验收不合格' " + where + ") as xfysfybhgsl";
                sql += " from i_s_gc_xfys as a where a.zt = 'BAQR' and a.typebh in('02','03','05','06') " + where + "";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("ccxm", datas);
                sql = "select count(1) as sl from i_s_gc_xfys as a where a.issclxd='是' and a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') " + where + "";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("lxdsl", datas);
                sql = "select count(1) as sl from i_s_gc_xfys as a left join i_s_gc_xfysjl as b on a.serialno=b.serialno where a.issclxd='是' and a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') " + where + "";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("wtsl", datas);
                sql = "select b.zxsyxz1,count(1) as sl from i_s_gc_xfys as a left join i_m_gc_xfys as b on a.fxbaid=b.fxbaid where a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') and isnull(b.zxsyxz1,'')<>'' " + where + " group by b.zxsyxz1 order by b.zxsyxz1";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("syxz", datas);
                //sql = "select b.szjd,count(1) as sl from i_s_gc_xfys as a left join i_m_gc_xfys as b on a.fxbaid = b.fxbaid where a.ishg like '%不%' and a.zt = 'BAQR' and a.typebh in('02','03','05','06') and isnull(b.szjd,'')<>'' " + where + " group by b.szjd order by b.szjd";
                //datas = CommonService.GetDataTable(sql);
                //dpzhxx.Add("szjd", datas);
                IDictionary<string, object> wtlist = new Dictionary<string, object>();
                IList<IDictionary<string, object>> wtjh = new List<IDictionary<string, object>>();
                IList<IDictionary<string, string>> wtlxs = CommonService.GetDataTable("select * from h_wtlx where isqy=1");
                if (wtlxs.Count > 0)
                {
                    foreach (IDictionary<string, string> wt in wtlxs)
                    {
                        wtlist.Add("wt", wt["wt"].GetSafeString());
                        datas = CommonService.GetDataTable("select count(1) as sl from i_s_gc_xfysjl as b left join i_s_gc_xfys as a on b.serialno=a.serialno where a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') and b.jdjl like '%" + wt["wt"].GetSafeString() + "%' " + where + "");
                        wtlist.Add("sl", datas[0]["sl"].GetSafeString());
                        wtjh.Add(wtlist);
                        wtlist = new Dictionary<string, object>();
                    }
                    if (wtjh.Count > 5)
                    {
                        wtjh = wtjh.OrderByDescending(x => x["sl"].GetSafeInt()).Take(5).ToList();
                    }
                }
                dpzhxx.Add("wtfx", wtjh);
                sql = "select b.fxbaid,b.gcmc,b.gczb from i_s_gc_xfys as a left join i_m_gc_xfys as b on a.fxbaid=b.fxbaid where a.ishg like '%不%' and a.zt='BAQR' and a.typebh in('02','03','05','06') " + where + "";
                datas = CommonService.GetDataTable(sql);
                dpzhxx.Add("gczb", datas);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"rows\":{0}}}", jss.Serialize(dpzhxx)));
                Response.End();
            }
        }

        #endregion

        #region 获取测绘密钥
        public void getChxtKey()
        {
            string strkey = "";
            IDictionary<string, object> datas = new Dictionary<string, object>();
            try
            {
                strkey = Post("http://127.0.0.1:8090/SR/u!loginkey", "", "");
                datas.Add("key",strkey);
                datas.Add("name",CurrentUser.RealName);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"rows\":{0}}}", jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 浙里办用户单点登录校验
        public ActionResult PhoneZlbQyDl()
        {
            bool success = true;
            string msg = "";
            try
            {

                //string url = "https://puser.zjzwfw.gov.cn/sso/mobile.do?action=oauth&scope=1&servicecode=【接入代码】&goto=【附带跳转地址，以sp参数返回】";

            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);

            }
            if (success)
            {
                return new RedirectResult("/user/mainyh");
            }
            else
            {
                ViewBag.error = msg;
                return View();
            }
        }
        #endregion

        #region 生成验证码
        public ActionResult SecurityCode()
        {
            //string oldcode = Session["USERLOGIN_SecurityCode"] as string;
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
            string allChar = "1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,g,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
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
                int t = rand.Next(32);
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
                Font font = new Font("Arial", 15, (FontStyle.Bold | FontStyle.Italic));
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

        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        public static string DecodeBase64(Encoding encode, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                return encode.GetString(bytes);
            }
            catch
            {
                return result;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        [Authorize]
        public void DoChangePass()
        {
            string username = Request["username"].GetSafeString();
            string oldpass = Request["pass1"].GetSafeString();
            string newpass = Request["pass2"].GetSafeString();
            string err = "";
            oldpass = DecodeBase64(oldpass);
            newpass = DecodeBase64(newpass);
            bool ret = false;
            try
            {
                string RealUserName = "";
                string json = "";
                //string RealUserName = UserService.GetUserCode(username) ;
                if (username != "")
                {
                    RealUserName = username;
                }
                else
                {
                    RealUserName = CurrentUser.RealUserName;
                    //ret = true;
                }

                ret = Remote.UserService.Login(RealUserName, oldpass, out err);

                if (ret)
                {
                    json = UserService.ReSetPassWord(RealUserName, newpass);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Dictionary<string, object> datas = jss.Deserialize<Dictionary<string, object>>(json);
                    if (datas.Count > 0)
                    {
                        ret = datas["success"].GetSafeBool();
                    }
                    //JToken jsons = JToken.Parse(json);//转化为JToken（JObject基类）
                    //ret = jsons["success"].GetSafeBool();
                }
                else
                {
                    if (err == "")
                        err = "账户密码验证失败，请确认输入正确账户和密码";
                }
                
                if (ret)
                {
                    Session["USER_INFO_USERNAME"] = CurrentUser.RealUserName;
                    Session["USER_INFO_PASSWORD"] = newpass;

                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select updatetime from U_UserP where UserCode='" + username + "'");
                    if (dt.Count > 0)
                    {
                        CommonService.Execsql("update [U_UserP] set [Updatetime]=getdate() where [UserCode]='" + username + "'");

                    }
                    else
                    {
                        CommonService.Execsql("INSERT INTO [U_UserP]([UserCode],[Updatetime])VALUES('" + username + "',getdate())");
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = "程序出错";//err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }
        #endregion

        #region 忘记密码时校验
        public void CheckRegister2()
        {
            string msg = "";
            bool code = false;
            try
            {
                string username = Request["username"].GetSafeString();
                string zhtype = Request["zhtype"].GetSafeString();
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["REGISTER_VERIFY_CODE"] as string;// 验证码,时间
                string pwd = Request["pwd"].GetSafeString();
                if (yzmE == null)
                    msg = "验证码无效，请点击“发送验证码”获取验证码";
                else
                {
                    string[] arr = yzmE.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length < 3)
                    {
                        code = false;
                        msg = "验证码无效，请点击“发送验证码”获取验证码";
                    }
                    else
                    {
                        DateTime timeOld = arr[1].GetSafeDate();
                        string yzmEs = arr[0];
                        string phone = arr[2].GetSafeString();
                        int vcminutes = Func.GlobalVariable.GetSmsRegisterVerifyCodeSeconds();
                        if (DateTime.Now.Subtract(timeOld).TotalMinutes > vcminutes)
                            msg = "验证码已超时，请重新获取验证码";
                        else if (!yzmEs.Equals(yzm, StringComparison.OrdinalIgnoreCase))
                            msg = "验证码错误，请输入正确的验证码";
                        else
                        {

                            if (!GetQYSJHM(phone, username, zhtype))
                            {
                                code = false;
                                msg = "该手机号与该单位账号不对应";
                            }
                            else
                            {
                                Session["REGISTER_VERIFY_CODE"] = null;
                                string json = UserService.ReSetPassWord(username, pwd);

                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                Dictionary<string, object> datas = jss.Deserialize<Dictionary<string, object>>(json);
                                if (datas.Count > 0)
                                {
                                    code = datas["success"].GetSafeBool();
                                    msg = datas["msg"].GetSafeString();
                                }

                                //JToken jsons = JToken.Parse(json);//转化为JToken（JObject基类）
                                //code = jsons["success"].GetSafeBool();
                                //msg = jsons["msg"].GetSafeString();
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                msg = "程序出错";//msg = e.Message;;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        private bool GetQYSJHM(string sjhm, string qyzh, string zhtype)
        {
            bool ret = false;
            string msg = "";
            try
            {
                string tablename = "I_M_QY";

                string sql = "select * from " + tablename + " where lxsj='" + sjhm + "' and zh='" + qyzh + "'";

                if (zhtype == "N")
                {
                    sql = "select SJHM from  I_M_NBRY  where SJHM='" + sjhm + "' and zh='" + qyzh + "'";
                }
                if (zhtype == "R")
                {
                    sql = "select SJHM from  i_M_RY  where SJHM='" + sjhm + "' and zh='" + qyzh + "'";
                }

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    ret = true;
                }
                else
                    ret = false;
            }
            catch (Exception e)
            {

            }
            return ret;
        }
        #endregion
    }

}