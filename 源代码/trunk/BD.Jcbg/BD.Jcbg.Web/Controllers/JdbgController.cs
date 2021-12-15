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
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using BD.Jcbg.Web.Func;
using EsignSharp.service;
using EsignSharp.service.impl;
using EsignUtils.utils.bean.result;
using System.Collections;
using EsignUtils.utils.bean;
using EsignUtils.utils.bean.constant;
using iTextSharp.text;
using iTextSharp.text.pdf;
using BD.Jcbg.Web.Remote;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace BD.Jcbg.Web.Controllers
{
    public class JdbgController:Controller
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

        private  IWorkFlowService _workflowService = null;
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

        private ISmsServiceWzzjz _smsServiceWzzjz = null;
        private ISmsServiceWzzjz SmsServiceWzzjz
        {
            get
            {
                if (_smsServiceWzzjz == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsServiceWzzjz = webApplicationContext.GetObject("SmsServiceWzzjz") as ISmsServiceWzzjz;
                }
                return _smsServiceWzzjz;
            }
        }
        #endregion
        #region 页面
        /// <summary>
        /// 选择分工程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Fgcxz()
        {
            ViewBag.fgc = Request["fgc"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 工程合并
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gchb()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 监督站内部工程查看，包含所有信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gccknb()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.gclxbh = Request["gclxbh"].GetSafeString();
            return View();
        }
        // 外部人员查看工程
        public ActionResult Gcckwb()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }
		/// <summary>
        /// 人员投诉反馈审批
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Rytsfksp()
        {
            return View();
        }
        /// <summary>
        /// 根据工程编号，组成流程中的发起任务url
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult StartGcWork()
        {
            string url = "";

            string gcbh = Request["gcbh"].GetSafeString();
            string fgcbh = Request["fgcbh"].GetSafeString();
            string processid=Request["processid"].GetSafeString();
            string callbackjs = Request["callbackjs"].GetSafeString();

            if (gcbh == "")
                url = "/workflow/startwork?processid=" + processid;
            else
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zjdjh,gcmc from i_m_gc where gcbh='" + gcbh + "'");
                string zjdjh = "";
                string gcmc = "";
                if (dt.Count > 0)
                {
                    zjdjh = dt[0]["zjdjh"].GetSafeString();
                    gcmc = dt[0]["gcmc"].GetSafeString();
                }
                string extrainfo1 = ("view_i_m_gc|" + ("gcbh='" + gcbh + "'").EncodeBase64()).EncodeBase64();
                string extrainfo2 = ("[" + zjdjh + "]" + gcmc).EncodeBase64();
                string extrainfo3 = gcbh.EncodeBase64();
                string extrainfo4 = "";
                if (fgcbh != "")
                    extrainfo4 = fgcbh.EncodeBase64();
                url = "/workflow/startwork?processid=" + processid + "&preurldone=true&extrainfo=" + Server.UrlEncode(extrainfo1) + 
                    "&extrainfo2=" + Server.UrlEncode(extrainfo2) + 
                    "&extrainfo3=" + Server.UrlEncode(extrainfo3) + 
                    "&extrainfo4=" + Server.UrlEncode(extrainfo4) +
                    "&callbackjs="+Server.UrlEncode(callbackjs);
            }
            return new RedirectResult(url);
        }

        /// <summary>
        /// 流程报表 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult FlowReport()
        {
            
            string url = "";
            string reportFile = Request["reportfile"].GetSafeString();
            string serial = Request["serial"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int jdjlid = Request["jdjlid"].GetSafeInt();
            int isprint = Request["print"].GetSafeInt(1);
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
            c.libType = ReportPrint.LibType.OpenXmlSdk;
            c.openType = ReportPrint.OpenType.PDF;
            
            //c.field = reportFile;
            c.fileindex = "0";
			c.table = "stformitem|view_i_m_gc|view_gc_ry|view_gc_qy|view_gc_xctp|jdbg_jdjl_xq|jdbg_jdjl|view_zgdhf_ztfj|view_zgdhf_zgtmhffj|view_zgd_zgtmfj|view_jdbg_zgdcfjl_last|view_zgdyq_ztfj|view_zgd_zgtm|view_jgys_fjmc|view_jgys_fj";          
            c.filename=reportFile;
            //c.field = "formid";

            c.where = "formid=" + formid + "|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|parentid=" + jdjlid + "|recid=" + jdjlid + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid + "|workserial='" + serial + "'" + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid + "|xformid=" + formid;

            c.signindex = 0;
            if (isprint == 1)
                c.customtools = "1,|2,|12,下载";
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

            #region 获取电子签章
            // 如果启用了电子签章，生成签章的文件
            bool enablesign = GlobalVariable.GetConfigValue("EnableESign") == "true";
            string signedfilename = "";

            
            if (enablesign)
            {

                if (GetSignedPdf(g, c, rd, out signedfilename))
                {
                    
                    c.fileindex = "2";
                    c.openType = ReportPrint.OpenType.PDFFile;
                    c.filename = signedfilename;
                }
                else
                {
                    return Content(signedfilename);
                }
            }
            #endregion


            // 启用电子签章并且是下载文件（APP中使用）
            if (enablesign && type == "download")
            {
                string filepath = Server.MapPath("~\\report\\pdftemp") + "\\" + signedfilename + ".pdf";
                if (System.IO.File.Exists(filepath))
                {
                    var myBytes = System.IO.File.ReadAllBytes(filepath);
                    string  mime = "application/pdf";
                    return File(myBytes, mime, signedfilename + ".pdf");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                if (type == "download")
                {
                    c.openType = ReportPrint.OpenType.PDFFileDown;
                }

                var guid = g.Add(c);
                url = "/reportPrint/Index?" + guid;
                //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
                return new RedirectResult(url);
            }
            
            

        }
        /// <summary>
        /// 务工人员打印
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WGRYReportDown()
        {
            try
            {
                string url = "";
                string reportFile = Request["reportfile"].GetSafeString();
                string tablename = Request["tablename"].GetSafeString();
                string where = Request["where"].GetSafeString();
                int type = Request["type"].GetSafeInt();
                string opentype = Request["opentype"].GetSafeString();
                int allowedit = Request["allowedit"].GetSafeInt();

                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();
                // c.type = ReportPrint.EnumType.Excel;
                if (type == 1)
                    c.type = (ReportPrint.EnumType)type;
                else
                    c.type = ReportPrint.EnumType.Word;
                //c.field = reportFile;
                c.fileindex = "0";

                c.filename = reportFile;
                c.table = tablename;
                c.where = where;
                if (opentype == "filedown")
                    c.openType = ReportPrint.OpenType.FileDown;
                else if (opentype == "word")
                    c.openType = ReportPrint.OpenType.Print;
                else
                    c.openType = ReportPrint.OpenType.PDF;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                c.signindex = 0;
                c.customtools = "1,|2,|3,|4,|5,|6,|12,";
                c.AllowVisitNum = 1;
                if (allowedit == 1)
                    c.allowEdit = true;
                var guid = g.Add(c);
               // g.GetFile(c,)
                url = "/reportPrint/Index?" + guid;

                return new RedirectResult(url);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 务工人员报表打印下载
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult WGRYReportDownWord()
        {
            try
            {
                string url = "";
                string reportFile = Request["reportfile"].GetSafeString();
                string tablename = Request["tablename"].GetSafeString();
                string where = Request["where"].GetSafeString();
                int type = Request["type"].GetSafeInt();
                string opentype = Request["opentype"].GetSafeString();

                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();
                // c.type = ReportPrint.EnumType.Excel;
                if (type == 1)
                    c.type = (ReportPrint.EnumType)type;
                else
                    c.type = ReportPrint.EnumType.Word;
                //c.field = reportFile;
                c.fileindex = "0";
                //c.table = "stformitem|view_i_m_gc|stfile";
                c.filename = reportFile;
                //c.field = "formid";
                c.table = tablename;
                c.where = where;
                //if (opentype == "filedown")
                //    c.openType = ReportPrint.OpenType.FileDown;
                //else
                //    c.openType = ReportPrint.OpenType.PDF;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                c.signindex = 0;
                c.customtools = "1,|2,|3,|4,|5,|6,|12,";
                c.AllowVisitNum = 1;
                var guid = g.Add(c);

                url = "/reportPrint/Index?" + guid;

                return new RedirectResult(url);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 批量打印
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult WGRYReportPrintMore()
        {
            string msg = "";
            bool code = false;
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                if (gcbh != "")
                {
                    string sql = "select SFZFYJ_FRONT,SFZFYJ_BACK from i_m_ry_info where SFZFYJ_FRONT is not null and SFZFYJ_BACK is not null and  sfzhm in (select sfzhm from i_m_wgry where jdzch='" + gcbh + "' and (yhkh='' or yhkh is null) and sfzdz !='' and csrq !='' and qfjg!='' and ryxm !='' and sfzhm !='' and mz !='' and xb  !='' and sfzyxq ! ='' and CAST(zp as nvarchar(max))   !='')";
                    IList<IDictionary<string, string>> rydatas = CommonService.GetDataTable(sql);

                    IList<IDictionary<string, object>> opendata = new List<IDictionary<string, object>>();
                    for (int j = 0; j < rydatas.Count; j++)
                    {
                        object c = rydatas[j]["sfzfyj_front"] as object;
                        object d = rydatas[j]["sfzfyj_back"] as object;

                        IDictionary<string, object> list = new Dictionary<string, object>();
                        list.Add("front", c);
                        opendata.Add(list);
                        list = new Dictionary<string, object>();
                        list.Add("front", d);
                        opendata.Add(list);
                    }
                    if (opendata.Count > 0)
                    {
                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.fileindex = "0";
                        c.filename = "身份证复印件";
                        c.table = "sfzfy";
                        c.where = "1=1";
                        c.openType = ReportPrint.OpenType.FileDown;
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        IDictionary<string, IList<IDictionary<string, object>>> data = new Dictionary<string, IList<IDictionary<string, object>>>();
                        data.Add("sfzfy", opendata);
                        c.data = data;
                        c.signindex = 0;
                        c.AllowVisitNum = 1;
                        var guid = g.Add(c); //把数组分成多个guid打印
                        var guid1 = g.Add(c);
                        var guid2 = g.Add(c);
                        var guid3 = g.Add(c);
                        string url = "/ReportPrint/BatchPrinting?id=" + guid1 + "|" + guid2 + "|" + guid3 + "&c=1";

                        return new RedirectResult(url);
                    }

                }

            }
            catch (Exception e)
            {

            }
            return null;
        }

        public ActionResult Player()
        {
            ViewBag.filepath = Request["filepath"].GetSafeString();
            ViewBag.height = Request["height"].GetSafeInt();
            ViewBag.width = Request["width"].GetSafeInt();
            return View();
        }
        /// <summary>
        /// 首页工程地图
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MapGc()
        {
            ViewBag.kgnf = Request["kgnf"].GetSafeString();
            ViewBag.jgnf = Request["jgnf"].GetSafeString();
            ViewBag.gczt = Request["gczt"].GetSafeString();
            ViewBag.gclx = Request["gclx"].GetSafeString();
            return View();
        }
		#endregion
		#region 获取数据
        /// <summary>
        /// 验收状态选择页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Ysztxz()
        {
            return View();
        }
        /// <summary>
        /// 工程层数设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Gccssz()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            string cs = "";
            if (gcbh!="")
            {
                string sql = string.Format("select cs from i_m_gc where gcbh='{0}'", gcbh);
                IList<IDictionary<string,string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    cs = dt[0]["cs"];
                }

            }
            ViewBag.gcbh = DataFormat.GetSafeString(Request["gcbh"]);
            ViewBag.cs = cs;
            return View();
        }
        /// <summary>
        /// 工作进度查询
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gzjdcx()
        {
            UserType usertype = JdbgService.GetUserType(CurrentUser.UserName);
            if (usertype == UserType.Invalid)
                return null;
            string status = "";
            if (usertype == UserType.InnerUser || usertype == UserType.ZjzUser)
                status = "0";
            else if (usertype == UserType.QyUser)
                status = "30";
            else if (usertype == UserType.RyUser)
                status = "20";
            return Redirect("/WebList/EasyUiIndex?FormDm=GZJDCX&FormStatus=" + status + "&FormParam=Gridwrap--1");
        }
        #endregion
        
        #region 获取数据
        /// <summary>
        /// 获取未审批的工程数据
        /// </summary>
        [Authorize]
        public void GetGcsbSum()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select count(*) as sum from i_m_gc where zt='LR'");
                msg = dt[0]["sum"];
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
        /// <summary>
        /// 获取监督站内部查看工程的菜单
        /// </summary>
        [Authorize]
        public void GetGccknbMenu()
        {
            IList<VCheckItem> ret = new List<VCheckItem>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string msg = "";
                VJdbgReportSumItem item = JdbgService.GetReportSum(gcbh, out msg);
                if (msg != "")
                {
                    SysLog4.WriteError("获取工程报告信息失败：" + msg);
                }
                // 工程基本信息
                ret.Add(new VCheckItem() { id = "I_JBXX", pId = "", name = "工程基本信息(1)", isParent = false, cevent = "I_JBXX", open = true });
                // 工程监督方案
                ret.Add(new VCheckItem() { id = "I_JDFA", pId = "", name = "工程监督方案(" + item.SumJDFA + ")", isParent = false, cevent = "I_JDFA", open = true });
                ret.Add(new VCheckItem() { id = "I_JDJD", pId = "", name = "监督交底(" + item.SumJDJD + ")", isParent = false, cevent = "I_JDJD", open = true });
                //ret.Add(new VCheckItem() { id = "04", pId = "", name = "质量行为监督检查记录(" + item.SumZLXWJCJL + ")", isParent = false, cevent = "04", open = true });
                ret.Add(new VCheckItem() { id = "G_ZLYS", pId = "", name = "工程质量监督验收", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_YSSQ", pId = "G_ZLYS", name = "工程验收申请记录(" + item.SumYSSQJL + ")", isParent = false, cevent = "I_YSSQ", open = false });
                ret.Add(new VCheckItem() { id = "I_YSAP", pId = "G_ZLYS", name = "工程验收安排记录(" + item.SumYSAPJL + ")", isParent = false, cevent = "I_YSAP", open = false });
                ret.Add(new VCheckItem() { id = "I_JDJL", pId = "G_ZLYS", name = "监督记录(" + item.SumJDJL + ")", isParent = false, cevent = "I_JDJL", open = false });
                ret.Add(new VCheckItem() { id = "I_ZGTZ", pId = "G_ZLYS", name = "整改通知(" + item.SumZgd + ")", isParent = false, cevent = "I_ZGTZ", open = false });
                ret.Add(new VCheckItem() { id = "I_JGYSTZ", pId = "G_ZLYS", name = "竣工验收通知书(" + item.SumJGYSJL + ")", isParent = false, cevent = "I_JGYSTZ", open = false });

                ret.Add(new VCheckItem() { id = "G_JCBG", pId = "", name = "检测报告查询", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_BHGBG", pId = "G_JCBG", name = "不合格检测报告(0)", isParent = false, cevent = "I_BHGBG", open = false });
                ret.Add(new VCheckItem() { id = "I_SYBG", pId = "G_JCBG", name = "所有检测报告(0)", isParent = false, cevent = "I_SYBG", open = false });
                //ret.Add(new VCheckItem() { id = "G_QTZL", pId = "", name = "其他资料", isParent = true, cevent = "", open = true });

                //ret.Add(new VCheckItem() { id = "I_JLYB", pId = "G_QTZL", name = "监理月报(0)", isParent = false, cevent = "I_JLYB", open = false });
                
                //ret.Add(new VCheckItem() { id = "0605", pId = "06", name = "扣分清单(0)", isParent = false, cevent = "0605", open = false });
                //ret.Add(new VCheckItem() { id = "0606", pId = "06", name = "现场图片(0)", isParent = false, cevent = "0606", open = false });
                ret.Add(new VCheckItem() { id = "I_JDBG", pId = "", name = "工程质量监督报告(" + item.SumJDBG + ")", isParent = false, cevent = "I_JDBG", open = true });
                ret.Add(new VCheckItem() { id = "I_RYDD", pId = "", name = "工程人员调动记录(" + item.SumRYLZJL + ")", isParent = false, cevent = "I_RYDD", open = true });
                ret.Add(new VCheckItem() { id = "I_DWDD", pId = "", name = "工程单位调换记录(" + item.SumQYLZJL + ")", isParent = false, cevent = "I_DWDD", open = true });
                ret.Add(new VCheckItem() { id = "I_GCBZ", pId = "", name = "监督人员备注(" + item.SumJDYBZ + ")", isParent = false, cevent = "I_JGYSZL", open = true });

                ret.Add(new VCheckItem() { id = "G_GDZL", pId = "", name = "归档资料", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_JGYSZL", pId = "G_GDZL", name = "竣工验收资料(0)", isParent = false, cevent = "I_JGYSZL", open = false });
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(ret));
            }
        }

        /// <summary>
        /// 获取外部人员查看工程的菜单
        /// </summary>
        [Authorize]
        public void GetGcckwbMenu()
        {
            IList<VCheckItem> ret = new List<VCheckItem>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string msg = "";
                VJdbgReportSumItem item = JdbgService.GetReportSum(gcbh, out msg);
                if (msg != "")
                {
                    SysLog4.WriteError("获取工程报告信息失败：" + msg);
                }
                // 工程基本信息
                ret.Add(new VCheckItem() { id = "I_JBXX", pId = "", name = "工程基本信息(1)", isParent = false, cevent = "I_JBXX", open = true });
                // 工程监督方案
                ret.Add(new VCheckItem() { id = "I_JDFA", pId = "", name = "工程监督方案(" + item.SumJDFA + ")", isParent = false, cevent = "I_JDFA", open = true });
                ret.Add(new VCheckItem() { id = "I_JDJD", pId = "", name = "监督交底(" + item.SumJDJD + ")", isParent = false, cevent = "I_JDJD", open = true });
                //ret.Add(new VCheckItem() { id = "04", pId = "", name = "质量行为监督检查记录(" + item.SumZLXWJCJL + ")", isParent = false, cevent = "04", open = true });
                ret.Add(new VCheckItem() { id = "G_ZLYS", pId = "", name = "工程质量监督验收", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_YSSQ", pId = "G_ZLYS", name = "工程验收申请记录(" + item.SumYSSQJL + ")", isParent = false, cevent = "I_YSSQ", open = false });
                ret.Add(new VCheckItem() { id = "I_YSAP", pId = "G_ZLYS", name = "工程验收安排记录(" + item.SumYSAPJL + ")", isParent = false, cevent = "I_YSAP", open = false });
                //ret.Add(new VCheckItem() { id = "I_JDJL", pId = "G_ZLYS", name = "监督记录(" + item.SumJDJL + ")", isParent = false, cevent = "I_JDJL", open = false });
                ret.Add(new VCheckItem() { id = "I_ZGTZ", pId = "G_ZLYS", name = "整改通知(" + item.SumZgdSp + ")", isParent = false, cevent = "I_ZGTZ", open = false });

                ret.Add(new VCheckItem() { id = "G_JCBG", pId = "", name = "检测报告查询", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_BHGBG", pId = "G_JCBG", name = "不合格检测报告(0)", isParent = false, cevent = "I_BHGBG", open = false });
                ret.Add(new VCheckItem() { id = "I_SYBG", pId = "G_JCBG", name = "所有检测报告(0)", isParent = false, cevent = "I_SYBG", open = false });
                //ret.Add(new VCheckItem() { id = "G_QTZL", pId = "", name = "其他资料", isParent = true, cevent = "", open = true });

                //ret.Add(new VCheckItem() { id = "I_JLYB", pId = "G_QTZL", name = "监理月报(0)", isParent = false, cevent = "I_JLYB", open = false });

                //ret.Add(new VCheckItem() { id = "0605", pId = "06", name = "扣分清单(0)", isParent = false, cevent = "0605", open = false });
                //ret.Add(new VCheckItem() { id = "0606", pId = "06", name = "现场图片(0)", isParent = false, cevent = "0606", open = false });
                //ret.Add(new VCheckItem() { id = "I_JDBG", pId = "", name = "工程质量监督报告(" + item.SumJDBG + ")", isParent = false, cevent = "I_JDBG", open = true });
                ret.Add(new VCheckItem() { id = "I_RYDD", pId = "", name = "工程人员调动记录(" + item.SumRYLZJL + ")", isParent = false, cevent = "I_RYDD", open = true });
                ret.Add(new VCheckItem() { id = "I_DWDD", pId = "", name = "工程单位调换记录(" + 0 + ")", isParent = false, cevent = "I_DWDD", open = true });
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(ret));
            }
        }
        /// <summary>
        /// 获取jdbgjdjl表中某类型内容数量
        /// </summary>
        [Authorize]
        public void GetJdjlSum()
        {
            bool code = false;
            string msg = "";
            try
            {
                string bglx = Request["lx"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                int sum = JdbgService.GetReportSum(gcbh, bglx, out msg);
                if (msg != "")
                    SysLog4.WriteError("获取工程" + bglx + "报告信息失败：" + msg);

                code = msg == "";
                msg = sum.ToString();   
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
        /// 获取个人手机上传问题图片列表
        /// </summary>
        [Authorize]
        public void GetSelfProblems()
        {
            bool code = false;
            string msg = "";
            int totalCount = 0;
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                int pageIndex = Request["pageindex"].GetSafeInt(1);
                int pageSize = Request["pagesize"].GetSafeInt(100);
                string key = Request["key"].GetSafeRequest();
                string gcbh = Request["gcbh"].GetSafeRequest();
                string dt1 = Request["dt1"].GetSafeRequest();
                string dt2 = Request["dt2"].GetSafeRequest();

                
                dts = JdbgService.GetProblems(key, gcbh, CurrentUser.UserName, dt1, dt2, "", pageSize, pageIndex, out totalCount);
                code = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg, totalCount, dts));
            }
        }
        [Authorize]
        public JsonResult GetSelfProblemImages(string gcbh)
        {
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                dts = JdbgService.GetProblemImages(gcbh, CurrentUser.UserName,"");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                
            }
            return Json(dts);
        }
        [Authorize]
        public JsonResult GetSelfSelectedImages(string gcbh)
        {
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                dts = JdbgService.GetProblemImages(gcbh, CurrentUser.UserName, "3");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(dts);
        }

        public JsonResult GetSelfSelectedContents(string gcbh)
        {
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                dts = JdbgService.GetProblemContents(gcbh, CurrentUser.UserName, "1");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(dts);
        }


        public JsonResult GetSelfSelectedContentsByWorkserial(string workserial)
        {
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                //dts = JdbgService.GetProblemContents(workserial, CurrentUser.UserName, "1");
                dts = JdbgService.GetProblemContentsByWorkserial(workserial,"", "");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(dts);
        }


        public JsonResult GetSelfSelectedImagesByWorkserial(string workserial)
        {
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                //dts = JdbgService.GetProblemImages(workserial, CurrentUser.UserName, "3");
                dts = JdbgService.GetProblemImagesByWorkserial(workserial, "", "");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(dts);
        }


        /// <summary>
        /// 获取手机某个图片缩略图的二进制流
        /// </summary>
        public void GetProblemImageSmall()
        {
            byte[] file = null;
            try
            {
                string key = RouteData.Values["id"].GetSafeString();

                file = JdbgService.GetProblemImageSmall(key);
                if (file != null)
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("pics.jpg"));
                    Response.BinaryWrite(file);
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
        /// 获取手机某个图片二进制流
        /// </summary>
        public void GetProblemImageBig()
        {
            byte[] file = null;
            try
            {
                string key = RouteData.Values["id"].GetSafeString();

                file = JdbgService.GetProblemImageBig(key);
                if (file != null)
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("picb.jpg"));
                    Response.BinaryWrite(file);
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
        /// 获取手机某个语音二进制流
        /// </summary>
        public void GetProblemVoice()
        {
            byte[] file = null;
            try
            {
                string key = RouteData.Values["id"].GetSafeString();// Request["id"].GetSafeString();

                file = JdbgService.GetProblemVoice(key);
                if (file != null)
                {
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    Response.Charset = "UTF-8";
                    //Response.ContentType = "video/mpeg4";
                    Response.ContentType = "audio/mp3";
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(key+".mp4"));
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(key + ".mp3"));
                    Response.BinaryWrite(file);
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
        /// 获取报告数量
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReportSum(string jcjg)
        {
            bool code = false;
            string msg = "";
            try
            {
                jcjg = jcjg.GetSafeRequest();
                string sql = "select count(*) as t1 from up_bgsj";
                if (jcjg != "")
                    sql += " where jcjg='" + jcjg + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                msg = dt[0]["t1"].GetSafeString();
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return Json(new VContrllerRet(code, msg));
        }

        public void GetGcs()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select qybh,sfqyzzh,zhlx from i_m_qyzh where yhzh='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                    sql = "select  * from view_i_m_gc_jd where gczt='在建' order by zjdjh desc";
                else
                {

                    string zybh = dt[0]["qybh"];
                    string zhlx = dt[0]["zhlx"];
                    if (zhlx == "N")
                    {
                        sql = "select  * from view_i_m_gc_jd where gczt='在建' order by zjdjh desc";
                    }
                    else
                    {
                        bool sfzzh = dt[0]["sfqyzzh"].GetSafeBool();
                        if (!sfzzh)
                            sql = "select * from view_i_m_gc_jd where gczt='在建' and  gcbh in (select gcbh from View_GC_RY where rybh = (select top 1 qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')) order by zjdjh desc";
                        else
                            sql = "select * from view_i_m_gc_jd where gczt='在建' and  gcbh in (select gcbh from View_GC_QY where qybh = (select top 1 qybh from i_m_qyzh where yhzh='" + CurrentUser.UserName + "')) order by zjdjh desc";
                    }
                    
                }

                ret = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.Write(jss.Serialize(ret));
        }
        /// <summary>
        /// 获取工程验收状态
        /// </summary>
        /// <returns></returns>
        public JsonResult GetYszts()
        {
            IList<IDictionary<string, string>> zts = new List<IDictionary<string, string>>();
            try
            {
                zts = JdbgService.GetGcyszts();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(zts);
        }
        /// <summary>
        /// 获取工程开始结束层数
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="fgcbh"></param>
        /// <returns></returns>
        public JsonResult GetGccs(string gcbh, string fgcbh)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = JdbgService.GetGccs(gcbh, fgcbh, out msg);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }
            finally
            {

            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }
        /// <summary>
        /// 获取工程类型
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetGclx()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = JdbgService.GetGclx();

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }
            finally
            {

            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }
        /// <summary>
        /// 获取首页工程统计
        /// </summary>
        /// <param name="kgnf">开工年份</param>
        /// <param name="jgnf">竣工年份</param>
        /// <param name="gczt">在建/竣工</param>
        /// <param name="gclx">工程类型</param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetGcStatistic1(string kgnf, string jgnf, string gczt, string gclx)
        {
            bool code = true;
            string msg = "";
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                IList<IDictionary<string, string>> datas = JdbgService.GetGcStatistic(kgnf, jgnf, gczt, gclx);
                if (datas.Count() > 0)
                    ret = datas[0];

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }
            finally
            {

            }
            return Json(new { code = code ? "0" : "1", msg = msg, record = ret });
        }
        /// <summary>
        /// 首页地图标注工程
        /// </summary>
        /// <param name="kgnf"></param>
        /// <param name="jgnf"></param>
        /// <param name="gczt"></param>
        /// <param name="gclx"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetStatisticGcList(string kgnf, string jgnf, string gczt, string gclx)
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = JdbgService.GetGcList(kgnf, jgnf, gczt, gclx);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }
            finally
            {

            }
            return Json(new { code = code ? "0" : "1", msg = msg, records = ret });
        }
        #endregion
		#region 更新数据
        /// <summary>
        /// 工程窗口申报审批
        /// </summary>
        [Authorize]
        public void DoGccksbsp()
        {
            bool code = true;
            string msg = "";
            string jsondata = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                IList<string> sqls = new List<string>();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select ZT,CKSBSP,ZJDJH,GCMC,GCBH from i_m_gc where gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "无效的工程信息";
                }
                else if (!dt[0]["zt"].GetSafeString().Equals("LR", StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "无效的工程状态";
                }
                else
                {
                    bool sbsp = dt[0]["cksbsp"].GetSafeBool();
                    
                    // 拒绝
                    if (!sbsp)
                    {
                        sqls.Add("update i_m_gc set sptg=0, sbsp=0, cksptg=0, cksbsp=0, sfyx=0,zt='YT' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "0";
                    }
                    // 同意
                    else
                    {
                        sqls.Add("update i_m_gc set sptg=0, sbsp=0, cksptg=1, cksbsp=1, sfyx=0,zt='LR', ckjbrzh='" + CurrentUser.UserName + "', ckjbrxm='" +  CurrentUser.RealName +"' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "1";
                        jsondata = "{\"zjdjh\":\"" + dt[0]["zjdjh"].GetSafeString() + "\",\"gcmc\":\"" + dt[0]["gcmc"].GetSafeString() + "\",\"gcbh\":\"" + dt[0]["gcbh"].GetSafeString() + "\"}";

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
                Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"data\":" + jsondata + "}");
            }
        }

        /// <summary>
        /// 工程申报审批
        /// </summary>
        [Authorize]
        public void DoGcsbsp()
        {
            bool code = true;
            string msg = "";
            string jsondata = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                IList<string> sqls = new List<string>();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select ZT,SBSP,ZJDJH,GCMC,GCBH from i_m_gc where gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "无效的工程信息";
                }
                else if (!dt[0]["zt"].GetSafeString().Equals("LR", StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "无效的工程状态";
                }
                else
                {
                    bool sbsp = dt[0]["sbsp"].GetSafeBool();
                    // 拒绝
                    if (!sbsp)
                    {
                        sqls.Add("update i_m_gc set sptg=0, sbsp=0, sfyx=0,zt='YT' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "0";
                    }
                    // 同意
                    else
                    {
                        string sql = string.Format("update i_m_gc set sptg=1,sbsp=1, sfyx=1, ckjbrzh='{0}', ckjbrxm='{1}', ckslrq=slrq where gcbh='{2}'", CurrentUser.RealUserName, CurrentUser.RealName, gcbh);
                        //sqls.Add("update i_m_gc set sptg=1,sbsp=1, sfyx=1 where gcbh='" + gcbh + "'");
                        sqls.Add(sql);
                        CommonService.ExecTrans(sqls);
                        msg = "1";
                        jsondata = "{\"zjdjh\":\"" + dt[0]["zjdjh"].GetSafeString() + "\",\"gcmc\":\"" + dt[0]["gcmc"].GetSafeString() + "\",\"gcbh\":\""+dt[0]["gcbh"].GetSafeString()+"\"}";

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
                Response.Write("{\"code\":\""+(code?0:1)+"\",\"msg\":\""+msg+"\",\"data\":"+jsondata+"}");
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        [Authorize]
        public void ReSertPassWord()
        {
            bool code = true;
            string msg = "";
            try
            {
                string username = Request["zh"].GetSafeString();
                string pwd = Request["pwd"].GetSafeString();
                string json = UserService.ReSetPassWord(username, pwd);

                JToken jsons = JToken.Parse(json);//转化为JToken（JObject基类）
                code = jsons["success"].GetSafeBool();
                msg = jsons["msg"].GetSafeString();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            Response.Write(JsonFormat.GetRetString(code, msg));

        }


        /// <summary>
        /// 工程申报审批
        /// </summary>
        [Authorize]
        public void DoGcsbsp2()
        {
            bool code = true;
            string msg = "";
            string jsondata = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string nbbj = Request["nbbj"].GetSafeString();
                string gczt = "LR";
                if (nbbj == "")
                    gczt = "YT";
                IList<string> sqls = new List<string>();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select ZT,SBSP,ZJDJH,GCMC,GCBH from i_m_gc where gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "无效的工程信息";
                }
                else if (!dt[0]["zt"].GetSafeString().Equals(gczt, StringComparison.OrdinalIgnoreCase) && !dt[0]["zt"].GetSafeString().Equals("YT", StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "无效的工程状态";
                }
                else
                {
                    bool sbsp = dt[0]["sbsp"].GetSafeBool();
                    // 拒绝
                    if (!sbsp)
                    {
                        sqls.Add("update i_m_gc set sptg=1,sfyx=0,zt='YT' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "0";
                    }
                    // 同意
                    else
                    {
                        sqls.Add("update i_m_gc set sptg=1,sfyx=1,zt='ZC',BJZT='BJ'  where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "1";
                        jsondata = "{\"zjdjh\":\"" + dt[0]["zjdjh"].GetSafeString() + "\",\"gcmc\":\"" + dt[0]["gcmc"].GetSafeString() + "\",\"gcbh\":\"" + dt[0]["gcbh"].GetSafeString() + "\"}";

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
                Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"data\":" + jsondata + "}");
            }
        }


        /// <summary>
        /// 工程质监申报审批
        /// </summary>
        [Authorize]
        public void DoGcZJsbsp()
        {
            bool code = true;
            string msg = "";
            string jsondata = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                IList<string> sqls = new List<string>();
                string sql = "select ZT,ZJSPYX,ZJDJH,GCMC,GCBH from i_m_gc where gcbh='" + gcbh + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "无效的工程信息";
                }
                else if (!dt[0]["zt"].GetSafeString().Equals("LR", StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "无效的工程状态";
                }
                else
                {
                    bool sbsp = dt[0]["zjspyx"].GetSafeBool();

                    // 拒绝
                    if (!sbsp)
                    {
                        sqls.Add("update i_m_gc set zjtj=0, zt='YT' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "0";
                    }
                    // 同意
                    else
                    {
                        sqls.Add("update i_m_gc set zt='LR', zjsprzh='" + CurrentUser.UserName + "', zjsprxm='" + CurrentUser.RealName + "' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "1";
                        jsondata = "{\"zjdjh\":\"" + dt[0]["zjdjh"].GetSafeString() + "\",\"gcmc\":\"" + dt[0]["gcmc"].GetSafeString() + "\",\"gcbh\":\"" + dt[0]["gcbh"].GetSafeString() + "\"}";

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
                Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"data\":" + jsondata + "}");
            }
        }
        /// <summary>
        /// 工程安监申报审批
        /// </summary>
        [Authorize]
        public void DoGcAJsbsp()
        {
            bool code = true;
            string msg = "";
            string jsondata = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                IList<string> sqls = new List<string>();
                string sql = "select ZT,AJSPYX,ZJDJH,GCMC,GCBH from i_m_gc where gcbh='" + gcbh + "'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    code = false;
                    msg = "无效的工程信息";
                }
                else if (!dt[0]["zt"].GetSafeString().Equals("LR", StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "无效的工程状态";
                }
                else
                {
                    bool sbsp = dt[0]["ajspyx"].GetSafeBool();

                    // 拒绝
                    if (!sbsp)
                    {
                        sqls.Add("update i_m_gc set ajtj=0, zt='YT' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "0";
                    }
                    // 同意
                    else
                    {
                        sqls.Add("update i_m_gc set zt='LR', ajsprzh='" + CurrentUser.UserName + "', ajsprxm='" + CurrentUser.RealName + "' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "1";
                        jsondata = "{\"zjdjh\":\"" + dt[0]["zjdjh"].GetSafeString() + "\",\"gcmc\":\"" + dt[0]["gcmc"].GetSafeString() + "\",\"gcbh\":\"" + dt[0]["gcbh"].GetSafeString() + "\"}";

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
                Response.Write("{\"code\":\"" + (code ? 0 : 1) + "\",\"msg\":\"" + msg + "\",\"data\":" + jsondata + "}");
            }
        }
        [Authorize]
        public void SaveTemplate()
        {
            bool code = false;
            string msg = "";
            try
            {
                string content =  WorkFlow.Common.DataFormat.DecodeBase64(Encoding.GetEncoding("GB2312"),Request["content"].GetSafeString());
                string template = Request["template"].GetSafeString();
                content = content.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;", "");
                byte[] filearr = System.IO.File.ReadAllBytes(Server.MapPath("/report/jdbg/"+template));
                string[] contentarr = content.Split(new string[]{"<br>", "<br/>","<br />"}, StringSplitOptions.RemoveEmptyEntries);

                Regex reg = new Regex("<img[^>]*>");
                Regex regImage = new Regex(@"/p-s\w+.jpg");
                string recids = "";
                for (int i = 0; i < contentarr.Length;i++ )
                {
                    string strRow = contentarr[i];
                    MatchCollection matchCol = reg.Matches(strRow);
                    if (matchCol.Count > 0)
                    {
                        foreach (Match matchItem in matchCol)
                        {
                            if (regImage.IsMatch(matchItem.Value))
                            {
                                Match matchImage = regImage.Match(matchItem.Value);
                                string strRecid = matchImage.Value.Substring(4, matchImage.Value.Length - 8);
                                recids += strRecid + ",";
                                string strImagePat = "#F:view_gc_xctp.thumbattachment-O:I-W:Recid=" + strRecid + "#";
                                contentarr[i] = strRow.Replace(matchItem.Value, strImagePat);
                            }
                        }
                    }
                }
                if (recids.Length > 0)
                {
                    string sql = "update I_S_GC_ProblemDetail set status=3 where recid in (" + recids.TrimEnd(new char[]{','}) + ")";
                    IList<string> lssql = new List<string>();
                    lssql.Add(sql);
                    CommonService.ExecTrans(lssql);
                }
                string realContent = "";
                foreach (string strRow in contentarr)
                {
                    if (realContent.Length > 0)
                        realContent += "\r\n";
                    realContent += strRow;
                }

                string filecontent = ReportPrintService.WordFunc.Replace(Convert.ToBase64String(filearr), "#REPLACEAREA#", realContent);
                string filename = Server.MapPath("/report/jdbg/"+Guid.NewGuid().ToString()+".docx");
                System.IO.File.WriteAllBytes(filename, Convert.FromBase64String(filecontent));
                code = true;
                msg = System.IO.Path.GetFileName(filename);
                msg = System.IO.Path.GetFileNameWithoutExtension(msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        [Authorize]
        public void SaveJDJLTemplate()
        {
            bool code = false;
            string msg = "";
            try
            {
                string content = WorkFlow.Common.DataFormat.DecodeBase64(Encoding.GetEncoding("GB2312"), Request["content"].GetSafeString());
                string template = Request["template"].GetSafeString();
                string serial = Request["serial"].GetSafeString();
                string existedReportFile = "";
                int formid = 0;
                string itemname = "jdjl";
                string itemvalue = "";
                
                //if (serial != "")
                //{
                //    // 获取已经保存过的监督记录
                //    string sql = "select workserial, reportfile from jdbg_jdjl where workserial='" + serial + "' ";
                //    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                //    if (dt.Count > 0)
                //    {
                //        serial = dt[0]["workserial"];
                //        existedReportFile = dt[0]["reportfile"];

                //        if (serial !="")
                //        {
                //            sql = string.Format("select f.formid, i.itemvalue from stform f, stformitem i where f.serialno='{0}' and f.formid=i.formid and i.itemname='{1}' ", serial, itemname);
                //            IList<IDictionary<string, string>> dtt = CommonService.GetDataTable(sql);
                //            if (dtt.Count > 0)
                //            {
                //                formid = dtt[0]["formid"].GetSafeInt();
                //                itemvalue = dtt[0]["itemvalue"].GetSafeString();
                //            }
                //            // 如果已经存在监督记录，将当前提交的记录拼接在以前的记录后面
                //            if (formid > 0 && itemvalue !="")
                //            {
                //                string existedjdjl = WorkFlow.Common.DataFormat.DecodeBase64(Encoding.GetEncoding("GB2312"), itemvalue);
                //                content = existedjdjl + "<br>" + content;
                //            }
                //        }
                //    }
                //}

                // 替换换行符
                content = content.Replace("\n", "\r\n");
                content = content.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;", " ");
                byte[] filearr = System.IO.File.ReadAllBytes(Server.MapPath("/report/jdbg/" + template));
                string[] contentarr = content.Split(new string[] { "<br>", "<br/>", "<br />" }, StringSplitOptions.RemoveEmptyEntries);

                Regex reg = new Regex("<img[^>]*>");
                Regex regImage = new Regex(@"/p-s\w+.jpg");
                string recids = "";
                for (int i = 0; i < contentarr.Length; i++)
                {
                    string strRow = contentarr[i];
                    MatchCollection matchCol = reg.Matches(strRow);
                    if (matchCol.Count > 0)
                    {
                        foreach (Match matchItem in matchCol)
                        {
                            if (regImage.IsMatch(matchItem.Value))
                            {
                                Match matchImage = regImage.Match(matchItem.Value);
                                string strRecid = matchImage.Value.Substring(4, matchImage.Value.Length - 8);
                                recids += strRecid + ",";
                                string strImagePat = "#F:view_gc_xctp.thumbattachment-O:I-W:Recid=" + strRecid + "#";
                                strRow = strRow.Replace(matchItem.Value, strImagePat);
                                contentarr[i] = strRow;
                            }
                        }
                    }
                }
                if (recids.Length > 0)
                {
                    string sql = "update I_S_GC_ProblemDetail set status=3 where recid in (" + recids.TrimEnd(new char[] { ',' }) + ")";
                    IList<string> lssql = new List<string>();
                    lssql.Add(sql);
                    CommonService.ExecTrans(lssql);
                }
                string realContent = "";
                foreach (string strRow in contentarr)
                {
                    if (realContent.Length > 0)
                        realContent += "\r\n";
                    realContent += strRow;
                }

                string filecontent = ReportPrintService.WordFunc.Replace(Convert.ToBase64String(filearr), "#REPLACEAREA#", realContent);
                string fileid = Guid.NewGuid().ToString();
                string filename = Server.MapPath("/report/jdbg/" + fileid + ".docx");
                System.IO.File.WriteAllBytes(filename, Convert.FromBase64String(filecontent));

                savejdjlxq(fileid, contentarr);
                // 删掉已经存在的监督记录文件
                if (existedReportFile !="")
                {
                     existedReportFile = Server.MapPath("/report/jdbg/" + existedReportFile + ".docx");
                    if (System.IO.File.Exists(existedReportFile))
                    {
                        System.IO.File.Delete(existedReportFile);
                    }
                }
                code = true;
                msg = System.IO.Path.GetFileName(filename);
                msg = System.IO.Path.GetFileNameWithoutExtension(msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        /// <summary>
        /// 根据流水号，生成手机填写的监督记录模板
        /// </summary>
        [Authorize]
        public void GeneratePhoneJDJLTemplate()
        {
            bool code = false;
            string msg = "";
            try
            {
                string content = "";
                string template = Request["template"].GetSafeString();
                string serial = Request["serial"].GetSafeString();
                string existedReportFile = "";
                int formid = 0;
                string itemname = "jdjl";
                string itemvalue = "";
                bool IsJDJLExisted = false;  // 是否存在已经保存过的监督记录

                if (serial != "")
                {
                    // 获取已经保存过的监督记录
                    string sql = "select workserial, reportfile from jdbg_jdjl where workserial='" + serial + "' ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        serial = dt[0]["workserial"];
                        existedReportFile = dt[0]["reportfile"];
                        IsJDJLExisted = true;

                        if (serial != "")
                        {
                            sql = string.Format("select f.formid, i.itemvalue from stform f, stformitem i where f.serialno='{0}' and f.formid=i.formid and i.itemname='{1}' ", serial, itemname);
                            IList<IDictionary<string, string>> dtt = CommonService.GetDataTable(sql);
                            if (dtt.Count > 0)
                            {
                                formid = dtt[0]["formid"].GetSafeInt();
                                itemvalue = dtt[0]["itemvalue"].GetSafeString();
                            }
                            // 如果已经存在监督记录，获取内容
                            if (formid > 0 && itemvalue != "")
                            {
                                content = WorkFlow.Common.DataFormat.DecodeBase64(Encoding.GetEncoding("GB2312"), itemvalue);
                            }
                        }
                    }
                }
                // 替换换行符
                content = content.Replace("\n", "\r\n");
                content = content.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;", "");
                byte[] filearr = System.IO.File.ReadAllBytes(Server.MapPath("/report/jdbg/" + template));
                string[] contentarr = content.Split(new string[] { "<br>", "<br/>", "<br />" }, StringSplitOptions.RemoveEmptyEntries);

                Regex reg = new Regex("<img[^>]*>");
                Regex regImage = new Regex(@"/p-s\w+.jpg");
                string recids = "";
                for (int i = 0; i < contentarr.Length; i++)
                {
                    string strRow = contentarr[i];
                    MatchCollection matchCol = reg.Matches(strRow);
                    if (matchCol.Count > 0)
                    {
                        foreach (Match matchItem in matchCol)
                        {
                            if (regImage.IsMatch(matchItem.Value))
                            {
                                Match matchImage = regImage.Match(matchItem.Value);
                                string strRecid = matchImage.Value.Substring(4, matchImage.Value.Length - 8);
                                recids += strRecid + ",";
                                string strImagePat = "#F:view_gc_xctp.thumbattachment-O:I-W:Recid=" + strRecid + "#";
                                strRow = strRow.Replace(matchItem.Value, strImagePat);
                                contentarr[i] = strRow;
                            }
                        }
                    }
                }
                if (recids.Length > 0)
                {
                    string sql = "update I_S_GC_ProblemDetail set status=3 where recid in (" + recids.TrimEnd(new char[] { ',' }) + ")";
                    IList<string> lssql = new List<string>();
                    lssql.Add(sql);
                    CommonService.ExecTrans(lssql);
                }
                string realContent = "";
                foreach (string strRow in contentarr)
                {
                    if (realContent.Length > 0)
                        realContent += "\r\n";
                    realContent += strRow;
                }

                string filecontent = ReportPrintService.WordFunc.Replace(Convert.ToBase64String(filearr), "#REPLACEAREA#", realContent);
                string filename = Server.MapPath("/report/jdbg/" + Guid.NewGuid().ToString() + ".docx");
                System.IO.File.WriteAllBytes(filename, Convert.FromBase64String(filecontent));
                // 删掉已经存在的监督记录文件
                if (existedReportFile != "")
                {
                    existedReportFile = Server.MapPath("/report/jdbg/" + existedReportFile + ".docx");
                    if (System.IO.File.Exists(existedReportFile))
                    {
                        System.IO.File.Delete(existedReportFile);
                    }
                }
                code = true;
                msg = System.IO.Path.GetFileName(filename);
                msg = System.IO.Path.GetFileNameWithoutExtension(msg);

                if (IsJDJLExisted)
                {
                    // 根据workserial, 更新reportfile字段
                    string updatesql = string.Format("update jdbg_jdjl set reportfile='{0}'  where workserial='{1}' ", msg, serial);
                    IList<string> lsupdatesql = new List<string>();
                    lsupdatesql.Add(updatesql);
                    CommonService.ExecTrans(lsupdatesql);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }

        
        [Authorize]
        public void SetGccs()
        {
            bool code = true;
            string msg = "";
            string gccss = Request["gccss"].GetSafeRequest();
            try
            {
                
                if (gccss == "")
                    code = true;
                else
                {
                    IList<IDictionary<string, string>> updateItems = new List<IDictionary<string, string>>();
                    string[] arritems = gccss.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string stritem in arritems)
                    {
                        string[] gc = stritem.Split(new char[]{','});
                        if (gc.Length < 4)
                            continue;
                        IDictionary<string, string> row = new Dictionary<string, string>();
                        row.Add("gcbh", gc[0]);
                        row.Add("zcb", gc[1]);
                        row.Add("kscs", gc[2]);
                        row.Add("jscs", gc[3]);
                        updateItems.Add(row);
                    }
                    if (updateItems.Count > 0)
                    {
                        code = JdbgService.SetGccs(updateItems,out msg);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

		#region 大检查

        /// <summary>
        /// 大检查 抽取工程和检查人员 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StartMajorInspection()
        {
            return View();
        }

        public ActionResult ShowMajorInspectionInfo()
        {
            ViewBag.DJCFQBH = Request["djcfqbh"].GetSafeString();
            return View();
        }
        /// <summary>
        /// 保存大检查发起数据
        /// </summary>
        [Transaction(ReadOnly =false)]
        public void SaveMajorInspection()
        {
            bool code = false;
            string msg = "";
            string data = "";
            try
            {
                string extractedgc = Request["extractedgc"].GetSafeString();
                string extractedzzry = Request["extractedzzry"].GetSafeString();
                string extractedazry = Request["extractedazry"].GetSafeString();
                string extractedtjry = Request["extractedtjry"].GetSafeString();
                string ccbt = Request["ccbt"].GetSafeString();
                


                // 校验成功，插入数据
                if (CheckMajorInspection(extractedgc, extractedzzry, extractedazry, extractedtjry, ccbt, out msg))
                {

                    IList<VUser> validUsers = GetValidUsers();

                    #region 获取发起人信息
                    string fqrzh = Session["USERNAME"].GetSafeString();
                    string fqrxm = Session["REALNAME"].GetSafeString();
                    #endregion

                    #region 获取工程信息
                    string extractedgcmc = "";
                    string gcwhere = " 1=1 ";
                    gcwhere += " and zt not in ('YT', 'LR', 'JGYS','JDBG','GDZL') ";
                    gcwhere += " and gcbh in ( " + DataFormat.FormatSQLInStr(extractedgc) + " ) ";
                    string gcsql = string.Format("select gcbh, gcmc,zjzbh, zjdjh, jsdwzh, sy_jsdwmc, jldwzh, jldwmc, sgdwzh, sgdwmc from view_i_m_gc where {0}", gcwhere);
                    IList<IDictionary<string, string>> gclist = CommonService.GetDataTable(gcsql);
                    if (gclist.Count > 0)
                    {
                        foreach (var gc in gclist)
                        {
                            if (extractedgcmc != "")
                            {
                                extractedgcmc += ",";
                            }
                            extractedgcmc += gc["gcmc"];
                        }
                    }
                    #endregion

                    #region 获取组长信息
                    string extractedzzryxm = "";
                    IList<VUser> zzryList = (from e in validUsers where ("," + extractedzzry + ",").IndexOf("," + e.UserId + ",") > -1 select e).ToList();
                    if (zzryList.Count() > 0)
                    {
                        foreach (var u in zzryList)
                        {
                            if (extractedzzryxm != "")
                            {
                                extractedzzryxm += ",";
                            }
                            extractedzzryxm += u.UserRealName;
                        }
                    }
                    #endregion

                    #region 获取安装监督员信息
                    string extractedazryxm = "";
                    IList<VUser> azryList = (from e in validUsers where ("," + extractedazry + ",").IndexOf("," + e.UserId + ",") > -1 select e).ToList();
                    if (azryList.Count() > 0)
                    {
                        foreach (var u in azryList)
                        {
                            if (extractedazryxm != "")
                            {
                                extractedazryxm += ",";
                            }
                            extractedazryxm += u.UserRealName;
                        }
                    }
                    #endregion

                    #region 获取土建监督员信息
                    string extractedtjryxm = "";
                    IList<VUser> tjryList = (from e in validUsers where ("," + extractedtjry + ",").IndexOf("," + e.UserId + ",") > -1 select e).ToList();
                    if (tjryList.Count() > 0)
                    {
                        foreach (var u in tjryList)
                        {
                            if (extractedtjryxm != "")
                            {
                                extractedtjryxm += ",";
                            }
                            extractedtjryxm += u.UserRealName;
                        }
                    }
                    #endregion


                    #region 保存大检查相关信息
                    string procstr = string.Format("DJCInsertFQ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", 
                        ccbt, fqrzh, fqrxm, extractedgc, extractedgcmc, extractedazry, extractedazryxm, extractedtjry,extractedtjryxm, extractedzzry, extractedzzryxm);
                    string DJCFQBH = "";

                    if (JdbgService.SaveDJCFQ(procstr, out msg, out DJCFQBH))
                    {
                        if (DJCFQBH.GetSafeString() != "")
                        {
                            IList<string> lssql = new List<string>();
                            string isql = "";
                            foreach (var gc in gclist)
                            {
                                isql = string.Format("insert into djc_fq_gc (DJCFQBH, GCBH, GCMC, ZJZBH, ZJDJH,JSDWZH, JSDWMC, JLDWZH, JLDWMC, SGDWZH, SGDWMC) ");
                                isql += string.Format("values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') ", 
                                    DJCFQBH, gc["gcbh"], gc["gcmc"], gc["zjzbh"], gc["zjdjh"], gc["jsdwzh"], gc["sy_jsdwmc"], gc["jldwzh"], gc["jldwmc"], gc["sgdwzh"], gc["sgdwmc"]);

                                lssql.Add(isql);
                            }
                            foreach (var cjry in zzryList)
                            {
                                isql = string.Format("insert into djc_fq_ry (DJCFQBH, CJRYZH, CJRYXM, CJRYGW) ");
                                isql += string.Format("values ('{0}','{1}','{2}','{3}') ", DJCFQBH, cjry.UserId, cjry.UserRealName, "组长");
                                lssql.Add(isql);
                            }
                            foreach (var cjry in azryList)
                            {
                                isql = string.Format("insert into djc_fq_ry (DJCFQBH, CJRYZH, CJRYXM, CJRYGW) ");
                                isql += string.Format("values ('{0}','{1}','{2}','{3}') ", DJCFQBH, cjry.UserId, cjry.UserRealName, "安装监督员");
                                lssql.Add(isql);
                            }

                            foreach (var cjry in tjryList)
                            {
                                isql = string.Format("insert into djc_fq_ry (DJCFQBH, CJRYZH, CJRYXM, CJRYGW) ");
                                isql += string.Format("values ('{0}','{1}','{2}','{3}') ", DJCFQBH, cjry.UserId, cjry.UserRealName, "土建监督员");
                                lssql.Add(isql);
                            }

                            if (!CommonService.ExecTrans(lssql))
                            {
                                msg = "保存大检查工程和人员信息失败！";
                            }
                            else if(DoStartMajorInspection(Request, DJCFQBH, gclist, azryList, tjryList, zzryList, ccbt, out msg))
                            {
                                code = true;
                                data = string.Format("{{\"DJCFQBH\":\"{0}\"}}", DJCFQBH);
                            }

                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg, data));
            }
        }

        /// <summary>
        /// 对于抽取的工程和人员，批量发起大检查流程
        /// </summary>
        /// <param name="request"></param>
        /// <param name="djcfqbh"></param>
        /// <param name="gclist"></param>
        /// <param name="cjryList"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool DoStartMajorInspection(HttpRequestBase request, string djcfqbh, IList<IDictionary<string, string>> gclist,
            IList<VUser> azryList, IList<VUser> tjryList, IList<VUser> zzryList, string ccbt, out string err)
        {
            bool ret = true;
            err = "";
            try
            {
                int processid = request["processid"].GetSafeInt();
               
                if (processid > 0)
                {
                    
                    
                    VOrderFlow flow = WorkFlowService.GetOrderFlow(processid);
                    if (flow == null || flow.Process.Processid == 0 || flow.RealStartActivity == null)
                    {
                        ret = false;
                        err = "无效的流程编号";
                    }
                    else
                    {
                        BD.WorkFlow.Common.ParamReqCreateWork param = new WorkFlow.Common.ParamReqCreateWork(request);
                        // 添加流程相关信息
                        param.ProcessId = flow.Process.Processid;
                        param.ActivityId = flow.RealStartActivity.Activityid;
                        param.NextStep = flow.GetActivityByStep(2).Activityid;
                        param.IsValid = true;
                        param.IsSubmit = true;
                        param.IsCopy = false;
                        // 为每一个抽取的工程，完成大检查第一步流程
                        foreach (IDictionary<string, string> gc in gclist)
                        {
                            // 每一次发起新的流程，都将流程的流水号清空
                            param.Serial = "";
                            CompleteDJCFQFirstStep(param, gc, azryList, tjryList, zzryList, djcfqbh,  ccbt);
                        }

                    }
                    

                }
                else
                {
                    ret = false;
                    err = "无效的流程编号";
                }
                

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                err = ex.Message;
                ret = false;
            }

            return ret;

        }

        private void CompleteDJCFQFirstStep(BD.WorkFlow.Common.ParamReqCreateWork param, IDictionary<string, string> gc,
            IList<VUser> azryList, IList<VUser> tjryList, IList<VUser> zzryList, string djcfqbh,  string ccbt)
        {
            string err = "";
            try
            {   //这里需要根据工程和人员信息，重新打造param参数
                string extrainfo = "view_djc_fq|" + DataFormat.EncodeBase64("djcfqbh='" + djcfqbh + "'");
                extrainfo += "||" + "djc_fq_gc|" + DataFormat.EncodeBase64("djcfqbh='" + djcfqbh + "' and " +  "gcbh='" + gc["gcbh"] + "'");

                string extrainfo2 = "[" + gc["zjdjh"] + "]" + gc["gcmc"];
                param.ExtraInfo = extrainfo;
                param.ExtraInfo2 = extrainfo2;
                param.ExtraInfo3 = gc["gcbh"];

                // 完成大检查流程第一步
                WorkFlowService.CompleteFirstTask(param, GetDJCFQInitFormItem(gc, azryList, tjryList, zzryList, djcfqbh, ccbt),
                        BD.WorkFlow.Common.WorkFlowUser.RealTaskUser, BD.WorkFlow.Common.WorkFlowUser.RealSignUser,
                        RemoteUserService.GetFlowCompanys(BD.WorkFlow.Common.WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowDepartments(BD.WorkFlow.Common.WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowUsers(BD.WorkFlow.Common.WorkFlowConfig.FlowManager),
                        RemoteUserService.GetFlowRoles(BD.WorkFlow.Common.WorkFlowConfig.FlowManager),
                        RemoteUserService.GetUserRoles(RemoteUserService.GetFlowUsers(BD.WorkFlow.Common.WorkFlowConfig.FlowManager)), out err);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }

        private IList<StFormItem> GetDJCFQInitFormItem(IDictionary<string, string> gc, IList<VUser> azryList, IList<VUser> tjryList,
             IList<VUser> zzryList, string djcfqbh, string ccbt)
        {

            IList<StFormItem> items = new List<StFormItem>();
            items.Add(new StFormItem() { Formid = 0, ItemName = "djcfqbh", ItemValue = djcfqbh });
            items.Add(new StFormItem() { Formid = 0, ItemName = "azryzh", ItemValue = string.Join(",", azryList.Select(ry => ry.UserId).ToArray())});
            items.Add(new StFormItem() { Formid = 0, ItemName = "azryxm", ItemValue = string.Join(",", azryList.Select(ry => ry.UserRealName).ToArray()) });
            items.Add(new StFormItem() { Formid = 0, ItemName = "tjryzh", ItemValue = string.Join(",", tjryList.Select(ry => ry.UserId).ToArray()) });
            items.Add(new StFormItem() { Formid = 0, ItemName = "tjryxm", ItemValue = string.Join(",", tjryList.Select(ry => ry.UserRealName).ToArray()) });
            items.Add(new StFormItem() { Formid = 0, ItemName = "zzryzh", ItemValue = string.Join(",", zzryList.Select(ry => ry.UserId).ToArray()) });
            items.Add(new StFormItem() { Formid = 0, ItemName = "zzryxm", ItemValue = string.Join(",", zzryList.Select(ry => ry.UserRealName).ToArray()) });
            items.Add(new StFormItem() { Formid = 0, ItemName = "ccbt", ItemValue = ccbt });
            return items;
        }
        /// <summary>
        /// 校验大检查发起数据
        /// </summary>
        /// <param name="extractegc"></param>
        /// <param name="extractedry"></param>
        /// <param name="leadman"></param>
        /// <param name="ccbt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckMajorInspection(string extractegc, string extractedzzry, string extractedazry, string extractedtjry, string ccbt, out string msg)
        {
            bool ret = true;
            msg = "";
            if (extractegc == "")
            {
                ret = false;
                msg = "抽取的工程为空，请重新抽取！";
            }
            else if (extractedzzry == "")
            {
                ret = false;
                msg = "抽取的组长为空，请重新抽取！";
            }
            else if (extractedazry == "")
            {
                ret = false;
                msg = "抽取的安装监督员为空，请重新抽取！";
            }
            else if (extractedtjry == "")
            {
                ret = false;
                msg = "抽取的土建监督员为空，请重新抽取！";
            }

            else if (ccbt == "")
            {
                ret = false;
                msg = "抽查标题为空，请重新填写！";
            }
            return ret;
        }

        /// <summary>
        /// 获取可以抽取的工程列表
        /// </summary>
        public void GetExtractableGCList()
        {
            string where = " 1=1 ";
            int count = Request["gcsl"].GetSafeInt();
            int skipdays = Request["skipdays"].GetSafeInt();

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                where += " and zt not in ('YT', 'LR', 'JGYS','JDBG','GDZL') ";

                string jdzch = Request["jdzch"].GetSafeString();
                string gcmc =  Request["gcmc"].GetSafeString();
                string zt = Request["zr"].GetSafeString();
                string gclxbh = Request["lx"].GetSafeString();
                string gcqybh = Request["qy"].GetSafeString();
                string jzxz = Request["jzxz"].GetSafeString();
                string jngc = Request["jngc"].GetSafeString();
                string czgc = Request["czgc"].GetSafeString();
                string zdgc = Request["zdgc"].GetSafeString();
                string jglb = Request["jglb"].GetSafeString();
                string jzmj = Request["jzmj"].GetSafeString();
                if (jdzch !="")
                {
                    where += " and zjdjh like '" + jdzch + "' ";
                }
                if (gcmc != "")
                {
                    where += " and gcmc like '" + gcmc + "' ";
                }
                if (zt != "")
                {
                    where += " and zt in (" + DataFormat.FormatSQLInStr(zt) + ") ";
                }
                if (gclxbh != "")
                {
                    where += " and gclxbh in (" + DataFormat.FormatSQLInStr(gclxbh) + ") ";
                }
                if (gcqybh != "")
                {
                    where += " and gcqybh in (" + DataFormat.FormatSQLInStr(gcqybh) + ") ";
                }
                if (jzxz != "")
                {
                    where += " and jzxz in (" + DataFormat.FormatSQLInStr(jzxz) + ") ";
                }
                if (jngc != "")
                {
                    where += " and jngc in(" + DataFormat.FormatSQLInStr(jngc) + ") ";
                }
                if (czgc != "")
                {
                    where += " and czgc in( " + DataFormat.FormatSQLInStr(czgc) + ") ";
                }
                if (zdgc != "")
                {
                    where += " and zdgc in(" + DataFormat.FormatSQLInStr(zdgc) + ") ";
                }
                if (jglb != "")
                {
                    where += " and jglb in(" + DataFormat.FormatSQLInStr(jglb) + ") ";
                }
                if (jzmj != "")
                {
                    where += " and jzmj " + jzmj + " ";
                }
                if (skipdays > 0)
                {
                    where += " and gcbh not in (select distinct gcbh from djc_fq_gc where ccsj >= dateadd(day, " + (-skipdays).ToString() + ", getdate() )) ";
                }
                string sql = string.Format("select top {0} newid() as id, gcbh, gcmc, zjdjh from view_i_m_gc where {1} order by id", count, where);
                datas = CommonService.GetDataTable(sql);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", datas.Count, jss.Serialize(datas)));
                Response.End();
            }

        }


        /// <summary>
        /// 获取所有科长 GetKZList
        /// </summary>
        public void GetKZList()
        {

            IList<VUser> validUsers = new List<VUser>();
            string rolecodes = Request["rolecodes"].GetSafeString();
            //string rolecodes = "CR201611000014";

            try
            {
                if (rolecodes!="")
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

        /// <summary>
        /// 获取请假和因公外出流程中，所有科长和副科长
        /// 请假和因公外出 科长审批这一步包含所有科长和副科长
        /// </summary>
        public void GetWCKZList()
        {
            IList<VUser> validUsers = new List<VUser>();

            string rolecodes = Request["rolecodes"].GetSafeString();

            try
            {
                if (rolecodes != "")
                {
                    
                    List<string> rolelist = rolecodes.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> usercodes = new List<string>();
                    foreach (var rolecode in rolelist)
                    {
                        IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = Remote.UserService.GetUserListByRolecode(rolecode);
                        usercodes.AddRange(urs.Select(x => x.USERCODE).ToList());
                    }
                    if (usercodes.Count > 0)
                    {
                        usercodes = usercodes.Distinct().ToList();
                        validUsers = GetValidUsers().Where(x => usercodes.Contains(x.UserCode)).ToList();
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", validUsers.Count, jss.Serialize(validUsers)));
                Response.End();
            }

        }



        /// <summary>
        /// 根据多个角色代码，获取用户信息
        /// 多个角色代码用逗号分开
        /// </summary>
        [Authorize]
        public void GetUserListByRoleCodes()
        {
            IList<VUser> validUsers = new List<VUser>();

            string rolecodes = Request["rolecodes"].GetSafeString();

            try
            {
                if (rolecodes != "")
                {

                    List<string> rolelist = rolecodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> usercodes = new List<string>();
                    foreach (var rolecode in rolelist)
                    {
                        IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = Remote.UserService.GetUserListByRolecode(rolecode);
                        usercodes.AddRange(urs.Select(x => x.USERCODE).ToList());
                    }
                    if (usercodes.Count > 0)
                    {
                        usercodes = usercodes.Distinct().ToList();
                        validUsers = GetValidUsers().Where(x => usercodes.Contains(x.UserCode)).ToList();
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", validUsers.Count, jss.Serialize(validUsers)));
                Response.End();
            }

        }


        public void GetFGZZList()
        {

            IList<VUser> validUsers = new List<VUser>();
            string rolecode = Request["rolecode"].GetSafeString();
            //string rolecode = "CR201611000012";

            try
            {
                validUsers = GetValidUsers();
                IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = Remote.UserService.GetUserListByRolecode(rolecode);
                List<string> usercodes = urs.Select(x => x.USERCODE).ToList();
                validUsers = validUsers.Where(x => usercodes.Contains(x.UserCode)).ToList();

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



        /// <summary>
        /// 获取可以抽取的组长列表
        /// </summary>
        public void GetExtractableZZRYList()
        {
            //string where = " 1=1 ";
            int count = Request["sl"].GetSafeInt();

            IList<VUser> validUsers = new List<VUser>();


            try
            {
                validUsers = GetValidUsers();
                if (count > 0)
                {

                    var q = (from e in validUsers orderby (Guid.NewGuid()) select e).Take(count);
                    validUsers = q.ToList<VUser>();
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

        /// <summary>
        /// 获取可以抽取的土建监督员列表
        /// </summary>
        public void GetExtractableAZRYList()
        {
            //string where = " 1=1 ";
            int count = Request["sl"].GetSafeInt();
            //string rolecode = "CR201611000019";  // 安装监督员的角色代码
            string rolecode = Request["rolecode"].GetSafeString();
            IList<VUser> validUsers = new List<VUser>();
            IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = new List<BD.Jcbg.Web.RemoteUserService.VUser>();

            try
            {
                validUsers = GetValidUsers();
                if (count > 0)
                {

                   
                    urs = Remote.UserService.GetUserListByRolecode(rolecode);
                    var q = (from e in urs orderby (Guid.NewGuid()) select e).Take(count);
                    urs = q.ToList<BD.Jcbg.Web.RemoteUserService.VUser>();
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


        /// <summary>
        /// 获取可以抽取的安装监督员列表
        /// </summary>
        public void GetExtractableTJRYList()
        {
            //string where = " 1=1 ";
            int count = Request["sl"].GetSafeInt();
            //string rolecode = "CR201611000020";  // 土建监督员的角色代码
            string rolecode = Request["rolecode"].GetSafeString();  // 土建监督员的角色代码
            IList<VUser> validUsers = new List<VUser>();
            IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = new List<BD.Jcbg.Web.RemoteUserService.VUser>();

            try
            {
                validUsers = GetValidUsers();
                if (count > 0)
                {


                    urs = Remote.UserService.GetUserListByRolecode(rolecode);
                    var q = (from e in urs orderby (Guid.NewGuid()) select e).Take(count);
                    urs = q.ToList<BD.Jcbg.Web.RemoteUserService.VUser>();
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
            string strExcludeUsers = "wzzjzadmin";

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
        /// 根据大检查发起编号，获取相应的工程信息和人员信息
        /// </summary>
        public void GetDJCDetailInfo()
        {
            string djcfqbh = Request["djcfqbh"].GetSafeString();
            IList<IDictionary<string, string>> gclist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> rylist = new List<IDictionary<string, string>>();

            try
            {
                string sql = "select gcbh, gcmc, zjdjh, jsdwmc, jldwmc, sgdwmc from djc_fq_gc where djcfqbh='" + djcfqbh + "' ";
                gclist = CommonService.GetDataTable(sql);

                sql = "select cjryxm, cjrygw from djc_fq_ry where djcfqbh='" + djcfqbh + "' ";
                rylist = CommonService.GetDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"gclist\":{0},\"rylist\":{1}}}", jss.Serialize(gclist), jss.Serialize(rylist)));
                Response.End();
            }

        }

        public void GetDJCSearchDS()
        {
            IList<IDictionary<string, string>> gcztlist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> gclxlist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> gcqylist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> jzxzlist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> zdgclist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> jglblist = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select bh, mc from h_gczt where bh NOT in ('YT','LR','JGYS','JDBG','GDZL') order by xssx ";
                gcztlist = CommonService.GetDataTable(sql);

                sql = "select lxbh, lxmc from h_gclx order by xssx ";
                gclxlist = CommonService.GetDataTable(sql);

                sql = "select qybh, qymc from h_gcqy order by xssx ";
                gcqylist = CommonService.GetDataTable(sql);

                sql = "select lxmc from h_gcjzxz order by xssx  ";
                jzxzlist = CommonService.GetDataTable(sql);

                sql = "select lxmc from h_zdgclx order by xssx  ";
                zdgclist = CommonService.GetDataTable(sql);

                sql = "select lxmc from H_GCJGLB order by xssx   ";
                jglblist = CommonService.GetDataTable(sql);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"gcztlist\":{0},\"gclxlist\":{1},\"gcqylist\":{2},\"jzxzlist\":{3},\"zdgclist\":{4},\"jglblist\":{5}}}", 
                    jss.Serialize(gcztlist), jss.Serialize(gclxlist),
                    jss.Serialize(gcqylist), jss.Serialize(jzxzlist),
                    jss.Serialize(zdgclist), jss.Serialize(jglblist)));
                Response.End();
            }
        }
        #endregion
		#region 外部查询

        /// <summary>
        /// 搜索企业
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchQY()
        {
            return View();
        }

        public void GetQYList()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string qymc = Request["qymc"].GetSafeString();
            string qyfzr = Request["qyfzr"].GetSafeString();
            string qyfr = Request["qyfr"].GetSafeString();

            int totalcount = 0;

            string where = "";

            string sql = "select * from View_I_M_QY  where sptg = 1 and sfyx = 1 ";
            if (qymc !="")
            {
                where += " and qymc like '%" + qymc + "%' ";
            }
            if (qyfzr != "")
            {
                where += " and qyfzr like '%" + qyfzr + "%' ";
            }
            if (qyfr != "")
            {
                where += " and qyfr like '%" + qyfr + "%' ";
            }
            if (where !="")
            {
                sql += where;
            }

            sql += " order by qymc asc ";
            IList<IDictionary<string, string>> dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);


            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
        }

        /// <summary>
        /// 搜索工程
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchGC()
        {
            return View();
        }

        public void GetGCList()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            
            string gcmc = Request["gcmc"].GetSafeString();
            string zjdjh = Request["zjdjh"].GetSafeString();
            

            int totalcount = 0;

            string where = "";

            string sql = "select * from View_I_M_GC_LB a where a.zt not in ('YT','LR') ";
            
            if (gcmc != "")
            {
                where += " and gcmc like '%" + gcmc + "%' ";
            }
            if (zjdjh != "")
            {
                where += " and zjdjh like '%" + zjdjh + "%' ";
            }

            
            if (where != "")
            {
                sql += where;
            }

            sql += " order by zjdjh desc ";
            IList<IDictionary<string, string>> dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);


            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
        }



        /// <summary>
        /// 搜索人员
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchRY()
        {
            return View();
        }
        public void GetRYList()
        {
            int pageindex = DataFormat.GetSafeInt(Request["page"], 1);
            int pagesize = DataFormat.GetSafeInt(Request["rows"], 10);
            string ryxm = Request["ryxm"].GetSafeString();
            string qymc = Request["qymc"].GetSafeString();
            string sfzhm = Request["sfzhm"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();

            int totalcount = 0;

            string where = "";

            string sql = "select * from View_I_M_RY where sptg=1 and sfyx=1 ";
            if (ryxm != "")
            {
                where += " and ryxm like '%" + ryxm + "%' ";
            }
            if (qymc != "")
            {
                where += " and qymc like '%" + qymc + "%' ";
            }
            if (sfzhm != "")
            {
                where += " and sfzhm like '%" + sfzhm + "%' ";
            }
            if (sjhm != "")
            {
                where += " and sjhm like '%" + sjhm + "%' ";
            }
            if (where != "")
            {
                sql += where;
            }

            sql += " order by rybh desc ";
            IList<IDictionary<string, string>> dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);


            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
        }

        /// <summary>
        /// 获取所有审批通过的工程
        /// </summary>
        public void GetAllGcList()
        {

            string sql = "select recid,gcbh,qymc,zjdjh,gcmc,gcdd,gclxmc,djjclxbh,jzfl,jzxz,jgxs,cs,jzmj,rfgcmj,dxsmj,gczj,dwmjzj,sjsynx,ghxkzh,schgsbh,scbasbh,gcjhkgrq,gcjhjgrq,gcjbrq,ckjbrxm,gczt,xxjd,gcjdzt,bz,sy_jsdwmc,jsdwxmfzrxm,jsdwxmfzrsfzhm,jsdwxmfzrsjhm,kcdwmc,kcdwxmfzrxm,kcdwxmfzrsfzhm,kcdwxmfzrsjhm,sjdwmc,sjdwxmfzrxm,sjdwxmfzrsfzhm,sjdwxmfzrsjhm,sgdwmc,sgdwxmfzrxm,sgdwxmfzrsfzhm,sgdwxmfzrsjhm,jldwmc,jldwxmfzrxm,jldwxmfzrsfzhm,jldwxmfzrsjhm,tsdwmc,jdgcsxm,tjjdyxm,azjdyxm,gcszrq,gczwysrq,gcjcfbysrq,gcztfbysrq,gcfhysrq,gcyysrq,gcjgysrq from View_I_M_GC_LB where sptg = 1 ";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);


            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", dt.Count, jss.Serialize(dt)));
        }

        public void GetAllRYList()
        {
            string sql = "select qymc,ryxm,xb,rybh,sjhm,zh,yxzssl,sfzhm,sy_hm,yzzt,gcgw from View_I_M_RY where sptg = 1 and sfyx = 1 ";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", dt.Count, jss.Serialize(dt)));
        }


        public void GetAllQYList()
        {
            string sql = "select lxmc,qybh,qymc,qyfzr,lxsj,qyfr,zh,qyfrsj,sy_zzmc,zzjgdm from View_I_M_QY where sptg = 1 and sfyx = 1";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", dt.Count, jss.Serialize(dt)));
        }

        /// <summary>
        /// 外部登录
        /// </summary>
        /// <returns></returns>
        public ActionResult ExternalLogin()
        {
            return View();
        }

        /// <summary>
        /// 获取工程统计信息
        /// </summary>
        public void GetGCTJXX()
        {
            bool code = true;
            string msg = "";
            
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string procstr = "GETGCTJXX()";
                dt = CommonService.ExecDataTableProc(procstr, out msg);
                code = msg == "";
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;

            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
            }
           
            
        }

        #endregion
		#region 自动发起整改
        public ActionResult StartZG()
        {
            string parentserial = Request["parentserial"].GetSafeString();
            string extrainfo = Request["extrainfo"].GetSafeString();
            string extrainfo2 = Request["extrainfo2"].GetSafeString();
            string extrainfo3 = Request["extrainfo3"].GetSafeString();
            string extrainfo4 = Request["extrainfo4"].GetSafeString();
            if (extrainfo!="")
            {
                extrainfo = DataFormat.DecodeBase64(extrainfo);
            }
            if (extrainfo2 != "")
            {
                extrainfo2 = DataFormat.DecodeBase64(extrainfo2);
            }
            if (extrainfo3 != "")
            {
                extrainfo3 = DataFormat.DecodeBase64(extrainfo3);
            }
            if (extrainfo4 != "")
            {
                extrainfo4 = DataFormat.DecodeBase64(extrainfo4);
            }
            ViewBag.ParentSerial = parentserial;
            ViewBag.ExtraInfo = extrainfo;
            ViewBag.ExtraInfo2 = extrainfo2;
            ViewBag.ExtraInfo3 = extrainfo3;
            ViewBag.ExtraInfo4 = extrainfo4;
            return View();
        }
        #endregion
		#region 人员投诉
        [Authorize]
        public void setryts()
        {
            string jdzch = Request["jdzch"].GetSafeString();
            bool code = false;
            string msg = "";
            try
            {
                IList<string> sqls = new List<string>();
                string serverip = Configs.GetOaServerIp;
                sqls.Add("update " + serverip + "[oa_jx].dbo.MJDZC SET ZT='该工程有人员投诉' where JDZCH='" + jdzch + "'");           
                code = CommonService.ExecSqls(sqls);
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
        [Authorize]
        public void Checkrytsfk()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeRequest();
                int checkoption = Request["checkoption"].GetSafeInt();
                string reason = Request["reason"].GetSafeRequest();
                string jdzch=Request["jdzch"].GetSafeRequest();
                IList<string> sqls = new List<string>();
                if (checkoption == 0)
                {
                    sqls.Add("update I_M_RY_TS set sfsp=1,sfyx=0,tjsp=0,tszt='投诉中',spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',REMARK='" + reason + "' where recid='" + recid + "'");
                }
                else
                {
                    sqls.Add("update I_M_RY_TS set sfsp=1,sfyx=1,tszt='已通过',spsj=getdate(),sprzh='" + CurrentUser.UserName + "',sprxm='" + CurrentUser.RealName + "',REMARK='' where recid='" + recid + "'");
                }
                code = CommonService.ExecTrans(sqls);
                //判断该工程状态是否存在其他人员投诉 ，设置ZT为正常
                if (checkoption != 0)
                {
                    string sql = "select count(1) as num from I_M_RY_TS where tszt!='已通过' and jdzch='" + jdzch + "'";
                    IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                    if (list.Count > 0) 
                    {
                        if(list[0]["num"].GetSafeInt()==0)
                        {
                            string serverip = Configs.GetOaServerIp;
                            sql = "update " + serverip + "[oa_jx].dbo.MJDZC SET ZT='正常' where JDZCH='" + jdzch + "'";
                            CommonService.Execsql(sql);
                        }
                        else//还有其他投诉
                        { }


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
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
		#endregion
		#region 质监，安监资料上传
        public ActionResult UploadZL()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            string zllx = Request["zllx"].GetSafeString();
            string gclxbh = Request["gclxbh"].GetSafeString();
            string isedit = Request["isedit"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            ViewBag.GCBH = gcbh;
            ViewBag.ZLLX = zllx;
            ViewBag.GCLXBH = gclxbh;
            ViewBag.ISEDIT = isedit;
            ViewBag.GCMC = gcmc;
            return View();
        }

        public void GetGCZLList()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            string zllx = Request["zllx"].GetSafeString().ToLower();
            string gclxbh = Request["gclxbh"].GetSafeString();
            string isedit = Request["isedit"].GetSafeString();

            IList<IDictionary<string, string>> zllist = new List<IDictionary<string, string>>();
            string sql = "";
            try
            {
                if (gcbh !="")
                {
                    // 质监资料
                    if (zllx == "zj")
                    {
                        sql = "select zllxbh as lxbh, lxmc, files from VIEW_I_S_GC_ZJZL where gcbh='" + gcbh + "' and gclxbh='" + gclxbh + "'";
                        zllist = CommonService.GetDataTable(sql);
                        // 当前工程没有上传过质监资料
                        if (zllist.Count == 0)
                        {
                            sql = "select lxbh, lxmc, null as files from h_gczjzllx  where gclxbh='" + gclxbh + "' order by xssx ";
                            zllist = CommonService.GetDataTable(sql);
                        }


                    }
                    // 安监资料
                    else if (zllx == "aj")
                    {
                        sql = "select zllxbh as lxbh, lxmc, files from VIEW_I_S_GC_AJZL where gcbh='" + gcbh + "'";
                        zllist = CommonService.GetDataTable(sql);
                        // 当前工程没有上传过安监资料
                        if (zllist.Count == 0)
                        {
                            sql = "select lxbh, lxmc, null as files from h_gcajzllx order by xssx ";
                            zllist = CommonService.GetDataTable(sql);
                        }
                    }  //基础验收
                    else if(zllx=="jcys")
                    {
                        sql = "select zllxbh as lxbh, lxmc, files from VIEW_I_S_GC_JCYSZL where gcbh='" + gcbh + "' and gclxbh='" + gclxbh + "'";
                        zllist = CommonService.GetDataTable(sql);
                        // 当前工程没有上传过安监资料
                        if (zllist.Count == 0)
                        {
                            sql = "select lxbh, lxmc, null as files from h_gcjcyszllx where gclxbh='" + gclxbh + "' order by xssx ";
                            zllist = CommonService.GetDataTable(sql);
                        }
                    }
                    else if (zllx == "ztys")
                    {
                        sql = "select zllxbh as lxbh, lxmc, files from VIEW_I_S_GC_ZTYSZL where gcbh='" + gcbh + "' and gclxbh='" + gclxbh + "'";
                        zllist = CommonService.GetDataTable(sql);
                        // 当前工程没有上传过安监资料
                        if (zllist.Count == 0)
                        {
                            sql = "select lxbh, lxmc, null as files from h_gcztyszllx where gclxbh='" + gclxbh + "' order by xssx ";
                            zllist = CommonService.GetDataTable(sql);
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", zllist.Count, jss.Serialize(zllist)));
                Response.End();
            }

        }

        public void DoUploadZL()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string zllx = Request["zllx"].GetSafeString().ToLower();
                string gclxbh = Request["gclxbh"].GetSafeString();
                string isedit = Request["isedit"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string bhliststr = Request["bhlist"].GetSafeString();
                string fileliststr = Request["filelist"].GetSafeString();
                List<string> bhlist = bhliststr.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                List<string> filelist = fileliststr.Split(new string[] { "," }, StringSplitOptions.None).ToList();

                if (gcbh !="")
                {
                    
                    if (bhlist.Count != filelist.Count)
                    {
                        code = false;
                        msg = "数据不匹配，上传失败!";
                    }
                    else
                    {
                        IList<string> lssql = new List<string>();
                        string sql = "";
                        if (zllx == "zj")
                        {
                            
                            sql = string.Format("delete from I_S_GC_ZJZL where gcbh='{0}' and zllxbh in (select lxbh from H_GCZJZLLX where gclxbh='{1}')", gcbh, gclxbh);
                            lssql.Add(sql);
                            for (int i = 0; i < bhlist.Count && i<filelist.Count; i++)
                            {
                                sql = string.Format("insert into I_S_GC_ZJZL (GCBH, ZLLXBH, FILES) VALUES ('{0}', '{1}', '{2}')", gcbh, bhlist[i].GetSafeString(), filelist[i].GetSafeString());
                                lssql.Add(sql);
                            }
                        }
                        else if (zllx == "aj")
                        {
                            sql = string.Format("delete from I_S_GC_AJZL where gcbh='{0}' and zllxbh in (select lxbh from H_GCAJZLLX) ", gcbh);
                            lssql.Add(sql);
                            for (int i = 0; i < bhlist.Count && i < filelist.Count; i++)
                            {
                                sql = string.Format("insert into I_S_GC_AJZL (GCBH, ZLLXBH, FILES) VALUES ('{0}', '{1}', '{2}')", gcbh, bhlist[i].GetSafeString(), filelist[i].GetSafeString());
                                lssql.Add(sql);
                            }

                        }
                        else if (zllx == "jcys")
                        {
                            sql = string.Format("delete from I_S_GC_JCYSZL where gcbh='{0}' and zllxbh in (select lxbh from H_GCJCYSZLLX) ", gcbh);
                            lssql.Add(sql);
                            for (int i = 0; i < bhlist.Count && i < filelist.Count; i++)
                            {
                                sql = string.Format("insert into I_S_GC_JCYSZL (GCBH, ZLLXBH, FILES) VALUES ('{0}', '{1}', '{2}')", gcbh, bhlist[i].GetSafeString(), filelist[i].GetSafeString());
                                lssql.Add(sql);
                            }

                        }
                        else if (zllx == "ztys")
                        {
                            sql = string.Format("delete from I_S_GC_ZTYSZL where gcbh='{0}' and zllxbh in (select lxbh from H_GCZTYSZLLX) ", gcbh);
                            lssql.Add(sql);
                            for (int i = 0; i < bhlist.Count && i < filelist.Count; i++)
                            {
                                sql = string.Format("insert into I_S_GC_ZTYSZL (GCBH, ZLLXBH, FILES) VALUES ('{0}', '{1}', '{2}')", gcbh, bhlist[i].GetSafeString(), filelist[i].GetSafeString());
                                lssql.Add(sql);
                            }

                        }
                        if (!CommonService.ExecTrans(lssql))
                        {
                            code = false;
                            msg = "资料上传失败！";
                        }
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
		#region 监督办公报表
        /// <summary>
        /// 监督办公报表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult JDBGReport()
        {
            string url = "";
            string reportFile = Request["reportfile"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            int isprint = Request["print"].GetSafeInt(1);

            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            c.fileindex = "0";
            c.table = "view_i_m_gc|view_gc_ry|view_gc_qy|view_gc_xctp";
            c.filename = reportFile;

            c.where = "gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'|gcbh='" + gcbh + "'";
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print;
            c.openType = ReportPrint.OpenType.PDF;
            c.AllowVisitNum = 1;
            if (isprint == 1)
                c.customtools = "1,|2,";
            else
                c.customtools = "2,";

            var guid = g.Add(c);

            

            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }
        #endregion
		#region 查询
        /// <summary>
        /// 用户忘记账号和密码时，通过输入用户姓名可以查找相应的账号
        /// </summary>
        public void GetYHList()
        {
            bool ret = true;
            string zhlx= Request["zhlx"].GetSafeString();
            string yhxm = Request["yhxm"].GetSafeString();

            List<IDictionary<string, string>> yhlist = new List<IDictionary<string, string>>();
            // 外部人员账号
            List<IDictionary<string, string>> wbyhlist = new List<IDictionary<string, string>>();
            // 站内人员账号
            List<IDictionary<string, string>> nbyhlist = new List<IDictionary<string, string>>();
            try
            {
                if (yhxm != "")
                {
                    string sql = "";
                    // 用户账号查询（包括外部注册人员和内部员工）
                    if (zhlx=="0")
                    {
                        // 查询外部注册人员
                        sql = string.Format("select mc, zh, sfzhm from view_i_m_zh where lx='人员'and mc like '%{0}%'", yhxm);
                        wbyhlist = CommonService.GetDataTable(sql).ToList();
                        if (wbyhlist.Count > 0)
                        {
                            foreach (IDictionary<string, string> yh in wbyhlist)
                            {
                                string mc = yh["mc"].GetSafeString();
                                string sfzhm = yh["sfzhm"].GetSafeString();
                                if (sfzhm !="")
                                {
                                    string maskedsfzhm = sfzhm;
                                    if (sfzhm.Length > 10)
                                    {
                                        maskedsfzhm = sfzhm.Substring(0, 6) + new string('*', sfzhm.Length - 10) + sfzhm.Substring(sfzhm.Length-4);
                                    }
                                    yh["mc"] = mc + "--" + "身份证号码：" + maskedsfzhm;
                                 }
                            }
                        }
                        // 查询内部员工
                        sql = string.Format("select realname as mc, username as zh, idno as sfzhm from userinfo where realname like '%{0}%'", yhxm);
                        nbyhlist = CommonService.GetDataTable(sql).ToList();
                        if (nbyhlist.Count > 0)
                        {
                            foreach (IDictionary<string, string> yh in nbyhlist)
                            {
                                string mc = yh["mc"].GetSafeString();
                                yh["mc"] = mc + "--" + "站内人员";
                            }
                        }
                        yhlist.AddRange(wbyhlist);
                        yhlist.AddRange(nbyhlist);


                    }
                    // 企业账号查询
                    else if(zhlx=="1")
                    {
                        sql = string.Format("select mc, zh, sfzhm from view_i_m_zh where lx='企业' and (mc like '%{0}%' or (exists (select * from i_m_qy_ls where qybh=view_i_m_zh.id and qymc like '%{0}%')))", yhxm);
                        yhlist = CommonService.GetDataTable(sql).ToList();
                    }

                    // 隐藏身份证号码中的中间几位（只显示前6位与后4位）
                    foreach (IDictionary<string, string> item in yhlist)
                    {
                        string sfzhm = item["sfzhm"].GetSafeString();
                        if (sfzhm !="" && sfzhm.Length > 10)
                        {
                            item["sfzhm"] = sfzhm.Substring(0, 6) + new string('*', sfzhm.Length - 10) + sfzhm.Substring(sfzhm.Length - 4);
                        }
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":{2}, \"zhlx\": \"{3}\"}}", yhlist.Count, jss.Serialize(yhlist), ret ?"0":"1", zhlx));
                Response.End();
            }

        }
        #endregion


        #region 整改单未回复状态更新
        [Authorize]
        public void UpdateZGDHFZT()
        {
            bool code = false;
            string msg = "";
            try
            {
                
                string hfzt = Request["hfzt"].GetSafeString();
                string serial = Request["serial"].GetSafeString();

                if (serial != "" && hfzt!="")
                {
                    // 获取已经保存过的整改单记录
                    string sql = "select workserial from jdbg_jdjl where lx='ZGD' and workserial='" + serial + "' ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        serial = dt[0]["workserial"];

                        if (serial != "")
                        {
                            // 根据workserial, 更新reportfile字段
                            string updatesql = string.Format("update jdbg_jdjl set extrainfo3='{0}'  where workserial='{1}' ", hfzt, serial);
                            IList<string> lsupdatesql = new List<string>();
                            lsupdatesql.Add(updatesql);
                            code = CommonService.ExecTrans(lsupdatesql);
                            msg = code ? "" : "设置失败！";

                        }
                    }
                    else
                    {
                        msg = "无法找到相应的整改记录！";
                    }
                }
                else
                {
                    msg = "流水号或未回复状态不能为空！";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        public ActionResult ZGDHFZT()
        {
            string currenthfzt = "";
            string serial = Request["serial"].GetSafeString();
            string zgdbh = Request["zgdbh"].GetSafeString();
            if (serial !="" && zgdbh !="")
            {
                string sql = string.Format("select extrainfo3 from view_jdbg_jdjl where lx='zgd' and workserial='{0}' and extrainfo4='{1}'", serial, zgdbh);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    currenthfzt = dt[0]["extrainfo3"].GetSafeString();
                }

            }
            ViewBag.currenthfzt = currenthfzt;

            return View();
            
        }

        #endregion

		#region 人员备案

        public ActionResult ReservedRYList()
        {
            string rybhlist = Request["rybhlist"];
            ViewBag.rybhlist = rybhlist;
            return View();
        }

        public void GetReservedRYList()
        {
            string msg = "";
            bool code = true;

            string rybhlist = Request["rybhlist"].GetSafeString();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                
                if (rybhlist !="")
                {
                    string sql = "select * from view_i_m_ry where rybh in (" + DataFormat.FormatSQLInStr(rybhlist) + ")";
                    dt = CommonService.GetDataTable(sql);
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
                
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" :"1", msg ,dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }

        public void DoMergeGc()
        {
            string msg = "";
            bool code = true;
            try
            {
                string reservedrybh = Request["reservedrybh"].GetSafeString();
                string removingrybhlist = Request["removingrybhlist"].GetSafeString();
                if (reservedrybh !="" && removingrybhlist!="")
                {
                    string procstr = string.Format("mergegc('{0}', '{1}')", reservedrybh, removingrybhlist);
                    code = CommonService.ExecProc(procstr, out msg);
                    msg = code ? "" : "操作失败！";

                }
                else
                {
                    code = false;
                    msg = "参数不完整！";
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

        public ActionResult SplitedGcList()
        {
            string rybh = Request["rybh"];
            string ryxm = Request["ryxm"];
            ViewBag.rybh = rybh;
            ViewBag.ryxm = ryxm;
            return View();
        }

        public void GetSplitedGCList()
        {
            string msg = "";
            bool code = true;

            string rybh = Request["rybh"].GetSafeString();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {

                if (rybh != "")
                {
                    string sql = string.Format("select * from view_gc_ry where rybh='{0}'", rybh);
                    dt = CommonService.GetDataTable(sql);
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }


        public void DoSplitGc()
        {
            string msg = "";
            bool code = true;
            try
            {
                string splitedgcbhlist = Request["splitedgcbhlist"].GetSafeString();
                string sourcerybh = Request["sourcerybh"].GetSafeString();
                string sourceryxm = Request["sourceryxm"].GetSafeString();
                if (splitedgcbhlist != "" && sourcerybh != "")
                {
                    string procstr = string.Format("splitgc('{0}', '{1}','{2}')", splitedgcbhlist, sourcerybh, sourceryxm);
                    code = CommonService.ExecProc(procstr, out msg);
                    msg = code ? "" : "操作失败！";

                }
                else
                {
                    code = false;
                    msg = "参数不完整！";
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
		#region 根据流水号获取整改记录
        public void GetZGJL()
        {
            bool ret = true;
            string workserial = Request["workserial"].GetSafeString();
            string zgdbh = Request["zgdbh"].GetSafeString();
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> yqdt = new List<IDictionary<string, string>>();
            int yqcs = 0;

            try
            {
                if (workserial != "")
                {
                    string sql = "";
                    sql = string.Format("select * from view_jdbg_jdjl where workserial='{0}' and lx='ZGD' ", workserial);
                    zgjllist = CommonService.GetDataTable(sql);
                    sql = string.Format("select count(*) as sum from view_jdbg_zgdyqjl where extrainfo4='{0}' and lx='ZGDYQ'", zgdbh);
                    yqdt = CommonService.GetDataTable(sql);
                    if (yqdt.Count > 0)
                    {
                        yqcs = yqdt[0]["sum"].GetSafeInt();
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":{2}, \"yqcs\":{3}}}", zgjllist.Count, jss.Serialize(zgjllist), ret ? "0" : "1", yqcs));
                Response.End();
            }
        }
        #endregion

		#region 发送邮件
        public void SetInfoMail()
        {
            bool code = true;
            string msg = "";
            try
            {
                string username = "";
                string realname = "";
                string mailcon = Request["mailcon"].GetSafeString();
                string maintitle = Request["maintitle"].GetSafeString();
                string Rolecode = Configs.GetConfigItem("zfryrole");
                string json = UserService.GetRoleUser(Rolecode);

                JToken jsons = JToken.Parse(json);//转化为JToken（JObject基类）
                JToken arr = jsons["data"];
                foreach (JToken baseJ in arr)//遍历数组
                {
                    username = baseJ.Value<string>("USERNAME");
                    realname = baseJ.Value<string>("REALNAME");
                    code = CommonService.sendmail(username, realname, mailcon, maintitle);
                    if (!code)
                    {
                        msg = "执行存储过程失败";
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
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion
        #region 整改单处罚

        /// <summary>
        /// 发送整改单处罚结果
        /// </summary>
        public void ZGDCFSENDMAIL()
        {
            bool ret = true;
            string msg = "";
            string serial = Request["serial"].GetSafeString();
            string reportfile = Request["reportfile"].GetSafeString();
            string ykfrq = Request["ykfrq"].GetSafeString();
            try
            {
                if (serial == "" || reportfile == "")
                {
                    ret = false;
                    msg = "流水号为空，无法获取数据！";
                }
                else
                {

                    string procstr = string.Format("DoZGDCFSendMail('{0}','{1}','{2}')", serial, HttpUtility.UrlEncode(reportfile), ykfrq);
                    CommonService.ExecDataTableProc(procstr, out msg);
                    ret = msg == "";

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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }



        /// <summary>
        /// 显示整改单处罚统计页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ZGDCF()
        {
            bool ret = true;
            string msg = "";
            string lastcfsj = "1900-01-01";
            string isshow = "0";
            IList<IDictionary<string, string>> cfsjlist = new List<IDictionary<string, string>>();
            try
            {
                string procstr = "GetZGDCFLastCFSJ()";
                cfsjlist = CommonService.ExecDataTableProc(procstr, out msg);
                ret = msg == "";
                if (cfsjlist.Count > 0)
                {
                    lastcfsj = cfsjlist[0]["lastcfsj"].GetSafeString();
                }
                else
                {
                    lastcfsj = "1900-01-01";

                }

                string sql = string.Format("select * from H_GCXG_SPR where lx='ZGDCFTJ' and  USERCODE='{0}' ", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    isshow = "1";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            ViewBag.lastcfsj = lastcfsj;
            ViewBag.currentcfsj = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.isshow = isshow;
            return View();

        }

        /// <summary>
        /// 获取整改单处罚统计记录（待处罚的整改单），以供筛选
        /// </summary>
        public void GetZGDCFTJJL()
        {
            bool ret = true;
            string msg = "";
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();

            try
            {
                if (lastcfsj=="" || currentcfsj=="")
                {
                    ret = false;
                    msg = "上次截止日期与本次截止日期不能为空";
                }
                else
                {
                    DateTime last = DateTime.Now;
                    DateTime current = DateTime.Now;
                    if (! DateTime.TryParse(lastcfsj, out last))
                    {
                        ret = false;
                        msg = "上次截止日期格式不对！";
                    }
                    else if (!DateTime.TryParse(currentcfsj, out current))
                    {
                        ret = false;
                        msg = "本次截止日期格式不对！";
                    }
                    else
                    {
                        //string sql = "";
                        //sql = string.Format("select * from view_jdbg_jdjl where lx='ZGD' ");
                        //zgjllist = CommonService.GetDataTable(sql);


                        string procstr = "";
                        procstr = string.Format("GetZGDCFTJJL('{0}','{1}')", lastcfsj, currentcfsj);
                        zgjllist = CommonService.ExecDataTableProc(procstr, out msg);
                        ret = msg == "";
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\", \"msg\": \"{3}\"}}", zgjllist.Count, jss.Serialize(zgjllist), ret ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        /// 根据多个整改单编号，获取相应的整改单内容
        /// </summary>
        public void GetZGDList()
        {
            bool ret = true;
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();


            try
            {
                if (zgdbhlist != "")
                {
                    string[] zgds = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (zgds.Count() > 0)
                    {
                        zgdbhlist = string.Join(",", zgds);
                        string sql = "";
                        sql = string.Format("select * from view_jdbg_jdjl where lx='ZGD'  and extrainfo4 in ({0})", DataFormat.FormatSQLInStr(zgdbhlist));
                        zgjllist = CommonService.GetDataTable(sql);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\"}}", zgjllist.Count, jss.Serialize(zgjllist), ret ? "0" : "1"));
                Response.End();
            }
        }

        /// <summary>
        /// 根据某个整改单编号，获取相应整改单内容，以及该整改单最后的审核人是否是分管站长
        /// </summary>
        public void GetZGD()
        {
            bool ret = true;
            string zgdbh = Request["zgdbh"].GetSafeString();
            string rolecode = Request["rolecode"].GetSafeString(); // 副站长角色代码  "CR201611000015"
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();
            bool shrisfgzz = false;
            


            try
            {
                if (zgdbh != "")
                {
                    string[] zgds = zgdbh.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (zgds.Count() > 0)
                    {
                        zgdbh = string.Join(",", zgds);
                        string sql = "";
                        sql = string.Format("select * from view_jdbg_jdjl where lx='ZGD'  and extrainfo4 in ({0})", DataFormat.FormatSQLInStr(zgdbh));
                        zgjllist = CommonService.GetDataTable(sql);
                        if (zgjllist.Count > 0)
                        {
                            string shrzh = zgjllist[0]["lrrzh"].GetSafeString();
                            if (shrzh !="")
                            {
                                shrisfgzz = HasRole(shrzh, rolecode);
                            }
                        }
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\", \"shrisfgzz\": \"{3}\"}}", zgjllist.Count, jss.Serialize(zgjllist), ret ? "0" : "1", shrisfgzz ? "1" : "0"));
                Response.End();
            }
        }

        /// <summary>
        /// 根据用户名和角色ID, 判断该用户是否具有相应的角色
        /// </summary>
        /// <param name="username"></param>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        private bool HasRole(string username, string rolecode)
        {
            if (username == "" || rolecode == "")
                return false;
            IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = Remote.UserService.GetUserListByRolecode(rolecode);
            if (urs.Count == 0)
            {
                return false;
            }
            else
            {
                urs = urs.Where(x => x.USERNAME==username).ToList();
                return urs.Count > 0;

            }

 
        }



        public void DoZGDCF()
        {
            bool ret = true;
            string msg = "";
            string success = Request["success"].GetSafeString();
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string workseriallist = Request["workseriallist"].GetSafeString();

            try
            {
                if (success=="" || zgdbhlist=="")
                {
                    ret = false;
                    msg = "处罚结果为空或者整改单编号为空！";
                }
                else
                {
                    string[] bhlist = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (bhlist.Count() > 0)
                    {
                        zgdbhlist = string.Join(",", bhlist);
                        string procstr = string.Format("DoZGDCF('{0}','{1}','{2}')", success, zgdbhlist, workseriallist);
                        CommonService.ExecDataTableProc(procstr, out msg);
                        ret = msg == "";
                    }
                    else
                    {
                        ret = false;
                        msg = "整改单编号不能为空！";
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}",  ret ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 设置撤销整改单处罚结果
        /// </summary>
        public void DoCXZGDCF()
        {
            bool ret = true;
            string msg = "";
            string success = Request["success"].GetSafeString();
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string cfworkserial = Request["cfworkserial"].GetSafeString(); 
            try
            {
                if (success == "" || zgdbhlist == "")
                {
                    ret = false;
                    msg = "撤销结果为空或者整改单编号为空！";
                }
                else
                {
                    string[] bhlist = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (bhlist.Count() > 0)
                    {
                        zgdbhlist = string.Join(",", bhlist);
                        string procstr = string.Format("DoCXZGDCF('{0}','{1}','{2}')", success, zgdbhlist, cfworkserial);
                        CommonService.ExecDataTableProc(procstr, out msg);
                        ret = msg == "";
                    }
                    else
                    {
                        ret = false;
                        msg = "整改单编号不能为空！";
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

       
        public void GetALLGcsAndZgds()
        {
            bool ret = true;
            string msg = "";
            string zjdjhlist = Request["zjdjhlist"].GetSafeString();
            IList<IDictionary<string, string>> gclist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> zgdlist = new List<IDictionary<string, string>>();
            try
            {
                if (zjdjhlist == "" )
                {
                    ret = false;
                    msg = "监督登记号为空！";
                }
                else
                {
                    string[] bhlist = zjdjhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (bhlist.Count() > 0)
                    {
                        zjdjhlist = string.Join(",", bhlist);
                        string sql = "";
                        // 获取工程列表
                        sql = string.Format(" select * from view_i_m_gc where left(zjdjh,8) in (" + DataFormat.FormatSQLInStr(zjdjhlist) + ")");
                        gclist = CommonService.GetDataTable(sql);
                        // 获取整改单列表
                        sql = string.Format(" select * from view_jdbg_jdjl where lx='ZGD' and left(zjdjh, 8) in (" + DataFormat.FormatSQLInStr(zjdjhlist) + ") order by extrainfo4 asc");
                        zgdlist = CommonService.GetDataTable(sql);
                    }
                    else
                    {
                        ret = false;
                        msg = "监督登记号为空！";
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\", \"gclist\":{2}, \"gccount\": {3},\"zgdlist\":{4}, \"zgdcount\": {5}}}", ret ? "0" : "1", msg, jss.Serialize(gclist), gclist.Count, jss.Serialize(zgdlist), zgdlist.Count));
                Response.End();
            }

        }

        // 校验整改单经办人是否为当前用户
        public void CheckZGDJBR()
        {
            bool ret = true;
            string msg = "";
            string jbr = Request["jbr"].GetSafeString();
            try
            {
                if (jbr=="")
                {

                    ret = false;
                    msg = "整改单经办人不能为空！";
                }
                else
                {
                    bool isjbr = jbr == CurrentUser.UserName;
                    msg = isjbr ? "1" : "0";
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }



        #endregion


        #region 安排验收

        /// <summary>
        /// 根据账号获取用户信息
        /// </summary>
        public void GetUserinfoByzh()
        {
            bool ret = true;
            string zhlist = Request["zhlist"].GetSafeString();

            List<string> departmentidlist = new List<string>();
            List<string> departmentnamelist = new List<string>();
            List<string> wcjbrzhlist = new List<string>();
            List<string> wcjbrxmlist = new List<string>();

            try
            {
                if (zhlist != "")
                {
                    string[] zhs = zhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (zhs.Count() > 0)
                    {
                        IList<VUser> validusers = GetValidUsers();
                        IList<BD.Jcbg.Web.RemoteUserService.VUser> urs = Remote.UserService.Users;
                        foreach (var zh in zhs)
                        {
                            VUser vu = validusers.Where(x => x.UserId == zh).FirstOrDefault();
                            if (vu != null)
                            {
                                string usercode = vu.UserCode;
                                departmentidlist.Add(vu.DepartmentId);
                                wcjbrzhlist.Add(vu.UserId);
                                wcjbrxmlist.Add(vu.UserRealName);
                                BD.Jcbg.Web.RemoteUserService.VUser rvu = urs.Where(x => x.USERCODE == usercode).FirstOrDefault();
                                departmentnamelist.Add(rvu == null ? "" : rvu.DEPNAME);
                            }

                        }
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{ \"code\":\"{0}\", \"departmentidlist\":\"{1}\",\"departmentnamelist\":\"{2}\",\"wcjbrzhlist\":\"{3}\",\"wcjbrxmlist\":\"{4}\",\"count\":\"{5}\"}}", ret ? "0" : "1", string.Join(",", departmentidlist), string.Join(",", departmentnamelist), string.Join(",", wcjbrzhlist), string.Join(",", wcjbrxmlist), departmentidlist.Count));
                Response.End();
            }

        }
        #endregion

        #region 流程操作

        [Authorize]
        public JsonResult DelZgtzItem(string serial, string prefix)
        {
            bool code = true;
            string msg = "";
            try
            {
                if (serial != "" && prefix != "")
                {
                    string sql = "delete from stformitem where formid=(select formid from stform where serialno='" + serial +"') and itemname like '%" + prefix + "%'";
                    IList<string> sqls = new List<string>();
                    sqls.Add(sql);
                    
                    code = CommonService.ExecTrans(sqls, out msg);
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }

            return Json(new { code = code ? "0" : "1", msg = msg });

        }
        #endregion

        #region 延期整改单查看附件
        public ActionResult ShowYQFJ()
        {
            string serial = Request["serial"].GetSafeString();
            string zgdbh = Request["zgdbh"].GetSafeString();
            string fileids = "";
            string yqhfrq = "";
            string yqts = "";
            string yqsj = "";
            string zgdserial = "";
            string zgdreport = "";
            string jdjlid = "";
            List<string> idlist = new List<string>();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            if (serial != "")
            {
                string sql = string.Format("select * from stform where serialno='{0}'", serial);
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    int formid = dt[0]["formid"].GetSafeInt();
                    if (formid > 0)
                    {
                        // 获取附件ID列表
                        sql = string.Format("select fileid from view_zgdyq_ztfj where xformid={0}", formid);
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count>0)
                        {
                            for(int j=0; j<dt.Count; j++)
                            {
                                string id = dt[j]["fileid"].GetSafeString();
                                if (id!="")
                                {
                                    idlist.Add(id);
                                }
                            }
                            if (idlist.Count>0)
                            {
                                fileids = string.Join(",", idlist);
                            }
                        }

                        // 获取原始整改期限
                        sql = string.Format("select convert(nvarchar(max), itemvalue) as yqhfrq  from stformitem where formid={0} and itemname='yqhfrq' ", formid);
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            yqhfrq = dt[0]["yqhfrq"].GetSafeString();
                        }

                        // 获取延期天数
                        sql = string.Format("select convert(nvarchar(max), itemvalue) as yqts  from stformitem where formid={0} and itemname='yqts' ", formid);
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            yqts = dt[0]["yqts"].GetSafeString();
                        }


                        // 获取延期时间
                        sql = string.Format("select convert(nvarchar(max), itemvalue) as yqsj  from stformitem where formid={0} and itemname='yqsj' ", formid);
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            yqsj = dt[0]["yqsj"].GetSafeString();
                        }

                        // 获取整改单报告
                        sql = string.Format("select recid, workserial, reportfile   from view_jdbg_jdjl where extrainfo4='{0}' and lx='zgd' ", zgdbh);
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                             jdjlid= dt[0]["recid"].GetSafeString();
                             zgdserial = dt[0]["workserial"].GetSafeString();
                             zgdreport = dt[0]["reportfile"].GetSafeString();
                        }

                    }
                }
            }

            ViewBag.idlist = fileids;
            ViewBag.yqhfrq = yqhfrq;
            ViewBag.yqts = yqts;
            ViewBag.yqsj = yqsj;
            ViewBag.jdjlid = jdjlid;
            ViewBag.zgdserial = zgdserial;
            ViewBag.zgdreport = zgdreport;
            return View();


        }
        #endregion

        #region 短信管理

        #region 通讯录管理

        public ActionResult Sms()
        {
            return View();
        }

        public ActionResult SmsWelcome()
        {
            return View();
        }

        public void GetSmSTree()
        {
            List<VCheckItem> ret = new List<VCheckItem>();
            try
            {
                string sql = "select * from OA_SMS_FUNMENU";

                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

                dt = CommonService.GetDataTable(sql);

                List<VCheckItem> list = GetCheckItems(dt, "");
                if (list.Count > 0)
                {
                    ret.AddRange(list);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(ret));
            }
        }

        public ActionResult SmsTXL()
        {
            return View();
        }

        /// <summary>
        /// 通讯录新增和编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult SmsTXLAdd()
        {
            string pid = Request["pid"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string name = Request["name"].GetSafeString();
            ViewBag.pid = pid;
            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }

        /// <summary>
        /// 保存通讯录目录
        /// </summary>
        [Authorize]
        public void SaveSmsML()
        {
            bool ret = true;
            string msg = "";
            string pid = Request["pid"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string name = Request["name"].GetSafeString();
            try
            {
                if (pid == "")
                {
                    ret = false;
                    msg = "父目录不能为空！";
                }
                else
                {
                    if (name == "")
                    {
                        ret = false;
                        msg = "目录名称不能为空！";
                    }
                    else
                    {
                        string procstr = string.Format("DoSaveSmsML('{0}', '{1}', '{2}')", pid, id, name);
                        ret = CommonService.ExecProc(procstr, out msg);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }


        }

        public void DeleteSmsML()
        {
            bool ret = true;
            string msg = "";
            string id = Request["id"].GetSafeString();
            try
            {
                if (id == "")
                {
                    ret = false;
                    msg = "目录不存在！";
                }
                else
                {
                    string procstr = string.Format("DoDeleteSmsML('{0}')", id);
                    ret = CommonService.ExecProc(procstr, out msg);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

        public void GetSmSTxlTree()
        {
            List<VCheckItem> ret = new List<VCheckItem>();
            try
            {
                string sql = "select * from OA_SMS_TXLML";

                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

                dt = CommonService.GetDataTable(sql);

                List<VCheckItem> list = GetCheckItems(dt, "");
                if (list.Count > 0)
                {
                    ret.AddRange(list);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(new JavaScriptSerializer().Serialize(ret));
            }
        }

        public ActionResult SmsCY()
        {
            return View();
        }

        public ActionResult SmsTXLCYAdd()
        {
            string mlid = Request["mlid"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string name = Request["name"].GetSafeString();
            string zw = Request["zw"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();
            ViewBag.mlid = mlid;
            ViewBag.id = id;
            ViewBag.name = name;
            ViewBag.zw = zw;
            ViewBag.sjhm = sjhm;
            return View();
        }

        public void SaveSmsCY()
        {
            bool ret = true;
            string msg = "";
            string mlid = Request["mlid"].GetSafeString();
            string id = Request["id"].GetSafeString();
            string name = Request["name"].GetSafeString();
            string zw = Request["zw"].GetSafeString();
            string sjhm = Request["sjhm"].GetSafeString();
            try
            {
                if (mlid == "")
                {
                    ret = false;
                    msg = "目录不能为空！";
                }
                else
                {
                    if (name == "")
                    {
                        ret = false;
                        msg = "姓名不能为空！";
                    }
                    else if (sjhm == "")
                    {
                        ret = false;
                        msg = "手机不能为空！";
                    }
                    else
                    {
                        string procstr = string.Format("DoSaveSmsCY('{0}', '{1}', '{2}', '{3}', '{4}')", mlid, id, name, zw, sjhm);
                        ret = CommonService.ExecProc(procstr, out msg);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }


        }

        public void ExportAllCY()
        {
            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {

                //居中样式
                ICellStyle cellstyle = wk.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.WrapText = true;
                //创建一个Sheet  
                ISheet sheet = wk.CreateSheet("所有成员");
                //sheet.DefaultColumnWidth = 15 * 10;
                //sheet.DefaultRowHeightInPoints = 15;  
                IRow row;
                ICell cell;

                // 定义所有列
                List<KeyValuePair<string,string>> listcol = new List<KeyValuePair<string, string>>() { };
                listcol.Add(new KeyValuePair<string, string>("姓名", "name"));
                listcol.Add(new KeyValuePair<string, string>("职位", "zw"));
                listcol.Add(new KeyValuePair<string, string>("手机号码", "sjhm"));
                listcol.Add(new KeyValuePair<string, string>("所在目录", "mlname"));
                listcol.Add(new KeyValuePair<string, string>("顺序号", "xssx"));



                 //定义导出标题
                 row = sheet.CreateRow(0);
                for (int i = 0; i < listcol.Count; i++)
                {
                    sheet.SetColumnWidth(i, 20 * 256);
                    cell = row.CreateCell(i);
                    cell.SetCellValue(listcol[i].Key);
                    cell.CellStyle = cellstyle;
                }

                string sql = "select * from view_oa_sms_txlcy order by mlid asc, xssx asc ";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int j = 0; j < dt.Count; j++)
                {
                    row = sheet.CreateRow(j + 1);
                    for (int i = 0; i < listcol.Count; i++)
                    {
                        //sheet.SetColumnWidth(i, 20 * 256);
                        string zdmc = (listcol[i].Value).GetSafeString();
                        cell = row.CreateCell(i);
                        cell.SetCellValue(dt[j][zdmc].GetSafeString());
                        cell.CellStyle = cellstyle;
                    }

                }
                sheet.CreateFreezePane(1, 1, 1, 1);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                using (MemoryStream memoryStram = new MemoryStream())
                {
                    //把工作簿写入到内存流中
                    wk.Write(memoryStram);
                    //设置输出编码格式
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    //设置输出流
                    Response.ContentType = "application/octet-stream";
                    //防止中文乱码
                    string fileName = HttpUtility.UrlEncode("所有成员");
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }

            }
        }

        public ActionResult ImportCY()
        {
            string mlid = Request["mlid"].GetSafeString();
            ViewBag.mlid = mlid;
            return View();
        }

        public void DoImportCY()
        {
            bool ret = true;
            string msg = "";
            string mlid = Request["mlid"].GetSafeString();
            try
            {
                if (mlid == "")
                {
                    ret = false;
                    msg = "目录不能为空！";
                }
                else if(Request.Files.Count==0)
                {
                    ret = false;
                    msg = "文件不能为空！";
                }
                else
                {

                    HttpPostedFileBase postfile = Request.Files[0];

                    // 允许的扩展名
                    List<string> extensions = new List<string>() { ".xls", ".xlsx", ".txt", ".csv" };
                    string filename = postfile.FileName;
                    string ext = System.IO.Path.GetExtension(postfile.FileName).GetSafeString();
                    if (ext=="" || (!extensions.Contains(ext)))
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
                                dt = GetDataFromExcel(postfile.InputStream);
                                break;
                            case ".xlsx":
                                dt = GetDataFromExcel(postfile.InputStream,true);
                                break;
                            case ".txt":
                            case ".csv":
                                dt = GetDataFromTxt(postfile.InputStream);
                                break;
                            default:
                                break;
                        }
                        if (dt.Count > 0)
                        {
                            foreach (var row in dt)
                            {
                                string procstr = string.Format("DoSaveSmsCY('{0}','{1}','{2}','{3}','{4}')", mlid, "", row["name"], row["zw"], row["sjhm"]);
                                ret=CommonService.ExecProc(procstr, out msg);
                            }

                        }

                    }

                    
                    
                }
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }

        private List<Dictionary<string,string>> GetDataFromTxt(Stream input)
        {
            List<Dictionary<string, string>> dt = new List<Dictionary<string, string>>();
            StreamReader sr = null;
            using (sr = new StreamReader(input))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] info = line.Split(new char[] { ',', '，' });
                    if (info.Length == 3)
                    {
                        Dictionary<string, string> d = new Dictionary<string, string>();
                        d.Add("name", info[0].GetSafeString().Trim());
                        d.Add("zw", info[1].GetSafeString().Trim());
                        d.Add("sjhm", info[2].GetSafeString().Trim());
                        dt.Add(d);
                    }
                    line = sr.ReadLine();

                }

            }
            return dt;
        }

        private List<Dictionary<string, string>> GetDataFromExcel(Stream input, bool isExcel2007=false)
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
            
            for (int sheetNum = 0; sheetNum < wb.NumberOfSheets; sheetNum++)
            {
                ISheet sheet = wb.GetSheetAt(sheetNum);
                if (sheet != null)
                {
                    for (int j = 0; j <= sheet.LastRowNum; j++)
                    {
                        IRow row = sheet.GetRow(j);
                        if (row != null)
                        {
                            string name = row.GetCell(0).GetSafeString().Trim();
                            string zw = row.GetCell(1).GetSafeString().Trim();
                            string sjhm = row.GetCell(2).GetSafeString().Trim();
                            if (name !="" && sjhm != "")
                            {
                                Dictionary<string, string> d = new Dictionary<string, string>();
                                d.Add("name", name);
                                d.Add("zw", zw);
                                d.Add("sjhm", sjhm);
                                dt.Add(d);
                            }
                        }
                    }
                }
            }
            
            return dt;
        }



        #endregion


        #region 短信发送
        public ActionResult SendSmsTXL()
        {
            return View();
        }



        public ActionResult SendSmsDDD()
        {
            return View();
        }

        public void DoSendSmsTXL()
        {
            // 发送类型
            string lx = Request["lx"].GetSafeString();  // 0: 通讯录发送，1:点对点发送
            // 接收人，含义如下
            // 通讯录发送：多个用逗号分隔的目录ID或者成员ID
            // 点对点发送： 多个用逗号分隔的手机号码
            string receivers = Request["receivers"].GetSafeString(); 
            // 发送内容
            string contents = Request["contents"].GetSafeString();
            // 发送方式， 0：立即发送，1：定时发送
            string fsfs = Request["fsfs"].GetSafeString();
            // 定时发送时间（对于立即发送无用）
            string fssj = Request["fssj"].GetSafeString();

            bool ret = true;
            string msg = "";
            try
            {
                if (lx == "")
                {
                    ret = false;
                    msg = "操作错误！";
                }
                else if(receivers=="")
                {
                    ret = false;
                    msg = "接收人不能为空！";
                }
                else if (contents == "")
                {
                    ret = false;
                    msg = "内容不能为空！";
                }
                else if (fsfs == "")
                {
                    ret = false;
                    msg = "发送方式错误！";
                }
                else
                {
                    string[] rlist = receivers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rlist.Length == 0)
                    {
                        ret = false;
                        msg = "接收人不能为空！";
                    }
                    else
                    {
                        #region 获取手机号码列表

                        List<string> sjhmList = new List<string>();
                        // 通讯录发送，调用存储过程，获取成员的手机号码
                        // 根据目录ID, 获取该目录下所有成员的手机号码, 以及子目录下所有成员的手机号码
                        // 根据成员ID, 获取成员的手机号码
                        if (lx == "0")
                        {
                            string procstr = string.Format("GetPhoneList('{0}')", string.Join(",", rlist));
                            IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                            if (dt != null && dt.Count > 0)
                            {

                                foreach (var row in dt)
                                {
                                    string sjhm = row["sjhm"].GetSafeString();
                                    if (sjhm.Length == 11 && sjhm.StartsWith("1"))
                                    {
                                        sjhmList.Add(sjhm);
                                    }
                                }
                            }

                        }
                        // 点对点发送
                        // 传进来的参数就是手机号码列表
                        else if (lx == "1")
                        {
                            foreach (var item in rlist)
                            {
                                sjhmList.Add(item);
                            }

                        }
                        #endregion

                        #region 发送短信
                        if (sjhmList.Count > 0)
                        {
                            sjhmList = sjhmList.Distinct().ToList(); // 去重
                            // 立即发送
                            if (fsfs == "0")
                            {
                                foreach (var sjhm in sjhmList)
                                {
                                    SmsServiceWzzjz.SendMessageV2(Func.GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), sjhm, contents, out msg);
                                }
                                
                            }
                            // 定时发送
                            else if (fsfs == "1")
                            {
                                List<string> sqls = new List<string>();
                                string encodedcontents = DataFormat.EncodeBase64(contents);
                                foreach (var sjhm in sjhmList)
                                {
                                    
                                    string sql = string.Format("insert into OA_SMS_DXDSFS (sjhm, contents, fssj) VALUES('{0}', '{1}', '{2}')", sjhm, encodedcontents, fssj);
                                    sqls.Add(sql);
                                }
                                ret = CommonService.ExecTrans(sqls);
                                msg = ret ? "" : "定时发送操作失败！";

                            }
                        }
                        #endregion


                    }
                }


            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }

        // 根据目录ID获取成员列表
        public void GetCYList()
        {
            bool ret = true;
            string msg = "";
            string mlid = Request["mlid"].GetSafeString();

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            int totalcount = 0;

            try
            {
                string sql = "select * from oa_sms_txlcy";
                dt = CommonService.GetDataTable(sql);
                if (mlid != "")
                {
                        dt = dt.Where(x => x["mlid"]==mlid).ToList();
                }
                totalcount = dt.Count;

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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\",\"total\":\"{2}\",\"rows\":{3}}}", ret ? "0" : "1", msg, totalcount, jss.Serialize(dt)));
                Response.End();
            }

        }
        #endregion

        #region 常用短信

        // 获取常用短信列表
        public void GetCYDXList()
        {
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            int totalcount = 0;

            try
            {
                string sql = "select * from oa_sms_cydx";
                dt = CommonService.GetDataTable(sql);
                if (type != "" && key != "")
                {
                    
                    if (type == "contents")
                    {
                        dt = dt.Where(x => x["contents"].Contains(key)).ToList();
                    }
                }
                totalcount = dt.Count;
                dt = dt.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
                Response.End();
            }

        }
        #endregion






        private List<VCheckItem> GetCheckItems(IList<IDictionary<string, string>> dt, string PID)
        {
            List<VCheckItem> list = new List<VCheckItem>();
            if (dt.Count > 0)
            {
                IList<IDictionary<string, string>> dtt = dt.Where(x => x["pid"].GetSafeString() == PID).OrderBy(x => x["xssx"].GetSafeDouble()).ToList();
                if (dtt.Count > 0)
                {
                    foreach (var row in dtt)
                    {
                        VCheckItem ci = new VCheckItem() { id = row["id"].GetSafeString(), pId = row["pid"].GetSafeString(), name = row["name"].GetSafeString(), isParent = row["isparent"].GetSafeBool(), cevent = row["cevent"], open = row["isopen"].GetSafeBool() };
                        list.Add(ci);
                        List<VCheckItem> children = GetCheckItems(dt, row["id"].GetSafeString());
                        if (children.Count > 0)
                        {
                            list.AddRange(children);
                        }
                    }
                }
            }
            return list;

        }
        #endregion

        /// <summary>
        /// 手动更改报告pdf文件之后，重新生成签名文件
        /// </summary>
        [Authorize]
        public void ReGenerateReport()
        {
            bool code = true;
            string msg = "";
            try
            {
                string src = Request["src"].GetSafeString();
                string type = Request["type"].GetSafeString();
                string bgbh = Request["bgbh"].GetSafeString();
                if (src !="" && type !="" && bgbh!="")
                {
                    string dir = Server.MapPath("~\\report\\pdftemp");
                    if (Directory.Exists(dir))
                    {
                        string sourcefile = dir + "\\" + src + ".pdf";
                        if (System.IO.File.Exists(sourcefile))
                        {
                            string destf = "";
                            if (type == "ZGD")
                            {
                                destf = "P-ZGD-" + bgbh + ".pdf";
                            }
                            else if (type == "JDBG")
                            {
                                destf = "P-JDBG-" + bgbh + ".pdf";
                            }

                            if (destf != "")
                            {
                                bool ismultipages = CheckPdfMultiPages(sourcefile);
                                string sql = string.Format("select * from h_reportesign where inuse=1 and reporttype='{0}'", type);
                                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                                if (dt.Count > 0)
                                {
                                    string newfile = "";
                                    string destfile = "";
                                    foreach (var row in dt)
                                    {
                                        string appkey = row["appkey"].GetSafeString();
                                        SignType st = (SignType)row["signtype"].GetSafeInt();

                                        // 如果是骑缝章，并且不是多页（也就是单页的pdf上盖骑缝章），不用盖章
                                        if (st == SignType.Edges && !ismultipages)
                                        {
                                            continue;
                                        }
                                        int pt = row["postype"].GetSafeInt();
                                        string pp = row["pospage"].GetSafeString();
                                        float posx = float.Parse(row["posx"]);
                                        float posy = float.Parse(row["posy"]);
                                        int sealid = row["sealid"].GetSafeInt();
                                        string key = row["keyword"].GetSafeString();
                                        PosBean pb = new PosBean();
                                        pb.PosType = pt;//1.坐标盖章  2.关键字盖章
                                        pb.PosPage = pp;//签章页数设置，单页格式“1”，若为多页签章，连续页码格式“1-3”，非连续多页为“1,3”若为坐标定位时，不可空
                                        pb.PosX = posx;//若为坐标盖章，需要设置X坐标
                                        pb.PosY = posy;//若为坐标盖章，需要设置Y坐标
                                        pb.Key = key;

                                        // 签名目标文件
                                        newfile = "T-" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                        destfile = dir + "\\" + newfile + ".pdf";
                                        if (ESign.SignPdf(appkey, sourcefile, destfile, pb, sealid, st, out msg))
                                        {
                                            code = true;
                                            sourcefile = destfile;
                                        }
                                        else
                                        {
                                            code = false;
                                            break;
                                        }
                                    }
                                    if (code)
                                    {
                                        destf = dir + "\\" + destf;
                                        if (System.IO.File.Exists(destf))
                                        {
                                            System.IO.File.Delete(destf);
                                        }
                                        FileInfo fi = new FileInfo(sourcefile);
                                        fi.MoveTo(destf);
                                    }
                                }

                            }
                        }
                        
                    }
                }
            }
            catch (Exception e )
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
		

        /// <summary>
        /// 获取电子签章
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        /// <param name="lx"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool GetSignedPdf(ReportPrint.GenerateGuid g, ReportPrint.CLGenerateGuid c, Dictionary<string,object> extraparams, out string msg )
        {
            bool ret = true;
            byte[] pdfbytes = null;
            msg = "";
            try
            {
                string jsonparam = new JavaScriptSerializer().Serialize(extraparams);
                bool iscache = false;
                string cachename = "";
                string procs = string.Format("GetReportCacheConfig('{0}')", jsonparam);
                IList<IDictionary<string, string>> dc = CommonService.ExecDataTableProc(procs, out msg);
                if (dc.Count > 0)
                {
                    iscache = dc[0]["iscache"].GetSafeString() == "1";
                    cachename = dc[0]["cachename"].GetSafeString();
                }
                // 缓存的目录
                string dir = Server.MapPath("~\\report\\pdftemp");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

               
                // 如果报告需要缓存,并且报告已经存在， 直接返回该报告
                if (iscache && System.IO.File.Exists(dir + "\\" + cachename + ".pdf"))
                {
                    ret = true;
                    msg = cachename;
                }
                else
                {
                    //原始pdf
                    string oname = "T-" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string fname = oname + ".pdf";

                    string sourcefile = "";
                    string destfile = "";
                    string newfile = "";
                    
                    if (g.GetFile(c, out pdfbytes, out msg))
                    {
                        
                        // 获取原始pdf
                        sourcefile = dir + "\\" + fname;
                        FileStream fs = new FileStream(sourcefile, FileMode.Create, FileAccess.Write);
                        fs.Write(pdfbytes, 0, pdfbytes.Length);
                        fs.Close();
                        bool ismultipages = CheckPdfMultiPages(sourcefile);

                        // 获取电子签章配置数据
                        string procstr = string.Format("GetEsignData('{0}')", jsonparam);
                        IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                        // 存在电子签章，生成经过签章的文件
                        if (dt.Count > 0)
                        {
                            foreach (var row in dt)
                            {
                                string appkey = row["appkey"].GetSafeString();
                                SignType st = (SignType)row["signtype"].GetSafeInt();

                                // 如果是骑缝章，并且不是多页（也就是单页的pdf上盖骑缝章），不用盖章
                                if (st == SignType.Edges && !ismultipages)
                                {
                                    continue;
                                }
                                int pt = row["postype"].GetSafeInt();
                                string pp = row["pospage"].GetSafeString();
                                float posx = float.Parse(row["posx"]);
                                float posy = float.Parse(row["posy"]);
                                int sealid = row["sealid"].GetSafeInt();
                                string key = row["keyword"].GetSafeString();
                                PosBean pb = new PosBean();
                                pb.PosType = pt;//1.坐标盖章  2.关键字盖章
                                pb.PosPage = pp;//签章页数设置，单页格式“1”，若为多页签章，连续页码格式“1-3”，非连续多页为“1,3”若为坐标定位时，不可空
                                pb.PosX = posx;//若为坐标盖章，需要设置X坐标
                                pb.PosY = posy;//若为坐标盖章，需要设置Y坐标
                                pb.Key = key;

                                // 签名目标文件
                                newfile = "T-" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                destfile = dir + "\\" + newfile + ".pdf";
                                if (ESign.SignPdf(appkey,sourcefile, destfile, pb, sealid, st, out msg))
                                {
                                    ret = true;
                                    sourcefile = destfile;
                                    oname = newfile;
                                }
                                else
                                {
                                    // 签名失败，这里本来应该返回错误信息的，为了回退，直接返回原始未签名的文件
                                    //ret = false;
                                    //break;
                                    ret = true;
                                    iscache = false;
                                    break;
                                }
                            }
                        }
                        else // 不存在电子签章
                        {
                            ret = true;
                        }
                        // 文件生成成功
                        if (ret)
                        {
                            // 需要缓存文件
                            if (iscache)
                            {
                                FileInfo fi = new FileInfo(sourcefile);
                                fi.MoveTo(dir + "\\" + cachename + ".pdf");
                                msg = cachename;
                            }
                            else
                            {
                                msg = oname;
                            }

                        }

                    }
                    else
                    {
                        ret = false;
                    }

                }

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 校验pdf文件是否为多页
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private bool CheckPdfMultiPages(string filepath)
        {
            bool ret = false;
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    PdfReader reader = new PdfReader(filepath);
                    int iPageNum = reader.NumberOfPages;
                    reader.Close();
                    ret = iPageNum > 1;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;

        }
        
        /// <summary>
        /// 查看报告之前，校验一下是否需要用户输入手机验证码
        /// </summary>
        /// <param name="rd"></param>
        /// <returns></returns>
        private bool CheckNeedToValidateUser(Dictionary<string, object> rd)
        {
            bool ret = true; 
            string msg = "";
            try
            {
                string usercode = CurrentUser.UserName.GetSafeString();
                if (usercode !="")
                {
                    Dictionary<string, object> infos = new Dictionary<string, object>();
                    foreach (var item in rd)
                    {
                        infos.Add(item.Key, item.Value);
                    }
                    infos.Add("usercode", usercode);

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = Int32.MaxValue;
                    string jsonparams = jss.Serialize(infos);
                    string procstr = string.Format("CheckNeedToValidateUser('{0}')", jsonparams);
                    IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        string result = dt[0]["result"];
                        if (result == "1")
                        {
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                    else
                    {
                        ret = true;
                    }
                }
                else
                {
                    ret = true; // 用户未登录
                }
            }
            catch (Exception e)
            {
                ret = true;
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        //修改监督记录还是有问题，就是图文混排的，好像一直没存到stformitem，流程发起的那个地方也要说个完成刷新列表，否则流水号不更新
        public ActionResult FlowReportDown()
        {
            //string url = "";
            //string reportFile = Request["reportfile"].GetSafeString();
            //string serial = Request["serial"].GetSafeString();

            //StForm form = WorkFlowService.GetForm(serial);
            //int formid = 0;

            //if (form != null)
            //    formid = form.Formid;

            //var g = new ReportPrint.GenerateGuid();
            //var c = g.Get();
            //c.type = ReportPrint.EnumType.Word;
            ////c.field = reportFile;
            //c.fileindex = "0";
            //c.table = "stformitem";
            //c.filename = reportFile;
            ////c.field = "formid";
            //c.where = "formid=" + formid;
            //c.openType = ReportPrint.OpenType.FileDown;
            //c.signindex = 0;
            //c.AllowVisitNum = 1;
            //var guid = g.Add(c);

            //url = "/reportPrint/Index?" + guid;
            ////url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            //return new RedirectResult(url);

            string url = "";
            string reportFile = Request["reportfile"].GetSafeString();
            string serial = Request["serial"].GetSafeString();
            int jdjlid = Request["jdjlid"].GetSafeInt();
            int isprint = Request["print"].GetSafeInt(1);
            StForm form = WorkFlowService.GetForm(serial);
            int formid = 0;

            if (form != null)
                formid = form.Formid;


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            c.openType = ReportPrint.OpenType.FileDown;
            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "stformitem|view_i_m_gc|view_gc_ry|view_gc_qy|view_gc_xctp|jdbg_jdjl_xq";
            c.filename = reportFile;
            //c.field = "formid";
            c.where = "formid=" + formid + "|gcbh='" + form.ExtraInfo3 + "'|gcbh='" + form.ExtraInfo3 + "'|gcbh='" + form.ExtraInfo3 + "'|gcbh='" + form.ExtraInfo3 + "'|parentid=" + jdjlid;
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            if (isprint == 1)
                c.customtools = "1,|2,";
            else
                c.customtools = "2,";
            var guid = g.Add(c);


            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }
        /// <summary>
        ///  个人现场图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Grxctp()
        {
            return View();
        }
        /// <summary>
        /// 个人现场图片修改
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Grxctpxg()
        {
            ViewBag.id = Request["id"].GetSafeInt();
            return View();
        }








        /// <summary>
        /// 修改设备备案信息
        /// </summary>
        public void editsb()
        {
            bool code = true;
            string msg = "";
            StringBuilder ret = new StringBuilder();
            try
            {
                string recid = Request["recid"].GetSafeString();
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select CreaterName from View_SB_BA where recid='" + recid + "'");
                if (dt.Count > 0)
                {
                    string CreaterName = dt[0]["creatername"].GetSafeString();
                    if (CreaterName != CurrentUser.UserName)
                    {
                        code = false;
                        msg = "该账号不能修改该信息,请登录录入该设备的账号进行修改";
                    }
                }
                else
                {
                    code = false;
                    msg = "找不到该设备信息";
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

        private void savejdjlxq(string fileid, string[] contentarr)
        {
            List<string> allcontents = new List<string>();
            foreach (var item in contentarr)
            {
                allcontents.AddRange(item.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            if (allcontents.Count > 0)
            {
                List<string> lsql = new List<string>();
                foreach (var c in allcontents)
                {
                    string sql = "";
                    string s = c;
                    Regex regImage = new Regex(@"W:Recid=\w+#");
                    if (s.Replace(" ","") == "")
                    {
                        continue;
                    }
                    else if (regImage.IsMatch(s))
                    {
                        MatchCollection matchCol = regImage.Matches(s);
                        if (matchCol.Count > 0)
                        {
                            foreach (Match matchItem in matchCol)
                            {
                                string strRecid = matchItem.Value.Substring(8, matchItem.Value.Length - 9);
                                sql = string.Format("insert into JDBG_JDJLNR_XQ (fileid, content, type, lrsj) values ('{0}','{1}','{2}', getdate())", fileid, DataFormat.EncodeBase64(strRecid), "img");
                                lsql.Add(sql);
                            }
                        }
                        
                    }
                    else
                    {
                        string[] carr = s.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in carr)
                        {
                            string ss = item.Trim();
                            if (ss!="")
                            {
                                sql = string.Format("insert into JDBG_JDJLNR_XQ (fileid, content, type, lrsj) values ('{0}','{1}','{2}', getdate())", fileid, DataFormat.EncodeBase64(ss), "txt");
                                lsql.Add(sql);
                            }
                        }

                    }
                }

                if (lsql.Count > 0)
                {
                    string csql = string.Format("delete from JDBG_JDJLNR_XQ where fileid='{0}'", fileid);
                    lsql.Insert(0, csql);

                    if (lsql.Count > 0)
                    {
                        CommonService.ExecTrans(lsql);
                    }
                }
                
            }
        }

        

        

        

        


        

        

        


    }
}