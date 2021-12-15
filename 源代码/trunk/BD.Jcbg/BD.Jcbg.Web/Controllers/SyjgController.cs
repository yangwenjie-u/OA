using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using System.IO;
using Spring.Context;
using Spring.Context.Support;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;

namespace BD.Jcbg.Web.Controllers
{
    public class SyjgController : Controller
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
        public ISysjService _sysjService = null;
        public ISysjService SysjService
        {
            get
            {
                if (_sysjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _sysjService = webApplicationContext.GetObject("SysjService") as ISysjService;
                }
                return _sysjService;
            }
        }
        #endregion


        #region 页面

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Video()
        {
            ViewData.Add("CtrlWidth", Request["width"].GetSafeInt());
            ViewData.Add("CtrlHeight", Request["height"].GetSafeInt());
            string VideoFile = Request["filename"].GetSafeString();
            ViewData.Add("FileName", "/videos/" + VideoFile + "." + Configs.VideoExt);
            return View();
        }
        public ActionResult ViewImage()
        {
            ViewData.Add("Recid", Request["recid"].GetSafeInt());
            return View();
        }
        public ActionResult DclogRedos()
        {
            int recid = Request["recid"].GetSafeInt();
            DcLog sysj = SysjService.GetDclog(recid);
            ViewData.Add("uniqcode", sysj.UniqCode);
            return View();
        }
        public ActionResult bhz()
        {
            
            return View();
        }



        public ActionResult Recode()
        {
            int recid = Request["id"].GetSafeInt();
            string name = "";
            string handleman = "";
            string icp = "";
            string context = "";
            string btntext = "";
            string MachineryName = "";
            ViewBag.id = recid;

            //ViewData.Add("id", Request["id"].GetSafeInt());
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            datas = CommonService.GetDataTable("select BeiAnICP,ReplyMan1,ReplyMan2,ReplyContent1,BeiAnStatus,MachineryName from sb_ba where recid=" + recid);
            if (datas.Count > 0)
            {
                icp = datas[0]["beianicp"];
                name = datas[0]["replyman1"];
                handleman = datas[0]["replyman2"];
                context = datas[0]["replycontent1"];
                btntext = datas[0]["beianstatus"].GetSafeInt(0).ToString();
                MachineryName = datas[0]["machineryname"];

            }
            if (name == "")
                name = CurrentUser.RealName;
            ViewBag.icp = icp;
            ViewBag.name = name;
            ViewBag.handleman = handleman;
            ViewBag.context = context;
            ViewBag.btntext = btntext;
            ViewBag.machineryname = MachineryName;


            return View();
        }

        /// <summary>
        /// 设备备案经办
        /// </summary>
        /// <returns></returns>
        public ActionResult SBHandle()
        {
            int recid = Request["id"].GetSafeInt();
            string name = "";
            string btntext = "";
            ViewBag.id = recid;

            //ViewData.Add("id", Request["id"].GetSafeInt());
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            datas = CommonService.GetDataTable("select ReplyMan2,BeiAnStatus,MachineryName from sb_ba where recid=" + recid);
            if (datas.Count > 0)
            {
                name = datas[0]["replyman2"];
                btntext = datas[0]["beianstatus"].GetSafeInt(0).ToString();

            }
            else
            {

            }
            if (name == "")
                name = CurrentUser.RealName;
            ViewBag.handleman = name;
            ViewBag.btntext = btntext;

            return View();
        }

        /// <summary>
        /// 设备备案经办
        /// </summary>
        /// <returns></returns>
        public ActionResult SBSpr()
        {
            int recid = Request["id"].GetSafeInt();

            ViewBag.id = recid;
            string spr = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            datas = CommonService.GetDataTable("select sbspzh from H_ZJZ where RECID=" + recid);
            if (datas.Count > 0)
            {
                spr = datas[0]["sbspzh"];

            }
            ViewBag.spr = spr;
            return View();
        }


        public ActionResult AddInstall()
        {
            string InstallID = Request["id"].GetSafeString();
            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_Install";
            param.t1_pri = "InstallID";
            param.t1_title = "安装告知";
            if (InstallID != "")
                param.jydbh = InstallID;

            param.t2_tablename = "SB_SpecialPerson";
            ////主键
            param.t2_pri = "FKID,SPID";
            ////标题
            param.t2_title = "特种人员";

            param.preproc = "data_input_check_Install|$SB_Install.CQBH";

            param.sufproc = "data_input_after_Install|$JYDBH$";


            param.rownum = 3;
            param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            return RedirectToAction("Index", "DataInput", param);
        }

        public ActionResult AddUse()
        {

            string InstallID = Request["id"].GetSafeString();
            string UseID = Request["id2"].GetSafeString();
            string BaID = "", SBMC = "", CQBH = "", ProName = "", ConsPermitNo = "", WeiBaoUnitMan = "", WeiBaoUnitManTel = "";
            int install = 0, usereg = 0;
            IList<IDictionary<string, string>> dt = null;
            string sql = "select installprogstatus,userprogstatus from sb_reportsbsy where InstallID='" + InstallID + "'";
            dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                install = dt[0]["installprogstatus"].GetSafeInt();
                usereg = dt[0]["userprogstatus"].GetSafeInt();
            }

            if (install != 4 || usereg == 4)
                return View("Error");

            sql = "select baid,sbmc,cqbh,proname,conspermitno,installunittechman,installunittechmantel from sb_install where InstallID='" + InstallID + "'";
            dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                BaID = dt[0]["baid"].GetSafeString();
                SBMC = dt[0]["sbmc"].GetSafeString();
                CQBH = dt[0]["cqbh"].GetSafeString();
                ProName = dt[0]["proname"].GetSafeString();
                ConsPermitNo = dt[0]["conspermitno"].GetSafeString();
                WeiBaoUnitMan = dt[0]["installunittechman"].GetSafeString();
                WeiBaoUnitManTel = dt[0]["installunittechmantel"].GetSafeString();
            }


            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_UseReg";
            param.t1_pri = "UseID";
            param.t1_title = "使用登记";
            if (UseID != "")
                param.jydbh = UseID;

            param.fieldparam = "SB_UseReg,InstallID," + InstallID + "|SB_UseReg,BaID," + BaID + "|SB_UseReg,SBMC," + SBMC + "|SB_UseReg,CQBH," + CQBH + "|SB_UseReg,ProName," + ProName + "|SB_UseReg,ConsPermitNo," + ConsPermitNo + "|SB_UseReg,WeiBaoUnitMan," + WeiBaoUnitMan + "|SB_UseReg,WeiBaoUnitManTel," + WeiBaoUnitManTel;
            param.t2_tablename = "SB_SpecialPerson";
            ////主键
            param.t2_pri = "FKID,SPID";
            ////标题
            param.t2_title = "特种人员";

            param.preproc = "data_input_check_Use|$SB_UseReg.CheckDate|$SB_UseReg.ADetectDate|$SB_UseReg.BDetectDate";

            param.sufproc = "data_input_after_use|$JYDBH$";


            param.rownum = 3;
            param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            return RedirectToAction("Index", "DataInput", param);
        }

        public ActionResult AddUnInstall()
        {

            string InstallID = Request["id"].GetSafeString();
            string UnInstallID = Request["id2"].GetSafeString();
            string BaID = "", ConsPermitNo = "", CQBH = "", CreaterID = "", CreaterName = "", ProName = "", SBMC = "", UnFinalInstallHeight = "", UnFirstInstallHeight = "", UnInstallPlan = "", UnInstallUnitCertificate = "", UnInstallUnitName = "", UnInstallUnitProductionLicense = "", UnInstallUnitTechMan = "", UnInstallUnitTechManTel = "";
            int uninstall = 0, usereg = 0;
            IList<IDictionary<string, string>> dt = null;
            string sql = "select uninstallprogstatus,userprogstatus from sb_reportsbsy where InstallID='" + InstallID + "'";
            dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                uninstall = dt[0]["uninstallprogstatus"].GetSafeInt();
                usereg = dt[0]["userprogstatus"].GetSafeInt();
            }

            if (uninstall == 4 || usereg != 4)
                return View("Error");

            sql = "select baid,conspermitno,cqbh,createrid,creatername,proname,sbmc,finalinstallheight,firstinstallheight, installplan,installunitcertificate,installunitname,installunitproductionlicense,installunittechman,installunittechmantel from sb_install where InstallID='" + InstallID + "'";
            dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                BaID = dt[0]["baid"].GetSafeString();
                ConsPermitNo = dt[0]["conspermitno"].GetSafeString();
                CQBH = dt[0]["cqbh"].GetSafeString();
                CreaterID = dt[0]["createrid"].GetSafeString();
                CreaterName = dt[0]["creatername"].GetSafeString();
                ProName = dt[0]["proname"].GetSafeString();
                SBMC = dt[0]["sbmc"].GetSafeString();
                UnFinalInstallHeight = dt[0]["finalinstallheight"].GetSafeString();
                UnFirstInstallHeight = dt[0]["firstinstallheight"].GetSafeString();
                UnInstallPlan = dt[0]["installplan"].GetSafeString();
                UnInstallUnitCertificate = dt[0]["installunitcertificate"].GetSafeString();
                UnInstallUnitName = dt[0]["installunitname"].GetSafeString();
                UnInstallUnitProductionLicense = dt[0]["installunitproductionlicense"].GetSafeString();
                UnInstallUnitTechMan = dt[0]["installunittechman"].GetSafeString();
                UnInstallUnitTechManTel = dt[0]["installunittechmantel"].GetSafeString();
            }


            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_UnInstall";
            param.t1_pri = "UnInstallID";
            param.t1_title = "使用登记";
            if (UnInstallID != "")
                param.jydbh = UnInstallID;

            param.fieldparam = "SB_UnInstall,InstallID," + InstallID + "|SB_UnInstall,BaID," + BaID + "|SB_UnInstall,ConsPermitNo," + ConsPermitNo + "|SB_UnInstall,CQBH," + CQBH + "|SB_UnInstall,CreaterID," + CreaterID + "|SB_UnInstall,CreaterName," + CreaterName + "|SB_UnInstall,ProName," + ProName + "|SB_UnInstall,SBMC," + SBMC + "|SB_UnInstall,UnFinalInstallHeight," + UnFinalInstallHeight + "|SB_UnInstall,UnFirstInstallHeight," + UnFirstInstallHeight + "|SB_UnInstall,UnInstallPlan," + UnInstallPlan + "|SB_UnInstall,UnInstallUnitCertificate," + UnInstallUnitCertificate + "|SB_UnInstall,UnInstallUnitName," + UnInstallUnitName + "|SB_UnInstall,UnInstallUnitProductionLicense," + UnInstallUnitProductionLicense + "|SB_UnInstall,UnInstallUnitTechMan," + UnInstallUnitTechMan + "|SB_UnInstall,UnInstallUnitTechManTel," + UnInstallUnitTechManTel;
            param.t2_tablename = "SB_SpecialPerson";
            ////主键
            param.t2_pri = "FKID,SPID";
            ////标题
            param.t2_title = "特种人员";

            //param.preproc = "data_input_check_Use|$SB_UseReg.CheckDate|$SB_UseReg.ADetectDate|$SB_UseReg.BDetectDate";

            param.sufproc = "data_input_after_UnInstall|$JYDBH$";


            param.rownum = 3;
            param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            return RedirectToAction("Index", "DataInput", param);
        }


        public ActionResult viewInstall()
        {
            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_Install";
            param.t1_pri = "InstallID";
            param.t1_title = "安装告知";
            param.jydbh = Request["id"].ToString();
            //param.view = true;

            param.t2_tablename = "SB_SpecialPerson";
            ////主键
            param.t2_pri = "FKID,SPID";
            ////标题
            param.t2_title = "特种人员";

            param.preproc = "data_input_check_Install|$SB_Install.CQBH";

            param.sufproc = "data_input_after_Install|$JYDBH$";


            param.rownum = 3;
            //param.button = "保存|TJ|/WebForm/Index?FormDm=companysupplies1&FormStatus=0|fa fa-calendar-times-o|保存成功！||返回|FH|http://www.baidu.com";

            return RedirectToAction("Index", "DataInput", param);
        }

        public ActionResult viewUse()
        {

            string UseID = Request["id"].GetSafeString();


            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_UseReg";
            param.t1_pri = "UseID";
            param.t1_title = "使用登记";
            param.jydbh = UseID;


            param.t2_tablename = "SB_SpecialPerson";
            ////主键
            param.t2_pri = "FKID,SPID";
            ////标题
            param.t2_title = "特种人员";

            //param.view = true;


            param.rownum = 3;


            return RedirectToAction("Index", "DataInput", param);
        }

        public ActionResult viewUnInstall()
        {

            string UnInstallID = Request["id"].GetSafeString();



            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "datazdzd";
            param.t1_tablename = "SB_UnInstall";
            param.t1_pri = "UnInstallID";
            param.t1_title = "使用登记";
            if (UnInstallID != "")
                param.jydbh = UnInstallID;

            //param.fieldparam = "SB_UseReg,InstallID," + InstallID + "|SB_UseReg,BaID," + BaID + "|SB_UseReg,SBMC," + SBMC + "|SB_UseReg,CQBH," + CQBH + "|SB_UseReg,ProName," + ProName + "|SB_UseReg,ConsPermitNo," + ConsPermitNo + "|SB_UseReg,WeiBaoUnitMan," + WeiBaoUnitMan + "|SB_UseReg,WeiBaoUnitManTel," + WeiBaoUnitManTel;
            param.t2_tablename = "SB_SpecialPerson";
            ////主键
            param.t2_pri = "FKID,SPID";
            ////标题
            param.t2_title = "特种人员";

            //param.preproc = "data_input_check_Use|$SB_UseReg.CheckDate|$SB_UseReg.ADetectDate|$SB_UseReg.BDetectDate";

            param.sufproc = "data_input_after_UnInstall|$JYDBH$";


            param.rownum = 3;
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            return RedirectToAction("Index", "DataInput", param);
        }


        public ActionResult sbqrcode()
        {
            string recid = Request["recid"].GetSafeString();
            ViewData.Add("recid", recid);
            return View();
        }
        public ActionResult sbcontent()
        {
            string recid = Request["recid"].GetSafeString();
            ViewData.Add("recid", recid);
            return View();
        }


        #endregion

        #region 获取各种列表
        [Authorize]
        public void GetSylbs()
        {
            IList<Xcsj> rows = SysjService.GetXcsjs();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(jss.Serialize(rows));
            Response.End();
        }

        [Authorize]
        public void GetDetail()
        {
            int recid = Request["recid"].GetSafeInt(0);

            DcLog sysj = SysjService.GetDclog(recid);
            string commsylb = sysj.Csylb;
            IList<Sysjsd> sysjsds = SysjService.GetSysjsds(commsylb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' border='0'><tr><td align='center'><table class='form2' width='100%' cellspacing='0'  style='border-top-style: solid;border-top-color: #ebebeb;border-top-width: 1px;border-left-color: #ebebeb;border-left-style: solid;border-left-width: 1px;'>");
            sb.Append("<tr style='background-color: #f5f5f5;'><th width='100' style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>试验编号</th><td  width='180' style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + sysj.Wtbh + "</td><th width='100' style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>试验人</th><td style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + sysj.Syr + "</td></tr>");
            sb.Append("<tr style='background-color: #f5f5f5;'><th style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>试验日期</th><td style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + sysj.Syrq.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td><th style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>试验设备</th><td style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + sysj.Sbmcbh + "</td></tr>");
            for (int i = 0; i < sysjsds.Count; i += 2)
            {
                Sysjsd sd1 = sysjsds[i];
                Sysjsd sd2 = null;
                if (i + 1 < sysjsds.Count)
                    sd2 = sysjsds[i + 1];
                sb.Append("<tr><th style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + sd1.Sy + "</th><td style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + GetSysjItem(sysj, sd1.SysjZd.Trim()) + "</td>");
                sb.Append("<th style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + (sd2 != null ? sd2.Sy : "") + "</th><td style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;'>" + (sd2 != null ? GetSysjItem(sysj, sd2.SysjZd.Trim()) : "") + "</td></tr>");
            }
            sb.Append("</table></td></tr></table>");
            Response.Write(sb.ToString());
            Response.End();
        }

        public void GetBHZDetail()
        {
            int recid = Request["recid"].GetSafeInt(0);
            string ret = "";
            BanHeZhanData row = SysjService.GetBHZData(recid);
            if (row != null)
            {
                ret = "<table style='border-top-style: solid;border-top-color: #ebebeb;border-top-width: 1px;border-left-color: #ebebeb;border-left-style: solid;border-left-width: 1px;'><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>拌和机编号：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.SheBeiBianHao + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>工单号：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.GongDanHao + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>操作者：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ChaoZuoZhe + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>设计方量：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.SheJiFangLiang + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>实际方量：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.GuJiFangShu + "</td></tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>出料时间：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ChuLiaoShiJian + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>工程名称：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.GongChengMingMheng + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>施工地点：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.SiGongDiDian + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>浇注部位：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.JiaoZuoBuWei + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水泥品种：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ShuiNiPingZhong + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>配方号：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.PeiFangHao + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>强度等级：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.QiangDuDengJi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>搅拌时长：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.JiaoBanShiJian + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>保存时间：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.BaoCunShiJian + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>唯一码(本拌合机数据唯一编号)：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.KeHuDuanBianhao + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>采集时间：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.GetTime + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>细骨料1：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.XiGuLiao1ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>细骨料1配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.XiGuLiao1LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>细骨料2：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.XiGuLiao2ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>细骨料2配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.XiGuLiao2LiLnZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粗骨料1：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.CuGuLiao1ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粗骨料1配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.CuGuLiao1LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粗骨料2：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.CuGuLiao2ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粗骨料2配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.CuGuLiao2LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粗骨料3：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.CuGuLiao3ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粗骨料3配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.CuGuLiao3LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水泥1：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ShuiNi1ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水泥1配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ShuiNi1LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水泥2：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ShuiNi2ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水泥2配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.ShuiNi2LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>矿粉：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.KuangFen3ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>矿粉配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.KuangFen3LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粉煤灰：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.FenMeiHui4ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粉煤灰配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.FenMeiHui4LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粉料5：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.FenLiao5ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粉料5配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.FenLiao5LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粉料6：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.FenLiao6ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>粉料6配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.FenLiao6LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水1：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.Shui1ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水1配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.Shui1LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水2：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.Shui2ShijiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>水2配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.Shui2LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂1：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi1ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂1配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi1LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂2：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi2ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂2配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi2LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂3：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi3ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂3配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi3LiLunZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂4：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi4ShiJiZhi + "</td></tr><tr><th width='150’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>外加剂4配比：</th><td  width='400’ style='border-right-style: solid; border-right-color: #dddddd;  border-right-width: 1px; border-bottom-color: #dddddd;  border-bottom-style: solid;  border-bottom-width: 1px;  padding: 5px;’>" + row.WaiJiaJi4LiLunZhi + "</td></tr><tr></table>";
            }
            Response.Write(ret);
            Response.End();
        }
        [Authorize]
        public void GetCurve()
        {
            int type = Request["type"].GetSafeInt(1);
            int recid = Request["recid"].GetSafeInt(0);
            string syr = "", sybh = "", datalist = "";
            DateTime syrq = DateTime.Now;
            if (type == 1)
            {
                DcLog sysj = SysjService.GetDclog(recid);
                syr = sysj.Syr;
                sybh = sysj.Wtbh;
                datalist = sysj.DataList;
                syrq = sysj.Syrq.Value;
            }
            else if (type == 2)
            {
                DcLogRedo redo = SysjService.GetDclogRedo(recid);
                syr = redo.Syr;
                sybh = redo.Wtbh;
                datalist = redo.DataList;
                syrq = redo.Syrq.Value;
            }
            MyCurve curve = new MyCurve();
            curve.DrawImage(Response, datalist, sybh, syr, syrq);
            Response.End();
        }
        [Authorize]
        public void GetRedos()
        {
            string uniqcode = Request["uniqcode"].GetSafeString();
            IList<DcLogRedo> rows = SysjService.GetDclogRedos(uniqcode);
            foreach (DcLogRedo row in rows)
                row.DataList = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Response.Write(jss.Serialize(rows));
            Response.End();
        }

        /// <summary>
        /// 获取拌合站标段
        /// </summary>
        public void GetBHZSection()
        {
            string r = "";
            try
            {
                //r = CQDK.GetSection();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            Response.Write(r);
            Response.End();
        }
        public void GetBHZ()
        {
            //这里要重新写过，通过线程轮训数据保存到本地，数据库实体类cs和xml已经建好了，还有dao文件没建立和配置
            //int pageindex = Request["page"].GetSafeInt(1);
            //int pagesize = Request["rows"].GetSafeInt(20);
            //int sectionid = Request["section"].GetSafeInt(0);
            //string r = CQDK.GetData("GETBHZDATA", 0, sectionid, pageindex, pagesize);
            //Response.Write(r);
            //Response.End();
        }

        #endregion
        #region 页面操作

        /// <summary>
        /// 备案登记
        /// </summary>
        [Authorize]
        public void SubMachineIcp()
        {
            int recid = Request["id"].GetSafeInt();
            string icp = Request["icp"].GetSafeString();
            string replyman = Request["replyman"].GetSafeString();
            string replyman2 = Request["handleman"].GetSafeString();
            string replycontext = Request["replycontext"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {

                IList<string> sqls = new List<string>();
                sqls.Add("update sb_ba set BeiAnStatus=1, BeiAnICP='" + icp + "',ReplyMan1='" + replyman + "',replyman2='" + replyman2 + "',ReplyContent1='" + replycontext + "',ReplyDate1='" + DateTime.Now + "' where recid=" + recid);
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
        /// 获取设备二维码
        /// </summary>
        public void ShowQRCode()
        {
            byte[] ret = null;
            try
            {
                string recid = Request["recid"].GetSafeString();
                string conten = "";
                string baid = "";
                string sql = "select QRcode,BaId from sb_ba where recid='" + recid + "'";
                IList<IDictionary<string, string>> data = CommonService.GetDataTable(sql);
                if (data.Count > 0)
                {
                    conten = data[0]["qrcode"].GetSafeString();
                    baid = data[0]["baid"].GetSafeString();
                    if (string.IsNullOrEmpty(conten))
                    {
                        string text = "http://118.178.120.218/syjg/sbcontent?recid=" + baid;// new JavaScriptSerializer().Serialize(list);

                        byte[] barimage = Barcode.GetBarcode2(text, 200, 200);
                        IList<string> sqls = new List<string>();
                        conten = barimage.EncodeBase64();

                        sql = "update sb_ba set QRcode='" + conten + "' where recid='" + recid + "'";
                        sqls.Add(sql);
                        CommonService.ExecTrans(sqls);
                    }
                }


                ret = conten.DecodeBase64Array();

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
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
        }


        public void GetSB_INFO()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string sql = "select * from sb_ba where baid='" + recid + "'";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                msg = new JavaScriptSerializer().Serialize(list);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";
                Response.Write(JsonFormat.GetRetString(ret, msg));
                Response.End();
            }
        }
        /// <summary>
        /// 备案经办审核
        /// </summary>
        [Authorize]
        public void subbahandle()
        {
            int recid = Request["id"].GetSafeInt();
            string replyman = Request["handleman"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {
                IList<string> sqls = new List<string>();
                sqls.Add("update sb_ba set ProgStatus=3,ReplyMan2='" + CurrentUser.UserName + "',ReplyMan2RealName='" + CurrentUser.RealName + "',replydate2='" + DateTime.Now + "' where recid=" + recid);
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
        public void RemoveMachineIcp()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            bool ret = false;
            try
            {

                IList<string> sqls = new List<string>();
                sqls.Add("update sb_ba set RevokeDate=getdate(), BeiAnStatus=2 where recid=" + recid);
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
        public void aprove()
        {
            int recid = Request["recid"].GetSafeInt();
            string type = Request["type"].GetSafeString("3");
            string zd = Request["zd"].GetSafeString("");
            string err = "";
            bool ret = false;
            try
            {
                if (zd != "")
                {
                    IList<string> sqls = new List<string>();
                    sqls.Add("update SB_ReportSBSY set " + zd + "=" + type + " where RECID=" + recid);
                    ret = CommonService.ExecTrans(sqls);
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
        public void SubSbspr()
        {
            int recid = Request["recid"].GetSafeInt();
            string readers = Request["readers"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {
                string spr = "";
                string[] arrreader = readers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string struser in arrreader)
                {
                    var q = from e in BD.Jcbg.Web.Remote.UserService.Users where e.USERNAME.Equals(struser, StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() > 0)
                    {
                        RemoteUserService.VUser vuser = q.First();
                        if (spr != "")
                            spr += ",";
                        spr += vuser.USERNAME;
                    }
                }
                IList<string> sqls = new List<string>();
                sqls.Add("update H_ZJZ set SBSPZH='" + spr + "' where RECID=" + recid);
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



        #endregion


        #region 通用函数
        private string GetSysjItem(DcLog sysj, string colname)
        {
            PropertyInfo[] fields = typeof(DcLog).GetProperties();
            var q = from e in fields where e.Name.Equals(colname.Trim(), StringComparison.OrdinalIgnoreCase) select e;
            if (q.Count() > 0)
                return q.First().GetValue(sysj, null).GetSafeString();
            return "";
        }
        #endregion

    }
}
