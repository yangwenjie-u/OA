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
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.Functions;
using NPOI.POIFS.FileSystem;
using NPOI.HPSF;
using NPOI.SS.Util;
using System.Drawing;
using NPOI.HSSF.Util;
using BD.Jcbg.Web.Func;
using System.Threading;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Collections;
using System.Data.SqlClient;
//using NPOI.XWPF.UserModel;

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 温州瓯海监督站个性化控制器
    /// </summary>
    public class DwgxWzOhController : Controller
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

        private ISmsServiceWzzjz _smsServiceWzzjz = null;
        private ISmsServiceWzzjz SmsServiceWzzjz
        {
            get
            {
                if (_smsServiceWzzjz == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsServiceWzzjz = webApplicationContext.GetObject("SmsServiceWzzjz") as ISmsServiceWzzjz;
                }
                return _smsServiceWzzjz;
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

        private IMySqlService _mySqlService = null;
        private IMySqlService MySqlService
        {
            get
            {
                if (_mySqlService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _mySqlService = webApplicationContext.GetObject("WzzjzMySqlService") as IMySqlService;
                }
                return _mySqlService;
            }
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取工程的单位及单位工程负责人
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetGcdwfzr(string gcbh)
        {
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string sql = "(select '建设单位' as lx,qymc,qyfr," +
                             "       xmfzr = STUFF ((select  ',' + ryxm from i_s_gc_jsry where i_s_gc_jsry.qybh=i_s_gc_jsdw.gcqybh and gw='建设单位项目负责人' FOR XML PATH('')), 1, 1, '') " +
                             "   from i_s_gc_jsdw where gcbh='" + gcbh + "') " +
                             "   union ALL " +
                             "   ( " +
                             "   select '勘查单位' as lx,qymc,qyfr, " +
                             "       xmfzr = STUFF ((select  ',' + ryxm from i_s_gc_kcry where i_s_gc_kcry.qybh=i_s_gc_kcdw.gcqybh and gw='勘察单位项目负责人' FOR XML PATH('')), 1, 1, '')  " +
                             "   from i_s_gc_kcdw where gcbh='" + gcbh + "') " +
                             "   union ALL " +
                             "   ( " +
                             "   select '设计单位' as lx,qymc,qyfr, " +
                             "       xmfzr = STUFF ((select  ',' + ryxm from i_s_gc_sjry where i_s_gc_sjry.qybh=i_s_gc_sjdw.gcqybh and gw='设计单位项目负责人' FOR XML PATH('')), 1, 1, '')  " +
                             "   from i_s_gc_sjdw where gcbh='" + gcbh + "') " +
                             "   union ALL " +
                             "   ( " +
                             "   select '监理单位' as lx,qymc,qyfr, " +
                             "       xmfzr = STUFF ((select  ',' + ryxm from i_s_gc_jlry where i_s_gc_jlry.qybh=i_s_gc_jldw.gcqybh and gw='项目总监' FOR XML PATH('')), 1, 1, '')  " +
                             "   from i_s_gc_jldw where gcbh='" + gcbh + "') " +
                             "   union ALL " +
                             "   ( " +
                             "   select '施工单位' as lx,qymc,qyfr, " +
                             "       xmfzr = STUFF ((select  ',' + ryxm from i_s_gc_sgry where i_s_gc_sgry.qybh=i_s_gc_sgdw.gcqybh and gw='项目经理' FOR XML PATH('')), 1, 1, '')  " +
                             "   from i_s_gc_sgdw where gcbh='" + gcbh + "') " +
                             "   union ALL " +
                             "   ( " +
                             "   select '图审单位' as lx,qymc,qyfr, " +
                             "       '' as xmfzr  " +
                             "   from i_s_gc_tsdw where gcbh='" + gcbh + "') ";
                dt = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(dt);
        }
        /// <summary>
        /// 获取工程抽查次数
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetGccccs(string gcbh)
        {
            int sum = 0;
            try
            {
                string sql = "select count(*) as sum1 from JDBG_YSAPJL where gcbh='" + gcbh + "' and ysry is not null and ysry<>''";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                    sum = dt[0]["sum1"].GetSafeInt();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(new { sum = sum });
        }

        /// <summary>
        /// 根据多个质监登记号，获取工程抽查次数
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        public JsonResult GetGccccsByZjdjhlist(string zjdjhlist)
        {
            int sum = 0;
            try
            {
                if (zjdjhlist !="")
                {
                    string[] bhlist = zjdjhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (bhlist.Count() > 0)
                    {
                        zjdjhlist = string.Join(",", bhlist);
                        string sql = "select count(*) as sum1 from JDBG_YSAPJL where zjdjh in (" + DataFormat.FormatSQLInStr(zjdjhlist) + ") and ysry is not null and ysry<>''";
                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                            sum = dt[0]["sum1"].GetSafeInt();
                    }
                   
                }
               
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(new { sum = sum });
        }
        /// <summary>
        /// 获取工程备注
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetGcbz(string gcbh)
        {
            string bz = "";
            try
            {
                IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
                //string sql = "select bz from i_m_gc where gcbh='" + gcbh + "'";
                //dt = CommonService.GetDataTable(sql);
                //if (dt.Count > 0)
                //    bz = dt[0]["bz"].GetSafeString();
                dt = CommonService.GetDataTable("select remark from view_i_s_gc_bz where GCBH='" + gcbh + "' and ZT=1");
                for (int i = 0; i < dt.Count; i++)
                {
                    bz += " " + dt[i]["remark"].GetSafeString();
                }
                bz = bz.EncodeBase64();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(new { bz = bz });
        }


        /// <summary>
        /// 获取工程备注
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetGcbzbyzjdjhlist(string zjdjhlist)
        {
            string bz = "";
            try
            {
                if (zjdjhlist != "")
                {
                    string[] bhlist = zjdjhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (bhlist.Count() > 0)
                    {
                        zjdjhlist = string.Join(",", bhlist);
                        string sql = string.Format("GetGcBzByZjdjhlist('{0}')", zjdjhlist);
                        string msg = "";
                        IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(sql, out msg);
                        if (dt.Count > 0)
                        {
                            foreach (var row in dt)
                            {
                                if (bz!="")
                                {
                                    bz += "\r\n";
                                }
                                bz += row["zjdjh"].GetSafeString() + "：" + row["bz"].GetSafeString();
                            }
                        }
                        bz = bz.EncodeBase64();

                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            finally { }
            return Json(new { bz = bz });
        }

        /// <summary>
        /// 这个是获取首页title的，跑马灯效果提示
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetSysTitle()
        {
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                //dts = JdbgService.GetProblemContents(workserial, CurrentUser.UserName, "1");
                dts = CommonService.GetDataTable("select remark from Sys_title order by recid desc");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(dts);
        }


        /// <summary>
        /// 获取五方主体一样的工程
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetSameGC(string gcbh)
        {

            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {

                //dts = JdbgService.GetProblemContents(workserial, CurrentUser.UserName, "1");
                dts = CommonService.GetDataTable("select gcmc,gcbh,zjdjh from View_I_M_GC_LB where zt not in ('YT','LR','JDBG','GDZL') and isnumeric(zt)=0 and gcbh!='" + gcbh + "' and SY_JSDWMC=(select SY_JSDWMC from View_I_M_GC_LB where gcbh='" + gcbh + "') and SGDWMC=(select SGDWMC from View_I_M_GC_LB where gcbh='" + gcbh + "') and JLDWMC=(select JLDWMC from View_I_M_GC_LB where gcbh='" + gcbh + "') and KCDWMC=(select KCDWMC from View_I_M_GC_LB where gcbh='" + gcbh + "') and SJDWMC=(select SJDWMC from View_I_M_GC_LB where gcbh='" + gcbh + "')");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {

            }
            return Json(dts);
        }


        /// <summary>
        /// 获取修改监督记录，监督报告的流程和父流程的基本信息
        /// </summary>
        /// <param name="gcbh"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetJDBGJDJLModifyInfo(string serialno, string lx, string createuser)
        {
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            int processid = 0;
            string extrainfo1 = "";
            string extrainfo2 = "";
            string extrainfo3 = "";
            string extrainfo4 = "";
            string extrainfo5 = "";
            string extrainfo6 = "";
            string msg = "";
            try
            {

                StForm form = WorkFlowService.GetForm(serialno);
                if (form.DoState.Value == 0)
                {
                    processid = 0;
                    msg = "当前记录还没有审批完成，请审批人员退回或则等待任务完成再修改";
                }
                else if (createuser != CurrentUser.UserName)
                {
                    processid = 0;
                    msg = "你不能修改别人的记录";
                }
                else
                {

                    string sql = "select (select processid from H_JDBGJDJL_Modify where JDBGJDJLLX='" + lx + "') as processid,a.extrainfo1,a.extrainfo2,a.extrainfo3,a.extrainfo4,a.extrainfo5,a.extrainfo6 from stform a where a.serialno='" + serialno + "' ";
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        processid = dt[0]["processid"].GetSafeInt(0);
                        extrainfo1 = dt[0]["extrainfo1"].GetSafeString();
                        extrainfo2 = dt[0]["extrainfo2"].GetSafeString();
                        extrainfo3 = dt[0]["extrainfo3"].GetSafeString();
                        extrainfo4 = dt[0]["extrainfo4"].GetSafeString();
                        extrainfo5 = dt[0]["extrainfo5"].GetSafeString();
                        extrainfo6 = dt[0]["extrainfo6"].GetSafeString();
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally { }
            return Json(new { processid = processid, extrainfo1 = extrainfo1, extrainfo2 = extrainfo2, extrainfo3 = extrainfo3, extrainfo4 = extrainfo4, extrainfo5 = extrainfo5, extrainfo6 = extrainfo6, msg = msg });
        }

        /// <summary>
        /// 获取整改单企业扣分列表数据, 用于整改处罚申请第一步
        /// </summary>
        public void GetZGDQYKFLB()
        {
            bool ret = true;
            string msg = "";
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                if (zgdbhlist != "" && currentcfsj != "" && lastcfsj != "")
                {
                    string[] zgds = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (zgds.Count() > 0)
                    {
                        zgdbhlist = string.Join(",", zgds);
                        string procstr = "";
                        procstr = string.Format("GetZGDQYKFLB('{0}','{1}','{2}')", zgdbhlist, currentcfsj, lastcfsj);
                        dts = CommonService.ExecDataTableProc(procstr, out msg);
                    }

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\"}}", dts.Count, jss.Serialize(dts), ret ? "0" : "1"));
                Response.End();
            }

        }

        /// <summary>
        /// 导出整改单企业扣分列表
        /// </summary>
        public void ExportZGDCFQYKFLB()
        {
            string msg = "";
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            IList<IDictionary<string, string>> qykflb = new List<IDictionary<string, string>>();
            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {
                //居中样式
                ICellStyle cellstyle = wk.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.WrapText = true;




                #region 标题配置
                Dictionary<string, Dictionary<string, object>> st = new Dictionary<string, Dictionary<string, object>>() {
                    {
                        "房建总承包", new Dictionary<string, object>() {
                            {
                                "kfdwxz", "施工企业"
                            },
                            {
                                "heads", new List<string>() {
                                    "序号",
                                    "施工单位",
                                    "份数",
                                    "监督登记号",
                                    "工程名称",
                                    "建设单位",
                                    "整改单通知编号",
                                    "扣分值",
                                    "备注",
                                }
                            }
                        }
                    },
                    {
                        "监理企业", new Dictionary<string, object>() {
                            {
                                "kfdwxz", "监理企业"
                            },
                            {
                                "heads", new List<string>() {
                                    "序号",
                                    "监理单位",
                                    "份数",
                                    "监督登记号",
                                    "工程名称",
                                    "建设单位",
                                    "整改单通知编号",
                                    "扣分值",
                                    "备注",
                                }
                            }
                        }
                    }
                };
                #endregion


                int cols = 0;
                int rows = 0;
                IRow row;
                ICell cell;

                #region 生成工作表和标题

                foreach (var item in st)
                {
                    ISheet sheet = wk.CreateSheet(item.Key);
                    Dictionary<string, object> config = item.Value;
                    List<string> heads = config["heads"] as List<string>;
                    //定义导出标题
                    row = sheet.CreateRow(rows);
                    row.Height = 50 * 20;


                    for (cols = 0; cols < heads.Count; cols++)
                    {
                        sheet.SetColumnWidth(cols, 20 * 256);
                        // 定义每一列
                        cell = row.CreateCell(cols);
                        //设置值
                        cell.SetCellValue(heads[cols]);
                        //设置样式
                        cell.CellStyle = cellstyle;
                    }
                    sheet.CreateFreezePane(1, 1, 1, 1);
                }
                #endregion

                #region 获取数据
                string serial = "";
                if (zgdbhlist != "" && currentcfsj != "" && lastcfsj != "")
                {
                    string sql = string.Format("select top 1 workserial from JDBG_ZGDCFSQJL where spzt is null and cfsj='{0}' and cflx='YQ' order by recid desc", currentcfsj);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        serial = dt[0]["workserial"].GetSafeString();
                    }
                    if (serial == "")
                    {
                        string[] zgds = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (zgds.Count() > 0)
                        {
                            zgdbhlist = string.Join(",", zgds);
                            string procstr = "";
                            procstr = string.Format("GetZGDQYKFLB('{0}','{1}','{2}')", zgdbhlist, currentcfsj, lastcfsj);
                            qykflb = CommonService.ExecDataTableProc(procstr, out msg);
                        }
                    }
                    else
                    {
                        string proc = string.Format("GetZGDQYKFLBBYSERIAL('{0}')", serial);
                        qykflb = CommonService.ExecDataTableProc(proc, out msg);
                    }

                }
                #endregion

                #region 将数据写入工作表
                if (qykflb.Count > 0)
                {
                    foreach (var item in st)
                    {
                        ISheet sheet = wk.GetSheet(item.Key);
                        Dictionary<string, object> config = item.Value;
                        string kfdwxz = config["kfdwxz"] as string;
                        List<string> heads = config["heads"] as List<string>;
                        var datalist = qykflb.Where(x => x["kfdwxz"] == kfdwxz).ToList();
                        // excel表格中的当前行数
                        int count = 0;
                        // 序号
                        int xh = 0;
                        foreach (var data in datalist)
                        {
                            var gczgdlist = data["zgdbhlist"].Split(new char[] { ',' });
                            var zjdjhlist = data["zjdjhlist"].Split(new char[] { ',' });
                            var gcmclist = data["gcmclist"].Split(new char[] { ',' });
                            var jsdwmclist = data["jsdwmclist"].Split(new char[] { ',' });
                            var bzlist = data["bz"].Split(new char[] { ',' });
                            var ykfzlist = data["ykfz"].Split(new char[] { ',' });
                            // 当前记录实际占的行数
                            int rowcount = data["fs"].GetSafeInt();
                            xh++;
                            if (serial == "")
                            {
                                #region 插入单行
                                // 一个整改单一行
                                if (rowcount == 1)
                                {
                                    count++;
                                    row = sheet.CreateRow(count);
                                    row.Height = 50 * 20;
                                    // 定义每一列
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        cell = row.CreateCell(cols);
                                        string v = "";
                                        //设置值
                                        switch (cols)
                                        {
                                            case 0:
                                                v = xh.ToString();
                                                break;
                                            case 1:
                                                v = data["dwmc"];
                                                break;
                                            case 2:
                                                v = data["fs"];
                                                break;
                                            case 3:
                                                v = data["zjdjhlist"];
                                                break;
                                            case 4:
                                                v = data["gcmclist"];
                                                break;
                                            case 5:
                                                v = data["jsdwmclist"];
                                                break;
                                            case 6:
                                                v = data["zgdbhlist"];
                                                break;
                                            case 7:
                                                v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                break;
                                            case 8:
                                                v = data["bz"].Replace(data["zgdbhlist"] + ":", "");
                                                break;
                                            default:
                                                break;
                                        }
                                        cell.SetCellValue(v);
                                        //设置样式
                                        cell.CellStyle = cellstyle;
                                    }


                                }
                                #endregion
                                #region 插入多行
                                // 多个整改单的话，需要插入多行数据
                                else if (rowcount > 1)
                                {
                                    int num = rowcount;
                                    int start = count + 1;
                                    int end = start + num - 1;
                                    //（序号，单位名称，份数）这三列需要合并
                                    List<int> mergedcols = new List<int>() { 0, 1, 2 };
                                    //（监督登记号，工程名称，建设单位）这三列需要合并
                                    List<int> mergedsubcols = new List<int>() { 3, 4, 5 };
                                    // 创建单元格合并区域（序号，单位名称，份数）
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        if (mergedcols.Contains(cols))
                                        {
                                            sheet.AddMergedRegion(new CellRangeAddress(start, end, cols, cols));
                                        }
                                    }
                                    // 创建行
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.CreateRow(i);
                                        row.Height = 50 * 20;
                                    }
                                    // 创建单元格
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.GetRow(i);
                                        for (cols = 0; cols < heads.Count; cols++)
                                        {
                                            string v = "";

                                            if (mergedcols.Contains(cols))
                                            {
                                                if (i == start)
                                                {
                                                    cell = row.CreateCell(cols);

                                                    //设置值
                                                    switch (cols)
                                                    {
                                                        case 0:
                                                            v = xh.ToString();
                                                            break;
                                                        case 1:
                                                            v = data["dwmc"];
                                                            break;
                                                        case 2:
                                                            v = data["fs"];
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    cell.SetCellValue(v);
                                                    //设置样式
                                                    cell.CellStyle = cellstyle;
                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
                                            else
                                            {
                                                int offset = start;
                                                for (int j = 0; j < gczgdlist.Length; j++)
                                                {
                                                    var bhlist = gczgdlist[j].Split(new char[] { '#' });
                                                    int subrow = bhlist.Length;
                                                    if (mergedsubcols.Contains(cols))
                                                    {
                                                        sheet.AddMergedRegion(new CellRangeAddress(offset, offset + subrow - 1, cols, cols));
                                                        cell = sheet.GetRow(offset).CreateCell(cols);
                                                        //设置值
                                                        switch (cols)
                                                        {
                                                            case 3:
                                                                v = zjdjhlist[j];
                                                                break;
                                                            case 4:
                                                                v = gcmclist[j];
                                                                break;
                                                            case 5:
                                                                v = jsdwmclist[j];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        cell.SetCellValue(v);
                                                        //设置样式
                                                        cell.CellStyle = cellstyle;
                                                    }
                                                    else
                                                    {
                                                        if (subrow == 1)
                                                        {
                                                            cell = sheet.GetRow(offset).CreateCell(cols);
                                                            //设置值
                                                            switch (cols)
                                                            {
                                                                case 6:
                                                                    v = bhlist[0];
                                                                    break;
                                                                case 7:
                                                                    v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                                    break;
                                                                case 8:
                                                                    v = bzlist.Where(x => x.StartsWith(bhlist[0] + ":")).FirstOrDefault().GetSafeString().Replace(bhlist[0] + ":", "");
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            cell.SetCellValue(v);
                                                            //设置样式
                                                            cell.CellStyle = cellstyle;
                                                        }
                                                        else if (subrow > 1)
                                                        {
                                                            for (int k = 0; k < subrow; k++)
                                                            {
                                                                cell = sheet.GetRow(offset + k).CreateCell(cols);
                                                                //设置值
                                                                switch (cols)
                                                                {
                                                                    case 6:
                                                                        v = bhlist[k];
                                                                        break;
                                                                    case 7:
                                                                        v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                                        break;
                                                                    case 8:
                                                                        v = bzlist.Where(x => x.StartsWith(bhlist[k] + ":")).FirstOrDefault().GetSafeString().Replace(bhlist[k] + ":", "");
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                cell.SetCellValue(v);
                                                                //设置样式
                                                                cell.CellStyle = cellstyle;
                                                            }

                                                        }
                                                    }

                                                    offset = offset + subrow;
                                                }

                                            }

                                        }
                                    }
                                    // 这里行数需要增加多个（不止一个）
                                    count = count + num;

                                }
                                #endregion
                            }
                            else
                            {
                                #region 插入单行
                                // 一个整改单一行
                                if (rowcount == 1)
                                {
                                    count++;
                                    row = sheet.CreateRow(count);
                                    row.Height = 50 * 20;
                                    // 定义每一列
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        cell = row.CreateCell(cols);
                                        string v = "";
                                        //设置值
                                        switch (cols)
                                        {
                                            case 0:
                                                v = xh.ToString();
                                                break;
                                            case 1:
                                                v = data["dwmc"];
                                                break;
                                            case 2:
                                                v = data["fs"];
                                                break;
                                            case 3:
                                                v = data["zjdjhlist"];
                                                break;
                                            case 4:
                                                v = data["gcmclist"];
                                                break;
                                            case 5:
                                                v = data["jsdwmclist"];
                                                break;
                                            case 6:
                                                v = data["zgdbhlist"];
                                                break;
                                            case 7:
                                                v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                break;
                                            case 8:
                                                v = data["bz"];
                                                break;
                                            default:
                                                break;
                                        }
                                        cell.SetCellValue(v);
                                        //设置样式
                                        cell.CellStyle = cellstyle;
                                    }


                                }
                                #endregion
                                #region 插入多行
                                // 多个整改单的话，需要插入多行数据
                                else if (rowcount > 1)
                                {
                                    int num = rowcount;
                                    int start = count + 1;
                                    int end = start + num - 1;
                                    //（序号，单位名称，份数）这三列需要合并
                                    List<int> mergedcols = new List<int>() { 0, 1, 2 };
                                    //（监督登记号，工程名称，建设单位）这三列需要合并
                                    List<int> mergedsubcols = new List<int>() { 3, 4, 5 };
                                    // 创建单元格合并区域（序号，单位名称，份数）
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        if (mergedcols.Contains(cols))
                                        {
                                            sheet.AddMergedRegion(new CellRangeAddress(start, end, cols, cols));
                                        }
                                    }
                                    // 创建行
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.CreateRow(i);
                                    }
                                    // 创建单元格
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.GetRow(i);
                                        row.Height = 50 * 20;
                                        for (cols = 0; cols < heads.Count; cols++)
                                        {
                                            string v = "";

                                            if (mergedcols.Contains(cols))
                                            {
                                                if (i == start)
                                                {
                                                    cell = row.CreateCell(cols);

                                                    //设置值
                                                    switch (cols)
                                                    {
                                                        case 0:
                                                            v = xh.ToString();
                                                            break;
                                                        case 1:
                                                            v = data["dwmc"];
                                                            break;
                                                        case 2:
                                                            v = data["fs"];
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    cell.SetCellValue(v);
                                                    //设置样式
                                                    cell.CellStyle = cellstyle;
                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
                                            else
                                            {
                                                int offset = start;
                                                for (int j = 0; j < gczgdlist.Length; j++)
                                                {
                                                    var bhlist = gczgdlist[j].Split(new char[] { '#' });
                                                    var kflist = ykfzlist[j].Split(new char[] { '#' });
                                                    var bznrlist = bzlist[j].Split(new char[] { '#' });
                                                    int subrow = bhlist.Length;
                                                    if (mergedsubcols.Contains(cols))
                                                    {
                                                        sheet.AddMergedRegion(new CellRangeAddress(offset, offset + subrow - 1, cols, cols));
                                                        cell = sheet.GetRow(offset).CreateCell(cols);
                                                        //设置值
                                                        switch (cols)
                                                        {
                                                            case 3:
                                                                v = zjdjhlist[j];
                                                                break;
                                                            case 4:
                                                                v = gcmclist[j];
                                                                break;
                                                            case 5:
                                                                v = jsdwmclist[j];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        cell.SetCellValue(v);
                                                        //设置样式
                                                        cell.CellStyle = cellstyle;
                                                    }
                                                    else
                                                    {
                                                        if (subrow == 1)
                                                        {
                                                            cell = sheet.GetRow(offset).CreateCell(cols);
                                                            //设置值
                                                            switch (cols)
                                                            {
                                                                case 6:
                                                                    v = bhlist[0];
                                                                    break;
                                                                case 7:
                                                                    v = kflist[0] != "" ? kflist[0] + "分" : "";
                                                                    break;
                                                                case 8:
                                                                    v = bznrlist[0];
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            cell.SetCellValue(v);
                                                            //设置样式
                                                            cell.CellStyle = cellstyle;
                                                        }
                                                        else if (subrow > 1)
                                                        {
                                                            for (int k = 0; k < subrow; k++)
                                                            {
                                                                cell = sheet.GetRow(offset + k).CreateCell(cols);
                                                                //设置值
                                                                switch (cols)
                                                                {
                                                                    case 6:
                                                                        v = bhlist[k];
                                                                        break;
                                                                    case 7:
                                                                        v = kflist[k] != "" ? kflist[k] + "分" : "";
                                                                        break;
                                                                    case 8:
                                                                        v = bznrlist[k];
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                cell.SetCellValue(v);
                                                                //设置样式
                                                                cell.CellStyle = cellstyle;
                                                            }

                                                        }
                                                    }

                                                    offset = offset + subrow;
                                                }

                                            }

                                        }
                                    }
                                    // 这里行数需要增加多个（不止一个）
                                    count = count + num;

                                }
                                #endregion
                            }


                        }
                    }
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
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
                    string fileName = HttpUtility.UrlEncode(lastcfsj + "到" + currentcfsj + "整改单处罚详表");
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }
            }
        }


        /// <summary>
        /// 导出整改单统计记录
        /// </summary>
        public void ExportZGDCFTJJL()
        {
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            string ignoredtoken = Request["ignoredtoken"].GetSafeString();
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();
            string msg = "";
            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {

                #region 标题栏

                List<KeyValuePair<string, string>> heads = new List<KeyValuePair<string, string>>();
                //List<KeyValuePair<string, string>> heads = new List<KeyValuePair<string, string>>() {
                //   new KeyValuePair<string, string>("extrainfo4","整改通知编号"),
                //    new KeyValuePair<string, string>("zjdjh","监督登记号"),
                //    new KeyValuePair<string, string>("gcmc","工程名称"),
                //    new KeyValuePair<string, string>("sy_jsdwmc","建设单位"),
                //    new KeyValuePair<string, string>("sjdwmc","设计单位"),
                //    new KeyValuePair<string, string>("sgdwmc","施工单位"),
                //    new KeyValuePair<string, string>("jldwmc","监理单位"),
                //    new KeyValuePair<string, string>("extrainfo2","抽查部位"),
                //    new KeyValuePair<string, string>("cjryxm","经办人"),
                //    new KeyValuePair<string, string>("lrrxm","审核人"),
                //    new KeyValuePair<string, string>("jdgcsxm","监督工程师"),
                //    new KeyValuePair<string, string>("sy_lrsj","出单日期"),
                //    new KeyValuePair<string, string>("extrainfo5","当前整改期限"),
                //    new KeyValuePair<string, string>("extrainfo14","原始整改期限"),
                //    new KeyValuePair<string, string>("yqjl","整改延期记录"),
                //    new KeyValuePair<string, string>("extrainfo1","是否回复"),
                //    new KeyValuePair<string, string>("extrainfo3","整改状态"),
                //    new KeyValuePair<string, string>("sy_jdjlsj","回复日期"),
                //    new KeyValuePair<string, string>("sgdwkfqk","施工单位扣分设置"),
                //    new KeyValuePair<string, string>("sgdwlastkfjg","施工单位最近一次扣分结果"),
                //    new KeyValuePair<string, string>("jldwkfqk","监理单位扣分设置"),
                //    new KeyValuePair<string, string>("jldwlastkfjg","监理单位最近一次扣分结果"),
                //    new KeyValuePair<string, string>("lastkfrq","最近一次扣分日期"),
                //    new KeyValuePair<string, string>("kfyy","扣分原因")

                //};

                string headsql = "select field, title from h_zgdcftjexport_field where sfyx=1 order by xssx";
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
                ISheet sheet = wk.CreateSheet(lastcfsj + "-" + currentcfsj + "整改单处罚统计");
                IRow row;
                ICell cell;


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
                string procstr = "";
                procstr = string.Format("GetZGDCFTJJLNEW('{0}','{1}','{2}')", lastcfsj, currentcfsj, ignoredtoken);
                zgjllist = CommonService.ExecDataTableProc(procstr, out msg);

                if (zgjllist.Count > 0)
                {
                    for (rows = 0; rows < zgjllist.Count; rows++)
                    {
                        IDictionary<string, string> data = zgjllist[rows];
                        row = sheet.CreateRow(rows + 1);
                        for (cols = 0; cols < heads.Count; cols++)
                        {
                            //定义每一列
                            cell = row.CreateCell(cols);
                            //设置值
                            cell.SetCellValue(data[heads[cols].Key].GetSafeString().Replace("<br/>", "\n"));
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
                    string fileName = HttpUtility.UrlEncode(lastcfsj + "到" + currentcfsj + "整改单处罚统计");
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }

            }
        }

        /// <summary>
        /// 获取整改单企业扣分列表数据, 用于整改处罚申请审批（非第一步）
        /// </summary>
        public void GetZGDQYKFLBBYSERIAL()
        {
            bool ret = true;
            string msg = "";
            string serial = Request["serial"].GetSafeString();
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                if (serial != "")
                {
                    string procstr = "";
                    procstr = string.Format("GetZGDQYKFLBBYSERIAL('{0}')", serial);
                    dts = CommonService.ExecDataTableProc(procstr, out msg);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\"}}", dts.Count, jss.Serialize(dts), ret ? "0" : "1"));
                Response.End();
            }

        }

        // 临时保存整改单处罚统计中相关的整改单编号
        public void SaveZGDCFTJZGDBH()
        {
            bool ret = true;
            string msg = "";
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string id = "";
            try
            {
                if (zgdbhlist != "")
                {
                    id = Guid.NewGuid().ToString("N");
                    string sql = string.Format("insert into jdbg_zgdcf_zgdbhcache ( id, zgdbhlist) values ('{0}', '{1}')", id, zgdbhlist);
                    ret = CommonService.ExecSql(sql, out msg);
                }
                else
                {
                    ret = false;
                    msg = "整改单编号不能为空";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"id\": \"{2}\"}}", ret ? "0" : "1", msg, id));
                Response.End();
            }
        }

        // 获取临时保存的整改单处罚统计中相关的整改单编号
        public void GetZGDCFTJZGDBH()
        {
            bool ret = true;
            string msg = "";
            string id = Request["id"].GetSafeString();
            string zgdbhlist = "";
            try
            {
                if (id != "")
                {
                    string sql = string.Format("select zgdbhlist from jdbg_zgdcf_zgdbhcache where id='{0}'", id);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        zgdbhlist = dt[0]["zgdbhlist"];
                    }
                }
                else
                {
                    ret = false;
                    msg = "缺少参数！";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"zgdbhlist\": \"{2}\"}}", ret ? "0" : "1", msg, zgdbhlist));
                Response.End();
            }
        }



        /// <summary>
        /// 刷新缓存的短信模板
        /// </summary>
        public void RefreshDXMB()
        {
            bool ret = true;
            string msg = "";

            try
            {
                Func.GlobalVariable.ReloadWzzjzSmsTemplates();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        /// 手动统计考勤，主要是怕考勤线程间隔中需要做统计的时候用
        /// </summary>
        [Authorize]
        public void DoKQCount()
        {
            bool code = false;
            string msg = "";
            try
            {
                code = Func.KqjThread.dokqcount();
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
        /// 考勤设置允许不考勤
        /// </summary>
        [Authorize]
        public void ExtrIn()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string sql = "update UserInfo set kqextr=1 where UserCode='" + usercode + "'";
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
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


        /// <summary>
        /// 考勤设置取消不考勤
        /// </summary>
        [Authorize]
        public void ExtrOut()
        {
            bool code = false;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string sql = "update UserInfo set kqextr=0 where UserCode='" + usercode + "'";
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
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


        public void ExportMonth()
        {
            DateTime t1 = Request["t1"].GetSafeDate(DateTime.Now);
            DateTime t2 = Request["t2"].GetSafeDate(DateTime.Now);
            string deps = Request["deps"].GetSafeString();


            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {

                //居中样式
                ICellStyle cellstyle = wk.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.WrapText = true;
                //创建一个Sheet  
                ISheet sheet = wk.CreateSheet("导出信息");
                //sheet.DefaultColumnWidth = 15 * 10;
                //sheet.DefaultRowHeightInPoints = 15;  
                IRow row;
                ICell cell;
                //定义导出标题
                row = sheet.CreateRow(0);

                int titilei = 0;
                TimeSpan sp = t2.Subtract(t1);


                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("姓名");
                //设置样式
                cell.CellStyle = cellstyle;

                //titilei++;
                for (int j = 0; j <= sp.Days; j++)
                {
                    DateTime tdate = t1.AddDays(j);
                    titilei = titilei + 1;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(tdate.ToString("yyyy-MM-dd"));
                    //设置样式
                    cell.CellStyle = cellstyle;
                }
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("迟到");
                //设置样式
                cell.CellStyle = cellstyle;

                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("未签到");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("早退");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("未签退");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("因公外出");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("请假");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("年休");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("本月年休");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("年累计年休");
                //设置样式
                cell.CellStyle = cellstyle;






                string sql = "select usercode,realname from userinfo where needkq=1 ";
                if (deps != "")
                {
                    string strTmp = "";
                    string[] arrDep = deps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arrDep.Length; i++)
                    {
                        if (strTmp != "")
                            strTmp += " or ";
                        strTmp += " departmentid='" + arrDep[i] + "' ";
                    }
                    if (strTmp != "")
                        sql += " and ( " + strTmp + " )";
                }
                int datanum = 0;
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    datanum = datanum + 1;
                    row = sheet.CreateRow(datanum);
                    titilei = 0;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(dt[i]["realname"].GetSafeString());
                    //设置样式
                    cell.CellStyle = cellstyle;

                    int cd = 0;
                    int zt = 0;
                    int wcd = 0;
                    int wct = 0;
                    double ygwcts = 0;
                    double qjts = 0;
                    double nxts = 0;
                    double nxjts = 0;

                    //titilei++;
                    for (int j = 0; j <= sp.Days; j++)
                    {



                        DateTime tdate = t1.AddDays(j);
                        titilei = titilei + 1;
                        sheet.SetColumnWidth(titilei, 20 * 256);
                        //定义每一列
                        cell = row.CreateCell(titilei);
                        //设置值
                        string SignDate = tdate.ToString("yyyy-MM-dd");
                        KqUserSign sign = SystemService.getUserSign(dt[i]["usercode"].GetSafeString(), SignDate);

                        string temtext = "";

                        if (sign != null)
                        {
                            if (sign.S1Type == "-1")
                            {
                                temtext += "[" + sign.S1Text + "]";
                            }
                            else
                            {
                                if (sign.S1 != null)
                                {
                                    temtext += sign.S1.Value.ToShortTimeString() + "[" + sign.S1Text + "]";
                                }
                                else
                                {
                                    temtext += "[" + sign.S1Text + "]";
                                }
                                //temtext += "[" + sign.S1Text + "]";
                            }
                            if (sign.S4Type == "-1")
                            {
                                temtext = temtext + "-" + "[" + sign.S4Text + "]";
                            }
                            else
                            {

                                if (sign.S4 != null)
                                {
                                    temtext = temtext + "-" + sign.S4.Value.ToShortTimeString() + "[" + sign.S4Text + "]";
                                }
                                else
                                {
                                    temtext = temtext + "-" + "[" + sign.S4Text + "]";
                                }
                                //temtext = temtext + "-" + "[" + sign.S4Text + "]";
                            }

                            if (sign.S1Type == "-1")
                            {
                                wcd++;
                            }
                            else if (sign.S1Type != "0")
                            {
                                cd++;
                            }

                            if (sign.S4Type == "-1")
                            {
                                wct++;
                            }
                            else if (sign.S4Type != "0")
                            {
                                zt++;
                            }



                        }
                        else
                        {
                            temtext = "/";
                        }


                        IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select recid,wcry, wcryzh, wcsqsjstart, wcsqsjend,spzt,wcbzsm, nr, spztms from View_QJ_YGWCJL where wcryzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((wcsqsjstart>'" + SignDate + "' and wcsqsjstart<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (wcsqsjend>'" + SignDate + "' and wcsqsjend<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                        //每天的记录查找下

                        for (int t = 0; t < dt1.Count; t++)
                        {
                            temtext += "\n";
                            temtext += dt1[t]["nr"].GetSafeString() + "[" + dt1[t]["spztms"].GetSafeString() + "]" + "[" + dt1[t]["wcsqsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "-" + dt1[t]["wcsqsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "]";

                        }

                        dt1 = CommonService.GetDataTable("select recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx, qjyy, spztms from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((qjsjstart>'" + SignDate + "' and qjsjstart<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (qjsjend>'" + SignDate + "' and qjsjend<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                        //每天的记录查找下

                        for (int t = 0; t < dt1.Count; t++)
                        {
                            temtext += "\n";
                            temtext += dt1[t]["qjyy"].GetSafeString() + "[" + dt1[t]["spztms"].GetSafeString() + "]" + "[" + dt1[t]["qjsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "-" + dt1[t]["qjsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "]";

                        }

                        cell.SetCellValue(temtext);
                        //设置样式
                        cell.CellStyle = cellstyle;
                    }



                    IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select datediff( hour, wcsqsjstart, wcsqsjend ) as wchour,datediff( day, wcsqsjstart, wcsqsjend ) as wcday,wcry, wcryzh, wcsqsjstart, wcsqsjend,spzt,wcbzsm from View_QJ_YGWCJL where wcryzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((wcsqsjstart>'" + t1.ToString("yyyy-MM-dd") + "' and wcsqsjstart<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (wcsqsjend>'" + t1.ToString("yyyy-MM-dd") + "' and wcsqsjend<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                    //每天的记录查找下

                    for (int t = 0; t < dt2.Count; t++)
                    {
                        ygwcts = ygwcts + dt2[t]["wcday"].GetSafeDouble();
                        double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                        if (hourtoday > 0 && hourtoday <= 4)
                        {
                            ygwcts = ygwcts + 0.5;
                        }
                        else if (hourtoday > 4)
                        {
                            ygwcts = ygwcts + 1;
                        }

                    }

                    dt2 = CommonService.GetDataTable("select datediff( hour, qjsjstart, qjsjend ) as wchour,datediff( day, qjsjstart, qjsjend ) as wcday,recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((qjsjstart>'" + t1.ToString("yyyy-MM-dd") + "' and qjsjstart<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (qjsjend>'" + t1.ToString("yyyy-MM-dd") + "' and qjsjend<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                    //每天的记录查找下

                    for (int t = 0; t < dt2.Count; t++)
                    {


                        if (dt2[t]["qjlx"].GetSafeString() == "年休")
                        {
                            nxts = nxts + dt2[t]["wcday"].GetSafeDouble();
                            double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                            if (hourtoday > 0 && hourtoday <= 4)
                            {
                                nxts = nxts + 0.5;
                            }
                            else if (hourtoday > 4)
                            {
                                nxts = nxts + 1;
                            }
                        }
                        else
                        {
                            qjts = qjts + dt2[t]["wcday"].GetSafeDouble();
                            double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                            if (hourtoday > 0 && hourtoday <= 4)
                            {
                                qjts = qjts + 0.5;
                            }
                            else if (hourtoday > 4)
                            {
                                qjts = qjts + 1;
                            }
                        }

                    }

                    dt2 = CommonService.GetDataTable("select datediff( hour, qjsjstart, qjsjend ) as wchour,datediff( day, qjsjstart, qjsjend ) as wcday,recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((qjsjstart>'" + t1.ToString("yyyy-01-01") + "' and qjsjstart<'" + t2.AddYears(1).ToString("yyyy-01-01") + "' ) or (qjsjend>'" + t1.ToString("yyyy-01-01") + "' and qjsjend<'" + t2.AddYears(1).ToString("yyyy-01-01") + "' ))");
                    //每天的记录查找下

                    for (int t = 0; t < dt2.Count; t++)
                    {
                        nxjts = nxjts + dt2[t]["wcday"].GetSafeDouble();
                        double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                        if (hourtoday > 0 && hourtoday <= 4)
                        {
                            nxjts = nxjts + 0.5;
                        }
                        else if (hourtoday > 4)
                        {
                            nxjts = nxjts + 1;
                        }
                    }


                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(cd.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;

                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(wcd.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(zt.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(wct.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;



                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(ygwcts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(qjts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(nxts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue((qjts + nxts).ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(nxjts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;





                }
                sheet.CreateFreezePane(1, 1, 1, 1);

                /*

                for (int i = 0; i < zdzdExportList.Count; i++)
                {
                    sheet.SetColumnWidth(i, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(i);
                    //设置值
                    cell.SetCellValue(zdzdExportList[i].ZdSy);
                    //设置样式
                    cell.CellStyle = cellstyle;
                }

                //定义数据行
                int datanum = 0;
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select * from userinfo ");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    datanum = datanum + 1;
                    row = sheet.CreateRow(datanum);
                    //创建行数据
                    for (int i = 0; i < zdzdExportList.Count; i++)
                    {
                        cell = row.CreateCell(i);
                        //设置值
                        cell.SetCellValue(dr[zdzdExportList[i].ZdName].GetString());
                    }
                }
                */

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
                    string fileName = HttpUtility.UrlEncode(t1.ToString("yyyy-MM-dd") + "到" + t2.ToString("yyyy-MM-dd") + "考勤统计");
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }

            }
        }


        public void ExportMonth2()
        {
            DateTime t1 = Request["t1"].GetSafeDate(DateTime.Now);
            DateTime t2 = Request["t2"].GetSafeDate(DateTime.Now);
            string deps = Request["deps"].GetSafeString();


            XSSFWorkbook wk = new XSSFWorkbook();
            try
            {

                //居中样式
                ICellStyle cellstyle = wk.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.WrapText = true;
                //创建一个Sheet  
                ISheet sheet = wk.CreateSheet("导出信息");
                //sheet.DefaultColumnWidth = 15 * 10;
                //sheet.DefaultRowHeightInPoints = 15;  
                IRow row;
                ICell cell;
                //定义导出标题
                row = sheet.CreateRow(0);

                int titilei = 0;
                TimeSpan sp = t2.Subtract(t1);


                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("姓名");
                //设置样式
                cell.CellStyle = cellstyle;

                //titilei++;
                for (int j = 0; j <= sp.Days; j++)
                {
                    DateTime tdate = t1.AddDays(j);
                    titilei = titilei + 1;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(tdate.ToString("yyyy-MM-dd"));
                    //设置样式
                    cell.CellStyle = cellstyle;
                }
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("迟到");
                //设置样式
                cell.CellStyle = cellstyle;

                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("未签到");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("早退");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("未签退");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("因公外出");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("请假");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("年休");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("本月年休");
                //设置样式
                cell.CellStyle = cellstyle;
                titilei++;
                sheet.SetColumnWidth(titilei, 20 * 256);
                //定义每一列
                cell = row.CreateCell(titilei);
                //设置值
                cell.SetCellValue("年累计年休");
                //设置样式
                cell.CellStyle = cellstyle;






                string sql = "select usercode,realname from userinfo where needkq=1 ";
                if (deps != "")
                {
                    string strTmp = "";
                    string[] arrDep = deps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arrDep.Length; i++)
                    {
                        if (strTmp != "")
                            strTmp += " or ";
                        strTmp += " departmentid='" + arrDep[i] + "' ";
                    }
                    if (strTmp != "")
                        sql += " and ( " + strTmp + " )";
                }
                int datanum = 0;
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                for (int i = 0; i < dt.Count; i++)
                {
                    datanum = datanum + 1;
                    row = sheet.CreateRow(datanum);
                    titilei = 0;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(dt[i]["realname"].GetSafeString());
                    //设置样式
                    cell.CellStyle = cellstyle;

                    int cd = 0;
                    int zt = 0;
                    int wcd = 0;
                    int wct = 0;
                    double ygwcts = 0;
                    double qjts = 0;
                    double nxts = 0;
                    double nxjts = 0;

                    //titilei++;
                    for (int j = 0; j <= sp.Days; j++)
                    {



                        DateTime tdate = t1.AddDays(j);
                        titilei = titilei + 1;
                        sheet.SetColumnWidth(titilei, 20 * 256);
                        //定义每一列
                        cell = row.CreateCell(titilei);
                        //设置值
                        string SignDate = tdate.ToString("yyyy-MM-dd");
                        KqUserSign sign = SystemService.getUserSign(dt[i]["usercode"].GetSafeString(), SignDate);

                        string temtext = "";

                        if (sign != null)
                        {
                            if (sign.S1Type == "-1")
                            {
                                temtext += "[" + sign.S1Text + "]";
                            }
                            else
                            {
                                if (sign.S1 != null)
                                {
                                    temtext += sign.S1.Value.ToShortTimeString() + "[" + sign.S1Text + "]";
                                }
                                else
                                {
                                    temtext += "[" + sign.S1Text + "]";
                                }
                                //temtext += "[" + sign.S1Text + "]";
                            }
                            if (sign.S4Type == "-1")
                            {
                                temtext = temtext + "-" + "[" + sign.S4Text + "]";
                            }
                            else
                            {

                                if (sign.S4 != null)
                                {
                                    temtext = temtext + "-" + sign.S4.Value.ToShortTimeString() + "[" + sign.S4Text + "]";
                                }
                                else
                                {
                                    temtext = temtext + "-" + "[" + sign.S4Text + "]";
                                }
                                //temtext = temtext + "-" + "[" + sign.S4Text + "]";
                            }

                            if (sign.S1Type == "-1")
                            {
                                wcd++;
                            }
                            else if (sign.S1Type != "0")
                            {
                                cd++;
                            }

                            if (sign.S4Type == "-1")
                            {
                                wct++;
                            }
                            else if (sign.S4Type != "0")
                            {
                                zt++;
                            }



                        }
                        else
                        {
                            temtext = "/";
                        }


                        IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select recid,wcry, wcryzh, wcsqsjstart, wcsqsjend,spzt,wcbzsm, nr, spztms from View_QJ_YGWCJL where wcryzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((wcsqsjstart>'" + SignDate + "' and wcsqsjstart<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (wcsqsjend>'" + SignDate + "' and wcsqsjend<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                        //每天的记录查找下

                        for (int t = 0; t < dt1.Count; t++)
                        {
                            temtext += "\n";
                            temtext += dt1[t]["nr"].GetSafeString() + "[" + dt1[t]["spztms"].GetSafeString() + "]" + "[" + dt1[t]["wcsqsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "-" + dt1[t]["wcsqsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "]";

                        }

                        dt1 = CommonService.GetDataTable("select recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx, qjyy, spztms from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((qjsjstart>'" + SignDate + "' and qjsjstart<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (qjsjend>'" + SignDate + "' and qjsjend<'" + tdate.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                        //每天的记录查找下

                        for (int t = 0; t < dt1.Count; t++)
                        {
                            temtext += "\n";
                            temtext += dt1[t]["qjyy"].GetSafeString() + "[" + dt1[t]["spztms"].GetSafeString() + "]" + "[" + dt1[t]["qjsjstart"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "-" + dt1[t]["qjsjend"].GetSafeDate().ToString("yyyy-MM-dd HH:mm") + "]";

                        }

                        cell.SetCellValue(temtext);
                        //设置样式
                        cell.CellStyle = cellstyle;
                    }



                    IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select datediff( hour, wcsqsjstart, wcsqsjend ) as wchour,datediff( day, wcsqsjstart, wcsqsjend ) as wcday,wcry, wcryzh, wcsqsjstart, wcsqsjend,spzt,wcbzsm from View_QJ_YGWCJL where wcryzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((wcsqsjstart>'" + t1.ToString("yyyy-MM-dd") + "' and wcsqsjstart<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (wcsqsjend>'" + t1.ToString("yyyy-MM-dd") + "' and wcsqsjend<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                    //每天的记录查找下

                    for (int t = 0; t < dt2.Count; t++)
                    {
                        ygwcts = ygwcts + dt2[t]["wcday"].GetSafeDouble();
                        double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                        if (hourtoday > 0 && hourtoday <= 4)
                        {
                            ygwcts = ygwcts + 0.5;
                        }
                        else if (hourtoday > 4)
                        {
                            ygwcts = ygwcts + 1;
                        }

                    }

                    dt2 = CommonService.GetDataTable("select datediff( hour, qjsjstart, qjsjend ) as wchour,datediff( day, qjsjstart, qjsjend ) as wcday,recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((qjsjstart>'" + t1.ToString("yyyy-MM-dd") + "' and qjsjstart<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ) or (qjsjend>'" + t1.ToString("yyyy-MM-dd") + "' and qjsjend<'" + t2.AddDays(1).ToString("yyyy-MM-dd") + "' ))");
                    //每天的记录查找下

                    for (int t = 0; t < dt2.Count; t++)
                    {


                        if (dt2[t]["qjlx"].GetSafeString() == "年休")
                        {
                            nxts = nxts + dt2[t]["wcday"].GetSafeDouble();
                            double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                            if (hourtoday > 0 && hourtoday <= 4)
                            {
                                nxts = nxts + 0.5;
                            }
                            else if (hourtoday > 4)
                            {
                                nxts = nxts + 1;
                            }
                        }
                        else
                        {
                            qjts = qjts + dt2[t]["wcday"].GetSafeDouble();
                            double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                            if (hourtoday > 0 && hourtoday <= 4)
                            {
                                qjts = qjts + 0.5;
                            }
                            else if (hourtoday > 4)
                            {
                                qjts = qjts + 1;
                            }
                        }

                    }

                    dt2 = CommonService.GetDataTable("select datediff( hour, qjsjstart, qjsjend ) as wchour,datediff( day, qjsjstart, qjsjend ) as wcday,recid,qjrxm,qjrzh, qjsjstart,qjsjend,spzt,qjlx from View_QJ_YSWCJL where qjrzh='" + dt[i]["usercode"].GetSafeString() + "' and SPZT=1 and ((qjsjstart>'" + t1.ToString("yyyy-01-01") + "' and qjsjstart<'" + t2.AddYears(1).ToString("yyyy-01-01") + "' ) or (qjsjend>'" + t1.ToString("yyyy-01-01") + "' and qjsjend<'" + t2.AddYears(1).ToString("yyyy-01-01") + "' ))");
                    //每天的记录查找下

                    for (int t = 0; t < dt2.Count; t++)
                    {
                        nxjts = nxjts + dt2[t]["wcday"].GetSafeDouble();
                        double hourtoday = dt2[t]["wchour"].GetSafeDouble() - (2 * dt2[t]["wcday"].GetSafeDouble());

                        if (hourtoday > 0 && hourtoday <= 4)
                        {
                            nxjts = nxjts + 0.5;
                        }
                        else if (hourtoday > 4)
                        {
                            nxjts = nxjts + 1;
                        }
                    }


                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(cd.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;

                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(wcd.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(zt.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(wct.ToString() + "次");
                    //设置样式
                    cell.CellStyle = cellstyle;



                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(ygwcts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(qjts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(nxts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue((qjts + nxts).ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;
                    titilei++;
                    sheet.SetColumnWidth(titilei, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(titilei);
                    //设置值
                    cell.SetCellValue(nxjts.ToString() + "天");
                    //设置样式
                    cell.CellStyle = cellstyle;





                }
                sheet.CreateFreezePane(1, 1, 1, 1);

                /*

                for (int i = 0; i < zdzdExportList.Count; i++)
                {
                    sheet.SetColumnWidth(i, 20 * 256);
                    //定义每一列
                    cell = row.CreateCell(i);
                    //设置值
                    cell.SetCellValue(zdzdExportList[i].ZdSy);
                    //设置样式
                    cell.CellStyle = cellstyle;
                }

                //定义数据行
                int datanum = 0;
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select * from userinfo ");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    datanum = datanum + 1;
                    row = sheet.CreateRow(datanum);
                    //创建行数据
                    for (int i = 0; i < zdzdExportList.Count; i++)
                    {
                        cell = row.CreateCell(i);
                        //设置值
                        cell.SetCellValue(dr[zdzdExportList[i].ZdName].GetString());
                    }
                }
                */

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                //using (MemoryStream memoryStram = new MemoryStream())
                //{
                //    //把工作簿写入到内存流中
                //    wk.Write(memoryStram);
                //    byte[] data = memoryStram.GetBuffer();
                //    int size = data.Length;

                //    //设置输出编码格式
                //    Response.ContentEncoding = System.Text.Encoding.UTF8;
                //    //设置输出流
                //    Response.ContentType = "application/octet-stream";
                //    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                //    // 设置字节大小
                //    Response.AddHeader("Content-Length", size.ToString());
                //    //防止中文乱码
                //    string fileName = HttpUtility.UrlEncode(t1.ToString("yyyy-MM-dd") + "到" + t2.ToString("yyyy-MM-dd") + "考勤统计");
                //    //设置输出文件名
                //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xlsx");

                //    //输出
                //    Response.BinaryWrite(data);
                //}
                string fname = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                FileStream fileStream = new FileStream(Server.MapPath("~/tempfiles/" + fname + ".xlsx"), FileMode.Create, FileAccess.Write);

                wk.Write(fileStream);//调用这个后会关于文件流，在HSSFWorkbook不会关闭所以在处理时应注意               
                FileStream fs = new FileStream(Server.MapPath("~/tempfiles/" + fname + ".xlsx"), FileMode.Open, FileAccess.Read);
                long fileSize = fs.Length;


                byte[] fileBuffer = new byte[fileSize];
                fs.Read(fileBuffer, 0, (int)fileSize);
                fs.Close();


                //设置输出编码格式
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                //设置输出流
                Response.ContentType = "application/octet-stream";
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                //加上设置大小下载下来的.xlsx文件打开时才没有错误
                Response.AddHeader("Content-Length", fileSize.ToString());
                //防止中文乱码
                string fileName = HttpUtility.UrlEncode(t1.ToString("yyyy-MM-dd") + "到" + t2.ToString("yyyy-MM-dd") + "考勤统计");
                //设置输出文件名
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xlsx");

                //输出
                Response.BinaryWrite(fileBuffer);








            }
        }


        public ActionResult SetNeedKQ()
        {
            string usercode = Request["usercode"].GetSafeString();
            string needkq = Request["needkq"].GetSafeString();
            ViewBag.needkq = needkq;
            ViewBag.usercode = usercode;
            return View();
        }

        public void DoSetNeedKQ()
        {
            bool code = true;
            string msg = "";
            try
            {
                string usercode = Request["usercode"].GetSafeString();
                string needkq = Request["needkq"].GetSafeString();
                string sql = string.Format("update userinfo set needkq={0} where usercode='{1}'", needkq, usercode);
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
                code = CommonService.ExecTrans(sqls);
                msg = code ? "" : "设置失败！";
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

        public void DoDeleteKq()
        {
            bool code = true;
            string msg = "";
            try
            {
                string ids = Request["ids"].GetSafeString();
                if (ids == "")
                {
                    code = false;
                    msg = "提交的参数不正确!";
                }
                else
                {
                    string sql = "delete from kqusersign where  recid in (" + ids.FormatSQLInStr() + ")";
                    code = CommonService.ExecSql(sql, out msg);
                    if (!code)
                    {
                        msg = "删除失败！";
                    }
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e.Message);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }


        public void DoDeleteYGWC()
        {
            bool code = true;
            string msg = "";
            try
            {
                string ids = Request["ids"].GetSafeString();
                if (ids == "")
                {
                    code = false;
                    msg = "提交的参数不正确!";
                }
                else
                {
                    string proc = string.Format("DeleteQJYGWCJL('{0}')", ids);
                    CommonService.ExecProc(proc, out msg);
                    code = msg == "";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e.Message);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        public void DoDeleteYSWC()
        {
            bool code = true;
            string msg = "";
            try
            {
                string ids = Request["ids"].GetSafeString();
                if (ids == "")
                {
                    code = false;
                    msg = "提交的参数不正确!";
                }
                else
                {
                    string proc = string.Format("DeleteQJYSWCJL('{0}')", ids);
                    CommonService.ExecProc(proc, out msg);
                    code = msg == "";
                }
            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
                SysLog4.WriteLog(e.Message);
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }

        #region 查看发文文件
        public ActionResult showfw()
        {
            string serial = Request["serial"].GetSafeString();
            string idlist = "";
            string namelist = "";
            List<string> list = new List<string>();
            List<string> nlist = new List<string>();
            if (serial != "")
            {
                string sql = string.Format("select fileid, fileorgname from view_jdbg_fw_fj where workserial='{0}' ", serial);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    foreach (var row in dt)
                    {
                        list.Add(row["fileid"].GetSafeString());
                        nlist.Add(row["fileorgname"].GetSafeString());
                    }
                }
                if (list.Count > 0)
                {
                    list = list.Distinct().ToList();
                    idlist = string.Join(",", list.ToArray());
                    namelist = string.Join(",", nlist.ToArray());
                }
            }

            ViewBag.idlist = idlist;
            ViewBag.namelist = namelist;
            return View();

        }
        #endregion

        #region 查看收文文件
        public ActionResult showsw()
        {
            string serial = Request["serial"].GetSafeString();
            string idlist = "";
            string namelist = "";
            List<string> list = new List<string>();
            List<string> nlist = new List<string>();
            if (serial != "")
            {
                string sql = string.Format("select fileid, fileorgname from view_jdbg_sw_fj where workserial='{0}' ", serial);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    foreach (var row in dt)
                    {
                        list.Add(row["fileid"].GetSafeString());
                        nlist.Add(row["fileorgname"].GetSafeString());
                    }
                }
                if (list.Count > 0)
                {
                    list = list.Distinct().ToList();
                    idlist = string.Join(",", list.ToArray());
                    namelist = string.Join(",", nlist.ToArray());
                }
            }

            ViewBag.idlist = idlist;
            ViewBag.namelist = namelist;
            return View();

        }
        #endregion

        #region 第一次查看报告时，需要输入手机验证码校验

        /// <summary>
        /// 校验当前用户是否第一次打开报告
        /// 主要用于各方主体第一次打开整改单时，记录电子签收的相关信息
        /// </summary>
        [Authorize]
        public void CheckReportUser()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            string result = "";
            string jsonparams = "";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            try
            {
                string reportFile = Request["reportfile"].GetSafeString();
                string serial = Request["serial"].GetSafeString();
                string type = Request["type"].GetSafeString();
                int jdjlid = Request["jdjlid"].GetSafeInt();
                int isprint = Request["print"].GetSafeInt(1);
                StForm form = WorkFlowService.GetForm(serial);
                int formid = 0;

                if (form != null)
                    formid = form.Formid;

                // 获取客户端传入的reporttype
                string reporttype = Request["reporttype"].GetSafeString();

                // 需要替换的字典
                Dictionary<string, object> rd = new Dictionary<string, object>()
                {
                    { "formid", formid },
                    { "gcbh", form.ExtraInfo3 },
                    { "parentid", jdjlid },
                    { "recid", jdjlid },
                    { "xformid", formid },
                    { "serial", serial},
                    { "reporttype", reporttype},
                    { "reportfile", reportFile},
                    { "isprint", isprint},
                    { "type", type}
                };
                // 是否启用了电子签章
                bool enablesign = GlobalVariable.GetConfigValue("EnableESign") == "true";
                bool enablereceipt = GlobalVariable.GetConfigValue("EnableEReceipt") == "true";
                // 启用了电子签章并且reporttype为整改单
                if (enablesign)
                {
                    // 需要验证用户
                    if (enablereceipt)
                    {
                        // 报告类型为整改单时，才需要记录电子签收的信息
                        if (reporttype.ToLower() == "zgd")
                        {
                            // 需要用户验证
                            if (CheckNeedToValidateUser(rd))
                            {
                                code = true;
                                result = "0";
                                jsonparams = jss.Serialize(rd).EncodeBase64();
                            }
                            else
                            {
                                code = true;
                                result = "1";
                            }
                        }
                        else
                        {
                            code = true;
                            result = "1";
                        }

                    }
                    else
                    {
                        code = true;
                        result = "1";
                        Dictionary<string, object> dt = new Dictionary<string, object>();
                        dt.Add("serial", serial);
                        dt.Add("usercode", CurrentUser.UserName);
                        dt.Add("realname", CurrentUser.RealName);
                        dt.Add("ip", GetIP());
                        string p = jss.Serialize(dt);
                        string procstr = string.Format("InsertReportQSJLDirect('{0}')", p);
                        CommonService.ExecProc(procstr, out msg);
                        msg = "";


                    }

                }
                // 未启用电子签章, 不需要用户输入手机验证码校验
                else
                {
                    code = true;
                    result = "1";
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
                data.Add("result", result);
                data.Add("jsonparams", jsonparams);
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }




        }



        /// <summary>
        /// 校验一下，是否需要验证用户
        /// </summary>
        /// <param name="rd"></param>
        /// <returns></returns>
        private bool CheckNeedToValidateUser(Dictionary<string, object> rd)
        {
            bool ret = true;
            string msg = "";
            try
            {
                string usercode = CurrentUser.UserName.GetSafeString();
                if (usercode != "")
                {
                    Dictionary<string, object> infos = new Dictionary<string, object>();
                    foreach (var item in rd)
                    {
                        infos.Add(item.Key, item.Value);
                    }
                    infos.Add("usercode", usercode);

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = Int32.MaxValue;
                    string jsonparams = jss.Serialize(infos);
                    string procstr = string.Format("CheckNeedToValidateUser('{0}')", jsonparams);
                    IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                    if (dt.Count > 0)
                    {
                        string result = dt[0]["result"];
                        if (result == "1")
                        {
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                    else
                    {
                        ret = true;
                    }
                }
                else
                {
                    ret = true; // 用户未登录
                }
            }
            catch (Exception e)
            {
                ret = true;
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        [Authorize]
        public ActionResult ValidateReportUser()
        {
            string jsonparams = Request["p"].GetSafeString();
            string phone = "";
            string msg = "";
            string procstr = string.Format("GetSJHMByUsercode('{0}')", CurrentUser.UserName);
            IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
            if (dt.Count > 0)
            {
                phone = dt[0]["sjhm"].GetSafeString();
            }
            ViewBag.jsonparams = jsonparams;
            ViewBag.phone = phone;
            return View();
        }


        public void CheckReportQFR()
        {
            bool code = true;
            string msg = "";
            Dictionary<string, object> data = new Dictionary<string, object>();
            string result = "";
            string bgbh = "";
            string jsonparams = "";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            try
            {
                // 兼容手机, 防止用户信息丢失
                string username = DataFormat.GetSafeString(Request["login_name"]);
                string password = DataFormat.GetSafeString(Request["login_pwd"]);
                if (username != "" && password != "")
                {
                    if (!BD.WorkFlow.Common.WorkFlowUser.IsLogin)
                        PhoneSetUser(username);
                }

                bool enablesign = GlobalVariable.GetConfigValue("EnableESign") == "true";
                bool enableeqf = GlobalVariable.GetConfigValue("EnableEQF") == "true";
                if (enablesign)
                {
                    if (enableeqf)
                    {
                        result = "1";
                        string serial = Request["serial"].GetSafeString();
                        string lx = Request["lx"].GetSafeString();
                        Dictionary<string, object> rd = new Dictionary<string, object>()
                        {
                            { "serial", serial},
                            { "lx", lx }
                        };
                        jsonparams = jss.Serialize(rd);
                        string procstr = string.Format("CheckReportQFR('{0}')", jsonparams);
                        IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
                        if (dt.Count > 0)
                        {
                            result = dt[0]["result"];
                            bgbh = dt[0]["bgbh"];
                        }

                    }
                    else
                    {
                        result = "0";
                        bgbh = "";
                        string serial = Request["serial"].GetSafeString();
                        string lx = Request["lx"].GetSafeString();
                        Dictionary<string, object> rd = new Dictionary<string, object>()
                        {
                            { "serial", serial},
                            { "lx", lx }
                        };
                        rd.Add("usercode", CurrentUser.UserName);
                        rd.Add("realname", CurrentUser.RealName);
                        rd.Add("ip", GetIP());

                        jsonparams = jss.Serialize(rd);
                        string procstr = string.Format("InsertReportQFJLDirect('{0}')", jsonparams);
                        CommonService.ExecDataTableProc(procstr, out msg);

                    }


                }
                else
                {
                    result = "0";
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
                data.Add("result", result);
                data.Add("bgbh", bgbh);
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\", \"data\":{2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }

        /// <summary>
        /// 用户登录，防止用户信息丢失
        /// </summary>
        /// <param name="username"></param>
        public void PhoneSetUser(string username)
        {
            VUser vuser = RemoteUserService.GetUser(username);
            if (vuser != null)
            {
                VCompany company = RemoteUserService.GetDepartment(vuser.CompanyId);
                VCompany dep = RemoteUserService.GetDepartment(vuser.DepartmentId);
                WorkFlow.Common.SessionUser user = new WorkFlow.Common.SessionUser()
                {
                    UserName = vuser.UserCode,
                    RealName = vuser.UserRealName,
                    CompanyId = vuser.CompanyId,
                    CompanyName = company.CompanyName,
                    DepartmentId = vuser.DepartmentId,
                    DepartmentName = dep.CompanyName,
                    DutyLevel = "2",
                };

                BD.WorkFlow.Common.WorkFlowUser.SetUserInfo(user, null);
                /*
                UserManager.UserMgr.USERCODE = vuser.UserCode;
                UserManager.UserMgr.USERNAME = user.UserName;
                UserManager.UserMgr.REALNAME = user.RealName;
                UserManager.UserMgr.CPCODE = user.CompanyId;
                UserManager.UserMgr.CPNAME = user.CompanyName;
                UserManager.UserMgr.DEPCODE = user.DepartmentId;
                UserManager.UserMgr.DEPNAME = user.DepartmentName;*/
                // 设置录入界面用户
                Session["USERCODE"] = vuser.UserCode;
                Session["USERNAME"] = user.UserName;
                Session["REALNAME"] = user.RealName;
                Session["CPCODE"] = user.CompanyId;
                Session["CPNAME"] = user.CompanyName;
                Session["DEPCODE"] = user.DepartmentId;
                Session["DEPNAME"] = user.DepartmentName;

                System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName, false);
            }
        }


        [Authorize]
        public ActionResult ValidateReportQFR()
        {
            string jsonparams = Request["p"].GetSafeString();
            string phone = "";
            string msg = "";
            string procstr = string.Format("GetSJHMByUsercode('{0}')", CurrentUser.UserName);
            IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(procstr, out msg);
            if (dt.Count > 0)
            {
                phone = dt[0]["sjhm"].GetSafeString();
            }
            ViewBag.phone = phone;
            ViewBag.jsonparams = jsonparams;
            return View();
        }

        [Authorize]
        public void CheckReportUserYZM()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["VALIDATEREPORTUSER_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
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
                        Session["VALIDATEREPORTUSER_VERIFY_CODE"] = null;
                        code = msg == "";
                        // 验证码正确，记录签收信息
                        if (code)
                        {
                            string jsonparams = Request["jsonparams"].GetSafeString();
                            string sjhm = Request["sjhm"].GetSafeString();
                            if (jsonparams != "")
                            {
                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                jss.MaxJsonLength = Int32.MaxValue;
                                Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(jsonparams);
                                if (dt != null)
                                {
                                    dt.Add("usercode", CurrentUser.UserName);
                                    dt.Add("yzm", yzm);
                                    dt.Add("sjhm", sjhm);
                                    dt.Add("ip", GetIP());
                                }
                                string p = jss.Serialize(dt);
                                string procstr = string.Format("InsertReportQSJL('{0}')", p);
                                CommonService.ExecProc(procstr, out msg);
                                msg = "";
                            }
                        }
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


        public void CheckReportQFRYZM()
        {
            string msg = "";
            bool code = false;
            try
            {
                string yzm = Request["yzm"].GetSafeString();
                string yzmE = Session["VALIDATEREPORTQFR_VERIFY_CODE"] as string;// 验证码,时间
                if (yzmE == null)
                    msg = "验证码无效，请点击“获取验证码”重新获取";
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
                        Session["VALIDATEREPORTQFR_VERIFY_CODE"] = null;
                        code = msg == "";
                        // 验证码正确，记录签收信息
                        if (code)
                        {
                            string jsonparams = Request["jsonparams"].GetSafeString();
                            string sjhm = Request["sjhm"].GetSafeString();
                            if (jsonparams != "")
                            {
                                JavaScriptSerializer jss = new JavaScriptSerializer();
                                jss.MaxJsonLength = Int32.MaxValue;
                                Dictionary<string, object> dt = jss.Deserialize<Dictionary<string, object>>(jsonparams);
                                if (dt != null)
                                {
                                    dt.Add("userid", CurrentUser.UserName);
                                    dt.Add("realname", CurrentUser.RealName);
                                    dt.Add("yzm", yzm);
                                    dt.Add("sjhm", sjhm);
                                    dt.Add("ip", GetIP());
                                }
                                string p = jss.Serialize(dt);
                                string procstr = string.Format("InsertReportQFJL('{0}')", p);
                                CommonService.ExecProc(procstr, out msg);
                                msg = "";
                            }
                        }
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

        public void DeleteReportCache()
        {
            bool code = true;
            string msg = "";
            try
            {
                string lx = Request["lx"].GetSafeString();
                string bgbh = Request["bgbh"].GetSafeString();
                if (bgbh != "")
                {
                    // 缓存的目录
                    string dir = Server.MapPath("~\\report\\pdftemp");
                    if (Directory.Exists(dir))
                    {
                        string fpath = "";
                        if (lx == "ZGD")
                        {
                            fpath = dir + "\\" + "P-ZGD-" + bgbh + ".pdf";
                        }
                        else if (lx == "JDBG")
                        {
                            fpath = dir + "\\" + "P-JDBG-" + bgbh + ".pdf";
                        }

                        if (fpath != "" && System.IO.File.Exists(fpath))
                        {
                            System.IO.File.Delete(fpath);
                        }


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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }


        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回空字符串</returns>
        private string GetIP()
        {
            string IP = "";
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            string userHostAddress = "";
            if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                userHostAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            //否则直接读取REMOTE_ADDR获取客户端IP地址
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress))
            {
                // IPV6的localhost
                if (userHostAddress == "::1")
                {
                    IP = "127.0.0.1";
                }

                if (Regex.IsMatch(userHostAddress, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                {
                    IP = userHostAddress;
                }
            }
            return IP;
        }

        /// <summary>
        /// 获取整改回复流程列表
        /// </summary>
        [Authorize]
        public void GetZGDHFProcess()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select * from h_zgdhfprocess where inuse=1  order by xssx";
                data = CommonService.GetDataTable2(sql);
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\", \"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }

        }

        #endregion

        #region 执法检查备案表设置阅读状态

        public ActionResult YSAPZT()
        {
            string currentzt = "";
            string apid = Request["apid"].GetSafeString();
            if (apid != "")
            {
                string sql = string.Format("select readstate from view_jdbg_ysapjl where apid='{0}' ", apid);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    currentzt = dt[0]["readstate"].GetSafeString();
                }

            }
            ViewBag.currentzt = currentzt;

            return View();

        }

        public void UpdateYSAPZT()
        {
            bool code = false;
            string msg = "";
            try
            {

                string zt = Request["zt"].GetSafeString();
                string apid = Request["apid"].GetSafeString();

                if (zt != "" && apid != "")
                {
                    string updatesql = string.Format("update jdbg_ysapjl set isread={0} where apid='{1}' ", zt, apid);
                    IList<string> lsupdatesql = new List<string>();
                    lsupdatesql.Add(updatesql);
                    code = CommonService.ExecTrans(lsupdatesql);
                    msg = code ? "" : "设置失败！";
                }
                else
                {
                    msg = "阅读状态不能为空！";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion

        #region 修改工程形象进度进行身份校验
        [Authorize]
        public void CheckXGXXJD()
        {
            string msg = "";
            bool code = true;
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh != "")
                {
                    string proc = string.Format("CheckXGXXJD('{0}', '{1}')", gcbh, CurrentUser.UserName);
                    IList<IDictionary<string, string>> dt = CommonService.ExecDataTableProc(proc, out msg);
                    if (dt.Count > 0)
                    {
                        code = dt[0]["ret"] == "1";
                        msg = dt[0]["err"];
                    }

                }
                else
                {
                    code = false;
                    msg = "工程编号不能为空！";
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
        #endregion

        #region 撤销整改单处罚校验经办人
        [Authorize]
        public void CheckCXZGDCFJBR()
        {
            string msg = "";
            bool code = true;
            try
            {
                string zgdbh = Request["zgdbh"].GetSafeString();
                if (zgdbh != "")
                {
                    string sql = string.Format("select * from view_jdbg_jdjl where lx='zgd' and  extrainfo4='{0}' and cjry='{1}'", zgdbh, CurrentUser.UserName);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        code = false;
                        msg = "您不是当前整改单的经办人，无法申请撤销处罚！";
                    }

                }
                else
                {
                    code = false;
                    msg = "整改单编号不能为空！";
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
        #endregion


        #region 整改单处罚统计页面校验权限
        [Authorize]
        public void checkZGDCFTJ()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sql = string.Format("select * from H_GCXG_SPR where lx='ZGDCFTJ' and  USERCODE='{0}' ", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    code = false;
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
        #endregion




        #region ajax长连接
        public void TestPoll()
        {
            bool code = true;
            string msg = "";
            IList<IDictionary<string, object>> data = new List<IDictionary<string, object>>();
            try
            {
                int timeout = Request["timeout"].GetSafeInt();
                DateTime dt = DateTime.Now.AddMilliseconds((double)timeout);
                bool ready = false;
                while (Response.IsClientConnected)
                {
                    Thread.Sleep(1000);
                    // 超时之后
                    if (DateTime.Compare(dt, DateTime.Now) < 0)
                    {
                        code = false;
                        msg = "timeout";
                        break;
                    }
                    // 未超时，查询新的数据
                    string sql = "select * from h_poll where isdeal=0 ";
                    data = CommonService.GetDataTable2(sql);
                    if (data.Count == 0)
                    {
                        code = false;
                        msg = "no data";
                    }
                    else
                    {
                        ready = true;
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\", \"data\": {2}}}", code ? "0" : "1", msg, jss.Serialize(data)));
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

        #endregion

        #region 删除数据
        /// <summary>
        /// 删除工程，同时删除从表
        /// </summary>
        [Authorize]
        public void DeleteIMGc()
        {
            bool code = false;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();

                IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select zt from i_m_gc where gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的工程信息";
                }
                else
                {
                    string str = dt[0]["zt"].GetSafeString();
                    if (!GcStatus.CanDelete(str))
                        msg = "工程不允许删除";
                    else
                    {
                        IList<string> sqls = new List<string>();
                        sqls.Add("delete from i_m_gc where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_jlry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_sgry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_kcry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_sjry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_jldw where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_sgdw where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_kcdw where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_sjdw where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_fgc where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_jzry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_syry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_jsdw where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_jsry where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_tsdw where gcbh='" + gcbh + "' ");
                        sqls.Add("delete from i_s_gc_tsry where gcbh='" + gcbh + "' ");


                        code = CommonService.ExecTrans(sqls);
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

        #region 获取整改单扣分条例
        public void GetZGDKFTL()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select * from h_zgdxypjbz where sfyx=1 order by xssx";
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"kftl\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }
        #endregion

        #region 整改内容扣分
        /// <summary>
        /// 显示整改单处罚统计页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ZGDNRKF()
        {
            bool ret = true;
            string msg = "";
            string lastcfsj = "1900-01-01";
            string isshow = "0";
            //string isshow = "1";
            IList<IDictionary<string, string>> cfsjlist = new List<IDictionary<string, string>>();
            try
            {
                string procstr = "GetZGDNRKFLastCFSJ()";
                cfsjlist = CommonService.ExecDataTableProc(procstr, out msg);
                ret = msg == "";
                if (cfsjlist.Count > 0)
                {
                    lastcfsj = cfsjlist[0]["lastcfsj"].GetSafeString();
                }
                else
                {
                    lastcfsj = "1900-01-01";

                }

                string sql = string.Format("select * from H_GCXG_SPR where lx='ZGDCFTJ' and  USERCODE='{0}' ", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    isshow = "1";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            ViewBag.lastcfsj = lastcfsj;
            ViewBag.currentcfsj = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.isshow = isshow;
            return View();

        }

        /// <summary>
        /// 获取整改单内容扣分统计记录（待处罚的整改单），以供筛选
        /// </summary>
        public void GetZGDNRKFTJJL()
        {
            bool ret = true;
            string msg = "";
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();

            try
            {
                if (lastcfsj == "" || currentcfsj == "")
                {
                    ret = false;
                    msg = "上次截止日期与本次截止日期不能为空";
                }
                else
                {
                    DateTime last = DateTime.Now;
                    DateTime current = DateTime.Now;
                    if (!DateTime.TryParse(lastcfsj, out last))
                    {
                        ret = false;
                        msg = "上次截止日期格式不对！";
                    }
                    else if (!DateTime.TryParse(currentcfsj, out current))
                    {
                        ret = false;
                        msg = "本次截止日期格式不对！";
                    }
                    else
                    {
                        //string sql = "";
                        //sql = string.Format("select * from view_jdbg_jdjl where lx='ZGD' ");
                        //zgjllist = CommonService.GetDataTable(sql);


                        string procstr = "";
                        procstr = string.Format("GetZGDNRKFTJJL('{0}','{1}')", lastcfsj, currentcfsj);
                        zgjllist = CommonService.ExecDataTableProc(procstr, out msg);
                        ret = msg == "";
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\", \"msg\": \"{3}\"}}", zgjllist.Count, jss.Serialize(zgjllist), ret ? "0" : "1", msg));
                Response.End();
            }
        }

        /// <summary>
        /// 导出整改单内容扣分统计记录
        /// </summary>
        public void ExportZGDNRKFTJJL()
        {
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();
            string msg = "";
            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {

                #region 标题栏

                List<KeyValuePair<string, string>> heads = new List<KeyValuePair<string, string>>();
                
                string headsql = "select field, title from h_zgdcftjexport_field where sfyx=1 order by xssx";
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
                ISheet sheet = wk.CreateSheet(lastcfsj + "-" + currentcfsj + "整改单内容扣分统计");
                IRow row;
                ICell cell;


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
                string procstr = "";
                procstr = string.Format("GetZGDNRKFTJJL('{0}','{1}')", lastcfsj, currentcfsj);
                zgjllist = CommonService.ExecDataTableProc(procstr, out msg);

                if (zgjllist.Count > 0)
                {
                    for (rows = 0; rows < zgjllist.Count; rows++)
                    {
                        IDictionary<string, string> data = zgjllist[rows];
                        row = sheet.CreateRow(rows + 1);
                        for (cols = 0; cols < heads.Count; cols++)
                        {
                            //定义每一列
                            cell = row.CreateCell(cols);
                            //设置值
                            cell.SetCellValue(data[heads[cols].Key].GetSafeString().Replace("<br/>", "\n"));
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
                    string fileName = HttpUtility.UrlEncode(lastcfsj + "到" + currentcfsj + "整改单内容扣分统计");
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }

            }
        }

        /// <summary>
        /// 导出整改单内容扣分企业扣分列表
        /// </summary>
        public void ExportZGDNRKFQYKFLB()
        {
            string msg = "";
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            IList<IDictionary<string, string>> qykflb = new List<IDictionary<string, string>>();
            HSSFWorkbook wk = new HSSFWorkbook();
            try
            {
                //居中样式
                ICellStyle cellstyle = wk.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                cellstyle.Alignment = HorizontalAlignment.Center;
                cellstyle.WrapText = true;




                #region 标题配置
                Dictionary<string, Dictionary<string, object>> st = new Dictionary<string, Dictionary<string, object>>() {
                    {
                        "房建总承包", new Dictionary<string, object>() {
                            {
                                "kfdwxz", "施工企业"
                            },
                            {
                                "heads", new List<string>() {
                                    "序号",
                                    "施工单位",
                                    "份数",
                                    "监督登记号",
                                    "工程名称",
                                    "建设单位",
                                    "整改单通知编号",
                                    "扣分值",
                                    "备注",
                                }
                            }
                        }
                    },
                    {
                        "监理企业", new Dictionary<string, object>() {
                            {
                                "kfdwxz", "监理企业"
                            },
                            {
                                "heads", new List<string>() {
                                    "序号",
                                    "监理单位",
                                    "份数",
                                    "监督登记号",
                                    "工程名称",
                                    "建设单位",
                                    "整改单通知编号",
                                    "扣分值",
                                    "备注",
                                }
                            }
                        }
                    }
                };
                #endregion


                int cols = 0;
                int rows = 0;
                IRow row;
                ICell cell;

                #region 生成工作表和标题

                foreach (var item in st)
                {
                    ISheet sheet = wk.CreateSheet(item.Key);
                    Dictionary<string, object> config = item.Value;
                    List<string> heads = config["heads"] as List<string>;
                    //定义导出标题
                    row = sheet.CreateRow(rows);
                    row.Height = 50 * 20;


                    for (cols = 0; cols < heads.Count; cols++)
                    {
                        sheet.SetColumnWidth(cols, 20 * 256);
                        // 定义每一列
                        cell = row.CreateCell(cols);
                        //设置值
                        cell.SetCellValue(heads[cols]);
                        //设置样式
                        cell.CellStyle = cellstyle;
                    }
                    sheet.CreateFreezePane(1, 1, 1, 1);
                }
                #endregion

                #region 获取数据
                string serial = "";
                if (zgdbhlist != "" && currentcfsj != "" && lastcfsj != "")
                {
                    string sql = string.Format("select top 1 workserial from JDBG_ZGDCFSQJL where cfsj='{0}' and cflx='NRKF' order by recid desc", currentcfsj);
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        serial = dt[0]["workserial"].GetSafeString();
                    }
                    if (serial == "")
                    {
                        string[] zgds = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (zgds.Count() > 0)
                        {
                            zgdbhlist = string.Join(",", zgds);
                            string procstr = "";
                            procstr = string.Format("GetZGDNRKFQYKFLB('{0}','{1}','{2}')", zgdbhlist, currentcfsj, lastcfsj);
                            qykflb = CommonService.ExecDataTableProc(procstr, out msg);
                        }
                    }
                    else
                    {
                        string proc = string.Format("GetZGDNRKFQYKFLBBYSERIAL('{0}')", serial);
                        qykflb = CommonService.ExecDataTableProc(proc, out msg);
                    }

                }
                #endregion

                #region 将数据写入工作表
                if (qykflb.Count > 0)
                {
                    foreach (var item in st)
                    {
                        ISheet sheet = wk.GetSheet(item.Key);
                        Dictionary<string, object> config = item.Value;
                        string kfdwxz = config["kfdwxz"] as string;
                        List<string> heads = config["heads"] as List<string>;
                        var datalist = qykflb.Where(x => x["kfdwxz"] == kfdwxz).ToList();
                        // excel表格中的当前行数
                        int count = 0;
                        // 序号
                        int xh = 0;
                        foreach (var data in datalist)
                        {
                            var gczgdlist = data["zgdbhlist"].Split(new char[] { ',' });
                            var zjdjhlist = data["zjdjhlist"].Split(new char[] { ',' });
                            var gcmclist = data["gcmclist"].Split(new char[] { ',' });
                            var jsdwmclist = data["jsdwmclist"].Split(new char[] { ',' });
                            var bzlist = data["bz"].Split(new char[] { ',' });
                            var ykfzlist = data["ykfz"].Split(new char[] { ',' });
                            // 当前记录实际占的行数
                            int rowcount = data["fs"].GetSafeInt();
                            xh++;
                            if (serial == "")
                            {
                                #region 插入单行
                                // 一个整改单一行
                                if (rowcount == 1)
                                {
                                    count++;
                                    row = sheet.CreateRow(count);
                                    row.Height = 50 * 20;
                                    // 定义每一列
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        cell = row.CreateCell(cols);
                                        string v = "";
                                        //设置值
                                        switch (cols)
                                        {
                                            case 0:
                                                v = xh.ToString();
                                                break;
                                            case 1:
                                                v = data["dwmc"];
                                                break;
                                            case 2:
                                                v = data["fs"];
                                                break;
                                            case 3:
                                                v = data["zjdjhlist"];
                                                break;
                                            case 4:
                                                v = data["gcmclist"];
                                                break;
                                            case 5:
                                                v = data["jsdwmclist"];
                                                break;
                                            case 6:
                                                v = data["zgdbhlist"];
                                                break;
                                            case 7:
                                                v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                break;
                                            case 8:
                                                v = data["bz"].Replace(data["zgdbhlist"] + ":", "");
                                                break;
                                            default:
                                                break;
                                        }
                                        cell.SetCellValue(v);
                                        //设置样式
                                        cell.CellStyle = cellstyle;
                                    }


                                }
                                #endregion
                                #region 插入多行
                                // 多个整改单的话，需要插入多行数据
                                else if (rowcount > 1)
                                {
                                    int num = rowcount;
                                    int start = count + 1;
                                    int end = start + num - 1;
                                    //（序号，单位名称，份数）这三列需要合并
                                    List<int> mergedcols = new List<int>() { 0, 1, 2 };
                                    //（监督登记号，工程名称，建设单位）这三列需要合并
                                    List<int> mergedsubcols = new List<int>() { 3, 4, 5 };
                                    // 创建单元格合并区域（序号，单位名称，份数）
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        if (mergedcols.Contains(cols))
                                        {
                                            sheet.AddMergedRegion(new CellRangeAddress(start, end, cols, cols));
                                        }
                                    }
                                    // 创建行
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.CreateRow(i);
                                        row.Height = 50 * 20;
                                    }
                                    // 创建单元格
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.GetRow(i);
                                        for (cols = 0; cols < heads.Count; cols++)
                                        {
                                            string v = "";

                                            if (mergedcols.Contains(cols))
                                            {
                                                if (i == start)
                                                {
                                                    cell = row.CreateCell(cols);

                                                    //设置值
                                                    switch (cols)
                                                    {
                                                        case 0:
                                                            v = xh.ToString();
                                                            break;
                                                        case 1:
                                                            v = data["dwmc"];
                                                            break;
                                                        case 2:
                                                            v = data["fs"];
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    cell.SetCellValue(v);
                                                    //设置样式
                                                    cell.CellStyle = cellstyle;
                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
                                            else
                                            {
                                                int offset = start;
                                                for (int j = 0; j < gczgdlist.Length; j++)
                                                {
                                                    var bhlist = gczgdlist[j].Split(new char[] { '#' });
                                                    var kflist = ykfzlist[j].Split(new char[] { '#' });
                                                    int subrow = bhlist.Length;
                                                    if (mergedsubcols.Contains(cols))
                                                    {
                                                        sheet.AddMergedRegion(new CellRangeAddress(offset, offset + subrow - 1, cols, cols));
                                                        cell = sheet.GetRow(offset).CreateCell(cols);
                                                        //设置值
                                                        switch (cols)
                                                        {
                                                            case 3:
                                                                v = zjdjhlist[j];
                                                                break;
                                                            case 4:
                                                                v = gcmclist[j];
                                                                break;
                                                            case 5:
                                                                v = jsdwmclist[j];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        cell.SetCellValue(v);
                                                        //设置样式
                                                        cell.CellStyle = cellstyle;
                                                    }
                                                    else
                                                    {
                                                        if (subrow == 1)
                                                        {
                                                            cell = sheet.GetRow(offset).CreateCell(cols);
                                                            //设置值
                                                            switch (cols)
                                                            {
                                                                case 6:
                                                                    v = bhlist[0];
                                                                    break;
                                                                case 7:
                                                                    v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                                    break;
                                                                case 8:
                                                                    v = bzlist.Where(x => x.StartsWith(bhlist[0] + ":")).FirstOrDefault().GetSafeString().Replace(bhlist[0] + ":", "");
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            cell.SetCellValue(v);
                                                            //设置样式
                                                            cell.CellStyle = cellstyle;
                                                        }
                                                        else if (subrow > 1)
                                                        {
                                                            for (int k = 0; k < subrow; k++)
                                                            {
                                                                cell = sheet.GetRow(offset + k).CreateCell(cols);
                                                                //设置值
                                                                switch (cols)
                                                                {
                                                                    case 6:
                                                                        v = bhlist[k];
                                                                        break;
                                                                    case 7:
                                                                        v = kflist[k] != "" ? kflist[k] + "分" : "";
                                                                        break;
                                                                    case 8:
                                                                        v = bzlist.Where(x => x.StartsWith(bhlist[k] + ":")).FirstOrDefault().GetSafeString().Replace(bhlist[k] + ":", "");
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                cell.SetCellValue(v);
                                                                //设置样式
                                                                cell.CellStyle = cellstyle;
                                                            }

                                                        }
                                                    }

                                                    offset = offset + subrow;
                                                }

                                            }

                                        }
                                    }
                                    // 这里行数需要增加多个（不止一个）
                                    count = count + num;

                                }
                                #endregion
                            }
                            else
                            {
                                #region 插入单行
                                // 一个整改单一行
                                if (rowcount == 1)
                                {
                                    count++;
                                    row = sheet.CreateRow(count);
                                    row.Height = 50 * 20;
                                    // 定义每一列
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        cell = row.CreateCell(cols);
                                        string v = "";
                                        //设置值
                                        switch (cols)
                                        {
                                            case 0:
                                                v = xh.ToString();
                                                break;
                                            case 1:
                                                v = data["dwmc"];
                                                break;
                                            case 2:
                                                v = data["fs"];
                                                break;
                                            case 3:
                                                v = data["zjdjhlist"];
                                                break;
                                            case 4:
                                                v = data["gcmclist"];
                                                break;
                                            case 5:
                                                v = data["jsdwmclist"];
                                                break;
                                            case 6:
                                                v = data["zgdbhlist"];
                                                break;
                                            case 7:
                                                v = data["ykfz"] != "" ? data["ykfz"] + "分" : "";
                                                break;
                                            case 8:
                                                v = data["bz"];
                                                break;
                                            default:
                                                break;
                                        }
                                        cell.SetCellValue(v);
                                        //设置样式
                                        cell.CellStyle = cellstyle;
                                    }


                                }
                                #endregion
                                #region 插入多行
                                // 多个整改单的话，需要插入多行数据
                                else if (rowcount > 1)
                                {
                                    int num = rowcount;
                                    int start = count + 1;
                                    int end = start + num - 1;
                                    //（序号，单位名称，份数）这三列需要合并
                                    List<int> mergedcols = new List<int>() { 0, 1, 2 };
                                    //（监督登记号，工程名称，建设单位）这三列需要合并
                                    List<int> mergedsubcols = new List<int>() { 3, 4, 5 };
                                    // 创建单元格合并区域（序号，单位名称，份数）
                                    for (cols = 0; cols < heads.Count; cols++)
                                    {
                                        if (mergedcols.Contains(cols))
                                        {
                                            sheet.AddMergedRegion(new CellRangeAddress(start, end, cols, cols));
                                        }
                                    }
                                    // 创建行
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.CreateRow(i);
                                    }
                                    // 创建单元格
                                    for (int i = start; i <= end; i++)
                                    {
                                        row = sheet.GetRow(i);
                                        row.Height = 50 * 20;
                                        for (cols = 0; cols < heads.Count; cols++)
                                        {
                                            string v = "";

                                            if (mergedcols.Contains(cols))
                                            {
                                                if (i == start)
                                                {
                                                    cell = row.CreateCell(cols);

                                                    //设置值
                                                    switch (cols)
                                                    {
                                                        case 0:
                                                            v = xh.ToString();
                                                            break;
                                                        case 1:
                                                            v = data["dwmc"];
                                                            break;
                                                        case 2:
                                                            v = data["fs"];
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    cell.SetCellValue(v);
                                                    //设置样式
                                                    cell.CellStyle = cellstyle;
                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
                                            else
                                            {
                                                int offset = start;
                                                for (int j = 0; j < gczgdlist.Length; j++)
                                                {
                                                    var bhlist = gczgdlist[j].Split(new char[] { '#' });
                                                    var kflist = ykfzlist[j].Split(new char[] { '#' });
                                                    var bznrlist = bzlist[j].Split(new char[] { '#' });
                                                    int subrow = bhlist.Length;
                                                    if (mergedsubcols.Contains(cols))
                                                    {
                                                        sheet.AddMergedRegion(new CellRangeAddress(offset, offset + subrow - 1, cols, cols));
                                                        cell = sheet.GetRow(offset).CreateCell(cols);
                                                        //设置值
                                                        switch (cols)
                                                        {
                                                            case 3:
                                                                v = zjdjhlist[j];
                                                                break;
                                                            case 4:
                                                                v = gcmclist[j];
                                                                break;
                                                            case 5:
                                                                v = jsdwmclist[j];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                        cell.SetCellValue(v);
                                                        //设置样式
                                                        cell.CellStyle = cellstyle;
                                                    }
                                                    else
                                                    {
                                                        if (subrow == 1)
                                                        {
                                                            cell = sheet.GetRow(offset).CreateCell(cols);
                                                            //设置值
                                                            switch (cols)
                                                            {
                                                                case 6:
                                                                    v = bhlist[0];
                                                                    break;
                                                                case 7:
                                                                    v = kflist[0] != "" ? kflist[0] + "分" : "";
                                                                    break;
                                                                case 8:
                                                                    v = bznrlist[0];
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                            cell.SetCellValue(v);
                                                            //设置样式
                                                            cell.CellStyle = cellstyle;
                                                        }
                                                        else if (subrow > 1)
                                                        {
                                                            for (int k = 0; k < subrow; k++)
                                                            {
                                                                cell = sheet.GetRow(offset + k).CreateCell(cols);
                                                                //设置值
                                                                switch (cols)
                                                                {
                                                                    case 6:
                                                                        v = bhlist[k];
                                                                        break;
                                                                    case 7:
                                                                        v = kflist[k] != "" ? kflist[k] + "分" : "";
                                                                        break;
                                                                    case 8:
                                                                        v = bznrlist[k];
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                cell.SetCellValue(v);
                                                                //设置样式
                                                                cell.CellStyle = cellstyle;
                                                            }

                                                        }
                                                    }

                                                    offset = offset + subrow;
                                                }

                                            }

                                        }
                                    }
                                    // 这里行数需要增加多个（不止一个）
                                    count = count + num;

                                }
                                #endregion
                            }


                        }
                    }
                }
                #endregion

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
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
                    string fileName = HttpUtility.UrlEncode(lastcfsj + "到" + currentcfsj + "整改单处罚详表");
                    //设置输出文件名
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    //输出
                    Response.BinaryWrite(memoryStram.GetBuffer());
                }
            }
        }
        /// <summary>
        /// 获取整改单内容扣分企业扣分列表数据, 用于整改处罚申请第一步
        /// </summary>
        public void GetZGDNRKFQYKFLB()
        {
            bool ret = true;
            string msg = "";
            string zgdbhlist = Request["zgdbhlist"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                if (zgdbhlist != "" && currentcfsj != "" && lastcfsj != "")
                {
                    string[] zgds = zgdbhlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (zgds.Count() > 0)
                    {
                        zgdbhlist = string.Join(",", zgds);
                        string procstr = "";
                        procstr = string.Format("GetZGDNRKFQYKFLB('{0}','{1}','{2}')", zgdbhlist, currentcfsj, lastcfsj);
                        dts = CommonService.ExecDataTableProc(procstr, out msg);
                    }

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\"}}", dts.Count, jss.Serialize(dts), ret ? "0" : "1"));
                Response.End();
            }

        }

        /// <summary>
        /// 获取整改单内容扣分企业扣分列表数据, 用于整改处罚申请审批（非第一步）
        /// </summary>
        public void GetZGDNRKFQYKFLBBYSERIAL()
        {
            bool ret = true;
            string msg = "";
            string serial = Request["serial"].GetSafeString();
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                if (serial != "")
                {
                    string procstr = "";
                    procstr = string.Format("GetZGDNRKFQYKFLBBYSERIAL('{0}')", serial);
                    dts = CommonService.ExecDataTableProc(procstr, out msg);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\"}}", dts.Count, jss.Serialize(dts), ret ? "0" : "1"));
                Response.End();
            }

        }

        #region 查看整改单回复详情
        public ActionResult viewhfxq() {
            string zgdbh = Request["zgdbh"].GetSafeString();
            ViewBag.zgdbh = zgdbh;
            return View();
        }

        public void gethfxq() {
            bool ret = true;
            string msg = "";
            string zgdbh = Request["zgdbh"].GetSafeString();
            IList<IDictionary<string, string>> dts = new List<IDictionary<string, string>>();
            try
            {
                if (zgdbh != "")
                {
                    string procstr = string.Format("GetZGDHFXQ('{0}')", zgdbh);
                    dts = CommonService.ExecDataTableProc(procstr, out msg);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\", \"data\":{2}}}", ret ? "0" : "1", msg, jss.Serialize(dts)));
                Response.End();
            }
            
           
            
            
        }
        #endregion
        #endregion

        #region 首页定制
        public void GetTodayYSAP()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string sql = "select * from view_jdbg_ysapjl where gcyssj= convert(nvarchar(max),getdate(),23) ";
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

        public ActionResult welcomeN()
        {
            return View();
        }

        [Authorize]
        public ActionResult welcome()
        {
            ViewBag.Realname = CurrentUser.RealName;
            string jt = CurrentUser.CurUser.UrlJumpType;
            string zgdurl="";
            string yssqurl="";
            if(jt.ToUpper() == "R")
            {
                zgdurl = "/WebList/EasyUiIndex?FormDm=JdbgJdjl&FormStatus=21&FormParam=PARAM--ZGD|ALL||CHECKBOX--未回复,所有|所有";
                yssqurl = "/WebList/EasyUiIndex?FormDm=GCZL_YSSQJL&FormStatus=20&FormParam=PARAM--NOT||CHECKBOX--待验,全部|待验";
            }
            else if (jt.ToUpper() == "Q")
            {
                zgdurl = "/WebList/EasyUiIndex?FormDm=JdbgJdjl&FormStatus=31&FormParam=PARAM--ZGD|NOT||CHECKBOX--未回复,所有|未回复";
                yssqurl = "/WebList/EasyUiIndex?FormDm=GCZL_YSSQJL&FormStatus=30&FormParam=PARAM--NOT||CHECKBOX--待验,全部|待验";
            }
            ViewBag.zgdurl = zgdurl;
            ViewBag.yssqurl = yssqurl;
            return View();
        }
        #endregion

        #region 获取监督记录详情
        public void GetJDJLNRXQ()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
            try
            {
                string workserial = Request["workserial"].GetSafeString();
                if (workserial !="")
                {
                    string sql = string.Format("select * from view_jdbg_jdjlnr_xq where type='txt' and workserial='{0}'", workserial);
                    dt = CommonService.GetDataTable2(sql);
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

        #region 整改单修改
        public void CheckEditZGD()
        {
            bool ret = true;
            string msg = "";
            string jbr = Request["jbr"].GetSafeString();
            string zgdbh = Request["zgdbh"].GetSafeString();
            try
            {
                if (jbr == "")
                {

                    ret = false;
                    msg = "整改单经办人不能为空！";
                }
                else if (jbr != CurrentUser.UserName)
                {
                    ret = false;
                    msg = "您不是当前整改单经办人，不能修改整改单!";
                }
                else
                {
                    string proc = string.Format("CheckEditZGD('{0}')", zgdbh);
                    IList<IDictionary<string,string>> dt = CommonService.ExecDataTableProc(proc, out msg);
                    if (dt.Count > 0)
                    {
                        ret = dt[0]["ret"].GetSafeString() == "1";
                        msg = dt[0]["err"].GetSafeString();
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
                jss.MaxJsonLength = 10240000;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\": \"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 获取单个工程信息
        public void GetGC()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                if (gcbh != "")
                {
                    string sql = string.Format("select * from view_i_m_gc where gcbh='{0}'", gcbh);
                    dt = CommonService.GetDataTable(sql);
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


        #region 施工图纸编号设置
        public void SetSGHGSBH()
        {
            string msg = "";
            bool code = true;
            try
            {
                List<string> lssql = new List<string>();
                string sql = "";
                sql = "truncate table h_gc_sghgsbhdt";
                lssql.Add(sql);
                sql = "insert into h_gc_sghgsbhdt (gcbh,gcmc,zjdjh,schgsbh) " +
                        "select gcbh, gcmc,zjdjh ,schgsbh from i_m_gc " +
                        "where (schgsbh is not null and schgsbh<>'') " +
                        "and patindex('%wzs[ABCD]%', schgsbh)>0 " +
                        "and schgsbh like '%wzs[ABCD][0-9][0-9][0-9][0-9][0-9][0-9][0-9]%'";
                lssql.Add(sql);
                CommonService.ExecTrans(lssql);

                sql = "select * from h_gc_sghgsbhdt";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    lssql.Clear();
                    foreach (var row in dt)
                    {
                        var sghgsbh = row["schgsbh"].GetSafeString();
                        if (sghgsbh !="")
                        {
                            Regex reg = new Regex(@"WZS[ABCD]\d{7}", RegexOptions.IgnoreCase);
                            MatchCollection matchCol = reg.Matches(sghgsbh);
                            List<string> bhlist = new List<string>();
                            foreach (System.Text.RegularExpressions.Match matchItem in matchCol)
                            {
                                if (!bhlist.Contains(matchItem.Value.ToUpper())){
                                    bhlist.Add(matchItem.Value.ToUpper());
                                } 
                            }
                            if (bhlist.Count>0)
                            {
                                sql = string.Format("update h_gc_sghgsbhdt set schgsbhdt='{0}' where gcbh='{1}'", string.Join(",", bhlist.ToArray()), row["gcbh"]);
                                lssql.Add(sql);
                            }
                        }
                    }
                    if (lssql.Count > 0)
                    {
                        sql = "update i_m_gc set sghgsbhdetail=b.schgsbhdt from i_m_gc a, h_gc_sghgsbhdt b where a.gcbh=b.gcbh";
                        lssql.Add(sql);
                        CommonService.ExecTrans(lssql);
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

        public void GetZGDKFTM()
        {
            bool ret = true;
            string msg = "";
            IList<IDictionary<string, string>> dt = new List<IDictionary<string, string>>();
            try
            {
                string zgdbh = Request["zgdbh"].GetSafeString();
                if (zgdbh != "")
                {
                    string procstr = string.Format("GetZGDKFTM('{0}')", zgdbh);
                    dt = CommonService.ExecDataTableProc(procstr, out msg);
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
                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"info\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(dt)));
                Response.End();
            }
        }

        #endregion

        #region 人员离职
        [Authorize]
        public void ClearRydw()
        {
            bool code = true;
            string msg = "";
            try
            {
                string rybh = Request["rybh"].GetSafeRequest();
                List<string> rylist = rybh.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries).ToList();

                string procstr = string.Format("DoLeaveCompanyBatch('{0}')", string.Join(",", rylist));
                CommonService.ExecProc(procstr, out msg);
                if (msg!="")
                {
                    code = false;
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
        #endregion

        #region 整改单处罚（新规则）
        /// <summary>
        /// 显示整改单处罚统计页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ZGDCF()
        {
            bool ret = true;
            string msg = "";
            string lastcfsj = "1900-01-01";
            string isshow = "0";
            IList<IDictionary<string, string>> cfsjlist = new List<IDictionary<string, string>>();
            try
            {
                string procstr = "GetZGDCFLastCFSJ()";
                cfsjlist = CommonService.ExecDataTableProc(procstr, out msg);
                ret = msg == "";
                if (cfsjlist.Count > 0)
                {
                    lastcfsj = cfsjlist[0]["lastcfsj"].GetSafeString();
                }
                else
                {
                    lastcfsj = "1900-01-01";

                }

                string sql = string.Format("select * from H_GCXG_SPR where lx='ZGDCFTJ' and  USERCODE='{0}' ", CurrentUser.UserName);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    isshow = "1";
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            ViewBag.lastcfsj = lastcfsj;
            ViewBag.currentcfsj = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.isshow = isshow;
            return View();

        }

        /// <summary>
        /// 获取整改单处罚统计记录（待处罚的整改单），以供筛选
        /// </summary>
        public void GetZGDCFTJJL()
        {
            bool ret = true;
            string msg = "";
            string lastcfsj = Request["lastcfsj"].GetSafeString();
            string currentcfsj = Request["currentcfsj"].GetSafeString();
            string isnewrule = Request["isnewrule"].GetSafeString("0");
            string ignoredtoken = "";
            IList<IDictionary<string, string>> zgjllist = new List<IDictionary<string, string>>();

            try
            {
                if (lastcfsj == "" || currentcfsj == "")
                {
                    ret = false;
                    msg = "上次截止日期与本次截止日期不能为空";
                }
                else
                {
                    DateTime last = DateTime.Now;
                    DateTime current = DateTime.Now;
                    if (!DateTime.TryParse(lastcfsj, out last))
                    {
                        ret = false;
                        msg = "上次截止日期格式不对！";
                    }
                    else if (!DateTime.TryParse(currentcfsj, out current))
                    {
                        ret = false;
                        msg = "本次截止日期格式不对！";
                    }
                    else
                    {
                        string procstr = "";
                        // 按照新的规则统计
                        if (isnewrule == "1")
                        {
                            string proc = string.Format("GetGuid()");
                            IList<IDictionary<string,string>> info = CommonService.ExecDataTableProc(proc, out msg);
                            if (info.Count > 0)
                            {
                                ignoredtoken = info[0]["id"].GetSafeString();
                            }
                            if (ignoredtoken !="")
                            {
                                procstr = string.Format("GetZGDCFTJJLNEW('{0}','{1}','{2}')", lastcfsj, currentcfsj, ignoredtoken);
                                zgjllist = CommonService.ExecDataTableProc(procstr, out msg);
                                ret = msg == "";
                            }
                            else
                            {
                                ret = false;
                                msg = "获取数据失败！";
                            }

                        }
                        else
                        {
                            procstr = string.Format("GetZGDCFTJJL('{0}','{1}')", lastcfsj, currentcfsj);
                            zgjllist = CommonService.ExecDataTableProc(procstr, out msg);
                            ret = msg == "";
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
                jss.MaxJsonLength = int.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"total\":{0},\"rows\":{1}, \"code\":\"{2}\", \"msg\": \"{3}\",\"ignoredtoken\": \"{4}\"}}", zgjllist.Count, jss.Serialize(zgjllist), ret ? "0" : "1", msg, ignoredtoken));
                Response.End();
            }
        }
        #endregion


        #region 个人管理-接受短信设置
        [Authorize]
        public ActionResult SetSMSRecieve()
        {
            string rylx = "";
            string qybh = "";
            string sql = string.Format("select qybh, zhlx from I_M_QYZH where yhzh='{0}'", CurrentUser.UserName);
            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
            if (dt.Count > 0)
            {
                rylx = dt[0]["zhlx"];
                qybh = dt[0]["qybh"];
            }
            string alloptions = "";
            string selected = "";
            if (rylx !="")
            {
                // 获取 当前人员类型可以设置的短信种类
                sql = string.Format("select bh, mc, memo from JD_SMS_SENDTYPE where sfyx=1 and charindex(',{0},',','+lx+',') > 0  order by xssx ",rylx);
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    List<string> l = new List<string>();
                    foreach (var item in dt)
                    {
                        l.Add(string.Format("{0}|{1}|{2}",item["bh"],item["mc"],item["memo"]));
                    }
                    if (l.Count > 0)
                    {
                        alloptions = string.Join("||", l);
                    }
                    
                }

                // 获取当前人员已经设置的短信种类
                if (rylx == "R")
                {
                    sql = string.Format("select smsbhlist from I_M_RY where rybh='{0}'", qybh);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        selected = dt[0]["smsbhlist"];
                    }
                }
                else if (rylx == "N")
                {
                    sql = string.Format("select smsbhlist from userinfo where UserCode='{0}'", CurrentUser.UserName);
                    dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        selected = dt[0]["smsbhlist"];
                    }
                }
            }

            ViewBag.alloptions = alloptions;
            ViewBag.selected = selected;

            return View();
        }

        // 保存接受短信设置
        [Authorize]
        public void DoSaveSmsRecieve()
        {
            bool code = true;
            string msg = "";
            try
            {
                string smsset = Request["smsset"].GetSafeString();
                
                string procstr = string.Format("DoSaveSmsRecieve('{0}','{1}')", smsset, CurrentUser.UserName);
                CommonService.ExecProc(procstr, out msg);
                if (msg != "")
                {
                    code = false;
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
        #endregion

        #region 监督站内部工程查看，包含所有信息
        /// <summary>
        /// 监督站内部工程查看，包含所有信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gccknb()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.zjdjh = Request["zjdjh"].GetSafeString();
            ViewBag.gclxbh = Request["gclxbh"].GetSafeString();
            ViewBag.lszjdjh = Request["lszjdjh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 企业人员查看工程信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Gcckwb()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeString();
            ViewBag.zjdjh = Request["zjdjh"].GetSafeString();
            ViewBag.gclxbh = Request["gclxbh"].GetSafeString();
            ViewBag.lszjdjh = Request["lszjdjh"].GetSafeString();
            return View();
        }

        /// <summary>
        /// 获取外部人员查看工程的菜单
        /// </summary>
        [Authorize]
        public void GetGcckwbMenu()
        {
            IList<VCheckItem> ret = new List<VCheckItem>();
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string zjdjh = Request["zjdjh"].GetSafeString();
                string lszjdjh = Request["lszjdjh"].GetSafeString();
                string msg = "";
                VJdbgReportSumItem item = JdbgService.GetReportSum(gcbh, out msg);
                if (msg != "")
                {
                    SysLog4.WriteError("获取工程报告信息失败：" + msg);
                }

                int bhgbg = 0;
                int sybg = 0;
                bhgbg = JcjgBgService.GetBgsl(zjdjh, "2", "WZJCJG", lszjdjh);
                sybg = JcjgBgService.GetBgsl(zjdjh, "", "WZJCJG", lszjdjh);
                // 工程基本信息
                ret.Add(new VCheckItem() { id = "I_JBXX", pId = "", name = "工程基本信息(1)", isParent = false, cevent = "I_JBXX", open = true });
                // 工程监督方案
                ret.Add(new VCheckItem() { id = "I_JDFA", pId = "", name = "工程监督方案(" + item.SumJDFA + ")", isParent = false, cevent = "I_JDFA", open = true });
                ret.Add(new VCheckItem() { id = "I_JDJD", pId = "", name = "监督交底(" + item.SumJDJD + ")", isParent = false, cevent = "I_JDJD", open = true });
                //ret.Add(new VCheckItem() { id = "04", pId = "", name = "质量行为监督检查记录(" + item.SumZLXWJCJL + ")", isParent = false, cevent = "04", open = true });
                ret.Add(new VCheckItem() { id = "G_ZLYS", pId = "", name = "工程质量监督验收", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_YSSQ", pId = "G_ZLYS", name = "工程验收申请记录(" + item.SumYSSQJL + ")", isParent = false, cevent = "I_YSSQ", open = false });
                ret.Add(new VCheckItem() { id = "I_YSAP", pId = "G_ZLYS", name = "工程验收安排记录(" + item.SumYSAPJL + ")", isParent = false, cevent = "I_YSAP", open = false });
                //ret.Add(new VCheckItem() { id = "I_JDJL", pId = "G_ZLYS", name = "监督记录(" + item.SumJDJL + ")", isParent = false, cevent = "I_JDJL", open = false });
                ret.Add(new VCheckItem() { id = "I_ZGTZ", pId = "G_ZLYS", name = "整改通知(" + item.SumZgdSp + ")", isParent = false, cevent = "I_ZGTZ", open = false });

                ret.Add(new VCheckItem() { id = "G_JCBG", pId = "", name = "检测报告查询", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_BHGBG", pId = "G_JCBG", name = "不合格检测报告("+bhgbg +")", isParent = false, cevent = "I_BHGBG", open = false });
                ret.Add(new VCheckItem() { id = "I_SYBG", pId = "G_JCBG", name = "所有检测报告(" + sybg + ")", isParent = false, cevent = "I_SYBG", open = false });
                //ret.Add(new VCheckItem() { id = "G_QTZL", pId = "", name = "其他资料", isParent = true, cevent = "", open = true });

                //ret.Add(new VCheckItem() { id = "I_JLYB", pId = "G_QTZL", name = "监理月报(0)", isParent = false, cevent = "I_JLYB", open = false });

                //ret.Add(new VCheckItem() { id = "0605", pId = "06", name = "扣分清单(0)", isParent = false, cevent = "0605", open = false });
                //ret.Add(new VCheckItem() { id = "0606", pId = "06", name = "现场图片(0)", isParent = false, cevent = "0606", open = false });
                //ret.Add(new VCheckItem() { id = "I_JDBG", pId = "", name = "工程质量监督报告(" + item.SumJDBG + ")", isParent = false, cevent = "I_JDBG", open = true });
                ret.Add(new VCheckItem() { id = "I_RYDD", pId = "", name = "工程人员调动记录(" + item.SumRYLZJL + ")", isParent = false, cevent = "I_RYDD", open = true });
                ret.Add(new VCheckItem() { id = "I_DWDD", pId = "", name = "工程单位调换记录(" + 0 + ")", isParent = false, cevent = "I_DWDD", open = true });
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
                string lszjdjh = Request["lszjdjh"].GetSafeString();
                string msg = "";
                VJdbgReportSumItem item = JdbgService.GetReportSum(gcbh, out msg);
                if (msg != "")
                {
                    SysLog4.WriteError("获取工程报告信息失败：" + msg);
                }
                // 获取竣工验收资料数量
                int jgyszlsl = 0;
                string sql = string.Format("select count(*) as sum from VIEW_JGYSZL where gcbh='{0}'", gcbh);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    jgyszlsl = dt[0]["sum"].GetSafeInt();
                }
                int bhgbg = 0;
                int sybg = 0;
                bhgbg = JcjgBgService.GetBgsl(zjdjh, "2","WZJCJG", lszjdjh);
                sybg = JcjgBgService.GetBgsl(zjdjh, "","WZJCJG", lszjdjh);

                // 获取监督抽查联系单数量
                int jdcclxdsl = 0;
                sql = string.Format("select count(*) as sum from jdbg_jdccrwwtjl where gcbh='{0}'", gcbh);
                dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    jdcclxdsl = dt[0]["sum"].GetSafeInt();
                }
                // 工程基本信息
                ret.Add(new VCheckItem() { id = "I_JBXX", pId = "", name = "工程基本信息(1)", isParent = false, cevent = "I_JBXX", open = true });
                // 工程监督方案
                ret.Add(new VCheckItem() { id = "I_JDFA", pId = "", name = "工程监督方案(" + item.SumJDFA + ")", isParent = false, cevent = "I_JDFA", open = true });
                ret.Add(new VCheckItem() { id = "I_JDJD", pId = "", name = "监督交底(" + item.SumJDJD + ")", isParent = false, cevent = "I_JDJD", open = true });
                //ret.Add(new VCheckItem() { id = "04", pId = "", name = "质量行为监督检查记录(" + item.SumZLXWJCJL + ")", isParent = false, cevent = "04", open = true });
                ret.Add(new VCheckItem() { id = "G_ZLYS", pId = "", name = "工程质量监督验收", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_YSSQ", pId = "G_ZLYS", name = "工程验收申请记录(" + item.SumYSSQJL + ")", isParent = false, cevent = "I_YSSQ", open = false });
                ret.Add(new VCheckItem() { id = "I_YSAP", pId = "G_ZLYS", name = "工程验收安排记录(" + item.SumYSAPJL + ")", isParent = false, cevent = "I_YSAP", open = false });
                ret.Add(new VCheckItem() { id = "I_JDJL", pId = "G_ZLYS", name = "监督记录(" + item.SumJDJL + ")", isParent = false, cevent = "I_JDJL", open = false });
                ret.Add(new VCheckItem() { id = "I_ZGTZ", pId = "G_ZLYS", name = "整改通知(" + item.SumZgd + ")", isParent = false, cevent = "I_ZGTZ", open = false });
                ret.Add(new VCheckItem() { id = "I_JGYSTZ", pId = "G_ZLYS", name = "竣工验收通知书(" + item.SumJGYSJL + ")", isParent = false, cevent = "I_JGYSTZ", open = false });

                ret.Add(new VCheckItem() { id = "G_JCBG", pId = "", name = "检测报告查询", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_BHGBG", pId = "G_JCBG", name = "不合格检测报告(" + bhgbg + ")", isParent = false, cevent = "I_BHGBG", open = false });
                ret.Add(new VCheckItem() { id = "I_SYBG", pId = "G_JCBG", name = "所有检测报告(" + sybg + ")", isParent = false, cevent = "I_SYBG", open = false });
                //ret.Add(new VCheckItem() { id = "G_QTZL", pId = "", name = "其他资料", isParent = true, cevent = "", open = true });

                //ret.Add(new VCheckItem() { id = "I_JLYB", pId = "G_QTZL", name = "监理月报(0)", isParent = false, cevent = "I_JLYB", open = false });

                //ret.Add(new VCheckItem() { id = "0605", pId = "06", name = "扣分清单(0)", isParent = false, cevent = "0605", open = false });
                //ret.Add(new VCheckItem() { id = "0606", pId = "06", name = "现场图片(0)", isParent = false, cevent = "0606", open = false });
                ret.Add(new VCheckItem() { id = "I_JDCCLXD", pId = "", name = "工程监督抽查联系单(" + jdcclxdsl + ")", isParent = false, cevent = "I_JDCCLXD", open = true });
                ret.Add(new VCheckItem() { id = "I_JDBG", pId = "", name = "工程质量监督报告(" + item.SumJDBG + ")", isParent = false, cevent = "I_JDBG", open = true });
                ret.Add(new VCheckItem() { id = "I_RYDD", pId = "", name = "工程人员调动记录(" + item.SumRYLZJL + ")", isParent = false, cevent = "I_RYDD", open = true });
                ret.Add(new VCheckItem() { id = "I_DWDD", pId = "", name = "工程单位调换记录(" + item.SumQYLZJL + ")", isParent = false, cevent = "I_DWDD", open = true });
                ret.Add(new VCheckItem() { id = "I_GCBZ", pId = "", name = "监督人员备注(" + item.SumJDYBZ + ")", isParent = false, cevent = "I_GCBZ", open = true });

                ret.Add(new VCheckItem() { id = "G_GDZL", pId = "", name = "归档资料", isParent = true, cevent = "", open = true });
                ret.Add(new VCheckItem() { id = "I_JGYSZL", pId = "G_GDZL", name = "竣工验收资料(" + jgyszlsl.ToString() + ")", isParent = false, cevent = "I_JGYSZL", open = false });
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

        #region 检测监管页面跳转
        public void gotoJCJGPage()
        {
            try
            {
                string username = CurrentUser.RealUserName;
                string loginurl = "";
                string pageurl = "";
                string lx = Request["lx"].GetSafeString();
                string pageid = Request["pageid"].GetSafeString();
                string sql = string.Format("select loginurl,defaultusername from SysMenuJumpLoginUrl where lx='{0}'", lx);
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    loginurl = dt[0]["loginurl"];
                    string defaultusername = dt[0]["defaultusername"].GetSafeString();
                    if (defaultusername !="")
                    {
                        username = defaultusername;
                    }
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
        #endregion

        #region 获取检测报告 数量和记录
        private int GetBgsl(string zjdjh, string jcjg)
        {
            int count = 0;
            try
            {
                // JSON 序列化和反序列化类
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string url = "http://wzjcjg.jzyglxt.com/api/apizjz/PageReports";
                string postdata = "";
                postdata += "StationId=8CC246721E7F4492B2FF8DC0A527AF9E&Key=155E7133C96F18B638348BEE61560609";
                postdata += "&zjdjh=" + zjdjh + "&jcjg=" + jcjg;
                postdata += "&PageIndex=1" + "&PageSize=1";
                string retstring = SendDataByPost(url, postdata);
                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                if (retdata != null)
                {
                    string code = retdata["code"].GetSafeString();
                    string retmsg = retdata["msg"].GetSafeString();
                    int totalcount = retdata["totalcount"].GetSafeInt();
                    if (code == "true")
                    {
                        count = totalcount;
                    }

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }


            return count;
        }
        // 不合格报告
        public void GetBHGBG()
        {
            bool ret = true;
            string msg = "";
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                int page = 1;
                int pageSize = Request["pagesize"].GetSafeInt(5);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                // 查询数据
                string sql = "select * from H_JCJG_JDCCLXD_CONFIG where lx='JCBG'";
                IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                string url = "";
                string postdata = "";
                if (dt.Count > 0)
                {
                    url = dt[0]["url"].GetSafeString();
                    postdata = dt[0]["fixedparam"].GetSafeString();
                }
                postdata +=  (postdata=="" ? "":"&" ) + "PageIndex=" + page.ToString() + "&PageSize=" + pageSize.ToString() + "&jcjg=2";

                string retstring = SendDataByPost(url, postdata);
                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                if (retdata != null)
                {
                    string code = retdata["code"].GetSafeString();
                    string retmsg = retdata["msg"].GetSafeString();
                    int totalcount = retdata["totalcount"].GetSafeInt();
                    if (code == "true")
                    {
                        ArrayList arr = (ArrayList)retdata["records"];
                        if (arr.Count > 0)
                        {
                            // 将JSON数据包 转成list对象                                    
                            foreach (var item in arr)
                            {
                                Dictionary<string, object> r = (Dictionary<string, object>)item;
                                data.Add(r);
                            }
                        }
                    }
                    else
                    {
                        ret = false;
                        msg = retmsg;
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

                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "0" : "1", msg, jss.Serialize(data)));
                Response.End();
            }
        }
        #endregion

        #region 发送GET, POST请求
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
                Response.Write(string.Format("{{\"success\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "true" : "false", msg, jss.Serialize(info)));
                Response.End();
            }
        }
        /// <summary>
        /// 测试接口
        /// </summary>
        public void GetWebListData2()
        {
            bool ret = true;
            string msg = "";
            int total = 0;
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            try
            {
                string lx = Request.QueryString["lx"].GetSafeString();
                if (lx != "")
                {
                    #region 查询配置信息
                    string sql = string.Format("select top 1 * from formapi where lx='{0}'", lx);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    #endregion

                    // 无法获取信息，报错
                    if (dt.Count == 0)
                    {
                        ret = false;
                        msg = "无法获取配置信息！";
                    }
                    else
                    {
                        // 是否需要重新加载数据
                        // 主要用于过滤条件和查参数中有互相矛盾的情况
                        bool needtoload = true;

                        // JSON 序列化和反序列化类
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;

                        #region 获取配置信息
                        // 提取配置信息
                        IDictionary<string, object> config = dt[0];
                        // 数据配置
                        string urlDataConfig = config["urldataconfig"].GetSafeString();
                        string pageParamConfig = config["pageparamconfig"].GetSafeString();
                        string forgeFilterParamConfig = config["forgefilterparamconfig"].GetSafeString();
                        string retDataConfig = config["retdataconfig"].GetSafeString();
                        string orderbyConfig = config["orderbyconfig"].GetSafeString();
                        string ignoredParamConfig = config["ignoredparamconfig"].GetSafeString();
                        string requestMethod = config["requestmethod"].GetSafeString();
                        Dictionary<string, object> dtUrlDataConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtPageParamConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtForgeFilterParamConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtRetDataConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtOrderbyConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtIgnoredParamConfig = new Dictionary<string, object>();

                        if (urlDataConfig != "")
                        {
                            dtUrlDataConfig = jss.Deserialize<Dictionary<string, object>>(urlDataConfig);
                        }
                        if (pageParamConfig != "")
                        {
                            dtPageParamConfig = jss.Deserialize<Dictionary<string, object>>(pageParamConfig);
                        }
                        if (forgeFilterParamConfig != "")
                        {
                            dtForgeFilterParamConfig = jss.Deserialize<Dictionary<string, object>>(forgeFilterParamConfig);
                        }
                        if (retDataConfig != "")
                        {
                            dtRetDataConfig = jss.Deserialize<Dictionary<string, object>>(retDataConfig);
                        }
                        if (orderbyConfig != "")
                        {
                            dtOrderbyConfig = jss.Deserialize<Dictionary<string, object>>(orderbyConfig);
                        }
                        if (ignoredParamConfig != "")
                        {
                            dtIgnoredParamConfig = jss.Deserialize<Dictionary<string, object>>(ignoredParamConfig);
                        }
                        #endregion


                        if (dtUrlDataConfig == null || !dtUrlDataConfig.ContainsKey("url") || dtUrlDataConfig["url"].GetSafeString() == "")
                        {
                            ret = false;
                            msg = "请求地址配置不能为空！";
                        }
                        else
                        {
                            // 参数列表（来源：form、query、自定义）
                            List<KeyValuePair<string, string>> paramlist = new List<KeyValuePair<string, string>>();

                            #region 获取分页参数、过滤条件
                            // 获取分页参数
                            int page = Request.Form["page"].GetSafeInt(1);
                            int pageSize = Request.Form["pageSize"].GetSafeInt(20);
                            // 获取过滤条件
                            string filterRules = Request.Form["filterRules"].GetSafeString();

                            #endregion

                            #region 解析过滤条件
                            List<Dictionary<string, object>> filters = new List<Dictionary<string, object>>();
                            if (filterRules != "")
                            {
                                ArrayList al = jss.Deserialize<ArrayList>(filterRules);
                                if (al.Count > 0)
                                {
                                    foreach (var item in al)
                                    {
                                        filters.Add((Dictionary<string, object>)item);
                                    }

                                }
                            }
                            #endregion

                            #region  根据过滤条件生成参数列表
                            if (filters.Count > 0)
                            {
                                foreach (var f in filters)
                                {
                                    string fieldname = f["fieldname"].GetSafeString();
                                    string fieldvalue = f["fieldvalue"].GetSafeString();
                                    string fieldopt = f["fieldopt"].GetSafeString();
                                    if (fieldname != "" && fieldvalue != "")
                                    {
                                        paramlist.Add(new KeyValuePair<string, string>(fieldname.ToLower(), fieldvalue));

                                    }
                                }
                            }
                            #endregion

                            #region 将query参数合并到参数列表中,并去重
                            // 忽略固定的query参数:lx
                            List<string> keylist = Request.QueryString.AllKeys.Select(x => x.ToLower()).Where(x => x != "lx").ToList();

                            if (keylist.Count > 0)
                            {
                                foreach (var k in keylist)
                                {
                                    var p = paramlist.Where(x => x.Key == k);
                                    if (p.Count() > 0)
                                    {
                                        var fp = p.First();
                                        string fv = fp.Value;
                                        string qv = HttpUtility.UrlDecode(Request.QueryString[k]).Trim().GetSafeString();
                                        if (qv != "" && qv != fv)
                                        {
                                            needtoload = false; // url中的参数和filterRule中的查询条件有矛盾的地方
                                        }

                                    }
                                    else
                                    {
                                        paramlist.Add(new KeyValuePair<string, string>(k, Request.QueryString[k].GetSafeString()));
                                    }

                                }
                            }
                            #endregion

                            #region 获取字段映射配置
                            IList<IDictionary<string, string>> mpc = new List<IDictionary<string, string>>();
                            sql = string.Format("select * from FORMAPICONFIG where sfyx=1 and lx='{0}'", lx);
                            mpc = CommonService.GetDataTable(sql);
                            #endregion

                            #region 生成排序字段
                            Dictionary<string, string> dtOrderbyInfo = new Dictionary<string, string>();
                            string sort = "";
                            string order = "";
                            sort = Request.Form["sort"].GetSafeString();
                            order = Request.Form["order"].GetSafeString();
                            if (sort != "" && order != "")
                            {
                                var q = mpc.Select(x => new { src = x["sourcefield"].GetSafeString(), dest = x["destfield"].GetSafeString() }).Where(x => x.dest.ToLower() == sort.ToLower());
                                if (q.Count() > 0)
                                {
                                    sort = q.First().src;
                                }
                                dtOrderbyInfo.Add("sort", sort);
                                dtOrderbyInfo.Add("order", order);
                            }
                            #endregion

                            #region 查询数据
                            if (needtoload)
                            {

                                #region 在发送请求之前，修改paramlist
                                if (dtForgeFilterParamConfig != null)
                                {
                                    if (dtForgeFilterParamConfig.ContainsKey("method"))
                                    {
                                        string fm = dtForgeFilterParamConfig["method"].GetSafeString();
                                        if (fm != "")
                                        {
                                            paramlist = (List<KeyValuePair<string, string>>)this.InvokeMethod(fm, new object[] { paramlist });
                                        }
                                    }
                                }

                                #endregion

                                #region 根据paramlist和配置项生成请求参数字符串

                                string urldata = "";

                                #region 获取配置的分页参数
                                string pageField = "page";
                                string pageSizeField = "pageSize";
                                if (dtPageParamConfig != null)
                                {
                                    if (dtPageParamConfig.ContainsKey("page"))
                                    {
                                        var v = dtPageParamConfig["page"].GetSafeString();
                                        if (v != "")
                                        {
                                            pageField = v;
                                        }
                                    }
                                    if (dtPageParamConfig.ContainsKey("pageSize"))
                                    {
                                        var v = dtPageParamConfig["pageSize"].GetSafeString();
                                        if (v != "")
                                        {
                                            pageSizeField = v;
                                        }
                                    }
                                }
                                urldata += string.Format("{0}={1}&{2}={3}", pageField, page.ToString(), pageSizeField, pageSize.ToString());
                                #endregion

                                #region 获取配置的固定参数
                                if (dtUrlDataConfig.ContainsKey("fixedParams"))
                                {
                                    var fixedParams = dtUrlDataConfig["fixedParams"].GetSafeString();
                                    if (fixedParams != "")
                                    {
                                        urldata += "&" + fixedParams;
                                    }
                                }
                                #endregion

                                #region 获取配置的额外参数
                                if (dtUrlDataConfig.ContainsKey("extraParamsMethod"))
                                {
                                    var extraParamsMethod = dtUrlDataConfig["extraParamsMethod"].GetSafeString();
                                    if (extraParamsMethod != "")
                                    {
                                        Dictionary<string, object> extraParamInfo = (Dictionary<string, object>)this.InvokeMethod(extraParamsMethod, new object[] { paramlist });
                                        bool issuccess = extraParamInfo["code"].GetSafeBool();
                                        if (!issuccess)
                                        {
                                            throw new Exception(extraParamInfo["msg"].GetSafeString());
                                        }
                                        else
                                        {
                                            string extraParam = extraParamInfo["data"].GetSafeString();
                                            if (extraParam != "")
                                            {
                                                urldata += "&" + extraParam;
                                            }
                                        }

                                    }
                                }
                                #endregion                                

                                #region 获取配置的orderby参数
                                if (dtOrderbyConfig != null && dtOrderbyConfig.Count > 0)
                                {
                                    bool enable = dtOrderbyConfig["enable"].GetSafeBool();
                                    string paramName = dtOrderbyConfig["paramName"].GetSafeString();
                                    string paramFormat = dtOrderbyConfig["paramFormat"].GetSafeString();
                                    // 启用orderby
                                    if (enable)
                                    {
                                        string orderby = "";
                                        string field = "";
                                        string fieldorder = "";
                                        if (dtOrderbyInfo != null && dtOrderbyInfo.Count > 0)
                                        {
                                            field = dtOrderbyInfo["sort"];
                                            fieldorder = dtOrderbyInfo["order"];
                                            orderby = string.Format("{0}={1}", paramName, HttpUtility.UrlEncode(paramFormat.Replace("{sort}", field).Replace("{order}", fieldorder)));
                                        }
                                        else
                                        {
                                            if (dtOrderbyConfig.ContainsKey("defaultParam"))
                                            {
                                                var v = dtOrderbyConfig["defaultParam"].GetSafeString();
                                                if (v != null)
                                                {
                                                    orderby = string.Format("{0}={1}", paramName, HttpUtility.UrlEncode(v));
                                                }
                                            }
                                        }

                                        if (orderby != "")
                                        {
                                            urldata += "&" + orderby;
                                        }
                                    }
                                }
                                #endregion

                                #region 获取paramlist参数
                                // 过滤掉需要忽略的参数
                                if (dtIgnoredParamConfig != null && dtIgnoredParamConfig.Count > 0)
                                {
                                    if (dtIgnoredParamConfig.ContainsKey("ignoredlist"))
                                    {
                                        ArrayList al = (ArrayList)dtIgnoredParamConfig["ignoredlist"];
                                        if (al != null && al.Count > 0)
                                        {
                                            List<string> ipms = al.ToArray().Select(x => x.GetSafeString().ToLower()).ToList();
                                            paramlist = paramlist.Where(x => !ipms.Contains(x.Key.ToLower())).ToList();
                                        }
                                    }
                                }
                                foreach (var item in paramlist)
                                {
                                    urldata += "&" + item.Key + "=" + HttpUtility.UrlEncode(item.Value);
                                }
                                #endregion

                                #endregion

                                #region 发送请求，获取数据
                                // 请求返回的字符串
                                string retstring = "";
                                // 获取配置的第三方请求URL
                                string url = dtUrlDataConfig["url"].GetSafeString();


                                // 默认请求方式为POST
                                if (requestMethod == "")
                                {
                                    requestMethod = "POST";
                                }
                                // POST请求方式
                                if (requestMethod == "POST")
                                {
                                    retstring = SendDataByPost(url, urldata);
                                }
                                else
                                {
                                    // GET 请求
                                    url += (url.Contains("?") ? "&" : "?") + urldata;
                                    retstring = SendDataByGET(url);
                                }
                                #endregion

                                #region 处理返回的数据
                                SysLog4.WriteError(retstring);
                                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                                if (retdata != null)
                                {
                                    #region 获取返回数据的配置项
                                    string codeField = "code";
                                    string msgField = "msg";
                                    string totalField = "total";
                                    string dataField = "rows";
                                    string successMethod = "";
                                    string postMethod = "";
                                    if (dtRetDataConfig != null && dtRetDataConfig.Count > 0)
                                    {
                                        if (dtRetDataConfig.ContainsKey("code"))
                                        {
                                            var v = dtRetDataConfig["code"].GetSafeString();
                                            if (v != "")
                                            {
                                                codeField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("msg"))
                                        {
                                            var v = dtRetDataConfig["msg"].GetSafeString();
                                            if (v != "")
                                            {
                                                msgField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("total"))
                                        {
                                            var v = dtRetDataConfig["total"].GetSafeString();
                                            if (v != "")
                                            {
                                                totalField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("rows"))
                                        {
                                            var v = dtRetDataConfig["rows"].GetSafeString();
                                            if (v != "")
                                            {
                                                dataField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("successMethod"))
                                        {
                                            var v = dtRetDataConfig["successMethod"].GetSafeString();
                                            if (v != "")
                                            {
                                                successMethod = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("postMethod"))
                                        {
                                            var v = dtRetDataConfig["postMethod"].GetSafeString();
                                            if (v != "")
                                            {
                                                postMethod = v;
                                            }
                                        }
                                    }
                                    #endregion

                                    object code = retdata[codeField];
                                    string retmsg = retdata[msgField].GetSafeString();
                                    int totalcount = retdata[totalField].GetSafeInt();
                                    total = totalcount;
                                    bool issucess = false;
                                    if (successMethod != "")
                                    {
                                        issucess = (bool)this.InvokeMethod(successMethod, new object[] { code });
                                    }
                                    else
                                    {
                                        msg = "必须设置successMethod！";
                                    }

                                    if (issucess)
                                    {
                                        // 包装数据
                                        ArrayList arr = (ArrayList)retdata[dataField];
                                        if (arr != null && arr.Count > 0)
                                        {
                                            // 将JSON数据包 转成list对象
                                            List<Dictionary<string, object>> tmp = new List<Dictionary<string, object>>();
                                            foreach (var item in arr)
                                            {
                                                Dictionary<string, object> r = (Dictionary<string, object>)item;
                                                tmp.Add(r);
                                            }
                                            //数据映射
                                            var mp = mpc.Select(x => new { src = x["sourcefield"].GetSafeString(), dest = x["destfield"].GetSafeString() }).ToList();
                                            // 遍历每一行数据
                                            foreach (var item in tmp)
                                            {
                                                Dictionary<string, object> t = new Dictionary<string, object>();
                                                //遍历每一个字段
                                                foreach (var f in item)
                                                {
                                                    var q = mp.Where(x => x.src.Equals(f.Key, StringComparison.OrdinalIgnoreCase));
                                                    // 字段需要映射
                                                    if (q.Count() > 0)
                                                    {
                                                        t.Add(q.First().dest.ToLower(), f.Value);
                                                    }
                                                    else
                                                    {
                                                        // 不需要映射
                                                        t.Add(f.Key.ToLower(), f.Value);
                                                    }
                                                }
                                                data.Add(t);
                                            }
                                        }
                                        // 后处理
                                        if (postMethod != "")
                                        {
                                            data = (List<Dictionary<string, object>>)this.InvokeMethod(postMethod, new object[] { data });
                                        }
                                    }
                                    else
                                    {
                                        ret = false;
                                        msg = retmsg;
                                    }
                                }
                                else
                                {
                                    ret = false;
                                    msg = "没有返回数据！";
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    ret = false;
                    msg = "参数不全！";
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
                Dictionary<string, object> info = new Dictionary<string, object>();
                info.Add("total", total);
                info.Add("rows", data);
                Response.Write(string.Format("{{\"success\":\"{0}\", \"msg\":\"{1}\",\"data\": {2}}}", ret ? "true" : "false", msg, jss.Serialize(info)));
                Response.End();
            }
        }
        #endregion

        #region 反射调用本控制器中方法
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
        #endregion

        #region weblist第三方接口杂类函数
        /// <summary>
        /// 校验获取检测报告是否成功
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool checkJCBGSuccess(object code)
        {
            return code.GetSafeBool();
        }
        #endregion


        #region 监督记录模板修改内容
        public void ModifyJdjlTpl()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string sql = string.Format("select * from tmp_modifyjdjl");
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    foreach (var row in dt)
                    {
                        string fileid = row["reportfile"].GetSafeString();
                        string content = row["jdjl"].DecodeBase64(Encoding.GetEncoding("GB2312"));
                        // 替换换行符
                        content = content.Replace("\n", "\r\n");
                        content = content.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;", " ");
                        string[] contentarr = content.Split(new string[] { "<br>", "<br/>", "<br />" }, StringSplitOptions.RemoveEmptyEntries);

                        Regex reg = new Regex("<img[^>]*>");
                        Regex regImage = new Regex(@"/p-s\w+.jpg");
                        string recids = "";
                        for (int i = 0; i < contentarr.Length; i++)
                        {
                            string strRow = contentarr[i];
                            MatchCollection matchCol = reg.Matches(strRow);
                            if (matchCol.Count > 0)
                            {
                                foreach (System.Text.RegularExpressions.Match matchItem in matchCol)
                                {
                                    if (regImage.IsMatch(matchItem.Value))
                                    {
                                        System.Text.RegularExpressions.Match matchImage = regImage.Match(matchItem.Value);
                                        string strRecid = matchImage.Value.Substring(4, matchImage.Value.Length - 8);
                                        recids += strRecid + ",";
                                        string strImagePat = "#F:view_gc_xctp.thumbattachment-O:I-W:Recid=" + strRecid + "#";
                                        strRow = strRow.Replace(matchItem.Value, strImagePat);
                                        contentarr[i] = strRow;
                                    }
                                }
                            }
                        }
                        savejdjlxq(fileid, contentarr);


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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

        private void savejdjlxq(string fileid, string[] contentarr)
        {
            List<string> allcontents = new List<string>();
            foreach (var item in contentarr)
            {
                allcontents.AddRange(item.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            if (allcontents.Count > 0)
            {
                List<string> lsql = new List<string>();
                foreach (var c in allcontents)
                {
                    string sql = "";
                    string s = c;
                    Regex regImage = new Regex(@"W:Recid=\w+#");
                    if (s.Replace(" ", "") == "")
                    {
                        continue;
                    }
                    else if (regImage.IsMatch(s))
                    {
                        MatchCollection matchCol = regImage.Matches(s);
                        if (matchCol.Count > 0)
                        {
                            foreach (System.Text.RegularExpressions.Match matchItem in matchCol)
                            {
                                string strRecid = matchItem.Value.Substring(8, matchItem.Value.Length - 9);
                                sql = string.Format("insert into JDBG_JDJLNR_XQ (fileid, content, type, lrsj) values ('{0}','{1}','{2}', getdate())", fileid, DataFormat.EncodeBase64(strRecid), "img");
                                lsql.Add(sql);
                            }
                        }

                    }
                    else
                    {
                        string[] carr = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in carr)
                        {
                            string ss = item.Trim();
                            if (ss != "")
                            {
                                sql = string.Format("insert into JDBG_JDJLNR_XQ (fileid, content, type, lrsj) values ('{0}','{1}','{2}', getdate())", fileid, DataFormat.EncodeBase64(ss), "txt");
                                lsql.Add(sql);
                            }
                        }

                    }
                }

                if (lsql.Count > 0)
                {
                    string csql = string.Format("delete from JDBG_JDJLNR_XQ where fileid='{0}'", fileid);
                    lsql.Insert(0, csql);
                    if (lsql.Count > 0)
                    {
                        CommonService.ExecTrans(lsql);
                    }
                }

            }
        }

        #endregion

        #region 新短信接口
        public void TestNewSmS()
        {
            bool ret = true;
            string msg = "";
            try
            {
                ret = SmsServiceWzzjz.SendMessageV2(GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), "15157547103", "1111", out msg);
                
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

                Response.Write(string.Format("{{\"code\":\"{0}\", \"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 测试账号
        public void SetZH()
        {
            string msg = "";
            bool code = true;
            try
            {
                string sql = "select * from TMP_Migrate_UserPwd";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    List<string> lsql = new List<string>();
                    foreach (var item in dt)
                    {
                        string recid = item["recid"].GetSafeString();
                        string oldpwd = item["oldpwd"].GetSafeString();
                        string newpwd = item["newpwd"].GetSafeString();
                        string aoldpwd = CryptFun.Decode(oldpwd);
                        string anewpwd = CryptFun.Decode(newpwd);
                        sql = string.Format("update  TMP_Migrate_UserPwd set aoldpwd='{0}',anewpwd='{1}' where recid={2} ", aoldpwd, anewpwd, recid);
                        lsql.Add(sql);
                    }
                    if (lsql.Count > 0)
                    {
                        CommonService.ExecTrans(lsql);
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

        #region 监督抽查联系单相关接口
        public void GetJCJGS()
        {
            string msg = "";
            bool code = true;
            IList<Dictionary<string, object>> qylist = new List<Dictionary<string, object>>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = int.MaxValue;
            try
            {
                
                string data = "";
                string sql = "select * from h_jcjg_jdcclxd_config where lx='JCJG'";
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string gcbh = Request["gcbh"].GetSafeString();
                    IDictionary<string, string> config = dt[0];
                    string url = config["url"].GetSafeString();
                    string param = config["fixedparam"].GetSafeString();
                    param = param.Replace("{{gcbh}}", gcbh);
                    string method = "POST";
                    if (method == "")
                    {
                        method = "GET";
                    }
                    if (url !="")
                    {
                        if (method.Equals("GET",StringComparison.OrdinalIgnoreCase))
                        {
                            string concatestr = url.IndexOf('?') == -1 ? "?" : "&";
                            if (param != "")
                            {
                                url = url + concatestr + param;
                            }
                            //SysLog4.WriteError(url);
                            data = MyHttp.SendDataByGET(url);
                            
                        }
                        else if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                        {
                            data = MyHttp.SendDataByPost(url, param);
                            //SysLog4.WriteError(data);
                        }
                        
                    }
                }

                if (data !="")
                {
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(data);
                    if (retdata != null )
                    {
                        bool ret = retdata["code"].GetSafeBool();
                        string retmsg = retdata["msg"].GetSafeString();
                        if (!ret)
                        {
                            code = false;
                            msg = retmsg;
                        }
                        else
                        {
                            ArrayList al = retdata["records"] as ArrayList;
                            if (al!=null && al.Count > 0)
                            {
                                foreach (var item in al)
                                {
                                    Dictionary<string, object> r = (Dictionary<string, object>)item;
                                    qylist.Add(r);
                                }
                            }
                            
                        }
                    }
                }
                else
                {
                    code = false;
                    msg = "接口调用错误！";
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
                
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"qylist\":{2}}}", code ? "0" : "1", msg, jss.Serialize(qylist)));
                Response.End();
            }
        }
        public void GetSyxms()
        {
            string msg = "";
            bool code = true;
            IList<Dictionary<string, object>> syxmlist = new List<Dictionary<string, object>>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = int.MaxValue;
            try
            {
                string qybh = Request["qybh"].GetSafeString();
                if (qybh !="")
                {
                    string data = "";
                    string sql = "select * from h_jcjg_jdcclxd_config where lx='SYXM'";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        IDictionary<string, string> config = dt[0];
                        string url = config["url"].GetSafeString();
                        string param = config["fixedparam"].GetSafeString();
                        param = param.Replace("{{qybh}}", qybh);
                        string method = "POST";
                        if (method == "")
                        {
                            method = "GET";
                        }
                        if (url != "")
                        {
                            if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                            {
                                string concatestr = url.IndexOf('?') == -1 ? "?" : "&";
                                if (param != "")
                                {
                                    url = url + concatestr + param;
                                }
                                //SysLog4.WriteError(url);
                                data = MyHttp.SendDataByGET(url);

                            }
                            else if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                            {
                                data = MyHttp.SendDataByPost(url, param);
                                //SysLog4.WriteError(url);
                                //SysLog4.WriteError(param);
                                //SysLog4.WriteError(data);
                            }

                        }
                    }

                    if (data != "")
                    {
                        Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(data);
                        if (retdata != null)
                        {
                            bool ret = retdata["code"].GetSafeBool();
                            string retmsg = retdata["msg"].GetSafeString();
                            if (!ret)
                            {
                                code = false;
                                msg = retmsg;
                            }
                            else
                            {
                                ArrayList al = retdata["records"] as ArrayList;
                                if (al != null && al.Count > 0)
                                {
                                    foreach (var item in al)
                                    {
                                        Dictionary<string, object> r = (Dictionary<string, object>)item;
                                        syxmlist.Add(r);
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        code = false;
                        msg = "接口调用错误！";
                    }
                }
                else
                {
                    code = false;
                    msg = "企业编号不能为空！";
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

                Response.ContentEncoding = System.Text.Encoding.UTF8;

                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"syxmlist\":{2}}}", code ? "0" : "1", msg, jss.Serialize(syxmlist)));
                Response.End();
            }
        }

        [Authorize]
        public ActionResult Jcjgxz()
        {
            ViewBag.gcbh = Request["gcbh"].GetSafeRequest();
            return View();
        }

        public ActionResult Syxmxz()
        {
            ViewBag.qybh = Request["qybh"].GetSafeRequest();
            ViewBag.gcbh = Request["gcbh"].GetSafeRequest();
            ViewBag.gcmc = Request["gcmc"].GetSafeRequest();
            ViewBag.workdata = Request["workdata"].GetSafeRequest();
            return View();
        }


        #endregion

        #region 工程报监新接口

        /// <summary>
        /// 提交工程
        /// </summary>
        [Authorize]
        public void SubmitGc()
        {
            bool code = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeRequest();
                string procstr = string.Format("FlowSubmitGc('{0}')", gcbh);
                code= CommonService.ExecProc(procstr, out msg);
                //IList<string> sqls = new List<string>();
                //sqls.Add("update i_m_gc set zt='LR' where gcbh='" + gcbh + "' and zt='YT'");

                //code = CommonService.ExecTrans(sqls);
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
        #endregion

        #region 下载word报表

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

        #region 温州市住建局数据推送
        public void TestPushGc()
        {
            bool ret = true;
            string msg = "";
            try
            {
                List<string> lsql = new List<string>();
                string sql = "insert into company_information (qybh, qymc) values ('00000','00000')";
                lsql.Add(sql);
                sql = "insert into company_information (qybh, qymc) values ('10000','10000')";
                lsql.Add(sql);
                ret = MySqlService.ExecTrans(lsql, out msg);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            Response.Write(JsonFormat.GetRetString(ret, msg));
        }

        public void RefreshJCBGSL()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh_yc"].GetSafeString();
                if (gcbh !="")
                {
                    string procstr = string.Format("PushJCBGSLToZjj('{0}')", gcbh);
                    ret = CommonService.ExecProc(procstr, out msg);
                    if (!ret)
                    {
                        SysLog4.WriteError("推送检测报告数量失败,工程编号：" + gcbh + "\r\n" + msg);
                    }
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteError(e.Message);
            }
            Response.Write(JsonFormat.GetRetString(ret, msg));
        }
        #endregion

        #region 同步监督抽查联系单确认状态
        public void SyncJdcclxdQrzt()
        {
            string msg = "";
            bool code = true;
            try
            {
                string workserial = Request["workserial"].GetSafeString();
                string timestring = Request["timestring"].GetSafeString();
                string sign = Request["sign"].GetSafeString();
                string signstr = String.Format("timestring={0}&secret={1}", timestring, "sync_jdcclxd");
                if (sign == MD5Util.StringToMD5Hash(signstr, true))
                {
                    if (workserial != "")
                    {
                        string sql = string.Format("update jdbg_jdccrwwtjl set isconfirmed=1 where workserial='{0}'", workserial);
                        CommonService.Execsql(sql);
                    }
                    else
                    {
                        code = false;
                        msg = "同步监督抽查联系单确认状态错误：流水号为空";
                    }

                }
                else
                {
                    code = false;
                    msg = "同步监督抽查联系单确认状态错误：校验失败";
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
                jss.MaxJsonLength = Int32.MaxValue;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", code ? "0" : "1", msg));
                Response.End();
            }
        }
        #endregion

        #region 检测报告处理
        /// <summary>
        /// 修改检测报告过滤参数
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ForgeJcjgJcbgFilterParams(List<KeyValuePair<string, string>> paramlist)
        {
            var q = paramlist.Where(x => x.Key.Equals("forgecljg", StringComparison.OrdinalIgnoreCase));
            if (q.Count() > 0)
            {
                var p = q.First();
                string v = p.Value.GetSafeString();
                if (v!="")
                {
                    // 未处理
                    if (v=="未处理")
                    {
                        string dealedbglist = "";
                        var gcbh = "";
                        q = paramlist.Where(x => x.Key.Equals("forgegcbh", StringComparison.OrdinalIgnoreCase));
                        if (q.Count() > 0)
                        {
                            gcbh = q.First().Value.GetSafeString();
                        }
                        string sql = "select bgwyh from i_s_gc_jcbg_cljg where cljg=1 ";
                        if (gcbh !="")
                        {
                            sql += string.Format(" and gcbh='{0}'", gcbh);
                        }
                        IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                        if (dt.Count > 0)
                        {
                            dealedbglist = string.Join(",", dt.Select(x => x["bgwyh"].GetSafeString()).ToList());
                        }
                        if (dealedbglist!="")
                        {
                            paramlist.Add(new KeyValuePair<string, string>("BgWyh", dealedbglist));
                        }

                    }
                }

            }
            return paramlist;
        }


        /// <summary>
        /// 修改检测报告返回数据
        /// </summary>
        /// <param name="paramlist"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> ForgeJcjgJcbgData(List<Dictionary<string, object>> data)
        {
            string[] bgwyhList = data.Select(x => x["bgwyh"].GetSafeString()).ToArray();
            if (bgwyhList.Length > 0)
            {
                
                string sql = string.Format("select bgwyh from i_s_gc_jcbg_cljg where cljg=1 and bgwyh in ({0})", DataFormat.FormatSQLInStr(bgwyhList));
                IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                string[] dealedbglist = dt.Select(x => x["bgwyh"].GetSafeString()).ToArray();
                foreach (var item in data)
                {
                    if (dealedbglist.Contains(item["bgwyh"].GetSafeString()))
                    {
                        item.Add("forgecljg", "已处理");
                    }
                    else
                    {
                        item.Add("forgecljg", "未处理");
                    }
                }
            }

            
            return data;
        }

        public ActionResult cljgxz()
        {
            return View();
        }

        public void setJcbgCljg()
        {
            bool ret = true;
            string msg = "";
            try
            {
                string gcbh = Request["gcbh"].GetSafeString();
                string bgwyh = Request["bgwyh"].GetSafeString();
                string cljg = Request["cljg"].GetSafeString();
                if (gcbh !="" && bgwyh!="" && cljg!="")
                {
                    string procstr = "SetJcbgCljg('{0}','{1}','{2}')";
                    procstr = string.Format(procstr, gcbh, bgwyh, cljg);
                    ret = CommonService.ExecProc(procstr, out msg);
                }
                else
                {
                    ret = false;
                    msg = "参数错误";
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

        public void SaveBhgbg()
        {
            bool ret = true;
            string msg = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = int.MaxValue;
            try
            {
                string gcbh = Request["gcbh_yc"].GetSafeString();
                string bgdata = Request["bgdata"].GetSafeString();
                //SysLog4.WriteError("gcbh_yc:" + gcbh);
                // SysLog4.WriteError("bgdata:" + bgdata);
                if (gcbh != "" && bgdata != "")
                {
                    string data = DataFormat.DecodeBase64(bgdata);
                    if (data !="")
                    {
                        Dictionary<string,object> realdata = jss.Deserialize<Dictionary<string, object>>(data);
                        if (realdata !=null && realdata.Count > 0)
                        {
                            string bgwyh = realdata["报告唯一号"].GetSafeString();
                            string procstr = string.Format("SaveBhgbg('{0}','{1}','{2}')", gcbh, bgwyh, bgdata);
                            ret = CommonService.ExecProc(procstr, out msg);
                        }
                    }
                }
                else
                {
                    ret = false;
                    msg = "参数错误";
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
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\"}}", ret ? "0" : "1", msg));
                Response.End();
            }
        }

        public void GetGcinfo()
        {
            bool ret = true;
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
                    ret = false;
                    msg = "参数错误";
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
                Response.Write(string.Format("{{\"code\":\"{0}\",\"msg\":\"{1}\",\"data\":{2}}}", ret ? "0" : "1", msg,jss.Serialize(data)));
                Response.End();
            }
        }
        #endregion




    }
}