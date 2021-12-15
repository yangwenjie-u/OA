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
using BD.Jcbg.DataModal.VirutalEntity;
using ZXing;
using CryptFun = BD.Jcbg.Common.CryptFun;

namespace BD.Jcbg.Web.Controllers
{
    public class MachineController : Controller
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
        private IJcService _jcService = null;
        private IJcService JcService
        {
            get
            {
                if (_jcService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jcService = webApplicationContext.GetObject("JcService") as IJcService;
                }
                return _jcService;
            }
        }
        #endregion
        #region 页面
        public ActionResult Recode()
        {
            int recid = Request["id"].GetSafeInt();
            string Qrcode = "";
            ViewBag.id = recid;
            string btntext = "";
            //ViewData.Add("id", Request["id"].GetSafeInt());
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            datas = CommonService.GetDataTable("select qrcode,status from SB_BZJ where recid=" + recid);
            if (datas.Count > 0)
            {
                Qrcode = datas[0]["qrcode"];
                btntext = datas[0]["status"];

            }

            ViewBag.qrcode = Qrcode;
            ViewBag.btntext = btntext;

            return View();
        }
        /// <summary>
        /// 填单时选择委托单位
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Jcjgxz()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeRequest();
            return View();
        }


        /// <summary>
        /// 确认界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult confirm()
        {
            ViewBag.type = Request["type"].GetSafeRequest();
            ViewBag.key = Request["key"].GetSafeRequest();
            return View();
        }


        /// <summary>
        /// 填单时选择委托单位
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Qrcode()
        {
            //ViewBag.gcbh = Request["gcbh"].GetSafeRequest();
            return View();
        }

        /// <summary>
        /// 起重机械首页（三屏）
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            
            return View();
        }

        /// <summary>
        /// 填单时选择委托单位
        /// </summary>
        /// <returns></returns>

        public ActionResult Show()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
        }


        public ActionResult GetSbSHow()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
        }



        public ActionResult viewSb()
        {

            string baid = Request["baid"].ToString();
            string recid = "";
            string sql = "select recid from SB_BA where BaID='" + baid + "'";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            datas = CommonService.GetDataTable(sql);
            if (datas.Count > 0)
            {
                recid = datas[0]["recid"];
            }

            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_BA";
            param.t1_pri = "RECID";
            param.t1_title = "设备备案";
            param.jydbh = recid;
            param.view = true;

            param.t2_tablename = "SB_BA_List";
            ////主键
            param.t2_pri = "ParentBaID,ID";
            ////标题
            param.t2_title = "构件";


            param.rownum = 3;
            //param.button = "保存|TJ|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-calendar-times-o|保存成功！||返回|FH|http://www.baidu.com";

            return RedirectToAction("Index", "DataInput", param);
        }


        public ActionResult viewBZJ()
        {

            string bzjid = Request["bzjid"].ToString();
           

            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_BZJ";
            param.t1_pri = "RECID";
            param.t1_title = "标准节备案";
            param.jydbh = bzjid;
            param.view = true;

            //param.button = "保存|TJ|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-calendar-times-o|保存成功！||返回|FH|http://www.baidu.com";

            return RedirectToAction("Index", "DataInput", param);
        }

        


        public ActionResult TD()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
        }

        public ActionResult QZJ()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
        }

        public ActionResult SJJ()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
        }

        public ActionResult SJJ2()
        {
            ViewBag.recid = Request["recid"].GetSafeRequest();
            return View();
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
            c.where = "BaID='" + recid+"'";
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            c.customtools = "2,";
            var guid = g.Add(c);


            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }

        public ActionResult SBInstallReportDown()
        {

            string url = "";
            string reportFile = "安装告知表";
            string recid = Request["recid"].GetSafeString();
            string type = Request["type"].GetSafeString("down"); ;


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (type == "down")
                c.openType = ReportPrint.OpenType.FileDown;
            else if (type == "pic")
                c.openType = ReportPrint.OpenType.PIC;
            else
                c.openType = ReportPrint.OpenType.Print;

            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "View_SB_Install|SB_SpecialPerson";
            c.filename = reportFile;
            //c.field = "formid";
            //c.where = "RECID=" + recid + "|FKID=(select InstallID from View_SB_Install where RECID=" + recid + ")";
           
            c.where = "InstallID='" + recid + "'|FKID='" + recid + "'";
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            c.customtools = "2,";
            var guid = g.Add(c);


            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }

        public ActionResult SBInstallAddReportDown()
        {

            string url = "";
            string reportFile = "顶节加升";
            string recid = Request["recid"].GetSafeString();
            string type = Request["type"].GetSafeString("down"); ;

            /*文件下载，这里做一个判断，如果说没文件，需要先生成文件
            string filepath = Server.MapPath("~\\machinepdf") + "\\" + recid + ".pdf";
            if (System.IO.File.Exists(filepath))
            {
                var myBytes = System.IO.File.ReadAllBytes(filepath);
                string mime = "application/pdf";
                return File(myBytes, mime, recid + ".pdf");
            }*/

            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (type == "down")
                c.openType = ReportPrint.OpenType.FileDown;
            else if (type == "pic")
                c.openType = ReportPrint.OpenType.PIC;
            else
                c.openType = ReportPrint.OpenType.Print;

            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "SB_InstallAdd|SB_SpecialPerson";
            c.filename = reportFile;
            //c.field = "formid";
            //c.where = "RECID=" + recid + "|FKID=(select InstallID from View_SB_Install where RECID=" + recid + ")";

            c.where = "InstallADDID='" + recid + "'|FKID='" + recid + "'";
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            c.customtools = "2,";
            var guid = g.Add(c);

            


            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }




        public ActionResult SBUseRegReportDown()
        {

            string url = "";
            string reportFile = "使用登记表";
            string recid = Request["recid"].GetSafeString();
            string type = Request["type"].GetSafeString("down"); ;


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (type == "down")
                c.openType = ReportPrint.OpenType.FileDown;
            else if (type == "pic")
                c.openType = ReportPrint.OpenType.PIC;
            else
                c.openType = ReportPrint.OpenType.Print;

            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "View_SB_UseReg|View_SB_Install|SB_SpecialPerson";
            c.filename = reportFile;
            //c.field = "formid";
            //c.where = "RECID=" + recid + "|FKID=(select UseID from View_SB_UseReg where RECID=" + recid + ")";
            c.where = "UseID='" + recid + "'|InstallID=(select InstallID from View_SB_UseReg where UseID='" + recid + "')|FKID='" + recid + "'";
            c.signindex = 0;
            //c.openType = ReportPrint.OpenType.Print ;

            c.AllowVisitNum = 1;
            c.customtools = "2,";
            var guid = g.Add(c);


            url = "/reportPrint/Index?" + guid;
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }


        public ActionResult SBUnInstallReportDown()
        {

            string url = "";
            string reportFile = "拆卸告知表";
            string recid = Request["recid"].GetSafeString();
            string type = Request["type"].GetSafeString("down"); ;


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();
            c.type = ReportPrint.EnumType.Word;
            if (type == "down")
                c.openType = ReportPrint.OpenType.FileDown;
            else if (type == "pic")
                c.openType = ReportPrint.OpenType.PIC;
            else
                c.openType = ReportPrint.OpenType.Print;

            //c.field = reportFile;
            c.fileindex = "0";
            c.table = "View_SB_UnInstall|View_SB_Install|SB_SpecialPerson";
            c.filename = reportFile;
            //c.field = "formid";
            //c.where = "RECID=" + recid + "|FKID=(select UnInstallID from View_SB_UnInstall where RECID=" + recid + ")";
            c.where = "UnInstallID='" + recid + "'|InstallID=(select InstallID from View_SB_UnInstall where UnInstallID='" + recid + "')|FKID='" + recid + "'";
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


        #region 数据处理



         /// <summary>
        /// 备案登记
        /// </summary>
        [Authorize]
        public void SubBZJIcp()
        {
            int recid = Request["id"].GetSafeInt();
            string icp = Request["icp"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {

                IList<string> sqls = new List<string>();
                sqls.Add("update SB_BZJ set  Status=1,qrcode='" + icp + "' where recid=" + recid);
                ret = CommonService.ExecTrans(sqls);
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
        public void RemoveBZJIcp()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            bool ret = false;
            try
            {

                IList<string> sqls = new List<string>();
                sqls.Add("update SB_BZJ set  Status=2 where recid=" + recid);
                ret = CommonService.ExecTrans(sqls);
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

        /// <summary>
        /// 标准节添加审核
        /// </summary>
        public void SubBZJADD()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            bool ret = false;
            try
            {

                IList<string> sqls = new List<string>();
                sqls.Add("update SB_BZJ set  Status=1, where recid=" + recid);
                ret = CommonService.ExecTrans(sqls);
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



        /// <summary>
        /// 获取检测机构
        /// </summary>
        [Authorize]
        public void GetJcjg()
        {
            string ret = "[]";
            try
            {
                string filtergc = Request["filtergc"].GetSafeRequest();
                bool jdywt = Request["jdywt"].GetSafeInt() == 1;

                IList<IDictionary<string, string>> gcinfos = new List<IDictionary<string, string>>();

                gcinfos = CommonService.GetDataTable("select qybh,qymc from i_m_qy where lxbh='26' or exists (select * from I_S_QY_QYZZ where qylxbh='26' and I_S_QY_QYZZ.qybh=i_m_qy.qybh)");
                /*
                if (filtergc != "")
                {
                    IList<IDictionary<string, string>> hts = CommonService.GetDataTable("select jcjgbh,fbjcjgbh from i_m_ht where sfyx=1 and gcbh='" + filtergc + "'");
                    if (hts.Count > 0)
                    {
                        IList<string> allqybhs = new List<string>();
                        foreach (IDictionary<string, string> row in hts)
                        {
                            allqybhs.Add(row["jcjgbh"]);
                            allqybhs.Add(row["fbjcjgbh"]);
                        }
                        for (int i = gcinfos.Count - 1; i >= 0; i--)
                        {
                            string qybh = gcinfos[i]["qybh"];
                            var q = from e in allqybhs where e == qybh select e;
                            if (q.Count() == 0)
                                gcinfos.RemoveAt(i);
                        }
                    }
                }
                */
                ret = new JavaScriptSerializer().Serialize(gcinfos);
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


        public void SetWtdStatusXf()
        {
            string recid = Request["recid"].GetSafeString();
            string dwbh = Request["dwbh"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {

                ret = JcService.SetWtdStatusXf(recid, dwbh, out err);


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



        /// <summary>
        /// 设备统计
        /// </summary>
        public void GetSBTJ()
        {
            string msg = "";
            bool code = true;
            string province = Request["province"].GetSafeString();
            string city = Request["city"].GetSafeString();
            string district = Request["district"].GetSafeString();
            string jd = Request["jd"].GetSafeString();


            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string sql = "";
                string basb = "0";
                sql = "select count(1) as num from dbo.SB_BA where BeiAnStatus='1' ";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    basb = retdt[0]["num"];

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("name", "备案");
                di.Add("value", basb);
                dt.Add(di);

                string sql2 = "";
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

                if (district != "")
                    where += " and SZXQ='" + district + "'";
                if (jd != "")
                    where += " and SZJD='" + jd + "'";
                sql2 = "select gcbh from i_M_GC where 1=1 " + where;


                string zysb = "0";
                sql = "select count(1) as num from SB_ReportSBSY where state!=2  and jdzch in(" + sql2 + ") ";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    zysb = retdt[0]["num"];
                IDictionary<string, string> di2 = new Dictionary<string, string>();
                di2.Add("name", "在用");
                di2.Add("value", zysb);
                dt.Add(di2);

                /*
                string sql = "";
                string where = "";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable("select lxmc,lxbh from H_GCLX");
                for (int i = 0; i < dtlx.Count; i++)
                {
                    string lxbh = dtlx[i]["lxbh"];
                    string retsum = "0";
                    sql = "select count(1) as num from i_M_GC where  GCLXBH='" + lxbh + "' and  zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];

                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", dtlx[i]["lxmc"]);
                    di.Add("value", retsum);
                    dt.Add(di);


                }*/

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }



        public void GetTjlist()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {

                string sql = "";
                IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                


                string sgdwnum = "0";
                sql = "select count(1) as num from dbo.SB_BA where BeiAnStatus='1' ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    sgdwnum = dt_zjgcs[0]["num"];

                string zgcs = "0";
                sql = "select count(1) as num from SB_ReportSBSY where state!=2  ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zgcs = dt_zjgcs[0]["num"];

                string jzmj = "0";
                sql = "select count(1) as num from dbo.SB_Person  ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    jzmj = dt_zjgcs[0]["num"];

                string gczj = "0";
                sql = "select count(1) as num from dbo.SB_SpecialPerson where FKID in (select UseID from dbo.SB_ReportSBSY where state!=2 ) ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    gczj = dt_zjgcs[0]["num"];

                string dqry = "0";
                sql = "select count(1) as num from SB_ReportSBSY where state!=2  and UserProgStatus=4 ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    dqry = dt_zjgcs[0]["num"];


                string zzry = "0";
                sql = "select count(1) as num from SB_ReportSBSY where InstallProgStatus=4 and  UserProgStatus!=4 ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zzry = dt_zjgcs[0]["num"];

                string yfje = "0";
                /*
                sql = "select count(1) as num from SB_ReportSBSY where InstallProgStatus=4 and  UserProgStatu!s=4 ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    yfje = dt_zjgcs[0]["num"];*/
                string sfje = "0";
                /*
                sql = "select count(1) as num from SB_ReportSBSY where InstallProgStatus=4 and  UserProgStatu!s=4 ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    sfje = dt_zjgcs[0]["num"];*/

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("sgdwnum", sgdwnum);
                di.Add("zgcs", zgcs);
                di.Add("jzmj", jzmj);
                di.Add("gczj", gczj);
                di.Add("dqry", dqry);
                di.Add("zzry", zzry);
                di.Add("yfje", yfje);
                di.Add("sfje", sfje);
                //di.Add("ffje", rd.Next(1,100).ToString());

                dt.Add(di);

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }

        }



        /// <summary>
        /// 设备统计
        /// </summary>
        public void GetGC_QYFBTJ()
        {
            string msg = "";
            bool code = true;
            string province = Request["province"].GetSafeString();
            string city = Request["city"].GetSafeString();
            string district = Request["district"].GetSafeString();
            string jd = Request["jd"].GetSafeString();
            string qylx = Request["qylx"].GetSafeString();
            string qybh = Request["qybh"].GetSafeString();
            string gczt = Request["gczt"].GetSafeString();
            string gcxz = Request["gcxz"].GetSafeString();
            string key = Request["key"].GetSafeString();



            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string sql = "";
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

                if (district != "")
                    where += " and SZXQ='" + district + "'";
                if (jd != "")
                    where += " and SZJD='" + jd + "'";
                //企业编号怎么写
                if (qybh != "")
                    where += " and gcbh in(select gcbh from dbo.View_GC_QY where qybh='" + qybh + "')";

                if (gcxz != "")
                    where += " and gclxbh='" + gcxz + "'";

                if (key != "")
                    where += " and gcmc like '%" + key + "%'";


                sql = "select count(1) as value ,qymc as name from  (select qymc  from dbo.View_I_M_GC1 where gcbh in(select jdzch from dbo.SB_ReportSBSY where state!=2) "+ where +") a group by qymc";
                dt = CommonService.GetDataTable(sql);

                /*
                string sql = "";
                string where = "";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable("select lxmc,lxbh from H_GCLX");
                for (int i = 0; i < dtlx.Count; i++)
                {
                    string lxbh = dtlx[i]["lxbh"];
                    string retsum = "0";
                    sql = "select count(1) as num from i_M_GC where  GCLXBH='" + lxbh + "' and  zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];

                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", dtlx[i]["lxmc"]);
                    di.Add("value", retsum);
                    dt.Add(di);


                }*/

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }



        /// <summary>
        /// 地图工程
        /// </summary>
        public void GetProjectMap()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {

                string sql = "";
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";

                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();
                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();
                string gcxz = Request["gcxz"].GetSafeString();
                string key = Request["key"].GetSafeString();

                string gcmc = Request["gcmc"].GetSafeString();
                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }
                if (gcmc != "")
                {
                    where = " and gcmc like '%" + gcmc + "%'";
                }


                if (district != "")
                    where += " and SZXQ='" + district + "'";
                if (jd != "")
                    where += " and SZJD='" + jd + "'";
                //企业编号怎么写
                if (qybh != "")
                    where += " and gcbh in(select gcbh from dbo.View_GC_QY where qybh='" + qybh + "')";

                if (gcxz != "")
                    where += " and gclxbh='" + gcxz + "'";

                if (key != "")
                    where += " and gcmc like '%" + key + "%'";

                sql = "select gcmc,gcbh,gczb,gclxbh,gcdd from View_I_M_GC1 where gczb is not null and gczb !='' and gcbh in(select jdzch from dbo.SB_ReportSBSY where state!=2)  and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    if (ret != "")
                        ret += ",";
                    ret += " { \"name\": \"" + dt[i]["gcmc"] + "\", \"position\": [" + dt[i]["gczb"] + "], \"status\": 0,\"gcbh\": \"" + dt[i]["gcbh"] + "\",\"gclxbh\": \"" + dt[i]["gclxbh"] + "\",\"gcdd\": \"" + dt[i]["gcdd"] + "\" }";
                }
                ret = "[" + ret + "]";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, ret));
                Response.End();
            }
        }



        public void GetGcList()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {

                string sql = "";
                dt = CommonService.GetDataTable("select lxmc,lxbh from H_GCLX");

                

                string where = "";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

                sql = "select gcmc,gcbh,gcdd,gczb, spzt as sy_gczt  from  dbo.View_I_M_GC1 where gcbh in(select jdzch from dbo.SB_ReportSBSY where state!=2)  and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt = CommonService.GetDataTable(sql);
                

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }



        /// <summary>
        /// 人员类型分布
        /// </summary>
        public void GetRYLXFB()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {

                string sql = "";
                string where = "";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }
                sql = "select count(1) as num ,major from ( select major from dbo.SB_SpecialPerson  where FKID in (select UseID from dbo.SB_ReportSBSY where state!=2 ) ) a  group by major";
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable(sql);
                for (int i = 0; i < dtlx.Count; i++)
                {
                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", dtlx[i]["major"]);
                    di.Add("value", dtlx[i]["num"]);
                    dt.Add(di);
                }
                /*
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable("select mc from h_RYGW ");
                for (int i = 0; i < dtlx.Count; i++)
                {
                    string lxbh = dtlx[i]["mc"];
                    string retsum = "0";
                    sql = "select count(1) as num from dbo.View_GC_RY_QYRYCK where gw='" + lxbh + "' and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where;
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];

                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", dtlx[i]["mc"]);
                    di.Add("value", retsum);
                    dt.Add(di);


                }*/

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }




        /// <summary>
        /// 设备详情
        /// </summary>
        public void GetSBDetail()
        {
            string msg = "";
            bool code = true;
            string cqbh="";
            string baid="";
            int id = Request["recid"].GetSafeInt();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string sql = "";
                sql = "select baid,cqbh from SB_ReportSBSY where recid=" + id;
                retdt = CommonService.GetDataTable(sql);
                if (retdt.Count > 0)
                {
                    cqbh = retdt[0]["cqbh"].GetSafeString();
                    baid = retdt[0]["baid"].GetSafeString();
                }
                dt = CommonService.GetDataTable("select * from dbo.SB_BZJADD where status!=2 and sbsyid=" + id + " order by DisOrder desc");
                

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"cqbh\":\"{3}\",\"baid\":\"{4}\",\"Datas\":{5}}}", code ? "0" : "1", msg, dt.Count, cqbh, baid, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 设备详情
        /// </summary>
        [Authorize]
        public void GetSBJD()
        {
            string msg = "";
            bool code = true;
            int id = Request["recid"].GetSafeInt();
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string sql = "";
                sql = "select * from SB_ReportSBSY where recid=" + id;

                dt = CommonService.GetDataTable(sql);


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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"Datas\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }



        [Authorize]
        public void Deletesb()
        {
            int recid = Request["recid"].GetSafeInt();
            string err = "";
            bool ret = false;
            try
            {

                string procstr = string.Format("CancelSBInstall({0})", recid.ToString());
                CommonService.ExecProc(procstr, out err);
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
        public void AddUnInstall()
        {
            string key = Request["key"].GetSafeString();

            string err = "";
            string newid = System.Guid.NewGuid().ToString("N");
            bool ret = false;
            try
            {

                string procstr = string.Format("SB_AddUnInstall('{0}','{1}')", key, newid);
                ret = CommonService.ExecProc(procstr, out err);
                if (ret)
                    err = newid;
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



        public void getSbLLList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();           
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(20);
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {

                    string sql = "select * from View_SBSY where baid='" + key + "' order by gzrq desc";

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


        public void getSbSyDetailList()
        {
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            int totalcount = 0;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                if (code)
                {
                    string baid = "";
                    string jdzch = "";
                    string installid = "";
                    string useid = "";
                    string uninstallid = "";
                    string sql = "select azrq,syrq,cxrq,baid,jdzch,installid,useid,uninstallid from View_SBSY where RECID=" + recid;

                   
                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count > 0)
                    {
                        baid = datas[0]["baid"].GetSafeString();
                        jdzch = datas[0]["jdzch"].GetSafeString();
                        installid = datas[0]["installid"].GetSafeString();
                        useid = datas[0]["useid"].GetSafeString();
                        uninstallid = datas[0]["uninstallid"].GetSafeString();
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name","安装告知");
                        di.Add("time",  datas[0]["azrq"].GetSafeDate().ToString("yyyy-MM-dd"));
                        di.Add("type", "0");
                        di.Add("order", "1");
                        retdt.Add(di);
                        IDictionary<string, string> di1 = new Dictionary<string, string>();
                        di1.Add("name", "使用登记");
                        di1.Add("time", datas[0]["syrq"].GetSafeDate().ToString("yyyy-MM-dd"));
                        di1.Add("type", "0");
                        di1.Add("order", "10");
                        retdt.Add(di1);
                        IDictionary<string, string> di2 = new Dictionary<string, string>();
                        di2.Add("name", "拆卸告知");
                        di2.Add("time", datas[0]["cxrq"].GetSafeDate().ToString("yyyy-MM-dd"));
                        di2.Add("type", "0");
                        di2.Add("order", "20");
                        retdt.Add(di2);
                    }
                    if (installid != "")
                    {
                        sql = "select replydate1,replydate2,aprovedate from sb_install where installid='" + installid + "'";


                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            IDictionary<string, string> di = new Dictionary<string, string>();
                            di.Add("name", "安装填写");
                            di.Add("time", datas[0]["replydate1"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di.Add("type", "1");
                            di.Add("order", "2");
                            retdt.Add(di);
                            IDictionary<string, string> di1 = new Dictionary<string, string>();
                            di1.Add("name", "安装填写监理确认");
                            di1.Add("time", datas[0]["replydate2"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di1.Add("type", "1");
                            di1.Add("order", "3");
                            retdt.Add(di1);
                            IDictionary<string, string> di2 = new Dictionary<string, string>();
                            di2.Add("name", "安装审核");
                            di2.Add("time", datas[0]["aprovedate"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di2.Add("type", "1");
                            di2.Add("order", "4");
                            retdt.Add(di2);
                        }
                    }
                    else
                    {
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name", "安装填写");
                        di.Add("time", "");
                        di.Add("type", "1");
                        di.Add("order", "2");
                        retdt.Add(di);
                        IDictionary<string, string> di1 = new Dictionary<string, string>();
                        di1.Add("name", "安装监理确认");
                        di1.Add("time", "");
                        di1.Add("type", "1");
                        di1.Add("order", "3");
                        retdt.Add(di1);
                        IDictionary<string, string> di2 = new Dictionary<string, string>();
                        di2.Add("name", "安装审核");
                        di2.Add("time", "");
                        di2.Add("type", "1");
                        di2.Add("order", "4");
                        retdt.Add(di2);
                    }


                    if (useid != "")
                    {
                        sql = "select replydate1,replydate2,aprovedate from SB_UseReg where useid='" + useid + "'";


                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            IDictionary<string, string> di = new Dictionary<string, string>();
                            di.Add("name", "使用填写");
                            di.Add("time", datas[0]["replydate1"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di.Add("type", "1");
                            di.Add("order", "12");
                            retdt.Add(di);
                            IDictionary<string, string> di1 = new Dictionary<string, string>();
                            di1.Add("name", "使用监理确认");
                            di1.Add("time", datas[0]["replydate2"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di1.Add("type", "1");
                            di1.Add("order", "13");
                            retdt.Add(di1);
                            IDictionary<string, string> di2 = new Dictionary<string, string>();
                            di2.Add("name", "使用审核");
                            di2.Add("time", datas[0]["aprovedate"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di2.Add("type", "1");
                            di2.Add("order", "14");
                            retdt.Add(di2);
                        }
                    }
                    else
                    {
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name", "使用填写");
                        di.Add("time", "");
                        di.Add("type", "1");
                        di.Add("order", "14");
                        retdt.Add(di);
                        IDictionary<string, string> di1 = new Dictionary<string, string>();
                        di1.Add("name", "使用监理确认");
                        di1.Add("time", "");
                        di1.Add("type", "1");
                        di1.Add("order", "13");
                        retdt.Add(di1);
                        IDictionary<string, string> di2 = new Dictionary<string, string>();
                        di2.Add("name", "使用审核");
                        di2.Add("time", "");
                        di2.Add("type", "1");
                        di2.Add("order", "14");
                        retdt.Add(di2);
                    }


                    if (uninstallid != "")
                    {
                        sql = "select replydate1,replydate2,aprovedate from SB_UnInstall where uninstallid='" + uninstallid + "'";


                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            IDictionary<string, string> di = new Dictionary<string, string>();
                            di.Add("name", "拆卸填写");
                            di.Add("time", datas[0]["replydate1"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di.Add("type", "1");
                            di.Add("order", "22");
                            retdt.Add(di);
                            IDictionary<string, string> di1 = new Dictionary<string, string>();
                            di1.Add("name", "拆卸监理确认");
                            di1.Add("time", datas[0]["replydate2"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di1.Add("type", "1");
                            di1.Add("order", "23");
                            retdt.Add(di1);
                            IDictionary<string, string> di2 = new Dictionary<string, string>();
                            di2.Add("name", "拆卸审核");
                            di2.Add("time", datas[0]["aprovedate"].GetSafeDate().ToString("yyyy-MM-dd"));
                            di2.Add("type", "1");
                            di2.Add("order", "24");
                            retdt.Add(di2);
                        }
                    }
                    else
                    {
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name", "拆卸填写");
                        di.Add("time", "");
                        di.Add("type", "1");
                        di.Add("order", "22");
                        retdt.Add(di);
                        IDictionary<string, string> di1 = new Dictionary<string, string>();
                        di1.Add("name", "拆卸监理确认");
                        di1.Add("time", "");
                        di1.Add("type", "1");
                        di1.Add("order", "23");
                        retdt.Add(di1);
                        IDictionary<string, string> di2 = new Dictionary<string, string>();
                        di2.Add("name", "拆卸审核");
                        di2.Add("time", "");
                        di2.Add("type", "1");
                        di2.Add("order", "24");
                        retdt.Add(di2);
                    }


                    sql = "select wbnr,wbsj from sb_wbjl where parentinstallid='" + installid + "'";


                    datas = CommonService.GetDataTable(sql);
                    for (int i = 0; i < datas.Count; i++)
                    {
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name", "维保:" + datas[0]["wbnr"].GetSafeString());
                        di.Add("time", datas[0]["wbsj"].GetSafeDate().ToString("yyyy-MM-dd"));
                        di.Add("type", "2");
                        di.Add("order", "0");
                        retdt.Add(di);
                    }


                    sql = "select jyrq  from reportqzj where (gcbh='" + jdzch + "' or gcbh=(select gcbh from I_M_GC where SJGCBH='" + jdzch + "')) and sbbaid='" + baid + "'";


                    datas = CommonService.GetDataTable(sql);
                    for (int i = 0; i < datas.Count; i++)
                    {
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name", "设备检测");
                        di.Add("time", datas[0]["jyrq"].GetSafeDate().ToString("yyyy-MM-dd"));
                        di.Add("type", "2");
                        di.Add("order", "0");
                        retdt.Add(di);
                    }


                    /*
                    for (int i = 0; i < dtlx.Count; i++)
                    {
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("name", dtlx[i]["gw"]);
                        di.Add("value", dtlx[i]["num"]);
                        dt.Add(di);
                    }*/
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", retdt.Count.ToString(), jss.Serialize(retdt)));
                Response.End();
            }
        }


        public void SbSH()
        {
            bool ret = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string type = Request["type"].GetSafeString();
            string key = Request["key"].GetSafeString();
            string icp = Request["icp"].GetSafeString();
            string op = Request["op"].GetSafeString("1");
            string err = "";
             IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    ret = Remote.UserService.Login(username, password, out err);

                if (ret)
                {
                    string optext = "";
                    if (op == "2")
                        optext = "退回";
                    else
                        optext = "同意";
                    string sql = "";
                    IList<string> sqls = new List<string>();
                    if (type == "azsg")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_Install set ProgStatus=3, ReplyMan1='" + CurrentUser.UserName + "',ReplyContent1='" + icp + "',ReplyManName1='" + CurrentUser.RealName + "',ReplyDate1=getdate() where InstallID='" + key + "'";
                            
                        }
                        else
                        {
                            sql = "update dbo.SB_Install set ProgStatus=3, ReplyMan1='" + CurrentUser.UserName + "',ReplyContent1='" + icp + "',ReplyManName1='" + CurrentUser.RealName + "',ReplyDate1=getdate() where InstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=2 where InstallID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'安装施工确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "azjl")
                    {
                        sql = "update dbo.SB_Install set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where InstallID='" + key + "'";
                        sqls.Add(sql);
                        if (op == "2")
                        {
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=0 where InstallID='" + key + "'");
                        }
                        else
                        {
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=3 where InstallID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'安装监理确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "azjs")
                    {
                        sql = "update dbo.SB_Install set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where InstallID='" + key + "'";
                        sqls.Add(sql);
                        if (op == "2")
                        {
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=0 where InstallID='" + key + "'");
                        }
                        else
                        {
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=3 where InstallID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'安装业主确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID='" + key + "'";
                        sqls.Add(sql);
                    }

                    if (type == "azjdz" || type == "azjjdz")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_Install set ProgStatus=3, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',  AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where InstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=0 where InstallID='" + key + "'");
                        }
                        else
                        {
                            sql = "update dbo.SB_Install set ProgStatus=4, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',  AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where InstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set InstallProgStatus=4 where InstallID='" + key + "'");
                            sqls.Add("INSERT INTO [H_SB_ToPic]([KeyID],[Type]) VALUES('" + key + "','az')");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'安装监督站确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "syjl")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UseReg set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UseID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UserProgStatus=2 where UseID='" + key + "'");
                        }
                        else 
                        {
                            sql = "update dbo.SB_UseReg set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UseID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UserProgStatus=3 where UseID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'使用监理确认：" + optext + "',getdate() from SB_ReportSBSY where UseID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "syjs")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UseReg set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UseID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UserProgStatus=2 where UseID='" + key + "'");
                        }
                        else
                        {
                            sql = "update dbo.SB_UseReg set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UseID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UserProgStatus=3 where UseID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'使用业主确认：" + optext + "',getdate() from SB_ReportSBSY where UseID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "syjdz")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UseReg set ProgStatus=3, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where UseID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UserProgStatus=2 where UseID='" + key + "'");
                        }
                        else
                        {
                            sql = "update dbo.SB_UseReg set ProgStatus=4, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where UseID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UserProgStatus=4 where UseID='" + key + "'");
                            sqls.Add("INSERT INTO [H_SB_ToPic]([KeyID],[Type]) VALUES('" + key + "','sy')");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'使用监督站确认：" + optext + "',getdate() from SB_ReportSBSY where UseID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "cxsg")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, ReplyMan1='" + CurrentUser.UserName + "',ReplyContent1='" + icp + "',ReplyManName1='" + CurrentUser.RealName + "',ReplyDate1=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=0 where UnInstallID='" + key + "'");
                        }
                        else
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, ReplyMan1='" + CurrentUser.UserName + "',ReplyContent1='" + icp + "',ReplyManName1='" + CurrentUser.RealName + "',ReplyDate1=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=2 where UnInstallID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'拆卸施工确认：" + optext + "',getdate() from SB_ReportSBSY where UnInstallID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "cxjl")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=0 where UnInstallID='" + key + "'");
                        }
                        else
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=3 where UnInstallID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'拆卸监理确认：" + optext + "',getdate() from SB_ReportSBSY where UnInstallID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "cxjs")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=0 where UnInstallID='" + key + "'");
                        }
                        else
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=3 where UnInstallID='" + key + "'");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'拆卸业主确认：" + optext + "',getdate() from SB_ReportSBSY where UnInstallID='" + key + "'";
                        sqls.Add(sql);
                    }
                    if (type == "cxjdz")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_UnInstall set ProgStatus=3, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set UnInstallProgStatus=0 where UnInstallID='" + key + "'");

                        }
                        else
                        {
                           
                            sql = "update dbo.SB_UnInstall set ProgStatus=4, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where UnInstallID='" + key + "'";
                            sqls.Add(sql);
                            sqls.Add("update SB_ReportSBSY set state=2, UnInstallProgStatus=4 where UnInstallID='" + key + "'");
                            sqls.Add("INSERT INTO [H_SB_ToPic]([KeyID],[Type]) VALUES('" + key + "','cx')");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'拆卸监督站确认：" + optext + "',getdate() from SB_ReportSBSY where UnInstallID='" + key + "'";
                        sqls.Add(sql);
                    }



                    if (type == "jssg")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=0, ReplyMan1='" + CurrentUser.UserName + "',ReplyContent1='" + icp + "',ReplyManName1='" + CurrentUser.RealName + "',ReplyDate1=getdate() where InstallADDID='" + key + "'";
                        }
                        else
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=2, ReplyMan1='" + CurrentUser.UserName + "',ReplyContent1='" + icp + "',ReplyManName1='" + CurrentUser.RealName + "',ReplyDate1=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);                           
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'加升施工确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID=(select InstallID from SB_InstallAdd where InstallADDID='" + key + "')";
                        sqls.Add(sql);
                    }
                    if (type == "jsjl")
                    {
                        
                        if (op == "2")
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=0, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);
                        }
                        else
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'加升监理确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID=(select InstallID from SB_InstallAdd where InstallADDID='" + key + "')";
                        sqls.Add(sql);
                    }
                    if (type == "jsjs")
                    {
                       
                        if (op == "2")
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=0, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);
                        }
                        else
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=3, ReplyMan2='" + CurrentUser.UserName + "',ReplyContent2='" + icp + "',ReplyManName2='" + CurrentUser.RealName + "',ReplyDate2=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'加升业主确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID=(select InstallID from SB_InstallAdd where InstallADDID='" + key + "')";
                        sqls.Add(sql);
                    }

                    if (type == "jsjdz")
                    {
                        if (op == "2")
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=0, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',  AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);
                           
                        }
                        else
                        {
                            sql = "update dbo.SB_InstallAdd set ProgStatus=4, AproveMan='" + CurrentUser.UserName + "',AproveContent='" + icp + "',  AproveManName='" + CurrentUser.RealName + "',AproveDate=getdate() where InstallADDID='" + key + "'";
                            sqls.Add(sql);
                            
                            sqls.Add("INSERT INTO [H_SB_ToPic]([KeyID],[Type]) VALUES('" + key + "','js')");
                        }
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID ,'加升监督站确认：" + optext + "',getdate() from SB_ReportSBSY where InstallID=(select InstallID from SB_InstallAdd where InstallADDID='" + key + "')";
                        sqls.Add(sql);
                    }


                    if (sql != "")
                    {
                        ret = CommonService.ExecSqls(sqls);
                    }
                    else
                    {
                        ret = false;
                        err = "非法操作，操作失败！";
                    }
                }
                else
                {
                    err = "登录失败！" + err;
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }


        #endregion

        #region 手机特殊接口


        /// <summary>
        /// 获取备案记录
        /// </summary>
        public void PhonegetBAList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string sql = " select * from  View_SB_BA where 1=1 ";
                    string strwhere = "";
                    
                    if (CurrentUser.CurUser.UrlJumpType == "Q" || CurrentUser.CurUser.UrlJumpType == "R")
                    {
                        sql += " and (CreaterName='" + CurrentUser.UserName + "' or propertycompanyname='" + CurrentUser.RealName + "')";
                    }
                    if (key != "")
                    {
                        sql += " and (PropertyCompanyName like '%" + key + "%' or FactoryName like '%" + key + "%' or FactoryNO like '%" + key + "%' or BeiAnICP like '%" + key + "%')";
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



        /// <summary>
        /// 获取备案部件记录
        /// </summary>
        public void PhonegetBAGJList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString(); 

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string sql = " select * from  SB_BA_List where 1=1 ";
                    string strwhere = "";
                    if (key != "")
                    {
                        sql += " and ParentBaID='" + key + "'";
                    }
                    else 
                    {
                        sql += " and 1=2";
                    }
                    datas = CommonService.GetDataTable(sql);
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", datas.Count.ToString(), jss.Serialize(datas)));
                Response.End();
            }
        }


        
        /// <summary>
        /// 获取时间轴
        /// </summary>
        public void getSYTimeList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string recid = Request["recid"].GetSafeString();

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string sql = " select SBSYID,ShowText,ShowImgURL,ShowRUL,CONVERT(varchar(100), ShowDate, 23) as ShowDate from dbo.SB_SBSY_Show where SBSYID=" + recid + " order by showdate desc ,recid desc ";
                   
                    datas = CommonService.GetDataTable(sql);
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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", datas.Count.ToString(), jss.Serialize(datas)));
                Response.End();
            }
        }


        /// <summary>
        /// 获取标准节记录
        /// </summary>
        public void PhonegetBZJList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {


                
                 if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                 if (CurrentUser.UserName == "")
                     code = Remote.UserService.Login(username, password, out msg);
                 if (code)
                 {
                     string sql = " select * from  View_SB_BZJ where 1=1 ";
                     string strwhere = "";
                     if (CurrentUser.CurUser.UrlJumpType == "Q" || CurrentUser.CurUser.UrlJumpType == "R")
                     {
                         sql += " and (createrid='" + CurrentUser.UserName + "' or propertycompanyname='" + CurrentUser.RealName + "')";
                     }
                     if (key != "")
                     {
                         sql += " and (PropertyCompanyName like '%" + key + "%' or FactoryName like '%" + key + "%' or FactoryNO like '%" + key + "%' or BZJType like '%" + key + "%' or Qrcode='" + key + "')";
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


        /// <summary>
        /// 标准节出库检查，是否有效能用
        /// </summary>
        public void PhonegetCheckBZJ()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    key = CryptFun.Decode(key);

                    if (key == "" || key==null)
                    {
                        code = false;
                        msg = "该二维码解码失败，二维码不存在，请确认！";
                    }
                    else
                    {
                        string sql = " select gcmc,cqbh from SB_ReportSBSY where State!=2 and RECID in (select SBSYID from SB_BZJADD where BZJID=(select RECID from dbo.SB_BZJ where Qrcode='" + key + "')) ";

                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {

                            code = false;
                            msg = "该标准节正在[" + datas[0]["gcmc"].GetSafeString() + "]工程的[" + datas[0]["cqbh"].GetSafeString() + "]设备中使用，无法重复使用，请确认！";

                        }
                        sql = "select useenddate from dbo.SB_BZJ where Qrcode='" + key + "'";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            if (datas[0]["useenddate"].GetSafeDate(DateTime.MinValue) == DateTime.MinValue || DateTime.Now.AddMonths(6) >= datas[0]["useenddate"].GetSafeDate(DateTime.MinValue))
                            {
                                code = false;
                                msg = "该标准节将在" + datas[0]["useenddate"].GetSafeDate(DateTime.MinValue).ToShortDateString() + "报废，系统禁止使用！";
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "该二维码不存在，请确认！";
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }



        /// <summary>
        /// 二维码注册，目前支持 类型：sbba 主机；gj 构件 ；bzj 标准节
        /// </summary>
        public void PhonegetSubQrcode()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            string type = Request["type"].GetSafeString();
            string value = Request["value"].GetSafeString();
            string icqrcode = Request["icqrcode"].GetSafeString();
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
              
                

                if (code)
                {
                    /*
                    List<MenuItem> menus = new List<MenuItem>();
                    menus = CurrentUser.Menus;
                    var canedit = (from a in menus
                                   where a.MenuCode.Equals("SBGL_BZJGL")
                                   select a).ToList();
                    if (canedit.Count <= 0)
                    {
                        code = false;
                        msg = "当前账户没有操作权限，请确认！";
                    }
                    */
                    if (value != "")
                        value = CryptFun.Decode(value);
                    if (value.GetSafeString("") == "")
                    {
                        code = false;
                        msg = "该铭牌二维码不存在，请确认！";
                    }
                    else
                    {

                        string sql = " select qrcode from INFO_QRCODE where qrcode='" + value + "' ";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {

                        }
                        else
                        {
                            code = false;
                            msg = "铭牌二维码不是平台生成，请确认！";
                        }
                        string iccard = "";
                        string icpassword = "";
                        sql = "select iccard,password from INFO_ICCard where Icno='" + icqrcode + "'";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            iccard = datas[0]["iccard"].GetSafeString();
                            icpassword = datas[0]["password"].GetSafeString();
                            if (icpassword == "")
                            {
                                code = false;
                                msg = "该电子标签二维码尚未注册，请更换一个电子标签！";
                            }

                        }
                        else
                        {
                            code = false;
                            msg = "电子标签二维码不是平台生成，请确认！";
                        }


                        if (code)
                        {
                            //
                        }
                        if (code)
                        {

                            sql = " select beianicp,machineryname from sb_ba where Qrcode1='" + value + "' ";

                            datas = CommonService.GetDataTable(sql);
                            if (datas.Count > 0)
                            {

                                code = false;
                                msg = "该二维码正在备案编号[" + datas[0]["beianicp"].GetSafeString() + "]的[" + datas[0]["machineryname"].GetSafeString() + "]设备中使用，无法重复使用，请确认！";

                            }
                        }
                        if (code)
                        {
                            sql = "select a.beianicp,a.machineryname,b.lx from dbo.SB_BA a,dbo.SB_BA_List b where a.BaID=b.ParentBaID and b.Qrcode='" + value + "'";
                            datas = CommonService.GetDataTable(sql);
                            if (datas.Count > 0)
                            {
                                code = false;
                                msg = "该二维码正在设备[" + datas[0]["beianicp"].GetSafeString() + "][" + datas[0]["machineryname"].GetSafeString() + "]的构件[" + datas[0]["lx"].GetSafeString() + "]中使用，无法重复使用，请确认！";

                            }
                        }
                        if (code)
                        {
                            sql = "select propertycompanyname,factoryno,bzjtype from SB_BZJ where Qrcode='" + value + "'";
                            datas = CommonService.GetDataTable(sql);
                            if (datas.Count > 0)
                            {
                               code = false;
                               msg = "该二维码正在[" + datas[0]["propertycompanyname"].GetSafeString() + "]的[" + datas[0]["factoryno"].GetSafeString() + "]的[" + datas[0]["bzjtype"].GetSafeString() + "]中使用，无法重复使用，请确认！";
                            }
                        }
                        if (code)
                        {
                            IList<string> sqls = new List<string>();
                            sql = "";
                            if (type == "sbba")
                                sql = "update SB_BA set Qrcode1='" + value + "' where RECID=" + key;
                            if (type == "gj")
                                sql = "update SB_BA_List set Qrcode='" + value + "' where RECID=" + key;
                            if (type == "bzj")
                                sql = "update SB_BZJ set Qrcode='" + value + "',status=1 where RECID=" + key;
                            sqls.Add(sql);
                            sql = "update INFO_QRCODE set ICcard='" + iccard + "',Password='" + icpassword + "' where qrcode='" + value + "'";
                            sqls.Add(sql);
                            sql = "INSERT INTO INFO_QRCODE_LS([QRType],[QRkey],[Qrcode],[CreatedBy],[CreatedOn])VALUES('" + type + "','" + key + "','" + value + "','" + CurrentUser.UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            sqls.Add(sql);
                            code = CommonService.ExecTrans(sqls);

                            //CommonService.Execsql(sql);
                        
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        ///  扫描二维码添加标准节,单节增加
        /// </summary>
        public void PhonegetAddBZJ()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            int recid = Request["recid"].GetSafeInt();
            string key = Request["key"].GetSafeString();
            string no = Request["no"].GetSafeString();
            string value = Request["value"].GetSafeString();
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {

                string factoryname = "";
                string factoryno = "";
                DateTime buydate = DateTime.MinValue;

                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    key = CryptFun.Decode(key).GetSafeString();
                    if (key == "")
                    {
                        code = false;
                        msg = "该二维码不存在，请确认！";
                    }
                    else
                    {
                        string sql = " select gcmc,cqbh from SB_ReportSBSY where State!=2 and RECID in (select SBSYID from SB_BZJADD where BZJID=(select RECID from dbo.SB_BZJ where Qrcode='" + key + "')) ";

                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {

                            code = false;
                            msg = "该标准节正在[" + datas[0]["gcmc"].GetSafeString() + "]工程的[" + datas[0]["cqbh"].GetSafeString() + "]设备中使用，无法重复使用，请确认！";

                        }
                        sql = "select useenddate,factoryname,factoryno,buydate from dbo.SB_BZJ where Qrcode='" + key + "'";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {

                            factoryname = datas[0]["factoryname"].GetSafeString();
                            factoryno = datas[0]["factoryno"].GetSafeString();
                            buydate = datas[0]["buydate"].GetSafeDate(DateTime.MinValue);
                            if (datas[0]["useenddate"].GetSafeDate(DateTime.MinValue) == DateTime.MinValue || DateTime.Now.AddMonths(6) >= datas[0]["useenddate"].GetSafeDate(DateTime.MinValue))
                            {
                                code = false;
                                msg = "该标准节将在" + datas[0]["useenddate"].GetSafeDate(DateTime.MinValue).ToShortDateString() + "报废，系统禁止使用！";
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "该二维码不存在，请确认！";
                        }

                        sql = "select state from dbo.SB_ReportSBSY where RECID=" + recid.ToString();
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            if (datas[0]["state"].GetSafeInt(2) == 2)
                            {
                                code = false;
                                msg = "该设备已经拆卸，不能添加标准节，请确认！";
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "该记录不存在，请确认！";
                        }

                        sql = "select count(1) as num from SB_BZJADD  where  (factoryname!='" + factoryname + "' or factoryno!='" + factoryno + "') and SBSYID=" + recid.ToString();
                        datas = CommonService.GetDataTable(sql);
                        {
                            if (datas[0]["num"].GetSafeInt(0) > 0)
                            {
                                code = false;
                                msg = "该标准节与设备原有标准节不是同一个厂家同一个型号，不能混用！";
                            }
                        }
                        /*
                        sql = "select madedate from sb_ba where baid in (select baid from SB_ReportSBSY where RECID=" + recid.ToString() + ")";
                        datas = CommonService.GetDataTable(sql);
                        {
                            if (datas[0]["madedate"].GetSafeDate(DateTime.Now) >= buydate)
                            {
                                code = false;
                                msg = "该标准节购买日期早于主机出场日期，不得使用！";
                            }
                        }
                        */
                        if (code)
                        {
                            //正确的时候，返回当前顺序号，怎么保存想想看，直接按照当前循序保存下去，不要管传上来的
                            int maxdisorder = 0;

                            sql = "select max(DisOrder) as num from SB_BZJADD where SBSYID=" + recid.ToString() + " and no=" + no.ToString();
                            datas = CommonService.GetDataTable(sql);
                            if (datas.Count > 0)
                            {
                                maxdisorder = datas[0]["num"].GetSafeInt(0);
                            }
                            maxdisorder = maxdisorder + 1;
                            sql = "INSERT INTO [SB_BZJADD] ([Qrcode] ,[BZJID] ,[PropertyCompanyName] ,[FactoryName] ,[FactoryNO] ,[SBSYID] ,[Status] ,[DisOrder] ,[CreaterName] ,[CreaterID] ,[ZL] ,[No] ,[BZJType]) select [Qrcode] ,RECID ,[PropertyCompanyName] ,[FactoryName] ,[FactoryNO] ," + recid.ToString() + " ,[Status] ,'" + maxdisorder.ToString() + "' ,[CreaterName] ,[CreaterID] ,[ZL] ," + no + " ,[BZJType] from SB_BZJ where [Qrcode]='" + key + "'";
                            code = CommonService.Execsql(sql);
                            if (code)
                                msg = maxdisorder.ToString();
                            else
                                msg = "保存失败！";
                        }
                    }
                }
                else 
                {
                    code = false;
                    msg = "用户登录失败，请退出重新登录！";
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }



        /// <summary>
        /// 手机设备详情
        /// </summary>
        public void PhoneGetSBDetail()
        {
            string msg = "";
            bool code = true;
            string cqbh = "";
            string baid = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            int recid = Request["recid"].GetSafeInt();
            string formid = "";
            string workserial = "";
            string jcdw = "";
            string jyrzh = "";
            string jyrxm = "";
            string gcbh = "";
            int hassz = 0;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
            try
            {
                 if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    
                    string sql = "";


                    string useid = "";
                    sql = "select * from View_SBSY where recid=" + recid;
                    retdt = CommonService.GetDataTable(sql);
                    if (retdt.Count > 0)
                    {
                        cqbh = retdt[0]["cqbh"].GetSafeString();
                        baid = retdt[0]["baid"].GetSafeString();
                        gcbh = retdt[0]["jdzch"].GetSafeString();
                        useid = retdt[0]["useid"].GetSafeString();
                    }


                    if (useid != "")
                    {
                        sql = "select checkunitname from dbo.SB_UseReg where UseID='" + useid + "'";
                        dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            jcdw = dt[0]["checkunitname"].GetSafeString();
                        }
                    }

                    sql = "select formid,workserial,jyrzh,jyrxm,ytdwmc from View_ReportQZJ where  sbbaid='" + baid + "' and (gcbh='" + gcbh + "' or gcbh=(select gcbh from I_M_GC where SJGCBH='" + gcbh + "'))";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        formid = dt[0]["formid"].GetSafeString();
                        workserial = dt[0]["workserial"].GetSafeString();
                        jyrzh = dt[0]["jyrzh"].GetSafeString();
                        jyrxm = dt[0]["jyrxm"].GetSafeString();
                        jcdw = dt[0]["ytdwmc"].GetSafeString();
                    }
                    dt = CommonService.GetDataTable("select count(1) as num from dbo.SB_BZJADD where no=2 and status!=2 and sbsyid=" + recid + " ");
                    if (dt.Count > 0)
                        hassz = dt[0]["num"].GetSafeInt(0);
                    dt = CommonService.GetDataTable("select * from dbo.SB_BZJADD where status!=2 and sbsyid=" + recid + " order by DisOrder desc");
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
                string ret = string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"sbsy\":{3},\"formid\":\"{4}\",\"workserial\":\"{5}\",\"jyrzh\":\"{6}\",\"jyrxm\":\"{7}\",\"jcdw\":\"{8}\",\"Datas\":{9}}}", code ? "0" : "1", msg, hassz.ToString(), jss.Serialize(retdt), formid, workserial, jyrzh, jyrxm, jcdw, jss.Serialize(dt));
                Response.Write(ret);
                Response.End();
            }
        }



        /// <summary>
        /// 扫描二维码添加标准节,多节增加
        /// </summary>
        public void PhonegetAddBZJS()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            int recid = Request["recid"].GetSafeInt();

            string msg = "添加成功";
            bool code = true;

            
            JavaScriptSerializer jss = new JavaScriptSerializer();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    IList<JBzjAdd> bzjs = jss.Deserialize<IList<JBzjAdd>>(key);
                    string bzjid = "";
                    for (int i = 0; i < bzjs.Count(); i++)
                    {
                        if (bzjid == "")
                            bzjid += ",";
                        bzjid += "'" + bzjs[i].name + "'";
                    }

                    string sql = " select qrcode from dbo.SB_BZJADD where BZJID=(select RECID from dbo.SB_BZJ where Qrcode in(" + bzjid + ")) and SBSYID=(select recid from SB_ReportSBSY where State=2) ";

                    datas = CommonService.GetDataTable(sql);
                    string errtext = "";
                    for (int i = 0; i < datas.Count; i++)
                    {
                        code = false;
                        var q = from e in bzjs where e.name.Equals(datas[i]["qrcode"], StringComparison.OrdinalIgnoreCase) select e;
                        if (q.Count() > 0)
                            errtext += q.First().no + "号柱，第" + q.First().value+"号标准节；";
                    }
                    if (errtext != "")
                    {
                        errtext += "上述标准节系统状态正在使用中，请确认！";
                    }

                    sql = "select useenddate from dbo.SB_BZJ where Qrcode='" + key + "'";
                    datas = CommonService.GetDataTable(sql);
                    for (int i = 0; i < datas.Count; i++)
                    {
                        if (datas[0]["useenddate"].GetSafeDate(DateTime.MinValue) == DateTime.MinValue || DateTime.Now.AddMonths(6) >= datas[0]["useenddate"].GetSafeDate(DateTime.MinValue))
                        {
                            code = false;
                            msg = "该标准节将在" + datas[0]["useenddate"].GetSafeDate(DateTime.MinValue).ToShortDateString() + "报废，系统禁止使用！";
                        }
                    }
                    /*
                    string sql = " select count(1) as num from dbo.SB_BZJADD where BZJID=(select RECID from dbo.SB_BZJ where Qrcode='" + key + "') and SBSYID=(select recid from SB_ReportSBSY where State=2) ";

                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count > 0)
                    {
                        if (datas[0]["num"].GetSafeInt(0) > 0)
                        {
                            code = false;
                            msg = "该标准节正在使用中，请确认！";
                        }
                    }
                    sql = "select useenddate from dbo.SB_BZJ where Qrcode='" + key + "'";
                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count > 0)
                    {
                        if (datas[0]["useenddate"].GetSafeDate(DateTime.MinValue) == DateTime.MinValue || DateTime.Now.AddMonths(6) >= datas[0]["useenddate"].GetSafeDate(DateTime.MinValue))
                        {
                            code = false;
                            msg = "该标准节将在" + datas[0]["useenddate"].GetSafeDate(DateTime.MinValue).ToShortDateString() + "报废，系统禁止使用！";
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "该二维码不存在，请确认！";
                    }*/

                }
                else
                {
                    code = false;
                    msg = "用户登录失败，请退出重新登录！";
                }

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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        /// 标准上传json实例化
        /// </summary>
        public class JBzjAdd
        {
            /// <summary>
            /// 二维码号码
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 顺序号（数字，整型）
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 柱号（数字，整型，只有1，2）
            /// </summary>
            public string no { get; set; }
        }


        public void PhonegetSBSYList()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            string baid = Request["baid"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string sql = " SELECT [RECID],[JDZCH],[GCMC],[CQDW],[CQBH],[AZRQ],[SYRQ],[CXRQ],[JCYXQ],[HXZBH],[SBBFRQ],[CreatedBy],[WorkSerial],[SBMC],[AZDW],[State],[Candit],[SGDW],[JLDW],[GZRQ],[BaID],[UseNo],[InstallID],[UseID],[UnInstallID],[SBXH],[CCBH],[ZZXKZ],[FZQ1],[FZQ2],[GCDD],[DJZSH],[AZDWBH],[SGDWBH],[JLDWBH],[JDZID],[InstallProgStatus],[UserProgStatus],[UnInstallProgStatus] FROM [View_SBSY] where 1=1 ";
                    string strwhere = "";
                    if (CurrentUser.CurUser.UrlJumpType == "Q" || CurrentUser.CurUser.UrlJumpType == "R")
                    {
                        sql += " and (SGDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "') or AZDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "') or JLDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "'))";
                    }
                    if (key != "")
                    {
                        sql += " and (JDZCH like '%" + key + "%' or GCMC like '%" + key + "%' or CQDW like '%" + key + "%' or CQBH like '%" + key + "%')";
                    }
                    if (baid != "")
                    {
                        sql += " and baid='" + baid + "'";
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

        public void PhonegetSBSYList2()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string type = Request["type"].GetSafeString("");
            int totalcount = 0;

            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {



                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string sql = " SELECT [RECID],[JDZCH],[GCMC],[CQDW],[CQBH],[AZRQ],[SYRQ],[CXRQ],[JCYXQ],[HXZBH],[SBBFRQ],[CreatedBy],[WorkSerial],[SBMC],[AZDW],[State],[Candit],[SGDW],[JLDW],[GZRQ],[BaID],[UseNo],[InstallID],[UseID],[UnInstallID],[SBXH],[CCBH],[ZZXKZ],[FZQ1],[FZQ2],[GCDD],[DJZSH],[AZDWBH],[SGDWBH],[JLDWBH],[JDZID],[InstallProgStatus],[UserProgStatus],[UnInstallProgStatus] FROM [View_SBSY] where 1=1 ";
                    string strwhere = "";
                    
                    
                    if (key != "")
                    {
                        sql += " and (JDZCH like '%" + key + "%' or GCMC like '%" + key + "%' or CQDW like '%" + key + "%' or CQBH like '%" + key + "%')";
                    }

                    if (type == "azsg")
                        sql += " and SGDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "')  and InstallProgStatus='草稿'";
                    else if (type == "azjl")
                        sql += " and JLDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "')  and InstallProgStatus='等待监理确认'";
                    else if (type == "azjdz" || type == "azjjdz")
                        sql += " and jdzid ='" + CurrentUser.CompanyCode + "'  and InstallProgStatus='等待监督站审批' ";
                    else if (type == "syjl")
                        sql += " and JLDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "') and UserProgStatus='等待监理确认'";
                    else if (type == "syjdz")
                        sql += " and jdzid ='" + CurrentUser.CompanyCode + "'  and UserProgStatus='等待监督站审批'";
                    else if (type == "cxsg")
                        sql += " and SGDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "')  and UnInstallProgStatus='草稿' ";
                    else if (type == "cxjl")
                        sql += " and JLDWBH=(select qybh from i_M_QYZH where yhzh='" + CurrentUser.UserName + "')  and UnInstallProgStatus='等待监理确认'";
                    else if (type == "cxjdz")
                        sql += " and jdzid ='" + CurrentUser.CompanyCode + "'  and UnInstallProgStatus='等待监督站审批'";
                    else if (type == "azjs")
                        sql += " and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))  and InstallProgStatus='等待监理确认'";
                    else if (type == "syjs")
                        sql += " and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))  and UserProgStatus='等待监理确认'";
                    else if (type == "cxjs")
                        sql += " and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))  and UnInstallProgStatus='等待监理确认'";
                    else
                        sql += " and 1=2";
                    sql += " order by AZRQ desc";

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


        public void PhoneGetMachineAttach()
        {
            string err = "";
            bool ret = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                string msg = "";
                if (!CurrentUser.IsLogin)
                    ret = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    ret = Remote.UserService.Login(username, password, out msg);
                if (ret)
                {
                    string key = Request["key"].GetSafeString();
                    string type = Request["type"].GetSafeString("");
                    string sql = "";
                    if (type.IndexOf("az") >= 0)
                    {
                        sql = "select attachfile from SB_Install where InstallID='" + key + "'";
                    }
                    else if (type.IndexOf("sy") >= 0)
                    {
                        sql = "select attachfile from SB_UseReg where UseID='" + key + "'";
                    }
                    else
                    {
                        sql = "select attachfile from SB_UnInstall where UnInstallID='" + key + "'";
                    }
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        err = dt[0]["attachfile"];
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
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(ret, err));
                Response.End();
            }
        }


        /// <summary>
        /// exmobi转换一道，因为不能带有两个参数
        /// </summary>
        /// <returns></returns>
        public ActionResult getattachfile()
        {

            string fileid = Request["fileid"].GetSafeString();

            return new RedirectResult("/DataInput/FileService?method=DownloadFile&fileid=" + fileid);
            //return RedirectToAction("FlowReportDown", "jdbg", new { reportfile = "%E7%9B%91%E7%9D%A3%E6%96%B9%E6%A1%88v1", serial = 20170116009 });

        }
        /// <summary>
        /// 自动生成二维码
        /// </summary>
        [Authorize]
        public void AddQRcode()
        {
            int no = Request["no"].GetSafeInt();
            string bz = Request["bz"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {

                if (no <= 0)
                {
                    ret = false;
                    err = "数量错误，请输入大于0的整数";
                }
                else
                {
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    string prtype = "";
                    string sql = "select pretext from dbo.H_Qrcode_User where username='" + CurrentUser.UserName + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        int maxid = 0;
                        sql = "select max(recid) as maxid from INFO_QRCODE";
                        IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                        dt2 = CommonService.GetDataTable(sql);
                        if (dt2.Count > 0)
                        {
                            maxid = dt2[0]["maxid"].GetSafeInt(0);
                        }

                        IList<string> sqls = new List<string>();
                        prtype = dt[0]["pretext"].GetSafeString();
                        for (int i = 0; i < no; i++)
                        {
                            string wyh = prtype + Guid.NewGuid().ToString("N");
                            string encode=CryptFun.Encode(wyh);
                            sqls.Add("INSERT INTO [INFO_QRCODE]([Qrcode],[EncodeQrcode],[CreatedName],[CreatedBy],[CreatedOn],[BZ])VALUES('" + wyh + "','" + encode + "','" + CurrentUser.RealName + "','" + CurrentUser.UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + bz + "')");
                        }
                         ret = CommonService.ExecTrans(sqls);
                         if (ret)
                             err = maxid.ToString();
                         else
                             err = "数据保存出错！";

                    }
                    else
                    {
                        ret = false;
                        err = "你没有权限生成二维码，请联系系统管理员";
                    }

                }

                /*
               
                sqls.Add("update SB_BZJ set  Status=1,qrcode='" + icp + "' where recid=" + recid);
                ret = CommonService.ExecTrans(sqls);*/
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



        /// <summary>
        /// 自动生成电子标签二维码
        /// </summary>
        [Authorize]
        public void AddICQRcode()
        {
            int no = Request["no"].GetSafeInt();
            string bz = Request["bz"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {

                if (no <= 0)
                {
                    ret = false;
                    err = "数量错误，请输入大于0的整数";
                }
                else
                {
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    string prtype = "";
                    string sql = "select pretext from dbo.H_Qrcode_User where username='" + CurrentUser.UserName + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        int maxid = 0;
                        sql = "select max(recid) as maxid from INFO_ICCard";
                        IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                        dt2 = CommonService.GetDataTable(sql);
                        if (dt2.Count > 0)
                        {
                            maxid = dt2[0]["maxid"].GetSafeInt(0);
                        }

                        IList<string> sqls = new List<string>();
                        prtype = dt[0]["pretext"].GetSafeString();
                        for (int i = 0; i < no; i++)
                        {
                            string iccard = Guid.NewGuid().ToString("N");
                            string icno = Guid.NewGuid().ToString("N");
                            string serialno = (maxid + i + 1).ToString().PadLeft(8, '0');
                            sqls.Add("INSERT INTO [INFO_ICCard] ([ICCard],[ICNo] ,[PassWord],[SerialNo]) VALUES('" + iccard + "','" + icno + "','','" + serialno + "')");
                        }
                        ret = CommonService.ExecTrans(sqls);
                        if (ret)
                            err = maxid.ToString();
                        else
                            err = "数据保存出错！";

                    }
                    else
                    {
                        ret = false;
                        err = "你没有权限生成二维码，请联系系统管理员";
                    }

                }

                /*
               
                sqls.Add("update SB_BZJ set  Status=1,qrcode='" + icp + "' where recid=" + recid);
                ret = CommonService.ExecTrans(sqls);*/
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



        /// <summary>
        /// 上传图片解析二维码
        /// </summary>
        [Authorize]
        public void JXQRcode()
        {
            int no = Request["no"].GetSafeInt();
            string bz = Request["bz"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postfile = Request.Files[0];
                    BarcodeReader reader = new BarcodeReader();
                    System.Drawing.Bitmap image =new System.Drawing.Bitmap(postfile.InputStream);
                    Result res=reader.Decode(image);
                    ret=true;
                    err = res.Text;

                }
                else
                {
                    ret = false;
                    err = "请上传二维码图片！";
                }
                /*
                ZXing.BarcodeReader reader = new BarcodeReader();
                reader.Decode()
                QRCodeDecoder decoder = new QRCodeDecoder(); 
                if (no <= 0)
                {
                    ret = false;
                    err = "数量错误，请输入大于0的整数";
                }
                else
                {
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    string prtype = "";
                    string sql = "select pretext from dbo.H_Qrcode_User where username='" + CurrentUser.UserName + "'";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        int maxid = 0;
                        sql = "select max(recid) as maxid from INFO_QRCODE";
                        IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                        dt2 = CommonService.GetDataTable(sql);
                        if (dt2.Count > 0)
                        {
                            maxid = dt2[0]["maxid"].GetSafeInt(0);
                        }

                        IList<string> sqls = new List<string>();
                        prtype = dt[0]["pretext"].GetSafeString();
                        for (int i = 0; i < no; i++)
                        {
                            string wyh = prtype + Guid.NewGuid().ToString("N");
                            string encode = CryptFun.Encode(wyh);
                            sqls.Add("INSERT INTO [INFO_QRCODE]([Qrcode],[EncodeQrcode],[CreatedName],[CreatedBy],[CreatedOn],[BZ])VALUES('" + wyh + "','" + encode + "','" + CurrentUser.RealName + "','" + CurrentUser.UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + bz + "')");
                        }
                        ret = CommonService.ExecTrans(sqls);
                        if (ret)
                            err = maxid.ToString();
                        else
                            err = "数据保存出错！";

                    }
                    else
                    {
                        ret = false;
                        err = "你没有权限生成二维码，请联系系统管理员";
                    }

                }

                /*
               
                sqls.Add("update SB_BZJ set  Status=1,qrcode='" + icp + "' where recid=" + recid);
                ret = CommonService.ExecTrans(sqls);*/
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

    }
}
