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
using System.IO;
using BD.DataInputModel.Entities;
using BD.DataInputDaoSqlServer;
using System.Data;
using System.Data.SqlClient;

namespace BD.Jcbg.Web.Controllers
{
    public class WzWgryController : Controller
    {
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
        [Authorize]
        public ActionResult index()
        {
            return View();
        }
        [Authorize]
        public ActionResult gcIndex()
        {
            string gcmc = "";
            string gcbh = Request["gcbh"].GetSafeString();
            string type = Request["type"].GetSafeString();
            if (gcbh == "")
            {
                gcbh = CurrentUser.Jdzch;
                type = "gc";
            }
            string sql = "select gcmc from i_m_gc where gcbh='" + gcbh + "'";
            IList<IDictionary<string, string>> dt=CommonService.GetDataTable(sql);
            if(dt.Count>0)
            {
                gcmc = dt[0]["gcmc"].GetSafeString();
            }
            ViewData["gcmc"] = gcmc;
            ViewData["gcbh"] = gcbh;
            ViewData["type"] = type;
            return View();
        }
        [Authorize]
        public ActionResult gcIndex_ZF()
        {
            string gcmc = "";
            string gcbh = Request["gcbh"].GetSafeString();
            if (gcbh == "")
                gcbh = CurrentUser.Jdzch;
            string sql = "select gcmc from i_m_gc where gcbh='" + gcbh + "'";
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                gcmc = dt[0]["gcmc"].GetSafeString();
            }
            ViewData["gcmc"] = gcmc;
            ViewData["gcbh"] = gcbh;
                     ViewData["type"] = "zf";
            return View();
        }
        [Authorize]
        public ActionResult qyIndex()
        {
            string qybh = Request["qybh"].GetSafeString();
            if (qybh == "")
                qybh = CurrentUser.Qybh;
            ViewData["qybh"] = qybh;
            ViewData["type"] = "qy";
            return View();
        }
        [Authorize]
        public ActionResult qyIndex_zf()
        {
            string qybh = Request["qybh"].GetSafeString();
            if (qybh == "")
                qybh = CurrentUser.Qybh;
            ViewData["qybh"] = qybh;
            ViewData["type"] = "qy";
            return View();
        }
        [Authorize]
        public ActionResult bank()
        {
            return View();
        }

        public ActionResult gcxwgl()
        {
            return View();
        }
        #endregion

        #region 获取数据
        #region 政府端
        /// <summary>
        /// 工程分布区域统计
        /// </summary>
        [Authorize]
        public void GetGC_QYFBTJ()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string gclx = Request["gclx"].GetSafeString();
                string key = Request["key"].GetSafeString();

                string where = " where 1=1 ";
                //判断是否企业账户登录
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

                if (province != "")
                    where += " and a.szsf='" + province + "'";
                if (city != "")
                    where += " and a.szcs='" + city + "'";
                if (qybh != "")
                    where += " and a.sgdwbh = '" + qybh + "'";
                if (gcbh != "")
                    where += " and a.gcmc ='" + gcbh + "'";
                if (gclx != "")
                    where += " and a.gclxbh='" + gclx + "'";

                if(key!="")
                    where += " and (a.gcmc like '%" + key + "%' or a.sgdwmc like '%" + key + "%')";



                //判断是显示区还是街道分布
                if (district != "")
                    where += " and a.szxq='" + district + "'";

                string sql = "select count(1) as value ";
                if (district != "")
                {
                    sql += ",szjd as name ";
                    where += " group by szjd"; 
                }
                else
                {
                    sql += ",szxq as name ";
                    where += " group by szxq"; 
                }

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

        /// <summary>
        /// 几块统计数据
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetTjlist()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();

                string gclx = Request["gclx"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();

                string gcbh = Request["gcbh"].GetSafeString();
                string key = Request["key"].GetSafeString();
                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }
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
                if (qybh != "")
                    where += " and A.sgdwbh = '" + qybh + "'";
                if (gcbh != "")
                    where += " and A.gcbh = '" + gcbh + "'";
                if (gclx != "")
                    where += " and A.gclxbh = '" + gclx + "'";
                if (key != "")
                    where += " and (a.gcmc like '%" + key + "%' or a.sgdwmc like '%" + key + "%')";
                //施工企业总数
                //string sql = "select count(1) as num from View_GC_QY where qylxmc='施工单位' group by qybh";
               // string sql = "select count(1) as num from (select qybh from View_GC_QY where qylxmc='施工单位' group by qybh) aa";
                string sql = "select szsf,szcs,szxq,szjd from View_H_ZFZH_XQ  where usercode='"+CurrentUser.UserName+"' ";
                IList<IDictionary<string, string>> zf_dqlist = CommonService.GetDataTable(sql);
                string szsf = "", szcs = "", szxq = "", szjd="" ;
                string whereqynum = "";
                for(int i=0;i<zf_dqlist.Count;i++)
                {
                    if (!string.IsNullOrEmpty(zf_dqlist[i]["szsf"]))
                        szsf += zf_dqlist[i]["szsf"] + ",";
                    if (!string.IsNullOrEmpty(zf_dqlist[i]["szcs"]))
                        szcs += zf_dqlist[i]["szcs"] + ",";
                    if (!string.IsNullOrEmpty(zf_dqlist[i]["szxq"]))
                        szxq += zf_dqlist[i]["szxq"] + ",";
                    if (!string.IsNullOrEmpty(zf_dqlist[i]["szjd"]))
                        szjd += zf_dqlist[i]["szjd"] + ",";
                }
                if(szsf!="")
                {
                    szsf = szsf.FormatSQLInStr(); ;
                    whereqynum += " and szsf in (" + szsf + ")";
                }
                if (szcs != "")
                {
                    szcs = szcs.FormatSQLInStr(); ;
                    whereqynum += " and szcs in (" + szcs + ")";
                }
                if (szxq != "")
                {
                    szxq = szxq.FormatSQLInStr(); ;
                    whereqynum += " and szxq in (" + szxq + ")";
                }
                if (szjd != "")
                {
                    szjd = szjd.FormatSQLInStr(); ;
                    whereqynum += " and szjd in (" + szjd + ")";
                }
                sql = "select count(1) as num from (select qybh from View_GC_QY where qylxmc='施工单位' " + whereqynum + "  group by qybh) aa";

              
                IList<IDictionary<string, string>> dt_sgdwnum = new List<IDictionary<string, string>>();
                dt_sgdwnum = CommonService.GetDataTable(sql);
                string sgdwnum = "0";
                if (null != dt_sgdwnum && dt_sgdwnum.Count != 0)
                    sgdwnum = dt_sgdwnum[0]["num"];

                //总工程数
                sql = "select count(1) as num from View_I_M_GC_ZS A " + where;
                IList<IDictionary<string, string>> dt_zgcs = new List<IDictionary<string, string>>();
                dt_zgcs = CommonService.GetDataTable(sql);
                string zgcs = "0";            
                if (null != dt_zgcs && dt_zgcs.Count != 0)
                {
                    zgcs = dt_zgcs[0]["num"];               
                }

                //在建工程数
                sql = "select count(1) as num ,sum(cast(isnull(jzmj,'0') as decimal)) as jzmj,sum(cast(isnull(gczj,'0') as decimal)) as gczj from View_I_M_GC_ZS A " + where + " and A.gczt='1'";
                IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                dt_zjgcs = CommonService.GetDataTable(sql);
                string zjgcs = "0";
                string jzmj = "0";
                string gczj = "0";
                if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                {
                    zjgcs = dt_zjgcs[0]["num"];
                    jzmj = dt_zjgcs[0]["jzmj"];// (Math.Round(int.Parse(dt_zjgcs[0]["jzmj"]) * 1.0 / 10000, 2)).ToString() + "万平方米"; 
                    gczj = dt_zjgcs[0]["gczj"];// (Math.Round(int.Parse(dt_zjgcs[0]["gczj"]) * 1.0 / 10000, 2)).ToString() + "万元";
                }

                //在册人数
                sql = "select count(1) as num from ";
                sql += "(select distinct b.sfzhm from View_I_M_GC_ZS A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH " + where + " and B.Recid IS NOT NULL) T";
                IList<IDictionary<string, string>> dt_zcry = new List<IDictionary<string, string>>();
                dt_zcry = CommonService.GetDataTable(sql);
                string zcry = "0";
                if (null != dt_zcry && dt_zcry.Count != 0)
                    zcry = dt_zcry[0]["num"];

                //在职人数
                sql = "select count(1) as num from ";
                sql += "(select distinct b.sfzhm from View_I_M_GC_ZS A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH " + where + " and B.hasdelete = '0') T";
                IList<IDictionary<string, string>> dt_zzry = new List<IDictionary<string, string>>();
                dt_zzry = CommonService.GetDataTable(sql);
                string zzry = "0";
                if (null != dt_zzry && dt_zzry.Count != 0)
                    zzry = dt_zzry[0]["num"];


                //在岗人数
                sql = "select sum(inrynum) as num from ";
                sql += " View_I_M_GC_ZS A ";
                sql += where;
                IList<IDictionary<string, string>> dt_zgry = new List<IDictionary<string, string>>();
                dt_zgry = CommonService.GetDataTable(sql);
                string zgry = "0";
                if (null != dt_zgry && dt_zgry.Count != 0)
                    zgry = dt_zgry[0]["num"];


                //当前人员
                string day = DateTime.Today.ToString("yyyy-MM-dd");
                sql = "select sum(inrynum) as num from View_I_M_GC_ZS A ";
                sql += where;


                IList<IDictionary<string, string>> dt_dqry = new List<IDictionary<string, string>>();
                dt_dqry = CommonService.GetDataTable(sql);
                string dqry = "0";
                if (null != dt_dqry && dt_dqry.Count != 0)
                    dqry = dt_dqry[0]["num"];

                //应发金额
                //sql = "select sum(cast(isnull(gczj,'0') as int)) as gczj from I_M_GC A " + where;
                //IList<IDictionary<string, string>> dt_gczj = new List<IDictionary<string, string>>();
                //dt_gczj = CommonService.GetDataTable(sql);
                //string gczj = "0";
                //if (null != dt_gczj && dt_gczj.Count != 0 && dt_gczj[0]["gczj"] != "")
                //    gczj = dt_gczj[0]["gczj"];
                //gczj = (Math.Round(int.Parse(gczj) * 0.3 / 10000, 2)).ToString();

                //应付金额

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("sgdwnum", sgdwnum);  //施工单位数
                di.Add("zgcs", zgcs); //总工程数
                di.Add("zjgcs", zjgcs); //在建工程数
                di.Add("jzmj", jzmj); //建筑面积
                di.Add("zcry", zcry); //在册人员
                di.Add("zzry", zzry); //在职人员
                di.Add("zgry", zgry); //在岗人员
                di.Add("dqry", dqry); //当前人员
                di.Add("gczj", gczj); //工程造价
                di.Add("yfje", "0"); //应发金额
                di.Add("sfje", "0"); //实发金额
                di.Add("ljffje", "0"); //累计发放金额


                dt.Add(di);
                datajson.Datas = dt;
                datajson.Status = "success";
                datajson.Msg = "";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
                datajson.Status = "failure";
                datajson.Msg = msg;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(jss.Serialize(datajson));
                //Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }
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
                string qybh="";

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
                sql += "select a.GCBH,a.SZSF,a.SZCS,a.SZXQ,a.SZJD,b.QYBH from I_M_GC A left join I_S_GC_SGDW b on b.gcbh=(case when a.gcbh_yc is null or a.gcbh_yc='' then a.gcbh else a.gcbh_yc end)";
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
        /// 获取安全教育统计
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetAQTjlist()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            WgrydataList datajson = new WgrydataList();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                string gclx = Request["gclx"].GetSafeString();
                string key = Request["key"].GetSafeString();

                string where = " where 1=1 ";
                //判断是否企业账户登录
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

                if (province != "")
                    where += " and a.szsf='" + province + "'";
                if (city != "")
                    where += " and a.szcs='" + city + "'";
                if (qybh != "")
                    where += " and a.sgdwbh = '" + qybh + "'";
                if (gcbh != "")
                    where += " and a.gcmc ='" + gcbh + "'";
                if (gclx != "")
                    where += " and a.gclxbh='" + gclx + "'";

                if (key != "")
                    where += " and (a.gcmc like '%" + key + "%' or a.sgdwmc like '%" + key + "%')";

                //全部认识
                string sql = "select count(1) as num from View_I_M_WGRY2 a ";
                string where1=where;
                string qyrs = "0";
                IList<IDictionary<string, string>> qbrslist = CommonService.GetDataTable(sql + where1);
                if (qbrslist.Count>0)
                {
                    qyrs = qbrslist[0]["num"];
                }

                //工伤参保人数
                string where2 = where;
                string gscbrs = "0";
                IList<IDictionary<string, string>> gscbrslist = CommonService.GetDataTable(sql + where2);
                if (gscbrslist.Count > 0)
                {
                    gscbrs = gscbrslist[0]["num"];
                }

                //安全教育人数
                string where3 = where+" and sfaqjy=1";
                string aqjyrs = "0";
                IList<IDictionary<string, string>> aqjyrslist = CommonService.GetDataTable(sql + where3);
                if (aqjyrslist.Count > 0)
                {
                    aqjyrs = aqjyrslist[0]["num"];
                }

                //劳动合同人数
                string where4 = where + " and htqd='是'";
                string ldhtrs = "0";
                IList<IDictionary<string, string>> ldhtrslist = CommonService.GetDataTable(sql + where4);
                if (ldhtrslist.Count > 0)
                {
                    ldhtrs = ldhtrslist[0]["num"];
                }

                sql += " from View_I_M_WGRY2 a ";
                sql += where;

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("qyrs", qyrs); //全部人数
                di.Add("gscbrs", gscbrs); //工伤参保人数
                di.Add("aqjyrs", aqjyrs); //安全教育人数
                di.Add("ldhtrs", ldhtrs); //劳动合同人数

                datajson.Datas = di;
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
            return Json(datajson, JsonRequestBehavior.AllowGet);
        }

        #region 政府端-企业管理菜单

        /// <summary>
        /// 获取相关企业数
        /// </summary>
        [Authorize]
        public void getQyNum()
        {
            string msg = "";
            bool code = true;
            try
            {

            }
            catch(Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                //JavaScriptSerializer jss = new JavaScriptSerializer();
                //jss.MaxJsonLength = 10240000;
                //Response.ContentEncoding = System.Text.Encoding.UTF8;

                //Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"total\":{2},\"rows\":{3}}}", code ? "0" : "1", msg, dt.Count, jss.Serialize(dt)));
                //Response.End();
            }
        }
        #endregion

        #endregion

        #region 企业端
        /// <summary>
        /// 获取企业的工种统计
        /// </summary>
        [Authorize]
        public void GetQYGzTj()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
            List<WgryGzTj> wgrygztjlist = new List<WgryGzTj>();
            try
            {
                string province = Request["province"].GetSafeString();
                string city = Request["city"].GetSafeString();
                string district = Request["district"].GetSafeString();
                string jd = Request["jd"].GetSafeString();

                string gclx = Request["gclx"].GetSafeString();

                string qylx = Request["qylx"].GetSafeString();
                string qybh = Request["qybh"].GetSafeString();
                string gczt = Request["gczt"].GetSafeString();

                string gcbh = Request["gcbh"].GetSafeString();
                string key = Request["key"].GetSafeString();
                string err = "";
                string username = Request["login_name"].GetSafeString();
                string password = Request["login_pwd"].GetSafeString();

                if (!CurrentUser.IsLogin)
                {
                    Remote.UserService.Login(username, password, out err);
                }
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
                        //datajson.Msg = "找不到企业信息";
                        //datajson.Status = "failure";
                    }
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
                if (jd != "")
                    where += " and A.szjd='" + jd + "'";
                if (gczt != "")
                    where += " and A.gczt='" + gczt + "'";
                if (qybh != "")
                    where += " and A.qybh = '" + qybh + "'";
                if (gcbh != "")
                    where += " and A.jdzch = '" + gcbh + "'";
                if (gclx != "")
                    where += " and A.gclxbh = '" + gclx + "'";
                if (key != "")
                    where += " and (a.gcmc like '%" + key + "%' or a.qymc like '%" + key + "%')";

                string sql = "select gz,count(1) as num from View_I_M_WGRY A ";
                string sql1 = sql + where;
                sql1 += "group by gz";
                dt1 = CommonService.GetDataTable(sql1);  //登记人员

                string sql2 = sql + where+ " and hasdelete=0 ";
                sql2 += "group by gz";
                dt2 = CommonService.GetDataTable(sql2); //在职人员

                string sql3 = "select * from ViewWgryCurrentWorker ";
                sql3 += " where companyid='" + qybh + "' and datediff(dd,logday,getdate())=0 ";
                dt3 = CommonService.GetDataTable(sql3); //当前人员

                WgryGzTj wgrygztj = new WgryGzTj();
                for (int i = 0; i < dt1.Count; i++)
                {
                    string num1 = "0";
                    string num2 = "0";
                    string num3 = "0";
                    string num4 = "0";
                    string num5 = "0";
                    string num6 = "0";
                    string gzname = dt1[i]["gz"];
                    wgrygztj = new WgryGzTj();
                    WgryGzRynumTj wgrygzrynumtj = new WgryGzRynumTj();
                    wgrygztj.Gz = gzname;
                    wgrygzrynumtj.data1 = dt1[i]["num"];
                    for (int j = 0; j < dt2.Count; j++)
                    {
                        if (gzname == dt2[j]["gz"])
                        {
                            num2 = dt2[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.data2 = num2;
                    for (int j = 0; j < dt3.Count; j++)
                    {
                        if (gzname == dt3[j]["gzname"])
                        {
                            num3 = dt3[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.data3 = num3;
  

                    wgrygztj.Datas = wgrygzrynumtj;
                    wgrygztjlist.Add(wgrygztj);
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
                Response.Write(jss.Serialize(wgrygztjlist));
               // Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"datas\":{2}}", code ? "0" : "1", msg,  jss.Serialize(wgrygztjlist)));
                Response.End();
            }

        }
        /// <summary>
        /// 初始化获取企业有工程的地点
        /// </summary>
        [Authorize]
        public void GetQYGcXQ()
        {
            string msg = "";
            bool code = true;
            Wgrydata datajson = new Wgrydata();
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                if (qybh == "undefined"|| qybh=="")
                    qybh = CurrentUser.Qybh; ;
             //  string sql = "select top 1 szsf,szcs,szxq from view_I_m_gc where sgdwzh= '" + CurrentUser.UserName + "'";

                string sql = "select top 1 szsf,szcs,szxq from View_I_S_ZFQY_ZS where bdqybh= '" + qybh + "'";
                IList<IDictionary<string, string>> list=CommonService.GetDataTable(sql);
                datajson.Datas = list;
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

        #endregion

        #region 项目端
        /// <summary>
        /// 获取项目人员数等信息
        /// </summary>
        [Authorize]
        public void getInfo_GC()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString() ;
                if(gcbh=="")
                    gcbh = CurrentUser.Jdzch;
                //实名制登记人员数
                string sql = "select count(1) as num  from I_M_WGRY a where jdzch='" + gcbh + "'";
                string smznum = "0";
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    smznum = dt[0]["num"].GetSafeString();
                }
                //在职人员数
                sql = "select count(1) as num  from I_M_WGRY a where jdzch='" + gcbh + "' and hasdelete=0";
                string zznum = "0";
                dt = CommonService.GetDataTable(sql);
                if(dt.Count>0)
                {
                    zznum = dt[0]["num"].GetSafeString();
                }
                //当前人员数
                string currRynum = "0";
                sql = "select inrynum as num from i_m_gc where gcbh='" + gcbh + "'";
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    currRynum = dt[0]["num"].GetSafeString();
                }
                //考勤异常的人数
                string kqycnum = "0";
                sql = "select count(1) as num from (select userid from ViewKqjUserDayLogDetail where DATEDIFF(dd,LogDay,GETDATE())=0 and (intime=OutTime or intime is null) and projectid='"+gcbh+"' group by UserId) a";
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    kqycnum = dt[0]["num"].GetSafeString();
                }



                //设备异常数 大于5分钟算异常
                string sbycnum = "0";
                sql = "select count(1) as num from View_I_M_KQJ where jdzch='" + gcbh + "' and datediff(mi,lastupdate,getdate())>5";
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    sbycnum = dt[0]["num"].GetSafeString();
                }
                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("smzry", smznum);//实名制登记人数
                di.Add("zzry", zznum);//在岗人数
                di.Add("dqry", currRynum); //当前人员数
                di.Add("yjrynum", "0"); //违规预警人数
                di.Add("kqycnum", kqycnum); //考勤异常人数
                di.Add("sbycnum", sbycnum); //设备异常数

                IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
                data.Add(di);
                datajson.Datas = data;
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
        /// <summary>
        /// 获取项目当前人员数
        /// </summary>
        [Authorize]
        public void getCurrRyNum()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string gcbh = CurrentUser.Jdzch;
                string sql = "select inrynum from i_m_gc where gcbh='" + gcbh + "'";

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

        /// <summary>
        /// 获取宿舍统计
        /// </summary>
        [Authorize]
        public void GetSStj()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("usenum", "100");//已入住宿舍
                di.Add("other", "80"); //空闲宿舍
     
                IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
                data.Add(di);

                datajson.Datas = data;
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
        /// <summary>
        /// 工资册填写、发放统计
        /// </summary>
        [Authorize]
        public void GetPaytj()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                DateTime datetime=DateTime.Now;
                string gcbh=CurrentUser.Jdzch;
                string year=datetime.Year.ToString();
                string month=datetime.Month.ToString();
                string where="where jdzch='"+gcbh+"'  and logyear='"+year+"' and logmonth='"+month+"' ";
                string sql = "select count(1) as num from KqjUserMonthPay " + where;
                string where1 = " and (bankpay is not null and bankpay<>0)";//已发放
                string where2 = " and (bankpay is null or bankpay=0) and (havepay is not null and havepay<>0) ";//已造册未发放
                string where3 = " and (havepay is null or havepay=0) ";//未造册
                string data1 = "0";
                string data2 = "0";
                string data3 = "0";
                dt = CommonService.GetDataTable(sql+where1);
                if(dt.Count>0)
                {
                    data1 = dt[0]["num"].GetSafeString();
                }
                dt = CommonService.GetDataTable(sql + where2);
                if (dt.Count > 0)
                {
                    data2 = dt[0]["num"].GetSafeString();
                }
                dt = CommonService.GetDataTable(sql + where3);
                if (dt.Count > 0)
                {
                    data3 = dt[0]["num"].GetSafeString();
                }

                IDictionary<string, string> di = new Dictionary<string, string>();
                di.Add("data1", data1);////已发放人数
                di.Add("data2", data2); ////已造册未发放人数
                di.Add("data3", data3); //未造册人数

                IList<IDictionary<string, string>> data = new List<IDictionary<string, string>>();
                data.Add(di);

                datajson.Datas = data;
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
        [Authorize]
        public void GetGzRynum()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt4 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt5 = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> dt6 = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            List<WgryGzTj> wgrygztjlist = new List<WgryGzTj>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString() ;
                if(gcbh=="")
                    gcbh = CurrentUser.Jdzch;

                string where1 = "where jdzch='" + gcbh + "' ";//总人数
                string where2 = "where jdzch='" + gcbh + "' and hasdelete=0 ";//在职人数

                string sql = "select gz,count(1) as num ,xssx from View_I_M_WGRY_GZ ";
                string sql1=sql + where1;
                sql1 += "group by gz,xssx";
                dt1 = CommonService.GetDataTable(sql1);  //登记人员

                string sql2 = sql + where2;
                sql2 += "group by gz";
                dt2 = CommonService.GetDataTable(sql2); //在职人员

                string sql3 = "select * from ViewWgryCurrentWorker ";
                sql3 += " where projectid='"+gcbh+"' and datediff(dd,logday,getdate())=0 ";
                dt3 = CommonService.GetDataTable(sql3); //当前人员

                string sql4 = "select count(1)as  num,gz from (select userid,logday,companyid,projectid,gz from ViewKqjUserDayLog group by userid,logday,companyid,projectid,gz)a ";
                sql4+=" where DATEDIFF(dd,LogDay,GETDATE())=0 and ProjectId='"+gcbh+"' group by gz";
                dt4 = CommonService.GetDataTable(sql4); //当天考勤人数

                DateTime datetime=DateTime.Now;
                string year=datetime.Year.ToString();
                string month=(datetime.Month - 1).ToString();
                if (datetime.Month == 1)
                {
                    month = "12";
                    year = (datetime.Year - 1).ToString() ;
                }
                string where = "where jdzch='" + gcbh + "'  and logyear='" + year + "' and logmonth='" + month + "' group by gzname";
                string sql5 = "select sum(isnull(havepay,0)) as num,gzname from kqjusermonthpay " + where;
                string sql6 = "select sum(isnull(bankpay,0)) as num,gzname from kqjusermonthpay " + where;
                dt5 = CommonService.GetDataTable(sql5);
                dt6 = CommonService.GetDataTable(sql6);

                WgryGzTj wgrygztj = new WgryGzTj();
                for (int i = 0; i < dt1.Count; i++)
                {
                    string num1 = "0";
                    string num2 = "0";
                    string num3 = "0";
                    string num4 = "0";
                    string num5 = "0";
                    string num6 = "0";
                    string gzname = dt1[i]["gz"];
                    wgrygztj = new WgryGzTj();
                    WgryGzRynumTj wgrygzrynumtj = new WgryGzRynumTj();
                    wgrygztj.Gz = gzname;
                    wgrygzrynumtj.data1 = dt1[i]["num"];
                    for (int j = 0; j < dt2.Count; j++)
                    {
                        if (gzname == dt2[j]["gz"])
                        {
                            num2 = dt2[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.data2 = num2;
                    for (int j = 0; j < dt3.Count; j++)
                    {
                        if (gzname == dt3[j]["gzname"])
                        {
                            num3 = dt3[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.data3 = num3;
                    for (int j = 0; j < dt4.Count; j++)
                    {
                        if (gzname == dt4[j]["gz"])
                        {
                            num4 = dt4[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.data4=num4;

                    for (int j = 0; j < dt5.Count; j++)
                    {
                        if (gzname == dt5[j]["gzname"])
                        {
                            num5 = dt5[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.shouldpaynum = num5;
                    for (int j = 0; j < dt6.Count; j++)
                    {
                        if (gzname == dt6[j]["gzname"])
                        {
                            num6 = dt6[j]["num"];
                            break;
                        }
                    }
                    wgrygzrynumtj.bankpaynum = num6;

                    wgrygzrynumtj.allcw = "1000";
                    wgrygzrynumtj.usecw = "500";
                    
                    wgrygztj.Datas = wgrygzrynumtj;
                    wgrygztjlist.Add(wgrygztj);
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

                Response.Write(jss.Serialize(wgrygztjlist));
                Response.End();
            }
        }
        #endregion
        #endregion

        #region
        public void test()
        {
            string a = EncryUtil.Encode("333333333");

            DateTime dt = DateTime.Now;
            string time = dt.ToShortTimeString() ;
            DateTime aa =  (dt.Date.ToShortDateString() + " 08:08").GetSafeDate();
            DateTime bb = "2018-5-8 15:18".GetSafeDate();
            double bbb = dt.Subtract(bb).TotalHours;

            string b = EncryUtil.Decode(a);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 10240000;
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            Response.Write(bbb.ToString());
            Response.End();
        }

        #endregion

        #region 二维码拍照上次合同
        public void UploadHT()
        {
            string username = Request["login_name"].GetSafeString();
            string password = Request["login_pwd"].GetSafeString();
            string key = Request["key"].GetSafeString();
            bool code = true;
            string err = "";
          

            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                if (!CurrentUser.IsLogin)
                    code = Remote.UserService.Login(username, password, out err);
                if (code)
                {
                    string sfz_gc = EncryUtil.Decode(key);
                    string[] list = sfz_gc.Split('_');
                    if(list.Length!=2)
                    {
                        err = "解析二维码失败";
                        code = false;
                    }
                    else
                    {
                        string jdzch = list[0];
                        string sfzhm = list[1];
                        string sql = "select top 1 jdzch from I_M_LZZGY_ZH where usercode='" + CurrentUser.UserName + "'";
                        IList<IDictionary<string, string>> zgylig= CommonService.GetDataTable(sql);
                        if(zgylig.Count>0)
                        {
                            if(jdzch!=zgylig[0]["jdzch"])
                            {
                                err = "用户账号与人员所在工程不匹配";
                                code = false;
                            }
                        }
                        if(code)
                        {
                            string filetext = "";
                            //解析保存照片
                            for (int i = 0; i < Request.Files.Count; i++)
                            {
                                string fileid = Guid.NewGuid().ToString("N");
                                HttpPostedFileBase file = Request.Files[i];
                                code = SelfService.SaveDataFile(file, Server, fileid, out err);
                                if (!code)
                                    break;
                                if (code)
                                {
                                    filetext += fileid + "," + fileid + ".jpg|";
                                }
                            }
                            if (code)
                            {
                                string sqlht = "update i_m_wgry set sfqdht=1 where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";
                                CommonService.Execsql(sqlht);
                                sqlht = "update i_s_gc_ht set hasdelete=1 where jdzch='" + jdzch + "' and sfzhm='" + sfzhm + "'";
                                CommonService.Execsql(sqlht);

                                sql = "INSERT INTO [dbo].[I_S_GC_HT]([JDZCH] ,[SFZHM] ,[HT_FILE],[LRRY],[LRRYXM],[LRSJ],[Hasdelete])VALUES(@jdzch,@sfzhm,@ht_file ,@lrry,@lrryxm,getdate(),0)";
                               // sql="UPDATE I_S_GC_HT set jdzch=@jdzch,sfzhm=@sfzhm,ht_file=@ht_file,lrry=@lrry,lrryxm=@lrryxm,lrsj=getdate(),hasdelete=0  where "                           
                                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                                IDataParameter sqlparam = new SqlParameter("@filetext", filetext);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@jdzch", jdzch);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@sfzhm", sfzhm);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@ht_file", filetext);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@lrry", CurrentUser.UserName);
                                sqlparams.Add(sqlparam);
                                sqlparam = new SqlParameter("@lrryxm", CurrentUser.RealName);
                                sqlparams.Add(sqlparam);
                                code = CommonService.ExecTrans(sql, sqlparams, out err);
                              
                            }
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
                Response.AddHeader("Content-Type", "application/octet-stream"); //手机form表单需要加
                Response.StatusCode = 200;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"Status\":\"{0}\",\"Msg\":\"{1}\"}}", code ? "True" : "False", err));
                Response.End();
            }
        }
        private void savePng(string filename, byte[] bytes)
        {
            try
            {
                string path = Server.MapPath("~/htpng/") + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + CurrentUser.RealName + "_" + filename;//设定上传的文件路径
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            { }
        }
        #endregion


        #region 远程接口
        /// <summary>
        /// 政府端多块数据
        /// </summary>
        public void GetTjlist_YC()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string zjzbh = Request["zjzbh"].GetSafeString();
                string wgptbh = Request["wgptbh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
             
                string key = Request["key"].GetSafeString();
                string err = "";

                if (wgptbh != "" && zjzbh!="")
                {
                    string where = " where 1=1";
                    if (gcbh != "")
                        where += " and A.gcbh_yc = '" + gcbh + "'";
                    if (zjzbh != "")
                        where += " and  '," + zjzbh + ",' like '%,'+A.zjzbh+',%'";
                    if (wgptbh != "")
                        where += " and A.wgptbh = '" + wgptbh + "'";

                    //施工企业总数
                    //string sql = "select count(1) as num from View_GC_QY where qylxmc='施工单位' group by qybh";
                    string sql = "select count(1) as num from (select qybh from View_GC_QY where qylxmc='施工单位' group by qybh) aa";

                    IList<IDictionary<string, string>> dt_sgdwnum = new List<IDictionary<string, string>>();
                    dt_sgdwnum = CommonService.GetDataTable(sql);
                    string sgdwnum = "0";
                    if (null != dt_sgdwnum && dt_sgdwnum.Count != 0)
                        sgdwnum = dt_sgdwnum[0]["num"];

                    //总工程数
                    sql = "select count(1) as num from View_I_M_GC_ZS A " + where;
                    IList<IDictionary<string, string>> dt_zgcs = new List<IDictionary<string, string>>();
                    dt_zgcs = CommonService.GetDataTable(sql);
                    string zgcs = "0";
                    if (null != dt_zgcs && dt_zgcs.Count != 0)
                    {
                        zgcs = dt_zgcs[0]["num"];
                    }

                    //在建工程数
                    sql = "select count(1) as num ,sum(cast(isnull(jzmj,'0') as decimal)) as jzmj,sum(cast(isnull(gczj,'0') as decimal)) as gczj from View_I_M_GC_ZS A " + where + " and A.gczt='1'";
                    IList<IDictionary<string, string>> dt_zjgcs = new List<IDictionary<string, string>>();
                    dt_zjgcs = CommonService.GetDataTable(sql);
                    string zjgcs = "0";
                    string jzmj = "0";
                    string gczj = "0";
                    if (null != dt_zjgcs && dt_zjgcs.Count != 0)
                    {
                        zjgcs = dt_zjgcs[0]["num"];
                        jzmj = dt_zjgcs[0]["jzmj"];// (Math.Round(int.Parse(dt_zjgcs[0]["jzmj"]) * 1.0 / 10000, 2)).ToString() + "万平方米"; 
                        gczj = dt_zjgcs[0]["gczj"];// (Math.Round(int.Parse(dt_zjgcs[0]["gczj"]) * 1.0 / 10000, 2)).ToString() + "万元";
                    }

                    //在册人数
                    sql = "select count(1) as num from ";
                    sql += "(select distinct b.sfzhm from View_I_M_GC_ZS A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH " + where + " and B.Recid IS NOT NULL) T";
                    IList<IDictionary<string, string>> dt_zcry = new List<IDictionary<string, string>>();
                    dt_zcry = CommonService.GetDataTable(sql);
                    string zcry = "0";
                    if (null != dt_zcry && dt_zcry.Count != 0)
                        zcry = dt_zcry[0]["num"];

                    //在职人数
                    sql = "select count(1) as num from ";
                    sql += "(select distinct b.sfzhm from View_I_M_GC_ZS A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH " + where + " and B.hasdelete = '0') T";
                    IList<IDictionary<string, string>> dt_zzry = new List<IDictionary<string, string>>();
                    dt_zzry = CommonService.GetDataTable(sql);
                    string zzry = "0";
                    if (null != dt_zzry && dt_zzry.Count != 0)
                        zzry = dt_zzry[0]["num"];


                    //在岗人数
                    sql = "select sum(inrynum) as num from ";
                    sql += " View_I_M_GC_ZS A ";
                    sql += where;
                    IList<IDictionary<string, string>> dt_zgry = new List<IDictionary<string, string>>();
                    dt_zgry = CommonService.GetDataTable(sql);
                    string zgry = "0";
                    if (null != dt_zgry && dt_zgry.Count != 0)
                        zgry = dt_zgry[0]["num"];


                    //当前人员
                    string day = DateTime.Today.ToString("yyyy-MM-dd");
                    sql = "select sum(inrynum) as num from View_I_M_GC_ZS A ";
                    sql += where;


                    IList<IDictionary<string, string>> dt_dqry = new List<IDictionary<string, string>>();
                    dt_dqry = CommonService.GetDataTable(sql);
                    string dqry = "0";
                    if (null != dt_dqry && dt_dqry.Count != 0)
                        dqry = dt_dqry[0]["num"];

                    //应发金额
                    //sql = "select sum(cast(isnull(gczj,'0') as int)) as gczj from I_M_GC A " + where;
                    //IList<IDictionary<string, string>> dt_gczj = new List<IDictionary<string, string>>();
                    //dt_gczj = CommonService.GetDataTable(sql);
                    //string gczj = "0";
                    //if (null != dt_gczj && dt_gczj.Count != 0 && dt_gczj[0]["gczj"] != "")
                    //    gczj = dt_gczj[0]["gczj"];
                    //gczj = (Math.Round(int.Parse(gczj) * 0.3 / 10000, 2)).ToString();

                    //应付金额

                    IDictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("sgdwnum", sgdwnum);  //施工单位数
                    di.Add("zgcs", zgcs); //总工程数
                    di.Add("zjgcs", zjgcs); //在建工程数
                    di.Add("jzmj", jzmj); //建筑面积
                    di.Add("zcry", zcry); //在册人员
                    di.Add("zzry", zzry); //在职人员
                    di.Add("zgry", zgry); //在岗人员
                    di.Add("dqry", dqry); //当前人员
                    di.Add("gczj", gczj); //工程造价
                    di.Add("yfje", "0"); //应发金额
                    di.Add("sfje", "0"); //实发金额
                    di.Add("ljffje", "0"); //累计发放金额


                    dt.Add(di);
                    datajson.Datas = dt;
                    datajson.Status = "success";
                    datajson.Msg = "";
                }
                else
                {
                    datajson.Status = "failure";
                    datajson.Msg = "参数错误,获取失败";
                }
                
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
                datajson.Status = "failure";
                datajson.Msg = msg;
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

        /// <summary>
        /// 工程分布区域统计
        /// </summary>
        public void GetGC_QYFBTJ_YC()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            Wgrydata datajson = new Wgrydata();
            try
            {
                string zjzbh = Request["zjzbh"].GetSafeString();
                string wgptbh = Request["wgptbh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();

                if (wgptbh != "" && zjzbh!="")
                {
                    string where = " where 1=1 ";

                    if (gcbh != "")
                        where += " and A.gcbh_yc = '" + gcbh + "'";
                    if (zjzbh != "")
                        where += " and  '," + zjzbh + ",' like '%,'+A.zjzbh+',%'";
                    if (wgptbh != "")
                        where += " and A.wgptbh = '" + wgptbh + "'";

                    string sql = "select count(1) as value ";
                    if (zjzbh.Contains(',')) //显示区
                    {
                        sql += ",szxq as name ";
                        where += " group by szxq";          
                    }
                    else //显示街道
                    {
                        sql += ",szjd as name ";
                        where += " group by szjd";       
                    }

                    sql += " from View_I_M_GC_ZS a ";
                    sql += where;
                    datajson.Datas = CommonService.GetDataTable(sql);
                    datajson.Status = "success";
                    datajson.Msg = "";
                }
                
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

        /// <summary>
        /// 工种类别统计数获取
        /// </summary>
        /// <returns></returns>
        public void GetStatisticsGz_YC()
        {
            string msg = "";
            bool code = true;

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string zjzbh = Request["zjzbh"].GetSafeString();
                string wgptbh = Request["wgptbh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();


                if (wgptbh != "" && zjzbh!="")
                {
                    string where = " where  B.Recid IS NOT NULL";

                    if (gcbh != "")
                        where += " and A.gcbh_yc = '" + gcbh + "'";
                    if (zjzbh != "")
                        where += " and  '," + zjzbh + ",' like '%,'+A.zjzbh+',%'";
                    if (wgptbh != "")
                        where += " and A.wgptbh = '" + wgptbh + "'";


                    string sql = "select b.gz as name,count(1) as value from I_M_GC A LEFT JOIN View_I_M_WGRY B ON A.GCBH = B.JDZCH ";
                    sql += where + " group by  b.gz";
                    dt = CommonService.GetDataTable(sql);
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
        /// 地图获取工程一览
        /// </summary>
        /// <returns></returns>
        public void GetGcList_YC()
        {

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();

            try
            {
                string zjzbh = Request["zjzbh"].GetSafeString();
                string wgptbh = Request["wgptbh"].GetSafeString();
                string gcmc = Request["gcmc"].GetSafeString();

                if (wgptbh != "" && zjzbh!="")
                {
                    string where = " where 1=1";
                    if (gcmc != "")
                        where += " and A.gcmc like '%" + gcmc + "%'";
                    if (zjzbh != "")
                        where += " and  '," + zjzbh + ",' like '%,'+A.zjzbh+',%'";
                    if (wgptbh != "")
                        where += " and A.wgptbh = '" + wgptbh + "'";

                    string day = DateTime.Today.ToString("yyyy-MM-dd");

                    string sql = "select A.*,ISNULL(B.ZCRS,0) AS ZCRS,ISNULL(C.ZZRS,0) AS ZZRS,ISNULL(D.LZRS,0) AS LZRS,ISNULL(E.KQRS,0) AS KQRS,ISNULL(F.ZXKQ,0) AS ZXKQ,ISNULL(G.KQJS,0) AS KQJS from View_I_M_GC A ";
                    sql += "LEFT JOIN (SELECT JDZCH,count(1) AS ZCRS FROM I_M_WGRY GROUP BY JDZCH) B ON A.GCBH = B.JDZCH ";
                    sql += "LEFT JOIN (SELECT JDZCH,count(1) AS ZZRS FROM I_M_WGRY WHERE hasdelete = 0 GROUP BY JDZCH) C ON A.GCBH = C.JDZCH ";
                    sql += "LEFT JOIN (SELECT JDZCH,count(1) AS LZRS FROM I_M_WGRY WHERE hasdelete != 0 GROUP BY JDZCH) D ON A.GCBH = D.JDZCH ";
                    sql += "LEFT JOIN (select gcbh, inrynum as KQRS from i_m_gc) E ON A.GCBH = E.gcbh ";
                    sql += "LEFT JOIN (SELECT JDZCH,count(1) AS ZXKQ FROM View_I_M_KQJ WHERE datediff(mi,lastupdate,getdate())>10 GROUP BY JDZCH) F ON A.GCBH = F.JDZCH ";
                    sql += "LEFT JOIN (SELECT JDZCH,count(1) AS KQJS FROM View_I_M_KQJ GROUP BY JDZCH) G ON A.GCBH = G.JDZCH ";
                    sql += where;

                    dt = CommonService.GetDataTable(sql);
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
                //Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", "0", "", dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }

        /// <summary>
        /// 人员工种投诉统计
        /// </summary>
        public void GetGcRYTSTJ_YC()
        {

            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            string msg="";
            bool code=false;
            try
            {
                string zjzbh = Request["zjzbh"].GetSafeString();
                string wgptbh = Request["wgptbh"].GetSafeString();
                string gcbh = Request["gcbh"].GetSafeString();
                if (wgptbh != "" && zjzbh != "")
                {
                    string where = " where b.gcbh is not null ";
                    if (zjzbh != "")
                        where += " and  '," + zjzbh + ",' like '%,'+b.zjzbh+',%'";
                    if (wgptbh != "")
                        where += " and b.wgptbh = '" + wgptbh + "'";
                    if (gcbh != "")
                        where += " and b.gcbh = '" + gcbh + "'";

                    string day = DateTime.Today.ToString("yyyy-MM-dd");

                    string sql = "select count(1) as num,a.gz from i_m_ry_ts a left join i_m_gc b on a.jdzch=b.gcbh ";
                    sql+= where;
                    sql+="group by a.gz,a.gz";

                    dt = CommonService.GetDataTable(sql);
                    code=true;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg=e.Message;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                //Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}}}", totalcount, jss.Serialize(dt)));
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"count\":{2},\"data\":{3}}}", code?"0":"1", msg, dt.Count, jss.Serialize(dt)));
                Response.End();
            }
        }
        #endregion
    }
}