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
    public class WgryController : Controller
    {
        #region 常量

        private readonly string QYLX_SGQY = "11";
        private readonly string QYLX_JLQY = "12";
        private readonly string QYLX_JSQY = "13";
        private readonly string QYLX_SJQY = "14";
        private readonly string QYLX_KCQY = "15";

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

        #region 数据取得

        public void GetQYGClist()
        {

        }

        public void GetGCRyList()
        {
            string msg = "";
            bool code = true;
            int totalcount = 0;
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string ryxm = Request["ryxm"].GetSafeString();

                if ( gcbh == "")
                {
                    msg = "请检查参数！";
                    code = false;
                }

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

                if (code)
                {

                    string day = DateTime.Today.ToString("yyyy-MM-dd");


                    string where = "WHERE 1=1 ";
                    where += "AND A.JDZCH = '" + gcbh + "'";
                    if(ryxm!="")
                    {
                        where += " and ryxm like '%" + ryxm + "%'";
                    }

                    string sql = "SELECT A.RYXM,A.SFZHM,A.GCMC,A.XB,A.GZ,A.GW,A.SFBZFZR,D.BZFZR AS BZFZR,A.CSRQ,DateName(year,GetDate()) - convert(int,Substring(A.CSRQ,1,4)) AS NL,  ";
                    sql += "CASE A.hasdelete WHEN 0 THEN '在职' ELSE '离职' END AS RYZT,convert(varchar(10),A.LRSJ,120) AS ZCSJ,  ";
                    sql += "'无' AS SFJL,'无' AS YWHMD,'无' AS YWZS ,isnull(E.kqcount,0) as kqcount,0 as blcount ";
                    sql += "FROM I_M_WGRY A  ";
                    sql += "LEFT JOIN I_M_GC B ON A.JDZCH = B.GCBH  ";
                    sql += "LEFT JOIN (SELECT DISTINCT SFZHM,RYXM AS BZFZR FROM I_M_WGRY WHERE SFBZFZR = '是' ) D ON A.BZFZR = D.SFZHM  ";
                    sql += "LEFT JOIN (SELECT count(1) AS kqcount, UserId FROM KqjUserLog WHERE Substring(CONVERT(varchar(100), LogDate, 120),1,10) = '" + day + "' group by UserId) E ON E.UserId = A.SFZHM ";
                    sql += where;

                    dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);

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
                Response.Write(JsonFormat.GetRetString(code, msg, totalcount, dt));
                Response.End();
            }
        }

        /// <summary>
        /// 通过人员编号，工程编号，年，月获取人员基本信息，今日考勤条数，不良行为条数，工作经历
        /// </summary>
        /// <returns></returns>
        public void GetRyXq()
        {
            string msg = "";
            bool code = true;
            int count = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();

                if ((sfzhm == "") || gcbh == "")
                {
                    msg = "请检查参数！";
                    code = false;
                }

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

                if (code)
                {

                    string day = DateTime.Today.ToString("yyyy-MM-dd");


                    string where = "WHERE 1=1 ";
                    where += "AND A.SFZHM = '" + sfzhm + "'";
                    where += "AND A.JDZCH = '" + gcbh + "'";

                    string sql = "SELECT A.RYXM,A.SFZHM,A.GCMC,A.XB,A.GZ,A.GW,A.SFBZFZR,D.BZFZR AS BZFZR,A.CSRQ,DateName(year,GetDate()) - convert(int,Substring(A.CSRQ,1,4)) AS NL,  ";
                    sql += "CASE A.hasdelete WHEN 0 THEN '在职' ELSE '离职' END AS RYZT,convert(varchar(10),A.LRSJ,120) AS ZCSJ,  ";
                    sql += "'无' AS SFJL,'无' AS YWHMD,'无' AS YWZS ,isnull(E.kqcount,0) as kqcount,0 as blcount ";
                    sql += "FROM I_M_WGRY A  ";
                    sql += "LEFT JOIN I_M_GC B ON A.JDZCH = B.GCBH  ";
                    sql += "LEFT JOIN (SELECT DISTINCT SFZHM,RYXM AS BZFZR FROM I_M_WGRY WHERE SFBZFZR = '是' ) D ON A.BZFZR = D.SFZHM  ";
                    sql += "LEFT JOIN (SELECT count(1) AS kqcount, UserId FROM KqjUserLog WHERE Substring(CONVERT(varchar(100), LogDate, 120),1,10) = '" + day + "' group by UserId) E ON E.UserId = A.SFZHM ";
                    sql += where;

                    dt = CommonService.GetDataTable(sql);

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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", code, msg, count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 通过人员编号，工程编号，获取工时工资
        /// </summary>
        /// <returns></returns>
        public void GetRyKqXq()
        {
            string msg = "";
            bool code = true;
            int count = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string daystart = Request["daystart"].GetSafeString();
                string dayend = Request["dayend"].GetSafeString();

                if (sfzhm == "" || gcbh == "")
                {
                    msg = "请检查参数！";
                    code = false;
                }

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

                if (code)
                {

                    string day = DateTime.Today.ToString("yyyy-MM-dd");

                    string where = "WHERE 1=1 ";
                    where += "AND UserId = '" + sfzhm + "'";
                    where += "AND PlaceId = '" + gcbh + "'";

                    //if (daystart == "" && dayend == "")
                    //{
                    //    daystart = day + " 00:00:00'";
                    //    dayend = day + " 23:59:59'";
                    //}
                    if (daystart != "")
                    {
                        where += "AND LogDate > '" + daystart + " 00:00:00'";
                    }

                    if (dayend != "")
                    {
                        where += "AND LogDate < '" + dayend + " 23:59:59'";
                    }

                    string sql = "SELECT UserId,LogDate FROM KqjUserLog  ";
                    sql += where;
                    sql += "order by LogDate desc";

                    dt = CommonService.GetDataTable(sql);

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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", code, msg, count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 通过人员编号，工程编号，年，月获取今日考勤
        /// </summary>
        /// <returns></returns>
        public void GetYj()
        {
            string msg = "";
            bool code = true;

            int qycount = 0, gccount = 0, rycount = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string type = Request["type"].GetSafeString();
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
                if (code)
                {
                    string sql = "select count(*) as xzyjnum from INFO_YJ_XZ where xzdw=0 or xzze=0";
                    IList<IDictionary<string, string>> xzyj = CommonService.GetDataTable(sql);
                    if (xzyj.Count > 0)
                    {
                        gccount = xzyj[0]["xzyjnum"].GetSafeInt();
                    }
                    sql = "select count(*) as ryyjnum from ViewKqjUserMonthPay where TX_YJZT=1 or FF_YJZT=1";
                    IList<IDictionary<string, string>> ryyj=CommonService.GetDataTable(sql);
                    if(ryyj.Count>0)
                    {
                        rycount = ryyj[0]["ryyjnum"].GetSafeInt();
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                string data = string.Format("[{{\"qycount\":\"{0}\",\"gccount\":\"{1}\",\"rycount\":\"{2}\"}}]", qycount, gccount, rycount);
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, qycount + gccount + rycount, data));
                Response.End();
            }
        }

        /// <summary>
        /// 获取工程一览
        /// </summary>
        /// <returns></returns>
        public void GetGcList()
        {
            string msg = "";
            bool code = true;
            int totalcount = 0;

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
                string gcmc = Request["gcmc"].GetSafeString();
                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    if (!Remote.UserService.Login(username, password, out err))
                    {
                        code = false;
                        msg = "登录失败,请重新登录!";
                    }
                }

                if (code)
                {

                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002") && qybh == "") //五方主体
                    {
                        //通过登录账号获取企业编号
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                        dtt = CommonService.GetDataTable(sqlqybh);
                        qybh = dtt[0]["qybh"].ToString();
                    }
                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001") && gcbh == "") //劳务部门
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
                    if (gcmc != "")
                        where += " and A.gcmc like '%" + gcmc + "%'";
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

                    string sql = "select A.GCBH,A.GCMC,B.* from I_M_GC A  ";
                    sql += "LEFT JOIN (SELECT JDZCH,SUM(ISNULL(WORKDAY,0)) AS WORKDAY, SUM(ISNULL(BANKPAY,0)) AS BANKPAY FROM KqjUserMonthPay GROUP BY JDZCH)  ";
                    sql += "B ON A.GCBH = B.JDZCH  ";
                    sql += where;
                    sql += " order by A.GCBH";

                    dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(dt)));

                Response.End();
            }
        }

        /// <summary>
        /// 获取工程一览每年汇总
        /// </summary>
        /// <returns></returns>
        public void GetGcYearPay()
        {
            string msg = "";
            bool code = true;
            int totalcount = 0;

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
                string gcmc = Request["gcmc"].GetSafeString();
                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                string startYear = Request["startYear"].GetSafeString();
                string endYear = Request["endYear"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    if (!Remote.UserService.Login(username, password, out err))
                    {
                        code = false;
                        msg = "登录失败,请重新登录!";
                    }
                }

                if (code)
                {

                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002") && qybh == "") //五方主体
                    {
                        //通过登录账号获取企业编号
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                        dtt = CommonService.GetDataTable(sqlqybh);
                        qybh = dtt[0]["qybh"].ToString();
                    }
                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001") && gcbh == "") //劳务部门
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
                    if (gcmc != "")
                        where += " and A.gcmc like '%" + gcmc + "%'";
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
                    if (startYear != "")
                        where += " and B.LogYear>='" + startYear + "'";
                    if (endYear != "")
                        where += " and B.LogYear<='" + endYear + "'";

                    string sql = "select A.GCBH,A.GCMC,B.* from I_M_GC A  ";
                    sql += "LEFT JOIN (SELECT JDZCH,LogYear,SUM(ISNULL(WORKDAY,0)) AS WORKDAY, SUM(ISNULL(BANKPAY,0)) AS BANKPAY FROM KqjUserMonthPay GROUP BY JDZCH,LogYear)  ";
                    sql += "B ON A.GCBH = B.JDZCH  ";
                    sql += where;
                    sql += " order by A.GCBH,B.LogYear desc";

                    dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(dt)));

                Response.End();
            }
        }

        /// <summary>
        /// 获取工程一览每月详细
        /// </summary>
        /// <returns></returns>
        public void GetGcMonthPay()
        {
            string msg = "";
            bool code = true;
            int totalcount = 0;

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
                string gcmc = Request["gcmc"].GetSafeString();
                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                string startYear = Request["startYear"].GetSafeString();
                string endYear = Request["endYear"].GetSafeString();

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    if (!Remote.UserService.Login(username, password, out err))
                    {
                        code = false;
                        msg = "登录失败,请重新登录!";
                    }
                }

                if (code)
                {

                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002") && qybh == "") //五方主体
                    {
                        //通过登录账号获取企业编号
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                        dtt = CommonService.GetDataTable(sqlqybh);
                        qybh = dtt[0]["qybh"].ToString();
                    }
                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000001") && gcbh == "") //劳务部门
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
                    if (gcmc != "")
                        where += " and A.gcmc like '%" + gcmc + "%'";
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
                    if (startYear != "")
                        where += " and B.LogYear>='" + startYear + "'";
                    if (endYear != "")
                        where += " and B.LogYear<='" + endYear + "'";

                    string sql = "select A.GCBH,A.GCMC,B.* from I_M_GC A  ";
                    sql += "LEFT JOIN (SELECT JDZCH,LogYear,LogMonth,SUM(ISNULL(WORKDAY,0)) AS WORKDAY, SUM(ISNULL(BANKPAY,0)) AS BANKPAY FROM KqjUserMonthPay GROUP BY JDZCH,LogYear,LogMonth)  ";
                    sql += "B ON A.GCBH = B.JDZCH  ";
                    sql += where;
                    sql += " order by A.GCBH,B.LogYear desc,B.LogMonth desc";

                    dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, totalcount, jss.Serialize(dt)));

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
            string msg = "";
            bool code = true;
            int totalcount = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string qymc = Request["qymc"].GetSafeString();
                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
              string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    if (!Remote.UserService.Login(username, password, out err))
                    {
                        code = false;
                        msg = "登录失败,请重新登录!";
                    }
                }

                if (code)
                {

                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002") && qybh == "") //五方主体
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
                    if (qymc != "")
                        where += " and b.qymc  like '%" + qymc + "%'";

                    string sql = "select distinct b.*,c.Lat,c.Lon,d.lxmc from( ";
                    sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,b.QYBH from I_M_GC A left join I_S_GC_SGDW b on a.GCBH_YC = b.gcbh ";
                    sql += "union all ";
                    sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,c.QYBH  from I_M_GC A left join I_S_GC_JLDW c on a.GCBH_YC = c.gcbh ";
                    sql += "union all ";
                    sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,d.QYBH  from I_M_GC A left join I_S_GC_JSDW d on a.GCBH_YC = d.gcbh ";
                    sql += "union all ";
                    sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,e.QYBH  from I_M_GC A left join I_S_GC_SJDW e on a.GCBH_YC = e.gcbh ";
                    sql += "union all ";
                    sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,f.QYBH  from I_M_GC A left join I_S_GC_KCDW f on a.GCBH_YC = f.gcbh ";
                    sql += ") a ";
                    sql += "left join I_M_QY b on a.QYBH = b.QYBH ";
                    sql += "LEFT JOIN dbo.I_M_QY_JWD c ON b.QYBH = c.QYBH  ";
                    sql += "left join H_QYLX d on b.lxbh = d.LXBH  ";
                    sql += where;

                    dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", "0", "", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 获取企业一览不跟着工程走
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetQyListNoGc()
        {
            string msg = "";
            bool code = true;
            int totalcount = 0;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string qymc = Request["qymc"].GetSafeString();
                string pagesize = Request["pagesize"].GetSafeString();
                if (pagesize == "")
                    pagesize = "20";
                string pageindex = Request["pageindex"].GetSafeString();
                if (pageindex == "")
                    pageindex = "1";

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

                if (code)
                {

                    if (CurrentUser.CurUser.DepartmentId.Equals("wgryDP201802000002")) //五方主体
                    {
                        //通过登录账号获取企业编号
                        IList<IDictionary<string, string>> dtt = new List<IDictionary<string, string>>();
                        string sqlqybh = "select b.QYBH,b.qymc from I_M_QYZH a left join I_M_QY b on a.qybh = b.QYBH where yhzh=  '" + CurrentUser.CurUser.UserCode + "'";
                        dtt = CommonService.GetDataTable(sqlqybh);
                        qybh = dtt[0]["qybh"].ToString();
                    }

                    string where = " where 1=1 ";
                    if (province != "")
                        where += " and a.szsf='" + province + "'";
                    if (city != "")
                        where += " and a.szcs='" + city + "'";
                    if (district != "")
                        where += " and a.szxq='" + district + "'";
                    if (qylx != "")
                        where += " and a.lxbh='" + qylx + "'";
                    if (qybh != "")
                        where += " and a.qybh='" + qybh + "'";
                    if (qymc != "")
                        where += " and a.qymc  like '%" + qymc + "%'";

                    string sql = "select a.*,b.Lat,b.Lon,c.lxmc  ";
                    sql += "from I_M_QY a  ";
                    sql += "LEFT JOIN dbo.I_M_QY_JWD b ON a.QYBH = b.QYBH    ";
                    sql += "LEFT JOIN H_QYLX c on a.lxbh = c.LXBH    ";
                    sql += where;
                    dt = CommonService.GetPageData(sql, int.Parse(pagesize), int.Parse(pageindex), out totalcount);
                }
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", "0", "", totalcount, jss.Serialize(dt)));
                Response.End();
            }
        }


        /// <summary>
        /// 通过身份证号码获取基础人员库中的人员信息
        /// </summary>
        /// <returns></returns>
        public void GetRyXqFromJc()
        {
            string msg = "";
            bool code = true;
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);
            int totalcount = 0;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();

                if ((sfzhm == ""))
                {
                    msg = "请检查参数！";
                    code = false;
                }

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

                if (code)
                {

                    string day = DateTime.Today.ToString("yyyy-MM-dd");
                    string where = "WHERE 1=1 ";
                    where += "AND SFZHM = '" + sfzhm + "'";
                    string sql = "select top 1 ryxm,sfzhm,xb,csrq,sjhm,ryzt, gcmc, gz,gw,sfbzfzr,bzfzrxm,zcsj , ywsfjl,ywhmd,ywzs from View_I_M_WGRY_BASE where sfzhm='" + sfzhm + "' and hasdelete=0 ";
                   // string sql = "SELECT [RYBH],[RYXM],[XB] ,[MZ] ,[CSRQ],[SFZHM] ,[SFZDZ] ,[QFJG] ,[SFZYXQ],[JCRJZH] ,[SPBZ] ,[RYBZ],[ZH],[SJHM] ,[ZC],[SJYZM] ,[TYYHXY] ,[SFZKH],[ZSFJ] FROM I_M_RY_INFO  ";
                    
                    dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
                    if(dt.Count==0) //查询人员没有工程
                    {
                        sql = "select top 1 ryxm,sfzhm,xb,csrq,sjhm,'' as ryzt,'' as gcmc,'' as gz,'' as gw,'' as sfbzfzr,'' as bzfzrxm,lrsj as zcsj ,'无' as ywsfjl,'无' as ywhmc,'无'as ywzs from i_m_ry_info";
                        sql += where;
                        dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
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
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(JsonFormat.GetRetString(code, msg, totalcount, dt));
                Response.End();
            }
        }


        /// <summary>
        /// 获取人员考勤详情
        /// </summary>
        public void GetRyKQDetail()
        {
            string msg = "";
            bool code = true;
            int pageindex = Request["page"].GetSafeInt(1);
            int pagesize = Request["rows"].GetSafeInt(10);

            int totalcount = 0;
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string sfzhm = Request["sfzhm"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string startDate = Request["startDate"].GetSafeString();
                string endDate = Request["endDate"].GetSafeString();

                if ((sfzhm == ""))
                {
                    msg = "请检查参数！";
                    code = false;
                }

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

                if (code)
                {
                    string where = "WHERE 1=1 ";
                    where += "AND userid = '" + sfzhm + "'";
                    if (gcbh != "")
                        where += " and projectid='"+gcbh+"' ";
                    if (startDate != "")
                        where += " and DATEDIFF(dd,logday,'" + startDate + "')=0";
                    if (endDate != "")
                        where += " and DATEDIFF(dd,logday,'" + endDate + "')=0";


                    string sql = "select  * from ViewKqjUserDayLogDetail "+where;
                    sql += " order by intime desc";
                    dt = CommonService.GetPageData(sql, pagesize, pageindex, out totalcount);
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
                Response.Write(JsonFormat.GetRetString(code, msg, totalcount, dt));
                Response.End();
            }
        }



        #endregion

    }
}
