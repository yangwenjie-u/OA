using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.JC.JS.Common.Entities;
using BD.JC.JS.Common;

namespace BD.JC.JS.Common.Controllers
{
    /// <summary>
    /// 主从表操作类
    /// </summary>
    public class ControllerMstable
    {
        /// <summary>
        /// 获取必有主从信息
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public static EntityMstable GetByxx(string recid, out string msg)
        {
            EntityMstable ret = null;
            msg = "";
            try
            {
                // 必有主表
                string sql = "select * from m_by where recid='" + recid + "'";
                IList<IDictionary<string, string>> mtable = SqlHelper.GetDataTable(sql, out msg);
                if (msg != "")
                    return ret;
                if (mtable.Count == 0)
                    return ret;
                // 必有从表
                sql = "select * from s_by where byzbrecid='" + recid + "'";               
                IList<IDictionary<string, string>> stable = SqlHelper.GetDataTable(sql, out msg);
                if (msg != "")
                    return ret;

                ret = new EntityMstable();
                ret.Load(mtable[0], stable);
                // 单位主表
                IList<IDictionary<string, string>> mdwtable = SqlHelper.GetDataTable("select * from m_d_" + ret.Syxmbh + " where recid='" + recid + "'", out msg);
                if (msg != "" || mdwtable.Count == 0)
                {
                    ret = null;
                    return ret;
                }
                // 单位从表
                IList<IDictionary<string, string>> sdwtable = SqlHelper.GetDataTable("select * from s_d_" + ret.Syxmbh + " where byzbrecid='" + recid + "'", out msg);
                if (msg != "" )
                {
                    ret = null;
                    return ret;
                }
                // 项目主表
                IList<IDictionary<string, string>> mxmtable = SqlHelper.GetDataTable("select * from m_" + ret.Syxmbh + " where recid='" + recid + "'", out msg);
                if (msg != "" || mxmtable.Count == 0)
                {
                    ret = null;
                    return ret;
                }
                // 项目从表
                IList<IDictionary<string, string>> sxmtable = SqlHelper.GetDataTable("select * from s_" + ret.Syxmbh + " where byzbrecid='" + recid + "'", out msg);
                if (msg != "")
                {
                    ret = null;
                    return ret;
                }

                ret.Load(mdwtable[0], mxmtable[0], sdwtable, sxmtable);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ret = null;
            }
            return ret;
        }

        public static bool SetNullToLine(EntityMstable mstable, CollectionZdzd zdzds, string dest, IList<KeyValuePair<string, string>> execludes, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<string> sqls = new List<string>();
                // 必有主表
                IList<EntityZdzd> zdzdmby = GetMNullFields(zdzds, 1, mstable, execludes);
                // 项目主表
                IList<EntityZdzd> zdzdmxm = GetMNullFields(zdzds, 2, mstable, execludes);
                // 单位主表
                IList<EntityZdzd> zdzdmdw = GetMNullFields(zdzds, 3, mstable, execludes);
                // 必有主表
                IDictionary<string, IList<EntityZdzd>> zdzdsby = GetSNullFields(zdzds, 1, mstable, execludes);
                // 项目从表
                IDictionary<string, IList<EntityZdzd>> zdzdsxm = GetSNullFields(zdzds, 2, mstable, execludes);
                // 单位
                IDictionary<string, IList<EntityZdzd>> zdzdsdw = GetSNullFields(zdzds, 3, mstable, execludes);

                // 获取sql
                string sql = GetNullUpdateSql(zdzdmby, dest, mstable.MBY, "recid='" + mstable.Wtdwyh + "'");
                if (sql.Length > 0)
                    sqls.Add(sql);
                sql = GetNullUpdateSql(zdzdmxm, dest, mstable.MXM, "recid='" + mstable.Wtdwyh + "'");
                if (sql.Length > 0)
                    sqls.Add(sql);
                sql = GetNullUpdateSql(zdzdmdw, dest, mstable.MDW, "recid='" + mstable.Wtdwyh + "'");
                if (sql.Length > 0)
                    sqls.Add(sql);
                foreach (string key in zdzdsby.Keys)
                {
                    IList<EntityZdzd> nullzdzds = zdzdsby[key];
                    sql = GetNullUpdateSql(nullzdzds, dest, mstable.SBY, "recid='" + key + "'");
                    if (sql.Length > 0)
                        sqls.Add(sql);
                }
                foreach (string key in zdzdsxm.Keys)
                {
                    IList<EntityZdzd> nullzdzds = zdzdsxm[key];
                    sql = GetNullUpdateSql(nullzdzds, dest, mstable.SXM, "recid='" + key + "'");
                    if (sql.Length > 0)
                        sqls.Add(sql);
                }
                foreach (string key in zdzdsdw.Keys)
                {
                    IList<EntityZdzd> nullzdzds = zdzdsdw[key];
                    sql = GetNullUpdateSql(nullzdzds, dest, mstable.SDW, "recid='" + key + "'");
                    if (sql.Length > 0)
                        sqls.Add(sql);
                }

                SqlHelper.ExecTrans(sqls, out msg);
                
                ret = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 获取某类主表委托时显示的的空字段
        /// </summary>
        /// <param name="zdzds"></param>
        /// <param name="blx">1-必有,2-项目,3-单位</param>
        /// <param name="mstable"></param>
        /// <returns></returns>
        protected static IList<EntityZdzd> GetMNullFields(CollectionZdzd zdzds, int blx, EntityMstable mstable, IList<KeyValuePair<string, string>> execludes)
        {
            IList<EntityZdzd> ret = new List<EntityZdzd>();
            try
            {
                // 获取zdzd
                string tablename = "";
                IDictionary<string, string> rowData = null;
                if (blx == 1)
                {
                    tablename = mstable.MBY;
                    rowData = mstable.MBydata;
                }
                else if (blx == 2)
                {
                    tablename = mstable.MXM;
                    rowData = mstable.MXmdata;
                }
                else
                {
                    tablename = mstable.MDW;
                    rowData = mstable.MDwdata;
                }

                foreach (EntityZdzd zdzd in zdzds.Zdzds)
                {
                    var q = from e in execludes where e.Key.Equals(zdzd.SJBMC, StringComparison.OrdinalIgnoreCase) && e.Value.Equals(zdzd.ZDMC, StringComparison.OrdinalIgnoreCase) select e;
                    if (q.Count() > 0)
                        continue;
                    // 必有主表的空字符串字段
                    if (zdzd.SJBMC.Equals(tablename, StringComparison.OrdinalIgnoreCase) && 
                        zdzd.SFXS &&
                        zdzd.LX.Trim().Equals("w", StringComparison.OrdinalIgnoreCase) &&
                        zdzd.IsStringField && 
                        rowData[zdzd.ZDMC.ToLower()].GetSafeString() == "")
                    {
                        ret.Add(zdzd);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ret;
        }
        /// <summary>
        /// 获取某类从表委托时显示的的空字段，
        /// </summary>
        /// <param name="zdzds"></param>
        /// <param name="blx">1-必有,2-项目,3-单位</param>
        /// <param name="mstable"></param>
        /// <returns></returns>
        protected static IDictionary<string, IList<EntityZdzd>> GetSNullFields(CollectionZdzd zdzds, int blx, EntityMstable mstable, IList<KeyValuePair<string, string>> execludes)
        {
            IDictionary<string, IList<EntityZdzd>> ret = new Dictionary<string, IList<EntityZdzd>>();
            try
            {
                // 获取zdzd
                string tablename = "";
                IList<IDictionary<string, string>> tableData = null;
                if (blx == 1)
                {
                    tablename = mstable.SBY;
                    tableData = mstable.SBydata;
                }
                else if (blx == 2)
                {
                    tablename = mstable.SXM;
                    tableData = mstable.SXmdata;
                }
                else
                {
                    tablename = mstable.SDW;
                    tableData = mstable.SDwdata;
                }

                foreach (IDictionary<string, string> rowData in tableData)
                {
                    string recid = rowData["recid"].GetSafeString();
                    IList<EntityZdzd> nullzdzds = new List<EntityZdzd>();
                    foreach (EntityZdzd zdzd in zdzds.Zdzds)
                    {
                        var q = from e in execludes where e.Key.Equals(zdzd.SJBMC, StringComparison.OrdinalIgnoreCase) && e.Value.Equals(zdzd.ZDMC, StringComparison.OrdinalIgnoreCase) select e;
                        if (q.Count() > 0)
                            continue;
                        if (zdzd.SJBMC.Equals(tablename, StringComparison.OrdinalIgnoreCase) &&
                            zdzd.SFXS &&
                            zdzd.LX.Trim().Equals("w", StringComparison.OrdinalIgnoreCase) &&
                            zdzd.IsStringField && 
                            rowData[zdzd.ZDMC.ToLower()].GetSafeString() == "")
                        {
                            nullzdzds.Add(zdzd);
                        }
                    }
                    if (nullzdzds.Count > 0)
                        ret.Add(recid, nullzdzds);
                }
            }
            catch (Exception ex)
            {

            }
            return ret;
        }
        /// <summary>
        /// 根据空值的zdzd，获取sql语句
        /// </summary>
        /// <param name="nullzdzds"></param>
        /// <param name="strDest"></param>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected static string GetNullUpdateSql(IList<EntityZdzd> nullzdzds, string strDest, string tablename, string where)
        {
            string ret = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (EntityZdzd zdzd in nullzdzds)
                {
                    if (sb.Length > 0)
                        sb.Append(",");
                    sb.Append(zdzd.ZDMC + "='" + strDest + "'");
                }
                if (sb.Length == 0)
                    return ret;
                ret = "update " + tablename + " set " + sb + " where " + where;
            }
            catch (Exception ex)
            {
                ret = "";
            }
            return ret;
        }
    }
}
