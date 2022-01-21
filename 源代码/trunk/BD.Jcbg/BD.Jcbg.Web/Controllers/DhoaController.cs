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
using System.Collections;

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
                    sqlStr = "update OA_GZZD set Name='" + name + "',fileoss='" + fileoss + "'," +
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
                string sql = "select post.gwbh,post.gwmc,ks.* from h_jcks  ks left join  OA_OperatingPost  post on ks.KSBH =post.ksbh   and post.status <>'-1'  where  ssdwbh ='" + CurrentUser.Qybh + "'  ";

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
                if (string.IsNullOrEmpty(gwbh))
                {
                    err = "删除异常，请联系管理员";
                    throw new Exception(err);

                }
                sqlStr = " select 1 from OA_UserArchives  archievs inner join I_M_NBRY_JC nbry on archievs.rybh=nbry.USERCODE and usingnow=1 where gwbh='" + gwbh + "'";

                var datas = CommonService.GetDataTable(sqlStr);

                if (datas.Count != 0)
                {
                    err = "删除失败，该岗位已经绑定人员。";
                    throw new Exception(err);
                }

                sqlStr = "update OA_OperatingPost set status='-1' where gwbh='" + gwbh + "'";
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
            string sql = "select * from[View_OA_RYXXGL] where usercode='" + usercode + "'";
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
                ViewBag.ksbh = string.IsNullOrEmpty(dt[i]["ksbh"]) ? dt[i]["ssksbh"] : dt[i]["ksbh"];
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
                    string procode = Configs.AppId;
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

                    var acrhievsData = CommonService.GetDataTable($"select  * from OA_UserArchives where RYBH='{item["usercode"]}'");

                    if (acrhievsData != null && acrhievsData.Count == 0)
                    {
                        var acrhievsRecid = Guid.NewGuid().ToString("N");
                        sqlStr = $"INSERT INTO [dbo].[OA_UserArchives]([Recid],[RYBH],[RYMC],[KSBH],[GWBH],[UseType],[ZZMM],[QRZXL],[ZZXL],[BYYX],[ZC],[LXSS],[JSDAID],[JCJGBH])" +
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
                        sqlStr = $"update OA_UserArchives set KSBH='{ksbh}',gwbh='{gwbh}',UseType='{ygxs}',zzmm='{zzmm}',qrzxl='{qrzxl}'" +
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


        /// <summary>
        /// 获取人员信息
        /// </summary>
        public void GetUserArchives()
        {
            StringBuilder sb = new StringBuilder();
            string ksbh = Request["ksbh"].GetSafeString();
            string sqlWhere = "";
            try
            {

                if (!string.IsNullOrEmpty(ksbh))
                {
                    sqlWhere += " and ksbh=" + ksbh;
                }
                sb.Append("[");

                string sql = "select * from OA_UserArchives where jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["rybh"].GetSafeString() + "\",\"name\":\"" + dt[i]["rymc"].GetSafeString() + "\"},");
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

        #region 材料管理
        #region 新增商品信息

        public ActionResult ProductEdit()
        {
            string recid = Request["recid"].GetSafeString();

            string sql = $"select * from dbo.OA_MaterialInfo where jcjgbh ='{CurrentUser.Qybh}' and Recid='" + recid + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {

                ViewBag.recid = dt[i]["recid"];
                ViewBag.materialbh = dt[i]["materialbh"];
                ViewBag.materialid = dt[i]["materialid"];
                ViewBag.materialname = dt[i]["materialname"];
                ViewBag.materialspecid = dt[i]["materialspecid"];
                ViewBag.materialspecname = dt[i]["materialspecname"];
                ViewBag.materialunit = dt[i]["materialunit"];

            }
            return View();
        }

        public void ProductModify()
        {
            string msg = "";
            bool code = false;
            try
            {
                //材料记录唯一号
                string recid = Request["recid"].GetSafeString();
                string productBH = Request["productbh"].GetSafeString();
                string materialId = Request["materialId"].GetSafeString();
                string materialtype = Request["materialtype"].GetSafeString();
                string materialName = Request["materialName"].GetSafeString();
                string materialSpecId = Request["materialSpecId"].GetSafeString();
                string materialSpecName = Request["materialSpecName"].GetSafeString();
                string materialUnit = Request["materialUnit"].GetSafeString();
                string status = Request["status"].GetSafeString();

                List<string> sqls = new List<string>();
                string sqlStr = "";


                if (string.IsNullOrEmpty(productBH))
                {
                    msg = "请输入产品编号";
                    return;
                }

                //新增时，判断编号是否重复
                if (string.IsNullOrEmpty(recid))
                {
                    sqlStr = $" select * from OA_MaterialInfo where MaterialBH='{productBH}' and JCJGBH ='{CurrentUser.Qybh}'";

                    var datas = CommonService.GetDataTable(sqlStr);
                    if (datas.Count > 0)
                    {
                        msg = "产品编号重复！";
                        return;
                    }
                    sqlStr = $" select * from OA_MaterialInfo where Materialid='{materialId}' and  MaterialSpecID='{materialSpecId}' and  type='{materialtype}'  and JCJGBH ='{CurrentUser.Qybh}'";

                    datas = CommonService.GetDataTable(sqlStr);
                    if (datas.Count > 0)
                    {
                        msg = "添加失败，已存在相同类型的产品！";
                        return;
                    }


                    recid = Guid.NewGuid().ToString("N");
                    sqls.Add($"INSERT INTO [dbo].[OA_MaterialInfo]([Recid],[type],[MaterialBH],[MaterialID],[MaterialName],[MaterialUnit],[MaterialSpecID],[MaterialSpecName]" +
                        $",[JCJGBH],[CreateTime],[Creator],[Status]) " +
                        $"VALUES " +
                        $"('{recid}'" +
                        $",'{materialtype}'" +
                        $",'{productBH}'" +
                        $",'{materialId}'" +
                        $",'{materialName}'" +
                        $",'{materialUnit}'" +
                        $",'{materialSpecId}'" +
                        $",'{materialSpecName}'" +
                        $",'{CurrentUser.Qybh}'" +
                        $",getdate()" +
                        $",'{CurrentUser.RealUserName}'" +
                        $",'{status}')");
                }
                else
                {
                    sqls.Add(string.Format(" update OA_MaterialInfo set MaterialName='" + materialName + "'  ,materialUnit='" + materialUnit + "'" +
                        ",materialSpecName='" + materialSpecName + "'" +
                        ",UpdateTime=getdate() ,Updater='" + CurrentUser.RealName + "' ,status='" + status + "'" +
                        "  where  recid='" + recid + "'"));
                }

                CommonService.ExecTrans(sqls, out msg);

                if (string.IsNullOrEmpty(msg))
                {
                    code = true;
                }

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
        #endregion

        #region 采购订单
        /// <summary>
        /// 采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PurchaseOrderEdit()
        {
            string method = Request["method"].GetSafeString();
            string recid = Request["recid"].GetSafeString();
            ViewBag.recid = recid;
            ViewBag.method = method;


            string sql = $"select * from dbo.OA_PurchaseOrder where Status<>'-1' and  jcjgbh ='{CurrentUser.Qybh}' and Recid='" + recid + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {

                ViewBag.orderRecid = dt[i]["recid"];
                ViewBag.materialBH = dt[i]["materialbh"];
                ViewBag.materialid = dt[i]["materialid"];
                ViewBag.materialname = dt[i]["materialname"];
                ViewBag.materialspecid = dt[i]["materialspecid"];
                ViewBag.materialspecname = dt[i]["materialspecname"];
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

        /// <summary>
        /// 修改采购订单（采购申请流程会调用）
        /// </summary>
        public void PurchaseOrderModify()
        {
            string err = "";
            bool ret = false;
            try
            {
                //材料记录唯一号
                string recid = Request["recid"].GetSafeString();
                string serialRecid = Request["guid"].GetSafeString();
                string method = Request["method"].GetSafeString();
                string quantity = Request["quantity"].GetSafeString();
                string materBH = Request["matBH"].GetSafeString();
                string materId = Request["matId"].GetSafeString();
                string materName = Request["matName"].GetSafeString();
                string specId = Request["specId"].GetSafeString();
                string specName = Request["specName"].GetSafeString();
                string price = Request["price"].GetSafeString();
                string purchasePrice = Request["purchasePrice"].GetSafeString();
                string technicalRequirement = Request["technicalRequirement"].GetSafeString();
                string supplier = Request["supplier"].GetSafeString();
                string manufacturer = Request["manufacturer"].GetSafeString();
                string purpose = Request["purpose"].GetSafeString();
                string requisitioner = CurrentUser.RealUserName;


                string sql = $" select Recid from OA_PurchaseOrder  where serialRecid='{serialRecid}'";

                List<string> sqls = new List<string>();
                var data = CommonService.GetDataTable(sql);
                if (data.Count() > 0)
                {
                    recid = data[0]["recid"];
                }
                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString("N");
                    sql = "INSERT INTO [dbo].[OA_PurchaseOrder]([Recid],serialRecid,MaterialBH,[MaterialID],[MaterialName],[MaterialSpecID],[MaterialSpecName],[Price]," +
                     "[PurchasePrice],[Quantity],[Purpose],[TechnicalRequirement],[Supplier],[Manufacturer],[Requisitioner],[JCJGBH],[Checker]," +
                     "[CheckTime],[CreateTime],[Creator],[UpdateTime],[Updater],[Status]) " +
                     "VALUES(" +
                     " '" + recid + "', '" + serialRecid + "', '" + materBH + "', '" + materId + "',  '" + materName + "', '" + specId + "',  '" + specName + "', '" + price + "','" + purchasePrice + "', '" + quantity + "'" +
                     ", '" + purpose + "', '" + technicalRequirement + "', '" + supplier + "',  '" + manufacturer + "',  '" + requisitioner + "',  '" + CurrentUser.Qybh + "'" +
                     ", null, null, getdate(), '" + CurrentUser.RealName + "', getdate(), '" + CurrentUser.RealName + "', 1)";
                }
                else
                {
                    sql = string.Format("update OA_PurchaseOrder set Price='" + price + "' ,PurchasePrice='" + purchasePrice + "'" +
                    ",Quantity='" + quantity + "' ,Purpose='" + purpose + "'" +
                    ",technicalRequirement='" + technicalRequirement + "' ,Supplier='" + supplier + "'" +
                    ",manufacturer='" + manufacturer + "' ,requisitioner='" + requisitioner + "'" +
                    ",UpdateTime=getdate() ,Updater='" + CurrentUser.RealName + "' " +
                    " where  recid='" + recid + "'");
                }

                sqls.Add(sql);
                sqls.Add($" update OA_MaterialInfo set price='{price}',LastPrice='{purchasePrice}',PurchasePrice='{purchasePrice}',Purpose='{purpose}',TechnicalRequirement='{technicalRequirement}'" +
                    $",Supplier='{supplier}',Manufacturer='{manufacturer}',Requisitioner='{requisitioner}'" +
                    $",UpdateTime=getdate() ,Updater='{CurrentUser.RealName }'" +
                    $" where  MaterialBH='{materBH}'");

                CommonService.ExecTrans(sqls, out err);

                if (string.IsNullOrEmpty(err))
                {
                    ret = true;
                }
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
            }
        }

        /// <summary>
        /// 产品入库
        /// </summary>
        public void ProductInStorage()
        {
            string err = "";
            bool ret = false;
            try
            {
                //材料记录唯一号
                string productbh = Request["productbh"].GetSafeString();
                //申请：applyfor 入库：instorage
                string method = Request["method"].GetSafeString();
                decimal quantity = Request["quantity"].GetSafeDecimal();

                if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(productbh))
                {
                    err = "参数异常，入库操作失败。";
                    return;
                }

                //更新商品库存
                ret = ProductInventoryOperation(productbh, method, quantity, out err);

                //更新材料消耗信息

                if (method.ToLower() == InventoryOper.InStorage)
                {
                    MaterialConsumeUpdate(productbh, quantity, out err);
                }
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

        #region 库存操作
        public struct InventoryOper
        {
            /// <summary>
            /// 材料领取
            /// </summary>
            static public string Applyfor = "applyfor";

            /// <summary>
            /// 入库
            /// </summary>
            static public string InStorage = "instorage";
        }

        /// <summary>
        /// 更新材料耗材信息
        /// </summary>
        /// <param name="productBH"></param>
        /// <param name="method"></param>
        /// <param name="operQuantity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool MaterialConsumeUpdate(string productBH, decimal operQuantity, out string msg)
        {
            msg = "";
            bool ret = false;

            List<string> sqls = new List<string>();
            string sqlStr = string.Empty;

            string warehouseBH = "";
            string warehouseName = "";
            //操作类型 1：入库 10领取
            string operType = "1";
            //库存数量
            decimal inventoryQuantity = 0;
            string recid = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(productBH))
                {
                    msg = "操作异常";
                    SysLog4.WriteLog($"方法【MaterialConsumeUpdate】中异常：" + msg);
                    return false;
                }

                #region 
                sqlStr = $"select * from OA_MaterialInfo where materialbh='{productBH}'";
                var materialData = CommonService.GetDataTable(sqlStr);

                if (materialData.Count == 0)
                {
                    msg = "找不到产品信息";
                    SysLog4.WriteLog($"方法【MaterialConsumeUpdate】中异常：" + msg);
                    return false;
                }

                //入库时，往材料消耗信息表中添加记录
                sqlStr = $"select * from MaterialConsume where materialbh='{productBH}'";
                var consumeData = CommonService.GetDataTable(sqlStr);
                if (consumeData.Count == 0)
                {
                    recid = Guid.NewGuid().ToString("N");
                    sqlStr = $"INSERT INTO [dbo].[OA_MaterialConsume]([Recid],type,[MaterialID],[MaterialName],[MaterialSpecID],[MaterialSpec],[Price]," +
                        $"[PurchasePrice],[Quantity],[Purpose],[TechnicalRequirement],[Supplier],[Manufacturer],[Requisitioner],[JCJGBH],[Checker]," +
                        $"[CheckTime],[CreateTime],[Creator],[UpdateTime],[Updater],[Status]) " +
                        $"VALUES('{recid}','{consumeData[0]["type"]}'" +
                        $",'{consumeData[0]["matId"]}'" +
                        $",'{consumeData[0]["matName"]}'" +
                        $",'{consumeData[0]["unitId"]}'" +
                        $",'{consumeData[0]["unitName"]}'" +
                        $",'{consumeData[0]["price"]}'" +
                        $",'{consumeData[0]["PurchasePrice"]}'" +
                        $",'{consumeData[0]["quantity"]}'" +
                        $",'{consumeData[0]["purpose"]}'" +
                        $",'{consumeData[0]["technicalRequirement"]}'" +
                        $",'{consumeData[0]["supplier"]}'" +
                        $",'{consumeData[0]["manufacturer"]}'" +
                        $",'{consumeData[0]["requisitioner"]}'" +
                        $",'{CurrentUser.Qybh}'" +
                        $",''" +
                        $",null" +
                        $",getdate()" +
                        $",'" + CurrentUser.RealName + "'" +
                        $",getdate()" +
                        $",'" + CurrentUser.RealName + "',1)";
                }

                //添加库存记录
                sqls.Add($"INSERT INTO [dbo].[OA_InventoryRecord]([Recid],[WarehouseBH],[WarehouseName],[ProductBH],[ProductID],[ProductName],[ProductSpecID],[ProductSpecName]," +
                    $"[ProductUnit],[Quantity],[Price],[PurchasePrice],[CreateTime],[Creator],[JCJGBH],[OperType]) " +
                    $"VALUES ('{Guid.NewGuid().ToString("N")}'" +
                    $",'{warehouseBH}'" +
                    $",'{warehouseName}'" +
                    $",'{productBH}'" +
                    $",'{materialData[0]["materialid"]}'" +
                    $",'{materialData[0]["materialname"]}'" +
                    $",'{materialData[0]["materialspecid"]}'" +
                    $",'{materialData[0]["materialspecname"]}'" +
                    $",'{materialData[0]["materialunit"]}'" +
                    $",'{operQuantity}'" +
                    $",'{materialData[0]["price"]}'" +
                    $",'{materialData[0]["purchaseprice"]}'" +
                    $",getdate()" +
                    $",'{CurrentUser.RealName}'" +
                    $",'{CurrentUser.Qybh}'" +
                    $",'{operType}')");

                #endregion

                #region 更新库存信息
                sqlStr = $"select * from OA_InventoryInfo where ProductBH='{productBH}' andjcjgbh ='{CurrentUser.Qybh}'";
                var inventoryData = CommonService.GetDataTable(sqlStr);

                if (inventoryData.Count == 0)
                {
                    //库存表中没有记录，直接插入
                    sqlStr=($"INSERT INTO [dbo].[OA_InventoryInfo]([Recid],[WarehouseBH],[WarehouseName],[ProductBH],[ProductID],[ProductName]," +
                        $"[ProductSpecID],[ProductSpecName],[ProductUnit],[Stock],[Price],[PurchasePrice],[UpdateTime],[Updater],[JCJGBH])" +
                        $"VALUES (" +
                        $"'{Guid.NewGuid().ToString("N")}'" +
                        $",'{warehouseBH}'" +
                        $",'{warehouseName}'" +
                        $",'{productBH}'" +
                        $",'{materialData[0]["materialid"]}'" +
                        $",'{materialData[0]["materialname"]}'" +
                        $",'{materialData[0]["materialspecid"]}'" +
                        $",'{materialData[0]["materialspecname"]}'" +
                        $",'{materialData[0]["materialunit"]}'" +
                        $",'{operQuantity}'" +
                        $",'{materialData[0]["price"]}'" +
                        $",'{materialData[0]["purchaseprice"]}'" +
                        $",getdate()" +
                        $",'{CurrentUser.RealName}'" +
                        $",'{CurrentUser.Qybh}')");
                }
                else if (inventoryData.Count == 1)
                {
                    //库存表中一条记录，更新库存信息
                    #region 库存数量判断
                    //if (method.ToLower() == InventoryOper.InStorage)
                    //{
                    //    inventoryQuantity = inventoryData[0]["stock"].GetSafeDecimal() + operQuantity;
                    //    //入库
                    //}
                    //else if (method.ToLower() == InventoryOper.Applyfor)
                    //{
                    //    //领取
                    //    inventoryQuantity = inventoryData[0]["stock"].GetSafeDecimal() + operQuantity;
                    //}
                    //else
                    //{

                    //}
                    #endregion

                    sqls.Add($"update OA_InventoryInfo set Stock='{inventoryQuantity}' ,UpdateTime=getdate(),Updater='{CurrentUser.Qybh}'" +
                        $" where  recid='{inventoryData[0]["recid"]}'");
                }
                else
                {
                    //库存表中有多条记录，正常不存在这种情况
                }


                CommonService.ExecTrans(sqls, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    ret = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                msg = "方法调用异常" + ex.Message;
                SysLog4.WriteLog($"方法【ProductInventoryOperation】中异常：" + msg);
                return false;
            }
            return ret;
        }
        /// <summary>
        /// 商品库存信息更新
        /// </summary>
        /// <param name="productBH">商品编号</param>
        /// <param name="method">标记入库\领取</param>
        /// <param name="operQuantity">入库或领取数量</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ProductInventoryOperation(string productBH, string method, decimal operQuantity, out string msg)
        {
            msg = "";
            bool ret = false;

            List<string> sqls = new List<string>();
            string sqlStr = string.Empty;

            string warehouseBH = "";
            string warehouseName = "";
            //操作类型 1：入库 10领取
            string operType = "1";
            //库存数量
            decimal inventoryQuantity = 0;
            //更新数量
            //operQuantity = 0;

            //商品编号
            //productBH = "";
            try
            {
                if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(productBH))
                {
                    msg = "操作异常";
                    SysLog4.WriteLog($"方法【ProductInventoryOperation】中异常：" + msg);
                    return false;
                }

                //入库操作
                if (method.ToLower() == "applyfor")
                {
                    operType = "10";
                }

                #region 添加库存记录
                sqlStr = $"select * from OA_MaterialInfo where materialbh='{productBH}'";
                var materialData = CommonService.GetDataTable(sqlStr);
                //List<MateriaInfo> sdf = materialData as List<MateriaInfo>;
                if (materialData.Count == 0)
                {
                    msg = "找不到产品信息";
                    SysLog4.WriteLog($"方法【ProductInventoryOperation】中异常：" + msg);
                    return false;
                }
                productBH = materialData[0]["materialbh"];
                sqls.Add($"INSERT INTO [dbo].[OA_InventoryRecord]([Recid],[WarehouseBH],[WarehouseName],[ProductBH],[ProductID],[ProductName],[ProductSpecID],[ProductSpecName]," +
                    $"[ProductUnit],[Quantity],[Price],[PurchasePrice],[CreateTime],[Creator],[JCJGBH],[OperType]) " +
                    $"VALUES ('{Guid.NewGuid().ToString("N")}'" +
                    $",'{warehouseBH}'" +
                    $",'{warehouseName}'" +
                    $",'{productBH}'" +
                    $",'{materialData[0]["materialid"]}'" +
                    $",'{materialData[0]["materialname"]}'" +
                    $",'{materialData[0]["materialspecid"]}'" +
                    $",'{materialData[0]["materialspecname"]}'" +
                    $",'{materialData[0]["materialunit"]}'" +
                    $",'{operQuantity}'" +
                    $",'{materialData[0]["price"]}'" +
                    $",'{materialData[0]["purchaseprice"]}'" +
                    $",getdate()" +
                    $",'{CurrentUser.RealName}'" +
                    $",'{CurrentUser.Qybh}'" +
                    $",'{operType}')");

                #endregion

                #region 更新库存信息
                sqlStr = $"select * from OA_InventoryInfo where ProductBH='{productBH}' andjcjgbh ='{CurrentUser.Qybh}'";
                var inventoryData = CommonService.GetDataTable(sqlStr);

                if (inventoryData.Count == 0)
                {
                    if (method.ToLower() == InventoryOper.Applyfor)
                    {
                        //正常不存在领取的情况
                    }

                    //库存表中没有记录，直接插入
                    sqls.Add($"INSERT INTO [dbo].[OA_InventoryInfo]([Recid],[WarehouseBH],[WarehouseName],[ProductBH],[ProductID],[ProductName]," +
                        $"[ProductSpecID],[ProductSpecName],[ProductUnit],[Stock],[Price],[PurchasePrice],[UpdateTime],[Updater],[JCJGBH])" +
                        $"VALUES (" +
                        $"'{Guid.NewGuid().ToString("N")}'" +
                        $",'{warehouseBH}'" +
                        $",'{warehouseName}'" +
                        $",'{productBH}'" +
                        $",'{materialData[0]["materialid"]}'" +
                        $",'{materialData[0]["materialname"]}'" +
                        $",'{materialData[0]["materialspecid"]}'" +
                        $",'{materialData[0]["materialspecname"]}'" +
                        $",'{materialData[0]["materialunit"]}'" +
                        $",'{operQuantity}'" +
                        $",'{materialData[0]["price"]}'" +
                        $",'{materialData[0]["purchaseprice"]}'" +
                        $",getdate()" +
                        $",'{CurrentUser.RealName}'" +
                        $",'{CurrentUser.Qybh}')");
                }
                else if (inventoryData.Count == 1)
                {
                    //库存表中一条记录，更新库存信息
                    #region 库存数量判断
                    if (method.ToLower() == InventoryOper.InStorage)
                    {
                        inventoryQuantity = inventoryData[0]["stock"].GetSafeDecimal() + operQuantity;
                        //入库
                    }
                    else if (method.ToLower() == InventoryOper.Applyfor)
                    {
                        //领取
                        inventoryQuantity = inventoryData[0]["stock"].GetSafeDecimal() + operQuantity;
                    }
                    else
                    {

                    }
                    #endregion

                    sqls.Add($"update OA_InventoryInfo set Stock='{inventoryQuantity}' ,UpdateTime=getdate(),Updater='{CurrentUser.Qybh}'" +
                        $" where  recid='{inventoryData[0]["recid"]}'");
                }
                else
                {
                    //库存表中有多条记录，正常不存在这种情况
                }


                CommonService.ExecTrans(sqls, out msg);
                if (string.IsNullOrEmpty(msg))
                {
                    ret = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                msg = "方法调用异常" + ex.Message;
                SysLog4.WriteLog($"方法【ProductInventoryOperation】中异常：" + msg);
                return false;
            }
            return ret;
        }
        #endregion

        #region 耗材
        /// <summary>
        /// 耗材管理
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MaterialConsumptionEdit()
        {
            string recid = Request["recid"].GetSafeString();
            ViewBag.recid = recid;
            string type = Request["type"].GetSafeString();
            ViewBag.type = type;

            string sql = "select * from dbo.OA_MaterialConsume where Status<>'-1' and Recid='" + recid + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {

                //ViewBag.recid = dt[i]["recid"];
                //ViewBag.type = dt[i]["type"];
                //ViewBag.materialname = dt[i]["materialname"];
                //ViewBag.MaterialSpec = dt[i]["MaterialSpec"];

                //ViewBag.materialid = dt[i]["materialid"];
                //ViewBag.MaterialSpecid = dt[i]["MaterialSpecid"];
                //ViewBag.price = dt[i]["price"];
                //ViewBag.quantity = dt[i]["quantity"];
                //ViewBag.technicalrequirement = dt[i]["technicalrequirement"];
                //ViewBag.supplier = dt[i]["supplier"];
                //ViewBag.manufacturer = dt[i]["manufacturer"];
                //ViewBag.purpose = dt[i]["purpose"];
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
                string warehouseBH = Request["warehouseBH"].GetSafeString();

                //1：办公消耗 2：试验消耗
                string type = Request["type"].GetSafeString();
                string matBH = Request["matBH"].GetSafeString();
                string matId = Request["matId"].GetSafeString();
                string matName = Request["matName"].GetSafeString();
                string unitId = Request["unitId"].GetSafeString();
                string unitName = Request["unitName"].GetSafeString();
                string price = Request["price"].GetSafeString("0");
                string quantity = Request["quantity"].GetSafeString("0");
                string purchasePrice = Request["purchasePrice"].GetSafeString("0");
                string technicalRequirement = Request["technicalRequirement"].GetSafeString();
                string supplier = Request["supplier"].GetSafeString();
                string manufacturer = Request["manufacturer"].GetSafeString();
                string purpose = Request["purpose"].GetSafeString();
                string requisitioner = Request["purpose"].GetSafeString();

                string sqlStr = "";

                ///产品库存数量
                decimal productInventortQuantity = 0;
                List<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(recid))
                {
                    #region 更新产品的库存信息
                    sqlStr = $"select * from OA_InventoryInfo where ProductBH='{matBH}' and ISNULL(WarehouseBH,'') ='{warehouseBH}'and jcjgbh='{CurrentUser.Qybh}'";
                    var inventDatas = CommonService.GetDataTable(sqlStr);

                    if (inventDatas.Count == 0)
                    {
                        sqls.Add($"INSERT INTO [dbo].[OA_InventoryInfo]([Recid],[WarehouseBH],[ProductBH]," +
                            $"[Stock],[Price],[PurchasePrice],[UpdateTime],[Updater],[JCJGBH])" +
                           $"VALUES (" +
                           $"'{Guid.NewGuid().ToString("N")}'" +
                           $",'{warehouseBH}'" +
                           $",'{matBH}'" +
                           $",'{quantity}'" +
                           $",'{price}'" +
                           $",'{purchasePrice}'" +
                           $",getdate()" +
                           $",'{CurrentUser.RealName}'" +
                           $",'{CurrentUser.Qybh}')");

                    }
                    else if (inventDatas.Count == 1)
                    {
                        productInventortQuantity = inventDatas[0]["stock"].GetSafeDecimal() + quantity.GetSafeDecimal();
                        sqlStr = $"update OA_InventoryInfo set Stock='{productInventortQuantity}' ,UpdateTime=getdate(),Updater='{CurrentUser.RealUserName}'" +
                       $" where  ProductBH='{matBH}' and ISNULL(WarehouseBH,'') ='{warehouseBH}'and jcjgbh='{CurrentUser.Qybh}'";
                        sqls.Add(sqlStr);
                    }
                    else
                    {
                        //正常情况不存在的
                    }

               
                    #endregion

                    #region 更新产品消耗信息
                    sqlStr = $"select * from OA_MaterialConsume where MaterialBH ='{matBH}' and  ISNULL(warehouseBH,'') ='{warehouseBH}' and jcjgbh='{CurrentUser.Qybh}'";
                    //判断OA_MaterialConsume是否已经有记录
                    var datas = CommonService.GetDataTable(sqlStr);
                    if (datas.Count() == 0)
                    {
                        recid = Guid.NewGuid().ToString("N");

                        sqlStr = "INSERT INTO [dbo].[OA_MaterialConsume]([Recid],type,MaterialBH,[MaterialID],[MaterialName],[MaterialSpecID],[MaterialSpecName],[Price]," +
                            "[PurchasePrice],[Quantity],[Purpose],[TechnicalRequirement],[Supplier],[Manufacturer],[Requisitioner],[JCJGBH],[Checker]," +
                            "[CheckTime],[CreateTime],[Creator],[UpdateTime],[Updater],[Status]) " +
                            "VALUES(" +
                            "'" + recid + "','" + type + "','" + matBH + "','" + matId + "','" + matName + "','" + unitId + "','" + unitName + "','" + price + "','" + purchasePrice + "','" + quantity + "'" +
                            ",'" + purpose + "','" + technicalRequirement + "','" + supplier + "','" + manufacturer + "','" + requisitioner + "','" + CurrentUser.Qybh + "'" +
                            ",null,null,getdate(),'" + CurrentUser.RealName + "',getdate(),'" + CurrentUser.RealName + "',1)";
                        sqls.Add(sqlStr);
                    }
                    else if (datas.Count() == 1)
                    {
                        sqlStr = $" update OA_MaterialConsume set quantity='{productInventortQuantity}'  where MaterialBH ='{matBH}' and ISNULL(warehouseBH,'') ='{warehouseBH}' and jcjgbh='{CurrentUser.Qybh}'";
                        sqls.Add(sqlStr);
                    }
                    else
                    {
                        //正常不存在的情况
                    }
                    #endregion


                    //添加入库记录
                    sqlStr = $"INSERT INTO [dbo].[OA_InventoryRecord]([Recid],[WarehouseBH],[ProductBH]," +
                        $"[Quantity],[Price],[PurchasePrice],[CreateTime],[Creator],[JCJGBH],[OperType]) " +
                        $"VALUES ('{Guid.NewGuid().ToString("N")}'" +
                        $",'{warehouseBH}'" +
                        $",'{matBH}'" +
                        $",'{quantity.GetSafeDecimal()}'" +
                        $",'{price}'" +
                        $",'{purchasePrice}'" +
                        $",getdate()" +
                        $",'{CurrentUser.RealName}'" +
                        $",'{CurrentUser.Qybh}'" +
                        $",'1')";
                    sqls.Add(sqlStr);

                    CommonService.ExecTrans(sqls, out msg);
                    if (string.IsNullOrEmpty(msg))
                    {
                        code = true;
                    }
                }
                else
                {
                    //更新产品消耗信息
                }
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

                sqlStr = "update OA_MaterialConsume set Status='-1',UpdateTime=getdate(),Updater='" + CurrentUser.UserName + "' where recid='" + recid + "'";

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

        #region  材料及规格
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
        /// 获取材料规格
        /// </summary>
        public void GetMaterialSpec()
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
                string sql = "select * from OA_MaterialSpec where    jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["id"].GetSafeString() + "\",\"name\":\"" + dt[i]["spec"].GetSafeString() + "\"},");
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
        /// 获取材料规格
        /// </summary>
        public void GetMaterialSpecByBH()
        {
            StringBuilder sb = new StringBuilder();
            string materialBH = Request["materialBH"].GetSafeString();
            string sqlWhere = "";
            try
            {
                sqlWhere = " and  status <>'-1' ";

                if (string.IsNullOrEmpty(materialBH))
                {
                    sb.Append("");
                    return;
                }
                sqlWhere += " and materialBH='" + materialBH + "'";
                sb.Append("[");
                string sql = "select distinct materialid  ,Materialspecid   ,Materialspecname  from OA_MaterialInfo where    jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["materialspecid"].GetSafeString() + "\",\"name\":\"" + dt[i]["materialspecname"].GetSafeString() + "\",\"materId\":\"" + dt[i]["materialid"].GetSafeString() + "\"},");
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
        /// 获取已添加的材料信息
        /// </summary>
        public void GetMaterialInfo()
        {
            StringBuilder sb = new StringBuilder();

            string sqlWhere = "";
            string type = Request["type"].GetSafeString();

            try
            {
                sqlWhere = $" and type='{type}' and  status <>'-1'  ";
                sb.Append("[");
                string sql = "select distinct materialid, materialname from OA_MaterialInfo where   jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"id\":\"" + dt[i]["materialid"].GetSafeString() + "\",\"name\":\"" + dt[i]["materialname"].GetSafeString() + "\"},");
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
        /// 获取材料规格
        /// </summary>
        public void GetMaterialInfoSpec()
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
                string sql = "select * from OA_MaterialInfo where jcjgbh ='" + CurrentUser.Qybh + "'" + sqlWhere;

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    sb.Append("{\"bh\":\"" + dt[i]["materialbh"].GetSafeString() + "\",\"id\":\"" + dt[i]["materialspecid"].GetSafeString() + "\",\"name\":\"" + dt[i]["materialspecname"].GetSafeString() + "\"},");
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

        #region 流程
        /// <summary>
        /// 添加考勤信息
        /// </summary>
        public void AttendanceInformationAdd()
        {
            bool code = true;
            string msg = "";
            string sqlStr = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string serialRecid = Request["guid"].GetSafeString();
                string type = Request["type"].GetSafeString();
                string ksbh = Request["ksbh"].GetSafeString();
                string rybh = Request["rybh"].GetSafeString();
                string startTime = Request["startTime"].GetSafeString();
                string predictEndTime = Request["predictEndTime"].GetSafeString();//计划返回时间
                string endTime = Request["endTime"].GetSafeString();//返回时间
                string remark = Request["remark"].GetSafeString();
                string peopleTogether = Request["peopleTogether"].GetSafeString();//同去人
                string WFDD = Request["WFDD"].GetSafeString();//往返地点
                string hours = Request["hours"].GetSafeString(); //预计时间
                string dispatchDuration = Request["dispatchDuration"].GetSafeString(); //派遣时长
                string roomCount = Request["roomCount"].GetSafeString(); //房间数量
                string isStay = Request["isStay"].GetSafeString(); //是否住宿
                string stayDays = Request["stayDays"].GetSafeString(); //住宿天数
                string invoiceHolder = Request["invoiceHolder"].GetSafeString(); //发票持有人
                string invoiceAmount = Request["invoiceAmount"].GetSafeString(); //发票金额
                string lx = Request["lx"].GetSafeString(); //请假类型

                string reviewerbh = Request["reviewerbh"].GetSafeString(); //审核人
                string reviewTime = Request["invoiceHolder"].GetSafeString(); //审核时间

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString("N");
                    sqlStr = $"INSERT INTO [dbo].[OA_AttendanceManage]([Recid],[Type],[RYBH],[RYXM],[StartTime],[EndTime],[Remark],[Status],[Hours],[LX],[PeopleTogether],[WFDD],[CreateTime],[Creater],[UpdateTime],[Updater],serialRecid,[JCJGBH]) " +
                        $"VALUES ('{recid}','{type}'" +
                        $",'{rybh}'" +
                        $",'{CurrentUser.RealName}'" +// RYXM, varchar(255),>
                        $",'{startTime}'" +// StartTime, datetime2(7),>
                        $",'{endTime}'" +// EndTime, datetime2(7),>
                        $",'{remark}'" +// Remark, nvarchar(255),>
                        $",'1'" +// Status, varchar(4),>
                        $",'{hours}'" +// Hours, decimal(10, 2),>
                        $",'{lx}'" +// LX, varchar(8),>
                        $",'{peopleTogether}'" +// PeopleTogether, nvarchar(50),>
                        $",'{WFDD}'" +// WFDD, nvarchar(50),>
                        $",getdate()" +// CreateTime, datetime2(7),>
                        $",'{CurrentUser.RealName}'" +// Creater, varchar(255),>
                        $",getdate()" +// UpdateTime, datetime2(7),>
                        $",'{CurrentUser.RealName}'" +// Updater, varchar(255),>
                        $",'{serialRecid}'" +// Updater, varchar(255),>
                        $",'{CurrentUser.Qybh}')";// JCJGBH, varchar(32),>)
                }
                else
                {
                    //sqlStr = "update OA_AttendanceManage set applicant='" + applicant + "', totalkilometers='" + totalkilometers + "'," +
                    //    " maintenancetime='" + maintenancetime + "' where id=" + dataId;
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
        /// 添加检查派遣
        /// </summary>
        public void DispatchRecordAdd()
        {
            bool code = false;
            string msg = "";
            string sqlStr = "";
            try
            {
                string recid = Request["recid"].GetSafeString();
                string serialRecid = Request["guid"].GetSafeString();
                string ksbh = Request["ksbh"].GetSafeString();
                string htbh = Request["htbh"].GetSafeString();
                string startTime = Request["startTime"].GetSafeString();
                string returnTime = Request["returnTime"].GetSafeString();//返回时间
                string peopleTogether = Request["peopleTogether"].GetSafeString();//同去人
                string GZDQ = Request["GZDQ"].GetSafeString();//工作地区
                string GCMC = Request["GCMC"].GetSafeString();//往返地点
                string JCXM = Request["JCXM"].GetSafeString();//检测项目
                string GCL = Request["GCL"].GetSafeString();//工程量
                string carId = Request["carId"].GetSafeString(); //车牌号
                string dispatchDuration = Request["dispatchDuration"].GetSafeString(); //派遣时长
                string roomCount = Request["roomCount"].GetSafeString(); //房间数量
                string isStay = Request["isStay"].GetSafeString(); //是否住宿
                string stayDays = Request["stayDays"].GetSafeString(); //住宿天数
                string invoiceHolder = Request["invoiceHolder"].GetSafeString(); //发票持有人
                string invoiceAmount = Request["invoiceAmount"].GetSafeString(); //发票金额
                string reviewerbh = Request["reviewerbh"].GetSafeString(); //审核人
                string reviewTime = Request["invoiceHolder"].GetSafeString(); //审核时间

                IList<string> sqls = new List<string>();
                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString("N");
                    sqlStr = $"INSERT INTO [dbo].[OA_DispatchRecord]([Recid],SerialRecid,[HTBH],[RYMC],[RYBH],[GZDQ],[GCMC],[JCXM],[GCL],[StartTime],[ReturnTime],[CarId],[PeopleTogether],[StayDays],[InvoiceHolder],[InvoiceAmount],[JCJGBH],[CreateTime],[Creator],[UpdateTime],[Updater],[Status])" +
                        $"VALUES('{recid}'" +
                        $",'{serialRecid}'" +
                        $",'{htbh}'" +
                        $",'{CurrentUser.RealName}'" +//< RYMC, varchar(255),>
                        $",'{CurrentUser.UserCode}'" +//< RYBH, varchar(255),>
                        $",'{GZDQ}'" +//< GZDQ, varchar(255),>
                        $",'{GCMC}'" +//< GCMC, varchar(255),>
                        $",'{JCXM}'" +//< JCXM, varchar(255),>
                        $",'{GCL}'" +//< GCL, varchar(255),>
                        $",'{startTime}'" +//< StartTime, datetime2(7),>
                        $",'{returnTime}'" +//< ReturnTime, datetime2(7),>
                        $",'{carId}'" +//< CarId, int,>
                        $",'{peopleTogether}'" +//< PeopleTogether, varchar(255),>
                        $",'{stayDays}'" +//< StayDays, decimal(18, 0),>
                        $",'{invoiceHolder}'" +//< InvoiceHolder, varchar(255),>
                        $",'{invoiceAmount}'" +//< InvoiceAmount, varchar(255),>
                        $",'{CurrentUser.Qybh}'" +//< JCJGBH, varchar(255),>
                        $",getdate()" +//< CreateTime, datetime2(7),>
                        $",'{CurrentUser.RealName}'" +//< Creator, varchar(255),>
                        $",getdate()" +//< UpdateTime, datetime2(7),>
                        $",'{CurrentUser.RealName}'" +//< Updater, nvarchar(255),>
                        $",'1')";
                }
                else
                {
                    //sqlStr = "update OA_DispatchRecord set applicant='" + applicant + "', totalkilometers='" + totalkilometers + "'," +
                    //    " maintenancetime='" + maintenancetime + "' where id=" + dataId;
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

        #region 首页看板
        #region 合同、工程、报告统计
        /// <summary>
        /// 合同信息统计（首页看板）
        /// </summary>
        public void GetHTDataStatistic()
        {
            string code = "0";
            string msg = "";
            string strSql = "";
            string sqlWhere = "";
            string gcbh = Request["gcbh"].GetSafeString();
            string userCode = Request["userCode"].GetSafeString();
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();

            if (string.IsNullOrEmpty(userCode))
            {
                userCode = CurrentUser.UserCode;
            }
            userCode = "URCQ1uHnsuOjIN";
            IList<IDictionary<string, object>> retData = new List<IDictionary<string, object>>();
            //List<string,object> retData= new List<object>();
            try
            {
                #region 合同数据
                sqlWhere = "";
                strSql = $"select htzt_dybg,htzt_gz,htzt_js,jgzt,jcht.GCBH from View_I_M_XJCHT as jcht " +
                          $"left join  I_M_GC as gc on jcht.GCBH = gc.GCBH " +
                          $"where((jcjgbh = (select qybh from i_m_qyzh where yhzh = '{userCode}') " +
                          $"or jcjgbh = (select jcjgbh from i_m_nbry_jc where usingnow = 1  and usercode = '{userCode}') " +
                          $"or ywyxmbh = '{userCode}' ))";
                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and jcht.LRSJ >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and jcht.LRSJ <='{endTime}' ";
                }
                strSql += sqlWhere;
                var datas = CommonService.GetDataTable(strSql);

                var HTDatas = new
                {
                    Total = datas.Count,  //总数量
                    //未存档未完工
                    UnSaveAndUnFinish = datas.Where(u => u["htzt_gz"] == "否" && string.IsNullOrEmpty(u["jgzt"]) == true).ToList().Count,
                    //已存档未完工
                    SaveAndUnFinish = datas.Where(u => u["htzt_gz"] == "是" && string.IsNullOrEmpty(u["jgzt"]) == true).ToList().Count,
                    //已存档已完工
                    SaveAndFinish = datas.Where(u => u["htzt_gz"] == "是" && string.IsNullOrEmpty(u["jgzt"]) == false).ToList().Count,
                    //不能打印报告的合同
                    CannotPrint = datas.Where(u => u["htzt_dybg"] == "否").ToList().Count,
                };
                #endregion

                #region  工程数据
                bool isPay = false;
                bool isFinish = false;
                int finishAndPayCount = 0;
                int finishAndUnPayCount = 0;
                int unFinishAndUnPayCount = 0;
                sqlWhere = "";
                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and a.LRSJ >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and a.LRSJ <='{endTime}' ";
                }

                strSql = $"select  GCBH,jgzt from I_M_GC a " +
                   $"where ((exists (select 1 from i_m_nbry_jc b where usingnow=1  and usercode='{userCode}' and a.SSJCJGBH =b.JCJGBH)) " +
                   $"or exists(select* from i_m_qyzh c where yhzh = '{userCode}' and a.SSJCJGBH= c.QYBH)) " + sqlWhere +
                   $"order by gcbh desc";

                var GCData = CommonService.GetDataTable(strSql);
                foreach (var item in GCData)
                {
                    isPay = datas.Where(u => u["gcbh"] == item["gcbh"] && u["htzt_js"] == "是").ToList().Count == 0 ? false : true;
                    isFinish = string.IsNullOrEmpty(item["jgzt"]) ? false : true;

                    if (isFinish && isPay)
                    {
                        finishAndPayCount++;
                    }
                    else if (isFinish && !isPay)
                    {
                        finishAndUnPayCount++;
                    }
                    else if (!isFinish && !isPay)
                    {
                        unFinishAndUnPayCount++;
                    }
                    else
                    {
                        //已付款未完工的情况是否存在？
                        unFinishAndUnPayCount++;
                    }
                }

                var GCDatas = new
                {
                    //总数量
                    Total = GCData.Count,
                    //已完工已结清
                    FinishAndPay = finishAndPayCount,
                    //已完工未结清
                    FinishAndUnPay = finishAndUnPayCount,
                    //未完工未结清
                    UnFinishAndUnPay = unFinishAndUnPayCount
                };
                #endregion

                #region 外检报告
                sqlWhere = "";
                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and TJSJ >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and TJSJ <='{endTime}' ";
                }

                //获取所有委托单信息
                strSql = $"select ZT,jcjg  from m_by_bg WHERE WTDBH in ( select  WTDBH from View_WTDJD_LB where sy_jcjgbh in ( " +
                    $"select distinct qybh  from i_m_qy i " +
                    $"left join I_M_NBRY_JC j on j.jcjgbh = i.qybh or j.jcjgbh = i.parentQYBH " +
                    $"where j.usercode = '{CurrentUser.UserCode}') " +
                    $"and isnull(jysj,'1900-01-01')<> '1900-01-01' " +
                    $"and sy_wjxm = '是' )" +
                    $"and ISNULL(SYRZH,'')= ''";
                strSql += sqlWhere;

                var WTDDatas = CommonService.GetDataTable(strSql);
                //B0000040000000000000
                //外检数据
                var WJDatas = new
                {
                    Total = WTDDatas.Count,  //总数量
                    //已出报告
                    OutReport = WTDDatas.Where(u => "567".Contains(u["zt"].Substring(6, 1))).ToList().Count,
                    //未出报告
                    NoReport = WTDDatas.Where(u => !"567".Contains(u["zt"].Substring(6, 1))).ToList().Count,
                    //不合格报告
                    UnqualifiedReport = WTDDatas.Where(u => "567".Contains(u["zt"].Substring(6, 1)) && u["jcjg"] == "不合格").ToList().Count
                };
                #endregion

                #region 内检报告
                //获取所有委托单信息
                strSql = $"select ZT,jcjg  from m_by_bg WHERE WTDBH in ( select  WTDBH from View_WTDJD_LB where sy_jcjgbh in ( " +
                    $"select distinct qybh  from i_m_qy i " +
                    $"left join I_M_NBRY_JC j on j.jcjgbh = i.qybh or j.jcjgbh = i.parentQYBH " +
                    $"where j.usercode = '{CurrentUser.UserCode}') " +
                    $"and isnull(jysj,'1900-01-01')<> '1900-01-01' " +
                    $"and sy_wjxm = '否' )" +
                    $"and ISNULL(SYRZH,'')<> ''";

                strSql += sqlWhere;
                WTDDatas = CommonService.GetDataTable(strSql);

                //内检数据
                var NJDatas = new
                {
                    Total = WTDDatas.Count,  //总数量
                    //已出报告
                    OutReport = WTDDatas.Where(u => "567".Contains(u["zt"].Substring(7, 1))).ToList().Count,
                    //未出报告
                    NoReport = WTDDatas.Where(u => !"567".Contains(u["zt"].Substring(7, 1))).ToList().Count,
                    //不合格报告
                    UnqualifiedReport = WTDDatas.Where(u => "567".Contains(u["zt"].Substring(7, 1)) && u["jcjg"] == "不合格").ToList().Count
                };

                #endregion

                IDictionary<string, object> dicObj = new Dictionary<string, object>();

                dicObj.Add("HTDatas", HTDatas);
                dicObj.Add("GCDatas", GCDatas);
                dicObj.Add("WJDatas", WJDatas);
                dicObj.Add("NJDatas", NJDatas);
                retData.Add(dicObj);
            }
            catch (Exception ex)
            {
                code = "1";
                msg = "获取合同、工程、报告统计数据异常";
                SysLog4.WriteError("获取首页看板信息异常【GetHTDataStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code, msg, jss.Serialize(retData)));
                Response.End();
            }
        }
        #endregion

        #region 人员统计
        /// <summary>
        /// 人员统计（首页看板）
        /// </summary>
        public void GetPersionDataStatistic()
        {
            string code = "0";
            string msg = "";
            string strSql = "";

            IList<IDictionary<string, object>> retData = new List<IDictionary<string, object>>();
            IDictionary<string, object> dicObj = new Dictionary<string, object>();

            try
            {
                // 部门人员统计
                strSql = $" select count(1) as TypeCount, SSKSBH ,jcks.KSMC as TypeName from I_M_NBRY_JC nbry left join h_jcks jcks on jcks.KSBH=nbry.SSKSBH " +
                 $"where jcjgbh ='{CurrentUser.Qybh}' " +
                 $"group by SSKSBH,jcks.KSMC";
                var datas = CommonService.GetDataTable(strSql);

                dicObj.Add("Depmartment", datas);
                // 岗位统计
                strSql = $"select  count(1) as TypeCount,GWMC as TypeName from OA_UserArchives  archives left join OA_OperatingPost post on archives.KSBH=post.KSBH " +
                 $"where jcjgbh ='{CurrentUser.Qybh}' " +
                 $"group by GWMC";
                datas = CommonService.GetDataTable(strSql);
                dicObj.Add("OperatingPost", datas);

                retData.Add(dicObj);
            }
            catch (Exception ex)
            {
                code = "1";
                msg = "获取人员统计信息异常";
                SysLog4.WriteError("获取人员统计信息异常【GetPersionDataStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code, msg, jss.Serialize(retData)));
                Response.End();
            }
        }
        #endregion

        #region 公告&文件
        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <returns></returns>
        //[LoginAuthorize]
        public void GetAnnouncementList()
        {
            bool code = true;
            string msg = "";
            string where = "";
            int totalcount = 0;
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(20);
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string recid = Request["recid"].GetSafeString();
                if (recid != "")
                    where = $" and recid='{recid}'";
                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    where += $" and LRSJ >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    where += $" and LRSJ <='{endTime}' ";
                }

                string sql = $"select recid,title,LRSJ from AnnouncementNotice where jcjgbh='{CurrentUser.Qybh}' {where} order by lrsj desc";
                datas = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
            }
            catch (Exception ex)
            {
                code = false;
                msg = "获取公告列表异常";
                SysLog4.WriteError(msg + "【GetAnnouncementList】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 预警信息
        /// <summary>
        /// 获取预警信息
        /// </summary>
        /// <returns></returns>
        //[LoginAuthorize]
        public void GetWarningInfo()
        {
            bool code = true;
            string msg = string.Empty;
            string sql = string.Empty;
            string sqlWhere = string.Empty;
            string lxbh = string.Empty;
            string lxmc = string.Empty;
            int unHandledCount = 0; //未处理
            int handledCount = 0;//已处理预警

            string jcjgbh = CurrentUser.Qybh;
            List<object> datas = new List<object>();
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();
            try
            {
                if (!string.IsNullOrEmpty(jcjgbh))
                {
                    if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                    {
                        sqlWhere += $" and SCSJ >='{startTime}' ";
                    }
                    if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                    {
                        sqlWhere += $" and SCSJ <='{endTime}' ";
                    }

                    //加个配置条件
                    sql = $"select yj.lxbh,yj.zt from new_yj_jcht yj left join i_m_jcht ht on ht.recid =yj.htrecid where ht.jcjgbh='{jcjgbh}'";
                    sql += sqlWhere;
                    var yjdatas = CommonService.GetDataTable(sql);
                    sql = $"select * from h_yjlx where sfxs='1' and jcjgbh='{jcjgbh}' order by xssx";
                    var yjxxdatas = CommonService.GetDataTable(sql);
                    foreach (var item in yjxxdatas)
                    {
                        lxbh = item["lxbh"];
                        lxmc = item["reallxmc"];

                        if (lxbh == "09")
                        {
                            sql = $"select recid from view_yj_sb where days>-30  and  ssdwbh ='{CurrentUser.Qybh}' order by lrsj desc";
                            unHandledCount = CommonService.GetDataTable(sql).Count;
                        }
                        else
                        {
                            unHandledCount = yjdatas.Where(x => x["zt"].GetSafeString() == "0").Count(x => x["lxbh"] == lxbh).GetSafeInt();
                            handledCount = yjdatas.Where(x => x["zt"].GetSafeString() == "1").Count(x => x["lxbh"] == lxbh).GetSafeInt();
                        }

                        object obj = new { lxbh, lxmc, unHandledCount, handledCount };
                        datas.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                code = false;
                msg = "获取预警信息异常";
                SysLog4.WriteError(msg + "【GetWarningInfo】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 获取一页待办事项
        //[LoginAuthorize]
        public void GetWorkTodoTasks()
        {
            bool code = true;
            string msg = "";
            string strSql = string.Empty;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            try
            {
                var userName = CurrentUser.UserName;
                string startTime = Request["startTime"].GetSafeString();
                string endTime = Request["endTime"].GetSafeString();

                strSql = $"select count(1) as count,process.ProcessID,process.ProcessName  from STFORM  st left join STPROCESS process on process.ProcessID= st.ProcessID " +
                    $"where SerialNo in (select  SerialNo from STTODOTASKs where UserID = '{userName}') " +
                    $"group by process.ProcessID,process.ProcessName ";

                datas = CommonService.GetDataTable(strSql);

            }
            catch (Exception ex)
            {
                code = false;
                msg = "获取一页待办事项异常";
                SysLog4.WriteError(msg + "【GetWorkTodoTasks】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 开票金额
        /// <summary>
        /// 获取发票金额统计
        /// </summary>
        /// <returns></returns>
        //[LoginAuthorize]
        public void GetInvoiceStatistic()
        {
            bool code = true;
            string msg = "";
            string strSql = string.Empty;
            string sqlWhere = string.Empty;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            string jcjgbh = CurrentUser.Qybh;
            decimal invoiceTotal = 0;//总金额
            decimal invoiceYDZ = 0;//已到账
            decimal invoiceWDZ = 0;//未到账
            try
            {
                var userName = CurrentUser.UserName;
                string startTime = Request["startTime"].GetSafeString();
                string endTime = Request["endTime"].GetSafeString();
                if (!string.IsNullOrEmpty(startTime))
                {
                    sqlWhere += $" and datediff(dd,invoiceDate,'{startTime}')<=0";
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    sqlWhere += $" and datediff(dd,invoiceDate,'{endTime}')>=0";
                }
                strSql = $"select * from H_Invoice_QY where jcjgbh='{jcjgbh}'";
                var xfqy = CommonService.GetDataTable(strSql);
                string xfqybh = xfqy.Count > 0 ? xfqy[0]["xfqybh"] : "";

                //总开发票金额
                strSql = $"select sum(invoiceTotalPriceTax) as invoiceTotalPriceTax " +
                    $"from i_m_invoice where dwbh in ('{jcjgbh}','{xfqybh}')  and InvoiceStatus<>'3'";

                strSql += sqlWhere;
                datas = CommonService.GetDataTable(strSql);

                invoiceTotal = datas.Count > 0 ? datas[0]["invoicetotalpricetax"].GetSafeDecimal() : 0;
                //已开发票的已到账金额
                strSql = $"select sum(ydz) as totalydz from i_m_dzjl where jcjgbh = '{jcjgbh}' and(lx = '1' or lx = '3')";
                sqlWhere = "";
                if (!string.IsNullOrEmpty(startTime))
                {
                    sqlWhere += $" and datediff(dd,dzsj,'{startTime}')<=0";
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    sqlWhere += $" and datediff(dd,dzsj,'{endTime}')>=0";
                }
                strSql += sqlWhere;

                datas = CommonService.GetDataTable(strSql);

                invoiceYDZ = datas.Count > 0 ? Math.Round(datas[0]["totalydz"].GetSafeDecimal(), 2) : 0;

                invoiceWDZ = invoiceTotal - invoiceYDZ;

            }
            catch (Exception ex)
            {
                code = false;
                msg = "获取发票金额统计异常";
                SysLog4.WriteError(msg + "【GetInvoiceStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 获取不合格数量报告类型统计
        /// <summary>
        /// 获取不合格数量报告类型统计
        /// </summary>
        /// <returns></returns>
        //[LoginAuthorize]
        public void GetReportTypeStatistic()
        {
            bool code = true;
            string msg = "";
            string strSql = "";
            int totalcount = 0;
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(20);
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();
            string sortType = Request["sortType"].GetSafeString("");
            string reportType = Request["reportType"].GetSafeString("");

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

            try
            {
                string where = "and isnull(tjsj,'1900-01-01')<> '1900-01-01' ";

                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    where += $" and tjsj >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    where += $" and tjsj <='{endTime}' ";
                }
                if (string.IsNullOrEmpty(reportType))
                {
                    where += $" and jcjg = '不合格' ";
                }
                if (string.IsNullOrEmpty(sortType))
                {
                    sortType = " desc ";
                }

                strSql = $"select syxm.SYXMBH,syxm.SYXMMC,count(1) as count from m_by_bg  bg  " +
                   $"inner join PR_M_SYXM as syxm on syxm.SYXMBH = bg.SYXMBH " +
                   $"WHERE WTDBH in ( select WTDBH from View_WTDJD_LB where sy_jcjgbh ='{CurrentUser.Qybh}') " +
                   $"{where} " +
                   $"group by syxm.SYXMBH,syxm.SYXMMC " +
                   $"order by count {sortType}";
                datas = CommonService.GetPageData(strSql, pagesize, pageindex, out totalcount);

            }
            catch (Exception ex)
            {
                code = false;
                msg = "获取不合格数量报告类型统计信息异常";
                SysLog4.WriteError(msg + "【GetReportTypeStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(datas)));
                Response.End();
            }
        }
        #endregion

        #region 检测费用
        /// <summary>
        /// 检测费用统计
        /// </summary>
        public void GetCheckCostStatistic()
        {
            string code = "0";
            string msg = "";
            string strSql = "";
            string sqlWhere = "";
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();
            decimal totalCost = 0;

            IList<IDictionary<string, object>> retData = new List<IDictionary<string, object>>();
            try
            {
                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and scsj >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and scsj <='{endTime}' ";
                }


                #region 企业订单
                strSql = $"select SSFY,DDState from m_by_dd " +
                    $"where HTRECID in (select RECID from I_M_JCHT  where  JCJGBH='{CurrentUser.Qybh}' and HTLX='企业合同' ) ";
                strSql += sqlWhere;

                var datas = CommonService.GetDataTable(strSql);

                var QYOrder = new
                {
                    //已付款
                    HasPay = datas.Where(u => u["ddstate"] == "4").ToList().Sum(u => u["ssfy"].GetSafeDecimal()),
                    //未付款
                    UnPay = datas.Where(u => u["ddstate"] == "1").ToList().Sum(u => u["ssfy"].GetSafeDecimal()),
                };
                #endregion

                #region 临时订单
                strSql = $"select SSFY,DDState from m_by_dd " +
                    $"where HTRECID in (select RECID from I_M_JCHT  where  JCJGBH='{CurrentUser.Qybh}' and HTLX='临时合同' )) ";

                strSql += sqlWhere;
                datas = CommonService.GetDataTable(strSql);


                var LSOrder = new
                {
                    //已付款
                    HasPay = datas.Where(u => u["ddstate"] == "4").ToList().Sum(u => u["ssfy"].GetSafeDecimal()),
                    //已存档未完工
                    UnPay = datas.Where(u => u["ddstate"] == "1").ToList().Sum(u => u["ssfy"].GetSafeDecimal()),
                };

                #endregion

                #region 专项合同
                strSql = $"select SSFY,DDState from m_by_dd " +
                    $"where HTRECID in (select RECID from I_M_JCHT  where  JCJGBH='{CurrentUser.Qybh}' and HTLX='专项合同' )) ";
                strSql += sqlWhere;
                datas = CommonService.GetDataTable(strSql);


                var ZXOrder = new
                {
                    //已付款
                    HasPay = datas.Where(u => u["ddstate"] == "4").ToList().Sum(u => u["ssfy"].GetSafeDecimal()),
                    //已存档未完工
                    UnPay = datas.Where(u => u["ddstate"] == "1").ToList().Sum(u => u["ssfy"].GetSafeDecimal()),
                };

                #endregion
                IDictionary<string, object> dicObj = new Dictionary<string, object>();

                totalCost = QYOrder.HasPay + QYOrder.UnPay + LSOrder.HasPay + LSOrder.UnPay + ZXOrder.HasPay + ZXOrder.UnPay;
                dicObj.Add("QYData", QYOrder);
                dicObj.Add("LSData", LSOrder);
                dicObj.Add("ZXData", ZXOrder);

                retData.Add(dicObj);
            }
            catch (Exception ex)
            {
                code = "1";
                msg = "获取检测费用统计数据异常";
                SysLog4.WriteError(msg + "【GetCheckCostStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"totalCost\": {2},\"data\": {3}}}", code, msg, totalCost, jss.Serialize(retData)));
                Response.End();
            }
        }
        #endregion

        #region 外检单位信息统计
        /// <summary>
        /// 外检单位信息统计
        /// </summary>
        public void GetWJDWDataStatistic()
        {
            string code = "0";
            string msg = "";
            string strSql = "";
            string sqlWhere = "";
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();
            int total = 0;

            IList<IDictionary<string, string>> retData = new List<IDictionary<string, string>>();
            try
            {
                #region  外检单位数据

                if (!string.IsNullOrEmpty(startTime) && startTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and scsj >='{startTime}' ";
                }
                if (!string.IsNullOrEmpty(endTime) && endTime.GetSafeDate() != new DateTime(1900, 1, 1))
                {
                    sqlWhere += $" and scsj <='{endTime}' ";
                }
                strSql = $"select  mby.QZJCJGMC, mby.QZJCJGBH, SSFY,DDState,InvoiceStatus from m_by_dd dd " +
                 $"inner join m_by mby on dd.BYZBRECID = mby.RECID " +
                 $"where ISNULL(mby.QZJCJGBH, '') <> '' " +
                 $"and HTRECID in (select RECID from I_M_JCHT  where  JCJGBH='{CurrentUser.Qybh}') " +
                 $"and DDState in (1, 4) ";
                strSql += sqlWhere;

                var allDatas = CommonService.GetDataTable(strSql);
                //--发票状态 InvoiceStatus  0未申请，1已申请，2已开票，4已打印, 6部分打印
                //--订单状态 DDState  0 未生成订单,
                //--1 已生成订单,    
                //--2 申请退回订单,
                //--3 申请退回失败,
                //--4 订单已支付,
                //--5 订单已退款
                //--外检订单费用

                var orderDatas = allDatas.Where(u => !string.IsNullOrEmpty(u["qzjcjgbh"])).ToList();
                var wjdwDatas = CommonService.GetDataTable($"select qymc,qybh from wjdw  where JCJGBH='{CurrentUser.Qybh}'");

                Dictionary<string, string> dicObj = new Dictionary<string, string>();

                foreach (var item in wjdwDatas)
                {
                    dicObj = new Dictionary<string, string>();
                    //机构名称
                    dicObj.Add("JcjgName", item["qymc"]);
                    //总检查费用
                    dicObj.Add("TotalCost", orderDatas.Where(u => u["qzjcjgbh"] == item["qybh"]).ToList().Sum(u => u["ssfy"].GetSafeDecimal()).ToString());
                    //已付金额
                    dicObj.Add("HasPay", orderDatas.Where(u => u["qzjcjgbh"] == item["qybh"] && u["ddstate"] == "4").ToList().Sum(u => u["ssfy"].GetSafeDecimal()).ToString());
                    //未付金额
                    dicObj.Add("UnPay", orderDatas.Where(u => u["qzjcjgbh"] == item["qybh"] && u["ddstate"] == "1").ToList().Sum(u => u["ssfy"].GetSafeDecimal()).ToString());
                    //开票已到账
                    dicObj.Add("YDZ", "0");
                    //开票未到账
                    dicObj.Add("WDZ", "0");
                    retData.Add(dicObj);

                }


                #endregion


            }
            catch (Exception ex)
            {
                code = "1";
                msg = "外检单位信息统计数据异常";
                SysLog4.WriteError(msg + "【GetWJDWDataStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\": {2},\"data\": {3}}}", code, msg, retData.Count, jss.Serialize(retData)));
                Response.End();
            }
        }
        #endregion

        #region 其他信息统计
        /// <summary>
        /// 其他信息统计
        /// </summary>
        public void GetOtherDataStatistic()
        {
            string code = "0";
            string msg = "";
            string strSql = "";
            string sqlWhere = "";
            string startTime = Request["startTime"].GetSafeString();
            string endTime = Request["endTime"].GetSafeString();

            IList<IDictionary<string, string>> retData = new List<IDictionary<string, string>>();
            try
            {
                Dictionary<string, string> dicObj = new Dictionary<string, string>();

                #region   信息统计
                //人员数量
                strSql = $"select count(1   from I_M_NBRY_JC where JCJGBH='{CurrentUser.Qybh}' and usingnow =1 ";
                var datas = CommonService.GetDataTable(strSql);
                dicObj.Add("UserCount", datas.Count().ToString());

                //汽车数量
                strSql = $"select count(1) from OA_CarInfomation where JCJGBH='{CurrentUser.Qybh}' and Status =1 ";
                datas = CommonService.GetDataTable(strSql);
                dicObj.Add("CarCount", datas.Count().ToString());

                //设备数量
                strSql = $"select count(1) from OA_CarInfomation where JCJGBH='{CurrentUser.Qybh}' and Status =1 ";
                datas = CommonService.GetDataTable(strSql);
                dicObj.Add("DeviceCount", datas.Count().ToString());

                //固定资产
                dicObj.Add("AssetsCount", "0");

                //耗材数量
                strSql = $"select count(1) from OA_CarInfomation where JCJGBH='{CurrentUser.Qybh}' and Status =1 ";
                datas = CommonService.GetDataTable(strSql);
                dicObj.Add("ConsumeMaterialCount", datas.Count().ToString());

                //文件数量
                //strSql = $"select count(1) from OA_CarInfomation where JCJGBH='{CurrentUser.Qybh}' and Status =1 ";
                //datas = CommonService.GetDataTable(strSql);
                dicObj.Add("StandardCount", "0");


                //标准数量
                //strSql = $"select count(1) from OA_CarInfomation where JCJGBH='{CurrentUser.Qybh}' and Status =1 ";
                //datas = CommonService.GetDataTable(strSql);
                dicObj.Add("StandardCount", "0");

                //印章数量
                strSql = $"select count(1) from OA_SignatureManage where JCJGBH='{CurrentUser.Qybh}' and Status =1 ";
                datas = CommonService.GetDataTable(strSql);
                dicObj.Add("SignatureCount", datas.Count().ToString());

                #endregion

                retData.Add(dicObj);

            }
            catch (Exception ex)
            {
                code = "1";
                msg = "外检单位信息统计数据异常";
                SysLog4.WriteError(msg + "【GetWJDWDataStatistic】：" + ex.ToString());
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"total\": {2},\"data\": {3}}}", code, msg, retData.Count, jss.Serialize(retData)));
                Response.End();
            }
        }
        #endregion

        #endregion

    }
    static class Extensions
    {
        public static T Cast<T>(this object obj, T sample)
        {
            return (T)obj;
        }
    }
}

