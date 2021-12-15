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

        #region 德浩数据

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

        #region  人员管理-人员信息

        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UmsEdit2()
        {

            string usercode = Request["usercode"].GetSafeString();

            string sql = "select zh,jcjgbh,rybh,ryxm,xb,sfzhm,sjhm,ssksbh,usercode,cpcode,sfsyr from dbo.I_M_NBRY_JC where usingnow=1 and usercode='" + usercode + "'";
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
                ViewBag.ksbh = dt[i]["ssksbh"];
                ViewBag.sfsyr = dt[i]["sfsyr"];
            }

            //增加一个获取用户权限，然后丢回去的

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



        #endregion


        #region 材料管理
        /// <summary>
        /// 用户管理界面，列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MaterialEdit()
        {
            string recid = Request["recid"].GetSafeString();

            string sql = "select * from dbo.OA_MateriaInfo where Status<>'-1' and Recid='" + recid + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            for (int i = 0; i < dt.Count; i++)
            {
                ViewBag.MaterialName = dt[i]["materialName"];
                ViewBag.MaterialUnit = dt[i]["materialUnit"];
                ViewBag.Price = dt[i]["price"];
                ViewBag.PurchasePrice = dt[i]["purchasePrice"];
                ViewBag.Quantity = dt[i]["quantity"];
                ViewBag.TechnicalRequirement = dt[i]["technicalRequirement"];
                ViewBag.Supplier = dt[i]["supplier"];
                ViewBag.Manufacturer = dt[i]["manufacturer"];
                ViewBag.Purpose = dt[i]["purpose"];
            }
            return View();
        }

        public void ModifyMaterialInfo()
        {
            string err = "";
            bool ret = true;
            try
            {
                //材料记录唯一号
                string recid = Request["recid"].GetSafeString();

                //操作记录 add新增  update 修改 
                string type = Request["operType"].GetSafeString();
                //数据类型 1：办公消耗 2：试验消耗 3：采购
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


                ret = OaService.ModifyMaterialInfo(type, dataType, recid, matId, matName, unitId, unitName, price, purchasePrice, quantity, purpose, technicalRequirement, supplier, manufacturer, requisitioner);


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
            }
            finally
            {
                //JavaScriptSerializer jss = new JavaScriptSerializer();
                //jss.MaxJsonLength = 10240000;
                //Response.ContentEncoding = System.Text.Encoding.UTF8;
                //Response.Write(string.Format("{{ \"code\":\"{0}\", \"msg\": \"{1}\"}}", code, err));
                //Response.End();
                //Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"Datas\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));

                Response.ContentType = "text/plain";
                Response.Write(ret);
                Response.End();
            }
        }



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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "1" : "0", msg));
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "1" : "0", msg));
                Response.End();
            }
        }
        #endregion


        #region 车辆管理
        /// <summary>
        /// 车辆信息修改
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CarEdit()
        {
            string id = Request["id"].GetSafeString();

            string sql = " select  * from OA_CarInfomation where   Status <>-1 and   JCJGBH='" + CurrentUser.Qybh + "' and id='" + id + "'";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", code ? "1" : "0", msg));
                Response.End();
            }
        }

        #endregion
        #endregion
    }
}