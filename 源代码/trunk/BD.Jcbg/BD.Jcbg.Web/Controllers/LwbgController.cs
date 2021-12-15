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
using BD.Jcbg.Web.Remote;

namespace BD.Jcbg.Web.Controllers
{
    public class LwbgController:Controller
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
        /// 监督站内部工程查看，包含所有信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gccknb()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
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

            StForm form = WorkFlowService.GetForm(serial);
            int formid = 0;

            if (form != null)
                formid = form.Formid;

            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "stformitem|view_i_m_gc|view_gc_ry|view_gc_qy|view_gc_xctp";
            c.filename=reportFile;
            //c.field = "formid";
            c.where = "formid=" + formid + "|gcbh='" + form.ExtraInfo3 + "'|gcbh='" + form.ExtraInfo3 + "'|gcbh='" + form.ExtraInfo3 + "'|gcbh='" + form.ExtraInfo3 + "'";
            c.signindex = 0;
            c.openType = ReportPrint.OpenType.Print ;
            c.AllowVisitNum = 1;
            var guid = g.Add(c);

            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }

        public ActionResult FlowReportDown()
        {
            string url = "";
            string reportFile = Request["reportfile"].GetSafeString();
            string serial = Request["serial"].GetSafeString();

            StForm form = WorkFlowService.GetForm(serial);
            int formid = 0;

            if (form != null)
                formid = form.Formid;

            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "stformitem";
            c.filename = reportFile;
            //c.field = "formid";
            c.where = "formid=" + formid;
            c.openType = ReportPrint.OpenType.FileDown;
            c.signindex = 0;
            c.AllowVisitNum = 1;
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

        public ActionResult Player()
        {
            ViewBag.filepath = Request["filepath"].GetSafeString();
            ViewBag.height = Request["height"].GetSafeInt();
            ViewBag.width = Request["width"].GetSafeInt();
            return View();
        }

        

        #endregion

        #region 获取数据

        
        /// <summary>
        /// 获取工程信息
        /// </summary>
        [Authorize]
        public void GetGcInfo()
        {
            string msg = "";
            bool code = true;
            StringBuilder ret = new StringBuilder();
            try
            {
                msg = LwService.getAllGC();
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(msg);
               // Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        

        
        #endregion

        #region 更新数据
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
                        sqls.Add("update i_m_gc set sptg=0,sfyx=0,gczt='YT' where gcbh='" + gcbh + "'");
                        CommonService.ExecTrans(sqls);
                        msg = "0";
                    }
                    // 同意
                    else
                    {
                        sqls.Add("update i_m_gc set sptg=1,sfyx=1 where gcbh='" + gcbh + "'");
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
                
                if (serial != "")
                {
                    // 获取已经保存过的监督记录
                    string sql = "select workserial, reportfile from jdbg_jdjl where workserial='" + serial + "' ";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        serial = dt[0]["workserial"];
                        existedReportFile = dt[0]["reportfile"];

                        if (serial !="")
                        {
                            sql = string.Format("select f.formid, i.itemvalue from stform f, stformitem i where f.serialno='{0}' and f.formid=i.formid and i.itemname='{1}' ", serial, itemname);
                            IList<IDictionary<string, string>> dtt = CommonService.GetDataTable(sql);
                            if (dtt.Count > 0)
                            {
                                formid = dtt[0]["formid"].GetSafeInt();
                                itemvalue = dtt[0]["itemvalue"].GetSafeString();
                            }
                            // 如果已经存在监督记录，将当前提交的记录拼接在以前的记录后面
                            if (formid > 0 && itemvalue !="")
                            {
                                string existedjdjl = WorkFlow.Common.DataFormat.DecodeBase64(Encoding.GetEncoding("GB2312"), itemvalue);
                                content = existedjdjl + "<br>" + content;
                            }
                        }
                    }
                }
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
                                contentarr[i] = strRow.Replace(matchItem.Value, strImagePat);
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
                                contentarr[i] = strRow.Replace(matchItem.Value, strImagePat);
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
        /// 获取可以抽取的组长列表
        /// </summary>
        public void GetExtractableZZRYList()
        {
            string where = " 1=1 ";
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
            string where = " 1=1 ";
            int count = Request["sl"].GetSafeInt();
            string rolecode = "CR201611000019";  // 安装监督员的角色代码

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
            string where = " 1=1 ";
            int count = Request["sl"].GetSafeInt();
            string rolecode = "CR201611000020";  // 土建监督员的角色代码

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
            string strExcludeDeps = "DP201611000012,DP201611000013";
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

        /// <summary>
        /// 搜索工程
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchGC()
        {
            return View();
        }

        /// <summary>
        /// 搜索人员
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchRY()
        {
            return View();
        }

        /// <summary>
        /// 外部登录
        /// </summary>
        /// <returns></returns>
        public ActionResult ExternalLogin()
        {
            return View();
        }

        #endregion
    }
}