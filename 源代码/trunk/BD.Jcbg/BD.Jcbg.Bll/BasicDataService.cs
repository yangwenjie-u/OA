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

namespace BD.Jcbg.Bll
{
    public class BasicDataService : IBasicDataService
    {
        #region 数据库对象
        public ICommonDao CommonDao { get; set; }
        #endregion

        #region 服务


        #region 统一接口
        /// <summary>
        /// 统一接口， 用来获取满足查询条件的所有数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="where"></param>
        /// <param name="totalcount"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetAllData(BasicDataType type, BasicDataBase where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            try
            {
                switch (type)
                {
                    case BasicDataType.GC:
                        GetGcs(where as VBasicDataGetGc, orderbystr, out totalcount, out records, out msg);
                        break;
                    case BasicDataType.QY:
                        GetQys(where as VBasicDataGetQy, orderbystr, out totalcount, out records, out msg);
                        break;
                    case BasicDataType.RY:
                        GetRys(where as VBasicDataGetRy, orderbystr, out totalcount, out records, out msg);
                        break;
                    case BasicDataType.SB:
                        GetSbs(where as VBasicDataGetSb, orderbystr, out totalcount, out records, out msg);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;

        }

        /// <summary>
        /// 统一接口，用来获取满足查询条件的分页数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalcount"></param>
        /// <param name="records"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetDataList(BasicDataType type, BasicDataBase where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            try
            {
                switch (type)
                {
                    case BasicDataType.GC:
                        ret = GetGcs(where as VBasicDataGetGc, orderbystr, pagesize, pageindex, out totalcount, out records, out msg);
                        break;
                    case BasicDataType.QY:
                        ret = GetQys(where as VBasicDataGetQy, orderbystr, pagesize, pageindex, out totalcount, out records, out msg);
                        break;
                    case BasicDataType.RY:
                        ret = GetRys(where as VBasicDataGetRy, orderbystr, pagesize, pageindex, out totalcount, out records, out msg);
                        break;
                    case BasicDataType.SB:
                        ret = GetSbs(where as VBasicDataGetSb, orderbystr, pagesize, pageindex, out totalcount, out records, out msg);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }
        #endregion


        #region 工程
        public bool GetGcs(VBasicDataGetGc where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            try
            {
                //string sql = "select recid,gcbh,qymc,zjdjh,gcmc,gcdd,gclxmc,djjclxbh,jzfl,jzxz,jgxs,cs,jzmj,rfgcmj,dxsmj,gczj,dwmjzj,sjsynx,ghxkzh,schgsbh,scbasbh,gcjhkgrq,gcjhjgrq,gcjbrq,ckjbrxm,gczt,xxjd,gcjdzt,bz,sy_jsdwmc,jsdwxmfzrxm,jsdwxmfzrsfzhm,jsdwxmfzrsjhm,kcdwmc,kcdwxmfzrxm,kcdwxmfzrsfzhm,kcdwxmfzrsjhm,sjdwmc,sjdwxmfzrxm,sjdwxmfzrsfzhm,sjdwxmfzrsjhm,sgdwmc,sgdwxmfzrxm,sgdwxmfzrsfzhm,sgdwxmfzrsjhm,jldwmc,jldwxmfzrxm,jldwxmfzrsfzhm,jldwxmfzrsjhm,tsdwmc,jdgcsxm,tjjdyxm,azjdyxm,gcszrq,gczwysrq,gcjcfbysrq,gcztfbysrq,gcfhysrq,gcyysrq,gcjgysrq from View_I_M_GC_LB where sptg = 1 ";
                string sql = string.Format("select {0} from View_I_M_GC_LB where sptg = 1 ", GetZdList(BasicDataType.GC));
                StringBuilder sb = new StringBuilder();
                if (where.gcbh.GetSafeRequest() != "")
                {
                    sb.Append(" and gcbh like '%" + where.gcbh.GetSafeRequest() + "%' ");
                }
                if (where.gcmc.GetSafeRequest() != "")
                {
                    sb.Append(" and gcmc like '%" + where.gcmc.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "" )
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryDataTable(sql + sb.ToString());
                totalcount = records.Count;
                AddExtraInfoForGc( ref records);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;

        }

        public bool GetGcs(VBasicDataGetGc where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
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
                //string sql = "select recid,gcbh,qymc,zjdjh,gcmc,gcdd,gclxmc,djjclxbh,jzfl,jzxz,jgxs,cs,jzmj,rfgcmj,dxsmj,gczj,dwmjzj,sjsynx,ghxkzh,schgsbh,scbasbh,gcjhkgrq,gcjhjgrq,gcjbrq,ckjbrxm,gczt,xxjd,gcjdzt,bz,sy_jsdwmc,jsdwxmfzrxm,jsdwxmfzrsfzhm,jsdwxmfzrsjhm,kcdwmc,kcdwxmfzrxm,kcdwxmfzrsfzhm,kcdwxmfzrsjhm,sjdwmc,sjdwxmfzrxm,sjdwxmfzrsfzhm,sjdwxmfzrsjhm,sgdwmc,sgdwxmfzrxm,sgdwxmfzrsfzhm,sgdwxmfzrsjhm,jldwmc,jldwxmfzrxm,jldwxmfzrsfzhm,jldwxmfzrsjhm,tsdwmc,jdgcsxm,tjjdyxm,azjdyxm,gcszrq,gczwysrq,gcjcfbysrq,gcztfbysrq,gcfhysrq,gcyysrq,gcjgysrq from View_I_M_GC_LB where sptg = 1 ";
                string sql = string.Format("select {0} from View_I_M_GC_LB where sptg = 1 ", GetZdList(BasicDataType.GC));
                StringBuilder sb = new StringBuilder();
                if (where.gcbh.GetSafeRequest() != "")
                {
                    sb.Append(" and gcbh like '%" + where.gcbh.GetSafeRequest() + "%' ");
                }
                if (where.gcmc.GetSafeRequest() != "")
                {
                    sb.Append(" and gcmc like '%" + where.gcmc.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryPageData(sql + sb.ToString(), pagesize, pageindex, out totalcount);

                AddExtraInfoForGc(ref records);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        #endregion


        #region 企业

        public bool GetQys(VBasicDataGetQy where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            try
            {
                //string sql = "select lxmc,qybh,qymc,qyfzr,lxsj,qyfr,zh,qyfrsj,sy_zzmc,zzjgdm from View_I_M_QY where sptg = 1 and sfyx = 1 ";
                string sql = string.Format("select {0} from View_I_M_QY where sptg = 1 and sfyx = 1 ", GetZdList(BasicDataType.QY));
                StringBuilder sb = new StringBuilder();
                if (where.qybh.GetSafeRequest() != "")
                {
                    sb.Append(" and qybh like '%" + where.qybh.GetSafeRequest() + "%' ");
                }
                if (where.qymc.GetSafeRequest() != "")
                {
                    sb.Append(" and qymc like '%" + where.qymc.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryDataTable(sql + sb.ToString());
                totalcount = records.Count;
                AddExtraInfoForQy(ref records);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        public bool GetQys(VBasicDataGetQy where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
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
                //string sql = "select lxmc,qybh,qymc,qyfzr,lxsj,qyfr,zh,qyfrsj,sy_zzmc,zzjgdm from View_I_M_QY where sptg = 1 and sfyx = 1 ";
                string sql = string.Format("select {0} from View_I_M_QY where sptg = 1 and sfyx = 1 ", GetZdList(BasicDataType.QY));
                StringBuilder sb = new StringBuilder();
                if (where.qybh.GetSafeRequest() != "")
                {
                    sb.Append(" and qybh like '%" + where.qybh.GetSafeRequest() + "%' ");
                }
                if (where.qymc.GetSafeRequest() != "")
                {
                    sb.Append(" and qymc like '%" + where.qymc.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryPageData(sql + sb.ToString(), pagesize, pageindex, out totalcount);
                AddExtraInfoForQy(ref records);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        #endregion


        #region 人员
        public bool GetRys(VBasicDataGetRy where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            try
            {
                //string sql = "select qymc,ryxm,xb,rybh,sjhm,zh,yxzssl,sfzhm,sy_hm,yzzt,gcgw from View_I_M_RY where sptg = 1 and sfyx = 1 ";
                string sql = string.Format("select {0} from View_I_M_RY where sptg = 1 and sfyx = 1 ", GetZdList(BasicDataType.RY));
                StringBuilder sb = new StringBuilder();
                if (where.rybh.GetSafeRequest() != "")
                {
                    sb.Append(" and rybh like '%" + where.rybh.GetSafeRequest() + "%' ");
                }
                if (where.ryxm.GetSafeRequest() != "")
                {
                    sb.Append(" and ryxm like '%" + where.ryxm.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryDataTable(sql + sb.ToString());
                totalcount = records.Count;
                AddExtraInfoForRy(ref records);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        public bool GetRys(VBasicDataGetRy where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
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
                //string sql = "select qymc,ryxm,xb,rybh,sjhm,zh,yxzssl,sfzhm,sy_hm,yzzt,gcgw from View_I_M_RY where sptg = 1 and sfyx = 1 ";
                string sql = string.Format("select {0} from View_I_M_RY where  sptg = 1 and sfyx = 1 ", GetZdList(BasicDataType.RY));
                StringBuilder sb = new StringBuilder();
                if (where.rybh.GetSafeRequest() != "")
                {
                    sb.Append(" and rybh like '%" + where.rybh.GetSafeRequest() + "%' ");
                }
                if (where.ryxm.GetSafeRequest() != "")
                {
                    sb.Append(" and ryxm like '%" + where.ryxm.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryPageData(sql + sb.ToString(), pagesize, pageindex, out totalcount);
                AddExtraInfoForRy(ref records);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        #endregion


        #region 设备

        public bool GetSbs(VBasicDataGetSb where, string orderbystr, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
        {
            bool ret = true;
            msg = "";
            totalcount = 0;
            records = new List<IDictionary<string, object>>();
            try
            {
                string sql = string.Format("select {0} from view_i_m_sb ", GetZdList(BasicDataType.SB));
                StringBuilder sb = new StringBuilder();
                if (where.ssdwbh.GetSafeRequest() != "")
                {
                    sb.Append(" and ssdwbh like '%" + where.ssdwbh.GetSafeRequest() + "%' ");
                }
                if (where.ssdwmc.GetSafeRequest() != "")
                {
                    sb.Append(" and ssdwmc like '%" + where.ssdwmc.GetSafeRequest() + "%' ");
                }
                if (where.sbbh.GetSafeRequest() != "")
                {
                    sb.Append(" and sbbh like '%" + where.sbbh.GetSafeRequest() + "%' ");
                }
                if (where.sbmc.GetSafeRequest() != "")
                {
                    sb.Append(" and sbmc like '%" + where.sbmc.GetSafeRequest() + "%' ");
                }
                if (where.sbxh.GetSafeRequest() != "")
                {
                    sb.Append(" and sbxh like '%" + where.sbxh.GetSafeRequest() + "%' ");
                }
                if (where.sccj.GetSafeRequest() != "")
                {
                    sb.Append(" and sccj like '%" + where.sccj.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryDataTable(sql + sb.ToString());
                totalcount = records.Count;
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }
        public bool GetSbs(VBasicDataGetSb where, string orderbystr, int pagesize, int pageindex, out int totalcount, out IList<IDictionary<string, object>> records, out string msg)
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
                string sql = string.Format("select {0} from view_i_m_sb ", GetZdList(BasicDataType.SB));
                StringBuilder sb = new StringBuilder();
                if (where.ssdwbh.GetSafeRequest() != "")
                {
                    sb.Append(" and ssdwbh like '%" + where.ssdwbh.GetSafeRequest() + "%' ");
                }
                if (where.ssdwmc.GetSafeRequest() != "")
                {
                    sb.Append(" and ssdwmc like '%" + where.ssdwmc.GetSafeRequest() + "%' ");
                }
                if (where.sbbh.GetSafeRequest() != "")
                {
                    sb.Append(" and sbbh like '%" + where.sbbh.GetSafeRequest() + "%' ");
                }
                if (where.sbmc.GetSafeRequest() != "")
                {
                    sb.Append(" and sbmc like '%" + where.sbmc.GetSafeRequest() + "%' ");
                }
                if (where.sbxh.GetSafeRequest() != "")
                {
                    sb.Append(" and sbxh like '%" + where.sbxh.GetSafeRequest() + "%' ");
                }
                if (where.sccj.GetSafeRequest() != "")
                {
                    sb.Append(" and sccj like '%" + where.sccj.GetSafeRequest() + "%' ");
                }
                if (orderbystr.GetSafeString() != "")
                {
                    sb.Append(" order by " + orderbystr.GetSafeString());
                }

                records = CommonDao.GetBinaryPageData(sql + sb.ToString(), pagesize, pageindex, out totalcount);

            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        #endregion

        #region 添加额外信息
        /// <summary>
        /// 为工程添加企业、人员相关信息
        /// </summary>
        /// <param name="gclist"></param>
        private void AddExtraInfoForGc( ref IList<IDictionary<string, object>> gclist)
        {
            StringBuilder sbGcbhs = new StringBuilder();
            foreach (IDictionary<string, object> row in gclist)
                sbGcbhs.Append(row["gcbh"] + ",");
            string gcbhs = sbGcbhs.ToString().FormatSQLInStr();

            // 分工程信息
            IList<IDictionary<string, object>> tbIsgcfgc = CommonDao.GetBinaryDataTable("select * from I_S_GC_FGC where gcbh in (" + gcbhs + ")");
            // 监理单位
            IList<IDictionary<string, object>> tbIsgcjldw = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_JLDW_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 监理人员
            IList<IDictionary<string, object>> tbIsgcjlry = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_JLRY_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 施工单位
            IList<IDictionary<string, object>> tbIsgcsgdw = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_SGDW_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 施工人员
            IList<IDictionary<string, object>> tbIsgcsgry = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_SGRY_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 设计单位
            IList<IDictionary<string, object>> tbIsgcsjdw = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_SJDW_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 设计人员
            IList<IDictionary<string, object>> tbIsgcsjry = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_SJRY_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 建设单位
            IList<IDictionary<string, object>> tbIsgcjsdw = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_JSDW_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 建设人员
            IList<IDictionary<string, object>> tbIsgcjsry = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_JSRY_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 勘察单位
            IList<IDictionary<string, object>> tbIsgckcdw = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_KCDW_WITH_LZ where gcbh in (" + gcbhs + ")");
            // 勘察人员
            IList<IDictionary<string, object>> tbIsgckcry = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_GC_KCRY_WITH_LZ where gcbh in (" + gcbhs + ")");
            //// 图审单位
            //IList<IDictionary<string, object>> tbIsgctsdw = CommonDao.GetBinaryDataTable("select * from I_S_GC_TSDW where gcbh in (" + gcbhs + ")");
            //// 图审人员
            //IList<IDictionary<string, object>> tbIsgctsry = CommonDao.GetBinaryDataTable("select * from I_S_GC_TSRY where gcbh in (" + gcbhs + ")");

            foreach (IDictionary<string, object> mrow in gclist)
            {
                string gcbh = mrow["gcbh"].GetSafeString() ;
                // 分工程
                var q = from e in tbIsgcfgc where e["gcbh"].GetSafeString().Equals(gcbh) select e;
                mrow.Add("fgclist", q.ToList<IDictionary<string, object>>());

                // 监理单位
                IList<IDictionary<string, object>> jldws = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsgcjldw)
                {
                    string tmpgcbh = row["gcbh"].GetSafeString();
                    if (gcbh != tmpgcbh)
                        continue;
                    q = from e in tbIsgcjlry where e["qybh"].GetSafeString().Equals(row["qybh"].GetSafeString()) select e;
                    row.Add("rylist", q.ToList<IDictionary<string, object>>());
                    jldws.Add(row);
                }
                mrow.Add("jldwlist", jldws);


                // 施工单位
                IList<IDictionary<string, object>> sgdws = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsgcsgdw)
                {
                    string tmpgcbh = row["gcbh"].GetSafeString();
                    if (gcbh != tmpgcbh)
                        continue;
                    q = from e in tbIsgcsgry where e["qybh"].GetSafeString().Equals(row["qybh"].GetSafeString()) select e;
                    row.Add("rylist", q.ToList<IDictionary<string, object>>());
                    sgdws.Add(row);
                }
                mrow.Add("sgdwlist", sgdws);


                // 设计单位
                IList<IDictionary<string, object>> sjdws = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsgcsjdw)
                {
                    string tmpgcbh = row["gcbh"].GetSafeString();
                    if (gcbh != tmpgcbh)
                        continue;
                    q = from e in tbIsgcsjry where e["qybh"].GetSafeString().Equals(row["qybh"].GetSafeString()) select e;
                    row.Add("rylist", q.ToList<IDictionary<string, object>>());
                    sjdws.Add(row);
                }
                mrow.Add("sjdwlist", sjdws);


                // 建设单位
                IList<IDictionary<string, object>> jsdws = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsgcjsdw)
                {
                    string tmpgcbh = row["gcbh"].GetSafeString();
                    if (gcbh != tmpgcbh)
                        continue;
                    q = from e in tbIsgcjsry where e["qybh"].GetSafeString().Equals(row["qybh"].GetSafeString()) select e;
                    row.Add("rylist", q.ToList<IDictionary<string, object>>());
                    jsdws.Add(row);
                }
                mrow.Add("jsdwlist", jsdws);


                // 勘察单位
                IList<IDictionary<string, object>> kcdws = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsgckcdw)
                {
                    string tmpgcbh = row["gcbh"].GetSafeString();
                    if (gcbh != tmpgcbh)
                        continue;
                    q = from e in tbIsgckcry where e["qybh"].GetSafeString().Equals(row["qybh"].GetSafeString()) select e;
                    row.Add("rylist", q.ToList<IDictionary<string, object>>());
                    kcdws.Add(row);
                }
                mrow.Add("kcdwlist", kcdws);

                //// 图审单位
                //IList<IDictionary<string, object>> tsdws = new List<IDictionary<string, object>>();
                //foreach (IDictionary<string, object> row in tbIsgctsdw)
                //{
                //    string tmpgcbh = row["gcbh"].GetSafeString();
                //    if (gcbh != tmpgcbh)
                //        continue;
                //    q = from e in tbIsgctsry where e["qybh"].GetSafeString().Equals(row["qybh"].GetSafeString()) select e;
                //    row.Add("rylist", q.ToList<IDictionary<string, object>>());
                //    tsdws.Add(row);
                //}
                //mrow.Add("tsdwlist", tsdws);
            }
        }

        /// <summary>
        /// 为企业添加资质相关信息
        /// </summary>
        /// <param name="qylist"></param>
        private void AddExtraInfoForQy(ref IList<IDictionary<string, object>> qylist)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IDictionary<string, object> row in qylist)
                sb.Append(row["qybh"] + ",");
            string qybhs = sb.ToString().FormatSQLInStr();


            // 资质信息
            IList<IDictionary<string, object>> tbIsqyqyzz = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_QY_QYZZ where sptg=1 and sfyx=1  and qybh in (" + qybhs + ")");

            foreach (IDictionary<string, object> mrow in qylist)
            {
                string qybh = mrow["qybh"].GetSafeString();

                // 资质信息
                IList<IDictionary<string, object>> qyzzs = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsqyqyzz)
                {
                    string tmpqybh = row["qybh"].GetSafeString();
                    if (qybh != tmpqybh)
                        continue;
                    qyzzs.Add(row);
                }
                mrow.Add("qyzzlist", qyzzs);
            }

        }

        /// <summary>
        /// 为人员添加资质证书相关信息
        /// </summary>
        /// <param name="qylist"></param>
        private void AddExtraInfoForRy(ref IList<IDictionary<string, object>> rylist)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IDictionary<string, object> row in rylist)
                sb.Append(row["rybh"] + ",");
            string rybhs = sb.ToString().FormatSQLInStr();


            // 资质信息
            IList<IDictionary<string, object>> tbIsryryzz = CommonDao.GetBinaryDataTable("select * from VIEW_I_S_RY_RYZZ where sptg=1 and sfyx=1  and rybh in (" + rybhs + ")");

            foreach (IDictionary<string, object> mrow in rylist)
            {
                string rybh = mrow["rybh"].GetSafeString();

                // 证书信息
                IList<IDictionary<string, object>> ryzzs = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> row in tbIsryryzz)
                {
                    string tmprybh = row["rybh"].GetSafeString();
                    if (rybh != tmprybh)
                        continue;
                    ryzzs.Add(row);
                }
                mrow.Add("ryzzlist", ryzzs);
            }

        }


        #endregion

        #region 帮助函数

        private string GetZdList(BasicDataType type)
        {
            string ret = "";
            string formdm = "";
            int formstatus = 0;
            switch (type)
            {
                case BasicDataType.GC:
                    formdm = "GCZL_WDGC";
                    formstatus = 0;
                    break;
                case BasicDataType.QY:
                    formdm = "QYGL_QYBA";
                    formstatus = 0; 
                    break;
                case BasicDataType.RY:
                    formdm = "QYGL_RYBA";
                    formstatus = 0;
                    break;
                case BasicDataType.SB:
                    formdm = "JCJGGL_SBGL";
                    formstatus = 0;
                    break;
                default:
                    break;
            }
            if (formdm != "")
            {
                List<string> zdnamelist = new List<string>();
                string sql = string.Format("select zdname from formzdzd where formdm='{0}' and formstatus={1} ", formdm, formstatus );
                IList<IDictionary<string, string>> zdzd = CommonDao.GetDataTable(sql);
                foreach (IDictionary<string, string> row in zdzd)
                {
                    if (row["zdname"] !="")
                    {
                        zdnamelist.Add(row["zdname"]);
                    }
                }
                if (zdnamelist.Count > 0)
                {
                    ret = string.Join(",", zdnamelist);
                }
            }
            return ret;
            
        }
        #endregion



        #endregion





    }
}
