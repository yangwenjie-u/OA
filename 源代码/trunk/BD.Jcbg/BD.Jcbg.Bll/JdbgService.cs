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
    public class JdbgService:IJdbgService
    {
        #region 数据库对象
        public ICommonDao CommonDao { get; set; }
        #endregion
        #region 服务
        /// <summary>
        /// 获取某个工程所有种类报告数量
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public VJdbgReportSumItem GetReportSum(string gcbh, out string msg)
        {
            VJdbgReportSumItem ret = new VJdbgReportSumItem();
            msg = "";
            try
            {
                // 获取监督记录表中的统计信息
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select lx,count(*) as sum from JDBG_JDJL where gcbh='" + gcbh + "' group by lx");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }
                // 审批完成的整改单
                dt = CommonDao.GetDataTable("select 'ZGD_SP' as lx,count(*) as sum from JDBG_JDJL where lx='zgd' and lrrxm<>'' and gcbh='" + gcbh + "' ");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }
                // 获取人员离职统计信息
                dt = CommonDao.GetDataTable("select 'RYLZJL' as lx,count(*) as sum from I_S_GC_RYYJ where gcbh='" + gcbh + "'");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }

                // 获取验收申请统计信息
                dt = CommonDao.GetDataTable("select 'YSSQJL' as lx,count(*) as sum from JDBG_YSSQJL where gcbh='" + gcbh + "'");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }

                // 获取验收安排统计信息
                dt = CommonDao.GetDataTable("select 'YSAPJL' as lx,count(*) as sum from JDBG_YSAPJL where gcbh='" + gcbh + "'");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }

                // 获取竣工验收统计信息
                dt = CommonDao.GetDataTable("select 'JGYSJL' as lx,count(*) as sum from JDBG_YSSQJL where gcbh='" + gcbh + "' and yssqlx='JGYSSQ'");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }

                // 获取企业调岗记录
                dt = CommonDao.GetDataTable("select 'QYLZJL' as lx,count(*) as sum from I_S_GC_DWYJ where gcbh='" + gcbh + "'");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
                }

                // 获取监督员备注
                dt = CommonDao.GetDataTable("select 'JDYBZ' as lx,count(*) as sum from view_i_s_gc_bz where  gcbh='" + gcbh + "' and ZT=1 ");
                foreach (IDictionary<string, string> row in dt)
                {
                    string lx = row["lx"];
                    int sum = row["sum"].GetSafeInt();
                    ret.SetSum(lx, sum);
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
        /// 获取某个工程某个类型报告数量
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int GetReportSum(string gcbh, string item, out string msg)
        {
            int ret = 0;
            msg = "";
            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select count(*) as sum from JDBG_JDJL where gcbh='" + gcbh + "' and lx='"+item+"'");
                foreach (IDictionary<string, string> row in dt)
                {
                    ret = row["sum"].GetSafeInt();
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
        /// 个人查找手机上传的内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="gcbh"></param>
        /// <param name="username"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<IDictionary<string,string>> GetProblems(string key, string gcbh, string username, string dt1, string dt2, string ispub,
            int pageSize, int pageIndex, out int totalCount)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            totalCount = 0;
            try
            {
                string pubstr = "";
                if (ispub == "1")
                    pubstr += " and b.status=3 ";
                else if (ispub == "0")
                    pubstr += " and b.status=1 ";
                string sql = "select a.*,IMGIDS = STUFF((SELECT ',' + convert(varchar(10),recid)+'_'+convert(varchar(10),isused)+'_'+convert(varchar(10),status) FROM i_s_gc_problemdetail b WHERE b.problemid = a.problemid and b.type='img' " + pubstr + " FOR XML PATH('')), 1, 1, ''),VOICEIDS = STUFF((SELECT ',' + convert(varchar(10),recid) FROM i_s_gc_problemdetail b WHERE b.problemid = a.problemid and b.attachment<>'' and b.type='voice' " + pubstr + " FOR XML PATH('')), 1, 1, '') from i_s_gc_problem a where 1=1 ";
                if (key != "")
                    sql += " and (a.gcmc like '%" + key + "%' or a.comment like '%" + key + "%' or a.title like '%" + key + "%')";
                if (gcbh != "")
                    sql += " and a.gcbh='" + gcbh + "' ";
                if (username != "")
                    sql += " and a.username='" + username + "'";
                if (dt1 != "")
                    sql += " and time>=convert(datetime,'" + dt1 + "') ";
                if (dt2 != "")
                    sql += " and time<convert(datetime,'" + dt2.GetSafeDate().AddDays(1).ToString("yyyy-MM-dd") + "') ";
                sql += " order by time desc";
                ret = CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<IDictionary<string, string>> GetProblemImages(string gcbh, string username, string status)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select b.recid,b.type,a.time,b.comment from i_s_gc_problem a inner join i_s_gc_problemdetail b on a.problemid=b.problemid where 1=1 ";
                if (gcbh != "")
                    sql += " and a.gcbh='" + gcbh + "' ";
                if (username != "")
                    sql += " and a.username='" + username + "'";
                if (status != "")
                    sql += " and b.status=" + status;
                
                sql += " order by b.recid desc";
                SysLog4.WriteError(sql);
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<IDictionary<string, string>> GetProblemImagesByWorkserial(string workserial, string username, string status)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                if (workserial != "")
                {
                    string sql = "select b.recid,b.type,a.time,b.comment from i_s_gc_problem a inner join i_s_gc_problemdetail b on a.problemid=b.problemid where 1=1 ";
                    if (workserial != "")
                        sql += " and a.workserial='" + workserial + "' ";
                    if (username != "")
                        sql += " and a.username='" + username + "'";
                    if (status != "")
                        sql += " and b.status=" + status;

                    sql += " order by b.recid";
                    SysLog4.WriteError(sql);
                    ret = CommonDao.GetDataTable(sql);

                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }


        public IList<IDictionary<string, string>> GetProblemContents(string gcbh, string username, string status)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select recid, comment, title, time from i_s_gc_problem where 1=1 ";
                if (gcbh != "")
                    sql += " and gcbh='" + gcbh + "' ";
                if (username != "")
                    sql += " and username='" + username + "'";
                if (status != "")
                    sql += " and status=" + status;

                sql += " order by recid desc";
                SysLog4.WriteError(sql);
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<IDictionary<string, string>> GetProblemContentsByWorkserial(string workserial, string username, string status)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select recid, comment, title, time from i_s_gc_problem where 1=1 ";
                if (workserial != "")
                    sql += " and workserial='" + workserial + "' ";
                if (username != "")
                    sql += " and username='" + username + "'";
                if (status != "")
                    sql += " and status=" + status;

                sql += " order by recid desc";
                SysLog4.WriteError(sql);
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;

        }
        /// <summary>
        /// 获取问题图片小图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] GetProblemImageSmall(string id)
        {
            byte[] ret = null;
            try
            {
                string sql = "select thumbattachment from I_S_GC_ProblemDetail where RECID=" + id;
                IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                if (dt.Count > 0)
                    ret = dt[0]["thumbattachment"] as byte[];

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取问题图片大图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public byte[] GetProblemImageBig(string id)
        {
            byte[] ret = null;
            try
            {
                string sql = "select filecontent,storagetype, fileurl from stfile where fileid=(select RealFileID from I_S_GC_ProblemDetail where RECID=" + id + ")";
                IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                if (dt.Count > 0)
                    ret = dt[0]["filecontent"] as byte[];
                // 获取OSS
                if (ret == null || ret.Length ==0)
                {
                    string storagetype = dt[0]["storagetype"].GetSafeString();
                    string fileurl = dt[0]["fileurl"].GetSafeString();
                    if (storagetype.Equals("OSS", StringComparison.OrdinalIgnoreCase) && (!string.IsNullOrEmpty(fileurl)))
                    {
                        ret = OssCdnHelper.DownFile(fileurl);
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
        /// 获取问题语音
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] GetProblemVoice(string id)
        {
            byte[] ret = null;
            try
            {
                string sql = "select filecontent, storagetype, fileurl from stfile where fileid=(select RealFileID from I_S_GC_ProblemDetail where RECID=" + id + ")";
                IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                if (dt.Count > 0)
                    ret = dt[0]["filecontent"] as byte[];

                // 获取OSS
                if (ret == null || ret.Length == 0)
                {
                    string storagetype = dt[0]["storagetype"].GetSafeString();
                    string fileurl = dt[0]["fileurl"].GetSafeString();
                    if (storagetype.Equals("OSS", StringComparison.OrdinalIgnoreCase) && (!string.IsNullOrEmpty(fileurl)))
                    {
                        ret = OssCdnHelper.DownFile(fileurl);
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public bool SaveDJCFQ(string procstr, out string err, out string FKID)
        {
            bool ret = false;
            err = "";
            FKID = "";
            try
            {
                IList<IDictionary<string, string>> datas = CommonDao.ExecDataTableProc(procstr, out err);
                if (datas.Count == 0)
                {
                    err = "大检查记录保存失败";
                    return ret;
                }

                IDictionary<string, string> row = datas[0];
                string code = "";
                if (!row.TryGetValue("ret", out code))
                {
                    err = "存储过程未返回ret，操作失败";
                    return ret;
                }
                ret = code == "1";
                row.TryGetValue("err", out err);
                row.TryGetValue("djcfqbh", out FKID);

            }
            catch (Exception ex )
            {
                SysLog4.WriteLog(ex);
                err = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取工程和分工程的开始结束层数
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="fgcbhs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGccs(string gcbh, string fgcbhs, out string msg)
        {
            msg = "";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string fgcwhere = "";
                if (fgcbhs != "")
                    fgcwhere = " and FGCBH in (" + fgcbhs.FormatSQLInStr() + ")";
                string sql = "(select gcbh,gcmc,1 as zcb,kscs,jscs from i_m_gc where gcbh='" + gcbh + "') union all (select convert(varchar(50),recid) as gcbh,fgcmc as gcmc,0 as zcb,kscs,jscs from i_s_gc_fgc where  gcbh='" + gcbh + "' "+fgcwhere+")";
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
        ///  获取工程的验收状态
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGcyszts()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                string sql = "select bh,mc,bm from h_gczt where lx like '%Y%' or lx like '%Q%' order by xssx asc";
                ret = CommonDao.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 更新工程及分工程的开始结束层数
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        [Transaction(ReadOnly=false)]
        public bool SetGccs(IList<IDictionary<string, string>> infos, out string msg)
        {
            msg = "";
            bool ret = false;

            try
            {
                foreach (IDictionary<string, string> row in infos)
                {
                    string sql = "";
                    if (row["zcb"] == "1")
                        sql = "update i_m_gc set kscs='" + row["kscs"] + "',jscs='" + row["jscs"] + "' where gcbh='" + row["gcbh"] + "'";
                    else
                        sql = "update i_s_gc_fgc set kscs='" + row["kscs"] + "',jscs='" + row["jscs"] + "' where recid=" + row["gcbh"] + "";
                    CommonDao.ExecCommand(sql, CommandType.Text);
                }
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
        /// 返回人员类型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserType GetUserType(string username)
        {
            UserType ret = UserType.Invalid;

            try
            {
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable("select * from i_m_qyzh where yhzh='" + username + "'");
                if (dt.Count > 0)
                {
                    string zhlx = dt[0]["zhlx"].GetSafeString();
                    if (zhlx == "N" || zhlx.Equals("P"))
                    {
                        ret = UserType.InnerUser; 
                    }
                    else if (zhlx == "R")
                    {
                        ret = UserType.RyUser;
                    }
                    else if (zhlx == "Q")
                    {
                        ret = UserType.QyUser;
                    }
                    else if (zhlx == "P")
                    {
                        ret = UserType.ZjzUser;
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
        /// 获取工程类型
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGclx()
        {
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                datas = CommonDao.GetDataTable("select lxbh,lxmc from h_gclx order by xssx");
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return datas;
        }
        /// <summary>
        /// 工程统计，返回工程数量、总面积、总造价
        /// </summary>
        /// <param name="kgnf"></param>
        /// <param name="jgnf"></param>
        /// <param name="gczt"></param>
        /// <param name="gclx"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGcStatistic(string kgnf, string jgnf, string gczt, string gclx)
        {
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select count(*) as gcs, sum(convert(decimal,jzmj)) as zmj, sum(convert(decimal,gczj)) as zzj from i_m_gc where 1=1 and ZT not in('YT','LR') ");
                if (!string.IsNullOrEmpty(kgnf))
                    sb.Append(" and (kgrq is not null and datepart(year,kgrq)="+kgnf+") ");
                if (!string.IsNullOrEmpty(jgnf))
                    sb.Append(" and (jgrq is not null and datepart(year,jgrq)=" + jgnf + ") ");
                if (gczt.Equals("zj", StringComparison.OrdinalIgnoreCase))
                    sb.Append(" and zt not in ('JGYS','JDBG','GDZL') ");
                else if (gczt.Equals("jg", StringComparison.OrdinalIgnoreCase))
                    sb.Append(" and zt in ('JGYS','JDBG','GDZL') ");
                if (!string.IsNullOrEmpty(gclx))
                    sb.Append(" and gclxbh='" + gclx + "'");

                datas = CommonDao.GetDataTable(sb.ToString());
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return datas;
        }

        /// <summary>
        /// 首页工程地图标注
        /// </summary>
        /// <param name="kgnf"></param>
        /// <param name="jgnf"></param>
        /// <param name="gczt"></param>
        /// <param name="gclx"></param>
        /// <returns></returns>
        public IList<IDictionary<string,string>> GetGcList(string kgnf, string jgnf, string gczt, string gclx)
        {
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select gcbh,gcmc,zjdjh,jzmj,gczj,gcdd,gclxmc,gczb,sy_jsdwmc as jsdwmc,sy_jsdwfzr as jsdwfzr,jsdwxmfzrsjhm,jldwmc,jldwfzr,jldwxmfzrsjhm,sgdwmc,sgdwfzr,sgdwxmfzrsjhm,kcdwmc,kcdwfzr,kcdwxmfzrsjhm, sjdwmc,sjdwfzr,sjdwxmfzrsjhm,gcjdzt as gczt,zt from View_I_M_GC_LB where 1=1 and zt not in ('YT','LR')");
                if (!string.IsNullOrEmpty(kgnf))
                    sb.Append(" and (kgrq is not null and datepart(year,kgrq)="+kgnf+") ");
                if (!string.IsNullOrEmpty(jgnf))
                    sb.Append(" and (jgrq is not null and datepart(year,jgrq)=" + jgnf + ") ");
                if (gczt.Equals("zj", StringComparison.OrdinalIgnoreCase))
                    sb.Append(" and zt not in ('JGYS','JDBG','GDZL') ");
                else if (gczt.Equals("jg", StringComparison.OrdinalIgnoreCase))
                    sb.Append(" and zt in ('JGYS','JDBG','GDZL') ");
                if (!string.IsNullOrEmpty(gclx))
                    sb.Append(" and gclxbh='" + gclx + "'");
                sb.Append(" order by gcbh desc");

                datas = CommonDao.GetDataTable(sb.ToString());

                foreach (IDictionary<string,string> row in datas)
                {
                    string gczb = row["gczb"].GetSafeString();
                    string longitude = "0";
                    string latitude = "0";
                    if (!string.IsNullOrEmpty(gczb))
                    {
                        string[] arr = gczb.Split(new char[] { ',' });
                        longitude = arr[0];
                        if (arr.Length > 1)
                            latitude = arr[1];
                    }
                    row.Add("longitude", longitude);
                    row.Add("latitude", latitude);
                }
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return datas;
        }
        #endregion

    }
}

