using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;
using System.IO;
using ReportPrint.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BD.Jcbg.Web.Func;
using System.Collections;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 往温州住建局前置机数据库推送数据
    /// </summary>
    public class JobPushGcToZJJWzzjz:ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
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
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobPushGcToZJJWzzjz");
            while (true)
            {
                try
                {
                    bool ret = true;
                    string msg = "";
                    string top = Configs.GetConfigItem("pushzjjtopnum").GetSafeString("10");
                    string sql = $"select top {top} * from h_wzzjz_info_push where ( issync=0 and synctime is null ) order by recid ";
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    IList<IDictionary<string, object>> tableList = CommonService.GetDataTable2("select * from h_wzzjz_info_push_table");
                    IList<IDictionary<string, object>> fieldList = CommonService.GetDataTable2("select * from h_wzzjz_info_push_field");
                    if (dt.Count > 0)
                    {
                        foreach (var item in dt)
                        {
                            string recid = item["recid"].GetSafeString();
                            string type = item["type"].GetSafeString();
                            string PrimaryKey = item["primarykey"].GetSafeString();
                            string PrimaryKeyValue = item["primarykeyvalue"].GetSafeString();
                            if (PrimaryKeyValue!="")
                            {
                                ret = SyncInfo(type, PrimaryKey, PrimaryKeyValue,tableList ,fieldList, out msg);
                                string issync = ret ? "1" : "0";
                                sql = $"update h_wzzjz_info_push set issync={issync}, synctime=getdate() where recid={recid}";
                                CommonService.Execsql(sql);
                                if (!ret)
                                {
                                    SysLog4.WriteError(msg);
                                }
                                
                            }
                            
                        }
                    }

                }
                catch (Exception ex)
                {
                    SysLog4.WriteError(ex.Message);

                }


                Thread.Sleep(Interval);
            }

        }

        private bool SyncInfo(string type, string PrimaryKey, string PrimaryKeyValue, IList<IDictionary<string, object>> tableList, IList<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                List<IDictionary<string, object>> realTableList = tableList.Where(x => x["type"].GetSafeString().Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
                List<IDictionary<string, object>> realFieldList = fieldList.Where(x => x["type"].GetSafeString().Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
                // 推送工程
                if (type == "GC")
                {
                    ret = SyncGc(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "QY")
                {
                    ret = SyncQy(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "RY")
                {
                    ret = SyncRy(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "JCBGSL")
                {
                    ret = SyncJCBGSL(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "JDFA")
                {
                    ret = SyncJDFA(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "JDJD")
                {
                    ret = SyncJDJD(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "YSAPJL")
                {
                    ret = SyncYSAPJL(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "JDJL")
                {
                    ret = SyncJDJL(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "ZGD")
                {
                    ret = SyncZGD(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
                else if (type == "JDBG")
                {
                    ret = SyncJDBG(PrimaryKey, PrimaryKeyValue, realTableList, realFieldList, out msg);
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        private bool SyncGc(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList,out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                
                #region 工程
                // 工程信息
                string sql = $"select top 1 * from i_m_gc where gcbh='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_M_GC", dt);
                // 分工程
                sql = $"select * from i_s_gc_fgc where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_FGC", dt);
                #endregion

                #region 单位
                // 施工单位
                sql = $"select * from i_s_gc_sgdw where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_SGDW", dt);
                // 监理单位
                sql = $"select * from i_s_gc_jldw where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_JLDW", dt);
                // 建设单位
                sql = $"select * from i_s_gc_jsdw where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_JSDW", dt);
                // 设计单位
                sql = $"select * from i_s_gc_sjdw where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_SJDW", dt);
                // 勘察单位
                sql = $"select * from i_s_gc_kcdw where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_KCDW", dt);
                // 图审单位
                sql = $"select * from i_s_gc_tsdw where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_TSDW", dt);
                #endregion

                #region 人员
                // 施工人员
                sql = $"select * from i_s_gc_sgry where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_SGRY", dt);
                // 监理人员
                sql = $"select * from i_s_gc_jlry where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_JLRY", dt);
                // 建设人员
                sql = $"select * from i_s_gc_jsry where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_JSRY", dt);
                // 设计人员
                sql = $"select * from i_s_gc_sjry where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_SJRY", dt);
                // 勘察人员
                sql = $"select * from i_s_gc_kcry where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_KCRY", dt);
                // 图审人员
                sql = $"select * from i_s_gc_tsry where gcbh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_GC_TSRY", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("gcbh")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }
                    
                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncQy(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region 企业
                // 企业信息
                string sql = $"select top 1 * from i_m_qy where qybh='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_M_QY", dt);
                // 企业资质
                sql = $"select * from i_s_qy_qyzz where qybh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_QY_QYZZ", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("qybh")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncRy(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region 人员
                // 人员信息
                string sql = $"select top 1 * from i_m_ry where rybh='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_M_RY", dt);
                // 人员资质
                sql = $"select * from i_s_ry_ryzz where rybh='{PrimaryKeyValue}'";
                dt = CommonService.GetDataTable2(sql);
                alldata.Add("I_S_RY_RYZZ", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("rybh")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncJCBGSL(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region 工程报告数量
                // 工程信息
                string sql = $"select top 1 * from i_m_gc where gcbh='{PrimaryKeyValue}'";
                SysLog4.WriteError(sql);
                var ddt = CommonService.GetDataTable2(sql);
                if (ddt.Count > 0)
                {
                    string gcbh = ddt[0]["gcbh"].GetSafeString();
                    string zjdjh = ddt[0]["zjdjh"].GetSafeString();
                    string lszjdjh = ddt[0]["lszjdjh"].GetSafeString();
                    if (lszjdjh=="")
                    {
                        lszjdjh = " ";
                    }
                    int bhgbg = 0;
                    int sybg = 0;
                    bhgbg = JcjgBgService.GetBgsl(zjdjh, "2", "WZJCJG", lszjdjh);
                    sybg = JcjgBgService.GetBgsl(zjdjh, "", "WZJCJG", lszjdjh);
                    IList<IDictionary<string, object>> dt = new List<IDictionary<string, object>>();
                    Dictionary<string, object> data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"GCBH", gcbh },
                        {"BHGSL", bhgbg.GetSafeString()},
                        {"HGSL", (sybg-bhgbg).GetSafeString() }
                    };
                    dt.Add(data);
                    alldata.Add("JCBGSL", dt);
                }
                
               
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("gcbh")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncJDFA(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region 监督方案
                string sql = $"select top 1 * from VIEW_JDBG_JDJL where lx='JDFA' AND workserial='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("JDFA", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("workserial")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncJDJD(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region 监督交底
                string sql = $"select top 1 * from VIEW_JDBG_JDJL where lx='JDJD' AND workserial='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("JDJD", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("workserial")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncYSAPJL(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region 验收安排记录
                string sql = $"select top 1 * from View_JDBG_YSAPJL where APID='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("YSAPJL", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("apid")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncJDJL(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region
                string sql = $"select top 1 * from VIEW_JDBG_JDJL where LX='JDJL' and WORKSERIAL='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("JDJL", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("workserial")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncZGD(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region
                string sql = $"select top 1 * from VIEW_JDBG_JDJL where LX='ZGD' and WORKSERIAL='{PrimaryKeyValue}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("ZGD", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where {quoteName("workserial")}='{PrimaryKeyValue}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private bool SyncJDBG(string PrimaryKey, string PrimaryKeyValue, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                Dictionary<string, object> alldata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                #region
                string[] keyList = PrimaryKeyValue.Split(new char[] { '|' });
                string sql = $"select top 1 * from VIEW_JDBG_JDJL where LX='JDBG' and recid={keyList[0]} and WORKSERIAL='{keyList[1]}'";
                var dt = CommonService.GetDataTable2(sql);
                alldata.Add("JDBG", dt);
                #endregion

                #region 生成sql
                if (alldata.Count > 0)
                {
                    List<string> lsql = new List<string>();

                    // 清除原有的数据
                    foreach (var t in tableList)
                    {
                        string desttable = t["desttable"].GetSafeString();
                        sql = $"delete from {quoteName(desttable)} where recid={keyList[0]} and workserial='{keyList[1]}'";
                        SysLog4.WriteError(sql);
                        lsql.Add(sql);
                    }
                    lsql.AddRange(GetSqls(alldata, tableList, fieldList));

                    if (lsql.Count > 0)
                    {
                        ret = MySqlService.ExecTrans(lsql, out msg);
                    }

                }
                #endregion



            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message + "\r\n" + e.StackTrace;
            }
            return ret;
        }

        private string quoteName(string name)
        {
            return "`" + name + "`";
        }

        /// <summary>
        /// 转换对象的实际值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldtype">字段类型</param>
        /// <param name="v"></param>
        /// <returns>true 返回值为 null， false 返回值为字符串</returns>
        private bool GetStringValue(object obj, string fieldtype, out string v)
        {
            bool IsNull = false;
            v = "";
            if (obj == null || Convert.IsDBNull(obj))
            {
                return true;
            }
            // 忽略所有的二进制字段，统一返回null
            if (fieldtype.Equals("VARBINARY",StringComparison.OrdinalIgnoreCase))
            {
                IsNull = true;
            }
            else if(fieldtype.Equals("BIT", StringComparison.OrdinalIgnoreCase))
            {
                v = obj.GetSafeBool() ? "1" : "0";
            }
            else if (fieldtype.Equals("DATETIME", StringComparison.OrdinalIgnoreCase))
            {
                v = obj.GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (fieldtype.Equals("DATE", StringComparison.OrdinalIgnoreCase))
            {
                v = obj.GetSafeDate().ToString("yyyy-MM-dd");
            }
            else if (fieldtype.Equals("INT", StringComparison.OrdinalIgnoreCase))
            {
                v = obj.GetSafeInt(0).GetSafeString();
            }
            else if (fieldtype.Equals("FLOAT", StringComparison.OrdinalIgnoreCase))
            {
                if (obj.GetSafeString().Trim() == "")
                {
                    v = "0";
                }
                else
                {
                    v = Convert.ToString(obj);
                }
            }
            else
            {
                v = Convert.ToString(obj);
            }
            
            return IsNull;
        }

        private List<string> GetSqls(Dictionary<string, object> alldata, List<IDictionary<string, object>> tableList, List<IDictionary<string, object>> fieldList)
        {
            List<string> lsql = new List<string>();
            string sql = "";
            // 生成新的数据
            foreach (var data in alldata)
            {
                string srcTable = data.Key;
                IList<IDictionary<string, object>> records = (IList<IDictionary<string, object>>)data.Value;
                // 获取目标表名
                string destTable = "";
                var q = tableList.Where(x => x["srctable"].GetSafeString().Equals(srcTable, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (q != null)
                {
                    destTable = q["desttable"].GetSafeString();
                }
                SysLog4.WriteError("测试源表和目标表1");
                // 源表与目标表都不为空
                if (srcTable != "" && destTable != "")
                {
                    // 获取源表字段和目标表字段
                    var fields = fieldList.Where(
                        x =>
                        x["srctable"].GetSafeString().Equals(srcTable, StringComparison.OrdinalIgnoreCase)
                        &&
                        x["desttable"].GetSafeString().Equals(destTable, StringComparison.OrdinalIgnoreCase)
                    ).ToList();

                    SysLog4.WriteError("测试源表和目标表2");
                    if (records != null && records.Count > 0 && fields.Count > 0)
                    {
                        foreach (var rec in records)
                        {
                            string fieldstr = "";
                            string fieldVstr = "";
                            string v = "";
                            foreach (var f in fields)
                            {
                                
                                if (fieldstr !="")
                                {
                                    fieldstr += ",";
                                }
                                fieldstr += quoteName(f["destfield"].GetSafeString());

                                if (fieldVstr !="")
                                {
                                    fieldVstr += ",";
                                }
                                
                                if (GetStringValue(rec[f["srcfield"].GetSafeString().ToLower()], f["destfieldtype"].GetSafeString(), out v))
                                {
                                    fieldVstr += "null";
                                }
                                else
                                {
                                    fieldVstr += "'" + v +"'";
                                }
                                
                            }
                            sql = "insert into " + quoteName(destTable) + " " +
                                   "(" + fieldstr + ") values (" + fieldVstr + ")";
                            SysLog4.WriteError(sql);
                            lsql.Add(sql);
                        }
                    }

                }


            }
            return lsql;
        }

    }
}