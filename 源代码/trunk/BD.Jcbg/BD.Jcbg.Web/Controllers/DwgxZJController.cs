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


namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 诸暨个性化控制器
    /// </summary>
    public class DwgxZJController : Controller
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

        private IDwgxZJService _dwgxZJService = null;
        private IDwgxZJService DwgxZJService
        {
            get
            {
                if (_dwgxZJService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dwgxZJService = webApplicationContext.GetObject("DwgxZJService") as IDwgxZJService;
                }
                return _dwgxZJService;
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

        #region 获取数据

        /// <summary>
        /// 企业信息变更类型选择
        /// </summary>
        /// <returns></returns>
        public ActionResult QYXXBGLXXZ()
        {
            return View();
        }


        /// <summary>
        /// 获取企业信息变更类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetQYXXBGLX()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select lxbh, lxmc from h_qyxxbg_lx where sfyx=1 order by xssx ";
                ret = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(ret);
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



        public ActionResult ShowGC()
        {

            string gcbh = Request["gcbh"].GetSafeString();

            string strlx = "";
            IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
            dt_zjgcs = CommonService.GetDataTable("select gclxbh from I_M_GC where gcbh='" + gcbh + "'");
            string gclxbh = "";
            if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                gclxbh = dt_zjgcs[0]["gclxbh"];

            if (gclxbh == "01")
                strlx = "N";
            else
                strlx = "S";


            DataInputParam.DataParam param = new DataInputParam.DataParam();

            param.zdzdtable = "ZDZD_TZ";
            param.t1_tablename = "I_M_GC";
            param.t1_pri = "GCBH";
            param.t1_title = "工程信息";
            if (gcbh != "")
                param.jydbh = gcbh;

            //param.fieldparam = "SB_UseReg,InstallID," + InstallID + "|SB_UseReg,BaID," + BaID + "|SB_UseReg,SBMC," + SBMC + "|SB_UseReg,CQBH," + CQBH + "|SB_UseReg,ProName," + ProName + "|SB_UseReg,ConsPermitNo," + ConsPermitNo + "|SB_UseReg,WeiBaoUnitMan," + WeiBaoUnitMan + "|SB_UseReg,WeiBaoUnitManTel," + WeiBaoUnitManTel;
            param.t2_tablename = "I_S_GC_SGDW|I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_TSDW|I_S_GC_FGC";
            ////主键
            param.t2_pri = "GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID";
            ////标题
            param.t2_title = "施工单位|建设单位|监理单位|勘察单位|设计单位|图审单位|单位工程";

            param.t3_tablename = "I_S_GC_SGDW|I_S_GC_SGRY||I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY";
            param.t3_title = "施工人员|建设人员|监理人员|勘察人员|设计人员";
            param.t3_pri = "GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID";

            //param.preproc = "data_input_check_Use|$SB_UseReg.CheckDate|$SB_UseReg.ADetectDate|$SB_UseReg.BDetectDate";

            param.js = "searchRYZS.js";

            param.lx = strlx;


            param.rownum = 2;
            param.view = true;
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            return RedirectToAction("Index", "DataInput", param);
        }

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


        public void gotoZAJ()
        {

            //SendDataByGET("http://101.37.84.226:10001/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            string token = RSAUtil.GetToken("jcgl", "", "88888");
            //Response.RedirectPermanent("http://47.97.22.69:10003/user/main/");
            string url = "http%3a%2f%2fwzlcq.jzyglxt.com%2fuser%2fmain";
            Response.Redirect("LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
        }
        public void gotoWG()
        {

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            string url = "http%3a%2f%2f47.97.22.69%3a10003%2fWzWgry%2findex";
            string token = RSAUtil.GetToken("zf", "1", "88888");
            Response.Redirect("http://47.97.22.69:10003/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
        }


        public void gotoZAJ1()
        {

            //SendDataByGO("http://101.37.84.226:10001/user/dologin?login_name=wzzz&login_pwd=888888", "http://101.37.84.226:10001/user/main");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";
            //SendDataByGET("http://101.37.84.226:10001/user/dologin?login_name=wzzz&login_pwd=888888");

            //Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.location = 'http://101.37.84.226:10001/user/main';</script>");
            // Response.End();
            string url = "http%3a%2f%2f101.37.84.226%3a20002%2fuser%2fmain";
            string token = RSAUtil.GetToken("wzzz", "1", "888888");
            Response.Redirect("http://101.37.84.226:20002/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("Index", "LoginJump", "ClientKey=" + token + "&ul=" + url);
        }

        // 信用评价
        public void gotoXYPJ()
        {

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            string url = "http%3a%2f%2f47.97.22.69%3a10005%2fhome%2findex";
            string token = RSAUtil.GetToken("tzadmin", "1", "88888");
            Response.Redirect("http://47.97.22.69:10005/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
        }

        // 检测监管
        public void gotoJCJG()
        {

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            string url = Server.UrlEncode("http://47.97.22.69:10006/user/main");
            //"http%3a%2f%2f47.97.22.69%3a10007%2fuser%2findex";

            string token = RSAUtil.GetToken("wzzz", "1", "888888");
            Response.Redirect("http://47.97.22.69:10006/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
        }

        // 五大员考勤
        public void gotoWDYKQ()
        {

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            string url = Server.UrlEncode("http://47.97.22.69:10007/user/main");
            //"http%3a%2f%2f47.97.22.69%3a10007%2fuser%2findex";

            string token = RSAUtil.GetToken("admin", "1", "88888");
            Response.Redirect("http://47.97.22.69:10007/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
        }


        public void GetGCCAMERA()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string gcbh = Request["gcbh"].GetSafeString();
                retdt = CommonService.GetDataTable("select * from dbo.I_S_GC_CAMERA where gcbh='" + gcbh + "'");
                for (int i = 0; i < retdt.Count; i++)
                {
                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", retdt[i]["name"]);
                    //di.Add("camera", CryptFun.Encode(retdt[i]["camera"]));
                    di.Add("camera", Convert.ToBase64String(Encoding.UTF8.GetBytes(retdt[i]["camera"])));

                    dt.Add(di);
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

        #region 图表首页接口

        public void GetStatistics()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {

                string sql = "";
                string where = "";
                where = " and 1=1";

                //if (CurrentUser.CompanyCode == "CP201402000001")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}

                sql = "select count(1) as num from I_M_GC  where zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + "";
                IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zjgcs = "0";
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zjgcs = dt_zjgcs[0]["num"];

                sql = "select sum(convert(numeric(18, 2),JZMJ)) as num from I_M_GC  where ISNUMERIC(JZMJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zmj = "0";
                double zmjd = 0;
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd = dt_zjgcs[0]["num"].GetSafeDouble(0);

                //sql = "select sum(convert(numeric(18, 2),SZDLMJ)) as num from I_M_GC  where ISNUMERIC(SZDLMJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + "";
                //dt_zjgcs = CommonService.GetDataTable(sql);
                //if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                //    zmjd += dt_zjgcs[0]["num"].GetSafeDouble(0);

                //sql = "select sum(convert(numeric(18, 2),SZQL)) as num from I_M_GC  where ISNUMERIC(SZQL)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + "";
                //dt_zjgcs = CommonService.GetDataTable(sql);
                //if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                //    zmjd += dt_zjgcs[0]["num"].GetSafeDouble(0);
                zmj = zmjd.ToString();

                string zzj = "0";
                sql = "select sum(convert(numeric(18, 2),GCZJ)) as num from I_M_GC  where ISNUMERIC(GCZJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zzj = dt_zjgcs[0]["num"];

                string jggc = "0";
                sql = "select sum(convert(numeric(18, 2),GCZJ)) as num from I_M_GC  where zt in( select bh from h_gczt where xssx>=12 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    jggc = dt_zjgcs[0]["num"];

                string zcry = "0";
                sql = "select count(1) as num from i_M_RY where SPTG=1";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zcry = dt_zjgcs[0]["num"];

                string zgry = "0";
                sql = "select count(1) as num  from i_M_RY where rybh in (select rybh from dbo.View_GC_RY_QYRYCK where zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + ")";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zgry = dt_zjgcs[0]["num"];

                string basb = "0";
                sql = "select count(1) as num from dbo.INFO_CQBA where (serialno is null or serialno='') ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    basb = dt_zjgcs[0]["num"];

                string zysb = "0";
                sql = "select count(1) as num from SB_ReportSBSY where state!=2  ";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zysb = dt_zjgcs[0]["num"];



                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("zjgcs", zjgcs);
                di.Add("zmj", zmj);
                di.Add("zzj", zzj);
                di.Add("jggc", jggc);
                di.Add("zcry", zcry);
                di.Add("zgry", zgry);
                di.Add("basb", basb);
                di.Add("zysb", zysb);
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 工程类型
        /// </summary>
        public void GetGCLX()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {

                string sql = "";
                string where = "";
                where = " and 1=1";


                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable("select lxmc,lxbh from H_GCLX");
                for (int i = 0; i < dtlx.Count; i++)
                {
                    string lxbh = dtlx[i]["lxbh"];
                    string lxmc = dtlx[i]["lxmc"];
                    string retsum = "0";
                    sql = "select count(1) as num from i_M_GC where  ','+ gclxbh + ',' like '%," + lxbh + ",%' and  zt in( select bh from h_gczt where xssx >=2 and xssx<12 )  " + where + "";
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];

                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", lxmc);
                    di.Add("value", retsum);
                    dt.Add(di);


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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 工程异常
        /// </summary>
        public void GetGCYC()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
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
        /// 近期工程
        /// </summary>
        public void GetJQGC()
        {
            string msg = "";
            bool code = true;

            //IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string dt = "";
            try
            {

                string sql = "";
                string where = "";
                where = " and 1=1";
                //if (CurrentUser.CompanyCode == "CP201707000004")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}

                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                int year = DateTime.Now.Year;
                string yeartext = "";
                string linetext = "";
                for (int i = year - 5; i <= year; i++)
                {
                    if (yeartext != "")
                    {
                        yeartext += ",";
                        linetext += ",";
                    }

                    string retsum = "0";
                    sql = "select count(1) as num from i_M_GC where  slrq>='" + i.ToString() + "-1-1' and slrq<'" + (i + 1).ToString() + "-1-1'   " + where + "";
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];
                    yeartext += i.ToString();
                    linetext += retsum;



                }
                dt = " {\"year\": [" + yeartext + "], \"line\": [{\"data\": [" + linetext + "],\"name\": \"报监工程\" }]}";

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, 0, dt));
                Response.End();
            }
        }


        /// <summary>
        /// 检测异常
        /// </summary>
        public void GetJCYC()
        {
            string msg = "";
            bool code = true;

            //IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string dt = "";
            try
            {
                dt = " {  \"noPass\": 0, \"catch\": 0}";


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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, 0, dt));
                Response.End();
            }
        }


        /// <summary>
        /// 在岗人员
        /// </summary>
        public void GetRYTJ()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string sql = "";
                string where = "";
                where = " and 1=1";
                //if (CurrentUser.CompanyCode == "CP201707000004")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}

                string zcry = "0";
                sql = "select count(1) as num from i_M_RY where SPTG=1";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    zcry = retdt[0]["num"];

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("name", "注册");
                di.Add("value", zcry);
                dt.Add(di);

                string zgry = "0";
                sql = "select count(1) as num  from i_M_RY where rybh in (select rybh from dbo.View_GC_RY_QYRYCK where zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + ")";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    zgry = retdt[0]["num"];

                IDictionary<string, string> di2 = new Dictionary<string, string>();
                di2.Add("name", "在岗");
                di2.Add("value", zgry);
                dt.Add(di2);

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
        /// 人员记分统计
        /// </summary>
        public void GetRYJFTJ()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string sql = "";
                string where = "";
                where = " and 1=1";

                //if (CurrentUser.CompanyCode == "CP201707000004")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}

                string jf = "0";
                sql = "select count(1) as num from dbo.I_S_RY_JF_List  where JFYEAR=" + DateTime.Now.Year.ToString() + " and RYState=0";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    jf = retdt[0]["num"];

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("name", "记分");
                di.Add("value", jf);
                dt.Add(di);

                string bl = "0";
                sql = "select count(1) as num from dbo.I_S_RY_JF_List  where JFYEAR=" + DateTime.Now.Year.ToString() + " and RYState=1";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    bl = retdt[0]["num"];

                IDictionary<string, string> di2 = new Dictionary<string, string>();
                di2.Add("name", "不良行为");
                di2.Add("value", bl);
                dt.Add(di2);


                string hmd = "0";
                sql = "select count(1) as num from dbo.I_S_RY_JF_List  where JFYEAR=" + DateTime.Now.Year.ToString() + " and RYState=2";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    hmd = retdt[0]["num"];

                IDictionary<string, string> di3 = new Dictionary<string, string>();
                di3.Add("name", "黑名单");
                di3.Add("value", hmd);
                dt.Add(di3);

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
        /// 上访人员
        /// </summary>
        public void GetSFRY()
        {
            string err = "";
            string ret = "";
            try
            {

                string url = "http://47.97.22.69:10003/wzwgry/GetGcRYTSTJ_YC";
                string sql = "select zjzbh from H_ZJZ ";
                if (CurrentUser.CompanyCode == "CP201707000004")
                {

                }
                else
                {
                    sql += " where MZJZBH like '%" + CurrentUser.CompanyCode + "%'";
                }
                IList<IDictionary<string, string>> zjzbhs = CommonService.GetDataTable(sql);
                StringBuilder zjzret = new StringBuilder();
                foreach (IDictionary<string, string> item in zjzbhs)
                {
                    if (zjzret.Length > 1)
                        zjzret.Append(",");
                    zjzret.Append(item["zjzbh"]);
                }
                if (zjzret.Length == 0)
                    zjzret.Append(CurrentUser.CompanyCode);

                url += "?zjzbh=" + zjzret.ToString() + "&WGPTBH=TZBG";
                ret = SendDataByGET(url);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"sgdwnum\":\"38\",\"zgcs\":\"64\",\"zjgcs\":\"58\",\"jzmj\":\"796865\",\"zcry\":\"17404\",\"zzry\":\"17402\",\"zgry\":\"267\",\"dqry\":\"267\",\"gczj\":\"154464\",\"yfje\":\"0\",\"sfje\":\"0\",\"ljffje\":\"0\"}]}";

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
                where = " and 1=1";
                //if (CurrentUser.CompanyCode == "CP201707000004")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}
                sql = "select count(1) as num ,gw from (select gw from dbo.View_GC_RY_QYRYCK where zt in( select bh from h_gczt where xssx >=2 and xssx<7 ) " + where + " ) a group by gw";
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable(sql);
                for (int i = 0; i < dtlx.Count; i++)
                {
                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", dtlx[i]["gw"]);
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
                string where = "";
                string gcmc = Request["gcmc"].GetSafeString();
                where = " and 1=1";
                //if (CurrentUser.CompanyCode == "CP201707000004")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}

                if (gcmc != "")
                {
                    where = " and gcmc like '%" + gcmc + "%'";
                }
                sql = "select gcmc,gcbh,gczb,gclxbh,gcdd from I_M_GC where gczb is not null and gczb !='' and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where;
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


        #endregion

        #region 务工人员接口


        public void GetTjlist()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            string ret = "";
            try
            {

                string url = "http://47.97.22.69:10003/wzwgry/GetTjlist_YC";
                string sql = "select zjzbh from H_ZJZ ";
                if (CurrentUser.CompanyCode == "CP201707000004")
                {

                }
                else
                {
                    sql += " where MZJZBH like '%" + CurrentUser.CompanyCode + "%'";
                }
                IList<IDictionary<string, string>> zjzbhs = CommonService.GetDataTable(sql);
                StringBuilder zjzret = new StringBuilder();
                foreach (IDictionary<string, string> item in zjzbhs)
                {
                    if (zjzret.Length > 1)
                        zjzret.Append(",");
                    zjzret.Append(item["zjzbh"]);
                }
                if (zjzret.Length == 0)
                    zjzret.Append(CurrentUser.CompanyCode);

                url += "?zjzbh=" + zjzret.ToString() + "&WGPTBH=TZBG";
                ret = SendDataByGET(url);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"sgdwnum\":\"38\",\"zgcs\":\"64\",\"zjgcs\":\"58\",\"jzmj\":\"796865\",\"zcry\":\"17404\",\"zzry\":\"17402\",\"zgry\":\"267\",\"dqry\":\"267\",\"gczj\":\"154464\",\"yfje\":\"0\",\"sfje\":\"0\",\"ljffje\":\"0\"}]}";

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

        public void GetGC_QYFBTJ()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            string ret = "";
            try
            {

                string url = "http://47.97.22.69:10003/wzwgry/GetGC_QYFBTJ_YC";
                string sql = "select zjzbh from H_ZJZ ";
                if (CurrentUser.CompanyCode == "CP201707000004")
                {

                }
                else
                {
                    sql += " where MZJZBH like '%" + CurrentUser.CompanyCode + "%'";
                }
                IList<IDictionary<string, string>> zjzbhs = CommonService.GetDataTable(sql);
                StringBuilder zjzret = new StringBuilder();
                foreach (IDictionary<string, string> item in zjzbhs)
                {
                    if (zjzret.Length > 1)
                        zjzret.Append(",");
                    zjzret.Append(item["zjzbh"]);
                }
                if (zjzret.Length == 0)
                    zjzret.Append(CurrentUser.CompanyCode);

                url += "?zjzbh=" + zjzret.ToString() + "&WGPTBH=TZBG";
                ret = SendDataByGET(url);
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


        public void GetStatisticsGz()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            string ret = "";
            try
            {

                string url = "http://47.97.22.69:10003/wzwgry/GetStatisticsGz_YC";
                string sql = "select zjzbh from H_ZJZ ";
                if (CurrentUser.CompanyCode == "CP201707000004")
                {

                }
                else
                {
                    sql += " where MZJZBH like '%" + CurrentUser.CompanyCode + "%'";
                }
                IList<IDictionary<string, string>> zjzbhs = CommonService.GetDataTable(sql);
                StringBuilder zjzret = new StringBuilder();
                foreach (IDictionary<string, string> item in zjzbhs)
                {
                    if (zjzret.Length > 1)
                        zjzret.Append(",");
                    zjzret.Append(item["zjzbh"]);
                }
                if (zjzret.Length == 0)
                    zjzret.Append(CurrentUser.CompanyCode);

                url += "?zjzbh=" + zjzret.ToString() + "&WGPTBH=TZBG";
                ret = SendDataByGET(url);
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


        public void GetGcList()
        {
            string gcmc = Request["gcmc"].GetSafeString();
            string err = "";
            string ret = "";
            try
            {

                string url = "http://47.97.22.69:10003/wzwgry/GetGcList_YC";
                string sql = "select zjzbh from H_ZJZ ";
                if (CurrentUser.CompanyCode == "CP201707000004")
                {

                }
                else
                {
                    sql += " where MZJZBH like '%" + CurrentUser.CompanyCode + "%'";
                }
                IList<IDictionary<string, string>> zjzbhs = CommonService.GetDataTable(sql);
                StringBuilder zjzret = new StringBuilder();
                foreach (IDictionary<string, string> item in zjzbhs)
                {
                    if (zjzret.Length > 1)
                        zjzret.Append(",");
                    zjzret.Append(item["zjzbh"]);
                }
                if (zjzret.Length == 0)
                    zjzret.Append(CurrentUser.CompanyCode);

                url += "?zjzbh=" + zjzret.ToString() + "&WGPTBH=TZBG&gcmc=" + gcmc;
                ret = SendDataByGET(url);
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

        #region 节能审查汇总
        /// <summary>
        /// 导入节能审查汇总资料
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ImportJnschz()
        {
            return View();
        }

        public void DoImportJnschz()
        {
            bool ret = true;
            string msg = "";
            try
            {
                if (Request.Files.Count == 0)
                {
                    ret = false;
                    msg = "文件不能为空！";
                }
                else
                {

                    HttpPostedFileBase postfile = Request.Files[0];

                    // 允许的扩展名
                    List<string> extensions = new List<string>() { ".xls", ".xlsx" };
                    string filename = postfile.FileName;
                    string ext = System.IO.Path.GetExtension(postfile.FileName).GetSafeString().ToLower();
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
                                dt = GetDataFromExcel(postfile.InputStream);
                                break;
                            case ".xlsx":
                                dt = GetDataFromExcel(postfile.InputStream, true);
                                break;
                            default:
                                break;
                        }
                        if (dt.Count > 0)
                        {
                            List<string> lsql = new List<string>();
                            foreach (var row in dt)
                            {
                                string sql = string.Format(
                                    @"insert into jdbg_jnsc_hzjl
                                    (ZSH,GCMC,JSDW,LXRXM,LXFS,DZ,SJDW,PGJG,JSGW,BZ) values 
                                    ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                    row["zsh"], row["gcmc"], row["jsdw"], row["lxrxm"], row["lxfs"],
                                    row["dz"], row["sjdw"], row["pgjg"], row["jsgw"], row["bz"]);
                                lsql.Add(sql);
                            }
                            if (lsql.Count > 0)
                            {
                                ret = CommonService.ExecTrans(lsql);
                            }
                            if (!ret)
                            {
                                msg = "导入数据失败！";
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
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

        private List<Dictionary<string, string>> GetDataFromExcel(Stream input, bool isExcel2007 = false)
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
                    for (int j = 2; j <= sheet.LastRowNum; j++)
                    {
                        IRow row = sheet.GetRow(j);
                        if (row != null)
                        {
                            string xh = row.GetCell(0).GetSafeString().Trim();
                            string zsh = row.GetCell(1).GetSafeString().Trim();
                            string gcmc = row.GetCell(2).GetSafeString().Trim();
                            if (zsh != "" && gcmc != "")
                            {
                                string jsdw = row.GetCell(3).GetSafeString().Trim();
                                string lxrxm = row.GetCell(4).GetSafeString().Trim();
                                string lxfs = row.GetCell(5).GetSafeString().Trim();
                                string dz = row.GetCell(6).GetSafeString().Trim();
                                string sjdw = row.GetCell(7).GetSafeString().Trim();
                                string pgjg = row.GetCell(8).GetSafeString().Trim();
                                string jsgw = row.GetCell(9).GetSafeString().Trim();
                                string bz = row.GetCell(10).GetSafeString().Trim();
                                Dictionary<string, string> d = new Dictionary<string, string>();
                                d.Add("zsh", zsh);
                                d.Add("gcmc", gcmc);
                                d.Add("jsdw", jsdw);
                                d.Add("lxrxm", lxrxm);
                                d.Add("lxfs", lxfs);
                                d.Add("dz", dz);
                                d.Add("sjdw", sjdw);
                                d.Add("pgjg", pgjg);
                                d.Add("jsgw", jsgw);
                                d.Add("bz", bz);
                                dt.Add(d);
                            }
                        }
                    }
                }
            }

            return dt;
        }

        #endregion

        #region 大屏统计

        /// <summary>
        /// 获取工程相关的统计信息
        /// </summary>
        public void GetGcxxtj()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> info = new Dictionary<string, object>();

            try
            {
                // 所有工程数量
                double allcount = 106;
                // 总面积
                double allmj = 356.5;
                // 总资金
                double allzj = 1273.4;
                // 在监工程
                double jdcount = 41;
                // 在建工程
                double zjcount = 20;
                //本地企业数
                double localqy = 11;
                // 本地企业参建工程数
                double localqygc = 31;
                // 外地企业数
                double nonlocalqy = 6;
                // 外地企业参建工程数
                double nonlocalqygc = 4;

                info.Add("allcount", allcount);
                info.Add("allmj", allmj);
                info.Add("allzj", allzj);
                info.Add("jdcount", jdcount);
                info.Add("zjcount", zjcount);
                info.Add("localqy", localqy);
                info.Add("localqygc", localqygc);
                info.Add("nonlocalqy", nonlocalqy);
                info.Add("nonlocalqygc", nonlocalqygc);

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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(info)));
                Response.End();
            }
        }
        private List<Dictionary<string, object>> FormatV(string v)
        {
            List<Dictionary<string, object>> d = new List<Dictionary<string, object>>();
            d.Add(new Dictionary<string, object>() {
                { "value", v}
            });
            return d;
        }
        /// <summary>
        /// 务工人员工种分布
        /// </summary>
        public void GetWgryGzTJ()
        {
            bool ret = true;
            string msg = "";
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                data.Add(new Dictionary<string, object>() {
                    { "name", "管理人员" },
                    { "value", 97 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "木工" },
                    { "value", 225 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "钢筋工" },
                    { "value", 143 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "砼工" },
                    { "value", 53 }
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "砌筑工" },
                    { "value", 45 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "架子工" },
                    { "value", 73 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "特种工" },
                    { "value", 8 }
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "水电安装" },
                    { "value", 43 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "专业分包" },
                    { "value", 6 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "其他工种" },
                    { "value", 12 }
                });
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }
        /// <summary>
        /// 务工人员实名制工程数
        /// </summary>
        public void GetWgryGcTJ()
        {
            bool ret = true;
            string msg = "";
            double v = 0;
            try
            {
                v = 0.18;
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, v));
                Response.End();
            }

        }

        /// <summary>
        /// 年工程数量统计
        /// </summary>
        public void GetYearGc()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {

                string sql = "";
                string where = "";
                where = " and 1=1";

                IList<IDictionary<string, object>> retdt = new List<IDictionary<string, object>>();
                int year = DateTime.Now.Year;
                for (int i = year - 5; i <= year; i++)
                {
                    sql = "select count(1) as num from i_M_GC where  slrq>='" + i.ToString() + "-1-1' and slrq<'" + (i + 1).ToString() + "-1-1'   " + where + "";
                    retdt = CommonService.GetDataTable2(sql);
                    if (null != retdt && retdt.Count != 0)
                    {
                        dt.Add(new Dictionary<string, object>() {
                            { "year", i},
                            { "count", retdt[0]["num"]}
                        });
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 检测报告异常
        /// </summary>
        public void GetJCBGYC()
        {
            string msg = "";
            bool code = true;

            //IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string dt = "";
            try
            {
                dt = " {  \"noPass\": 0.6, \"catch\": 0.5}";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, dt));
                Response.End();
            }
        }



        /// <summary>
        /// 不合格工程数量
        /// </summary>
        public void GetBHGGc()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {

                dt.Add(new Dictionary<string, object>() {
                    { "type", "房建"},
                    { "value", 32},
                });
                dt.Add(new Dictionary<string, object>() {
                    { "type", "市政公用工程"},
                    { "value", 54}
                });
                dt.Add(new Dictionary<string, object>() {
                    { "type", "园林绿化"},
                    { "value", 45}
                });
                dt.Add(new Dictionary<string, object>() {
                    { "type", "燃气管道"},
                    { "value", 12}
                });

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
        /// 资金统计
        /// </summary>
        public void GetZjTJ()
        {
            string msg = "";
            bool code = true;
            Dictionary<string, object> info = new Dictionary<string, object>();
            try
            {
                //已建专用账户工程数
                info.Add("zyzhgcs", 0.832);

                //累计资金到位
                IList<IDictionary<string, object>> ljzjdw = new List<IDictionary<string, object>>();
                ljzjdw.Add(new Dictionary<string, object>() {
                    { "text", "总造价"},
                    { "value", 1},
                });
                ljzjdw.Add(new Dictionary<string, object>() {
                    { "text", "应到"},
                    { "value", 0.8}
                });
                ljzjdw.Add(new Dictionary<string, object>() {
                    { "text", "实到"},
                    { "value", 0.5}
                });
                ljzjdw.Add(new Dictionary<string, object>() {
                    { "text", "实际发放"},
                    { "value", 0.452}
                });
                info.Add("ljzjdw", ljzjdw);
                //工资拖欠统计
                IList<IDictionary<string, object>> gztqtj = new List<IDictionary<string, object>>();
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "1月"},
                    { "y", 200},
                    { "z", -10},
                });
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "2月"},
                    { "y", 235.5},
                    { "z", 0},
                });
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "3月"},
                    { "y", 156.3},
                    { "z", -33.6},
                });
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "4月"},
                    { "y", 123.5},
                    { "z", -21},
                });
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "5月"},
                    { "y", 423.5},
                    { "z", 42.9},
                });
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "6月"},
                    { "y", 456.5},
                    { "z", 7.8},
                });
                gztqtj.Add(new Dictionary<string, object>() {
                    { "x", "7月"},
                    { "y", 356.5},
                    { "z", -21.9},
                });
                info.Add("gztqtj", gztqtj);

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

        /// <summary>
        /// 物资统计
        /// </summary>
        public void GetWZTJ()
        {
            string msg = "";
            bool code = true;
            Dictionary<string, object> info = new Dictionary<string, object>();
            try
            {
                //危险源数
                IList<IDictionary<string, object>> wxysl = new List<IDictionary<string, object>>();
                wxysl.Add(new Dictionary<string, object>() {
                    { "x", "塔机"},
                    { "y", 235},
                    { "z", 100},
                });
                wxysl.Add(new Dictionary<string, object>() {
                    { "x", "升降机"},
                    { "y", 156},
                    { "z", 76},
                });
                wxysl.Add(new Dictionary<string, object>() {
                    { "x", "物料提升机"},
                    { "y", 123},
                    { "z", 68},
                });
                info.Add("wxysl", wxysl);

                //设备数量
                IList<IDictionary<string, object>> sbsl = new List<IDictionary<string, object>>();
                sbsl.Add(new Dictionary<string, object>() {
                    { "x", "监控机"},
                    { "y", 75},
                    { "z", 68},
                });
                sbsl.Add(new Dictionary<string, object>() {
                    { "x", "考勤机"},
                    { "y", 60},
                    { "z", 43},
                });
                sbsl.Add(new Dictionary<string, object>() {
                    { "x", "扬尘设备"},
                    { "y", 54},
                    { "z", 25},
                });

                info.Add("sbsl", sbsl);

                //总体设备统计
                IList<IDictionary<string, object>> ztsbtj = new List<IDictionary<string, object>>();
                ztsbtj.Add(new Dictionary<string, object>() {
                    { "x", "监控设备"},
                    { "y", 375},
                    { "r", 40},
                });
                ztsbtj.Add(new Dictionary<string, object>() {
                    { "x", "考勤机"},
                    { "y", 200},
                    { "r", 55},
                });
                ztsbtj.Add(new Dictionary<string, object>() {
                    { "x", "扬尘设备"},
                    { "y", 125},
                    { "r", 20},
                });
                ztsbtj.Add(new Dictionary<string, object>() {
                    { "x", "塔机"},
                    { "y", 190},
                    { "r", 20},
                });
                ztsbtj.Add(new Dictionary<string, object>() {
                    { "x", "物料提升机"},
                    { "y", 190},
                    { "r", 20},
                });
                ztsbtj.Add(new Dictionary<string, object>() {
                    { "x", "升降机"},
                    { "y", 90},
                    { "r", 33},
                });
                info.Add("ztsbtj", ztsbtj);

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

        /// <summary>
        /// 工地情况统计
        /// </summary>
        public void GetGZQKTJ()
        {
            string msg = "";
            bool code = true;
            Dictionary<string, object> info = new Dictionary<string, object>();
            try
            {
                //各阶段工程统计
                IList<IDictionary<string, object>> gjdgctj = new List<IDictionary<string, object>>();
                gjdgctj.Add(new Dictionary<string, object>() {
                    { "value",  314529403.31},
                    { "content", "基础阶段"},
                });
                gjdgctj.Add(new Dictionary<string, object>() {
                    { "value",  64329403.31},
                    { "content", "主体阶段"},
                });
                gjdgctj.Add(new Dictionary<string, object>() {
                    { "value",  34529403},
                    { "content", "装饰阶段"},
                });
                gjdgctj.Add(new Dictionary<string, object>() {
                    { "value",  354529403.31},
                    { "content", "今年竣工"},
                });

                info.Add("gjdgctj", gjdgctj);

                //在线设备
                info.Add("zxsbsl", 0.832);

                //设备维保和检查记录统计
                IList<IDictionary<string, object>> sbwbjc = new List<IDictionary<string, object>>();
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "考勤机"},
                    { "y", 300},
                    { "s", "1"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "考勤机"},
                    { "y", 180},
                    { "s", "2"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "吊机"},
                    { "y", 200},
                    { "s", "1"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "吊机"},
                    { "y", 100},
                    { "s", "2"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "起重机"},
                    { "y", 65},
                    { "s", "1"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "起重机"},
                    { "y", 175},
                    { "s", "2"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "塔吊"},
                    { "y", 190},
                    { "s", "1"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "塔吊"},
                    { "y", 110},
                    { "s", "2"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "摄像头"},
                    { "y", 200},
                    { "s", "1"},
                });
                sbwbjc.Add(new Dictionary<string, object>() {
                    { "x", "摄像头"},
                    { "y", 280},
                    { "s", "2"},
                });

                info.Add("sbwbjc", sbwbjc);

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

        /// <summary>
        /// 获取地图-工程分布统计
        /// </summary>
        public void GetMAPGCFBTJ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select recid as doit,lat,lng,value,info,type,name from h_gcfb_tj";
                data = CommonService.GetDataTable2(sql);
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  0},
                //    { "lat", 31.8998},
                //    { "lng", 102.2212},
                //    { "value", 8},
                //    { "info", "恒大珺悦府"},
                //    { "type", "zj"},
                //    { "name", "点0"},
                //    { "rotationAngle", 45},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  1},
                //    { "lat", 29.706026},
                //    { "lng", 120.290348},
                //    { "value", 21},
                //    { "info", "华鸿东城雅郡"},
                //    { "type", "zj"},
                //    { "name", "点1"},
                //    { "rotationAngle", 45},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  2},
                //    { "lat", 29.648225},
                //    { "lng", 120.279209},
                //    { "value",4},
                //    { "info", "徐水经济开发区哈弗城三期项目"},
                //    { "type", "tg"},
                //    { "name", "点2"},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  3},
                //    { "lat", 29.678291},
                //    { "lng", 120.184995},
                //    { "value", 13},
                //    { "info", "华鸿国樾府"},
                //    { "type", "zg"},
                //    { "name", "点3"},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  4},
                //    { "lat", 29.814258},
                //    { "lng", 120.319525},
                //    { "value", 18},
                //    { "info", "巨鼎红郡"},
                //    { "type", "yc"},
                //    { "name", "点4"},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  5},
                //    { "lat", 29.903463},
                //    { "lng", 120.257434},
                //    { "value", 38},
                //    { "info", "描述信息5"},
                //    { "type", "zj"},
                //    { "name", "点5"},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  6},
                //    { "lat", 29.81877},
                //    { "lng", 120.157974},
                //    { "value", 39},
                //    { "info", "描述信息6"},
                //    { "type", "zj"},
                //    { "name", "点6"},

                //});
                //data.Add(new Dictionary<string, object>() {
                //    { "dotid",  7},
                //    { "lat", 29.712594},
                //    { "lng", 120.247087},
                //    { "value", 41},
                //    { "info", "诸暨唐韵广场"},
                //    { "type", "zj"},
                //    { "name", "点7"},

                //});

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
        /// 工程统计分布类型
        /// </summary>
        public void GetMAPTJFBLX()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select id,type,name from h_map_tjfblx where sfyx=1 order by xssx";
                data = CommonService.GetDataTable2(sql);
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
        /// 工程统计分布类型-子类型
        /// </summary>
        public void GetMAPTJFBLXZLX()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {

                string type = Request["type"].GetSafeString();
                string sql = string.Format("select lxbh,mc from h_map_tjfblx_zlx where sfyx=1 and type='{0}' order by xssx", type);
                data = CommonService.GetDataTable2(sql);
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


        public void GetMAPFBTJ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string lx = Request["lx"].GetSafeString();
                string sql = string.Format("select recid as doit,lat,lng,value,info,type,name,gcbh from h_fbtj where lx='{0}'", lx);
                data = CommonService.GetDataTable2(sql);

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

        public void GetGDSPByGcbh()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string where = " 1=1 and sfyx=1 ";
                if (gcbh != "")
                {
                    where += " and gcbh='" + gcbh + "'";
                }
                string sql = "select * from i_s_gc_video_channel where " + where;
                data = CommonService.GetDataTable2(sql);

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


        public void GetGDGC_QYFBTJ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                data.Add(new Dictionary<string, object>() {
                    { "name","店口镇"},
                    { "value",1},
                });
                data.Add(new Dictionary<string, object>() {
                    { "name","暨阳街道"},
                    { "value",4},
                });
                data.Add(new Dictionary<string, object>() {
                    { "name","陶朱街道"},
                    { "value",1},
                });

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

        public void GetGcInfoList()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                data.Add(new Dictionary<string, object>() {
                    { "detail","人员调岗审批（[2017-032]学府水岸小区一标段21#整体，15#，16#，20#，22#-25#）"}
                });
                data.Add(new Dictionary<string, object>() {
                    { "detail","大唐镇馨园住宅小区工程进入施工确认阶段"}
                });
                data.Add(new Dictionary<string, object>() {
                    { "detail","新入册木工200名"}
                });
                data.Add(new Dictionary<string, object>() {
                    { "detail","大唐镇馨园住宅小区已完成"}
                });
                data.Add(new Dictionary<string, object>() {
                    { "detail","浙江鼎晟休闲用品有限公司厂房工程经过监督站审批"}
                });

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
        // 设备数量统计
        public void GetSbSLTJ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string proc = "GetSbSLTJ()";
                data = CommonService.ExecDataTableProc(proc, out msg);


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

        // 重大危险源和当年巡查次数
        public void GetWxyAndXCCS()
        {
            string msg = "";
            bool code = true;
            IDictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                string proc = "GetWxyAndXCCS()";
                IList<IDictionary<string, string>> infolist = CommonService.ExecDataTableProc(proc, out msg);
                if (infolist.Count > 0)
                {
                    data = infolist[0];
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

        // 获取设计企业年终评分
        public void GetSJDWNZPF()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string proc = "GetSJDWNZPF()";
                data = CommonService.ExecDataTableProc(proc, out msg);

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
        // 获取企业年终评分
        public void GetSGDWNZPF()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string proc = "GetSGDWNZPF()";
                data = CommonService.ExecDataTableProc(proc, out msg);

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

        // 获取务工人员工种统计V2
        public void GetWgryGzTJV2()
        {
            bool ret = true;
            string msg = "";
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                //string url = "http://120.27.218.55:8001/welcome/GetZFWGRYGZ_V2";
                //string postdata = "";
                data.Add(new Dictionary<string, object>() {
                    { "name", "管理人员" },
                    { "value", 1414 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "木工" },
                    { "value", 5913 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "钢筋工" },
                    { "value", 3278 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "砼工" },
                    { "value", 1601 }
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "粉刷工" },
                    { "value", 2156 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "砌筑工" },
                    { "value", 2218 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "架子工" },
                    { "value", 1985 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "特种工" },
                    { "value", 456 }
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "水电安装" },
                    { "value", 1434 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "专业分包" },
                    { "value", 834 },
                });
                data.Add(new Dictionary<string, object>() {
                    { "name", "其他工种" },
                    { "value", 1471 }
                });
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }


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


        #region 企业文件共享
        public ActionResult viewqywjgxxq()
        {
            string serial = Request["serial"].GetSafeString();
            string wjnr = Request["wjnr"].GetSafeString();
            ViewBag.serial = serial;
            ViewBag.wjnr = wjnr;
            return View();
        }

        public void getwjgxlist()
        {

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string msg = "";
            bool code = true;
            try
            {
                string serial = Request["serial"].GetSafeString();
                string procstr = string.Format("GetWJGX('{0}')", serial);
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


        public ActionResult viewgxwj()
        {
            string serial = Request["serial"].GetSafeString();
            string wjnr = Request["wjnr"].GetSafeString();
            string qybh = Request["qybh"].GetSafeString();
            ViewBag.serial = serial;
            ViewBag.wjnr = wjnr;
            ViewBag.qybh = qybh;
            return View();
        }

        public void WJGXFileView()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int fileid = DataFormat.GetSafeInt(Request["id"]);
            string qybh = Request["qybh"].GetSafeString();
            string serial = Request["serial"].GetSafeString();
            try
            {
                // 更新下载状态
                string sql = string.Format("update jdbg_qywjgx_reader set isdownload=1 where qybh='{0}' and workserial='{1}'", qybh, serial);
                CommonService.Execsql(sql);
                // 获取文件
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
        #endregion

        #region 获取工地视频通道
        public void GetGcVideoChannel()
        {
            string gcbh = Request["gcbh"].GetSafeString();

        }
        #endregion

        #region 企业统计信息汇总
        public ActionResult tjxxhz()
        {
            return View();
        }

        public void loadtjxxhz()
        {
            string msg = "";
            bool code = true;

            string tjjg = "";
            try
            {
                string startny = Request["startny"].GetSafeString();
                string endny = Request["endny"].GetSafeString();
                if (startny == "" || endny == "")
                {
                    code = false;
                    msg = "开始年月与结束年月不能为空！";
                }
                else
                {
                    string proc = string.Format("Gettjxxhz('{0}','{1}')", startny, endny);
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    dt = CommonService.ExecDataTableProc(proc, out msg);
                    if (dt.Count > 0)
                    {
                        tjjg = dt[0]["tjjg"];
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(tjjg)));
                Response.End();
            }
        }

        public void savetjxxhz()
        {
            string msg = "";
            bool code = true;

            try
            {
                string startny = Request["startny"].GetSafeString();
                string endny = Request["endny"].GetSafeString();
                string tjjg = Request["tjjg"].GetSafeString();
                if (startny == "" || endny == "")
                {
                    code = false;
                    msg = "开始年月与结束年月不能为空！";
                }
                else
                {
                    string proc = string.Format("Savetjxxhz('{0}','{1}','{2}')", startny, endny, tjjg);
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
                    string depcode = Request["depcode"].GetSafeString(); ;
                    string xb = Request["xb"].GetSafeString();
                    string sjhm = Request["sjhm"].GetSafeString();
                    string rolecodelist = Request["rolecodelist"].GetSafeString();
                    List<string> rl = rolecodelist.Split(new char[] { ',' }).ToList();
                    List<string> defrl = new List<string>();
                    string sql = string.Format("select * from h_nbry_defrole where cpcode='{0}' and depcode='{1}'", cpcode, depcode);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        foreach (var item in dt)
                        {
                            defrl.Add(item["rolecode"].GetSafeString());
                        }
                    }
                    foreach (var item in defrl)
                    {
                        if (!rl.Contains(item))
                        {
                            rl.Add(item);
                        }
                    }
                    rolecodelist = string.Join(",", rl.ToArray());


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
                    string depcode = Request["depcode"].GetSafeString();
                    string postdm = "";
                    string xb = Request["xb"].GetSafeString();
                    string sjhm = Request["sjhm"].GetSafeString();

                    string rolecodelist = Request["rolecodelist"].GetSafeString();
                    List<string> rl = rolecodelist.Split(new char[] { ',' }).ToList();
                    // 质监站
                    if (cpcode == "CP201809000001")
                    {
                        // 普通员工
                        List<string> defrl = new List<string>()
                        {
                            "14006961813357568"
                        };
                        foreach (var item in defrl)
                        {
                            if (!rl.Contains(item))
                            {
                                rl.Add(item);
                            }
                        }
                    }
                    rolecodelist = string.Join(",", rl.ToArray());

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
                string dates = "method=User&opt=AddUser&check=0&username=" + username + "&realname=" + realname + "&sfzh=" + sfzh + "&password=" + password + "&cpcode=" + cpcode + "&depcode=" + depcode + "&rolecodelist=" + rolecodelist + "&postdm=" + postdm + "&timestring=" + timestring + "&sign=" + sign;
                ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                    string sql = "INSERT INTO I_M_NBRY([ZH],[ZJZBH],[RYBH],[RYXM],[XB],[SFZHM],[SJHM],[SFYX],[DEPARTMENTID]) VALUES('" + username + "','" + cpcode + "','" + param["usercode"] + "','" + realname + "','" + xb + "','" + sfzh + "','" + sjhm + "',1,'" + depcode + "')";
                    CommonService.ExecSql(sql, out err);
                    string zhlx = "";
                    if (cpcode == "CP201402000001")
                    {
                        zhlx = "N";
                    }
                    else if (cpcode == "CP201809000001")
                    {
                        zhlx = "P";
                    }

                    sql = "insert into i_m_qyzh (qybh, yhzh,sfqyzzh, lrsj, zhlx) " +
                        " values ('{0}','{1}',0, getdate(),'{2}') ";
                    sql = string.Format(sql, param["usercode"], param["usercode"], zhlx);
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
                    string sql = "update I_M_NBRY set zh='" + username + "',zjzbh='" + cpcode + "',ryxm='" + realname + "',sjhm='" + sjhm + "',xb='" + xb + "', departmentid='" + depcode + "' where rybh='" + usercode + "'";
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
                    sql += " and (a.ryxm  like '%" + realname + "%' or a.zh like '%" + realname + "%')";
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
                if (CurrentUser.CompanyCode == "CP201402000001")
                    sql = "select * from h_zjz where 1=1 order by xssx asc";
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

        [Authorize]
        public void GetDepListByCpCode()
        {
            bool ret = true;
            string msg = "";
            List<object> list = new List<object>();
            try
            {
                string cpcode = Request["cpcode"].GetSafeString();
                var deplist = Remote.UserService.GetDepListByCpCode(cpcode);
                foreach (var item in deplist)
                {
                    list.Add(new { depcode = item.DEPCODE, depname = item.DEPNAME });
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(list)));
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
            ViewBag.cpcode = CurrentUser.CurUser.CompanyId;
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
            string cpcode = Request["cpcode"].GetSafeString();
            string sql = "select zh,zjzbh,rybh,ryxm,xb,sfzhm,sjhm,departmentid from dbo.I_M_NBRY where rybh='" + usercode + "'";
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
                ViewBag.depcode = dt[i]["departmentid"];
            }

            if (usercode == "")
            {
                ViewBag.usercode = usercode;
                ViewBag.cpcode = cpcode;
                ViewBag.depcode = "";
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

                if (sfzh == "zf0011" || checktype == 1)
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
                string s = string.Format("{0}{1}{2}", sfzhm, time, md5key + md5key);
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
                if (retdata != null)
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
                SysLog4.WriteError(loginurl + "?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);
                Response.Redirect(loginurl + "?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

        }
        #endregion

        /// <summary>
        /// 传感器地图界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult cgqshow()
        {
            return View();
        }


        /// <summary>
        /// 扬尘首页
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult DIndex(string deviceCode)
        {
            ViewData["DeviceCode"] = deviceCode;
            return View();
        }

        public ActionResult GetSensorList()
        {
            string sql = "SELECT SensorCode,SensorName FROM dbo.H_Sensor ORDER BY OrderBy";
            IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
            return Content(JsonConvert.SerializeObject(list));
        }

        public ActionResult GetDustInfo(FormCollection form)
        {
            string json, title = "扬尘设备实时数据", legend = "", xAxis = "";
            decimal minLimit = 0, maxLimit = 0;
            var sensors = new List<List<string>>();
            try
            {
                string deviceCode = form.Get("deviceCode");
                string dustType = form.Get("dustType");
                DateTime startTime = DateTime.Parse(form.Get("startTime"));
                DateTime endTime = DateTime.Parse(form.Get("endTime"));
                /*string deviceCode = "16057716", dustType = "1803";
                DateTime startTime = DateTime.Parse("2018-09-01 00:00:00"), endTime = DateTime.Parse("2018-09-20 00:00:00");*/
                //超标传感器数据列表
                string sql = "SELECT DeviceCode,DustType,DustName,DustIndex,DustValue,CurrentTime FROM View_Tj_I_M_Dust WHERE DeviceCode='" + deviceCode + "' AND DustType='" + dustType + "' AND CONVERT(NVARCHAR(10),CurrentTime,120)>='" + startTime + "' AND CONVERT(NVARCHAR(10),CurrentTime,120)<='" + endTime + "' ORDER BY DustIndex ASC,CurrentTime DESC";
                IList<IDictionary<string, string>> datas = CommonService.GetDataTable(sql);
                //超标传感器序号列表
                sql = "SELECT DustIndex FROM dbo.I_M_Dust WHERE DustType='" + dustType + "' GROUP BY DustIndex";
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                if (datas.Count > 0 && list.Count > 0)
                {
                    int j = 0;
                    var dustIndex = "";
                    //用来存储不同传感器序号的数据
                    var sensor = new List<string>();
                    for (int i = 0; i < datas.Count; i++)
                    {
                        if (dustIndex == "")
                        {
                            dustIndex = datas[i]["dustindex"];
                            legend += "\"" + datas[i]["dustname"] + j + "\",";
                        }
                        else if (dustIndex != datas[i]["dustindex"])
                        {
                            dustIndex = datas[i]["dustindex"];
                            j++;
                            legend += "\"" + datas[i]["dustname"] + j + "\",";
                            sensors.Add(sensor);
                            sensor = new List<string>();
                        }
                        //同一个传感器，日期只需要执行一次即可
                        if (j == 0 && dustIndex == datas[i]["dustindex"])
                        {
                            xAxis += "\"" + datas[i]["currenttime"] + "\",";
                        }
                        sensor.Add(datas[i]["dustvalue"]);
                    }
                    //最后一次需要添加到list中
                    sensors.Add(sensor);
                    sql = "SELECT * FROM View_Tj_Dust_Limit_List where DeviceCode='" + deviceCode + "' and SensorCode='" + dustType + "'";
                    IList<IDictionary<string, string>> limit = CommonService.GetDataTable(sql);
                    if (limit.Count > 0)
                    {
                        minLimit = decimal.Parse(limit[0]["minlimit"]);
                        maxLimit = decimal.Parse(limit[0]["maxlimit"]);
                    }
                    json = "{\"Code\":\"success\",\"Msg\":\"\",";
                }
                else
                {
                    json = "{\"Code\":\"failure\",\"Msg\":\"当前没有数据\",";
                }
            }
            catch (Exception exception)
            {
                json = "{\"Code\":\"error\",\"Msg\":\"" + exception.Message + "\",";
            }
            json += "\"Datas\":{\"title\":\"" + title + "\",\"legend\":[" + legend.Trim(',') + "],\"xAxis\":[" + xAxis.Trim(',') + "],\"sensors\":" + JsonConvert.SerializeObject(sensors) + ",\"minLimit\":" + minLimit + ",\"maxLimit\":" + maxLimit + "}}";
            return Content(json);
        }


        #region 统计界面筛选

        /// <summary>
        /// 获取省列表
        /// </summary>
        public void GetProvinceList()
        {
            try
            {
                string sql = "select distinct szsf,sfid from h_city where szsf='浙江省' ";

                sql += " order by sfid";
                IList<IDictionary<string, string>> plist = CommonService.GetDataTable(sql);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(plist));
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 获取市列表
        /// </summary>
        public void GetCityList()
        {
            string province = Request["province"];
            try
            {


                string sql = "select distinct szcs from h_city where szsf='" + province + "' and  szcs='绍兴市'";
                //sql += where;
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(list));
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 获取辖区列
        /// </summary>
        public void GetXQList()
        {
            string province = Request["province"];
            string city = Request["city"];
            try
            {
                string where = "";


                string sql = "select distinct szxq from h_city where szsf='" + province + "'and szcs='" + city + "'" + " and szxq='诸暨市'";
                sql += where;
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(list));
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 获取街道列表
        /// </summary>
        public void GetJDList()
        {
            string province = Request["province"];
            string city = Request["city"];
            string xq = Request["xq"];
            try
            {
                string where = "";


                string sql = "select distinct szjd from h_city where szsf='" + province + "'and szcs='" + city + "' and szxq='" + xq + "'";
                sql += where;
                IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(list));
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 获取工程类型
        /// </summary>
        public void GetGCXZ()
        {
            try
            {
                string where = "";
                //string sql_jd = "select gclx as lxmc from View_H_ZFZH_GCLX where usercode='" + CurrentUser.UserName + "'";
                //IList<IDictionary<string, string>> gclxlist = CommonService.GetDataTable(sql_jd);
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string sql = "select distinct lxmc from h_gclx";
                list = CommonService.GetDataTable(sql);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(list));
            }
            catch (Exception e)
            { }
        }
        /*
        /// <summary>
        /// 根据施工企业编号获取工程列表
        /// </summary>
        [Authorize]
        public void GetGcList_ByQybh()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string qybh = Request["qybh"].GetSafeString();

                string where = " where 1=1";
                if (CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("wfztbmdm"))) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where a.yhzh=  '" + CurrentUser.UserCode + "' and sfqyzzh=1";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    if (dtt.Count > 0)
                        qybh = dtt[0]["qybh"].ToString();
                    else
                    {
                        datajson.Msg = "找不到企业信息";
                        datajson.Status = "failure";
                    }
                }

                if (qybh != "")
                    where += " and a.sgdwbh='" + qybh + "'";

                string sql = "select gcmc,gcbh ";
                sql += " from View_I_M_GC_ZS a ";
                sql += where;
                datajson.Datas = CommonService.GetDataTable(sql);
                datajson.Status = "success";
                datajson.Msg = "";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
                datajson.Msg = msg;
                datajson.Status = "failure";
            }
            finally
            {

                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(jss.Serialize(datajson));
                Response.End();
            }
        }

        */
        /// <summary>
        /// 获取下拉框施工企业一览
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetSelectQyList()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();
                string qylx = Request["qylx"].GetSafeString();
                string qybh = "";
                string where = "";
                /*
                string where = " where b.QYBH is not null";
                if (province != "")
                    where += " and a.szsf='" + province + "'";
                if (city != "")
                    where += " and a.szcs='" + city + "'";
                if (district != "")
                    where += " and a.szxq='" + district + "'";
                if (jd != "")
                    where += " and a.szjd='" + jd + "'";
                if (qylx != "")
                    where += " and b.lxbh='" + qylx + "'";
                
                if (CurrentUser.CurUser.DepartmentId.Equals(Configs.GetConfigItem("wfztbmdm"))) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where a.yhzh=  '" + CurrentUser.UserCode + "' and sfqyzzh=1";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    if (dtt.Count > 0)
                        qybh = dtt[0]["qybh"].ToString();

                }
                if (qybh != "")
                    where += " and b.qybh='" + qybh + "'";

                string sql = "select distinct b.QYBH,b.QYMC from( ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,b.QYBH from I_M_GC A left join I_S_GC_SGDW b on b.gcbh=A.gcbh"; //(case when a.gcbh_yc is null or a.gcbh_yc='' then a.gcbh else a.gcbh_yc end)
                sql += ") a ";
                sql += "left join I_M_QY b on a.QYBH = b.QYBH ";
                sql += where;*/
                if (district != "")
                    where += " and SZXQ='" + district + "'";
                if (jd != "")
                    where += " and SZJD='" + jd + "'";
                //企业编号怎么写
                if (qybh != "")
                    where += " and gcbh in(select gcbh from dbo.View_GC_QY where qybh='" + qybh + "')";

                //string sql = "select distinct QYBH,QYMC from dbo.View_GC_QY where gcbh in(select gcbh from i_m_gc where zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + ")";
                string sql = "select QYBH,QYMC from I_M_QY";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 传感器地图工程
        /// </summary>
        public void GetCGProjectMap()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();
                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();
                string gcxz = Request["gcxz"].GetSafeString();
                string key = Request["key"].GetSafeString();

                string sql = "";
                string where = " and 1=1 ";
                string gcmc = Request["gcmc"].GetSafeString();
                //if (CurrentUser.CompanyCode == "CP201707000004")
                //{
                //    where = " and 1=1";
                //}
                //else
                //{
                //    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                //}

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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";

                sql = "SELECT GCBH, GCMC, GCZB, DustType, SensorName, Total, Number FROM View_Tj_Dust_Map where gcbh in(select gcbh from View_I_M_GC1 where gczb is not null and gczb !=''  and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + " )";
                dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    if (ret != "")
                        ret += ",";
                    ret += " { \"name\": \"" + dt[i]["gcmc"].Replace("\"", "‘") + "\", \"position\": [" + dt[i]["gczb"] + "], \"status\": " + dt[i]["total"] + ",\"gcbh\": \"" + dt[i]["gcbh"] + "\",\"sensorname\": \"" + dt[i]["sensorname"] + "\",\"dusttype\": \"" + dt[i]["dusttype"] + "\",\"number\": \"" + dt[i]["number"] + "\" }";
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



        #endregion

        #region 视频中心
        public void GetVideoSearchType()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select type, name from h_gc_video_searchtype where sfyx=1 order by xssx";
                dt = CommonService.GetDataTable2(sql);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }

        }

        public void GetVideoTreeBySearchType()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = Request["type"].GetSafeString();
                if (type != "")
                {
                    if (type == "gc")
                    {
                        string sql = "select gcbh,max(gcmc) as gcmc, count(*) as total from i_s_gc_video_channel where sfyx=1 " +
                                 "group by gcbh order by gcbh";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["gcmc"]);
                            node.Add("total", row["total"]);
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);
                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from view_i_s_gc_video_channel where sfyx=1 and gcbh='{0}'", row["gcbh"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["channelname"]);
                                cnode.Add("type", type);
                                cnode.Add("data", cdrow);
                                children.Add(cnode);
                            }
                            item.Add("children", children);
                            dt.Add(item);
                        }
                    }
                    else if (type == "camera")
                    {
                        string sql = "select cameratype,max(cameratypename) as cameratypename, count(*) as total from view_i_s_gc_video_channel " +
                                 "group by cameratype order by cameratype desc";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["cameratypename"]);
                            node.Add("total", row["total"]);
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);
                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from view_i_s_gc_video_channel where cameratype='{0}'", row["cameratype"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["channelname"]);
                                cnode.Add("type", type);
                                cnode.Add("data", cdrow);
                                children.Add(cnode);
                            }
                            item.Add("children", children);
                            dt.Add(item);
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
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        // 获取企业相关的工地视频
        [Authorize]
        public void QYGetVideoTree()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = "gc";
                string sql = "select gcbh,max(gcmc) as gcmc, count(*) as total from i_s_gc_video_channel where sfyx=1 ";
                sql += string.Format(" and gcbh in (select distinct gcbh from view_i_s_gc_allqy where qybh in (select qybh from i_m_qyzh where yhzh='{0}'))", CurrentUser.UserName);
                sql += " group by gcbh order by gcbh";
                IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                foreach (var row in d)
                {
                    IDictionary<string, object> item = new Dictionary<string, object>();
                    // 生成父节点
                    IDictionary<string, object> node = new Dictionary<string, object>();
                    node.Add("name", row["gcmc"]);
                    node.Add("total", row["total"]);
                    node.Add("type", type);
                    node.Add("data", row);
                    item.Add("node", node);
                    // 生成子节点
                    IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                    sql = string.Format("select * from view_i_s_gc_video_channel where  sfyx=1 and gcbh='{0}'", row["gcbh"].GetSafeString());
                    IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                    foreach (var cdrow in cd)
                    {
                        IDictionary<string, object> cnode = new Dictionary<string, object>();
                        cnode.Add("name", cdrow["channelname"]);
                        cnode.Add("type", type);
                        cnode.Add("data", cdrow);
                        children.Add(cnode);
                    }
                    item.Add("children", children);
                    dt.Add(item);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }


        // 获取人员相关的工地视频
        [Authorize]
        public void RYGetVideoTree()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = "gc";
                string sql = "select gcbh,max(gcmc) as gcmc, count(*) as total from i_s_gc_video_channel where sfyx=1 ";
                sql += string.Format(" and gcbh in (select distinct gcbh from view_i_s_gc_allry where rybh in (select qybh from i_m_qyzh where yhzh='{0}'))", CurrentUser.UserName);
                sql += " group by gcbh order by gcbh";
                IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                foreach (var row in d)
                {
                    IDictionary<string, object> item = new Dictionary<string, object>();
                    // 生成父节点
                    IDictionary<string, object> node = new Dictionary<string, object>();
                    node.Add("name", row["gcmc"]);
                    node.Add("total", row["total"]);
                    node.Add("type", type);
                    node.Add("data", row);
                    item.Add("node", node);
                    // 生成子节点
                    IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                    sql = string.Format("select * from view_i_s_gc_video_channel where  sfyx=1 and gcbh='{0}'", row["gcbh"].GetSafeString());
                    IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                    foreach (var cdrow in cd)
                    {
                        IDictionary<string, object> cnode = new Dictionary<string, object>();
                        cnode.Add("name", cdrow["channelname"]);
                        cnode.Add("type", type);
                        cnode.Add("data", cdrow);
                        children.Add(cnode);
                    }
                    item.Add("children", children);
                    dt.Add(item);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }


        // 获取安全监督员相关的工地视频
        [Authorize]
        public void JDYGetVideoTree()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = "gc";
                string sql = "select gcbh,max(gcmc) as gcmc, count(*) as total from i_s_gc_video_channel where sfyx=1 ";
                sql += string.Format(" and gcbh in (select distinct gcbh from i_m_gc where aqjdy='{0}')", CurrentUser.UserName);
                sql += " group by gcbh order by gcbh";
                IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                foreach (var row in d)
                {
                    IDictionary<string, object> item = new Dictionary<string, object>();
                    // 生成父节点
                    IDictionary<string, object> node = new Dictionary<string, object>();
                    node.Add("name", row["gcmc"]);
                    node.Add("total", row["total"]);
                    node.Add("type", type);
                    node.Add("data", row);
                    item.Add("node", node);
                    // 生成子节点
                    IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                    sql = string.Format("select * from view_i_s_gc_video_channel where  sfyx=1 and gcbh='{0}'", row["gcbh"].GetSafeString());
                    IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                    foreach (var cdrow in cd)
                    {
                        IDictionary<string, object> cnode = new Dictionary<string, object>();
                        cnode.Add("name", cdrow["channelname"]);
                        cnode.Add("type", type);
                        cnode.Add("data", cdrow);
                        children.Add(cnode);
                    }
                    item.Add("children", children);
                    dt.Add(item);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        #endregion

        #region 大屏统计
        public void GetGCTJInfo()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string procstr = "GetGCTJInfo()";
                dt = CommonService.ExecDataTableProc(procstr, out msg);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        public void GetZJGCFB()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string procstr = "GetZJGCFB()";
                dt = CommonService.ExecDataTableProc(procstr, out msg);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        public void GetPageProjectList()
        {

            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            string key = Request["key"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, object>> datas = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (key != "")
                {
                    strwhere += " and ( zjdjh like '" + key + "' or gcmc like '%" + key + "%' )";
                }
                string sql = " from view_i_m_gc where gczt='在建' " + strwhere;
                sql = "select gcbh,gcmc,gczb,zjdjh " + sql;
                datas = CommonService.GetPageData2(sql, pagesize, pageindex, out totalcount);
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
        /// 获取所有标注经纬度的工程
        /// </summary>
        public void GetALLZBProjectList()
        {

            string key = Request["key"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, object>> datas = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = " and (gczb is not null and gczb<>'') ";
                if (key != "")
                {
                    strwhere += " and ( zjdjh like '" + key + "' or gcmc like '%" + key + "%' )";
                }
                string sql = " from view_i_m_gc where gczt='在建' " + strwhere;
                sql = "select gcbh,gcmc,gczb,zjdjh ,sy_jsdwmc, sgdwmc, jldwmc,kcdwmc,sjdwmc " + sql;
                datas = CommonService.GetDataTable2(sql);
                totalcount = datas.Count;
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

        public void GetALLProjectListBySZJD()
        {

            string szjd = Request["szjd"].GetSafeString();
            int totalcount = 0;
            IList<IDictionary<string, object>> datas = new List<IDictionary<string, object>>();
            try
            {
                string strwhere = "";
                if (szjd != "")
                {
                    strwhere += " and ( szjd='" + szjd + "' and szsf='浙江省' and szcs='绍兴市' and szxq='诸暨市')";
                }
                string sql = " from view_i_m_gc where gczt='在建' " + strwhere;
                sql = "select gcbh,gcmc,gczb,zjdjh " + sql;
                datas = CommonService.GetDataTable2(sql);
                totalcount = datas.Count;
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

        #region 质安监
        #region 工程相关资料
        //删除质量监督方案
        public void DelZljdfa()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id == "")
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from ReportJDJH where recid={0}", id);
                    ret = CommonService.ExecSql(sql, out msg);
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
        //删除安全监督方案
        public void Delaqjdfa()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id == "")
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from ReportJDJHAJ where recid={0}", id);
                    ret = CommonService.ExecSql(sql, out msg);
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
        //删除监督记录
        public void Deljdjl()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id == "")
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from ProjectJDJL where recid={0}", id);
                    ret = CommonService.ExecSql(sql, out msg);
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

        //删除重大危险源
        public void Delzdwxy()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id == "")
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    List<string> lsql = new List<string>();
                    string sql = string.Format("delete from ProjectWXYM where recid={0}", id);
                    lsql.Add(sql);
                    sql = string.Format("delete from ProjectWXYS where parentid={0}", id);
                    lsql.Add(sql);
                    CommonService.ExecTrans(lsql);
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

        #endregion

        #region 企业填报

        #endregion

        #region 设备使用登记

        //删除产权备案
        public void delcqba()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id == "")
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from INFO_CQBA where recid={0}", id);
                    ret = CommonService.ExecSql(sql, out msg);
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
        /// <summary>
        /// 删除产权单位
        /// </summary>
        public void delcqdw()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id == "")
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from INFO_CQDW where recid={0}", id);
                    ret = CommonService.ExecSql(sql, out msg);
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
        #endregion

        #region 手机接口

        // 获取危险源列表
        public void GetWXYList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");
                    int wxyzt = Request["zt"].GetSafeInt(0);
                    string strSql = "select a.* from view_gclr_wxy a where 1=1 ";
                    if (wxyzt == 1)//申报但未实施
                    {
                        strSql += " and  a.State in (0,2)";
                    }
                    else if (wxyzt == 2)//已完成销项
                    {
                        strSql += " and  a.State in (1)";
                    }
                    else //进行或申报销项
                    {
                        strSql += " and  a.State in (3,4)";
                    }

                    if (jdzch != "")
                    {
                        strSql += " and a.jdzch like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and a.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and a.jsdw like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and a.sgdw like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and a.jldw like '%" + jldw + "%'";
                    }

                    string tem = "select zjdjh from i_m_gc where zjdjh=a.jdzch";

                    strSql += " and (jdzch in (" + tem + ") or a.jdzch='' or a.jdzch is null)";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);


                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 获取危险源详情列表
        public void GetWXYDetail()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int wxyid = Request["id"].GetSafeInt(0);
                    string strSql = string.Format("select a.* from view_gclr_wxy a where recid={0} ", wxyid);
                    dt = CommonService.GetDataTable2(strSql);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 危险源销项确认
        public void EndWXY()
        {
            bool ret = true;
            string msg = "";
            bool code = true;

            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int wxyid = Request["id"].GetSafeInt(0);

                    DateTime enddate = Request["enddate"].GetSafeDate(DateTime.Now);
                    string strSql = string.Format("update gclr_wxy set state=1, enddate='{0}' where recid={1} ", enddate.ToString("yyyy-MM-dd"), wxyid);
                    ret = CommonService.ExecSql(strSql, out msg);
                }
                else
                {
                    ret = false;
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
        // 标准、法规文件列表
        public void GetLawFileList()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string strSql = "select lawid,categoryid,lawcontent,lawdesc,owner,createdon,createdby,displayorder from laws order by displayorder ";
                    IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(strSql);
                    foreach (var item in ddt)
                    {
                        string name = item["lawcontent"].GetSafeString();
                        string value = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/dwgxzj/GetLawFile?id=" + item["lawid"].GetSafeString();
                        dt.Add(new Dictionary<string, object>() {
                            { "name", name},
                            { "value", value},
                        });
                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 获取标准、法规文件
        public void GetLawFile()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int lawid = DataFormat.GetSafeInt(Request["id"]);
            try
            {
                string sql = string.Format("select lawcontent,filecontent from laws where lawid={0}", lawid);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (dt.Count > 0)
                {
                    ret = (byte[])dt[0]["filecontent"];
                    filesize = ret.Length;
                    filename = dt[0]["lawcontent"].GetSafeString();
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

        // 获取起重机械设备列表
        public void GetQZJSBList()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            int total = 0;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string temBABH = Request["BABH"].GetSafeString("");
                    string temSBMC = Request["SBMC"].GetSafeString("0");
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string strSql = "select * from VIEW_SB_ReportSBSY where 1=1 ";
                    if (temBABH != "")
                    {
                        strSql += " and CQBH like '%" + temBABH + "%'";
                    }
                    if (temSBMC != "0")
                    {
                        strSql += " and SBMC like '%" + temSBMC + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and GCMC like '%" + gcmc + "%'";
                    }

                    if (jdzch != "")
                    {
                        strSql += " and jdzch like '%" + jdzch + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and sgdw like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and jldw like '%" + jldw + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and jdzch in (select zjdjh from view_i_m_gc_lb where sy_jsdwmc like '%" + jsdw + "%') ";
                    }
                    strSql += " order by recid desc";
                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);
                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":\"{2}\",\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 保存曝光台
        public void SavePG()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string body = "";
                    string title = Request["title"].GetSafeString("");
                    string imageName = "";
                    // 保存article
                    NewsArtcle article = new NewsArtcle();
                    article.Categoryid = 101;
                    article.Templateid = 3;
                    article.ArticleTitle = title;
                    article.ArticleKey = "";
                    article.ArticleFrom = "本站原创";
                    article.ArticleDate = DateTime.Now;
                    article.ArticleContent = body;
                    article.IsRecommand = false;
                    article.IsImage = true;
                    article.IsLink = false;
                    article.IsAudited = false;
                    article.CreatedOn = DateTime.Now;
                    article.CreatedBy = CurrentUser.UserName;
                    article.Hits = 0;
                    article.ImageUrl = imageName;

                    article = NewsArtcleService.Save(article);




                    body += "<p><font size='5'><strong>我站于" + DateTime.Now.ToLongDateString() + "对" + title + "工程进行监督检查，发现存在安全隐患较多，现通报如下</strong>：</font></p>";
                    HttpFileCollectionBase files = Request.Files;

                    string editor = "<p><font size='5'><strong>我站于" + DateTime.Now.ToLongDateString() + "对" + title + "工程进行监督检查，发现存在安全隐患较多，现通报如下</strong>：</font></p>";

                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            string tmpName = "";

                            bool upload = SaveUserFile(article.Articleid, files[i], ".*", 20480, out tmpName);
                            if (upload)
                            {
                                string temms = Request.Form["ms" + i.ToString()].GetSafeString("");

                                if (Request.Form[files[i].FileName].GetSafeString("") != "")
                                    temms = Request.Form[files[i].FileName].GetSafeString("");
                                else
                                    temms = Request.Form["ms" + i.ToString()].GetSafeString("");
                                //这里还没写完，做到兼容新老，万一哪个二笔没更新也在用
                                body += "<p align='center'><font size='3'>" + temms + "</font></p><p align='center'><img alt='' width='480' height='289' src='/dwgxzj/getAttachFile?id=" + tmpName + "' /></p>";
                                editor += "<p align='center'><font size='3'>" + temms + "</font></p><p align='center'><img alt='' width='480' height='289' src='/xwwzUser/getAttachFile?aid={fileid" + i.ToString() + "}' /></p>";
                                if (i == 0)
                                {
                                    imageName = tmpName;
                                }
                            }
                        }

                    }

                    article.ArticleContent = body;
                    article.ImageUrl = imageName;

                    NewsArtcleService.Update(article);

                    // 同步该曝光信息到新闻网站
                    Dictionary<string, string> pgtdatas = new Dictionary<string, string>() {
                        {"islink","" },
                        {"articlelink","" },
                        {"isimage","on" },
                        {"isimportant","" },
                        {"articletitle",title},
                        {"categoryid","101" },
                        {"articledate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        {"articlekey","" },
                        {"articlefrom","本站原创" },
                        {"context",editor.EncodeBase64() },
                        {"id","" },
                        {"type","add" },
                        {"isfile","" },
                        { "createdby", CurrentUser.UserName}
                    };

                    SyncPGToXWWZ(pgtdatas, GetPGFiles(Request.Files), out msg);
                    SysLog4.WriteError("同步pgt返回信息: " + msg);




                    ret = true;
                    msg = "";

                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }
        // 保存新闻附件
        public bool SaveUserFile(int articleid, HttpPostedFileBase file, string strAllowExt, int maxSize, out string msg)
        {
            bool ret = false;
            msg = "";
            string strSaveName = "";

            do
            {
                // 判断是否有文件
                if (file.ContentLength == 0)
                {
                    msg = "上传文件为空！";
                    break;
                }
                string tmpName = "";
                string tmpExt = "";
                strSaveName = file.FileName;
                if (strSaveName.LastIndexOf("\\") > -1)
                    strSaveName = strSaveName.Substring(strSaveName.LastIndexOf("\\") + 1);

                if (strSaveName.IndexOf(".") > 0)
                {
                    tmpName = strSaveName.Substring(0, strSaveName.LastIndexOf('.'));
                    tmpExt = strSaveName.Substring(strSaveName.LastIndexOf('.'), strSaveName.Length - strSaveName.LastIndexOf('.'));
                }
                else
                {
                    tmpName = file.FileName;
                }
                // 判断扩展名是否符合
                string[] allowExtArr = strAllowExt.Split(new char[] { '/' });
                bool find = false;
                for (int i = 0; i < allowExtArr.Length; i++)
                {
                    if (allowExtArr[i].ToLower() == tmpExt.ToLower() || allowExtArr[i] == ".*")
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    msg = "文件格式不正确！";
                    break;
                }
                // 判断文件大小是否符合,maxSize in kbytes
                //if (file.ContentLength > maxSize * 1024)
                //{
                //    msg = "文件大小超出了限制！";
                //    break;
                //}

                // 保存文件
                try
                {
                    byte[] postcontent = new byte[file.ContentLength];
                    int readlength = 0;
                    while (readlength < file.ContentLength)
                    {
                        int tmplen = file.InputStream.Read(postcontent, readlength, file.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    NewsAttach att = new NewsAttach();
                    att.Articleid = articleid;
                    att.DocName = strSaveName;
                    att.SaveName = strSaveName;
                    att.Filecontent = postcontent;
                    att = NewsAttachService.Save(att);
                    msg = att.Attachid.ToString();
                    ret = true;
                }
                catch
                {
                    msg = "保存文件失败！";
                }

            } while (false);

            return ret;
        }
        //获取新闻附件
        public void GetAttachFile()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int attachid = DataFormat.GetSafeInt(Request["id"]);
            try
            {
                string sql = string.Format("select docname,filecontent from news_attach where attachid={0}", attachid);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (dt.Count > 0)
                {
                    ret = (byte[])dt[0]["filecontent"];
                    filesize = ret.Length;
                    filename = dt[0]["docname"].GetSafeString();
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
        // 现场签到
        public void Sign()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string work = Request["work"].GetSafeString("现场签到");
                    string[] location = Request["location"].GetSafeString("").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (location.Length > 1)
                    {
                        string sql = string.Format("insert into U_UserLocation(Latitude,Longitude,Markdate,Work,UserCode,Realname) values ('{0}','{1}','{2}','{3}','{4}','{5}')"
                            , location[0].GetSafeDouble(0)
                            , location[1].GetSafeDouble(0)
                            , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            , work
                            , CurrentUser.UserName
                            , CurrentUser.RealName
                            );
                        ret = CommonService.ExecSql(sql, out msg);
                    }
                    else
                    {
                        ret = false;
                        msg = "签到失败，请确认设置（网络状态，GPS是否开启）。请稍后重试";
                    }

                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }
        // 获取企业工程列表
        public void QYGetProjectList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    //SysLog4.WriteError(CurrentUser.UserName);
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");

                    string strSql = "select gcbh,zjdjh,gcmc,sy_jsdwmc  from view_i_m_gc a where 1=1 and zt not in('LR','JDBG','GDZL')";

                    strSql += " and ( " +
                                " charindex(','+'" + CurrentUser.UserName + "'+',' , ','+ a.sgdwzh +',') > 0 " +
                                " or " +
                                " charindex(','+'" + CurrentUser.UserName + "'+',' , ','+ a.jldwzh +',') > 0 " +
                                ")";

                    if (jdzch != "")
                    {
                        strSql += " and a.zjdjh like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and a.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and a.sy_jsdwmc like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and a.sgdwmc like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and a.jldwmc like '%" + jldw + "%'";
                    }


                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);


                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 获取企业工程详情
        public void QYGetProjectDetail()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "[]";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string gcbh = Request["id"].GetSafeString();
                    string sql = "select zjdjh,gcmc,jgxs,cs,gczj,gcdd,jzfl,jzmj,sy_jsdwmc,sgdwmc,kcdwmc,jldwmc,sjdwmc,fgc,jlryxx, zjry,sgryxx,fbsgry,sjdwxmfzrxm,kcdwxmfzrxm,jdzcrq,jdgcsxm,tjjdyxm,azjdyxm,aqjdyxm " +
                        "from view_i_m_gc WHERE gcbh = '" + gcbh + "'";

                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        IDictionary<string, object> info = dt[0];
                        data = "[";
                        data += "{ \"name\": \"监督注册号\", \"value\":\"" + info["zjdjh"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"工程名称\", \"value\":\"" + info["gcmc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"建构/层次\", \"value\":\"" + info["jgxs"].GetSafeString() + "/" + info["cs"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"工程类别\", \"value\":\"" + info["jzfl"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"建设规模\", \"value\":\"" + info["jzmj"].GetSafeString() + "平方米\"},";
                        data += "{ \"name\": \"工程造价\", \"value\":\"" + info["gczj"].GetSafeString() + "万元\"},";
                        data += "{ \"name\": \"工程地点\", \"value\":\"" + info["gcdd"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"报监时间\", \"value\":\"" + info["jdzcrq"].GetSafeString() + "\"},";

                        string jdyname = "";
                        string strName2 = info["jdgcsxm"].GetSafeString();
                        if (strName2 != "")
                            jdyname = strName2;
                        strName2 = info["tjjdyxm"].GetSafeString();
                        if (strName2 != "")
                            jdyname += "," + strName2;
                        strName2 = info["azjdyxm"].GetSafeString();
                        if (strName2 != "")
                            jdyname += "," + strName2;
                        strName2 = info["aqjdyxm"].GetSafeString();
                        if (strName2 != "")
                            jdyname += "," + strName2;

                        data += "{ \"name\": \"监督员\", \"value\":\"" + jdyname + "\"},";
                        data += "{ \"name\": \"建设单位\", \"value\":\"" + info["sy_jsdwmc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"施工单位\", \"value\":\"" + info["sgdwmc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"监理单位\", \"value\":\"" + info["jldwmc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"勘察单位\", \"value\":\"" + info["kcdwmc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"勘察负责人\", \"value\":\"" + info["kcdwxmfzrxm"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"设计单位\", \"value\":\"" + info["sjdwmc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"设计负责人\", \"value\":\"" + info["sjdwxmfzrxm"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"分工程\", \"value\":\"" + info["fgc"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"监理人员\", \"value\":\"" + info["jlryxx"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"自监人员\", \"value\":\"" + info["zjry"].GetSafeString() + "\"},";
                        data += "{ \"name\": \"施工人员\", \"value\":\"" + info["sgryxx"].GetSafeString() + "\"},";


                        data += "{ \"name\": \"分包单位施工人员\", \"value\":\"" + info["fbsgry"] + "\"}";
                        data += "]";
                    }
                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\":{2}}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }

        // 获取企业设备列表
        public void QYGetSBList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string babh = Request["BABH"].GetSafeString("");
                    string sbmc = Request["SBMC"].GetSafeString("0");
                    string gcmc = Request["GCMC"].GetSafeString("");
                    int pageIndex = Request["page"].GetSafeInt(1);

                    string strSql = "select * from VIEW_SB_ReportSBSY where 1=1 ";

                    strSql += " and ( " +
                            "( AZDW='" + CurrentUser.RealName + "')" + "or " +
                            "( SGDW='" + CurrentUser.RealName + "')" + "or " +
                            "( JLDW='" + CurrentUser.RealName + "')" + "or " +
                            "(jdzch in (select zjdjh from view_i_m_gc_lb where sgdwmc='" + CurrentUser.RealName + "' ))" + "or " +
                            "(jdzch in (select zjdjh from view_i_m_gc_lb where jldwmc='" + CurrentUser.RealName + "' ))" +
                            " )";

                    if (babh != "")
                    {
                        strSql += " and CQBH like '%" + babh + "%'";
                    }
                    if (sbmc != "0")
                    {
                        strSql += " and SBMC like '%" + sbmc + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and GCMC like '%" + gcmc + "%'";
                    }
                    strSql += " order by gzrq desc";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                    if (dt.Count > 0)
                    {
                        foreach (var item in dt)
                        {
                            string sgdw = item["sgdw"].GetSafeString();
                            string jldw = item["jldw"].GetSafeString();
                            string jdzch = item["jdzch"].GetSafeString();
                            bool issg = (sgdw == CurrentUser.RealName) || CheckIsSG(CurrentUser.UserName, jdzch);
                            bool isjl = (jldw == CurrentUser.RealName) || CheckIsJL(CurrentUser.UserName, jdzch);
                            string context = "";
                            int state = item["state"].GetSafeInt();
                            string cxsgqr = item["sy_cxsgqr"].GetSafeString();
                            string cxjlqr = item["sy_cxjlqr"].GetSafeString();
                            string syjlqr = item["sy_sysgqr"].GetSafeString();
                            string sysgqr = item["sy_syjlqr"].GetSafeString();
                            if (issg)
                            {
                                context = "设备已撤销";
                                if (state == 1)
                                {
                                    context = "安装告知确认";
                                }
                                else if (state == 2)
                                {
                                    context = "等待监理单位确认";
                                }
                                else
                                {
                                    context = item["sy_context"].GetSafeString();
                                    if (context == "设备使用中")
                                    {
                                        if (cxjlqr != "")
                                        {
                                            context = "拆卸告知安监站审核中";

                                        }
                                        else if (cxsgqr != "")
                                        {
                                            context = "拆卸告知监理确认中";
                                        }
                                    }


                                }

                            }
                            else if (isjl)
                            {
                                context = "设备已撤销";
                                if (state == 1)
                                {
                                    context = "等待施工单位确认";
                                }
                                else if (state == 2)
                                {
                                    context = "设备安装告知中（点击办理确认）";
                                }
                                else
                                {
                                    context = item["sy_context"].GetSafeString();

                                    if (context == "安装告知完成")
                                    {

                                        if (syjlqr != "")
                                        {
                                            context = "使用登记安监站审核中";
                                        }
                                        else if (sysgqr != "")
                                        {
                                            context = "设备使用登记中（点击办理确认）";
                                        }
                                    }
                                    if (context == "设备使用中")
                                    {
                                        if (cxjlqr != "")
                                        {
                                            context = "拆卸告知安监站审核中";
                                        }
                                        else if (cxsgqr != "")
                                        {
                                            context = "设备拆卸告知（点击办理确认）";
                                        }
                                    }



                                }
                            }
                            else
                            {
                                context = "设备已撤销";
                                if (state == 1)
                                {
                                    context = "等待施工单位确认";
                                }
                                else if (state == 2)
                                {
                                    context = "等待监理单位确认";

                                }
                                else
                                {
                                    context = item["sy_context"].GetSafeString();
                                    if (context == "安监站审核中")
                                    {
                                        context = "安监站审核中,点击修改";
                                    }
                                    if (context == "设备使用中")
                                    {
                                        context = "设备使用中，点击办理设备拆卸";
                                    }
                                }
                            }

                            item["sy_context"] = context;
                        }
                    }
                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 获取企业危险源列表
        public void QYGetWXYList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");
                    int wxyzt = Request["zt"].GetSafeInt(-1);
                    string strSql = "select a.* from view_gclr_wxy a where 1=1 ";

                    strSql += " and ( " +
                            "( a.sgdw='" + CurrentUser.RealName + "')" + " or " +
                            "( a.jldw='" + CurrentUser.RealName + "') " + " or " +
                            "(jdzch in (select zjdjh from view_i_m_gc_lb where sgdwmc='" + CurrentUser.RealName + "' ))" + "or " +
                            "(jdzch in (select zjdjh from view_i_m_gc_lb where jldwmc='" + CurrentUser.RealName + "' ))" +
                            " ) ";
                    if (wxyzt != -1)
                    {
                        if (wxyzt == 0)
                        {
                            strSql += "and State in (0,2)";
                        }
                        else
                        {
                            strSql += "and State=" + wxyzt;
                        }
                    }


                    if (jdzch != "")
                    {
                        strSql += " and a.jdzch like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and a.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and a.jsdw like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and a.sgdw like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and a.jldw like '%" + jldw + "%'";
                    }
                    strSql += " order by startdate desc,JDZCH,Title ";


                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);


                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 企业获取危险源详情
        public void QYWXYDetail()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int wxyid = Request["id"].GetSafeInt(0);
                    string sql = string.Format("select * from view_gclr_wxy where recid={0}", wxyid);
                    dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        IDictionary<string, object> info = dt[0];
                        string wxyform = "";
                        string sgdw = info["sgdw"].GetSafeString();
                        string jldw = info["jldw"].GetSafeString();
                        string wxy = info["wxy"].GetSafeString();
                        string jdzch = info["jdzch"].GetSafeString();
                        bool issg = (sgdw == CurrentUser.RealName) || CheckIsSG(CurrentUser.UserName, jdzch);
                        bool isjl = (jldw == CurrentUser.RealName) || CheckIsJL(CurrentUser.UserName, jdzch);
                        int state = info["state"].GetSafeInt();
                        if (state == 2)
                        {
                            if (issg)
                            {
                                string jsystring = "<div class='label-left'>";
                                string ss = "select ryxm,yhzh=(select yhzh from i_m_qyzh where qybh=a.rybh) from i_m_nbry a where a.departmentid='14006961804985345'";
                                IList<IDictionary<string, string>> jdylist = CommonService.GetDataTable(ss);
                                foreach (var jdy in jdylist)
                                {
                                    jsystring += "<input class='checkbox1' type='checkbox' name='tzjdy' value='" + jdy["yhzh"] + "' />" + jdy["ryxm"] + "<br/>";
                                }
                                jsystring += "</div>";
                                wxyform = "<font class='label-left'>开始时间：</font><object id='kssj' class='label-right' name='kssj' type='date' validate='required' validatemsg='开始时间不能为空！' ></object> <hr /><font class='label-left'>预计结束时间：</font><object id='jsrq' class='label-right' name='jsrq' type='date' validate='required' validatemsg='结束时间不能为空！' ></object> <hr /><font class='label-left'>安全监控责任人：</font> <input id='aqzrr' class='label-right' displaysteps='1' validate='required' validatemsg='安全责任人不能为空！' name='aqzrr' /> <hr /><font class='label-left'>责任人联系电话：</font> <input id='zrrdh' class='label-right' displaysteps='1' name='zrrdh' /> <hr /><font class='label-left'>通知监督员：</font>" + jsystring + " <hr /><font class='label-left'>控制措施：</font> <br /><textarea style='width: 100%' id='cs' class='label-right' rows='6' displaysteps='6' name='cs'></textarea> ";
                                if (wxy == "起重机械设备的安装、拆卸")
                                {
                                    wxyform = "<font class='label-left'>设备编号：</font> <input id='sbbh' class='label-right' displaysteps='1' validate='required' validatemsg='设备编号不能为空！' name='sbbh' /> <hr />" + wxyform;
                                }
                                wxyform += "<font class='label-left'>附件：</font><div class='label-right'><input type='button' value='添加附件' onClick='document.getElementById(\\\"$cameraid\\\").openCameraUpload(true)'/> <br/><photoupload id='$cameraid' name='$cameraid' pwidth='1200' savealbum='true' nums='10'/></div>";
                                wxyform += "<div><input type='button' id='submit' value='危险源实施' style='width:50%;align:center;' onclick='SubDate(\\\"wxyss\\\")'/></div>";
                            }
                        }
                        if (state == 3)
                        {
                            string kssj = info["sy_realdate"].GetSafeString();
                            string yjjssj = info["sy_enddate2"].GetSafeString();
                            string aqzrr = info["aqzrr"].GetSafeString("");
                            string zrrdh = info["zrrdh"].GetSafeString("");
                            string cs = info["cs"].GetSafeString("");
                            string files = info["files"].GetSafeString("");
                            string attchstring = "";
                            if (files != "")
                            {
                                string[] filelist = files.Split('|');
                                if (filelist.Length > 0)
                                {
                                    for (int i = 0; i < filelist.Length; i++)
                                    {
                                        string[] fileinfo = filelist[i].Split(',');
                                        if (fileinfo.Length == 2)
                                        {
                                            string fileid = fileinfo[0];
                                            string filename = fileinfo[1];
                                            if (fileid != "")
                                            {
                                                attchstring += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' urltype='openfile'>" + filename + "</a><br/>";
                                            }

                                        }
                                    }
                                }

                            }

                            wxyform = "<font class='label-left'>开始时间：</font><font class='label-right'>" + kssj + "</font> <hr /><font class='label-left'>预计结束时间：</font><font class='label-right'>" + yjjssj + "</font> <hr /><font class='label-left'>安全监控责任人：</font> <font class='label-right'>" + aqzrr + "</font> <hr /><font class='label-left'>责任人联系电话：</font> <font class='label-right'>" + zrrdh + "</font> <hr /><font class='label-left'>控制措施：</font> <br /><div class='label-right'>" + cs + "</div><br />附件：<br />" + attchstring;
                            if (issg)
                            {
                                string jsystring = "<div class='label-left'>";
                                string ss = "select ryxm,yhzh=(select yhzh from i_m_qyzh where qybh=a.rybh) from i_m_nbry a where a.departmentid='14006961804985345'";
                                IList<IDictionary<string, string>> jdylist = CommonService.GetDataTable(ss);
                                foreach (var jdy in jdylist)
                                {
                                    jsystring += "<input class='checkbox1' type='checkbox' name='tzjdy' value='" + jdy["yhzh"] + "' />" + jdy["ryxm"] + "<br/>";
                                }
                                jsystring += "</div>";
                                wxyform += "<font class='label-left'>通知监督员：</font>" + jsystring + " <hr /><font class='label-left'>结束时间：</font><object id='jsrq' class='label-right' name='jsrq' type='date' validate='required' validatemsg='结束时间不能为空！' ></object> <hr /><font class='label-left'>附件：</font><div class='label-right'><input type='button' value='添加附件' onClick='document.getElementById(\\\"$cameraid\\\").openCameraUpload(true)'/> <br/><photoupload id='$cameraid' name='$cameraid' pwidth='1200' savealbum='true' nums='10'/></div>";
                                wxyform += "<div><input type='button' id='submit' value='危险源销项' style='width:50%;align:center;' onclick='SubDate(\\\"wxyss\\\")'/></div>";
                            }
                        }
                        if (state == 4)
                        {
                            string kssj = info["sy_realdate"].GetSafeString();
                            string yjjssj = info["sy_enddate2"].GetSafeString();
                            string aqzrr = info["aqzrr"].GetSafeString("");
                            string zrrdh = info["zrrdh"].GetSafeString("");
                            string cs = info["cs"].GetSafeString("");
                            string files = info["files"].GetSafeString("");
                            string attchstring = "";
                            if (files != "")
                            {
                                string[] filelist = files.Split('|');
                                if (filelist.Length > 0)
                                {
                                    for (int i = 0; i < filelist.Length; i++)
                                    {
                                        string[] fileinfo = filelist[i].Split(',');
                                        if (fileinfo.Length == 2)
                                        {
                                            string fileid = fileinfo[0];
                                            string filename = fileinfo[1];
                                            if (fileid != "")
                                            {
                                                attchstring += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' urltype='openfile'>" + filename + "</a><br/>";
                                            }

                                        }
                                    }
                                }

                            }

                            wxyform = "<font class='label-left'>开始时间：</font><font class='label-right'>" + kssj + "</font> <hr /><font class='label-left'>预计结束时间：</font><font class='label-right'>" + yjjssj + "</font> <hr /><font class='label-left'>安全监控责任人：</font> <font class='label-right'>" + aqzrr + "</font> <hr /><font class='label-left'>责任人联系电话：</font> <font class='label-right'>" + zrrdh + "</font> <hr /><font class='label-left'>控制措施：</font> <br /><div class='label-right'>" + cs + "</div><br />附件：<br />" + attchstring;
                        }
                        if (state == 1)
                        {
                            string kssj = info["sy_realdate"].GetSafeString();
                            string yjjssj = info["sy_enddate2"].GetSafeString();
                            string aqzrr = info["aqzrr"].GetSafeString("");
                            string zrrdh = info["zrrdh"].GetSafeString("");
                            string files = info["files"].GetSafeString("");
                            string cs = info["cs"].GetSafeString("");
                            string attchstring = "";
                            if (files != "")
                            {
                                string[] filelist = files.Split('|');
                                if (filelist.Length > 0)
                                {
                                    for (int i = 0; i < filelist.Length; i++)
                                    {
                                        string[] fileinfo = filelist[i].Split(',');
                                        if (fileinfo.Length == 2)
                                        {
                                            string fileid = fileinfo[0];
                                            string filename = fileinfo[1];
                                            if (fileid != "")
                                            {
                                                attchstring += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' urltype='openfile'>" + filename + "</a><br/>";
                                            }

                                        }
                                    }
                                }

                            }
                            wxyform = "<font class='label-left'>开始时间：</font><font class='label-right'>" + kssj + "</font> <hr /><font class='label-left'>预计结束时间：</font><font class='label-right'>" + yjjssj + "</font> <hr /><font class='label-left'>安全监控责任人：</font> <font class='label-right'>" + aqzrr + "</font> <hr /><font class='label-left'>责任人联系电话：</font> <font class='label-right'>" + zrrdh + "</font> <hr /><font class='label-left'>控制措施：</font> <br /><div class='label-right'>" + cs + "</div><br />附件：<br />" + attchstring;
                        }
                        if (state == 0 && isjl)
                        {
                            wxyform += "<div><input type='button' id='submit' value='监理确认' style='width:50%;align:center;' onclick='SubDate(\\\"wxyss\\\")'/></div>";
                        }
                        dt[0]["form"] = wxyform;



                    }
                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }

        }


        // 保存危险源附件
        public bool SaveWXYFile(int wxyid, HttpPostedFileBase file, string strAllowExt, int maxSize, out string msg)
        {
            bool ret = false;
            msg = "";
            string strSaveName = "";

            do
            {
                // 判断是否有文件
                if (file.ContentLength == 0)
                {
                    msg = "上传文件为空！";
                    break;
                }
                string tmpName = "";
                string tmpExt = "";
                strSaveName = file.FileName;
                if (strSaveName.LastIndexOf("\\") > -1)
                    strSaveName = strSaveName.Substring(strSaveName.LastIndexOf("\\") + 1);

                if (strSaveName.IndexOf(".") > 0)
                {
                    tmpName = strSaveName.Substring(0, strSaveName.LastIndexOf('.'));
                    tmpExt = strSaveName.Substring(strSaveName.LastIndexOf('.'), strSaveName.Length - strSaveName.LastIndexOf('.'));
                }
                else
                {
                    tmpName = file.FileName;
                }
                // 判断扩展名是否符合
                string[] allowExtArr = strAllowExt.Split(new char[] { '/' });
                bool find = false;
                for (int i = 0; i < allowExtArr.Length; i++)
                {
                    if (allowExtArr[i].ToLower() == tmpExt.ToLower() || allowExtArr[i] == ".*")
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    msg = "文件格式不正确！";
                    break;
                }
                // 判断文件大小是否符合,maxSize in kbytes
                //if (file.ContentLength > maxSize * 1024)
                //{
                //    msg = "文件大小超出了限制！";
                //    break;
                //}

                // 保存文件
                try
                {
                    byte[] postcontent = new byte[file.ContentLength];
                    int readlength = 0;
                    while (readlength < file.ContentLength)
                    {
                        int tmplen = file.InputStream.Read(postcontent, readlength, file.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    GclrWxyFile att = new GclrWxyFile();
                    att.WXYid = wxyid;
                    att.FileName = strSaveName;
                    att.SaveName = strSaveName;
                    att.FILECONTENT = postcontent;
                    att = GclrWxyFileService.Save(att);
                    msg = att.FileID.ToString();
                    string id = new Guid().ToString("N");
                    string s = string.Format("MigrateWXYFile('{0}','{1}','{2}','{3}')", id, strSaveName, tmpExt, att.FileID.ToString());
                    ret = CommonService.ExecProc(s, out msg);
                    if (ret)
                    {
                        msg = id;
                    }
                }
                catch
                {
                    msg = "保存文件失败！";
                }

            } while (false);

            return ret;
        }

        // 企业实施危险源
        public void QYWXYSS()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int wxyid = Request["id"].GetSafeInt(0);
                    string sql = string.Format("select * from view_gclr_wxy where recid={0}", wxyid);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        IDictionary<string, object> info = dt[0];
                        string sgdw = info["sgdw"].GetSafeString();
                        string jldw = info["jldw"].GetSafeString();
                        string wxy = info["wxy"].GetSafeString();
                        string jdzch = info["jdzch"].GetSafeString();
                        bool issg = (sgdw == CurrentUser.RealName) || CheckIsSG(CurrentUser.UserName, jdzch);
                        bool isjl = (jldw == CurrentUser.RealName) || CheckIsJL(CurrentUser.UserName, jdzch);
                        int state = info["state"].GetSafeInt();
                        string gcmc = info["gcmc"].GetSafeString();
                        string bw = info["bw"].GetSafeString();
                        string startdate = info["sy_startdate"].GetSafeString();
                        string sbbh = this.Request.Form["sbbh"].GetSafeString("");
                        string tzjdy = Request.Form["tzjdy"].GetSafeString();
                        if (isjl && state == 0)
                        {
                            string attitle = gcmc + "[" + jdzch + "]危险源监理单位确认";
                            string atbody = gcmc + "[" + jdzch + "]危险源监理单位确认,<br/>危险源：" + wxy + "<br/>部位：" + bw + "，<br/>预计实施时间：" + startdate + "。请知晓。";
                            string aqjdy = "";

                            string sql1 = "select aqjdy,gcmc from i_m_gc where zjdjh='" + jdzch + "'";
                            IList<IDictionary<string, object>> gcinfo = CommonService.GetDataTable2(sql1);
                            if (gcinfo.Count > 0)
                            {
                                IDictionary<string, object> gc = gcinfo[0];
                                aqjdy = gc["aqjdy"].GetSafeString();
                            }
                            // 通知安全监督员
                            if (aqjdy != "")
                            {
                                Alert at = new Alert();
                                at.Reader = aqjdy;
                                at.AlertTitle = attitle;
                                at.AlertBody = atbody;
                                at.CreatedOn = DateTime.Now;
                                at.CreatedBy = "[系统]";
                                at.HasRead = false;
                                at.AlertType = 8;
                                AlertService.Save(at);

                            }
                            // 更新状态
                            string upsql = string.Format("update gclr_wxy set state=2 where recid={0}", wxyid);
                            CommonService.Execsql(upsql);

                            if (jdzch != "")
                            {
                                Alert attp = new Alert();
                                attp.Reader = jdzch;
                                attp.AlertTitle = attitle;
                                attp.AlertBody = atbody;
                                attp.CreatedOn = DateTime.Now;
                                attp.CreatedBy = CurrentUser.UserName;
                                attp.HasRead = false;
                                attp.AlertType = 8;
                                AlertService.Save(attp);
                            }

                        }
                        else if (issg && state == 2)
                        {
                            if (wxy == "起重机械设备的安装、拆卸")
                            {
                                int temcount = 0;
                                string ss1 = "select count(recid) as sum from GCLR_WXY where State in (3,4) and SBBH='" + sbbh + "'";
                                IList<IDictionary<string, object>> clist = CommonService.GetDataTable2(ss1);
                                if (clist.Count > 0)
                                {
                                    temcount = clist[0]["sum"].GetSafeInt();
                                }
                                if (temcount > 0)
                                {
                                    ret = false;

                                    msg = "该编号设备危险源还没有销项，保存失败！请确认设备编号正确或联系监督站确认。";
                                }
                            }
                            if (ret)
                            {
                                string realdate = Request.Form["kssj"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                                string enddate2 = Request.Form["jsrq"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                                string azzrr = Request.Form["aqzrr"].GetSafeString();
                                string zrrdh = Request.Form["zrrdh"].GetSafeString();
                                string cs = Request.Form["cs"].GetSafeString().Replace("\r\n", "<br/>");
                                string ss2 = string.Format("update GCLR_WXY set sbbh='{0}',realdate='{1}',,enddate2='{2}',aqzrr='{3}',zrrdh='{4}',cs='{5}',state=3 where recid={6}",
                                    sbbh, realdate, enddate2, azzrr, zrrdh, cs, wxyid
                                    );
                                CommonService.Execsql(ss2);

                                HttpFileCollectionBase files = Request.Files;
                                for (int i = 0; i < files.Count; i++)
                                {
                                    string tmpName = "";
                                    SaveWXYFile(wxyid, files[i], ".*", 20480, out tmpName);
                                }

                                string attitle = gcmc + "[" + jdzch + "]危险源实施";
                                string atbody = gcmc + "[" + jdzch + "]危险源实施,<br/>部位：" + bw + "<br/>危险源：" + wxy + "<br/>实施时间：" + Request.Form["kssj"].GetSafeDate(DateTime.Now).ToShortDateString() + "。请知晓。";

                                string ss3 = string.Format("select yhzh from i_m_qyzh where qybh in (select qybh from i_m_qy where qymc='{0}')", jldw);
                                IList<IDictionary<string, string>> zhlist = CommonService.GetDataTable(ss3);
                                if (zhlist.Count > 0)
                                {
                                    string zh = zhlist[0]["yhzh"];
                                    Alert attp = new Alert();
                                    attp.Reader = zh;
                                    attp.AlertTitle = attitle;
                                    attp.AlertBody = atbody;
                                    attp.CreatedOn = DateTime.Now;
                                    attp.CreatedBy = CurrentUser.UserName;
                                    attp.HasRead = false;
                                    attp.AlertType = 9;
                                    AlertService.Save(attp);
                                }
                                if (tzjdy != "")
                                {
                                    string[] jdys = tzjdy.Split(',');
                                    for (int i = 0; i < jdys.Length; i++)
                                    {
                                        Alert attp = new Alert();
                                        attp.Reader = jdys[i];
                                        attp.AlertTitle = attitle;
                                        attp.AlertBody = atbody;
                                        attp.CreatedOn = DateTime.Now;
                                        attp.CreatedBy = "[系统]";
                                        attp.HasRead = false;
                                        attp.AlertType = 8;
                                        AlertService.Save(attp);
                                    }
                                }

                            }



                        }
                        else if (issg && state == 3)
                        {
                            string enddate = Request.Form["jsrq"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                            string s1 = string.Format("update gclr_wxy set enddate='{0}', state=4 where reid={1}", enddate, wxyid);
                            CommonService.Execsql(s1);


                            HttpFileCollectionBase files = Request.Files;

                            for (int i = 0; i < files.Count; i++)
                            {
                                string tmpName = "";
                                SaveWXYFile(wxyid, files[i], ".*", 20480, out tmpName);

                            }

                            string attitle = gcmc + "[" + jdzch + "]危险源实施";
                            string atbody = gcmc + "[" + jdzch + "]危险源实施,<br/>部位：" + bw + "<br/>危险源：" + wxy + "<br/>实施时间：" + Request.Form["kssj"].GetSafeDate(DateTime.Now).ToShortDateString() + "。请知晓。";

                            string ss3 = string.Format("select yhzh from i_m_qyzh where qybh in (select qybh from i_m_qy where qymc='{0}')", jldw);
                            IList<IDictionary<string, string>> zhlist = CommonService.GetDataTable(ss3);
                            if (zhlist.Count > 0)
                            {
                                string zh = zhlist[0]["yhzh"];
                                Alert attp = new Alert();
                                attp.Reader = zh;
                                attp.AlertTitle = attitle;
                                attp.AlertBody = atbody;
                                attp.CreatedOn = DateTime.Now;
                                attp.CreatedBy = CurrentUser.UserName;
                                attp.HasRead = false;
                                attp.AlertType = 9;
                                AlertService.Save(attp);

                            }
                            if (tzjdy != "")
                            {
                                string[] jdys = tzjdy.Split(',');
                                for (int i = 0; i < jdys.Length; i++)
                                {
                                    Alert attp = new Alert();
                                    attp.Reader = jdys[i];
                                    attp.AlertTitle = attitle;
                                    attp.AlertBody = atbody;
                                    attp.CreatedOn = DateTime.Now;
                                    attp.CreatedBy = "[系统]";
                                    attp.HasRead = false;
                                    attp.AlertType = 8;
                                    AlertService.Save(attp);

                                }
                            }

                        }
                        else
                        {
                            ret = false;
                            msg = "危险源状态不一致，请重新打开！";
                        }
                    }

                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }

        // 企业整改单列表
        public void QYGetZGDList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string zgdbh = Request["zgdbh"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string qyjdzch = "";
                    qyjdzch = " select zjdjh from view_i_m_gc_lb where (jldwmc='" + CurrentUser.RealName + "'  or sgdwmc='" + CurrentUser.RealName + "')";

                    string sql = "select * from VIEW_ProjectReport where 1=1 and bglx='ZGD' and jdzch in(" + qyjdzch + ") ";
                    if (gcmc != "")
                    {
                        sql += " and gcmc like '%" + gcmc + "%' ";
                    }
                    if (zgdbh != "")
                    {
                        sql += " and bh like '%" + zgdbh + "%' ";
                    }
                    sql += " order by recid desc";

                    dt = CommonService.GetPageData2(sql, 10, pageIndex, out total);
                    foreach (var item in dt)
                    {
                        item["step"] = "4";
                    }


                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 企业监督记录
        public void QYGetJDJLList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string gcmc = Request["gcmc"].GetSafeString("");
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string qyjdzch = "";
                    qyjdzch = " select zjdjh from view_i_m_gc_lb where (jldwmc='" + CurrentUser.RealName + "'  or sgdwmc='" + CurrentUser.RealName + "')";

                    string sql = "select * from VIEW_ProjectJDJL where 1=1  and jdzch in(" + qyjdzch + ") ";
                    if (gcmc != "")
                    {
                        sql += " and gcmc like '%" + gcmc + "%' ";
                    }

                    sql += " order by recid desc";

                    dt = CommonService.GetPageData2(sql, 10, pageIndex, out total);
                    foreach (var item in dt)
                    {
                        item["step"] = "4";
                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 企业监督通知
        public void QYGetTZDList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string gcmc = Request["gcmc"].GetSafeString("");
                    int pageIndex = Request["page"].GetSafeInt(1);
                    DateTime startdate = Request["date1"].GetSafeDate(DateTime.MinValue);
                    DateTime enddate = Request["date2"].GetSafeDate(DateTime.MinValue);
                    string qyjdzch = "";
                    qyjdzch = " select zjdjh from view_i_m_gc_lb where (jldwmc='" + CurrentUser.RealName + "'  or sgdwmc='" + CurrentUser.RealName + "')";

                    string strSql = "select * from VIEW_ReportWWGZ where 1=1 and jdzch in (" + qyjdzch + ") ";


                    if (gcmc != "")
                    {
                        strSql += " and GCMC like '%" + gcmc + "%'";
                    }
                    if (startdate != DateTime.MinValue)
                    {
                        strSql += " and JDdate>=convert(datetime,'" + startdate.ToShortDateString() + "')";
                    }
                    if (enddate != DateTime.MinValue)
                    {
                        strSql += " and JDdate<=convert(datetime,'" + enddate.ToShortDateString() + "')";
                    }
                    strSql += " order by recid desc";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 企业通知单
        public void QYGetTZD()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "{}";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string ss = "select ryxm,yhzh=(select yhzh from i_m_qyzh where qybh=a.rybh) from i_m_nbry a where a.departmentid IN ('14006961804985345','14006961804968960')";
                    IList<IDictionary<string, string>> jdylist = CommonService.GetDataTable(ss);

                    int temRecid = Request["recid"].GetSafeInt();
                    if (temRecid > 0)
                    {
                        string ssql = string.Format("select * from VIEW_REPORTWWGZ where recid={0}", temRecid.ToString());
                        IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(ssql);
                        if (ddt.Count > 0)
                        {
                            IDictionary<string, object> info = ddt[0];
                            string files = info["files"].GetSafeString("");
                            string fjtext = "";
                            if (files != "")
                            {
                                string[] filelist = files.Split('|');
                                if (filelist.Length > 0)
                                {
                                    for (int i = 0; i < filelist.Length; i++)
                                    {
                                        string[] fileinfo = filelist[i].Split(',');
                                        if (fileinfo.Length == 2)
                                        {
                                            string fileid = fileinfo[0];
                                            string filename = fileinfo[1];
                                            if (fileid != "")
                                            {
                                                fjtext += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' urltype='openfile'>" + filename + "</a><br/>";
                                            }

                                        }
                                    }
                                }

                            }


                            string jdytext = "";
                            int count = 0;
                            foreach (var jdy in jdylist)
                            {
                                count++;
                                if (("," + info["jdy"].GetSafeString() + ",").IndexOf("," + jdy["yhzh"].GetSafeString() + ",") > -1)
                                {
                                    jdytext += "<input type='checkbox' name='jdy' checked='true' caption='" + jdy["ryxm"].GetSafeString() + "' value='" + jdy["yhzh"].GetSafeString() + "'/>";
                                }
                                else
                                {
                                    jdytext += "<input type='checkbox' name='jdy' caption='" + jdy["ryxm"].GetSafeString() + "' value='" + jdy["yhzh"].GetSafeString() + "'/>";
                                }
                                if (count % 2 == 0)
                                    jdytext += "<br/>";

                            }

                            data = "{\"dwmc\": \"" + info["dwmc"].GetSafeString() + "\",  \"gcmc\": \"" + info["gcmc"].GetSafeString() + "\", \"jdzch\":\"" + info["jdzch"].GetSafeString() + "\",\"jdrq\": \"" + info["sy_jddate"].GetSafeString() + "\",\"jdnr\": \"" + info["jdnr"].GetSafeString() + "\",\"jdr\": \"" + jdytext + "\",\"fj\": \"" + fjtext + "\",\"isconfirm\": \"" + info["isconfirm"].GetSafeInt(0).ToString() + "\"}";
                        }
                        else
                        {
                            ret = false;
                            msg = "该记录不存在！";

                        }
                    }
                    else
                    {
                        string fjtext = "";
                        string jdytext = "";
                        foreach (var jdy in jdylist)
                        {
                            jdytext += "<input type='checkbox' name='jdy' caption='" + jdy["ryxm"].GetSafeString() + "' value='" + jdy["yhzh"].GetSafeString() + "'/>";
                        }
                        data = "{\"dwmc\": \"" + CurrentUser.RealName + "\",  \"gcmc\": \"\", \"jdzch\":\"\",\"jdrq\": \"\",\"jdnr\": \"\",\"jdr\": \"" + jdytext + "\",\"fj\": \"" + fjtext + "\"}";
                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }

        // 获取危险源附件
        public void GetWWGZFile()
        {
            string filename = "";
            long filesize = 0;
            byte[] ret = null;
            int fileid = DataFormat.GetSafeInt(Request["id"]);
            try
            {
                string sql = string.Format("select savename,filecontent from ReportWWGZ_file where fileid={0}", fileid);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (dt.Count > 0)
                {
                    ret = (byte[])dt[0]["filecontent"];
                    filesize = ret.Length;
                    filename = dt[0]["savename"].GetSafeString();
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
        // 添加企业监督通知单
        public void QYAddTZD()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int RECID = Request["recid"].GetSafeInt(0);
                    // 更新
                    if (RECID > 0)
                    {
                        ReportWWGZ rgz = ReportWWGZService.Get(RECID);
                        rgz.DWMC = Request.Form["dwmc"].GetSafeString();
                        rgz.GCMC = Request.Form["xmmc"].GetSafeString();
                        rgz.JDZCH = Request.Form["jdbah"].GetSafeString();
                        rgz.JDY = Request.Form["jdy"].GetSafeString();
                        rgz.CreatedBy = CurrentUser.UserName;
                        rgz.CreatedOn = DateTime.Now;
                        rgz.JDNR = Request.Form["jdnr"].GetSafeString();
                        rgz.JDDate = Request.Form["jdrq"].GetSafeDate(DateTime.Now);
                        ReportWWGZService.Update(rgz);
                    }
                    // 新增
                    else
                    {
                        ReportWWGZ rgz = new ReportWWGZ();
                        rgz.DWMC = Request.Form["dwmc"].GetSafeString("");
                        rgz.GCMC = Request.Form["xmmc"].GetSafeString("");
                        rgz.JDZCH = Request.Form["jdbah"].GetSafeString("");
                        rgz.JDY = Request.Form["jdy"].GetSafeString("");
                        rgz.CreatedBy = CurrentUser.UserName;
                        rgz.CreatedOn = DateTime.Now;
                        rgz.JDNR = Request.Form["jdnr"].GetSafeString("");
                        rgz.JDDate = Request.Form["jdrq"].GetSafeDate(DateTime.Now);
                        rgz = ReportWWGZService.Save(rgz);
                        RECID = rgz.RECID;
                    }

                    // 保存附件
                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        byte[] postcontent = new byte[file.ContentLength];
                        int readlength = 0;
                        while (readlength < file.ContentLength)
                        {
                            int tmplen = file.InputStream.Read(postcontent, readlength, file.ContentLength - readlength);
                            readlength += tmplen;
                        }
                        file.InputStream.Seek(0, SeekOrigin.Begin);
                        ReportWWGZfile att = new ReportWWGZfile();
                        att.FileName = file.FileName;
                        att.SaveName = file.FileName;
                        att.WWGZid = RECID;
                        att.FILECONTENT = postcontent;

                        att = ReportWWGZfileService.Save(att);

                        string err = "";
                        string id = new Guid().ToString("N") + DateTime.Now.ToString("yyyyMMDDHHmmssfff");
                        string tmpExt = file.FileName.Substring(file.FileName.LastIndexOf('.'), file.FileName.Length - file.FileName.LastIndexOf('.'));
                        string s = string.Format("MigrateWWGZFile('{0}','{1}','{2}','{3}')", id, att.SaveName, tmpExt, att.FileID.ToString());
                        CommonService.ExecProc(s, out err);
                        SysLog4.WriteError(err);
                    }
                    // 通知监督员
                    string[] jdys = Request.Form["jdy"].GetSafeString("").Split(',');
                    for (int i = 0; i < jdys.Length; i++)
                    {
                        string attitle = Request.Form["xmmc"].GetSafeString("") + "新增监督通知书";
                        string atbody = Request.Form["dwmc"].GetSafeString("") + "，新增工程：" + Request.Form["xmmc"].GetSafeString("") + "[" + Request.Form["jdbah"].GetSafeString("") + "]" + "，监督通知书，要求监督内容：" + Request.Form["jdnr"].GetSafeString("") + "，要求监督时间：" + Request.Form["jdrq"].GetSafeDate(DateTime.Now).ToShortDateString() + "。请知晓";
                        if (RECID > 0)
                        {
                            Alert alert = new Alert();
                            alert.AlertType = 8;
                            alert.Reader = jdys[i];
                            alert.AlertTitle = attitle;
                            alert.AlertBody = atbody;
                            alert.HasRead = false;
                            alert.CreatedBy = "[系统]";
                            AlertService.Save(alert);
                        }
                    }




                    ret = true;
                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }



        // 判断某个企业是否是某个工程的施工单位
        private bool CheckIsSG(string usercode, string jdzch)
        {
            bool ret = false;
            string sql = string.Format("select gcbh from view_i_m_gc where zjdjh='{0}' and ','+sgdwzh+',' like '%,{1},%' ", jdzch, usercode);
            IList<IDictionary<string, string>> gclist = CommonService.GetDataTable(sql);
            if (gclist.Count > 0)
            {
                ret = true;
            }
            return ret;

        }

        // 判断某个企业是否是某个工程的监理单位
        private bool CheckIsJL(string usercode, string jdzch)
        {
            bool ret = false;
            string sql = string.Format("select gcbh from view_i_m_gc where zjdjh='{0}' and ','+jldwzh+',' like '%,{1},%' ", jdzch, usercode);
            IList<IDictionary<string, string>> gclist = CommonService.GetDataTable(sql);
            if (gclist.Count > 0)
            {
                ret = true;
            }
            return ret;

        }
        // 监督记录列表
        public void GetJDJLList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");

                    string strSql = "select a.* from VIEW_ProjectJDJL a,VIEW_I_M_GC_LB b  where a.projectkey='JDZC' and a.jdzch=b.zjdjh ";
                    //strSql += " and ','+a.jdry+','+a.createdby+',' like '%," + CurrentUser.UserName + ",%' ";

                    if (jdzch != "")
                    {
                        strSql += " and b.zjdjh like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and b.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and b.sy_jsdwmc like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and b.sgdwmc like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and b.jldwmc like '%" + jldw + "%'";
                    }

                    strSql += " order by a.lrsj desc ";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                    foreach (var item in dt)
                    {
                        item["xxjd"] = item["xxjd"].GetSafeString().Replace("\r\n", " ");
                    }
                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }

        // 监督记录详情
        public void GetJDJLDetail()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int id = Request["id"].GetSafeInt(0);
                    string strSql = string.Format("select * from view_projectjdjl a where recid={0} ", id);
                    dt = CommonService.GetDataTable2(strSql);
                    if (dt.Count > 0)
                    {
                        IDictionary<string, object> info = dt[0];
                        string fj = "";
                        string serial = info["workserial"].GetSafeString();
                        string fjms = info["fjlist"].GetSafeString();
                        if (serial != "")
                        {
                            string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                            if (fjlist.Length > 0)
                            {
                                for (int i = 0; i < fjlist.Length; i++)
                                {
                                    string[] file = fjlist[i].Split('|');
                                    if (file.Length > 0)
                                    {
                                        fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + file[1] + "' urltype='openfile'>" + file[0] + "</a><br/>";
                                    }

                                }
                            }

                        }
                        info["fj"] = fj;
                        info["xxjd"] = info["xxjd"].GetSafeString().Replace("\r\n", " ");
                        info["jdjl"] = info["jdqk"].GetSafeString().Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>").Replace("\\r\\n", "<br/>").Replace("\\n", "<br/>").Replace("\\r", "<br/>");
                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 整改单列表
        public void GetZGDList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");

                    string strSql = "select a.* from VIEW_ProjectReport a,VIEW_I_M_GC_LB b  where a.bglx='ZGD' and a.jdzch=b.zjdjh ";


                    if (jdzch != "")
                    {
                        strSql += " and b.zjdjh like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and b.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and b.sy_jsdwmc like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and b.sgdwmc like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and b.jldwmc like '%" + jldw + "%'";
                    }

                    strSql += " order by a.createdon desc ";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 通知单列表
        public void GetTGDList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");

                    string strSql = "select a.* from VIEW_ProjectReport a,VIEW_I_M_GC_LB b  where a.bglx='TGL' and a.jdzch=b.zjdjh ";


                    if (jdzch != "")
                    {
                        strSql += " and b.zjdjh like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and b.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and b.sy_jsdwmc like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and b.sgdwmc like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and b.jldwmc like '%" + jldw + "%'";
                    }

                    strSql += " order by a.createdon desc ";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 复工单列表
        public void GetFGDList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string jdzch = Request["jdzch"].GetSafeString("");
                    string gcmc = Request["gcmc"].GetSafeString("");
                    string jsdw = Request["jsdw"].GetSafeString("");
                    string sgdw = Request["sgdw"].GetSafeString("");
                    string jldw = Request["jldw"].GetSafeString("");

                    string strSql = "select a.* from VIEW_ProjectReport a,VIEW_I_M_GC_LB b  where a.bglx='FGL' and a.jdzch=b.zjdjh ";


                    if (jdzch != "")
                    {
                        strSql += " and b.zjdjh like '%" + jdzch + "%'";
                    }
                    if (gcmc != "")
                    {
                        strSql += " and b.gcmc like '%" + gcmc + "%'";
                    }
                    if (jsdw != "")
                    {
                        strSql += " and b.sy_jsdwmc like '%" + jsdw + "%'";
                    }
                    if (sgdw != "")
                    {
                        strSql += " and b.sgdwmc like '%" + sgdw + "%'";
                    }
                    if (jldw != "")
                    {
                        strSql += " and b.jldwmc like '%" + jldw + "%'";
                    }

                    strSql += " order by a.createdon desc ";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 监督通知单列表
        public void GetTZDList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string gcmc = Request["gcmc"].GetSafeString("");
                    int pageIndex = Request["page"].GetSafeInt(1);
                    DateTime startdate = Request["date1"].GetSafeDate(DateTime.MinValue);
                    DateTime enddate = Request["date2"].GetSafeDate(DateTime.MinValue);

                    string strSql = "select * from VIEW_ReportWWGZ where 1=1 ";

                    if (gcmc != "")
                    {
                        strSql += " and GCMC like '%" + gcmc + "%'";
                    }
                    if (startdate != DateTime.MinValue)
                    {
                        strSql += " and JDdate>=convert(datetime,'" + startdate.ToShortDateString() + "')";
                    }
                    if (enddate != DateTime.MinValue)
                    {
                        strSql += " and JDdate<=convert(datetime,'" + enddate.ToShortDateString() + "')";
                    }
                    strSql += " order by recid desc";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 监督通知单详情
        public void GetTZD()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "{}";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {


                    int temRecid = Request["recid"].GetSafeInt();
                    if (temRecid > 0)
                    {
                        string ssql = string.Format("select * from VIEW_REPORTWWGZ where recid={0}", temRecid.ToString());
                        IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(ssql);
                        if (ddt.Count > 0)
                        {
                            IDictionary<string, object> info = ddt[0];
                            string files = info["files"].GetSafeString("");
                            string fjtext = "";
                            if (files != "")
                            {
                                string[] filelist = files.Split('|');
                                if (filelist.Length > 0)
                                {
                                    for (int i = 0; i < filelist.Length; i++)
                                    {
                                        string[] fileinfo = filelist[i].Split(',');
                                        if (fileinfo.Length == 2)
                                        {
                                            string fileid = fileinfo[0];
                                            string filename = fileinfo[1];
                                            if (fileid != "")
                                            {
                                                fjtext += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "' urltype='openfile'>" + filename + "</a><br/>";
                                            }

                                        }
                                    }
                                }

                            }

                            data = "{\"dwmc\": \"" + info["dwmc"].GetSafeString() + "\",  \"gcmc\": \"" + info["gcmc"].GetSafeString() + "\", \"jdzch\":\"" + info["jdzch"].GetSafeString() + "\",\"jdrq\": \"" + info["sy_jddate"].GetSafeString() + "\",\"jdnr\": \"" + info["jdnr"].GetSafeString() + "\",\"jdr\": \"" + info["jdyxm"].GetSafeString() + "\",\"fj\": \"" + fjtext + "\"}";
                        }
                        else
                        {
                            ret = false;
                            msg = "该记录不存在！";

                        }
                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }
        // 日程安排列表
        public void GetRCAPList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    DateTime date = Request["date"].GetSafeDate(DateTime.MinValue);
                    int pageIndex = Request["page"].GetSafeInt(1);
                    string datestart = "";
                    string dateend = "";
                    string mindTimeStart = "2000-01-01";
                    string mindTimeEnd = "3000-01-01";
                    string strSql = "";
                    if (date == DateTime.MinValue)
                    {
                        datestart = "2000-01-01";
                        dateend = "2050-01-01";
                    }
                    else
                    {
                        datestart = date.ToString("yyyy-MM-dd");
                        dateend = date.AddDays(1).ToString("yyyy-MM-dd");
                    }

                    strSql = "select * from VIEW_Calendar where 1=1 ";
                    strSql += " and (not ( DateStart>=convert(datetime,'" + dateend + "') or DateEnd<convert(datetime,'" + datestart + "'))) ";
                    strSql += " and CreatedBy='" + CurrentUser.UserName + "' ";
                    strSql += " and MindTime>=convert(datetime,'" + mindTimeStart + "') ";
                    strSql += " and MindTime<=convert(datetime,'" + mindTimeEnd + "') ";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }
        // 新增日程安排
        public void ADDRCAP()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int taskid = Request["taskid"].GetSafeInt(0);
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now;
                    if (Request.Form["startdate"].GetSafeString("").Trim() != "" && Request.Form["starttime"].GetSafeString("").Trim() != "")
                    {
                        startdate = DateTime.Parse(Request.Form["startdate"].GetSafeString("").Trim() + " " + Request.Form["starttime"].GetSafeString("").Trim());
                    }
                    if (Request.Form["enddate"].GetSafeString("").Trim() != "" && Request.Form["endtime"].GetSafeString("").Trim() != "")
                    {
                        startdate = DateTime.Parse(Request.Form["enddate"].GetSafeString("").Trim() + " " + Request.Form["endtime"].GetSafeString("").Trim());
                    }

                    string sql = string.Format("insert into calendar ( datestart, dateend, content,mindtime,createdon, createdby,hasmind) values ('{0}','{1}','{2}','{3}','{4}','{5}',0)",
                        startdate.ToString("yyyy-MM-dd HH:mm:ss"),
                        enddate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Request.Form["context"].GetSafeString().Trim(),
                        startdate.ToString("yyyy-MM-dd HH:mm:ss"),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        CurrentUser.UserName
                        );
                    ret = CommonService.ExecSql(sql, out msg);


                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }

        //获取设备详情
        public void GetSBDetail()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strSerial = Request["serial"].GetSafeString("");
                    string strSql = string.Format("select * from view_sb_reportsbsy where workserial='{0}' ", strSerial);
                    IList<IDictionary<string, string>> syjllist = CommonService.GetDataTable(strSql);
                    if (syjllist.Count > 0)
                    {
                        IDictionary<string, string> syjl = syjllist[0];
                        string recid = syjl["recid"];
                        string workserial = syjl["workserial"];
                        data += "<font style = 'font-weight:bold;font-style:italic' > 详情：</font ><br/>";
                        string url = "";
                        url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + "SBAZGZ";
                        data += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>查看安装告知</a><br/>";
                        url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + "SBSYDJ";
                        data += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>查看使用登记</a><br/>";
                        url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + "SBCXGZ";
                        data += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>查看拆卸告知</a><br/>";
                        string fj = "";
                        if (workserial != "")
                        {
                            string s = string.Format("select fileid,FileOrgName from stfile where formid in (select formid from stform where serialno='{0}')", workserial);
                            IList<IDictionary<string, string>> attachlist = CommonService.GetDataTable(s);
                            if (attachlist.Count == 0)
                            {
                                s = string.Format("select convert(nvarchar(max),itemvalue) as fj from stformitem where  itemname='fj' and formid in (select formid from stform where serialno='{0}')", workserial);
                                IList<IDictionary<string, string>> info = CommonService.GetDataTable(s);
                                if (info.Count > 0)
                                {
                                    string fjms = info[0]["fj"];
                                    string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (fjlist.Length > 0)
                                    {
                                        List<string> fileidlist = new List<string>();
                                        // 获取附件ID列表
                                        for (int i = 0; i < fjlist.Length; i++)
                                        {
                                            string[] file = fjlist[i].Split('|');
                                            if (file.Length == 2)
                                            {
                                                fileidlist.Add(file[1]);
                                            }
                                        }
                                        if (fileidlist.Count > 0)
                                        {
                                            s = string.Format("select fileid,FileOrgName from stfile where fileid in ({0}))", DataFormat.FormatSQLInStr(fileidlist));
                                            attachlist = CommonService.GetDataTable(s);
                                        }
                                    }

                                }
                            }
                            foreach (var att in attachlist)
                            {
                                string fileid = att["fileid"];
                                string fileorgname = att["fileorgname"];
                                fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + fileid + "' urltype='openfile'>" + fileorgname + "</a><br/>";
                            }
                        }

                        fj = "<br/><font style='font-weight:bold;font-style:italic'>附件：</font><br/>" + fj;
                        data += fj;

                    }


                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": \"{2}\"}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }

        // 危险源批量销项
        public void BatchXX()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string ids = Request["ids"].GetSafeString();
                if (ids != "")
                {
                    string[] idlist = ids.Split(',');
                    if (idlist.Length > 0)
                    {
                        string sql = string.Format("update gclr_wxy set state=1 where state=4 and recid in ({0})", string.Join(",", idlist));
                        ret = CommonService.ExecSql(sql, out msg);
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
        // 删除危险源
        public void DeleteWXY()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string sql = string.Format("delete from  gclr_wxy where recid={0}", id);
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

        public void FormShow()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strSerial = Request["serial"].GetSafeString("");
                    string type = Request["type"].GetSafeString("");
                    List<string> types = new List<string>() { "zgd", "tgd", "fgd" };
                    if (types.Contains(type))
                    {
                        string bglx = "";
                        string reporttype = "";
                        string title = "";
                        if (type == "zgd")
                        {
                            bglx = "ZGD";
                            reporttype = "ZGTZS";
                            title = "查看整改单";
                        }
                        else if (type == "tgd")
                        {
                            bglx = "TGL";
                            reporttype = "TGL";
                            title = "查看停工单";
                        }
                        else if (type == "fgd")
                        {
                            bglx = "FGL";
                            reporttype = "FGL";
                            title = "查看复工单";
                        }

                        if (bglx != "")
                        {

                            string strSql = string.Format("select recid,reportfile,workserial,fjms from view_projectreport where workserial='{0}' and bglx='{1}'", strSerial, bglx);
                            IList<IDictionary<string, string>> rptlist = CommonService.GetDataTable(strSql);
                            if (rptlist.Count > 0)
                            {
                                IDictionary<string, string> rpt = rptlist[0];
                                string recid = rpt["recid"];
                                string workserial = rpt["workserial"];
                                string fjms = rpt["fjms"];
                                string url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + reporttype;
                                SysLog4.WriteError(url);
                                data += "<font style = 'font-weight:bold;font-style:italic' > 详情：</font ><br/>" + "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>" + title + "</a><br/>";
                                if (fjms != "")
                                {
                                    string fj = "";
                                    string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (fjlist.Length > 0)
                                    {
                                        for (int i = 0; i < fjlist.Length; i++)
                                        {
                                            string[] file = fjlist[i].Split('|');
                                            if (file.Length > 0)
                                            {
                                                fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + file[0] + "' urltype='openfile'>" + file[1] + "</a><br/>";
                                            }

                                        }
                                    }
                                    fj = "<br/><font style='font-weight:bold;font-style:italic'>附件：</font><br/>" + fj;
                                    data += fj;
                                }
                                //SysLog4.WriteError(data);
                            }

                        }

                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": \"{2}\"}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }

        public ActionResult getReportDown()
        {
            string reporttype = "";
            string filename = "";
            string serial = "";
            string jdjlid = "";
            try
            {
                string id = Request["id"].GetSafeString();
                string[] t = id.Split('$');
                jdjlid = t[0];
                serial = t[1];
                reporttype = t[2];
                if (reporttype == "JDXCJL")
                {
                    filename = "监督巡查记录V1";
                }
                else if (reporttype == "JDYSJL")
                {
                    filename = "监督验收记录V1";
                }
                else if (reporttype == "JDXWCC")
                {
                    filename = "监督行为抽查V1";
                }
                else if (reporttype == "JDSTCC")
                {
                    filename = "监督实体抽查V1";
                }
                else if (reporttype == "JDZLCC")
                {
                    filename = "监督资料抽查V1";
                }
                else if (reporttype == "SBAZGZ")
                {
                    filename = "安装告知表V1";
                }
                else if (reporttype == "SBSYDJ")
                {
                    filename = "使用登记表V1";
                }
                else if (reporttype == "SBCXGZ")
                {
                    filename = "拆卸告知表V1";
                }
                else
                {
                    string strSql = string.Format("select recid,reportfile,workserial from view_projectreport where workserial='{0}' and recid='{1}'", serial, jdjlid);
                    IList<IDictionary<string, string>> rptlist = CommonService.GetDataTable(strSql);
                    if (rptlist.Count > 0)
                    {
                        IDictionary<string, string> rpt = rptlist[0];
                        filename = rpt["reportfile"];
                    }
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            //SysLog4.WriteError("/dwgxzj/FlowReport?reportfile=" + filename + "&serial=" + serial + "&type=download" + "&jdjlid=" + jdjlid + "&reporttype=" + reporttype);
            return new RedirectResult("/dwgxzj/FlowReport?reportfile=" + filename + "&serial=" + serial + "&type=download" + "&jdjlid=" + jdjlid + "&reporttype=" + reporttype);
            //return RedirectToAction("FlowReportDown", "jdbg", new { reportfile = "%E7%9B%91%E7%9D%A3%E6%96%B9%E6%A1%88v1", serial = 20170116009 });

        }

        public void QYFormShow()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string strSerial = Request["id"].GetSafeString("");
                    string type = Request["type"].GetSafeString("");
                    List<string> types = new List<string>() { "zgd", "tgd", "fgd", "jdjl", "sbdetail" };
                    if (types.Contains(type))
                    {
                        string bglx = "";
                        string reporttype = "";
                        string title = "";
                        if (type == "zgd")
                        {
                            bglx = "ZGD";
                            reporttype = "ZGTZS";
                            title = "查看整改单";
                        }
                        else if (type == "tgd")
                        {
                            bglx = "TGL";
                            reporttype = "TGL";
                            title = "查看停工单";
                        }
                        else if (type == "fgd")
                        {
                            bglx = "FGL";
                            reporttype = "FGL";
                            title = "查看复工单";
                        }
                        else if (type == "jdjl")
                        {
                            bglx = "JDJL";
                            reporttype = "JDXCJL";
                            title = "查看监督记录";
                        }
                        else if (type == "sbdetail")
                        {
                            bglx = "SBDETAIL";
                            reporttype = "";
                            title = "";
                        }

                        if (bglx != "")
                        {
                            if (bglx == "JDJL")
                            {
                                string strSql = string.Format("select * from view_projectjdjl where workserial='{0}'", strSerial);
                                IList<IDictionary<string, string>> jdjllist = CommonService.GetDataTable(strSql);
                                if (jdjllist.Count > 0)
                                {
                                    IDictionary<string, string> jdjl = jdjllist[0];
                                    string recid = jdjl["recid"];
                                    string workserial = jdjl["workserial"];
                                    string reportfile = jdjl["reportfile"];
                                    reporttype = "JDXCJL";
                                    if (reportfile == "监督验收记录V1")
                                    {
                                        reporttype = "JDYSJL";
                                    }
                                    else if (reportfile == "监督行为抽查V1")
                                    {
                                        reporttype = "JDXWCC";
                                    }
                                    else if (reportfile == "监督实体抽查V1")
                                    {
                                        reporttype = "JDSTCC";
                                    }
                                    else if (reportfile == "监督资料抽查V1")
                                    {
                                        reporttype = "JDZLCC";
                                    }

                                    string fjms = jdjl["fjlist"].GetSafeString();
                                    string url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + reporttype;
                                    data += "<font style = 'font-weight:bold;font-style:italic' > 详情：</font ><br/>" + "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>" + title + "</a><br/>";
                                    string fj = "";
                                    if (workserial != "")
                                    {
                                        string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                        if (fjlist.Length > 0)
                                        {
                                            for (int i = 0; i < fjlist.Length; i++)
                                            {
                                                string[] file = fjlist[i].Split('|');
                                                if (file.Length > 0)
                                                {
                                                    fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + file[1] + "' urltype='openfile'>" + file[0] + "</a><br/>";
                                                }

                                            }
                                        }

                                    }
                                    fj = "<br/><font style='font-weight:bold;font-style:italic'>附件：</font><br/>" + fj;
                                    data += fj;

                                }

                            }
                            else if (bglx == "SBDETAIL")
                            {
                                string strSql = string.Format("select * from view_sb_reportsbsy where workserial='{0}' ", strSerial);
                                IList<IDictionary<string, string>> syjllist = CommonService.GetDataTable(strSql);
                                if (syjllist.Count > 0)
                                {
                                    IDictionary<string, string> syjl = syjllist[0];
                                    string recid = syjl["recid"];
                                    string workserial = syjl["workserial"];
                                    data += "<font style = 'font-weight:bold;font-style:italic' > 详情：</font ><br/>";
                                    string url = "";
                                    url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + "SBAZGZ";
                                    data += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>查看安装告知</a><br/>";
                                    url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + "SBSYDJ";
                                    data += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>查看使用登记</a><br/>";
                                    url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + "SBCXGZ";
                                    data += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>查看拆卸告知</a><br/>";
                                    string fj = "";
                                    if (workserial != "")
                                    {
                                        string s = string.Format("select fileid,FileOrgName from stfile where formid in (select formid from stform where serialno='{0}')", workserial);
                                        IList<IDictionary<string, string>> attachlist = CommonService.GetDataTable(s);
                                        if (attachlist.Count == 0)
                                        {
                                            s = string.Format("select convert(nvarchar(max),itemvalue) as fj from stformitem where formid in (select formid from stform where serialno='{0}')", workserial);
                                            IList<IDictionary<string, string>> info = CommonService.GetDataTable(s);
                                            if (info.Count > 0)
                                            {
                                                string fjms = info[0]["fj"];
                                                string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                                if (fjlist.Length > 0)
                                                {
                                                    List<string> fileidlist = new List<string>();
                                                    // 获取附件ID列表
                                                    for (int i = 0; i < fjlist.Length; i++)
                                                    {
                                                        string[] file = fjlist[i].Split('|');
                                                        if (file.Length == 2)
                                                        {
                                                            fileidlist.Add(file[1]);
                                                        }
                                                    }
                                                    if (fileidlist.Count > 0)
                                                    {
                                                        s = string.Format("select fileid,FileOrgName from stfile where fileid in ({0}))", DataFormat.FormatSQLInStr(fileidlist));
                                                        attachlist = CommonService.GetDataTable(s);
                                                    }
                                                }

                                            }
                                        }
                                        foreach (var att in attachlist)
                                        {
                                            string fileid = att["fileid"];
                                            string fileorgname = att["fileorgname"];
                                            fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + fileid + "' urltype='openfile'>" + fileorgname + "</a><br/>";
                                        }
                                    }

                                    fj = "<br/><font style='font-weight:bold;font-style:italic'>附件：</font><br/>" + fj;
                                    data += fj;

                                }
                            }
                            else
                            {
                                string strSql = string.Format("select recid,reportfile,workserial,fjms from view_projectreport where workserial='{0}' and bglx='{1}'", strSerial, bglx);
                                IList<IDictionary<string, string>> rptlist = CommonService.GetDataTable(strSql);
                                if (rptlist.Count > 0)
                                {
                                    IDictionary<string, string> rpt = rptlist[0];
                                    string recid = rpt["recid"];
                                    string workserial = rpt["workserial"];
                                    string fjms = rpt["fjms"];
                                    string url = "/dwgxzj/getReportDown?id=" + recid + "$" + workserial + "$" + reporttype;
                                    SysLog4.WriteError(url);
                                    data += "<font style = 'font-weight:bold;font-style:italic' > 详情：</font ><br/>" + "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + url + "' urltype='openfile'>" + title + "</a><br/>";
                                    if (fjms != "")
                                    {
                                        string fj = "";
                                        string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                        if (fjlist.Length > 0)
                                        {
                                            for (int i = 0; i < fjlist.Length; i++)
                                            {
                                                string[] file = fjlist[i].Split('|');
                                                if (file.Length > 0)
                                                {
                                                    fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + file[0] + "' urltype='openfile'>" + file[1] + "</a><br/>";
                                                }

                                            }
                                        }
                                        fj = "<br/><font style='font-weight:bold;font-style:italic'>附件：</font><br/>" + fj;
                                        data += fj;
                                    }
                                    //SysLog4.WriteError(data);
                                }
                            }


                        }

                    }

                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": \"{2}\"}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }

        public void DeleteSBSY()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {

                    string s = string.Format("DeleteSBSY('{0}')", id);
                    ret = CommonService.ExecProc(s, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
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

        public void CanditSBSY()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {

                    string s = string.Format("update sb_reportsbsy set Candit=1 where recid={0}", id);
                    ret = CommonService.ExecSql(s, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
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















        #endregion

        #region 首页
        /// <summary>
        /// 获取近日报监工程
        /// </summary>
        [Authorize]
        public void GetJdyGcs()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select top 10 gcbh,zjdjh,gcmc,slrq,jdgcsxm,tjjdyxm,azjdyxm,aqjdyxm from view_i_m_gc_jd where zt not in ('YT','LR') order by slrq desc";
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
        /// 获取企业相关工程
        /// </summary>
        [Authorize]
        public void GetQyGcs()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> qys = CommonService.GetDataTable("select qybh from i_m_qyzh where yhzh='" + CurrentUser.UserCode + "'");
                if (qys.Count() > 0)
                {
                    string qybh = qys[0]["qybh"];
                    string sql = "select  gcbh,zjdjh,gcmc,slrq,jdgcsxm,tjjdyxm,azjdyxm,aqjdyxm from view_i_m_gc_jd where gcbh in (select distinct gcbh from View_I_S_GC_ALLQY where qybh='" + qybh + "') order by slrq desc";
                    ret = CommonService.GetDataTable(sql);
                }

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
        /// 获取人员去向
        /// </summary>
        [Authorize]
        public void GetRyqx()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = string.Format("select top 10 recid,realname,reason,applydate,fromdate,todate from userleave where fromdate is not null and (username='{0}' or exists(select * from h_func_qx where lx='RYQX' and usercode='{1}')) order by applydate desc", CurrentUser.UserName, CurrentUser.UserName);
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
        /// 获取整改单
        /// </summary>
        [Authorize]
        public void GetZgds()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select top 10 a.jdzch,a.gcmc,a.workserial,a.createdon,a.bh,c.ryxm,a.reportfile,a.recid from ProjectReport a left outer join i_m_qyzh b on a.createdby=b.yhzh left outer join i_m_nbry c on b.qybh=c.rybh where a.bglx='zgd' order by a.createdon desc";
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

        [Authorize]
        public void GetJdjls()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select top 10 * from view_projectjdjl a  where (','+a.JDRY +',' like '%," + CurrentUser.UserName + ",%' or exists(select * from h_func_qx where lx='JDJL' and usercode='" + CurrentUser.UserName + "'))order by jdsj desc";
                ret = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = int.MaxValue;
            Response.Write(jss.Serialize(ret));
        }

        [Authorize]
        public ActionResult WelcomeZaj2()
        {
            return View();
        }
        #endregion

        #region 诸暨政务网用户接口
        /// <summary>
        /// 新增用户
        /// </summary>
        public void ImportUser()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string username = Request["username"].GetSafeString();
                string realname = Request["realname"].GetSafeString();
                if (usercode == "")
                {
                    ret = false;
                    msg = "usercode 不能为空！";
                }
                else if (username == "")
                {
                    ret = false;
                    msg = "username 不能为空！";
                }
                //else if (realname == "")
                //{
                //    ret = false;
                //    msg = "realname 不能为空！";
                //}
                else
                {
                    string s1 = string.Format("select yhzh from i_m_qyzh where yhzh='{0}'", usercode);
                    IList<IDictionary<string, string>> ddt = CommonService.GetDataTable(s1);
                    if (ddt.Count > 0)
                    {
                        ret = false;
                        msg = "usercode 已经存在！";
                    }
                    else
                    {
                        string sql = string.Format("insert into i_m_nbry (zh,zjzbh,rybh,ryxm,sptg,sfyx) values ('{0}','{1}','{2}','{3}',1,1)", username, "CP201402000001", usercode, realname);
                        ret = CommonService.ExecSql(sql, out msg);

                        if (ret)
                        {
                            sql = string.Format("insert into i_m_qyzh (qybh,yhzh,sfqyzzh,lrsj,zhlx) values ('{0}','{1}',0,getdate(),'N')", usercode, usercode);
                            ret = CommonService.ExecSql(sql, out msg);
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
        /// <summary>
        /// 更新用户
        /// </summary>
        public void UpdateUser()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string username = Request["username"].GetSafeString();
                string realname = Request["realname"].GetSafeString();
                if (usercode == "")
                {
                    ret = false;
                    msg = "usercode 不能为空！";
                }
                else if (username == "")
                {
                    ret = false;
                    msg = "username 不能为空！";
                }
                //else if (realname == "")
                //{
                //    ret = false;
                //    msg = "realname 不能为空！";
                //}
                else
                {
                    string s1 = string.Format("select yhzh from i_m_qyzh where yhzh='{0}'", usercode);
                    IList<IDictionary<string, string>> ddt = CommonService.GetDataTable(s1);
                    if (ddt.Count == 0)
                    {
                        ret = false;
                        msg = "usercode 不存在！";
                    }
                    else
                    {
                        string sql = string.Format("update i_m_nbry set zh='{0}',ryxm='{1}' where rybh in (select qybh from i_m_qyzh where yhzh='{2}')", username, realname, usercode);
                        ret = CommonService.ExecSql(sql, out msg);
                        if (!ret && msg == "")
                        {
                            msg = "更新失败！";
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

        /// <summary>
        /// 删除用户
        /// </summary>
        public void DelUser()
        {
            bool ret = true;
            string msg = "";

            try
            {
                string usercode = Request["usercode"].GetSafeString();
                if (usercode == "")
                {
                    ret = false;
                    msg = "usercode 不能为空！";
                }
                else
                {
                    string s1 = string.Format("select yhzh from i_m_qyzh where yhzh='{0}'", usercode);
                    IList<IDictionary<string, string>> ddt = CommonService.GetDataTable(s1);
                    if (ddt.Count == 0)
                    {
                        ret = false;
                        msg = "usercode 不存在！";
                    }
                    else
                    {
                        List<string> lsql = new List<string>();
                        string s = string.Format("delete from i_m_nbry where rybh in (select qybh from i_m_qyzh where yhzh='{0}')", usercode);
                        lsql.Add(s);
                        s = string.Format("delete from i_m_qyzh where yhzh='{0}'", usercode);
                        lsql.Add(s);

                        ret = CommonService.ExecTrans(lsql, out msg);
                        if (!ret && msg == "")
                        {
                            msg = "删除失败！";
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
        #endregion
        #endregion

        #region 工程详情
        public ActionResult Gccknb()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.zjdjh = Request["zjdjh"].GetSafeString();
            ViewBag.gclxbh = Request["gclxbh"].GetSafeString();
            return View();
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
                string zjdjh = Request["zjdjh"].GetSafeString();
                string msg = "";
                VJdbgReportSumItem item = JdbgService.GetReportSum(gcbh, out msg);
                if (msg != "")
                {
                    SysLog4.WriteError("获取工程报告信息失败：" + msg);
                }
                string procstr = string.Format("GetGCXQTJXX('{0}','{1}')", gcbh, zjdjh);
                IList<IDictionary<string, string>> infolist = CommonService.ExecDataTableProc(procstr, out msg);
                int jdjlsum = 0;
                int zgdsum = 0;
                int zljdfasum = 0;
                int aqjdfasum = 0;
                int jdbgsum = 0;
                if (infolist.Count > 0)
                {
                    jdjlsum = infolist[0]["jdjlsum"].GetSafeInt();
                    zgdsum = infolist[0]["zgdsum"].GetSafeInt();
                    zljdfasum = infolist[0]["zljdfasum"].GetSafeInt();
                    aqjdfasum = infolist[0]["aqjdfasum"].GetSafeInt();
                    jdbgsum = infolist[0]["jdbgsum"].GetSafeInt();

                }
                int bhgbg = 0;
                int sybg = 0;
                bhgbg = JcjgBgService.GetBgsl(zjdjh, "2", "ZJJCJG");
                sybg = JcjgBgService.GetBgsl(zjdjh, "", "ZJJCJG");
                // 工程基本信息
                ret.Add(new VCheckItem() { id = "I_JBXX", pId = "", name = "工程基本信息(1)", isParent = false, cevent = "I_JBXX", open = true });
                // 工程监督方案
                ret.Add(new VCheckItem() { id = "I_JDFA", pId = "", name = "工程质量监督方案(" + zljdfasum + ")", isParent = false, cevent = "I_JDFA", open = true });
                ret.Add(new VCheckItem() { id = "I_JDFAAQ", pId = "", name = "工程安全监督方案(" + aqjdfasum + ")", isParent = false, cevent = "I_JDFAAQ", open = true });
                ret.Add(new VCheckItem() { id = "I_JDJD", pId = "", name = "监督交底(" + item.SumJDJD + ")", isParent = false, cevent = "I_JDJD", open = true });
                //ret.Add(new VCheckItem() { id = "04", pId = "", name = "质量行为监督检查记录(" + item.SumZLXWJCJL + ")", isParent = false, cevent = "04", open = true });
                ret.Add(new VCheckItem() { id = "G_ZLYS", pId = "", name = "工程质量监督验收", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_YSSQ", pId = "G_ZLYS", name = "工程验收申请记录(" + item.SumYSSQJL + ")", isParent = false, cevent = "I_YSSQ", open = false });
                ret.Add(new VCheckItem() { id = "I_YSAP", pId = "G_ZLYS", name = "工程验收安排记录(" + item.SumYSAPJL + ")", isParent = false, cevent = "I_YSAP", open = false });
                ret.Add(new VCheckItem() { id = "I_JDJL", pId = "G_ZLYS", name = "监督记录(" + jdjlsum + ")", isParent = false, cevent = "I_JDJL", open = false });
                ret.Add(new VCheckItem() { id = "I_ZGTZ", pId = "G_ZLYS", name = "整改通知(" + zgdsum + ")", isParent = false, cevent = "I_ZGTZ", open = false });
                ret.Add(new VCheckItem() { id = "I_JGYSTZ", pId = "G_ZLYS", name = "竣工验收通知书(" + item.SumJGYSJL + ")", isParent = false, cevent = "I_JGYSTZ", open = false });

                ret.Add(new VCheckItem() { id = "G_JCBG", pId = "", name = "检测报告查询", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_BHGBG", pId = "G_JCBG", name = "不合格检测报告(" + bhgbg + ")", isParent = false, cevent = "I_BHGBG", open = false });
                ret.Add(new VCheckItem() { id = "I_SYBG", pId = "G_JCBG", name = "所有检测报告(" + sybg + ")", isParent = false, cevent = "I_SYBG", open = false });
                //ret.Add(new VCheckItem() { id = "G_QTZL", pId = "", name = "其他资料", isParent = true, cevent = "", open = true });

                //ret.Add(new VCheckItem() { id = "I_JLYB", pId = "G_QTZL", name = "监理月报(0)", isParent = false, cevent = "I_JLYB", open = false });

                //ret.Add(new VCheckItem() { id = "0605", pId = "06", name = "扣分清单(0)", isParent = false, cevent = "0605", open = false });
                //ret.Add(new VCheckItem() { id = "0606", pId = "06", name = "现场图片(0)", isParent = false, cevent = "0606", open = false });
                ret.Add(new VCheckItem() { id = "I_JDBG", pId = "", name = "工程质量监督报告(" + jdbgsum + ")", isParent = false, cevent = "I_JDBG", open = true });
                ret.Add(new VCheckItem() { id = "I_RYDD", pId = "", name = "工程人员调动记录(" + item.SumRYLZJL + ")", isParent = false, cevent = "I_RYDD", open = true });
                ret.Add(new VCheckItem() { id = "I_DWDD", pId = "", name = "工程单位调换记录(" + item.SumQYLZJL + ")", isParent = false, cevent = "I_DWDD", open = true });
                ret.Add(new VCheckItem() { id = "I_GCBZ", pId = "", name = "监督人员备注(" + item.SumJDYBZ + ")", isParent = false, cevent = "I_GCBZ", open = true });
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
        #endregion

        #region 抓拍
        public void GetZPToken()
        {
            bool ret = true;
            string msg = "";
            string info = "{}";

            try
            {
                string url = "http://112.16.114.227:81/ipms/subSystem/generate/token?userName=system";
                info = SendDataByGET(url);


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
                Response.Write(info);
                Response.End();
            }
        }

        public void GetZPImages()
        {
            bool ret = true;
            string msg = "";
            string info = "{}";

            try
            {
                string token = Request["token"].GetSafeString();
                string carDirect = Request["carDirect"].GetSafeString();
                int pageNum = Request["pageNum"].GetSafeInt(1);
                int pageSize = Request["pageSize"].GetSafeInt(20);
                string carNum = Request["carNum"].GetSafeString();
                string url = "http://112.16.114.227:81/ipms/carcapture/find/conditions";
                url += "?carDirect=" + carDirect + "&pageNum=" + pageNum + "&pageSize=" + pageSize + "&carNumLikeStr=" + carNum;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("accessToken", token);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                info = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();


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
                Response.Write(info);
                Response.End();
            }
        }
        #endregion

        #region 工程位置标注

        // 工程位置标注
        public void SetGcbz()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {

                    string gcbh = Request["gcbh"].GetSafeString("");
                    if (gcbh == "")
                    {
                        ret = false;
                        msg = "工程不能为空！";
                    }
                    else
                    {
                        string[] location = Request["location"].GetSafeString("").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (location.Length > 1)
                        {
                            string longitude = location[0].GetSafeString();
                            string latitude = location[1].GetSafeString();
                            if (longitude != "" && latitude != "")
                            {
                                string gczb = "";
                                gczb = longitude + "," + latitude;

                                float v = float.Parse(longitude);
                                if (v < 90)
                                {
                                    gczb = latitude + "," + longitude;
                                }
                                string sql = string.Format("update i_m_gc set gczb='{0}' where gcbh='{1}'", gczb, gcbh);
                                ret = CommonService.ExecSql(sql, out msg);
                            }
                            else
                            {
                                ret = false;
                                msg = "缺少经度或纬度信息！";
                            }

                        }
                        else
                        {
                            ret = false;
                            msg = "标注失败，请确认设置（网络状态，GPS是否开启）。请稍后重试";
                        }

                    }


                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

        }

        #endregion

        #region 设备使用登记 办理任务
        [Authorize]
        public void CheckSBSYJDZBL()
        {
            bool ret = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string serial = Request["serial"].GetSafeString();
                if (serial != "")
                {
                    string sql = string.Format("select * from sttodotasks where taskstatus=3 and serialno='{0}' and userid='{1}'", serial, CurrentUser.UserName);
                    IList<IDictionary<string, object>> tasklist = CommonService.GetDataTable2(sql);
                    if (tasklist.Count > 0)
                    {
                        int taskid = tasklist[0]["taskid"].GetSafeInt();
                        data.Add("taskid", taskid);
                    }
                    else
                    {
                        ret = false;
                        msg = "您不能办理当前任务！";
                    }
                }
                else
                {
                    ret = false;
                    msg = "任务信息缺失，无法办理！";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }

        #endregion

        #region APP首页设备统计
        public void GetSBTJ()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                string sql = "";
                string basb = "0";
                sql = "select count(1) as num from dbo.INFO_CQBA where (serialno is null or serialno='') ";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    basb = retdt[0]["num"];

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("name", "备案");
                di.Add("value", basb);
                dt.Add(di);

                string zysb = "0";
                sql = "select count(1) as num from SB_ReportSBSY where state!=2  ";
                retdt = CommonService.GetDataTable(sql);
                if (null != retdt && retdt.Count != 0)
                    zysb = retdt[0]["num"];
                IDictionary<string, string> di2 = new Dictionary<string, string>();
                di2.Add("name", "在用");
                di2.Add("value", zysb);
                dt.Add(di2);



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
                    string[] info = idlist.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (info.Length > 0)
                    {
                        foreach (var item in info)
                        {
                            string[] idinfo = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (idinfo.Length == 2)
                            {
                                int id = idinfo[0].GetSafeInt();
                                int readerid = idinfo[1].GetSafeInt();
                                code = OaService.DeleteMail(id, readerid, CurrentUser.UserName, out msg);
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

        #region 录入质量监督方案流程打开录入界面

        public ActionResult Lrzljdfa()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            string returnurl = Request["Returnurl"].GetSafeString();
            ViewBag.returnurl = returnurl;
            ViewBag.gcbh = gcbh;
            return View();
        }
        /// <summary>
        /// 根据工程编号获取工程信息
        /// </summary>
        public void getgcdetail()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh != "")
                {
                    string sql = string.Format("select * from view_i_m_gc_lb where gcbh='{0}'", gcbh);
                    data = CommonService.GetDataTable2(sql);

                }
                else
                {
                    ret = false;
                    msg = "缺少工程信息！";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }


        public ActionResult Lraqjdfa()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            string returnurl = Request["Returnurl"].GetSafeString();
            ViewBag.returnurl = returnurl;
            ViewBag.gcbh = gcbh;
            return View();
        }


        #endregion

        #region 手机APP 人员签到

        public void SaveKQXX()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string title = Request["title"].GetSafeString("");
                    List<string> fileidlist = new List<string>();

                    // 保存附件
                    HttpFileCollectionBase files = Request.Files;

                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase postfile = files[i];
                            string filename = postfile.FileName;
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
                            if (file.Fileid > 0)
                            {
                                fileidlist.Add(string.Format("{0}|{1}", file.FileOrgName, file.Fileid.ToString()));
                            }
                        }

                    }

                    string location = "";
                    string[] loc = Request["location"].GetSafeString("").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (loc.Length == 2)
                    {
                        // 经度，纬度
                        location = loc[1] + "," + loc[0];
                    }
                    // 保存考勤信息
                    string sql = string.Format("insert into JDBG_RY_KQXX( USERCODE, REALNAME,LOCATION,SJ,TITLE,FJLIST) VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}')",
                            CurrentUser.UserName,
                            CurrentUser.RealName,
                            location,
                            title,
                            string.Join("||", fileidlist)
                        );
                    ret = CommonService.ExecSql(sql, out msg);

                }
                else
                {
                    ret = false;
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }
        public void GetKQXXList()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            bool code = true;
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    int pageIndex = Request["page"].GetSafeInt(1);


                    string strSql = "select * from view_jdbg_ry_kqxx where usercode='" + CurrentUser.UserName + "'";
                    strSql += " order by sj desc ";

                    dt = CommonService.GetPageData2(strSql, 10, pageIndex, out total);
                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total, jss.Serialize(dt)));
                Response.End();
            }

        }


        public void GetKQXXDetail()
        {
            bool ret = true;
            string msg = "";
            bool code = true;
            string data = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();

            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string id = Request["id"].GetSafeString("");
                    if (id != "")
                    {
                        string strSql = string.Format("select * from view_jdbg_ry_kqxx where recid='{0}'", id);
                        IList<IDictionary<string, string>> kqxxlist = CommonService.GetDataTable(strSql);
                        if (kqxxlist.Count > 0)
                        {
                            IDictionary<string, string> kqxx = kqxxlist[0];
                            string recid = kqxx["recid"];
                            string title = kqxx["title"];
                            string sj = kqxx["sy_lrsj"];

                            string fjms = kqxx["fjlist"];

                            data += "<font style = 'font-weight:bold;font-style:italic' > 详情：</font ><br/>" + "工程：" + title + "<br/>" + "考勤时间：" + sj + "<br/>";
                            if (fjms != "")
                            {
                                string fj = "";
                                string[] fjlist = fjms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                                if (fjlist.Length > 0)
                                {
                                    for (int i = 0; i < fjlist.Length; i++)
                                    {
                                        string[] file = fjlist[i].Split('|');
                                        if (file.Length > 0)
                                        {
                                            fj += "<a href='http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString() + "/workflow/fileview?id=" + file[1] + "' urltype='openfile'>" + file[0] + "</a><br/>";
                                        }

                                    }
                                }
                                fj = "<br/><font style='font-weight:bold;font-style:italic'>现场照片：</font><br/>" + fj;
                                data += fj;
                            }
                            //SysLog4.WriteError(data);
                        }


                    }


                }
                else
                {
                    ret = false;
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": \"{2}\"}}", ret ? "0" : "1", msg, data));
                Response.End();
            }

        }
        #endregion

        #region 质安监门户网站统计接口
        public void GetZAJGCTJXX()
        {
            bool ret = true;
            string msg = "";
            IDictionary<string, string> data = new Dictionary<string, string>();

            try
            {
                string procstr = "GetZAJGCTJXX()";
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data = dt[0];
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        public void GetZAJWXY()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                string sql = "select top 10 * from dbo.VIEW_GCLR_WXY where State=3 order by realdate desc";
                data = CommonService.GetDataTable(sql);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        public void GetZAJSBSY()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();

            try
            {
                string sql = "select top 10 * from dbo.VIEW_SB_ReportSBSY where State=1 and UnInstallProgStatus=-1 order by SY_SYBARQ desc";
                data = CommonService.GetDataTable(sql);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }
        #endregion

        #region 监控中心（三级菜单）
        public void GetVideoTreeBySearchType2()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            int total = 0;
            try
            {
                string type = Request["type"].GetSafeString();
                if (type != "")
                {
                    if (type == "gc")
                    {
                        // 获取所有有效的摄像头
                        string sql = "select * from i_s_gc_video_channel where sfyx=1 ";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);

                        if (d.Count > 0)
                        {
                            // 根据街道分类，获取每个街道的工程摄像头
                            List<string> szjdlist = d.Select(x => x["szjd"].GetSafeString()).OrderBy(x => x).Distinct().ToList();
                            List<string> reorderjdlist = new List<string>() {
                                "暨阳街道","浣东街道","陶朱街道","大唐街道"
                            };

                            // 将特定的街道放在最前面
                            for (int i = reorderjdlist.Count - 1; i >= 0; i--)
                            {
                                szjdlist.Remove(reorderjdlist[i]);
                                szjdlist.Insert(0, reorderjdlist[i]);
                            }

                            foreach (var szjd in szjdlist)
                            {


                                // 生成每个街道的节点信息
                                Dictionary<string, object> node = new Dictionary<string, object>();
                                List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
                                // 处理街道名称
                                node.Add("szjd", szjd);

                                // 处理街道下面的所有的工程
                                List<IDictionary<string, object>> allgclist = d.Where(x => x["szjd"].GetSafeString() == szjd).ToList();
                                var g = allgclist.GroupBy(x => new { gcbh = x["gcbh"].GetSafeString(), gcmc = x["gcmc"].GetSafeString() }).Select(x => x.Key).ToList();
                                total += g.Count;
                                foreach (var item in g)
                                {
                                    Dictionary<string, object> gn = new Dictionary<string, object>();
                                    Dictionary<string, object> n2 = new Dictionary<string, object>();
                                    List<Dictionary<string, object>> c2 = new List<Dictionary<string, object>>();
                                    // 某个工程下面的摄像头
                                    List<IDictionary<string, object>> chns = allgclist.Where(x => x["gcbh"].GetSafeString() == item.gcbh).ToList();
                                    n2.Add("name", item.gcmc);
                                    n2.Add("total", chns.Count);
                                    n2.Add("type", type);
                                    n2.Add("data", new Dictionary<string, object>() {
                                        { "gcbh", item.gcbh},
                                        { "gcmc", item.gcmc},
                                        { "total", chns.Count}
                                    });
                                    gn.Add("node", n2);

                                    foreach (var chn in chns)
                                    {
                                        c2.Add(new Dictionary<string, object>() {
                                            { "name", chn["channelname"].GetSafeString()},
                                            { "type", type},
                                            { "data", chn}
                                        });
                                    }
                                    gn.Add("children", c2);
                                    children.Add(gn);

                                }
                                // 把街道节点信息添加到列表中
                                dt.Add(new Dictionary<string, object>() {
                                    { "node", node},
                                    { "children", children}
                                });



                            }
                        }
                    }
                    else if (type == "camera")
                    {
                        string sql = "select cameratype,max(cameratypename) as cameratypename, count(*) as total from view_i_s_gc_video_channel " +
                                 "group by cameratype order by cameratype desc";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["cameratypename"]);
                            node.Add("total", row["total"]);
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);
                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from view_i_s_gc_video_channel where cameratype='{0}'", row["cameratype"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["channelname"]);
                                cnode.Add("type", type);
                                cnode.Add("data", cdrow);
                                children.Add(cnode);
                            }
                            item.Add("children", children);
                            dt.Add(item);
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
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2},\"total\": {3}}}", ret ? "0" : "1", msg, jss.Serialize(dt), total.ToString()));
                Response.End();
            }
        }

        public void GetVideoTreeBySearchType3()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            int total = 0;
            try
            {
                string type = Request["type"].GetSafeString();
                if (type != "")
                {
                    if (type == "gc")
                    {
                        // 获取所有有效的摄像头
                        string sql = "select * from i_s_gc_video_channel where sfyx=1 and isnewpt=1 ";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);

                        if (d.Count > 0)
                        {
                            // 根据街道分类，获取每个街道的工程摄像头
                            List<string> szjdlist = d.Select(x => x["szjd"].GetSafeString()).OrderBy(x => x).Distinct().ToList();
                            List<string> reorderjdlist = new List<string>() {
                                "暨阳街道","浣东街道","陶朱街道","大唐街道"
                            };

                            // 将特定的街道放在最前面
                            for (int i = reorderjdlist.Count - 1; i >= 0; i--)
                            {
                                if (szjdlist.Contains(reorderjdlist[i]))
                                {
                                    szjdlist.Remove(reorderjdlist[i]);
                                    szjdlist.Insert(0, reorderjdlist[i]);
                                }

                            }

                            foreach (var szjd in szjdlist)
                            {


                                // 生成每个街道的节点信息
                                Dictionary<string, object> node = new Dictionary<string, object>();
                                List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
                                // 处理街道名称
                                node.Add("szjd", szjd);

                                // 处理街道下面的所有的工程
                                List<IDictionary<string, object>> allgclist = d.Where(x => x["szjd"].GetSafeString() == szjd).ToList();
                                var g = allgclist.GroupBy(x => new { gcbh = x["gcbh"].GetSafeString(), gcmc = x["gcmc"].GetSafeString() }).Select(x => x.Key).ToList();
                                total += g.Count;
                                foreach (var item in g)
                                {
                                    Dictionary<string, object> gn = new Dictionary<string, object>();
                                    Dictionary<string, object> n2 = new Dictionary<string, object>();
                                    List<Dictionary<string, object>> c2 = new List<Dictionary<string, object>>();
                                    // 某个工程下面的摄像头
                                    List<IDictionary<string, object>> chns = allgclist.Where(x => x["gcbh"].GetSafeString() == item.gcbh).ToList();
                                    n2.Add("name", item.gcmc);
                                    n2.Add("total", chns.Count);
                                    n2.Add("type", type);
                                    n2.Add("data", new Dictionary<string, object>() {
                                        { "gcbh", item.gcbh},
                                        { "gcmc", item.gcmc},
                                        { "total", chns.Count}
                                    });
                                    gn.Add("node", n2);

                                    foreach (var chn in chns)
                                    {
                                        c2.Add(new Dictionary<string, object>() {
                                            { "name", chn["channelname"].GetSafeString()},
                                            { "type", type},
                                            { "data", chn}
                                        });
                                    }
                                    gn.Add("children", c2);
                                    children.Add(gn);

                                }
                                // 把街道节点信息添加到列表中
                                dt.Add(new Dictionary<string, object>() {
                                    { "node", node},
                                    { "children", children}
                                });



                            }
                        }
                    }
                    else if (type == "camera")
                    {
                        string sql = "select cameratype,max(cameratypename) as cameratypename, count(*) as total from view_i_s_gc_video_channel " +
                                 "group by cameratype order by cameratype desc";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["cameratypename"]);
                            node.Add("total", row["total"]);
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);
                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from view_i_s_gc_video_channel where cameratype='{0}'", row["cameratype"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["channelname"]);
                                cnode.Add("type", type);
                                cnode.Add("data", cdrow);
                                children.Add(cnode);
                            }
                            item.Add("children", children);
                            dt.Add(item);
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
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2},\"total\": {3}}}", ret ? "0" : "1", msg, jss.Serialize(dt), total.ToString()));
                Response.End();
            }
        }

        // 获取企业相关的工地视频
        [Authorize]
        public void QYGetVideoTree2()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = Request["type"].GetSafeString();
                if (type != "")
                {
                    if (type == "gc")
                    {
                        // 获取所有有效的摄像头
                        string sql = "select * from i_s_gc_video_channel where sfyx=1 ";
                        sql += string.Format(" and gcbh in (select distinct gcbh from view_i_s_gc_allqy where qybh in (select qybh from i_m_qyzh where yhzh='{0}'))", CurrentUser.UserName);
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        if (d.Count > 0)
                        {
                            // 根据街道分类，获取每个街道的工程摄像头
                            List<string> szjdlist = d.Select(x => x["szjd"].GetSafeString()).OrderBy(x => x).Distinct().ToList();
                            foreach (var szjd in szjdlist)
                            {
                                // 生成每个街道的节点信息
                                Dictionary<string, object> node = new Dictionary<string, object>();
                                List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
                                // 处理街道名称
                                node.Add("szjd", szjd);

                                // 处理街道下面的所有的工程
                                List<IDictionary<string, object>> allgclist = d.Where(x => x["szjd"].GetSafeString() == szjd).ToList();
                                var g = allgclist.GroupBy(x => new { gcbh = x["gcbh"].GetSafeString(), gcmc = x["gcmc"].GetSafeString() }).Select(x => x.Key).ToList();
                                foreach (var item in g)
                                {
                                    Dictionary<string, object> gn = new Dictionary<string, object>();
                                    Dictionary<string, object> n2 = new Dictionary<string, object>();
                                    List<Dictionary<string, object>> c2 = new List<Dictionary<string, object>>();
                                    // 某个工程下面的摄像头
                                    List<IDictionary<string, object>> chns = allgclist.Where(x => x["gcbh"].GetSafeString() == item.gcbh).ToList();
                                    n2.Add("name", item.gcmc);
                                    n2.Add("total", chns.Count);
                                    n2.Add("type", type);
                                    n2.Add("data", new Dictionary<string, object>() {
                                        { "gcbh", item.gcbh},
                                        { "gcmc", item.gcmc},
                                        { "total", chns.Count}
                                    });
                                    gn.Add("node", n2);

                                    foreach (var chn in chns)
                                    {
                                        c2.Add(new Dictionary<string, object>() {
                                            { "name", chn["channelname"].GetSafeString()},
                                            { "type", type},
                                            { "data", chn}
                                        });
                                    }
                                    gn.Add("children", c2);
                                    children.Add(gn);

                                }
                                // 把街道节点信息添加到列表中
                                dt.Add(new Dictionary<string, object>() {
                                    { "node", node},
                                    { "children", children}
                                });



                            }
                        }
                    }
                    else if (type == "camera")
                    {
                        string sql = "select cameratype,max(cameratypename) as cameratypename, count(*) as total from view_i_s_gc_video_channel " +
                                 "group by cameratype order by cameratype desc";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["cameratypename"]);
                            node.Add("total", row["total"]);
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);
                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from view_i_s_gc_video_channel where cameratype='{0}'", row["cameratype"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["channelname"]);
                                cnode.Add("type", type);
                                cnode.Add("data", cdrow);
                                children.Add(cnode);
                            }
                            item.Add("children", children);
                            dt.Add(item);
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
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        // 获取人员相关的工地视频
        [Authorize]
        public void RYGetVideoTree2()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = Request["type"].GetSafeString();
                if (type != "")
                {
                    if (type == "gc")
                    {
                        // 获取所有有效的摄像头
                        string sql = "select * from i_s_gc_video_channel where sfyx=1 ";
                        sql += string.Format(" and gcbh in (select distinct gcbh from view_i_s_gc_allry where rybh in (select qybh from i_m_qyzh where yhzh='{0}'))", CurrentUser.UserName);
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);

                        if (d.Count > 0)
                        {
                            // 根据街道分类，获取每个街道的工程摄像头
                            List<string> szjdlist = d.Select(x => x["szjd"].GetSafeString()).OrderBy(x => x).Distinct().ToList();
                            foreach (var szjd in szjdlist)
                            {


                                // 生成每个街道的节点信息
                                Dictionary<string, object> node = new Dictionary<string, object>();
                                List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
                                // 处理街道名称
                                node.Add("szjd", szjd);

                                // 处理街道下面的所有的工程
                                List<IDictionary<string, object>> allgclist = d.Where(x => x["szjd"].GetSafeString() == szjd).ToList();
                                var g = allgclist.GroupBy(x => new { gcbh = x["gcbh"].GetSafeString(), gcmc = x["gcmc"].GetSafeString() }).Select(x => x.Key).ToList();
                                foreach (var item in g)
                                {
                                    Dictionary<string, object> gn = new Dictionary<string, object>();
                                    Dictionary<string, object> n2 = new Dictionary<string, object>();
                                    List<Dictionary<string, object>> c2 = new List<Dictionary<string, object>>();
                                    // 某个工程下面的摄像头
                                    List<IDictionary<string, object>> chns = allgclist.Where(x => x["gcbh"].GetSafeString() == item.gcbh).ToList();
                                    n2.Add("name", item.gcmc);
                                    n2.Add("total", chns.Count);
                                    n2.Add("type", type);
                                    n2.Add("data", new Dictionary<string, object>() {
                                        { "gcbh", item.gcbh},
                                        { "gcmc", item.gcmc},
                                        { "total", chns.Count}
                                    });
                                    gn.Add("node", n2);

                                    foreach (var chn in chns)
                                    {
                                        c2.Add(new Dictionary<string, object>() {
                                            { "name", chn["channelname"].GetSafeString()},
                                            { "type", type},
                                            { "data", chn}
                                        });
                                    }
                                    gn.Add("children", c2);
                                    children.Add(gn);

                                }
                                // 把街道节点信息添加到列表中
                                dt.Add(new Dictionary<string, object>() {
                                    { "node", node},
                                    { "children", children}
                                });



                            }
                        }
                    }
                    else if (type == "camera")
                    {
                        string sql = "select cameratype,max(cameratypename) as cameratypename, count(*) as total from view_i_s_gc_video_channel " +
                                 "group by cameratype order by cameratype desc";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["cameratypename"]);
                            node.Add("total", row["total"]);
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);
                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from view_i_s_gc_video_channel where cameratype='{0}'", row["cameratype"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);
                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["channelname"]);
                                cnode.Add("type", type);
                                cnode.Add("data", cdrow);
                                children.Add(cnode);
                            }
                            item.Add("children", children);
                            dt.Add(item);
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
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        // 获取某个工程的有效摄像头
        public void GetVideoChannelByGcbh()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            int total = 0;
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh != "")
                {
                    // 获取所有有效的摄像头
                    string sql = "select * from i_s_gc_video_channel where sfyx=1 and gcbh='{0}' ";
                    sql = string.Format(sql, gcbh);
                    dt = CommonService.GetDataTable2(sql);
                    total = dt.Count;
                }
                else
                {
                    ret = false;
                    msg = "参数错误！";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2},\"total\": {3}}}", ret ? "0" : "1", msg, jss.Serialize(dt), total.ToString()));
                Response.End();
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
                SysLog4.WriteError(jss.Serialize(data));
                Response.Write(string.Format("{{\"success\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "true" : "false", msg, jss.Serialize(info)));
                Response.End();
            }
        }
        #endregion

        #region 获取检测报告 数量和记录
        // 不合格报告
        public void GetBHGBG()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                int page = 1;
                int pageSize = Request["pagesize"].GetSafeInt(5);
                Dictionary<string, string> info = new Dictionary<string, string>() {
                    {"page", page.ToString() },
                    {"pagesize", pageSize.ToString() },
                    {"zjdjh", "" },
                    {"jcjg", "2" }
                };
                ret = JcjgBgService.GetBHGBG("ZJJCJG", info, out data, out total, out msg);
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

                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\":{2},\"data\": {3}}}", ret ? "0" : "1", msg, total.ToString(), jss.Serialize(data)));
                Response.End();
            }
        }

        /// <summary>
        /// 根据委托单唯一号获取报告文件列表
        /// </summary>
        public void GetBGList()
        {
            bool ret = true;
            string msg = "";
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                string wtdwyh = Request["wtdwyh"].GetSafeString();
                Dictionary<string, string> info = new Dictionary<string, string>() {
                    {"wtdwyh", wtdwyh}
                };
                ret = JcjgBgService.GetBGList("ZJJCJG", info, out data, out msg);
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

                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
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

        #region 新闻网站 等效龄期接口
        public void Get600days()
        {
            bool ret = true;
            string msg = "";
            string data = "";
            try
            {
                data = WeatherService.Get600days();
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }
        #endregion


        #region 质监站曝光台信息同步到新闻网站
        public void SavePGToXWWZ()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string retstring = "";
                string url = "http://zjzhjg.jzyglxt.com/dwgxzj/SavePGTest";
                Dictionary<string, string> datas = new Dictionary<string, string>();
                Dictionary<string, Dictionary<string, byte[]>> files = new Dictionary<string, Dictionary<string, byte[]>>();
                datas.Add("title", "某某工程测试-xjj");
                ret = MyHttp.Post(url, datas, files, out retstring);
                if (ret)
                {
                    if (retstring != "")
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                        if (retdata != null)
                        {
                            string code = retdata["code"].GetSafeString();
                            msg = retdata["msg"].GetSafeString();
                            ret = code == "0";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(msg);
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
        /// <summary>
        /// 将曝光台的新闻同步到新闻网站
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool SyncPGToXWWZ(Dictionary<string, string> datas, Dictionary<string, Dictionary<string, byte[]>> files, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string retstring = "";
                //string url = "http://zjzhjg.jzyglxt.com/dwgxzj/SavePGTest";
                string url = Configs.GetConfigItem("xwwzsavepgturl");
                ret = MyHttp.Post(url, datas, files, out retstring);
                if (ret)
                {
                    if (retstring != "")
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                        if (retdata != null)
                        {
                            ret = retdata["success"].GetSafeBool();
                            msg = retdata["msg"].GetSafeString();

                        }
                    }
                }


            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(msg);
            }

            return ret;




        }

        private Dictionary<string, Dictionary<string, byte[]>> GetPGFiles(HttpFileCollectionBase files)
        {
            Dictionary<string, Dictionary<string, byte[]>> pgfiles = new Dictionary<string, Dictionary<string, byte[]>>();

            if (files.Count > 0)
            {
                Dictionary<string, byte[]> allimgs = new Dictionary<string, byte[]>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string filename = file.FileName;
                    byte[] postcontent = new byte[file.ContentLength];
                    int readlength = 0;
                    while (readlength < file.ContentLength)
                    {
                        int tmplen = file.InputStream.Read(postcontent, readlength, file.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    file.InputStream.Seek(0, SeekOrigin.Begin);

                    allimgs.Add(filename, postcontent);
                    if (i == 0)
                    {
                        byte[] fimg = new byte[postcontent.Length];
                        Array.Copy(postcontent, fimg, postcontent.Length);
                        pgfiles.Add("imagelink", new Dictionary<string, byte[]>() {
                            { "bgpgt" + filename, fimg}
                        });
                    }




                }

                //SysLog4.WriteError("开始输出字节长度");
                //foreach (var img in allimgs)
                //{
                //    SysLog4.WriteError(img.Value.Length.ToString());
                //}
                //SysLog4.WriteError("结束输出字节长度");
                //SysLog4.WriteError(allimgs.Count.ToString());
                pgfiles.Add("pgtfilelist[]", allimgs);
                //pgfiles.Add("pgtfilelist2[]", allimgs);
            }
            return pgfiles;
        }

        // 保存曝光台
        public void SavePGTest()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string body = "";
                string title = Request["title"].GetSafeString("");
                string imageName = "";
                // 保存article
                NewsArtcle article = new NewsArtcle();
                article.Categoryid = 101;
                article.Templateid = 3;
                article.ArticleTitle = title;
                article.ArticleKey = "";
                article.ArticleFrom = "本站原创";
                article.ArticleDate = DateTime.Now;
                article.ArticleContent = body;
                article.IsRecommand = false;
                article.IsImage = true;
                article.IsLink = false;
                article.IsAudited = false;
                article.CreatedOn = DateTime.Now;
                article.CreatedBy = "xxx-xjj";
                article.Hits = 0;
                article.ImageUrl = imageName;

                article = NewsArtcleService.Save(article);


                body += "<p><font size='5'><strong>我站于" + DateTime.Now.ToLongDateString() + "对" + title + "工程进行监督检查，发现存在安全隐患较多，现通报如下</strong>：</font></p>";
                HttpFileCollectionBase files = Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string tmpName = "";

                        bool upload = SaveUserFile(article.Articleid, files[i], ".*", 20480, out tmpName);
                        if (upload)
                        {
                            string temms = Request.Form["ms" + i.ToString()].GetSafeString("");

                            if (Request.Form[files[i].FileName].GetSafeString("") != "")
                                temms = Request.Form[files[i].FileName].GetSafeString("");
                            else
                                temms = Request.Form["ms" + i.ToString()].GetSafeString("");
                            //这里还没写完，做到兼容新老，万一哪个二笔没更新也在用
                            body += "<p align='center'><font size='3'>" + temms + "</font></p><p align='center'><img alt='' width='480' height='289' src='/dwgxzj/getAttachFile?id=" + tmpName + "' /></p>";

                            if (i == 0)
                            {
                                imageName = tmpName;
                            }
                        }
                    }

                }

                article.ArticleContent = body;
                article.ImageUrl = imageName;

                NewsArtcleService.Update(article);

                ret = true;


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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }

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
                string folderid = Request["folderid"].GetSafeString();
                string foldertype = Request["foldertype"].GetSafeString(ShareFolderType.All);
                string username = CurrentUser.UserName;
                string key = Request["key"].GetSafeString();
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

        #region 企业月报表未申报年份选择
        public ActionResult ymxz()
        {
            return View();
        }
        #endregion

        #region 企业季报表未申报季度选择
        public ActionResult jdxz()
        {
            return View();
        }
        #endregion

        #region 手动上传报告
        public ActionResult UploadReportSD()
        {
            return View();
        }

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
                    HttpPostedFileBase file = files[i];
                    // 文件名
                    string strSaveName = file.FileName;
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
                        dt = GetBGInfo(ext, file);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        // 根据文件类型，从文件中提取报告内容
        private Dictionary<string, object> GetBGInfo(string fileext, HttpPostedFileBase file)
        {
            Dictionary<string, object> info = new Dictionary<string, object>();
            try
            {
                if (fileext == ".docx" || fileext == ".doc")
                {
                    // 加载报告
                    StringBuilder sb = new StringBuilder();
                    XWPFDocument doc = new XWPFDocument(file.InputStream);

                    // 获取报告字段配置项
                    //string sdscbgconfig = Configs.GetConfigItem("sdscbg", "sdscbgconfigs.xml").GetSafeString();
                    //if (sdscbgconfig != "")
                    //{
                    //    JavaScriptSerializer jss = new JavaScriptSerializer();
                    //    jss.MaxJsonLength = Int32.MaxValue;
                    //    ArrayList ar = jss.Deserialize<ArrayList>(sdscbgconfig);
                    //    foreach (var e in ar)
                    //    {
                    //        var bgc = (Dictionary<string,object>) e;
                    //        if (bgc!=null && bgc.Count > 0)
                    //        {
                    //            string bglx = bgc["bglx"].GetSafeString();
                    //            int bglxposition = bgc["bglxposition"].GetSafeInt();
                    //            string t = doc.Paragraphs[bglxposition].ParagraphText;
                    //            // 找到了
                    //            if (t.Trim().Replace(" ","") == bglx)
                    //            {
                    //                info.Add("bglx", bglx);
                    //                ArrayList bgfields = (ArrayList)bgc["bgfields"];
                    //                if (bgfields != null && bgfields.Count > 0)
                    //                {
                    //                    foreach (Dictionary<string,object> f in bgfields)
                    //                    {
                    //                        string fieldname = f["name"].GetSafeString();
                    //                        int position = f["position"].GetSafeInt();
                    //                        string prefix = f["prefix"].GetSafeString();
                    //                        t = doc.Paragraphs[position].ParagraphText.Trim();
                    //                        if (prefix !="")
                    //                        {
                    //                            t = t.Replace(" ", "").Replace(prefix,"");
                    //                        }
                    //                        if (info.ContainsKey(fieldname))
                    //                        {
                    //                            info[fieldname] = t;
                    //                        }
                    //                        else
                    //                        {
                    //                            info.Add(fieldname, t);
                    //                        }

                    //                    }
                    //                }
                    //                break;
                    //            }
                    //            else
                    //            {
                    //                continue;
                    //            }



                    //        }

                    //    }
                    //}

                    for (int i = 0; i < doc.Tables.Count; i++)
                    {

                        XWPFTable t = doc.Tables[i];
                        int rc = t.NumberOfRows;
                        List<XWPFTableRow> rows = t.Rows;
                        for (int j = 0; j < rows.Count; j++)
                        {
                            var row = rows[j];
                            var cells = row.GetTableCells();
                            for (int k = 0; k < cells.Count; k++)
                            {
                                var cell = cells[k];
                                IList<XWPFParagraph> plist = cell.Paragraphs;
                                for (int l = 0; l < plist.Count; l++)
                                {
                                    var p = plist[l];
                                    var rr = p.CreateRun();

                                    for (int m = 0; m < p.Runs.Count; m++)
                                    {
                                        string txt = p.Runs[m].Text;
                                        sb.AppendLine(string.Format("t{0}:r{1}:c{2}:p{3}:r{4}:\r\n{5}", i.ToString(), j.ToString(), k.ToString(), l.ToString(), m.ToString(), txt));
                                    }

                                }

                            }
                        }


                    }

                    SysLog4.WriteError(sb.ToString());
                }


            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return info;
        }
        #endregion

        #region weblist第三方接口杂类函数

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private string DustGetToken()
        {
            return Dust.DustGetToken();
        }

        /// <summary>
        /// 修改扬尘过滤参数
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ForgeDustyFilterParams(List<KeyValuePair<string, string>> paramlist)
        {
            double dustydefaulthours = 1;
            if (!Double.TryParse(Configs.GetConfigItem("dustydefaulthours"), out dustydefaulthours))
            {
                dustydefaulthours = 1;
            }
            double dustydefaultlastminutes = 3;
            if (!Double.TryParse(Configs.GetConfigItem("dustydefaultlastminutes"), out dustydefaultlastminutes))
            {
                dustydefaultlastminutes = 3;
            }
            // 开始时间和结束时间
            string startdate = DateTime.Now.AddHours(-dustydefaulthours).ToString("yyyy-MM-dd HH:mm:ss");
            string enddate = DateTime.Now.AddMinutes(-dustydefaultlastminutes).ToString("yyyy-MM-dd HH:mm:ss");

            var q = paramlist.Where(x => x.Key.Equals("CurrentTime", StringComparison.OrdinalIgnoreCase));
            if (q.Count() > 0)
            {
                var p = q.First();
                string v = p.Value;
                if (v != "")
                {
                    string[] vl = v.Split('~');
                    if (vl.Length == 2)
                    {
                        startdate = vl[0] + " 00:00:00";
                        enddate = vl[1] + " 23:59:59";
                    }
                }
            }

            paramlist.Add(new KeyValuePair<string, string>("StartTime", Dust.getUnixTimestamp(DateTime.Parse(startdate)).ToString()));
            paramlist.Add(new KeyValuePair<string, string>("EndTime", Dust.getUnixTimestamp(DateTime.Parse(enddate)).ToString()));

            paramlist = ForgeGcbhParam(paramlist);
            return paramlist;
        }

        private List<KeyValuePair<string, string>> ForgeDustyWarningFilterParams(List<KeyValuePair<string, string>> paramlist)
        {
            paramlist = ForgeGcbhParam(paramlist);
            return paramlist;
        }

        private List<KeyValuePair<string, string>> ForgeGcbhParam(List<KeyValuePair<string, string>> paramlist)
        {
            
            var q = paramlist.Where(x => x.Key.Equals("gcbh", StringComparison.OrdinalIgnoreCase));
            if (q.Count() > 0)
            {
                var p = q.First();
                string gcbh = p.Value.GetSafeString().Trim();
                if (gcbh != "")
                {
                    string sql = string.Format("select devicecode from i_s_gc_dusty_device where gcbh='{0}'", gcbh);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        string devicecode = string.Join(",", dt.Select(x => x["devicecode"].GetSafeString()).ToList());
                        if (devicecode !="")
                        {
                            q = paramlist.Where(x => x.Key.Equals("devicecode", StringComparison.OrdinalIgnoreCase));
                            // devicecode参数不存在 添加devicecode参数
                            // 若已存在，则忽略
                            if (q.Count() == 0)
                            {
                                paramlist.Add(new KeyValuePair<string, string>("DeviceCode", devicecode));
                            }
                            
                        }
                    }
                }
            }
            return paramlist;
        }

        /// <summary>
        /// 修改扬尘设备列表过滤数据
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ForgeDustyDeviceFilterParams(List<KeyValuePair<string, string>> paramlist)
        {
            // 更改设备状态过滤参数
            string filterValue = "";
            var q = paramlist.Where(x => x.Key.Equals("ForgeStatus", StringComparison.OrdinalIgnoreCase));
            if (q.Count() > 0)
            {
                var p = q.First();
                string v = p.Value;
                if (v != "")
                {
                    if (v == "1")
                    {
                        filterValue = "1";
                    }
                    else
                    {
                        filterValue = "0";
                    }
                    paramlist.Add(new KeyValuePair<string, string>("Status", filterValue));
                    paramlist.Remove(p);
                }
            }

            paramlist = ForgeGcbhParam(paramlist);
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
            if (m != null)
            {
                ret = m.Invoke(this, parameters);
            }
            return ret;
        }

        private bool checkDustySuccess(object code)
        {
            return code.GetSafeInt() == 0;
        }

        private Dictionary<string, object> extraParamGetDustyToken(List<KeyValuePair<string, string>> paramlist)
        {
            bool code = true;
            string msg = "";
            string data = DustGetToken();
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

        /// <summary>
        /// 获取到远程的扬尘数据之后，添加工程相关信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> ForgeDustData(List<Dictionary<string, object>> data)
        {
            if (data != null)
            {
                Dictionary<string, IDictionary<string, object>> extradata = new Dictionary<string, IDictionary<string, object>>();
                List<string> deviceList = data.Select(x => x["devicecode"].GetSafeString()).Distinct().ToList();
                string sql = string.Format("select gcbh, gcmc,devicecode from i_s_gc_dusty_device where devicecode in ( " + string.Join(",", deviceList).FormatSQLInStr() + " )");
                //SysLog4.WriteError(sql);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                foreach (var devicecode in deviceList)
                {
                    extradata.Add(devicecode, dt.Where(x => x["devicecode"].GetSafeString() == devicecode).FirstOrDefault());
                }
                foreach (var item in data)
                {
                    var extra = extradata[item["devicecode"].GetSafeString()];
                    item["forgegcmc"] = extra == null ? "" : extra["gcmc"].GetSafeString();
                    item["forgegcbh"] = extra == null ? "" : extra["gcbh"].GetSafeString();
                }
            }

            return data;
        }
        /// <summary>
        /// 获取扬尘预警列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> ForgeDustWarningData(List<Dictionary<string, object>> data)
        {
            if (data != null)
            {
                Dictionary<string, IDictionary<string, object>> extradata = new Dictionary<string, IDictionary<string, object>>();
                List<string> deviceList = data.Select(x => x["devicecode"].GetSafeString()).Distinct().ToList();
                string sql = string.Format("select gcbh, gcmc,devicecode from i_s_gc_dusty_device where devicecode in ( " + string.Join(",", deviceList).FormatSQLInStr() + " )");
                //SysLog4.WriteError(sql);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                foreach (var devicecode in deviceList)
                {
                    extradata.Add(devicecode, dt.Where(x => x["devicecode"].GetSafeString() == devicecode).FirstOrDefault());
                }
                foreach (var item in data)
                {
                    var extra = extradata[item["devicecode"].GetSafeString()];
                    item["forgegcmc"] = extra == null ? "" : extra["gcmc"].GetSafeString();
                    item["forgegcbh"] = extra == null ? "" : extra["gcbh"].GetSafeString();
                }
            }

            return data;
        }

        /// <summary>
        /// 获取扬尘设备列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> ForgeDustDeviceData(List<Dictionary<string, object>> data)
        {
            if (data != null)
            {
                Dictionary<string, IDictionary<string, object>> extradata = new Dictionary<string, IDictionary<string, object>>();
                List<string> deviceList = data.Select(x => x["devicecode"].GetSafeString()).Distinct().ToList();
                string sql = string.Format("select gcbh, gcmc,devicecode from i_s_gc_dusty_device where devicecode in ( " + string.Join(",", deviceList).FormatSQLInStr() + " )");
                //SysLog4.WriteError(sql);
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                foreach (var devicecode in deviceList)
                {
                    extradata.Add(devicecode, dt.Where(x => x["devicecode"].GetSafeString() == devicecode).FirstOrDefault());
                }
                foreach (var item in data)
                {
                    var extra = extradata[item["devicecode"].GetSafeString()];
                    item["forgegcmc"] = extra == null ? "" : extra["gcmc"].GetSafeString();
                    item["forgegcbh"] = extra == null ? "" : extra["gcbh"].GetSafeString();
                    item["forgestatus"] = item["status"].GetSafeBool() ? "在线" : "离线";
                }
            }

            return data;
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
                if (serial != "")
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

        #region 扬尘设备
        public ActionResult ShowDustLocation()
        {
            return View();
        }
        #endregion

        #region 设备产权备案登记扫描查看
        public ActionResult ViewSbcqba()
        {
            string id = Request["id"].GetSafeString();
            ViewBag.id = id;
            return View();
        }

        public void GetSbcqbaByWybh()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string wybh = Request["wybh"].GetSafeString();
                if (wybh != "")
                {
                    data = DwgxZJService.GetSbcqbaByWybh(wybh);
                }
                else
                {
                    code = false;
                    msg = "唯一编号不能为空！";
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

        public ActionResult ViewSBReport()
        {
            string serial = Request["serial"].GetSafeString();
            string reporttype = Request["reporttype"].GetSafeString();
            ViewBag.serial = serial;
            ViewBag.reporttype = reporttype;
            return View();
        }

        public void GetSbReportFile()
        {
            bool code = false;
            string msg = "";
            byte[] file = null;
            try
            {
                string serial = Request["serial"].GetSafeString();
                string reporttype = Request["reporttype"].GetSafeString();
                if (serial != "" && reporttype != "")
                {
                    code = DwgxZJService.GetSbReportFile(serial, reporttype, out file, out msg);
                    if (code && file != null)
                    {
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                        Response.Charset = "UTF-8";
                        Response.ContentType = "application/pdf";
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
        #endregion

        #region 市外施工监理企业备案登记
        /// <summary>
        /// 删除市外施工项目
        /// </summary>
        public void DelSYSGXM()
        {
            bool ret = true;
            string msg = "";
            try
            {
                int recid = Request["recid"].GetSafeInt();
                if (recid <= 0)
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from JDBG_SYSGQYGCBA where recid={0}", recid);
                    ret = CommonService.ExecSql(sql, out msg);
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

        public void DelSYJLXM()
        {
            bool ret = true;
            string msg = "";
            try
            {
                int recid = Request["recid"].GetSafeInt();
                if (recid <= 0)
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from JDBG_SYJLQYGCBA where recid={0}", recid);
                    ret = CommonService.ExecSql(sql, out msg);
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

        public void DelMGGZBZJJNSQ()
        {
            bool ret = true;
            string msg = "";
            try
            {
                int recid = Request["recid"].GetSafeInt();
                if (recid <= 0)
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from JDBG_MGGZBZJJNSQ where recid={0}", recid);
                    ret = CommonService.ExecSql(sql, out msg);
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

        public void DelMGGZBZJTKSQ()
        {
            bool ret = true;
            string msg = "";
            try
            {
                int recid = Request["recid"].GetSafeInt();
                if (recid <= 0)
                {
                    ret = false;
                    msg = "参数错误！";
                }
                else
                {
                    string sql = string.Format("delete from JDBG_MGGZBZJTKSQ where recid={0}", recid);
                    ret = CommonService.ExecSql(sql, out msg);
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

        #endregion

        #region 导出市外中标企业
        public void ExportDJSGQY()
        {

            IList<IDictionary<string, string>> qylist = new List<IDictionary<string, string>>();
            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {
                string startdate = Request["t1"].GetSafeString();
                string enddate = Request["t2"].GetSafeString();
                #region 标题栏

                List<KeyValuePair<string, string>> heads = new List<KeyValuePair<string, string>>();

                string headsql = "select field, title from h_export_field where sfyx=1 and lx='SYSGQY' order by xssx";
                IList<IDictionary<string, string>> headdt = CommonService.GetDataTable(headsql);
                foreach (var item in headdt)
                {
                    heads.Add(new KeyValuePair<string, string>(item["field"].GetSafeString(), item["title"].GetSafeString()));
                }

                #endregion

                //居中样式
                ICellStyle cellstyle = wk.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.WrapText = true;

                //创建一个Sheet  
                ISheet sheet = wk.CreateSheet("市外企业标后备案");
                IRow row;
                NPOI.SS.UserModel.ICell cell;


                int cols = 0;
                int rows = 0;

                //定义导出标题
                row = sheet.CreateRow(rows);

                for (cols = 0; cols < heads.Count; cols++)
                {
                    sheet.SetColumnWidth(cols, 20 * 256);
                    // 定义每一列
                    cell = row.CreateCell(cols);
                    //设置值
                    cell.SetCellValue(heads[cols].Value);
                    //设置样式
                    cell.CellStyle = cellstyle;
                }


                // 获取数据
                string sql = string.Format("select * from VIEW_JDBG_SYSGQYGCBA where lrsj>='{0}' and lrsj<='{1}'", startdate, enddate);

                qylist = CommonService.GetDataTable(sql);
                //SysLog4.WriteError(qylist.Count.ToString());

                if (qylist.Count > 0)
                {
                    for (rows = 0; rows < qylist.Count; rows++)
                    {
                        IDictionary<string, string> data = qylist[rows];
                        row = sheet.CreateRow(rows + 1);
                        for (cols = 0; cols < heads.Count; cols++)
                        {
                            //定义每一列
                            cell = row.CreateCell(cols);
                            string content = heads[cols].Key.GetSafeString();
                            //SysLog4.WriteError(content);
                            Regex reg = new Regex(@"{{\w+}}", RegexOptions.IgnoreCase);
                            MatchCollection matchCol = reg.Matches(content);
                            foreach (Match matchItem in matchCol)
                            {
                                string mv = matchItem.Value.GetSafeString();
                                string f = mv.Replace("{", "").Replace("}", "");
                                if (f != "")
                                {
                                    content = content.Replace(mv, data[f]);
                                }
                            }


                            //设置值
                            cell.SetCellValue(content);
                            //设置样式
                            cell.CellStyle = cellstyle;
                        }


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
                    string fileName = HttpUtility.UrlEncode("市外企业标后备案" + DateTime.Now.ToString("yyyyMMdd"));
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }

            }
        }
        #endregion

        #region 删除企业经营区域

        public JsonResult deleteJYQY()
        {
            bool code = true;
            string msg = "";
            try
            {
                string id = Request["id"].GetSafeString();
                if (id != "")
                {
                    string procstr = string.Format("DeleteQYJYQY({0})", id);
                    code = CommonService.ExecProc(procstr, out msg);
                }
                else
                {
                    code = false;
                    msg = "参数错误！";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }

            return Json(new { code = code ? "0" : "1", msg = msg });
        }
        #endregion

        #region 导出市外施工企业、监理企业项目登记

        public ActionResult SGQYExport()
        {
            return View();
        }

        public ActionResult JLQYExport()
        {
            return View();
        }
        #endregion

        #region 现场会工地数据接口

        /// <summary>
        /// 根据gcbh获取工程基本信息
        /// </summary>
        public void GetGCInfo()
        {
            bool code = true;
            string msg = "";
            object data = null;
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh != "")
                {
                    string sql = string.Format("select * from view_i_m_gc where gcbh='{0}'", gcbh);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        data = dt[0];
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

        // 获取务工人员平台的工程编号
        public void getWGXTGCBH()
        {
            string ret = "";
            string err = "";
            string gcbh = Request["gcbh"].GetSafeString();
            try
            {
                ret = SendDataByGET("http://120.27.218.55:8001/Home/GetGCBH?GCBH_YC=" + gcbh + "&WGPTBH=ZJBG");
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

        // 应付工资
        public void getYFGZ()
        {
            //http://120.27.218.55:8001/gc/GetIndexMonthgztj
            string ret = "";
            string gcbh = Request["gcbh"].GetSafeString("LG006737");

            ret = Func.ZJWGRY.GetWgryTJ("http://120.27.218.55:8001/gc/GetIndexMonthgztj", "gcbh=" + gcbh);
            Response.Write(ret);
            //Response.Write("[{\"year\":2019,\"month\":4,\"yfMoney\":\"8.27\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":3,\"yfMoney\":\"14.38\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":2,\"yfMoney\":\"2.54\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":1,\"yfMoney\":\"5.82\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":12,\"yfMoney\":\"4.12\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":11,\"yfMoney\":\"3.26\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":10,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":9,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":8,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"}]");
            Response.End();
        }
        // 获取务工人员某个工程考勤信息
        public void GetSiteAttendanceTotal()
        {

            string ret = "";
            string gcbh = Request["gcbh"].GetSafeString("LG006737");

            ret = Func.ZJWGRY.GetWgryTJ("http://120.27.218.55/gc/GetSiteAttendanceTotal", "gcbh=" + gcbh);
            Response.Write(ret);
            //Response.Write("[{\"year\":2019,\"month\":4,\"yfMoney\":\"8.27\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":3,\"yfMoney\":\"14.38\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":2,\"yfMoney\":\"2.54\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":1,\"yfMoney\":\"5.82\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":12,\"yfMoney\":\"4.12\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":11,\"yfMoney\":\"3.26\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":10,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":9,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":8,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"}]");
            Response.End();
        }

        // 获取务工人员某个工程在岗人数统计
        public void GetGZZGRSList()
        {

            string ret = "";
            string gcbh = Request["gcbh"].GetSafeString("LG006737");

            ret = Func.ZJWGRY.GetWgryTJ("http://120.27.218.55/gc/GetGZZGRSList", "gcbh=" + gcbh);
            Response.Write(ret);
            //Response.Write("[{\"year\":2019,\"month\":4,\"yfMoney\":\"8.27\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":3,\"yfMoney\":\"14.38\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":2,\"yfMoney\":\"2.54\",\"sfMoney\":\"0.00\"},{\"year\":2019,\"month\":1,\"yfMoney\":\"5.82\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":12,\"yfMoney\":\"4.12\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":11,\"yfMoney\":\"3.26\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":10,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":9,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"},{\"year\":2018,\"month\":8,\"yfMoney\":\"0.00\",\"sfMoney\":\"0.00\"}]");
            Response.End();
        }


        // 项目人数统计
        public void getXMTJ()
        {
            //http://120.27.218.55:8001/wzwgry/GetWgryTJ

            string ret = "";
            string gcbh = Request["gcbh"].GetSafeString("LG006737");
            ret = Func.ZJWGRY.GetWgryTJ("http://120.27.218.55:8001/wzwgry/GetWgryTJ", "gcbh=" + gcbh);
            Response.Write(ret);
            //Response.Write("{\"DJRS\":\"310\",\"ZZRS\":\"181\",\"ZGRS\":\"0\",\"WGYJRS\":\"12\",\"KQYCRS\":\"0\",\"SBYC\":\"0\",\"AQJYRS\":\"0\",\"LDHTRS\":\"0\",\"GSCBRS\":\"310\"}");
            Response.End();
        }

        // 获取务工人员工种统计
        public void getGZTJ()
        {
            //http://120.27.218.55:8001/wzwgry/GetGZTJ  LG000606
            string ret = "";

            string gcbh = Request["gcbh"].GetSafeString("LG006737");
            ret = Func.ZJWGRY.GetWgryTJ("http://120.27.218.55:8001/wzwgry/GetGZTJ", "gcbh=" + gcbh);
            Response.Write(ret);
        }
        // 获取某个扬尘设备的最新数据
        public void GetGCDustyDevice()
        {
            bool code = true;
            string msg = "";
            object data = null;
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh != "")
                {
                    string sql = string.Format("select top 1 * from i_s_gc_dusty_device where gcbh='{0}'", gcbh);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        string deviceCode = dt[0]["devicecode"].GetSafeString();
                        string postdata = "Token=" + HttpUtility.UrlEncode(Dust.DustGetToken()) +
                            "&DeviceCode=" + HttpUtility.UrlEncode(deviceCode);
                        string url = "";
                        sql = "select * from h_dust_apiconfig where lx='GetLatestDustList'and sfyx=1";
                        IList<IDictionary<string, object>> ddt = CommonService.GetDataTable2(sql);
                        if (ddt.Count > 0)
                        {
                            url = ddt[0]["url"].GetSafeString();
                        }
                        if (url != "")
                        {
                            string retmsg = MyHttp.SendDataByPost(url, postdata);
                            SysLog4.WriteError(retmsg);
                            if (retmsg != "")
                            {
                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                jss.MaxJsonLength = int.MaxValue;
                                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                if (retdata != null)
                                {
                                    int retcode = retdata["Code"].GetSafeInt();
                                    if (retcode == 0)
                                    {
                                        code = true;
                                        data = retdata["Datas"];
                                    }
                                    else
                                    {
                                        code = false;
                                        msg = retdata["Msg"].GetSafeString();
                                        data = retdata;
                                    }
                                }
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "url 配置不正确！";
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
        // 工程数量统计
        public void GetGCSLTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string procstr = string.Format("GetGCSLTJ()");
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("zjgc", dt[0]["zjgc"].GetSafeInt());
                    data.Add("tggc", dt[0]["tggc"].GetSafeInt());
                    data.Add("zggc", dt[0]["zggc"].GetSafeInt());
                    data.Add("ycgc", dt[0]["ycgc"].GetSafeInt());

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
        // 工程分布统计
        public void GetGCFBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string procstr = string.Format("GetGCFBTJ()");
                data = CommonService.ExecDataTableProc(procstr, out msg);
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
        // 务工人员数量统计
        public void GetWGRYSLTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string url = "http://120.27.218.55:8023/api/gov/worker/totalstats?szsf=%E6%B5%99%E6%B1%9F%E7%9C%81&szcs=%E7%BB%8D%E5%85%B4%E5%B8%82&szxq=%E8%AF%B8%E6%9A%A8%E5%B8%82&szjd=&qybh=&gcbh=&gclx=&key=";
                string retmsg = MyHttp.SendDataByGET(url);
                if (retmsg != "")
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                    if (retdata != null)
                    {
                        bool success = retdata["success"].GetSafeBool();
                        if (success)
                        {
                            data = (Dictionary<string, object>)retdata["data"];
                        }
                        else
                        {
                            code = false;
                            msg = retdata["message"].GetSafeString();
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "数据格式错误！";
                    }
                }
                else
                {
                    code = false;
                    msg = "获取数据失败！";
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
        // 设备数量统计
        public void GetGCSBTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string procstr = string.Format("GetGCSBTJ()");
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("zs", dt[0]["zs"].GetSafeInt());
                    data.Add("zysl", dt[0]["zysl"].GetSafeInt());

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
        // 设备分布统计
        public void GetGCSBFBTJ()
        {
            bool code = true;
            string msg = "";
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                string procstr = string.Format("GetGCSBFBTJ()");
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    // 按照工程分组设备
                    var g = dt.GroupBy(
                        x => new
                        {
                            zjdjh = x["zjdjh"].GetSafeString(),
                            gcmc = x["gcmc"].GetSafeString(),
                            gcbh = x["gcbh"].GetSafeString(),
                            gczb = x["gczb"].GetSafeString()
                        }
                        ).Select(x => x.Key).ToList();
                    foreach (var item in g)
                    {
                        var sblist = dt.Where(x => x["gcbh"].GetSafeString() == item.gcbh)
                                        .Select(
                                            x => new
                                            {
                                                sbmc = x["sbmc"].GetSafeString(),
                                                cqbh = x["cqbh"].GetSafeString(),
                                                cqdw = x["cqdw"].GetSafeString(),
                                                sy_state = x["sy_state"].GetSafeString()

                                            }
                                        ).ToList();

                        data.Add(new Dictionary<string, object>() {
                            {"zjdjh", item.zjdjh },
                            {"gcmc", item.gcmc},
                            {"gcbh", item.gcbh },
                            {"gczb", item.gczb},
                            {"sblist", sblist}
                        });

                    }

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
        // 视频数量统计
        public void GetGCVideoTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string procstr = string.Format("GetGCVideoTJ()");
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("gcsl", dt[0]["gcsl"].GetSafeInt());
                    data.Add("spsl", dt[0]["spsl"].GetSafeInt());

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
        // 视频分布统计
        public void GetGCVideoFBTJ()
        {
            bool code = true;
            string msg = "";
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                string procstr = string.Format("GetGCVideoFBTJ()");
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    // 按照工程分组摄像头
                    var g = dt.GroupBy(
                        x => new
                        {
                            zjdjh = x["zjdjh"].GetSafeString(),
                            gcmc = x["gcmc"].GetSafeString(),
                            gcbh = x["gcbh"].GetSafeString(),
                            gczb = x["gczb"].GetSafeString()
                        }
                        ).Select(x => x.Key).ToList();
                    foreach (var item in g)
                    {
                        var cameralist = dt.Where(x => x["gcbh"].GetSafeString() == item.gcbh)
                                        .Select(
                                            x => new
                                            {
                                                channelid = x["channelid"].GetSafeString(),
                                                channelname = x["channelname"].GetSafeString(),
                                                cameratype = x["cameratype"].GetSafeString()


                                            }
                                        ).ToList();

                        data.Add(new Dictionary<string, object>() {
                            {"zjdjh", item.zjdjh },
                            {"gcmc", item.gcmc},
                            {"gcbh", item.gcbh },
                            {"gczb", item.gczb},
                            {"cameralist", cameralist}
                        });

                    }

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
        // 工程搜索
        public void GetALLGCFBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string gcmc = Request["gcmc"].GetSafeString();
                string procstr = string.Format("GetAllGCFBTJ('{0}')", gcmc);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        // 工程整改单和验收次数统计
        public void GetGCZGYSTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string procstr = string.Format("GetGCZGYSTJ()");
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("zgcs", dt[0]["zgcs"].GetSafeInt());
                    data.Add("yscs", dt[0]["yscs"].GetSafeInt());
                    data.Add("jdxccs", dt[0]["jdxccs"].GetSafeInt());

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
        // 务工人员籍贯分布统计
        public void GetWGRYJGSLTJ()
        {
            bool code = true;
            string msg = "";
            ArrayList data = new ArrayList();
            try
            {
                string url = "http://120.27.218.55:8023/api/gov/worker/workerbirthplacestats?szsf=%E6%B5%99%E6%B1%9F%E7%9C%81&szcs=%E7%BB%8D%E5%85%B4%E5%B8%82&szxq=%E8%AF%B8%E6%9A%A8%E5%B8%82&szjd=&qybh=&gcbh=&gclx=&key=";
                string retmsg = MyHttp.SendDataByGET(url);
                if (retmsg != "")
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = int.MaxValue;
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                    if (retdata != null)
                    {
                        bool success = retdata["success"].GetSafeBool();
                        if (success)
                        {
                            data = (ArrayList)retdata["data"];
                        }
                        else
                        {
                            code = false;
                            msg = retdata["message"].GetSafeString();
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "数据格式错误！";
                    }
                }
                else
                {
                    code = false;
                    msg = "获取数据失败！";
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

        // 扬尘监控统计
        public void GetGCYCTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string procstr = string.Format("GetGCYCTJ()");
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        // 工地监控摄像头数量统计
        public void GetGCJKSLTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string procstr = string.Format("GetGCJKSLTJ()");
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        // 工地监控区域分布统计
        public void GetGCJKFBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string procstr = string.Format("GetGCJKFBTJ()");
                data = CommonService.ExecDataTableProc(procstr, out msg);
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
        // 单个工程的设备统计
        public void GetSingleGCSBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string procstr = string.Format("GetSingleGCSBTJ('{0}')", gcbh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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
        // 单个工程监控统计
        public void GetSingleGCJKTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string procstr = string.Format("GetSingleGCJKTJ('{0}')", gcbh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        //根据工程编号获取工程基本信息
        public void GetGCJBXX()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string procstr = string.Format("GetGCJBXX('{0}')", gcbh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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
        #endregion

        #region 企业端看板
        [Authorize]
        // 获取企业名称
        public void GetQYMC()
        {
            bool code = true;
            string msg = "";
            object data = null;
            try
            {
                data = new Dictionary<string, object>()
                {
                    {"qymc", CurrentUser.RealName }
                };
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
        // 获取工程基本统计信息
        [Authorize]
        public void GetGCTJInfoByQY()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetGCTJInfoByQY('{0}')", qybh);
                dt = CommonService.ExecDataTableProc(procstr, out msg);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        // 获取企业工程报监数量统计
        [Authorize]
        public void GetQYGCTJXX()
        {
            bool ret = true;
            string msg = "";
            IDictionary<string, string> data = new Dictionary<string, string>();

            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCTJXX('{0}')", qybh);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data = dt[0];
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }
        [Authorize]
        // 企业 工程整改单和验收次数统计
        public void GetQYGCZGYSTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCZGYSTJ('{0}')", qybh);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("zgcs", dt[0]["zgcs"].GetSafeInt());
                    data.Add("yscs", dt[0]["yscs"].GetSafeInt());
                    data.Add("jdxccs", dt[0]["jdxccs"].GetSafeInt());

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

        [Authorize]
        // 企业工程数量统计
        public void GetQYGCSLTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCSLTJ('{0}')", qybh);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("zjgc", dt[0]["zjgc"].GetSafeInt());
                    data.Add("tggc", dt[0]["tggc"].GetSafeInt());
                    data.Add("zggc", dt[0]["zggc"].GetSafeInt());
                    data.Add("ycgc", dt[0]["ycgc"].GetSafeInt());

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


        // 企业工程分布统计
        [Authorize]
        public void GetQYGCFBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCFBTJ('{0}')", qybh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        [Authorize]
        // 获取企业在建工程区域分布
        public void GetQYZJGCFB()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYZJGCFB('{0}')", qybh);
                dt = CommonService.ExecDataTableProc(procstr, out msg);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        [Authorize]
        // 企业工程搜索
        public void GetQYALLGCFBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string procstr = string.Format("GetQYALLGCFBTJ('{0}','{1}')", gcmc, qybh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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
        [Authorize]
        // 企业相关工程设备数量统计
        public void GetQYGCSBTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCSBTJ('{0}')", qybh);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("zs", dt[0]["zs"].GetSafeInt());
                    data.Add("zysl", dt[0]["zysl"].GetSafeInt());

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

        [Authorize]
        // 企业相关工程重大危险源和当年巡查次数
        public void GetQYWxyAndXCCS()
        {
            string msg = "";
            bool code = true;
            IDictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string proc = string.Format("GetQYWxyAndXCCS('{0}')", qybh);
                IList<IDictionary<string, string>> infolist = CommonService.ExecDataTableProc(proc, out msg);
                if (infolist.Count > 0)
                {
                    data = infolist[0];
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
        [Authorize]
        // 企业相关工程设备数量统计
        public void GetQYSbSLTJ()
        {
            string msg = "";
            bool code = true;
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string proc = string.Format("GetQYSbSLTJ('{0}')", qybh);
                data = CommonService.ExecDataTableProc(proc, out msg);


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
        [Authorize]
        // 企业相关工程扬尘监控统计
        public void GetQYGCYCTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCYCTJ('{0}')", qybh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        [Authorize]
        // 企业相关工程
        // 视频数量统计
        public void GetQYGCVideoTJ()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCVideoTJ('{0}')", qybh);
                IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                if (dt.Count > 0)
                {
                    data.Add("gcsl", dt[0]["gcsl"].GetSafeInt());
                    data.Add("spsl", dt[0]["spsl"].GetSafeInt());

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

        [Authorize]
        // 企业相关工程
        // 工地监控摄像头数量统计
        public void GetQYGCJKSLTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCJKSLTJ('{0}')", qybh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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

        [Authorize]
        // 企业相关工程
        // 工地监控区域分布统计
        public void GetQYGCJKFBTJ()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string qybh = Session["USERQYBH"].GetSafeString();
                string procstr = string.Format("GetQYGCJKFBTJ('{0}')", qybh);
                data = CommonService.ExecDataTableProc(procstr, out msg);
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
        #endregion

        #region 扬尘预警数据校验和接受

        public void GetAllDustAlert()
        {

            string msg = "";
            try
            {
                string method = Request.HttpMethod.GetSafeString();
                if (method == "GET")
                {
                    string token = Request.QueryString["token"].GetSafeString();
                    string timeStamp = Request.QueryString["timeStamp"].GetSafeString();
                    string nonce = Request.QueryString["nonce"].GetSafeString();
                    msg = SHA1Util.StringToSHA1Hash(token + timeStamp + nonce);
                }
                else if (method == "POST")
                {
                    string jsondata = "";
                    Request.InputStream.Position = 0;
                    StreamReader streamReader = new StreamReader(Request.InputStream);
                    jsondata = streamReader.ReadToEnd();
                    if (jsondata != "")
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(jsondata);
                        if (retdata != null)
                        {
                            string sql = "insert into Dusty_Elimit_Warning (Id,ProjectId,DeviceCode,SensorCode,SensorIndex,SensorValue,CurrentTime,DeLimitId,StartTime,EndTime,MinLimit,MaxLimit) " +
                                " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
                            sql = string.Format(sql,
                                    retdata["Id"].GetSafeString(),
                                    retdata["ProjectId"].GetSafeString(),
                                    retdata["DeviceCode"].GetSafeString(),
                                    retdata["SensorCode"].GetSafeString(),
                                    retdata["SensorIndex"].GetSafeString(),
                                    retdata["SensorValue"].GetSafeString(),
                                    Dust.getDatetimeFromUnix(retdata["CurrentTime"].GetSafeDouble()).ToString("yyyy-MM-dd HH:mm:ss"),
                                    retdata["DeLimitId"].GetSafeString(),
                                    Dust.getDatetimeFromUnix(retdata["StartTime"].GetSafeDouble()).ToString("yyyy-MM-dd HH:mm:ss"),
                                    Dust.getDatetimeFromUnix(retdata["EndTime"].GetSafeDouble()).ToString("yyyy-MM-dd HH:mm:ss"),
                                    retdata["MinLimit"].GetSafeString(),
                                    retdata["MaxLimit"].GetSafeString()
                                );
                            if (CommonService.Execsql(sql))
                            {
                                msg = "1";
                            }
                            else
                            {
                                msg = "保存推送数据失败";
                            }
                        }
                        else
                        {
                            msg = "解析推送数据失败";
                        }
                    }
                    else
                    {
                        msg = "推送数据为空";
                    }
                }

            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(msg);
                Response.End();
            }



        }
        #endregion

        #region 扬尘监控地图展示接口
        // 获取诸暨市街道列表
        public void GetZJJDList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select distinct szjd from h_city where szsf='浙江省' and szcs='绍兴市' and szxq='诸暨市'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    List<string> excluded = new List<string>() {
                        "王家井镇","街亭镇","大唐镇","草塔镇","阮市镇",
                        "江藻镇","直埠镇"
                    };
                    foreach (var item in dt)
                    {
                        if (!excluded.Contains(item["szjd"].GetSafeString()))
                        {
                            data.Add(item);
                        }
                    }
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }

        public void GetZJSensorList()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select sensorcode,sensorname from h_sensor_type where sfyx=1 ";
                data = CommonService.GetDataTable(sql);

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }
        #endregion

        #region 施工许可证查询
        public void gotoSgxkzcx()
        {
            Response.Redirect(Configs.GetConfigItem("sgxkzurl"));
        }
        #endregion


        #region 控制大华投屏

        public void dhtestlogin()
        {
            bool ret = true;
            string msg = "";
            try
            {
                ret = DllApi.login(out msg);
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
        #endregion

        #region 大屏控制
        public void SetWindowSourceALL()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string cameraidlist = Request["cameraidlist"].GetSafeString();
                if (cameraidlist != "")
                {
                    string url = Configs.GetConfigItem("dhsetwindowsourceallurl").GetSafeString();
                    if (url != "")
                    {
                        string retmsg = MyHttp.SendDataByPost(url, "cameraidlist=" + HttpUtility.UrlEncode(cameraidlist));
                        SysLog4.WriteError(retmsg);
                        if (retmsg != "")
                        {
                            // JSON 序列化和反序列化类
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            jss.MaxJsonLength = int.MaxValue;
                            Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                            if (retdata != null)
                            {
                                int code = retdata["code"].GetSafeInt();
                                ret = code == 0;
                                if (!ret)
                                {
                                    msg = retdata["msg"].GetSafeString();
                                }
                            }
                        }
                    }
                    else
                    {
                        ret = false;
                        msg = "未配置URL";
                    }

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
        #endregion

        #region 按照工程查看扬尘数据
        public ActionResult ViewDusty()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            ViewBag.gcbh = gcbh;
            return View();
        }

        public void CheckHasDustyDevice()
        {
            bool ret = true;
            string msg = "";
            int deviceCount = 0;
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh !="")
                {
                    string sql = string.Format("select count(*) as num from i_s_gc_dusty_device where sfyx=1 and gcbh='{0}'", gcbh);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0)
                    {
                        deviceCount = dt[0]["num"].GetSafeInt();
                    }
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2}}}", ret ? "0" : "1", msg,deviceCount));
                Response.End();
            }

        }

        #endregion

        #region OSS测试
        public void TestOss()
        {
            bool ret = true;
            string msg = "";
            try
            {
                
                int fileid = Request["fileid"].GetSafeInt();
                if (fileid > 0)
                {
                    StFile file = WorkFlowService.GetFile(fileid);
                    WorkFlowService.SaveFile(file);
                }
                else
                {
                    ret = false;
                    msg = "fileid不能为空";
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

        public void TestDataFile()
        {
            bool ret = true;
            string msg = "";
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    string id = Guid.NewGuid().ToString("N");
                    string strSaveName = file.FileName;
                    string fileext = "";
                    if (strSaveName.LastIndexOf("\\") > -1)
                        strSaveName = strSaveName.Substring(strSaveName.LastIndexOf("\\") + 1);

                    if (strSaveName.IndexOf(".") > 0)
                    {

                        fileext = strSaveName.Substring(strSaveName.LastIndexOf('.'), strSaveName.Length - strSaveName.LastIndexOf('.'));
                    }

                    byte[] postcontent = new byte[file.ContentLength];
                    int readlength = 0;
                    while (readlength < file.ContentLength)
                    {
                        int tmplen = file.InputStream.Read(postcontent, readlength, file.ContentLength - readlength);
                        readlength += tmplen;
                    }
                    ret = DataFileService.SaveDataFile(id, strSaveName, postcontent, fileext, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out msg);
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
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
        #endregion

    }
}