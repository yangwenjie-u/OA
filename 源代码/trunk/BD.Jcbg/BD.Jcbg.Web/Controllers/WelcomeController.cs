using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using BD.Jcbg.Web;

namespace BD.Jcbg.Web.Controllers
{
    public class WelcomeController : Controller
    {
        #region 常量

        private readonly string QYLX_SGQY = "11";
        private readonly string QYLX_JLQY= "12";
        private readonly string QYLX_JSQY= "13";
        private readonly string QYLX_SJQY= "14";
        private readonly string QYLX_KCQY= "15";

        private readonly string[] m_strProvince = { "台湾", "河北", "山西", "内蒙古", "辽宁", "吉林", "黑龙江", "江苏", "浙江", "安徽", "福建", "江西", "山东", "河南", "湖北", "湖南", "广东", "广西", "海南", "四川", "贵州", "云南", "西藏", "陕西", "甘肃", "青海", "宁夏", "新疆", "北京", "天津", "上海", "重庆", "香港", "澳门" };
        
        #endregion

        #region 服务
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
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                try
                {
                    if (_commonService == null)
                    {
                        IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                        _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                return _commonService;
            }
        }
        private BD.Log.IBll.ILogService _logService = null;
        private BD.Log.IBll.ILogService LogService
        {
            get
            {
                if (_logService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _logService = webApplicationContext.GetObject("LogService") as BD.Log.IBll.ILogService;
                }
                return _logService;
            }
        }
        private BD.WorkFlow.Bll.RemoteUserService _remoteUserService = null;
        private BD.WorkFlow.Bll.RemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as BD.WorkFlow.Bll.RemoteUserService;
                }
                return _remoteUserService;
            }
        }
        #endregion

        #region 页面

        /// <summary>
        /// 政府端展示页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomezf()
        {
            ViewData["userName"] = CurrentUser.RealName;
            ViewData["provinceName"] = "";
            ViewData["cityName"] = "";
            ViewData["districtName"] = "";
            return View();
        }

        /// <summary>
        /// 政府端展示页全屏
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomezfqp()
        {
            ViewData["userName"] = CurrentUser.RealName;
            ViewData["provinceName"] = "浙江省";
            ViewData["cityName"] = "";
            ViewData["districtName"] = "";
            return View();
        }

        /// 企业端展示页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomeqy()
        {
            ViewData["userName"] = CurrentUser.RealName;
            ViewData["provinceName"] = "浙江省";
            ViewData["cityName"] = "";
            ViewData["districtName"] = "";

            //通过登录账号获取企业编号
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string sql = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
            dt = CommonService.GetDataTable(sql);

            string qybh = dt[0]["qybh"].ToString();
            string qymc = dt[0]["qymc"].ToString();

            ViewData["qybh"] = qybh;
            ViewData["qymc"] = qymc;
            return View();
        }

        /// <summary>
        /// 企业端展示页全屏
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomeqyqp()
        {
            ViewData["userName"] = CurrentUser.RealName;
            ViewData["provinceName"] = "浙江省";
            ViewData["cityName"] = "台州市";
            ViewData["districtName"] = "";

            //通过登录账号获取企业编号
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string sql = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
            dt = CommonService.GetDataTable(sql);

            string qybh = dt[0]["qybh"].ToString();
            string qymc = dt[0]["qymc"].ToString();

            ViewData["qybh"] = qybh;
            ViewData["qymc"] = qymc;
            return View();
        }

        /// <summary>
        /// 工程端展示页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomegc()
        {

            //通过登录账号获取工程编号
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string sql = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'"; 
            dt = CommonService.GetDataTable(sql);

            string gcbh = dt[0]["jdzch"].ToString();
            string gcmc = dt[0]["gcmc"].ToString();
            ViewData["gcbh"] = gcbh;
            ViewData["gcmc"] = gcmc;

            return View();
        }

        /// <summary>
        /// 工程端展示页全屏
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomegcqp()
        {
            string gcbh = Request["gcbh"].GetSafeString();
            string gcmc = Request["gcmc"].GetSafeString();
            ViewData["gcbh"] = gcbh;
            ViewData["gcmc"] = gcmc;
            return View();
        }

        /// <summary>
        /// 考勤历史
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomerykqls()
        {
            string sfzh = Request["sfzh"].GetSafeString();
            ViewData["sfzh"] = sfzh;
            return View();
        }

        /// <summary>
        /// 人员信息类似时间轴展示
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomeryz()
        {
            string sfzhm = Request["sfzhm"].GetSafeString();
            string gcbh = Request["gcbh"].GetSafeString();
            ViewData["sfzhm"] = sfzhm;
            ViewData["gcbh"] = gcbh;
            return View();
        }

        /// <summary>
        /// 企业相关工程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult welcomeqygc()
        {
            string qybh = Request["qybh"].GetSafeString();
            ViewData["qybh"] = qybh;
            return View();
        }
        
     
        #endregion


        #region 数据取得
        /// <summary>
        /// 获取下拉框企业一览
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

                string sql = "select distinct b.QYBH,b.QYMC from( ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,b.QYBH from I_M_GC A left join I_S_GC_SGDW b on a.GCBH = b.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,c.QYBH  from I_M_GC A left join I_S_GC_JLDW c on a.GCBH = c.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,d.QYBH  from I_M_GC A left join I_S_GC_JSDW d on a.GCBH = d.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,e.QYBH  from I_M_GC A left join I_S_GC_SJDW e on a.GCBH = e.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,f.QYBH  from I_M_GC A left join I_S_GC_KCDW f on a.GCBH = f.gcbh ";
                sql += ") a ";
                sql += "left join I_M_QY b on a.QYBH = b.QYBH ";
                sql += where;
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
        /// 获取工程一览
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetGcList()
        {
            int totalcount = 0;

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
                string gcmc = Request["gcmc"].GetSafeString();

                string gclx = Request["gcxz"].GetSafeString();

                string pagesize = Request["limit"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["page"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }
                string qybh_yc = "";
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc,b.qybh_yc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                    qybh_yc = dtt[0]["qybh_yc"].ToString();
                }
                if (qybh_yc == "")
                    qybh_yc = qybh;
                string gcbh = "";
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
                {
                    //通过登录账号获取工程编号
                    IList<IDictionary<string, string>> dtgc = new List<IDictionary<string, string>>();
                    string sqlgc = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'";
                    dtgc = CommonService.GetDataTable(sqlgc);
                    gcbh = dtgc[0]["jdzch"].ToString();
                }

                string where = " where 1=1";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                if (jd != "")
                    where += " and A.szjd='" + jd + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";
                if (gcmc != "")
                    where += " and A.gcmc like '%" + gcmc + "%'";
                if (gclx!="")
                    where += " and A.gclxbh = '" + gclx + "'";
                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh_yc != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh_yc + "')) ";
                        where += "OR (A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh_yc + "')) ";
                        where += "OR (A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh_yc + "')) ";
                        where += "OR (A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh_yc + "')) ";
                        where += "OR (A.GCBH_YC IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh_yc + "')) ";
                        where += " ) ";
                    }
                }
                string day = DateTime.Today.ToString("yyyy-MM-dd");

                string sql = "select A.*,ISNULL(B.ZCRS,0) AS ZCRS,ISNULL(C.ZZRS,0) AS ZZRS,ISNULL(D.LZRS,0) AS LZRS,ISNULL(E.KQRS,0) AS KQRS,ISNULL(F.ZXKQ,0) AS ZXKQ,ISNULL(G.KQJS,0) AS KQJS from View_I_M_GC A ";
                sql += "LEFT JOIN (SELECT JDZCH,count(1) AS ZCRS FROM I_M_WGRY GROUP BY JDZCH) B ON A.GCBH = B.JDZCH ";
                sql += "LEFT JOIN (SELECT JDZCH,count(1) AS ZZRS FROM I_M_WGRY WHERE hasdelete = 0 GROUP BY JDZCH) C ON A.GCBH = C.JDZCH ";
                sql += "LEFT JOIN (SELECT JDZCH,count(1) AS LZRS FROM I_M_WGRY WHERE hasdelete != 0 GROUP BY JDZCH) D ON A.GCBH = D.JDZCH ";
                sql += "LEFT JOIN (select gcbh, inrynum as KQRS from i_m_gc) E ON A.GCBH = E.gcbh ";
                //sql += "LEFT JOIN (SELECT A.ProjectId,count(1) AS KQRS FROM KqjUserDayLog A LEFT JOIN KqjUserDayLogDetail B ON A.RECID = B.ParentId WHERE B.OutTime IS NULL AND A.LogDay = '" + day +"' GROUP BY A.ProjectId) E ON A.GCBH = E.ProjectId ";
                sql += "LEFT JOIN (SELECT JDZCH,count(1) AS ZXKQ FROM View_I_M_KQJ WHERE LastUpdate >=  '" + day + "' GROUP BY JDZCH) F ON A.GCBH = F.JDZCH ";
                sql += "LEFT JOIN (SELECT JDZCH,count(1) AS KQJS FROM View_I_M_KQJ GROUP BY JDZCH) G ON A.GCBH = G.JDZCH ";
                sql += where;

                dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);


            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                //Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", "0", "", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 获取企业一览
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetQyList()
        {
            int totalcount = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string qymc = Request["qymc"].GetSafeString();
                string pagesize = Request["limit"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["page"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }

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
                if (qybh != "")
                    where += " and b.qybh='" + qybh + "'";
                if (qymc != "")
                    where += " and b.qymc  like '%" + qymc + "%'";

                string sql = "select distinct b.*,c.Lat,c.Lon,d.lxmc from( ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,b.QYBH from I_M_GC A left join I_S_GC_SGDW b on a.GCBH = b.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,c.QYBH  from I_M_GC A left join I_S_GC_JLDW c on a.GCBH = c.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,d.QYBH  from I_M_GC A left join I_S_GC_JSDW d on a.GCBH = d.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,e.QYBH  from I_M_GC A left join I_S_GC_SJDW e on a.GCBH = e.gcbh ";
                sql += "union all ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,f.QYBH  from I_M_GC A left join I_S_GC_KCDW f on a.GCBH = f.gcbh ";
                sql += ") a ";
                sql += "left join I_M_QY b on a.QYBH = b.QYBH ";
                sql += "LEFT JOIN dbo.I_M_QY_JWD c ON b.QYBH = c.QYBH  ";
                sql += "left join H_QYLX d on b.lxbh = d.LXBH  ";
                sql += where;

                dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", "0", "", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 获取人员一览
        /// </summary>
        /// <returns></returns>
        public void GetRyList()
        {
            int totalcount = 0;

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
                string gcmc = Request["gcmc"].GetSafeString();
                string key = Request["key"].GetSafeString();

                string gcbh = Request["gcbh"].GetSafeString();

                string pagesize = Request["limit"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["page"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
                {
                    //通过登录账号获取工程编号
                    IList<IDictionary<string, string>> dtgc = new List<IDictionary<string, string>>();
                    string sqlgc = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'";
                    dtgc = CommonService.GetDataTable(sqlgc);
                    gcbh = dtgc[0]["jdzch"].ToString();
                }

                string where = " where  B.Recid IS NOT NULL";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                if (jd != "")
                    where += " and A.szjd='" + jd + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (gcmc != "")
                    where += " and A.gcmc like '%" + gcmc + "%'";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";
                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "')) ";
                        where += " ) ";
                    }
                }
                if (key != "")
                    where += " and (b.sfzhm like '%" + key + "%' or  b.ryxm like '%" + key + "%')";

                string sql = "select A.GCBH,(c.Lon + ',' + c.Lat) as GCBZ,B.* from I_M_GC A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH left join I_M_GC_JWD c on a.gcbh = c.JDZCH";
                sql += where;

                dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", "0", "", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 八块内容取得
        /// </summary>
        /// <returns></returns>
        public void GetStatistics()
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
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();

                string gcbh = Request["gcbh"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
                {
                    //通过登录账号获取工程编号
                    IList<IDictionary<string, string>> dtgc = new List<IDictionary<string, string>>();
                    string sqlgc = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'";
                    dtgc = CommonService.GetDataTable(sqlgc);
                    gcbh = dtgc[0]["jdzch"].ToString();
                }

                string where = " where 1=1";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                //else
                //{
                //    where +=
                //}
                if (jd != "")
                    where += " and A.szjd='" + jd + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";
                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "')) ";
                        where += " ) ";
                    }
                }

                //总工程数
                string sql = "select count(1) as num from I_M_GC A " + where;
                IList<IDictionary<string, string>> dt_zgcs = new List<IDictionary<string, string>>();
                dt_zgcs = CommonService.GetDataTable(sql);
                string zgcs = "0";
                if (null != dt_zgcs && dt_zgcs.Count != 0)
                    zgcs = dt_zgcs[0]["num"];

                //在建工程数
                sql = "select count(1) as num from I_M_GC A " + where + " and A.gczt='1'";
                IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zjgcs = "0";
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    zjgcs = dt_zjgcs[0]["num"];

                //在册人数
                sql = "select count(1) as num from ";
                sql += "(select distinct b.sfzhm from I_M_GC A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH " + where + " and B.Recid IS NOT NULL) T";
                IList<IDictionary<string, string>> dt_zcry = new List<IDictionary<string, string>>();
                dt_zcry = CommonService.GetDataTable(sql);
                string zcry = "0";
                if (null != dt_zcry && dt_zcry.Count != 0)
                    zcry = dt_zcry[0]["num"];

                //在岗人数(在职)
                sql = "select count(1) as num from ";
                sql += "(select distinct b.sfzhm from I_M_GC A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH " + where + " and B.hasdelete = '0') T";
                IList<IDictionary<string, string>> dt_zgry = new List<IDictionary<string, string>>();
                dt_zgry = CommonService.GetDataTable(sql);
                string zgry = "0";
                if (null != dt_zgry && dt_zgry.Count != 0)
                    zgry = dt_zgry[0]["num"];

                //当前人员
                string day = DateTime.Today.ToString("yyyy-MM-dd");
                //sql = "select count(1) as num from ";
                //sql += "(select distinct e.UserId from I_M_GC A ";
                //sql += "LEFT JOIN (SELECT A.Projec tId,count(1) AS KQRS FROM KqjUserDayLog A LEFT JOIN KqjUserDayLogDetail B ON A.RECID = B.ParentId WHERE B.OutTime IS NULL AND A.LogDay = '" + day + "' GROUP BY A.ProjectId) E ON A.GCBH = E.ProjectId";
                //sql += where + " and E.ProjectId is not null) T";
               
                //sql = "select kqrs as num from I_M_GC A ";
                //sql += "LEFT JOIN (SELECT A.ProjectId,count(1) AS KQRS FROM KqjUserDayLog A LEFT JOIN KqjUserDayLogDetail B ON A.RECID = B.ParentId WHERE B.OutTime IS NULL AND A.LogDay = '" + day + "' GROUP BY A.ProjectId) E ON A.GCBH = E.ProjectId";
                //sql += where + " and E.ProjectId is not null";

                sql = "select sum(inrynum) as num from i_m_gc A ";
                sql += where;

                IList<IDictionary<string, string>> dt_dqry = new List<IDictionary<string, string>>();
                dt_dqry = CommonService.GetDataTable(sql);
                string dqry = "0";
                if (null != dt_dqry && dt_dqry.Count != 0)
                    dqry = dt_dqry[0]["num"];

                //计划金额
                sql = "select sum(cast(isnull(gczj,'0') as int)) as gczj from I_M_GC A " + where;
                IList<IDictionary<string, string>> dt_gczj = new List<IDictionary<string, string>>();
                dt_gczj = CommonService.GetDataTable(sql);
                string gczj = "0";
                if (null != dt_gczj && dt_gczj.Count != 0 && dt_gczj[0]["gczj"] != "")
                    gczj = dt_gczj[0]["gczj"];
                gczj = (Math.Round(int.Parse(gczj) * 1.0 / 10000)).ToString();
               
                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("zgcs", zgcs);
                di.Add("zjgcs", zjgcs);
                di.Add("zcry", zcry);
                di.Add("zgry", zgry);
                di.Add("dqry", dqry);
                di.Add("jhje", gczj);
                di.Add("dwje", "0");
                di.Add("ffje", "0");
              

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
        /// 企业类别统计数获取
        /// </summary>
        /// <returns></returns>
        public void GetStatisticsQylb()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }

                string where = " where b.QYBH is not null";
                if (province != "")
                    where += " and a.szsf='" + province + "'";
                if (city != "")
                    where += " and a.szcs='" + city + "'";
                if (district != "")
                    where += " and a.szxq='" + district + "'";
                if (qylx != "")
                    where += " and b.lxbh='" + qylx + "'";
                if (qybh != "")
                    where += " and b.qybh='" + qybh + "'";

                string sql = "select  t.lxmc as name,count(1) as value  from ( ";
                sql += "select distinct d.lxmc,b.QYBH ";
                sql += "from(  ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,b.QYBH from I_M_GC A left join I_S_GC_SGDW b on a.GCBH_YC = b.gcbh union all  ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,c.QYBH  from I_M_GC A left join I_S_GC_JLDW c on a.GCBH_YC = c.gcbh union all  ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,d.QYBH  from I_M_GC A left join I_S_GC_JSDW d on a.GCBH_YC = d.gcbh union all  ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,e.QYBH  from I_M_GC A left join I_S_GC_SJDW e on a.GCBH_YC = e.gcbh union all  ";
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,f.QYBH  from I_M_GC A left join I_S_GC_KCDW f on a.GCBH_YC = f.gcbh )  ";
                sql += "a  ";
                sql += "left join I_M_QY b on a.QYBH = b.QYBH  ";
                sql += "LEFT JOIN dbo.I_M_QY_JWD c ON b.QYBH = c.QYBH  ";
                sql += "left join H_QYLX d on b.lxbh = d.LXBH   ";
                sql += where;
                sql += ") T ";
                sql += "group by t.lxmc ";

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
        /// 工程类别统计数获取
        /// </summary>
        /// <returns></returns>
        public void GetStatisticsGclb()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }
                string gcbh = "";
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
                {
                    //通过登录账号获取工程编号
                    IList<IDictionary<string, string>> dtgc = new List<IDictionary<string, string>>();
                    string sqlgc = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'";
                    dtgc = CommonService.GetDataTable(sqlgc);
                    gcbh = dtgc[0]["jdzch"].ToString();
                }

                string where = " where 1=1";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";
                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "')) ";
                        where += " ) ";
                    }
                }
                string day = DateTime.Today.ToString("yyyy-MM-dd");

                string sql = "select A.gclxbh as name,count(1) as value from I_M_GC A ";
                sql += where + " group by  A.gclxbh";

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
        /// 获取人员构成
        /// </summary>
        /// <returns></returns>
        public void GetRyGc()
        {
            string msg = "";
            bool code = true;

            int mancount = 0, womancount = 0, mancountsmall = 0, mancountmiddle = 0, mancountlager = 0, womancountsmall = 0, womancountmiddle = 0, womancountlager = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();

                string pagesize = Request["limit"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["page"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
                {
                    //通过登录账号获取工程编号
                    IList<IDictionary<string, string>> dtgc = new List<IDictionary<string, string>>();
                    string sqlgc = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'";
                    dtgc = CommonService.GetDataTable(sqlgc);
                    gcbh = dtgc[0]["jdzch"].ToString();
                }

                string where = " where  B.Recid IS NOT NULL";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (gcmc != "")
                    where += " and A.gcmc like '%" + gcmc + "%'";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";
                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "')) ";
                        where += " ) ";
                    }
                }

                //男性人数
                //男性30岁以下
                //男性30-60岁
                //男性60岁以上

                //女性人数
                //女性30岁以下
                //女性30-60岁
                //女性60岁以上
                string sql = "SELECT count(1) AS MANCOUNT FROM ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '男' ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                mancount = int.Parse(dt[0]["mancount"].ToString());

                sql = "SELECT count(1) AS WOMANCOUNT FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '女' ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                womancount = int.Parse(dt[0]["womancount"].ToString());

                sql = "SELECT count(1) AS MANCOUNTSMALL FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ,DateName(year,GetDate()) - convert(int,Substring(B.CSRQ,1,4)) AS NL FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '男' AND  NL < 30 ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                mancountsmall = int.Parse(dt[0]["mancountsmall"].ToString());

                sql = "SELECT count(1) AS MANCOUNTMIDDLE FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ,DateName(year,GetDate()) - convert(int,Substring(B.CSRQ,1,4)) AS NL FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '男' AND  NL >= 30 AND  NL <= 60 ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                mancountmiddle = int.Parse(dt[0]["mancountmiddle"].ToString());

                sql = "SELECT count(1) AS MANCOUNTLAGER FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ,DateName(year,GetDate()) - convert(int,Substring(B.CSRQ,1,4)) AS NL FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '男' AND  NL > 60 ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                mancountlager = int.Parse(dt[0]["mancountlager"].ToString());

                sql = "SELECT count(1) AS WOMANCOUNTSMALL FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ,DateName(year,GetDate()) - convert(int,Substring(B.CSRQ,1,4)) AS NL FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '女' AND  NL < 30 ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                womancountsmall = int.Parse(dt[0]["womancountsmall"].ToString());

                sql = "SELECT count(1) AS WOMANCOUNTMIDDLE FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ,DateName(year,GetDate()) - convert(int,Substring(B.CSRQ,1,4)) AS NL FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '女' AND  NL >= 30 AND  NL <= 60 ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                womancountmiddle = int.Parse(dt[0]["womancountmiddle"].ToString());

                sql = "SELECT count(1) AS WOMANCOUNTLAGER FROM  ";
                sql += "( ";
                sql += "SELECT DISTINCT B.RYXM,B.SFZHM,B.XB,B.CSRQ,DateName(year,GetDate()) - convert(int,Substring(B.CSRQ,1,4)) AS NL FROM I_M_GC A LEFT JOIN I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where;
                sql += ") T ";
                sql += "WHERE XB = '女' AND  NL > 60 ";

                dt = new List<IDictionary<string, string>>();
                dt = CommonService.GetDataTable(sql);
                womancountlager = int.Parse(dt[0]["womancountlager"].ToString());
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                string data = string.Format("{{\"mancount\":\"{0}\",\"womancount\":\"{1}\",\"mancountsmall\":\"{2}\",\"mancountmiddle\":\"{3}\",\"mancountlager\":\"{4}\",\"womancountsmall\":\"{5}\",\"womancountmiddle\":{6},\"womancountlager\":{7}}}", mancount, womancount, mancountsmall, mancountmiddle, mancountlager, womancountsmall, womancountmiddle, womancountlager);
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, data));

                //Response.Write(string.Format("{{\"mancount\":\"{0}\",\"womancount\":\"{1}\",\"mancountsmall\":\"{2}\",\"mancountmiddle\":\"{3}\",\"mancountlager\":\"{4}\",\"womancountsmall\":\"{5}\",\"womancountmiddle\":{6},\"womancountlager\":{7}}}", mancount, womancount, mancountsmall, mancountmiddle, mancountlager, womancountsmall, womancountmiddle, womancountlager));
                Response.End();
            }
        }


        /// <summary>
        /// 工种类别统计数获取
        /// </summary>
        /// <returns></returns>
        public void GetStatisticsGz()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();

                string gcbh = Request["gcbh"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }
                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001")) //劳务部门
                {
                    //通过登录账号获取工程编号
                    IList<IDictionary<string, string>> dtgc = new List<IDictionary<string, string>>();
                    string sqlgc = "SELECT TOP 1 jdzch,gcmc FROM I_M_LZZGY_ZH WHERE ZH =  '" + CurrentUser.RealUserName + "'";
                    dtgc = CommonService.GetDataTable(sqlgc);
                    gcbh = dtgc[0]["jdzch"].ToString();
                }

                string where = " where  B.Recid IS NOT NULL";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";

                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "')) ";
                        where += " ) ";
                    }
                }


                string sql = "select b.gz as name,count(1) as value from I_M_GC A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where + " group by  b.gz";
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
        /// 籍贯统计数获取
        /// </summary>
        /// <returns></returns>
        public void GetStatisticsJg()
        {
            string msg = "";
            bool code = true;

            //取数据用
            IList<IDictionary<string, string>> dtdata = new List<IDictionary<string, string>>();
            //data数据组装用
            IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt4 = new List<IDictionary<string, string>>();
            //最终数据
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }

                if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                {
                    //通过登录账号获取企业编号
                    IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                    string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                    dtt = CommonService.GetDataTable(sqlqybh);
                    qybh = dtt[0]["qybh"].ToString();
                }
                string gczt = Request["gczt"].GetSafeString();

                string where = " where  B.Recid IS NOT NULL";
                if (province != "")
                    where += " and A.szsf='" + province + "'";
                if (city != "")
                    where += " and A.szcs='" + city + "'";
                if (district != "")
                    where += " and A.szxq='" + district + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";

                if (qylx != "")
                {
                    if (qylx == QYLX_SGQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JLQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_JSQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_SJQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "') ";
                    }
                    if (qylx == QYLX_KCQY)
                    {
                        if (qybh == "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW) ";
                        if (qybh != "")
                            where += " AND A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "') ";
                    }
                }
                else
                {
                    if (qybh != "")
                    {
                        where += " AND ( ";
                        where += "(A.GCBH IN (SELECT GCBH FROM I_S_GC_KCDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JLDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_JSDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SJDW WHERE QYBH = '" + qybh + "')) ";
                        where += "OR (A.GCBH IN (SELECT GCBH FROM I_S_GC_SGDW WHERE QYBH = '" + qybh + "')) ";
                        where += " ) ";
                    }
                }

                string sql = "select left(B.SFZDZ,2) as name,count(1) as value from I_M_GC A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH ";
                sql += where + " group by left(B.SFZDZ,2) order by count(1) ";
                dtdata = CommonService.GetDataTable(sql);

                if (null == dtdata || dtdata.Count == 0) {
                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di = new Dictionary<string, string>();
                    di.Add("name", "0-0");
                    di.Add("data", null);
                    dt.Add(di);

                    di = new Dictionary<string, string>();
                    di.Add("name", "0-0");
                    di.Add("data", null);
                    dt.Add(di);

                    di = new Dictionary<string, string>();
                    di.Add("name", "0-0");
                    di.Add("data", null);
                    dt.Add(di);

                    di = new Dictionary<string, string>();
                    di.Add("name", "0-0");
                    di.Add("data", null);
                    dt.Add(di);
                }
                else
                {
                    //间隔
                    int maxValue = int.Parse(dtdata[dtdata.Count - 1]["value"].ToString());
                    int spaceValue = maxValue / 4;

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = 10240000;
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    IDictionary<string, string> di = new Dictionary<string, string>();
                    //data数据组装用
                    IDictionary<string, string> didata = new Dictionary<string, string>();

                    int vaule = 0;
                    di = new Dictionary<string, string>();
                    for (int r = 0; r < dtdata.Count; r++)
                    {
                        vaule = int.Parse(dtdata[r]["value"].ToString());
                        if (vaule < spaceValue)
                        {
                            didata = new Dictionary<string, string>();
                            didata.Add("name", getProvince(dtdata[r]["name"].ToString()));
                            didata.Add("value", dtdata[r]["value"].ToString());
                            dt1.Add(didata);
                        }
                        else if (vaule >= spaceValue && vaule < (2 * spaceValue))
                        {
                            didata = new Dictionary<string, string>();
                            didata.Add("name", getProvince(dtdata[r]["name"].ToString()));
                            didata.Add("value", dtdata[r]["value"].ToString());
                            dt2.Add(didata);
                        }
                        else if (vaule >= (2 * spaceValue) && vaule < (3 * spaceValue))
                        {
                            didata = new Dictionary<string, string>();
                            didata.Add("name", getProvince(dtdata[r]["name"].ToString()));
                            didata.Add("value", dtdata[r]["value"].ToString());
                            dt3.Add(didata);
                        }
                        else if (vaule >= (3 * spaceValue))
                        {
                            didata = new Dictionary<string, string>();
                            didata.Add("name", getProvince(dtdata[r]["name"].ToString()));
                            didata.Add("value", dtdata[r]["value"].ToString());
                            dt4.Add(didata);
                        }
                    }

                    di = new Dictionary<string, string>();
                    di.Add("name", "0-" + (spaceValue - 1).ToString());
                    di.Add("data", jss.Serialize(dt1));
                    dt.Add(di);

                    di = new Dictionary<string, string>();
                    di.Add("name", spaceValue.ToString() + "-" + (2 * spaceValue - 1).ToString());
                    di.Add("data", jss.Serialize(dt2));
                    dt.Add(di);

                    di = new Dictionary<string, string>();
                    di.Add("name", (2 * spaceValue).ToString() + "-" + (3 * spaceValue - 1).ToString());
                    di.Add("data", jss.Serialize(dt3));
                    dt.Add(di);

                    di = new Dictionary<string, string>();
                    di.Add("name", (3 * spaceValue).ToString() + "-" + maxValue.ToString());
                    di.Add("data", jss.Serialize(dt4));
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

        /// <summary>
        /// 工程头部滚动内容
        /// </summary>
        /// <returns></returns>
        public void getdowebok()
        {
            string msg = "";
            bool code = true;

            string dowebok = "";

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string gcbh = Request["gcbh"].GetSafeString();

                string where = " where F.Recid is not null";
                if (gcbh != "")
                    where += " and A.gcbh='" + gcbh + "'";

                string day = DateTime.Today.ToString("yyyy-MM-dd");

                string sql = "";
                sql += "select F.gz,COUNT(1) AS num from I_M_GC A ";
                sql += "LEFT JOIN ( ";
                sql += "SELECT A.* ";
                sql += "FROM KqjUserDayLog A  ";
                sql += "LEFT JOIN KqjUserDayLogDetail B ON A.RECID = B.ParentId  ";
                sql += "WHERE B.OutTime IS NULL AND A.LogDay = '" + day + "' ) ";
                sql += "E ON A.GCBH = E.ProjectId ";
                sql += "LEFT JOIN I_M_WGRY F ON E.UserId = F.SFZHM and f.JDZCH = a.gcbh ";
                sql += where;
                sql += "GROUP BY F.GZ ";
                dt = CommonService.GetDataTable(sql);

                dowebok = "";
                int allRy = 0;
                for (int r = 0; r < dt.Count; r++)
                {
                    allRy += int.Parse(dt[r]["num"].ToString());
                    dowebok += "其中" + dt[r]["gz"].ToString() + dt[r]["num"].ToString() + "名，";
                }
                dowebok = "工地当前总共" + allRy.ToString() + "人," + dowebok;
                dowebok = dowebok.Substring(0, dowebok.Length - 1) + "。";

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

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", dowebok, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 通过人员编号(或者微信ID)，工程编号，年，月获取人员基本信息，注册时间，每月考勤工数，每月收到的工资记录，上访记录，有无黑名单，有无证书
        /// </summary>
        /// <returns></returns>
        public void GetRyXx()
        {
            string msg = "";
            bool code = true;
            int count = 0;

            IList<IDictionary<string, string>> dtjbxx = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dtlist = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dtrygcbh = new List<IDictionary<string, string>>();

            try
            {
                string wxkey = Request["wxkey"].GetSafeString();     //不传的话根据身份证号
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();

                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                if ((wxkey == "" && sfzhm == "") || gcbh == "")
                {
                    msg = "请检查参数！";
                    code = false;
                }

                if (code && (wxkey != "" && sfzhm != ""))
                {
                    msg = "请检查参数！";
                    code = false;
                }

                if (code && wxkey != "")
                {
                    IList<IDictionary<string, string>> sfzdatas = new List<IDictionary<string, string>>();
                    string sql = "select sfzhm from i_m_ry_info where wxkey='" + wxkey + "'";
                    sfzdatas = CommonService.GetDataTable(sql);
                    if (sfzdatas.Count > 0)
                    {
                        sfzhm = sfzdatas[0]["sfzhm"];
                    }
                    else
                    {
                        msg = "当前微信没有绑定账号";
                        code = false;
                    }
                }

                if (code && sfzhm == "")
                {
                    string err = "";
                    string username = Request["login_name"].GetSafeString();
                    string password = Request["login_pwd"].GetSafeString();

                    if (!CurrentUser.IsLogin)
                    {
                        if (!Remote.UserService.Login(username, password, out err)) {
                            msg = "请重新登录！";
                            code = false;
                        }
                    }
                }

                if (code)
                {
                    string where = "WHERE 1=1 ";
                    where += "AND A.SFZHM = '" + sfzhm + "'";
                    where += "AND A.JDZCH = '" + gcbh + "'";

                    string sql = "SELECT A.RYXM,A.SFZHM,A.GCMC,A.XB,A.GZ,A.GW,A.SFBZFZR,D.BZFZR AS BZFZR,A.CSRQ,A.SFZDZ,A.SJHM,DateName(year,GetDate()) - convert(int,Substring(A.CSRQ,1,4)) AS NL, ";
                    sql += "CASE A.hasdelete WHEN 0 THEN '在职' ELSE '离职' END AS RYZT,convert(varchar(10),A.LRSJ,120) AS ZCSJ, ";
                    sql += "'无' AS SFJL,'无' AS YWHMD,'无' AS YWZS ";
                    sql += "FROM I_M_WGRY A ";
                    sql += "LEFT JOIN I_M_GC B ON A.JDZCH = B.GCBH ";
                    sql += "LEFT JOIN (SELECT DISTINCT SFZHM,RYXM AS BZFZR FROM I_M_WGRY WHERE SFBZFZR = '是' ) D ON A.BZFZR = D.SFZHM ";
                    sql += where;

                    dtjbxx = CommonService.GetDataTable(sql);

                    sql = "SELECT C.LogYear,C.LogMonth,C.workday AS KQGS,ISNULL(C.BankPay,0) AS GZJL ";
                    sql += "FROM I_M_WGRY A ";
                    sql += "LEFT JOIN I_M_GC B ON A.JDZCH = B.GCBH ";
                    sql += "LEFT JOIN KqjUserMonthPay C ON A.SFZHM = C.UserId AND A.JDZCH = C.JDZCH ";
                    sql += "LEFT JOIN (SELECT DISTINCT SFZHM,RYXM AS BZFZR FROM I_M_WGRY WHERE SFBZFZR = '是' ) D ON A.BZFZR = D.SFZHM ";
                    sql += where;
                    dtlist = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out count);

                    sql = "SELECT distinct UserId as sfzhm,JDZCH as gcbh FROM KqjUserMonthPay where UserId =  '" + sfzhm + "'" ;
                    dtrygcbh = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out count);

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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3},\"datalist\":{4},\"dtrygcbh\":{5}}}", code, msg, count, jss.Serialize(dtjbxx), jss.Serialize(dtlist), jss.Serialize(dtrygcbh)));
                Response.End();
            }
        }

        /// <summary>
        /// 通过工程编号获取工程基本信息，每月工人的工数，每月发放工资，有无欠薪，有无设备异常
        /// </summary>
        /// <returns></returns>
        public void GetGcXx()
        {
            int count = 0;
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dtjbxx = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dtlist = new List<IDictionary<string, string>>();

            try
            {
                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    if (!Remote.UserService.Login(username, password, out err))
                    {
                        msg = "请重新登录！";
                        code = false;
                    }
                }

                string gcbh = Request["gcbh"].GetSafeString();

                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                if (code && (gcbh == ""))
                {
                    msg = "请检查参数！";
                    code = false;
                }

                if (code)
                {
                    string where = "WHERE 1=1 ";
                    where += "AND A.GCBH = '" + gcbh + "'";

                    string sql = "SELECT A.GCMC,A.GCLXBH,A.SY_JSDWMC,A.SGDWMC,A.JLDWMC,A.KCDWMC,A.SJDWMC,A.GCZJ,A.JZMJ,A.JHKGRQ,A.JHJGRQ,'无' as ywqx,'无' as ywsbyc  ";
                    sql += "FROM VIEW_I_M_GC A ";
                    sql += where;
                    dtjbxx = CommonService.GetDataTable(sql);

                    sql = "SELECT  ";
                    sql += "B.LogYear,B.LogMonth,isnull(B.monthworkday,0) as monthworkday,isnull(B.monthbankpay,0) as monthbankpay ";
                    sql += "FROM VIEW_I_M_GC A ";
                    sql += "LEFT JOIN (SELECT JDZCH,LogYear,LogMonth,SUM(ISNULL(workday,0)) AS monthworkday ,SUM(ISNULL(BankPay,0)) monthbankpay  ";
                    sql += "FROM KqjUserMonthPay GROUP BY JDZCH,LogYear,LogMonth) AS B ON A.GCBH = B.JDZCH ";
                    sql += where;
                    sql += " order by B.LogYear desc ,B.LogMonth desc";
                    dtlist = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out count);

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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3},\"datalist\":{4}}}", code, msg, count, jss.Serialize(dtjbxx), jss.Serialize(dtlist)));
                Response.End();
            }
        }

        /// <summary>
        /// 通过工程编号获取工程基本信息，每月工人的工数，每月发放工资，有无欠薪，有无设备异常
        /// </summary>
        /// <returns></returns>
        public void GetQyXx()
        {
            string msg = "";
            bool code = true;
            int count = 0;

            IList<IDictionary<string, string>> dtjbxx = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dtlist = new List<IDictionary<string, string>>();

            try
            {
                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    if (!Remote.UserService.Login(username, password, out err))
                    {
                        msg = "请重新登录！";
                        code = false;
                    }
                }

                string qybh = Request["qybh"].GetSafeString();


                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                if (code && (qybh == ""))
                {
                    msg = "请检查参数！";
                    code = false;
                }

                if (code)
                {
                    string where = "WHERE 1=1 ";
                    where += "AND A.QYBH = '" + qybh + "'";

                    string sql = " SELECT A.QYMC,A.QYBH,A.LXMC,A.QYFR,A.QYFZR,A.LXDH,convert(varchar(10),A.LRSJ,120) AS ZCSJ,'无' as ywts ";
                    sql += "FROM VIEW_I_M_QY A  ";
                    sql += where;
                    dtjbxx = CommonService.GetDataTable(sql);

                    sql = " SELECT  ";
                    sql += "B.LogYear,B.LogMonth,isnull(B.monthbankpay,0) as monthbankpay ";
                    sql += "FROM VIEW_I_M_QY A  ";
                    sql += "LEFT JOIN (SELECT QYBH,LogYear,LogMonth,SUM(ISNULL(workday,0)) AS monthworkday ,SUM(ISNULL(BankPay,0)) monthbankpay  ";
                    sql += "FROM KqjUserMonthPay GROUP BY QYBH,LogYear,LogMonth) AS B ON A.QYBH = B.QYBH ";

                    sql += where;
                    dtlist = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out count);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3},\"datalist\":{4}}}", code, msg, count, jss.Serialize(dtjbxx), jss.Serialize(dtlist)));
                Response.End();
            }
        }


        private string getProvince(string name)
        {
            for (int i = 0; i < m_strProvince.Count(); i++)
            {
                if (m_strProvince[i].ToString().Contains(name))
                    return m_strProvince[i].ToString();
            }
            return "";
        }
        /// <summary>
        /// 获取省列表
        /// </summary>
        public void GetProvinceList()
        {
            try
            {
                string sql = "select distinct szsf,sfid from h_city ";
                if (!CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //非五方主体
                {
                    if (CurrentUser.UserName != "")
                        sql += " where szsf in (select szsf from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "')";
                }        
                sql += " order by sfid";
                IList<IDictionary<string, string>> plist=CommonService.GetDataTable(sql);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(plist));
            }
            catch(Exception e)
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
                string where = "";
                string sql_city = "select szcs from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>> citylist = CommonService.GetDataTable(sql_city);
                if(citylist.Count>0)
                {
                    if(string.IsNullOrEmpty(citylist[0]["szcs"])) //没有市，即表示所有市
                    {

                    }
                    else
                    {
                        if (!CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //非五方主体
                        {
                            if (CurrentUser.UserName != "")
                                where += " and szcs in (select szcs from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "')";
                        }
                      
                    }
                }

                string sql = "select distinct szcs from h_city where szsf='" + province + "'";
                sql += where;
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
                string sql_xq = "select szxq from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>> xqlist = CommonService.GetDataTable(sql_xq);
                if (xqlist.Count > 0)
                {
                    if (string.IsNullOrEmpty(xqlist[0]["szxq"])) //没有区，即表示所有区
                    {

                    }
                    else
                    {
                        if (!CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //非五方主体
                        {
                            if (CurrentUser.UserName != "")
                                where += " and szxq in (select szxq from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "')";
                        }
                       
                    }
                }

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
                string sql_jd = "select szjd from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>> jdlist = CommonService.GetDataTable(sql_jd);
                if (jdlist.Count > 0)
                {
                    if (string.IsNullOrEmpty(jdlist[0]["szjd"])) //没有区，即表示所有区
                    {

                    }
                    else
                    {
                        if (!CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //非五方主体
                        {
                            if (CurrentUser.UserName != "")
                                where += " and szjd in (select szjd from View_H_ZFZH_XQ where usercode='" + CurrentUser.UserName + "')";
                        }                      
                    }
                }

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
                string sql_jd = "select gclx from View_H_ZFZH_GCLX where usercode='" + CurrentUser.UserName + "'";
                IList<IDictionary<string, string>> gclxlist = CommonService.GetDataTable(sql_jd);
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                if (gclxlist.Count > 0)
                {
                    if (string.IsNullOrEmpty(gclxlist[0]["gclx"])) //没有区，即表示所有区
                    {
                        string sql = "select distinct lxmc from h_gclx";
                        list = CommonService.GetDataTable(sql);
                    }
                    else
                        list = gclxlist;
                }
                else
                {
                    string sql = "select distinct lxmc from h_gclx";
                    list = CommonService.GetDataTable(sql);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.Write(jss.Serialize(list));
            }
            catch (Exception e)
            { }
        }

        #endregion

    }
}
