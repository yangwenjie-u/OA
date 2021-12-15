using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web;
using BD.IDataInputDao;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using BD.Jcbg.Service;
using ReportPrint.Common;
using BD.Jcbg.Service.Jc;
using System.Web.Script.Serialization;
using BD.Jcbg.DataModal.VirutalEntity.Jc;
using NHibernate;
using System.Collections;

namespace BD.Jcbg.Bll
{
    public class JcService : IJcService
    {
        #region 数据库对象

        public IPrmDywjDao PrmDywjDao { get; set; }
        public IPrmWjDao PrmWjDao { get; set; }
        public ICommonDao CommonDao { get; set; }
        public IWebDataInputDao WebDataInputDao { get; set; }
        public IDataFileDao DataFileDao { get; set; }
        public ISysLogPicDao SysLogPicDao { get; set; }

        #endregion

        #region 服务
        /// <summary>
        /// 获取委托单信息
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool GetWtd(string recid, out IDictionary<string, string> mtable,
            out IList<IDictionary<string, string>> stable, out string msg)
        {
            bool ret = false;
            //英文
            if (GlobalVariableConfig.GLOBAL_INTERFACE_CNEN == InterfaceEnum.EN.ToString())
            {
                ret = GetWtdEN(recid, out mtable, out stable, out msg);
            }
            else
            {
                ret = GetWtdCN(recid, out mtable, out stable, out msg);
            }
            return ret;
        }

        #region 单委托单下载区分中文与英文
        #region 中文
        private bool GetWtdCN(string recid, out IDictionary<string, string> mtable,
            out IList<IDictionary<string, string>> stable, out string msg)
        {
            bool ret = false;
            msg = "";
            mtable = new Dictionary<string, string>();
            stable = new List<IDictionary<string, string>>();
            try
            {
                // 排除字段
                IList<IDictionary<string, string>> execludes =
                    CommonDao.GetDataTable("select tablename,fieldname from SysDownSetting");
                // 获取必有主表
                IList<IDictionary<string, string>> byzb =
                    CommonDao.GetDataTable("select * from m_by where recid='" + recid + "'");
                if (byzb.Count == 0)
                {
                    msg = "编号无效，获取记录失败";
                    return ret;
                }

                string syxmbh = byzb[0]["syxmbh"].ToLower();
                msg = syxmbh;
                if (syxmbh == "")
                {
                    msg = "获取项目代码失败";
                    return ret;
                }

                string zt = byzb[0]["zt"];
                WtsStatus objzt = new WtsStatus(zt);
                if (!objzt.HasWtdSubmit)
                {
                    msg = "委托单未提交，无法获取信息";
                    return ret;
                }

                if (objzt.HasWtdDown)
                {
                    msg = "委托单已送样，无法获取信息";
                    return ret;
                }

                //添加委托单确认判断
                var qrxz = byzb[0]["qrxz"].GetSafeBool();

                if (!qrxz)
                {
                    msg = "委托单没有确认下载到检测系统";
                    return ret;
                }

                // 获取zdzd
                IList<IDictionary<string, string>> zdzdsby =
                    CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from XTZD_BY");
                IList<IDictionary<string, string>> zdzdsdw =
                    CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from dwzd_" + syxmbh);
                IList<IDictionary<string, string>> zdzdsxm =
                    CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from zdzd_" + syxmbh);

                // 获取必有从表
                IList<IDictionary<string, string>> bycb =
                    CommonDao.GetDataTable("select * from s_by where byzbrecid='" + recid + "' order by len(zh),zh");

                // 获取单位主表
                string dwzbmc = "m_d_" + syxmbh;
                IList<IDictionary<string, string>> dwzb =
                    CommonDao.GetDataTable("select * from " + dwzbmc + " where recid='" + recid + "'");

                // 获取单位从表
                string dwcbmc = "s_d_" + syxmbh;
                IList<IDictionary<string, string>> dwcb =
                    CommonDao.GetDataTable("select * from " + dwcbmc + " where byzbrecid='" + recid + "'");

                // 获取项目主表
                string xmzbmc = "m_" + syxmbh;
                IList<IDictionary<string, string>> xmzb =
                    CommonDao.GetDataTable("select * from " + xmzbmc + " where recid='" + recid + "'");

                // 获取项目从表
                string xmcbmc = "s_" + syxmbh;
                IList<IDictionary<string, string>> xmcb =
                    CommonDao.GetDataTable("select * from " + xmcbmc + " where byzbrecid='" + recid + "'");

                // 加入返回值
                // 必有主表
                foreach (string key in byzb[0].Keys)
                {
                    string strValue = "";
                    if (IsExeclude(execludes, "m_by", key))
                        continue;
                    if (!mtable.TryGetValue(key, out strValue))
                    {
                        var q = from e in zdzdsby
                                where e["sjbmc"].Equals("m_by", StringComparison.OrdinalIgnoreCase) &&
                                      e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                select e;
                        if (q.Count() == 0)
                            continue;
                        var zdzd = q.First();
                        AddSafeToDictionary(mtable, zdzd["sy"], zdzd["zdlx"], byzb[0][key]);
                    }
                }

                // 单位主表
                foreach (string key in dwzb[0].Keys)
                {
                    if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (IsExeclude(execludes, dwzbmc, key))
                        continue;
                    string strValue = "";
                    if (!mtable.TryGetValue(key, out strValue))
                    {
                        var q = from e in zdzdsdw
                                where e["sjbmc"].Equals(dwzbmc, StringComparison.OrdinalIgnoreCase) &&
                                      e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                select e;
                        if (q.Count() == 0)
                            continue;
                        var zdzd = q.First();
                        AddSafeToDictionary(mtable, zdzd["sy"], zdzd["zdlx"], dwzb[0][key]);
                    }
                }

                // 项目主表
                foreach (string key in xmzb[0].Keys)
                {
                    if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (IsExeclude(execludes, xmzbmc, key))
                        continue;
                    string strValue = "";
                    if (!mtable.TryGetValue(key, out strValue))
                    {
                        var q = from e in zdzdsxm
                                where e["sjbmc"].Equals(xmzbmc, StringComparison.OrdinalIgnoreCase) &&
                                      e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                select e;
                        if (q.Count() == 0)
                            continue;
                        var zdzd = q.First();
                        AddSafeToDictionary(mtable, zdzd["sy"], zdzd["zdlx"], xmzb[0][key]);
                    }
                }

                // 从表
                foreach (IDictionary<string, string> row in bycb)
                {
                    IDictionary<string, string> srow = new Dictionary<string, string>();
                    stable.Add(srow);
                    // 必有从表
                    string srecid = "";
                    foreach (string key in row.Keys)
                    {
                        if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                            srecid = row[key];
                        if (IsExeclude(execludes, "s_by", key))
                            continue;
                        string strValue = "";
                        if (!srow.TryGetValue(key, out strValue))
                        {
                            var q = from e in zdzdsby
                                    where e["sjbmc"].Equals("s_by", StringComparison.OrdinalIgnoreCase) &&
                                          e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                    select e;
                            if (q.Count() == 0)
                                continue;
                            var zdzd = q.First();
                            AddSafeToDictionary(srow, zdzd["sy"], zdzd["zdlx"], row[key]);
                        }
                    }

                    // 单位从表
                    foreach (IDictionary<string, string> drow in dwcb)
                    {
                        if (drow["recid"].Equals(srecid))
                        {
                            foreach (string key in drow.Keys)
                            {
                                if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                    key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                    continue;
                                if (IsExeclude(execludes, dwcbmc, key))
                                    continue;
                                string strValue = "";
                                if (!srow.TryGetValue(key, out strValue))
                                {
                                    var q = from e in zdzdsdw
                                            where e["sjbmc"].Equals(dwcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                            select e;
                                    if (q.Count() == 0)
                                        continue;
                                    var zdzd = q.First();
                                    AddSafeToDictionary(srow, zdzd["sy"], zdzd["zdlx"], drow[key]);
                                }
                            }
                        }
                    }

                    //项目从表
                    foreach (IDictionary<string, string> xrow in xmcb)
                    {
                        if (xrow["recid"].Equals(srecid))
                        {
                            foreach (string key in xrow.Keys)
                            {
                                if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                    key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                    continue;
                                if (IsExeclude(execludes, xmcbmc, key))
                                    continue;
                                string strValue = "";
                                if (!srow.TryGetValue(key, out strValue))
                                {
                                    var q = from e in zdzdsxm
                                            where e["sjbmc"].Equals(xmcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                            select e;
                                    if (q.Count() == 0)
                                        continue;
                                    var zdzd = q.First();
                                    AddSafeToDictionary(srow, zdzd["sy"], zdzd["zdlx"], xrow[key]);
                                }
                            }
                        }
                    }


                }

                CommonDao.ExecCommand("update m_by set sfxf=1 where recid='" + recid + "'", CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }
        #endregion

        #region 英文
        private bool GetWtdEN(string recid, out IDictionary<string, string> mtable,
            out IList<IDictionary<string, string>> stable, out string msg)
        {
            bool ret = false;
            msg = "";
            mtable = new Dictionary<string, string>();
            stable = new List<IDictionary<string, string>>();
            try
            {
                // 排除字段
                IList<IDictionary<string, string>> execludes =
                    CommonDao.GetDataTable("select tablename,fieldname from SysDownSetting");
                // 获取必有主表
                IList<IDictionary<string, string>> byzb =
                    CommonDao.GetDataTable("select * from m_by where recid='" + recid + "'");
                if (byzb.Count == 0)
                {
                    msg = "编号无效，获取记录失败";
                    return ret;
                }

                string syxmbh = byzb[0]["syxmbh"].ToLower();
                msg = syxmbh;
                if (syxmbh == "")
                {
                    msg = "获取项目代码失败";
                    return ret;
                }

                string zt = byzb[0]["zt"];
                WtsStatus objzt = new WtsStatus(zt);
                if (!objzt.HasWtdSubmit)
                {
                    msg = "委托单未提交，无法获取信息";
                    return ret;
                }

                if (objzt.HasWtdDown)
                {
                    msg = "委托单已送样，无法获取信息";
                    return ret;
                }

                //添加委托单确认判断
                var qrxz = byzb[0]["qrxz"].GetSafeBool();

                if (!qrxz)
                {
                    msg = "委托单没有确认下载到检测系统";
                    return ret;
                }

                // 获取zdzd
                IList<IDictionary<string, string>> zdzdsby =
                    CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from XTZD_BY");
                IList<IDictionary<string, string>> zdzdsdw =
                    CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from dwzd_" + syxmbh);
                IList<IDictionary<string, string>> zdzdsxm =
                    CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from zdzd_" + syxmbh);

                // 获取必有从表
                IList<IDictionary<string, string>> bycb =
                    CommonDao.GetDataTable("select * from s_by where byzbrecid='" + recid + "' order by len(zh),zh");

                // 获取单位主表
                string dwzbmc = "m_d_" + syxmbh;
                IList<IDictionary<string, string>> dwzb =
                    CommonDao.GetDataTable("select * from " + dwzbmc + " where recid='" + recid + "'");

                // 获取单位从表
                string dwcbmc = "s_d_" + syxmbh;
                IList<IDictionary<string, string>> dwcb =
                    CommonDao.GetDataTable("select * from " + dwcbmc + " where byzbrecid='" + recid + "'");

                // 获取项目主表
                string xmzbmc = "m_" + syxmbh;
                IList<IDictionary<string, string>> xmzb =
                    CommonDao.GetDataTable("select * from " + xmzbmc + " where recid='" + recid + "'");

                // 获取项目从表
                string xmcbmc = "s_" + syxmbh;
                IList<IDictionary<string, string>> xmcb =
                    CommonDao.GetDataTable("select * from " + xmcbmc + " where byzbrecid='" + recid + "'");

                // 加入返回值
                // 必有主表
                foreach (string key in byzb[0].Keys)
                {
                    string strValue = "";
                    if (IsExeclude(execludes, "m_by", key))
                        continue;
                    if (!mtable.TryGetValue(key, out strValue))
                    {
                        var q = from e in zdzdsby
                                where e["sjbmc"].Equals("m_by", StringComparison.OrdinalIgnoreCase) &&
                                      e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                select e;
                        if (q.Count() == 0)
                            continue;
                        var zdzd = q.First();
                        //释义改成字段名
                        AddSafeToDictionary(mtable, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], byzb[0][key]);
                    }
                }

                // 单位主表
                foreach (string key in dwzb[0].Keys)
                {
                    if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (IsExeclude(execludes, dwzbmc, key))
                        continue;
                    string strValue = "";
                    if (!mtable.TryGetValue(key, out strValue))
                    {
                        var q = from e in zdzdsdw
                                where e["sjbmc"].Equals(dwzbmc, StringComparison.OrdinalIgnoreCase) &&
                                      e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                select e;
                        if (q.Count() == 0)
                            continue;
                        var zdzd = q.First();
                        //释义改成字段名
                        AddSafeToDictionary(mtable, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], dwzb[0][key]);
                    }
                }

                // 项目主表
                foreach (string key in xmzb[0].Keys)
                {
                    if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (IsExeclude(execludes, xmzbmc, key))
                        continue;
                    string strValue = "";
                    if (!mtable.TryGetValue(key, out strValue))
                    {
                        var q = from e in zdzdsxm
                                where e["sjbmc"].Equals(xmzbmc, StringComparison.OrdinalIgnoreCase) &&
                                      e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                select e;
                        if (q.Count() == 0)
                            continue;
                        var zdzd = q.First();
                        //释义改成字段名
                        AddSafeToDictionary(mtable, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], xmzb[0][key]);
                    }
                }

                // 从表
                foreach (IDictionary<string, string> row in bycb)
                {
                    IDictionary<string, string> srow = new Dictionary<string, string>();
                    stable.Add(srow);
                    // 必有从表
                    string srecid = "";
                    foreach (string key in row.Keys)
                    {
                        if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                            srecid = row[key];
                        if (IsExeclude(execludes, "s_by", key))
                            continue;
                        string strValue = "";
                        if (!srow.TryGetValue(key, out strValue))
                        {
                            var q = from e in zdzdsby
                                    where e["sjbmc"].Equals("s_by", StringComparison.OrdinalIgnoreCase) &&
                                          e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                    select e;
                            if (q.Count() == 0)
                                continue;
                            var zdzd = q.First();
                            //释义改成字段名
                            AddSafeToDictionary(srow, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], row[key]);
                        }
                    }

                    // 单位从表
                    foreach (IDictionary<string, string> drow in dwcb)
                    {
                        if (drow["recid"].Equals(srecid))
                        {
                            foreach (string key in drow.Keys)
                            {
                                if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                    key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                    continue;
                                if (IsExeclude(execludes, dwcbmc, key))
                                    continue;
                                string strValue = "";
                                if (!srow.TryGetValue(key, out strValue))
                                {
                                    var q = from e in zdzdsdw
                                            where e["sjbmc"].Equals(dwcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                            select e;
                                    if (q.Count() == 0)
                                        continue;
                                    var zdzd = q.First();
                                    //释义改成字段名
                                    AddSafeToDictionary(srow, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], drow[key]);
                                }
                            }
                        }
                    }

                    //项目从表
                    foreach (IDictionary<string, string> xrow in xmcb)
                    {
                        if (xrow["recid"].Equals(srecid))
                        {
                            foreach (string key in xrow.Keys)
                            {
                                if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                    key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                    continue;
                                if (IsExeclude(execludes, xmcbmc, key))
                                    continue;
                                string strValue = "";
                                if (!srow.TryGetValue(key, out strValue))
                                {
                                    var q = from e in zdzdsxm
                                            where e["sjbmc"].Equals(xmcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                            select e;
                                    if (q.Count() == 0)
                                        continue;
                                    var zdzd = q.First();
                                    //释义改成字段名
                                    AddSafeToDictionary(srow, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], xrow[key]);
                                }
                            }
                        }
                    }
                }
                CommonDao.ExecCommand("update m_by set sfxf=1 where recid='" + recid + "'", CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        #endregion
        #endregion


        [Transaction(ReadOnly = false)]
        public bool GetWtds(string depcode, VTransDownGetWtd[] where, out IList<IDictionary<string, object>> records,
            out string msg)
        {
            bool ret = false;
            msg = "";
            //英文
            if (GlobalVariableConfig.GLOBAL_INTERFACE_CNEN == InterfaceEnum.EN.ToString())
            {
                ret = GetWtdsEN(depcode, where, out records, out msg);
            }
            else
            {
                ret = GetWtdsCN(depcode, where, out records, out msg);
            }
            return ret;
        }

        #region 多份委托单下载
        #region 中文
        public bool GetWtdsCN(string depcode, VTransDownGetWtd[] where, out IList<IDictionary<string, object>> records,
            out string msg)
        {
            bool ret = false;
            msg = "";
            records = new List<IDictionary<string, object>>();
            try
            {
                if (where != null && where.Length > 0)
                {

                    // 获取委托单
                    StringBuilder sbWhere = new StringBuilder();
                    foreach (VTransDownGetWtd item in where)
                    {
                        string gcbh = item.gcbh.GetSafeRequest();
                        string lrr = item.lrr.GetSafeRequest();

                        if (sbWhere.Length > 0)
                            sbWhere.Append(" or ");
                        sbWhere.Append("(");
                        string str = "";
                        if (gcbh != "")
                        {
                            str += "gcbh='" + gcbh + "'";
                        }

                        if (lrr != "")
                        {
                            if (str.Length > 0)
                                str += " and ";
                            str += "wtslrrzh='" + lrr + "'";
                        }

                        if (str == "")
                            str = "1=1";
                        sbWhere.Append(str);

                        sbWhere.Append(")");
                    }

                    // 排除字段
                    IList<IDictionary<string, string>> execludes =
                        CommonDao.GetDataTable("select tablename,fieldname from SysDownSetting");
                    // 获取必有主表
                    if (depcode != "")
                        depcode = " and ytdwbh='" + depcode + "'";
                    IList<IDictionary<string, string>> byzb = CommonDao.GetDataTable(
                        "select * from m_by where dbo.IsWtdCanGet(zt)=1 and qrxz = 1 and (" + sbWhere.ToString() + ")" +
                        depcode);

                    // 获取主表recid 和 委托单编号
                    IDictionary<string, string> tableids = new Dictionary<string, string>();
                    string allrecids = "";
                    foreach (IDictionary<string, string> row in byzb)
                    {
                        string syxmbh = row["syxmbh"].ToLower();
                        if (syxmbh == "")
                            continue;
                        string recid = row["recid"].GetSafeString();
                        allrecids += recid + ",";
                        string ids = "";
                        if (tableids.TryGetValue(syxmbh, out ids))
                            tableids[syxmbh] = ids + "," + recid;
                        else
                            tableids.Add(syxmbh, recid);
                    }

                    // 获取所有必有从表
                    IList<IDictionary<string, string>> allbycbs = new List<IDictionary<string, string>>();
                    if (allrecids != "")
                        allbycbs = CommonDao.GetDataTable("select * from s_by where byzbrecid in (" +
                                                          allrecids.FormatSQLInStr() +
                                                          ") order by byzbrecid,len(zh),zh");
                    // 获取所有从表和zdzd表
                    IList<IDictionary<string, string>> zdzdsby =
                        CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from XTZD_BY");
                    IDictionary<string, IList<IDictionary<string, string>>> allOtherZdzds =
                        new Dictionary<string, IList<IDictionary<string, string>>>();
                    IDictionary<string, IList<IDictionary<string, string>>> allOtherTables =
                        new Dictionary<string, IList<IDictionary<string, string>>>();
                    foreach (string syxmbh in tableids.Keys)
                    {
                        // 获取zdzd
                        IList<IDictionary<string, string>> zdzdsdw =
                            CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from dwzd_" + syxmbh);
                        IList<IDictionary<string, string>> zdzdsxm =
                            CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from zdzd_" + syxmbh);
                        allOtherZdzds.Add(syxmbh + "_dw", zdzdsdw);
                        allOtherZdzds.Add(syxmbh + "_xm", zdzdsxm);
                        // 获取必有从表
                        string recids = "";
                        if (!tableids.TryGetValue(syxmbh, out recids))
                            continue;
                        recids = recids.FormatSQLInStr();
                        // 获取单位主表
                        string dwzbmc = "m_d_" + syxmbh;
                        IList<IDictionary<string, string>> dwzb =
                            CommonDao.GetDataTable("select * from " + dwzbmc + " where recid in (" + recids + ")");

                        // 获取单位从表
                        string dwcbmc = "s_d_" + syxmbh;
                        IList<IDictionary<string, string>> dwcb =
                            CommonDao.GetDataTable("select * from " + dwcbmc + " where byzbrecid in (" + recids + ")");

                        // 获取项目主表
                        string xmzbmc = "m_" + syxmbh;
                        IList<IDictionary<string, string>> xmzb =
                            CommonDao.GetDataTable("select * from " + xmzbmc + " where recid in (" + recids + ")");

                        // 获取项目从表
                        string xmcbmc = "s_" + syxmbh;
                        IList<IDictionary<string, string>> xmcb =
                            CommonDao.GetDataTable("select * from " + xmcbmc + " where byzbrecid in (" + recids + ")");
                        allOtherTables.Add(dwzbmc, dwzb);
                        allOtherTables.Add(dwcbmc, dwcb);
                        allOtherTables.Add(xmzbmc, xmzb);
                        allOtherTables.Add(xmcbmc, xmcb);

                    }

                    foreach (IDictionary<string, string> row in byzb)
                    {
                        string syxmbh = row["syxmbh"].ToLower();
                        if (syxmbh == "")
                            continue;
                        string recid = row["recid"].GetSafeString();

                        IDictionary<string, string> mtable = new Dictionary<string, string>();
                        IList<IDictionary<string, string>> stable = new List<IDictionary<string, string>>();

                        // 获取zdzd
                        IList<IDictionary<string, string>> zdzdsdw = allOtherZdzds[syxmbh + "_dw"];
                        IList<IDictionary<string, string>> zdzdsxm = allOtherZdzds[syxmbh + "_xm"];
                        // 获取必有从表
                        var q1 = from e in allbycbs where e["byzbrecid"] == recid select e;
                        IList<IDictionary<string, string>> bycb = q1.ToList();

                        // 获取单位主表
                        string dwzbmc = "m_d_" + syxmbh;
                        var q2 = from e in allOtherTables[dwzbmc] where e["recid"] == recid select e;
                        IList<IDictionary<string, string>> dwzb = q2.ToList();
                        if (dwzb.Count() == 0)
                        {
                            msg = syxmbh + "单位主表没记录";
                            return false;
                        }

                        // 获取单位从表
                        string dwcbmc = "s_d_" + syxmbh;
                        q2 = from e in allOtherTables[dwcbmc] where e["byzbrecid"] == recid select e;
                        IList<IDictionary<string, string>> dwcb = q2.ToList();

                        // 获取项目主表
                        string xmzbmc = "m_" + syxmbh;
                        q2 = from e in allOtherTables[xmzbmc] where e["recid"] == recid select e;
                        IList<IDictionary<string, string>> xmzb = q2.ToList();
                        if (xmzb.Count() == 0)
                        {
                            msg = syxmbh + "项目主表没记录";
                            return false;
                        }

                        // 获取项目从表
                        string xmcbmc = "s_" + syxmbh;
                        q2 = from e in allOtherTables[xmcbmc] where e["byzbrecid"] == recid select e;
                        IList<IDictionary<string, string>> xmcb = q2.ToList();

                        // 加入返回值
                        // 必有主表
                        foreach (string key in row.Keys)
                        {
                            string strValue = "";
                            if (IsExeclude(execludes, "m_by", key))
                                continue;
                            if (!mtable.TryGetValue(key, out strValue))
                            {
                                var q = from e in zdzdsby
                                        where e["sjbmc"].Equals("m_by", StringComparison.OrdinalIgnoreCase) &&
                                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                        select e;
                                if (q.Count() == 0)
                                    continue;
                                var zdzd = q.First();


                                AddSafeToDictionary(mtable, zdzd["sy"], zdzd["zdlx"], row[key].ToJson());
                            }
                        }

                        // 单位主表
                        foreach (string key in dwzb[0].Keys)
                        {
                            if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                continue;
                            if (IsExeclude(execludes, dwzbmc, key))
                                continue;
                            string strValue = "";
                            if (!mtable.TryGetValue(key, out strValue))
                            {
                                var q = from e in zdzdsdw
                                        where e["sjbmc"].Equals(dwzbmc, StringComparison.OrdinalIgnoreCase) &&
                                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                        select e;
                                if (q.Count() == 0)
                                    continue;
                                var zdzd = q.First();
                                AddSafeToDictionary(mtable, zdzd["sy"], zdzd["zdlx"], dwzb[0][key].ToJson());
                            }

                        }

                        // 项目主表
                        foreach (string key in xmzb[0].Keys)
                        {
                            if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                continue;
                            if (IsExeclude(execludes, xmzbmc, key))
                                continue;

                            string strValue = "";
                            if (!mtable.TryGetValue(key, out strValue))
                            {
                                var q = from e in zdzdsxm
                                        where e["sjbmc"].Equals(xmzbmc, StringComparison.OrdinalIgnoreCase) &&
                                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                        select e;
                                if (q.Count() == 0)
                                    continue;
                                var zdzd = q.First();
                                AddSafeToDictionary(mtable, zdzd["sy"], zdzd["zdlx"], xmzb[0][key].ToJson());
                            }
                        }

                        // 从表
                        foreach (IDictionary<string, string> row1 in bycb)
                        {
                            IDictionary<string, string> srow = new Dictionary<string, string>();
                            stable.Add(srow);
                            // 必有从表
                            string srecid = "";
                            foreach (string key in row1.Keys)
                            {
                                if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                    srecid = row1[key];
                                if (IsExeclude(execludes, "s_by", key))
                                    continue;
                                string strValue = "";
                                if (!srow.TryGetValue(key, out strValue))
                                {
                                    var q = from e in zdzdsby
                                            where e["sjbmc"].Equals("s_by", StringComparison.OrdinalIgnoreCase) &&
                                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                            select e;
                                    if (q.Count() == 0)
                                        continue;
                                    var zdzd = q.First();

                                    AddSafeToDictionary(srow, zdzd["sy"], zdzd["zdlx"], row1[key].ToJson());
                                }
                            }

                            // 单位从表
                            foreach (IDictionary<string, string> drow in dwcb)
                            {
                                if (drow["recid"].Equals(srecid))
                                {
                                    foreach (string key in drow.Keys)
                                    {
                                        if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                            key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                            continue;
                                        if (IsExeclude(execludes, dwcbmc, key))
                                            continue;
                                        string strValue = "";
                                        if (!srow.TryGetValue(key, out strValue))
                                        {
                                            var q = from e in zdzdsdw
                                                    where e["sjbmc"].Equals(dwcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                          e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                                    select e;
                                            if (q.Count() == 0)
                                                continue;
                                            var zdzd = q.First();
                                            AddSafeToDictionary(srow, zdzd["sy"], zdzd["zdlx"], drow[key].ToJson());
                                        }
                                    }
                                }
                            }

                            //项目从表
                            foreach (IDictionary<string, string> xrow in xmcb)
                            {
                                if (xrow["recid"].Equals(srecid))
                                {
                                    foreach (string key in xrow.Keys)
                                    {
                                        if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                            key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                            continue;
                                        if (IsExeclude(execludes, xmcbmc, key))
                                            continue;
                                        string strValue = "";
                                        if (!srow.TryGetValue(key, out strValue))
                                        {
                                            var q = from e in zdzdsxm
                                                    where e["sjbmc"].Equals(xmcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                          e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                                    select e;
                                            if (q.Count() == 0)
                                                continue;
                                            var zdzd = q.First();
                                            AddSafeToDictionary(srow, zdzd["sy"], zdzd["zdlx"], xrow[key].ToJson());
                                        }
                                    }
                                }
                            }
                        }

                        IDictionary<string, object> retRow = new Dictionary<string, object>();
                        retRow.Add("syxmbh", syxmbh);
                        retRow.Add("m", mtable);
                        retRow.Add("s", stable);
                        records.Add(retRow);
                    }

                    string sql = "update m_by set sfxf=1 where dbo.IsWtdCanGet(zt)=1 and qrxz = 1 and sfxf=0 and (" + sbWhere.ToString() + ")" + depcode;
                    //SysLog4.WriteError(sql);
                    CommonDao.ExecCommand(sql, CommandType.Text);
                    ret = true;
                }


            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            if (!ret)
                records.Clear();
            return ret;
        }
        #endregion

        #region 英文
        public bool GetWtdsEN(string depcode, VTransDownGetWtd[] where, out IList<IDictionary<string, object>> records,
            out string msg)
        {
            bool ret = false;
            msg = "";
            records = new List<IDictionary<string, object>>();
            try
            {
                if (where != null && where.Length > 0)
                {

                    // 获取委托单
                    StringBuilder sbWhere = new StringBuilder();
                    foreach (VTransDownGetWtd item in where)
                    {
                        string gcbh = item.gcbh.GetSafeRequest();
                        string lrr = item.lrr.GetSafeRequest();

                        if (sbWhere.Length > 0)
                            sbWhere.Append(" or ");
                        sbWhere.Append("(");
                        string str = "";
                        if (gcbh != "")
                        {
                            str += "gcbh='" + gcbh + "'";
                        }

                        if (lrr != "")
                        {
                            if (str.Length > 0)
                                str += " and ";
                            str += "wtslrrzh='" + lrr + "'";
                        }

                        if (str == "")
                            str = "1=1";
                        sbWhere.Append(str);

                        sbWhere.Append(")");
                    }

                    // 排除字段
                    IList<IDictionary<string, string>> execludes =
                        CommonDao.GetDataTable("select tablename,fieldname from SysDownSetting");
                    // 获取必有主表
                    if (depcode != "")
                        depcode = " and ytdwbh='" + depcode + "'";
                    IList<IDictionary<string, string>> byzb = CommonDao.GetDataTable(
                        "select * from m_by where dbo.IsWtdCanGet(zt)=1 and qrxz = 1 and (" + sbWhere.ToString() + ")" +
                        depcode);

                    // 获取主表recid 和 委托单编号
                    IDictionary<string, string> tableids = new Dictionary<string, string>();
                    string allrecids = "";
                    foreach (IDictionary<string, string> row in byzb)
                    {
                        string syxmbh = row["syxmbh"].ToLower();
                        if (syxmbh == "")
                            continue;
                        string recid = row["recid"].GetSafeString();
                        allrecids += recid + ",";
                        string ids = "";
                        if (tableids.TryGetValue(syxmbh, out ids))
                            tableids[syxmbh] = ids + "," + recid;
                        else
                            tableids.Add(syxmbh, recid);
                    }

                    // 获取所有必有从表
                    IList<IDictionary<string, string>> allbycbs = new List<IDictionary<string, string>>();
                    if (allrecids != "")
                        allbycbs = CommonDao.GetDataTable("select * from s_by where byzbrecid in (" +
                                                          allrecids.FormatSQLInStr() +
                                                          ") order by byzbrecid,len(zh),zh");
                    // 获取所有从表和zdzd表
                    IList<IDictionary<string, string>> zdzdsby =
                        CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from XTZD_BY");
                    IDictionary<string, IList<IDictionary<string, string>>> allOtherZdzds =
                        new Dictionary<string, IList<IDictionary<string, string>>>();
                    IDictionary<string, IList<IDictionary<string, string>>> allOtherTables =
                        new Dictionary<string, IList<IDictionary<string, string>>>();
                    foreach (string syxmbh in tableids.Keys)
                    {
                        // 获取zdzd
                        IList<IDictionary<string, string>> zdzdsdw =
                            CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from dwzd_" + syxmbh);
                        IList<IDictionary<string, string>> zdzdsxm =
                            CommonDao.GetDataTable("select sjbmc,zdmc,sy,zdlx from zdzd_" + syxmbh);
                        allOtherZdzds.Add(syxmbh + "_dw", zdzdsdw);
                        allOtherZdzds.Add(syxmbh + "_xm", zdzdsxm);
                        // 获取必有从表
                        string recids = "";
                        if (!tableids.TryGetValue(syxmbh, out recids))
                            continue;
                        recids = recids.FormatSQLInStr();
                        // 获取单位主表
                        string dwzbmc = "m_d_" + syxmbh;
                        IList<IDictionary<string, string>> dwzb =
                            CommonDao.GetDataTable("select * from " + dwzbmc + " where recid in (" + recids + ")");

                        // 获取单位从表
                        string dwcbmc = "s_d_" + syxmbh;
                        IList<IDictionary<string, string>> dwcb =
                            CommonDao.GetDataTable("select * from " + dwcbmc + " where byzbrecid in (" + recids + ")");

                        // 获取项目主表
                        string xmzbmc = "m_" + syxmbh;
                        IList<IDictionary<string, string>> xmzb =
                            CommonDao.GetDataTable("select * from " + xmzbmc + " where recid in (" + recids + ")");

                        // 获取项目从表
                        string xmcbmc = "s_" + syxmbh;
                        IList<IDictionary<string, string>> xmcb =
                            CommonDao.GetDataTable("select * from " + xmcbmc + " where byzbrecid in (" + recids + ")");
                        allOtherTables.Add(dwzbmc, dwzb);
                        allOtherTables.Add(dwcbmc, dwcb);
                        allOtherTables.Add(xmzbmc, xmzb);
                        allOtherTables.Add(xmcbmc, xmcb);

                    }

                    foreach (IDictionary<string, string> row in byzb)
                    {
                        string syxmbh = row["syxmbh"].ToLower();
                        if (syxmbh == "")
                            continue;
                        string recid = row["recid"].GetSafeString();

                        IDictionary<string, string> mtable = new Dictionary<string, string>();
                        IList<IDictionary<string, string>> stable = new List<IDictionary<string, string>>();

                        // 获取zdzd
                        IList<IDictionary<string, string>> zdzdsdw = allOtherZdzds[syxmbh + "_dw"];
                        IList<IDictionary<string, string>> zdzdsxm = allOtherZdzds[syxmbh + "_xm"];
                        // 获取必有从表
                        var q1 = from e in allbycbs where e["byzbrecid"] == recid select e;
                        IList<IDictionary<string, string>> bycb = q1.ToList();

                        // 获取单位主表
                        string dwzbmc = "m_d_" + syxmbh;
                        var q2 = from e in allOtherTables[dwzbmc] where e["recid"] == recid select e;
                        IList<IDictionary<string, string>> dwzb = q2.ToList();
                        if (dwzb.Count() == 0)
                        {
                            msg = syxmbh + "单位主表没记录";
                            return false;
                        }

                        // 获取单位从表
                        string dwcbmc = "s_d_" + syxmbh;
                        q2 = from e in allOtherTables[dwcbmc] where e["byzbrecid"] == recid select e;
                        IList<IDictionary<string, string>> dwcb = q2.ToList();

                        // 获取项目主表
                        string xmzbmc = "m_" + syxmbh;
                        q2 = from e in allOtherTables[xmzbmc] where e["recid"] == recid select e;
                        IList<IDictionary<string, string>> xmzb = q2.ToList();
                        if (xmzb.Count() == 0)
                        {
                            msg = syxmbh + "项目主表没记录";
                            return false;
                        }

                        // 获取项目从表
                        string xmcbmc = "s_" + syxmbh;
                        q2 = from e in allOtherTables[xmcbmc] where e["byzbrecid"] == recid select e;
                        IList<IDictionary<string, string>> xmcb = q2.ToList();

                        // 加入返回值
                        // 必有主表
                        foreach (string key in row.Keys)
                        {
                            string strValue = "";
                            if (IsExeclude(execludes, "m_by", key))
                                continue;
                            if (!mtable.TryGetValue(key, out strValue))
                            {
                                var q = from e in zdzdsby
                                        where e["sjbmc"].Equals("m_by", StringComparison.OrdinalIgnoreCase) &&
                                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                        select e;
                                if (q.Count() == 0)
                                    continue;
                                var zdzd = q.First();

                                //释义改成字段
                                AddSafeToDictionary(mtable, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], row[key].ToJson());
                            }
                        }

                        // 单位主表
                        foreach (string key in dwzb[0].Keys)
                        {
                            if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                continue;
                            if (IsExeclude(execludes, dwzbmc, key))
                                continue;
                            string strValue = "";
                            if (!mtable.TryGetValue(key, out strValue))
                            {
                                var q = from e in zdzdsdw
                                        where e["sjbmc"].Equals(dwzbmc, StringComparison.OrdinalIgnoreCase) &&
                                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                        select e;
                                if (q.Count() == 0)
                                    continue;
                                var zdzd = q.First();
                                //释义改成字段
                                AddSafeToDictionary(mtable, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], dwzb[0][key].ToJson());
                            }

                        }

                        // 项目主表
                        foreach (string key in xmzb[0].Keys)
                        {
                            if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                continue;
                            if (IsExeclude(execludes, xmzbmc, key))
                                continue;

                            string strValue = "";
                            if (!mtable.TryGetValue(key, out strValue))
                            {
                                var q = from e in zdzdsxm
                                        where e["sjbmc"].Equals(xmzbmc, StringComparison.OrdinalIgnoreCase) &&
                                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                        select e;
                                if (q.Count() == 0)
                                    continue;
                                var zdzd = q.First();
                                //释义改成字段
                                AddSafeToDictionary(mtable, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], xmzb[0][key].ToJson());
                            }
                        }

                        // 从表
                        foreach (IDictionary<string, string> row1 in bycb)
                        {
                            IDictionary<string, string> srow = new Dictionary<string, string>();
                            stable.Add(srow);
                            // 必有从表
                            string srecid = "";
                            foreach (string key in row1.Keys)
                            {
                                if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                    srecid = row1[key];
                                if (IsExeclude(execludes, "s_by", key))
                                    continue;
                                string strValue = "";
                                if (!srow.TryGetValue(key, out strValue))
                                {
                                    var q = from e in zdzdsby
                                            where e["sjbmc"].Equals("s_by", StringComparison.OrdinalIgnoreCase) &&
                                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                            select e;
                                    if (q.Count() == 0)
                                        continue;
                                    var zdzd = q.First();
                                    //释义改成字段
                                    AddSafeToDictionary(srow, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], row1[key].ToJson());
                                }
                            }

                            // 单位从表
                            foreach (IDictionary<string, string> drow in dwcb)
                            {
                                if (drow["recid"].Equals(srecid))
                                {
                                    foreach (string key in drow.Keys)
                                    {
                                        if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                            key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                            continue;
                                        if (IsExeclude(execludes, dwcbmc, key))
                                            continue;
                                        string strValue = "";
                                        if (!srow.TryGetValue(key, out strValue))
                                        {
                                            var q = from e in zdzdsdw
                                                    where e["sjbmc"].Equals(dwcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                          e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                                    select e;
                                            if (q.Count() == 0)
                                                continue;
                                            var zdzd = q.First();
                                            //释义改成字段
                                            AddSafeToDictionary(srow, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], drow[key].ToJson());
                                        }
                                    }
                                }
                            }

                            //项目从表
                            foreach (IDictionary<string, string> xrow in xmcb)
                            {
                                if (xrow["recid"].Equals(srecid))
                                {
                                    foreach (string key in xrow.Keys)
                                    {
                                        if (key.Equals("byzbrecid", StringComparison.OrdinalIgnoreCase) ||
                                            key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                                            continue;
                                        if (IsExeclude(execludes, xmcbmc, key))
                                            continue;
                                        string strValue = "";
                                        if (!srow.TryGetValue(key, out strValue))
                                        {
                                            var q = from e in zdzdsxm
                                                    where e["sjbmc"].Equals(xmcbmc, StringComparison.OrdinalIgnoreCase) &&
                                                          e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                                                    select e;
                                            if (q.Count() == 0)
                                                continue;
                                            var zdzd = q.First();
                                            //释义改成字段
                                            AddSafeToDictionary(srow, zdzd["zdmc"].ToUpper(), zdzd["zdlx"], xrow[key].ToJson());
                                        }
                                    }
                                }
                            }
                        }

                        IDictionary<string, object> retRow = new Dictionary<string, object>();
                        retRow.Add("syxmbh", syxmbh);
                        retRow.Add("m", mtable);
                        retRow.Add("s", stable);
                        records.Add(retRow);
                    }

                    string sql = "update m_by set sfxf=1 where dbo.IsWtdCanGet(zt)=1 and qrxz = 1 and sfxf=0 and (" + sbWhere.ToString() + ")" + depcode;
                    //SysLog4.WriteError(sql);
                    CommonDao.ExecCommand(sql, CommandType.Text);
                    ret = true;
                }


            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            if (!ret)
                records.Clear();
            return ret;
        }
        #endregion
        #endregion

        /// <summary>
        /// 设置资质有效期
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetZzyxq(int recid, string datetype, DateTime datevalue, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                if (datevalue.Equals(DateTime.MinValue))
                {
                    msg = "请输入有效的日期格式";
                    return ret;
                }

                string sql = "update PR_M_QYZB set ";
                if (datetype.Equals("s"))
                    sql += " yxqs=convert(datetime,'" + datevalue.ToString("yyyy-MM-dd") + "')";
                else
                    sql += " yxqz=convert(datetime,'" + datevalue.ToString("yyyy-MM-dd") + "')";

                sql += " where recid=" + recid;


                ret = CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 检测单位项目禁用启用
        /// </summary>
        /// <param name="dicIds"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool EnableJcdwXm(IDictionary<int, int> dicIds, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                foreach (KeyValuePair<int, int> itm in dicIds)
                    CommonDao.ExecCommand("update pr_m_syxm set sfyx=" + itm.Value + " where recid=" + itm.Key,
                        CommandType.Text);

                ret = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 获取检测机构数据传输密钥
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetJcjgmy(string qybh, out string msg)
        {
            bool ret = false;
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select sjcsmy from i_m_qy where qybh='" + qybh + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的企业编号";
                    return ret;
                }

                msg = dt[0]["sjcsmy"];
                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;

            }

            return ret;

        }

        /// <summary>
        /// 删除委托单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DeleteWtds(string ids, string userCode, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                if (ids.Length == 0)
                {
                    ret = true;
                    return ret;
                }

                string inids = ids.Trim(new char[] {','}).FormatSQLInStr();
                string sql = "select recid,syxmmc,wtdbh,syxmbh,zt,wtslrrzh from m_by where recid in (" + inids + ")";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                string undeleteinfo = "";
                string nopermissinfo = "";
                int sysWzJgJyNewJzqy = GetSysWzJgJyNewJzqy();
                foreach (IDictionary<string, string> row in dt)
                {
                    string recid = row["recid"].GetSafeString();
                    string syxmmc = row["syxmmc"].GetSafeString();
                    string wtdbh = row["wtdbh"].GetSafeString();
                    string syxmbh = row["syxmbh"].GetSafeString();
                    string zt = row["zt"].GetSafeString();
                    string wtslrrzh = row["wtslrrzh"].GetSafeString();
                    WtsStatus status = new WtsStatus(zt);
                    if (!status.CanDelete)
                    {
                        if (undeleteinfo != "")
                            undeleteinfo += "，";
                        undeleteinfo += "[" + row["syxmmc"] + "]" + row["wtdbh"];
                    }
                    else if (!string.IsNullOrEmpty(wtslrrzh) && userCode != wtslrrzh)
                    {
                        if (nopermissinfo != "")
                            nopermissinfo += "，";
                        nopermissinfo += "[" + row["syxmmc"] + "]" + row["wtdbh"];
                    }
                    else
                    {
                        List<string> sqls = new List<string>();

                        sqls.Add("delete from m_by where recid='" + recid + "'");
                        sqls.Add("delete from s_by where byzbrecid='" + recid + "'");
                        sqls.Add("delete from m_d_" + syxmbh + " where recid='" + recid + "' ");
                        sqls.Add("delete from s_d_" + syxmbh + " where byzbrecid='" + recid + "' ");
                        sqls.Add("delete from m_" + syxmbh + " where recid='" + recid + "' ");
                        sqls.Add("delete from s_" + syxmbh + " where byzbrecid='" + recid + "' ");
                        sqls.Add("update UP_WTDTP set SFYX = 0 where WTDWYH = '" + recid + "' ");
                        sqls.Add("update UP_WTDTP set SFYX = 0 where OLDWTDWYH = '" + recid + "' ");

                        if (sysWzJgJyNewJzqy == (int)SysWzJgJyNewJzqyEnum.Enabled)
                        {
                            var sdt = CommonDao.GetDataTable(string.Format("select jzrecid from s_by where byzbrecid = '{0}'", recid));
                            var jzrecids = sdt.Select(x => x["jzrecid"]).Where(x => !string.IsNullOrEmpty(x)).ToList();

                            if (jzrecids.Count() > 0)
                            {
                                var remoteRet = JyJzqyService.UpdateOrderStatus(jzrecids, 0, recid);

                                if (!remoteRet.success)
                                {
                                    sqls.Add(string.Format(@"insert into UP_JyOrderStatus (WtdWyh, jzRecIds, OrderStatus, RemoteMsg, IsUpdate)
                                values ('{0}', '{1}', '{2}', '{3}', 0)", recid, JsonSerializer.Serialize(jzrecids), 0, remoteRet.msg));
                                }
                            }
                        }

                        foreach (string str in sqls)
                        {
                            CommonDao.ExecCommand(str);
                        }
                    }
                }

                if (undeleteinfo != "")
                    msg = "委托单" + undeleteinfo + "已送样，不允许删除";

                if (nopermissinfo != "")
                    msg += "委托单" + nopermissinfo + "不是该账号创建，不允许删除";

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 获取最后一份已有二维码的报告唯一号和顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgwyh"></param>
        /// <param name="sxh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetReportAbs(string wtdwyh, out string bgwyh, out string sxh, out string msg)
        {
            bool ret = false;
            msg = "";
            bgwyh = "";
            sxh = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select bgwyh from up_bgsj where wtdbh='" + wtdwyh + "' and bgewm<>'' order by scsj desc");
                if (dt.Count == 0)
                {
                    msg = "没有生成二维码的对应报告摘要";
                    return ret;
                }

                bgwyh = dt[0]["bgwyh"];
                IList<IDictionary<string, object>> dtf =
                    CommonDao.GetBinaryDataTable("select sxh from up_bgwj where bgwyh='" + bgwyh + "'");
                if (dtf.Count == 0)
                {
                    msg = "没有生成二维码的对应报告文件";
                    return ret;
                }

                foreach (IDictionary<string, object> row in dtf)
                {
                    sxh += row["sxh"].GetSafeString() + ",";
                }

                sxh = sxh.Trim(new char[] {','});

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        public bool GetReportAbsByBgwyh(string bgwyh, string sdsc, out string sxh, out string msg)
        {
            bool ret = false;
            msg = "";
            sxh = "";
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(sdsc))
                    where = " and sdsc=" + sdsc.GetSafeInt();
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select bgwyh from up_bgsj where bgwyh='" + bgwyh + "' and bgewm<>'' " + where +
                    " order by scsj desc");
                if (dt.Count == 0)
                {
                    msg = "没有生成二维码的对应报告摘要";
                    return ret;
                }

                IList<IDictionary<string, object>> dtf =
                    CommonDao.GetBinaryDataTable("select sxh from up_bgwj where bgwyh='" + bgwyh + "'");
                if (dtf.Count == 0)
                {
                    msg = "没有生成二维码的对应报告文件";
                    return ret;
                }

                foreach (IDictionary<string, object> row in dtf)
                {
                    sxh += row["sxh"].GetSafeString() + ",";
                }

                sxh = sxh.Trim(new char[] {','});

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 返回某个委托单所有已有二维码的报告唯一号和顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgabsjson"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetReportAbs(string wtdwyh, out IList<IDictionary<string, string>> bgabs, out string msg)
        {
            bool ret = false;
            msg = "";
            bgabs = new List<IDictionary<string, string>>();
            try
            {
                bgabs = CommonDao.GetDataTable(
                    "select a.bgwyh,convert(varchar(50),a.scsj,120) as scsj,b.sxh from up_bgsj a inner join up_bgwj b on a.bgwyh=b.bgwyh where a.wtdbh='" +
                    wtdwyh + "' and bgewm<>'' order by a.scsj desc,b.sxh asc");
                if (bgabs.Count == 0)
                {
                    msg = "没有生成二维码的对应报告";
                    return ret;
                }

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 根据报告唯一号和顺序号获取报告内容
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgwyh"></param>
        /// <param name="sxh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetReportFile(string bgwyh, string sxh, out byte[] file, out string msg)
        {
            bool ret = false;
            msg = "";
            file = null;
            try
            {
                IList<IDictionary<string, object>> dtf = CommonDao.GetBinaryDataTable(
                    "select bgwj,osscdnurl from up_bgwj where bgwyh='" + bgwyh + "' and sxh=" + sxh.GetSafeInt());
                if (dtf.Count == 0)
                {
                    msg = "找不到对应的报告文件";
                    return ret;
                }

                //目前先兼容数据库存二进制文件,转换完之后把bgwj字段备份后替换为空
                var ossCdnUrl = dtf[0]["osscdnurl"].GetSafeString();

                if (!string.IsNullOrEmpty(ossCdnUrl))
                {
                    var fileReturn = OssCdnHelper.GetByOssCdnUrl(ossCdnUrl, "pdf");

                    if (fileReturn.Success)
                    {
                        file = fileReturn.FileBytes;
                    }
                    else
                    {
                        msg = fileReturn.ErrorMsg;
                        return ret;
                    }
                }
                else
                {
                    file = dtf[0]["bgwj"] as byte[];
                }

                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 对外设置委托单状态
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetWtdycztAll(out string msg)
        {
            msg = "";
            bool ret = true;
            return ret;
            /*
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select recid from m_by order by recid");
                foreach (IDictionary<string, string> row in dt)
                {
                    string recid = row["recid"];
                    ret = SetWtdycztIn(recid, out msg);
                    if (!ret)
                        SysLog4.WriteError("设置委托单：" + recid + "状态错误，信息：" + msg);
                }

            }
            catch (Exception ex)
            {
                ret = false;
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;*/
        }

        /// <summary>
        /// 对外设置委托单状态
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetWtdyczt(string wtdwyh, out string msg)
        {
            return SetWtdycztIn(wtdwyh, out msg);
        }

        /// <summary>
        /// 设置委托单异常状态，以及数据状态
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool SetWtdycztIn(string wtdwyh, out string msg)
        {

            //bool ret = false;
            msg = "";
            return true;
            /*
            try
            {
                // 获取委托单原始数据
                //IDictionary<string, string> orgwtdm = null;
                //IList<IDictionary<string, string>> orgwtds = null;
                //ret = GetWtd(wtdwyh, out orgwtdm, out orgwtds, out msg);
                //if (!ret)
                //    return ret;
                // -------------获取上传的报告数据-------------
                IList<IDictionary<string, string>> upbgsj = CommonDao.GetDataTable("select * from UP_BGSJ where wtdbh='" + wtdwyh + "' order by scsj desc");
                string bgwyhs = "";
                foreach (IDictionary<string, string> row in upbgsj)
                    bgwyhs += row["bgwyh"] + ",";
                bgwyhs = bgwyhs.FormatSQLInStr();
                IList<IDictionary<string, string>> upbgxqm = CommonDao.GetDataTable("select * from UP_BGXQM where bgwyh in (" + bgwyhs + ")");
                IList<IDictionary<string, string>> upbgxqs = CommonDao.GetDataTable("select * from UP_BGXQS where bgwyh in (" + bgwyhs + ")");
                // --------------获取上传的采集数据-------------
                IList<IDictionary<string, string>> upsysj = CommonDao.GetDataTable("select * from UP_SYSJ where wtdbh='" + wtdwyh + "' order by scsj desc");
                string sywyhs = "";
                bool hasVideo = false;
                foreach (IDictionary<string, string> row in upsysj)
                {
                    sywyhs += row["sywyh"] + ",";
                    if (row["spwj"].GetSafeString() != "" || row["lpwj"].GetSafeString() != "")
                        hasVideo = true;
                }
                sywyhs = sywyhs.FormatSQLInStr();
                IList<IDictionary<string, string>> upsyxq = CommonDao.GetDataTable("select * from UP_SYXQ where sywyh in (" + sywyhs + ")");
                IList<IDictionary<string, string>> upsyqx = CommonDao.GetDataTable("select count(*) as c1 from up_syqx where sywyh in (" + sywyhs + ")");
                IList<IDictionary<string, string>> upczsj = CommonDao.GetDataTable("select count(*) as c1 from UP_CZSJ where sywyh in (" + sywyhs + ")");
                // -----------------获取变更单--------------------
                IList<IDictionary<string, string>> upbgds = CommonDao.GetDataTable("select count(*) as c1 from up_bgd where wtdbh='" + wtdwyh + "'");
                // -----------------设置数据状态------------------                
                WtsSjzt sjzt = new WtsSjzt();                
                // 采集数据
                if (upsysj.Count > 0)
                    sjzt.AddStatus(WtsSjzt.HasData);
                // 曲线
                if (upsyqx[0]["c1"].GetSafeInt() > 0)
                    sjzt.AddStatus(WtsSjzt.HasCurve);
                // 视频
                if (hasVideo)
                    sjzt.AddStatus(WtsSjzt.HasVideo);
                // -------------------设置异常状态-----------------
                WtsYczt yczt = new WtsYczt();
                // 委托单修改
				
                if (upbgds[0]["c1"].GetSafeInt() > 0)
                    yczt.AddStatus(WtsYczt.WtsModify);
                
                // 自动采集数据修改
                IDictionary<string, string> lastSywyhs = GetLastSywyhGroupBySymc(upsysj);// 最后试验唯一号
                if (upbgsj.Count > 0 && lastSywyhs.Keys.Count > 0)
                {
                    var q = from e in upbgxqs where e["bgwyh"] == upbgsj[0]["bgwyh"] select e;
                    IList<IDictionary<string, string>> lastBgsjs = q.ToList();
                    string strLastSywyh = ",";
                    foreach (string key in lastSywyhs.Keys)
                    {
                        strLastSywyh += lastSywyhs[key] + ",";
                    }
                    q = from e in upsyxq where strLastSywyh.IndexOf("," + e["sywyh"] + ",") > -1 select e;
                    IList<IDictionary<string, string>> lastSysjs = q.ToList();
                    foreach (IDictionary<string, string> row in lastSysjs)
                    {
                        string sy_zdhy = row["zdhy"];
                        string sy_zdz = row["zdz"];
                        q = from e in lastBgsjs where e["zdhy"] == sy_zdhy && e["zdz"] != sy_zdz select e;
                        if (q.Count() > 0)
                        {
                    yczt.AddStatus(WtsYczt.DataModify);
                            break;
                        }
                    }

                }
                // 未保存数据
                var q4 = from e in upsysj where e["sfbc"].GetSafeInt() == 0 select e;
                if (q4.Count() > 0)
                    yczt.AddStatus(WtsYczt.DataUnsave);
                // 重做数据
                if (upczsj[0]["c1"].GetSafeInt() > 0)
                    yczt.AddStatus(WtsYczt.DataRedo);
                // 重复试验
                if (upbgsj.Count > lastSywyhs.Keys.Count)
                    yczt.AddStatus(WtsYczt.DataRepeat);
                // 重复报告
                if (upsysj.Count > 1)
                    yczt.AddStatus(WtsYczt.ReportRepeat);
                ret = CommonDao.ExecCommand("update m_by set yczt=" + yczt.Status + ",sjzt=" + sjzt.Status + ",SYSJZHSCSJ=(select max(scsj) from up_sysj where wtdbh='" + wtdwyh + "'),BGZHSCSJ=(select max(scsj) from up_bgsj where wtdbh='" + wtdwyh + "'),SYKSSJ=(select top 1 sykssj from up_sysj where wtdbh='" + wtdwyh + "' order by scsj desc),SYJSSJ=(select top 1 SYJSSJ from up_sysj where wtdbh='" + wtdwyh + "' order by scsj desc),jcjg=(select top 1 jcjg from up_bgsj where wtdbh='"+wtdwyh+"' order by scsj desc),jcjgms=(select top 1 jcjgms from up_bgsj where wtdbh='"+wtdwyh+"' order by scsj desc)  where recid='" + wtdwyh + "'", CommandType.Text);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;*/
        }

        /// <summary>
        /// 获取一个委托单的最后试验唯一号，根据symc分组
        /// </summary>
        /// <param name="sysjs">已经根据scsj逆序</param>
        /// <returns>试验名称，试验唯一号的集合</returns>
        private IDictionary<string, string> GetLastSywyhGroupBySymc(IList<IDictionary<string, string>> sysjs)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                foreach (IDictionary<string, string> row in sysjs)
                {
                    string sywyh = row["sywyh"];
                    string symc = row["symc"];
                    if (!ret.ContainsKey(symc))
                        ret.Add(symc, sywyh);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 获取某次试验的数据
        /// </summary>
        /// <param name="sywyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetSysjxq(string sywyh, out string msg)
        {
            IList<IDictionary<string, string>> ret = null;
            msg = "";
            try
            {
                string sql = "select * from up_syxq where SYWYH='" + sywyh + "'";
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取试验曲线图片
        /// </summary>
        /// <param name="sywyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] GetSysjqx(string sywyh, out string msg)
        {
            msg = "";
            byte[] file = null;
            try
            {
                IList<IDictionary<string, object>> dtf =
                    CommonDao.GetBinaryDataTable("select qxtp from up_syqx where sywyh='" + sywyh + "'");
                if (dtf.Count == 0)
                {
                    msg = "找不到对应的报告文件";
                    return file;
                }

                file = dtf[0]["qxtp"] as byte[];

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return file;
        }

        /// <summary>
        /// 获取委托单的所有试验记录
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetWtdSysjs(string wtdwyh, out string msg)
        {
            IList<IDictionary<string, string>> ret = null;
            msg = "";
            try
            {
                string sql =
                    "select *,(select b.uploadfileid from companyfileoss b where b.srcfilename=up_sysj.spwj+'.flv') as uploadfileid from up_sysj where wtdbh='" +
                    wtdwyh + "' order by scsj desc,zh asc";
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单所有试验记录详情
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetWtdSysjxqs(string wtdwyh, out string msg)
        {
            IList<IDictionary<string, string>> ret = null;
            msg = "";
            try
            {
                string sql = "select * from up_syxq where SYWYH in (select sywyh from up_sysj where wtdbh='" + wtdwyh +
                             "') order by sywyh";
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单的所有报告和报告顺序号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgwyh"></param>
        /// <param name="sxh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetWtdReports(string wtdwyh, out string msg)
        {
            msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select a.bgwyh,a.qfrq,a.scsj,b.sxh from up_bgsj a inner join up_bgwj b on a.bgwyh=b.bgwyh where a.wtdbh='" +
                    wtdwyh + "'  order by a.scsj desc,b.sxh asc");
                if (dt.Count == 0)
                {
                    msg = "没有报告记录";
                    return ret;
                }

                foreach (IDictionary<string, string> dr in dt)
                {
                    string bgwyh = dr["bgwyh"];
                    string qfrq = dr["qfrq"].GetSafeDate().ToString("yyyy-MM-dd");
                    string sxh = dr["sxh"];
                    string scsj = dr["scsj"].GetSafeDate().ToString("yyyy-MM-dd");

                    IDictionary<string, string> row = new Dictionary<string, string>();
                    row.Add("bgwyh", bgwyh);
                    row.Add("qfrq", qfrq);
                    row.Add("sxh", sxh);
                    row.Add("scsj", scsj);
                    ret.Add(row);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 查询所有委托单，包括收样和未收样的
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="zt"></param>
        /// <param name="lrrzh"></param>
        /// <param name="gcbh"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetWtds(string syxmbh, string zt, string lrrzh, string gcbh, int pageSize, int pageIndex,
            out int totalCount, out IList<IDictionary<string, string>> records, out string msg)
        {
            bool ret = false;
            ;
            msg = "";
            records = new List<IDictionary<string, string>>();
            totalCount = 0;
            try
            {

                if (lrrzh == "")
                {
                    msg = "人员账号不能为空";
                    return ret;
                }

                string sql = "select * from View_WtdZtfx where (not exists (select * from i_m_qyzh where yhzh='" +
                             lrrzh + "') or ( gcbh in (select gcbh from i_s_gc_syry where rybh='" + lrrzh + "'))) ";
                if (syxmbh != "" && !syxmbh.Equals("all", StringComparison.OrdinalIgnoreCase))
                    sql += " and syxmbh='" + syxmbh + "'";

                if (gcbh != "")
                    sql += " and gcbh='" + gcbh + "'";
                if (zt != "")
                {
                    if (zt == "B1")
                        sql += " and SY_BGBS='是'";
                    else if (zt == "S1")
                        sql += "and SY_SYBS='是'";
                    else if (zt == "W4")
                        sql += " and SY_XFBS='是'";
                }

                sql += " order by recid desc";
                records = CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        /// <summary>
        /// 获取人员相关工程
        /// </summary>
        /// <param name="ryzh"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetGcs(string ryzh, out IList<IDictionary<string, string>> records, out string msg)
        {
            bool ret = false;
            ;
            msg = "";
            records = new List<IDictionary<string, string>>();
            try
            {

                if (ryzh == "")
                {
                    msg = "人员账号不能为空";
                    return ret;
                }

                string sql = "select * from i_m_gc where (not exists (select * from i_m_qyzh where yhzh='" + ryzh +
                             "') or ( gcbh in (select gcbh from i_s_gc_syry where rybh='" + ryzh +
                             "'))) order by gcmc asc";

                records = CommonDao.GetDataTable(sql);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        /// <summary>
        /// 根据用户代码获取企业编号
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public string GetQybh(string usercode)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select qybh,sfqyzzh from i_m_qyzh where yhzh='" + usercode + "'");
                if (dt.Count == 0)
                    return ret;
                if (dt[0]["sfqyzzh"].GetSafeBool())
                    ret = dt[0]["qybh"].GetSafeString();
                else
                {
                    dt = CommonDao.GetDataTable("select qybh from i_m_ry where rybh='" + dt[0]["qybh"] + "'");
                    if (dt.Count > 0)
                        ret = dt[0]["qybh"].GetSafeString();
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 根据企业编号获取企业名称
        /// </summary>
        /// <param name="qybh"></param>
        /// <returns></returns>
        public string GetQymc(string qybh)
        {
            string ret = "";

            try
            {
                var dt = CommonDao.GetDataTable("select QYMC from I_M_QY where qybh='" + qybh + "'");
                if (dt.Count > 0)
                    ret = dt[0]["QYMC"].GetSafeString();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 根据用户代码获取登录账号编号
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>   
        public string GetUserbh(string usercode)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select qybh,sfqyzzh from i_m_qyzh where yhzh='" + usercode + "'");
                if (dt.Count == 0)
                    return ret;
                ret = dt[0]["qybh"].GetSafeString();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 根据企业编号获取企业类型编号
        /// </summary>
        /// <param name="qyzhbh"></param>
        /// <returns></returns>
        public string GetLxbh(string qyzhbh)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select STUFF((SELECT ','+QYLXBH FROM I_S_QY_QYZZ WHERE QYBH=a.QYBH group by QYLXBH FOR XML PATH('')),1,1,'') as lxbh from I_M_QY a where qybh='" + qyzhbh + "'");
                if (dt.Count == 0)
                    return ret;
                ret = dt[0]["lxbh"].GetSafeString();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 获取现场检测委托单的摄像头编号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        public string GetWtdXcjcSxt(string wtdwyh)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select sxtbh from UP_CJSXT where wtdbh='" + wtdwyh + "' and sfjs<>1");
                if (dt.Count > 0)
                    ret = dt[0]["sxtbh"];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单对应项目打印的委托单份数
        /// </summary>
        /// <param name="wtdwyhs">委托单唯一号列表</param>
        /// <param name="msg">错误信息</param>
        /// <returns>返回字典列表：wtdwyh,wtddyfs</returns>
        public IList<IDictionary<string, string>> GetSyxmWtddyfs(IList<string> wtdwyhs, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                string sql =
                    "select a.recid as wtdwyh,b.wtddyfs from m_by  a inner join pr_m_syxm b on a.ytdwbh=b.ssdwbh and a.syxmbh=b.syxmbh where a.recid in (" +
                    string.Join(",", wtdwyhs.ToArray()).FormatSQLInStr() + ")";
                ret = CommonDao.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单打印内容
        /// </summary>
        /// <param name="wheres">syxmbh|recid</param>
        /// <returns>(recid,委托单数据（表名，表记录集）)</returns>
        public IDictionary<string, IDictionary<string, IList<IDictionary<string, object>>>> GetWtdPrintInfos(
            string[] wheres, out string msg)
        {
            msg = "";
            IDictionary<string, IDictionary<string, IList<IDictionary<string, object>>>> ret =
                new Dictionary<string, IDictionary<string, IList<IDictionary<string, object>>>>(StringComparer
                    .OrdinalIgnoreCase);
            try
            {
                IDictionary<string, string> sylbs = new Dictionary<string, string>();
                foreach (string item in wheres)
                {
                    string[] arr = item.Split(new char[] {'|'});
                    if (arr.Length < 2)
                        continue;
                    string sylb = arr[0], recid = arr[1];
                    if (sylbs.ContainsKey(sylb))
                        sylbs[sylb] = sylbs[sylb] + recid + ",";
                    else
                        sylbs.Add(sylb, recid + ",");
                }

                foreach (string sylb in sylbs.Keys)
                {
                    string idwhere = sylbs[sylb].FormatSQLInStr();
                    IList<IDictionary<string, object>> byzb =
                        CommonDao.GetBinaryDataTable("select * from m_by where recid in (" + idwhere + ")");
                    // 获取必有从表
                    IList<IDictionary<string, object>> bycb =
                        CommonDao.GetBinaryDataTable("select * from s_by where byzbrecid in (" + idwhere + ")");
                    // 获取单位主表
                    string dwzbmc = "m_d_" + sylb;
                    IList<IDictionary<string, object>> dwzb =
                        CommonDao.GetBinaryDataTable("select * from " + dwzbmc + " where recid in (" + idwhere + ")");
                    // 获取单位从表
                    string dwcbmc = "s_d_" + sylb;
                    IList<IDictionary<string, object>> dwcb =
                        CommonDao.GetBinaryDataTable(
                            "select * from " + dwcbmc + " where byzbrecid in (" + idwhere + ")");
                    // 获取项目主表
                    string xmzbmc = "m_" + sylb;
                    IList<IDictionary<string, object>> xmzb =
                        CommonDao.GetBinaryDataTable("select * from " + xmzbmc + " where recid in (" + idwhere + ")");
                    // 获取项目从表
                    string xmcbmc = "s_" + sylb;
                    IList<IDictionary<string, object>> xmcb =
                        CommonDao.GetBinaryDataTable(
                            "select * from " + xmcbmc + " where byzbrecid in (" + idwhere + ")");

                    foreach (IDictionary<string, object> byzbrow in byzb)
                    {
                        string recid = byzbrow["recid"].GetSafeString();
                        IDictionary<string, IList<IDictionary<string, object>>> tableDatas =
                            new Dictionary<string, IList<IDictionary<string, object>>>(StringComparer
                                .OrdinalIgnoreCase);
                        IList<IDictionary<string, object>> byzbs = new List<IDictionary<string, object>>();
                        byzbs.Add(byzbrow);
                        tableDatas.Add("m_by", byzbs);
                        IList<IDictionary<string, object>> bycbs = bycb
                            .Where(e => e["byzbrecid"].GetSafeString() == recid).OrderBy(e => e["zh"].GetSafeInt())
                            .ToList();
                        tableDatas.Add("s_by", bycbs);
                        IList<IDictionary<string, object>> dwzbs = dwzb.Where(e => e["recid"].GetSafeString() == recid)
                            .ToList();
                        tableDatas.Add(dwzbmc, dwzbs);
                        IList<IDictionary<string, object>> xmzbs = xmzb.Where(e => e["recid"].GetSafeString() == recid)
                            .ToList();
                        tableDatas.Add(xmzbmc, xmzbs);
                        IList<IDictionary<string, object>> dwcbsOrg =
                            dwcb.Where(e => e["byzbrecid"].GetSafeString() == recid).ToList();
                        IList<IDictionary<string, object>> xmcbsOrg =
                            xmcb.Where(e => e["byzbrecid"].GetSafeString() == recid).ToList();
                        IList<IDictionary<string, object>> dwcbs = new List<IDictionary<string, object>>();
                        IList<IDictionary<string, object>> xmcbs = new List<IDictionary<string, object>>();
                        foreach (IDictionary<string, object> bycbRow in bycbs)
                        {
                            var findsDw = dwcbsOrg.Where(e =>
                                e["recid"].GetSafeString() == bycbRow["recid"].GetSafeString());
                            if (findsDw.Count() > 0)
                                dwcbs.Add(findsDw.First());
                            var findsXm = xmcbsOrg.Where(e =>
                                e["recid"].GetSafeString() == bycbRow["recid"].GetSafeString());
                            if (findsXm.Count() > 0)
                                xmcbs.Add(findsXm.First());
                        }

                        tableDatas.Add(dwcbmc, dwcbs);
                        tableDatas.Add(xmcbmc, xmcbs);

                        ret.Add(recid, tableDatas);

                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        #endregion

        #region 委托单状态设置

        /// <summary>
        /// 设置委托单提交状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool SetWtdStatusTj(string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select zt from m_by where recid='" + recid + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的记录号";
                    return ret;
                }

                string zt = dt[0]["zt"];
                WtsStatus objzt = new WtsStatus(zt);
                if (!objzt.CanWtsSubmit)
                {
                    msg = "委托单已提交，不需要重复提交";
                    return ret;
                }

                if (!objzt.SetWtdTj(out msg))
                    return ret;
                ret = CommonDao.ExecCommand(
                    "update m_by set zt='" + objzt.GetStatus() + "',sfxf=0 where recid='" + recid + "'",
                    CommandType.Text);

                if (!ret)
                {
                    msg = "更新失败";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单保存状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdStatusBc(string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单打印状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool SetWtdStatusDy(string recid, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                if (recid == "")
                    return ret;
                recid = recid.FormatSQLInStr();
                string sql = "update m_by set zt=substring(zt,1," + WtsStatus.WtStateIndex + ")+'" +
                             WtsStatus.WtStateDy + "'+substring(zt," + (WtsStatus.WtStateIndex + 2) + ",len(zt)-" +
                             (WtsStatus.WtStateIndex + 1) + "),WTSDYRZH='" + CurrentUser.UserName + "',WTSDYRXM='" +
                             CurrentUser.RealName + "',WTSDYSJ=getdate() where recid in (" + recid +
                             ") and dbo.isvalidwtd(zt)=0";
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单状态为下发到检测机构
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool SetWtdStatusXf(string recid, string qybh, out string msg, bool sdsy = false)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 查询原始记录
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select a.syxmbh,a.zt,a.sydwbh,a.jclx,b.jztpqyrq from m_by a left outer join h_zjz b on a.sszjzbh=b.zjzbh where a.recid='" +
                    recid + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的记录号";
                    return ret;
                }

                // 判断状态
                string zt = dt[0]["zt"];
                string syxmbh = dt[0]["syxmbh"];
                string sydwbh = dt[0]["sydwbh"];
                string jclx = dt[0]["jclx"];
                DateTime qyrq = dt[0]["jztpqyrq"].GetSafeDate();
                WtsStatus objzt = new WtsStatus(zt);
                if (!objzt.CanWtsDown)
                {
                    if (sydwbh == qybh)
                        return true;
                    msg = "该委托单已经送样，不能再送样";
                    return ret;
                }

                // 见证取样，并且质监站已经启用，进行比较
                if (SysUseSytp() && jclx == mStrJzqy && (qyrq.Year != 1900 && qyrq < DateTime.Now))
                {
                    // 项目是否需要上传现场照片，如果要，需要见证人确认
                    dt = CommonDao.GetDataTable("select SCJZQYTP from PR_M_SYXM where ssdwbh='" + qybh +
                                                "' and syxmbh='" + syxmbh + "'");
                    if (dt.Count() == 0)
                    {
                        msg = "找不到对应的项目";
                        return ret;
                    }

                    bool scjzqytp = dt[0]["scjzqytp"].GetSafeString().ToLower() == "true";
                    if (scjzqytp && objzt.CanSetJzzt)
                    {
                        msg = "需要见证人比对图片同意才能收样";
                        return ret;
                    }
                }

                // 送样单位信息
                dt = CommonDao.GetDataTable("select qymc from i_m_qy where qybh='" + qybh + "'");
                if (dt.Count == 0)
                {
                    msg = "单位代码无效";
                    return ret;
                }

                string qymc = dt[0]["qymc"];
                // 判断检测机构资质
                dt = CommonDao.GetDataTable("select jcxm from s_" + syxmbh + " where byzbrecid='" + recid + "'");
                if (dt.Count > 0)
                {
                    IList<string> wtszbs = new List<string>();
                    for (int i = 0; i < dt.Count; i++)
                    {
                        string zbs = dt[i]["jcxm"].Trim();
                        if (zbs.GetSafeString() == "")
                            continue;
                        wtszbs = zbs.StringToList(new char[] {','}, true, wtszbs);
                    }

                    if (wtszbs.Count > 0)
                    {
                        IList<string> jczxzbs = new List<string>();
                        dt = CommonDao.GetDataTable(
                            "select distinct b.zbmc from pr_m_qyzb a inner join pr_m_zb b on a.zbbh=b.recid where a.yxqs<=getdate() and dateadd(d,1,a.yxqz)>getdate() and a.qybh='" +
                            qybh + "' and b.sfyx=1 and b.syxmbh='" + syxmbh + "'");
                        for (int i = 0; i < dt.Count; i++)
                        {
                            string zb = dt[i]["zbmc"].Trim();
                            if (zb.GetSafeString() == "")
                                continue;
                            jczxzbs.Add(zb);
                        }

                        if (!jczxzbs.ListContains(wtszbs))
                        {
                            msg = "检测中心资质不够，无法送样";
                            return ret;
                        }
                    }
                }

                // 设置送样机构和状态
                if (!objzt.SetWtdXf(out msg))
                    return ret;
                ret = CommonDao.ExecCommand(
                    "update m_by set zt='" + objzt.GetStatus() + "',sydwbh='" + qybh + "',sydwmc='" + qymc +
                    "',JYSJ= getdate(), sdsy = " + (sdsy ? 1:0) + " where recid='" + recid + "'", CommandType.Text);
                if (!ret)
                {
                    msg = "更新委托单状态失败";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 取消委托单下发状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool CancelWtdStatusXf(string recid, string qybh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 查询原始记录
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select syxmbh,zt,sydwbh from m_by where recid='" + recid + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的记录号";
                    return ret;
                }

                // 判断状态
                string zt = dt[0]["zt"];
                string sydwbh = dt[0]["sydwbh"];
                WtsStatus objzt = new WtsStatus(zt);
                if (objzt.CanWtsDown)
                    return true;
                if (!objzt.CanWtsCancelDown)
                {
                    msg = "该委托单已经试验，或者是未送样，不能取消送样";
                    return ret;
                }

                if (sydwbh != qybh)
                {
                    msg = "不能取消其他单位送样的委托单";
                    return ret;
                }


                // 设置送样机构和状态
                if (!objzt.SetWtdDy(out msg))
                    return ret;
                ret = CommonDao.ExecCommand(
                    "update m_by set zt='" + objzt.GetStatus() + "',sydwbh='',sydwmc='',JYSJ=null where recid='" +
                    recid + "'", CommandType.Text);
                if (!ret)
                {
                    msg = "更新委托单状态失败";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单为已试验状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdStatusSy(string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单为报告已出状态
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetWtdStatusBg(string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                if (recid == "")
                    return ret;
                recid = recid.FormatSQLInStr();
                string sql = "update m_by set zt='" + WtsStatus.MainStateBg +
                             "'+substring(zt,2,len(zt)-1) where recid='" + recid + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单锁定解锁，sfsd-1锁定，0解锁
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool SetWtdSd(string dwbh, string recids, int sfsd, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string str = sfsd != 0 ? "1" : "0";
                ret = CommonDao.ExecCommand(
                    "update m_by set zt=STUFF (zt,10,1,'" + str + "') where recid in (" + recids.FormatSQLInStr() + ")",
                    CommandType.Text);
                if (!ret)
                    msg = "更新失败";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单作废
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recid"></param>
        /// <param name="reason"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetWtdZf(string dwbh, string recid, string reason, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                if (string.IsNullOrEmpty(reason))
                {
                    msg = "作废原因不能为空";
                    return ret;
                }

                if (reason.Length > 500)
                {
                    msg = "作废原因不能超过500个汉字";
                    return ret;
                }

                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select sydwbh from m_by where recid='" + recid + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return ret;
                }

                string sydwbh = dt[0]["sydwbh"].GetSafeString();
                if (string.IsNullOrEmpty(sydwbh))
                {
                    msg = "未送样的委托单不能作废";
                    return ret;
                }

                if (sydwbh != dwbh)
                {
                    msg = "不能作废其他单位的委托单";
                    return ret;
                }

                ret = CommonDao.ExecCommand(
                    "update m_by set zt=STUFF (zt," + (WtsStatus.ZfStateIndex + 1) + ",1,'1'),zfyy='" + reason +
                    "',slwtdbh = '', scwts=1, scwtsdz = '' where recid='" + recid + "'", CommandType.Text);

                if (ret)
                {
                    int sysWzJgJyNewJzqy = GetSysWzJgJyNewJzqy();

                    if (sysWzJgJyNewJzqy == (int)SysWzJgJyNewJzqyEnum.Enabled)
                    {
                        var sdt = CommonDao.GetDataTable(string.Format("select jzrecid from s_by where byzbrecid = '{0}'", recid));
                        var jzrecids = sdt.Select(x => x["jzrecid"]).Where(x => !string.IsNullOrEmpty(x)).ToList();

                        if (jzrecids.Count() > 0)
                        {
                            var remoteRet = JyJzqyService.UpdateOrderStatus(jzrecids, 0, recid);

                            if (!remoteRet.success)
                            {
                                CommonDao.ExecCommand(string.Format(@"insert into UP_JyOrderStatus (WtdWyh, jzRecIds, OrderStatus, RemoteMsg, IsUpdate)
                                values ('{0}', '{1}', '{2}', '{3}', 0)", recid, JsonSerializer.Serialize(jzrecids), 0, remoteRet.msg));
                            }
                        }

                        //作废后,取样记录变更为可用
                        CommonDao.ExecCommand(string.Format("update s_by set jzrecid = '' where byzbrecid = '{0}' and jzrecid > ''", recid));
                    }
                }
                else
                    msg = "更新失败";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单未收样
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetWtdWSY(string dwbh, string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string,string>> dt = CommonDao.GetDataTable("select sydwbh, zt from m_by where recid='" + recid + "'");
                
                if (dt.Count() == 0)
                {
                    msg = "无效的平台编号";
                    return ret;
                }

                string sydwbh = dt[0]["sydwbh"].GetSafeString();
                WtsStatus status = new WtsStatus(dt[0]["zt"]);

                if (string.IsNullOrEmpty(sydwbh) || !status.HasWtdDown)
                {
                    msg = "未送样的委托单不能退回";
                    return ret;
                }

                if (sydwbh != dwbh)
                {
                    msg = "不能操作其他单位的委托单";
                    return ret;
                }

                var result = status.SetWtdWSY(out msg);

                if (!result)
                {
                    return ret;
                }

                string sql = string.Format("update m_by set zt = '{0}', slwtdbh = '', scwts=1, scwtsdz = '', syth = 1 where recid='{1}'", status.GetStatus(), recid);

                ret = CommonDao.ExecCommand(sql, CommandType.Text);

                if (!ret)
                    msg = "更新失败";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 设置委托单报告查看
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetWtdBgck(string dwbh, string recid, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select sydwbh, zt from m_by where recid='" + recid + "'");

                if (dt.Count() == 0)
                {
                    msg = "无效的平台编号";
                    return ret;
                }

                string sydwbh = dt[0]["sydwbh"].GetSafeString();

                if (sydwbh != dwbh)
                {
                    msg = "不能操作其他单位的委托单";
                    return ret;
                }

                string sql = string.Format("update m_by set bgck = 1 where recid='{0}'", recid);

                ret = CommonDao.ExecCommand(sql, CommandType.Text);

                if (!ret)
                    msg = "更新失败";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        #endregion

        #region 对外接口

        /// <summary>
        /// 根据报告唯一号获取委托单编号
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/6/6 13:19
        public string GetWtdbh(string bgwyh)
        {
            List<IDictionary<string, string>> dt =
                CommonDao.GetDataTable("select wtdbh from up_bgsj where bgewm='" + bgwyh + "'").ToList();
            if (dt.Count > 0)
            {
                return dt[0]["wtdbh"];
            }

            return "";
        }

        /// <summary>
        /// 获取最后一份加了二维码的报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string, byte[]> GetReportFiles(string wtdwyh, out string msg)
        {
            IDictionary<string, byte[]> ret = null;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select bgwyh,bgbh from up_bgsj where wtdbh='" + wtdwyh + "' and bgewm<>'' order by scsj desc");
                string where = string.Empty;

                if (dt.Count == 0)
                {
                    msg = "没有生成二维码的对应报告摘要";
                    return ret;
                }
                else
                {
                    //特殊情况,有多份份报告,3天和28天,7天和28天,7天和28天和90天
                    List<string> days = new List<string>();
                    days.Add("3天");
                    days.Add("7天");
                    days.Add("28天");
                    days.Add("90天");
                    IDictionary<string, string> dict = null;
                    var bgbh = dt[0]["bgbh"];

                    foreach (var day in days)
                    {
                        if (bgbh.IndexOf(day) != -1)
                        {
                            var tempDays = days.Where(x => x != day).ToList();

                            foreach (var tempDay in tempDays)
                            {
                                dict = dt.FirstOrDefault(x => x["bgbh"].IndexOf(tempDay) != -1);

                                if (dict != null)
                                {
                                    where += string.Format(" or a.bgwyh='{0}' ", dict["bgwyh"]);
                                }
                            }
                        }
                    }
                }

                string sql =
                    "select b.bgbh,a.bgwj,a.osscdnurl from up_bgwj a inner join up_bgsj b on a.BGWYH = b.BGWYH where a.bgwyh='" +
                    dt[0]["bgwyh"] + "'";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += where;
                }

                sql += " order by b.scsj desc ";

                IList<IDictionary<string, object>> dtf = CommonDao.GetBinaryDataTable(sql);
                if (dtf.Count == 0)
                {
                    msg = "没有生成二维码的对应报告文件";
                    return ret;
                }

                ret = new Dictionary<string, byte[]>();
                //目前先兼容数据库存二进制文件,转换完之后把bgwj字段备份后替换为空
                foreach (IDictionary<string, object> row in dtf)
                {
                    var ossCdnUrl = row["osscdnurl"].GetSafeString();

                    if (!string.IsNullOrEmpty(ossCdnUrl))
                    {
                        var fileReturn = OssCdnHelper.GetByOssCdnUrl(ossCdnUrl, "pdf");

                        if (fileReturn.Success)
                        {
                            if (!ret.ContainsKey(row["bgbh"].GetSafeString()))
                                ret.Add(row["bgbh"].GetSafeString(), fileReturn.FileBytes);
                        }
                        else
                        {
                            msg = fileReturn.ErrorMsg;
                            return ret;
                        }
                    }
                    else
                    {
                        if (!ret.ContainsKey(row["bgbh"].GetSafeString()))
                            ret.Add(row["bgbh"].GetSafeString(), row["bgwj"] as byte[]);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /////// <summary>
        /////// 获取最后一份加了二维码的报告
        /////// </summary>
        /////// <param name="wtdwyh"></param>
        /////// <param name="msg"></param>
        /////// <returns></returns>
        ////public IList<byte[]> GetReportFiles(string wtdwyh, out string msg)
        ////{
        ////    IList<byte[]> ret = null;
        ////    msg = "";
        ////    try
        ////    {
        ////        IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select bgwyh,bgbh from up_bgsj where wtdbh='" + wtdwyh + "' and bgewm<>'' order by scsj desc");
        ////        string secondBgwyh = string.Empty;

        ////        if (dt.Count == 0)
        ////        {
        ////            msg = "没有生成二维码的对应报告摘要";
        ////            return ret;
        ////        }
        ////        else
        ////        {
        ////            //特殊情况,有二份报告
        ////            string threeDay = "3天";
        ////            string twentyEightDay = "28天";
        ////            IDictionary<string, string> dict = null;

        ////            if (dt[0]["bgbh"].IndexOf(threeDay) != -1)
        ////            {
        ////                dict = dt.FirstOrDefault(x => x["bgbh"].IndexOf(twentyEightDay) != -1);
        ////            }
        ////            else if (dt[0]["bgbh"].IndexOf(twentyEightDay) != -1)
        ////            {
        ////                dict = dt.FirstOrDefault(x => x["bgbh"].IndexOf(threeDay) != -1);
        ////            }

        ////            if (dict != null)
        ////            {
        ////                secondBgwyh = dict["bgwyh"];
        ////            }
        ////        }

        ////        string sql = "select a.bgwj,a.osscdnurl from up_bgwj a inner join up_bgsj b on a.BGWYH = b.BGWYH where a.bgwyh='" + dt[0]["bgwyh"] + "'";

        ////        if (!string.IsNullOrEmpty(secondBgwyh))
        ////        {
        ////            sql += string.Format(" or a.bgwyh='{0}'", secondBgwyh);
        ////        }

        ////        sql += " order by b.scsj desc ";

        ////        IList<IDictionary<string, object>> dtf = CommonDao.GetBinaryDataTable(sql);
        ////        if (dtf.Count == 0)
        ////        {
        ////            msg = "没有生成二维码的对应报告文件";
        ////            return ret;
        ////        }
        ////        ret = new List<byte[]>();

        ////        //目前先兼容数据库存二进制文件,转换完之后把bgwj字段备份后替换为空
        ////        foreach (IDictionary<string, object> row in dtf)
        ////        {
        ////            var ossCdnUrl = row["osscdnurl"].GetSafeString();

        ////            if (!string.IsNullOrEmpty(ossCdnUrl))
        ////            {
        ////                var fileReturn = OssCdnHelper.GetByOssCdnUrl(ossCdnUrl, "pdf");

        ////                if (fileReturn.Success)
        ////                {
        ////                    ret.Add(fileReturn.FileBytes);
        ////                }
        ////                else
        ////                {
        ////                    msg = fileReturn.ErrorMsg;
        ////                    return ret;
        ////                }
        ////            }
        ////            else
        ////            {
        ////                ret.Add(row["bgwj"] as byte[]);
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        SysLog4.WriteLog(ex);
        ////        msg = ex.Message;
        ////        throw ex;
        ////    }
        ////    return ret;
        ////}

        /// <summary>
        /// 上传报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="bgbh"></param>
        /// <param name="syr"></param>
        /// <param name="shr"></param>
        /// <param name="qfr"></param>
        /// <param name="syrq"></param>
        /// <param name="qfrq"></param>
        /// <param name="jcjg"></param>
        /// <param name="jcjgms"></param>
        /// <param name="datajson"></param>
        /// <param name="pdfjson"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool UpReport(string dwbh, string wtdwyh, string bgbh, string syr, string shr, string qfr,
            DateTime syrq, DateTime qfrq, int jcjg, string jcjgms, string mdatajson, string sdatajson, string pdfjson,
            bool setBarcode, bool sdsc, string datajson,
            out string msg)
        {
            bool ret = false;
            msg = "";
            string repeatField = "";
            string barcode = "";
            StringBuilder logs = new StringBuilder();

            try
            {
                lock (this)
                {
                    string sql = "select * from m_by where recid='" + wtdwyh + "' and dbo.IsValidWtd(zt)=1";
                    if (sdsc)
                        sql = "select * from m_by where recid='" + wtdwyh + "'";
                    IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                    if (dt.Count == 0)
                    {
                        msg = "找不到已送样的委托单记录";
                        return ret;
                    }

                    if (dt.Count() > 1)
                    {
                        msg = "委托单异常，有多份记录";
                        return ret;
                    }

                    IDictionary<string, string> wtd = dt[0];
                    string sydwbh = wtd["sydwbh"];
                    if (sydwbh != dwbh && sydwbh != "")
                    {
                        msg = "非本单位送样试验，无法上传报告";
                        return ret;
                    }

                    if (!setBarcode)
                    {
                        IList<IDictionary<string, string>> dttmp =
                            CommonDao.GetDataTable("select hqewm from i_m_qy where qybh='" + dwbh + "'");
                        if (dttmp.Count == 0)
                            setBarcode = true;
                        else
                        {
                            int hqewm = dttmp[0]["hqewm"].GetSafeInt();
                            if (hqewm == 0)
                                setBarcode = true;
                        }
                    }

                    // 二维码条件
                    BarcodeOptionReport barcodeOption = GetBarCodeOption(dwbh, wtd["syxmbh"]);

                    // 报告明细json
                    //var mdataReplaceJson = mdatajson.Replace("\\\\\"", "");

                    JsonDeSerializer<VTransUpBgxqm[]> bgxqmDe = new JsonDeSerializer<VTransUpBgxqm[]>();
                    VTransUpBgxqm[] arrBgxqm = bgxqmDe.DeSerializer(mdatajson, out msg);
                    if (msg != "")
                    {
                        SysLog4.WriteError("上传报告:委托单唯一主键[" + wtdwyh + "],单位编号[" + dwbh + "],报告详情主表转json失败");
                        msg = "报告详情主表转json失败，详细内容：" + msg + "。源数据：" + mdatajson;
                        return ret;
                    }

                    //var sdataReplaceJson = sdatajson.Replace("\\\\\"", "");

                    JsonDeSerializer<VTransUpBgxqs[]> bgxqsDe = new JsonDeSerializer<VTransUpBgxqs[]>();
                    VTransUpBgxqs[] arrBgxqs = bgxqsDe.DeSerializer(sdatajson, out msg);
                    if (msg != "")
                    {
                        SysLog4.WriteError("上传报告:委托单唯一主键[" + wtdwyh + "],单位编号[" + dwbh + "],报告详情从表转json失败");
                        msg = "报告详情从表转json失败，详细内容：" + msg + "。源数据：" + sdatajson;
                        return ret;
                    }

                    // 报告pdf json
                    JsonDeSerializer<VTransUpBgwj[]> bgwjDe = new JsonDeSerializer<VTransUpBgwj[]>();
                    VTransUpBgwj[] arrBgwj = bgwjDe.DeSerializer(pdfjson, out msg);
                    if (msg != "")
                    {
                        msg = "报告文件转json失败，详细内容：" + msg;
                        return ret;
                    }

                    //判断报告pdf是否有 2019-07-15 tmx
                    if (arrBgwj.Count() == 0)
                    {
                        msg = "没有上传任何报告文件";
                        return ret;
                    }

                    // 判断重复
                    var orderBgxqm = arrBgxqm.OrderBy(e => e.zdhy);
                    StringBuilder sbMd5Src = new StringBuilder();
                    foreach (VTransUpBgxqm item in orderBgxqm)
                        sbMd5Src.Append(item.zdz.GetSafeString());
                    var orderBgxqs = arrBgxqs.OrderBy(e => e.zh);
                    orderBgxqs = orderBgxqs.OrderBy(e => e.zdhy);
                    foreach (VTransUpBgxqs item in orderBgxqs)
                        sbMd5Src.Append(item.zdz.GetSafeString());

                    string md5 = MD5Util.StringToMD5Hash(sbMd5Src.ToString());

                    IList<IDictionary<string, string>> dtExists =
                        CommonDao.GetDataTable("select * from up_bgsj where wtdbh='" + wtdwyh + "' and bgxq_md5='" +
                                               md5 + "'");
                    if (dtExists.Count > 0 && !sdsc)
                    {
                        msg = "报告已上传，不能上传重复报告";
                        return ret;
                    }

                    // 获取试验人，审核人，签发人
                    //dt = CommonDao.GetDataTable("select rybh,ryxm from i_m_ry where jcrjzh in (" + (syr + "," + shr + "," + qfr).FormatSQLInStr() + ")");
                    string syrxm = syr, shrxm = shr, qfrxm = qfr;

                    // 更新委托单信息
                    string bgewm = dt[0]["bgwyxbh"].GetSafeString();
                    string appendUpdate = "";
                    //判断二维码服务类型
                    IBaseService baseService = ServiceManager.GetBaseService();
                    IDictionary<string, string> qrDic = null;
                    //标点服务
                    if(baseService is BDService)
                    { 
                        if (bgewm == "")
                        {
                            //通过服务接口获取二维码信息       
                            ResultParam qrRet = baseService.GetQrCode(qrDic);
                            if (!qrRet.success)
                                throw new Exception(qrRet.msg);
                            //获取返回二维码
                            bgewm = qrRet.data.ToString();
                            appendUpdate = ",bgwyxbh='" + bgewm + "'";
                        }
                    }
                    //如果是萧山协会接口
                    else if (baseService is XsXhService)
                    {
                        //参数初始化
                        qrDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        // **** 获取机构信息 ****
                        IList<IDictionary<string, string>> xhDt = CommonDao.GetDataTable(String.Format("select * from SysYcjkQy where QYBH='{0}'", dwbh));
                        if (xhDt.Count == 0)
                        {
                            //企业信息配置不存在
                            throw new Exception("检测机构远程参数未配置！");
                        }
                        //机构ID
                        string jgid = xhDt[0]["JGID"].GetSafeString();
                        //区域代码
                        string jgqy = xhDt[0]["JGQY"].GetSafeString();
                        //获取试验项目编号
                        xhDt = CommonDao.GetDataTable(String.Format("select syxmbh from m_by where sydwbh = '{0}' and recid='{1}'", dwbh, wtdwyh));
                        if (xhDt.Count == 0)
                        {
                            throw new Exception(String.Format("检测机构：{0}，委托单唯一号：{1}不存在！",dwbh, wtdwyh));
                        }
                        string syxmbh = xhDt[0]["syxmbh"];
                        //判断二维码是否已经使用过
                        string type = "";
                        string code = "";
                        //根据试验项目获取是非两块两材还是两块两材
                        xhDt = CommonDao.GetDataTable(String.Format("select lx,code from SysYcjk_Xsxh_SyxmbhCode where syxmbh = '{0}'", syxmbh));
                        if (xhDt.Count == 0)
                        {
                            throw new Exception(String.Format("试验项目：{0}对应协会两材类型未设置！", dwbh, wtdwyh));
                        }
                        type = xhDt[0]["lx"];
                        code = xhDt[0]["code"];
                        //判断二维码是否生成过
                        xhDt = CommonDao.GetDataTable(String.Format("select * from SysYcjk_Xsxh_Qrcode where SFQY = '0' and QYBH = '{0}' and SYXMBH = '{1}' and BGBH = '{2}'", dwbh, syxmbh, bgbh));
                        if (xhDt.Count == 0)
                        {                         
                            //生成二维码
                            qrDic.Add("jgid", jgid);
                            qrDic.Add("bgbh", bgbh);
                            qrDic.Add("type", type);
                            //通过服务接口获取二维码信息       
                            ResultParam qrRet = baseService.GetQrCode(qrDic);
                            if (!qrRet.success)
                                throw new Exception(qrRet.msg);
                            //获取返回二维码
                            bgewm = qrRet.data.ToString();
                            sql = String.Format("insert into SysYcjk_Xsxh_Qrcode(QYBH,SYXMBH,BGBH,QRCODE,SFQY) values('{0}','{1}','{2}','{3}','{4}')", dwbh, syxmbh, bgbh, bgewm, "0");
                            CommonDao.ExecCommandOpenSession(sql, CommandType.Text);
                        }
                        else
                        {
                            bgewm = xhDt[0]["QRCODE"];     
                        }
                        //上传协会数据记录
                        #region 上传协会记录
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        IDictionary<string, object> dataJsonDic = js.DeserializeObject(datajson) as Dictionary<string, object>;
                        //获取公共信息
                        object basicObj = null;
                        //获取样品信息
                        object sampleObj = null;
                        // 非两块两材
                        if(type == "0")
                        {
                            //获取公共信息
                            basicObj = dataJsonDic["basic"];
                        }
                        else if(type == "1")
                        {
                            //获取公共信息
                            basicObj = dataJsonDic["basic"];
                            //获取样品信息
                            sampleObj = dataJsonDic["sample"];
                        }
                        else
                        {
                            throw new Exception(String.Format("两材的类型【{0}】不存在！", type));
                        }
                        //获取公共信息
                        IDictionary<string, object> basicDic = (Dictionary<string, object>)basicObj;
                        StringBuilder basicXml = new StringBuilder();
                        basicXml.Append("<xml>");
                        //遍历
                        foreach (KeyValuePair<string, object> kvp in basicDic)
                        {
                            basicXml.AppendFormat("<{0}>{1}</{0}>", kvp.Key, kvp.Value);
                        }
                        basicXml.Append("</xml>");

                        //获取样品信息
                        StringBuilder pyXml = new StringBuilder();
                        if(type == "1")
                        { 
                            object[] sampleObjList = (object[])sampleObj;
                            IDictionary<string, object> sampleDic;
                            
                            pyXml.Append("<xml>");
                            foreach (object item in sampleObjList)
                            {
                                pyXml.Append("<YP>");
                                sampleDic = (Dictionary<string, object>)item;
                                //遍历
                                foreach (KeyValuePair<string, object> kvp in sampleDic)
                                {
                                    pyXml.AppendFormat("<{0}>{1}</{0}>", kvp.Key, kvp.Value);
                                }
                                pyXml.Append("</YP>");
                            }
                            pyXml.Append("</xml>");
                        }
                        //上传协会
                        //参数
                        IDictionary<string, string> dataDic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        //机构ID
                        dataDic.Add("jgid", jgid);
                        //机构区域
                        dataDic.Add("jgqy", jgqy);
                        //共用信息
                        dataDic.Add("basic", basicXml.ToString());
                        //返回结果
                        ResultParam uploadRet;
                        //非两块两材
                        if (type == "0")
                        {
                            uploadRet = baseService.UploadFeiLiangKuai(dataDic);
                        }
                        //两块两材
                        else
                        {
                            //样品信息
                            dataDic.Add("sample", pyXml.ToString());
                            //代号
                            dataDic.Add("code", code);
                            uploadRet = baseService.UploadLiangKuai(dataDic);
                        }
                        if (!uploadRet.success)
                            throw new Exception(uploadRet.msg);
                        //更新两维码状态为已使用
                        sql = String.Format("update SysYcjk_Xsxh_Qrcode set SFQY = '1' where QYBH = '{0}' and SYXMBH = '{1}' and BGBH = '{2}' and QRCODE = '{3}'", dwbh, syxmbh, bgbh, bgewm);
                        CommonDao.ExecCommandOpenSession(sql, CommandType.Text);
                        #endregion
                        appendUpdate = ",bgwyxbh='" + bgewm + "'";
                    }

                    // 添加报告文件 提前上传文件，防止表阻塞
                    string bgwyh = Guid.NewGuid().ToString();

                    // 是否开启现场监管检测，开启了则需要填写检测日期
                    var isXcjgjc = IsXcJgjc(dwbh, wtdwyh);

                    List<Dictionary<string, object>> bgwjs = new List<Dictionary<string, object>>();
                    int index = 1;
                    int sysFileStorage = GetSysFileStorage();
                    foreach (var bgwj in arrBgwj)
                    {
                        Dictionary<string, object> bgwjDict = new Dictionary<string, object>();
                        bgwjDict.Add("sxh", index);
                        byte[] pdffile = bgwj.bgwj.DecodeBase64Array();
                        byte[] barimage = Barcode.GetBarcode2NoWhite(bgewm, barcodeOption.Width, barcodeOption.Width);

                        if (setBarcode && isXcjgjc)
                            pdffile = PdfWaterMark.SetWaterMark(pdffile, barimage, barcodeOption.PageModule,
                                barcodeOption.PositionModule, barcodeOption.HSpan, barcodeOption.VSpan);

                        //文件存储方式判断
                        if (sysFileStorage == (int)SysFileStorageEnum.SqlData)
                        {
                            bgwjDict.Add("bgwj", pdffile);
                            bgwjDict.Add("osscdnurl", string.Empty);
                            bgwjDict.Add("isupload", 0);
                        }
                        else
                        {
                            OSS_CDN oss = new OSS_CDN(Configs.FileOssCdn);
                            var result = oss.UploadFile(Configs.OssCdnCodeBg, pdffile, string.Format("bg_{0}_{1}.pdf", bgwyh, index));

                            if (result.success)
                            {
                                bgwjDict.Add("bgwj", new byte[] { 0x01 });
                                bgwjDict.Add("osscdnurl", result.Url);
                                bgwjDict.Add("isupload", 1);
                            }
                            else
                            {
                                SysLog4.WriteError(String.Format("上传到OSS出错！原因：{0}", result.message));
                                throw new Exception("上传到OSS出错！");
                            }
                        }

                        bgwjs.Add(bgwjDict);
                        index++;
                    }

                    // 添加报告记录主表
                    //SysLog4.WriteLog("----------------------------------唯一号：" + bgwyh + ",md5：" + md5 + "，源字符串：" +
                    //                 sbMd5Src.ToString());

                    barcode = bgewm;
                    sql =
                        "insert into up_bgsj([BGWYH],[WTDBH],[BGBH],[SYR],[SHR],[QFR],[QFRQ],[JCJG],[JCJGMS],[BGEWM],[SYRQ],[SCSJ],[SYRXM],[SHRXM],[QFRXM],[BGXQ_MD5],[SDSC]) values('" +
                        bgwyh + "','" + wtdwyh + "','" + bgbh + "','" + syr + "','" + shr + "','" +
                        qfr + "',convert(datetime,'" + qfrq.ToString("yyyy-MM-dd HH:mm:ss") + "')," + jcjg + ",'" +
                        jcjgms + "','" + bgewm + "',convert(datetime,'" +
                        syrq.ToString("yyyy-MM-dd HH:mm:ss") + "'),getdate(),'" + syrxm + "','" + shrxm + "','" +
                        qfrxm + "','" + md5 + "'," + (sdsc ? 1 : 0) + ")";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                    // 添加报告详情
                    // 根据ZDHY重复去除记录
                    List<VTransUpBgxqm> arrBgxqmTemp = new List<VTransUpBgxqm>();

                    foreach (var bgxqm in arrBgxqm)
                    {
                        var findIndex = arrBgxqmTemp.FindIndex(x => x.zdhy.GetSafeDbValue() == bgxqm.zdhy.GetSafeDbValue());

                        if (findIndex == -1)
                        {
                            arrBgxqmTemp.Add(bgxqm);
                        }
                    }

                    foreach (var bgxqm in arrBgxqmTemp)
                    {
                        repeatField = bgxqm.zdhy + ":" + bgxqm.zdz;
                        CommonDao.ExecCommand(
                            "insert into up_bgxqm(bgwyh,zdhy,zdz,ysz,bjjg) values('" + bgwyh + "','" +
                            bgxqm.zdhy.GetSafeDbValue() + "','" + bgxqm.zdz.GetSafeDbValue() + "','',0)",
                            CommandType.Text);
                    }

                    //根据ZH,ZDHY重复去除记录
                    List<VTransUpBgxqs> arrBgxqsTemp = new List<VTransUpBgxqs>();

                    foreach (var bgxq in arrBgxqs)
                    {
                        var findIndex = arrBgxqsTemp.FindIndex(x => x.zh == bgxq.zh 
                            && x.zdhy.GetSafeDbValue() == bgxq.zdhy.GetSafeDbValue());

                        if (findIndex == -1)
                        {
                            arrBgxqsTemp.Add(bgxq);
                        }
                    }

                    foreach (var bgxq in arrBgxqsTemp)
                    {
                        repeatField = bgxq.zdhy + ":" + bgxq.zdz;
                        CommonDao.ExecCommand(
                            "insert into up_bgxqs(bgwyh,zh,zdhy,zdz,ysz,bjjg) values('" + bgwyh + "','" + bgxq.zh +
                            "','" + bgxq.zdhy.GetSafeDbValue() + "','" + bgxq.zdz.GetSafeDbValue() + "','',0)",
                            CommandType.Text);
                    }

                    // 添加报告文件
                    foreach (var bgwj in bgwjs)
                    {
                        sql = "insert into up_bgwj(bgwyh,sxh,bgwj,ossCdnUrl,isUpload) values(@bgwyh,@sxh,@bgwj,@ossCdnUrl,@isUpload)";
                        IList<IDataParameter> arrParams = new List<IDataParameter>();
                        arrParams.Add(new SqlParameter("@bgwyh", bgwyh));
                        arrParams.Add(new SqlParameter("@sxh", bgwj["sxh"].GetSafeInt()));
                        arrParams.Add(new SqlParameter("@bgwj", SqlDbType.VarBinary) { Value = bgwj["bgwj"] });
                        arrParams.Add(new SqlParameter("@ossCdnUrl", bgwj["osscdnurl"].GetSafeString()));
                        arrParams.Add(new SqlParameter("@isUpload", bgwj["isupload"].GetSafeInt()));
                        //logs.Append(sql + "\r\n" + bgwyh + "," + bgwj["sxh"].GetSafeInt() + "\r\n");
                        CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                    }

                    string extrafields = "";
                    if (string.IsNullOrEmpty(sydwbh))
                    {
                        var sydwmc = CommonDao.GetSingleData(string.Format("select top 1 qymc from i_m_qy where qybh = '{0}'", dwbh)).GetSafeString();
                        extrafields += "sydwbh='" + dwbh + "', sydwmc = '" + sydwmc + "',";
                    }
                    sql = "update m_by set " + extrafields + "ZT='" + WtsStatus.MainStateBg +
                          "'+substring(zt,2,len(zt)-1),SYRZH='" + syr + "',SYRXM='" + syrxm +
                          "',SYKSSJ=convert(datetime,'" + syrq.ToString("yyyy-MM-dd HH:mm:ss") + "'),BGSHRZH='" + shr +
                          "',BGSHRXM='" + shrxm + "',BGQFRZH='" + qfr + "',BGQFRXM='" + qfrxm +
                          "',BGQFWCSJ=convert(datetime,'" + qfrq.ToString("yyyy-MM-dd HH:mm:ss") + "')" + appendUpdate +
                          " where recid='" + wtdwyh + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    ret = true;

                    //ret = SetWtdycztIn(wtdwyh, out msg);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteError("插入文件sql：" + logs.ToString());
                SysLog4.WriteLog("最后字段：" + repeatField, ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 是否开启现场监管检测，开启了则需要填写检测日期
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        private bool IsXcJgjc(string qybh, string wtdwyh)
        {
            var sql = string.Format("select xcjgjc from i_m_qy where qybh = '{0}'", qybh);
            var xcjgjc = CommonDao.GetSingleData(sql).GetSafeBool();

            if (xcjgjc)
            {
                sql = string.Format(@"select a.recid, c.jcrq
                                        from m_by a 
                                        inner join PR_M_SYXM b ON a.SYXMBH = b.SYXMBH AND a.YTDWBH = b.SSDWBH AND b.SFYX = 1 AND b.XCXM = 1
                                        left join I_S_Xcjgjc c on a.recid = c.WtdWyh
                                        where a.recid = '{0}'", wtdwyh);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    if (string.IsNullOrEmpty(dt[0]["jcrq"]))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 上传试验数据
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="syxmmc"></param>
        /// <param name="ypbh"></param>
        /// <param name="zh"></param>
        /// <param name="syr"></param>
        /// <param name="sysb"></param>
        /// <param name="sykssj"></param>
        /// <param name="syjssj"></param>
        /// <param name="syqx"></param>
        /// <param name="datajson"></param>
        /// <param name="czdatajson"></param>
        /// <param name="sfbc"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool UpData(string dwbh, string wtdwyh, string syxmmc, string ypbh, string zh, string syr, string sysb,
            DateTime sykssj, DateTime syjssj, string syqx, string videofiles, string recordfiles,
            string datajson, string czdatajson, bool sfbc, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select * from m_by where recid='" + wtdwyh + "' and dbo.IsValidWtd(zt)=1");
                if (dt.Count == 0)
                {
                    msg = "找不到已送样的委托单记录";
                    return ret;
                }

                if (dt.Count() > 1)
                {
                    msg = "委托单异常，有多份记录";
                    return ret;
                }

                IDictionary<string, string> wtd = dt[0];
                if (wtd["sydwbh"] != dwbh)
                {
                    msg = "非本单位送样试验，无法上传数据";
                    return ret;
                }

                string md5 = MD5Util.StringToMD5Hash(datajson);
                IList<IDictionary<string, string>> dtExists = CommonDao.GetDataTable(
                    "select * from up_sysj where wtdbh='" + wtdwyh + "' and zh='" + zh + "' and syxq_md5='" + md5 +
                    "'");
                // 因试验员失误,有需要重复试验情况
                //if (dtExists.Count > 0)
                //{
                //    msg = "数据已上传，不能上传重复数据";
                //    return ret;
                //}

                // 采集明细json
                JsonDeSerializer<VTransUpSyxq[]> syxqDe = new JsonDeSerializer<VTransUpSyxq[]>();
                VTransUpSyxq[] arrSyxq = syxqDe.DeSerializer(datajson, out msg);
                if (msg != "")
                {
                    msg = "试验详情转json失败，详细内容：" + msg;
                    return ret;
                }

                // 重做明细 json
                VTransUpCzsj[] arrCzsj = null;
                if (czdatajson != "")
                {
                    JsonDeSerializer<VTransUpCzsj[]> czwjDe = new JsonDeSerializer<VTransUpCzsj[]>();
                    VTransUpCzsj[] arrCzwj = czwjDe.DeSerializer(czdatajson, out msg);
                    if (msg != "")
                    {
                        msg = "重做记录转json失败，详细内容：" + msg;
                        return ret;
                    }
                }

                bool hasCz = arrCzsj != null && arrCzsj.Length > 0;
                // 曲线
                byte[] imageQx = null;
                if (syqx != "")
                    imageQx = syqx.DecodeBase64Array();
                bool hasQx = imageQx != null;

                // 获取试验人，审核人，签发人
                //dt = CommonDao.GetDataTable("select rybh,ryxm from i_m_ry where jcrjzh in (" + (syr + "," + shr + "," + qfr).FormatSQLInStr() + ")");
                string syrxm = syr;

                // 更新委托单信息
                string sql = "update m_by set SYRZH='" + syr + "',SYRXM='" + syrxm + "',SYKSSJ=convert(datetime,'" +
                             syjssj.ToString("yyyy-MM-dd HH:mm:ss") + "'),SYJSSJ='" +
                             syjssj.ToString("yyyy-MM-dd HH:mm:ss") + "' where recid='" + wtdwyh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                sql = "update m_by set ZT='" + WtsStatus.MainStateSy + "'+substring(zt,2,len(zt)-1) where recid='" +
                      wtdwyh + "' and zt like '" + WtsStatus.MainStateWt + "%'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                // 添加采集记录主表
                // 文件名存在时，才加.flv
                if (videofiles.IndexOf(".") == -1 && videofiles.GetSafeString() != "")
                    videofiles += ".flv";
                if (recordfiles.IndexOf(".") == -1 && videofiles.GetSafeString() != "")
                    recordfiles += ".flv";
                string sywyh = Guid.NewGuid().ToString();
                sql =
                    "INSERT INTO UP_SYSJ([SYWYH],[WTDBH],[SYMC],[YPBH],[ZH],[SYR],[SYRXM],[SYSB],[SYKSSJ],[SYJSSJ],[SFBC],[SFCZ],[SFZZSJ],[SFYQX],[SCSJ],[SPWJ],[LPWJ],[SYXQ_MD5]) values('" +
                    sywyh + "','" + wtdwyh + "','" + syxmmc + "','" + ypbh + "','" + zh + "','" +
                    syr + "','" + syrxm + "','" + sysb + "',convert(datetime,'" +
                    sykssj.ToString("yyyy-MM-dd HH:mm:ss") + "'),convert(datetime,'" +
                    syjssj.ToString("yyyy-MM-dd HH:mm:ss") + "')," +
                    (sfbc ? 1 : 0) + "," + (hasCz ? 1 : 0) + ",1," + (hasQx ? 1 : 0) + ",getdate(), '" + videofiles +
                    "','" + recordfiles + "','" + md5 + "')";
                CommonDao.ExecCommand(sql, CommandType.Text);
                // 添加采集详情
                foreach (var syxq in arrSyxq)
                {
                    if (GlobalVariableConfig.GLOBAL_INTERFACE_CNEN == "EN")
                    {
                        //判断是否是其他厂家上传
                        if (syxq.zdmc == null || syxq.zdmc == "")
                            sql = "insert into up_syxq(sywyh,zdhy,zdz,bgz,bjjg) values('" + sywyh + "','" + syxq.zdhy +
                                  "','" + syxq.zdz + "','',0)";
                        else
                            sql = "insert into up_syxq(sywyh,zdhy,zdz,bgz,bjjg,zdsy) values('" + sywyh + "','" + syxq.zdmc +
                                  "','" + syxq.zdz + "','',0,'" + syxq.zdhy + "')";
                    }
                    else
                        sql = "insert into up_syxq(sywyh,zdhy,zdz,bgz,bjjg) values('" + sywyh + "','" + syxq.zdhy +
                              "','" + syxq.zdz + "','',0)";
                    CommonDao.ExecCommand(sql
                        , CommandType.Text);
                }

                // 添加曲线
                IList<IDataParameter> arrParams = new List<IDataParameter>();
                SqlParameter param = new SqlParameter("@qxtp", SqlDbType.VarBinary) {Value = imageQx};
                arrParams.Add(param);
                CommonDao.ExecCommand("insert into up_syqx(sywyh,sxh,qxtp) values('" + sywyh + "'," + 1 + ",@qxtp)",
                    CommandType.Text, arrParams);
                // 添加重装记录
                if (arrCzsj != null)
                {
                    foreach (var czxq in arrCzsj)
                    {
                        string czwyh = new Guid().ToString();
                        sql =
                            "insert into up_czsj([CZWYH],[SYWYH],[WTDBH],[SYMC],[YPBH],[ZH],[SYR],[SYRXM],[PZR],[PZRXM],[SYSB],[SYSJ],[JH],[CZYY],[SCSJ]) values('" +
                            czwyh + "','" + sywyh + "','" + wtdwyh + "','" + syxmmc + "','" + ypbh + "','" + zh +
                            "','" + syr + "','" + syrxm + "','" +
                            czxq.pzr + "','" + czxq.pzr + "','" + czxq.sysb + "',convert(datetime,'" +
                            sykssj.ToString("yyyy-MM-dd HH:mm:ss") + "'),'" + czxq.jh + "','" +
                            czxq.czyy + "',getdate())";
                        CommonDao.ExecCommand(sql, CommandType.Text);

                        byte[] cztp = null;
                        if (czxq.syqx != "")
                            cztp = czxq.syqx.DecodeBase64Array();
                        sql = "insert into up_czqx(czwyh,qxtp) values('" + czwyh + "',@qxtp)";
                        arrParams.Clear();
                        param = new SqlParameter("@qxtp", SqlDbType.VarBinary) {Value = cztp};
                        arrParams.Add(param);
                        CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                    }
                }

                ret = true;
                //ret = SetWtdycztIn(wtdwyh, out msg);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 工程信息查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetGcs(string jcjgbh, string stationid, VTransDownGetGc where, int pagesize, int pageindex,
            out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            if (pagesize <= 0)
                pagesize = 20;
            if (pageindex <= 0)
                pageindex = 1;
            try
            {
                // 获取zdzd 
                IList<IDictionary<string, string>> zdzds = CommonDao.GetDataTable(
                    "select sjbmc,zdmc,sy,zdlx from zdzd_jc where sjbmc in ('i_m_gc','I_S_GC_FGC','I_S_GC_JLDW','I_S_GC_JLRY','I_S_GC_JZRY','I_S_GC_SGDW','I_S_GC_SGRY','I_S_GC_SJDW','I_S_GC_SYRY','I_S_GC_JSDW','I_S_GC_JSRY','I_S_GC_KCDW','I_S_GC_KCRY') ");
                // 查询条件
                StringBuilder sbWhere = new StringBuilder();
                if (where.gcbh.GetSafeRequest() != "")
                    sbWhere.Append(" and a.gcbh like '%" + where.gcbh.GetSafeRequest() + "%' ");
                if (where.gcmc.GetSafeRequest() != "")
                    sbWhere.Append(" and a.gcmc like '%" + where.gcmc.GetSafeRequest() + "%' ");
                if (where.gcqy.GetSafeRequest() != "")
                    sbWhere.Append(" and a.qymc like '%" + where.gcqy.GetSafeRequest() + "%' ");
                if (where.gclx.GetSafeRequest() != "")
                    sbWhere.Append(" and a.gclxmc like '%" + where.gclx.GetSafeRequest() + "%' ");
                if (where.jsdw.GetSafeRequest() != "")
                    sbWhere.Append(" and a.jsdwmc like '%" + where.jsdw.GetSafeRequest() + "%' ");
                if (where.sgdw.GetSafeRequest() != "")
                    sbWhere.Append(" and a.sgdwmc like '%" + where.sgdw.GetSafeRequest() + "%' ");
                if (where.jldw.GetSafeRequest() != "")
                    sbWhere.Append(" and a.jldwmc like '%" + where.jldw.GetSafeRequest() + "%' ");
                if (where.jzry.GetSafeRequest() != "")
                    sbWhere.Append(" and a.jzryxm like '%" + where.jzry.GetSafeRequest() + "%' ");
                if (where.syry.GetSafeRequest() != "")
                    sbWhere.Append(" and a.syryxm like '%" + where.syry.GetSafeRequest() + "%' ");
                // 工程信息
                string sql = "select * from i_m_gc where recid in (select recid from view_i_m_gc a where 1=1 " +
                             sbWhere.ToString() + ") ";
                if (!string.IsNullOrEmpty(jcjgbh))
                    sql += " and ssjcjgbh='" + jcjgbh + "' ";
                if (!string.IsNullOrEmpty(stationid))
                    sql += " and zjzbh in (select extrainfo1 from i_m_call where stationid='" + stationid +
                           "') and (sjgcbh is null or sjgcbh='') ";
                sql += "order by recid desc";
                IList<IDictionary<string, string>> tbImgc =
                    CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);
                // 获取查询的工程编号
                StringBuilder sbGcbhs = new StringBuilder();
                foreach (IDictionary<string, string> row in tbImgc)
                    sbGcbhs.Append(row["gcbh"] + ",");
                string gcbhs = sbGcbhs.ToString().FormatSQLInStr();
                // 分工程信息
                IList<IDictionary<string, string>> tbIsgcfgc =
                    CommonDao.GetDataTable("select * from i_s_gc_fgc where gcbh in (" + gcbhs + ")");
                // 勘察单位
                IList<IDictionary<string, string>> tbIsgckcdw =
                    CommonDao.GetDataTable("select * from I_S_GC_KCDW where gcbh in (" + gcbhs + ")");
                // 勘察人员
                IList<IDictionary<string, string>> tbIsgckcry =
                    CommonDao.GetDataTable("select * from I_S_GC_KCRY where gcbh in (" + gcbhs + ")");
                // 建设单位
                IList<IDictionary<string, string>> tbIsgcjsdw =
                    CommonDao.GetDataTable("select * from I_S_GC_JSDW where gcbh in (" + gcbhs + ")");
                // 建设人员
                IList<IDictionary<string, string>> tbIsgcjsry =
                    CommonDao.GetDataTable("select * from I_S_GC_JSRY where gcbh in (" + gcbhs + ")");
                // 监理单位
                IList<IDictionary<string, string>> tbIsgcjldw =
                    CommonDao.GetDataTable("select * from I_S_GC_JLDW where gcbh in (" + gcbhs + ")");
                // 监理人员
                IList<IDictionary<string, string>> tbIsgcjlry =
                    CommonDao.GetDataTable("select * from I_S_GC_JLRY where gcbh in (" + gcbhs + ")");
                // 施工单位
                IList<IDictionary<string, string>> tbIsgcsgdw =
                    CommonDao.GetDataTable("select * from I_S_GC_SGDW where gcbh in (" + gcbhs + ")");
                // 施工人员
                IList<IDictionary<string, string>> tbIsgcsgry =
                    CommonDao.GetDataTable("select * from I_S_GC_SGRY where gcbh in (" + gcbhs + ")");
                // 设计单位
                IList<IDictionary<string, string>> tbIsgcsjdw =
                    CommonDao.GetDataTable("select * from I_S_GC_SJDW where gcbh in (" + gcbhs + ")");
                // 送样人员
                IList<IDictionary<string, string>> tbIsgcsyry =
                    CommonDao.GetDataTable("select * from I_S_GC_SYRY where gcbh in (" + gcbhs + ")");
                // 见证人员
                IList<IDictionary<string, string>> tbIsgcjzry =
                    CommonDao.GetDataTable("select * from I_S_GC_JZRY where gcbh in (" + gcbhs + ")");
                // 质监站
                IList<IDictionary<string, string>> tbHzjz =
                    CommonDao.GetDataTable("select * from H_ZJZ");
                // 匹配对应关系
                foreach (IDictionary<string, string> mrow in tbImgc)
                {
                    string gcbh = mrow["gcbh"];
                    // 工程信息
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    // ****************** 通过工程中质监站编号获取质监站名称 **********************
                    // 工程字典值
                    IDictionary<string, string> dicIgc = FieldNameToDesc("i_m_gc", mrow, zdzds);
                    // 获取工程中质监站对应名称
                    var zjz = from e in tbHzjz where e["zjzbh"].Equals(mrow["zjzbh"]) select e;
                    IList<IDictionary<string, string>> zjzList = zjz.ToList<IDictionary<string, string>>();
                    string zjzmc = zjzList.Count > 0 ? zjzList[0]["zjzmc"] : "";
                    dicIgc.Add("所属质监站名称", zjzmc);
                    // ****************************************************************************
                    compexRow.Add("工程信息", dicIgc);
                    // 分工程
                    var q = from e in tbIsgcfgc where e["gcbh"].Equals(gcbh) select e;
                    compexRow.Add("分工程", FieldNameToDesc("i_s_gc_fgc", q.ToList<IDictionary<string, string>>(), zdzds));
                    // 勘察单位
                    IList<IDictionary<string, object>> kcdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgckcdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> kcdwRow = new Dictionary<string, object>();
                        kcdwRow.Add("单位", FieldNameToDesc("I_S_GC_KCDW", row, zdzds));
                        q = from e in tbIsgcjsry where e["qybh"].Equals(row["qybh"]) select e;
                        kcdwRow.Add("人员",
                            FieldNameToDesc("I_S_GC_KCRY", q.ToList<IDictionary<string, string>>(), zdzds));
                        kcdws.Add(kcdwRow);
                    }
                    compexRow.Add("勘察单位", kcdws);
                    // 建设单位
                    IList<IDictionary<string, object>> jsdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcjsdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> jsdwRow = new Dictionary<string, object>();
                        jsdwRow.Add("单位", FieldNameToDesc("I_S_GC_JSDW", row, zdzds));
                        q = from e in tbIsgcjsry where e["qybh"].Equals(row["qybh"]) select e;
                        jsdwRow.Add("人员",
                            FieldNameToDesc("I_S_GC_JSRY", q.ToList<IDictionary<string, string>>(), zdzds));
                        jsdws.Add(jsdwRow);
                    }

                    compexRow.Add("建设单位", jsdws);
                    // 监理单位
                    IList<IDictionary<string, object>> jldws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcjldw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> jldwRow = new Dictionary<string, object>();
                        jldwRow.Add("单位", FieldNameToDesc("I_S_GC_JLDW", row, zdzds));
                        q = from e in tbIsgcjlry where e["qybh"].Equals(row["qybh"]) select e;
                        jldwRow.Add("人员",
                            FieldNameToDesc("I_S_GC_JLRY", q.ToList<IDictionary<string, string>>(), zdzds));
                        jldws.Add(jldwRow);
                    }

                    compexRow.Add("监理单位", jldws);
                    // 施工单位
                    IList<IDictionary<string, object>> sgdws = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, string> row in tbIsgcsgdw)
                    {
                        string tmpgcbh = row["gcbh"];
                        if (gcbh != tmpgcbh)
                            continue;
                        IDictionary<string, object> sgdwRow = new Dictionary<string, object>();
                        sgdwRow.Add("单位", FieldNameToDesc("I_S_GC_SGDW", row, zdzds));
                        q = from e in tbIsgcjlry where e["qybh"].Equals(row["qybh"]) select e;
                        sgdwRow.Add("人员",
                            FieldNameToDesc("I_S_GC_SGRY", q.ToList<IDictionary<string, string>>(), zdzds));
                        sgdws.Add(sgdwRow);
                    }

                    compexRow.Add("施工单位", sgdws);
                    // 设计单位
                    q = from e in tbIsgcsjdw where e["gcbh"].Equals(gcbh) select e;
                    compexRow.Add("设计单位",
                        FieldNameToDesc("I_S_GC_SJDW", q.ToList<IDictionary<string, string>>(), zdzds));
                    // 送样人员
                    q = from e in tbIsgcsyry where e["gcbh"].Equals(gcbh) select e;
                    compexRow.Add("送样人员",
                        FieldNameToDesc("I_S_GC_SYRY", q.ToList<IDictionary<string, string>>(), zdzds));
                    // 见证人员
                    q = from e in tbIsgcjzry where e["gcbh"].Equals(gcbh) select e;
                    compexRow.Add("见证人员",
                        FieldNameToDesc("I_S_GC_JZRY", q.ToList<IDictionary<string, string>>(), zdzds));

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }


        /// <summary>
        /// 上传变更单
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="bgyy"></param>
        /// <param name="bgsj"></param>
        /// <param name="where"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool UpBgd(string dwbh, string wtdwyh, string bgyy, DateTime bgsj, VTransUpBgd[] bgds, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select * from m_by where recid='" + wtdwyh + "' and dbo.IsValidWtd(zt)=1");
                if (dt.Count == 0)
                {
                    msg = "找不到已送样的委托单记录";
                    return ret;
                }

                if (dt.Count() > 1)
                {
                    msg = "委托单异常，有多份记录";
                    return ret;
                }

                IDictionary<string, string> wtd = dt[0];
                if (wtd["sydwbh"] != dwbh)
                {
                    msg = "非本单位送样试验，无法上传变更单";
                    return ret;
                }

                if (bgds == null || bgds.Length == 0)
                {
                    msg = "变更单文件不能为空";
                    return ret;
                }

                // 添加变更单主表
                string bgdwyh = Guid.NewGuid().ToString();
                string sql = "INSERT INTO [UP_BGD]([BGDWYH],[WTDBH],[BGYY],[BGSJ],[SCSJ]) values ('" +
                             bgdwyh + "','" + wtdwyh + "','" + bgyy + "',convert(datetime,'" +
                             bgsj.ToString("yyyy-MM-dd HH:mm:ss") + "'),getdate())";
                CommonDao.ExecCommand(sql, CommandType.Text);

                IList<IDataParameter> arrParams = new List<IDataParameter>();
                int index = 1;
                // 添加变更单文件
                foreach (var file in bgds)
                {

                    byte[] bfile = null;
                    bfile = file.file.DecodeBase64Array();
                    sql = "insert into UP_BGDWJ([BGDWYH],[XH],[BGWJ]) values('" + bgdwyh + "'," + index + ",@bgwj)";
                    arrParams.Clear();
                    SqlParameter param = new SqlParameter("@bgwj", SqlDbType.VarBinary) {Value = bfile};
                    arrParams.Add(param);
                    CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单的二维码字符串。检测中心设置中，允许提前获取二维码的才可以
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool GetBarcode(string dwbh, string wtdwyh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string str = "select hqewm from i_m_qy where qybh='" + dwbh + "'";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(str);
                if (dt.Count == 0)
                    msg = "单位编号无效";
                else
                {
                    int hqewm = dt[0]["hqewm"].GetSafeInt();
                    if (hqewm == 0)
                        msg = "不允许提前获取二维码";
                    else
                    {
                        //ret = GetAndUpdateBgwyh(wtdwyh, out msg);
                        string bgwyh = "";
                        dt = CommonDao.GetDataTable("select bgwyxbh from m_by where recid='" + wtdwyh + "'");
                        if (dt.Count == 0)
                            msg = "获取委托单信息失败";
                        else
                        {
                            bgwyh = dt[0]["bgwyxbh"];
                            if (bgwyh == "")
                            {
                                //服务类
                                IBaseService baseService = ServiceManager.GetBaseService();
                                IDictionary<string, string> qrDic = null;
                                //判断是否标点服务
                                if (!(baseService is BDService))
                                {
                                    throw new Exception("非浙江标点系统不允许提前获取二维码！");
                                }
                                //获取二维码
                                ResultParam qrRet = baseService.GetQrCode(qrDic);
                                if (!qrRet.success)
                                    throw new Exception(qrRet.msg);
                                //获取返回二维码
                                bgwyh = qrRet.data.ToString();
                                CommonDao.ExecCommand(
                                    "update m_by set bgwyxbh='" + bgwyh + "' where recid='" + wtdwyh + "'",
                                    CommandType.Text);
                            }

                            msg = bgwyh;
                            ret = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                ret = false;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 获取检测合同
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetJchts(string dwbh, VTransDownGetJcht where, int pagesize, int pageindex, out int totalcount,
            out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            if (pagesize <= 0)
                pagesize = 20;
            if (pageindex <= 0)
                pageindex = 1;
            try
            {
                // 获取zdzd 
                IList<IDictionary<string, string>> zdzds =
                    CommonDao.GetDataTable(
                        "select sjbmc,zdmc,sy,zdlx from zdzd_jc where sjbmc in ('i_m_jcht','I_S_JCHT_JZRY') ");
                // 查询条件
                StringBuilder sbWhere = new StringBuilder();
                sbWhere.Append("a.sfyx=1 and a.jcjgbh='" + dwbh + "'");
                if (where.htlx.GetSafeRequest() != "")
                {
                    if (where.htlx.Equals("企业合同"))
                        sbWhere.Append(" and a.htlx='QYHT' ");
                    else if (where.htlx.Equals("监督合同"))
                        sbWhere.Append(" and a.htlx='JDHT' ");
                }

                if (where.jchtbh.GetSafeRequest() != "")
                    sbWhere.Append(" and a.jchtbh like '%" + where.jchtbh.GetSafeRequest() + "%' ");
                if (where.gcbh.GetSafeRequest() != "")
                    sbWhere.Append(" and a.gcbh like '%" + where.gcbh.GetSafeRequest() + "%' ");
                if (where.gcmc.GetSafeRequest() != "")
                    sbWhere.Append(" and a.gcmc like '%" + where.gcmc.GetSafeRequest() + "%' ");
                if (where.khdwmc.GetSafeRequest() != "")
                    sbWhere.Append(" and a.khdwmc like '%" + where.khdwmc.GetSafeRequest() + "%' ");
                if (where.zjzmc.GetSafeRequest() != "")
                    sbWhere.Append(" and a.zjzmc like '%" + where.zjzmc.GetSafeRequest() + "%' ");
                if (where.zjdjh.GetSafeRequest() != "")
                    sbWhere.Append(" and a.zjdjh like '%" + where.zjdjh.GetSafeRequest() + "%' ");
                if (where.syrxm.GetSafeRequest() != "")
                    sbWhere.Append(" and a.syrxm like '%" + where.syrxm.GetSafeRequest() + "%' ");
                if (where.sybmmc.GetSafeRequest() != "")
                    sbWhere.Append(" and a.sybmmc like '%" + where.sybmmc.GetSafeRequest() + "%' ");
                if (where.gsbmmc.GetSafeRequest() != "")
                    sbWhere.Append(" and a.gsbmmc like '%" + where.gsbmmc.GetSafeRequest() + "%' ");
                if (where.htqdr.GetSafeRequest() != "")
                    sbWhere.Append(" and a.htqdr like '%" + where.htqdr.GetSafeRequest() + "%' ");

                // 合同信息
                IList<IDictionary<string, string>> tbImjcht = CommonDao.GetPageData(
                    "select * from i_m_jcht a where " + sbWhere.ToString() + " order by jchtbh desc", pagesize,
                    pageindex, out totalcount);
                // 移除一些字段

                string[] removeMFields = new string[]
                {
                    "htzp", "syxmbh", "syxmmc", "lrrzh", "lrrxm",
                    "sfyx", "sxsj", "xgrzh", "xgrxm", "pzrzh",
                    "pzrxm", "sxlx", "serialno", "sxyy", "rowstat"
                };
                // 获取查询的检测合同id，并把合同类型设置成中文
                StringBuilder sbHtids = new StringBuilder();
                foreach (IDictionary<string, string> row in tbImjcht)
                {
                    string htlx = row["htlx"].GetSafeString();
                    if (htlx.Equals("QYHT", StringComparison.OrdinalIgnoreCase))
                        htlx = "企业合同";
                    else if (htlx.Equals("JDHT", StringComparison.OrdinalIgnoreCase))
                        htlx = "监督合同";
                    row["htlx"] = htlx;
                    sbHtids.Append(row["recid"] + ",");
                    foreach (string str in removeMFields)
                    {
                        if (row.ContainsKey(str))
                            row.Remove(str);
                    }

                }

                string htids = sbHtids.ToString().FormatSQLInStr();
                // 见证人员
                IList<IDictionary<string, string>> tbIsjchtjzry =
                    CommonDao.GetDataTable("select * from I_S_JCHT_JZRY where jchtrecid in (" + htids + ")");

                // 匹配对应关系
                foreach (IDictionary<string, string> mrow in tbImjcht)
                {
                    string jchtid = mrow["recid"];
                    // 工程信息
                    IDictionary<string, object> compexRow = new Dictionary<string, object>();
                    compexRow.Add("合同信息", FieldNameToDesc("i_m_jcht", mrow, zdzds));
                    // 见证人员
                    var q = from e in tbIsjchtjzry where e["jchtrecid"].Equals(jchtid) select e;
                    compexRow.Add("见证人员",
                        FieldNameToDesc("i_s_jcht_jzry", q.ToList<IDictionary<string, string>>(), zdzds));

                    records.Add(compexRow);
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        /// <summary>
        /// 设置结算人账户余额
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="jsrbh"></param>
        /// <param name="ye"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetJsrYe(string dwbh, string jsrbh, decimal ye, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select recid from I_M_JSR where jsrbh='" + jsrbh + "' and jcjgbh='" + dwbh +
                                           "'");
                if (dt.Count == 0)
                {
                    msg = "单位下面没有登记该结算人编号";
                    return ret;
                }

                if (dt.Count > 1)
                {
                    msg = "计算人编号重复";
                    return ret;
                }

                string recid = dt[0]["recid"].GetSafeString();

                ret = CommonDao.ExecCommand("update i_m_jsr set zhye=" + ye + " where recid='" + recid + "'",
                    CommandType.Text);
                if (!ret)
                    msg = "更新失败";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        /// <summary>
        /// 判断委托单是否已作废
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        public bool IsWtdZf(string wtdwyh)
        {
            bool ret = false;
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select zt from m_by where recid='" + wtdwyh + "'");
                if (dt.Count > 0)
                {
                    string zt = dt[0]["zt"];
                    WtsStatus objZt = new WtsStatus(zt);
                    ret = objZt.StateZf;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        #endregion

        #region 内部函数

        private bool IsExeclude(IList<IDictionary<string, string>> execludes, string tablename, string fieldname)
        {
            bool ret = false;
            try
            {
                var q = from e in execludes
                    where e["tablename"].Equals(tablename, StringComparison.OrdinalIgnoreCase) &&
                          e["fieldname"].Equals(fieldname, StringComparison.OrdinalIgnoreCase)
                    select e;
                ret = q.Count() > 0;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        private void AddSafeToDictionary(IDictionary<string, string> row, string sy, string zldx, string value)
        {
            try
            {
                if (zldx.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                    value = value.GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                else if (zldx.Equals("date", StringComparison.OrdinalIgnoreCase))
                    value = value.GetSafeDate().ToString("yyyy-MM-dd");

                row.Add(sy, value);
            }
            catch //(Exception ex)
            {
                //SysLog4.WriteLog(ex);
            }
        }

        /// <summary>
        /// 把一行记录的字段名称转换成中文释义，并对日期格式化输出
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="row"></param>
        /// <param name="zdzds"></param>
        /// <returns></returns>
        private IDictionary<string, string> FieldNameToDesc(string tablename, IDictionary<string, string> row,
            IList<IDictionary<string, string>> zdzds)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            try
            {
                foreach (string key in row.Keys)
                {
                    string orgValue = "";
                    if (!row.TryGetValue(key, out orgValue))
                        continue;
                    var q = from e in zdzds
                        where e["sjbmc"].Equals(tablename, StringComparison.OrdinalIgnoreCase) &&
                              e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                        select e;
                    if (q.Count() > 0)
                    {
                        var zdzd = q.First();
                        AddSafeToDictionary(ret, zdzd["sy"], zdzd["zdlx"], orgValue);
                    }
                    else
                        AddSafeToDictionary(ret, key, "", orgValue);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 把一行记录的字段名称转换成中文释义，并对日期格式化输出
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="row"></param>
        /// <param name="zdzds"></param>
        /// <returns></returns>
        private IList<IDictionary<string, string>> FieldNameToDesc(string tablename,
            IList<IDictionary<string, string>> rows, IList<IDictionary<string, string>> zdzds)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                foreach (IDictionary<string, string> row in rows)
                {
                    IDictionary<string, string> retRow = new Dictionary<string, string>();
                    ret.Add(retRow);
                    foreach (string key in row.Keys)
                    {
                        string orgValue = "";
                        if (!row.TryGetValue(key, out orgValue))
                            continue;
                        var q = from e in zdzds
                            where e["sjbmc"].Equals(tablename, StringComparison.OrdinalIgnoreCase) &&
                                  e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase)
                            select e;
                        if (q.Count() > 0)
                        {
                            var zdzd = q.First();
                            AddSafeToDictionary(retRow, zdzd["sy"], zdzd["zdlx"], orgValue);
                        }

                        //else
                        //    AddSafeToDictionary(retRow, key, "", orgValue);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        static IDictionary<string, BarcodeOptionReport> BarcodeOptions = new Dictionary<string, BarcodeOptionReport>();

        private BarcodeOptionReport GetBarCodeOption(string dwbh, string syxmbh)
        {
            BarcodeOptionReport ret = new BarcodeOptionReport();
            try
            {
                string key = dwbh + "_" + syxmbh;
                if (!BarcodeOptions.TryGetValue(key, out ret))
                {
                    ret = new BarcodeOptionReport();
                    string sql = "select top 1 * from syspdfwatermark where (dwbh='' or dwbh='" + dwbh +
                                 "') and (syxmbh='' or syxmbh='" + syxmbh +
                                 "') order by len(dwbh) desc, len(syxmbh) desc";
                    IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        ret.PageModule = dt[0]["ymlx"].GetSafeInt();
                        ret.PositionModule = dt[0]["wzlx"].GetSafeInt();
                        ret.Width = dt[0]["bc"].GetSafeInt();
                        ret.HSpan = dt[0]["hxbj"].GetSafeInt();
                        ret.VSpan = dt[0]["zxbj"].GetSafeInt();
                        BarcodeOptions.Add(key, ret);
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 如果必有主表的bgwyxbh有值，返回该值；如果没有值，把guid赋给bgwyxbh，并保存数据库。如果错误，返回空
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        private bool GetAndUpdateBgwyh(string wtdwyh, out string msg)
        {
            bool ret = false;
            msg = "";

            try
            {
                string bgwyh = "";
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select bgwyxbh from m_by where recid='" + wtdwyh + "'");
                if (dt.Count == 0)
                    msg = "获取委托单信息失败";
                else
                {
                    bgwyh = dt[0]["bgwyxbh"];
                    if (bgwyh == "")
                    {
                        bgwyh = Guid.NewGuid().ToString();
                        CommonDao.ExecCommand("update m_by set bgwyxbh='" + bgwyh + "' where recid='" + wtdwyh + "'",
                            CommandType.Text);
                    }

                    msg = bgwyh;
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        #endregion

        #region 非检测中心调用接口

        /// <summary>
        /// 获取加密key
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="calltype"></param>
        /// <param name="msg"></param>
        /// <returns></returns>

        public bool GetStationEnctyptKey(string stationid, string calltype, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                stationid = stationid.GetSafeRequest();
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select encryptkey from i_m_call where stationid='" + stationid + "' and calltype='" + calltype +
                    "'");
                if (dt.Count == 0)
                    msg = "无效的stationid";
                else
                {
                    msg = dt[0]["encryptkey"];
                    code = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return code;

        }

        /// <summary>
        /// 获取下发的报告条目
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="lastid"></param>
        /// <param name="count"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetDownReports(string stationid, string lastid, int count,
            out string msg)
        {
            msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                stationid = stationid.GetSafeRequest();
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select extrainfo1 from i_m_call where stationid='" + stationid + "'");
                if (dt.Count == 0)
                    msg = "无效的stationid";
                else
                {
                    string zjzcode = dt[0]["extrainfo1"];
                    if (zjzcode == "")
                    {
                        msg = "未配置对应质监站代码";
                    }
                    else
                    {
                        string where = "";
                        if (lastid != "")
                        {
                            dt = CommonDao.GetDataTable("select 上传时间 from view_down_bgsj where 报告唯一号='" + lastid + "'");
                            if (dt.Count > 0)
                            {
                                where += " and 上传时间>(select 上传时间 from view_down_bgsj where 报告唯一号='" + lastid + "') ";
                            }

                        }

                        string top = "";
                        if (count > 0)
                        {
                            top = " top " + count + " ";
                        }

                        string sql =
                            string.Format(
                                "select {0} * from view_down_bgsj where 质监站编号='" + zjzcode + "' {1} order by 上传时间 asc",
                                top, where);
                        ret = CommonDao.GetDataTable(sql);
                        foreach (IDictionary<string, string> row in ret)
                        {
                            row["上传时间"] = row["上传时间"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                            row["试验日期"] = row["试验日期"].GetSafeDate().ToString("yyyy-MM-dd");
                            row["签发日期"] = row["签发日期"].GetSafeDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取分页的报告数据
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="jcdwbh"></param>
        /// <param name="jcdwmc"></param>
        /// <param name="wtdbh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="syxmmc"></param>
        /// <param name="bgbh"></param>
        /// <param name="zjdjh"></param>
        /// <param name="gcbh"></param>
        /// <param name="gcmc"></param>
        /// <param name="khdwmc"></param>
        /// <param name="jcjg">0-不下结论，1-合格，2-不合格</param>
        /// <param name="qfrq1"></param>
        /// <param name="qfrq2"></param>
        /// <param name="scsj1"></param>
        /// <param name="scsj2"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetDownReports(string stationid, string jcdwbh, string jcdwmc,
            string wtdbh, string syxmbh, string syxmmc, string bgbh, string zjdjh, string gcbh, string gcmc,
            string khdwmc, string jcjg, string qfrq1, string qfrq2, string scsj1, string scsj2,
            int pagesize, int pageindex, string orderfield, out int totalcount, out string msg)
        {
            msg = "";
            totalcount = 0;
            if (string.IsNullOrEmpty(orderfield))
                orderfield = "上传时间 desc";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                stationid = stationid.GetSafeRequest();
                jcdwbh = jcdwbh.GetSafeRequest();
                jcdwmc = jcdwmc.GetSafeRequest();
                wtdbh = wtdbh.GetSafeRequest();
                syxmbh = syxmbh.GetSafeRequest();
                syxmmc = syxmmc.GetSafeRequest();
                bgbh = bgbh.GetSafeRequest();
                zjdjh = zjdjh.GetSafeRequest();
                gcbh = gcbh.GetSafeRequest();
                gcmc = gcmc.GetSafeRequest();
                khdwmc = khdwmc.GetSafeRequest();
                jcjg = jcjg.GetSafeRequest();
                qfrq1 = qfrq1.GetSafeRequest();
                qfrq2 = qfrq2.GetSafeRequest();
                scsj1 = scsj1.GetSafeRequest();
                scsj2 = scsj2.GetSafeRequest();
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select extrainfo1 from i_m_call where stationid='" + stationid + "'");
                if (dt.Count == 0)
                    msg = "无效的stationid";
                else
                {
                    string zjzcode = dt[0]["extrainfo1"];
                    if (zjzcode == "")
                    {
                        msg = "未配置对应质监站代码";
                    }
                    else
                    {
                        string where = "";
                        if (jcdwbh != "")
                            where += " and 检测单位编号='" + jcdwbh + "' ";
                        if (jcdwmc != "")
                            where += " and 检测单位名称 like '%" + jcdwmc + "%' ";
                        if (wtdbh != "")
                            where += " and 委托单编号 like '%" + wtdbh + "%' ";
                        if (syxmbh != "")
                            where += " and 试验项目编号='" + syxmbh + "' ";
                        if (syxmmc != "")
                            where += " and 试验项目名称 like '%" + syxmmc + "%' ";
                        if (bgbh != "")
                            where += " and 报告编号 like '%" + bgbh + "%' ";
                        if (zjdjh != "")
                            where += " and 质监登记号 like '%" + zjdjh + "%' ";
                        if (gcbh != "")
                            where += " and 工程编号='" + gcbh + "' ";
                        if (gcmc != "")
                            where += " and 工程名称 like '%" + gcmc + "%' ";
                        if (khdwmc != "")
                            where += " and 客户单位名称 like '%" + khdwmc + "%' ";

                        if (jcjg != "")
                            where += " and 检测结果代码=" + jcjg + " ";
                        if (qfrq1 != "")
                            where += " and 签发日期>= convert(datetime,'" + qfrq1 + "') ";
                        if (qfrq2 != "")
                            where += " and 签发日期<convert(datetime,'" +
                                     qfrq2.GetSafeDate().AddDays(1).ToString("yyyy-MM-dd") + "') ";
                        if (scsj1 != "")
                            where += " and 上传时间>= convert(datetime,'" + scsj1 + "') ";
                        if (scsj2 != "")
                            where += " and 上传时间<convert(datetime,'" +
                                     scsj2.GetSafeDate().AddDays(1).ToString("yyyy-MM-dd") + "') ";

                        string sql =
                            string.Format(
                                "select * from view_down_bgsj where 质监站编号='" + zjzcode + "' {0} order by " + orderfield,
                                where);
                        SysLog4.WriteLog("----call sql:" + sql);
                        ret = CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);
                        foreach (IDictionary<string, string> row in ret)
                        {
                            row["上传时间"] = row["上传时间"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                            row["试验日期"] = row["试验日期"].GetSafeDate().ToString("yyyy-MM-dd");
                            row["签发日期"] = row["签发日期"].GetSafeDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 设置报告处理意见
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="bgwyh"></param>
        /// <param name="opinion"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetReportDealOpinion(string stationid, string bgwyh, string opinion, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = "update up_bgsj set cljd=1,clyj='" + opinion + "' where bgwyh in (" +
                             bgwyh.FormatSQLInStr() + ")";
                code = CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return code;
        }

        /// <summary>
        /// 设置报告处理结果
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="bgwyh"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetReportDealResult(string stationid, string bgwyh, string result, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = "update up_bgsj set cljd=2,cljg='" + result + "' where bgwyh in (" +
                             bgwyh.FormatSQLInStr() + ")";
                code = CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return code;
        }

        #endregion

        #region 现场检测调用

        /// <summary>
        /// 人员手机sim卡号是否已绑定
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public bool HasBindPhoneSim(string usercode)
        {
            bool ret = false;
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable(string.Format("select count(*) as s1 from i_m_rysj where username='{0}'",
                        usercode));
                ret = dt[0]["s1"].GetSafeInt() > 0;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 绑定账号手机sim
        /// </summary>
        /// <param name="username">登录名，不是用户代码</param>
        /// <param name="sim"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool BindPhoneSim(string usercode, string sim, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                CommonDao.ExecCommand(string.Format("delete from i_m_rysj where username='{0}'", usercode),
                    CommandType.Text);
                ret = CommonDao.ExecCommand(
                    string.Format(
                        "insert into i_m_rysj(bindid,username,phone,simcode,bindtime) select newid(),b.yhzh,a.sjhm,'{1}',getdate() from i_m_ry a inner join I_M_QYZH b on a.rybh=b.qybh where b.yhzh='{0}'",
                        usercode, sim), CommandType.Text);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// sim卡是否已被别的人绑定
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public bool HasSimUsed(string usercode, string sim)
        {
            bool ret = false;
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable(string.Format("select username from i_m_rysj where simcode='{0}'", sim));

                if (dt.Count > 0)
                    ret = !usercode.Equals(dt[0]["username"], StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 获取分页现场项目
        /// </summary>
        /// <param name="htxmisxm"></param>
        /// <param name="dwbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcSyxmList(string dwbh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(key))
                    where = " and (syxmbh like '%" + key + "%' or syxmmc like '%" + key + "%')";
                ret = CommonDao.GetPageData(
                    "select syxmbh,syxmmc from PR_M_SYXM a where a.SSDWBH='" + dwbh +
                    "' and a.sfyx=1 and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh and b.yxqs<=getdate() and b.yxqz>=convert(datetime,'" +
                    DateTime.Now.ToString("yyyy-MM-dd") + "')) and a.XCXM=1 " + where + " order by syxmmc",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取分页现场试验编号
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcSybhList(string dwbh, string syxmbh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                //现场桩号
                string xczh = string.Empty;
                //先判断单位的试验项目设置是否存在，如不存在，则取默认值
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(string.Format("select XCZH from PR_M_SYXM where isnull(SSDWBH,'')='{1}' and SYXMBH='{0}'", syxmbh, dwbh));
                if (dt.Count > 0)
                {
                    xczh = dt[0]["XCZH"].GetSafeString();
                }
                else
                {
                    dt = CommonDao.GetDataTable(string.Format("select XCZH from PR_M_SYXM where isnull(SSDWBH,'')='' and SYXMBH='{0}'",syxmbh));
                    if (dt.Count == 0)
                    {
                        return ret;
                    }

                    xczh = dt[0]["XCZH"].GetSafeString();
                }

                //是否启用桩号
                string sTableName = "s_by";
                string zh = "ZH";
                string zlxmc = "组号";

                if (!string.IsNullOrEmpty(xczh))
                {
                    var xczhArr = xczh.Split(',');
                    if (xczhArr.Count() == 3)
                    {
                        sTableName = xczhArr[0];
                        zh = xczhArr[1];
                        zlxmc = xczhArr[2];
                    }
                }

                //桩号
                StringBuilder sql = new StringBuilder();
                //查询字段
                sql.Append("select zh,zlx,zlxmc,lsh,ptbh,wtdbh,pdfurl,gcmc,(select count(*) from UP_CJJL where UP_CJJL.wtdbh=a.ptbh and UP_CJJL.zh=a.zh and SFJS=0) as c1,(select count(*) from UP_CJJL where UP_CJJL.wtdbh=a.ptbh and UP_CJJL.zh=a.zh and SFJS=1) as c2 ");
                sql.Append(" from ( ");

                string sqlSelect = " select z_lsh as lsh,m_by.recid as ptbh,wtdbh,scwtsdz as pdfurl,gcmc " + string.Format(", {0} as zh ", zh) + string.Format(", '{0}' as zlx ", zh) + string.Format(", '{0}' as zlxmc ", zlxmc);
                sql.Append(sqlSelect);

                //查询表
                sql.Append(" from m_by ");
                //是否启用桩号
                sql.AppendFormat(" inner join {0} on m_by.recid = {0}.byzbrecid ", sTableName);

                //条件
                string where = " where sydwbh='" + dwbh + "' and syxmbh='" + syxmbh + "' and (m_by.zt like 'W%' or m_by.zt like 'S%') and m_by.zt not like '_1________' ";
                if (!string.IsNullOrEmpty(key))
                    where += string.Format(" and (m_by.recid like '%{0}%' or m_by.gcmc like '%{0}%' or m_by.wtdbh like '%{0}%' or m_by.z_lsh like '%{0}%' or {1}.{2} like '%{0}%') ", key, sTableName, zh);
                sql.Append(where);
                //没有启用APP只显示第一组的
                sql.Append(" and not exists(select * from I_S_ZJZ_XCJKXM a where m_by.sszjzbh = a.ZJZBH and m_by.syxmbh = a.SYXMBH and APPZSXDYZ = 1) ");

                sql.Append(" union all ");

                sql.Append(sqlSelect);

                sql.Append(" from m_by ");

                if (sTableName.ToLower() == "s_by")
                {
                    sql.AppendFormat(" inner join {0} on m_by.recid = {0}.byzbrecid and s_by.zh = '1' ", sTableName);
                }
                else {
                    sql.AppendFormat(@" inner join {0} on m_by.recid = {0}.byzbrecid 
                                        inner join s_by on m_by.recid = s_by.BYZBRECID and {0}.recid = s_by.recid and s_by.zh = '1'", sTableName);
                }

                sql.Append(where);
                //启用APP只显示第一组
                sql.Append(" and exists(select * from I_S_ZJZ_XCJKXM a where m_by.sszjzbh = a.ZJZBH and m_by.syxmbh = a.SYXMBH and APPZSXDYZ = 1) ");
                sql.Append(") a order by lsh");

                ret = CommonDao.GetPageData(
                   sql.ToString(),
                    pagesize, pageindex, out totalcount);

                foreach (IDictionary<string, string> row in ret)
                {
                    int c1 = row["c1"].GetSafeInt();
                    int c2 = row["c2"].GetSafeInt();
                    row.Remove("c1");
                    row.Remove("c2");
                    row.Remove("rowstat");
                    int state = 0;
                    if (c1 + c2 == 0)
                        state = 0;
                    else if (c1 > 0)
                        state = 1;
                    else if (c2 > 0)
                        state = 2;
                    row.Add("state", state.ToString());
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取分页现场试验设备列表
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="ptbh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcSyDevList(string dwbh, string ptbh, int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                if (string.IsNullOrEmpty(ptbh))
                {
                    msg = "传入的平台编号不能为空";
                    return ret;
                }
                //获取主表信息
                string sql = String.Format("select SYDWBH,SYXMBH from m_by where recid= '{0}'", ptbh);
                IList<IDictionary<string, string>> mdatas = CommonDao.GetDataTable(sql);
                if (mdatas.Count == 0)
                {
                    msg = "委托单信息不存在！";
                    return ret;
                }
                //送样单位
                string sydwbh = mdatas[0]["SYDWBH"].GetSafeString();
                //试验项目
                string syxmbh = mdatas[0]["SYXMBH"].GetSafeString();
                //判断当前用户所在企业是否为此检测机构
                if (sydwbh != dwbh)
                {
                    msg = "当前试验员非此送样检测机构！";
                    return ret;
                }
                //当前年份
                string year = TimeUtil.GetYear();
                //获取此检测机构的可用设备
                sql = String.Format(
                    "select SBBH, SBMC from I_S_QYSB_SB where SBWYH in (select SBWYH from I_M_QYSB where SSDWBH = '{0}' and SBNF = '{1}') group by SBBH, SBMC order by SBMC",
                    sydwbh, year);
                ret = CommonDao.GetPageData(sql, pagesize, pageindex, out totalcount);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取分页现场试验同检人员列表
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="usercode"></param>
        /// <param name="ptbh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcSyrList(string dwbh, string usercode, string ptbh, int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                if (string.IsNullOrEmpty(ptbh))
                {
                    msg = "传入的平台编号不能为空";
                    return ret;
                }
                //获取主表信息
                string sql = String.Format("select SYDWBH,SYXMBH from m_by where recid= '{0}'", ptbh);
                IList<IDictionary<string, string>> mdatas = CommonDao.GetDataTable(sql);
                if (mdatas.Count == 0)
                {
                    msg = "委托单信息不存在！";
                    return ret;
                }
                //送样单位
                string sydwbh = mdatas[0]["SYDWBH"].GetSafeString();
                //试验项目
                string syxmbh = mdatas[0]["SYXMBH"].GetSafeString();
                //判断当前用户所在企业是否为此检测机构
                if (sydwbh != dwbh)
                {
                    msg = "当前试验员非此送样检测机构！";
                    return ret;
                }
                //获取当前用户人员编号
                sql = String.Format("select QYBH from i_m_qyzh where yhzh = '{0}'", usercode);
                IList<IDictionary<string, string>> curUser = CommonDao.GetDataTable(sql);
                if (curUser.Count == 0)
                {
                    msg = "当前试验员信息不存在！";
                    return ret;
                }

                string rybh = curUser[0]["QYBH"].GetSafeString();
                //年份
                string year = TimeUtil.GetYear();
                //获取此检测机构试验员
                sql = String.Format("select select RYBH,RYXM from I_S_QYSB_RY where SBWYH in (select SBWYH from I_M_QYSB where SSDWBH = '{0}' and SBNF = '{1}') and RYBH in (select RYBH from I_M_RY where QYBH = '{0}' and ',JC02,' like '%,'+ zwbh + ',%' and RYBH <> '{2}')", sydwbh, year, rybh);
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取委托单详情
        /// </summary>
        /// <param name="ptbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetXcjcSyDetail(string ptbh, out string msg)
        {
            IDictionary<string, object> ret = new Dictionary<string, object>();
            msg = "";
            try
            {
                if(string.IsNullOrEmpty(ptbh))
                {
                    msg = "传入的平台编号不能为空";
                    return ret;
                }

                string sql = string.Format("select top 1 * from m_by where recid = {0}", ptbh);

                IList<IDictionary<string, string>> mdatas = CommonDao.GetDataTable(sql);

                if (mdatas.Count() == 0)
                {
                    msg = "平台编号记录不存在";
                    return ret;
                }

                var mdict = mdatas[0];
                string zdzdTableName = string.Format("zdzd_{0}", mdict["syxmbh"]);
                string sTableName = string.Format("s_{0}", mdict["syxmbh"]);

                sql = @"select sjbmc, zdmc
                          from xtzd_by
                         where sjbmc in ('m_by', 's_by')
                           and ',' + lx + ',' like '%,W,%'
                        order by xssx ";

                IList<IDictionary<string, string>> zdzds = CommonDao.GetDataTable(sql);

                sql = string.Format("select * from s_by where byzbrecid = '{0}'", ptbh);

                IList<IDictionary<string, string>> sdatas = CommonDao.GetDataTable(sql);

                sql = string.Format("select * from {0} where byzbrecid = '{1}'", sTableName, ptbh);

                IList<IDictionary<string, string>> sedatas = CommonDao.GetDataTable(sql);

                var mzdzds = zdzds.Where(x => x["sjbmc"].ToLower() == "m_by").Select(x => x["zdmc"].ToLower()).ToList();
                var szdzds = zdzds.Where(x => x["sjbmc"].ToLower() == "s_by").Select(x => x["zdmc"].ToLower()).ToList();

                foreach (var mzdzd in mzdzds)
                {
                    if (mdict.ContainsKey(mzdzd))
                    {
                        ret.Add(mzdzd, mdict[mzdzd]);
                    }
                }

                IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();

                foreach (var sdata in sdatas)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();

                    foreach (var szdzd in szdzds)
                    {
                        if (sdata.ContainsKey(szdzd))
                        {
                            data.Add(szdzd, sdata[szdzd]);
                        }
                    }

                    if (sedatas != null)
                    {
                        var sedata = sedatas.FirstOrDefault(x => x["recid"] == sdata["recid"]);

                        if (sedata != null)
                        {
                            foreach (var value in sedata)
                            {
                                if (!data.ContainsKey(value.Key))
                                {
                                    data.Add(value.Key, value.Value);
                                }
                            }
                        }
                    }

                    datas.Add(data);
                }

                ret.Add("sdatas", datas);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取人员企业编号
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public string GetRyQybh(string username, out string msg)
        {
            string ret = "";
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select qybh from i_m_ry where zh='" + username + "'");
                if (dt.Count > 0)
                    ret = dt[0]["qybh"].GetSafeString();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取分页现场试验部位
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcSybwList(string syxmbh, string key, int pagesize,
            int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(key))
                    where = " and (bwmc like '%" + key + "%')";
                ret = CommonDao.GetPageData(
                    "select bwmc from h_xmbw where syxmbh='" + syxmbh + "' " + where + " order by xssx,bwmc",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取分页现场摄像头
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcSxtList(string dwbh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(key))
                    where = " and (sxtmc like '%" + key + "%')";
                ret = CommonDao.GetPageData(
                    "select sbxx1 as recid,sxtmc from i_m_jcsxt where ssdwbh='" + dwbh + "' and sfyx=1 " + where +
                    " order by sxtmc",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 现场检测开始试验
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="sxts"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool XcjcStartExperment(string wtdwyh, int lsh, string zh, string zlx, string syrzh, string syrxm, string longitude, string latitude,
            IList<VTransXcjcReqStartItem> sxts, IList<byte[]> files, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                //判断是否启用摄像头
                string sql = string.Format("select recid, syxmbh, sszjzbh from m_by where recid = '{0}'", wtdwyh);
                var mDt = CommonDao.GetDataTable(sql);

                if (mDt.Count() == 0)
                {
                    msg = "该委托单不存在";
                    return code;
                }

                sql = string.Format("select qysxt from i_s_zjz_xcjkxm where zjzbh = '{0}' and syxmbh = '{1}'", mDt[0]["sszjzbh"], mDt[0]["syxmbh"]);
                var xcjkxmDt = CommonDao.GetDataTable(sql);

                if (xcjkxmDt.Count() > 0)
                {
                    if (xcjkxmDt[0]["qysxt"].GetSafeBool() && sxts.Count() == 0)
                    {
                        msg = "试验项目已启用摄像头,必须上传摄像头数据";
                        return false;
                    }
                }

                StringBuilder sbSxtIds = new StringBuilder();
                foreach (VTransXcjcReqStartItem item in sxts)
                    sbSxtIds.Append(item.sxtbh + ",");
                // 海康摄像头id转换成平台id
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select recid,sbxx1 from I_M_JCSXT where sfyx = 1 and sbxx1 in (" + sbSxtIds.ToString().FormatSQLInStr() + ")");
                sbSxtIds.Clear();
                foreach (IDictionary<string, string> row in dt)
                {
                    sbSxtIds.Append(row["recid"] + ",");
                    foreach (VTransXcjcReqStartItem item in sxts)
                    {
                        if (item.sxtbh.Equals(row["sbxx1"], StringComparison.OrdinalIgnoreCase))
                        {
                            item.sxtbh = row["recid"];
                            break;
                        }
                    }
                }

                // 更新委托单信息
                sql = "update m_by set SYRZH='" + syrzh + "',SYRXM='" + syrxm + "',SYKSSJ=convert(datetime,'" +
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') where recid='" + wtdwyh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                sql = "update m_by set ZT='" + WtsStatus.MainStateSy + "'+substring(zt,2,len(zt)-1) where recid='" +
                      wtdwyh + "' and zt like '" + WtsStatus.MainStateWt + "%'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                CommonDao.ExecCommand(
                    "update UP_CJSXT set JSSJ=getdate(),SFJS=1,JSLX=2 where SXTBH in (" +
                    sbSxtIds.ToString().FormatSQLInStr() + ") and SFJS=0",
                    CommandType.Text);

                string cjjlwyh = Guid.NewGuid().ToString();
                sql = string.Format(
                    "insert into UP_CJJL(CJSJWYH,WTDBH,SYR,SYRXM,KSSJ,Longitude,Latitude,SFJS,JSLX,CJSYBH,LSH,ZH,ZLX) values('{0}','{1}','{2}','{3}',getdate(),{4},{5},0,0,'{6}',{7},'{8}','{9}')",
                    cjjlwyh, wtdwyh, syrzh, syrxm, longitude, latitude, "", lsh, zh, zlx);
                CommonDao.ExecCommand(sql, CommandType.Text);

                foreach (VTransXcjcReqStartItem item in sxts)
                {
                    string recid = Guid.NewGuid().ToString();
                    sql = string.Format(
                        "insert into UP_CJSXT(CJSXTWYH,WTDBH,SYR,SYRXM,SXTBH,KSSJ,Longitude,Latitude,SFJS,JSLX,BWMC,CJSJWYH,LSH,ZH) values('{0}','{1}','{2}','{3}','{4}',getdate(),{5},{6},0,0,'{7}','{8}',{9},'{10}')",
                        recid, wtdwyh, syrzh, syrxm, item.sxtbh, longitude, latitude, item.bw, cjjlwyh, lsh, zh);
                    CommonDao.ExecCommand(sql, CommandType.Text);

                }
                //SysLog4.WriteError(String.Format("调试：委托单唯一号：{0}上传图片数：{1}", wtdwyh, files.Count));
                foreach (byte[] img in files)
                {
                    string subid = Guid.NewGuid().ToString();
                    sql = string.Format(
                        "insert into UP_CJSXTTP(TPWYH,CJSXTWYH,TPXQ,SCSJ,TPLX,ZH) values('{0}','{1}',@tpxq,getdate(),1,'{2}')",
                        subid, wtdwyh, zh);
                    IList<IDataParameter> arrParams = new List<IDataParameter>();
                    SqlParameter param = new SqlParameter("@tpxq", SqlDbType.VarBinary) {Value = img};
                    arrParams.Add(param);
                    CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                }

                if (files.Count > 0)
                    CommonDao.ExecCommand(
                        "update m_by set sjzt=sjzt|" + WtsSjzt.XcHasImage + " where recid='" + wtdwyh + "'",
                        CommandType.Text);

                //更新现场监控
                CommonDao.ExecCommand(string.Format("update m_by set sjzt=sjzt|{0} where recid='{1}' and exists (select 1 from UP_CJSXT where m_by.recid = UP_CJSXT.wtdbh and sfjs = 0)", WtsSjzt.XcjkVideo, wtdwyh));
                //**************************** 开始自动抓拍线程 ***********************************

                //*********************************************************************************
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 试验图片上传
        /// </summary>
        /// <param name="sxts"></param>
        /// <param name="imageType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool XcjcUpImage(string wtdwyh, string zh, IList<byte[]> files, int imageType, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select * from up_cjjl where  wtdbh='" + wtdwyh + "' and zh='" + zh + "' and SFJS=0",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "已经结束试验，不能上传图片";
                    return code;
                }

                if (msg != "")
                    return code;
                foreach (byte[] img in files)
                {
                    string subid = Guid.NewGuid().ToString();
                    string sql = string.Format(
                        "insert into UP_CJSXTTP(TPWYH,CJSXTWYH,TPXQ,SCSJ,TPLX,ZH) values('{0}','{1}',@tpxq,getdate(),{2},'{3}')",
                        subid, wtdwyh, imageType, zh);
                    IList<IDataParameter> arrParams = new List<IDataParameter>();
                    SqlParameter param = new SqlParameter("@tpxq", SqlDbType.VarBinary) {Value = img};
                    arrParams.Add(param);
                    CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                }

                if (files.Count > 0)
                    CommonDao.ExecCommand(
                        "update m_by set sjzt=sjzt|" + WtsSjzt.XcHasImage + " where recid='" + wtdwyh + "'",
                        CommandType.Text);
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 结束试验
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="sxts"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool XcjcStopExperment(string wtdwyh, string zh, IList<byte[]> files, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                // 更新委托单信息
                string sql = "update m_by set SYJSSJ=convert(datetime,'" +
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') where recid='" + wtdwyh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                CommonDao.ExecCommand(
                    "update UP_CJJL set JSSJ=getdate(),SFJS=1,JSLX=1 where WTDBH='" + wtdwyh + "' and SFJS=0 and ZH = '" + zh + "'",
                    CommandType.Text);
                CommonDao.ExecCommand(
                    "update UP_CJSXT set JSSJ=getdate(),SFJS=1,JSLX=1 where WTDBH='" + wtdwyh + "' and SFJS=0 and ZH = '" + zh + "'",
                    CommandType.Text);
                foreach (byte[] img in files)
                {
                    string subid = Guid.NewGuid().ToString();
                    sql = string.Format(
                        "insert into UP_CJSXTTP(TPWYH,CJSXTWYH,TPXQ,SCSJ,TPLX,ZH) values('{0}','{1}',@tpxq,getdate(),3,'{2}')",
                        subid, wtdwyh, zh);
                    IList<IDataParameter> arrParams = new List<IDataParameter>();
                    SqlParameter param = new SqlParameter("@tpxq", SqlDbType.VarBinary) {Value = img};
                    arrParams.Add(param);
                    CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
                }

                if (files.Count > 0)
                    CommonDao.ExecCommand(
                        "update m_by set sjzt=sjzt|" + WtsSjzt.XcHasImage + " where recid='" + wtdwyh + "'",
                        CommandType.Text);

                //现场监控结束后,判断是否存在未结束的,不存在,则去掉现场监控
                CommonDao.ExecCommand(string.Format("update m_by set sjzt = sjzt - {0} where recid='{1}' and not exists (select 1 from UP_CJSXT where m_by.recid = UP_CJSXT.wtdbh and sfjs = 0) and sjzt & {0} > 0", WtsSjzt.XcjkVideo, wtdwyh));

                //现场监控结束后,标记为图片链
                CommonDao.ExecCommand(string.Format("update m_by set sjzt=sjzt|{0} where recid='{1}' and exists (select 1 from UP_CJSXT where m_by.recid = UP_CJSXT.wtdbh and sfjs = 1)", WtsSjzt.XcjkTpl, wtdwyh));
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 获取正在试验的编号
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcInSybhList(string dwbh, string syrzh, string syxmbh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                //现场桩号
                string xczh = string.Empty;
                //先判断单位的试验项目设置是否存在，如不存在，则取默认值
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(string.Format("select XCZH from PR_M_SYXM where isnull(SSDWBH,'')='{1}' and SYXMBH='{0}'", syxmbh, dwbh));
                if (dt.Count > 0)
                {
                    xczh = dt[0]["XCZH"].GetSafeString();
                }
                else
                {
                    dt = CommonDao.GetDataTable(string.Format("select XCZH from PR_M_SYXM where isnull(SSDWBH,'')='' and SYXMBH='{0}'", syxmbh));
                    if (dt.Count == 0)
                    {
                        return ret;
                    }

                    xczh = dt[0]["XCZH"].GetSafeString();
                }

                //获取试验记录
                string where = " and b.syr='" + syrzh + "'";
                if (!string.IsNullOrEmpty(syxmbh))
                    where += " and a.syxmbh='" + syxmbh + "'";

                if (!string.IsNullOrEmpty(key))
                    where += string.Format(" and (a.recid like '%{0}%' or a.wtdbh like '%{0}%' or a.gcmc like '%{0}%' or a.lsh like '%{0}%' or b.zh like '%{0}%') ", key);


                StringBuilder sql = new StringBuilder();
                sql.Append("select  a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.lsh,b.zh,b.zlx,a.zlxmc ");
                sql.Append(" from (");
                sql.Append(" select m_by.syxmbh,m_by.syxmmc,m_by.recid,m_by.wtdbh,m_by.gcmc,m_by.z_lsh as lsh ");
                //是否启用桩号
                string sTableName = "s_by";
                string zh = "ZH";
                string zlxmc = "组号";

                if (!string.IsNullOrEmpty(xczh))
                {
                    var xczhArr = xczh.Split(',');
                    if (xczhArr.Count() == 3)
                    {
                        sTableName = xczhArr[0];
                        zh = xczhArr[1];
                        zlxmc = xczhArr[2];
                    }
                }

                sql.Append(string.Format(", {0} as zh ", zh));
                sql.Append(string.Format(", '{0}' as zlx ", zh));
                sql.Append(string.Format(", '{0}' as zlxmc ", zlxmc));

                //查询表
                sql.Append("from m_by ");
                //是否启用桩号
                sql.AppendFormat("inner join {0} on m_by.recid = {0}.byzbrecid ", sTableName);

                sql.Append(") a ");
                sql.Append("  inner join UP_CJJL b on a.recid=b.WTDBH and a.zh = b.zh where b.sfjs=0 ");
                sql.Append(where);
                sql.Append(" order by a.syxmbh, a.recid ");

                ret = CommonDao.GetPageData(
                    sql.ToString(),
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 保存人员签字
        /// </summary>
        /// <param name="username"></param>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool XcjcSetRySign(string username, string filename, byte[] image, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                if (image == null)
                    return true;
                // 保存到datafile
                string fileid = Guid.NewGuid().ToString();
                string fileext = System.IO.Path.GetExtension(filename);
                if (fileext != "")
                    fileext = "." + fileext;
                string sql = string.Format(
                    "INSERT INTO DATAFILE([FILEID],[FILENAME],[FILECONTENT],[FILEEXT],[CJSJ],[SMALLCONTENT])VALUES('{0}','{1}',@FILECONTENT,'{2}','{3}',@SMALLCONTENT)",
                    fileid, filename, fileext, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                IList<IDataParameter> arrParams = new List<IDataParameter>();
                SqlParameter param = new SqlParameter("@FILECONTENT", SqlDbType.VarBinary) {Value = image};
                arrParams.Add(param);
                param = new SqlParameter("@SMALLCONTENT", SqlDbType.VarBinary) {Value = image};
                arrParams.Add(param);
                CommonDao.ExecCommand(sql, CommandType.Text, arrParams);

                CommonDao.ExecCommand(
                    "update i_m_ry set qz='" + fileid + "," + filename + "|' where zh='" + username + "'",
                    CommandType.Text);
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 获取某个委托单现场上传图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string, object> XcjcGetImages(string url, string wtdwyh, string zh, out string msg)
        {
            IDictionary<string, object> ret = new Dictionary<string, object>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select syxmbh,syxmmc,recid as ptbh,wtdbh,gcmc from m_by where recid='" + wtdwyh + "'");
                if (dt.Count == 0)
                {
                    msg = "平台编号无效，获取委托单失败";
                    return ret;
                }

                IDictionary<string, string> row = dt[0];
                ret.Add("syxmbh", row["syxmbh"]);
                ret.Add("syxmmc", row["syxmmc"]);
                ret.Add("ptbh", row["ptbh"]);
                ret.Add("wtdbh", row["wtdbh"]);
                ret.Add("gcmc", row["gcmc"]);

                dt = CommonDao.GetDataTable("select tpwyh,scsj,tplx from UP_CJSXTTP where cjsxtwyh='" + wtdwyh + "' and zh='" + zh + "' order by scsj desc");
                IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
                foreach (IDictionary<string, string> r1 in dt)
                {
                    string tpwyh = r1["tpwyh"].GetSafeString();
                    string scsj = r1["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                    string tplx = r1["tplx"].GetSafeString();
                    IDictionary<string, string> dr1 = new Dictionary<string, string>();

                    dr1.Add("scsj", scsj);
                    dr1.Add("tpwyh", tpwyh);
                    dr1.Add("tplx", tplx);
                    dr1.Add("url", string.Format(url, tpwyh));

                    dt1.Add(dr1);
                }

                ret.Add("images", dt1);

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取现场检测图片详情
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public byte[] GetXcjcUpImage(string tpid)
        {
            byte[] ret = null;
            try
            {
                IList<IDictionary<string, object>> dt =
                    CommonDao.GetBinaryDataTable("select tpxq from UP_CJSXTTP where  TPWYH='" + tpid + "'");
                if (dt.Count > 0)
                    ret = dt[0]["tpxq"] as byte[];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 获取正在进行的现场检测
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> XcjcGetOnExperments(string usercode)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                StringBuilder sql = new StringBuilder();
                //sql.Append(
                //    "select a.sydwmc,a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,c.cjsxtwyh,b.syr,b.syrxm,c.sxtbh,b.kssj,b.longitude,b.latitude,b.cjsybh,");
                //sql.Append(",c.bwmc");
                //sql.Append("sps=(select count(*) from UP_CJSP where UP_CJSP.CJSJWYH=b.CJSJWYH) ");
                //sql.Append("from m_by a inner join up_cjjl b on a.recid=b.WTDBH ");
                //sql.Append("left outer join UP_CJSXT c on b.CJSJWYH=c.CJSJWYH ");
                //sql.Append("where b.sfjs=0 ");
                //sql.Append("and (");
                //sql.Append("a.gcbh in (select gcbh from i_m_gc where zjzbh in (select zjzbh from i_m_nbry where rybh in (select qybh from i_m_qyzh where yhzh='" + usercode + "'))) ";
                //sql.AppendFormat("or a.ytdwbh in (select qybh from i_m_qyzh where yhzh='{0}') ",usercode);
                //sql.Append(") order by b.kssj desc ");
                sql.Append("select a.sydwbh, a.sydwmc,a.syxmbh,a.syxmmc,a.recid as ptbh,a.z_lsh as lsh,a.wtdbh,a.gcmc,");
                sql.Append("longitude=(select top 1 longitude from up_cjjl where WTDBH=a.RECID and sfjs=0), ");
                sql.Append("latitude=(select top 1 latitude from up_cjjl where WTDBH=a.RECID and sfjs=0), ");
                sql.Append("sxtbhs=(select count(*) from UP_CJSXT where UP_CJSXT.WTDBH=a.RECID), ");
                sql.Append("cjsybhs=(select count(*) from up_cjjl where up_cjjl.WTDBH=a.RECID), "); //试验数量
                sql.Append("sps=(select count(*) from UP_CJSP where UP_CJSP.WTDBH=a.RECID), ");
                //sql.Append("rykq=(select count(*) from up_cjjl x inner join i_m_qyzh y on x.syr=y.yhzh inner join i_m_ry z on y.qybh=z.rybh inner join kqjuserlog p on z.sfzhm=p.userid where convert(date,p.logdate)=convert(date,getdate()) and x.WTDBH=a.RECID and x.sfjs<>1 ) ");
                sql.Append("syry=(select top 1 syrxm from up_cjjl where wtdbh=a.recid and sfjs=0), ");
                sql.Append("wtsl=(select count(*) from s_by where s_by.byzbrecid = a.recid), ");      //委托数量
                sql.Append("zhs = STUFF((SELECT  ',' + isnull(zh, '') FROM up_cjjl WHERE sfjs = 0 and wtdbh = a.recid FOR XML PATH('')), 1, 1, '')");   //正在试验中的桩号
                sql.Append("from m_by a ");
                sql.Append("where 1=1 ");
                sql.Append("and exists(select * from up_cjjl b where b.WTDBH = a.recid and b.sfjs=0)");
                sql.Append("and (");
                sql.Append("a.gcbh in (select gcbh from i_m_gc where (select zjzbh + ',' + IsNUll(xsbm, '') from h_zjz where zjzbh in (select zjzbh from i_m_nbry where rybh in (select qybh from i_m_qyzh where yhzh='" + usercode + "'))) like '%' + zjzbh + '%') ");
                sql.AppendFormat("or a.ytdwbh in (select qybh from i_m_qyzh where yhzh='{0}') ",usercode);
                sql.Append(")  ");
                ret = CommonDao.GetDataTable(sql.ToString());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }

        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="splx"></param>
        /// <param name="videos"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool XcjcUploadVideo(string wtdwyh, string splx, IList<VTransXcjcReqVideoInfoItem> videos,
            out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                if (videos == null || videos.Count == 0)
                {
                    msg = "视频文件为空或者不正确的json数组";
                    return code;
                }

                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select top 1 cjsjwyh from up_cjjl where  wtdbh='" + wtdwyh + "'  order by SFJS,kssj desc",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "找不到试验记录，上传视频失败";
                    return code;
                }

                if (msg != "")
                    return code;

                //**** 判断是否为离线试验，获取状态 ****
                IList<IDictionary<string, string>> dtZt =
                    CommonDao.GetDataTable("select zt from m_by where recid='" + wtdwyh + "'");
                if (dtZt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }

                WtsStatus status = new WtsStatus(dtZt[0]["zt"].GetSafeString());
                //判断是否状态为已试验
                if (!status.StateSy)
                {
                    //如果还是委托状态，变成已试验
                    if (status.StateWt)
                    {
                        CommonDao.ExecCommand(
                            "update m_by set SYKSSJ=convert(datetime,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                            "'),ZT='" + WtsStatus.MainStateSy + "'+substring(zt,2,len(zt)-1) where recid='" + wtdwyh +
                            "' and zt like '" + WtsStatus.MainStateWt + "%'", CommandType.Text);
                    }
                }
                //**************************************

                // 更新委托单信息
                string sql = "update m_by set sjzt=sjzt|" + WtsSjzt.XcHasVideo + " where recid='" + wtdwyh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                string cjsjwyh = dt[0]["cjsjwyh"];
                foreach (VTransXcjcReqVideoInfoItem video in videos)
                {
                    string subid = Guid.NewGuid().ToString();
                    sql = string.Format(
                        "insert into UP_CJSP(CJSJSPWYH,CJSJWYH,WTDBH,KSSJ,JSSJ,SCSJ,SPWJM,SPLX,SFSC,Longitude,Latitude) values('{0}','{1}','{2}',convert(datetime,'{3}'),convert(datetime,'{4}'),getdate(),'{5}','{6}',0,{7},{8})",
                        subid, cjsjwyh, wtdwyh, video.kssj.GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss"),
                        video.jssj.GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss"), video.spwjm, splx,
                        video.longitude.GetSafeDecimal(), video.latitude.GetSafeDecimal());

                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 设置厂家试验编号
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="cjsybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool XcjcSetCjsybh(string wtdwyh, string zhuanghao, string cjsybh, out string msg)
        {
            bool code = false;
            string sql = "";
            msg = "";
            try
            {
                //判断流水号是否是整数
                if (wtdwyh.GetSafeInt() == 0)
                {
                    msg = "试验流水号输入错误！";
                    return code;
                }

                //使用必有主表LSH字段来判断
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select recid from M_BY where z_lsh='" + wtdwyh + "'", CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "试验流水号不存在！";
                    return code;
                }
                //重置委托唯一号
                wtdwyh = dt[0]["recid"].GetSafeString();
                //判断试验是否开始
                sql = "select count(*) as num from UP_CJJL where  wtdbh='" + wtdwyh + "' and ZH='" + zhuanghao + "'";
                dt = CommonDao.GetDataTable(sql, CommandType.Text);
                if (dt[0]["num"].GetSafeInt() == 0)
                {
                    //更新数据
                    sql = "update m_by set sjzt=sjzt+1  where recid='" + wtdwyh + "' and sjzt&1=0";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                    //更新曲线
                    sql = "update m_by set sjzt=sjzt+2  where recid='" + wtdwyh + "' and sjzt&2=0";
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    SysLog4.WriteLog(String.Format("试验未开始，SQL：{0}", sql));
                    msg = "试验未开始";
                    return code;
                }
                // 更新委托单信息
                sql = "update UP_CJJL set cjsybh=cjsybh+'" + cjsybh + "||' where wtdbh='" + wtdwyh + "' and ZH='" + zhuanghao + "' and cjsybh not like '%" + cjsybh + "%'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                sql = "update m_by set sjzt=sjzt+8  where recid='" + wtdwyh + "' and sjzt&8=0";
                CommonDao.ExecCommand(sql, CommandType.Text);
                //更新数据
                sql = "update m_by set sjzt=sjzt+1  where recid='" + wtdwyh + "' and sjzt&1=0";
                CommonDao.ExecCommand(sql, CommandType.Text);
                //更新曲线
                sql = "update m_by set sjzt=sjzt+2  where recid='" + wtdwyh + "' and sjzt&2=0";
                CommonDao.ExecCommand(sql, CommandType.Text);
                //更新委托单试验状态
                sql = "update m_by set ZT='" + WtsStatus.MainStateSy + "' + substring(zt,2,len(zt)-1), SYKSSJ = Getdate() where recid='" +
                   wtdwyh + "' and zt like '" + WtsStatus.MainStateWt + "%'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 现场图片链
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public ResultParam XcjcTpl(string wtdwyh,string zh, int pagesize, int pageindex)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //数据包记录条数及明细
                IDictionary<string, object> data = new Dictionary<string, object>();
                //明细数组
                IList<IDictionary<string,string>> datas = new List<IDictionary<string, string>>();
                //明细数据
                IDictionary<string, string> mx;
                //记录总数
                int totalcount = 0;
                //SQL内容
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from UP_XCCJTP ");
                sql.Append("where 1=1 ");
                sql.AppendFormat("and WTDWYH='{0}' ", wtdwyh);
                //判断组号是否存在
                if (zh != "")
                {
                    sql.AppendFormat("and ZH = '{0}' ", zh);
                }
                //排序
                sql.Append("order by WTDWYH, ZH, recid ");
                //获取记录信息
                IList<IDictionary<string, string>> dt = CommonDao.GetPageData(sql.ToString(), pagesize, pageindex, out totalcount);
                //循环记录
                foreach (IDictionary<string, string> item in dt)
                {
                    mx = new Dictionary<string, string>();
                    //ID
                    mx.Add("id", item["RECID"]);
                    //文本
                    mx.Add("text", item["CONTEXT"]);
                    //图片地址
                    mx.Add("imgurl", item["IMGURL"]);
                    //地址URL
                    mx.Add("url", item["URL"]);
                    //时间
                    mx.Add("date",item["RQ"]);
                    //颜色
                    mx.Add("color", item["COLOR"]);
                    //圈类型
                    mx.Add("type", item["TYPE"]);
                    datas.Add(mx);
                }
                data.Add("total", totalcount);
                data.Add("rows", datas);
                ret.data = data;
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据委托单获取采集试验编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>

        public ResultParam XcjcGetCjsybhs(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //获取busynessid
                string busynessidStr = "";
                //获取流水号
                //判断流水号
                string sql = String.Format("select top 1 lsh,zh from up_cjjl where wtdbh='{0}' order by zh", wtdwyh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                string ptbh = dt.Count > 0 ? dt[0]["lsh"].GetSafeString() : "";
                //SQL
                sql = String.Format("select isnull(cjsybh,'') as cjsybh,lsh,zh from up_cjjl where wtdbh='{0}' and isnull(cjsybh,'')<>'' order by zh", wtdwyh);
                 dt = CommonDao.GetDataTable(sql);
                //数据包
                StringBuilder data = new StringBuilder();
                //遍历
                foreach (var item in dt)
                {
                    data.AppendFormat("{0}||", item["cjsybh"]);
                    //获取业务编号
                    if (item["cjsybh"].GetSafeString() != "" && busynessidStr == "")
                        busynessidStr = item["cjsybh"].GetSafeString();
                }

                //处理data排序 按时间升序排序
                List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
                StringBuilder tempData = new StringBuilder();

                if (!string.IsNullOrEmpty(data.ToString()))
                {
                    Dictionary<string, string> dataDict = new Dictionary<string, string>();
                    var startTime = string.Empty;
                    var dataArrs = data.ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var dataArr in dataArrs)
                    {
                        dataDict = new Dictionary<string, string>();
                        startTime = string.Empty;
                        int startIndex = dataArr.LastIndexOf("[");
                        int endIndex = dataArr.LastIndexOf("]");

                        if (startIndex > 0 && endIndex > 0)
                        {
                            startTime = dataArr.Substring(startIndex + 1, endIndex - startIndex - 1);
                        }

                        dataDict.Add("value", dataArr);
                        dataDict.Add("starttime", startTime);
                        dataList.Add(dataDict);
                    }

                    var tempDataList = dataList.OrderBy(x => x["starttime"]).Select(x => x["value"]).ToList();

                    foreach (var item in tempDataList)
                    {
                        tempData.AppendFormat("{0}||", item);
                    }
                }

                #region 获取设备数据
                var resp = new XcjcRespSyPageList()
                {
                    issuccess = false,
                    message = "",
                    totalcount = 0,
                    records = new List<Dictionary<string, string>>()
                };

                if (!string.IsNullOrEmpty(ptbh))
                {
                    //返回信息
                    string msg = "";
                    string url = GetXcjcUrl();
                    //获取业务编号
                    string busynessidStrOne = busynessidStr.Split(new string[] { "||" }, StringSplitOptions.None)[0];
                    string busynessid = busynessidStrOne.Split('|')[0];
                    //string ptbh = dt.Count > 0 ? dt[0]["lsh"].GetSafeString() : "";
                    IDictionary<string, string> queryParams = new Dictionary<string, string>();
                    queryParams.Add("key", Configs.XcjcKey);
                    queryParams.Add("pagesize", "1000");
                    queryParams.Add("pageindex", "1");
                    queryParams.Add("busynessid", busynessid);
                    queryParams.Add("isvalid", "0");
                    queryParams.Add("ptbh", ptbh);
                    queryParams.Add("zh", "");

                    var code = MyHttp.Post(url + "/xcjc/GetPageSyList", queryParams, out msg);

                    if (code)
                    {
                        resp = JsonSerializer.Deserialize<XcjcRespSyPageList>(msg);
                    }
                }

                //获取未上传数据
                StringBuilder other = new StringBuilder();
                if (resp.records.Count() > 0)
                {
                    var records = resp.records.OrderBy(x => x["starttime"]).ToList();

                    foreach (Dictionary<string, string> item in records)
                    {
                        var q = from en in dt where en["zh"].GetSafeString() == item["zh"].GetSafeString() select en;
                        if (q.Count() == 0)
                            other.AppendFormat("{0}|{1},{2}[{3}]||", item["busynessid"], item["syid"], item["zh"], item["starttime"]);
                    }
                }
                #endregion
                //定义返回格式
                IDictionary<string,string> datas = new Dictionary<string,string>();
                //基础数据包
                datas.Add("base", tempData.ToString());
                //异常数据包
                datas.Add("other", other.ToString());
                //返回数据包
                ret.data = datas;
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据委托单获取采集图片编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        public ResultParam XcjcGetCjtp(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //SQL
                string sql = String.Format("select distinct zh from UP_CJSXTTP where cjsxtwyh='{0}' order by zh", wtdwyh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                //数据包
                StringBuilder data = new StringBuilder();
                //遍历
                foreach (var item in dt)
                {
                    data.AppendFormat("{0},{1}||", wtdwyh, item["zh"]);
                }
                //返回数据包
                ret.data = data.ToString();
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据委托单获取视频编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        public ResultParam XcjcGetCjsp(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //SQL
                string sql = String.Format("select sxtbh,zh from UP_CJSXT where wtdbh='{0}' and sfjs<>1 order by kssj", wtdwyh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                //数据包
                StringBuilder data = new StringBuilder();
                //遍历
                foreach (var item in dt)
                {
                    data.AppendFormat("{0},{1}||", item["sxtbh"], item["zh"]);
                }
                //返回数据包
                ret.data = data.ToString();
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据委托单获取图片链编号组
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <returns></returns>
        public ResultParam XcjcGetCjtpl(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //SQL
                string sql = String.Format("select distinct zh from UP_XCCJTP where wtdwyh = '{0}' and isnull(zh,'')<>'' order by zh  ", wtdwyh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                //数据包
                StringBuilder data = new StringBuilder();
                //遍历
                foreach (var item in dt)
                {
                    data.AppendFormat("{0},{0}||", item["zh"]);
                }
                //返回数据包
                ret.data = data.ToString();
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 见证取样

        /// <summary>
        /// 获取分页所有实验项目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageBaseSyxmList(string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = " and a.SCJZQYTP=1";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.syxmbh like '%" + key + "%' or a.syxmmc like '%" + key + "%')";
                ret = CommonDao.GetPageData(
                    "select a.syxmbh,a.syxmmc from PR_M_SYXM a inner join PR_M_SYXMXSFL b on a.xsflbh=b.XSFLBH  where a.ssdwbh='' and b.ssdwbh='' " +
                    where + " order by b.XSSX, a.XSSX",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取见证人工程列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageJzrGcList(string username, string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = " ";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.gcmc like '%" + key + "%')";
                ret = CommonDao.GetPageData(
                    "select a.gcbh,a.gcmc from i_m_gc a inner join I_S_GC_JZRY b on a.gcbh=b.gcbh where (a.ssjcjgbh is not null and a.ssjcjgbh<>'') and b.rybh in (select rybh from i_m_ry where zh='" +
                    username + "') " + where + " order by a.lrsj desc",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取收样人工程列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageSyrGcList(string qybh, string key,
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = " ";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.gcmc like '%" + key + "%')";
                ret = CommonDao.GetPageData(
                    "select a.gcbh,a.gcmc from i_m_gc a where gcbh in (select gcbh from m_by where ytdwbh='" + qybh +
                    "') and (a.ssjcjgbh is not null and a.ssjcjgbh<>'') " + where + " order by gcmc asc",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取人员类型，非I_M_RY表中的人员，返回空；否则返回i_m_ry中的rylx加i_s_ry_ryzz中的人员类型（这2种00类型不返回），再加上i_m_ry中的zwbh
        /// </summary>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetVirtualRylx(string username, out string msg)
        {
            msg = "";
            bool code = true;
            try
            {
                IList<IDictionary<string, string>> dt =
                    CommonDao.GetDataTable("select lxbh,zwbh,rybh from i_m_ry where zh='" + username + "'");
                if (dt.Count > 0)
                {
                    string lxbh = dt[0]["lxbh"].GetSafeString();
                    if (lxbh != "00" && lxbh != "")
                        msg += lxbh + ",";
                    string zwbh = dt[0]["zwbh"].GetSafeString();
                    if (zwbh != "")
                        msg += zwbh + ",";
                    string rybh = dt[0]["rybh"];
                    dt = CommonDao.GetDataTable("select rylxbh from i_s_ry_ryzz where rybh='" + rybh + "'");
                    foreach (IDictionary<string, string> row in dt)
                    {
                        if (("," + msg + ",").IndexOf("," + row["rylxbh"] + ",") == -1)
                            msg += row["rylxbh"] + ",";
                    }

                    msg = msg.Trim(new char[] {','});
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }

            return code;
        }


        protected string mStrJzqy = "见证取样";

        /// <summary>
        /// 获取需要见证的试验列表
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> JzqyGetJzrSybhList(string qybh, string gcbh, string syxmbh,
            string key, string zt, int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = " and a.zt like 'W___[0-3]%' and a.jclx='" + mStrJzqy +
                               "' and a.sszjzbh in (select zjzbh from h_zjz where jztpqyrq is not null and jztpqyrq<>convert(datetime,'1900-1-1') and jztpqyrq<getdate()) ";
                if (!string.IsNullOrEmpty(syxmbh))
                    where += " and a.syxmbh='" + syxmbh + "'";
                if (!string.IsNullOrEmpty(gcbh))
                    where += " and a.gcbh='" + gcbh + "'";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.recid like '%" + key + "%' or a.wtdbh like '%" + key + "%' or a.gcmc like '%" +
                             key + "%' or a.syxmmc like '%" + key + "%' )";
                if (!string.IsNullOrEmpty(zt))
                {
                    StringBuilder sb = new StringBuilder();
                    string[] arr = zt.Split(new char[] {','});
                    foreach (string str in arr)
                    {
                        if (sb.Length > 0)
                            sb.Append(" or ");
                        sb.Append("a.zt like '_______" + str + "__'");
                    }

                    where += " and (" + sb.ToString() + ")";
                }

                ret = CommonDao.GetPageData(
                    "select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.zt from m_by a where a.jzrbh in (select rybh from i_m_ry where qybh='" +
                    qybh + "') and a.syxmbh in (select distinct syxmbh from PR_M_SYXM where SCJZQYTP=1) " + where +
                    " order by a.syxmbh, a.recid",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                {
                    row["zt"] = row["zt"].Substring(WtsStatus.JzStateIndex, 1);
                    row.Remove("rowstat");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取需要见证的试验列表，包含图片详情
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, object>> JzqyGetJzrSybhListWithDetail(string username, string gcbh,
            string syxmbh, string key, string zt,
            string tprq1, string tprq2, string url, out string msg)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            msg = "";
            try
            {
                //string where = " and a.zt like 'W___[0-3]%' and a.jclx='" + mStrJzqy + "' and a.sszjzbh in (select zjzbh from h_zjz where jztpqyrq is not null and jztpqyrq<>convert(datetime,'1900-1-1') and jztpqyrq<getdate()) ";
                //公众号的见证上传的委托列表，工程里没有选质监站的也要能拍照
                string where = " and a.zt like 'W___[0-3]%' and a.jclx='" + mStrJzqy + "' ";
                if (!string.IsNullOrEmpty(syxmbh))
                    where += " and a.syxmbh='" + syxmbh + "'";
                if (!string.IsNullOrEmpty(gcbh))
                    where += " and a.gcbh='" + gcbh + "'";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.recid like '%" + key + "%' or a.wtdbh like '%" + key + "%' or a.gcmc like '%" +
                             key + "%' or a.syxmmc like '%" + key + "%' )";
                if (!string.IsNullOrEmpty(zt))
                {
                    StringBuilder sb = new StringBuilder();
                    string[] arr = zt.Split(new char[] {','});
                    foreach (string str in arr)
                    {
                        if (sb.Length > 0)
                            sb.Append(" or ");
                        sb.Append("a.zt like '_______" + str + "__'");
                    }

                    where += " and (" + sb.ToString() + ")";
                }

                if (!string.IsNullOrEmpty(tprq1) || !string.IsNullOrEmpty(tprq2))
                {
                    where += " and a.recid in (select wtdwyh from up_wtdtp where 1=1 ";
                    if (!string.IsNullOrEmpty(tprq1))
                        where += " and scsj>=convert(datetime,'" + tprq1 + "') ";
                    if (!string.IsNullOrEmpty(tprq2))
                        where += " and scsj<=convert(datetime,'" + tprq2 + " 23:59:59') ";
                    where += ")";
                }

                //排除无二维码申请的内容限
                where +=
                    " and not exists(select * from I_S_JZSQ where I_S_JZSQ.WTDWYH = a.recid and isnull(I_S_JZSQ.WTDWYH,'')<>'' and isAudit = 1) ";

                IList<IDictionary<string, object>> dtMdatas = CommonDao.GetBinaryDataTable(
                    "select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.zt,c.SCJZQYTP as ewmgl from m_by a inner join PR_M_SYXM c on c.ssdwbh=a.ytdwbh and c.syxmbh=a.syxmbh where exists(select 1 from s_by where s_by.byzbrecid=a.recid) and c.SCJZQYTP=1 and a.gcbh in (select gcbh from I_S_GC_JZRY where rybh in (select rybh from i_m_ry where zh='" +
                    username + "'))" + where + " order by a.syxmbh, a.wtdbh, a.recid");
                StringBuilder sbPtbhs = new StringBuilder();
                foreach (IDictionary<string, object> row in dtMdatas)
                {
                    row["zt"] = row["zt"].GetSafeString().Substring(WtsStatus.JzStateIndex, 1);
                    row.Remove("rowstat");
                    sbPtbhs.Append(row["ptbh"]);
                    sbPtbhs.Append(",");
                }

                IList<IDictionary<string, object>> dtSdatas = CommonDao.GetBinaryDataTable(
                    "select recid,byzbrecid,zh,gcbw,ypewm, isnull(ypysc, 0) ypysc from s_by where byzbrecid in (" +
                    sbPtbhs.ToString().FormatSQLInStr() + ") order by len(zh), zh");
                IList<IDictionary<string, object>> dtTps = CommonDao.GetBinaryDataTable(
                    "select a.wtdwyh,b.tpxqwyh,b.scsj,b.scrxm,b.tplx,a.zh,a.ewm,a.longitude,a.latitude from UP_WTDTPXQ b inner join UP_WTDTP a on a.TPWYH=b.TPWYH where a.sfyx=1 and b.sfyx=1 and b.tplx in ('JZ01','JZ02','JZ03','JZ04') and a.wtdwyh in (" +
                    sbPtbhs.ToString().FormatSQLInStr() + ")");
                IList<IDictionary<string, string>> jzqyZdzds = new List<IDictionary<string, string>>();

                if (dtMdatas.Count() > 0)
                {
                    var tableNames = dtMdatas.Select(x => string.Format("s_{0}", x["syxmbh"].GetSafeString()))
                        .Distinct().ToList();
                    jzqyZdzds = CommonDao.GetDataTable(string.Format(
                        "select sjbmc,zdmc,sy from zdzd_jzqy where sjbmc in ({0}) and sfxs = 1 order by xssx desc",
                        string.Join(",", tableNames).FormatSQLInStr()));
                }

                foreach (IDictionary<string, object> mrow in dtMdatas)
                {
                    string wtdwyh = mrow["ptbh"].GetSafeString();
                    var findSdatas = dtSdatas.Where(e => e["byzbrecid"].GetSafeString() == wtdwyh).OrderBy(x => x["zh"].GetSafeString().Length).OrderBy(x => x["zh"].GetSafeString());
                    var findTps = dtTps.Where(e => e["wtdwyh"].GetSafeString() == wtdwyh);
                    IList<IDictionary<string, object>> srows = new List<IDictionary<string, object>>();

                    //获取自定义参数配置
                    string tableName = string.Format("s_{0}", mrow["syxmbh"].GetSafeString());
                    var customZdzds = jzqyZdzds.Where(x => x["sjbmc"].ToLower() == tableName.ToLower()).ToList();

                    IList<IDictionary<string, string>> extendFileds = new List<IDictionary<string, string>>();

                    if (customZdzds.Count > 0)
                    {
                        extendFileds = CommonDao.GetDataTable(string.Format("select * from {0} where BYZBRECID = '{1}'",
                            tableName, wtdwyh));
                    }

                    foreach (IDictionary<string, object> srow in findSdatas)
                    {
                        //增加自定义字段信息
                        List<Dictionary<string, string>> customFields = new List<Dictionary<string, string>>();
                        var extendFiled = extendFileds.FirstOrDefault(x => x["recid"] == srow["recid"].GetSafeString());

                        if (extendFiled != null)
                        {
                            foreach (var customZdzd in customZdzds)
                            {
                                if (extendFiled.ContainsKey(customZdzd["zdmc"].Trim()))
                                {
                                    Dictionary<string, string> dict = new Dictionary<string, string>();
                                    dict.Add("fieldName", customZdzd["sy"].Trim());
                                    dict.Add("fieldValue", extendFiled[customZdzd["zdmc"].Trim()]);
                                    customFields.Add(dict);
                                }
                            }
                        }

                        srow.Add("customFields", customFields);

                        var findStps = findTps.Where(e => e["zh"].GetSafeString() == srow["zh"].GetSafeString());
                        IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt4 = new List<IDictionary<string, string>>();
                        foreach (IDictionary<string, object> tpRow in findStps)
                        {
                            string tpxqwyh = tpRow["tpxqwyh"].GetSafeString();
                            string scsj = tpRow["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                            string scrxm = tpRow["scrxm"].GetSafeString();
                            string tplx = tpRow["tplx"].GetSafeString();
                            string ewm = tpRow["ewm"].GetSafeString();
                            string longitude = tpRow["longitude"].GetSafeString();
                            string latitude = tpRow["latitude"].GetSafeString();

                            IDictionary<string, string> dr1 = new Dictionary<string, string>();

                            dr1.Add("scsj", scsj);
                            dr1.Add("scrxm", scrxm);
                            dr1.Add("url", string.Format(url, tpxqwyh));
                            dr1.Add("ewm", ewm);
                            dr1.Add("longitude", longitude.ToString());
                            dr1.Add("latitude", latitude.ToString());

                            if (tplx.Equals("JZ01", StringComparison.OrdinalIgnoreCase))
                                dt1.Add(dr1);
                            else if (tplx.Equals("JZ02", StringComparison.OrdinalIgnoreCase))
                                dt2.Add(dr1);
                            else if (tplx.Equals("JZ03", StringComparison.OrdinalIgnoreCase))
                                dt3.Add(dr1);
                            else if (tplx.Equals("JZ04", StringComparison.OrdinalIgnoreCase))
                                dt4.Add(dr1);
                        }

                        srow.Add("xctp", dt1);
                        srow.Add("sytp", dt2);
                        srow.Add("qrcode", dt3);
                        srow.Add("yptp", dt4);
                        srows.Add(srow);
                    }

                    mrow.Add("sdata", srows);

                    ret.Add(mrow);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取送样人送样试验列表
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> JzqyGetSyrSybhList(string qybh, string gcbh, string syxmbh,
            string key, string zt, int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = " and a.zt like 'W___[0-3]%' and a.ytdwbh='" + qybh + "'  and a.jclx='" + mStrJzqy +
                               "' and a.sszjzbh in (select zjzbh from h_zjz where jztpqyrq is not null and jztpqyrq<>convert(datetime,'1900-1-1') and jztpqyrq<getdate()) ";
                if (!string.IsNullOrEmpty(syxmbh))
                    where += " and a.syxmbh='" + syxmbh + "'";
                if (!string.IsNullOrEmpty(gcbh))
                    where += " and a.gcbh='" + gcbh + "'";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.recid like '%" + key + "%' or a.wtdbh like '%" + key + "%' or a.gcmc like '%" +
                             key + "%' )";
                if (!string.IsNullOrEmpty(zt))
                {
                    StringBuilder sb = new StringBuilder();
                    string[] arr = zt.Split(new char[] {','});
                    foreach (string str in arr)
                    {
                        if (sb.Length > 0)
                            sb.Append(" or ");
                        sb.Append("a.zt like '_______" + str + "__'");
                    }

                    where += " and (" + sb.ToString() + ")";
                }

                ret = CommonDao.GetPageData(
                    "select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.zt from m_by a where a.syxmbh in (select distinct syxmbh from PR_M_SYXM where SCJZQYTP=1) and a.jzrbh is not null and a.jzrbh<>'' and a.jzrbh<>'----' " +
                    where + " order by a.syxmbh, a.recid",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                {
                    row["zt"] = row["zt"].Substring(WtsStatus.JzStateIndex, 1);
                    row.Remove("rowstat");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 获取需要见证的试验列表，包含图片详情
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, object>> JzqyGetSyrSybhListWithDetail(string qybh, string gcbh, string syxmbh,
            string key, string zt,
            string tprq1, string tprq2, string url, out string msg)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            msg = "";
            try
            {
                string where = " and a.zt like 'W___[0-3]%'  and a.jclx='" + mStrJzqy +
                               "' and a.sszjzbh in (select zjzbh from h_zjz where jztpqyrq is not null and jztpqyrq<>convert(datetime,'1900-1-1') and jztpqyrq<getdate()) ";
                if (!string.IsNullOrEmpty(syxmbh))
                    where += " and a.syxmbh='" + syxmbh + "'";
                if (!string.IsNullOrEmpty(gcbh))
                    where += " and a.gcbh='" + gcbh + "'";
                if (!string.IsNullOrEmpty(key))
                    where += " and (a.recid like '%" + key + "%' or a.wtdbh like '%" + key + "%' or a.gcmc like '%" +
                             key + "%' or a.syxmmc like '%" + key + "%' )";
                if (!string.IsNullOrEmpty(zt))
                {
                    StringBuilder sb = new StringBuilder();
                    string[] arr = zt.Split(new char[] {','});
                    foreach (string str in arr)
                    {
                        if (sb.Length > 0)
                            sb.Append(" or ");
                        sb.Append("a.zt like '_______" + str + "__'");
                    }

                    where += " and (" + sb.ToString() + ")";
                }

                if (!string.IsNullOrEmpty(tprq1) || !string.IsNullOrEmpty(tprq2))
                {
                    where += " and a.recid in (select wtdwyh from up_wtdtp where 1=1 ";
                    if (!string.IsNullOrEmpty(tprq1))
                        where += " and scsj>=convert(datetime,'" + tprq1 + "') ";
                    if (!string.IsNullOrEmpty(tprq2))
                        where += " and scsj<=convert(datetime,'" + tprq2 + " 23:59:59') ";
                    where += ")";
                }

                IList<IDictionary<string, object>> dtMdatas = CommonDao.GetBinaryDataTable(
                    "select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.zt,c.SCJZQYTP as ewmgl from m_by a inner join PR_M_SYXM c on c.ssdwbh=a.ytdwbh and c.syxmbh=a.syxmbh where c.SCJZQYTP=1 and a.ytdwbh='" +
                    qybh + "' " + where + " order by a.syxmbh, a.recid");
                StringBuilder sbPtbhs = new StringBuilder();
                foreach (IDictionary<string, object> row in dtMdatas)
                {
                    row["zt"] = row["zt"].GetSafeString().Substring(WtsStatus.JzStateIndex, 1);
                    row.Remove("rowstat");
                    sbPtbhs.Append(row["ptbh"]);
                    sbPtbhs.Append(",");
                }

                IList<IDictionary<string, object>> dtSdatas = CommonDao.GetBinaryDataTable(
                    "select byzbrecid,zh,ypewm from s_by where byzbrecid in (" + sbPtbhs.ToString().FormatSQLInStr() +
                    ") order by zh");
                IList<IDictionary<string, object>> dtTps = CommonDao.GetBinaryDataTable(
                    "select a.wtdwyh,b.tpxqwyh,b.scsj,b.scrxm,b.tplx,a.zh,a.ewm,a.longitude,a.latitude from UP_WTDTPXQ b inner join UP_WTDTP a on a.TPWYH=b.TPWYH where a.sfyx=1 and b.sfyx=1 and b.tplx in ('JZ01','JZ02','JZ03','JZ04') and a.wtdwyh in (" +
                    sbPtbhs.ToString().FormatSQLInStr() + ")");
                foreach (IDictionary<string, object> mrow in dtMdatas)
                {
                    string wtdwyh = mrow["ptbh"].GetSafeString();
                    var findSdatas = dtSdatas.Where(e => e["byzbrecid"].GetSafeString() == wtdwyh);
                    var findTps = dtTps.Where(e => e["wtdwyh"].GetSafeString() == wtdwyh);
                    IList<IDictionary<string, object>> srows = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, object> srow in findSdatas)
                    {
                        var findStps = findTps.Where(e => e["zh"].GetSafeString() == srow["zh"].GetSafeString());
                        IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt4 = new List<IDictionary<string, string>>();
                        foreach (IDictionary<string, object> tpRow in findStps)
                        {
                            string tpxqwyh = tpRow["tpxqwyh"].GetSafeString();
                            string scsj = tpRow["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                            string scrxm = tpRow["scrxm"].GetSafeString();
                            string tplx = tpRow["tplx"].GetSafeString();
                            string ewm = tpRow["ewm"].GetSafeString();
                            string longitude = tpRow["longitude"].GetSafeString();
                            string latitude = tpRow["latitude"].GetSafeString();

                            IDictionary<string, string> dr1 = new Dictionary<string, string>();

                            dr1.Add("scsj", scsj);
                            dr1.Add("scrxm", scrxm);
                            dr1.Add("url", string.Format(url, tpxqwyh));
                            dr1.Add("ewm", ewm);
                            dr1.Add("longitude", longitude.ToString());
                            dr1.Add("latitude", latitude.ToString());

                            if (tplx.Equals("JZ01", StringComparison.OrdinalIgnoreCase))
                                dt1.Add(dr1);
                            else if (tplx.Equals("JZ02", StringComparison.OrdinalIgnoreCase))
                                dt2.Add(dr1);
                            else if (tplx.Equals("JZ03", StringComparison.OrdinalIgnoreCase))
                                dt3.Add(dr1);
                            else if (tplx.Equals("JZ04", StringComparison.OrdinalIgnoreCase))
                                dt4.Add(dr1);

                        }

                        srow.Add("xctp", dt1);
                        srow.Add("sytp", dt2);
                        srow.Add("qrcode", dt3);
                        srow.Add("yptp", dt4);
                        srows.Add(srow);
                    }

                    mrow.Add("sdata", srows);

                    ret.Add(mrow);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="ryzh">usercode</param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqyUpImage(string wtdwyh, string qybh, string ryzh, string ryxm, IList<byte[]> files, string ewm,
            string lon, string lat, string zh, string imagetype, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string tplx = "JZ01";

                if (!string.IsNullOrEmpty(imagetype))
                    tplx = imagetype;

                if (files == null || files.Count == 0)
                {
                    msg = "没有图片文件，上传失败";
                    return code;
                }

                string sql = "";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }

                dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }

                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能上传图片";
                    return code;
                }

                string gcbh = dt[0]["gcbh"].GetSafeString();

                // 非自己见证的不能上传现场图片
                string jzrbh = dt[0]["jzrbh"];
                dt = CommonDao.GetDataTable(
                    "select * from I_S_GC_JZRY where rybh in (select qybh from i_m_qyzh where yhzh='" + ryzh +
                    "') and gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "非工程见证人员，无法上传图片";
                    return code;
                }

                string updateEwm = "";
                // 判断二维码是否被使用
                if (!string.IsNullOrEmpty(ewm))
                {
                    string debugSql = "select WTDWYH,ZH from up_wtdtp where not (WTDWYH ='" + wtdwyh + "' and ZH ='" + zh +
                                 "') and EWM='" + ewm + "' and SFYX=1";
                    dt = CommonDao.GetDataTable(debugSql);
                    if (dt.Count() > 0)
                    {
                        string tmpWtdbh = dt[0]["wtdwyh"];
                        string tmpZh = dt[0]["zh"];
                        msg = "二维码【" + ewm + "】已被平台号：" + tmpWtdbh + "，组号：" + tmpZh + "的委托单使用";
                        SysLog4.WriteError(String.Format("JzqyUpImage：{0}，SQL：{1}", msg, debugSql));
                        return code;
                    }

                    // 设置必有从表获二维码
                    dt = CommonDao.GetDataTable("select EWM from up_wtdtp where WTDWYH='" + wtdwyh + "' and ZH='" + zh +
                                                "'  and SFYX=1");
                    foreach (IDictionary<string, string> row in dt)
                    {
                        if (updateEwm.IndexOf(row["ewm"]) == -1)
                            updateEwm += row["ewm"] + ",";
                    }

                    if (updateEwm.IndexOf(ewm) == -1)
                        updateEwm += ewm + ",";
                    updateEwm = updateEwm.Trim(new char[] {','});
                }

                //先上传到Oss, 防止表阻塞
                List<Dictionary<string, string>> jzPics = new List<Dictionary<string, string>>();

                foreach (var file in files)
                {
                    Dictionary<string, string> jzPic = new Dictionary<string, string>();
                    var uploadResult = UploadJzPicOss(file, tplx, out jzPic);

                    if (!uploadResult)
                    {
                        msg = "上传图片到OSS服务器出错,请重新上传";
                        return code;
                    }
                    jzPics.Add(jzPic);
                }

                sql = "update UP_WTDTP set SFYX=0 where WTDWYH='" + wtdwyh + "' and TPLX='" + tplx + "' and zh='" + zh +
                      "' and SFYX=1 and ewm='" + ewm + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                sql = "update UP_WTDTPXQ set SFYX=0 where TPWYH in (select TPWYH from UP_WTDTP where WTDWYH='" +
                      wtdwyh + "' and TPLX='" + tplx + "' and SFYX=0 and zh='" + zh + "') and SFYX=1";
                CommonDao.ExecCommand(sql, CommandType.Text);

                if (!string.IsNullOrEmpty(ewm))
                {
                    sql = "update s_by set ypewm='" + updateEwm + "' where byzbrecid='" + wtdwyh + "' and zh='" + zh +
                          "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                // -- 保存图片之类
                string tpwyh = Guid.NewGuid().ToString();

                sql = string.Format(
                    "insert into UP_WTDTP(TPWYH,WTDWYH,SCSJ,SCR,SCRXM,SFYX,TPLX,zh,ewm,longitude,latitude) values('{0}','{1}',getdate(),'{2}','{3}',1,'{4}','{5}','{6}',{7},{8})",
                    tpwyh, wtdwyh, ryzh, ryxm, tplx, zh, ewm.GetSafeString(), lon.GetSafeDecimal(),
                    lat.GetSafeDecimal());
                CommonDao.ExecCommand(sql, CommandType.Text);

                //插入图片信息
                foreach (var jzPic in jzPics)
                {
                    InsertJzPic(jzPic, tpwyh, ryzh, ryxm, tplx);
                }
                //char newZt = WtsStatus.JzStateTp1;
                //if (status.NeedUpdateImageJzzt(newZt))
                //{
                //    // 是否所有组都上传了现场图片
                //    int sSum = 0, tpSum = 0;
                //    IList<IDictionary<string, string>> tmpDt =
                //        CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' ");
                //    sSum = tmpDt[0]["c1"].GetSafeInt();
                //    tmpDt = CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh +
                //                                       "' and isnull(YPYSC,0) = 1");
                //    tpSum = tmpDt[0]["c1"].GetSafeInt();
                //    if (tpSum == sSum)
                //    {
                //        status.SetWtdJzqyzt(newZt, out msg);
                //        sql = "update m_by set zt='" + status.GetStatus() + "' where recid='" + wtdwyh + "'";
                //        CommonDao.ExecCommand(sql, CommandType.Text);
                //    }
                //}


                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="ryzh">usercode</param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqyUpImage2(string wtdwyh, string qybh, string ryzh, string ryxm,
            IList<IDictionary<string, byte[]>> files, string ewm, string lon, string lat, string zh, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                //判断是否上传了文件
                if (files == null || files.Count == 0)
                {
                    msg = "没有图片文件，上传失败";
                    return code;
                }

                //判断二维码是否为空
                if (ewm == "")
                {
                    msg = "二维码不能为空！";
                    return code;
                }

                string sql = "";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }

                dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh,wtdbh from m_by where recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }

                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能上传图片";
                    return code;
                }

                string gcbh = dt[0]["gcbh"].GetSafeString();

                // 非自己见证的不能上传现场图片
                string jzrbh = dt[0]["jzrbh"];
                dt = CommonDao.GetDataTable(
                    "select * from I_S_GC_JZRY where rybh in (select qybh from i_m_qyzh where yhzh='" + ryzh +
                    "') and gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "非工程见证人员，无法上传图片";
                    return code;
                }

                string updateEwm = "";
                // 判断二维码是否被使用
                if (!string.IsNullOrEmpty(ewm))
                {
                    dt = CommonDao.GetDataTable("select WTDWYH,ZH from up_wtdtp where not (WTDWYH = '" + wtdwyh +
                                                "' and ZH ='" + zh + "') and EWM='" + ewm + "' and SFYX=1");
                    if (dt.Count() > 0)
                    {
                        string tmpWtdbh = dt[0]["wtdwyh"];
                        string tmpZh = dt[0]["zh"];
                        //获取委托单编号
                        string tmpWtdh = "";
                        dt = CommonDao.GetDataTable("select wtdbh from m_by where  recid='" + tmpWtdbh + "'",
                            CommandType.Text);
                        if (dt.Count > 0)
                        {
                            tmpWtdh = dt[0]["wtdbh"].GetSafeString();
                        }

                        msg = "二维码【" + ewm + "】已被平台号：" + tmpWtdbh + "，委托单编号：" + tmpWtdh + "，组号：" + tmpZh + "的委托单使用";
                        return code;
                    }

                    // 设置必有从表获二维码
                    dt = CommonDao.GetDataTable("select EWM from up_wtdtp where WTDWYH='" + wtdwyh + "' and ZH='" + zh +
                                                "'  and SFYX=1");
                    foreach (IDictionary<string, string> row in dt)
                    {
                        if (updateEwm.IndexOf(row["ewm"]) == -1)
                            updateEwm += row["ewm"] + ",";
                    }

                    if (updateEwm.IndexOf(ewm) == -1)
                        updateEwm += ewm + ",";
                    updateEwm = updateEwm.Trim(new char[] {','});
                }

                //单组更新二维码不能为空
                if (updateEwm == "")
                {
                    msg = "平台号：" + wtdwyh + "，组号：" + zh + "中委托单图片中二维码数据不存在！";
                    return code;
                }

                //遍历多种图片类型
                //判断有效类型数量
                int num = 0;
                string tplx;
                byte[] fileContent;
                List<Dictionary<string, string>> jzPics = new List<Dictionary<string, string>>();

                //先上传到Oss上，防止表阻塞
                foreach (IDictionary<string, byte[]> kvp in files)
                {
                    foreach (var key in kvp.Keys)
                    {
                        tplx = "";
                        //获取类型
                        switch (key)
                        {
                            //见证人图片
                            case "jzrtp":
                                tplx = "JZ01";
                                break;
                            //二维码图片
                            case "ewmtp":
                                tplx = "JZ03";
                                break;
                            //样品图片
                            case "yptp":
                                tplx = "JZ04";
                                break;
                            //收样图片
                            case "sytp":
                                tplx = "JZ02";
                                break;
                        }

                        //判断是否为空
                        if (tplx == "")
                        {
                            num++;
                            continue;
                        }

                        //获取文件二进制
                        fileContent = kvp[key];

                        Dictionary<string, string> jzPic = new Dictionary<string, string>();
                        var uploadResult = UploadJzPicOss(fileContent, tplx, out jzPic);

                        if (!uploadResult)
                        {
                            msg = "上传图片到OSS服务器出错,请重新上传";
                            return code;
                        }
                        jzPics.Add(jzPic);
                    }
                }

                //判断上传的图片是否都带有有效类别
                if (num == files.Count)
                {
                    msg = "上传的图片类型无效！";
                    return code;
                }

                //清空记录
                foreach (var jzPic in jzPics)
                {
                    sql = "update UP_WTDTP set SFYX=0 where WTDWYH='" + wtdwyh + "' and TPLX='" + jzPic["tplx"] +
                          "' and zh='" + zh + "' and SFYX=1 and ewm='" + ewm + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    sql = "update UP_WTDTPXQ set SFYX=0 where TPWYH in (select TPWYH from UP_WTDTP where WTDWYH='" +
                          wtdwyh + "' and TPLX='" + jzPic["tplx"] + "' and SFYX=0 and zh='" + zh + "') and SFYX=1";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //添加记录
                foreach(var jzPic in jzPics)
                {
                    // -- 保存图片之类
                    string tpwyh = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(ewm))
                        ewm = tpwyh;

                    sql = string.Format(
                        "insert into UP_WTDTP(TPWYH,WTDWYH,SCSJ,SCR,SCRXM,SFYX,TPLX,zh,ewm,longitude,latitude) values('{0}','{1}',getdate(),'{2}','{3}',1,'{4}','{5}','{6}',{7},{8})",
                        tpwyh, wtdwyh, ryzh, ryxm, jzPic["tplx"], zh, ewm.GetSafeString(), lon.GetSafeDecimal(),
                        lat.GetSafeDecimal());
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    //保存见证图片
                    InsertJzPic(jzPic, tpwyh, ryzh, ryxm, jzPic["tplx"]);
                }

                //更新从表一组的二维码信息
                if (!string.IsNullOrEmpty(ewm))
                {
                    sql = "update s_by set ypewm='" + updateEwm + "' where byzbrecid='" + wtdwyh + "' and zh='" + zh +
                          "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //char newZt = WtsStatus.JzStateTp1;
                //if (status.NeedUpdateImageJzzt(newZt))
                //{
                //    // 是否所有组都上传了现场图片
                //    int sSum = 0, tpSum = 0;
                //    IList<IDictionary<string, string>> tmpDt = CommonDao.GetDataTable("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' ");
                //    sSum = tmpDt[0]["c1"].GetSafeInt();
                //    tmpDt = CommonDao.GetDataTable("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' and isnull(YPYSC,0) = 1");
                //    tpSum = tmpDt[0]["c1"].GetSafeInt();
                //    if (tpSum == sSum)
                //    {
                //        status.SetWtdJzqyzt(newZt, out msg);
                //        sql = "update m_by set zt='" + status.GetStatus() + "' where recid='" + wtdwyh + "'";
                //        CommonDao.ExecCommand(sql, CommandType.Text);
                //    }
                //}
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 设置单组完成状态
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="zh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqySetDzwcStatus(string wtdwyh, string zh, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = "";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }

                if (string.IsNullOrEmpty(dt[0]["ypewm"]))
                {
                    msg = "样品二维码不存在";
                    return code;
                }

                dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }

                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能再设置单组完成状态";
                    return code;
                }

                sql = "update s_by set YPYSC=1  where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                char newZt = WtsStatus.JzStateTp1;
                //if (status.NeedUpdateImageJzzt(newZt))
                //{
                // 是否所有组都上传了现场图片
                int sSum = 0, tpSum = 0;
                IList<IDictionary<string, string>> tmpDt =
                    CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' ");
                sSum = tmpDt[0]["c1"].GetSafeInt();
                tmpDt = CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh +
                                                   "' and isnull(YPYSC,0) = 1");
                tpSum = tmpDt[0]["c1"].GetSafeInt();
                if (tpSum == sSum)
                {
                    status.SetWtdJzqyzt(newZt, out msg);
                    sql = "update m_by set zt='" + status.GetStatus() + "' where recid='" + wtdwyh + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //}
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        /// <summary>
        /// 设置工程坐标
        /// </summary>
        /// <param name="wtdwyh">工程编号</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqySetGczb(string gcbh, string longitude, string latitude, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = "";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    String.Format("select * from I_M_GC where  GCBH='{0}'",gcbh),
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = String.Format("工程编号：{0}不存在！", gcbh);
                    return code;
                }
                if (dt.Count != 1)
                {
                    msg = String.Format("工程编号：{0}存在多个情况！", gcbh);
                    return code;
                }
                //经度
                decimal jd = longitude.GetSafeDecimal();
                //纬度
                decimal wd = latitude.GetSafeDecimal();
                if (jd == 0 || wd == 0)
                {
                    msg = String.Format("经度：{0}或纬度：{1}不正确！", longitude, latitude);
                    return code;
                }

                sql = String.Format("update I_M_GC set GCZB='{1},{2}' where GCBH='{0}'", gcbh, longitude, latitude);
                CommonDao.ExecCommand(sql, CommandType.Text);
                code = true;
                msg = "设置成功！";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }


        /// <summary>
        /// 无安全信息上传送样图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="ryzh">usercode</param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        /// <param name="files"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqyUpImageNotSafe(string wtdwyh, string qybh, string ryxm, IList<byte[]> files, string ewm, string lon, string lat, string zh, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                if (files == null || files.Count == 0)
                {
                    msg = "没有图片文件，上传失败";
                    return code;
                }
                string sql = "";
                string tplx = "JZ02";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }
                dt = CommonDao.GetDataTable("select zt,jzrbh,ytdwbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }
                string ytdwbh = dt[0]["ytdwbh"].GetSafeString();
                if (!ytdwbh.Equals(qybh))
                {
                    msg = "送样单位不对";
                    return code;
                }
                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能上传图片";
                    return code;
                }
                // 判断是否有现场图片
                dt = CommonDao.GetDataTable("select * from UP_WTDTP where tplx='JZ01' and ewm='" + ewm + "' and wtdwyh='" + wtdwyh + "' and zh='"+zh+"'");
                if (dt.Count() == 0)
                {
                    msg = "无法找到对应的现场图片";
                    return code;
                }

                string updateEwm = "";
                // 判断二维码是否被使用
                if (!string.IsNullOrEmpty(ewm))
                {
                    dt = CommonDao.GetDataTable("select WTDWYH,ZH,WTDBH from up_wtdtp where (WTDWYH<>'" + wtdwyh + "' or ZH<>'" + zh + "') and EWM='" + ewm + "' and SFYX=1");
                    if (dt.Count() > 0)
                    {
                        string tmpWtdbh = dt[0]["wtdbh"];
                        string tmpZh = dt[0]["zh"];
                        msg = "二维码已被委托单：" + tmpWtdbh + "，组号：" + tmpZh + "的委托单使用";
                        return code;
                    }

                    // 设置必有从表获二维码
                    dt = CommonDao.GetDataTable("select EWM from up_wtdtp where WTDWYH='" + wtdwyh + "' and ZH='" + zh + "'  and SFYX=1");
                    foreach (IDictionary<string, string> row in dt)
                    {
                        if (updateEwm.IndexOf(row["ewm"]) == -1)
                            updateEwm += row["ewm"] + ",";
                    }
                    if (updateEwm.IndexOf(ewm) == -1)
                        updateEwm += ewm + ",";
                    updateEwm = updateEwm.Trim(new char[] { ',' });
                }

                sql = "update UP_WTDTP set SFYX=0 where WTDWYH='" + wtdwyh + "' and TPLX='" + tplx + "' and zh='" + zh + "' and SFYX=1 and ewm='" + ewm + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                sql = "update UP_WTDTPXQ set SFYX=0 where TPWYH in (select TPWYH from UP_WTDTP where WTDWYH='" + wtdwyh + "' and TPLX='" + tplx + "' and SFYX=0 and zh='" + zh + "') and SFYX=1";
                CommonDao.ExecCommand(sql, CommandType.Text);

                if (!string.IsNullOrEmpty(ewm))
                {
                    sql = "update s_by set ypewm='" + updateEwm + "' where byzbrecid='" + wtdwyh + "' and zh='" + zh + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                string tpwyh = Guid.NewGuid().ToString();
                sql = string.Format("insert into UP_WTDTP(TPWYH,WTDWYH,SCSJ,SCR,SCRXM,SFYX,TPLX,zh,ewm,longitude,latitude) values('{0}','{1}',getdate(),'{2}','{3}',1,'{4}','{5}','{6}',{7},{8})",
                    tpwyh, wtdwyh, "", ryxm, tplx, zh, ewm.GetSafeString(), lon.GetSafeDecimal(), lat.GetSafeDecimal());
                CommonDao.ExecCommand(sql, CommandType.Text);
                foreach (byte[] img in files)
                {
                    //保存见证图片
                    UploadJzPic(img, tpwyh, "", ryxm, tplx);
                }

                char newZt = WtsStatus.JzStateTp2;
                if (status.NeedUpdateImageJzzt(newZt))
                {
                    // 是否每个现场图片都有对应的收样图片
                    IList<IDictionary<string, string>> tmpDt1 = CommonDao.GetDataTable("select distinct zh from UP_WTDTP where wtdwyh='"+wtdwyh+"' and tplx='jz01' and sfyx=1 ");
                    IList<IDictionary<string, string>> tmpDt2 = CommonDao.GetDataTable("select distinct zh from UP_WTDTP where wtdwyh='"+wtdwyh+"' and tplx='jz02' and sfyx=1 ");
                    bool notIn = false;
                    foreach (var row in tmpDt1)
                    {
                        string tmpZh = row["zh"];
                        if (tmpZh != zh)
                        {
                            var finds = tmpDt2.Where(e => e["zh"] == tmpZh);
                            if (finds.Count() == 0)
                                notIn = true;
                        }

                    }
                    if (!notIn)
                    {
                        status.SetWtdJzqyzt(newZt, out msg);
                        sql = "update m_by set zt='" + status.GetStatus() + "' where recid='" + wtdwyh + "'";
                        CommonDao.ExecCommand(sql, CommandType.Text);
                    }
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return code;
        }
        /// <summary>
        /// 获取某个委托单见证详情
        /// </summary>
        /// <param name="syrzh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, object>> JzqyGetWtdJzxq(string url, string wtdwyh,string viewWtdUrl, out string msg)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dtWtds = CommonDao.GetDataTable("select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.zt,a.gcbh,b.recid,b.gcbw,b.zh,b.ypewm,b.ypysc,c.SCJZQYTP as ewmgl from m_by a inner join s_by b on a.recid=b.byzbrecid inner join PR_M_SYXM c on c.ssdwbh=a.ytdwbh and c.syxmbh=a.syxmbh where a.recid='" + wtdwyh + "' order by len(b.zh), b.zh ");
                
                if (dtWtds.Count == 0)
                {
                    msg = "平台编号无效，获取委托单失败";
                    return ret;
                }

                //获取自定义参数配置
                string tableName = string.Format("s_{0}", dtWtds[0]["syxmbh"].GetSafeString());

                IList<IDictionary<string, string>> customZdzds = CommonDao.GetDataTable(string.Format("select zdmc,sy from zdzd_jzqy where sjbmc = '{0}' and sfxs = 1 order by xssx desc", tableName));
                IList<IDictionary<string, string>> extendFileds = new List<IDictionary<string, string>>();

                if (customZdzds.Count > 0)
                { 
                    extendFileds = CommonDao.GetDataTable(string.Format("select * from {0} where BYZBRECID = '{1}'", tableName, wtdwyh));
                }

                IList<IDictionary<string, string>> dtTps = CommonDao.GetDataTable("select b.tpxqwyh,(case when a.jzsj > '' Then a.jzsj else a.scsj end) scsj,b.scrxm,b.tplx,a.zh,a.ewm,a.longitude,a.latitude from UP_WTDTPXQ b inner join UP_WTDTP a on a.TPWYH=b.TPWYH where a.sfyx=1 and b.sfyx=1 and b.tplx in ('JZ01','JZ02','JZ03','JZ04') and a.wtdwyh='" + wtdwyh + "'");

                foreach (IDictionary<string,string> rowWtd in dtWtds)
                {
                    IDictionary<string, object> retRow = new Dictionary<string, object>();                    
                    IDictionary<string, string> row = rowWtd;
                    retRow.Add("syxmbh", row["syxmbh"]);
                    retRow.Add("syxmmc", row["syxmmc"]);
                    retRow.Add("ptbh", row["ptbh"]);
                    retRow.Add("wtdbh", row["wtdbh"]);
                    retRow.Add("gcmc", row["gcmc"]);
                    retRow.Add("zt", row["zt"].Substring(WtsStatus.JzStateIndex, 1));
                    retRow.Add("gcbh", row["gcbh"]);
                    retRow.Add("ypysc", row["ypysc"].GetSafeBool() ? "1" : "0");
                    retRow.Add("gcbw", row["gcbw"]);
                    retRow.Add("zh", row["zh"]);
                    retRow.Add("ypewm", row["ypewm"]);
                    retRow.Add("ewmgl", row["ewmgl"].GetSafeBool()?"1":"0");
                    retRow.Add("viewwtdurl", viewWtdUrl);

                    //增加自定义字段信息
                    List<Dictionary<string, string>> customFields = new List<Dictionary<string, string>>();
                    var extendFiled = extendFileds.FirstOrDefault(x => x["recid"] == row["recid"]);

                    if (extendFiled != null)
                    {
                        foreach (var customZdzd in customZdzds)
                        {
                            if (extendFiled.ContainsKey(customZdzd["zdmc"].Trim()))
                            {
                                Dictionary<string, string> dict = new Dictionary<string, string>();
                                dict.Add("fieldName", customZdzd["sy"].Trim());
                                dict.Add("fieldValue", extendFiled[customZdzd["zdmc"].Trim()]);
                                customFields.Add(dict);
                            }
                        }
                    }

                    retRow.Add("customFields", customFields);

                    string zh = row["zh"];
                    
                    IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
                    IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                    IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
                    IList<IDictionary<string, string>> dt4 = new List<IDictionary<string, string>>();
                    foreach (IDictionary<string, string> tpRow in dtTps)
                    {
                        if (!tpRow["zh"].Equals(zh, StringComparison.OrdinalIgnoreCase))
                            continue;
                        string tpxqwyh = tpRow["tpxqwyh"].GetSafeString();
                        string scsj = tpRow["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                        string scrxm = tpRow["scrxm"].GetSafeString();
                        string tplx = tpRow["tplx"].GetSafeString();
                        string tpZh = tpRow["zh"].GetSafeString();
                        string ewm = tpRow["ewm"].GetSafeString();
                        string longitude = tpRow["longitude"].GetSafeString();
                        string latitude = tpRow["latitude"].GetSafeString(); 

                        IDictionary<string, string> dr1 = new Dictionary<string, string>();

                        dr1.Add("scsj", scsj);
                        dr1.Add("scrxm", scrxm);
                        dr1.Add("zh", tpZh);
                        dr1.Add("url", string.Format(url, tpxqwyh));
                        dr1.Add("ewm", ewm);
                        dr1.Add("longitude",longitude.ToString());
                        dr1.Add("latitude", latitude.ToString());

                        if (tplx.Equals("JZ01", StringComparison.OrdinalIgnoreCase))
                            dt1.Add(dr1);
                        else if (tplx.Equals("JZ02", StringComparison.OrdinalIgnoreCase))
                            dt2.Add(dr1);
                        else if (tplx.Equals("JZ03", StringComparison.OrdinalIgnoreCase))
                            dt3.Add(dr1);
                        else if (tplx.Equals("JZ04", StringComparison.OrdinalIgnoreCase))
                            dt4.Add(dr1);
                    }
                    retRow.Add("xctp", dt1);
                    retRow.Add("sytp", dt2);
                    retRow.Add("qrcode", dt3);
                    retRow.Add("yptp", dt4);
                    ret.Add(retRow);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取UP_WTDTPXQ内容
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public byte[] GetWtdUpImage(string tpid)
        {
            byte[] ret = null;
            try
            {
                IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable("select tpnr,osscdnurl from UP_WTDTPXQ where TPXQWYH='" + tpid + "'");
                if (dt.Count > 0)
                {
                    //目前先兼容数据库存二进制文件,转换完之后把tpnr字段备份后替换为空
                    var ossCdnUrl = dt[0]["osscdnurl"].GetSafeString();

                    if (!string.IsNullOrEmpty(ossCdnUrl))
                    {
                        var fileReturn = OssCdnHelper.GetByOssCdnUrl(ossCdnUrl, "jpg");

                        if (fileReturn.Success)
                        {
                            ret = fileReturn.FileBytes;
                        }
                    }
                    else
                    {
                        ret = dt[0]["tpnr"] as byte[];
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 设置见证取样是否同意
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqySetStatus(string qybh, string ryzh, string ryxm, string wtdwyh, bool agree, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = "";
                // 见证图片
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }
                string zt = dt[0]["zt"].GetSafeString();

                WtsStatus status = new WtsStatus(zt);
                if (!status.CanSetJzzt)
                {
                    msg = "已经完成见证，无法重复确认";
                    return code;
                }

                // 非自己单位的无法见证
                string gcbh = dt[0]["gcbh"];
                dt = CommonDao.GetDataTable("select * from i_s_gc_jzry where gcbh='"+gcbh+"' and rybh in ( select qybh from i_m_qyzh where yhzh='" + ryzh + "')");
                if (dt.Count == 0)
                {
                    msg = "非自己见证的工程，无法确认";
                    return code;
                }

                if (agree)
                    status.SetWtdJzqyzt(WtsStatus.JzStateTy, out msg);
                else
                    status.SetWtdJzqyzt(WtsStatus.JzStateJj, out msg);


                sql = "update m_by set zt='" + status.GetStatus() + "',JZQRRZH='"+ryzh+"',JZQRRXM='"+ryxm+"',JZQRSJ=getdate() where recid='" + wtdwyh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return code;
        }
        /// <summary>
        /// 获取检测软件账号
        /// </summary>
        /// <param name="ryzh"></param>
        /// <returns></returns>
        public string GetJcrjzh(string ryzh)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select JCRJZH from i_m_ry where rybh in (select qybh from i_m_qyzh where yhzh='"+ryzh+"')");
                if (dt.Count > 0)
                    ret = dt[0]["jcrjzh"].GetSafeString();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取某个委托单见证试验详情
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, object>> JzqyGetSyInfo(string wtdwyh, string viewWtdUrl, string url, out string msg)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            msg = "";
            try
            {
                
                IList<IDictionary<string,object>> dtMdatas = CommonDao.GetBinaryDataTable("select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,a.gcmc,a.zt,c.SCJZQYTP as ewmgl from m_by a inner join PR_M_SYXM c on c.ssdwbh=a.ytdwbh and c.syxmbh=a.syxmbh where a.recid='"+wtdwyh+"' order by a.syxmbh, a.recid");
                if (dtMdatas.Count == 0)
                {
                    msg = "平台编号无效，获取委托单失败";
                    return ret;
                }
                foreach (IDictionary<string, object> row in dtMdatas)
                {
                    row["zt"] = row["zt"].GetSafeString().Substring(WtsStatus.JzStateIndex, 1);
                    row.Remove("rowstat");
                }
                IList<IDictionary<string, object>> dtSdatas = CommonDao.GetBinaryDataTable("select byzbrecid,zh,ypewm from s_by where byzbrecid='"+wtdwyh+"' order by zh");
                IList<IDictionary<string, object>> dtTps = CommonDao.GetBinaryDataTable("select a.wtdwyh,b.tpxqwyh,b.scsj,b.scrxm,b.tplx,a.zh,a.ewm,a.longitude,a.latitude from UP_WTDTPXQ b inner join UP_WTDTP a on a.TPWYH=b.TPWYH where a.sfyx=1 and b.sfyx=1 and b.tplx in ('JZ01','JZ02','JZ03','JZ04') and a.wtdwyh='" + wtdwyh + "'");
                foreach (IDictionary<string, object> mrow in dtMdatas)
                {
                    string tmpRecid = mrow["ptbh"].GetSafeString();
                    var findSdatas = dtSdatas.Where(e => e["byzbrecid"].GetSafeString() == tmpRecid);
                    var findTps = dtTps.Where(e => e["wtdwyh"].GetSafeString() == tmpRecid);
                    IList<IDictionary<string, object>> srows = new List<IDictionary<string, object>>();
                    foreach (IDictionary<string, object> srow in findSdatas)
                    {
                        var findStps = findTps.Where(e => e["zh"].GetSafeString() == srow["zh"].GetSafeString());
                        IList<IDictionary<string, string>> dt1 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt2 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt3 = new List<IDictionary<string, string>>();
                        IList<IDictionary<string, string>> dt4 = new List<IDictionary<string, string>>();
                        foreach (IDictionary<string, object> tpRow in findStps)
                        {
                            string tpxqwyh = tpRow["tpxqwyh"].GetSafeString();
                            string scsj = tpRow["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                            string scrxm = tpRow["scrxm"].GetSafeString();
                            string tplx = tpRow["tplx"].GetSafeString();
                            string ewm = tpRow["ewm"].GetSafeString();
                            string longitude = tpRow["longitude"].GetSafeString();
                            string latitude = tpRow["latitude"].GetSafeString(); 

                            IDictionary<string, string> dr1 = new Dictionary<string, string>();

                            dr1.Add("scsj", scsj);
                            dr1.Add("scrxm", scrxm);
                            dr1.Add("url", string.Format(url, tpxqwyh));
                            dr1.Add("ewm", ewm);
                            dr1.Add("longitude",longitude.ToString());
                            dr1.Add("latitude", latitude.ToString());

                            if (tplx.Equals("JZ01", StringComparison.OrdinalIgnoreCase))
                                dt1.Add(dr1);
                            else if (tplx.Equals("JZ02", StringComparison.OrdinalIgnoreCase))
                                dt2.Add(dr1);
                            else if (tplx.Equals("JZ03", StringComparison.OrdinalIgnoreCase))
                                dt3.Add(dr1);
                            else if (tplx.Equals("JZ04", StringComparison.OrdinalIgnoreCase))
                                dt4.Add(dr1);
                        }
                        srow.Add("xctp", dt1);
                        srow.Add("sytp", dt2);
                        srow.Add("qrcode", dt3);
                        srow.Add("yptp", dt4);
                        srows.Add(srow);
                    }
                    mrow.Add("sdata", srows);

                    ret.Add(mrow);
                }
                

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据二维码获取委托单信息
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string,object> JzqyGetSyInfoByEwm(string ewmbh, out string msg)
        {
            IDictionary<string, object> ret = new Dictionary<string, object>();
            msg = "";
            try
            {
                
                IList<IDictionary<string,object>> dtDatas = CommonDao.GetBinaryDataTable("select a.syxmbh,a.syxmmc,a.recid as ptbh,a.wtdbh,b.zh from m_by a inner join s_by b on b.byzbrecid=a.recid where ','+b.ypewm+',' like '%,"+ewmbh+",%'");
                if (dtDatas.Count == 0)
                {
                    msg = "获取试验信息失败";
                    return ret;
                }
                ret = dtDatas[0];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据二维码获取图片信息
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string,object> JzqyGetImagesByEwm(string url, string ewmbh, out string msg)
        {
            IDictionary<string, object> ret = new Dictionary<string, object>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> xcTps = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> syTps = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> qrcodeTps = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> yptpTps = new List<IDictionary<string, string>>();
                ret.Add("xctp", xcTps);
                ret.Add("sytp", syTps);
                ret.Add("qrcodetp", qrcodeTps);
                ret.Add("yptptp", yptpTps);
                IList<IDictionary<string, string>> dtTps = CommonDao.GetDataTable("select a.wtdwyh,b.tpxqwyh,b.scsj,b.scrxm,b.tplx,a.zh,a.ewm,a.longitude,a.latitude from UP_WTDTPXQ b inner join UP_WTDTP a on a.TPWYH=b.TPWYH where a.sfyx=1 and b.sfyx=1 and b.tplx in ('JZ01','JZ02','JZ03','JZ04') and a.ewm='" + ewmbh + "'");

                foreach (IDictionary<string, string> tpRow in dtTps)
                {
                    string tpxqwyh = tpRow["tpxqwyh"].GetSafeString();
                    string scsj = tpRow["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                    string scrxm = tpRow["scrxm"].GetSafeString();
                    string tplx = tpRow["tplx"].GetSafeString();
                    string ewm = tpRow["ewm"].GetSafeString();
                    string longitude = tpRow["longitude"].GetSafeString();
                    string latitude = tpRow["latitude"].GetSafeString();

                    IDictionary<string, string> dr1 = new Dictionary<string, string>();

                    dr1.Add("scsj", scsj);
                    dr1.Add("scrxm", scrxm);
                    dr1.Add("url", string.Format(url, tpxqwyh));
                    dr1.Add("ewm", ewm);
                    dr1.Add("longitude", longitude.ToString());
                    dr1.Add("latitude", latitude.ToString());

                    if (tplx.Equals("JZ01", StringComparison.OrdinalIgnoreCase))
                        xcTps.Add(dr1);
                    else if (tplx.Equals("JZ02", StringComparison.OrdinalIgnoreCase))
                        syTps.Add(dr1);
                    else if (tplx.Equals("JZ03", StringComparison.OrdinalIgnoreCase))
                        qrcodeTps.Add(dr1);
                    else if (tplx.Equals("JZ04", StringComparison.OrdinalIgnoreCase))
                        yptpTps.Add(dr1);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 根据平台编号组号取图片信息
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string,object> JzqyGetImagesByZh(string url, string ptbh,string zh, out string msg)
        {
            IDictionary<string, object> ret = new Dictionary<string, object>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dtNopics = CommonDao.GetDataTable("select zh from i_s_by where (ypewm is null or ypewm='') and byzbrecid='"+ptbh+"' ");
                if (dtNopics.Count > 0)
                {
                    msg = "现场图片不完整";
                    return ret;
                }
                IList<IDictionary<string, string>> xcTps = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> syTps = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> qrcodeTps = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> yptpTps = new List<IDictionary<string, string>>();
                ret.Add("xctp", xcTps);
                ret.Add("sytp", syTps);
                ret.Add("qrcodetp", qrcodeTps);
                ret.Add("yptptp", yptpTps);
                IList<IDictionary<string, string>> dtTps = CommonDao.GetDataTable("select a.wtdwyh,b.tpxqwyh,b.scsj,b.scrxm,b.tplx,a.zh,a.ewm,a.longitude,a.latitude from UP_WTDTPXQ b inner join UP_WTDTP a on a.TPWYH=b.TPWYH where a.sfyx=1 and b.sfyx=1 and b.tplx in ('JZ01','JZ02','JZ03','JZ04') and a.wtdwyh='" + ptbh + "' and zh='" + zh + "' order by len(zh),zh");

                foreach (IDictionary<string, string> tpRow in dtTps)
                {
                    string tpxqwyh = tpRow["tpxqwyh"].GetSafeString();
                    string scsj = tpRow["scsj"].GetSafeDate().ToString("yyyy-MM-dd HH:mm:ss");
                    string scrxm = tpRow["scrxm"].GetSafeString();
                    string tplx = tpRow["tplx"].GetSafeString();
                    string ewm = tpRow["ewm"].GetSafeString();
                    string longitude = tpRow["longitude"].GetSafeString();
                    string latitude = tpRow["latitude"].GetSafeString();

                    IDictionary<string, string> dr1 = new Dictionary<string, string>();

                    dr1.Add("scsj", scsj);
                    dr1.Add("scrxm", scrxm);
                    dr1.Add("url", string.Format(url, tpxqwyh));
                    dr1.Add("ewm", ewm);
                    dr1.Add("longitude", longitude.ToString());
                    dr1.Add("latitude", latitude.ToString());

                    if (tplx.Equals("JZ01", StringComparison.OrdinalIgnoreCase))
                        xcTps.Add(dr1);
                    else if (tplx.Equals("JZ02", StringComparison.OrdinalIgnoreCase))
                        syTps.Add(dr1);
                    else if (tplx.Equals("JZ03", StringComparison.OrdinalIgnoreCase))
                        qrcodeTps.Add(dr1);
                    else if (tplx.Equals("JZ04", StringComparison.OrdinalIgnoreCase))
                        yptpTps.Add(dr1);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取某几个编号是否现场图片没拍，收样图片没拍
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool JzqyGetWtdTpzt(string qybh, string recids, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                //***************************添加过滤见证人扫描**************************
                StringBuilder recidsNew = new StringBuilder();
                string[] wtdwyhList = recids.Split(',');
                string sql = "";
                int num = 0;
                IList<IDictionary<string, string>> tmpDt;
                foreach (string item in wtdwyhList)
                {
                    sql = String.Format("select count(*) as num from I_S_JZSQ where WTDWYH = '{0}' and isAudit = 1", item);
                    tmpDt = CommonDao.GetDataTable(sql);
                    num = tmpDt[0]["num"].GetSafeInt();
                    if (num == 0)
                        recidsNew.AppendFormat("{0},", item);
                }
                if (recidsNew.Length > 0 && recidsNew[recidsNew.Length - 1] == ',')
                    recidsNew.Remove(recidsNew.Length - 1, 1);
                recids = recidsNew.ToString();
                //***********************************************************************
                // 现场图片没传
                IList<IDictionary<string, string>> dtNopics = CommonDao.GetDataTable("select distinct a.wtdbh as recid from m_by a inner join s_by b on a.recid=b.byzbrecid where a.jclx='"+mStrJzqy+"' and a.sszjzbh in (select zjzbh from h_zjz where jztpqyrq is not null and jztpqyrq<>convert(datetime,'1900-1-1') and jztpqyrq<getdate()) and (b.ypewm is null or b.ypewm='') and a.ytdwbh='"+qybh+"' and a.recid in ("+recids.FormatSQLInStr()+") and a.syxmbh in (select syxmbh from pr_m_syxm where scjzqytp=1 and pr_m_syxm.ssdwbh='"+qybh+"')");
                StringBuilder sb = new StringBuilder();
                foreach (IDictionary<string,string> row in dtNopics)
                {
                    sb.Append(row["recid"] + ",");
                }
                if (sb.Length > 0)
                    msg += sb.ToString().Trim(new char[] { ',' }) + "没有上传现场图片。";
                sb.Clear();


                //**** 湖南二维码防伪中无收样图片 ****
                //dtNopics = CommonDao.GetDataTable("select distinct a.wtdbh as recid from m_by a where a.jclx='"+mStrJzqy+"' and a.sszjzbh in (select zjzbh from h_zjz where jztpqyrq is not null and jztpqyrq<>convert(datetime,'1900-1-1') and jztpqyrq<getdate()) and a.ytdwbh='"+qybh+"' and a.recid in ("+recids.FormatSQLInStr()+") and a.syxmbh in (select syxmbh from pr_m_syxm where scjzqytp=1 and pr_m_syxm.ssdwbh='"+qybh+"') and a.recid in (select wtdwyh from UP_WTDTP c where c.wtdwyh=a.recid and c.sfyx=1 and c.tplx='jz01' and ewm not in (select ewm from UP_WTDTP d where d.wtdwyh=a.recid and d.sfyx=1 and d.tplx='jz02') )");
                //foreach (IDictionary<string,string> row in dtNopics)
                //{
                //    if (msg.IndexOf(row["recid"]) == -1)
                //        sb.Append(row["recid"] + ",");
                //}
                //if (sb.Length > 0)
                //    msg += sb.ToString().Trim(new char[] { ',' }) + "没有上传收样图片。";
                ret = msg.Length == 0;

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取某几个编号是否现场图片没拍，收样图片没拍
        /// </summary>
        /// <param name="ewmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly =false)]
        public bool JzqyDelEwm(string ewm, out string msg)
        {
            bool code = true;
            msg = "";
            try
            {
                string sql = "";
                // 见证图片

                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select wtdwyh,zh from UP_WTDTP where sfyx=1 and ewm='" + ewm + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                    return code;

                string wtdwyh = dt[0]["wtdwyh"];
                string zh = dt[0]["zh"];

                dt = CommonDao.GetDataTable("select zt from m_by where recid='" + wtdwyh + "'");
                if (dt.Count == 0)
                {
                    msg = "获取委托单信息失败";
                    return false;
                }
                string zt = dt[0]["zt"];
                WtsStatus status = new WtsStatus(zt);

                dt = CommonDao.GetDataTable("select count(*) as c1 from UP_WTDTP where wtdwyh='" + wtdwyh + "' and zh='" + zh + "' and sfyx=1 and tplx='JZ02'");
                if (dt[0]["c1"].GetSafeInt()>0)
                {
                    msg = "已上传收样图片，不能操作";
                    return false;
                }
                if (status.JzStateCompleteTp1)
                {
                    string tmpmsg = "";
                    status.SetWtdJzqyzt(WtsStatus.JzStateNo, out tmpmsg);
                    CommonDao.ExecCommand("update m_by set zt='" + status.GetStatus() + "'  where recid='" + wtdwyh + "'", CommandType.Text);
                }
                CommonDao.ExecCommand("delete from UP_WTDTP  where sfyx=1 and ewm='" + ewm + "'", CommandType.Text);
                CommonDao.ExecCommand("update s_by set ypewm=replace(ypewm,'" + ewm + "','') where byzbrecid='" + wtdwyh + "' and zh='" + zh + "'", CommandType.Text);
                //设置二维码库
                CommonDao.ExecCommand(String.Format("update H_QRCode set IsUse = 0 where QRCode = '{0}'", ewm), CommandType.Text);
            }
            catch (Exception ex)
            {
                code = false;
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return code;
        }
        /// <summary>
        /// 根据委托单获取工程信息及送样人信息
        /// </summary>
        /// <param name="wtdwyhs"></param>
        /// <param name="gcmc"></param>
        /// <param name="ryxx"></param>
        /// <returns></returns>
        public bool JzqyGetJzrSmsInfo(string wtdwyhs, out string gcmc, out IList<string> ryxx)
        {
            bool ret = true;
            gcmc = "";
            ryxx = new List<string>();
            try
            {
                
                // 现场图片没传
                IList<IDictionary<string, string>> dtGcs = CommonDao.GetDataTable("select distinct gcmc from m_by where recid in ("+wtdwyhs.FormatSQLInStr()+") ");
                if (dtGcs.Count == 0)
                    return false;
                gcmc = dtGcs[0]["gcmc"];

                IList<IDictionary<string, string>> dtRys = CommonDao.GetDataTable("select sjhm from i_m_ry where rybh in (select qybh from i_m_qyzh where yhzh in (select scr from UP_WTDTP where wtdwyh in (" + wtdwyhs.FormatSQLInStr() + ") and sfyx=1 and tplx='JZ01'))");
                foreach (IDictionary<string,string> row in dtRys)
                {
                    string sjhm = row["sjhm"].GetSafeString();
                    if (!string.IsNullOrEmpty(sjhm))
                        ryxx.Add(sjhm);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        #endregion

        #region 监管首页面

        /// <summary>
        /// 管理人员对应区域列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJgAreaList(string usercode, out string msg)
        {
            List<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allcitys = GetHcitys();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();
                
                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                if (!string.IsNullOrEmpty(xsbm))
                {
                    xsbm = "," + xsbm + ",";
                    IList<IDictionary<string, string>> dt2 = allzjzs.Where(e => xsbm.IndexOf(e["zjzbh"])>-1).ToList();
                    ((List<IDictionary<string, string>>)zjztable).AddRange(dt2);
                }
                
                foreach (IDictionary<string,string> row in zjztable)
                {

                    string sf = row["sssf"].GetSafeString();      // 省
                    string cs = row["sscs"].GetSafeString();      // 市
                    string xq = row["ssxq"].GetSafeString();      // 区县
                    bool allArea = false;

                    IList<IDictionary<string, string>> tmpDt = null;
                    if (!string.IsNullOrEmpty(xq))
                        tmpDt = allcitys.Where(e => e["xqid"].Equals(xq)).ToList();
                    else if (!string.IsNullOrEmpty(cs))
                        tmpDt = allcitys.Where(e => e["csid"].Equals(cs)).ToList();
                    else if (!string.IsNullOrEmpty(sf))
                        tmpDt = allcitys.Where(e => e["sfid"].Equals(sf)).ToList();
                    else
                    {
                        tmpDt = allcitys;
                        allArea = true;
                    }
                    ret.AddRange(tmpDt);
                    if (allArea)
                        break;
                }

                ret = ret.Distinct().OrderBy(e=>e["szsf"]).OrderBy(e=>e["szcs"]).OrderBy(e=>e["szxq"]).ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 管理人员对应检测机构列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJgJcjgList(string usercode, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allqys = GetImqys();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                ret = allqys.Where(e =>
                {
                    return zjzjczxs.Where(p => p["jcjgbh"].Equals(e["qybh"])).Count() > 0;
                }).OrderBy(e=>e["qymc"]).ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 管理人员对应工程列表
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJgGcList(string usercode, string sfid, string csid, string qxid, string jdid,
            out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allgcs = GetImgcs();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                var q = allgcs.Where(e => zjzbh.IndexOf(e["zjzbh"]) > -1 && e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));

                
                StringBuilder sbwhere = new StringBuilder();
                if (!string.IsNullOrEmpty(jdid))
                    q = q.Where(e => e["ssjd"].Equals(jdid));
                else if (!string.IsNullOrEmpty(qxid))
                    q = q.Where(e => e["ssxq"].Equals(qxid));
                else if (!string.IsNullOrEmpty(csid))
                    q = q.Where(e => e["sscs"].Equals(csid));
                else if (!string.IsNullOrEmpty(sfid))
                    q = q.Where(e => e["sssf"].Equals(sfid));
                ret = q.OrderBy(e => e["gcmc"]).ToList();
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 检测机构统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetJgStatistic(string usercode, string sfid, string csid, string qxid, string jdid, 
            string jcjgid, string gcid, out string msg)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allgcs = GetImgcs();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();
                

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                IList<IDictionary<string, string>> jdgcs = allgcs.Where(e => zjzbh.IndexOf(e["zjzbh"]) > -1 && e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"])).ToList();
                IList<IDictionary<string, string>> fjdgcs = allgcs.Where(e => e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"])).ToList();
                fjdgcs = fjdgcs.Where(e =>
                {
                    return jdgcs.Where(p => p["gcbh"].Equals(e["sjgcbh"], StringComparison.OrdinalIgnoreCase)).Count() > 0;
                }).ToList();


                StringBuilder sbwhere = new StringBuilder();
                if (!string.IsNullOrEmpty(jdid))
                {
                    jdgcs = jdgcs.Where(e => e["ssjd"].Equals(jdid)).ToList();
                    fjdgcs = fjdgcs.Where(e => e["ssjd"].Equals(jdid)).ToList();
                }
                else if (!string.IsNullOrEmpty(qxid))
                {
                    jdgcs = jdgcs.Where(e => e["ssxq"].Equals(qxid)).ToList();
                    fjdgcs = fjdgcs.Where(e => e["ssxq"].Equals(qxid)).ToList();
                }
                else if (!string.IsNullOrEmpty(csid))
                {
                    jdgcs = jdgcs.Where(e => e["sscs"].Equals(csid)).ToList();
                    fjdgcs = fjdgcs.Where(e => e["sscs"].Equals(csid)).ToList();
                }
                else if (!string.IsNullOrEmpty(sfid))
                {
                    jdgcs = jdgcs.Where(e => e["sssf"].Equals(sfid)).ToList();
                    fjdgcs = fjdgcs.Where(e => e["sssf"].Equals(sfid)).ToList();
                }
                if (!string.IsNullOrEmpty(gcid))
                {
                    jdgcs = jdgcs.Where(e => e["gcbh"].Equals(gcid)).ToList();
                    fjdgcs = fjdgcs.Where(e => e["gcbh"].Equals(gcid)).ToList();
                }

                ret.Add("gcs", jdgcs.Count().ToString());
                ret.Add("jzmj", jdgcs.Sum(e => e["jzmj"].GetSafeDecimal()).ToString());

                IList<IDictionary<string, string>> allwtds = GetMbys();
                List<IDictionary<string, string>> validGcs = new List<IDictionary<string, string>>();
                validGcs.AddRange(jdgcs);
                validGcs.AddRange(fjdgcs);
                StringBuilder sbvalidgcs = new StringBuilder();
                foreach (IDictionary<string, string> gcrow in validGcs)
                    sbvalidgcs.Append(gcrow["gcbh"] + ",");
                string strvalidgcbhs = "," + sbvalidgcs.ToString() + ",";
                var validWtds = allwtds.Where(e => strvalidgcbhs.Contains(e["gcbh"])).ToList();
                if (!string.IsNullOrEmpty(jcjgid))
                    validWtds = validWtds.Where(e => e["ytdwbh"].Equals(jcjgid)).ToList();
                ret.Add("wtds", validWtds.Sum(e=>e["s1"].GetSafeInt()).ToString());
                var bgWtds = validWtds.Where(e => (e["zt"].StartsWith("B") || e["zt"].StartsWith("G"))).ToList();
                ret.Add("bgs", bgWtds.Sum(e => e["s1"].GetSafeInt()).ToString());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取报告合格不合格统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetJgBgStatistic(string usercode, string sfid, string csid, string qxid, string jdid, 
            string jcjgid, string gcid, out string msg)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allgcs = GetImgcs();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                IList<IDictionary<string,string>> jdgcs = allgcs.Where(e => zjzbh.IndexOf(e["zjzbh"]) > -1 && e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"])).ToList();
                IList<IDictionary<string, string>> fjdgcs = allgcs.Where(e => e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"])).ToList();
                fjdgcs = fjdgcs.Where(e =>
                {
                    return jdgcs.Where(p => p["gcbh"].Equals(e["sjgcbh"], StringComparison.OrdinalIgnoreCase)).Count() > 0;
                }).ToList();

                List<IDictionary<string, string>> validGcs = new List<IDictionary<string, string>>();
                validGcs.AddRange(jdgcs);
                validGcs.AddRange(fjdgcs);

                if (!string.IsNullOrEmpty(jdid))
                    validGcs = validGcs.Where(e => e["ssjd"].Equals(jdid)).ToList();
                else if (!string.IsNullOrEmpty(qxid))
                    validGcs = validGcs.Where(e => e["ssxq"].Equals(qxid)).ToList();
                else if (!string.IsNullOrEmpty(csid))
                    validGcs = validGcs.Where(e => e["sscs"].Equals(csid)).ToList();
                else if (!string.IsNullOrEmpty(sfid))
                    validGcs = validGcs.Where(e => e["sssf"].Equals(sfid)).ToList();                
                if (!string.IsNullOrEmpty(gcid))
                    validGcs = validGcs.Where(e => e["gcbh"].Equals(gcid)).ToList();
                

                IList<IDictionary<string, string>> allwtds = GetMbys();
                if (!string.IsNullOrEmpty(jcjgid))
                    allwtds = allwtds.Where(e => e["ytdwbh"].Equals(jcjgid)).ToList();
                allwtds = allwtds.Where(e => e["zt"].IndexOf("B") == 0).ToList();
                
                StringBuilder sbvalidgcs = new StringBuilder();
                foreach (IDictionary<string, string> gcrow in validGcs)
                    sbvalidgcs.Append(gcrow["gcbh"] + ",");
                string strvalidgcbhs = "," + sbvalidgcs.ToString() + ",";
                var validWtds = allwtds.Where(e => strvalidgcbhs.Contains(e["gcbh"])).ToList();

                ret.Add("不下结论", validWtds.Where(e=>e["jcjg"].GetSafeInt() == 0).Sum(e=>e["s1"].GetSafeInt()).ToString());
                ret.Add("合格", validWtds.Where(e => e["jcjg"].GetSafeInt() == 1).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("不合格", validWtds.Where(e => e["jcjg"].GetSafeInt() == 2).Sum(e => e["s1"].GetSafeInt()).ToString());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取委托单状态统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetJgWtdztStatistic(string usercode, string sfid, string csid, string qxid, string jdid, string jcjgid, string gcid, out string msg)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allgcs = GetImgcs();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                var jdgcs = allgcs.Where(e => zjzbh.IndexOf(e["zjzbh"]) > -1 && e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));
                var fjdgcs = allgcs.Where(e => e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));
                fjdgcs = fjdgcs.Where(e =>
                {
                    return jdgcs.Where(p => p["gcbh"].Equals(e["sjgcbh"], StringComparison.OrdinalIgnoreCase)).Count() > 0;
                });

                List<IDictionary<string, string>> validGcs = new List<IDictionary<string, string>>();
                validGcs.AddRange(jdgcs);
                validGcs.AddRange(fjdgcs);

                if (!string.IsNullOrEmpty(jdid))
                    validGcs = validGcs.Where(e => e["ssjd"].Equals(jdid)).ToList();
                else if (!string.IsNullOrEmpty(qxid))
                    validGcs = validGcs.Where(e => e["ssxq"].Equals(qxid)).ToList();
                else if (!string.IsNullOrEmpty(csid))
                    validGcs = validGcs.Where(e => e["sscs"].Equals(csid)).ToList();
                else if (!string.IsNullOrEmpty(sfid))
                    validGcs = validGcs.Where(e => e["sssf"].Equals(sfid)).ToList();
                if (!string.IsNullOrEmpty(gcid))
                    validGcs = validGcs.Where(e => e["gcbh"].Equals(gcid)).ToList();

                IList<IDictionary<string, string>> allwtds = GetMbys();
                if (!string.IsNullOrEmpty(jcjgid))
                    allwtds = allwtds.Where(e => e["ytdwbh"].Equals(jcjgid)).ToList();
                StringBuilder sbvalidgcs = new StringBuilder();
                foreach (IDictionary<string, string> gcrow in validGcs)
                    sbvalidgcs.Append(gcrow["gcbh"] + ",");
                string strvalidgcbhs = "," + sbvalidgcs.ToString() + ",";
                var validWtds = allwtds.Where(e => strvalidgcbhs.Contains(e["gcbh"])).ToList();
                ret.Add("已委托", validWtds.Where(e => e["zt"].IndexOf("W")==0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("已试验", validWtds.Where(e => e["zt"].IndexOf("S") == 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("已出报告", validWtds.Where(e => e["zt"].IndexOf("B") == 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取委托单异常状态统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetJgWtdycztStatistic(string usercode, string sfid, string csid, string qxid, string jdid, string jcjgid, string gcid, out string msg)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allgcs = GetImgcs();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                var jdgcs = allgcs.Where(e => zjzbh.IndexOf(e["zjzbh"]) > -1 && e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));
                var fjdgcs = allgcs.Where(e => e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));
                fjdgcs = fjdgcs.Where(e =>
                {
                    return jdgcs.Where(p => p["gcbh"].Equals(e["sjgcbh"], StringComparison.OrdinalIgnoreCase)).Count() > 0;
                });

                List<IDictionary<string, string>> validGcs = new List<IDictionary<string, string>>();
                validGcs.AddRange(jdgcs);
                validGcs.AddRange(fjdgcs);

                if (!string.IsNullOrEmpty(jdid))
                    validGcs = validGcs.Where(e => e["ssjd"].Equals(jdid)).ToList();
                else if (!string.IsNullOrEmpty(qxid))
                    validGcs = validGcs.Where(e => e["ssxq"].Equals(qxid)).ToList();
                else if (!string.IsNullOrEmpty(csid))
                    validGcs = validGcs.Where(e => e["sscs"].Equals(csid)).ToList();
                else if (!string.IsNullOrEmpty(sfid))
                    validGcs = validGcs.Where(e => e["sssf"].Equals(sfid)).ToList();
                if (!string.IsNullOrEmpty(gcid))
                    validGcs = validGcs.Where(e => e["gcbh"].Equals(gcid)).ToList();

                IList<IDictionary<string, string>> allwtds = GetMbys();
                if (!string.IsNullOrEmpty(jcjgid))
                    allwtds = allwtds.Where(e => e["ytdwbh"].Equals(jcjgid)).ToList();
                StringBuilder sbvalidgcs = new StringBuilder();
                foreach (IDictionary<string, string> gcrow in validGcs)
                    sbvalidgcs.Append(gcrow["gcbh"] + ",");
                string strvalidgcbhs = "," + sbvalidgcs.ToString() + ",";
                var validWtds = allwtds.Where(e => strvalidgcbhs.Contains(e["gcbh"])).ToList();
                ret.Add("正常", validWtds.Where(e => e["yczt"].GetSafeInt() == 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("委托单有修改", validWtds.Where(e => (e["yczt"].GetSafeInt()&1)>0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("委托书字段未全部上传", validWtds.Where(e => (e["yczt"].GetSafeInt() & 2) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("自动采集数据有修改", validWtds.Where(e => (e["yczt"].GetSafeInt() & 4) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("自动采集有未保存数据", validWtds.Where(e => (e["yczt"].GetSafeInt() & 8) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("自动采集有重做数据", validWtds.Where(e => (e["yczt"].GetSafeInt() & 16) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("自动采集有重复试验", validWtds.Where(e => (e["yczt"].GetSafeInt() & 32) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("有重复报告", validWtds.Where(e => (e["yczt"].GetSafeInt() & 64) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("试验员未登记", validWtds.Where(e => (e["yczt"].GetSafeInt() & 128) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("试验员未到场", validWtds.Where(e => (e["yczt"].GetSafeInt() & 256) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("数据上传时间超差", validWtds.Where(e => (e["yczt"].GetSafeInt() & 512) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                ret.Add("报告上传超差", validWtds.Where(e => (e["yczt"].GetSafeInt() & 1024) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());               

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取检测结构委托单状态统计
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public IDictionary<string, IDictionary<string,string>> GetJgJcjgWtdztStatistic(string usercode, string sfid, string csid, string qxid, string jdid, string jcjgid, string gcid, out string msg)
        {
            IDictionary<string, IDictionary < string,string>> ret = new Dictionary<string, IDictionary<string, string>>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> allnbrys = GetImnbrys();
                IList<IDictionary<string, string>> allzjzs = GetHzjzs();
                IList<IDictionary<string, string>> allqyzhs = GetImqyzhs();
                IList<IDictionary<string, string>> allgcs = GetImgcs();
                IList<IDictionary<string, string>> allzjzjczxs = GetIszjzjczxs();
                IDictionary<string, string> qyzhrow = allqyzhs.Where(e => e["yhzh"].Equals(usercode)).First();
                IDictionary<string, string> nbryrow = allnbrys.Where(e => e["rybh"].Equals(qyzhrow["qybh"])).First();

                string zjzbh = nbryrow["zjzbh"].GetSafeString();
                if (string.IsNullOrEmpty(zjzbh))
                    return ret;
                IList<IDictionary<string, string>> zjztable = allzjzs.Where(e => e["zjzbh"].Equals(zjzbh)).ToList();
                IDictionary<string, string> zjzrow = zjztable[0];
                string xsbm = zjzrow["xsbm"].GetSafeString();    // 下属质监站，只有一级，无多级
                zjzbh = "," + zjzbh + "," + xsbm + ",";
                IList<IDictionary<string, string>> zjzjczxs = allzjzjczxs.Where(e => zjzbh.IndexOf("," + e["zjzbh"] + ",") > -1).ToList();
                var jdgcs = allgcs.Where(e => zjzbh.IndexOf(e["zjzbh"]) > -1 && e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));
                var fjdgcs = allgcs.Where(e => e["sptg"].GetSafeBool() && string.IsNullOrEmpty(e["ssjcjgbh"]));
                fjdgcs = fjdgcs.Where(e =>
                {
                    return jdgcs.Where(p => p["gcbh"].Equals(e["sjgcbh"], StringComparison.OrdinalIgnoreCase)).Count() > 0;
                });

                List<IDictionary<string, string>> validGcs = new List<IDictionary<string, string>>();
                validGcs.AddRange(jdgcs);
                validGcs.AddRange(fjdgcs);

                if (!string.IsNullOrEmpty(jdid))
                    validGcs = validGcs.Where(e => e["ssjd"].Equals(jdid)).ToList();
                else if (!string.IsNullOrEmpty(qxid))
                    validGcs = validGcs.Where(e => e["ssxq"].Equals(qxid)).ToList();
                else if (!string.IsNullOrEmpty(csid))
                    validGcs = validGcs.Where(e => e["sscs"].Equals(csid)).ToList();
                else if (!string.IsNullOrEmpty(sfid))
                    validGcs = validGcs.Where(e => e["sssf"].Equals(sfid)).ToList();
                if (!string.IsNullOrEmpty(gcid))
                    validGcs = validGcs.Where(e => e["gcbh"].Equals(gcid)).ToList();

                IList<IDictionary<string, string>> allwtds = GetMbys();
                if (!string.IsNullOrEmpty(jcjgid))
                    allwtds = allwtds.Where(e => e["ytdwbh"].Equals(jcjgid)).ToList();
                StringBuilder sbvalidgcs = new StringBuilder();
                foreach (IDictionary<string, string> gcrow in validGcs)
                    sbvalidgcs.Append(gcrow["gcbh"] + ",");
                string strvalidgcbhs = "," + sbvalidgcs.ToString() + ",";
                var validWtds = allwtds.Where(e => strvalidgcbhs.Contains(e["gcbh"])).ToList();
                IList<IDictionary<string, string>> allqys = GetImqys();


                foreach (IDictionary<string, string> qy in allqys)
                {
                    string qybh = qy["qybh"];
                    string qymc = qy["qymc"];

                    var jcjgwtds = validWtds.Where(e => e["ytdwbh"].Equals(qybh));

                    IDictionary<string, string> dwdata = new Dictionary<string, string>();

                    dwdata.Add("已委托", jcjgwtds.Where(e => e["zt"].IndexOf("W") == 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("已试验", jcjgwtds.Where(e => e["zt"].IndexOf("S") == 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("已出报告", jcjgwtds.Where(e => e["zt"].IndexOf("B") == 0).Sum(e => e["s1"].GetSafeInt()).ToString());

                    dwdata.Add("正常", jcjgwtds.Where(e => e["yczt"].GetSafeInt() == 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("委托单有修改", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 1) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("委托书字段未全部上传", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 2) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("自动采集数据有修改", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 4) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("自动采集有未保存数据", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 8) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("自动采集有重做数据", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 16) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("自动采集有重复试验", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 32) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("有重复报告", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 64) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("试验员未登记", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 128) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("试验员未到场", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 256) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("数据上传时间超差", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 512) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());
                    dwdata.Add("报告上传超差", jcjgwtds.Where(e => (e["yczt"].GetSafeInt() & 1024) > 0).Sum(e => e["s1"].GetSafeInt()).ToString());

                    ret.Add(qymc, dwdata);
                }
                
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 监管首页缓存i_m_nbry,h_zjz,i_s_zjz_jczx,i_m_qy(检测机构),i_m_gc,m_by,h_city,i_m_qyzh
        /// <summary>
        /// i_m_nbry 缓存时间300分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string,string>> GetImnbrys()
        {
            const string Key = "JCSERVICE_I_M_NBRY";
            const int InvalidMinutes = 300;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string,string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select zh,zjzbh,rybh,ryxm,lxbh from i_m_nbry");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// h_zjz 缓存时间300分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetHzjzs()
        {
            const string Key = "JCSERVICE_H_ZJZ";
            const int InvalidMinutes = 300;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select * from h_zjz");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// i_s_zjz_jczx 缓存时间300分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetIszjzjczxs()
        {
            const string Key = "JCSERVICE_I_S_ZJZ_JCZX";
            const int InvalidMinutes = 300;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    //ret = CommonDao.GetDataTable("select * from I_S_ZJZ_JCZX");
                    ret = CommonDao.GetDataTable(@"select a.zjzbh, b.qybh jcjgbh from h_zjz a
                                                    inner join view_i_m_qy_jczx b on 1 = 1 
                                                   where not exists (select 'X' from I_S_ZJZ_JYJCJG c
                                                   where a.ZJZBH = c.ZJZBH
                                                    and b.QYBH = c.JCJGBH
                                                    and c.SFYX = 1
                                                    and c.YXQS <= GETDATE() 
                                                    and c.YXQZ >= CONVERT(varchar(10), getdate(), 120))");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// i_m_qy（检测机构） 缓存时间300分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetImqys()
        {
            const string Key = "JCSERVICE_I_M_JCJG";
            const int InvalidMinutes = 300;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select qybh,qymc,sptg,sfyx from view_i_m_qy_jczx");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// i_m_gc 缓存时间60分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetImgcs()
        {
            const string Key = "JCSERVICE_I_M_GC";
            const int InvalidMinutes = 60;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select gcbh,gcmc,ssjcjgbh,sssf,sscs,ssxq,ssjd,jzmj,sptg,zjzbh,sjgcbh from i_m_gc order by gcmc");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// m_by 缓存时间30分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetMbys()
        {
            const string Key = "JCSERVICE_M_BY";
            const int InvalidMinutes = 30;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select gcbh,ytdwbh,zt,jcjg,yczt,count(*) as s1 from m_by group by gcbh,ytdwbh,zt,jcjg,yczt");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// h_city 缓存时间1000分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetHcitys()
        {
            const string Key = "JCSERVICE_H_CITY";
            const int InvalidMinutes = 1000;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select sfid,szsf,csid,szcs,xqid,szxq,jdid,szjd from h_city");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// i_m_qyzh 缓存时间60分钟
        /// </summary>
        /// <returns></returns>
        private IList<IDictionary<string, string>> GetImqyzhs()
        {
            const string Key = "JCSERVICE_I_M_QYZH";
            const int InvalidMinutes = 60;
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = HttpRuntime.Cache.Get(Key) as IList<IDictionary<string, string>>;
                if (ret == null)
                {
                    ret = CommonDao.GetDataTable("select * from I_M_QYZH");
                    if (ret != null)
                        HttpRuntime.Cache.Insert(Key, ret, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(InvalidMinutes));
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        #endregion

        #region 组合项目
        /// <summary>
        /// 组合项目自动生成子项目委托单
        /// </summary>
        /// <param name="wtdbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly =false)]        
        public bool CopyCombinationInfos(string wtdbh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 找委托单
                DataTable wtdList = CommonDao.GetDataSet("select * from m_by where recid='" + wtdbh + "'", CommandType.Text).Tables[0];
                if (wtdList.Rows.Count == 0)
                {
                    msg = "找不到委托单记录";
                    return ret;
                }
                DataRow wtdInfo = wtdList.Rows[0];
                string ytdwbh = wtdInfo["ytdwbh"].GetSafeString();
                string syxmbh = wtdInfo["syxmbh"].GetSafeString();
                // 是否组合项目，不是返回
                IList<IDictionary<string, string>> syxmList = CommonDao.GetDataTable("select xmlx from PR_M_SYXM where syxmbh='"+syxmbh+"' and ssdwbh='"+ytdwbh+"'");
                if (syxmList.Count() == 0)
                {
                    msg = "找不到单位试验项目";
                    return ret;
                }
                string xmlx = syxmList[0]["xmlx"];
                if (xmlx != "2")
                {
                    msg = "非组合项目";
                    return ret;
                }
                // 有指标得项目
                IList<IDictionary<string, string>> sWtdList = CommonDao.GetDataTable("select jcxm from s_" + syxmbh + " where byzbrecid='" + wtdbh + "' order by recid");
                StringBuilder sbChildSyxms = new StringBuilder();
                foreach (IDictionary<string, string> swtd in sWtdList)
                    sbChildSyxms.Append(swtd["jcxm"].GetSafeString() + ",");
                string childSyxms = sbChildSyxms.ToString().Trim(new char[] { ',' });
                if (childSyxms.Length == 0)
                    return true;
                childSyxms = "," + childSyxms + ",";
                // 获取编号的zdzd
                IList<IDictionary<string, string>> zdzdList = CommonDao.GetDataTable("select sjbmc,zdmc,bhms from XTZD_BY where sfbhzd=1 order by sjbmc");
                // 获取必有从表
                DataTable sbyList = CommonDao.GetDataSet("select top 1 * from s_by where byzbrecid='" + wtdbh + "' order by zh", CommandType.Text).Tables[0];
                if (sbyList.Rows.Count == 0)
                {
                    msg = "找不到必有从表记录";
                    return ret;
                }
                DataRow sbyData = sbyList.Rows[0];
                // 获取已保存的子项记录
                IList<IDictionary<string, string>> savedSubRecords = CommonDao.GetDataTable("select recid,syxmbh from m_by where sjrecid='" + wtdbh + "'");
                
                // 获取子项目
                IList<IDictionary<string, string>> childSyxmList = CommonDao.GetDataTable("select syxmbh,syxmmc from pr_m_syxm where ssdwbh='"+ytdwbh+"' and syxmbh in (select zxmbh from PR_M_ZB where zxmbh is not null and zxmbh<>'' and syxmbh='"+syxmbh+"' and recid in (select zbbh from PR_S_CP_ZB where '"+childSyxms+"' like '%,'+bgxsmc+',%'))");

                // 删除之前保存，现在没有的项目
                var deleteItems = savedSubRecords.Where(e =>
                {
                    return childSyxmList.Where(p => p["syxmbh"].Equals(e["syxbh"])).Count() == 0;
                });
                foreach (IDictionary<string,string> deleteRow in deleteItems)
                {
                    string childSyxmbh = deleteRow["syxmbh"];
                    string zbrecid = deleteRow["recid"];

                    CommonDao.ExecCommand("delete from m_" + childSyxmbh + " where recid='"+zbrecid+"'", CommandType.Text);
                    CommonDao.ExecCommand("delete from m_d_" + childSyxmbh + " where recid='"+zbrecid+"'", CommandType.Text);
                    CommonDao.ExecCommand("delete from s_" + childSyxmbh + " where byzbrecid='"+zbrecid+"'", CommandType.Text);
                    CommonDao.ExecCommand("delete from s_d_" + childSyxmbh + " where byzbrecid='"+zbrecid+"'", CommandType.Text);
                    CommonDao.ExecCommand("delete from s_by where byzbrecid='"+zbrecid+"'", CommandType.Text);
                    CommonDao.ExecCommand("delete from m_by where recid='"+zbrecid+"'", CommandType.Text);
                }
                // 必有主表赋值
                wtdInfo["SJRECID"] = wtdbh;

                // 添加不存在的子项
                foreach (IDictionary<string,string> childSyxmRow in childSyxmList)
                {
                    string childSyxmbh = childSyxmRow["syxmbh"];
                    string childSyxmmc = childSyxmRow["syxmmc"];
                    wtdInfo["SYXMBH"] = childSyxmbh;
                    wtdInfo["SYXMMC"] = childSyxmmc;
                    var findSavedItems = savedSubRecords.Where(e => e["syxmbh"].Equals(childSyxmbh, StringComparison.OrdinalIgnoreCase));
                    if (findSavedItems.Count() > 0)
                        continue;
                    string sql = "";
                    string zbrecid = "";
                    string cbrecid = "";
                    IList<IDataParameter> sqlParams = new List<IDataParameter>();

                    if (!GetInsertSql("m_by", wtdList, zdzdList, out sql, ref sqlParams, ref zbrecid))
                        throw new Exception("获取必有主表sql失败");
                    CommonDao.ExecCommand(sql, CommandType.Text, sqlParams);

                    sbyData["BYZBRECID"] = zbrecid;
                    if (!GetInsertSql("s_by", sbyList, zdzdList, out sql, ref sqlParams, ref cbrecid))
                        throw new Exception("获取必有从表sql失败");
                    CommonDao.ExecCommand(sql, CommandType.Text, sqlParams);

                    CommonDao.ExecCommand("insert into m_" + childSyxmbh + "(recid) values('"+zbrecid+"')", CommandType.Text);
                    CommonDao.ExecCommand("insert into m_d_" + childSyxmbh + "(recid) values('"+zbrecid+"')", CommandType.Text);
                    CommonDao.ExecCommand("insert into s_" + childSyxmbh + "(recid,byzbrecid) values('"+cbrecid+"','"+zbrecid+"')", CommandType.Text);
                    CommonDao.ExecCommand("insert into s_d_" + childSyxmbh + "(recid,byzbrecid) values('"+cbrecid+"','"+zbrecid+"')", CommandType.Text);

                }
                ret = true;
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;                
                ret = false;
                throw ex;
            }
            return ret;
        }

        private bool GetInsertSql(string tablename, DataTable dt, IList<IDictionary<string,string>> zdzdList, out string sql, ref IList<IDataParameter> dataParams, ref string recid)
        {
            sql = "";
            dataParams.Clear();
            if (dt == null || dt.Rows.Count == 0 || dt.Columns.Count == 0)
                return false;
            DataRow dr = dt.Rows[0];
            StringBuilder sbZb1 = new StringBuilder();
            StringBuilder sbZb2 = new StringBuilder();
                    
            foreach (DataColumn column in dt.Columns)
            {
                string key = column.ColumnName;
                sbZb1.Append(key + ",");
                sbZb2.Append("@" + key + ",");
                object objValue = dr[key];
                var findBhzds = zdzdList.Where(e => e["sjbmc"].Equals(tablename, StringComparison.OrdinalIgnoreCase) && e["zdmc"].Equals(key, StringComparison.OrdinalIgnoreCase));
                if (findBhzds.Count() > 0)
                {
                    IDictionary<string, string> zdzdRow = findBhzds.ElementAt(0);
                    string bhms = zdzdRow["bhms"];
                        bool firstOpt = false;
                    objValue = WebDataInputDao.GetBH(bhms, tablename, key, dr, null, ref firstOpt);
                    if (key.Equals("recid", StringComparison.OrdinalIgnoreCase))
                        recid = objValue.ToString();
                }
                dataParams.Add(new SqlParameter("@" + key, objValue));
            }            
            sql = "insert into "+tablename+"(" + sbZb1.ToString().Trim(new char[] { ',' }) + ") values(" + sbZb2.ToString().Trim(new char[] { ',' }) + ")";
            return true;
        }
        #endregion

        #region 二维码绑定

        /// <summary>
        /// 获取分页现场项目
        /// </summary>
        /// <param name="htxmisxm"></param>
        /// <param name="dwbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetEwmSyxmList(string dwbh, string key, 
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(key))
                    where = " and (syxmbh like '%" + key + "%' or syxmmc like '%" + key + "%')";
                ret = CommonDao.GetPageData("select syxmbh,syxmmc from PR_M_SYXM a where a.SSDWBH='" + dwbh + "' and a.sfyx=1 and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh and b.yxqs<=getdate() and b.yxqz>=convert(datetime,'" + DateTime.Now.ToString("yyyy-MM-dd") + "')) and a.SCJZQYTP=1 " + where + " order by syxmmc",
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                    row.Remove("rowstat");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
         /// <summary>
        /// 获取分页二维码委托编号
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="syxmbh"></param>
        /// <param name="key"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetEwmWtbhList(string rybh, string syxmbh, string key, 
            int pagesize, int pageindex, out int totalcount, out string msg)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            msg = "";
            totalcount = 0;
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(key))
                    where = " and (recid like '%" + key + "%' or gcmc like '%" + key + "%' or wtdbh like '%" + key + "%' or a.syxmbh like '%"+key+"%' or a.syxmmc like '%"+key+"%')";
                ret = CommonDao.GetPageData("select a.recid as ptbh,a.wtdbh,a.gcmc,b.zh,b.ypewm from m_by a inner join s_by b on a.recid=b.byzbrecid where a.gcbh in (select gcbh from i_s_gc_jlry where rybh='" + rybh + "') and (zt like 'W%') "+where,
                    pagesize, pageindex, out totalcount);
                foreach (IDictionary<string, string> row in ret)
                {
                    row.Remove("rowstat");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;            
        }
        /// <summary>
        /// 设置委托单的二维码
        /// </summary>
        /// <param name="wtds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly =false)]
        public bool SetWtbhEwm(IList<VEwmWtd> wtds, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string evms = "";
                // 去空，去内部重
                for (int i=wtds.Count-1; i>=0; i--)
                {
                    var wtd = wtds[i];
                    if (string.IsNullOrEmpty(wtd.ewm))
                    {
                        msg = wtd.wtdbh + "," + wtd.zh + ",二维码为空。";
                        wtds.RemoveAt(i);
                    }
                    else
                    {
                        var finds = wtds.Where(e => e.ewm.Equals(wtd.ewm, StringComparison.OrdinalIgnoreCase) && !(e.wtdbh == wtd.wtdbh && e.zh==wtd.zh));
                        if (finds.Count() > 0)
                        {
                            msg = wtd.wtdbh + "," + wtd.zh + ",的二维码本次提交其他委托单已用。";
                            wtds.RemoveAt(i);
                        }
                    }
                }
                // 数据库去重
                foreach (VEwmWtd item in wtds)
                {
                    evms += item.ewm + ",";
                }
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select a.recid,b.ypewm,b.zh from m_by a inner join s_by b on a.recid=b.byzbrecid where b.ypewm in (" + evms.FormatSQLInStr() + ")");
                foreach (IDictionary<string, string> row in dt)
                {
                    string ypewm = row["ypewm"];
                    string wtdbh = row["recid"];
                    string zh = row["zh"];
                    var finds = wtds.Where(e => e.ewm.Equals(ypewm, StringComparison.OrdinalIgnoreCase) && !(e.wtdbh == wtdbh && e.zh==zh));
                    foreach (var wtd in finds)
                    {
                        msg = wtd.wtdbh + "," + wtd.zh + ",的二维码已经被" + wtdbh + "使用。";
                        wtds.Remove(wtd);
                    }
                }

                foreach (VEwmWtd item in wtds)
                {
                    bool code = CommonDao.ExecCommand("update s_by set ypewm='" + item.ewm + "' where byzbrecid='" + item.wtdbh + "' and zh='" + item.zh + "'", CommandType.Text);
                    if (!code)
                        msg = item.wtdbh + "," + item.zh + ",设置二维码失败。";
                    else
                        ret = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;   
        }
        #endregion

        #region 手动上传报告
        /// <summary>
        /// 删除手动上传的报告，如果报告没有了，设置为试验状态
        /// </summary>
        /// <param name="bgwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly =false)]
        public bool DeleteReportSdsc(string bgwyh, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                //IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select count(*) as t1 from up_bgsj where wtdbh=(select wtdbh from up_bgsj where bgwyh='" + bgwyh + "') and bgwyh<>'" + bgwyh + "'");
                //bool needupdate = dt[0]["t1"].GetSafeInt() == 0;
                CommonDao.ExecCommand("delete from UP_BGWJ where BGWYH=(select bgwyh from up_bgsj where sdsc=1 and bgwyh='" + bgwyh + "')", CommandType.Text);
                CommonDao.ExecCommand("delete from up_bgsj where sdsc=1 and bgwyh='" + bgwyh + "'", CommandType.Text);

            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                code = false;
            }
            return code;
        }
        /// <summary>
        /// 获取最后一份有效报告
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public List<byte[]> GetLastReportFile(string wtdwyh, out string msg)
        {
            List<byte[]> ret = new List<byte[]>();
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select top 1 bgwyh from up_bgsj where wtdbh='" + wtdwyh + "' order by scsj desc");
                
                if (dt.Count == 0)
                {
                    msg = "报告不存在";
                    return ret;
                }

                string bgwyh = dt[0]["bgwyh"];
                IList<IDictionary<string, object>> reports = CommonDao.GetBinaryDataTable("select bgwj,osscdnurl from UP_BGWJ where bgwyh='" + bgwyh + "' order by sxh");
                
                if (reports.Count == 0)
                {
                    msg = "报告文件不存在";
                    return ret;
                }

                //目前先兼容数据库存二进制文件,转换完之后把bgwj字段备份后替换为空
                foreach (IDictionary<string, object> row in reports)
                {
                    var ossCdnUrl = row["osscdnurl"].GetSafeString();

                    if (!string.IsNullOrEmpty(ossCdnUrl))
                    {
                        var fileReturn = OssCdnHelper.GetByOssCdnUrl(ossCdnUrl, "pdf");

                        if (fileReturn.Success)
                        {
                            ret.Add(fileReturn.FileBytes);
                        }
                        else
                        {
                            msg = fileReturn.ErrorMsg;
                            return ret;
                        }
                    }
                    else
                    {
                        ret.Add(row["bgwj"] as byte[]);
                    }
                }
            }catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 系统变量

        private static IList<IDictionary<string, string>> m_SysVariables = null;
        private string GetSysSettingValue(string key)
        {
            string ret = "";
            try
            {
                if (m_SysVariables == null)
                    LoadSysVariables();
                key = key.ToLower();

                var q = from e in m_SysVariables where e["settingcode"].Equals(key, StringComparison.OrdinalIgnoreCase) && e["istemplate"].Equals("False") && e["companycode"] == "" select e;
                if (q.Count() > 0)
                    ret = q.First()["settingvalue"];

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                //ret = e.Message;
            }
            return ret;
        }

        private void LoadSysVariables()
        {
            try
            {
                m_SysVariables = CommonDao.GetDataTable("select * from syssetting");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        #region 其他设置
        /// <summary>
        /// 是否启用收样图片（0-不启用，1-启用）
        /// </summary>
        /// <returns></returns>
        public bool SysUseSytp()
        {
            return GetSysSettingValue("OTHER_SETTING_USE_SYTP").GetSafeInt() == 1;
        }
        #endregion
        #endregion

        #region 登录上传图片
        public IList<SysLogPic> SysLogPicGets(string userCode)
        {
            return SysLogPicDao.Gets(userCode);
        }

        public SysLogPic SysLogPicSave(SysLogPic sysLogPic)
        {
            return SysLogPicDao.Save(sysLogPic);
        } 
        #endregion

        #region 无见证二维码

        [Transaction(ReadOnly = false)]
        public bool NoQrcodeReq(string wtdwyh, string sqms, string username, string realName, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                if (sqms == "")
                {
                    msg = "申请描述不能为空！";
                    return ret;
                }

                //判断是否已经申请过
                string sql = "";
                int num = 0;
                //每份委托单唯一码
                string[] wtdwyhList = wtdwyh.Split(',');
                //去掉重复申请
                wtdwyhList = wtdwyhList.Distinct().ToArray();
                foreach (string item in wtdwyhList)
                {
                    sql = "select count(*) as num from I_S_JZSQ where WTDWYH = '" + item + "'";
                    IList<IDictionary<string, string>> tmpDt = CommonDao.GetDataTableTran(sql);
                    num = tmpDt[0]["num"].GetSafeInt();
                    if (num > 0)
                    {
                        msg = String.Format("{0}委托单唯一号已申请，不允许重复申请！", item);
                        return ret;
                    }
                }

                sql = string.Format(@"select a.recid, a.sszjzbh, a.sszjzmc, IsNUll(b.noqrcodeaudit,0) noqrcodeaudit
                                        from m_by a
                                        left join h_zjz b on a.sszjzbh = b.zjzbh
                                     where a.recid in ({0})", wtdwyh.FormatSQLInStr());

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    msg = "所选委托单不存在";
                    return ret;
                }

                if (dt.Where(x => string.IsNullOrEmpty(x["sszjzbh"])).Count() > 0)
                {
                    msg = "所选委托单中所属质监站不存在，不能申请无二维码";
                    return ret;
                }

                string recid = DateTime.Now.ToString("yyyyMMddhhmmssffff");
                sql = "insert into I_M_JZSQ([RECID],[SQMS],[SQSJ],[SQRZH],[SQRMC]) values('" +
                             recid + "','" + sqms + "','" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + "','" + username + "','" + realName + "')";
                CommonDao.ExecCommand(sql, CommandType.Text);
                //每份委托单唯一码
                foreach (var item in dt)
                {
                    sql = string.Format("insert into I_S_JZSQ(recid,wtdwyh,isaudit) values ('{0}', '{1}', {2})",
                        recid, item["recid"], item["noqrcodeaudit"].GetSafeBool() ? 0 : 1);
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }
                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteError("NoQrcodeReq出错：" + ex.Message);
                msg = ex.Message;
                throw ex;
            }
            return ret;
        }
        #endregion

        #region 委托单上传到oss
        /// <summary>
        /// 获取需要上传的委托单
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetUnUploadWtds()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = CommonDao.GetDataTable("select top 10 recid,syxmbh,wtsmb from m_by where scwts=1 and ssjcf>0 order by wtslrsj");
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 设置委托单上传结果
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="succedd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SetWtdUploadResult(string recid, bool succeed, ref string msg)
        {
            bool code = true;
            try
            {
                if (succeed)
                    code = CommonDao.ExecCommand("update m_by set scwtsdz='" + msg + "',scwts=0 where recid='" + recid + "'", CommandType.Text);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return code;
        }
        /// <summary>
        /// 获取已上传到oss的委托单地址
        /// </summary>
        /// <returns></returns>
        public string GetUploadWtdUrl(string recid)
        {
            string ret = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select scwts,scwtsdz from m_by where recid='" + recid + "'");
                if (dt.Count > 0)
                    ret = DataFormat.GetSafeString(dt[0]["scwtsdz"]);
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        #endregion

        #region 见证取样-标点
        /// <summary>
        /// 根据试验项目获取见证图片
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IDictionary<string, object> JzqyGetSyxmTpType(string syxmbh, out string msg)
        {
            IDictionary<string,object> ret = new Dictionary<string, object>();
            msg = "";
            try
            {
                //判断试验项目是否存在
                if (syxmbh == "")
                {
                    msg = "试验项目代号不存在！";
                    return ret;
                }
                //数据查询
                string sql = "";
                //获取见证试验项目
                sql = String.Format("select * from H_JZSYXM where SYXMBH = '{0}'", syxmbh);
                IList<IDictionary<string,string>> dt = CommonDao.GetDataTable(sql, CommandType.Text);
                //如果试验项目的见证信息不存在,则返回默认统一信息
                if (dt.Count == 0)
                {
                    sql = "select * from H_JZSYXM where SYXMBH = ''";
                    dt = CommonDao.GetDataTable(sql, CommandType.Text);
                }
                
                //判断记录数是否存在
                if (dt.Count != 1)
                {
                    msg = "见证图片数未设置！";
                    return ret;
                }
            
                //记录返回值
                foreach (IDictionary<string,string> item in dt)
                {
                    //试验项目
                    ret.Add("SYXMBH",item["SYXMBH"].GetSafeString());
                    //见证人图片
                    ret.Add("JZRTP",item["JZRTP"].GetSafeInt());
                    //二维码图片
                    ret.Add("EWMTP",item["EWMTP"].GetSafeInt());
                    //样品图片
                    ret.Add("YPTP",item["YPTP"].GetSafeInt());
                    //收样图片
                    ret.Add("SYTP",item["SYTP"].GetSafeInt());
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="qybh"></param>
        /// <param name="ryzh"></param>
        /// <param name="ryxm"></param>
        /// <param name="files"></param>
        /// <param name="ewm"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqyUpImage3(string wtdwyh, string qybh, string ryzh, string ryxm, IList<IDictionary<string, byte[]>> files, byte[] ewmBtye, string lon, string lat, string zh, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                //判断是否上传了文件
                if (files == null || files.Count == 0)
                {
                    msg = "没有图片文件，上传失败";
                    return code;
                }

                //判断二维码是否为空
                if (ewmBtye == null)
                {
                    msg = "请上传二维码图片！";
                    return code;
                }  
                //解析二维码图片
                string ewm = QrCodeHelper.ReadQrCode(ewmBtye);
                if (ewm == "")
                {
                    msg = "二维码图片不清楚，无法识别！";
                    return code;
                }
                string sql = "";
                //判断二维码是否使用过
                sql = String.Format("select * from H_QRCode where QRCode='{0}'", ewm);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql, CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "非本系统二维码！";
                    return code;
                }
                //判断二维码是否已使用
                if (dt[0]["IsUse"].GetSafeString() == "true")
                {
                    msg = "此二维码已被使用！";
                    return code;
                }

                //判断委托单唯一号及组号是否存在
                dt = CommonDao.GetDataTable("select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'", CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }
                //判断平台编号是否存在
                dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh,wtdbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }
                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能上传图片";
                    return code;
                }
                string gcbh = dt[0]["gcbh"].GetSafeString();
                string wtdbh = dt[0]["wtdbh"].GetSafeString();
                // 非自己见证的不能上传现场图片
                string jzrbh = dt[0]["jzrbh"];
                dt = CommonDao.GetDataTable("select * from I_S_GC_JZRY where rybh in (select qybh from i_m_qyzh where yhzh='" + ryzh + "') and gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "非工程见证人员，无法上传图片";
                    return code;
                }
                string updateEwm = "";
                // 判断二维码是否被使用
                dt = CommonDao.GetDataTable("select WTDWYH,ZH from up_wtdtp where not (WTDWYH = '" + wtdwyh + "' and ZH = '" + zh + "') and EWM='" + ewm + "' and SFYX=1");
                if (dt.Count() > 0)
                {
                    string tmpWtdbh = dt[0]["wtdwyh"];
                    string tmpZh = dt[0]["zh"];
                    msg = "二维码已被平台号：" + tmpWtdbh + "，委托单编号：" + wtdbh + "，组号：" + tmpZh + "的委托单使用";
                    return code;
                }
                
                // 设置必有从表获二维码
                dt = CommonDao.GetDataTable("select EWM from up_wtdtp where WTDWYH='" + wtdwyh + "' and ZH='" + zh + "'  and SFYX=1");
                foreach (IDictionary<string, string> row in dt)
                {
                    if (updateEwm.IndexOf(row["ewm"]) == -1)
                        updateEwm += row["ewm"] + ",";
                }
                if (updateEwm.IndexOf(ewm) == -1)
                    updateEwm += ewm + ",";
                updateEwm = updateEwm.Trim(new char[] { ',' });

                //单组更新二维码不能为空
                if (updateEwm == "")
                {
                    msg = "平台号：" + wtdwyh + "，组号：" + zh + "中委托单图片中二维码数据不存在！";
                    return code;
                }

                
                //遍历多种图片类型
                //判断有效类型数量
                int num = 0;
                string tplx;
                byte[] fileContent;
                List<Dictionary<string, string>> jzPics = new List<Dictionary<string, string>>();
                
                //清除记录
                foreach (IDictionary<string, byte[]> kvp in files)
                {
                    foreach (var key in kvp.Keys)
                    {
                        //获取类型
                        tplx = GetJzqyImageType(key);
                        //判断是否为空
                        if (tplx == "")
                        {
                            num++;
                            continue;
                        }

                        //获取文件二进制
                        fileContent = kvp[key];

                        Dictionary<string, string> jzPic = new Dictionary<string, string>();
                        var uploadResult = UploadJzPicOss(fileContent, tplx, out jzPic);

                        if (!uploadResult)
                        {
                            msg = "上传图片到OSS服务器出错,请重新上传";
                            return code;
                        }
                        jzPics.Add(jzPic);
                    }
                }

                //判断上传的图片是否都带有有效类别
                if (num == files.Count)
                {
                    msg = "上传的图片类型无效！";
                    return code;
                }

                //清空记录
                foreach (var jzPic in jzPics)
                {
                    sql = "update UP_WTDTP set SFYX=0 where WTDWYH='" + wtdwyh + "' and TPLX='" + jzPic["tplx"] +
                          "' and zh='" + zh + "' and SFYX=1 and ewm='" + ewm + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    sql = "update UP_WTDTPXQ set SFYX=0 where TPWYH in (select TPWYH from UP_WTDTP where WTDWYH='" +
                          wtdwyh + "' and TPLX='" + jzPic["tplx"] + "' and SFYX=0 and zh='" + zh + "') and SFYX=1";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //添加记录
                foreach (var jzPic in jzPics)
                {
                    // -- 保存图片之类
                    string tpwyh = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(ewm))
                        ewm = tpwyh;

                    sql = string.Format(
                        "insert into UP_WTDTP(TPWYH,WTDWYH,SCSJ,SCR,SCRXM,SFYX,TPLX,zh,ewm,longitude,latitude) values('{0}','{1}',getdate(),'{2}','{3}',1,'{4}','{5}','{6}',{7},{8})",
                        tpwyh, wtdwyh, ryzh, ryxm, jzPic["tplx"], zh, ewm.GetSafeString(), lon.GetSafeDecimal(),
                        lat.GetSafeDecimal());
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    //保存见证图片
                    InsertJzPic(jzPic, tpwyh, ryzh, ryxm, jzPic["tplx"]);
                }

                //设置二维码库已使用
                sql = String.Format("update H_QRCode set IsUse=1 where QRCode = '{0}'", ewm);
                CommonDao.ExecCommand(sql, CommandType.Text);

                //更新从表一组的二维码信息
                if (!string.IsNullOrEmpty(ewm))
                {
                    sql = "update s_by set ypewm='" + updateEwm + "' where byzbrecid='" + wtdwyh + "' and zh='" + zh + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //设置单组完成状态
                sql = "update s_by set YPYSC=1  where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                //设置委托单状态
                char newZt = WtsStatus.JzStateTp1;
                if (status.NeedUpdateImageJzzt(newZt))
                {
                    // 是否所有组都上传了现场图片
                    int sSum = 0, tpSum = 0;
                    IList<IDictionary<string, string>> tmpDt = CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' ");
                    sSum = tmpDt[0]["c1"].GetSafeInt();
                    tmpDt = CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' and isnull(YPYSC,0) = 1");
                    tpSum = tmpDt[0]["c1"].GetSafeInt();
                    if (tpSum == sSum)
                    {
                        status.SetWtdJzqyzt(newZt, out msg);
                        sql = "update m_by set zt='" + status.GetStatus() + "' where recid='" + wtdwyh + "'";
                        CommonDao.ExecCommand(sql, CommandType.Text);
                    }
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return code;
        }

        /// <summary>
        /// 见证人上传现场图片
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="qybh"></param>
        /// <param name="ryzh"></param>
        /// <param name="ryxm"></param>
        /// <param name="files"></param>
        /// <param name="ewm"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="zh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool JzqyUpImage4(string wtdwyh, string qybh, string ryzh, string ryxm, IList<IDictionary<string, byte[]>> files, string ewm, string lon, string lat, string zh, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                //判断是否上传了文件
                if (files == null || files.Count == 0)
                {
                    msg = "没有图片文件，上传失败";
                    return code;
                }

                //判断二维码是否为空
                if (ewm == "")
                {
                    msg = "二维码不能为空！";
                    return code;
                }

                string sql = "";
                //判断二维码是否使用过
                sql = String.Format("select * from H_QRCode where QRCode='{0}'", ewm);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql, CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "非本系统二维码！";
                    return code;
                }
                //判断二维码是否已使用
                if (dt[0]["IsUse"].GetSafeString() == "true")
                {
                    msg = "此二维码已被使用！";
                    return code;
                }

                //判断委托单唯一号及组号是否存在
                dt = CommonDao.GetDataTable("select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'", CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }
                //判断平台编号是否存在
                dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh,wtdbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }
                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能上传图片";
                    return code;
                }
                string gcbh = dt[0]["gcbh"].GetSafeString();

                // 非自己见证的不能上传现场图片
                string jzrbh = dt[0]["jzrbh"];
                dt = CommonDao.GetDataTable("select * from I_S_GC_JZRY where rybh in (select qybh from i_m_qyzh where yhzh='" + ryzh + "') and gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "非工程见证人员，无法上传图片";
                    return code;
                }
                string updateEwm = "";
                // 判断二维码是否被使用
                dt = CommonDao.GetDataTable("select WTDWYH,ZH from up_wtdtp where not (WTDWYH = '" + wtdwyh + "' and ZH = '" + zh + "') and EWM='" + ewm + "' and SFYX=1");
                if (dt.Count() > 0)
                {
                    string tmpWtdbh = dt[0]["wtdwyh"];
                    string tmpZh = dt[0]["zh"];

                    //获取委托单编号
                    string tmpWtdh = "";
                    dt = CommonDao.GetDataTable("select wtdbh from m_by where  recid='" + tmpWtdbh + "'", CommandType.Text);
                    if (dt.Count > 0)
                    {
                        tmpWtdh = dt[0]["wtdbh"].GetSafeString();
                    }

                    msg = "二维码已被平台号：" + tmpWtdbh + "，委托单编号：" + tmpWtdh + "，组号：" + tmpZh + "的委托单使用";
                    return code;
                }
              
                // 设置必有从表获二维码
                dt = CommonDao.GetDataTable("select EWM from up_wtdtp where WTDWYH='" + wtdwyh + "' and ZH='" + zh + "'  and SFYX=1");
                foreach (IDictionary<string, string> row in dt)
                {
                    if (updateEwm.IndexOf(row["ewm"]) == -1)
                        updateEwm += row["ewm"] + ",";
                }
                if (updateEwm.IndexOf(ewm) == -1)
                    updateEwm += ewm + ",";
                updateEwm = updateEwm.Trim(new char[] { ',' });

                //单组更新二维码不能为空
                if (updateEwm == "")
                {
                    msg = "平台号：" + wtdwyh + "，组号：" + zh + "中委托单图片中二维码数据不存在！";
                    return code;
                }

                //遍历多种图片类型
                //判断有效类型数量
                int num = 0;
                string tplx;
                byte[] fileContent;
                List<Dictionary<string, string>> jzPics = new List<Dictionary<string, string>>();

                foreach (IDictionary<string, byte[]> kvp in files)
                {
                    foreach (var key in kvp.Keys)
                    {
                        //获取类型
                        tplx = GetJzqyImageType(key);
                        //判断是否为空
                        if (tplx == "")
                        {
                            num++;
                            continue;
                        }
                        
                        //获取文件二进制
                        fileContent = kvp[key];

                        Dictionary<string, string> jzPic = new Dictionary<string, string>();
                        var uploadResult = UploadJzPicOss(fileContent, tplx, out jzPic);

                        if (!uploadResult)
                        {
                            msg = "上传图片到OSS服务器出错,请重新上传";
                            return code;
                        }
                        jzPics.Add(jzPic);
                    }
                }
                //判断上传的图片是否都带有有效类别
                if (num == files.Count)
                {
                    msg = "上传的图片类型无效！";
                    return code;
                }

                //清空记录
                foreach (var jzPic in jzPics)
                {
                    sql = "update UP_WTDTP set SFYX=0 where WTDWYH='" + wtdwyh + "' and TPLX='" + jzPic["tplx"] +
                          "' and zh='" + zh + "' and SFYX=1 and ewm='" + ewm + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    sql = "update UP_WTDTPXQ set SFYX=0 where TPWYH in (select TPWYH from UP_WTDTP where WTDWYH='" +
                          wtdwyh + "' and TPLX='" + jzPic["tplx"] + "' and SFYX=0 and zh='" + zh + "') and SFYX=1";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //添加记录
                foreach (var jzPic in jzPics)
                {
                    // -- 保存图片之类
                    string tpwyh = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(ewm))
                        ewm = tpwyh;

                    sql = string.Format(
                        "insert into UP_WTDTP(TPWYH,WTDWYH,SCSJ,SCR,SCRXM,SFYX,TPLX,zh,ewm,longitude,latitude) values('{0}','{1}',getdate(),'{2}','{3}',1,'{4}','{5}','{6}',{7},{8})",
                        tpwyh, wtdwyh, ryzh, ryxm, jzPic["tplx"], zh, ewm.GetSafeString(), lon.GetSafeDecimal(),
                        lat.GetSafeDecimal());
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    //保存见证图片
                    InsertJzPic(jzPic, tpwyh, ryzh, ryxm, jzPic["tplx"]);
                }

                //设置二维码库已使用
                sql = String.Format("update H_QRCode set IsUse=1 where QRCode = '{0}'", ewm);
                CommonDao.ExecCommand(sql, CommandType.Text);

                //更新从表一组的二维码信息
                if (!string.IsNullOrEmpty(ewm))
                {
                    sql = "update s_by set ypewm='" + updateEwm + "' where byzbrecid='" + wtdwyh + "' and zh='" + zh + "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                //设置单组完成状态
                sql = "update s_by set YPYSC=1  where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                //设置委托单状态
                char newZt = WtsStatus.JzStateTp1;
                if (status.NeedUpdateImageJzzt(newZt))
                {
                    // 是否所有组都上传了现场图片
                    int sSum = 0, tpSum = 0;
                    IList<IDictionary<string, string>> tmpDt = CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' ");
                    sSum = tmpDt[0]["c1"].GetSafeInt();
                    tmpDt = CommonDao.GetDataTableTran("select count(*) as c1 from s_by where byzbrecid='" + wtdwyh + "' and isnull(YPYSC,0) = 1");
                    tpSum = tmpDt[0]["c1"].GetSafeInt();
                    if (tpSum == sSum)
                    {
                        status.SetWtdJzqyzt(newZt, out msg);
                        sql = "update m_by set zt='" + status.GetStatus() + "' where recid='" + wtdwyh + "'";
                        CommonDao.ExecCommand(sql, CommandType.Text);
                    }
                }
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }
            return code;
        }

        [Transaction(ReadOnly = false)]
        public bool JzqyUpImage5(string wtdwyh, string qybh, string ryzh, string ryxm, IList<byte[]> files, string ewm,
            string lon, string lat, string zh, string type, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string tplx = GetJzqyImageType(type);

                if (tplx == "")
                {
                    msg = "图片类型传入出错";
                    return code;
                }

                if (files == null || files.Count == 0)
                {
                    msg = "没有图片文件，上传失败";
                    return code;
                }

                string sql = "";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select * from s_by where  byzbrecid='" + wtdwyh + "' and zh='" + zh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的组号";
                    return code;
                }

                dt = CommonDao.GetDataTable("select zt,jzrbh,gcbh from m_by where  recid='" + wtdwyh + "'",
                    CommandType.Text);
                if (dt.Count == 0)
                {
                    msg = "无效的平台编号";
                    return code;
                }

                string zt = dt[0]["zt"].GetSafeString();
                WtsStatus status = new WtsStatus(zt);
                if (!status.CanUpXcpt)
                {
                    msg = "已经完成见证，不能上传图片";
                    return code;
                }

                string gcbh = dt[0]["gcbh"].GetSafeString();

                // 非自己见证的不能上传现场图片
                string jzrbh = dt[0]["jzrbh"];
                dt = CommonDao.GetDataTable(
                    "select * from I_S_GC_JZRY where rybh in (select qybh from i_m_qyzh where yhzh='" + ryzh +
                    "') and gcbh='" + gcbh + "'");
                if (dt.Count == 0)
                {
                    msg = "非工程见证人员，无法上传图片";
                    return code;
                }

                string updateEwm = "";
                // 判断二维码是否被使用
                if (!string.IsNullOrEmpty(ewm))
                {
                    string debugSql = "select WTDWYH,ZH from up_wtdtp where not (WTDWYH ='" + wtdwyh + "' and ZH ='" + zh +
                                 "') and EWM='" + ewm + "' and SFYX=1";
                    dt = CommonDao.GetDataTable(debugSql);
                    if (dt.Count() > 0)
                    {
                        string tmpWtdbh = dt[0]["wtdwyh"];
                        string tmpZh = dt[0]["zh"];
                        msg = "二维码【" + ewm + "】已被平台号：" + tmpWtdbh + "，组号：" + tmpZh + "的委托单使用";
                        SysLog4.WriteError(String.Format("JzqyUpImage5：{0}，SQL：{1}", msg, debugSql));
                        return code;
                    }

                    // 设置必有从表获二维码
                    dt = CommonDao.GetDataTable("select EWM from up_wtdtp where WTDWYH='" + wtdwyh + "' and ZH='" + zh +
                                                "'  and SFYX=1");
                    foreach (IDictionary<string, string> row in dt)
                    {
                        if (updateEwm.IndexOf(row["ewm"]) == -1)
                            updateEwm += row["ewm"] + ",";
                    }

                    if (updateEwm.IndexOf(ewm) == -1)
                        updateEwm += ewm + ",";
                    updateEwm = updateEwm.Trim(new char[] { ',' });
                }

                //先上传到Oss, 防止表阻塞
                List<Dictionary<string, string>> jzPics = new List<Dictionary<string, string>>();

                foreach (var file in files)
                {
                    Dictionary<string, string> jzPic = new Dictionary<string, string>();
                    var uploadResult = UploadJzPicOss(file, tplx, out jzPic);

                    if (!uploadResult)
                    {
                        msg = "上传图片到OSS服务器出错,请重新上传";
                        return code;
                    }
                    jzPics.Add(jzPic);
                }

                sql = "update UP_WTDTP set SFYX=0 where WTDWYH='" + wtdwyh + "' and TPLX='" + tplx + "' and zh='" + zh +
                      "' and SFYX=1 and ewm='" + ewm + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);

                sql = "update UP_WTDTPXQ set SFYX=0 where TPWYH in (select TPWYH from UP_WTDTP where WTDWYH='" +
                      wtdwyh + "' and TPLX='" + tplx + "' and SFYX=0 and zh='" + zh + "') and SFYX=1";
                CommonDao.ExecCommand(sql, CommandType.Text);

                if (!string.IsNullOrEmpty(ewm))
                {
                    sql = "update s_by set ypewm='" + updateEwm + "' where byzbrecid='" + wtdwyh + "' and zh='" + zh +
                          "'";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                // -- 保存图片之类
                string tpwyh = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(ewm))
                    ewm = tpwyh;

                sql = string.Format(
                    "insert into UP_WTDTP(TPWYH,WTDWYH,SCSJ,SCR,SCRXM,SFYX,TPLX,zh,ewm,longitude,latitude) values('{0}','{1}',getdate(),'{2}','{3}',1,'{4}','{5}','{6}',{7},{8})",
                    tpwyh, wtdwyh, ryzh, ryxm, tplx, zh, ewm.GetSafeString(), lon.GetSafeDecimal(),
                    lat.GetSafeDecimal());
                CommonDao.ExecCommand(sql, CommandType.Text);

                //插入图片信息
                foreach (var jzPic in jzPics)
                {
                    InsertJzPic(jzPic, tpwyh, ryzh, ryxm, tplx);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }

        #region 公共参数
        /// <summary>
        /// 获取见证取样的图片类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetJzqyImageType(string type)
        {
            string tplx = "";
            //获取类型
            switch (type)
            {
                //见证人图片
                case "jzrtp":
                    tplx = "JZ01";
                    break;
                //二维码图片
                case "ewmtp":
                    tplx = "JZ03";
                    break;
                //样品图片
                case "yptp":
                    tplx = "JZ04";
                    break;
                //收样图片
                case "sytp":
                    tplx = "JZ02";
                    break;
            }
            return tplx;
        }

        #endregion
        #endregion

        #region 更新委托单确定下载
        /// <summary>
        /// 更新委托单确定下载
        /// </summary>
        /// <param name="recids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateConfirmDownload(string recids, int qrxz, out string msg)
        {
            bool code = true;
            msg = string.Empty;

            try
            {
                code = CommonDao.ExecSql(string.Format("update m_by set qrxz={0} where recid in ({1})", qrxz, recids.FormatSQLInStr()));
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        } 
        #endregion

        #region 获取需要上传到Oss上的报告
        /// <summary>
        /// 获取需要上传到Oss上的报告
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, object>> GetUploadReport()
        {
            IList<IDictionary<string, object>> result = new List<IDictionary<string, object>>();

            try
            {
                result = CommonDao.GetBinaryDataTable("select top 5 a.* from up_bgwj a left join up_bgsj b on a.BGWYH = b.BGWYH where a.IsUpload = 0 order by b.scsj desc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return result;
        } 
        #endregion

        #region 设置报告上传到Oss成功的结果
        /// <summary>
        /// 设置报告上传到Oss成功的结果
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="succedd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetUploadReportResult(string bgwyh, int sxh, string ossCdnUrl)
        {
            bool code = true;
            try
            {
                string sql = string.Format(@"update up_bgwj set isUpload= 1, ossCdnUrl='{0}' 
                             where bgwyh='{1}' and sxh={2}", ossCdnUrl, bgwyh, sxh);
                code = CommonDao.ExecSql(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
            }
            return code;
        } 
        #endregion
        
        #region 获取需要上传到Oss上的见证图片
        /// <summary>
        /// 获取需要上传到Oss上的见证图片
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, object>> GetUploadJzPic()
        {
            IList<IDictionary<string, object>> result = new List<IDictionary<string, object>>();

            try
            {
                result = CommonDao.GetBinaryDataTable("select top 5 * from up_wtdtpxq where isUpload=0 order by scsj desc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return result;
        } 
        #endregion

        #region 设置见证图片上传到Oss成功的结果
        /// <summary>
        /// 设置见证图片上传到Oss成功的结果
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="succedd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetUploadJzPicResult(string tpxqwyh, string ossCdnUrl)
        {
            bool code = true;
            try
            {
                string sql = string.Format(@"update up_wtdtpxq set isUpload=1, ossCdnUrl='{0}' 
                             where tpxqwyh='{1}'", ossCdnUrl, tpxqwyh);
                code = CommonDao.ExecSql(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
            }
            return code;
        }
        #endregion

        #region 微信图片上传

        /// <summary>
        /// 下载微信图片
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        public ResultParam DownloadWeiXinImage(string mediaid)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //判断媒体ID是否存在
                if (mediaid == "")
                {
                    ret.msg = "媒体ID不能为空！";
                    return ret;
                }
                //下载
                HttpHelper http = new HttpHelper();
                HttpItem item = new HttpItem()
                {
                    URL = String.Format(WeiXinHelper.mediaUrl, WeiXinHelper.getAccessToken(), mediaid),//URL     必需项
                    Encoding = null,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                    //Encoding = Encoding.Default,
                    ResultType = ResultType.Byte
                };
                //得到HTML代码
                HttpResult result = http.GetHtml(item);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ret.msg = String.Format("获取图片失败，原因：{0}", result.StatusDescription);
                    return ret;
                }
                //把得到的Byte转成字符串
                ret.data = Convert.ToBase64String(result.ResultByte);
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        #endregion

        #region 上传单个见证图片
        /// <summary>
        /// 上传单个见证图片
        /// </summary>
        /// <param name="filebytes"></param>
        /// <param name="tpwyh"></param>
        /// <param name="ryzh"></param>
        /// <param name="ryxm"></param>
        /// <param name="tplx"></param>
        private void UploadJzPic(byte[] filebytes, string tpwyh, string ryzh, string ryxm, string tplx)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();

            string tpxqwyh = Guid.NewGuid().ToString();
            var sql = string.Format("insert into UP_WTDTPXQ(TPXQWYH,TPWYH,TPNR,SFYX,SCSJ,SCR,SCRXM,TPLX,ossCdnUrl,isUpload) values('{0}','{1}',@tpnr,1,getdate(),'{2}','{3}','{4}', @ossCdnUrl, @isUpload)",
                tpxqwyh, tpwyh, ryzh, ryxm, tplx);

            IList<IDataParameter> arrParams = new List<IDataParameter>();

            OSS_CDN oss = new OSS_CDN(Configs.FileOssCdn);
            var result = oss.UploadFile(Configs.OssCdnCodeJz, filebytes, string.Format("jz_{0}.jpg", tpxqwyh));

            //如果没有上传到OSS上，通过后台线程去上传
            if (result.success)
            {
                arrParams.Add(new SqlParameter("@tpnr", SqlDbType.VarBinary) { Value = new byte[] { 0x01 } });
                arrParams.Add(new SqlParameter("@ossCdnUrl", result.Url));
                arrParams.Add(new SqlParameter("@isUpload", 1));
            }
            else
            {
                arrParams.Add(new SqlParameter("@tpnr", SqlDbType.VarBinary) { Value = filebytes });
                arrParams.Add(new SqlParameter("@ossCdnUrl", string.Empty));
                arrParams.Add(new SqlParameter("@isUpload", 0));
            }

            CommonDao.ExecCommand(sql, CommandType.Text, arrParams);

            sw.Stop();
            SysLog4.WriteError(tpxqwyh + "上传见证图片耗时:" + sw.ElapsedMilliseconds + "毫秒\r\n");
        }
        #endregion

        #region 先将见证图片上传到OSS, 防止表阻塞
        private bool UploadJzPicOss(byte[] img, string tplx, out Dictionary<string, string> jzPic)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();

            jzPic = new Dictionary<string, string>();
            string tpxqwyh = Guid.NewGuid().ToString();
            jzPic.Add("tpxqwyh", tpxqwyh);
            jzPic.Add("tplx", tplx);

            OSS_CDN oss = new OSS_CDN(Configs.FileOssCdn);
            var result = oss.UploadFile(Configs.OssCdnCodeJz, img, string.Format("jz_{0}.jpg", tpxqwyh));

            if (result.success)
            {
                jzPic.Add("osscdnurl", result.Url);
            }
            else
            {
                return false;
            }

            sw.Stop();
            SysLog4.WriteError(tpxqwyh + "上传见证图片耗时:" + sw.ElapsedMilliseconds + "毫秒\r\n");

            return true;
        }

        private void InsertJzPic(Dictionary<string, string> jzPic, string tpwyh, string ryzh, string ryxm, string tplx)
        {
            var sql = string.Format("insert into UP_WTDTPXQ(TPXQWYH,TPWYH,TPNR,SFYX,SCSJ,SCR,SCRXM,TPLX,ossCdnUrl,isUpload) values('{0}','{1}',@tpnr,1,getdate(),'{2}','{3}','{4}', @ossCdnUrl, @isUpload)",
                 jzPic["tpxqwyh"], tpwyh, ryzh, ryxm, tplx);

            IList<IDataParameter> arrParams = new List<IDataParameter>();

            arrParams.Add(new SqlParameter("@tpnr", SqlDbType.VarBinary) { Value = new byte[] { 0x01 } });
            arrParams.Add(new SqlParameter("@ossCdnUrl", jzPic["osscdnurl"]));
            arrParams.Add(new SqlParameter("@isUpload", 1));

            CommonDao.ExecCommand(sql, CommandType.Text, arrParams);
        } 
        #endregion

        #region 获取质监站下的所有检测机构
        /// <summary>
        /// 获取质监站下的所有检测机构
        /// </summary>
        /// <param name="ryzh"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetJcjgs(string stationid, out IList<IDictionary<string, string>> records, out string msg)
        {
            bool ret = false;
            msg = "";
            records = new List<IDictionary<string, string>>();
            try
            {
                var zjzbh = CommonDao.GetSingleData("select top 1 extrainfo1 from i_m_call where stationid='" + stationid + "'").GetSafeString();

                if (string.IsNullOrEmpty(zjzbh))
                {
                    msg = "未配置对应质监站代码";
                }
                else
                {
                    string sql = string.Format(@"select b.QYBH, b.QYMC
	                                                from I_S_ZJZ_JCZX a
	                                                inner join i_m_qy b on a.JCJGBH = b.QYBH
	                                                where a.SFYX = 1
	                                                and a.YXQS <= GETDATE()
	                                                and a.YXQZ >= CONVERT(datetime, CONVERT(varchar(50), getdate(), 23))
	                                                and a.ZJZBH = '{0}'", zjzbh);

                    records = CommonDao.GetDataTable(sql);
                    ret = true;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        } 
        #endregion

        #region 根据检测机构获取试验项目
        /// <summary>
        /// 根据检测机构获取试验项目
        /// </summary>
        /// <param name="ryzh"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetSyxms(string stationid, string qybh, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = false;
            msg = "";
            records = new List<IDictionary<string, object>>();
            try
            {
                var zjzbh = CommonDao.GetSingleData("select top 1 extrainfo1 from i_m_call where stationid='" + stationid + "'").GetSafeString();

                if (string.IsNullOrEmpty(zjzbh))
                {
                    msg = "未配置对应质监站代码";
                }
                else
                {
                    string xsflSql = string.Format(@"select sjxsflbh,xsflbh,xsflmc from pr_m_syxmxsfl where ssdwbh= '{0}' order by xssx", qybh);

                    var xsfls = CommonDao.GetDataTable(xsflSql);

                    string syxmSql = string.Format(@"select a.xsflbh,a.syxmbh,a.syxmmc
                                                        from pr_m_syxm a
                                                        where a.ssdwbh= '{0}'
                                                        and a.xmlx<>'3' 
                                                        and a.sfyx=1
                                                        and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh ) 
                                                        order by a.xsflbh,a.xssx", qybh);

                    var syxms = CommonDao.GetDataTable(syxmSql);

                    var firstXsfls = xsfls.Where(x => x["sjxsflbh"] == "").ToList();

                    foreach (var firstXsfl in firstXsfls)
                    {
                        IDictionary<string, object> dict = new Dictionary<string, object>();

                        var secondXsfls = xsfls.Where(x => x["sjxsflbh"] == firstXsfl["xsflbh"]).ToList();
                        IList<IDictionary<string, object>> secondList = new List<IDictionary<string, object>>();

                        foreach (var secondXsfl in secondXsfls)
                        {
                            var subSyxms = syxms.Where(x => x["xsflbh"] == secondXsfl["xsflbh"]).ToList();

                            if (subSyxms.Count() > 0)
                            {
                                IDictionary<string, object> secondDict = new Dictionary<string, object>();
                                secondDict.Add("sjxsflbh", secondXsfl["sjxsflbh"]);
                                secondDict.Add("xsflbh", secondXsfl["xsflbh"]);
                                secondDict.Add("xsflmc", secondXsfl["xsflmc"]);
                                secondDict.Add("syxms", subSyxms);

                                secondList.Add(secondDict);
                            }
                        }

                        if (secondList.Count() > 0)
                        {
                            dict.Add("sjxsflbh", firstXsfl["sjxsflbh"]);
                            dict.Add("xsflbh", firstXsfl["xsflbh"]);
                            dict.Add("xsflmc", firstXsfl["xsflmc"]);
                            dict.Add("subs", secondList);
                            records.Add(dict);
                        }    
                    }

                    ret = true;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }
        #endregion

        #region 委托单取消见证
        /// <summary>
        /// 取消见证
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool CancelWtdJz(string wtdwyh, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string zt  = CommonDao.GetSingleData("select top 1 zt from m_by where recid='" + wtdwyh + "'").GetSafeString();
                
                if (string.IsNullOrEmpty(zt))
                {
                    msg = "无效的平台编号";
                    return code;
                }

                WtsStatus status = new WtsStatus(zt);

                if (status.HasWtdDown)
                {
                    msg = "委托单已送样，不能取消见证信息";
                    return code;
                }

                if (!status.HasWtdJz)
                {
                    msg = "委托单未见证，不能取消见证信息";
                    return code;
                }

                int count = CommonDao.GetSingleData("select count(1) from up_wtdtp where wtdwyh = '" + wtdwyh + "' and sfyx = 1 ").GetSafeInt();

                if (count == 0)
                {
                    msg = "委托单唯一号[" + wtdwyh + "]的见证信息不存在";
                    return code;
                }

                status.SetWtdJzqyzt(WtsStatus.JzStateNo, out msg);

                string sql = string.Format("update m_by set zt = '{0}' where recid = '{1}'", status.GetStatus(), wtdwyh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                sql = string.Format("update s_by set YPYSC = 0, ypewm = '' where BYZBRECID = '{0}'", wtdwyh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                sql = string.Format("update up_wtdtp set wtdwyh = '', oldwtdwyh = '{0}', qxr = '{1}', qxrxm = '{2}', qxsj = getdate() where wtdwyh = '{3}' and sfyx = 1", wtdwyh, CurrentUser.UserCode, CurrentUser.RealName, wtdwyh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion

        #region 委托单关联见证
        /// <summary>
        /// 委托单关联见证
        /// </summary>
        /// <param name="wtdwyh">委托单唯一号</param>
        /// <param name="oldwtdwyh">关联的委托单唯一号</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool ContactWtdJz(string wtdwyh, string oldwtdwyh, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string zt = CommonDao.GetSingleData("select top 1 zt from m_by where recid='" + wtdwyh + "'").GetSafeString();

                if (string.IsNullOrEmpty(zt))
                {
                    msg = "无效的平台编号";
                    return code;
                }

                WtsStatus status = new WtsStatus(zt);

                if (status.HasWtdDown)
                {
                    msg = "委托单已送样，不能关联见证信息";
                    return code;
                }

                if (status.HasWtdJz)
                {
                    msg = "委托单已见证，不能关联见证信息";
                    return code;
                }

                var dts = CommonDao.GetDataTable("select distinct zh,ewm,qxr from up_wtdtp where oldwtdwyh = '" + oldwtdwyh + "' and sfyx = 1 ");

                if (dts.Count == 0)
                {
                    msg = "委托单唯一号[" + oldwtdwyh + "]的见证信息不存在, 请先取消该委托单的见证信息";
                    return code;
                }
                else
                {
                    if (dts[0]["qxr"] != CurrentUser.UserCode)
                    {
                        msg = "委托单唯一号[" + oldwtdwyh + "]取消见证的人和当前登录用户不一致，不允许操作";
                        return code;
                    }
                }
                
                status.SetWtdJzqyzt(WtsStatus.JzStateTp1, out msg);

                string sql = string.Format("update m_by set zt = '{0}' where recid = '{1}'", status.GetStatus(), wtdwyh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                foreach (var dt in dts)
                {
                    sql = string.Format("update s_by set YPYSC = 1, ypewm = '{0}' where BYZBRECID = '{1}' and zh = '{2}'", dt["ewm"], wtdwyh, dt["zh"]);
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                sql = string.Format("update up_wtdtp set wtdwyh = '{0}', oldwtdwyh = '' where oldwtdwyh = '{1}'", wtdwyh, oldwtdwyh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion

        #region 查询当前用户可以关联的委托单
        /// <summary>
        /// 查询当前用户可以关联的委托单
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetContactWtd(out string msg, out string wtdwyhs)
        {
            bool code = false;
            msg = string.Empty;
            wtdwyhs = string.Empty;

            try
            {
                string sql = string.Format("select distinct oldwtdwyh from up_wtdtp where oldwtdwyh <> '' and qxr = '{0}'", CurrentUser.UserCode);

                var dts = CommonDao.GetDataTable(sql);

                foreach (var dt in dts)
                {
                    wtdwyhs += dt["oldwtdwyh"] + ",";
                }

                if (!string.IsNullOrEmpty(wtdwyhs))
                {
                    wtdwyhs = wtdwyhs.TrimEnd(',');
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion

        #region 微信端
        /// <summary>
        /// 二维码扫描
        /// </summary>
        /// <param name="bgwyh"></param>
        /// <returns></returns>
        public ResultParam QRAntiFake(string bgwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //判断唯一号是否存在
                if (bgwyh == "")
                {
                    ret.msg = "无效参数！";
                    return ret;
                }
                string wtdwyh = GetWtdbh(bgwyh);
                if (string.IsNullOrEmpty(wtdwyh))
                {
                    ret.msg = "当前编号的报告还没有生成,无法查询!";
                    return ret;
                }
                if (IsWtdZf(wtdwyh))
                {
                    ret.msg = "委托单已作废!";
                    return ret;
                }
                //定义SQL
                StringBuilder sql = new StringBuilder();
                //判断唯一号记录是否存在
                sql.AppendFormat("select syxmbh from m_by where recid = '{0}'", wtdwyh);
                IList<IDictionary<string,string>> dt = CommonDao.GetDataTable(sql.ToString(), CommandType.Text);
                if (dt.Count == 0)
                {
                    ret.msg = "二维码所对应的委托单信息不存在！";
                    return ret;
                }
                //判断报告是否已经上传
                sql.Clear();
                sql.AppendFormat("select bgwyh,bgbh from up_bgsj where wtdbh='{0}' and bgewm<>'' order by scsj desc",wtdwyh);
                dt = CommonDao.GetDataTable(sql.ToString(), CommandType.Text);
                if (dt.Count == 0)
                {
                    ret.msg = "没有生成二维码的对应报告摘要！";
                    return ret;
                }      
                //获取最后一份报告文件
                sql.Clear();
                sql.AppendFormat("select top 1 b.bgbh,a.bgwj,a.osscdnurl from up_bgwj a inner join up_bgsj b on a.BGWYH = b.BGWYH where b.wtdbh='{0}' order by b.scsj desc", wtdwyh);
                dt = CommonDao.GetDataTable(sql.ToString(), CommandType.Text);
                if (dt.Count == 0)
                {
                    ret.msg = "二维码所对应的报告文件不存在！";
                    return ret;
                }    
                //下载PDF图片
                string pdfFileStr = null;
                try
                {
                    string pdfCdnUrl = dt[0]["osscdnurl"].GetSafeString();
                    //下载
                    HttpHelper http = new HttpHelper();
                    HttpItem item = new HttpItem()
                    {
                        URL = pdfCdnUrl,
                        Encoding = null,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                        //Encoding = Encoding.Default,
                        ResultType = ResultType.Byte
                    };
                    //得到HTML代码
                    HttpResult result = http.GetHtml(item);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ret.msg = String.Format("获取报告文件失败，原因：{0}", result.StatusDescription);
                        return ret;
                    }
                    //把得到的Byte转成字符串
                    pdfFileStr =  Convert.ToBase64String(result.ResultByte);
                }
                catch (Exception ex)
                {
                    SysLog4.WriteError(String.Format("获取报告文件出错，原因： {0}", ex.Message));
                    ret.msg = String.Format("获取报告文件出错，原因： {0}", ex.Message);
                }
                //生成图片缩略图
                byte[] imgByte = null;
                try
                {
                    string imgFileStr = "";
                    string msg = "";
                    //生成缩略图
                    if (!new OfficeConvert().ConvertPdfToPicStr(pdfFileStr + "|1|1", out imgFileStr, out msg))
                    {
                        ret.msg = msg;
                    }
                    string imgStr = imgFileStr.Split('|')[0];
                    imgByte = Convert.FromBase64String(imgStr);
                }
                catch (Exception ex)
                {
                    SysLog4.WriteError(String.Format("生成报告缩略图出错，原因： {0}", ex.Message));
                    ret.msg = String.Format("生成报告缩略图出错，原因： {0}", ex.Message);
                }
                //保存缩略图
                string imgId = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
                try
                {
                    BD.DataInputModel.Entities.DataFile dataFileModel = new BD.DataInputModel.Entities.DataFile();
                    //文件代码
                    dataFileModel.FILEID = imgId;
                    //文件内容
                    dataFileModel.FILECONTENT = imgByte;
                    //OSS对象存储
                    dataFileModel.STORAGETYPE = "";
                    //文件名
                    dataFileModel.FILENAME = String.Format("{0}.jpg", imgId);
                    //扩展名
                    dataFileModel.FILEEXT = ".jpg";
                    //上传时间
                    dataFileModel.CJSJ = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
                    //保存
                    DataFileDao.SaveFile(dataFileModel);
                }
                catch (Exception ex)
                {
                    SysLog4.WriteError(String.Format("保存缩略图出错，原因： {0}", ex.Message));
                    ret.msg = String.Format("保存缩略图出错，原因： {0}", ex.Message);
                }

                //返回
                IDictionary<string,string> map = new Dictionary<string, string>();
                //报告展示地址
                map.Add("reportShowUrl", String.Format("{0}{1}", Configs.ViewReport, wtdwyh));
                //缩略图
                map.Add("reportThumbnailUrl",
                    String.Format("{0}/DataInput/FileService?method=DownloadFile\u0026fileid={1}", Configs.ViewHost,
                        imgId));
                //报告如有异常，给出提示“详细信息见监管平台”
                map.Add("reportStatus", "详细信息见监管平台");
                //返回内容
                ret.data = map;
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 获取人员工程和人员类型(见证人,取样人)
        /// <summary>
        /// 获取人员工程和人员类型(见证人,取样人)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="wtdwyhs"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGcAndRylx(string userName, out bool code, out string msg)
        {
            code = false;
            msg = string.Empty;

            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = string.Format(@"select a.gcbh, a.gcmc, b.rylx, b.rylxmc from i_m_gc a inner join 
                                             (select gcbh, rybh, 'JZY' rylx, '见证员' rylxmc
                                                from i_s_gc_jzry union all
                                              select gcbh, rybh, 'QYY' rylx, '取样员' rylxmc
                                              from i_s_gc_qyry) b on a.gcbh = b.gcbh
                                              inner join i_m_ry c on b.rybh = c.rybh
                                             where a.sfyx = 1
                                               and c.zh = '{0}'", userName);

                ret = CommonDao.GetDataTable(sql);
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return ret;
        }
        #endregion

        #region 非监督工程关联上级监督工程
        /// <summary>
        /// 非监督工程关联上级监督工程
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool ContactJdgc(string gcbh, string ssjcjgbh, string sjgcbh, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string sql = string.Format(@"select top 1 gcbh
                                               from i_m_gc
                                              where sfyx = 1
                                                and ssjcjgbh = '{0}'
                                                and sjgcbh = '{1}'
                                                and gcbh <> '{2}'", ssjcjgbh, sjgcbh, gcbh);

                var gcbhExist = CommonDao.GetSingleData(sql).GetSafeString();

                if (!string.IsNullOrEmpty(gcbhExist))
                {
                    msg = string.Format("监督工程[{0}]已经被[{1}]工程引用过了,不能重复引用", sjgcbh, gcbhExist);
                    return code;
                }

                sql = string.Format("select top 1 zjdjh,gcbh_yc from i_m_gc where gcbh = '{0}'", sjgcbh);

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    msg = string.Format("监督工程[{0}]工程信息不存在");
                    return code;
                }

                var zjdjh = dt[0]["zjdjh"];
                var gcbhYc = dt[0]["gcbh_yc"];

                sql = string.Format("update i_m_gc set sjgcbh = '{0}', zjdjh = '{1}', gcbh_yc = '{2}' where gcbh = '{3}'", sjgcbh, zjdjh, gcbhYc, gcbh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                sql = string.Format("update i_m_jcht set zjdjh = '{0}' where gcbh = '{1}'", zjdjh, gcbh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                sql = string.Format("update m_by set zjdjh = '{0}' where gcbh = '{1}'", zjdjh, gcbh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }



        #endregion

        #region 判断上级监督工程是否被检测机构重复引用
        /// <summary>
        /// 判断上级监督工程是否被检测机构重复引用
        /// </summary>
        /// <param name="sjgcbh"></param>
        /// <param name="qybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsJdgcUsed(string sjgcbh, out string qybh, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            qybh = string.Empty;

            try
            {
                var sql = string.Format("select qybh from i_m_qyzh where yhzh='{0}'", CurrentUser.UserName);
                qybh = CommonDao.GetSingleData(sql).GetSafeString();

                if (string.IsNullOrEmpty(qybh))
                {
                    msg = "企业编号不存在";
                    return code;
                }

                sql = string.Format(@"select top 1 gcbh
                                        from i_m_gc
                                       where sfyx = 1
                                         and ssjcjgbh = '{0}'
                                         and sjgcbh = '{1}'", qybh, sjgcbh);

                var gcbhExist = CommonDao.GetSingleData(sql).GetSafeString();

                if (!string.IsNullOrEmpty(gcbhExist))
                {
                    msg = string.Format("监督工程[{0}]已经被[{1}]工程引用过了,不能重复引用", sjgcbh, gcbhExist);
                    return code;
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return code;
        } 
        #endregion

        #region 更新监督抽查联系单内容
        [Transaction(ReadOnly = false)]
        public bool InsertJG_JDBG_JDCCRWWTJL(VJG_JDBG_JDCCRWWTJL opinion, out string msg)
        {
            bool code = true;
            msg = "";
            HttpRequest Request = HttpContext.Current.Request;
           // if (Request.Files.Count > 0)
            {
                byte[] t_word = Convert.FromBase64String(opinion.word);
                byte[] word = GZipUtil.Decompress(t_word);//new byte[file.ContentLength];
              
                string fileid = Guid.NewGuid().ToString("N");
                string filename = fileid + ".docx";
                string sqlstr = "INSERT INTO [DATAFILE]([FILEID],[FILENAME] ,[FILECONTENT],[FILEEXT],[CJSJ])VALUES(@FILEID,@FILENAME ,@FILECONTENT,@FILEEXT,@CJSJ)";
                IList<IDataParameter> sqlparams = new List<IDataParameter>();
                IDataParameter sqlparam = new SqlParameter("@FILEID", fileid);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@FILENAME", filename);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@FILECONTENT", word);
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@FILEEXT", ".docx");
                sqlparams.Add(sqlparam);
                sqlparam = new SqlParameter("@CJSJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlparams.Add(sqlparam);
                // 保存附件成功，保存联系单记录
                if (CommonDao.ExecCommand(sqlstr, CommandType.Text, sqlparams))
                {
                    sqlparams.Clear();
                    sqlstr = "INSERT INTO [JG_JDBG_JDCCRWWTJL]" +
                                "([GCBH_YC],[ZJDJH],[QYBH],[QYMC],[SYXMBH],[SYXMMC],[JBRZH],[JBRXM],[WTDBH],[NO],[GCMC],[GCDD],[CYRQ],[CYNR],[CJBW],[LXR],[CJNR],[YPGGSL],[WTKS],[BZ],[WORKSERIAL],[FILEID],[LRSJ],[SPRZH],[SPRXM],[SPSJ],[SPZT],[SPYJ],[STATIONID],[KEY])" +
                                "VALUES" +
                                "(@GCBH_YC,@ZJDJH,@QYBH,@QYMC,@SYXMBH,@SYXMMC,@JBRZH,@JBRXM,@WTDBH,@NO,@GCMC,@GCDD,@CYRQ,@CYNR,@CJBW,@LXR,@CJNR,@YPGGSL,@WTKS,@BZ,@WORKSERIAL,@FILEID,@LRSJ,@SPRZH,@SPRXM,@SPSJ,@SPZT,@SPYJ,@STATIONID,@KEY)";
                    sqlparam = new SqlParameter("@GCBH_YC", opinion.gcbh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@ZJDJH", opinion.zjdjh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@QYBH", opinion.qybh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@QYMC", opinion.qymc);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SYXMBH", opinion.syxmbh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SYXMMC", opinion.syxmmc);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@JBRZH", opinion.jbrzh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@JBRXM", opinion.jbrxm);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@WTDBH", opinion.wtdbh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@NO", opinion.no);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@GCMC", opinion.gcmc);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@GCDD", opinion.gcdd);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@CYRQ", opinion.sy_cyrq.ToString("yyyy-MM-dd HH:mm:ss"));
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@CYNR", opinion.cynr);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@CJBW", opinion.cjbw);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@LXR", opinion.lxr);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@CJNR", opinion.cjnr);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@YPGGSL", opinion.ypggsl);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@WTKS", opinion.wtks);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@BZ", opinion.bz);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@WORKSERIAL", opinion.workserial);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@FILEID", fileid);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@LRSJ", opinion.lrsj.ToString("yyyy-MM-dd HH:mm:ss"));
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SPRZH", opinion.sprzh);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SPRXM", opinion.sprxm);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SPSJ", opinion.spsj.ToString("yyyy-MM-dd HH:mm:ss"));
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SPZT", opinion.spzt);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@SPYJ", opinion.spyj);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@STATIONID", opinion.stationid);
                    sqlparams.Add(sqlparam);
                    sqlparam = new SqlParameter("@KEY", opinion.key);
                    sqlparams.Add(sqlparam);
                    if (!CommonDao.ExecCommand(sqlstr, CommandType.Text, sqlparams))
                    {
                        code = false;
                        if (msg == "")
                        {
                            msg = "保存联系单失败！";
                        }
                    }
                }
                else
                {
                    code = false;
                    msg = "保存附件失败！";
                }
            }
            return code;
        }
        public bool InsertJG_JDBG_JDCCRWWTJL_DX(VJG_JDBG_JDCCRWWTJL opinion, out string msg)
        {
            msg = "";
            string qybh = opinion.qybh;
            string sql = string.Format("select * from View_I_M_QY where qybh='{0}'", qybh);
            var dts = CommonDao.GetDataTable(sql);
            foreach (var dt in dts)
            {
                string lxdh =  dt["lxsj"];
                string sql_in = string.Format("INSERT INTO OA_SMS_DXDSFS ([SJHM] ,[CONTENTS],[FSSJ])VALUES ('{0}', '{1}',getdate())", lxdh, "检测监管系统有监督抽查联系单，请及时处理".EncodeBase64());
                CommonDao.ExecSql(sql_in);
            }
            return true;
        }
        #endregion

        #region 判断委托单是否可以保存，校验见证类型和非见证类型的区别
        /// <summary>
        /// 判断委托单是否可以保存，校验见证类型和非见证类型的区别
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="jydbh"></param>
        /// <param name="sdt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsWtdSave(IDbCommand cmd, string jydbh, out List<string> jzRecIds, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            jzRecIds = new List<string>();

            try
            {
                string sql = string.Format("select jclx from m_by where recid = '{0}'", jydbh);
                var mdt = CommonDao.GetDataTableByCmd(cmd, sql);

                if (mdt.Count() == 0)
                {
                    msg = string.Format("委托单唯一号[{0}]不存在", jydbh);
                    return code;
                }

                sql = string.Format("select recid,zh,jzrecid from s_by where byzbrecid = '{0}'", jydbh);
                var sdt = CommonDao.GetDataTableByCmd(cmd, sql);

                if (sdt.Count() == 0)
                {
                    msg = string.Format("委托单唯一号[{0}]从表记录不存在", jydbh);
                    return code;
                }

                var jclx = mdt[0]["jclx"];
                var jzRecId = string.Empty;

                if (jclx == "见证取样")
                {
                    foreach (var dt in sdt)
                    {
                        jzRecId = dt["jzrecid"];

                        if (string.IsNullOrEmpty(jzRecId) || jzRecId == "----")
                        {
                            msg = string.Format("检测类型是见证取样,组号{0}必须有见证取样唯一号", dt["zh"]);
                            return code;
                        }

                        //判断所选见证取样唯一号重复使用
                        var exist = sdt.FirstOrDefault(x => x["jzrecid"] == jzRecId && x["recid"] != dt["recid"]);
                        if (exist != null)
                        {
                            msg = string.Format("检测类型是见证取样,组号{0}的见证取样唯一号已经被组号{1}使用", exist["zh"], dt["zh"]);
                            return code;
                        }

                        jzRecIds.Add(jzRecId);
                    }
                }
                else
                {
                    foreach (var dt in sdt)
                    {
                        jzRecId = dt["jzrecid"];

                        if (!string.IsNullOrEmpty(jzRecId) && jzRecId != "----")
                        {
                            msg = string.Format("检测类型不是见证取样,组号{0}不能有见证取样唯一号", dt["zh"]);
                            return code;
                        }
                    }
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        } 
        #endregion

        #region 同步工程信息给建研
        /// <summary>
        /// 同步工程信息给建研
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SyncGcInfo(string gcbh, out Dictionary<string, string> dict, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            dict = new Dictionary<string, string>();

            try
            {
                string sql = string.Format(@"select gcbh, gcmc, lrrxm from i_m_gc where gcbh = '{0}'", gcbh);

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    msg = "该工程不存在";
                    return code;
                }

                sql = string.Format(@"select distinct b.zh as no, a.ryxm as name
                                        from i_s_gc_qyry a 
                                        inner join i_m_ry b on a.rybh = b.rybh
                                        where gcbh = '{0}'", gcbh);

                var dtQyr = CommonDao.GetDataTable(sql);
                var slpeoplejson = JsonSerializer.Serialize(dtQyr, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    return code;
                }

                sql = string.Format(@"select distinct b.zh as no, a.ryxm as name
                                        from i_s_gc_jzry a 
                                        inner join i_m_ry b on a.rybh = b.rybh
                                        where gcbh = '{0}'", gcbh);

                var dtJzr = CommonDao.GetDataTable(sql);
                var spnpeoplejson = JsonSerializer.Serialize(dtJzr, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    return code;
                }

                dict.Add("projectnum", dt[0]["gcbh"]);
                dict.Add("projectname", dt[0]["gcmc"]);
                dict.Add("constractionunit", "");
                dict.Add("inspectunit", "");
                dict.Add("slpeoplejson", slpeoplejson);
                dict.Add("spnpeoplejson", spnpeoplejson);
                dict.Add("createunit", dt[0]["lrrxm"]);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion

        #region 同步合同 同步委托单

        [Transaction(ReadOnly = false)]
        public ResultParam SyncJcjgHt(string htJson, string userCode, string userName)
        {
            ResultParam ret = new ResultParam();
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(htJson))
                {
                    ret.msg = "传入的参数不能为空";
                    return ret;
                }

                var htDict = JsonSerializer.Deserialize<Dictionary<string, string>>(htJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                string sql = string.Format(@"select sjbmc, zdmc, sy, sfbhzd, bhms, defaval, mustin
                                               from zdzd_jc
                                              where sjbmc = 'i_m_jcht'
                                                and ',' + lx + ',' like '%,N,%'
                                              order by xssx asc");

                var zdzds = CommonDao.GetDataTable(sql);
                ret = GetSyncInsertDict(zdzds, htDict, null);

                if (!ret.success)
                    return ret;

                var dict = ret.data as Dictionary<string, string>;
                
                //设置默认值
                DictionaryHelper.SetValue(dict, "LRRZH", userCode);
                DictionaryHelper.SetValue(dict, "LRRXM", userName);
                DictionaryHelper.SetValue(dict, "LRSJ", DateTime.Now.ToString());
                DictionaryHelper.SetValue(dict, "SFYX", "1");
                DictionaryHelper.SetValue(dict, "HTLX", "QYHT");

                sql = MakeSqlHelper.InsertSql("I_M_JCHT", dict);
                CommonDao.ExecCommand(sql);

                ret.success = true;
                ret.data = DictionaryHelper.GetValue(dict, "RECID");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }

            return ret;
        } 

        [Transaction(ReadOnly = false)]
        public ResultParam SyncJcjgWtd(string mJson, string sJson, string userCode, string userName)
        {
            ResultParam ret = new ResultParam();
            string msg = string.Empty;

            try
            {
                var mDataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(mJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var sDataList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(sJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                if (sDataList.Count() == 0)
                {
                    ret.msg = "从表最少有一条记录";
                    return ret;
                }

                var syxmbh = DictionaryHelper.GetValue(mDataDict, "SYXMBH");

                if (string.IsNullOrEmpty(syxmbh))
                {
                    ret.msg = "[SYXMBH]字段对应数据不能为空";
                    return ret;
                }

                string tableSql = @"select sjbmc, zdmc, sy, sfbhzd, bhms, defaval, mustin 
                                      from {0}
                                     where sjbmc = '{1}'
                                       and ',' + lx + ',' like '%,W,%'
                                  order by xssx asc";

                string sql = string.Format(tableSql, "xtzd_by", "m_by");
                var mZdzds = CommonDao.GetDataTable(sql);

                sql = string.Format(tableSql, "xtzd_by", "s_by");
                var sZdzds = CommonDao.GetDataTable(sql);

                sql = string.Format(tableSql, "zdzd_" + syxmbh, "s_" + syxmbh);
                var seZdzds = CommonDao.GetDataTable(sql);

                DataTable dataTable = new DataTable("table");
                dataTable.Columns.Add(new DataColumn() { ColumnName = "SYXMBH" });
                dataTable.Columns.Add(new DataColumn() { ColumnName = "BYZBRECID" });

                DataRow dr = dataTable.NewRow();
                dr["SYXMBH"] = syxmbh;
                ret = GetSyncInsertDict(mZdzds, mDataDict, dr);

                if (!ret.success)
                    return ret;

                var dict = ret.data as Dictionary<string, string>;

                //设置主表默认值
                DictionaryHelper.SetValue(dict, "ZT", "W000200000");
                DictionaryHelper.SetValue(dict, "JCJG", "0");
                DictionaryHelper.SetValue(dict, "WTSLRRZH", userCode);
                DictionaryHelper.SetValue(dict, "WTSLRRXM", userName);
                DictionaryHelper.SetValue(dict, "WTSLRSJ", DateTime.Now.ToString());

                List<string> sqls = new List<string>();
                var byzbrecId = dict["RECID"];
                var meDict = new Dictionary<string, string>();
                meDict.Add("RECID", byzbrecId);

                sqls.Add(MakeSqlHelper.InsertSql("m_by", dict));
                sqls.Add(MakeSqlHelper.InsertSql("m_" + syxmbh, meDict));
                sqls.Add(MakeSqlHelper.InsertSql("m_d_" + syxmbh, meDict));

                DataRow sdr = dataTable.NewRow();
                sdr["BYZBRECID"] = byzbrecId;

                foreach (var sDataDict in sDataList)
                {
                    ret = GetSyncInsertDict(sZdzds, sDataDict, sdr);

                    if (!ret.success)
                        return ret;

                    var sDict = ret.data as Dictionary<string, string>;

                    //设置从表默认值
                    DictionaryHelper.SetValue(sDict, "JM", "W000000000");

                    sDict["BYZBRECID"] = byzbrecId;
                    var sRecId = sDict["RECID"];

                    ret = GetSyncInsertDict(seZdzds, sDataDict, sdr);

                    if (!ret.success)
                        return ret;

                    var seDict = ret.data as Dictionary<string, string>;
                    DictionaryHelper.SetValue(seDict, "RECID", sRecId);
                    DictionaryHelper.SetValue(seDict, "BYZBRECID", byzbrecId);

                    var sdeDict = new Dictionary<string, string>();
                    sdeDict.Add("RECID", sRecId);
                    sdeDict.Add("BYZBRECID", byzbrecId);

                    sqls.Add(MakeSqlHelper.InsertSql("s_by", sDict));
                    sqls.Add(MakeSqlHelper.InsertSql("s_" + syxmbh, seDict));
                    sqls.Add(MakeSqlHelper.InsertSql("s_d_" + syxmbh, sdeDict));
                }

                foreach (var sqlItem in sqls)
                    CommonDao.ExecCommand(sqlItem);

                ret.success = true;
                ret.data = byzbrecId;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        public ResultParam GetSyncInsertDict(IList<IDictionary<string, string>> zdzds, Dictionary<string, string> dataDict, DataRow dr)
        {
            ResultParam ret = new ResultParam();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string msg = string.Empty;
            string value = string.Empty;

            foreach (var zdzd in zdzds)
            {
                var code = GetZdzdDict(dataDict, zdzd, dr, out value, out msg);

                if (!code)
                {
                    ret.msg = msg;
                    return ret;
                }

                dict.Add(zdzd["zdmc"].ToUpper(), value);
            }

            ret.success = true;
            ret.data = dict;
            return ret;
        }

        private bool GetZdzdDict(Dictionary<string, string> dataDict, IDictionary<string, string> zdzd, DataRow dr, out string value, out string msg)
        {
            var zdmc = zdzd["zdmc"];
            value = string.Empty;
            msg = string.Empty;

            //编号模式字段去获取值
            if (zdzd["sfbhzd"].GetSafeBool())
            {
                var firstOpt = false;
                value = WebDataInputDao.GetBH(zdzd["bhms"], zdzd["sjbmc"], zdzd["zdmc"], dr, null, ref firstOpt);

                if (string.IsNullOrEmpty(value))
                {
                    msg = string.Format("获取[{0}]编号模式出错", zdmc);
                    return false;
                }
            }
            else
            {
                value = DictionaryHelper.GetValue(dataDict, zdmc.ToUpper());
            }

            return true;
        }

        #endregion

        #region 获取需要同步图片的见证记录(建研)
        /// <summary>
        /// 获取需要同步图片的见证记录
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJyJzPic()
        {
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();

            try
            {
                result = CommonDao.GetDataTable("select top 10 * from Up_JySyncInfo where PicSync = 0 order by RecId desc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return result;
        }
        #endregion

        #region 插入见证图片信息(建研)
        [Transaction(ReadOnly = false)]
        public bool InsertWtdJzPic(IDictionary<string, string> jyjzPic, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string sql = string.Empty;
                string scr = string.Empty;
                string scrxm = string.Empty;

                //判断s_by是否已经有二维码更新标志
                sql = string.Format("select IsNull(ypysc,0) ypysc from s_by where byzbrecid = '{0}' and jzrecid = '{1}'", jyjzPic["wtdwyh"], jyjzPic["jzrecid"]);

                var ypysc = CommonDao.GetSingleData(sql).GetSafeInt();

                if (ypysc == 1)
                {
                    sql = string.Format("update Up_JySyncInfo set PicSync = 1 where recid = '{0}'", jyjzPic["recid"]);
                    CommonDao.ExecCommand(sql);
                    code = true;
                    return code;
                }

                //获取图片
                var result = JyJzqyService.GetDataReocrdPhotoBase64(jyjzPic["jzrecid"]);

                if (result.success)
                {
                    var data = result.data as Dictionary<string, object>;

                    jyjzPic.Add("qrcode", data["qrinfo"].GetSafeString());
                    jyjzPic.Add("gcbh", data["projectnum"].GetSafeString());
                    jyjzPic.Add("qylongitude", data["sllong"].GetSafeString());
                    jyjzPic.Add("qylatitude", data["sllat"].GetSafeString());
                    jyjzPic.Add("jzlongitude", data["spnlong"].GetSafeString());
                    jyjzPic.Add("jzlatitude", data["spnlat"].GetSafeString());
                    jyjzPic.Add("qyrxm", data["slname"].GetSafeString());
                    jyjzPic.Add("qysj", data["sldate"].GetSafeString());
                    jyjzPic.Add("jzrxm", data["spnname"].GetSafeString());
                    jyjzPic.Add("jzsj", data["spndate"].GetSafeString());

                    List<Dictionary<string, string>> tps = new List<Dictionary<string, string>>();

                    //见证人图片
                    string jzrtp = data["jzrtp"].GetSafeString();

                    if (!string.IsNullOrEmpty(jzrtp))
                    {
                        Dictionary<string, string> tp = new Dictionary<string, string>();
                        tp.Add("tplx", "JZ01");
                        tp.Add("ewm", "");
                        tp.Add("img", jzrtp);
                        tps.Add(tp);
                    }

                    //样品图片
                    string yptp = data["yptp"].GetSafeString();

                    if (!string.IsNullOrEmpty(yptp))
                    {
                        Dictionary<string, string> tp = new Dictionary<string, string>();
                        tp.Add("tplx", "JZ04");
                        tp.Add("ewm", "");
                        tp.Add("img", yptp);
                        tps.Add(tp);
                    }

                    //二维码图片
                    string ewmtp = data["ewmtp"].GetSafeString();

                    if (!string.IsNullOrEmpty(ewmtp))
                    {
                        var ewmtps = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(ewmtp);

                        foreach (var ewmtpTemp in ewmtps)
                        {
                            if (!string.IsNullOrEmpty(ewmtpTemp["image"]))
                            {
                                Dictionary<string, string> tp = new Dictionary<string, string>();
                                tp.Add("tplx", "JZ03");
                                tp.Add("ewm", ewmtpTemp["ewm"]);
                                tp.Add("img", ewmtpTemp["image"]);
                                tps.Add(tp);
                            }
                        }
                    }

                    //获取上传人姓名
                    sql = string.Format(@"select b.YHZH scr, a.RYXM scrxm
                                        from i_m_ry a inner join i_m_qyzh b on a.RYBH = b.QYBH
                                        where rybh = '{0}'", jyjzPic["jzrbh"]);

                    var rydt = CommonDao.GetDataTable(sql);

                    if (rydt.Count() > 0)
                    {
                        scr = rydt[0]["scr"];
                        scrxm = rydt[0]["scrxm"];
                    }

                    //先上传到Oss, 防止表阻塞
                    List<Dictionary<string, string>> jzPics = new List<Dictionary<string, string>>();

                    foreach (var tp in tps)
                    {
                        if (string.IsNullOrEmpty(tp["img"]))
                            continue;

                        string tpwyh = Guid.NewGuid().ToString();

                        Dictionary<string, string> jzPic = new Dictionary<string, string>();
                        var uploadResult = UploadJzPicOss(Convert.FromBase64String(tp["img"]), tp["tplx"], out jzPic);

                        if (!uploadResult)
                        {
                            msg = "上传图片到Oss服务器出错";
                            return code;
                        }

                        jzPic.Add("ewm", tp["ewm"]);
                        jzPics.Add(jzPic);
                    }

                    foreach (var jzPic in jzPics)
                    {
                        string tpwyh = Guid.NewGuid().ToString();

                        sql = string.Format(@"insert into UP_WTDTP (tpwyh,wtdwyh,scsj,scr,scrxm,sfyx,tplx,zh,ewm,longitude,latitude,qyrxm,qysj,jzrxm,jzsj,qylongitude,qylatitude)
                                             values ('{0}', '{1}', getdate(), '{2}', '{3}', 1, '{4}', '{5}', '{6}', {7}, {8}, '{9}', '{10}', '{11}', '{12}', {13}, {14})",
                                                     tpwyh, jyjzPic["wtdwyh"], scr, scrxm, jzPic["tplx"], jyjzPic["zh"], jzPic["ewm"], jyjzPic["jzlongitude"].GetSafeDecimal(), jyjzPic["jzlatitude"].GetSafeDecimal(),
                                                     jyjzPic["qyrxm"], jyjzPic["qysj"].GetSafeDate(), jyjzPic["jzrxm"], jyjzPic["jzsj"].GetSafeDate(), jyjzPic["qylongitude"].GetSafeDecimal(), jyjzPic["qylatitude"].GetSafeDecimal());

                        CommonDao.ExecCommand(sql);

                        InsertJzPic(jzPic, tpwyh, scr, scrxm, jzPic["tplx"]);
                    }

                    if (tps.Count() > 0)
                    {
                        sql = string.Format("update s_by set ypewm = '{0}', ypysc = 1 where byzbrecid = '{1}' and jzrecid = '{2}'", jyjzPic["qrcode"], jyjzPic["wtdwyh"], jyjzPic["jzrecid"]);
                        CommonDao.ExecCommand(sql);

                        sql = string.Format(@"insert into UP_GcPos (gcbh, qylongitude, qylatitude, jzlongitude, jzlatitude, wtdwyh, jzrecid, createtime)
                                      values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', getdate())", jyjzPic["gcbh"], jyjzPic["qylongitude"].GetSafeDecimal(), jyjzPic["qylatitude"].GetSafeDecimal(),
                          jyjzPic["jzlongitude"].GetSafeDecimal(), jyjzPic["jzlatitude"].GetSafeDecimal(), jyjzPic["wtdwyh"], jyjzPic["jzrecid"]);
                        CommonDao.ExecCommand(sql);
                    }

                    //所有样品记录完成，更新委托单状态
                    sql = string.Format(@"select count(1) as countSum, sum(case when ypysc = 1 Then 1 else 0 end) as ypyscSum
                                        from s_by where byzbrecid = '{0}'", jyjzPic["wtdwyh"]);

                    var dt = CommonDao.GetDataTableTran(sql);

                    if (dt.Count() == 1)
                    {
                        sql = string.Format("select zt from m_by where recid = '{0}'", jyjzPic["wtdwyh"]);

                        var zt = CommonDao.GetSingleData(sql).GetSafeString();
                        WtsStatus status = new WtsStatus(zt);
                        var countSum = dt[0]["countSum"].GetSafeInt();
                        var ypyscSum = dt[0]["ypyscSum"].GetSafeInt();

                        if (countSum == ypyscSum)
                        {
                            status.SetWtdJzqyzt(WtsStatus.JzStateTp1, out msg);
                            sql = string.Format("update m_by set zt='{0}' where recid='{1}'", status.GetStatus(), jyjzPic["wtdwyh"]);
                            CommonDao.ExecCommand(sql);
                        }
                    }

                    //标记同步状态
                    sql = string.Format("update Up_JySyncInfo set PicSync = 1 where recid = '{0}'", jyjzPic["recid"]);
                    CommonDao.ExecCommand(sql);

                    code = true;
                }
                else
                {
                    msg = result.msg;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion 

        #region 获取需要同步委托状态的见证记录(建研)
        /// <summary>
        /// 获取需要同步委托状态的见证记录
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJyOrderStatus()
        {
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();

            try
            {
                result = CommonDao.GetDataTable("select top 100 * from UP_JyOrderStatus where isUpdate = 0 order by RecId desc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return result;
        }
        #endregion

        #region 更新委托状态同步标记(建研)
        [Transaction(ReadOnly = false)]
        public bool UpdateJyOrderStatus(int recId, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var sql = string.Format("update UP_JyOrderStatus set IsUpdate = 1 where recid = {0}", recId);

                CommonDao.ExecCommand(sql, CommandType.Text);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion 

        #region 获取需要同步的见证取样修改记录(建研)
        /// <summary>
        /// 获取需要同步的见证取样修改记录(建研)
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJyItemUpdate()
        {
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();

            try
            {
                result = CommonDao.GetDataTable("select top 100 * from Up_JySyncInfo where ItemUpdateSync = 0 order by RecId desc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return result;
        }
        #endregion

        #region 获取需要同步的取样见证记录(建研)
        /// <summary>
        /// 获取需要同步的取样见证记录(建研)
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetJyQyData()
        {
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();

            try
            {
                result = CommonDao.GetDataTable("select top 100 * from Up_JySyncInfo where QyDataSync = 0 order by RecId desc");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return result;
        }
        #endregion

        #region 插入见证取样修改记录
        [Transaction(ReadOnly = false)]
        public bool InsertJyItemUpdate(string recid, string wtdwyh, string sRecId, List<Dictionary<string, string>> datas, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string sql = string.Empty;

                sql = string.Format("select count(1) num from I_S_JyWtdModifyLog where WtdWyh = '{0}' and SRecId = '{1}'", wtdwyh, sRecId);
                var num = CommonDao.GetSingleData(sql).GetSafeInt();

                if (num > 0)
                {
                    sql = string.Format("update Up_JySyncInfo set ItemUpdateSync = 1 where recid = '{0}'", recid);
                    CommonDao.ExecCommand(sql);
                    code = true;
                    return code;
                }

                foreach (var data in datas)
                {
                    sql = string.Format(@"Insert Into I_S_JyWtdModifyLog (WtdWyh, SRecId, JzRecId, Field, FieldName, OldValue, NewValue, ModifyUser, ModifyUserName, ModifyTime)
                                 Values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                                 wtdwyh, sRecId, data["GUID"], data["COLUMNEG"], data["COLUMNCH"], data["OLDVALUE"], data["NEWVALUE"], data["USERCODE"], data["USERNAME"], data["DATE"]);

                    CommonDao.ExecCommand(sql);
                }

                sql = string.Format("update Up_JySyncInfo set ItemUpdateSync = 1 where recid = '{0}'", recid);
                CommonDao.ExecCommand(sql);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion 

        #region 插入见证取样记录
        [Transaction(ReadOnly = false)]
        public bool InsertJyQyData(string recid, string wtdwyh, string jzRecId, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                string sql = string.Empty;

                sql = string.Format("select count(1) num from UP_JyQyData where Guid = '{0}'", jzRecId);
                var num = CommonDao.GetSingleData(sql).GetSafeInt();

                if (num > 0)
                {
                    sql = string.Format("update Up_JySyncInfo set QyDataSync = 1 where recid = '{0}'", recid);
                    CommonDao.ExecCommand(sql);
                    code = true;
                    return code;
                }

                var result = JyJzqyService.GetDataReocrdPhotoUrl(jzRecId);

                if (result.success)
                {
                    var data = result.data as Dictionary<string, object>;

                    sql = string.Format(@"Insert Into UP_JyQyData (Guid, QRInfo, ProjectNum, ProjectName, ProjLong, ProjLat, ItemCode, SlId, SlName, SlLong, SlLat, SlDate, SpnId, SpnName, SpnLong, SpnLat, SpnDate, IsOrder, OrderNum, OrderDate, ItemJson, JzrTp, YpTp, EwmTp)
                                Values ('{0}', '{1}', '{2}', '{3}', {4}, {5}, '{6}', '{7}', '{8}', {9}, {10}, '{11}', '{12}', '{13}', {14}, {15}, '{16}', {17}, '{18}', '{19}', '{20}', '{21}', '{22}', '{23}')",
                                data["guid"].GetSafeString(), data["qrinfo"].GetSafeString(), data["projectnum"].GetSafeString(), data["projectname"].GetSafeString(), data["projlong"].GetSafeDecimal(),
                                data["projlat"].GetSafeDecimal(), data["itemcode"].GetSafeString(), data["slid"].GetSafeString(), data["slname"].GetSafeString(), data["sllong"].GetSafeDecimal(),
                                data["sllat"].GetSafeDecimal(), data["sldate"].GetSafeDate(), data["spnid"].GetSafeString(), data["spnname"].GetSafeString(), data["spnlong"].GetSafeDecimal(),
                                data["spnlat"].GetSafeDecimal(), data["spndate"].GetSafeDate(), data["isorder"].GetSafeInt(), data["ordernum"].GetSafeString(), data["orderdate"].GetSafeDate(),
                                data["itemjson"].GetSafeString(), data["jzrtp"].GetSafeString(), data["yptp"].GetSafeString(), JsonSerializer.Serialize(data["ewmtp"]));
                    CommonDao.ExecCommand(sql);

                    sql = string.Format("update s_by set ypewm = '{0}' where byzbrecid = '{1}' and jzrecid = '{2}' and (ypewm is null or ypewm = '')", data["qrinfo"].GetSafeString(), wtdwyh, data["guid"].GetSafeString());
                    CommonDao.ExecCommand(sql);

                    sql = string.Format("update Up_JySyncInfo set QyDataSync = 1 where recid = '{0}'", recid);
                    CommonDao.ExecCommand(sql);

                    code = true;
                }
                else
                {
                    msg = result.msg;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion 

        #region 根据账号获取人员编号
        public bool GetRybhByZh(string zhs, out IList<IDictionary<string, string>> rys, out string msg)
        {
            bool code = false;
            rys = new List<IDictionary<string, string>>();
            msg = string.Empty;

            try
            {
                if(string.IsNullOrEmpty(zhs))
                {
                    code = true;
                    return code;
                }

                var sql = string.Format("select rybh,zh,ryxm from i_m_ry where zh in ({0})", zhs.FormatSQLInStr());

                rys = CommonDao.GetDataTable(sql);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion 

        #region 根据工程编号获取试验项目
        /// <summary>
        /// 根据检测机构获取试验项目
        /// </summary>
        /// <param name="ryzh"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetSyxmsByGcbh(string gcbh, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = false;
            msg = "";
            records = new List<IDictionary<string, object>>();
            try
            {
                string sql = string.Format("select top 1 ssjcjgbh from i_m_gc where gcbh = '{0}'", gcbh);
                string qybh = CommonDao.GetSingleData(sql).GetSafeString();

                if (string.IsNullOrEmpty(qybh))
                {
                    msg = "该工程所属的检测机构不存在";
                    return ret;
                }

                string xsflSql = string.Format(@"select sjxsflbh,xsflbh,xsflmc from pr_m_syxmxsfl where ssdwbh= '{0}' order by xssx", qybh);

                var xsfls = CommonDao.GetDataTable(xsflSql);

                string syxmSql = string.Format(@"select a.xsflbh,a.syxmbh,a.syxmmc
                                                   from pr_m_syxm a
                                                  where a.ssdwbh= '{0}'
                                                    and a.xmlx<>'3' 
                                                    and a.sfyx=1
                                                    and exists(select * from pr_m_qyzb b inner join pr_s_cp_zb c on b.zbbh=c.zbbh where b.qybh=a.ssdwbh and c.syxmbh=a.syxmbh ) 
                                                    order by a.xsflbh,a.xssx", qybh);

                var syxms = CommonDao.GetDataTable(syxmSql);

                var firstXsfls = xsfls.Where(x => x["sjxsflbh"] == "").ToList();

                foreach (var firstXsfl in firstXsfls)
                {
                    IDictionary<string, object> dict = new Dictionary<string, object>();

                    var secondXsfls = xsfls.Where(x => x["sjxsflbh"] == firstXsfl["xsflbh"]).ToList();
                    IList<IDictionary<string, object>> secondList = new List<IDictionary<string, object>>();

                    foreach (var secondXsfl in secondXsfls)
                    {
                        var subSyxms = syxms.Where(x => x["xsflbh"] == secondXsfl["xsflbh"]).ToList();

                        if (subSyxms.Count() > 0)
                        {
                            IDictionary<string, object> secondDict = new Dictionary<string, object>();
                            secondDict.Add("sjxsflbh", secondXsfl["sjxsflbh"]);
                            secondDict.Add("xsflbh", secondXsfl["xsflbh"]);
                            secondDict.Add("xsflmc", secondXsfl["xsflmc"]);
                            secondDict.Add("syxms", subSyxms);

                            secondList.Add(secondDict);
                        }
                    }

                    if (secondList.Count() > 0)
                    {
                        dict.Add("sjxsflbh", firstXsfl["sjxsflbh"]);
                        dict.Add("xsflbh", firstXsfl["xsflbh"]);
                        dict.Add("xsflmc", firstXsfl["xsflmc"]);
                        dict.Add("subs", secondList);
                        records.Add(dict);
                    }
                }

                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }
        #endregion

        #region 获取工程坐标
        /// <summary>
        /// 获取工程坐标
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetGczb(string gcbh, out List<Dictionary<string,string>> result, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            result = new List<Dictionary<string, string>>();

            try
            {
                string sql = string.Format("select gcmc,gczb,jdzb,jzzb from i_m_gc where gcbh = '{0}'", gcbh);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    msg = "该工程信息不存在";
                    return code;
                }

                var dict = GetGczbDict(dt[0]["gcmc"], dt[0]["gczb"], "工程坐标");

                if(dict != null)
                    result.Add(dict);

                dict = GetGczbDict(dt[0]["gcmc"], dt[0]["jdzb"], "监督坐标");

                if (dict != null)
                    result.Add(dict);

                dict = GetGczbDict(dt[0]["gcmc"], dt[0]["jzzb"], "见证坐标");

                if (dict != null)
                    result.Add(dict);

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }

        private Dictionary<string, string> GetGczbDict(string gcmc, string zb, string zblx)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("gcmc", gcmc);
            dict.Add("zblx", zblx);

            if (!string.IsNullOrEmpty(zb))
            {
                var arr = zb.Split(',');

                if (arr.Length == 2)
                {
                    dict.Add("longitude", arr[0]);
                    dict.Add("latitude", arr[1]);
                    return dict;
                }
            }

            return null;
        }
        #endregion

        #region 更新i_m_gc表的GCMCNEW字段
        public bool UpdateGCMCNEW()
        {
            string sql = "select gcbh, gcmc from i_m_gc";
            IList<IDictionary<string, string>> dt1 = CommonDao.GetDataTable(sql);
            List<string> sqls = new List<string>();
            for (int i = 0; i < dt1.Count; i++)
            {
                string gcbh = dt1[i]["gcbh"];
                string gcmc = dt1[i]["gcmc"];
                string gcmcnew = gcmc.GetFormateString();
                string sql_new = string.Format("update i_m_gc set gcmcnew ='{0}' where gcbh='{1}'", gcmcnew, gcbh);
                CommonDao.ExecSql(sql_new);
            }
            return true;
        } 
        #endregion

        #region 获取单个工程的坐标
        public bool GetGcPos(string gcbh, out object result, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            result = null;

            try
            {
                string sql = string.Format("select gcbh,gcmc,jdzb,jzzb from i_m_gc where gcbh = '{0}'", gcbh);
                var gcList = CommonDao.GetDataTable(sql);

                if (gcList.Count() == 0)
                {
                    msg = "该工程信息不存在";
                    return code;
                }

                var jdPos = GetGcPosDict(gcList[0]["jdzb"]);
                var jhPos = GetGcPosDict(gcList[0]["jzzb"]);

                sql = string.Format(@"select a.recid, a.syxmbh, a.syxmmc, b.qylongitude, b.qylatitude, b.jzlongitude, b.jzlatitude
                                        from m_by a inner join up_gcpos b on a.recid = b.wtdwyh
                                        where b.gcbh = '{0}'", gcbh);
                var posList = CommonDao.GetDataTable(sql);

                var qyPos = new List<Dictionary<string, string>>();
                var jzPos = new List<Dictionary<string, string>>();

                foreach (var pos in posList)
                {
                    var recid = pos["recid"];
                    var syxmmc = pos["syxmmc"];
                    var qylongitude = pos["qylongitude"];
                    var qylatitude = pos["qylatitude"];
                    var jzlongitude = pos["jzlongitude"];
                    var jzlatitude = pos["jzlatitude"];

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(qylongitude)
                     && !string.IsNullOrEmpty(qylatitude))
                    {
                        if (qyPos.FindIndex(x => x["recid"] == recid && x["longitude"] == qylongitude && x["latitude"] == qylatitude) == -1)
                        {
                            dict.Add("recid", recid);
                            dict.Add("syxmmc", syxmmc);
                            dict.Add("longitude", qylongitude);
                            dict.Add("latitude", qylatitude);
                            qyPos.Add(dict);
                        }
                    }

                    if (!string.IsNullOrEmpty(jzlongitude)
                     && !string.IsNullOrEmpty(jzlatitude))
                    {
                        if (jzPos.FindIndex(x => x["recid"] == recid && x["longitude"] == jzlongitude && x["latitude"] == jzlatitude) == -1)
                        {
                            dict = new Dictionary<string, string>();
                            dict.Add("recid", recid);
                            dict.Add("syxmmc", syxmmc);
                            dict.Add("longitude", jzlongitude);
                            dict.Add("latitude", jzlatitude);
                            jzPos.Add(dict);
                        }
                    }
                }

                result = new
                {
                    gcbh = gcList[0]["gcbh"],
                    gcmc = gcList[0]["gcmc"],
                    jdPos,
                    jhPos,
                    qyPos,
                    jzPos
                };

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }

        private Dictionary<string, string> GetGcPosDict(string pos)
        {
            var dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(pos))
            {
                var arr = pos.Split(',');

                if (arr.Length == 2)
                {
                    dict.Add("longitude", arr[0]);
                    dict.Add("latitude", arr[1]);
                }
            }

            return dict;
        }
        #endregion

        #region 获取所有工程的坐标
        public bool GetAllGcPos(string qybh, out List<Dictionary<string, object>> result, out string msg)
        {
            bool code = false;
            msg = string.Empty;
            result = new List<Dictionary<string, object>>();

            try
            {
                string sql = string.Format("select gcbh,gcmc,jdzb,jzzb from i_m_gc where jzzb > '' and ssjcjgbh='{0}'", qybh);
                var gcList = CommonDao.GetDataTable(sql);

                foreach (var gc in gcList)
                {
                    var dict = new Dictionary<string, object>();
                    dict.Add("gcbh", gc["gcbh"]);
                    dict.Add("gcmc", gc["gcmc"]);
                    dict.Add("jdPos", GetGcPosDict(gc["jdzb"]));
                    dict.Add("jhPos", GetGcPosDict(gc["jzzb"]));
                    result.Add(dict);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion

        #region 无二维码申请审核
        [Transaction(ReadOnly = false)]
        public bool NoQrCodeAudit(string data, string userCode, string userName, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var dicts = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    return code;
                }

                foreach (var dict in dicts)
                {
                    var sql = string.Format(@"update I_S_JZSQ set AuditUser = '{0}', AuditUserName ='{1}', AuditTime = getDate(),
                    isAudit = 1 where recid = '{2}' and wtdwyh = '{3}' and isAudit = 0", userCode, userName, dict["recid"], dict["wtdwyh"]);

                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion

        #region 设置非监督工程检测审核项目
        [Transaction(ReadOnly = false)]
        public bool SetFjdGcAuditXm(string gcbh, string zjzbh, Dictionary<string, int> dict, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var sql = string.Format("delete from I_S_FJDGC_Audit where gcbh = '{0}' and zjzbh='{1}'", gcbh, zjzbh);
                CommonDao.ExecCommand(sql, CommandType.Text);

                foreach (var value in dict)
                {
                    sql = string.Format("insert into I_S_FJDGC_Audit (gcbh,zjzbh,syxmbh,isAudit) values ('{0}', '{1}', '{2}', {3})",
                        gcbh, zjzbh, value.Key, value.Value);

                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion 

        #region 判断非监督工程检测项目是否可以填单
        public bool JudgeFjdGcAuditXm(string dwbh, string gcbh, string zjzbh, string syxmbh, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var sql = string.Format("select fjdgcaudit from h_zjz where zjzbh = '{0}'", zjzbh);
                var fjdgcAudit = CommonDao.GetSingleData(sql).GetSafeString();

                //质监站没有配置, 不需要判断
                if (string.IsNullOrEmpty(fjdgcAudit))
                {
                    code = true;
                    return code;
                }

                sql = string.Format(@"select count(1) gcnum
                                            from i_m_gc
                                           where ssjcjgbh > '' and (sjgcbh = '' OR sjgcbh IS NULL) 
                                             and gcbh = '{0}'", gcbh);
                var gcNum = CommonDao.GetSingleData(sql).GetSafeInt();

                //监督工程，不需要判断
                if (gcNum == 0)
                {
                    code = true;
                    return code;
                }

                sql = string.Format(@"select count(1) xmnum 
                                        from pr_m_syxm 
                                       where syxmbh='{0}' and ssdwbh='{1}' and '{2}' like '%' + xsflbh1 + '%'", syxmbh, dwbh, fjdgcAudit);

                var xmNum = CommonDao.GetSingleData(sql).GetSafeInt();

                //项目不存在设置审核的大类中，不需要判断
                if (xmNum == 0)
                {
                    code = true;
                    return code;
                }

                sql = string.Format(@"select isaudit
                                        from i_s_fjdgc_audit
                                       where gcbh = '{0}'
                                         and zjzbh = '{1}'
                                         and syxmbh = '{2}'", gcbh, zjzbh, syxmbh);

                var isAudit = CommonDao.GetSingleData(sql).GetSafeInt();
                if (isAudit == 0)
                {
                    code = false;
                    msg = "非监督工程检测需要质监站审核通过才可以填单,请联系检测机构向相应质监站申请";
                    return code;
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion 

        #region 新增人员变更记录
        public bool InsertRyBghj(string zh, string czr, string czrxm, int czlx, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var sql = string.Format("select rybh, ryxm from i_m_ry where zh = '{0}'", zh);

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    msg = "该人员不存在";
                    return code;
                }

                sql = string.Format(@"insert into I_S_RY_BGHJ (rybh, ryxm, czr, czrxm, czsj, czlx) 
                      values ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}')", dt[0]["rybh"], dt[0]["ryxm"], czr, czrxm, czlx);

                code = CommonDao.ExecSql(sql);

                if (code == false)
                {
                    msg = "插入变更记录出错";
                    return code;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        } 
        #endregion

        #region 设置人员单位
        [Transaction(ReadOnly = false)]
        public bool SetRydw(string rybhs, string czr, string czrxm, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var rybhArr = rybhs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int sysRylysq = GetSysRylysq();
                foreach (var rybh in rybhArr)
                {
                    string sql = string.Empty;

                    sql = string.Format("select ryxm from i_m_ry where rybh = '{0}'", rybh);
                    var ryxm = CommonDao.GetSingleData(sql).GetSafeString();

                    if (sysRylysq == (int)SysRylysqEnum.Enabled)
                    {
                        //是否存在离职记录
                        sql = string.Format("select count(1) num from i_s_ry_bghj where rybh = '{0}' and czlx = 2", rybh);
                        var num = CommonDao.GetSingleData(sql).GetSafeInt();

                        if (num > 0)
                        {
                            //插入监管方审批记录
                            sql = string.Format(@"select count(1) num from I_S_RY_LYSQ where rybh = '{0}' and czr = '{1}' and sfsh = 0", rybh, czr);
                            num = CommonDao.GetSingleData(sql).GetSafeInt();

                            if (num == 0)
                            {
                                sql = string.Format(@"insert into I_S_RY_LYSQ (rybh, ryxm, czr, czrxm, czsj, sfsh)
                                                values ('{0}', '{1}', '{2}', '{3}', getdate(), 0)", rybh, ryxm, czr, czrxm);
                                CommonDao.ExecCommand(sql, CommandType.Text);
                            }

                            continue;
                        }
                    }

                    sql = string.Format(@"insert into I_S_RY_BGHJ (rybh, ryxm, czr, czrxm, czsj, czlx) 
                      values ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}')", rybh, ryxm, czr, czrxm, 1);
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    sql = string.Format(@"update i_m_ry set qybh= (select qybh from i_m_qyzh where yhzh= '{0}') 
                    where rybh = '{1}' and (qybh is null or qybh = '')", czr, rybh);
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion

        #region 清除人员单位
        [Transaction(ReadOnly = false)]
        public bool ClearRydw(string rybhs, string czr, string czrxm, out string msg)
        {
            bool code = false;
            msg = string.Empty;

            try
            {
                var rybhArr = rybhs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var rybh in rybhArr)
                {
                    var sql = string.Format("select ryxm from i_m_ry where rybh = '{0}'", rybh);
                    var ryxm = CommonDao.GetSingleData(sql).GetSafeString();

                    sql = string.Format(@"insert into I_S_RY_BGHJ (rybh, ryxm, czr, czrxm, czsj, czlx) 
                      values ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}')", rybh, ryxm, czr, czrxm, 2);
                    CommonDao.ExecCommand(sql, CommandType.Text);

                    sql = string.Format(@"update i_m_ry set qybh='' where rybh = '{0}'", rybh);
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
                throw ex;
            }

            return code;
        }
        #endregion

        #region 判断检测机构是否填写现场监管检测的检测日期
        public bool JudgeXcjgjc(string qybh, string wtdwyhs, out string infoMsg, out string msg)
        {
            bool code = false;
            infoMsg = string.Empty;
            msg = string.Empty;

            try
            {
                //判断是否开启现场监管检测
                var sql = string.Format("select xcjgjc from i_m_qy where qybh = '{0}'", qybh);
                var xcjgjc = CommonDao.GetSingleData(sql).GetSafeBool();

                if (xcjgjc)
                {
                    sql = string.Format(@"select a.recid, c.jcrq
                                            from m_by a 
                                            inner join PR_M_SYXM b ON a.SYXMBH = b.SYXMBH AND a.YTDWBH = b.SSDWBH AND b.SFYX = 1 AND b.XCXM = 1
                                            left join I_S_Xcjgjc c on a.recid = c.WtdWyh
                                           where a.recid in ({0})", wtdwyhs.FormatSQLInStr());

                    var dts = CommonDao.GetDataTable(sql);

                    if (dts.Count() > 0)
                    {
                        foreach (var dt in dts)
                        {
                            if (string.IsNullOrEmpty(dt["jcrq"]))
                                infoMsg += string.Format("委托单唯一号[{0}]是现场项目,必须填写检测日期,没有填写则上传报告无效", dt["recid"]);
                        }
                    }
                }

                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }

            return code;
        }
        #endregion

        #region 删除企业信息
        [Transaction(ReadOnly = false)]
        public ResultParam DeleteIQy(string qybh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                if (string.IsNullOrEmpty(qybh))
                {
                    ret.msg = "参数不能为空";
                    return ret;
                }

                var gcCount = CommonDao.GetDataTable("select count(*) as c1 from View_GC_QY where qybh='" + qybh + "'");
                if (gcCount.Count > 0 && gcCount[0]["c1"].GetSafeInt() > 0)
                {
                    ret.msg = "企业已有工程，无法删除";
                    return ret;
                }

                var sql = string.Format("select count(1) from i_m_gc where ssjcjgbh='{0}'", qybh);
                var gcNum = CommonDao.GetSingleData(sql).GetSafeInt();

                if (gcNum > 0)
                {
                    ret.msg = "企业已有工程,无法删除";
                    return ret;
                }

                sql = string.Format("select zh from i_m_qy where qybh = '{0}'", qybh);
                var zh = CommonDao.GetSingleData(sql).GetSafeString();

                if (!string.IsNullOrEmpty(zh))
                {
                    ret.msg = "企业已存在账号,无法删除";
                    return ret;
                }

                CommonDao.ExecCommand("delete from I_M_QY where qybh='" + qybh + "'");
                CommonDao.ExecCommand("delete from I_M_QYZH where qybh='" + qybh + "' and SFQYZZH=1");
                CommonDao.ExecCommand("delete from I_S_QY_QYZZ where qybh='" + qybh + "' ");

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }

            return ret;
        }
        #endregion

        #region 设置受理委托编号
        /// <summary>
        /// 设置受理委托编号
        /// </summary>
        /// <param name="dwbh"></param>
        /// <param name="dataJson"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ResultParam SetSlWtdbh(string dwbh, List<Dictionary<string, string>> datas)
        {
            ResultParam ret = new ResultParam();
            List<string> list = new List<string>();
            try
            {
                foreach (var data in datas)
                {
                    var recid = data["wtdwyh"].GetSafeRequest();
                    var slWtdbh = data["slwtdbh"].GetSafeRequest();

                    if (string.IsNullOrEmpty(recid) || string.IsNullOrEmpty(slWtdbh))
                    {
                        ret.msg = "委托单唯一号或受理委托编号不能为空";
                        return ret;
                    }

                    IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(string.Format("select slwtdbh, sydwbh, zt from m_by where recid='{0}'", recid));

                    if (dt.Count() == 0)
                    {
                        ret.msg = string.Format("[{0}]无效的平台编号", recid);
                        return ret;
                    }

                    //重置委托单编号，默认为0, 重置为1
                    int czbj = 0;
                    if (data.ContainsKey("czbj"))
                    {
                        czbj = data["czbj"].GetSafeInt();
                    }

                    if (!string.IsNullOrEmpty(dt[0]["slwtdbh"]) && czbj == 0)
                    {
                        ret.msg = string.Format("[{0}]受理委托编号已经存在，不能重复更新", recid);
                        return ret;
                    }

                    string sydwbh = dt[0]["sydwbh"];
                    WtsStatus status = new WtsStatus(dt[0]["zt"]);

                    if (string.IsNullOrEmpty(sydwbh) || !status.HasWtdDown)
                    {
                        ret.msg = string.Format("[{0}]未送样的委托单不能更新受理委托编号", recid);
                        return ret;
                    }

                    if (sydwbh != dwbh)
                    {
                        ret.msg = "不能操作其他单位的委托单";
                        return ret;
                    }

                    var sql = string.Format("update m_by set slwtdbh='{0}', scwts=1, scwtsdz = '' where recid = '{1}'", slWtdbh, recid);
                    list.Add(sql);
                }

                ret.success = true;
                ret.data = list;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }

            return ret;
        }
        #endregion

        #region 获取委托单打印文件
        public ResultParam GetWtdDywj(Dictionary<string, string> data)
        {
            ResultParam ret = new ResultParam();
            try
            {
                var recid = data["wtdwyh"].GetSafeRequest();

                if (string.IsNullOrEmpty(recid))
                {
                    ret.msg = "参数不能为空";
                    return ret;
                }

                var dt = CommonDao.GetDataTable(string.Format("select recid, syxmbh, wtsmb, scwts, scwtsdz from m_by where recid='{0}'", recid));

                if (dt.Count() == 0)
                {
                    ret.msg = "委托单不存在";
                    return ret;
                }

                ret.success = true;
                ret.data = dt[0];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        } 
        #endregion

        #region 设置委托单试验状态
        [Transaction(ReadOnly = false)]
        public ResultParam SetWtdSyZt(string dwbh, Dictionary<string, string> data)
        {
            ResultParam ret = new ResultParam();
            string msg = string.Empty;
            try
            {
                var recid = data["wtdwyh"].GetSafeRequest();

                if (string.IsNullOrEmpty(recid))
                {
                    ret.msg = "参数不能为空";
                    return ret;
                }

                var dt = CommonDao.GetDataTable(string.Format("select recid, zt, sydwbh from m_by where recid='{0}'", recid));

                if (dt.Count() == 0)
                {
                    ret.msg = "委托单不存在";
                    return ret;
                }

                WtsStatus status = new WtsStatus(dt[0]["zt"]);

                //已委托未试验
                if (status.StateWt && !status.StateSy)
                {
                    if (!status.SetWtdSyZt(out msg))
                    {
                        ret.msg = msg;
                        return ret;
                    }

                    CommonDao.ExecCommand(string.Format("update m_by set SYKSSJ = getdate(),ZT='{0}' where recid='{1}' and zt like '{2}%'", status.GetStatus(), recid, WtsStatus.MainStateWt));
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }
        #endregion

        #region 设置委托单状态为下发到检测机构, 判断检测系统和监管系统委托单是否一致
        /// <summary>
        /// 设置委托单状态为下发到检测机构, 判断检测系统和监管系统委托单是否一致
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        public bool SetWtdStatusXf2(string qybh, IDictionary<string, string> data, out string msg, bool sdsy = false)
        {
            bool ret = false;
            //委托单唯一号
            string recid = data["wtdwyh"].GetSafeRequest();
            //委托单编号
            string wtdbh = data["wtdbh"].GetSafeRequest();
            //收样地址
            string sydz = "";
            if (data.ContainsKey("sydz"))
            {
                sydz = data["sydz"].GetSafeRequest();
            }
            //检测费
            string jcf = data["jcf"].GetSafeString();
            if (data.ContainsKey("jcf"))
            {
                jcf = data["jcf"].GetSafeDecimal().ToString();
            }

            msg = "";
            try
            {
                // 查询原始记录
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(
                    "select a.wtdbh, a.syxmbh,a.zt,a.sydwbh,a.jclx,b.jztpqyrq from m_by a left outer join h_zjz b on a.sszjzbh=b.zjzbh where a.recid='" +
                    recid + "'");
                if (dt.Count == 0)
                {
                    msg = "无效的记录号";
                    return ret;
                }

                // 判断状态
                string zt = dt[0]["zt"];
                string syxmbh = dt[0]["syxmbh"];
                string sydwbh = dt[0]["sydwbh"];
                string jclx = dt[0]["jclx"];
                DateTime qyrq = dt[0]["jztpqyrq"].GetSafeDate();
                WtsStatus objzt = new WtsStatus(zt);
                if (!objzt.CanWtsDown)
                {
                    if (sydwbh == qybh)
                        return true;
                    msg = "该委托单已经送样，不能再送样";
                    return ret;
                }

                //判断检测系统和监管系统中，委托单信息是否一致
                if (dt[0]["wtdbh"] != wtdbh)
                {
                    msg = "委托单已修改，请重新下载";
                    return ret;
                }

                //见证取样，并且质监站已经启用，进行比较
                if (!JudgeJzqy(qybh, jclx, qyrq, syxmbh, objzt.CanSetJzzt, out msg))
                {
                    return ret;
                }

                //判断检测机构资质
                if (!JudgeJcJgZZ(qybh, recid, syxmbh, out msg))
                {
                    return ret;
                }

                // 送样单位信息
                dt = CommonDao.GetDataTable("select qymc from i_m_qy where qybh='" + qybh + "'");
                if (dt.Count == 0)
                {
                    msg = "单位代码无效";
                    return ret;
                }

                string qymc = dt[0]["qymc"];

                // 设置送样机构和状态
                if (!objzt.SetWtdXf(out msg))
                    return ret;
                ret = CommonDao.ExecCommand(
                    "update m_by set zt='" + objzt.GetStatus() + "',sydwbh='" + qybh + "',sydwmc='" + qymc +
                    "',JYSJ= getdate(), sdsy = " + (sdsy ? 1 : 0) + ", ssjcf = " + jcf + ", SYJCBMDZ = '" + sydz + "' where recid='" + recid + "'", CommandType.Text);
                if (!ret)
                {
                    msg = "更新委托单状态失败";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        //见证取样，并且质监站已经启用，进行比较
        private bool JudgeJzqy(string qybh, string jclx, DateTime qyrq, string syxmbh, bool canSetJzzt, out string msg)
        {
            msg = string.Empty;

            if (SysUseSytp() && jclx == mStrJzqy && (qyrq.Year != 1900 && qyrq < DateTime.Now))
            {
                // 项目是否需要上传现场照片，如果要，需要见证人确认
                var dt = CommonDao.GetDataTable(string.Format("select SCJZQYTP from PR_M_SYXM where ssdwbh='{0}' and syxmbh='{1}'", qybh, syxmbh));
                if (dt.Count() == 0)
                {
                    msg = "找不到对应的项目";
                    return false;
                }

                bool scjzqytp = dt[0]["scjzqytp"].GetSafeString().ToLower() == "true";
                if (scjzqytp && canSetJzzt)
                {
                    msg = "需要见证人比对图片同意才能收样";
                    return false;
                }
            }

            return true;
        }

        //判断检测机构资质
        private bool JudgeJcJgZZ(string qybh, string wtdwyh, string syxmbh, out string msg)
        {
            msg = string.Empty;
            var dt = CommonDao.GetDataTable(string.Format("select jcxm from s_{0} where byzbrecid='{1}'", syxmbh, wtdwyh));
            if (dt.Count > 0)
            {
                IList<string> wtszbs = new List<string>();
                for (int i = 0; i < dt.Count; i++)
                {
                    string zbs = dt[i]["jcxm"].Trim();
                    if (zbs.GetSafeString() == "")
                        continue;
                    wtszbs = zbs.StringToList(new char[] { ',' }, true, wtszbs);
                }

                if (wtszbs.Count > 0)
                {
                    IList<string> jczxzbs = new List<string>();
                    dt = CommonDao.GetDataTable(string.Format("select distinct b.zbmc from pr_m_qyzb a inner join pr_m_zb b on a.zbbh=b.recid where a.yxqs<=getdate() and dateadd(d,1,a.yxqz)>getdate() and a.qybh='{0}' and b.sfyx=1 and b.syxmbh='{1}'", qybh, syxmbh));
                    for (int i = 0; i < dt.Count; i++)
                    {
                        string zb = dt[i]["zbmc"].Trim();
                        if (zb.GetSafeString() == "")
                            continue;
                        jczxzbs.Add(zb);
                    }

                    if (!jczxzbs.ListContains(wtszbs))
                    {
                        msg = "检测中心资质不够，无法送样";
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region 文件存储方式(1-OSS, 2-数据库)
        /// <summary>
        /// 文件存储方式(1-OSS, 2-数据库)
        /// </summary>
        /// <returns></returns>
        public int GetSysFileStorage()
        {
            return GetSysSettingValue("OTHER_SETTING_FILESTORAGE").GetSafeInt();
        } 
        #endregion

        #region 温州监管建研新见证流程(0-不启用, 1-启用)
        /// <summary>
        /// 温州监管建研新见证流程(0-不启用, 1-启用)
        /// </summary>
        /// <returns></returns>
        public int GetSysWzJgJyNewJzqy()
        {
            return GetSysSettingValue("OTHER_SETTING_WZJGJYNEWJZQY").GetSafeInt();
        }
        #endregion

        #region 人员录用申请(0-不启用, 1-启用)
        /// <summary>
        /// 人员录用申请(0-不启用, 1-启用)
        /// </summary>
        /// <returns></returns>
        public int GetSysRylysq()
        {
            return GetSysSettingValue("OTHER_SETTING_RYLYSQ").GetSafeInt();
        }
        #endregion

        #region 一个见证人员只允许添加一个工程(0-不启用, 1-启用)
        /// <summary>
        /// 一个见证人员只允许添加一个工程(0-不启用, 1-启用)
        /// </summary>
        /// <returns></returns>
        public int GetSysJzryZyxOneGc()
        {
            return GetSysSettingValue("OTHER_SETTING_JZRYZYXONEGC").GetSafeInt();
        }
        #endregion

        #region 检测机构区域审批(0-不启用,1-启用)
        /// <summary>
        /// 检测机构区域审批(0-不启用,1-启用)
        /// </summary>
        /// <returns></returns>
        public int GetSysJcjgQySp()
        {
            return GetSysSettingValue("OTHER_SETTING_JCJGQYSP").GetSafeInt();
        }
        #endregion

        #region 判断委托单修改申请
        public ResultParam JudgeWtdModifyApply(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select zt from m_by where recid = '{0}'", wtdwyh);
                var zt = CommonDao.GetSingleData(sql).GetSafeString();

                var status = new WtsStatus(zt);

                if (!status.HasWtdDown)
                {
                    ret.msg = "委托单还未送样, 不允许进行修改申请";
                    return ret;
                }

                if (status.HasWtdZf)
                {
                    ret.msg = "委托单已作废, 不允许进行修改申请";
                    return ret;
                }

                sql = string.Format("select count(1) from I_M_WtdModifyApply where wtdwyh = '{0}' and isApply = 1 and isAudit = 0", wtdwyh);
                var num = CommonDao.GetSingleData(sql).GetSafeInt();

                if (num > 0)
                {
                    ret.msg = "委托单正在申请修改,不允许重复申请";
                    return ret;
                }

                sql = string.Format("select applyreason from I_M_WtdModifyApply where wtdwyh = '{0}' and isApply = 0", wtdwyh);

                ret.success = true;
                ret.data = CommonDao.GetSingleData(sql).GetSafeString();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return ret;
        }
        #endregion

        #region 保存委托单修改申请
        [Transaction(ReadOnly = false)]
        public ResultParam SaveWtdModifyApply(string wtdwyh, string applyReason, string applyUser, string applyUserName)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select recid from I_M_WtdModifyApply where wtdwyh = '{0}' and isApply = 0", wtdwyh);
                string recid = CommonDao.GetSingleData(sql).GetSafeString();

                if (!string.IsNullOrEmpty(recid))
                {
                    sql = string.Format(@"update I_M_WtdModifyApply set ApplyReason = '{0}' where recid = '{1}'", applyReason, recid);
                }
                else
                {
                    recid = Guid.NewGuid().ToString();
                    sql = string.Format(@"insert into I_M_WtdModifyApply (RecId,WtdWyh,ApplyReason,ApplyUser,ApplyUserName,ApplyTime,IsSync,IsApply,IsAudit)
                                          values ('{0}', '{1}', '{2}', '{3}', '{4}', getdate(), 0, 0, 0) ", recid, wtdwyh, applyReason, applyUser, applyUserName);
                }

                var code = CommonDao.ExecCommand(sql);

                if (!code)
                    ret.msg = "执行失败";

                ret.success = true;
                ret.data = recid;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }

            return ret;
        }
        #endregion

        #region 获取委托单修改申请
        public ResultParam GetWtdModifyApply(string dwbh, Dictionary<string, string> data)
        {
            ResultParam ret = new ResultParam();
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();
            try
            {
                string where = string.Empty;

                if (data.ContainsKey("wtdwyh"))
                {
                    if (!string.IsNullOrEmpty(data["wtdwyh"]))
                        where = string.Format(" and a.wtdwyh = '{0}'", data["wtdwyh"]);
                }

                string sql = string.Format(@"select a.recid as applyid, a.wtdwyh, a.applyreason, a.applyusername, a.applytime, b.syxmbh
                                               from I_M_WtdModifyApply a inner join m_by b on a.wtdwyh = b.recid
                                              where a.isApply = 1 and a.isAudit = 0 and b.ytdwbh = '{0}' {1} order by a.applytime desc", dwbh, where);
                var dts = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select c.recid, c.applyid, c.field, c.fieldname, c.oldvalue, c.newvalue, c.modifytype from I_M_WtdModifyApply a inner join m_by b on a.wtdwyh = b.recid
                                        inner join I_S_WtdModifyLog c on a.RecId = c.ApplyId
                                       where a.isApply = 1 and a.isAudit = 0 and c.tabletype = 1 and b.ytdwbh = '{0}' {1}", dwbh, where);
                var mdts = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select c.recid, c.applyid, c.field, c.fieldname, c.oldvalue, c.newvalue, d.zh, c.modifytype, c.srecid from I_M_WtdModifyApply a inner join m_by b on a.wtdwyh = b.recid
                                        inner join I_S_WtdModifyLog c on a.RecId = c.ApplyId
                                        left join S_By d on c.SRecId = d.RecId
                                       where a.isApply = 1 and a.isAudit = 0 and c.tabletype = 2 and b.ytdwbh = '{0}' {1}", dwbh, where);
                var sdts = CommonDao.GetDataTable(sql);

                Dictionary<string, object> record = new Dictionary<string, object>();

                foreach (var dt in dts)
                {
                    record = new Dictionary<string, object>();
                    var applyId = dt["applyid"];

                    foreach (var value in dt)
                    {
                        record.Add(value.Key, value.Value);
                    }
                    
                    var mdt = mdts.Where(x => x["applyid"] == applyId).OrderBy(x => x["recid"].GetSafeInt()).ToList();
                    var sdt = sdts.Where(x => x["applyid"] == applyId).OrderBy(x => x["recid"].GetSafeInt()).ToList();

                    //新增类型增加组号
                    var addSdts = sdt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Add).ToList();
                    var addDict = addSdts.Where(x => x["field"].ToUpper() == "ZH").ToDictionary(x => x["srecid"], x => x["newvalue"]);

                    foreach (var addSdt in addSdts)
                    {
                        addSdt["zh"] = DictionaryHelper.GetValue(addDict, addSdt["srecid"]);
                    }

                    //删除类型去重
                    var tempSdts = sdt.Where(x => (x["modifytype"].GetSafeInt() != (int)WtdModifyTypeEnum.Delete)).ToList();
                    var deleteSdts = sdt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Delete && x["field"].ToUpper() == "RECID").ToList();

                    foreach (var deleteSdt in deleteSdts)
                    { 
                        if(tempSdts.FindIndex(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Delete && x["zh"] == deleteSdt["zh"]) == -1)
                        {
                            tempSdts.Add(deleteSdt);
                        }
                    }

                    record.Add("mdata", mdt);
                    record.Add("sdata", tempSdts);
                    records.Add(record);
                }

                //去除不要的字段
                foreach(var item in records)
                {
                    var mdt = item["mdata"] as List<IDictionary<string, string>>;

                    foreach (var mitem in mdt)
                    {
                        mitem.Remove("applyid");
                    }

                    var sdt = item["sdata"] as List<IDictionary<string, string>>;

                    foreach (var sitem in sdt)
                    {
                        sitem.Remove("applyid");
                    }
                }

                ret.success = true;
                ret.data = records;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return ret;
        } 
        #endregion

        #region 审核委托单修改申请
        [Transaction(ReadOnly = false)]
        public ResultParam AuditWtdModifyApply(string dwbh, Dictionary<string, string> data)
        {
            ResultParam ret = new ResultParam() {
                success = false,
                msg = string.Empty,
                data = string.Empty
            };

            try
            {
                var applyId = data["applyid"].GetSafeRequest();
                var auditUserName = data["auditusername"].GetSafeRequest();
                var auditType = data["audittype"].GetSafeInt();

                if (string.IsNullOrEmpty(applyId))
                {
                    ret.msg = "申请唯一号不能为空";
                    return ret;
                }

                if (auditType != (int)WtdApplyAuditEnum.Agree && auditType != (int)WtdApplyAuditEnum.DisAgree)
                {
                    ret.msg = "审核类型传入出错";
                    return ret;
                }

                string sql = string.Format(@"select a.WtdWyh, a.IsAudit, b.Syxmbh
                                               from I_M_WtdModifyApply a
                                              inner join M_BY b on a.wtdwyh = b.recid
                                              where a.RecId = '{0}'", applyId);

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该申请不存在";
                    return ret;
                }

                var wtdwyh = dt[0]["wtdwyh"];
                var isAudit = dt[0]["isaudit"].GetSafeInt();
                var syxmbh = dt[0]["syxmbh"];

                if (isAudit != (int)WtdApplyAuditEnum.NoAudit)
                {
                    //已审核的，返回成功
                    ret.success = true;
                    ret.msg = "该申请已经被审核，不允许重复审核";
                    return ret;
                }

                //更新申请表审核
                sql = string.Format(@"update I_M_WtdModifyApply
                                         set AuditUserName = '{0}',
                                             AuditTime = GetDate(),
	                                         IsAudit = '{1}'
                                       where RecId = '{2}'", auditUserName, auditType, applyId);
                CommonDao.ExecCommand(sql);

                if (auditType == (int)WtdApplyAuditEnum.DisAgree)
                {
                    //修改申请为未同意修改
                    sql = string.Format("update m_by set xgsq = {0} where recid = '{1}'", (int)WtdXgsqEnum.DisAgree, wtdwyh);
                    CommonDao.ExecCommand(sql);
                }
                else if (auditType == (int)WtdApplyAuditEnum.Agree)
                {
                    sql = string.Format("select * from I_S_WtdModifyLog where applyId = '{0}' and wtdwyh = '{1}'", applyId, wtdwyh);
                    var wtdModifyDt = CommonDao.GetDataTable(sql);

                    var mWtdModifyDt = wtdModifyDt.Where(x => x["tabletype"].GetSafeInt() == (int)WtdModifyTableTypeEnum.First).ToList();
                    var sWtdModifyDt = wtdModifyDt.Where(x => x["tabletype"].GetSafeInt() == (int)WtdModifyTableTypeEnum.Second).ToList();

                    var sWtdDeleteDt = sWtdModifyDt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Delete).ToList();
                    var sWtdUpdateDt = sWtdModifyDt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Update).ToList();
                    var sWtdAddDt = sWtdModifyDt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Add).ToList();
                    
                    //更新主表记录
                    string where = string.Format(" recid = '{0}'", wtdwyh);
                    var mDatas = GetNoRepeatList(mWtdModifyDt, "wtdwyh", "tablename");
                    foreach (var mData in mDatas)
                    {
                        var tempDt = mWtdModifyDt.Where(x => x["wtdwyh"] == mData.RecId && x["tablename"] == mData.TableName).ToList();
                        var zdzds = tempDt.ToDictionary(x => x["field"], x => x["newvalue"]);
                        sql = MakeSqlHelper.UpdateSql(mData.TableName, zdzds, where);
                        CommonDao.ExecCommand(sql);
                    }

                    //删除从表记录
                    var sDeleteDatas = GetNoRepeatList(sWtdDeleteDt, "srecid", "tablename");

                    foreach (var sDeleteData in sDeleteDatas)
                    {
                        where = string.Format(" recid = '{0}' and byzbrecid = '{1}' ", sDeleteData.RecId, wtdwyh);
                        sql = MakeSqlHelper.DeleteSql(sDeleteData.TableName, where);
                        CommonDao.ExecCommand(sql);
                    }

                    //更新从表记录
                    var sUpdateDatas = GetNoRepeatList(sWtdUpdateDt, "srecid", "tablename");
                    foreach (var sUpdateData in sUpdateDatas)
                    {
                        where = string.Format(" recid = '{0}' and byzbrecid = '{1}' ", sUpdateData.RecId, wtdwyh);
                        var tempDt = sWtdUpdateDt.Where(x => x["srecid"] == sUpdateData.RecId && x["tablename"] == sUpdateData.TableName).ToList();
                        var zdzds = tempDt.ToDictionary(x => x["field"], x => x["newvalue"]);
                        sql = MakeSqlHelper.UpdateSql(sUpdateData.TableName, zdzds, where);
                        CommonDao.ExecCommand(sql);
                    }

                    //添加从表记录
                    var sAddDatas = GetNoRepeatList(sWtdAddDt, "srecid", "tablename");
                    
                    foreach (var sAddData in sAddDatas)
                    {
                        var tempDt = sWtdAddDt.Where(x => x["srecid"] == sAddData.RecId && x["tablename"] == sAddData.TableName).ToList();
                        var zdzds = tempDt.ToDictionary(x => x["field"], x => x["newvalue"]);
                        sql = MakeSqlHelper.InsertSql(sAddData.TableName, zdzds);
                        CommonDao.ExecCommand(sql);
                    }

                    //去除委托单打印地址缓存, 修改申请为已同意修改
                    sql = string.Format("update m_by set scwts = 1, scwtsdz = '', xgsq = {0} where recid = '{1}'", (int)WtdXgsqEnum.Agree, wtdwyh);
                    CommonDao.ExecCommand(sql);

                    ret.data = wtdwyh;
                }
                
                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }

            return ret;
        }

        private List<WtdModifyData> GetNoRepeatList(List<IDictionary<string, string>> dts, string recid, string tablename)
        {
            List<WtdModifyData> list = new List<WtdModifyData>();

            foreach (var dt in dts)
            {
                var findIndex = list.FindIndex(x => x.RecId == dt[recid] && x.TableName == dt[tablename]);

                if (findIndex == -1)
                {
                    list.Add(new WtdModifyData
                    {
                        RecId = dt[recid],
                        TableName = dt[tablename]
                    });
                }
            }

            return list;
        }

        #endregion

        #region 根据委托单获取修改申请记录
        public ResultParam GetViewWtdModifyApply(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select * from I_M_WtdModifyApply where IsApply = 1 and WtdWyh = '{0}' order by ApplyTime desc", wtdwyh);
                var dt = CommonDao.GetDataTable(sql);

                StringBuilder data = new StringBuilder();

                foreach (var item in dt)
                {
                    data.AppendFormat("{0},{1}||", item["recid"], item["applytime"]);
                }

                ret.data = data.ToString();
                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return ret;
        } 
        #endregion

        #region 获取修改申请详情
        public ResultParam GetViewWtdModifyDetail(string applyId)
        {
            ResultParam ret = new ResultParam();
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string sql = string.Format("select * from I_M_WtdModifyApply where recid = '{0}'", applyId);
                var mdt = CommonDao.GetDataTable(sql);

                sql = string.Format("select a.*, b.zh from I_S_WtdModifyLog a left join S_By b on a.SRecId = b.RecId where a.applyid = '{0}'", applyId);
                var sdt = CommonDao.GetDataTable(sql);

                var smdt = sdt.Where(x => x["tabletype"].GetSafeInt() == (int)WtdModifyTableTypeEnum.First).OrderBy(x => x["recid"].GetSafeInt()).ToList();
                var ssdt = sdt.Where(x => x["tabletype"].GetSafeInt() == (int)WtdModifyTableTypeEnum.Second).ToList();
                //删除
                var sdeletesdt = ssdt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Delete).OrderBy(x => x["recid"].GetSafeInt()).ToList();
                //添加
                var saddsdt = ssdt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Add).OrderBy(x => x["recid"].GetSafeInt()).ToList();
                //修改
                var supdatesdt = ssdt.Where(x => x["modifytype"].GetSafeInt() == (int)WtdModifyTypeEnum.Update).OrderBy(x => x["recid"].GetSafeInt()).ToList();

                data.Add("mdata", mdt);
                data.Add("smdata", smdt);
                data.Add("sdeletedata", sdeletesdt);
                data.Add("sadddata", saddsdt);
                data.Add("supdatedata", supdatesdt);
                ret.data = data;
                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return ret;
        }
        #endregion

        #region 获取修改申请记录Id
        public ResultParam GetWtdModifyApplyId(string wtdwyh)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string sql = string.Format("select recid from I_M_WtdModifyApply where wtdwyh = '{0}' and isApply = 1 and isAudit = 0", wtdwyh);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该委托单的修改申请不存在";
                    return ret;
                }

                ret.success = true;
                ret.data = dt[0]["recid"];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }

            return ret;
        } 
        #endregion

        #region 获取现场项目类别
        public IList<IDictionary<string, string>> GetXcxmlb()
        {
            string sql = "select xcxmmc, syxmbhs, url from h_xcxmzs where sfyx = 1 order by orderno ";
            return CommonDao.GetDataTable(sql);
        }

        #endregion

        #region 获取现场项目数据

        public IList<IDictionary<string, string>> GetXcxmData(string syxmbhs, int pageSize, int pageIndex, out int totalCount)
        {
            string dateString = "2019-08-19";

            string sql = string.Format(@"select *
                                           from
                                        (select a.RECID, a.WTDBH, a.Z_LSH, a.YTDWMC, a.GCMC, a.SYXMBH, a.SYXMMC, c.XSFLMC as JCZZ,
                                            (Case When d.zt = 1 or (a.zt Not like 'W%' and (a.WTSLRSJ > '{0}' or a.SYKSSJ > '{1}')) Then '已试验' else '未试验' end) SFSYMC
                                            from m_by a inner join (SELECT a.SYXMBH, b.XSFLMC
                                            from PR_M_SYXM AS a INNER JOIN PR_M_SYXMXSFL AS b ON a.XSFLBH1 = b.XSFLBH 
                                            AND b.SSDWBH = '' AND b.SFYX = 1
                                        WHERE a.SSDWBH = '' AND a.SFYX = 1) c on a.SYXMBH = c.SYXMBH
                                            left join YS_SJZS d on a.recid = d.wtdwyh
                                       where a.SYXMBH in ({2})) a order by a.SFSYMC desc", dateString, dateString, syxmbhs.FormatSQLInStr());
                
            return CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);      
        }
        #endregion

        #region 设置委托单审核签发
        [Transaction(ReadOnly = false)]
        public ResultParam SetWtdSHQF(string dwbh, Dictionary<string, string> data)
        {
            ResultParam ret = new ResultParam();
            try
            {
                var recid = data["wtdwyh"].GetSafeRequest();

                if (string.IsNullOrEmpty(recid))
                {
                    ret.msg = "参数不能为空";
                    return ret;
                }

                var dt = CommonDao.GetDataTable(string.Format("select recid, zt, sydwbh from m_by where recid='{0}'", recid));

                if (dt.Count() == 0)
                {
                    ret.msg = "委托单不存在";
                    return ret;
                }

                //if (dwbh != dt[0]["sydwbh"])
                //{
                //    ret.msg = "不能操作其他单位的委托单";
                //    return ret;
                //}

                var zt = data["zt"].GetSafeInt();
                string fszt = string.Empty;

                if (zt == 1)
                    fszt = "30";
                else if (zt == 2)
                    fszt = "31";
                else
                {
                    ret.msg = "传入的不是审核和签发状态";
                    return ret;
                }

                string sql = string.Format(@"insert into up_jcgj (RECID, WTDBH, FSZT, FSSJ, FSRYXM, FSDD)
                values ('{0}', '{1}', '{2}', getdate(), '', '')", Guid.NewGuid(), recid, fszt);
                var result = CommonDao.ExecCommand(sql);

                if (!result)
                {
                    ret.msg = "执行出错";
                    return ret;
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }
        #endregion

        #region 获取建研见证取样配置
        public IList<IDictionary<string, string>> GetJyJzqyHelpLink(string syxmbhs)
        {
            string sql = string.Format(@"select a.syxmbh, a.syxmmc, b.helplnk
                                           from pr_m_syxm a
                                           left join datazdzd_individualproject b on a.syxmbh = b.syxmbh
                                          where a.ssdwbh = ''
                                            and a.syxmbh in ({0})", syxmbhs.FormatSQLInStr());
             return CommonDao.GetDataTable(sql);
        }
        #endregion

        #region 设置建研委托清单传递参数
        [Transaction(ReadOnly = false)]
        public ResultParam InsertJyWtqd(string wtqdContent)
        {
            ResultParam ret = new ResultParam();
            try
            {
                var recid = Guid.NewGuid().ToString();
                string sql = string.Format("insert into i_s_jywtqd (recid, wtqdcontent, createTime) values ('{0}', '{1}', getdate())", recid, wtqdContent);
                var result = CommonDao.ExecCommand(sql);

                if (!result)
                {
                    ret.msg = "执行出错";
                    return ret;
                }

                ret.success = true;
                ret.data = recid;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }
        #endregion

        #region 获取建研委托清单传递参数
        public string GetJyWtqd(string recid)
        {
            var sql = string.Format("select wtqdcontent from i_s_jywtqd where recid = '{0}'", recid);
            return CommonDao.GetSingleData(sql).GetSafeString();
        }
        #endregion

        #region 人员录用申请审核
        [Transaction(ReadOnly = false)]
        public ResultParam RylySqSh(string recids, string shr, string shrxm)
        {
            ResultParam ret = new ResultParam();
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();
            try
            {
                string sql = string.Format("select * from I_S_RY_LYSQ where SFSH = 0 and recid in ({0})", recids.FormatSQLInStr());
                var dts = CommonDao.GetDataTable(sql);

                foreach (var dt in dts)
                {
                    sql = string.Format(@"insert into I_S_RY_BGHJ (rybh, ryxm, czr, czrxm, czsj, czlx) 
                      values ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}')", dt["rybh"], dt["ryxm"], dt["czr"], dt["czrxm"], 1);
                    CommonDao.ExecCommand(sql);

                    sql = string.Format(@"update i_m_ry set qybh= (select qybh from i_m_qyzh where yhzh= '{0}') 
                    where rybh = '{1}' and (qybh is null or qybh = '')", dt["czr"], dt["rybh"]);
                    CommonDao.ExecCommand(sql);

                    sql = string.Format("update I_S_RY_LYSQ set shr = '{0}', shrxm = '{1}', shsj = getdate(), sfsh = 1 where recid = '{2}'", shr, shrxm, dt["recid"]);
                    CommonDao.ExecCommand(sql);
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }

            return ret;
        }
        #endregion

        #region 获取需要同步的工程见证坐标
        /// <summary>
        /// 获取需要同步的工程见证坐标
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetSyncGcPos(string startTime, string endTime)
        {
            IList<IDictionary<string, string>> result = new List<IDictionary<string, string>>();

            try
            {
                string sql = string.Format(@"select b.gcbh, null qylongitude, null qylatitude, a.longitude jzlongitude , a.latitude jzlatitude, a.wtdwyh
                                               from 
                                            (select distinct a.wtdwyh, a.longitude, a.latitude
                                                from UP_WTDTP a
	                                          where a.sfyx = 1
                                                and a.longitude > 0
                                                and a.latitude > 0
	                                            and a.scsj >= '{0}'
                                                and a.scsj <= '{1}')a inner join m_by b on a.wtdwyh = b.recid", startTime, endTime);

                result = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        
            return result;
        }
        #endregion

        #region 设置工程见证坐标记录
        [Transaction(ReadOnly = false)]
        public ResultParam InsertGcPos(IDictionary<string, string> dict)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select count(1) from up_gcpos where gcbh = '{0}' and wtdwyh = '{1}' and JzLongitude = '{2}' and JzLatitude = '{3}'",
                     dict["gcbh"], dict["wtdwyh"], dict["jzlongitude"], dict["jzlatitude"]);

                int count = CommonDao.GetSingleData(sql).GetSafeInt();

                if (count > 0)
                {
                    ret.success = true;
                    return ret;
                }

                sql = string.Format("insert into up_gcpos (gcbh, jzlongitude, jzlatitude, wtdwyh, createtime) values ('{0}', '{1}', '{2}', '{3}', getdate())",
                     dict["gcbh"], dict["jzlongitude"], dict["jzlatitude"], dict["wtdwyh"]);

                CommonDao.ExecCommand(sql);

                string jzzb = dict["jzlongitude"] + "," + dict["jzlatitude"];
                sql = string.Format("update i_m_gc set jzzb = '{0}' where gcbh = '{1}' and (jzzb is null or jzzb = '')", jzzb, dict["gcbh"]);
                CommonDao.ExecCommand(sql);

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }
        #endregion

        #region 数据库配置值
        public string GetSysConfigValue(string configKey)
        {
            string sql = string.Format("select configValue from sysConfigs where configKey = '{0}'", configKey);
            return CommonDao.GetSingleData(sql).GetSafeString();
        }

        public bool SetSysConfigValue(string configKey, string configValue)
        {
            string sql = string.Format("update sysConfigs set configValue = '{0}' where configKey = '{1}'", configValue, configKey);
            return CommonDao.ExecSql(sql);
        }
        #endregion

        #region 现场检测数据查看地址
        public string GetXcjcUrl()
        {
            return GetSysSettingValue("OTHER_SETTING_XCJCCKDZ");
        } 
        #endregion

        #region 现场检测桩号修改申请
        /// <summary>
        /// 保存现场检测桩号修改申请
        /// </summary>
        /// <param name="syid"></param>
        /// <param name="newptbh"></param>
        /// <param name="newzh"></param>
        /// <param name="oldptbh"></param>
        /// <param name="oldzh"></param>
        /// <param name="applyUser"></param>
        /// <param name="applyUserName"></param>
        /// <param name="applyReason"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public ResultParam SaveXcjcModifyApply(string busynessid, string syid, string newptbh, string newzh, string oldptbh, string oldzh,
            string applyUser, string applyUserName, string applyReason)
        {
            ResultParam ret = new ResultParam();
            try
            {
                if (string.IsNullOrEmpty(newptbh) || string.IsNullOrEmpty(newzh) || string.IsNullOrEmpty(applyReason))
                {
                    ret.msg = "平台流水号,桩号,申请原因不能为空";
                    return ret;
                }

                string sql = string.Format(@"select count(1) from I_M_XcjcModifyApply where busynessid = '{0}' and syid = '{1}' and Audit = 0", busynessid, syid);
                int count = CommonDao.GetSingleData(sql).GetSafeInt();

                if (count > 0)
                {
                    ret.msg = "修改申请正在审核中, 无法再次提交";
                    return ret;
                }

                if (newptbh == oldptbh && newzh == oldzh)
                {
                    ret.msg = "平台流水号和桩号都没有修改,不需要申请";
                    return ret;
                }

                string recid = Guid.NewGuid().ToString();

                sql = string.Format(@"insert into I_M_XcjcModifyApply (recid, busynessid, syid, applyreason, applyuser, applyusername, applytime, audit)
                values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', getdate(), 0)", recid, busynessid, syid, applyReason, applyUser, applyUserName);
                CommonDao.ExecCommand(sql);

                sql = string.Format(@"insert into I_S_XcjcModifyLog (applyid, field, fieldname, oldvalue, newvalue) 
                           values ('{0}', '{1}', '{2}', '{3}', '{4}')", recid, "ptbh", "平台流水号", oldptbh, newptbh);
                CommonDao.ExecCommand(sql);

                sql = string.Format(@"insert into I_S_XcjcModifyLog (applyid, field, fieldname, oldvalue, newvalue) 
                           values ('{0}', '{1}', '{2}', '{3}', '{4}')", recid, "zh", "桩号", oldzh, newzh);
                    CommonDao.ExecCommand(sql);

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// 获取修改申请记录
        /// </summary>
        /// <param name="syids"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetXcjcModifyApply(string syids)
        {
            string sql = string.Format(@"select * from I_M_XcjcModifyApply where Syid in ({0}) order by ApplyTime desc", syids.FormatSQLInStr());
            return CommonDao.GetDataTable(sql);
        }

        /// <summary>
        /// 获取修改申请的内容
        /// </summary>
        /// <param name="syid"></param>
        /// <returns></returns>
        public ResultParam GetXcjcModifyContent(string busynessid, string syid)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select recid from I_M_XcjcModifyApply where busynessid = '{0}' and syid = '{1}' and audit = 0", busynessid, syid);
                var applyId = CommonDao.GetSingleData(sql).GetSafeString();

                if (string.IsNullOrEmpty(applyId))
                {
                    ret.msg = "没有申请修改,无法审核";
                    return ret;
                }

                sql = string.Format("select * from I_S_XcjcModifyLog where applyId = '{0}'", applyId);
                var dts = CommonDao.GetDataTable(sql);
                var dict = dts.ToDictionary(x => x["field"], x => x["newvalue"]);
                var ptbh = DictionaryHelper.GetValue(dict, "ptbh");
                var zh = DictionaryHelper.GetValue(dict, "zh");

                if (string.IsNullOrEmpty(ptbh) || string.IsNullOrEmpty(zh))
                {
                    ret.msg = "获取平台流水号,桩号更新值出错";
                    return ret;
                }
                
                Dictionary<string, string> data = new Dictionary<string,string>();
                data.Add("applyid", applyId);
                data.Add("ptbh", ptbh);
                data.Add("zh", zh);

                ret.success = true;
                ret.data = data;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取所有的修改申请内容
        /// </summary>
        /// <param name="syid"></param>
        /// <returns></returns>
        public ResultParam GetAllXcjcModifyContent(string busynessid, string syid)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select * from I_M_XcjcModifyApply where busynessid = '{0}' and syid = '{1}' order by applytime desc", busynessid, syid);
                var mdts = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select b.*
                                        from I_M_XcjcModifyApply a
                                        inner join I_S_XcjcModifyLog b on a.RecId = b.ApplyId
                                        where busynessid = '{0}' and syid = '{1}' order by applytime desc", busynessid, syid);
                var sdts = CommonDao.GetDataTable(sql);

                List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
                string applycontent = string.Empty;

                foreach (var mdt in mdts)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("applyreason", mdt["applyreason"]);
                    dict.Add("applytime", mdt["applytime"]);

                    var tempsdts = sdts.Where(x => x["applyid"] == mdt["recid"]).ToList();
                    applycontent = string.Empty;
                    foreach(var sdt in tempsdts)
                    {
                        applycontent += string.Format("{0}:{1}->{2};", sdt["fieldname"], sdt["oldvalue"], sdt["newvalue"]);
                    }

                    dict.Add("applycontent", applycontent);
                    dict.Add("audit", mdt["audit"].GetSafeInt() == 1 ? "审批通过" : "已申请");
                    data.Add(dict);
                }

                ret.success = true;
                ret.data = data;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 审核修改申请记录
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="auditUser"></param>
        /// <param name="auditUserName"></param>
        /// <param name="audit"></param>
        /// <returns></returns>
        public ResultParam AuditXcjcModifyApply(string applyId, string auditUser, string auditUserName, int audit)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format(@"update I_M_XcjcModifyApply set AuditUser = '{0}', AuditUserName = '{1}', AuditTime = getdate(), Audit = '{2}'
                           where recid = '{3}'", auditUser, auditUserName, audit, applyId);

                ret.success = CommonDao.ExecSql(sql);

                if (!ret.success)
                    ret.msg = "sql执行失败";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 校验检测机构区域内外是否已经审批
        public ResultParam CheckJcjgQySp(string dwbh, string htbh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                if (GetSysJcjgQySp() == (int)SysJcjgQySpEnum.Enabled)
                {
                    var sql = string.Format("select qybs from i_m_qy where qybh = '{0}'", dwbh);
                    var qybs = CommonDao.GetSingleData(sql).GetSafeBool();

                    //区域外
                    if (qybs)
                    {
                        sql = string.Format("select htbh from i_m_jcht where recid = '{0}'", htbh);
                        var jchtbh = CommonDao.GetSingleData(sql).GetSafeString();

                        if (string.IsNullOrEmpty(jchtbh))
                        {
                            ret.msg = "区域外检测机构必须走检测合同流程,该合同没有关联检测合同,请联系检测机构处理";
                            return ret;
                        }
                    }
                    //区域内
                    else
                    {
                        sql = string.Format("select max(expirydate) from i_m_jcjg_qynsq where audit = 1 and jcjgbh = '{0}'", dwbh);
                        var expiryDate = CommonDao.GetSingleData(sql).GetSafeDate();

                        if (DateTime.Compare(expiryDate, DateTime.Now) < 0)
                        {
                            ret.msg = "区域内检测机构需要向质检站申请通过后才允许接业务,请联系检测机构处理";
                            return ret;
                        }
                    }
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 区域内检测机构审批
        public ResultParam JcJgQynSp(string recids, string expiryDate, string userCode, string realName)
        {
            ResultParam ret = new ResultParam();
            try
            {
                var sql = string.Format(@"update i_m_jcjg_qynsq set audituser = '{0}', auditusername = '{1}', 
                    audittime = getdate(), audit = 1, expirydate = '{2}' where recid in ({3}) and audit = 0", userCode, realName, expiryDate, recids.FormatSQLInt());

                CommonDao.ExecSql(sql);
                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        } 
        #endregion

        #region 初始化建研见证取样配置
        public ResultParam InitJyJzqyHelpLink(string url)
        {
            ResultParam ret = new ResultParam();
            try
            {
                var sql = @"SELECT SYXMBH FROM pr_m_syxm
                             WHERE (SSDWBH = '') AND (SCJZQYTP = 1)
                               AND syxmbh NOT IN (SELECT syxmbh
                                FROM DATAZDZD_INDIVIDUALPROJECT)
                            ORDER BY  XSFLBH1, XSFLBH, XSSX";

                var dt = CommonDao.GetDataTable(sql);

                foreach (var item in dt)
                {
                    var syxmbh = item["syxmbh"].ToUpper();
                    string zdzdTable = string.Format("zdzd_{0}", syxmbh);
                    var sTable = string.Format("s_{0}", syxmbh);

                    sql = string.Format(@"select * from {0} where sjbmc = '{1}'
                                            and sfxs = 1
                                            and lx like '%W%'
                                            and zdmc <> 'jcxm'
                                        order by xssx asc", zdzdTable, sTable);

                    var sZdzds = CommonDao.GetDataTable(sql);

                    var temp = string.Empty;
                    var tempName = string.Empty;
                    var tempCtrl = string.Empty;

                    foreach (var sZdzd in sZdzds)
                    {
                        temp += string.Format(",{0}", sZdzd["zdmc"].ToUpper());
                        tempName += string.Format(",{0}", sZdzd["sy"]);
                        tempCtrl += string.Format(",{0}.{1}", sZdzd["sjbmc"].ToUpper(), sZdzd["zdmc"].ToUpper());
                    }

                    var fieldName = "ISJZ,QRCODE,JZRXM,GCBW" + temp + ",JZRECID,JZRBH,EDITSTATUS";
                    var fieldMc = "是否见证,见证二维码,见证人姓名,工程部位" + tempName + ",见证取样唯一号,见证人编号,修改状态";
                    var targetField = "JZRECID,JZRBH,JZRBH,GCBW" + temp;
                    var targetCtrl = "S_BY.JZRECID,M_BY.JZRBH__main,S_BY.JZRBH,S_BY.GCBW" + tempCtrl;
                    var readonlyCtrl = "S_BY.GCBW" + tempCtrl;
                    var whereCtrlCustom = "M_BY.GCBH,M_BY.SYXMBH";
                    var whereField = "GCBH,SYXMBH";
                    var js = "checkfunwtdjyjzqy.js";
                    var checkfun = "check";

                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    dict["syxmbh"] = syxmbh;
                    dict["sylbzdzd"] = "XTZD_BY";
                    dict["sjbmc"] = "S_BY";
                    dict["zdmc"] = "JZRECID";
                    dict["sy"] = "获取样品信息";
                    dict["zdlx"] = "nvarchar";
                    dict["zdcd1"] = "50";
                    dict["zdcd2"] = "0";
                    dict["inputzdlx"] = "";
                    dict["kjlx"] = "";
                    dict["sfbhzd"] = "0";
                    dict["bhms"] = "";
                    dict["zdsx"] = "1";
                    dict["sfxs"] = "1";
                    dict["xscd"] = "350";
                    dict["xssx"] = "2.10";
                    dict["sfgd"] = "0";
                    dict["mustin"] = "0";
                    dict["defaval"] = "";
                    dict["helplnk"] = string.Format("helplink--url-{0}|fieldname-{1}|fieldmc-{2}|targetfield-{3}|targetctrl-{4}|wherectrl_custom-{5}|wherefield-{6}|readonlyctrl-{7}|js-{8}|checkfun-{9}|datatype-serviceRow",
                        url, fieldName, fieldMc, targetField, targetCtrl, whereCtrlCustom, whereField, readonlyCtrl, js, checkfun);
                    dict["ctrlstring"] = "";
                    dict["validproc"] = "";
                    dict["msginfo"] = "";
                    dict["lx"] = "W";

                    sql = MakeSqlHelper.InsertSql("DATAZDZD_INDIVIDUALPROJECT", dict);
                    CommonDao.ExecSql(sql);
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 判断是否使用标点检测系统
        public ResultParam JudgeBDJcxt(string qybh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select bdjcxt from i_m_qy where qybh = '{0}'", qybh);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该企业不存在";
                    return ret;
                }

                if (dt[0]["bdjcxt"].GetSafeBool())
                {
                    ret.msg = "该检测机构使用标点检测系统，请去检测系统中操作";
                    return ret;
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        } 
        #endregion

        #region 获取采集异常字段排除
        public IList<IDictionary<string, string>> GetCjycZdpc(string syxmbh)
        {
            string sql = string.Format("select * from PR_M_CJYC_ZDPC where syxmbh = '{0}'", syxmbh);
            return CommonDao.GetDataTable(sql);
        } 
        #endregion

        #region 获取报告异常字段排除
        public IList<IDictionary<string, string>> GetBgycZdpc(string syxmbh)
        {
            string sql = string.Format("select * from PR_M_BGYC_ZDPC where syxmbh = '{0}'", syxmbh);
            return CommonDao.GetDataTable(sql);
        } 
        #endregion

        #region 插入不合格报告的回复内容
        public ResultParam InsertBhgBgHf(string bgwyh, string userCode, string userName, string hfnr, int hflx)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select count(1) num from I_S_BG_BHGHF where bgwyh = '{0}' and hflx = {1}", bgwyh, hflx);
                var num = CommonDao.GetSingleData(sql).GetSafeInt();

                if (num > 0)
                {
                    sql = string.Format(@"update I_S_BG_BHGHF set hfnr = '{0}', hfsj = getdate() where bgwyh = '{1}' and hflx = {2}", hfnr, bgwyh, hflx);
                }
                else
                {
                    sql = string.Format(@"insert into I_S_BG_BHGHF (Bgwyh, Hfrzh, Hfrxm, Hfnr, Hfsj, Hflx) values 
                    ('{0}', '{1}', '{2}', '{3}', getdate(), {4})", bgwyh, userCode, userName, hfnr, hflx);
                }

                CommonDao.ExecSql(sql);
                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 共用函数 
        /// <summary>
        /// 判断唯一号是否存在
        /// </summary>
        /// <returns></returns>
        public ResultParam CheckWtdwyh(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //SQL语句
                string sql = String.Format("select count(*) num from m_by where recid= '{0}'", wtdwyh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                //判断记录是否存在
                if (dt[0]["num"].GetSafeInt() == 0)
                {
                    ret.data = "0";
                }
                else
                {
                    ret.data = "1";
                }
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 判断唯一号是否存在及返回对应的单据类型信息
        /// </summary>
        /// <returns></returns>
        public ResultParam CheckWtdwyhData(string wtdwyh)
        {
            ResultParam ret = new ResultParam();
            IDictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                //SQL语句
                string sql = String.Format("select syxmbh, syxmmc from m_by where recid= '{0}'", wtdwyh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                //判断记录是否存在
                if (dt.Count == 0)
                {
                    data.Add("sfjgwtd", false);
                    data.Add("syxmbh","");
                    data.Add("syxmmc","");
                }
                else
                {
                    data.Add("sfjgwtd", true);
                    data.Add("syxmbh", dt[0]["syxmbh"].GetSafeString());
                    data.Add("syxmmc", dt[0]["syxmmc"].GetSafeString());
                }

                ret.data = data;
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        #endregion
        #region 第三方平台接口
        #region 萧山协会
        /// <summary>
        /// 获取需要上传字段的接口
        /// </summary>
        /// <param name="syxmbh"></param>
        /// <param name="status"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public ResultParam XsxhInterface_UploadField(string syxmbh, string status)
        {
            ResultParam ret = new ResultParam();
            //对象返回项
            IDictionary<string, object> data = new Dictionary<string, object>();
            //判断是两块两材还是非两块两材
            string sql = String.Format("select lx from SysYcjk_Xsxh_SyxmbhCode where SYXMBH = '{0}'", syxmbh);
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            //判断试验项目配置情况
            if (dt.Count == 0)
            {
                ret.msg = String.Format("试验项目【{0}】未配置！", syxmbh);
                return ret;
            }
            //判断试验项目字段是否存在
            sql = String.Format("select count(*) as num from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}'", syxmbh);
            IList<IDictionary<string, string>> dtField = CommonDao.GetDataTable(sql);
            if (dtField[0]["num"].GetSafeInt() == 0)
            {
                ret.msg = String.Format("试验项目字段【{0}】未配置！", syxmbh);
                return ret;
            }
            //获取试验类型是两块还是非两块
            string lx = dt[0]["lx"].GetSafeString();
            //非两块两材
            if (lx == "0")
            {
                //标点系统
                if (status == "1")
                {
                    sql = String.Format("select sjbmc,zdmc,jkzdm,sy,type from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' order by ORDERNO", syxmbh);
                    //公共信息
                    data.Add("basic",CommonDao.GetDataTable(sql));
                }
                //第三方检测系统
                else
                {
                    sql = String.Format("select jkzdm,sy from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' order by ORDERNO", syxmbh);
                    //公共信息
                    data.Add("basic", CommonDao.GetDataTable(sql));
                }
            }
            //两块两材
            else if (lx == "1")
            {
                //标点系统
                if (status == "1")
                {
                    //公共信息
                    sql = String.Format("select sjbmc,zdmc,jkzdm,sy,type from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' and SIGN = '1' order by ORDERNO", syxmbh);          
                    data.Add("basic", CommonDao.GetDataTable(sql));
                    //样品信息
                    sql = String.Format("select sjbmc,zdmc,jkzdm,sy,type from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' and SIGN = '2' order by ORDERNO", syxmbh);          
                    data.Add("sample", CommonDao.GetDataTable(sql));
                }
                //第三方检测系统
                else
                {
                    //公共信息
                    sql = String.Format("select jkzdm,sy from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' and SIGN = '1' order by ORDERNO", syxmbh);          
                    data.Add("basic", CommonDao.GetDataTable(sql));
                    //样品信息
                    sql = String.Format("select jkzdm,sy from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' and SIGN = '2' order by ORDERNO", syxmbh);          
                    data.Add("sample", CommonDao.GetDataTable(sql));
                }
            }
            else
            {
                ret.msg = String.Format("试验项目【{0}】两块类型未配置！", syxmbh);
                return ret;
            }
            //返回结果
            ret.data = data;
            ret.success = true;
            return ret;
        }

        public IList<IDictionary<string, string>> getSysYcjkLx()
        {
            string sql = "select * from SysYcjkLx";
            return CommonDao.GetDataTable(sql);
        }
        public bool getSysYcjkLx(string qybh, out string msg)
        {
            string sql = string.Format("select * from SysYcjkQy where qybh='{0}'", qybh);
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            if (dt.Count > 0)
            {
                msg = dt[0]["recid"];
                return true;
            }
            else
            {
                msg = "";
                return false;
            }
        }

        /// <summary>
        /// 获取品铭工程信息
        /// </summary>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public ResultParam XspmInterface_DownloadProject(string startRq, string endRq)
        {
            ResultParam ret = new ResultParam();
            //判断程序是否运行中
            if (XsPinMingService.isRun)
            {
                ret.msg = "品铭工程下载中，请稍后... ...";
                return ret;
            }

            try
            {
                //设置服务运行状态
                XsPinMingService.isRun = true;
                //获取品铭数据包
                ResultParam optRet = XsPinMingService.GetProjectInfo(startRq, endRq);
                //判断结果
                if (!optRet.success)
                {
                    ret.msg = optRet.msg;
                    return ret;
                }
                //获取对象信息
                InterfaceXspmProject projectInfo = (InterfaceXspmProject)optRet.data;
                //判断记录是否存在
                if (projectInfo.total == 0)
                {
                    ret.success = true;
                    return ret;
                }
                //变量
                string msg = "";
                //会话
                ISession session = WebDataInputDao.GetDBSession();
                IDbCommand cmd = session.Connection.CreateCommand();
                //SQL
                StringBuilder sql = new StringBuilder();
                //数据包
                IList<IDictionary<string, string>> dt = null;
                //工程信息

                //企业信息
                string qybh = "";
                //质效编号
                string zzbh = "";
                //企业类型编号
                string qylxbh = "";
                //遍历工程信息
                foreach (var item in projectInfo.rows)
                {
                    //遍历每个工程的企业信息
                    foreach (var cpItem in item.Units)
                    {
                        //判断企业远程编号是否存在
                        sql.Clear();
                        sql.AppendFormat("select qybh from I_M_QY where QYBH_YC = '{0}'", cpItem.UnitCode);
                        dt = CommonDao.GetDataTableTran(sql.ToString());
                        if (dt.Count == 0)
                        {
                            //判断企业远程编号是否存在
                            sql.Clear();
                            sql.AppendFormat("select qybh from I_M_QY where QYMC = '{0}'", cpItem.UnitName);
                            dt = CommonDao.GetDataTableTran(sql.ToString());
                            if (dt.Count == 0)
                            {
                                bool firstOpt = true;
                                //生成企业编号
                                qybh = WebDataInputDao.GetBH(
                                    "TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_M_QY__QYBH'|maxbhfield-ZDBH",
                                    "I_M_QY", "QYBH", null, cmd, ref firstOpt);
                                sql.Clear();
                                sql.Append("insert into I_M_QY(");
                                sql.Append("QYBH,");
                                sql.Append("QYBH_YC,");
                                sql.Append("QYMC,");
                                sql.Append("QYFR,");
                                sql.Append("LXDH,");
                                sql.Append("LXBH,");
                                sql.Append("SPTG,");
                                sql.Append("SFYX");
                                sql.Append(") values(");
                                sql.AppendFormat("'{0}',", qybh); //企业编号
                                sql.AppendFormat("'{0}',", cpItem.UnitCode); //企业远程编号
                                sql.AppendFormat("'{0}',", cpItem.UnitName); //企业名称
                                sql.AppendFormat("'{0}',", cpItem.LegalName); //企业法人
                                sql.AppendFormat("'{0}',", cpItem.LegalPhone); //联系电话
                                sql.AppendFormat("'{0}',", "00"); //类型编号
                                sql.AppendFormat("{0},", "1"); //审批通过
                                sql.AppendFormat("{0}", "1"); //是否有效
                                sql.Append(")");
                                CommonDao.ExecSqlTran(sql.ToString());
                                //创建远程用户账号
                                sql.Clear();
                                sql.AppendFormat("update i_m_qy set zh=qybh where qybh='{0}' and (zh is null or zh='')",
                                    qybh);
                                CommonDao.ExecSqlTran(sql.ToString());
                                //添加企业资质
                                firstOpt = true;
                                //生成企业编号
                                //判断企业资质类型
                                qylxbh = "";
                                //监理单位
                                if (cpItem.UnitType == 2)
                                {
                                    qylxbh = "12";
                                }
                                //施工单位
                                else if (cpItem.UnitType == 1)
                                {
                                    qylxbh = "11";
                                }
                                //建设单位
                                else if (cpItem.UnitType == 5)
                                {
                                    qylxbh = "13";
                                }
                                //设计单位
                                else if (cpItem.UnitType == 4)
                                {
                                    qylxbh = "14";
                                }

                                zzbh = WebDataInputDao.GetBH(
                                    "TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_QY_QYZZ__ZZBH'|maxbhfield-ZDBH",
                                    "I_S_QY_QYZZ", "ZZBH", null, cmd, ref firstOpt);
                                sql.Clear();
                                sql.Append("insert into I_S_QY_QYZZ(");
                                sql.Append("ZZBH,");
                                sql.Append("QYBH,");
                                sql.Append("QYLXBH,");
                                sql.Append("sptg,");
                                sql.Append("sfyx");
                                sql.Append(") values (");
                                sql.AppendFormat("'{0}',", zzbh); //资质编号
                                sql.AppendFormat("'{0}',", qybh); //企业编号
                                sql.AppendFormat("'{0}',", qylxbh); //企业类型编号
                                sql.AppendFormat("{0},", "1"); //审批同意
                                sql.AppendFormat("{0}", "1"); //是否有效
                                sql.Append(")");
                                CommonDao.ExecSqlTran(sql.ToString());
                                // 查找单位信息，获取单位类型、代码、名称
                                sql.Clear();
                                sql.AppendFormat("select lxbh,qymc,zh from i_m_qy where qybh='{0}'", qybh);
                                dt = CommonDao.GetDataTableTran(sql.ToString());
                                if (dt.Count == 0)
                                {
                                    throw new Exception(String.Format("企业编号：{0}的企业类型不存在！", qybh));
                                }

                                string username = dt[0]["zh"].GetSafeString();
                                // 查找企业类型信息，获取默认单位、部门、角色    
                                sql.Clear();
                                sql.AppendFormat("select * from h_qylx where lxbh='{0}'", dt[0]["lxbh"]);
                                dt = CommonDao.GetDataTableTran(sql.ToString());
                                if (dt.Count == 0)
                                {
                                    throw new Exception(String.Format("企业编号：{0}找不到企业类型记录！", qybh));
                                }

                                // 不用创建账号，返回
                                if (!dt[0]["sfcjzh"].GetSafeBool())
                                {
                                    continue;
                                }

                                string companycode = dt[0]["zhdwbh"];
                                string depcode = dt[0]["zhbmbh"];
                                string rolecodelist = dt[0]["zhjsbh"];
                                string[] rolelist = rolecodelist.Split(',');
                                string rolecode = rolelist[0];
                                string postcode = dt[0]["gwbh"];
                                // 判断账号是否已创建
                                sql.Clear();
                                sql.AppendFormat("select * from i_m_qyzh where yhzh='{0}'", username);
                                dt = CommonDao.GetDataTableTran(sql.ToString());
                                if (dt.Count > 0)
                                {
                                    continue;
                                }

                                //初始化密码
                                string password = GlobalVariableConfig.GetDefaultUserPass();
                                if (password == "")
                                    password = RandomNumber.GetNew(RandomType.Number,
                                        GlobalVariableConfig.GetUserPasswordLength());
                                bool code = BD.Jcbg.Service.Jc.BdUserService.AddUser(companycode, depcode, username,
                                    cpItem.UnitName, rolecode, postcode, password, out msg);
                                if (!code)
                                {
                                    throw new Exception(String.Format("创建用户失败，用户名：{0}！", username));
                                }

                                string yhzh = msg;
                                if (rolelist.Length > 1)
                                {
                                    for (int i = 1; i < rolelist.Length; i++)
                                    {
                                        BD.Jcbg.Service.Jc.BdUserService.AddUserRole(username, rolelist[i], out msg);
                                    }
                                }

                                sql.Clear();
                                sql.Append("insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj,zhlx) values('" + qybh +
                                           "','" + yhzh + "',1,'','',getdate(),'Q')");
                                CommonDao.ExecSqlTran(sql.ToString());
                                //更新权限
                                sql.Clear();
                                sql.AppendFormat(
                                    "select zhjsbh from h_qylx where lxbh in (select qylxbh from i_s_qy_qyzz where qybh='" +
                                    qybh +
                                    "' and sptg=1 and sfyx=1) or lxbh='00' or lxbh in (select lxbh from i_m_qy where qybh='" +
                                    qybh + "')");
                                dt = CommonDao.GetDataTableTran(sql.ToString());

                                IList<string> roleCodes = new List<string>();
                                foreach (IDictionary<string, string> row in dt)
                                    roleCodes.Add(row["zhjsbh"].GetSafeString());
                                BD.Jcbg.Service.Jc.BdUserService.UpdateUserRole(username, "", roleCodes, out msg);
                            }
                            else
                            {
                                qybh = dt[0]["qybh"].GetSafeString();
                            }
                        }
                        //获取企业编号
                        else
                        {
                            qybh = dt[0]["qybh"].GetSafeString();
                        }
                        //赋值企业编号
                        cpItem.qybh = qybh;
                    }     
                }
                //添加监督工程信息
                //工程编号
                string gcbh = "";
                //企业信息中的工程企业编号
                string gcqybh = "";
                //企业类型表
                string qylxtb = "";
                //遍历工程
                foreach (var item in projectInfo.rows)
                {
                    //判断工程是否存在
                    sql.Clear();
                    sql.AppendFormat("select count(*) num from I_M_GC where isnull(SSJCJGBH,'') = '' and isnull(SJGCBH,'')='' and GCMC = '{0}'", item.ProjectName);
                    dt = CommonDao.GetDataTableTran(sql.ToString());
                    if (dt[0]["num"].GetSafeInt() == 0)
                    {
                        //获取工程编号
                        bool firstOpt = true;
                        //生成企业编号
                        gcbh = WebDataInputDao.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_M_GC__GCBH'|maxbhfield-ZDBH", "I_M_GC", "GCBH", null, cmd, ref firstOpt);
                        //插入工程信息
                        sql.Clear();
                        sql.Append("insert into I_M_GC(");
                        sql.Append("GCBH,");
                        sql.Append("GCMC,");
                        sql.Append("ZJDJH,");
                        sql.Append("JZMJ,");
                        sql.Append("GCLXBH,");
                        sql.Append("GCZB,");
                        sql.Append("LRRZH,");
                        sql.Append("LRRXM,");
                        sql.Append("LRSJ,");
                        sql.Append("ZJZBH");
                        sql.Append(") values (");
                        sql.AppendFormat("'{0}',", gcbh);
                        sql.AppendFormat("'{0}',", item.ProjectName);
                        sql.AppendFormat("'{0}',", item.RegNumber);
                        sql.AppendFormat("'{0}',", item.BuildingArea);
                        sql.AppendFormat("'{0}',", item.ProjectCategory == "1" || item.ProjectCategory == "2" ? String.Format("0{0}",item.ProjectCategory) : "");
                        sql.AppendFormat("'{0}',", item.Coordinate);
                        sql.AppendFormat("'{0}',", item.AddUserName);
                        sql.AppendFormat("'{0}',", item.AddTrueName);
                        sql.AppendFormat("'{0}',", item.AddDate);
                        sql.AppendFormat("'{0}'", "01");
                        sql.Append(")");
                        CommonDao.ExecSqlTran(sql.ToString());
                        //插入企业信息
                        //遍历每个工程的企业信息
                        foreach (var cpItem in item.Units)
                        {
                            //企业类型表参数清空
                            qylxtb = "";
                            //获取单位信息
                            sql.Clear();
                            sql.AppendFormat("select * from I_M_QY where QYBH = '{0}'", cpItem.qybh);
                            dt = CommonDao.GetDataTableTran(sql.ToString());
                            //判断企业类型
                            //监理单位
                            if (cpItem.UnitType == 2)
                            {
                                //生成工程企业编号
                                gcqybh = WebDataInputDao.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_GC_JLDW__GCQYBH'|maxbhfield-ZDBH", "I_S_GC_JLDW", "GCQYBH", null, cmd, ref firstOpt);
                                qylxtb = "I_S_GC_JLDW";                     
                            }
                            //施工单位
                            else if (cpItem.UnitType == 1)
                            {
                                //生成工程企业编号
                                gcqybh = WebDataInputDao.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_GC_SGDW__GCQYBH'|maxbhfield-ZDBH", "I_S_GC_SGDW", "GCQYBH", null, cmd, ref firstOpt);
                                qylxtb = "I_S_GC_SGDW";  
                            }
                            //建设单位
                            else if (cpItem.UnitType == 5)
                            {
                                //生成工程企业编号
                                gcqybh = WebDataInputDao.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_GC_JSDW__GCQYBH'|maxbhfield-ZDBH", "I_S_GC_JSDW", "GCQYBH", null, cmd, ref firstOpt);
                                qylxtb = "I_S_GC_JSDW"; 
                            }
                            //设计单位
                            else if (cpItem.UnitType == 4)
                            {
                                //生成工程企业编号
                                gcqybh = WebDataInputDao.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_GC_SJDW__GCQYBH'|maxbhfield-ZDBH", "I_S_GC_SJDW", "GCQYBH", null, cmd, ref firstOpt);
                                qylxtb = "I_S_GC_SJDW";
                            }
                            //勘察单位
                            else if (cpItem.UnitType == 3)
                            {
                                //生成工程企业编号
                                gcqybh = WebDataInputDao.GetBH("TB:table-PR_M_BHMS|fieldname-BHMS|customwhere-BHMSJZ='I_S_GC_KCDW__GCQYBH'|maxbhfield-ZDBH", "I_S_GC_KCDW", "GCQYBH", null, cmd, ref firstOpt);
                                qylxtb = "I_S_GC_KCDW";
                            }

                            //判断SQL长度
                            if (qylxtb != "")
                            {
                                sql.Clear();
                                sql.AppendFormat("insert into {0}(", qylxtb);
                                sql.Append("GCBH,");
                                sql.Append("GCQYBH,");
                                sql.Append("QYBH,");
                                sql.Append("QYMC,");
                                sql.Append("LXDH,");
                                sql.Append("QYFZR");
                                sql.Append(") values (");
                                sql.AppendFormat("'{0}',", gcbh);
                                sql.AppendFormat("'{0}',", gcqybh);
                                sql.AppendFormat("'{0}',", cpItem.qybh);
                                sql.AppendFormat("'{0}',", dt[0]["QYMC"]);
                                sql.AppendFormat("'{0}',", dt[0]["LXDH"].GetSafeString());
                                sql.AppendFormat("'{0}'", dt[0]["QYFZR"].GetSafeString());
                                sql.Append(")");                          
                                CommonDao.ExecSqlTran(sql.ToString());
                            }
                        }
                    }
                }

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(String.Format("保存工程信息出错，原因：{0}", ex.Message));
                ret.msg = ex.Message;
                throw new Exception(String.Format("保存工程信息出错，原因：{0}", ex.Message));
            }
            finally
            {
                //设置服务运行状态
                XsPinMingService.isRun = false;
            }
            return ret;
        }
        #endregion
        #endregion

        #region 采集系统接口
        /// <summary>
        /// 判断用户登录是否正确
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultParam CjxtCheckUser(string data)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //解析数据包
                string msg = "";
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断参数
                if (dataDict.Count == 0)
                {
                    ret.msg = "数据格式不正确！";
                    return ret;
                }
                //用户名
                if (dataDict["username"].GetSafeString() == "")
                {
                    ret.msg = "用户名不能为空！";
                    return ret;
                }
                //密码
                if (dataDict["userpwd"].GetSafeString() == "")
                {
                    ret.msg = "密码不能为空！";
                    return ret;
                }
                //检验类型
                string checkType = "";
                if (dataDict.ContainsKey("checktype"))
                    checkType = dataDict["checktype"].GetSafeString();
                //调用用户系统
                string timestring = TimeUtil.GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = Configs.UmsUrl + "/Api/Service";
                string dates = "method=User&opt=CheckUserByUsernameAndPwd&checktype=" + checkType + "&username=" + dataDict["username"].GetSafeString() + "&userpwd=" + dataDict["userpwd"].GetSafeString() + "&timestring=" + timestring + "&sign=" + sign;
                //访问登录
                string html = MyHttp.SendDataByPost(url, dates);
                //返回数据包
                var retDic = JsonSerializer.Deserialize<Dictionary<string, object>>(html, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断处理内容
                if (retDic.Count == 0)
                {
                    ret.msg = "返回信息有误！";
                    return ret;
                }
                //判断处理结果
                if (!retDic["success"].GetSafeBool())
                {
                    ret.msg = retDic["msg"].GetSafeString();
                    return ret;
                }
                //用户信息
                var infoDic = JsonSerializer.Deserialize<Dictionary<string, string>>(retDic["data"].GetSafeString(), out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断返回数据
                if (infoDic.Count == 0)
                {
                    ret.msg = "用户返回信息不存在！";
                    return ret;
                }
                //用户代码
                string usercode = infoDic["usercode"].GetSafeString();
                string realname = infoDic["realname"].GetSafeString();
                #region 用户权限
                timestring = TimeUtil.GetTimeStamp();
                sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                dates = "method=Power&opt=GetUserPowerListByProtypeAndUsername&procode=" + Configs.AppId + "&username=" + dataDict["username"].GetSafeString() + "&timestring=" + timestring + "&sign=" + sign;
                //访问登录
                html = MyHttp.SendDataByPost(url, dates);
                //返回数据包
                retDic = JsonSerializer.Deserialize<Dictionary<string, object>>(html, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断处理内容
                if (retDic.Count == 0)
                {
                    ret.msg = "返回信息有误！";
                    return ret;
                }
                //判断处理结果
                if (!retDic["success"].GetSafeBool())
                {
                    ret.msg = retDic["msg"].GetSafeString();
                    return ret;
                }
                //权限包
                var powerDic = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(retDic["data"].GetSafeString(), out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                #endregion
                //返回数据包
                IDictionary<string, object> dic = new Dictionary<string, object>();
                //用户信息
                IDictionary<string, object> userinfoDic = new Dictionary<string, object>();
                //业务单位编号
                userinfoDic.Add("qybh", GetQybh(usercode));
                //部门代码
                userinfoDic.Add("bmdm", "");
                //用户编号
                userinfoDic.Add("usercode", usercode);
                //用户姓名
                userinfoDic.Add("realname", realname);          
                //权限信息(采集权限 CJXTCJ 采集重做权限 CJXTCZ)
                IDictionary<string, object> userpowerDic = new Dictionary<string, object>();
                //判断是否有采集权限
                var q = from item in powerDic where item["menucode"] == "CJSY_CJSB_CJ" select item;
                userpowerDic.Add("cjqx", q.Count() > 0 ? true : false);
                //判断是否有重做权限
                q = from item in powerDic where item["menucode"] == "CJSY_CJSB_CZ" select item;
                userpowerDic.Add("czqx", q.Count() > 0 ? true : false);
                //返回数据包定义
                dic.Add("info", userinfoDic);
                dic.Add("power", userpowerDic);
                ret.data = dic;
                ret.msg = "登录成功！";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据参数获取用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultParam CjxtGetUserInfo(string data)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //解析数据包
                string msg = "";
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断参数
                if (dataDict.Count == 0)
                {
                    ret.msg = "数据格式不正确！";
                    return ret;
                }
                //用户名
                if (dataDict["username"].GetSafeString() == "")
                {
                    ret.msg = "用户名不能为空！";
                    return ret;
                }
 
                //调用用户系统
                string timestring = TimeUtil.GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = Configs.UmsUrl + "/Api/Service";
                string dates = "method=User&opt=GetUserByUsername&username=" + dataDict["username"].GetSafeString() + "&timestring=" + timestring + "&sign=" + sign;
                //访问登录
                string html = MyHttp.SendDataByPost(url, dates);
                //返回数据包
                var retDic = JsonSerializer.Deserialize<Dictionary<string, object>>(html, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断处理内容
                if (retDic.Count == 0)
                {
                    ret.msg = "返回信息有误！";
                    return ret;
                }
                //判断处理结果
                if (!retDic["success"].GetSafeBool())
                {
                    ret.msg = retDic["msg"].GetSafeString();
                    return ret;
                }         
                //返回数据包
                ret.data = retDic["data"];
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据参数获取用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultParam CjxtUserPower(string data)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //解析数据包
                string msg = "";
                var dataDict = JsonSerializer.Deserialize<Dictionary<string, string>>(data, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断参数
                if (dataDict.Count == 0)
                {
                    ret.msg = "数据格式不正确！";
                    return ret;
                }
                //用户名
                if (dataDict["username"].GetSafeString() == "")
                {
                    ret.msg = "用户名不能为空！";
                    return ret;
                }
                //调用用户系统
                string timestring = TimeUtil.GetTimeStamp();
                string sign = MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring));
                string url = Configs.UmsUrl + "/Api/Service";
                string dates = "method=Power&opt=GetUserPowerListByProtypeAndUsername&procode=" + Configs.AppId + "&username=" + dataDict["username"].GetSafeString() + "&timestring=" + timestring + "&sign=" + sign;
                //访问登录
                string html = MyHttp.SendDataByPost(url, dates); 
                //返回数据包
                var retDic = JsonSerializer.Deserialize<Dictionary<string, object>>(html, out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //判断处理内容
                if (retDic.Count == 0)
                {
                    ret.msg = "返回信息有误！";
                    return ret;
                }
                //判断处理结果
                if (!retDic["success"].GetSafeBool())
                {
                    ret.msg = retDic["msg"].GetSafeString();
                    return ret;
                }
                //权限包
                var powerDic = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(retDic["data"].GetSafeString(), out msg);
                if (msg != "")
                {
                    ret.msg = msg;
                    return ret;
                }
                //返回数据包(采集权限 CJXTCJ 采集重做权限 CJXTCZ)
                IDictionary<string, object> dic = new Dictionary<string, object>();
                //判断是否有采集权限
                var q = from item in powerDic where item["menucode"] == "CJSY_CJSB_CJ" select item;
                dic.Add("cjqx", q.Count() > 0 ? true : false);
                //判断是否有重做权限
                q = from item in powerDic where item["menucode"] == "CJSY_CJSB_CZ" select item;
                dic.Add("czqx", q.Count() > 0 ? true : false);
                //返回
                ret.data = dic;
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 统一入口
        /// <summary>
        /// 服务类(2019-10-19 杨鑫钢)
        /// </summary>
        /// <returns></returns>
        public ResultParam Service()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //方法
            string method = request["method"].GetSafeString();
            //处理类
            switch (method)
            {
                //黑名单
                case "Jghmd":
                    ret = sJghmd();
                    break;
                //委托单
                case "Wtd":
                    ret = sWtdData();
                    break;
                //混凝土企业
                case "Hntqy":
                    ret = sHntQyData();
                    break;
            }

            return ret;
        }

        #region 黑名单
        /// <summary>
        /// 黑名单处理
        /// </summary>
        /// <returns></returns>
        private ResultParam sJghmd()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //处理方法
            string opt = request["opt"].GetSafeString();
            //处理类
            switch (opt)
            {
                //删除
                case "del":
                    ret = sJghmd_d();
                    break;
                //检测用户指定机构黑名单
                case "hmd":
                    ret = sJghmd_hmd();
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 黑名单删除
        /// </summary>
        /// <returns></returns>
        private ResultParam sJghmd_d()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //获取数据包
            string hmdh = request.Form["hmdh"].GetSafeString();
            if (hmdh == "")
            {
                ret.msg = "单号不能为空！";
                return ret;
            }
            //删除操作
            string sql = String.Format("delete from I_M_JGHMD where HMDH = '{0}'", hmdh);
            if (!CommonDao.ExecCommandOpenSession(sql, CommandType.Text))
            {
                ret.msg = "删除失败！";
                return ret;
            }
            //返回
            ret.msg = "删除成功！";
            ret.success = true;
            return ret;
        }

        /// <summary>
        /// 检验黑名单
        /// </summary>
        /// <returns></returns>
        private ResultParam sJghmd_hmd()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //获取数据包
            string qybh = request.Form["qybh"].GetSafeString();
            if (qybh == "")
            {
                ret.msg = "企业信息不能为空！";
                return ret;
            }
            //当前用户编号
            string rybh = HttpContext.Current.Session["USERBH"].GetSafeString();
            if (rybh == "")
            {
                ret.msg = "超时，请重新登录";
                return ret;
            }

            try
            {
                string rq = TimeUtil.GetDate();
                string sql = String.Format("select max(isnull(tys,0)) as tys from I_M_JGHMD where SSDWBH = '{0}' and KSRQ <= '{1}' and JSRQ >= '{1}'", qybh, rq);
                IList<IDictionary<string, string>> hmdDt = CommonDao.GetDataTable(sql);
                if (hmdDt.Count == 0)
                {
                    ret.success = true;
                    return ret;
                }
                //获取最大停用数
                int tys = hmdDt[0]["tys"].GetSafeInt();
                if (tys == 0)
                {
                    ret.success = true;
                    return ret;
                }
                //判断人员限制记录是否存在
                sql = String.Format("select * from I_S_JGHMD_RY where SSDWBH = '{0}' and RYBH = '{1}'", qybh, rybh);
                IList<IDictionary<string, string>> ryhmdDt = CommonDao.GetDataTable(sql);
                //插入限制人员
                if (ryhmdDt.Count == 0)
                {
                    sql = String.Format("insert into I_S_JGHMD_RY(SSDWBH, RYBH, KSSJ, TYS) values('{0}','{1}','{2}',{3})", qybh, rybh, TimeUtil.GetDateTime(), tys);
                    CommonDao.ExecSql(sql);
                    sql = String.Format("select * from I_S_JGHMD_RY where SSDWBH = '{0}' and RYBH = '{1}'", qybh, rybh);
                    ryhmdDt = CommonDao.GetDataTable(sql);
                }
                //判断原有记录是否已到期

                //获取限制超时数
                DateTime dtStart = ryhmdDt[0]["KSSJ"].GetSafeDate();
                TimeSpan midTime = DateTime.Now - dtStart;
                if (midTime.TotalMinutes > tys)
                {
                    ret.success = true;
                    return ret;
                }
                //返回等待数
                ret.msg = String.Format("此检测机构受限，您还需等待{0}分钟即可填单！", Math.Round(Math.Abs(tys - midTime.TotalMinutes),0));
                return ret;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }
        #endregion

        #region 委托单
        /// <summary>
        /// 委托单
        /// </summary>
        /// <returns></returns>
        private ResultParam sWtdData()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //处理方法
            string opt = request["opt"].GetSafeString();
            //处理类
            switch (opt)
            {
                //判断是否现场项目
                case "checkXcxm":
                    ret = sWtd_checkXcxm();
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 判断是否现场项目
        /// </summary>
        /// <returns></returns>
        private ResultParam sWtd_checkXcxm()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            try
            {
                //请求对象
                HttpRequest request = HttpContext.Current.Request;
                //获取数据包
                string wtdwyh = request.Form["wtdwyh"].GetSafeString();
                if (wtdwyh == "")
                {
                    ret.msg = "单号不能为空！";
                    return ret;
                }
                //获取试验项目
                string sql = String.Format("select syxmbh from m_by where recid='{0}'", wtdwyh);
                IList<IDictionary<string, string>> dic = CommonDao.GetDataTable(sql);
                if (dic.Count == 0)
                {
                    ret.msg = "记录不存在！";
                    return ret;
                }

                string syxmbh = dic[0]["syxmbh"].GetSafeString();
                //根据试验项目获取是否现场项目
                sql = String.Format("select xcxm from PR_M_SYXM where syxmbh='{0}' and isnull(SSDWBH,'')=''", syxmbh);
                dic = CommonDao.GetDataTable(sql);
                if (dic.Count == 0)
                {
                    ret.msg = "试验项目不存在！";
                    return ret;
                }
                //判断是否为现场项目
                //返回
                ret.data = dic[0]["xcxm"].GetSafeBool();
                ret.msg = "获取成功！";
                ret.success = true;
            }
            catch (Exception e)
            {
                ret.msg = e.Message;
            }
            return ret;
        }
        #endregion

        #region
        /// <summary>
        /// 混凝土企业
        /// </summary>
        /// <returns></returns>
        private ResultParam sHntQyData()
        {
            //返回对象
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //处理方法
            string opt = request["opt"].GetSafeString();
            //处理类
            switch (opt)
            {
                //企业申报同意
                case "qysb_ty":
                    ret = sHntQy_qysb_tyData();
                    break;
                //企业申报审核
                case "qysb_sh":
                    ret = sHntQy_qysb_shData();
                    break;
                //企业申报审批
                case "qysb_sp":
                    ret = sHntQy_qysb_spData();
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 同意
        /// </summary>
        /// <returns></returns>
        private ResultParam sHntQy_qysb_tyData()
        {
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //id
            string recid = request.Form["recid"].GetSafeString();
            if (recid == "")
            {
                ret.msg = "处理编号不存在！";
                return ret;
            }

            try
            {
               
                string sql = String.Format("select ZT from I_M_QYSB_HNTQY where recid={0}", recid);
                IList<IDictionary<string, string>> dic = CommonDao.GetDataTable(sql);
                if (dic.Count == 0)
                {
                    ret.msg = "记录不存在！";
                    return ret;
                }
                string zt = dic[0]["zt"].GetSafeString();
                if (zt != "")
                {
                    ret.msg = "只允许经过未处理单据！";
                    return ret;
                }

                sql = String.Format("update I_M_QYSB_HNTQY set TYR = '{0}', TYSJ = '{1}', ZT='YJB' where RECID = {2}", HttpContext.Current.Session["REALNAME"], TimeUtil.GetDateTime(), recid);
                CommonDao.ExecSql(sql);

                ret.msg = "经办成功！";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }

            return ret;
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        private ResultParam sHntQy_qysb_shData()
        {
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //id
            string recid = request.Form["recid"].GetSafeString();
            if (recid == "")
            {
                ret.msg = "处理编号不存在！";
                return ret;
            }

            try
            {
                string sql = String.Format("select ZT from I_M_QYSB_HNTQY where recid={0}", recid);
                IList<IDictionary<string, string>> dic = CommonDao.GetDataTable(sql);
                if (dic.Count == 0)
                {
                    ret.msg = "记录不存在！";
                    return ret;
                }
                string zt = dic[0]["zt"].GetSafeString();
                if (zt != "YJB")
                {
                    ret.msg = "只允许处理“已经办”单据！";
                    return ret;
                }
                sql = String.Format("update I_M_QYSB_HNTQY set SHR = '{0}', SHSJ = '{1}', ZT='YSH' where RECID = {2}", HttpContext.Current.Session["REALNAME"], TimeUtil.GetDateTime(), recid);
                CommonDao.ExecSql(sql);

                ret.msg = "审核成功！";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <returns></returns>
        private ResultParam sHntQy_qysb_spData()
        {
            ResultParam ret = new ResultParam();
            //请求对象
            HttpRequest request = HttpContext.Current.Request;
            //id
            string recid = request.Form["recid"].GetSafeString();
            if (recid == "")
            {
                ret.msg = "处理编号不存在！";
                return ret;
            }

            try
            {
                string sql = String.Format("select ZT from I_M_QYSB_HNTQY where recid={0}", recid);
                IList<IDictionary<string, string>> dic = CommonDao.GetDataTable(sql);
                if (dic.Count == 0)
                {
                    ret.msg = "记录不存在！";
                    return ret;
                }
                string zt = dic[0]["zt"].GetSafeString();
                if (zt != "YSH")
                {
                    ret.msg = "只允许处理“已审核”单据！";
                    return ret;
                }
                sql = String.Format("update I_M_QYSB_HNTQY set SPR = '{0}', SPSJ = '{1}', ZT='YSP' where RECID = {2}", HttpContext.Current.Session["REALNAME"], TimeUtil.GetDateTime(), recid);
                CommonDao.ExecSql(sql);

                ret.msg = "审批成功！";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }

            return ret;
        }
        #endregion
        #endregion

        #region 大屏数据
        /// <summary>
        /// 获取检测机构与工程数
        /// </summary>
        /// <returns></returns>
        public ResultParam GetScreenJcjgWtds()
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql =
                    "select substring(a.qymc,1,4) as szjd,isnull(b.num,0) as total from (select QYBH, QYMC from View_I_M_QY where lxbh='01' or exists (select * from i_s_qy_qyzz where i_s_qy_qyzz.qybh=View_I_M_QY.qybh and i_s_qy_qyzz.qylxbh='01')) a left join (select ssjcjgbh,count(gcbh) as num from I_M_GC where isnull(ssjcjgbh,'')<>'' group by ssjcjgbh) b on a.QYBH = b.ssjcjgbh order by b.num desc,a.QYMC";
                IList<IDictionary<string, string>> dic = CommonDao.GetDataTable(sql);
                ret.data = dic;
                ret.code = "0";
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取基础数据统计
        /// </summary>
        /// <returns></returns>
        public ResultParam GetScreenJbsjTj()
        {
            ResultParam ret = new ResultParam();
            try
            {
                IList<IDictionary<string, string>> dics = new List<IDictionary<string, string>>();
                IList<IDictionary<string, string>> dic = null;
                IDictionary<string, string> item = null;
                string sql = "";
                //工程数 机构数 委托单数
                item = new Dictionary<string, string>();
                item.Add("lx","ALL");
                //工程数
                sql = "select count(*) as num from i_m_gc where (isnull(sjgcbh,'') = '') or (isnull(sjgcbh,'') <> '' and isnull(sjgcbh,'')='')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("total", dic[0]["num"].GetSafeString());
                //机构数
                sql = "select count(*) as num from View_I_M_QY where lxbh='01' or exists (select * from i_s_qy_qyzz where i_s_qy_qyzz.qybh=View_I_M_QY.qybh and i_s_qy_qyzz.qylxbh='01')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("mj", dic[0]["num"].GetSafeString());
                //委托单数
                sql = "select count(*) as num from m_by";
                dic = CommonDao.GetDataTable(sql);
                item.Add("gczj", dic[0]["num"].GetSafeString());
                dics.Add(item);
                //施工单位数 人员 委托单
                item = new Dictionary<string, string>();
                item.Add("lx", "ZF");
                //施工单位数
                sql = "select count(*) as num from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%施工企业%'";
                dic = CommonDao.GetDataTable(sql);
                item.Add("total", dic[0]["num"].GetSafeString());
                //人员
                sql ="select count(*) as num from i_m_ry where qybh in (select qybh from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%施工企业%')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("mj", dic[0]["num"].GetSafeString());
                //委托单
                sql = "select count(*) as num from m_by where sgdwbh in (select qybh from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%施工企业%')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("gczj", dic[0]["num"].GetSafeString());
                dics.Add(item);
                //建设单位数 人员 委托单
                item = new Dictionary<string, string>();
                item.Add("lx", "GY");
                //建设单位数
                sql = "select count(*) as num from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%建设企业%'";
                dic = CommonDao.GetDataTable(sql);
                item.Add("total", dic[0]["num"].GetSafeString());
                //人员
                sql = "select count(*) as num from i_m_ry where qybh in (select qybh from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%建设企业%')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("mj", dic[0]["num"].GetSafeString());
                //委托单
                sql = "select count(*) as num from m_by where sgdwbh in (select qybh from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%建设企业%')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("gczj", dic[0]["num"].GetSafeString());
                dics.Add(item);
                //监管单位数 人员 委托单
                item = new Dictionary<string, string>();
                item.Add("lx", "FC");
                //监理单位数
                sql = "select count(*) as num from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%监理企业%'";
                dic = CommonDao.GetDataTable(sql);
                item.Add("total", dic[0]["num"].GetSafeString());
                //人员
                sql = "select count(*) as num from i_m_ry where qybh in (select qybh from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%监理企业%')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("mj", dic[0]["num"].GetSafeString());
                //委托单
                sql = "select count(*) as num from m_by where sgdwbh in (select qybh from View_I_M_QY where sptg=1 and sfyx=1 and lxmc like '%监理企业%')";
                dic = CommonDao.GetDataTable(sql);
                item.Add("gczj", dic[0]["num"].GetSafeString());
                dics.Add(item);
                ret.data = dics;
                ret.code = "0";
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }


        /// <summary>
        /// 本月委托单与报告
        /// </summary>
        /// <returns></returns>
        public ResultParam GetByWtdAndBg()
        {
            ResultParam ret = new ResultParam();
            try
            {
                IDictionary<string, string> dics = new Dictionary<string, string>();
                IList<IDictionary<string, string>> dic = null;
                //监督工程
                string sql = String.Format("select count(*) as num from i_m_gc where isnull(ssjcjgbh,'') = '' and isnull(sjgcbh,'')=''");
                dic = CommonDao.GetDataTable(sql);
                dics.Add("bybjs", dic[0]["num"].GetSafeString());
                //非监督工程
                sql = String.Format("select count(*) as num from i_m_gc where isnull(ssjcjgbh,'') <> '' and isnull(sjgcbh,'')=''");
                dic = CommonDao.GetDataTable(sql);
                dics.Add("jnljs", dic[0]["num"].GetSafeString());
                //当年委托单
                sql = String.Format("select count(*) as num from m_by where substring(CONVERT(varchar(10), wtslrsj, 120),1,4)='{0}'", TimeUtil.GetYear());
                dic = CommonDao.GetDataTable(sql);
                dics.Add("bybjgcmj", dic[0]["num"].GetSafeString());
                //当年报告
                sql = String.Format("select count(*) as num from up_bgsj where substring(CONVERT(varchar(10), scsj, 120),1,4)='{0}'", TimeUtil.GetYear());
                dic = CommonDao.GetDataTable(sql);
                dics.Add("jnbjgcmj", dic[0]["num"].GetSafeString());
                //委托单数
                sql = String.Format("select count(*) as num from m_by where substring(CONVERT(varchar(10), wtslrsj, 120),1,7)='{0}'",TimeUtil.GetMonth());
                dic = CommonDao.GetDataTable(sql);
                dics.Add("byjggcs", dic[0]["num"].GetSafeString());
                //报告数
                sql = String.Format("select count(*) as num from up_bgsj where substring(CONVERT(varchar(10), scsj, 120),1,7)='{0}'", TimeUtil.GetMonth());
                dic = CommonDao.GetDataTable(sql);
                dics.Add("byjggcmj", dic[0]["num"].GetSafeString());

                ret.data = dics;
                ret.code = "0";
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }


        /// <summary>
        /// 获取机构数
        /// </summary>
        /// <returns></returns>
        public ResultParam GetJgs()
        {
            ResultParam ret = new ResultParam();
            try
            {
                IDictionary<string, string> dics = new Dictionary<string, string>();
                IList<IDictionary<string, string>> dic = null;
                //监督工程
                string sql = String.Format("select count(*) as num from View_I_M_QY where lxbh='01' or exists (select * from i_s_qy_qyzz where i_s_qy_qyzz.qybh=View_I_M_QY.qybh and i_s_qy_qyzz.qylxbh='01')");
                dic = CommonDao.GetDataTable(sql);
                dics.Add("jgs", dic[0]["num"].GetSafeString());

                ret.data = dics;
                ret.code = "0";
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取工程数
        /// </summary>
        /// <returns></returns>
        public ResultParam GetGcs()
        {
            ResultParam ret = new ResultParam();
            try
            {
                IDictionary<string, string> dics = new Dictionary<string, string>();
                IList<IDictionary<string, string>> dic = null;
                //监督工程
                string sql = String.Format("select count(*) as num from i_m_gc where (isnull(sjgcbh,'') = '') or (isnull(sjgcbh,'') <> '' and isnull(sjgcbh,'')='')");
                dic = CommonDao.GetDataTable(sql);
                dics.Add("gcs", dic[0]["num"].GetSafeString());

                ret.data = dics;
                ret.code = "0";
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 工程信息
        /// </summary>
        /// <returns></returns>
        public ResultParam GetGcInfo()
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql =
                    "select  gcbh,gcmc,gczb,zjdjh,sy_jsdwmc, sgdwmc, '' as jldwmc,'' as kcdwmc,'' as sjdwmc,'' as xxjk, '' as sy_gczt  from View_I_M_GC a where (isnull(sjgcbh,'') = '') or (isnull(sjgcbh,'') <> '' and isnull(sjgcbh,'')='') order by gcmc";
                IList<IDictionary<string, string>> dic = CommonDao.GetDataTable(sql);
                ret.data = dic;
                ret.code = "0";
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取检测机构数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ResultParam GetScreenJcjgData(string queryJson, int pageSize, int pageIndex)
        {
            ResultParam ret = new ResultParam();
            Dictionary<string, object> data = new Dictionary<string, object>();
            string msg = string.Empty;
            try
            {
                var queryDict = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(queryJson))
                     queryDict = JsonSerializer.Deserialize<Dictionary<string, string>>(queryJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                string where = " where 1=1 ";

                var endTime = DictionaryHelper.GetValue(queryDict, "endTime");

                if (!string.IsNullOrEmpty(endTime))
                    endTime += " 11:59:59";

                where += MakeSqlHelper.PackageQuerySql("a.wtslrsj", ">", DictionaryHelper.GetValue(queryDict, "startTime"));
                where += MakeSqlHelper.PackageQuerySql("a.wtslrsj", "<", endTime);

                string sql = @"select qybh, qymc jcjgmc, (select count(1) from i_m_gc where ssjcjgbh = a.qybh) gcnum from View_I_M_JCJG a";
                int totalCount = 0;

                var records = CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);

                sql = string.Format(@"select a.ytdwbh qybh, count(1) wtdnum from m_by a {0} group by a.ytdwbh", where);
                var wtdDt = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select a.ytdwbh qybh, count(distinct a.gcbh) cjnum from m_by a inner join up_cjjl b on a.recid = b.wtdbh and b.sfjs = 0 {0} group by a.ytdwbh", where);
                var cjDt = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select a.ytdwbh qybh, count(1) reportnum from m_by a inner join up_bgsj b on a.recid = b.wtdbh {0} group by a.ytdwbh", where);
                var reportDt = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select a.ytdwbh qybh, count(1) ycreportnum from m_by a {0} and a.yczt > 0 group by a.ytdwbh", where);
                var ycReportDt = CommonDao.GetDataTable(sql);

                sql = string.Format(@"select a.ytdwbh qybh, count(1) ycreportnum from m_by a {0} and a.yczt > 0  and not exists(select * from up_bgyc where bgwyh = a.recid and content > '') group by a.ytdwbh", where);
                var yclReportDt = CommonDao.GetDataTable(sql);

                foreach (var record in records)
                {
                    var qybh = record["qybh"];
                    var tempWtd = wtdDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempWtd != null)
                        record.Add("wtdnum", tempWtd["wtdnum"]);
                    else
                        record.Add("wtdnum", "0");

                    var tempCj = cjDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempCj != null)
                        record.Add("cjnum", tempCj["cjnum"]);
                    else
                        record.Add("cjnum", "0");

                    var tempReport = reportDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempReport != null)
                        record.Add("reportnum", tempReport["reportnum"]);
                    else
                        record.Add("reportnum", "0");

                    var tempYcReport = ycReportDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempYcReport != null)
                        record.Add("ycreportnum", tempYcReport["ycreportnum"]);
                    else
                        record.Add("ycreportnum", "0");

                    var yclreportnum = 0;
                    record.Add("yclreportnum", yclreportnum.GetSafeString());
                }

                data.Add("records", records);
                data.Add("totalcount", totalCount);
                data.Add("totalpage", (int)Math.Ceiling((double)totalCount / (double)pageSize));
                data.Add("pagesize", pageSize);
                data.Add("pageindex", pageIndex);

                ret.success = true;
                ret.data = data;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }

            return ret;
        }

        #endregion

        #region 是否存在form配置
        public ResultParam ExistFormDm(string formDm, string formStatus)
        {
            ResultParam ret = new ResultParam();

            try
            {
                string sql = string.Format("select count(1) as num from form where formDm = '{0}' and formStatus = '{1}'", formDm, formStatus);
                int num = CommonDao.GetSingleData(sql).GetSafeInt();

                ret.success = true;
                ret.data = num > 1 ? true : false;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret;
        } 
        #endregion

        #region 萧山手动上传报告(协会二维码)
        /// <summary>
        /// 获取需要手动上传报告填写的字段
        /// </summary>
        /// <param name="syxmbh">试验项目编号</param>
        /// <param name="type">1-手动填写的字段  2-委托单中获取的字段</param>
        /// <returns></returns>
        public ResultParam XsxhUploadBgField(string wtdwyh, string syxmbh, int type)
        {
            ResultParam ret = new ResultParam();
            //对象返回项
            IDictionary<string, object> data = new Dictionary<string, object>();

            if (GlobalVariableConfig.GLOBAL_SERVICE_GENERATE == ServiceEnum.XSXH.ToString())
            {
                data.Add("show", "1");
                ret.data = data;
            }
            else
            {
                data.Add("show", "0");
                ret.data = data;
                ret.success = true;
                return ret;
            }

            //判断是两块两材还是非两块两材
            string sql = String.Format("select lx from SysYcjk_Xsxh_SyxmbhCode where SYXMBH = '{0}'", syxmbh);
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            //判断试验项目配置情况
            if (dt.Count == 0)
            {
                ret.msg = String.Format("试验项目【{0}】未配置！", syxmbh);
                return ret;
            }
            //判断试验项目字段是否存在
            sql = String.Format("select count(*) as num from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}'", syxmbh);
            IList<IDictionary<string, string>> dtField = CommonDao.GetDataTable(sql);
            if (dtField[0]["num"].GetSafeInt() == 0)
            {
                ret.msg = String.Format("试验项目字段【{0}】未配置！", syxmbh);
                return ret;
            }
            //获取试验类型是两块还是非两块
            string lx = dt[0]["lx"].GetSafeString();
            string where = string.Empty;

            if (type == 1)
            {
                where = " and (JGZDMC = '' or JGZDMC is null) ";
            }
            else if (type == 2)
            {
                where = " and JGZDMC > '' ";
            }

            //非两块两材
            if (lx == "0")
            {
                sql = String.Format("select jkzdm,sy,type,jgzdmc,jgsjbmc from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' {1} order by ORDERNO", syxmbh, where);
                //公共信息
                data.Add("basic", CommonDao.GetDataTable(sql));
            }
            //两块两材
            else if (lx == "1")
            {
                //公共信息
                sql = String.Format("select jkzdm,sy,type,jgzdmc,jgsjbmc from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' and SIGN = '1' {1} order by ORDERNO", syxmbh, where);
                data.Add("basic", CommonDao.GetDataTable(sql));
                //样品信息
                sql = String.Format("select jkzdm,sy,type,jgzdmc,jgsjbmc from SysYcjk_Xsxh_UploadField where SYXMBH = '{0}' and SIGN = '2' {1} order by ORDERNO", syxmbh, where);
                data.Add("sample", CommonDao.GetDataTable(sql));
            }
            else
            {
                ret.msg = String.Format("试验项目【{0}】两块类型未配置！", syxmbh);
                return ret;
            }

            //组数
            sql = string.Format("select count(1) from s_by where BYZBRECID = '{0}'", wtdwyh);
            data.Add("zhs", CommonDao.GetSingleData(sql).GetSafeInt());

            //类型
            data.Add("lx", lx);

            //返回结果
            ret.data = data;
            ret.success = true;
            return ret;
        }

        /// <summary>
        /// 获取手动上传报告的数据(从委托单中获取)
        /// </summary>
        /// <param name="wtdwyh"></param>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        public ResultParam XsxhUploadBgData(string wtdwyh, string dataJson)
        {
            ResultParam ret = new ResultParam();
            string msg = string.Empty;

            try
            {
                if (GlobalVariableConfig.GLOBAL_SERVICE_GENERATE != ServiceEnum.XSXH.ToString())
                {
                    ret.success = true;
                    ret.data = "";
                    return ret;
                }

                string sql = string.Format("select * from m_by where recid = '{0}'", wtdwyh);
                var mdt = CommonDao.GetDataTable(sql);

                if (mdt.Count() == 0)
                {
                    ret.msg = "该委托单不存在";
                    return ret;
                }

                var mdict = mdt[0];
                var syxmbh = mdict["syxmbh"];
                var sTableName = "S_" + syxmbh.ToUpper();

                sql = string.Format("select a.* from s_by a where a.byzbrecid = '{0}' order by len(a.zh),a.zh", wtdwyh);
                var sdt = CommonDao.GetDataTable(sql);

                sql = string.Format("select b.* from s_by a inner join {0} b on a.recid = b.recid where a.byzbrecid = '{1}' order by len(a.zh),a.zh", sTableName, wtdwyh);
                var sedt = CommonDao.GetDataTable(sql);

                var result = XsxhUploadBgField(wtdwyh, syxmbh, 2);

                if (!result.success)
                {
                    ret.msg = result.msg;
                    return ret;
                }

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(dataJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var dataDict = result.data as IDictionary<string, object>;

                var lx = dataDict["lx"] as string;
                Dictionary<string, object> dict = new Dictionary<string, object>();

                //非两块两材
                if (lx == "0")
                {
                    var basicJson = data["basic"].GetSafeString();
                    var basic = JsonSerializer.Deserialize<Dictionary<string, string>>(basicJson);

                    var basicDict = dataDict["basic"] as IList<IDictionary<string, string>>;

                    foreach (var item in basicDict)
                    {
                        DictionaryHelper.SetValue(basic, item["jkzdm"], DictionaryHelper.GetValue(mdict, item["jgzdmc"]));
                    }

                    dict.Add("basic", basic);
                }
                //两块两材
                else if (lx == "1")
                {
                    //主表
                    var basicJson = data["basic"].GetSafeString();
                    var basic = JsonSerializer.Deserialize<Dictionary<string, string>>(basicJson);

                    //从表
                    var sampleJson = data["sample"].GetSafeString();
                    var sample = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(sampleJson);

                    var basicDict = dataDict["basic"] as IList<IDictionary<string, string>>;
                    var sampleDict = dataDict["sample"] as IList<IDictionary<string, string>>;

                    foreach (var item in basicDict)
                    {
                        DictionaryHelper.SetValue(basic, item["jkzdm"], DictionaryHelper.GetValue(mdict, item["jgzdmc"]));
                    }

                    for (int i = 0; i < sample.Count(); i++)
                    {
                        foreach (var item in sampleDict)
                        {
                            if (item["jgsjbmc"].ToUpper() == "S_BY")
                                DictionaryHelper.SetValue(sample[i], item["jkzdm"], DictionaryHelper.GetValue(sdt[i], item["jgzdmc"]));
                            else
                                DictionaryHelper.SetValue(sample[i], item["jkzdm"], DictionaryHelper.GetValue(sedt[i], item["jgzdmc"]));
                        }
                    }

                    dict.Add("basic", basic);
                    dict.Add("sample", sample);
                }

                ret.success = true;
                ret.data = JsonSerializer.Serialize(dict);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        } 
        #endregion

        #region 首页数据
        /// <summary>
        /// 首页工程信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetHomePageGcInfo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                //sum(cast(jzmj as decimal))
                string sql = "select count(1) gcnum,0 gcmj, 0 gczj from i_m_gc where isnull(ssjcjgbh,'') = '' and isnull(sjgcbh, '') = ''";
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                    dict = dt[0];
                else
                {
                    dict.Add("gcnum", "0");
                    dict.Add("gcmj", "0");
                    dict.Add("gczj", "0");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        /// <summary>
        /// 首页委托单信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetHomePageWtdInfo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                string sql = "select count(1) wtdnum, sum(CASE WHEN ZT NOT LIKE 'W___0_____' AND ZT NOT LIKE 'W___1_____' AND ZT NOT LIKE 'W___2_____' AND ZT NOT LIKE 'W___3_____' THEN 1 ELSE 0 End) slwtdnum from m_by";
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    dict = dt[0];
                    dict.Add("wslwtdnum", (dict["wtdnum"].GetSafeInt() - dict["slwtdnum"].GetSafeInt()).GetSafeString());
                }
                else
                {
                    dict.Add("wtdnum", "0");
                    dict.Add("slwtdnum", "0");
                    dict.Add("wslwtdnum", "0");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        /// <summary>
        /// 首页检测机构信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetHomePageJcjgInfo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                string sql = "select count(1) jcjgnum, sum(Case when sfbdqy = 1 Then 1 Else 0 End) bdjcjgnum from View_I_M_JCJG";
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    dict = dt[0];
                    dict.Add("wdjcjgnum", (dict["jcjgnum"].GetSafeInt() - dict["bdjcjgnum"].GetSafeInt()).GetSafeString());
                }
                else
                {
                    dict.Add("jcjgnum", "0");
                    dict.Add("bdjcjgnum", "0");
                    dict.Add("wdjcjgnum", "0");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        /// <summary>
        /// 首页报告信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetHomePageReportInfo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                string sql = "select count(1) bgnum, sum(case when JCJGMS = '合格' Then 1 Else 0 End) hgbgnum from up_bgsj";
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    dict = dt[0];
                    dict.Add("bhgbgnum", (dict["bgnum"].GetSafeInt() - dict["hgbgnum"].GetSafeInt()).GetSafeString());
                }
                else
                {
                    dict.Add("bgnum", "0");
                    dict.Add("hgbgnum", "0");
                    dict.Add("bhgbgnum", "0");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        /// <summary>
        /// 首页采集数据
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetHomePageCjInfo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                string sql = "select Sum(Case When SJZT & 2 > 0 Then 1 Else 0 End) cjqxnum, Sum(Case When SJZT & 4 > 0 and SPYSC = 1 Then 1 Else 0 End) cjspnum, Sum(Case When SJZT & 256 > 0 Then 1 Else 0 End) xczplnum from m_by ";
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    dict = dt[0];
                }
                else
                {
                    dict.Add("cjqxnum", "0");
                    dict.Add("cjspnum", "0");
                    dict.Add("xczplnum", "0");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        /// <summary>
        /// 首页异常报告信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetHomePageYcReportInfo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                string sql = "select count(1) from m_by where yczt > 0";
                var ycnum = CommonDao.GetSingleData(sql).GetSafeInt();

                sql = "select count(distinct bgwyh) from up_bgyc where content > ''";
                var yclycnum = CommonDao.GetSingleData(sql).GetSafeInt();

                dict.Add("ycnum", ycnum.GetSafeString());
                dict.Add("yclycnum", yclycnum.GetSafeString());
                dict.Add("wclycnum", (ycnum - yclycnum).GetSafeString());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetHomePageDbsxInfo(string userCode)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            try
            {
                string sql = string.Format(@"select count(1) num, IsNull(Max(DateCreated), '') createdate
                                               from STToDoTasks a inner join STForm b on a.SerialNo = b.SerialNo
                                              where b.ProcessID = 7
                                                and a.userid = '{0}'", userCode);

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    var date = dt[0]["createdate"].GetSafeDate().ToString("yyyy-MM-dd");

                    if (date == "1900-01-01")
                    {
                        date = "";
                    }

                    //qj - 请假, zz - 资质, ht- 合同
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("sort", "1");
                    dict.Add("lx", "ht");
                    dict.Add("num", dt[0]["num"]);
                    dict.Add("date", date);
                    list.Add(dict);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return list;
        }

        public IList<IDictionary<string, string>> GetHomePageGcData(int type)
        {
            IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();

            try
            {
                string sql = @"select gcbh, gcmc, gczb,
                                (Case When (select count(1) from m_by a inner join up_cjjl b on a.recid = b.wtdbh and b.sfjs = 0 and a.gcbh = i_m_gc.gcbh) > 0 Then 2
	                            When (select a.gcbh from m_by a inner join i_s_xcjgjc b on a.recid = b.wtdwyh and b.jcrq > getdate() and a.gcbh = i_m_gc.gcbh) > 0 Then 1
	                            Else 0 End) type
                                from i_m_gc where gczb > '' and LTRIM(RTRIM(gczb)) <> ','";

                //计划现场检测工程
                if (type == 1)
                {
                    sql += " and gcbh in (select a.gcbh from m_by a inner join i_s_xcjgjc b on a.recid = b.wtdwyh and b.jcrq > getdate())";
                }
                //正在现场检测工程
                else if (type == 2)
                {
                    sql += " and gcbh in (select a.gcbh from m_by a inner join up_cjjl b on a.recid = b.wtdbh and b.sfjs = 0)";
                }

                list = CommonDao.GetDataTable(sql);

                sql = @"select a.gcbh, Cast(b.Longitude as nvarchar(50)) + ',' + Cast(b.Latitude as nvarchar(50)) gczb
                          from m_by a inner join up_cjjl b on a.recid = b.WTDBH and b.SFJS = 0";

                var dt = CommonDao.GetDataTable(sql);

                foreach (var item in dt)
                {
                    var listDt = list.FirstOrDefault(x => x["gcbh"] == item["gcbh"]);

                    if (listDt != null)
                    {
                        if (string.IsNullOrEmpty(listDt["gczb"]) || listDt["gczb"].Trim() == ",")
                            listDt["gczb"] = item["gczb"];
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return list;
        }

        public IDictionary<string, string> GetHomePageGcDetailData(string gcbh)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                string sql = string.Format(@"select gcbh,gcmc,zjzmc,jsdwmc,sgdwmc,jldwmc,'' sjdwmc,'' kcdwmc, '' jsry, '' jsdh, '' sgry, '' sgdh, gcdd
                                               from view_i_m_gc where gcbh = '{0}'", gcbh);

                var dt = CommonDao.GetDataTable(sql);

                sql = string.Format("select qymc from i_s_gc_sjdw where gcbh = '{0}'", gcbh);
                var sjdwDt = CommonDao.GetDataTable(sql);

                sql = string.Format("select qymc from i_s_gc_kcdw where gcbh = '{0}'", gcbh);
                var kcdwDt = CommonDao.GetDataTable(sql);

                sql = string.Format("select ryxm, dh from i_s_gc_jsry where gcbh = '{0}'", gcbh);
                var jsryDt = CommonDao.GetDataTable(sql);

                sql = string.Format("select ryxm, dh from i_s_gc_sgry where gcbh = '{0}'", gcbh);
                var sgryDt = CommonDao.GetDataTable(sql);

                if (dt.Count() > 0)
                {
                    dict = dt[0];
                    var sjdwmc = string.Empty;
                    var kcdwmc = string.Empty;
                    var jsry = string.Empty;
                    var jsdh = string.Empty;
                    var sgry = string.Empty;
                    var sgdh = string.Empty;

                    foreach (var item in sjdwDt)
                    {
                        sjdwmc += item["qymc"] + ",";
                    }

                    foreach (var item in kcdwDt)
                    {
                        kcdwmc += item["qymc"] + ",";
                    }

                    foreach(var item in jsryDt)
                    {
                        jsry += item["ryxm"] + ",";
                        jsdh += item["dh"] + ",";
                    }

                    foreach (var item in sgryDt)
                    {
                        sgry += item["ryxm"] + ",";
                        sgdh += item["dh"] + ",";
                    }

                    dict["sjdwmc"] = sjdwmc.TrimEnd(',');
                    dict["kcdwmc"] = kcdwmc.TrimEnd(',');
                    dict["jsry"] = jsry.TrimEnd(',');
                    dict["jsdh"] = jsdh.TrimEnd(',');
                    dict["sgry"] = sgry.TrimEnd(',');
                    dict["sgdh"] = sgdh.TrimEnd(',');
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        }

        public Dictionary<string, object> GetHomePageJcjgData(int pageIndex, int pageSize)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            try
            {
                string sql = @"select qybh, qymc jcjgmc, (select count(1) from i_m_gc where ssjcjgbh = a.qybh) gcnum
                                    from View_I_M_JCJG a";

                int totalCount = 0;
                var records = CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);

                sql = @"select ytdwbh qybh, count(1) wtdnum from m_by group by ytdwbh";
                var wtdDt = CommonDao.GetDataTable(sql);

                sql = @"select a.ytdwbh qybh, count(1) bgnum, sum(case when b.jcjgms = '合格' Then 1 Else 0 End) hgbgnum from m_by a inner join up_bgsj b on a.recid = b.wtdbh group by a.ytdwbh";
                var bgDt = CommonDao.GetDataTable(sql);

                sql = @"select ytdwbh qybh, count(1) ycnum from m_by where yczt > 0 group by ytdwbh";
                var ycDt = CommonDao.GetDataTable(sql);

                sql = @"select sydwbh qybh, count(1) yclycnum from up_bgyc where content > '' group by sydwbh ";
                var yclycDt = CommonDao.GetDataTable(sql);

                sql = @"select ytdwbh qybh, Sum(Case When SJZT & 2 > 0 Then 1 Else 0 End) cjqxnum, Sum(Case When SJZT & 4 > 0 and SPYSC = 1 Then 1 Else 0 End) cjspnum, Sum(Case When SJZT & 32 > 0 Then 1 Else 0 End) xctpnum from m_by group by ytdwbh ";
                var cjDt = CommonDao.GetDataTable(sql);

                foreach (var record in records)
                {
                    var qybh = record["qybh"];
                    var tempWtd = wtdDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempWtd != null)
                        record.Add("wtdnum", tempWtd["wtdnum"]);
                    else
                        record.Add("wtdnum", "0");

                    var tempBg = bgDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempBg != null)
                    {
                        record.Add("bgnum", tempBg["bgnum"]);
                        record.Add("hgbgnum", tempBg["hgbgnum"]);
                        record.Add("bhgbgnum", (tempBg["bgnum"].GetSafeInt() - tempBg["hgbgnum"].GetSafeInt()).GetSafeString());
                    }
                    else
                    {
                        record.Add("bgnum", "0");
                        record.Add("hgbgnum", "0");
                        record.Add("bhgbgnum", "0");
                    }

                    var tempYc = ycDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempYc != null)
                        record.Add("ycnum", tempYc["ycnum"]);
                    else
                        record.Add("ycnum", "0");

                    var tempYclyc = yclycDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempYclyc != null)
                        record.Add("yclycnum", tempYclyc["yclycnum"]);
                    else
                        record.Add("yclycnum", "0");

                    record.Add("wclycnum", (record["ycnum"].GetSafeInt() - record["yclycnum"].GetSafeInt()).GetSafeString());

                    var tempCj = cjDt.FirstOrDefault(x => x["qybh"] == qybh);

                    if (tempCj != null)
                    {
                        record.Add("cjqxnum", tempCj["cjqxnum"]);
                        record.Add("cjspnum", tempCj["cjspnum"]);
                        record.Add("xctpnum", tempCj["xctpnum"]);
                    }
                    else
                    {
                        record.Add("cjqxnum", "0");
                        record.Add("cjspnum", "0");
                        record.Add("xctpnum", "0");
                    }
                }

                dict.Add("records", records);
                dict.Add("totalcount", totalCount);
                dict.Add("totalpage", (int)Math.Ceiling((double)totalCount / (double)pageSize));
                dict.Add("pagesize", pageSize);
                dict.Add("pageindex", pageIndex);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return dict;
        } 
        #endregion

        #region 企业申报
        /// <summary>
        /// 企业信息
        /// </summary>
        /// <returns></returns>
        public ResultParam GetQyApplyQyInfo(string qybh)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format(@"select qybh,qymc,qyfr,qyfrsj,lxmc qylx,dwwz,zzjgdm,qyfzr,jjxz,lxsj,(zcd1 + zcd2 + zcd3 + zcd4) qydz,lxyx,zzjgzs
                                             from view_i_m_qy where qybh = '{0}'", qybh);

                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该企业不存在";
                    return ret;
                }

                ret.success = true;
                ret.data = dt[0];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 人员信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultParam GetQyApplyRyInfo(string qybh, int pageIndex, int pageSize)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format(@"select rybh, ryxm, sjhm, zc, '' jszw, '' zyzs, '' sgzs
                                               from i_m_ry
                                              where qybh = '{0}'", qybh);

                int totalCount = 0;
                var dt = CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("records", dt);
                data.Add("totalcount", totalCount);
                data.Add("totalpage", (int)Math.Ceiling((double)totalCount / (double)pageSize));
                data.Add("pagesize", pageSize);
                data.Add("pageindex", pageIndex);

                ret.success = true;
                ret.data = data;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultParam GetQyApplySbInfo(string qybh, int pageIndex, int pageSize)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format(@"select recid sbid, sbbh, sbmc, sbxh, sccj, qyrq, bdzq, xcbdrq, '0' zt, '' jdzs, bfnx
                                               from i_m_sb
                                              where ssdwbh = '{0}'", qybh);

                int totalCount = 0;
                var dt = CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("records", dt);
                data.Add("totalcount", totalCount);
                data.Add("totalpage", (int)Math.Ceiling((double)totalCount / (double)pageSize));
                data.Add("pagesize", pageSize);
                data.Add("pageindex", pageIndex);

                ret.success = true;
                ret.data = data;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }

        public ResultParam GetQyApplyModifyInfo(string recid)
        {
            ResultParam ret = new ResultParam();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();

                //申报内容
                string sql = string.Format("select * from I_M_QyApply where recid = '{0}'", recid);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该申报不存在";
                    return ret;
                }

                var status = dt[0]["zt"];

                if (IsQyApplyAudit(status))
                {
                    ret.msg = "该申报已经审核,无法操作";
                    return ret;
                }

                foreach (var item in dt[0])
                {
                    data.Add(item.Key, item.Value);
                }

                //申报人员
                sql = string.Format("select * from I_S_QyApply_Ry where applyid = '{0}'", recid);
                var ryDt = CommonDao.GetDataTable(sql);

                data.Add("rys", ryDt);

                //申报设备
                sql = string.Format("select * from I_S_QyApply_Sb where applyid = '{0}'", recid);
                var sbDt = CommonDao.GetDataTable(sql);

                data.Add("sbs", sbDt);

                //申报资质
                sql = string.Format("select * from I_S_QyApply_Zz where applyid = '{0}'", recid);
                var zzDt = CommonDao.GetDataTable(sql);

                data.Add("zzs", zzDt);

                //申报承诺书
                sql = string.Format("select * from I_S_QyApply_Cns where applyid = '{0}'", recid);
                var cnsDt = CommonDao.GetDataTable(sql);

                data.Add("cns", cnsDt);

                ret.success = true;
                ret.data = data;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public ResultParam UpdateQyApply(string qyJson, string ryJson, string sbJson, string zzJson, string cnsJson, string saveType,
            string sqr, string sqrxm)
        {
            ResultParam ret = new ResultParam();
            string msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(qyJson) || string.IsNullOrEmpty(ryJson) || string.IsNullOrEmpty(sbJson)
                || string.IsNullOrEmpty(zzJson) || string.IsNullOrEmpty(cnsJson))
                {
                    ret.msg = "传入参数不能为空";
                    return ret;
                }

                var qyDict = JsonSerializer.Deserialize<Dictionary<string, string>>(qyJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var ryList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(ryJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var sbList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(sbJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var zzList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(zzJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                var cnsList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(cnsJson, out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    ret.msg = msg;
                    return ret;
                }

                string sql = string.Empty;
                var recid = DictionaryHelper.GetValue(qyDict, "recid");

                //新增
                if (string.IsNullOrEmpty(recid))
                {
                    recid = Guid.NewGuid().ToString();
                    DictionaryHelper.SetValue(qyDict, "recid", recid);
                    DictionaryHelper.SetValue(qyDict, "zt", saveType);
                    DictionaryHelper.SetValue(qyDict, "sqr", sqr);
                    DictionaryHelper.SetValue(qyDict, "sqrxm", sqrxm);
                    DictionaryHelper.SetValue(qyDict, "sqsj", DateTime.Now.ToString());

                    //企业信息
                    sql = MakeSqlHelper.InsertSql("I_M_QyApply", qyDict);
                    CommonDao.ExecCommand(sql);
                }
                //修改
                else
                {
                    sql = string.Format("select * from I_M_QyApply where recid = '{0}'", recid);
                    var dt = CommonDao.GetDataTable(sql);

                    if (dt.Count() == 0)
                    {
                        ret.msg = "该申报不存在，无法修改";
                        return ret;
                    }

                    var status = dt[0]["zt"];

                    if (IsQyApplyAudit(status))
                    {
                        ret.msg = "该申报已审核，无法修改";
                        return ret;
                    }

                    DictionaryHelper.SetValue(qyDict, "zt", saveType);

                    //企业信息
                    sql = MakeSqlHelper.UpdateSql("I_M_QyApply", qyDict, "where recid = '" + recid + "'");
                    CommonDao.ExecCommand(sql);

                    string where = string.Format(" where applyid = '{0}' ", recid);
                    //删除人员信息
                    sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Ry", where);
                    CommonDao.ExecCommand(sql);

                    //删除设备信息
                    sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Sb", where);
                    CommonDao.ExecCommand(sql);

                    //删除资质信息
                    sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Zz", where);
                    CommonDao.ExecCommand(sql);

                    //删除承诺书
                    sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Cns", where);
                    CommonDao.ExecCommand(sql);
                }

                //人员信息
                foreach (var ryDict in ryList)
                {
                    DictionaryHelper.SetValue(ryDict, "applyid", recid);
                    sql = MakeSqlHelper.InsertSql("I_S_QyApply_Ry", ryDict);
                    CommonDao.ExecCommand(sql);
                }

                //设备信息
                foreach (var sbDict in sbList)
                {
                    DictionaryHelper.SetValue(sbDict, "applyid", recid);
                    sql = MakeSqlHelper.InsertSql("I_S_QyApply_Sb", sbDict);
                    CommonDao.ExecCommand(sql);
                }

                //资质信息
                foreach (var zzDict in zzList)
                {
                    DictionaryHelper.SetValue(zzDict, "applyid", recid);
                    sql = MakeSqlHelper.InsertSql("I_S_QyApply_Zz", zzDict);
                    CommonDao.ExecCommand(sql);
                }

                //承诺书
                foreach (var cnsDict in cnsList)
                {
                    DictionaryHelper.SetValue(cnsDict, "applyid", recid);
                    sql = MakeSqlHelper.InsertSql("I_S_QyApply_Cns", cnsDict);
                    CommonDao.ExecCommand(sql);
                }

                ret.success = true;
                ret.data = recid;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public ResultParam AuditQyApply(string recid, string zt, string shr, string shrxm, string shsm)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select * from I_M_QyApply where recid = '{0}'", recid);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该申报记录不存在";
                    return ret;
                }

                var status = dt[0]["zt"];

                if (status == "0")
                {
                    ret.msg = "该申报还未报备";
                    return ret;
                }

                if (IsQyApplyAudit(status))
                {
                    ret.msg = "该申报记录已经被审核";
                    return ret;
                }

                sql = string.Format("update I_M_QyApply set shr = '{0}', shrxm = '{1}', shsj = getdate(), zt ='{2}', shsm = '{3}' where recid = '{4}'", shr, shrxm, zt, shsm, recid);
                CommonDao.ExecCommand(sql);

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public ResultParam DelQyApply(string recid)
        {
            ResultParam ret = new ResultParam();
            try
            {
                string sql = string.Format("select * from I_M_QyApply where recid = '{0}'", recid);
                var dt = CommonDao.GetDataTable(sql);

                if (dt.Count() == 0)
                {
                    ret.msg = "该申报记录不存在,无法删除";
                    return ret;
                }

                var status = dt[0]["zt"];

                if (IsQyApplyAudit(status))
                {
                    ret.msg = "该申报记录已经被审核,无法删除";
                    return ret;
                }

                sql = string.Format("delete I_M_QyApply where recid = '{0}'", recid);
                CommonDao.ExecCommand(sql);

                string where = string.Format(" where applyid = '{0}' ", recid);
                //删除人员信息
                sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Ry", where);
                CommonDao.ExecCommand(sql);

                //删除设备信息
                sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Sb", where);
                CommonDao.ExecCommand(sql);

                //删除资质信息
                sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Zz", where);
                CommonDao.ExecCommand(sql);

                //删除承诺书
                sql = MakeSqlHelper.DeleteSql("I_S_QyApply_Cns", where);
                CommonDao.ExecCommand(sql);

                ret.success = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                ret.msg = ex.Message;
                throw ex;
            }
            return ret;
        }

        private bool IsQyApplyAudit(string status)
        {
            if (status == "10" || status == "20")
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
