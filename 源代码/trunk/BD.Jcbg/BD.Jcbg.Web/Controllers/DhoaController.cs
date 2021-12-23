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
using BD.WorkFlow.IBll;
using BD.Jcbg.Web.Common;
namespace BD.Jcbg.Web.Controllers
{
    public class DhoaController : Controller
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

        #region 公告通知
        [LoginAuthorize]
        public ActionResult editor()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public JsonResult SaveAnnouncementNotice()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recid = Request["recid"].GetSafeRequest();
                string title = Request["title"].GetSafeRequest();
                string NoticeContent = Request["NoticeContent"].GetSafeRequest();
                OaService.SaveAnnouncementNotice(recid, title, NoticeContent);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg }, JsonRequestBehavior.AllowGet);
        }
        [LoginAuthorize]
        public JsonResult GetAnnouncementNotice()
        {
            bool code = true;
            string msg = "";
            object datas = null;
            try
            {
                string recid = Request["recid"].GetSafeString();
                datas = OaService.GetAnnouncementNotice(recid);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg, datas }, JsonRequestBehavior.AllowGet);
        }

        //删除公告通知
        [LoginAuthorize]
        public JsonResult DeleteAnnouncementNotice()
        {
            bool code = true;
            string msg = "";
            try
            {
                string recids = Request["recids"].GetSafeString();
                OaService.DelAnnouncementNotice(recids);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 规章制度

        #region 规章制度类型

        /// <summary>
        /// 规章制度类型
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GZZDLXEdit()
        {
            string typeid = Request["typeid"].GetSafeString();

            string sql = " select   * from [OA_GZZDLX] where   Status <>-1 and   JCJGBH='" + CurrentUser.Qybh + "' and typeid='" + typeid + "'";

            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.id = dt[i]["id"];
                ViewBag.typeid = dt[i]["typeid"];
                ViewBag.typename = dt[i]["typename"];
            }
            return View();
        }
        public void GZZDLXModify()
        {
            string msg = "";
            bool code = false;
            string sqlStr = "";
            try
            {
                //材料记录唯一号
                string typeid = Request["typeid"].GetSafeString();
                string typename = Request["typename"].GetSafeString();

                if (string.IsNullOrEmpty(typeid))
                {
                    typeid = Guid.NewGuid().ToString("N"); ;
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_GZZDLX]([typeid],[typeName],[JCJGBH],[status])" +
                        "VALUES('" + typeid + "'" +
                        ",'" + typename + "'" +
                        ",'" + CurrentUser.Qybh + "'" +
                        ",'1')");
                }
                else
                {
                    sqlStr = "update OA_GZZDLX set typeName='" + typename + "'   where typeid='" + typeid + "'";
                }

                code = CommonService.ExecSql(sqlStr, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        public void GZZDLXDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string typeid = Request["typeid"].GetSafeString();

                IList<string> sqls = new List<string>();

                sqlStr = "update OA_GZZDLX set Status='-1' where typeid='" + typeid + "'";

                code = CommonService.Execsql(sqlStr);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        public void GetGZZDName()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("[");
                string sql = "select * from OA_GZZDLX where status <>-1  and JCJGBH ='" + CurrentUser.Qybh + "' order by typename ";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"typeid\":\"" + dt[i]["typeid"].GetSafeString() + "\",\"name\":\"" + dt[i]["typename"].GetSafeString() + "\"},");
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

        #endregion

        #region 规章制度明细

        /// <summary>
        /// 规章制度
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GZZDEdit()
        {
            string gzzdid = Request["gzzdid"].GetSafeString();

            string sql = " select   * from [OA_GZZD] where   Status <>-1 and   JCJGBH='" + CurrentUser.Qybh + "' and gzzdid='" + gzzdid + "'";


            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.gzzdid = dt[i]["gzzdid"];
                ViewBag.typeid = dt[i]["typeid"];
                ViewBag.name = dt[i]["name"];
                ViewBag.fileoss = dt[i]["fileoss"];
            }
            return View();
        }
        public void GZZDModify()
        {
            string msg = "";
            bool code = false;
            string sqlStr = "";
            try
            {
                string recid = Request["gzzdid"].GetSafeString();
                string typeid = Request["typeid"].GetSafeString();
                string name = Request["name"].GetSafeString();
                string fileoss = Request["fileoss"].GetSafeString();

                if (string.IsNullOrEmpty(typeid))
                {
                    throw new Exception("获取类型异常");
                }

                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString("N"); ;
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_GZZD](typeid,[fileoss],[Name],[JCJGBH],[CreateTime],[Creater],[UpdateTime],[Updater],[Status])" +
                        "VALUES('" + typeid + "','" + fileoss + "'" +
                        ",'" + name + "'" +
                        ",'" + CurrentUser.Qybh + "'" +
                        ",getdate()" +
                        ",'" + CurrentUser.RealName + "'" +
                       ",getdate()" +
                        ",'" + CurrentUser.RealName + "'" +
                        ",'1')");
                }
                else
                {
                    sqlStr = "update OA_GZZD set typeName='" + name + "',fileoss='" + fileoss + "'," +
                        " Updater='" + CurrentUser.RealName + "'," +
                        " UpdateTime=getdate()  where gzzdid=" + recid;
                }

                code = CommonService.ExecSql(sqlStr, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        public void GZZDDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string recid = Request["recid"].GetSafeString();

                IList<string> sqls = new List<string>();

                sqlStr = "update [OA_GZZD] set Status='-1',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where GZZDID='" + recid + "'";

                code = CommonService.Execsql(sqlStr);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        #endregion
        #endregion

        #region  部门岗位


        [LoginAuthorize]
        public ActionResult KSGL()
        {
            return View();
        }


        [LoginAuthorize]
        /// <summary>
        /// 获取部门岗位信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartmentPost()
        {
            bool code = true;
            string msg = "";

            string gwbh = Request["gwbh"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string qybh = string.IsNullOrEmpty(Request["qybh"].GetSafeString()) ? CurrentUser.Qybh : Request["qybh"].GetSafeString();
                string sql = "select post.gwbh,post.gwmc,ks.* from h_jcks  ks left join  OA_OperatingPost  post on ks.KSBH =post.ksbh  where  ssdwbh ='" + CurrentUser.Qybh + "' and (post.status <>'-1' or post.status is null) ";

                if (!string.IsNullOrEmpty(gwbh))
                {
                    sql += " and  post.gwbh='" + gwbh + "' ";
                }
                sql += "  order by xssx ";
                datas = CommonService.GetDataTable(sql);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = msg, datas = datas }, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize]
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartment()
        {
            bool code = true;
            string msg = "";

            string gwbh = Request["gwbh"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string qybh = string.IsNullOrEmpty(Request["qybh"].GetSafeString()) ? CurrentUser.Qybh : Request["qybh"].GetSafeString();
                string sql = "select ks.* from h_jcks  ks   where ssdwbh ='" + CurrentUser.Qybh + "'";
                sql += "  order by xssx ";
                datas = CommonService.GetDataTable(sql);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = msg, datas = datas }, JsonRequestBehavior.AllowGet);
        }



        [LoginAuthorize]
        public JsonResult DepartmentPostModify()
        {
            string err = "";
            bool code = false;
            try
            {
                //材料记录唯一号
                string ksbh = Request["ksbh"].GetSafeString();
                string gwbh = Request["gwbh"].GetSafeString();
                string gwmc = Request["gwmc"].GetSafeString();
                string sqlStr = "";
                if (string.IsNullOrEmpty(gwbh))
                {
                    gwbh = Guid.NewGuid().ToString("N");
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_OperatingPost]([KSBH],[gwbh],[gwmc],[Status])" +
                        "VALUES ('" + ksbh + "', '" + gwbh + "' , '" + gwmc + "', 1)");
                }
                else
                {
                    sqlStr = "update OA_OperatingPost set KSBH='" + ksbh + "', gwmc='" + gwmc + "' where gwbh='" + gwbh + "'";
                }

                code = CommonService.ExecSql(sqlStr, out err);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                code = false;
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = err }, JsonRequestBehavior.AllowGet);

        }
        [LoginAuthorize]
        public JsonResult DepartmentPostDelete()
        {
            string err = "";
            bool code = false;
            try
            {
                //材料记录唯一号
                string gwbh = Request["gwbh"].GetSafeString();
                string sqlStr = "";
                if (!string.IsNullOrEmpty(gwbh))
                {
                    sqlStr = "update OA_OperatingPost set status='-1' where gwbh='" + gwbh + "'";
                    code = CommonService.ExecSql(sqlStr, out err);
                }
                else
                {
                    err = "删除异常，请联系管理员";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                code = false;
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = err }, JsonRequestBehavior.AllowGet);

        }
        //获取检测机构内部人员
        [LoginAuthorize]
        public JsonResult GetNbry_jc()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string qybh = CurrentUser.Qybh;
                string sql = $"select distinct usercode,ryxm from i_m_nbry_jc where jcjgbh in(select qybh from i_m_qy where parentqybh=(select parentqybh from i_m_qy where qybh='{qybh}'))";
                datas = CommonService.GetDataTable(sql);

            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = msg, datas = datas }, JsonRequestBehavior.AllowGet);
        }



        [LoginAuthorize]
        public JsonResult GetCurKsSyxm()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string qybh = CurrentUser.Qybh;
                string ksbh = Request["ksbh"].GetSafeRequest();
                string sql = $"select syxmbh from h_jcks where ksbh='{ksbh}' and ssdwbh='{qybh}'";
                datas = CommonService.GetDataTable(sql);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = msg, datas }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region  人员管理-人员信息

        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UmsEdit2()
        {

            string usercode = Request["usercode"].GetSafeString();
            ViewBag.usercode = usercode;
            string sql = "select* from[View_OA_RYXXGL] where usercode='" + usercode + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.username = dt[i]["zh"];
                ViewBag.xb = dt[i]["xb"];
                ViewBag.usercode = dt[i]["usercode"];
                ViewBag.realname = dt[i]["ryxm"];
                ViewBag.sfzh = dt[i]["sfzhm"].Replace('\\', '-');
                ViewBag.sjhm = dt[i]["sjhm"].Replace('\\', '-');
                ViewBag.cpcode = dt[i]["cpcode"];
                ViewBag.ksbh = dt[i]["ksbh"];
                ViewBag.gwbh = dt[i]["gwbh"];
                ViewBag.ygxs = dt[i]["usetype"];
                ViewBag.zzmm = dt[i]["zzmm"];
                ViewBag.qrzxl = dt[i]["qrzxl"];
                ViewBag.zzxl = dt[i]["zzxl"];
                ViewBag.byyx = dt[i]["byyx"]; 
                ViewBag.zc = dt[i]["userzc"]; 
                ViewBag.sfsyr = dt[i]["sfsyr"];

                ViewBag.sfsyr = dt[i]["sfsyr"];

            }



            return View();
        }




        /// <summary>
        /// 获取企业信息
        /// </summary>
        [LoginAuthorize]
        public void GetCompanys()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("[");
                string sql = "select * from h_jcjg where jcjgbh ='" + CurrentUser.Qybh + "'";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"CompanyId\":\"" + dt[i]["jcjgbh"].GetSafeString() + "\",\"CompanyName\":\"" + dt[i]["jcjgmc"].GetSafeString() + "\",\"CPCODE\":\"" + dt[i]["cpcode"].GetSafeString() + "\"},");
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

        /// <summary>
        /// 获取科室信息
        /// </summary>
        public void GetJcjgks()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("[");
                string sql = "select * from h_jcks where ssdwbh ='" + CurrentUser.Qybh + "'";

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"ksbh\":\"" + dt[i]["ksbh"].GetSafeString() + "\",\"ksmc\":\"" + dt[i]["ksmc"].GetSafeString() + "\"},");
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


        /// <summary>
        /// 获取部门列表
        /// </summary>
        public void GetGWList()
        {
            string ret = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                string ksbh = Request["ksbh"].GetSafeRequest();

                sb.Append("[");
                string sql = $"select * from OA_OperatingPost where STATUS =1  ";

                if (!string.IsNullOrEmpty(ksbh))
                {
                    sql += " AND  ksbh='" + ksbh + "'";
                }
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"gwbh\":\"" + dt[i]["gwbh"].GetSafeString() + "\",\"gwmc\":\"" + dt[i]["gwmc"].GetSafeString() + "\"},");
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



        /// <summary>
        /// 创建检测科室
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public JsonResult CreateJcks()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string qybh = CurrentUser.Qybh;
                string ksmc = Request["ksmc"].GetSafeRequest();
                string ksdz = Request["ksdz"].GetSafeRequest();
                string lxdh = Request["lxdh"].GetSafeRequest();
                string ksys = Request["ksys"].GetSafeRequest();
                string kszcode = Request["kszcode"].GetSafeRequest();
                string kszxm = Request["kszxm"].GetSafeRequest();
                string jsfzrcode = Request["jsfzrcode"].GetSafeRequest();
                string jsfzrxm = Request["jsfzrxm"].GetSafeRequest();
                string zlfzrcode = Request["zlfzrcode"].GetSafeRequest();
                string zlfzrxm = Request["zlfzrxm"].GetSafeRequest();
                string type = Request["type"].GetSafeRequest(); //type: N 新建，G：修改
                string ksbh = Request["ksbh"].GetSafeRequest();
                code = OaService.CreateJcks(type, ksbh, qybh, ksmc, ksdz, lxdh, ksys, kszcode, kszxm, jsfzrcode, jsfzrxm, zlfzrcode, zlfzrxm);
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg = msg }, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize]
        public void UmsApiService()
        {
            string err = "";
            string ret = "";
            try
            {
                string method = Request["method"].GetSafeString();
                string opt = Request["opt"].GetSafeString();
                string type = Request["type"].GetSafeString();

                if (method.ToLower() == "user" && opt.ToLower() == "checkuserbysfzh")
                {
                    string sfzh = Request["sfzh"].GetSafeString();
                    ret = JcjtService.CheckUserBySfzh(sfzh);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "adduser")  //添加用户
                {
                    string username = Request["username"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    string password = Request["password"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string sfzh = Request["sfzh"].GetSafeString();
                    string depcode = Request["depcode"].GetSafeString();
                    string xb = Request["xb"].GetSafeString();
                    string ksbh = Request["ksbh"].GetSafeString();
                    string sjhm = Request["sjhm"].GetSafeString();
                    string sfsyr = Request["sfsyr"].GetSafeString(); //是否试验人
                    string rolecodelist = Request["rolecodelist"].GetSafeString(); //,隔开
                    string json = Request["jsondata"].GetSafeString();
                    string jcjgbh = CurrentUser.Qybh;

                    ret = JcjtService.AddUser(jcjgbh, username, realname, password, sfzh, xb, sfsyr, sjhm, cpcode, depcode, ksbh, "", rolecodelist, json);
                }

                if (method.ToLower() == "role" && opt.ToLower() == "getownerrolelistbyusercode") //根据角色代码获取所有的用户及已经有此角色的用户标志
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string usercode = Request["usercode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();

                    if (cpcode != "")
                        cpcode += ",";
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString(); //"CP201611000001";//

                    }
                    string procode = Configs.AppId;// "WZJDBG";
                    ret = JcjtService.GetOwnerRoleListByUsercode(page, rows, usercode, cpcode, procode, "");
                }
                if (method.ToLower() == "role" && opt.ToLower() == "getrolelistbyusercode") //获取用户角色
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string usercode = Request["usercode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString(); //"CP201611000001";//
                    }
                    string procode = Configs.AppId;// "WZJDBG";
                    ret = JcjtService.GetRoleListByUsercode(page, rows, usercode, cpcode, procode, "");
                }
                if (method.ToLower() == "role" && opt.ToLower() == "getrolelist") //获取角色列表
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string usercode = Request["usercode"].GetSafeString();
                    string cpname = Request["cpname"].GetSafeString();
                    string proname = Request["proname"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    if (cpcode != "")
                        cpcode += ",";
                    cpcode += "CPCORL3uC5aV6S";
                    //IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    //if (dt.Count > 0)
                    //{
                    //    cpcode = dt[0]["cpcode"].GetSafeString(); //"CP201611000001";//
                    //}
                    string procode = Configs.AppId2;
                    ret = JcjtService.GetRoleList(page, rows, usercode, cpcode, cpname, procode, proname, "", "");
                }
                if (method.ToLower() == "user" && opt.ToLower() == "modifyuserinfobyusercode") //更新用户信息
                {
                    string username = Request["username"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    string usercode = Request["usercode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string ksbh = Request["ksbh"].GetSafeString();
                    string sfsyr = Request["sfsyr"].GetSafeString();
                    string depcode = "";
                    string postdm = "";
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString();
                    }
                    string xb = Request["xb"].GetSafeString();
                    string sjhm = Request["sjhm"].GetSafeString();
                    //IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select depcode from H_ZJZ where ZJZBH='" + cpcode + "'");
                    //if (dt.Count > 0)
                    //    depcode = dt[0]["depcode"].GetSafeString();
                    string rolecodelist = Request["rolecodelist"].GetSafeString();
                    string procode = Configs.AppId; //"WZJDBG";
                    string clearrole = Request["clearrole"].GetSafeString("true");
                    string sfzhm = Request["sfzh"].GetSafeString();
                    ret = JcjtService.ModifyUserInfoByUsercode(username, realname, usercode, xb, sfsyr, sjhm, cpcode, depcode, ksbh, "", procode, rolecodelist, clearrole, sfzhm, type);
                }
                if (method.ToLower() == "role" && opt.ToLower() == "addroleinfo") //添加角色
                {
                    string cpcode = Request["cpcode"].GetSafeString();
                    string rolename = Request["rolename"].GetSafeString();
                    string memo = Request["memo"].GetSafeString();
                    string procode = Configs.AppId;
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString(); //"CP201611000001";//
                    }
                    ret = JcjtService.AddRoleInfo(cpcode, procode, rolename, memo);
                }

                if (method.ToLower() == "user" && opt.ToLower() == "getowneruserlistbyrolecode") //根据角色代码获取所有的用户及已经有此角色的用户标志
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string rolecode = Request["rolecode"].GetSafeString();
                    string username = Request["username"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString();
                    }
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = JcjtService.GetOwnerUserListByRolecode(page, rows, rolecode, cpcode, username, realname);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "modifyuserstatusbyusercode") //根据用户代码禁用或启用用户
                {
                    string usercode = Request["usercode"].GetSafeString();
                    string userstatus = Request["userstatus"].GetSafeString();

                    ret = JcjtService.ModifyUserStatusByUsercode(usercode, userstatus);
                }

                if (method.ToLower() == "userrole" && opt.ToLower() == "modifyuserrolebyrolecodeandusercodelist") //根据角色代码及用户代码组更新用户角色信息
                {
                    string rolecode = Request["rolecode"].GetSafeString();
                    string usercodelist = Request["usercodelist"].GetSafeString();

                    ret = JcjtService.ModifyUserRoleByRolecodeAndUsercodeList(rolecode, usercodelist);
                }


                if (method.ToLower() == "power" && opt.ToLower() == "getownerpowerlistbyrolecode")  //获取角色所能有的权限及已经赋的权限
                {
                    string rolecode = Request["rolecode"].GetSafeString();


                    ret = JcjtService.GetOwnerPowerListByRolecode(rolecode);
                }

                if (method.ToLower() == "power" && opt.ToLower() == "savepowerbyrolecode") //保存角色权限信息
                {
                    string rolecode = Request["rolecode"].GetSafeString();
                    string menulist = Request["menulist"].GetSafeString();
                    string butlist = Request["butlist"].GetSafeString();

                    ret = JcjtService.SavePowerByRolecode(rolecode, menulist, butlist);
                }


                if (method.ToLower() == "power" && opt.ToLower() == "getpowerlistbyrolecode") //根据角色代码获取角色的权限
                {
                    string rolecode = Request["rolecode"].GetSafeString();

                    ret = JcjtService.GetPowerListByRolecode(rolecode);
                }


                if (method.ToLower() == "user" && opt.ToLower() == "getuserlistbymenucode") //根据权限代码获取所具有权限的人员信息
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string menucode = Request["menucode"].GetSafeString();
                    string cpcode = "";//这里没写完
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString();
                    }
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = JcjtService.GetUserListByMenucode(page, rows, procode, cpcode, menucode);
                }
                if (method.ToLower() == "role" && opt.ToLower() == "getrolelistbymenucode") //根据权限获取具有此权限的角色信息
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string menucode = Request["menucode"].GetSafeString();
                    string procode = Configs.AppId; //"WZJDBG";
                    ret = JcjtService.GetRoleListByMenucode(page, rows, procode, menucode);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "getuserlistbyrolecode") //根据角色代码获取有此角色的用户列表
                {
                    string page = Request["page"].GetSafeString();
                    string rows = Request["rows"].GetSafeString("1000");
                    string rolecode = Request["rolecode"].GetSafeString();
                    string cpcode = Request["cpcode"].GetSafeString();
                    string realname = Request["realname"].GetSafeString();
                    IList<IDictionary<string, string>> dt = JcjtService.GetH_JCJG(CurrentUser.UserCode);
                    if (dt.Count > 0)
                    {
                        cpcode = dt[0]["cpcode"].GetSafeString();
                    }
                    ret = JcjtService.GetUserListByRolecode(page, rows, rolecode, cpcode, realname);
                }
                if (method.ToLower() == "user" && opt.ToLower() == "getprocodeandmenubyusercode") //获取当前登录用户的所有项目及菜单
                {
                    ret = JcjtService.GetProcodeAndMenuByUsercode(CurrentUser.UserCode);
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

        /// <summary>
        /// 添加人员资料
        /// </summary>
        public void UserArchiveEdit()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string cpcode = Request["cpcode"].GetSafeString();
                string sfzh = Request["sfzh"].GetSafeString();
                string depcode = Request["depcode"].GetSafeString();
                string xb = Request["xb"].GetSafeString();
                string ksbh = Request["ksbh"].GetSafeString();
                string sjhm = Request["sjhm"].GetSafeString();
                string gwbh = Request["gwbh"].GetSafeString();//岗位编号
                string zc = Request["zc"].GetSafeString();//岗位编号
                string ygxs = Request["ygxs"].GetSafeString();//用工形式
                string zzmm = Request["zzmm"].GetSafeString();//政治面貌
                string qrzxl = Request["qrzxl"].GetSafeString();//全日制学历
                string zzxl = Request["zzxl"].GetSafeString();//在职学历
                string byyx = Request["byyx"].GetSafeString();//毕业院校
                string jcjgbh = CurrentUser.Qybh;

                IList<string> sqls = new List<string>();


                string sqrStr = "";


                if (string.IsNullOrEmpty(sfzh))
                {
                    code = false;
                    msg = "获取不到身份证信息";
                    return;
                }
                //从I_M_NBRY_JC获取用户信息
                sqrStr = $" select* from I_M_NBRY_JC where SFZHM = '{sfzh}' and jcjgbh='{jcjgbh}'";

                var datas = CommonService.GetDataTable(sqrStr);

                foreach (var item in datas)
                {

                    var acrhievsData = CommonService.GetDataTable($"select  * from OA_UserArchievs where RYBH='{item["usercode"]}'");

                    if (acrhievsData!=null&& acrhievsData.Count==0)
                    {
                        var acrhievsRecid = Guid.NewGuid().ToString("N");
                        sqlStr = $"INSERT INTO [dbo].[OA_UserArchievs]([Recid],[RYBH],[RYMC],[KSBH],[GWBH],[UseType],[ZZMM],[QRZXL],[ZZXL],[BYYX],[ZC],[LXSS],[JSDAID],[JCJGBH])" +
                            $"VALUES('{acrhievsRecid}'" +
                            $",'{item["usercode"]}'" +
                            $",'{item["ryxm"]}'" +
                            $",'{ksbh}'" +
                            $",'{gwbh}'" +
                            $",'{ygxs}'" +
                            $",'{zzmm}'" +
                            $",'{qrzxl}'" +
                            $",'{zzxl}'" +
                            $",'{byyx}'" +
                            $",'{zc}'" +
                            $",'{sjhm}'" +
                            $",''" +
                            $",'{jcjgbh}')";
                    }
                    else
                    {
                        sqlStr = $"update OA_UserArchievs set KSBH='{ksbh}',gwbh='{gwbh}',UseType='{ygxs}',zzmm='{zzmm}',qrzxl='{qrzxl}'" +
                            $",zzxl='{zzxl}',byyx='{byyx}',zc='{zc}' where RYBH='{item["usercode"]}'";

                    }
                }

                code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        #endregion


        #region 材料管理
        #region 采购订单
        /// <summary>
        /// 采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PurchaseOrderEdit()
        {
            //类型 采购关联：CGGL 办公耗材 BGHC  试验耗材：SYHC
            string method = Request["method"].GetSafeString();

            string recid = Request["recid"].GetSafeString();
            ViewBag.recid = recid;


            string sql = "select * from dbo.OA_PurchaseOrder where Status<>'-1' and Recid='" + recid + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {

                ViewBag.recid = dt[i]["recid"];
                ViewBag.materialname = dt[i]["materialname"];
                ViewBag.materialunit = dt[i]["materialunit"];

                ViewBag.materialid = dt[i]["materialid"];
                ViewBag.materialunitid = dt[i]["materialunitid"];
                ViewBag.price = dt[i]["price"];
                ViewBag.purchaseprice = dt[i]["purchaseprice"];
                ViewBag.quantity = dt[i]["quantity"];
                ViewBag.technicalrequirement = dt[i]["technicalrequirement"];
                ViewBag.supplier = dt[i]["supplier"];
                ViewBag.manufacturer = dt[i]["manufacturer"];
                ViewBag.purpose = dt[i]["purpose"];
            }
            return View();
        }

        public void PurchaseOrderModify()
        {
            string err = "";
            bool ret = false;
            try
            {
                //材料记录唯一号
                string recid = Request["recid"].GetSafeString();

                string dataType = Request["dataType"].GetSafeString();
                string matId = Request["matId"].GetSafeString();
                string matName = Request["matName"].GetSafeString();
                string unitId = Request["unitId"].GetSafeString();
                string unitName = Request["unitName"].GetSafeString();
                string price = Request["price"].GetSafeString();
                string quantity = Request["quantity"].GetSafeString();
                string purchasePrice = Request["purchasePrice"].GetSafeString();
                string technicalRequirement = Request["technicalRequirement"].GetSafeString();
                string supplier = Request["supplier"].GetSafeString();
                string manufacturer = Request["manufacturer"].GetSafeString();
                string purpose = Request["purpose"].GetSafeString();
                string requisitioner = Request["purpose"].GetSafeString();
                ret = OaService.PurchaseOrderModify(recid, matId, matName, unitId, unitName, price, purchasePrice, quantity, purpose, technicalRequirement, supplier, manufacturer, requisitioner);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", err));
                Response.End();
                //Response.ContentType = "text/plain";
                //Response.Write(ret);
                //Response.End();
            }
        }


        public void PurchaseOrderDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string recid = Request["recid"].GetSafeString();

                IList<string> sqls = new List<string>();

                sqlStr = "update OA_PurchaseOrder set Status='-1',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where recid='" + recid + "'";

                code = CommonService.Execsql(sqlStr);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 材料消耗
        /// <summary>
        /// 采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MaterialConsumptionEdit()
        {
            //类型  办公耗材 BGHC  试验耗材：SYHC
            string method = Request["method"].GetSafeString();
            //1：办公消耗 2：试验消耗
            ViewBag.type = Request["type"].GetSafeString();

            string recid = Request["recid"].GetSafeString();
            ViewBag.recid = recid;


            string sql = "select * from dbo.OA_MateriaInfo where Status<>'-1' and Recid='" + recid + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {

                ViewBag.recid = dt[i]["recid"];
                ViewBag.type = dt[i]["type"];
                ViewBag.materialname = dt[i]["materialname"];
                ViewBag.materialunit = dt[i]["materialunit"];

                ViewBag.materialid = dt[i]["materialid"];
                ViewBag.materialunitid = dt[i]["materialunitid"];
                ViewBag.price = dt[i]["price"];
                ViewBag.quantity = dt[i]["quantity"];
                ViewBag.technicalrequirement = dt[i]["technicalrequirement"];
                ViewBag.supplier = dt[i]["supplier"];
                ViewBag.manufacturer = dt[i]["manufacturer"];
                ViewBag.purpose = dt[i]["purpose"];
            }
            return View();
        }

        public void MaterialConsumptionModify()
        {
            string msg = "";
            bool code = false;
            try
            {
                //材料记录唯一号
                string recid = Request["recid"].GetSafeString();

                //1：办公消耗 2：试验消耗
                string type = Request["type"].GetSafeString();
                string matId = Request["matId"].GetSafeString();
                string matName = Request["matName"].GetSafeString();
                string unitId = Request["unitId"].GetSafeString();
                string unitName = Request["unitName"].GetSafeString();
                string price = Request["price"].GetSafeString();
                string quantity = Request["quantity"].GetSafeString();
                string purchasePrice = Request["purchasePrice"].GetSafeString();
                string technicalRequirement = Request["technicalRequirement"].GetSafeString();
                string supplier = Request["supplier"].GetSafeString();
                string manufacturer = Request["manufacturer"].GetSafeString();
                string purpose = Request["purpose"].GetSafeString();
                string requisitioner = Request["purpose"].GetSafeString();

                string sqlStr = "";
                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString("N");
                    sqlStr = "INSERT INTO [dbo].[OA_MateriaInfo]([Recid],type,[MaterialID],[MaterialName],[MaterialUnitID],[MaterialUnit],[Price]," +
                        "[PurchasePrice],[Quantity],[Purpose],[TechnicalRequirement],[Supplier],[Manufacturer],[Requisitioner],[JCJGBH],[Checker]," +
                        "[CheckTime],[CreateTime],[Creator],[UpdateTime],[Updater],[Status]) " +
                        "VALUES(" +
                        "'" + recid + "','" + type + "','" + matId + "','" + matName + "','" + unitId + "','" + unitName + "','" + price + "',null,'" + quantity + "'" +
                        ",'" + purpose + "','" + technicalRequirement + "','" + supplier + "','" + manufacturer + "','" + requisitioner + "','" + CurrentUser.Qybh + "'" +
                        ",null,null,getdate(),'" + CurrentUser.RealName + "',getdate(),'" + CurrentUser.RealName + "',1)";
                }
                else
                {
                    sqlStr = string.Format(" update OA_MateriaInfo set MaterialID='" + matId + "'  ,MaterialName='" + matName + "'" +
                        ",MaterialUnitID='" + unitId + "' ,MaterialUnit='" + unitName + "'" +
                        ",Price='" + price + "'" +
                        ",Quantity='" + quantity + "' ,Purpose='" + purpose + "'" +
                        ",technicalRequirement='" + technicalRequirement + "' ,Supplier='" + supplier + "'" +
                        ",manufacturer='" + manufacturer + "' ,requisitioner='" + requisitioner + "'" +
                        ",UpdateTime=getdate() ,Updater='" + CurrentUser.RealName + "'" +
                        "  where  recid='" + recid + "'");
                }

                code = CommonService.ExecSql(sqlStr, out msg);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();

            }
        }


        public void MaterialConsumptionDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string recid = Request["recid"].GetSafeString();

                IList<string> sqls = new List<string>();

                sqlStr = "update OA_MateriaInfo set Status='-1',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where recid='" + recid + "'";

                code = CommonService.Execsql(sqlStr);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        /// <summary>
        /// 获取材料
        /// </summary>
        public void GetMaterial()
        {
            StringBuilder sb = new StringBuilder();

            string sqlWhere = "";
            try
            {
                sqlWhere = " and  status <>'-1' ";
                sb.Append("[");
                string sql = "select * from OA_Material where   jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["id"].GetSafeString() + "\",\"name\":\"" + dt[i]["materialname"].GetSafeString() + "\"},");
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

        /// <summary>
        /// 获取材料
        /// </summary>
        public void GetMaterialUnit()
        {
            StringBuilder sb = new StringBuilder();
            string materialId = Request["materialId"].GetSafeString();
            string sqlWhere = "";
            try
            {
                sqlWhere = " and  status <>'-1' ";

                if (string.IsNullOrEmpty(materialId))
                {
                    sb.Append("");
                    return;
                }
                sqlWhere += " and materialId=" + materialId;
                sb.Append("[");
                string sql = "select * from OA_MaterialUnit where    jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["id"].GetSafeString() + "\",\"name\":\"" + dt[i]["unitname"].GetSafeString() + "\"},");
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
        #endregion


        #region  考勤管理

        /// <summary>
        /// 考勤机管理页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult AttendanceMachineEdit()
        {
            string id = Request["id"].GetSafeString();

            string sql = "select * from  OA_AttendanceMachine  where JCJGBH='" + CurrentUser.Qybh + "' and id='" + id + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.id = dt[i]["id"];
                ViewBag.name = dt[i]["name"];
                ViewBag.installposition = dt[i]["installposition"];
                ViewBag.serialnumber = dt[i]["serialnumber"];
                ViewBag.status = dt[i]["status"];
            }

            return View();
        }


        /// <summary>
        /// 更新考勤机
        /// </summary>
        public void AttendanceMachineUpdate()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {

                string machineId = Request["id"].GetSafeString();
                string machineName = Request["name"].GetSafeString();
                string installPosition = Request["installposition"].GetSafeString();
                string serialNumber = Request["serialnumber"].GetSafeString();
                IList<string> sqls = new List<string>();

                if (string.IsNullOrEmpty(machineId))
                {
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_AttendanceMachine]([Name],[InstallPosition],[SerialNumber],[JCJGBH],[CreateTime],[Creater],[UpdateTime],[Updater],[Status])" +
                        "VALUES ('{0}', '{1}' , '{2}' , '{3}' , getdate(),'{4}', getdate(),'{5}', 1)", machineName, installPosition, serialNumber, CurrentUser.Qybh, CurrentUser.UserName, CurrentUser.UserName);
                }
                else
                {
                    sqlStr = "update OA_AttendanceMachine set Name='" + machineName + "', InstallPosition='" + installPosition + "',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where id=" + machineId;
                }

                code = CommonService.Execsql(sqlStr);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        ///删除考勤机
        /// </summary>
        public void AttendanceMachineDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string machineId = Request["id"].GetSafeString();

                IList<string> sqls = new List<string>();

                if (string.IsNullOrEmpty(machineId))
                {
                    msg = "找不到考勤机，编号【" + machineId + " 】";
                }
                else
                {
                    sqlStr = "update OA_AttendanceMachine set Status='-1',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where id=" + machineId;
                }

                code = CommonService.Execsql(sqlStr);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 车辆管理
        #region 车辆信息
        /// <summary>
        /// 车辆信息修改
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CarEdit()
        {
            string id = Request["id"].GetSafeString();

            string sql = " select  * from OA_CarInfomation    where   Status <>-1 and   JCJGBH='" + CurrentUser.Qybh + "' and id='" + id + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.id = id;
                ViewBag.brand = dt[i]["brand"];
                ViewBag.type = dt[i]["type"];
                ViewBag.carid = dt[i]["carid"];
                ViewBag.price = dt[i]["price"];
                ViewBag.buytime = dt[i]["buytime"];//购买时间
                ViewBag.scrapyears = dt[i]["scrapyears"];//报废年限
                ViewBag.remark = dt[i]["remark"]; //备注
                ViewBag.drivinglicense = dt[i]["drivinglicense"];//行驶证
            }

            return View();
        }

        /// <summary>
        /// 更新车辆信息
        /// </summary>
        public void CarUpdate()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {

                string dataId = Request["id"].GetSafeString();
                string brand = Request["brand"].GetSafeString();
                string type = Request["type"].GetSafeString();
                string carId = Request["carid"].GetSafeString();
                string price = Request["price"].GetSafeString();
                string buyTime = Request["buytime"].GetSafeString();
                int scrapYears = Request["scrapyears"].GetSafeInt();
                string remark = Request["remark"].GetSafeString();
                string drivingLicense = Request["drivinglicense"].GetSafeString();
                IList<string> sqls = new List<string>();

                //是否报废
                string isScrap = "1";
                if (buyTime.GetSafeDate().AddYears(scrapYears) > DateTime.Now)
                {
                    isScrap = "0";
                }

                if (string.IsNullOrEmpty(dataId))
                {
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_CarInfomation](" +
                        "[Brand],[Type],[CarID],[IsScrap],[IsGoout],[Price]," +
                        "[CreateTime],[Creator],[UpdateTime],[JCJGBH],[Updater],[Remark],[Status],[IsUsing],[Destination]," +
                        "[BuyTime],[ScrapYears],[DrivingLicense]" +
                        " )VALUES ('{0}' , '{1}', '{2}', '{3}', '{4}', '{5}'," +
                        " getdate(), '{6}',getdate(), '{7}' , '{8}', '{9}', '1', 0,null, " +
                        "'{10}','{11}','{12}')",
                        brand, type, carId, isScrap, "0", price,
                        CurrentUser.UserName, CurrentUser.Qybh, CurrentUser.UserName, remark,
                        buyTime, scrapYears, drivingLicense);
                }
                else
                {
                    sqlStr = "update OA_CarInfomation set Brand='" + brand + "', Type='" + type + "', price='" + price + "'" +
                        ", buyTime='" + buyTime + "'" +
                        ", scrapYears='" + scrapYears + "'" +
                        ", drivingLicense='" + drivingLicense + "'" +
                        ", isScrap='" + isScrap + "'" +
                        ", remark='" + remark + "',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where id=" + dataId;
                }

                code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion
        #endregion

        #region 汽车使用记录
        /// <summary>
        /// 用车申请
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CarRecordEdit()
        {
            string id = Request["id"].GetSafeString();

            string oper = Request["oper"].GetSafeString();
            string method = Request["method"].GetSafeString();
            //主表唯一号
            string mid = Request["mid"].GetSafeString();

            //申请人
            ViewBag.applicant = CurrentUser.RealName;
            ViewBag.mid = mid;

            string sql = " select   * from [OA_CarIUseRecord]    where   Status <>-1 and   JCJGBH='" + CurrentUser.Qybh + "'";

            if (method == "applyfor")
            {
                //添加用车申请
                sql += " and  mid='" + mid + "'";
            }
            else if (method == "updateRecord")
            {
                //修改用车申请
                sql += " and  id='" + id + "'";
            }
            else if (method == "improveRecord")
            {
                //完善用车记录
                sql += " and  id='" + id + "'";
                ViewBag.method = "improveRecord";
            }

            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.id = dt[i]["id"];
                ViewBag.mid = dt[i]["mid"];
                //用车部门
                ViewBag.department = dt[i]["department"];
                //同车人
                ViewBag.copassenger = dt[i]["copassenger"];
                //申请人
                ViewBag.applicant = dt[i]["applicant"];
                //目的地
                ViewBag.destination = dt[i]["destination"];
                //出车用途
                ViewBag.usefor = dt[i]["usefor"];
                //出车时间
                ViewBag.outtime = dt[i]["outtime"];
                //返回时间
                ViewBag.returntime = dt[i]["returntime"];
                //行驶公里
                ViewBag.kilometers = dt[i]["kilometers"];
                //加油（升）
                ViewBag.oilcost = dt[i]["oilcost"];
                //过路过桥费
                ViewBag.roadtoll = dt[i]["roadtoll"];
                //备注
                ViewBag.remark = dt[i]["remark"];

            }


            return View();
        }

        /// <summary>
        /// 更新车辆信息
        /// </summary>
        public void CarRecordUpdate()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {

                string dataId = Request["id"].GetSafeString();
                string mId = Request["mid"].GetSafeString();
                //用车部门
                string department = Request["department"].GetSafeString();
                //申请人
                string applicant = Request["applicant"].GetSafeString();
                //同车人
                string copassenger = Request["copassenger"].GetSafeString();
                //目的地
                string destination = Request["destination"].GetSafeString();
                //出车用途
                string usefor = Request["usefor"].GetSafeString();
                //出车时间
                string outtime = Request["outtime"].GetSafeString();
                //返回时间
                string returntime = Request["returntime"].GetSafeString();
                //行驶公里
                string kilometers = Request["kilometers"].GetSafeString();
                //加油（升）
                string oilcost = Request["oilcost"].GetSafeString();
                //过路过桥费
                string roadtoll = Request["oilcost"].GetSafeString();
                //备注
                string remark = Request["remark"].GetSafeString();


                IList<string> sqls = new List<string>();


                if (string.IsNullOrEmpty(dataId))
                {
                    if (string.IsNullOrEmpty(mId))
                    {
                        msg = "车辆信息不存在";
                        throw new Exception();
                    }
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_CarIUseRecord]([MId],[UseRegion],[UseFor],[Status]," +
                        "[Applicant],[Department],[Destination],[OutTime],[ReturnTime],[CoPassenger],[Driver],[Kilometers],[OilCost],[RoadToll],[CreateTime],[Creator],[UpdateTime],[JCJGBH],[Updater],[Remark])" +
                        "VALUES('" + mId + "'" +
                        ",'1'" + //使用范围（1室内 2 市外）
                        ",'" + usefor + "'" + //用途
                        ",'1'" +//< status, int,>
                        ",'" + applicant + "'" +
                        ",'" + department + "'" +
                        ",'" + destination + "'" +
                        ",'" + outtime + "'" +
                        ",'" + returntime + "'" +
                        ",'" + copassenger + "'" +
                        ",''" +//driver
                        ",'" + kilometers + "'" +
                        ",'" + oilcost + "'" +
                        ",'" + roadtoll + "'" +
                        ",getdate()" +
                        ",'" + CurrentUser.RealName + "'" +
                        ",getdate()" +
                        ",'" + CurrentUser.Qybh + "'" +
                        ",'" + CurrentUser.RealName + "'" +
                        ",'" + remark + "')");
                }
                else
                {
                    sqlStr = "update OA_CarIUseRecord set department='" + department + "', applicant='" + applicant + "', copassenger='" + copassenger + "'" +
                        ", destination='" + destination + "'" +
                        ", usefor='" + usefor + "'" +
                        ", outtime='" + outtime + "'" +
                        ", returntime='" + returntime + "'" +
                        ", kilometers='" + kilometers + "'" +
                        ", oilcost='" + oilcost + "'" +
                        ", roadtoll='" + roadtoll + "'" +
                        ", remark='" + remark + "',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where id=" + dataId;
                }

                code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 删除车辆信息
        /// </summary>
        public void CarRecordDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string dataId = Request["id"].GetSafeString();

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(dataId))
                {
                    msg = "车辆信息不存在";
                }
                else
                {
                    sqlStr = "update OA_CarIUseRecord set  status ='-1',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where id=" + dataId;
                }

                code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        #endregion

        #region 维保信息
        [LoginAuthorize]
        public ActionResult CarMaintenanceEdit()
        {
            string id = Request["id"].GetSafeString();

            string method = Request["method"].GetSafeString();
            //主表唯一号
            string mid = Request["mid"].GetSafeString();

            string carid = Request["carid"].GetSafeString();

            //申请人
            ViewBag.applicant = CurrentUser.RealName;
            ViewBag.mid = mid;

            //主表唯一号
            string carId = Request["carId"].GetSafeString();

            string sql = " select main.CarID, sub.* from OA_CarMaintenance  sub  left join  OA_CarInfomation main on main.ID=sub.mid  where   sub.Status <>-1  and  sub.JCJGBH='" + CurrentUser.Qybh + "'";

            if (method == "add")
            {
                //添加维保记录
                sql += " and  mid='" + mid + "'";
                ViewBag.carid = carid;
            }
            else if (method == "update")
            {
                //修改维保记录
                sql += " and  sub.id='" + id + "'";
            }


            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.id = dt[i]["id"];
                ViewBag.mid = dt[i]["mid"];
                ViewBag.carid = dt[i]["carid"];
                //申请人
                ViewBag.applicant = dt[i]["applicant"];
                //申请时间
                ViewBag.createtime = dt[i]["createtime"];
                //保养日期
                ViewBag.maintenancetime = dt[i]["maintenancetime"];
                //行驶总里程
                ViewBag.totalkilometers = dt[i]["totalkilometers"];
                //保养内容
                ViewBag.maintenancecontent = dt[i]["maintenancecontent"];
            }

            return View();
        }


        public void CarMaintenanceUpdate()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string dataId = Request["id"].GetSafeString();
                string mId = Request["mid"].GetSafeString();
                //申请人
                string applicant = Request["applicant"].GetSafeString();
                //保养日期
                string maintenancetime = Request["maintenancetime"].GetSafeString();
                //行驶总里程
                string totalkilometers = Request["totalkilometers"].GetSafeString();
                //保养内容
                string maintenancecontent = Request["maintenancecontent"].GetSafeString();

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(dataId))
                {
                    if (string.IsNullOrEmpty(mId))
                    {
                        msg = "车辆信息不存在";
                        throw new Exception();
                    }
                    sqlStr = string.Format("INSERT INTO[dbo].[OA_CarMaintenance]([Mid],[Applicant],[TotalKilometers],[MaintenanceTime],[MaintenanceContent],[CreateTime],[Creator],[Status],[JCJGBH])" +
                        "VALUES ('" + mId + "','" + applicant + "'" +
                        ",'" + totalkilometers + "'" +
                        ", '" + maintenancetime + "'" +
                        ",'" + maintenancecontent + "'" +
                        ", getdate()" +
                        ", '" + CurrentUser.RealName + "' " +
                        ", '1'" +
                        ", '" + CurrentUser.Qybh + "'  )");
                }
                else
                {
                    sqlStr = "update OA_CarMaintenance set applicant='" + applicant + "', totalkilometers='" + totalkilometers + "'," +
                        " maintenancetime='" + maintenancetime + "' where id=" + dataId;
                }

                code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 删除维保信息
        /// </summary>
        public void CarMaintenanceDelete()
        {
            bool code = false;
            string msg = "";
            string sqlStr = "";
            try
            {
                string dataId = Request["id"].GetSafeString();

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(dataId))
                {
                    msg = "删除异常，请联系管理员";
                }
                else
                {
                    sqlStr = "update OA_CarMaintenance set  status ='-1' where id=" + dataId;
                    code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 印章管理

        /// <summary>
        /// 页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SignatureEdit()
        {
            string recid = Request["recid"].GetSafeString();

            string method = Request["method"].GetSafeString();
            //主表唯一号


            string sql = " select   * from [OA_SignatureManage]    where   Status <>-1 and   JCJGBH='" + CurrentUser.Qybh + "'";


            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.recid = dt[i]["recid"];
                ViewBag.signaturecode = dt[i]["signaturecode"];
                ViewBag.signaturename = dt[i]["signaturename"];
                ViewBag.departmentbh = dt[i]["departmentbh"];
                ViewBag.departmentname = dt[i]["departmentname"];
                ViewBag.ownercode = dt[i]["ownercode"];
                ViewBag.ownername = dt[i]["ownername"];
                ViewBag.custodian = dt[i]["custodian"];

            }


            return View();
        }


        /// <summary>
        /// 更新
        /// </summary>
        public void SignatureUpdate()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string mId = Request["mid"].GetSafeString();

                string signaturecode = Request["signaturecode"].GetSafeString();
                string signaturename = Request["signaturename"].GetSafeString();
                string departmentbh = Request["departmentbh"].GetSafeString();
                string ownername = Request["ownername"].GetSafeString();
                string custodian = Request["custodian"].GetSafeString();

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString("N"); ;
                    sqlStr = string.Format("INSERT INTO [dbo].[OA_SignatureManage]([SignatureCode],[SignatureName],recid,[DepartmentBH],[DepartmentName],[OwnerCode],[OwnerName],[Status],[Custodian],[CreateTime],[Creator],[UpdateTime],[JCJGBH],[Updater])" +
                        "VALUES('" + signaturecode + "'" +
                        ",'" + signaturename + "'" +
                        ",'" + recid + "'" +
                        ",'" + departmentbh + "'" +
                        ",null" +
                        ",null" +
                        ",'" + ownername + "'" +
                        ",'1'" +
                        ",'" + custodian + "'" +
                        ",getdate()" +
                        ",'" + CurrentUser.RealName + "'" +
                        ",getdate()" +
                        ",'" + CurrentUser.Qybh + "'" +
                        ",'" + CurrentUser.RealName + "')");
                }
                else
                {
                    sqlStr = "update OA_SignatureManage set signaturename='" + signaturename + "', departmentbh='" + departmentbh + "'," +
                        " ownername='" + ownername + "',UpdateTime=getdate(),Updater='" + CurrentUser.RealName + "'  where recid=" + recid;
                }

                code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void SignatureDelete()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string dataId = Request["recid"].GetSafeString();

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(dataId))
                {
                    msg = "删除异常，请联系管理员";
                    code = false;
                }
                else
                {
                    sqlStr = "update OA_SignatureManage set  status ='-1' where recid=" + dataId;
                    code = CommonService.ExecSql(sqlStr, out msg);
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

    }
}