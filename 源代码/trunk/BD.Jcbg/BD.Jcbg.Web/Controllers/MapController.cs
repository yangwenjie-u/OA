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

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 地图控制器
    /// </summary>
    public class MapController : Controller
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

        private  IWorkFlowService _workflowService = null;
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
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取地图上显示的模块列表
        /// </summary>
        public void GetModuleList()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try 
            {
                // 获取所有启用的模块
                string sql = "SELECT * FROM dbo.MapModule WHERE InUse=1 ORDER BY DisplayOrder";
                data = CommonService.GetDataTable2(sql);
                foreach (var module in data)
                {

                    IList<IDictionary<string, object>> searchfileds = new List<IDictionary<string, object>>();
                    IList<IDictionary<string, object>> urls = new List<IDictionary<string, object>>();
                    IList<IDictionary<string, object>> searchitems = new List<IDictionary<string, object>>();
                    string moduleid = module["moduleid"].GetSafeString();
                    if (moduleid !="")
                    {
                        sql = string.Format("select * from mapsearch where lower(moduleid)='{0}' order by displayorder ", moduleid);
                        searchfileds = CommonService.GetDataTable2(sql);
                        sql = string.Format("select * from mapurl where lower(moduleid)='{0}' and inuse=1 order by displayorder ", moduleid);
                        urls = CommonService.GetDataTable2(sql);
                        sql = string.Format("select * from MapSearchItems where lower(moduleid)='{0}' and inuse=1 order by displayorder ", moduleid);
                        searchitems = CommonService.GetDataTable2(sql);

                    }
                    module["searchfields"] = searchfileds;
                    module["urls"] = urls;
                    module["searchitems"] = searchitems;
                }

            }
            catch (Exception e )
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);    
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
            }
        }

        public void GetDataList()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            int totalcount = 0;
            try
            {
                string moduleid = Request["moduleid"].GetSafeRequest().ToLower();
                string tablename = Request["tablename"].GetSafeRequest().ToLower();
                int pageindex = Request["page"].GetSafeInt(1);
                int pagesize = Request["rows"].GetSafeInt(20);
                string where = Request["where"].GetSafeString();
                if (moduleid == "" )
                {
                    code = false;
                    msg = "模块名称不能为空!";

                }
                else if (tablename == "")
                {
                    code = false;
                    msg = "数据表名称不能为空!";
                }
                else
                {
                    string wherestr = " 1=1 ";

                    // 拼接查询条件
                    if (where != "")
                    {
                        
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = 10240000;
                        Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(where);
                        if (dt != null && dt.Keys.Count > 0)
                        {
                            string sql = string.Format("select fieldname, fieldoperation from mapsearch where lower(moduleid)='{0}' ", moduleid);
                            IList<IDictionary<string, object>> searchfields = CommonService.GetDataTable2(sql);
                            foreach (var f in searchfields)
                            {
                                string fieldname = f["fieldname"].GetSafeString().ToLower();
                                string op = f["fieldoperation"].GetSafeString().ToLower();
                                if (fieldname != "" && op != "")
                                {
                                    if (dt.Keys.Where(x => x.ToLower() == fieldname).Count() > 0 && dt[fieldname].GetSafeString()!="")
                                    {
                                        switch (op)
                                        {
                                            case "like":
                                                wherestr += string.Format(" and ( {0} like '%{1}%' ) ", fieldname, dt[fieldname].GetSafeString());
                                                break;
                                            case "=":
                                                wherestr += string.Format(" and ( {0} = '{1}' ) ", fieldname, dt[fieldname].GetSafeString());
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }

                        }
                    }

                    string s = string.Format("select * from {0} where {1}", tablename, wherestr);
                    list = CommonService.GetPageData2(s, pagesize, pageindex, out totalcount);
                    

                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("total", totalcount);
                data.Add("rows", list);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
            }
        }
        /// <summary>
        /// 周边搜索
        /// </summary>
        public void GetDataListInBounds()
        {

            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            try
            {
                string moduleid = Request["moduleid"].GetSafeString().ToLower();
                string searchitemid = Request["searchitemid"].GetSafeString().ToLower();
                string longitude = Request["longitude"].GetSafeString();
                string latitude = Request["latitude"].GetSafeString();
                string disc = Request["disc"].GetSafeString();
                if (moduleid == "")
                {
                    code = false;
                    msg = "模块名称不能为空!";

                }
                else if (searchitemid == "")
                {
                    code = false;
                    msg = "搜索项不能为空!";
                }
                else if (longitude == "" || latitude == "")
                {
                    code = false;
                    msg = "经度与纬度不能为空!";
                }
                else
                {
                    string sql = string.Format("select * from mapsearchitems where lower(moduleid)='{0}' and searchitemid='{1}' ", moduleid, searchitemid);
                    IList<IDictionary<string, string>> searchitems = CommonService.GetDataTable(sql);
                    if (searchitems.Count > 0)
                    {
                        string tablename = searchitems[0]["searchtable"];
                        string longitudefield = searchitems[0]["searchlongitude"];
                        string latitudefield = searchitems[0]["searchlatitude"];
                        sql = string.Format(
                            "select * from ( select *, dbo.GetDistance({0}, {1}, {2}, {3})  as disc from {4} ) t where disc < {5}", 
                            longitudefield,
                            latitudefield,
                            longitude,
                            latitude,
                            tablename,
                            disc
                            );
                        list = CommonService.GetDataTable2(sql);
                    }
                    else
                    {
                        code = false;
                        msg = "搜索项不存在!";
                    }
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength  = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(list)));
            }

        }

        public void GetCharts()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            try
            {
                string moduleid = Request["moduleid"].GetSafeString().ToLower();
                string panelid = Request["panelid"].GetSafeString().ToLower();
                if (moduleid =="")
                {
                    moduleid = "ALL";
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from mapchart where moduleid='{0}' ", moduleid);
                if (panelid !="")
                {
                    sb.AppendFormat(" and panelid='{0}' ", panelid);
                }
                sb.Append(" and inuse=1 order by displayorder ");
                string sql = sb.ToString();
                IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                if (d.Count > 0)
                {
                    foreach (var row in d)
                    {
                        string mid = row["moduleid"].GetSafeString();
                        string pid = row["panelid"].GetSafeString();
                        string chartid = row["chartid"].GetSafeString();

                        IDictionary<string, object> c = new Dictionary<string, object>();
                        c.Add("panelid", pid);
                        c.Add("charts", new Dictionary<string, object>() { { "rows", GetChartData(chartid) } });
                        list.Add(c);
                    }
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(list)));
            }
        }

        private Dictionary<string, object> GetChartData(string chartid)
        {
            Dictionary<string, object> cdata = null;
            try
            {
                chartid = chartid.GetSafeString();
                if (chartid !="")
                {
                    string sql = string.Format("select * from mapchart where chartid='{0}' and inuse=1", chartid);
                    IList<IDictionary<string, object>> d = CommonService.GetDataTable2(sql);
                    if (d.Count > 0)
                    {
                        cdata = new Dictionary<string, object>();
                        string id = d[0]["chartid"].GetSafeString();
                        string title = d[0]["title"].GetSafeString();
                        string type = d[0]["type"].GetSafeString();
                        cdata.Add("charttitle", title);
                        cdata.Add("type", type);

                        #region 柱状图和折线图
                        if (type == "bar" || type == "line")
                        {
                            // 获取XDATA（X轴数据）
                            List<string> xdata = new List<string>();
                            sql = string.Format("select * from mapchartx where chartid='{0}' and inuse=1 ", chartid);
                            IList<IDictionary<string, object>> d2 = CommonService.GetDataTable2(sql);
                            if (d2.Count > 0)
                            {
                                string sourcetype = d2[0]["sourcetype"].GetSafeString();
                                string source = d2[0]["source"].GetSafeString();
                                if (source != "")
                                {
                                    sourcetype = sourcetype.ToUpper();
                                    if (sourcetype == "FIXED")
                                    {
                                        xdata = source.Split(new char[] { '|' }).ToList();
                                    }
                                    else if (sourcetype == "SQL")
                                    {

                                    }
                                    else if (sourcetype == "PROC")
                                    {

                                    }

                                }
                            }
                            cdata.Add("xdata", xdata);

                            // 获取series数据（Y轴数据）
                            List<Dictionary<string, object>> series = new List<Dictionary<string, object>>();
                            sql = string.Format("select * from mapcharty where chartid='{0}' and inuse=1 order by displayorder ", chartid);
                            IList<IDictionary<string, object>> d3 = CommonService.GetDataTable2(sql);
                            if (d3.Count > 0)
                            {
                                foreach (var row in d3)
                                {
                                    Dictionary<string, object> y = new Dictionary<string, object>();

                                    string sourcetype = row["sourcetype"].GetSafeString();
                                    string source = row["source"].GetSafeString();
                                    string name = row["name"].GetSafeString();
                                    string color = row["color"].GetSafeString();
                                    List<string> ydata = new List<string>();

                                    y.Add("name", name);
                                    y.Add("color", color);

                                    if (source != "")
                                    {
                                        sourcetype = sourcetype.ToUpper();
                                        if (sourcetype == "FIXED")
                                        {
                                            ydata = source.Split(new char[] { '|' }).ToList();
                                        }
                                        else if (sourcetype == "SQL")
                                        {

                                        }
                                        else if (sourcetype == "PROC")
                                        {

                                        }

                                    }
                                    y.Add("data", ydata);

                                    series.Add(y);

                                }
                            }
                            cdata.Add("series", series);
                        }
                        #endregion

                        #region 饼图
                        else if (type == "pie")
                        {
                            List<Dictionary<string, object>> series = new List<Dictionary<string, object>>();
                            sql = string.Format("select * from mapchary where chartid='{0}' and inuse=1 order by displayorder ", chartid);
                            IList<IDictionary<string, object>> d4 = CommonService.GetDataTable2(sql);
                            if (d4.Count > 0)
                            {
                                foreach (var row in d4)
                                {
                                    Dictionary<string, object> y = new Dictionary<string, object>();

                                    string sourcetype = row["sourcetype"].GetSafeString();
                                    string source = row["source"].GetSafeString();
                                    string name = row["name"].GetSafeString();
                                    string color = row["color"].GetSafeString();
                                    string ydata = "";

                                    y.Add("name", name);
                                    y.Add("color", color);

                                    if (source != "")
                                    {
                                        sourcetype = sourcetype.ToUpper();
                                        if (sourcetype == "FIXED")
                                        {
                                            ydata = source;
                                        }
                                        else if (sourcetype == "SQL")
                                        {

                                        }
                                        else if (sourcetype == "PROC")
                                        {

                                        }

                                    }
                                    y.Add("value", ydata);

                                    series.Add(y);

                                }
                            }
                            cdata.Add("series", series);
                        }
                        #endregion




                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return cdata;

        }




        public void GetMapGCTJXX()
        {
            bool code = true;
            string msg = "";
            IDictionary<string, object> dt = new Dictionary<string, object>();
            
            try
            {
                #region 获取全市概况
                IDictionary<string, string> qsgk = new Dictionary<string, string>();
                IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                string proc = string.Format("GetMapGCTJXX()");
                list = CommonService.ExecDataTableProc(proc, out msg);
                if (list.Count > 0)
                {
                    qsgk = list[0];
                }
                dt.Add("qsgk", qsgk);
                #endregion

                #region 2018年在建工程和工程类型统计
                Dictionary<string, object> zjgclxtj = new Dictionary<string, object>();
                zjgclxtj.Add("static", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "factory_data", new List<int>() { 20,30,60}}
                    },
                    new Dictionary<string, object>( ) {
                        { "govern_data", new List<int>() { 20,70,60}}
                    },
                    new Dictionary<string, object>( )
                    {
                        { "house_data", new List<int>() { 20,80,60}}
                    }
                });
                zjgclxtj.Add("alldata", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>() {
                        { "value",600},
                        { "name","房建工程"}
                    },
                    new Dictionary<string, object>() {
                        { "value",600},
                        { "name","市政工程"}
                    },
                    new Dictionary<string, object>() {
                        { "value",700},
                        { "name","厂房工程"}
                    }
                });
                zjgclxtj.Add("singledata", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>() {
                        { "value",300},
                        { "name","在建"}
                    },
                    new Dictionary<string, object>() {
                        { "value",200},
                        { "name","异常"}
                    },
                    new Dictionary<string, object>() {
                        { "value",100},
                        { "name","竣工"}
                    },
                    new Dictionary<string, object>() {
                        { "value",300},
                        { "name","竣工"}
                    },
                    new Dictionary<string, object>() {
                        { "value",300},
                        { "name","在建"}
                    },
                    new Dictionary<string, object>() {
                        { "value",500},
                        { "name","竣工"}
                    },
                    new Dictionary<string, object>() {
                        { "value",200},
                        { "name","在建"}
                    }
                });
                dt.Add("zjgclxtj", zjgclxtj);
                #endregion

                #region 2012-2017工程数量统计
                Dictionary<string, object> gcsltj = new Dictionary<string, object>();
                gcsltj.Add("data", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "所有工程"},
                        { "allenginner", new List<int>() { 40,50,50,30,50,70}}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "竣工工程"},
                        { "finish_enginner", new List<int>() { 30,40,40,20,40,60}}
                    }
                });
                dt.Add("gcsltj", gcsltj);
                #endregion

                #region 2018年不合格统计报告/2018异常报告统计/2018年工程异常报告
                Dictionary<string, object> bgtj = new Dictionary<string, object>();
                bgtj.Add("data", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "总工程数"},
                        { "value", 100}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "不合格数"},
                        { "value", 5}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "异常数"},
                        { "value", 15}
                    }
                });
                bgtj.Add("errorenginner", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "假样品"},
                        { "value", 335}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "规定项目视频未上传"},
                        { "value", 310}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "考勤机未启动"},
                        { "value", 234}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "实验员未到场"},
                        { "value", 135}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "某个实验有重做"},
                        { "value", 548}
                    }
                });
                dt.Add("bgtj", bgtj);
                #endregion

                #region 在岗人员统计和各工种在岗人员统计
                Dictionary<string, object> zgrytj = new Dictionary<string, object>();
                zgrytj.Add("work_people", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "在岗人数"},
                        { "nowvalue", 304}
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "总人数"},
                        { "countvalue", 355}
                    }
                });
                zgrytj.Add("Workpersonnel", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "all_data", new List<int>() { 40, 70, 95, 40, 50 } }
                    },
                    new Dictionary<string, object>( ) {
                        { "work_data", new List<int>() { 26, 59, 90, 26, 28 } }
                    }
                });
                dt.Add("zgrytj", zgrytj);
                #endregion

                #region 各工种出勤率统计
                Dictionary<string, object> ggzcqltj = new Dictionary<string, object>();
                ggzcqltj.Add("data", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "平均信誉值"},
                        { "averagereputation", new List<int>() { 320, 302, 301, 334, 390, 330, 320, 220, 210 } }
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "最低信誉值"},
                        { "minimumredit", new List<int>() { 87, 92, 81, 72, 65, 90, 54, 48, 68 } }
                    }
                });
                dt.Add("ggzcqltj", ggzcqltj);
                #endregion

                #region 2018年工种计分与调离情况
                Dictionary<string, object> gzjfdl = new Dictionary<string, object>();
                gzjfdl.Add("data", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "latecount", new List<int>() { 30, 40, 50, 30, 60, 70, 80, 59, 120 } }
                    },
                    new Dictionary<string, object>( ) {
                        { "latemoney", new List<int>() { 30, 40, 50, 30, 60, 70, 80, 59, 120 } }
                    }
                });
                dt.Add("gzjfdl", gzjfdl);
                #endregion

                #region 务工人员比例和上访人员情况
                Dictionary<string, object> wgryblsfry = new Dictionary<string, object>();
                wgryblsfry.Add("workers", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "木工" },
                        { "value", 335 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "钢筋工" },
                        { "value", 310 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "粉刷工" },
                        { "value", 234 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "架子工" },
                        { "value", 135 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "水电安装" },
                        { "value", 548 },
                    }
                });
                wgryblsfry.Add("visitors", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "木工" },
                        { "value", 335 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "钢筋工" },
                        { "value", 310 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "粉刷工" },
                        { "value", 234 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "架子工" },
                        { "value", 135 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "水电安装" },
                        { "value", 548 },
                    }
                });
                dt.Add("wgryblsfry", wgryblsfry);
                #endregion

                #region 地图返回数据
                Dictionary<string, object> mappoints = new Dictionary<string, object>();
                mappoints.Add("points", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>  {
                        { "lng",120.2399974096},
                        { "lat",29.7089369885},
                        { "name","在建"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.2289858187},
                        { "lat",29.7887338502},
                        { "name","整改"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.3049038510},
                        { "lat",29.9053888775},
                        { "name","停工"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.1535083828},
                        { "lat",29.7083361206},
                        { "name","异常"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.1777851385},
                        { "lat",29.5073905124},
                        { "name","在建"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.2330108065},
                        { "lat",29.6052290559},
                        { "name","在建"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.2329404028},
                        { "lat",29.5003766439},
                        { "name","在建"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.2035083828},
                        { "lat",29.7583361206},
                        { "name","异常"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.5035083828},
                        { "lat",29.7683361206},
                        { "name","异常"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.3035083828},
                        { "lat",29.7783361206},
                        { "name","异常"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.2189858187},
                        { "lat",29.8087338502},
                        { "name","整改"}
                    },

                    new Dictionary<string, object>  {
                        { "lng",120.2289858187},
                        { "lat",29.7387338502},
                        { "name","整改"}
                    }
                });
                
                dt.Add("mappoints", mappoints);
                #endregion

                #region 企业四个图返数据
                Dictionary<string, object> qytjxx = new Dictionary<string, object>();
                qytjxx.Add("factory", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "总企业" },
                        { "allfactoryvalue", 335 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "在岗企业" },
                        { "existvalue", 304 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "所有单位" },
                        { "factory_all", new List<int>() { 26, 30, 45, 40, 28 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "在岗单位" },
                        { "factory_exist", new List<int>() { 22, 26, 40, 38, 25 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "最低信誉值" },
                        { "factory_mix", new List<int>() { 45, 65, 79, 64, 85 } },
                    }
                    ,
                    new Dictionary<string, object>( ) {
                        { "name", "平均信誉值" },
                        { "factory_average", new List<int>() { 82, 80, 95, 80, 85 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "拖欠次数" },
                        { "money_count", new List<int>() { 2, 4, 1, 2 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "拖欠金额" },
                        { "money_num", new List<int>() { 40, 80, 40, 20 } },
                    }
                });
                dt.Add("qytjxx", qytjxx);
                #endregion

                #region 企业四个图返数据
                Dictionary<string, object> sbtjxx = new Dictionary<string, object>();
                sbtjxx.Add("equment", new List<Dictionary<string, object>>() {
                    new Dictionary<string, object>( ) {
                        { "name", "总设备" },
                        { "allequmentvalue", 335 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "在线设备" },
                        { "equmentvalue", 170 },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "在线设备" },
                        { "online_equment", new List<int>() { 35, 35, 35 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "未使用设备" },
                        { "outline_equment", new List<int>() { 22, 22, 22 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "检查记录" },
                        { "checkrecord", new List<int>() { 98, 82, 85, 90, 80, 90 } },
                    },
                    new Dictionary<string, object>( ) {
                        { "name", "维保记录" },
                        { "checkfixed", new List<int>() { 45, 60, 50, 57, 59, 60 } },
                    }
                });
                dt.Add("sbtjxx", sbtjxx);
                #endregion

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(dt)));
            }
        }
















        #endregion



    }
}