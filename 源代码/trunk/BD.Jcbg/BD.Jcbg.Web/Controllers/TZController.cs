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
using CryptFun = BD.Jcbg.Common.CryptFun;
using BD.Jcbg.Web.Remote;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;

namespace BD.Jcbg.Web.Controllers
{
    public class TZController : Controller
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


        #endregion
        #region 页面


        private string wgryrul = "http://120.27.218.55:8001/";

        /// <summary>
        /// 欢迎页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Welcome()
        {

            ViewData["Page1"] = "display:none;";
            ViewData["Page2"] = "";
            return View();
        }



        /// <summary>
        /// 欢迎页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Welcome2()
        {

            ViewData["Page1"] = "display:none;";
            ViewData["Page2"] = "";
            return View();
        }


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
        /// 传感器地图界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult cgqshow()
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
        /// 登录成功后主界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Main()
        {
            /*
            if (CurrentUser.CurUser.DepartmentId.Equals("DP201405000001") || CurrentUser.CurUser.DepartmentId.Equals("DP201802000003")) //信访/政府部门
            {
                ViewData["Welcome"] = "/welcome/welcomezf";
                // ViewData["Welcome"] = "welcomeqy";
                // ViewData["Welcome"] = "welcomegc";
            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("DP201802000002")) //五方主体
            {
                ViewData["Welcome"] = "/welcome/welcomeqy";             
            }
            else if (CurrentUser.CurUser.DepartmentId.Equals("DP201802000001")) //劳务部门
            {
                ViewData["Welcome"] = "/welcome/welcomegc";
            }
            else
            {
                ViewData["Welcome"] = "/user/welcome";
            }*/
            //string welcome = GlobalVariable.GetSysSettingValue(true, "GLOBAL_PAGE_MAIN_URL");
            //if (string.IsNullOrEmpty(welcome))
            string jsdw = "";
            if (CurrentUser.CurUser.UrlJumpType == "Q")
            {

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select * from SB_ReportSBSY where UnInstallProgStatus!=4 and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))");
                if (dt.Count > 0)
                    jsdw = "1";
            }

            string welcome = "/user/welcome";
            ViewBag.url = welcome;
            ViewBag.Realname = CurrentUser.RealName;
            ViewBag.isjsdw = jsdw;
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
        /// 务工人员企业
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult KQJL()
        {


            ViewBag.sfzhm = Request["sfzhm"].GetSafeString();
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
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
                ViewBag.sfzh = dt[i]["sfzhm"].Replace('\\', '-');
                ViewBag.sjhm = dt[i]["sjhm"].Replace('\\', '-');
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



        /// <summary>
        /// 考试结果输入界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult KSJG()
        {


            ViewBag.recid = Request["recid"].GetSafeString();

            return View();
        }


        /// <summary>
        /// 务工人员首页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ifr()
        {
            ViewBag.url = Request["url"].GetSafeString();
            return View();
        }


        /// <summary>
        /// 视频列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult splist()
        {


            ViewBag.gcmc = Request["gcmc"].GetSafeString();
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }


        /// <summary>
        /// 视频播放
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult videolist()
        {

            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            return View();
        }



        /// <summary>
        /// 视频播放
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult video()
        {

            ViewBag.code = Request["code"].GetSafeString();
            return View();
        }


        /// <summary>
        /// 工程多平界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult gcview()
        {

            ViewBag.gcbh = Request["gcbh"].GetSafeString();
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


        /// <summary>
        /// 扬尘预警列表 
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult DustErrorList(string deviceCode)
        {
            ViewData["DeviceCode"] = deviceCode;
            return View();
        }

        /// <summary>
        /// 扬尘数据列表 
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult PageDustList(string deviceCode)
        {
            ViewData["DeviceCode"] = deviceCode;
            return View();
        }



        /// <summary>
        /// 扬尘数据列表 
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult DeviceChart(string deviceCode)
        {
            ViewData["DeviceCode"] = deviceCode;
            return View();
        }

        /// <summary>
        /// 视频地图 
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult VideoMap()
        {
            return View();
        }
        /// <summary>
        /// 视频中心 
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public ActionResult VideoIndex()
        {
            return View();
        }


        #endregion


        #region 数据处理

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
                string sfzhm = Request["sfzhm"].GetSafeString();
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
                if (sfzhm != "")
                    sql += " and a.sfzhm like '%" + sfzhm + "%'";
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
            param.t2_tablename = "I_S_GC_SGDW|I_S_GC_JSDW|I_S_GC_JLDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_TSDW|I_S_GC_FGC|I_S_GC_FBDW";
            ////主键
            param.t2_pri = "GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID|GCBH,GCQYBH";
            ////标题
            param.t2_title = "施工单位|建设单位|监理单位|勘察单位|设计单位|图审单位|单位工程|分包单位";

            param.t3_tablename = "I_S_GC_SGDW|I_S_GC_SGRY||I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_FBDW|I_S_GC_FBRY";
            param.t3_title = "施工人员|建设人员|监理人员|勘察人员|设计人员|分包人员";
            param.t3_pri = "GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID";

            //param.preproc = "data_input_check_Use|$SB_UseReg.CheckDate|$SB_UseReg.ADetectDate|$SB_UseReg.BDetectDate";

            param.js = "searchRYZS.js";

            param.lx = strlx;


            param.rownum = 2;
            param.view = true;
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            return RedirectToAction("Index", "DataInput", param);
        }


        public void gotoZAJ()
        {

            //SendDataByGET("http://101.37.84.226:10001/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            string token = RSAUtil.GetToken("jcgl", "", "88888");
            //Response.RedirectPermanent("http://120.27.218.55:8001/user/main/");
            string url = "http%3a%2f%2fwzlcq.jzyglxt.com%2fuser%2fmain";
            Response.Redirect("LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
        }
        public void gotoSelfWG()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

           
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = "http%3a%2f%2f120.27.218.55%3a8001%2fWzWgry%2findex";
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        public void gotoWG()
        {

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";
            /*
            string url = "http%3a%2f%2f120.27.218.55%3a8001%2fWzWgry%2findex";
            string token = RSAUtil.GetToken("zf", "1", "88888");
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
             * */

            string username = "zf";
            string url = "http%3a%2f%2f120.27.218.55%3a8001%2fWzWgry%2findex";
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

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




        /// <summary>
        /// 务工人员预警
        /// </summary>
        public void gotoWGRYYJ()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

           
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=YJ_RY&FormStatus=0");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        /// <summary>
        /// 务工薪资预警
        /// </summary>
        public void gotoWGXZYJ()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

           
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=YJ_XZ&FormStatus=0");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        /// <summary>
        /// 务工设备预警
        /// </summary>
        public void gotoWGSBYJ()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=YJ_SB&FormStatus=0");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }


        /// <summary>
        /// 务工所辖工程
        /// </summary>
        public void gotoWGSXGC()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=XQXX_GC&FormStatus=1&FormParam=PARAM--ALL");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }


        /// <summary>
        /// 务工所辖务工人员
        /// </summary>
        public void gotoWGSXWGRY()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

           
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=XQXX_RY&FormStatus=1");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }



        /// <summary>
        /// 务工实时进度
        /// </summary>
        public void gotoWGSSJD()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=I_S_GC_SSJD&FormStatus=0");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }



        /// <summary>
        /// 上线的工程
        /// </summary>
        public void gotoWGZXGC()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/SimpleShow/Index");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        /// <summary>
        /// 务工工资发放
        /// </summary>
        public void gotoWGGZFF()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

          
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=XQXX_GZFF&FormStatus=1");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        /// <summary>
        /// 务工人员黑名单
        /// </summary>
        public void gotoWGRYHMD()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

         
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=XQXX_RYHMD&FormStatus=1");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        /// <summary>
        /// 务工企业黑名单
        /// </summary>
        public void gotoWGQYHMD()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

            
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=XQXX_QYHMD&FormStatus=1");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }


        /// <summary>
        /// 务工人员信访
        /// </summary>
        public void gotoWGXF()
        {
            /*

            //SendDataByGET("http://101.37.84.226:20002/user/dologin?login_name=wzzz&login_pwd=888888");
            //param.button = "保存|TJ| |fa fa-calendar-times-o|保存成功！";

           
            Response.Redirect("http://120.27.218.55:8001/LoginJump/Index?ClientKey=" + token + "&ul=" + url);
            //return RedirectToAction("http://wz.jzyglxt.com/user/main/");
             * */
            string username = CurrentUser.RealUserName;
            string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/WebList/EasyUiIndex?FormDm=QYGL_RYTSTJ&FormStatus=0");
            string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string sign=MD5Util.StringToMD5Hash(timestring, true)
            string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
            sign = MD5Util.StringToMD5Hash(sign, true);
            Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);

        }

        //务工人员系统
        public void gotoWGRYSystem()
        {
            string gototype = Request["type"].GetSafeString();

            IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
            retdt = CommonService.GetDataTable("select wgryurl from SYS_WGRYURL where WGRYType='" + gototype + "'");
            if (retdt.Count > 0)
            {
                string username = CurrentUser.RealUserName;
                string url = HttpUtility.UrlEncode("http://120.27.218.55:8001/" + retdt[0]["wgryurl"].GetSafeString());
                string timestring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //string sign=MD5Util.StringToMD5Hash(timestring, true)
                string sign = String.Format("timestring={0}&secret={1}", timestring, "yc_login");
                sign = MD5Util.StringToMD5Hash(sign, true);
                Response.Redirect("http://120.27.218.55:8001/user/YC_Login?timestring=" + timestring + "&sign=" + sign + "&username=" + username + "&url=" + url);
            }
            else
            {
                Response.Write("系统错误！");
                Response.End();
            }

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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";


                sql = "select count(1) as num from View_I_M_GC1  where zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zjgcs = "0";
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zjgcs = dt_zjgcs[0]["num"];

                sql = "select sum(convert(numeric(18, 2),JZMJ)) as num from View_I_M_GC1  where ISNUMERIC(JZMJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zmj = "0";
                double zmjd = 0;
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd = dt_zjgcs[0]["num"].GetSafeDouble(0);

                sql = "select sum(convert(numeric(18, 2),SZDLMJ)) as num from View_I_M_GC1  where ISNUMERIC(SZDLMJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd += dt_zjgcs[0]["num"].GetSafeDouble(0);

                sql = "select sum(convert(numeric(18, 2),SZQL)) as num from View_I_M_GC1  where ISNUMERIC(SZQL)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd += dt_zjgcs[0]["num"].GetSafeDouble(0);
                zmj = zmjd.ToString();

                string zzj = "0";
                sql = "select sum(convert(numeric(18, 2),GCZJ)) as num from View_I_M_GC1  where ISNUMERIC(GCZJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zzj = dt_zjgcs[0]["num"];

                string jggc = "0";
                sql = "select sum(convert(numeric(18, 2),GCZJ)) as num from View_I_M_GC1  where zt in( select bh from h_gczt where xssx>=7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    jggc = dt_zjgcs[0]["num"];

                string zcry = "0";
                sql = "select count(1) as num from i_M_RY where SPTG=1 and SBSP=1";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zcry = dt_zjgcs[0]["num"];

                string zgry = "0";
                sql = "select count(1) as num  from i_M_RY where rybh in (select rybh from dbo.View_GC_RY_QYRYCK where zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + ")";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zgry = dt_zjgcs[0]["num"];

                string basb = "0";
                sql = "select count(1) as num from dbo.SB_BA where BeiAnStatus='1' ";
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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";

                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> dtlx = new List<IDictionary<string, string>>();
                dtlx = CommonService.GetDataTable("select lxmc,lxbh from H_GCLX");
                for (int i = 0; i < dtlx.Count; i++)
                {
                    string lxbh = dtlx[i]["lxbh"];
                    string retsum = "0";
                    sql = "select count(1) as num from View_I_M_GC1 where  GCLXBH='" + lxbh + "' and  zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];

                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("name", dtlx[i]["lxmc"]);
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

            string province = Request["province"].GetSafeString();
            string city = Request["city"].GetSafeString();
            string district = Request["district"].GetSafeString();
            string jd = Request["jd"].GetSafeString();
            string qylx = Request["qylx"].GetSafeString();
            string qybh = Request["qybh"].GetSafeString();
            string gczt = Request["gczt"].GetSafeString();
            string gcxz = Request["gcxz"].GetSafeString();
            string key = Request["key"].GetSafeString();


            //IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string dt = "";
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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";

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
                    sql = "select count(1) as num from View_I_M_GC1 where  slrq>='" + i.ToString() + "-1-1' and slrq<'" + (i + 1).ToString() + "-1-1'   " + where + "";
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

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

                string zcry = "0";
                sql = "select count(1) as num from i_M_RY where SPTG=1 and SBSP=1";
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

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }

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

                string url = "http://120.27.218.55:8001/wzwgry/GetGcRYTSTJ_YC";
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

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else
                {
                    where += " and zjzbh in(select zjzbh from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%')";
                }
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
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";
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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";

                sql = "select gcmc,gcbh,gczb,gclxbh,gcdd from View_I_M_GC1 where gczb is not null and gczb !='' and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where;
                dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    if (ret != "")
                        ret += ",";
                    ret += " { \"name\": \"" + dt[i]["gcmc"].Replace("\"", "‘") + "\", \"position\": [" + dt[i]["gczb"] + "], \"status\": 0,\"gcbh\": \"" + dt[i]["gcbh"] + "\",\"gclxbh\": \"" + dt[i]["gclxbh"] + "\",\"gcdd\": \"" + dt[i]["gcdd"] + "\" }";
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
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";
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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";

                sql = "select gcmc,gcbh,gczb,total,sensorname,sensorcode from View_Tj_Dust_Map where gcbh in(select gcbh from View_I_M_GC1 where gczb is not null and gczb !=''  and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + " )";
                dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    if (ret != "")
                        ret += ",";
                    ret += " { \"name\": \"" + dt[i]["gcmc"].Replace("\"", "‘") + "\", \"position\": [" + dt[i]["gczb"] + "], \"status\": " + dt[i]["total"] + ",\"gcbh\": \"" + dt[i]["gcbh"] + "\",\"sensorname\": \"" + dt[i]["sensorname"] + "\",\"sensorcode\": \"" + dt[i]["sensorcode"] + "\" }";
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








        #region 更新起重机械检测报告状态
        /// <summary>
        /// 更新起重机械检测报告状态
        /// </summary>
        public void UpdateQZJBGZT()
        {
            string recid = Request["recid"].GetSafeString();
            string zt = Request["zt"].GetSafeString();
            string err = "";
            bool ret = false;
            try
            {
                string sql = "";
                if (zt == "S")
                {
                    sql = string.Format("update reportqzj set shrzh='{0}',shrxm='{1}', shrq='{2}' where recid={3}", CurrentUser.UserName, CurrentUser.RealName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), recid);
                }
                else if (zt == "P")
                {
                    sql = string.Format("update reportqzj set pzrzh='{0}',pzrxm='{1}', pzrq='{2}' where recid={3}", CurrentUser.UserName, CurrentUser.RealName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), recid);
                }
                if (sql != "")
                {
                    ret = CommonService.ExecSql(sql, out err);
                }
                else
                {
                    err = "参数错误！";
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



        #endregion

        #region 务工人员接口


        public void GetTjlist()
        {
            int recid = Request["id"].GetSafeInt();
            string err = "";
            string ret = "";
            try
            {

                string url = "http://120.27.218.55:8001/wzwgry/GetTjlist_YC";
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

                string url = "http://120.27.218.55:8001/wzwgry/GetGC_QYFBTJ_YC";
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

                string url = "http://120.27.218.55:8001/wzwgry/GetStatisticsGz_YC";
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

                string url = "http://120.27.218.55:8001/wzwgry/GetGcList_YC";
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

        /// <summary>
        /// 获取考勤记录
        /// </summary>
        [Authorize]
        public void GetKQList()
        {
            string gcbh = Request["gcbh"].GetSafeString();

            string sfzhm = Request["sfzhm"].GetSafeString();
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(20);
            string dt1 = Request["dt1"].GetSafeString();
            string dt2 = Request["dt2"].GetSafeString();
            string err = "";
            string ret = "";
            try
            {

                string url = "http://120.27.218.55:8001/wzwgryfun/GetWdyKqlist";


                url += "?sfzhm=" + HttpUtility.UrlEncode(CryptFun.Encode(sfzhm)) + "&gcbh=" + HttpUtility.UrlEncode(CryptFun.Encode(gcbh)) + "&WGPTBH=" + HttpUtility.UrlEncode(CryptFun.Encode("TZBG")) + "&page=" + pageindex.GetSafeString() + "&rows=" + pagesize.GetSafeString() + "&dt1=" + HttpUtility.UrlEncode(dt1) + "&dt2=" + HttpUtility.UrlEncode(dt2);
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

        public string SendDataByPost_ForHK(string Url, string datas)
        {
            string retString = "";
            try
            {
                if (Url.StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
                }
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/json";



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
                retString = e.Message;

            }
            return retString;
        }


        public string SendDataByPost(string Url, string datas)
        {
            string retString = "";
            try
            {
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

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //为了通过证书验证，总是返回true
            return true;
        }


        #endregion

        #region 手机特殊接口
        /// <summary>
        /// 手机登录操作
        /// </summary>
        public void DoLoginForPhone()
        {
            string err = "";
            bool ret = false;
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
                    RealName = ret ? CurrentUser.RealName : "",
                    Remark = "",
                    Result = ret
                };

                // 获取页面跳转类型

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";
                if (err == "" && ret)
                {
                    err = JsonFormat.GetRetString(ret, CurrentUser.CurUser.UrlJumpType);
                    if (CurrentUser.CurUser.CompanyId == "CP201611000001")
                        err = JsonFormat.GetRetString(ret, "Y");
                }
                else
                    err = JsonFormat.GetRetString(false, err);

                Response.Write(err);
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
                    if (CurrentUser.CurUser.UrlJumpType == "Q")
                    {
                        dt = CommonService.GetDataTable("select * from SB_ReportSBSY where InstallProgStatus=2 and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))");
                        if (dt.Count > 0)
                            msg += ",azjs";
                        dt = CommonService.GetDataTable("select * from SB_ReportSBSY where UserProgStatus=2 and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))");
                        if (dt.Count > 0)
                            msg += ",syjs";
                        dt = CommonService.GetDataTable("select * from SB_ReportSBSY where UnInstallProgStatus=2 and jdzch in(select gcbh from dbo.View_I_M_GC1 where (jldwmc is null or jldwmc='') and gcbh in(select gcbh from I_S_GC_JSDW where QYMC='" + CurrentUser.RealName + "'))");
                        if (dt.Count > 0)
                            msg += ",cxjs";
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

                        sql = " from View_I_M_GC1 a where a.zt not in ('YT','LR') and (zjzbh in(select ZJZBH from H_ZJZ where MZJZBH like '%" + CurrentUser.CompanyCode + "%') or 'CP201707000004'='" + CurrentUser.CompanyCode + "') " + strwhere + " order by gcbh desc";
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
                    string sql = " select * from i_M_qy where 1=1 " + strwhere + "  order by qymc asc";

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


                    string sql = " select qybh,qymc,qyfzr,lxdh,qyfr,zh,lxsj,zzzt,qylxmcs,sszjz,sprxm from View_I_M_QY where qybh='" + qybh + "'";

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
                    string sql = " select rybh,ryxm,sjhm,sfzhm from i_m_ry where 1=1 " + strwhere + "  order by spsj desc";

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


                    string sql = " select qymc,ryxm,xb,rybh,sjhm,sfzhm,nfjg,sy_zt,sprxm,spsj,sszjz,qylxmcs,sy_zs from View_I_M_RY2 where rybh='" + rybh + "'";

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
                        string sql = "select useenddate from dbo.SB_BZJ where Qrcode='" + key + "'";
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
                        sql = " select gcmc,cqbh from SB_ReportSBSY where State!=2 and RECID in (select SBSYID from SB_BZJADD where BZJID=(select RECID from dbo.SB_BZJ where Qrcode='" + key + "')) ";

                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {

                            code = false;
                            msg = "该标准节正在[" + datas[0]["gcmc"].GetSafeString() + "]工程的[" + datas[0]["cqbh"].GetSafeString() + "]设备中使用，无法重复使用，请确认！";
                            type = "1";
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




        public ActionResult FlowReportDown()
        {

            string url = "";
            string reportFile = Request["filename"].GetSafeString(); //"起重机械产权备案表";
            string where = Request["where"].GetSafeString();
            string table = Request["table"].GetSafeString();
            string type = Request["type"].GetSafeString("down");


            var g = new ReportPrint.GenerateGuid();
            var c = g.Get();

            string signedfilename = (reportFile + where + table).Replace('|', '-');
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
            //url = "/reportPrint/Index?type=word&filename=1&table=stformitem&field=&where=1%3d1&fileindex=0";
            return new RedirectResult(url);
        }





        #endregion




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


                string sql = "select distinct szcs from h_city where szsf='" + province + "' and  szcs='台州市'";
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


                string sql = "select distinct szxq from h_city where szsf='" + province + "'and szcs='" + city + "'";
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



        #endregion


        #endregion


        [Authorize]
        public void UpdateKF()
        {
            bool code = true;
            string msg = "";
            try
            {
                //Js没有写完，考虑怎么写
                string type = Request["type"].GetSafeString();
                int recid = Request["id"].GetSafeInt();
                string kssj = Request["kssj"].GetSafeDate(DateTime.Now).ToString("yyyy-MM-dd");
                string bz = Request["bz"].GetSafeString();
                IList<string> sqls = new List<string>();

                if (type == "1")
                {
                    string sql = "update I_S_RY_JF_List set KSZT=1, KSTotal=Total,LRRSJ=getdate(),LRRZH='" + CurrentUser.UserName + "' where RECID=" + recid + " and Total>=6 and Total<10";
                    sqls.Add(sql);
                    sql = "update I_S_RY_JF_List set KSZT=2, KSTotal=Total,LRRZH='" + CurrentUser.UserName + "' where RECID=" + recid + " and Total>=10";
                    sqls.Add(sql);
                }
                else
                {
                    string sql = "update I_S_RY_JF_List set KSZT=-1, KSTotal=Total,LRRSJ=getdate(),LRRZH='" + CurrentUser.UserName + "' where RECID=" + recid;
                    sqls.Add(sql);
                }

                string tsql = "INSERT INTO [I_S_RY_JF_KS]([JFID],[KSZT],[KSRQ],[BZ],[KSTotal],[LRRZH],[LRRXM]) select " + recid.ToString() + "," + type.GetSafeInt(0).ToString() + ",'" + kssj + "','" + bz + "',KSTotal,'" + CurrentUser.UserName + "','" + CurrentUser.RealName + "' from I_S_RY_JF_List where RECID=" + recid;
                sqls.Add(tsql);

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



        #region 阿里大屏数据提供


        /// <summary>
        /// 地图工程
        /// </summary>
        public void ALGetProjectMap()
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
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";
                string gcmc = Request["gcmc"].GetSafeString();

                string lx = Request["lx"].GetSafeString();

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

                if (lx == "gc")
                {

                    sql = "select gcmc,gcbh,gczb,gclxbh,gcdd from View_I_M_GC1 where gczb is not null and gczb !='' and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where;
                    dt = CommonService.GetDataTable(sql);
                }
                if (lx == "sb")
                {

                    sql = "select gcmc,gcbh,gczb,gclxbh,gcdd from View_I_M_GC1 where gczb is not null and gczb !='' and gcbh in(select jdzch from dbo.SB_ReportSBSY where state!=2)  and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                    dt = CommonService.GetDataTable(sql);
                }

                if (lx == "sp")
                {

                    sql = "select gcmc,gcbh,gczb,gclxbh,gcdd from View_I_M_GC1 where gczb is not null and gczb !='' and gcbh in(select gcbh from dbo.I_S_GC_Video )  and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                    dt = CommonService.GetDataTable(sql);
                }


                for (int i = 0; i < dt.Count; i++)
                {
                    if (ret != "")
                        ret += ",";
                    string[] tt = dt[i]["gczb"].Split(',');
                    ret += "{\"doit\":" + i.ToString() + ",\"lat\":" + tt[1].ToString() + ",\"lng\":" + tt[0].ToString() + ",\"value\":\"" + dt[i]["gcbh"] + "\",\"info\":\"" + dt[i]["gcmc"].Replace("\"", "‘") + "\",\"type\":\"zj\",\"name\":\"" + dt[i]["gcmc"].Replace("\"", "‘") + "\"}";
                    //ret += " { \"name\": \"" + dt[i]["gcmc"].Replace("\"", "‘") + "\", \"position\": [" + dt[i]["gczb"] + "], \"status\": 0,\"gcbh\": \"" + dt[i]["gcbh"] + "\",\"gclxbh\": \"" + dt[i]["gclxbh"] + "\",\"gcdd\": \"" + dt[i]["gcdd"] + "\" }";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, ret));
                Response.End();
            }
        }
        public void ALQYTJ()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select zjzmc as name,count(zjzmc) as value from dbo.View_I_M_GC1 group by zjzmc having zjzmc is not null";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }



        public void ALGetJQGC()
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
            string ret = "";

            //IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string dt = "";
            try
            {

                string sql = "";
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";



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

                IList<IDictionary<string, string>> retdt = new List<IDictionary<string, string>>();
                int year = DateTime.Now.Year;
                for (int i = year - 5; i <= year; i++)
                {
                    if (ret != "")
                    {
                        ret += ",";
                    }

                    string retsum = "0";
                    sql = "select count(1) as num from View_I_M_GC1 where  slrq>='" + i.ToString() + "-1-1' and slrq<'" + (i + 1).ToString() + "-1-1'   " + where + "";
                    retdt = CommonService.GetDataTable(sql);
                    if (null != retdt && retdt.Count != 0)
                        retsum = retdt[0]["num"];
                    ret += "{\"year\":" + i.ToString() + ",\"count\":" + retsum + "}";


                }
                //dt = " {\"year\": [" + yeartext + "], \"line\": [{\"data\": [" + linetext + "],\"name\": \"报监工程\" }]}";

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":[{2}]}}", code ? "0" : "1", msg, ret));
                Response.End();
            }
        }


        public void ALGetStatistics()
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
            IDictionary<string, string> di = new Dictionary<string, string>();
            try
            {

                string sql = "";
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else if (CurrentUser.CompanyCode != "")
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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";


                sql = "select count(1) as num from View_I_M_GC1  where zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zjgcs = "0";
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zjgcs = dt_zjgcs[0]["num"];

                sql = "select sum(convert(numeric(18, 2),JZMJ)) as num from View_I_M_GC1  where ISNUMERIC(JZMJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zmj = "0";
                double zmjd = 0;
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd = dt_zjgcs[0]["num"].GetSafeDouble(0);

                sql = "select sum(convert(numeric(18, 2),SZDLMJ)) as num from View_I_M_GC1  where ISNUMERIC(SZDLMJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd += dt_zjgcs[0]["num"].GetSafeDouble(0);

                sql = "select sum(convert(numeric(18, 2),SZQL)) as num from View_I_M_GC1  where ISNUMERIC(SZQL)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zmjd += dt_zjgcs[0]["num"].GetSafeDouble(0);
                zmj = zmjd.ToString();

                string zzj = "0";
                sql = "select sum(convert(numeric(18, 2),GCZJ)) as num from View_I_M_GC1  where ISNUMERIC(GCZJ)>0 and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zzj = dt_zjgcs[0]["num"];

                /*
                string jggc = "0";
                sql = "select count(1) as num from View_I_M_GC1  where zt in( select bh from h_gczt where xssx>=7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    jggc = dt_zjgcs[0]["num"];
                 * */
                string jggc = "0";
                sql = "select count(1) as num from View_I_M_GC1  where  gclxbh='01' and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    jggc = dt_zjgcs[0]["num"];

                string szgc = "0";
                sql = "select count(1) as num from View_I_M_GC1  where  gclxbh='02' and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    szgc = dt_zjgcs[0]["num"];

                string localqy = "0";
                sql = "select count(1) as num from I_M_QY where ZJZBH!='0000'";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    localqy = dt_zjgcs[0]["num"];

                string localqygc = "0";
                sql = "select count(1)  as num  from dbo.View_I_M_GC1 where gcbh in(select gcbh from dbo.View_GC_QY where qybh in(select qybh from I_M_QY where ZJZBH!='0000')) and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    localqygc = dt_zjgcs[0]["num"];

                string nonlocalqy = "0";
                sql = "select count(1) as num from I_M_QY where ZJZBH='0000'";
                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    nonlocalqy = dt_zjgcs[0]["num"];

                string nonlocalqygc = "0";
                sql = "select count(1)  as num  from dbo.View_I_M_GC1 where gcbh in(select gcbh from dbo.View_GC_QY where qybh in(select qybh from I_M_QY where ZJZBH='0000')) and zt in( select bh from h_gczt where xssx >=2 and xssx<7 )  " + where + "";

                dt_zjgcs = CommonService.GetDataTable(sql);
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    nonlocalqygc = dt_zjgcs[0]["num"];




                di.Add("allcount", zjgcs);
                di.Add("allmj", zmj);
                di.Add("allzj", zzj);
                di.Add("jdcount", jggc);
                di.Add("zjcount", szgc);
                di.Add("localqy", localqy);
                di.Add("localqygc", localqygc);
                di.Add("nonlocalqy", nonlocalqy);

                di.Add("nonlocalqygc", nonlocalqygc);
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(di)));
                Response.End();
            }
        }


        public void ALGetGCJDs()
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
            IDictionary<string, string> di = new Dictionary<string, string>();
            try
            {

                string sql = "";
                string where = " and (SSJCJGBH='' or SSJCJGBH is null)";

                if (CurrentUser.CompanyCode == "CP201707000004")
                {
                    where = " and 1=1";
                }
                else if (CurrentUser.CompanyCode != "")
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
                    where += " and (gcmc like '%" + key + "%' or gcbh in(select gcbh from dbo.View_GC_QY where qymc like '%" + key + "%' ))";



                sql = "select mc,(select count(gcbh) from i_M_GC where zt=a.bh " + where + ") as num from h_gczt a where xssx >=2 order by xssx asc";
                //IList<IDictionary<string, string>> dt_gczt = new List<IDictionary<string, string>>();
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }



        #endregion


        #region 视频接口转接



        public void GetSPLIST()
        {
            int pageSize = Request["pageSize"].GetSafeInt(10);
            int pageCode = Request["pageCode"].GetSafeInt(1);
            string name = Request["name"].GetSafeString("");
            string err = "";
            string ret = "";
            string type = Request["type"].GetSafeString("yd");
            try
            {

                if (type != "dx")
                {

                    string url = "https://111.1.24.130/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/artemis/v1/agreementService/securityParam/appKey/28529791\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{},\"mock\":false,\"appKey\":\"28529791\",\"appSecret\":\"Dzr0w3g1zo41yje7UVmm\",\"appKey\":\"28529791\"}"); //SendDataByGET(url);

                }
                else
                {

                    string url = "https://160.191.150.2/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/artemis/v1/agreementService/securityParam/appKey/28594875\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{},\"mock\":false,\"appKey\":\"28594875\",\"appSecret\":\"EIWYYISHxvHN0NjA5oyo\",\"appKey\":\"28594875\"}"); //SendDataByGET(url);

                }
                /*
                string url = "http://111.3.65.57:8090/zhty/service/v1/import/list?type=8&pageSize=" + pageSize + "&pageCode=" + pageCode + "&name=" + name;
                ret = SendDataByGET(url);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";
                */
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

        public void GetHKQYLIST()
        {

            string unitCode = Request["unitCode"].GetSafeString("3310");
            string err = "";
            string ret = "";
            string type = Request["type"].GetSafeString("yd");
            try
            {

                if (type != "dx")
                {
                    string url = "https://111.1.24.130/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/common/v1/remoteControlUnitRestService/findControlUnitByUnitCode\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{\"unitCode\":\"" + unitCode + "\"},\"parameter\":{},\"mock\":false,\"appKey\":\"28529791\",\"appSecret\":\"Dzr0w3g1zo41yje7UVmm\"}");//SendDataByGET(url);
                }
                else
                {
                    string url = "https://60.191.150.2/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/common/v1/remoteControlUnitRestService/findControlUnitByUnitCode\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{\"unitCode\":\"" + unitCode + "\"},\"parameter\":{},\"mock\":false,\"appKey\":\"28594875\",\"appSecret\":\"EIWYYISHxvHN0NjA5oyo\"}");//SendDataByGET(url);
                }
                /*
                string url = "http://111.3.65.57:8090/zhty/service/v1/import/list?type=8&pageSize=" + pageSize + "&pageCode=" + pageCode + "&name=" + name;
                ret = SendDataByGET(url);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";
                */
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


        public void GetHKSPLIST()
        {

            string unitCode = Request["unitCode"].GetSafeString("3310");
            string err = "";
            string ret = "";
            string type = Request["type"].GetSafeString("yd");
            try
            {


                int pageSize = Request["pageSize"].GetSafeInt(20);
                int pageCode = Request["pageCode"].GetSafeInt(1);
                if (type != "dx")
                {
                    string url = "https://111.1.24.130/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/common/v1/remoteControlUnitRestService/findCameraInfoPageByTreeNode\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{\"treeNode\":\"" + unitCode + "\",\"start\":\"" + pageCode + "\",\"size\":\"" + pageSize + "\",\"order\":\"desc\",\"orderby\":\"createTime\"},\"parameter\":{},\"mock\":false,\"appKey\":\"28529791\",\"appSecret\":\"Dzr0w3g1zo41yje7UVmm\"}");//SendDataByGET(url);

                }
                else
                {
                    string url = "https://60.191.150.2/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/common/v1/remoteControlUnitRestService/findCameraInfoPageByTreeNode\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{\"treeNode\":\"" + unitCode + "\",\"start\":\"" + pageCode + "\",\"size\":\"" + pageSize + "\",\"order\":\"desc\",\"orderby\":\"createTime\"},\"parameter\":{},\"mock\":false,\"appKey\":\"28594875\",\"appSecret\":\"EIWYYISHxvHN0NjA5oyo\"}");//SendDataByGET(url);

                }
                /*
                string url = "http://111.3.65.57:8090/zhty/serv ice/v1/import/list?type=8&pageSize=" + pageSize + "&pageCode=" + pageCode + "&name=" + name;
                ret = SendDataByGET(url);
                //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";
                */
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



        public void GetSPCode()
        {
            string id = Request["id"].GetSafeString("");
            string err = "";
            string ret = "";
            try
            {


                string url = "http://111.3.65.57:8090/zhty/service/v1/import/getCamerasById?id=" + id;
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


        //电信的  60.191.150.2   28594875 EIWYYISHxvHN0NjA5oyo 

        /* 联通服务
        https://221.12.111.252:443/cms/services/IAuthService?wsdl 
        那个编码是000201
        zjjdj   Hik12345@systerm 
https://221.12.111.252:443/cms/services/ICommonService?wsdl */

        /*
    public string LT_applyToken()
    {
        string err = "";
        string ret = "";
        try
        {
            LTIAuthService.IAuthService lt = new LTIAuthService.IAuthService();
            ret = lt.login("zjjdj", "962f80071a0a6e582b332e1409c984e3d5aaab93fde3a7800a39462c6aeff075", "221.12.111.252", "127.0.0.1", "1");

            //ret=lt.applyToken(ret);
        }
        catch (Exception e)
        {
            SysLog4.WriteLog(e);
            ret = e.Message;
        }
        finally
        {

        }
        return ret;
    }
    */

        public string sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }


        /*
        public void LT_getAllResourceDetail()
        {

            string err = "";
            string ret = "";
           
            try
            {
                LTICommonService.ICommonService lt = new LTICommonService.ICommonService();
                //ret=lt.getAllResourceDetail("000201",)//这里参数什么鬼啊，不知道

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
        */
        /*
        public void LT_getAllResourceDetailByOrg()
        {

            string err = "";
            string ret = "";
           
            try
            {
                LTICommonService.ICommonService lt = new LTICommonService.ICommonService();
                //ret=lt.getAllResourceDetailByOrg("000201",)//这里参数什么鬼啊，不知道

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
        */


        public void getAppSercet()
        {

            string err = "";
            string ret = "";
            string type = Request["type"].GetSafeString("yd");
            try
            {

                if (type != "dx")//移动的
                {
                    string url = "https://111.1.24.130/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/artemis/v1/agreementService/securityParam/appKey/28529791\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{},\"mock\":false,\"appKey\":\"28529791\",\"appSecret\":\"Dzr0w3g1zo41yje7UVmm\",\"appKey\":\"28529791\"}"); //SendDataByGET(url);
                    //ret = "{\"Status\":\"success\",\"Msg\":\"\",\"Datas\":[{\"value\":\"5\",\"name\":\"\"},{\"value\":\"6\",\"name\":\"苍南县\"},{\"value\":\"5\",\"name\":\"黄岩区\"},{\"value\":\"1\",\"name\":\"椒江区\"},{\"value\":\"2\",\"name\":\"龙湾区\"},{\"value\":\"31\",\"name\":\"鹿城区\"},{\"value\":\"7\",\"name\":\"瓯海区\"},{\"value\":\"2\",\"name\":\"平阳县\"},{\"value\":\"2\",\"name\":\"瑞安市\"},{\"value\":\"1\",\"name\":\"泰顺县\"},{\"value\":\"1\",\"name\":\"越城区\"},{\"value\":\"1\",\"name\":\"诸暨市\"}]}";
                }
                else//电信的
                {
                    string url = "https://60.191.150.2/artemis-web/debug";// "http://111.3.65.57:8090/zhty/service/v1/import/getAppSercet";
                    ret = SendDataByPost_ForHK(url, "{\"httpMethod\":\"GET\",\"path\":\"/api/artemis/v1/agreementService/securityParam/appKey/28594875\",\"contentType\":\"application/x-www-form-urlencoded;charset=UTF-8\",\"headers\":{},\"query\":{},\"mock\":false,\"appKey\":\"28594875\",\"appSecret\":\"EIWYYISHxvHN0NjA5oyo\",\"appKey\":\"28594875\"}"); //SendDataByGET(url);

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

                Response.Write(ret);
                Response.End();
            }
        }

        public void saveSPcode()
        {
            string err = "";
            bool ret = false;

            string gcbh = Request["gcbh"].GetSafeString();
            string code = Request["code"].GetSafeString();
            string bz = Request["bz"].GetSafeString();
            string longitude = Request["longitude"].GetSafeDouble(0).ToString();
            string latitude = Request["latitude"].GetSafeDouble(0).ToString();
            string type = Request["type"].GetSafeString("yd");
            string bz2 = Request["bz2"].GetSafeString();
            try
            {
                IList<string> sqls = new List<string>();
                if (type != "lt")
                {
                    string zb = longitude + "," + latitude;
                    string sql = "INSERT INTO [I_S_GC_Video] ([GCBH] ,[code] ,[bz] ,[longitude] ,[latitude] ,[CreatedOn] ,[CreatedBy] ,[CreatedByName],VideoType,ExtrID) VALUES ('" + gcbh + "' ,'" + code + "' ,'" + bz + "' ," + longitude + " ," + latitude + " ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ,'" + CurrentUser.UserName + "' ,'" + CurrentUser.RealName + "','" + type + "','" + bz2 + "') ";
                    if (sql != "")
                    {
                        //ret = CommonService.ExecSql(sql, out err);
                        sqls.Add(sql);
                        sqls.Add("update i_M_GC set gczb='" + zb + "' where gcbh='" + gcbh + "' and (gczb='' or gczb is null)");
                    }
                }
                else
                {
                    //联通这里还没写完
                }


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




        public void GetSPCodeList()
        {

            string gcbh = Request["gcbh"].GetSafeString();
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select * from I_S_GC_Video where gcbh='" + gcbh + "'";

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
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", datas.Count.ToString(), jss.Serialize(datas)));
                Response.End();
            }
        }


        /// <summary>
        /// 通用删除，记录表删除
        /// </summary>
        [Authorize]
        public void Deletevideo()
        {
            bool code = true;
            string msg = "";
            try
            {
                string ID = Request["recid"].GetSafeString();
                IList<string> sqls = new List<string>();
                sqls.Add("delete from I_S_GC_Video where recid=" + ID);
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


        #region 起重机械app接口

        string apistring = "3hj7rml8q1ukez1b";

        /// <summary>
        /// 根据铭牌二维码，写入电子标签，（目前作废不用）
        /// </summary>
        public void AddICCode()
        {
            bool code = true;
            string msg = "";
            try
            {
                string sessionid = Request["sessionid"].GetSafeString();
                string checkcode = Request["checkcode"].GetSafeString();
                string needpower = Request["needpower"].GetSafeString();

                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(apistring + sessionid + needpower), StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "验证失败,非法操作";
                }
                else if (session == null)
                {
                    code = false;
                }
                else
                {
                    string username = Request["login_name"].GetSafeString();
                    string password = Request["login_pwd"].GetSafeString();

                    //if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                    if (code)
                    {
                        List<MenuItem> menus = new List<MenuItem>();
                        menus = CurrentUser.Menus;
                        //二维码登记
                        var canedit = (from a in menus
                                       where a.MenuCode.Equals("SBGL_BZJGL")
                                       select a).ToList();
                        if (canedit.Count > 0 || needpower == "0")
                        {
                            string qrcode = Request["qrcode"].GetSafeString();
                            string key = CryptFun.Decode(qrcode);

                            if (key == "" || key == null)
                            {
                                code = false;
                                msg = "该二维码解码失败，二维码不存在，请确认！";
                            }
                            else
                            {
                                msg = key.Remove(0, 2);
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "当前账户没有绑电子标签权限，请确认！";
                        }
                    }
                    else
                    {
                        msg = "登录失败，请重新登录app";
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



        /// <summary>
        /// 根据电子标签二维码，写电子标签
        /// </summary>
        public void AddICCode2()
        {
            bool code = true;
            string msg = "操作失败，请重新登陆！";
            string pw = "";
            string key = "";
            try
            {
                string sessionid = Request["sessionid"].GetSafeString();
                string checkcode = Request["checkcode"].GetSafeString();
                string needpower = Request["needpower"].GetSafeString();

                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(apistring + sessionid + needpower), StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "验证失败,非法操作";
                }
                else if (session == null)
                {
                    code = false;
                }
                else
                {
                    string username = Request["login_name"].GetSafeString();
                    string password = Request["login_pwd"].GetSafeString();

                    //if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                    if (code)
                    {
                        string qrcode = Request["qrcode"].GetSafeString();


                        Random rd = new Random();





                        IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                        dt = CommonService.GetDataTable("select password,iccard from INFO_ICCard where ICNo='" + qrcode + "'");
                        if (dt.Count > 0)
                        {

                            pw = dt[0]["password"].GetSafeString();

                            if (pw == "")
                            {
                                pw = rd.Next(10000000, 99999999).ToString();
                                string sql = "update [INFO_ICCard] set PassWord='" + pw + "' where ICNo='" + qrcode + "'";
                                IList<string> sqls = new List<string>();
                                sqls.Add(sql);
                                code = CommonService.ExecTrans(sqls);
                                if (!code)
                                    msg += "获取电子标签失败！";
                                else
                                {
                                    key = dt[0]["iccard"].GetSafeString();
                                    msg = key;
                                }
                            }
                            else
                            {
                                key = dt[0]["iccard"].GetSafeString();
                                code = false;
                                msg = "该电子标签已经绑定过！";
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "该电子标签二维码异常，不是系统生成二维码！";
                            /*
                            string sql = "INSERT INTO [INFO_ICCard]([ICCard],[ICNo],[PassWord])VALUES('" + key + "','" + qrcode + "','" + pw.ToString() + "')";
                            IList<string> sqls = new List<string>();
                            sqls.Add(sql);
                            code = CommonService.ExecTrans(sqls);
                            if (!code)
                                msg += "获取电子标签失败！";*/
                        }
                    }
                    else
                    {
                        msg = "登录失败，请重新登录app";
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
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("code", code ? "0" : "1");
                row.Add("key", key);
                row.Add("msg", msg);
                row.Add("password", pw);
                Response.Write(jss.Serialize(row));
            }

        }



        public void CheckICCode()
        {
            bool code = true;
            string msg = "";
            string pw = "";
            try
            {
                string sessionid = Request["sessionid"].GetSafeString();
                string checkcode = Request["checkcode"].GetSafeString();
                string needpower = Request["needpower"].GetSafeString();

                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(apistring + sessionid + needpower), StringComparison.OrdinalIgnoreCase))
                {
                    code = false;
                    msg = "验证失败,非法操作";
                }
                else if (session == null)
                {
                    code = false;
                }
                else
                {
                    string username = Request["login_name"].GetSafeString();
                    string password = Request["login_pwd"].GetSafeString();

                    //if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                    if (code)
                    {

                        string qrcode = Request["qrcode"].GetSafeString();
                        string key = CryptFun.Decode(qrcode);

                        if (key == "" || key == null)
                        {
                            code = false;
                            msg = "该二维码解码失败，二维码不存在，请确认！";
                        }
                        else
                        {

                            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                            string prtype = "";
                            string sql = "select password,iccard from dbo.INFO_QRCODE where Qrcode='" + key + "'";
                            dt = CommonService.GetDataTable(sql);
                            if (dt.Count > 0)
                            {
                                code = true;
                                msg = dt[0]["iccard"].GetSafeString();
                                pw = dt[0]["password"].GetSafeString();

                                if (pw == "")
                                {
                                    code = false;
                                    msg = "该二维码尚未绑定电子标签！";
                                }
                            }
                            else
                            {
                                code = false;
                                msg = "该二维码尚未绑定电子标签！";
                            }
                        }
                    }
                    else
                    {
                        msg = "登录失败，请重新登录app";
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
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("code", code ? "0" : "1");
                row.Add("msg", msg);
                row.Add("password", pw);
                Response.Write(jss.Serialize(row));
                //Response.Write(JsonFormat.GetRetString(code, msg));
            }

        }



        public void getWBRY()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                string sessionid = Request["sessionid"].GetSafeString();
                string checkcode = Request["checkcode"].GetSafeString();
                string notcheck = Request["notcheck"].GetSafeString("");
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(apistring + sessionid), StringComparison.OrdinalIgnoreCase) && notcheck != "1")
                {
                    code = false;
                    msg = "验证失败,非法操作";
                }
                else if (session == null && notcheck != "1")
                {
                    code = false;
                }
                else
                {
                    string username = Request["login_name"].GetSafeString();
                    string password = Request["login_pwd"].GetSafeString();

                    //if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                    if (code)
                    {
                        string qybh = "";
                        string sql = "select qybh from i_m_QY where zh='" + username + "' union all select qybh from I_M_RY where zh='" + username + "'";

                        datas = CommonService.GetDataTable(sql);

                        if (datas.Count > 0)
                        {
                            qybh = datas[0]["qybh"].GetSafeString();

                        }
                        else
                        {
                            code = false;
                            msg = "当前账户没有所属企业，不能录入维保记录";
                        }
                        if (code)
                        {
                            sql = "select distinct ryxm,rybh from View_I_M_RY_WITH_ZZ where qybh='" + qybh + "' and ZZZSLX in('096','091')";
                            datas = CommonService.GetDataTable(sql);

                        }


                    }
                    else
                    {
                        msg = "登录失败，请重新登录app";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(datas)));
                Response.End();
            }

        }




        public void SelectWBSB()
        {
            bool code = true;
            string msg = "";
            IDictionary<string, string> di = new Dictionary<string, string>();
            try
            {
                string sessionid = Request["sessionid"].GetSafeString();
                string checkcode = Request["checkcode"].GetSafeString();
                string notcheck = Request["notcheck"].GetSafeString("");
                SysSession session = ApiSessionService.GetSessionUser(sessionid, out msg);


                if (!checkcode.Equals(MD5Util.GetCommonMD5(apistring + sessionid), StringComparison.OrdinalIgnoreCase) && notcheck != "1")
                {
                    code = false;
                    msg = "验证失败,非法操作";
                }
                else if (session == null && notcheck != "1")
                {
                    code = false;
                }
                else
                {
                    string username = Request["login_name"].GetSafeString();
                    string password = Request["login_pwd"].GetSafeString();

                    //if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);

                    if (code)
                    {
                        string qrcode = Request["qrcode"].GetSafeString();
                        string key = CryptFun.Decode(qrcode);

                        if (key == "" || key == null)
                        {
                            code = false;
                            msg = "该二维码解码失败，二维码不存在，请确认！";
                        }
                        else
                        {
                            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                            string sql = "select top 1 installid,baid from SB_ReportSBSY where BaID in(select BaID from dbo.SB_BA where Qrcode1='" + key + "') or recid in (select sbsyid from SB_BZJADD where qrcode='" + key + "')";

                            datas = CommonService.GetDataTable(sql);

                            if (datas.Count > 0)
                            {
                                di.Add("installid", datas[0]["installid"].GetSafeString());
                                di.Add("baid", datas[0]["baid"].GetSafeString());
                            }
                            else
                            {
                                sql = "select baid from dbo.SB_BA where Qrcode1='" + key + "'";

                                datas = CommonService.GetDataTable(sql);
                                if (datas.Count > 0)
                                {
                                    di.Add("installid", "");
                                    di.Add("baid", datas[0]["baid"].GetSafeString());
                                }
                                else
                                {
                                    di.Add("installid", "");
                                    di.Add("baid", "");
                                    code = false;
                                    msg = "该设备不存在，请确认";
                                }
                            }
                        }
                    }
                    else
                    {
                        msg = "登录失败，请重新登录app";
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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(di)));
                Response.End();
            }

        }


        public void PhoneSubmitWBJL()
        {
            bool code = true;
            string msg = "";
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                //if (!CurrentUser.IsLogin)
                code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string wbqybh = "";
                    string wbqy = "";
                    IList<string> sqls = new List<string>();
                    string sql = "select qybh,qymc from i_m_QY where zh='" + username + "' or qybh in(select qybh from I_M_RY where zh='" + username + "')";
                    IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                    datas = CommonService.GetDataTable(sql);
                    if (datas.Count > 0)
                    {
                        wbqybh = datas[0]["qybh"].GetSafeString();
                        wbqy = datas[0]["qymc"].GetSafeString();
                    }
                    else
                    {
                        code = false;
                        msg = "当前账户没有所属企业，不能录入维保记录";
                    }
                    if (code)
                    {

                        string id = Guid.NewGuid().ToString();
                        string parentinstallid = Request["parentinstallid"].GetSafeString();
                        string baid = Request["baid"].GetSafeString();
                        string wbnr = Request["wbnr"].GetSafeString();
                        string filetext = "";

                        string wbsj = DateTime.Now.ToString("yyyy-MM-dd");

                        string wbry = Request["wbry"].GetSafeString();




                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase postfile = Request.Files[i];
                            string filename = "";
                            if (filename == "")
                                filename = postfile.FileName;
                            string tmpExt = "";
                            string fileid = Guid.NewGuid().ToString("N");
                            if (filename.IndexOf(".") > 0)
                            {
                                tmpExt = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.'));
                            }
                            byte[] postcontent = new byte[postfile.ContentLength];
                            int readlength = 0;
                            while (readlength < postfile.ContentLength)
                            {
                                int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                                readlength += tmplen;
                            }
                            string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                            IList<IDataParameter> sqlparams = new List<IDataParameter>();
                            IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@FILENAME", filename);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@FILECONTENT", postcontent);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@FILEEXT", tmpExt);
                            sqlparams.Add(sqlparam);
                            sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sqlparams.Add(sqlparam);

                            if (CommonService.ExecTrans(sqlstr, sqlparams, out msg))
                            {
                                filetext = filetext + fileid + "," + filename + "|";
                            }




                        }


                        sql = "INSERT INTO [SB_WBJL] ([ID] ,[ParentInstallID] ,[BAID] ,[WBQYBH] ,[WBQY] ,[WBSJ] ,[WBNR] ,[BeiZhu] ,[Filetext]) VALUES ('" + id + "' ,'" + parentinstallid + "' ,'" + baid + "' ,'" + wbqybh + "' ,'" + wbqy + "' ,'" + wbsj + "' ,'" + wbnr + "' ,'' ,'" + filetext + "') ";
                        sqls.Add(sql);
                        sql = "INSERT INTO [SB_WBJL_RY]([ID],[ParentID],[RYBH],[RYXM] ,[DH],[SFZHM]) SELECT  LOWER(REPLACE(LTRIM(NEWID()),'-','')),'" + id + "',rybh,ryxm,sjhm,sfzhm from i_m_ry where rybh in (" + wbry.FormatSQLInStr() + ")";
                        sqls.Add(sql);
                        sql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate]) select RECID,'设备维保',getdate() from SB_ReportSBSY where InstallID='" + parentinstallid + "'";
                        sqls.Add(sql);
                        code = CommonService.ExecTrans(sqls);
                    }
                }
                else
                {
                    msg = "系统登陆失败！请重新登陆app";
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
                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();

            }
        }




        public void PhoneAddBZJ()
        {
            bool code = true;
            string msg = "";
            try
            {
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();
                string value = Request["value"].GetSafeString();
                string icqrcode = Request["icqrcode"].GetSafeString();
                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
                string filetext = "";
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    if (value != "")
                        value = CryptFun.Decode(value);
                    if (value.GetSafeString("") == "")
                    {
                        code = false;
                        msg = "该二维码不存在，请确认！";
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
                        string serialno = "";
                        sql = "select iccard,password,serialno from INFO_ICCard where Icno='" + icqrcode + "'";
                        datas = CommonService.GetDataTable(sql);
                        if (datas.Count > 0)
                        {
                            iccard = datas[0]["iccard"].GetSafeString();
                            icpassword = datas[0]["password"].GetSafeString();
                            serialno = datas[0]["serialno"].GetSafeString();
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
                                msg = "该二维码" + value + "正在设备[" + datas[0]["beianicp"].GetSafeString() + "][" + datas[0]["machineryname"].GetSafeString() + "]的构件[" + datas[0]["lx"].GetSafeString() + "]中使用，无法重复使用，请确认！";

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
                            if (Request["UseEndDate"].GetSafeDate(DateTime.Now.AddDays(-1)) <= DateTime.Now)
                            {
                                code = false;
                                msg = "报废日期错误，报废日期不能早于当前日期！";
                            }
                            if (Request["BuyDate"].GetSafeDate(DateTime.Now.AddDays(1)) >= DateTime.Now)
                            {
                                code = false;
                                msg = "购买日期错误，报废日期不能晚于当前日期！";
                            }
                        }

                        if (code)
                        {
                            for (int i = 0; i < Request.Files.Count; i++)
                            {
                                HttpPostedFileBase postfile = Request.Files[i];
                                string filename = "";
                                if (filename == "")
                                    filename = postfile.FileName;
                                string tmpExt = "";
                                string fileid = Guid.NewGuid().ToString("N");
                                if (filename.IndexOf(".") > 0)
                                {
                                    tmpExt = filename.Substring(filename.LastIndexOf('.'), filename.Length - filename.LastIndexOf('.'));
                                }
                                byte[] postcontent = new byte[postfile.ContentLength];
                                int readlength = 0;
                                while (readlength < postfile.ContentLength)
                                {
                                    int tmplen = postfile.InputStream.Read(postcontent, readlength, postfile.ContentLength - readlength);
                                    readlength += tmplen;
                                }
                                string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@FILENAME", filename);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@FILECONTENT", postcontent);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@FILEEXT", tmpExt);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                sqlparams.Add(sqlparam);

                                if (CommonService.ExecTrans(sqlstr, sqlparams, out msg))
                                {
                                    filetext = filetext + fileid + "," + filename + "|";
                                }

                            }

                            IList<string> sqls = new List<string>();
                            sql = "INSERT INTO [SB_BZJ]([PropertyCompanyID],[PropertyCompanyName],[Status] ,[FactoryName],[FactoryNO],[CreaterName],[CreaterID],[ZL],[UseEndDate],[Qrcode],[BZJType],[BuyDate],[SerialNo])VALUES ('" + Request["PropertyCompanyID"].GetSafeString() + "','" + Request["PropertyCompanyName"].GetSafeString() + "',1 ,'" + Request["FactoryName"].GetSafeString() + "','" + Request["FactoryNO"].GetSafeString() + "','" + CurrentUser.RealName + "','" + CurrentUser.UserName + "','" + filetext + "','" + Request["UseEndDate"].GetSafeString() + "','" + value + "','" + Request["BZJType"].GetSafeString() + "','" + Request["BuyDate"].GetSafeString() + "','" + serialno + "')";
                            sqls.Add(sql);
                            sql = "update INFO_QRCODE set ICcard='" + iccard + "',Password='" + icpassword + "' where qrcode='" + value + "'";
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
                code = false;
                msg = e.Message;
            }
            finally
            {

                Response.AddHeader("Content-Type", "application/octet-stream");
                Response.StatusCode = 200;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();

            }
        }




        public void ICCardAddOrcode()
        {
            bool code = true;
            string msg = "";
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out msg);
                if (CurrentUser.UserName == "")
                    code = Remote.UserService.Login(username, password, out msg);
                if (code)
                {
                    string qrcode = Request["qrcode"].GetSafeString();
                    string icno = Request["icno"].GetSafeString();
                    string key = CryptFun.Decode(qrcode);

                    if (key == "" || key == null)
                    {
                        code = false;
                        msg = "该二维码解码失败，二维码不存在，请确认！";
                    }
                    else
                    {
                        string sql = "update INFO_QRCODE  set iccard=b.ICCard,PassWord=b.PassWord from INFO_QRCODE a,INFO_ICCard b where a.Qrcode='" + key + "' and b.ICNo='" + icno + "'";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        code = CommonService.ExecTrans(sqls);
                    }
                }
                else
                {
                    msg = "登录失败，请重新登录app";
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



        public void AddBHGD()
        {
            bool code = true;
            string msg = "";
            string gcbh = Request["gcbh"].GetSafeString();

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (CurrentUser.UserName == "")
                {
                    code = false;
                    msg = "登陆超时 ，请重新登陆！";
                }
                else
                {
                    datas = CommonService.GetDataTable("select recid from I_M_GC_BHJH where gcbh='" + gcbh + "'");
                    if (datas.Count > 0)
                    {
                        code = false;
                        msg = "该工程已经申请过，请不要重复申请！";
                    }
                    else
                    {
                        string procstr = string.Format("AddBHJH('{0}', '{1}','{2}')", gcbh, CurrentUser.UserName, CurrentUser.RealName);
                        code = CommonService.ExecProc(procstr, out msg);
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



        #region
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

        #endregion


        #region 用户系统交互

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
                    cpcode += "CP201702000001";
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
                    cpcode += "CP201702000001";
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
                    cpcode += "CP201702000001";
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
                    string sfzhm = Request["sfzh"].GetSafeString("true");
                    ret = ModifyUserInfoByUsercode(username, realname, usercode, xb, sjhm, cpcode, depcode, "", procode, rolecodelist, clearrole, sfzhm);
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

                    sql = "INSERT INTO [I_M_QYZH]([QYBH],[YHZH],[SFQYZZH],[LRRZH],[LRRXM],[LRSJ],[ZHLX]) select ZH,rybh,0,'" + CurrentUser.UserName + "','" + CurrentUser.RealName + "',getdate(),'N' from dbo.I_M_NBRY where rybh='" + param["usercode"] + "'";
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


        public string ModifyUserInfoByUsercode(string username, string realname, string usercode, string xb, string sjhm, string cpcode, string depcode, string postdm, string procode, string rolecodelist, string clearrole, string sfzhm)
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
                    string sql = "update I_M_NBRY set zh='" + username + "',zjzbh='" + cpcode + "',ryxm='" + realname + "',sjhm='" + sjhm + "',xb='" + xb + "',sfzhm='" + sfzhm + "' where rybh='" + usercode + "'";
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

        public void test()
        {
            string ret = Request["value"].GetSafeString();
            try
            {
                ret = "{\"success\": true,\"compress\": false,\"msg\": \"\",\"data\": [{\"usercode\": \"UR201603000004\",\"username\": \"B3\"}]}";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                UmsRet umsret = jss.Deserialize<UmsRet>(ret);
                if (umsret.success)
                {
                    //Dictionary<string, object> param = (Dictionary<string, object>)umsret.data;
                    IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(umsret.data); //(IList<Dictionary<string, object>>)umsret.data;
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
                ret = e.Message;
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }


        public string GetUserListBySfzh(string sfzhm)
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



        #endregion


        #region 扬尘交互


        string iotapi = "http://iot.jzyglxt.com/";

        [Authorize]
        public string GetToken()
        {
            string tokenstring = "";
            try
            {
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable("select token,expirestime from SysToken where TokenType='iot'");
                if (dt.Count > 0)
                {
                    DateTime expricestime = dt[0]["expirestime"].GetSafeDate(DateTime.Now);
                    if (expricestime < DateTime.Now)
                    {
                        tokenstring = GetIOTToken();
                    }
                    else
                    {
                        tokenstring = dt[0]["token"].ToString();
                    }

                }
                else
                {
                    tokenstring = GetIOTToken();
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                tokenstring = "";
            }

            return tokenstring;
        }

        [Authorize]
        public string GetIOTToken()
        {
            string token = "";
            try
            {

                string url = iotapi + "/Api/GetToken";
                string dates = "UserName=tz_bd&PassWord=036a63aa8897817cde36678f774b995c&AppSecret=" + HttpUtility.UrlEncode("dS/zXsaGeQMJEF40Y2/ThJIRKe4+oljgLYN+PnetYc01tPoMD8TSVA==");
                string ret = SendDataByPost(url, dates);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                IOTRet iotret = jss.Deserialize<IOTRet>(ret);
                if (iotret.Code == "40003" || iotret.Code == "0")
                {
                    /*
                    IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(iotret.Datas);
                    
                    if (param.Count > 0)
                    {
                        string ExpiresTime="";
                        DateTime TokenTime=ConvertLongToTime(param[0]["TokenTime"].GetSafeLong()).AddSeconds(param[0]["Expires"].GetSafeDouble());
                        ExpiresTime=TokenTime.ToString("yyyy-MM-dd HH:mm:ss");
                        string sql = "INSERT INTO [SysToken]([TokenType],[Token],[TokenTime],[Expires],[ExpiresTime])VALUES('IOT','" + param[0]["Token"].GetSafeString() + "','" + param[0]["TokenTime"].GetSafeString() + "','" + param[0]["Expires"].GetSafeString() + "','" + ExpiresTime + "')";
                        IList<string> sqls = new List<string>();
                        sqls.Add("delete from SysToken where tokentype='IOT'");
                        sqls.Add(sql);
                        CommonService.ExecTrans(sqls);
                        //token = param[0]["Token"].GetSafeString();
                    }*/

                    Dictionary<string, object> param = (Dictionary<string, object>)iotret.Datas;
                    string ExpiresTime = "";
                    DateTime TokenTime = ConvertLongToTime(param["TokenTime"].GetSafeLong()).AddSeconds(param["Expires"].GetSafeDouble());
                    ExpiresTime = TokenTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string sql = "INSERT INTO [SysToken]([TokenType],[Token],[TokenTime],[Expires],[ExpiresTime])VALUES('IOT','" + param["Token"].GetSafeString() + "','" + param["TokenTime"].GetSafeString() + "','" + param["Expires"].GetSafeString() + "','" + ExpiresTime + "')";
                    IList<string> sqls = new List<string>();
                    sqls.Add("delete from SysToken where tokentype='IOT'");
                    sqls.Add(sql);
                    CommonService.ExecTrans(sqls);
                    token = param["Token"].GetSafeString();

                }

                //token = ret;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                token = "";
            }

            return token;
        }

        public void AddOrUpdateDevice()
        {
            bool code = false;
            string msg = "";
            string id = Request["id"].GetSafeString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            IDictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string token = GetToken();
                if (token != "")
                {
                    IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                    dt = CommonService.GetDataTable("select id ,gcbh ,gcmc ,szsf ,szcs ,szxq ,szjd ,gczb ,gcdd ,devicecode ,currenttime ,intervalmin ,typecode ,deviceid ,createtime ,notes from i_s_gc_dust where id='" + id + "'");
                    if (dt.Count > 0)
                    {
                        string[] tem = dt[0]["gczb"].GetSafeString().Split(',');
                        string lat = "0";
                        string lng = "0";
                        if (tem.Length > 1)
                        {
                            lat = tem[1].GetSafeString();
                            lng = tem[0].GetSafeString();
                        }
                        string CreateTime = ConvertTimeToLong(dt[0]["deviceid"].GetSafeDate(DateTime.Now)).ToString();
                        //这里没写完
                        IDictionary<string, string> di = new Dictionary<string, string>();
                        di.Add("Id", dt[0]["deviceid"].GetSafeString());
                        di.Add("TypeCode", dt[0]["typecode"].GetSafeString());
                        di.Add("DeviceCode", dt[0]["devicecode"].GetSafeString());
                        di.Add("Lon", lng);
                        di.Add("Lat", lat);
                        di.Add("Expires", dt[0]["intervalmin"].GetSafeString());
                        di.Add("CreateTime", CreateTime);
                        di.Add("Notes", dt[0]["notes"].GetSafeString());
                        di.Add("Custom", dt[0]["gcbh"].GetSafeString() + "|" + dt[0]["gcmc"].GetSafeString());

                        string dates = "token=" + token + "&Datas=" + HttpUtility.UrlEncode(jss.Serialize(di));
                        if (dt[0]["deviceid"].GetSafeString() == "")
                        {
                            //这里写两个，一个save，一个update方法
                            code = AddDevice(dates, out msg);

                            if (code)
                            {
                                IList<string> sqls = new List<string>();
                                sqls.Add("update i_s_gc_dust set deviceid='" + msg + "' where id='" + id + "'");
                                code = CommonService.ExecTrans(sqls);
                                msg = "";
                            }
                        }
                        else
                        {
                            code = UpdateDevice(dates, out msg);
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "数据不存在!";
                    }
                }
                else
                {
                    code = false;
                    msg = "token无效，请联系管理员!";
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
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }


        }


        public void DeleteDevice()
        {
            bool code = false;
            string msg = "";
            string id = Request["id"].GetSafeString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            IDictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable("select id ,gcbh ,gcmc ,szsf ,szcs ,szxq ,szjd ,gczb ,gcdd ,devicecode ,currenttime ,intervalmin ,typecode ,deviceid ,createtime ,notes from i_s_gc_dust where id='" + id + "'");
                if (dt.Count > 0)
                {
                    if (dt[0]["deviceid"].GetSafeString() == "")
                    {
                        IList<string> sqls = new List<string>();
                        sqls.Add("delete from i_s_gc_dust where id='" + id + "'");
                        code = CommonService.ExecTrans(sqls);
                    }
                    else
                    {
                        string token = GetToken();
                        if (token != "")
                        {
                            string dates = "token=" + token + "&Datas=" + HttpUtility.UrlEncode(dt[0]["deviceid"].GetSafeString());
                            string url = iotapi + "/Api/DelDevice";
                            string ret = SendDataByPost(url, dates);
                            IOTRet iotret = jss.Deserialize<IOTRet>(ret);
                            if (iotret.Code == "0")
                            {
                                code = true;
                                IList<string> sqls = new List<string>();
                                sqls.Add("delete from i_s_gc_dust where id='" + id + "'");
                                code = CommonService.ExecTrans(sqls);
                            }
                            else
                            {
                                code = false;
                                msg = iotret.Msg.GetSafeString();
                                SysLog4.WriteLog("物联网大平台代码：" + iotret.Code + ":" + msg);
                            }
                        }
                        else
                        {
                            code = false;
                            msg = "token无效，请联系管理员!";
                        }
                    }
                }
                else
                {
                    code = false;
                    msg = "数据不存在!";
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
                Response.ContentType = "text/plain";

                Response.Write(JsonFormat.GetRetString(code, msg));
                Response.End();
            }
        }


        public bool AddDevice(string data, out string msg)
        {
            bool code = true;
            msg = "";
            try
            {
                string url = iotapi + "/Api/SaveDevice";
                string ret = SendDataByPost(url, data);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                IOTRet iotret = jss.Deserialize<IOTRet>(ret);
                if (iotret.Code == "0" || iotret.Code == "1")
                {
                    msg = iotret.Datas.GetSafeString();

                }
                else
                {
                    code = false;
                    msg = iotret.Msg;
                    SysLog4.WriteLog("物联网大平台代码：" + iotret.Code + ":" + msg);
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return code;
        }


        public bool UpdateDevice(string data, out string msg)
        {
            bool code = true;
            msg = "";
            try
            {
                string url = iotapi + "/Api/UpdateDevice";
                string ret = SendDataByPost(url, data);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                IOTRet iotret = jss.Deserialize<IOTRet>(ret);
                if (iotret.Code == "0")
                {
                    msg = iotret.Datas.GetSafeString();

                }
                else
                {
                    code = false;
                    msg = iotret.Msg;
                    SysLog4.WriteLog("物联网大平台代码：" + iotret.Code + ":" + msg);
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }

            return code;
        }



        public void GetHpTypeList()
        {
            bool code = false;
            string msg = "";

            IDictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string token = GetToken();
                if (token != "")
                {
                    string url = iotapi + "Api/GetHpTypeList";
                    string dates = "Token=" + HttpUtility.UrlEncode(token);
                    string ret = SendDataByPost(url, dates);
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    IOTRet iotret = jss.Deserialize<IOTRet>(ret);
                    if (iotret.Code == "0")
                    {
                        code = true;
                        IList<object> dt = new List<object>();
                        IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(iotret.Datas);
                        IList<IDictionary<string, string>> item = new List<IDictionary<string, string>>();

                        for (int i = 0; i < param.Count; i++)
                        {
                            IDictionary<string, string> di = new Dictionary<string, string>();
                            di.Add("fieldname", "Text");
                            di.Add("fieldvalue", param[i]["Text"].GetSafeString());

                            IDictionary<string, string> dv = new Dictionary<string, string>();
                            dv.Add("fieldname", "Id");
                            dv.Add("fieldvalue", param[i]["Id"].GetSafeString());

                            item.Add(di);
                            item.Add(dv);
                            dt.Add(item);
                        }

                        data.Add("total", param.Count);
                        data.Add("rows", dt);
                    }
                    else
                    {
                        code = false;
                        msg = iotret.Msg;
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
                string rettext = string.Format("{{\"success\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", code ? "true" : "false", msg, jss.Serialize(data));
                Response.ContentType = "text/plain";

                Response.Write(rettext);
                Response.End();
            }


        }



        /// <summary>
        /// 获取传感器类型列表
        /// </summary>
        public void GetHpDustList()
        {
            string ret = "";
            try
            {
                string token = GetToken();
                if (token != "")
                {
                    string url = iotapi + "Api/GetHpDustList";
                    string dates = "Token=" + HttpUtility.UrlEncode(token);
                    ret = SendDataByPost(url, dates);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            finally
            {

                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }


        /// <summary>
        /// 获取扬尘数据列表
        /// </summary>
        public void GetPageDustList()
        {
            string ret = "";
            try
            {
                string token = GetToken();
                int PageCount = Request["PageCount"].GetSafeInt(1);
                int PageSize = Request["PageSize"].GetSafeInt(20);
                string DeviceCode = Request["DeviceCode"].GetSafeString();
                string SensorCode = Request["SensorCode"].GetSafeString();
                string StartTime = Request["StartTime"].GetSafeString();
                string EndTime = Request["EndTime"].GetSafeString();
                StartTime = ConvertTimeToLong(StartTime.GetSafeDate(DateTime.Now)).ToString();
                EndTime = ConvertTimeToLong(EndTime.GetSafeDate(DateTime.Now)).ToString();
                if (token != "")
                {
                    string url = iotapi + "Api/GetPageDustList";
                    string dates = "Token=" + HttpUtility.UrlEncode(token) + "&PageCount=" + HttpUtility.UrlEncode(PageCount.ToString()) + "&PageSize=" + HttpUtility.UrlEncode(PageSize.ToString()) + "&DeviceCode=" + HttpUtility.UrlEncode(DeviceCode) + "&SensorCode=" + HttpUtility.UrlEncode(SensorCode) + "&StartTime=" + HttpUtility.UrlEncode(StartTime) + "&EndTime=" + HttpUtility.UrlEncode(EndTime);
                    ret = SendDataByPost(url, dates);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            finally
            {

                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }



        /// <summary>
        /// 获取扬尘预警列表
        /// </summary>
        public void GetDustErrorList()
        {
            string ret = "";
            try
            {
                string token = GetToken();
                int PageCount = Request["PageCount"].GetSafeInt(1);
                int PageSize = Request["PageSize"].GetSafeInt(20);
                string DeviceCode = Request["DeviceCode"].GetSafeString();
                string SensorCode = Request["SensorCode"].GetSafeString();
                string StartTime = Request["StartTime"].GetSafeString();
                string EndTime = Request["EndTime"].GetSafeString();
                StartTime = ConvertTimeToLong(StartTime.GetSafeDate(DateTime.Now)).ToString();
                EndTime = ConvertTimeToLong(EndTime.GetSafeDate(DateTime.Now)).ToString();
                if (token != "")
                {
                    string url = iotapi + "Api/GetDustErrorList";
                    string dates = "Token=" + HttpUtility.UrlEncode(token) + "&PageCount=" + HttpUtility.UrlEncode(PageCount.ToString()) + "&PageSize=" + HttpUtility.UrlEncode(PageSize.ToString()) + "&DeviceCode=" + HttpUtility.UrlEncode(DeviceCode) + "&SensorCode=" + HttpUtility.UrlEncode(SensorCode) + "&StartTime=" + HttpUtility.UrlEncode(StartTime) + "&EndTime=" + HttpUtility.UrlEncode(EndTime);
                    ret = SendDataByPost(url, dates);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            finally
            {

                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }


        /// <summary>
        /// 获取扬尘折线图
        /// </summary>
        public void GetDeviceChart()
        {
            string ret = "";
            try
            {
                string token = GetToken();
                string DeviceCode = Request["DeviceCode"].GetSafeString();
                string SensorName = Request["SensorName"].GetSafeString();

                string SensorCode = Request["SensorCode"].GetSafeString();
                string StartTime = Request["StartTime"].GetSafeString();
                string EndTime = Request["EndTime"].GetSafeString();
                StartTime = ConvertTimeToLong(StartTime.GetSafeDate(DateTime.Now)).ToString();
                EndTime = ConvertTimeToLong(EndTime.GetSafeDate(DateTime.Now)).ToString();
                if (token != "")
                {
                    string url = iotapi + "Api/GetDeviceChart";
                    string dates = "Token=" + HttpUtility.UrlEncode(token) + "&SensorName=" + HttpUtility.UrlEncode(SensorName) + "&DeviceCode=" + HttpUtility.UrlEncode(DeviceCode) + "&SensorCode=" + HttpUtility.UrlEncode(SensorCode) + "&StartTime=" + HttpUtility.UrlEncode(StartTime) + "&EndTime=" + HttpUtility.UrlEncode(EndTime);
                    ret = SendDataByPost(url, dates);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            finally
            {

                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }



        /// <summary>
        /// 获取扬尘折线图
        /// </summary>
        public void GetWarnDustMap()
        {
            string ret = "";
            try
            {
                string token = GetToken();
                string DeviceCode = Request["DeviceCode"].GetSafeString();
                string SensorName = Request["SensorName"].GetSafeString();

                string SensorCode = Request["SensorCode"].GetSafeString();
                string StartTime = Request["StartTime"].GetSafeString();
                string EndTime = Request["EndTime"].GetSafeString();
                StartTime = ConvertTimeToLong(StartTime.GetSafeDate(DateTime.Now)).ToString();
                EndTime = ConvertTimeToLong(EndTime.GetSafeDate(DateTime.Now)).ToString();
                if (token != "")
                {
                    string url = iotapi + "Api/GetWarnDustMap";
                    string dates = "Token=" + HttpUtility.UrlEncode(token) + "&SensorName=" + HttpUtility.UrlEncode(SensorName) + "&DeviceCode=" + HttpUtility.UrlEncode(DeviceCode) + "&SensorCode=" + HttpUtility.UrlEncode(SensorCode) + "&StartTime=" + HttpUtility.UrlEncode(StartTime) + "&EndTime=" + HttpUtility.UrlEncode(EndTime);
                    ret = SendDataByPost(url, dates);
                    /*
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    IOTRet iotret = jss.Deserialize<IOTRet>(ret);
                    if (iotret.Code == "0")
                    {
                        IDictionary<string, object> data = new Dictionary<string, object>();
                        IList<object> dt = new List<object>();
                        IList<Dictionary<string, object>> param = jss.ConvertToType<IList<Dictionary<string, object>>>(iotret.Datas);
                        IList<IDictionary<string, string>> item = new List<IDictionary<string, string>>();

                        for (int i = 0; i < param.Count; i++)
                        {
                            IDictionary<string, string> di = new Dictionary<string, string>();
                            di.Add("DeviceCode", param[i]["DeviceCode"].GetSafeString());
                            di.Add("SensorCode", param[i]["SensorCode"].GetSafeString());
                            di.Add("Total", param[i]["Total"].GetSafeString());
                            di.Add("Lon", param[i]["Lon"].GetSafeString());
                            di.Add("Lat", param[i]["Lat"].GetSafeString());

                            string gcmc = "";
                            string gcbh = "";
                            IList<IDictionary<string, string>> gcdt = new List<IDictionary<string, string>>();
                            gcdt = CommonService.GetDataTable("select id ,gcbh ,gcmc ,szsf ,szcs ,szxq ,szjd ,gczb ,gcdd ,devicecode ,currenttime ,intervalmin ,typecode ,deviceid ,createtime ,notes from i_s_gc_dust where devicecode='" + param[i]["DeviceCode"].GetSafeString() + "'");
                            if (gcdt.Count>0)
                           {
                               gcmc = gcdt[0]["gcmc"].GetSafeString();
                               gcbh = gcdt[0]["gcbh"].GetSafeString();
                            }
                            di.Add("gcmc", gcmc);
                            di.Add("gcbh", gcbh);

                            item.Add(di);
                            //dt.Add(item);
                        }

                        data.Add("Code", iotret.Code);
                        data.Add("Msg", iotret.Msg);
                        data.Add("Datas", item);
                        ret = jss.Serialize(data);
                    }*/


                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            finally
            {

                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }




        [Authorize]
        public void gotoIOT()
        {
            string err = "";
            string ret = "";
            try
            {
                string api = Request["api"].GetSafeString("Api");
                string func = Request["func"].GetSafeString("");

                if (func != "")
                {

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = JsonFormat.GetRetString(false, err);
            }
            finally
            {
                Response.ContentType = "text/plain";

                Response.Write(ret);
                Response.End();
            }
        }



        public class IOTRet
        {
            /// <summary>
            /// 返回结果
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public string Msg { get; set; }
            /// <summary>
            /// 对象
            /// </summary>
            public object Datas { get; set; }

        }


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

        /// <summary>
        /// Unix时间戳格式转换为DateTime时间格式
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2018/4/8 14:19
        private static DateTime ConvertLongToTime(long unixTimeStamp)
        {
            //当地时区
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return startTime.AddSeconds(unixTimeStamp);
        }



        /// <summary>
        /// 获取传感器工程
        /// </summary>
        public void GetCGQGC()
        {
            string msg = "";
            bool code = true;
            string ret = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                //string devicecode=Request
                //dt = CommonService.GetDataTable("select id ,gcbh ,gcmc ,szsf ,szcs ,szxq ,szjd ,gczb ,gcdd ,devicecode ,currenttime ,intervalmin ,typecode ,deviceid ,createtime ,notes from i_s_gc_dust where devicecode='" + devicecode + "'");

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }

        }




        #endregion





        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

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


        public ActionResult GetSensorList()
        {
            string sql = "SELECT SensorCode,SensorName FROM dbo.H_Sensor ORDER BY OrderBy";
            IList<IDictionary<string, string>> list = CommonService.GetDataTable(sql);
            return Content(JsonConvert.SerializeObject(list));
        }


        #region 视频中心数据提供

        public void GetVideoTreeBySearchType()
        {
            bool ret = true;
            string msg = "";
            string district = Request["district"].GetSafeString();
            string key = Request["key"].GetSafeString();
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string type = Request["type"].GetSafeString("gc");
                if (type != "")
                {
                    if (type == "gc")
                    {
                        string sql = "select gcmc,gcbh,gczb from i_M_GC where gcbh in(select gcbh from I_S_GC_Video) ";
                        if (district != "")
                            sql += " and SZXQ='" + district + "'";
                        if (key != "")
                            sql += " and gcmc like '%" + key + "%'";
                        IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                        foreach (var row in d)
                        {
                            IDictionary<string, object> item = new Dictionary<string, object>();
                            // 生成父节点
                            IDictionary<string, object> node = new Dictionary<string, object>();
                            node.Add("name", row["gcmc"]);
                            node.Add("gczb", row["gczb"]);

                            // 生成子节点
                            IList<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
                            sql = string.Format("select * from I_S_GC_Video where gcbh='{0}'", row["gcbh"].GetSafeString());
                            IList<IDictionary<string, object>> cd = CommonService.GetDataTable2(sql);

                            node.Add("total", cd.Count.ToString());
                            node.Add("type", type);
                            node.Add("data", row);
                            item.Add("node", node);



                            foreach (var cdrow in cd)
                            {
                                IDictionary<string, object> cnode = new Dictionary<string, object>();
                                cnode.Add("name", cdrow["bz"]);
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

                        /*
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
                        }*/
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

        #endregion


    }
}
