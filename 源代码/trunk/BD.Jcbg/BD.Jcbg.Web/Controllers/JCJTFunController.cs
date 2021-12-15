using System.Linq;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI;
using BD.DataInputCommon;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using BD.IDataInputBll;
using System.Web;
using ReportPrint.Common;
using SysLog4 = BD.Jcbg.Common.SysLog4;
using Ionic.Zip;
using NHibernate;
using System.Data;
using System.Net;
using ReportPrint;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Newtonsoft.Json.Linq;
using System;
using BD.Jcbg.Web.Common;
using System.Collections.Generic;

namespace BD.Jcbg.Web.Controllers
{
    public class JCJTFunController : Controller
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
        private ISelfService _selfService = null;
        private ISelfService SelfService
        {
            get
            {
                try
                {
                    if (_selfService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _selfService = webApplicationContext.GetObject("SelfService") as ISelfService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _selfService;
            }
        }
        #endregion

        #region  方法
        /// <summary>
        /// 获取检测单位项目分组
        /// </summary>
        [LoginAuthorize]
        public void GetJcdwXmfz()
        {
            string ret = "[]";
            string dwbh = Request["dwbh"].GetSafeString();
            if (dwbh == "")
                dwbh = CurrentUser.Qybh;
            try
            {
                string where = "";
                where += " and ssdwbh='" + dwbh + "'";
                IList<IDictionary<string, string>> xmfls = CommonService.GetDataTable("select sjxsflbh,xsflbh,xsflmc from pr_m_syxmxsfl where 1=1  and sfyx=1 " + where + " order by xssx");
                if (xmfls.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(xmfls);
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
        /// <summary>
        /// 获取检测单位的所有项目
        /// </summary>
        [LoginAuthorize]
        public void GetJcdwXm()
        {
            string ret = "[]";
            string dwbh = Request["dwbh"].GetSafeString();
            int yx = Request["yx"].GetSafeInt(1);
            int yzb = Request["yzb"].GetSafeInt(1);
            bool global = Request["global"].GetSafeBool();
            bool fbxm = Request["fbxm"].GetSafeBool(false);
            string limitxmbh = Request["limitxmbh"].GetSafeString();
            string usercode = Request["usercode"].GetSafeString();


            try
            {
                if (dwbh == "")
                    dwbh = CurrentUser.Qybh;
                //string where = " and a.xmlx<>'3' "; 结构实体子项目
                string where = " and 1=1 ";
                if (yx == 1)
                    where += " and a.sfyx=1 ";
                if (yzb == 1)
                    where += " and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh ) ";
                if (fbxm)
                    where += " and a.yxfb=1 ";
                if (!string.IsNullOrEmpty(limitxmbh) && limitxmbh != "null" && !limitxmbh.Equals("all", StringComparison.OrdinalIgnoreCase))
                    where += " and a.syxmbh in (" + limitxmbh.FormatSQLInStr() + ") ";
                if (!string.IsNullOrEmpty(usercode))
                    where += " and '' like  ";
                string sql = "select a.xsflbh,a.syxmbh,a.syxmmc,a.sfyx,a.recid,a.wtdlrbj,a.xmlx,(select top 1 b.xmdh from PR_M_WTDWJH b where b.ssdwbh=a.ssdwbh and a.syxmbh=b.syxmbh) as xmdh,(select count(*) from PR_M_WTDWJH b where b.ssdwbh=a.ssdwbh and a.syxmbh=b.syxmbh) as xmdhsl from pr_m_syxm a where a.ssdwbh='" + dwbh + "' " + where + "  order by a.xsflbh,a.xssx";

                IList<IDictionary<string, string>> xms = CommonService.GetDataTable(sql);
                foreach (IDictionary<string, string> row in xms)
                {
                    int xmdhsl = row["xmdhsl"].GetSafeInt();
                    if (xmdhsl == 0)
                        row["xmdh"] = row["syxmbh"];
                    row.Remove("xmdhsl");
                }
                if (xms.Count > 0)
                    ret = new JavaScriptSerializer().Serialize(xms);
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
        [LoginAuthorize]
        public void GetUserList2()
        {
            string msg = "";
            bool code = true;
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

                int pageindex = Request["page"].GetSafeInt(1);
                int pagesize = Request["rows"].GetSafeInt(20);

                sb.Append("[");

                string sql = $"select a.zh as name,b.jcjgmc as cpname,a.ksmc, a.usercode as id,a.ryxm as text,case when a.sfyx=1 then 1 else 0 end as sfyx " +
                    $"from View_I_M_NBRY_JCJG a, h_jcjg b " +
                    $"where a.jcjgbh=b.jcjgbh";
                if (companyid != "")
                    sql += " and a.jcjgbh='" + companyid + "'";
                if (realname != "")
                    sql += " and a.ryxm  like '%" + realname + "%'";
                if (sfzhm != "")
                    sql += " and a.sfzhm like '%" + sfzhm + "%'";
                if (CurrentUser.Qybh != "")
                    sql += " and a.jcjgbh='" + CurrentUser.Qybh + "'";

                dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 录入检测系统人员
        /// </summary>
        [LoginAuthorize]
        public void SetJCRydw()
        {
            bool code = false;
            string msg = "";
            try
            {
                string recids = Request["recids"].GetSafeRequest();
                string username = CurrentUser.UserName;
                if (string.IsNullOrEmpty(username))
                {
                    code = false;
                    msg = "当前用户信息获取失败，请重新登录系统";
                }
                else
                {
                    code = JcjtService.SetJCRydw(recids, username, CurrentUser.RealName, out msg);
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
        /// <summary>
        /// 辞退检测系统人员
        /// </summary>
        [LoginAuthorize]
        public void CtJCRydw()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeRequest();
                string username = CurrentUser.UserName;
                code = JcjtService.CtJCRy(usercode, username, out msg);
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

        /// <summary>
        /// 录用业务员
        /// </summary>
        [LoginAuthorize]
        public void SetYwy()
        {
            bool code = false;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                string username = CurrentUser.UserName;
                if (string.IsNullOrEmpty(username))
                {
                    code = false;
                    msg = "当前用户信息获取失败，请重新登录系统";
                }
                else
                {
                    code = JcjtService.SetYwy(rybh, out msg);
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
        /// <summary>
        /// 辞退业务员
        /// </summary>
        [LoginAuthorize]
        public void CtYwy()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeRequest();
                string username = CurrentUser.UserName;
                if (string.IsNullOrEmpty(usercode))
                    throw new Exception("请选择要辞退的业务员");
                code = JcjtService.CtYwy(usercode, username, out msg);
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

        [LoginAuthorize]
        public JsonResult GetRyJcjg()
        {
            bool code = true;
            string msg = "";
            string qybh = "";
            string qymc = "";
            try
            {
                string usercode = CurrentUser.UserCode;
                IList<IDictionary<string, string>> datas = JcjtService.getJcryJCJG(usercode, out msg);
                if (msg != "")
                    code = false;
                else
                {
                    qybh = datas[0]["jcjgbh"];
                    qymc = datas[0]["jcjgmc"];
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg = msg, qybh = qybh, qymc = qymc }, JsonRequestBehavior.AllowGet);
        }

        //登录前获取检测人员的所有检测机构，进行选择登录的单位
        public JsonResult GetJcRyAllJcjg()
        {
            bool code = true;
            string msg = "";
            string usercode = "";
            string rylogintype = "0"; //用户登录logintype 1：监管校验
            bool ret = false;
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                //用户名
                string username = Request["username"].GetSafeString();
                //密码
                string userpwd = Request["userpwd"].GetSafeString();
                string type = Request["logintype"].GetSafeString();
                //string verifyCode = Request["verifyCode"].GetSafeString();
                //判断参数情况
                if (username == "")
                {
                    code = false;
                    msg = "用户名不能为空！";
                }
                if (userpwd == "")
                {
                    code = false;
                    msg = "密码不能为空！";
                }
                if (code)
                {
                    if (type == "1")
                    {
                        if (IsVerifyCodeRight(username, userpwd, Request))
                        {
                            //err = "短信验证码错误";
                            //ret = SelfService.GetUserCode(username, out usercode);//手机号转 username =》 i_m_qyzh-YHZH 账号
                            //if (!ret && username == "") msg = "该手机号未绑定账号";
                            //else if (!ret && username != "") msg = usercode;
                            //else datas = JcjtService.GetJcRyAllJcjg(usercode, out msg, type);
                        }
                        else
                        {
                            code = false;
                            msg = "短信验证码错误";
                        }

                    }
                    else
                    {
                        //判断用户校验是检测还是监管的用户系统
                        string sql = $"select * from i_m_nbry_jc where zh='{username}'";
                        var rydt = CommonService.GetDataTable(sql);
                        if (rydt.Count == 0)
                        {
                            //throw new Exception("账户错误");
                            string realname = "";
                            code = UserService.CheckLogin(username, userpwd, out usercode, out realname, out msg);
                            if (code)
                            {
                                //datas = JcjtService.GetJcRyAllJcjg(usercode, out msg);
                            }
                            else
                            {
                                msg = "用户校验失败,账号或密码错误";
                            }
                        }
                        else
                        {
                            string logintype = rydt[0]["logintype"];
                            if (logintype == "1") //监管的用户系统
                            {
                                rylogintype = "1";
                                //datas = JcjtService.GetJcRyAllJcjgByUsername(username, out msg);
                            }
                            else
                            {
                                string realname = "";
                                code = UserService.CheckLogin(username, userpwd, out usercode, out realname, out msg);
                                if (code)
                                {
                                    //datas = JcjtService.GetJcRyAllJcjg(usercode, out msg);
                                }
                                else
                                {
                                    msg = "用户校验失败,账号或密码错误";
                                }
                            }
                        }

                    }
                }

            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg, datas, rylogintype }, JsonRequestBehavior.AllowGet);
        }


        //设置选择当前登录的检测机构
        public JsonResult SetCurrentLoginJcjg()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                //用户名
                string username = Request["username"].GetSafeString();
                //密码
                string userpwd = Request["userpwd"].GetSafeString();
                string jcjgbh = Request["jcjgbh"].GetSafeString();
                //判断参数情况
                if (username == "")
                {
                    code = false;
                    msg = "用户名不能为空！";
                }
                if (userpwd == "")
                {
                    code = false;
                    msg = "密码不能为空！";
                }
                if (jcjgbh == "")
                {
                    code = false;
                    msg = "选择检测机构不能为空！";
                }
                if (code)
                {
                    string usercode = "", realname = "";
                    code = UserService.CheckLogin(username, userpwd, out usercode, out realname, out msg);
                    if (code)
                    {
                        //code = JcjtService.SetCurrentLoginJcjg(usercode, jcjgbh);
                    }
                    else
                    {
                        msg = "用户校验失败,账号或密码错误";
                    }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            return Json(new { code = code ? "0" : "1", msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion
     


        #region 判断验证码是否有效
        private bool IsVerifyCodeRight(string username, string verifycode, HttpRequestBase request)
        {
            //if (!GlobalVariable.LoginSmsVerify(request))
            //    return true;
            string key = "USER_CONTROLLER_VERIFY_CODE_TIME_" + username;
            var obj = HttpRuntime.Cache.Get(key);
            if (obj == null)
                return false;
            IDictionary<string, object> cacheItem = obj as IDictionary<string, object>;
            string shouldCode = cacheItem["code"].GetSafeString();
            return shouldCode == verifycode;
        }
        #endregion

    

        #region 获取检测机构编号和名称
        [LoginAuthorize]
        public JsonResult GetJcjgInfos()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                //datas = JcjtService.GetJcjgInfos();
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

        #region 上传资料文件
        [LoginAuthorize]
        public JsonResult UploadFile()
        {
            bool code = true;
            string msg = "";
            string msg2 = "";
            try
            {
                string filepath = "D:/代码/Git/新检测集团版/newjcjt/源代码/trunk/BD.Jcbg/BD.Jcbg.Web/FILE/";
                DirectoryInfo TheFolder = new DirectoryInfo(filepath);
                //JcjtService.scan("/FILE/", TheFolder,"");               
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return Json(new { code = code ? "0" : "1", msg, msg2 }, JsonRequestBehavior.AllowGet);
        }

        #endregion        



    }
}