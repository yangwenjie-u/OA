using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Web.Script.Serialization;


namespace BD.Jcbg.Bll
{
    public class WgryKqjService : IWgryKqjService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        IKqjUserLogDao KqjUserLogDao { get; set; }
        IInfoScheduleDao InfoScheduleDao { get; set; }
        IKqjUserDayLogDao KqjUserDayLogDao { get; set; }
        IKqjUserDayLogDetailDao KqjUserDayLogDetailDao { get; set; }
        IInfoGzgzDao InfoGzgzDao { get; set; }
        IKqjUserMonthLogDao KqjUserMonthLogDao { get; set; }
        IInfoWgryHistoryDao InfoWgryHistoryDao { get; set; }

        #endregion

        #region 考勤机相关功能

        #region 考勤机轮询
        public IList<IDictionary<string, string>> GetKqjUserLog()
        {
            string sql = "select * from kqjuserlog where HasDeal='False' and serial!='' order by LogDate";
            return CommonDao.GetDataTable(sql);
        }

        public IList<IDictionary<string, string>> GetWxKqjUserLog()
        {
            string sql = "select * from kqjuserlog where HasDeal='False' and (serial='' or serial is null) order by LogDate";
            return CommonDao.GetDataTable(sql);
        }
        /// <summary>
        /// 保存一条考勤信息(考勤机)
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool SaveUserLog(string serial, string userid, DateTime time)
        {
            string qybh, qymc, jdzch, gcmc, kqjlx = "1", bzfzr, ryxm, bzfzrxm = "", kqtimes = "2";
            IDictionary<string, string> kqjrow = CommonDao.GetRowValue("qybh,qymc,gcmc,jdzch,kqjlx", "View_I_M_KQJ", "kqjbh='" + serial + "'");
            if (kqjrow == null)
                return false;
            kqjrow.TryGetValue("qybh", out qybh);
            kqjrow.TryGetValue("qymc", out qymc);
            kqjrow.TryGetValue("gcmc", out gcmc);
            kqjrow.TryGetValue("jdzch", out jdzch);
            kqjrow.TryGetValue("kqjlx", out kqjlx);

            IDictionary<string, string> kqjlogtoday = CommonDao.GetRowValue("recid", "kqjuserlog", "userid='" + userid + "'and placeid !='" + jdzch + "' and placeid!='' and datediff(dd,logdate,'" + time + "')=0");
            if (kqjlogtoday != null)
            {
                string recid = "";
                kqjlogtoday.TryGetValue("recid", out recid);
                //string sql = "delete from kqjuserlog where recid='" + recid + "'";
                //   string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 where recid='" + recid + "'";  //表示今天已考勤过
                //设置本次考勤无效，根据dealtype=0 为无效 1为有效
                //string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 where userid='" + userid + "'and companyid='" + qybh + "' and placeid='" + jdzch + "' and datediff(dd,logdate,'" + time + "')=0";  //表示今天已考勤过
                // string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 ,";  //表示今天已考勤过
                string sql = "Update kqjuserlog Set HasDeal = 1,DealType=0,companyid='" + qybh + "',placeid='" + jdzch + "' ";
                string where = "where serial='" + serial + "' and userid='" + userid + "' and logdate=convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "') and  HasDeal='False'";
                sql += where;
                CommonDao.ExecSql(sql);
                return false;
            }

            IDictionary<string, string> ryrow = null;
            string sfbzfzr = "";
            if (userid.Length == 16)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "View_I_M_WGRY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    ryrow.TryGetValue("sfzhm", out userid);
                }

            }
            ryrow = CommonDao.GetRowValue("bzfzr,ryxm,bzfzrxm,sfbzfzr,hasdelete", "View_I_M_WGRY", "sfzhm='" + userid + "' and jdzch='" + jdzch + "' and qybh='" + qybh + "'");

            if (ryrow == null)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "I_M_WGRY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    //string sql = "insert into i_m_wgry select top 1 [LXBH] ,'" + qybh + "','" + jdzch + "'  ,'" + gcmc + "' ,[RYBH] ,[RYXM] ,[ZSH] ,[DH] ,[XB] ,[MZ] ,[CSRQ] ,[SFZHM] ,[SFZDZ] ,[QFJG] ,[SFZYXQ] ,[LRRZH] ,[LRRXM] ,[SSDWBH] ,[SSDWMC] ,[LRSJ] ,[SPTG] ,[SFYX] ,[ZP] ,[HM] ,[HMZL] ,[JCRJZH]  ,[SPBZ] ,[RYBZ] ,[ZH] ,[SJHM] ,[ZC] ,[SJYZM] ,[TYYHXY] ,[GW] ,[GZ] ,[SFBZFZR] ,'' ,[YHKYH],[YHKH] ,[GZDJ],[Lon] ,[Lat],[RYBH_YC],[LastUpdate],0 ,[SFZKH],[YHKH],[GZDJ],[Lon] ,[Lat] ,[LastUpdate],[hasdelete] ,[SFZKH] ,[SFQDHT],[HTFJ] ,[SFAQJY] ,[XCZP] ";
                    //sql+=" from i_m_wgry where sfzhm='" + userid + "'";
                    //if(!CommonDao.ExecSql(sql))
                    return false;
                }
                else //工程人员表没有该人员
                    return false;

            }
            else
            {
                string hasdelete = "";
                ryrow.TryGetValue("hasdelete", out hasdelete);
                if (hasdelete == "True")
                {
                    string sql = "update i_m_wgry set hasdelete=0 where sfzhm='" + userid + "' and jdzch='" + jdzch + "' and qybh='" + qybh + "'";
                    CommonDao.ExecSql(sql);
                }
            }



            //获取项目是单向还是双向
            IDictionary<string, string> qykqrow = CommonDao.GetRowValue("kqtimes", "i_m_gc", "gcbh='" + jdzch + "'");
            if (qykqrow == null)
                return false;
            qykqrow.TryGetValue("kqtimes", out kqtimes);
            if (string.IsNullOrEmpty(kqtimes))
                kqtimes = "2";

            ryrow.TryGetValue("ryxm", out ryxm);
            ryrow.TryGetValue("bzfzr", out bzfzr);
            ryrow.TryGetValue("bzfzrxm", out bzfzrxm);
            ryrow.TryGetValue("sfbzfzr", out sfbzfzr);

            KqjUserLog log = new KqjUserLog()
            {
                CompanyId = qybh,
                LogDate = time,
                PlaceId = jdzch,
                Serial = serial,
                UserId = userid,
                ProjectName = gcmc,
                CompanyName = qymc

            };

            if (kqjlx.Equals("上班考勤"))
                kqjlx = UserLogType.In;
            else if (kqjlx.Equals("下班考勤"))
                kqjlx = UserLogType.Out;
            else if (kqjlx.Equals("门禁"))
                kqjlx = UserLogType.Check;
            else
                kqjlx = GetUserLogType(log);

            log.LogType = kqjlx;

            bool ret = updateKqjUserLog(serial, userid, qybh, jdzch, time, kqjlx, qymc, gcmc);


            if (!ret)
                return false;
            if (kqjlx == UserLogType.Check)
                return ret;
            if (!UpdateUserZT_GC(userid, qybh, jdzch)) //设置人员进场工地
                return false;
            if (!UpdateUserInTime(userid, jdzch))  //设置人员进场日期
                return false;
            SetInRyNum_GC(jdzch, userid, kqjlx); //设置在场人数

            if (kqtimes == "2") //有上下班考勤
                SaveUserDayLog2(log);
            else
                SaveUserDayLog(log);
            //SaveUserDayLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, bzfzr, bzfzrxm);
            return ret;
        }

        /// <summary>
        /// 保存一条考勤信息(微信二维码)
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool SaveUserLog_WX(string serial, string userid, DateTime time, string kqjlx)
        {
            string qybh, qymc, jdzch, gcmc, bzfzr, ryxm, bzfzrxm = "", kqtimes = "2";
            IDictionary<string, string> qrcoderow = CommonDao.GetRowValue("qybh,qymc,gcmc,gcbh", "View_I_M_QRCODE", "xlh='" + serial + "'");
            if (qrcoderow == null)
                return false;
            qrcoderow.TryGetValue("qybh", out qybh);
            qrcoderow.TryGetValue("qymc", out qymc);
            qrcoderow.TryGetValue("gcmc", out gcmc);
            qrcoderow.TryGetValue("gcbh", out jdzch);

            IDictionary<string, string> kqjlogtoday = CommonDao.GetRowValue("recid", "kqjuserlog", "userid='" + userid + "'and placeid !='" + jdzch + "' and placeid!='' and datediff(dd,logdate,'" + time + "')=0");
            if (kqjlogtoday != null)
            {
                string recid = "";
                kqjlogtoday.TryGetValue("recid", out recid);
                //string sql = "delete from kqjuserlog where recid='" + recid + "'";
                //   string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 where recid='" + recid + "'";  //表示今天已考勤过
                //设置本次考勤无效，根据dealtype=0 为无效 1为有效
                //string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 where userid='" + userid + "'and companyid='" + qybh + "' and placeid='" + jdzch + "' and datediff(dd,logdate,'" + time + "')=0";  //表示今天已考勤过
                // string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 ,";  //表示今天已考勤过
                string sql = "Update kqjuserlog Set HasDeal = 1,DealType=0,companyid='" + qybh + "',placeid='" + jdzch + "' ";
                string where = "where serial='" + serial + "' and userid='" + userid + "' and logdate=convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "') and  HasDeal='False'";
                sql += where;
                CommonDao.ExecSql(sql);
                return false;
            }

            IDictionary<string, string> ryrow = null;
            string sfbzfzr = "";
            if (userid.Length == 16)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "View_I_M_WGRY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    ryrow.TryGetValue("sfzhm", out userid);
                }

            }
            ryrow = CommonDao.GetRowValue("bzfzr,ryxm,bzfzrxm,sfbzfzr,hasdelete", "View_I_M_WGRY", "sfzhm='" + userid + "' and jdzch='" + jdzch + "' and qybh='" + qybh + "'");

            if (ryrow == null)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "I_M_WGRY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    //string sql = "insert into i_m_wgry select top 1 [LXBH] ,'" + qybh + "','" + jdzch + "'  ,'" + gcmc + "' ,[RYBH] ,[RYXM] ,[ZSH] ,[DH] ,[XB] ,[MZ] ,[CSRQ] ,[SFZHM] ,[SFZDZ] ,[QFJG] ,[SFZYXQ] ,[LRRZH] ,[LRRXM] ,[SSDWBH] ,[SSDWMC] ,[LRSJ] ,[SPTG] ,[SFYX] ,[ZP] ,[HM] ,[HMZL] ,[JCRJZH]  ,[SPBZ] ,[RYBZ] ,[ZH] ,[SJHM] ,[ZC] ,[SJYZM] ,[TYYHXY] ,[GW] ,[GZ] ,[SFBZFZR] ,'' ,[YHKYH],[YHKH] ,[GZDJ],[Lon] ,[Lat],[RYBH_YC],[LastUpdate],0 ,[SFZKH],[YHKH],[GZDJ],[Lon] ,[Lat] ,[LastUpdate],[hasdelete] ,[SFZKH] ,[SFQDHT],[HTFJ] ,[SFAQJY] ,[XCZP] ";
                    //sql+=" from i_m_wgry where sfzhm='" + userid + "'";
                    //if(!CommonDao.ExecSql(sql))
                    return false;
                }
                else //工程人员表没有该人员
                    return false;

            }
            else
            {
                string hasdelete = "";
                ryrow.TryGetValue("hasdelete", out hasdelete);
                if (hasdelete == "True")
                {
                    string sql = "update i_m_wgry set hasdelete=0 where sfzhm='" + userid + "' and jdzch='" + jdzch + "' and qybh='" + qybh + "'";
                    CommonDao.ExecSql(sql);
                }
            }



            //获取项目是单向还是双向
            IDictionary<string, string> qykqrow = CommonDao.GetRowValue("kqtimes", "i_m_gc", "gcbh='" + jdzch + "'");
            if (qykqrow == null)
                return false;
            qykqrow.TryGetValue("kqtimes", out kqtimes);
            if (string.IsNullOrEmpty(kqtimes))
                kqtimes = "2";

            ryrow.TryGetValue("ryxm", out ryxm);
            ryrow.TryGetValue("bzfzr", out bzfzr);
            ryrow.TryGetValue("bzfzrxm", out bzfzrxm);
            ryrow.TryGetValue("sfbzfzr", out sfbzfzr);

            KqjUserLog log = new KqjUserLog()
            {
                CompanyId = qybh,
                LogDate = time,
                PlaceId = jdzch,
                Serial = serial,
                UserId = userid,
                ProjectName = gcmc,
                CompanyName = qymc

            };


            log.LogType = kqjlx;

            bool ret = updateKqjUserLog(serial, userid, qybh, jdzch, time, kqjlx, qymc, gcmc);


            if (!ret)
                return false;
            if (kqjlx == UserLogType.Check)
                return ret;
            if (!UpdateUserZT_GC(userid, qybh, jdzch)) //设置人员进场工地
                return false;
            if (!UpdateUserInTime(userid, jdzch))  //设置人员进场日期
                return false;
            SetInRyNum_GC(jdzch, userid, kqjlx); //设置在场人数

            if (kqtimes == "2") //有上下班考勤
                SaveUserDayLog2(log);
            else
                SaveUserDayLog(log);
            //SaveUserDayLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, bzfzr, bzfzrxm);
            return ret;
        }
        /// <summary>
        /// 微信签到考勤-交通项目
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <param name="qybh"></param>
        /// <param name="jdzch"></param>
        /// <param name="kqtype"></param>
        /// <returns></returns>
        public bool SaveUserLogByWx(string userid, DateTime time, string qybh, string jdzch, string kqtype)
        {
            string qymc, gcmc, kqjlx = "1", bzfzr, ryxm, bzfzrxm = "", kqtimes = "2";

            IDictionary<string, string> qyrow = CommonDao.GetRowValue("qymc", "I_M_QY", "qybh='" + qybh + "'");
            if (qyrow == null)
                return false;
            qyrow.TryGetValue("qymc", out qymc);
            IDictionary<string, string> gcrow = CommonDao.GetRowValue("gcmc", "I_M_GC", "gcbh='" + jdzch + "'");
            if (gcrow == null)
                return false;
            gcrow.TryGetValue("gcmc", out gcmc);

            IDictionary<string, string> kqjlogtoday = CommonDao.GetRowValue("recid", "kqjuserlog", "userid='" + userid + "'and placeid !='" + jdzch + "' and placeid!='' and datediff(dd,logdate,'" + time + "')=0");
            if (kqjlogtoday != null)
            {
                string recid = "";
                kqjlogtoday.TryGetValue("recid", out recid);
                string sql = "Update kqjuserlog Set HasDeal = 1,DealType=0 ";
                string where = "where companyid='" + qybh + "',placeid='" + jdzch + "' and userid='" + userid + "' and logdate=convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "') and  HasDeal='False'";
                sql += where;
                CommonDao.ExecSql(sql);
                return false;
            }

            IDictionary<string, string> ryrow = null;
            string sfbzfzr = "";
            if (userid.Length == 16)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "View_I_M_WGRY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    ryrow.TryGetValue("sfzhm", out userid);
                }

            }
            ryrow = CommonDao.GetRowValue("bzfzr,ryxm,bzfzrxm,sfbzfzr,hasdelete", "View_I_M_WGRY", "sfzhm='" + userid + "' and jdzch='" + jdzch + "' and qybh='" + qybh + "'");

            if (ryrow == null)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "I_M_WGRY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    //string sql = "insert into i_m_wgry select top 1 [LXBH] ,'" + qybh + "','" + jdzch + "'  ,'" + gcmc + "' ,[RYBH] ,[RYXM] ,[ZSH] ,[DH] ,[XB] ,[MZ] ,[CSRQ] ,[SFZHM] ,[SFZDZ] ,[QFJG] ,[SFZYXQ] ,[LRRZH] ,[LRRXM] ,[SSDWBH] ,[SSDWMC] ,[LRSJ] ,[SPTG] ,[SFYX] ,[ZP] ,[HM] ,[HMZL] ,[JCRJZH]  ,[SPBZ] ,[RYBZ] ,[ZH] ,[SJHM] ,[ZC] ,[SJYZM] ,[TYYHXY] ,[GW] ,[GZ] ,[SFBZFZR] ,'' ,[YHKYH],[YHKH] ,[GZDJ],[Lon] ,[Lat],[RYBH_YC],[LastUpdate],0 ,[SFZKH] from i_m_wgry where sfzhm='" + userid + "'";
                    //if (!CommonDao.ExecSql(sql))
                    return false;
                }
                else //工程人员表没有该人员
                    return false;

            }
            else
            {
                string hasdelete = "";
                ryrow.TryGetValue("hasdelete", out hasdelete);
                if (hasdelete == "True")
                {
                    string sql = "update i_m_wgry set hasdelete=0 where sfzhm='" + userid + "' and jdzch='" + jdzch + "' and qybh='" + qybh + "'";
                    CommonDao.ExecSql(sql);
                }
            }

            //获取项目是单向还是双向
            IDictionary<string, string> qykqrow = CommonDao.GetRowValue("kqtimes", "i_m_gc", "gcbh='" + jdzch + "'");
            if (qykqrow == null)
                return false;
            qykqrow.TryGetValue("kqtimes", out kqtimes);
            if (kqtimes == "")
                kqtimes = "2";

            ryrow.TryGetValue("ryxm", out ryxm);
            ryrow.TryGetValue("bzfzr", out bzfzr);
            ryrow.TryGetValue("bzfzrxm", out bzfzrxm);
            ryrow.TryGetValue("sfbzfzr", out sfbzfzr);

            KqjUserLog log = new KqjUserLog()
            {
                CompanyId = qybh,
                LogDate = time,
                PlaceId = jdzch,
                UserId = userid,
                ProjectName = gcmc,
                CompanyName = qymc

            };

            kqjlx = kqtype;

            log.LogType = kqjlx;

            bool ret = updateWxkqlog(userid, qybh, jdzch, time, kqtype, qymc, gcmc);


            if (!ret)
                return false;
            if (kqjlx == UserLogType.Check)
                return ret;
            if (!UpdateUserZT_GC(userid, qybh, jdzch)) //设置人员进场工地
                return false;
            if (!UpdateUserInTime(userid, jdzch))  //设置人员进场日期
                return false;
            SetInRyNum_GC(jdzch, userid, kqjlx); //设置在场人数

            if (kqtimes == "2") //有上下班考勤
                SaveUserDayLog2(log);
            else
                SaveUserDayLog(log);
            //SaveUserDayLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, bzfzr, bzfzrxm);
            return ret;
        }

        /// <summary>
        /// 根据考勤时间，和排班，匹配属于哪条考勤日志；如果没有匹配到，新建日志
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="logs"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        protected bool SaveUserDayLog(KqjUserLog log)
        {
            bool ret = true;
            try
            {
                KqjUserDayLog daylog = null;
                KqjUserDayLogDetail daylogdetail = null;

                /* 当天和昨天已保存的考勤数据 */
                IList<KqjUserDayLog> daylogs = KqjUserDayLogDao.Gets(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);
                IList<KqjUserDayLogDetail> daylogdetails = KqjUserDayLogDetailDao.Gets(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);

                /* 工地排班记录 */
                IList<InfoSchedule> schedules = InfoScheduleDao.Gets(log.CompanyId, log.PlaceId);

                /* 考勤对应的排班 */
                InfoSchedule schedule = null;
                // 上班考勤,要么在考勤区间内，要么离上班时间最近的一个
                //if (log.LogTye == UserLogType.In)
                {
                    // 考勤区间
                    var q = schedules.Where(t => TimeSpanOperation.IsTimeIn(t.StartTime, t.EndTime, log.LogDate.Value));
                    if (q.Count() > 0)
                        schedule = q.First();
                    // 开始时间离上班考勤最近的
                    else
                    {
                        q = from e in schedules where TimeSpanOperation.GetTimeSpan(e.StartTime).TotalMinutes > log.LogDate.Value.TimeOfDay.TotalMinutes orderby TimeSpanOperation.GetTimeSpan(e.StartTime).Subtract(log.LogDate.Value.TimeOfDay) ascending select e;
                        if (q.Count() > 0)
                            schedule = q.First();
                    }
                }
                #region
                // 下班考勤，取最后未考勤记录 (没有下班考勤)
                //else
                //{
                //var q = daylogdetails.Where(t => (t.OutTime == null)).OrderByDescending(t => t.InTime);
                //if (q.Count() > 0)
                //{
                //    var q2 = daylogs.Where(t => (t.Recid == q.First().ParentId));
                //    if (q2.Count() > 0)
                //    {
                //        var q3 = schedules.Where(t => (t.Recid == q2.First().ScheduleId));
                //        if (q3.Count() > 0)
                //            schedule = q3.First();
                //    }
                //}
                //}
                #endregion
                /* 没有匹配到排班，返回错误 */
                if (schedule == null)
                    return false;

                /* 人员对应的工种ID */
                //string gzid = "", gzname = "";
                //IDictionary<string, string> gzs = CommonDao.GetRowValue("recid,helptext", "HpGz", "HelpValue in (select gz from infowgry where sfzhm='" + log.UserId + "')");
                //gzs.TryGetValue("recid", out gzid);
                //gzs.TryGetValue("helptext", out gzname);

                string gzid = "", gzname = "";
                IDictionary<string, string> gzs = CommonDao.GetRowValue("recid,gzname", "H_RYGZ", "gzname in (select gz from I_M_WGRY where sfzhm='" + log.UserId + "' and jdzch='" + log.PlaceId + "')");
                gzs.TryGetValue("recid", out gzid);
                gzs.TryGetValue("gzname", out gzname);

                string gw = "", ryxm = "", bzfzr = "";
                IDictionary<string, string> gws = CommonDao.GetRowValue("ryxm,gw,bzfzr", "I_M_WGRY", " sfzhm='" + log.UserId + "' and jdzch='" + log.PlaceId + "'");
                gws.TryGetValue("gw", out gw);
                gws.TryGetValue("ryxm", out ryxm);
                gws.TryGetValue("bzfzr", out bzfzr);

                string bzfzrxm = "";
                IDictionary<string, string> bzfzrs = CommonDao.GetRowValue("ryxm as bzfzrxm", "I_M_RY_INFO", " sfzhm='" + bzfzr + "'");
                if (bzfzrs != null)
                    bzfzrs.TryGetValue("bzfzrxm", out bzfzrxm);

                /* 上班考勤只找当天的，班次是一样的，并且还没下班考勤的*/
                //if (log.LogTye == UserLogType.In)
                {
                    // 当天班次一样的
                    var qdl = from e in daylogs where e.LogDay.Value.Equals(log.LogDate.Value.Date) && e.ScheduleId == schedule.Recid select e;
                    if (qdl.Count() > 0)
                    {
                        daylog = qdl.First();
                        // 未考勤的
                        //var qdld = from e in daylogdetails where e.ParentId == daylog.Recid && e.OutTime == null select e;
                        var qdld = from e in daylogdetails where e.ParentId == daylog.Recid select e;
                        if (qdld.Count() > 0)
                        {
                            daylogdetail = qdld.First();
                            //daylogdetail.InTime = log.LogDate;
                        }

                    }
                    // 没找到日考勤记录，新建记录
                    if (daylog == null)
                    {
                        daylog = GetNewDayLog(schedule, ryxm, gzid, gw, bzfzr, bzfzrxm, log);
                    }
                    // 没找到考勤记录详情，新建记录
                    if (daylogdetail == null)
                    {
                        daylogdetail = GetNewDayLogDetail(log.LogDate.Value);
                    }
                    //设置班次数目
                    if (daylog != null)
                        SetUserDayLogIn(schedule, log, daylog);
                }
                #region
                /* 下班当天或昨天的,还没有下班考勤的。
				** 如果班次一样的，更新考勤记录，如果班次
				** 如果没有未处理的上班记录，丢弃 */
                //else
                //{
                //    // 没有下班考勤的记录倒序
                //    var qdld = daylogdetails.Where(t => (t.OutTime == null)).OrderByDescending(t => t.InTime);
                //    if (qdld.Count() > 0)
                //    {
                //        daylogdetail = qdld.First();
                //        var qdl = daylogs.Where(t => (t.Recid == daylogdetail.ParentId));
                //        if (qdl.Count() > 0)
                //            daylog = qdl.First();
                //    }
                //    if (daylog != null && daylogdetail != null)
                //        SetUserDayLogOut(schedule, log, daylog, daylogdetail);
                //}
                #endregion
                if (daylog != null && daylogdetail != null)
                {
                    KqjUserDayLogDao.Save(daylog);
                    if (daylogdetail.ParentId == 0)
                        daylogdetail.ParentId = daylog.Recid;
                    daylogdetail.ScheduleId = schedule.Recid;
                    KqjUserDayLogDetailDao.Save(daylogdetail);
                    SaveUserMonthLog(daylog, gzid, gzname, UserLogType.In, log.LogDate.Value);// 设置考勤类型都为In ,每天一条记录,一天多个班次记录到同一条，考勤时间表示该天有考勤，数据有重叠，不做作用

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e + log.Recid.ToString());
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// 上下班考勤
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        protected bool SaveUserDayLog2(KqjUserLog log)
        {
            bool ret = true;
            try
            {
                KqjUserDayLog daylog = null;
                KqjUserDayLogDetail daylogdetail = null;

                /* 工地排班记录 */
                IList<InfoSchedule> schedules = InfoScheduleDao.Gets(log.CompanyId, log.PlaceId);
                /* 当天和昨天已保存的考勤数据 */
                IList<KqjUserDayLog> daylogs = KqjUserDayLogDao.Gets(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);
                // IList<KqjUserDayLogDetail> daylogdetails = KqjUserDayLogDetailDao.Gets(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);
                /* 所有没有出工地的考勤数据 */
                IList<KqjUserDayLogDetail> daylogdetails = KqjUserDayLogDetailDao.GetsOut(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);
                /* 考勤对应的排班 */
                InfoSchedule schedule = null;
                // 上班考勤,要么在考勤区间内，要么离上班时间最近的一个
                if (log.LogType == UserLogType.In)
                {
                    // 考勤区间
                    var q = schedules.Where(t => TimeSpanOperation.IsTimeIn(t.StartTime, t.EndTime, log.LogDate.Value));
                    if (q.Count() > 0)
                        schedule = q.First();
                    // 开始时间离上班考勤最近的
                    else
                    {
                        q = from e in schedules where TimeSpanOperation.GetTimeSpan(e.StartTime).TotalMinutes > log.LogDate.Value.TimeOfDay.TotalMinutes orderby TimeSpanOperation.GetTimeSpan(e.StartTime).Subtract(log.LogDate.Value.TimeOfDay) ascending select e;
                        if (q.Count() > 0)
                            schedule = q.First();
                    }
                }
                // 下班考勤，取最后未考勤记录 (没有下班考勤)
                else
                {
                    var q = daylogdetails.Where(t => (log.LogDate > t.InTime && (t.OutTime == null) || (t.OutTime == t.InTime))).OrderByDescending(t => t.InTime);
                    if (q.Count() > 0)
                    {
                        var q2 = daylogs.Where(t => (t.Recid == q.First().ParentId));
                        if (q2.Count() > 0)
                        {
                            var q3 = schedules.Where(t => (t.Recid == q2.First().ScheduleId));
                            if (q3.Count() > 0)
                                schedule = q3.First();
                        }
                    }
                    else //没有进的考勤过，没有当天考勤记录
                    {
                        var sc = schedules.Where(t => TimeSpanOperation.IsTimeIn(t.StartTime, t.EndTime, log.LogDate.Value));
                        if (sc.Count() > 0)
                            schedule = sc.First();
                    }
                }
                /* 没有匹配到排班，返回错误 */
                if (schedule == null)
                    return false;

                /* 人员对应的工种ID */
                string gzid = "", gzname = "";
                IDictionary<string, string> gzs = CommonDao.GetRowValue("recid,gzname", "H_RYGZ", "gzname in (select gz from i_m_wgry where sfzhm='" + log.UserId + "' and jdzch='" + log.PlaceId + "')");
                gzs.TryGetValue("recid", out gzid);
                gzs.TryGetValue("gzname", out gzname);

                string gw = "", ryxm = "", bzfzr = "";
                IDictionary<string, string> gws = CommonDao.GetRowValue("ryxm,gw,bzfzr", "I_M_WGRY", " sfzhm='" + log.UserId + "' and jdzch='" + log.PlaceId + "'");
                gws.TryGetValue("gw", out gw);
                gws.TryGetValue("ryxm", out ryxm);
                gws.TryGetValue("bzfzr", out bzfzr);

                string bzfzrxm = "";
                IDictionary<string, string> bzfzrs = CommonDao.GetRowValue("ryxm as bzfzrxm", "I_M_RY_INFO", " sfzhm='" + bzfzr + "'");
                if (bzfzrs != null)
                    bzfzrs.TryGetValue("bzfzrxm", out bzfzrxm);

                /* 上班考勤只找当天的，班次是一样的，并且还没下班考勤的*/
                if (log.LogType == UserLogType.In)
                {
                    // 当天班次一样的,如果是昨天的，这不会更新了
                    var qdl = from e in daylogs where e.LogDay.Value.Equals(log.LogDate.Value.Date) && e.ScheduleId == schedule.Recid select e;
                    if (qdl.Count() > 0)
                    {
                        daylog = qdl.First();
                        /* 当天和昨天没有进工地的考勤数据,且间隔小于18小时 */
                        IList<KqjUserDayLogDetail> daylogdetailsIn = KqjUserDayLogDetailDao.GetsIn(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);
                        //查找有出的考勤，没有进考勤的记录，插入进考勤记录并计算
                        var logwithoutIn = from e in daylogdetailsIn where e.ParentId == daylog.Recid && e.InTime == null && log.LogDate < e.OutTime orderby e.OutTime ascending select e;
                        if (logwithoutIn.Count() > 0)
                        {
                            daylogdetail = logwithoutIn.First();
                            if (daylog != null && daylogdetail != null)
                                SetUserDayLogIn(schedule, log, ref daylog, ref daylogdetail, schedules); //根据进的记录计算
                        }
                        else
                        {
                            // 未下班考勤的
                            var qdld = from e in daylogdetails where e.ParentId == daylog.Recid && e.OutTime == null select e;
                            if (qdld.Count() > 0)
                            {
                                daylogdetail = qdld.First();
                                if (log.LogDate.Value.Subtract(daylogdetail.InTime.Value).TotalMinutes < DataFormat.GetSafeInt(schedule.LjTime, 20))
                                    ;//daylogdetail.InTime = log.LogDate; 进工地时间不覆盖,即舍弃
                                else
                                {
                                    daylogdetail.OutTime = daylogdetail.InTime;
                                    KqjUserDayLogDetailDao.Save(daylogdetail);
                                    daylogdetail = null;
                                }
                            }
                        }
                    }
                    else //当天当前班次没有考勤过
                    {
                        for (int i = 0; i < daylogdetails.Count; i++)
                        {
                            if (daylogdetails[i].OutTime == null)
                            {
                                daylogdetails[i].OutTime = daylogdetails[i].InTime;
                                KqjUserDayLogDetailDao.Save(daylogdetails[i]);
                            }
                        }
                    }
                    // 没找到日考勤记录，新建记录
                    if (daylog == null)
                    {
                        daylog = GetNewDayLog(schedule, ryxm, gzid, gw, bzfzr, bzfzrxm, log);
                    }
                    // 没找到考勤记录详情，新建记录
                    if (daylogdetail == null)
                    {
                        daylogdetail = GetNewDayLogDetail(log.LogDate.Value);
                    }
                }
                /* 下班当天或昨天的,还没有下班考勤的。
				** 如果班次一样的，更新考勤记录，如果班次
				** 如果没有未处理的上班记录，丢弃 */
                else
                {
                    // 没有下班考勤的记录倒序
                    //var qdld = daylogdetails.Where(t => (t.OutTime == null && t.InTime < log.LogDate)).OrderByDescending(t => t.InTime);
                    var qdld = daylogdetails.Where(t => ((t.OutTime == null && t.InTime < log.LogDate) || (t.InTime == t.OutTime && t.InTime < log.LogDate))).OrderByDescending(t => t.InTime);
                    if (qdld.Count() > 0)
                    {
                        daylogdetail = qdld.First();
                        var qdl = daylogs.Where(t => (t.Recid == daylogdetail.ParentId));
                        if (qdl.Count() > 0)
                            daylog = qdl.First();
                    }
                    else  // 没有上班班考勤的记录倒序
                    {
                        //判断KqjUserDayLog表该天有没有插入过
                        IList<KqjUserDayLog> daylogs_log = KqjUserDayLogDao.GetDayLogs(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date, schedule);
                        if (daylogs_log.Count == 0)
                            daylog = null;
                        else
                            daylog = daylogs_log.First();
                    }
                    string msg = "";
                    if (daylog != null && daylogdetail != null)
                        SetUserDayLogOut(schedule, log, ref daylog, ref daylogdetail, schedules, out msg);
                    if (msg != "")  //考勤时间在已有的schedule 的intime和outtime之间，处理不容易，暂时不处理
                        return false;
                    ////////////////没有进的记录,记录出////////////////////////
                    if (daylog == null)
                    {
                        daylog = GetNewDayLog(schedule, ryxm, gzid, gw, bzfzr, bzfzrxm, log);
                    }
                    // 没找到考勤记录详情，新建记录
                    if (daylogdetail == null)
                    {
                        daylogdetail = GetNewDayLogOutDetail(log.LogDate.Value);
                    }
                    /////////////////////
                }
                if (daylog != null && daylogdetail != null)
                {
                    KqjUserDayLogDao.Save(daylog);
                    if (daylogdetail.ParentId == 0)
                        daylogdetail.ParentId = daylog.Recid;
                    daylogdetail.ScheduleId = schedule.Recid;
                    KqjUserDayLogDetailDao.Save(daylogdetail);
                    SaveUserMonthLog(daylog, gzid, gzname, log.LogType, log.LogDate.Value);

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e + log.Recid.ToString());
                ret = false;
            }

            return ret;
        }

        public bool UpdateUserInTime(string userid, string gcbh)
        {
            bool ret = false;
            IList<InfoWgryHistory> historys = InfoWgryHistoryDao.Gets(userid, "", gcbh);
            // 没有进出工地记录
            if ((historys == null || historys.Count == 0))
            {
                ret = SaveInfoWgryHistory(userid, gcbh, false);
            }
            else
            {
                var q = from e in historys where e.OutTime == null select e;
                if (q.Count() == 0) //记录同时有进场记录和退场记录，表示该考勤为新进场
                    ret = SaveInfoWgryHistory(userid, gcbh, false);
                else //有进场,无退场表示在该项目工作
                    ret = true;
            }
            return true;
        }

        public bool SetInRyNum_GC(string gcbh, string sfzhm, string kqjlx)
        {
            bool code = true;
            string sql = "SELECT recid FROM ViewKqjUserLogDetail WHERE OutTime IS NULL AND DATEDIFF(dd,LogDay,getdate())=0 ";
            sql += " And projectid='" + gcbh + "'";
            sql += " and userid='" + sfzhm + "'";

            //没有设置班次
            //string sql1 = "SELECT recid FROM ViewKqjUserLogDetail WHERE  DATEDIFF(dd,LogDay,getdate())=0 ";
            //string where= " And projectid='" + gcbh + "' and userid='" + sfzhm + "'";
            //sql+=where;
            //IList<IDictionary<string, string>> kqlist = CommonDao.GetDataTable(sql1);
            //if (kqlist.Count == 0)
            //    return true;

            IList<IDictionary<string, string>> rylist = CommonDao.GetDataTable(sql);
            if (kqjlx == UserLogType.In)
            {
                if (rylist.Count == 0)
                {
                    sql = "update i_m_gc set inrynum=isnull(inrynum,0)+1 where gcbh='" + gcbh + "'";
                    code = CommonDao.ExecSql(sql);
                }
            }
            else if (kqjlx == UserLogType.Out)
            {
                if (rylist.Count > 0)
                {
                    sql = "update i_m_gc set inrynum=(case when isnull(inrynum,0)=0 then 0 else isnull(inrynum,0)-1 end) where gcbh='" + gcbh + "'";
                    code = CommonDao.ExecSql(sql);
                }
            }

            return code;
        }

        /// <summary>
        /// 保存务工人员历史记录
        /// </summary>
        /// <param name="itm"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveInfoWgryHistory(string sfz, string gcbh, bool isOut, DateTime? logdate = null)
        {
            // 获取本次考勤人员工程记录
            IList<InfoWgryHistory> historys = InfoWgryHistoryDao.Gets(sfz, "", gcbh);
            // 没有进工地记录，保存出工地记录，抛弃
            if ((historys == null || historys.Count == 0) && isOut)
                return false;
            // 获取人员信息
            IDictionary<string, string> inforow = CommonDao.GetRowValue("sfzhm,ryxm as realname,qybh as dwbh,bzfzr", "i_m_wgry", "sfzhm='" + sfz + "'and jdzch='" + gcbh + "' and hasdelete=0");
            if (inforow == null)
                return false;
            string sfzhm, realname, dwbh, dwmc, gcmc, bzfzr, bzfzrxm;
            inforow.TryGetValue("sfzhm", out sfzhm);
            inforow.TryGetValue("realname", out realname);
            inforow.TryGetValue("dwbh", out dwbh);
            inforow.TryGetValue("bzfzr", out bzfzr);
            dwmc = CommonDao.GetFieldValue("qymc", "i_m_qy", "qybh='" + dwbh + "' or qybh_yc='" + dwbh + "'");
            gcmc = CommonDao.GetFieldValue("gcmc", "i_m_gc", "gcbh='" + gcbh + "'");

            bzfzrxm = CommonDao.GetFieldValue("ryxm", "i_m_wgry", "sfzhm='" + bzfzr + "' and jdzch='" + gcbh + "'");
            bool ret = true;
            try
            {
                // 如果是进工地，查找未登记退出工地的记录，并新增入工地记录
                // 如果是出工地，查找未登记退出工地并且工地信息相同的记录
                if (isOut)
                {
                    var q = from e in historys where e.OutTime == null && e.ProjectId == gcbh && e.CompanyId == dwbh select e;
                    foreach (InfoWgryHistory hist in q)
                    {
                        if (logdate == null)
                            hist.OutTime = DateTime.Now;
                        else
                        {
                            if (logdate > DataFormat.GetSafeDate("2000-01-01"))
                                hist.OutTime = logdate;
                            else//没有考勤记录
                            {
                                TimeSpan span = DateTime.Now - hist.InTime.Value;
                                if (span.TotalDays > Convert.ToInt32(StringResource.CheckRyOut))
                                {
                                    hist.OutTime = hist.InTime.Value.AddDays(1);
                                }
                                else
                                    hist.OutTime = null;
                            }

                        }

                        InfoWgryHistoryDao.Save(hist);
                    }
                }
                else
                {

                    var q = from e in historys where e.OutTime == null select e;
                    bool InCurrGC = false;
                    foreach (InfoWgryHistory hist in q) //设置所有以前的工程的退场时间为今天
                    {
                        if (dwbh == hist.CompanyId && gcbh == hist.ProjectId) // 为目前所在工程
                        {
                            InCurrGC = true;
                        }
                        else
                        {
                            hist.OutTime = DateTime.Now;
                            InfoWgryHistoryDao.Save(hist);
                        }
                    }
                    if (!InCurrGC) //如果没有退场记录的工程为下发模板的工程，则不用执行
                    {
                        InfoWgryHistory histnew = new InfoWgryHistory()
                        {
                            Bzfzr = bzfzr,
                            BzfzrRealName = bzfzrxm,
                            CompanyName = dwmc,
                            CompanyId = dwbh,
                            InTime = DateTime.Now,
                            ProjectId = gcbh,
                            ProjectName = gcmc,
                            RealName = realname,
                            Sfzhm = sfzhm
                        };
                        InfoWgryHistoryDao.Save(histnew);
                    }

                }
            }
            catch (Exception e)
            {
                ret = false;
                SysLog4.WriteLog(e);
            }
            return ret;

        }

        public bool UpdateInfoUserMonthPay(string userid, string gcbh)
        {
            bool ret = false;
            try
            {
                string sql = "select * from InfoUserMonthPay where userid='" + userid + "' and projectid='" + gcbh + "' and DATEPART(year, getdate())= LogYear and DATEPART(month,getdate()) = LogMonth and havekqjl='2'";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    sql = "update InfoUserMonthPay set havekqjl='1' where userid='" + userid + "' and projectid='" + gcbh + "' and DATEPART(year, getdate())= LogYear and DATEPART(month,getdate()) = LogMonth and havekqjl='2'";
                    ret = CommonDao.ExecSql(sql);
                }
            }
            catch (Exception e)
            {

            }
            return true;
        }

        protected bool UpdateUserZT_GC(string userid, string qybh, string gcbh)
        {
            bool ret = false;
            try
            {
                string sql = "update i_m_wgry set hasdelete=0 where sfzhm='" + userid + "' and jdzch='" + gcbh + "'";
                ret = CommonDao.ExecSql(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 对不标明上下班考勤的机器，根据规则判断上下班
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        protected string GetUserLogType(KqjUserLog log)
        {
            string ret = "";
            try
            {
                IList<KqjUserLog> userlogs = KqjUserLogDao.Gets(log.UserId, log.PlaceId, log.LogDate.Value.Date);
                if (userlogs.Count == 0)
                    ret = UserLogType.In;
                else
                    ret = userlogs[userlogs.Count - 1].LogType == UserLogType.In ? UserLogType.Out : UserLogType.In;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 考勤机考勤的考勤记录状态
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="userid"></param>
        /// <param name="companyid"></param>
        /// <param name="jdzch"></param>
        /// <param name="time"></param>
        /// <param name="kqjlx"></param>
        /// <param name="qymc"></param>
        /// <param name="gcmc"></param>
        /// <returns></returns>
        public bool updateKqjUserLog(
           string serial, string userid, string companyid, string jdzch, DateTime time, string kqjlx, string qymc, string gcmc)
        {
            string sql = "Update kqjuserlog Set HasDeal = 'True',companyid='" + companyid + "',placeid='" + jdzch + "',DealType=1,LogType='" + kqjlx + "', companyname='" + qymc + "',projectname='" + gcmc + "' ";
            string where = "where serial='" + serial + "' and userid='" + userid + "' and logdate=convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "') and  HasDeal='False'";
            sql += where;
            return CommonDao.ExecSql(sql);
        }

        public bool updateWxkqlog(string userid, string companyid, string jdzch, DateTime time, string kqjlx, string qymc, string gcmc)
        {
            string sql = "Update kqjuserlog Set HasDeal = 'True',DealType=1,LogType='" + kqjlx + "', companyname='" + qymc + "',projectname='" + gcmc + "' ";
            string where = "where companyid='" + companyid + "' and placeid='" + jdzch + "' and userid='" + userid + "' and logdate=convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "') and  HasDeal='False'";
            sql += where;
            return CommonDao.ExecSql(sql);
        }

        /// <summary>
        /// 写考勤记录到月考勤详情
        /// </summary>
        /// <param name="log"></param>
        public void SaveUserMonthLog(KqjUserDayLog log, string gzid, string gzname, string logtype, DateTime tm)
        {
            KqjUserMonthLog mlog = KqjUserMonthLogDao.Get(log.UserId, log.CompanyId, log.GzId, tm.Year, tm.Month, log.ScheduleId); //参数去掉了bzfzr,添加了log.ScheduleId,
            if (mlog == null)
            {
                mlog = new KqjUserMonthLog()
                {
                    UserId = log.UserId,
                    ScheduleId = log.ScheduleId,
                    CompanyId = log.CompanyId,
                    GzId = gzid,
                    GzName = gzname,
                    Bzfzr = log.Bzfzr,
                    RealName = log.RealName,
                    CompanyName = log.CompanyName,
                    BzfzrRealName = log.BzfzrRealName,
                    LogYear = tm.Year,
                    LogMonth = tm.Month,
                    ProjectId = log.ProjectId,
                    ProjectName = log.ProjectName
                };
                KqjUserMonthLogDao.SaveLog(mlog);
            }
            KqjUserMonthLogDao.SetLog(mlog.Recid, tm.Day, logtype, tm);
            //更新表的考勤天数
            SaveUserMonthPay(log, gzid, gzname, tm, logtype);
        }

        /// <summary>
        /// 写考勤记录到月考勤支付表
        /// </summary>
        /// <param name="log"></param>
        public void SaveUserMonthPay(KqjUserDayLog log, string gzid, string gzname, DateTime tm, string logtype)
        {
            try
            {
                string fieldnum = "logdaynum";
                string fieldsum = "logdaysum";

                string day = tm.Day.ToString();

                fieldnum += day;
                fieldsum += day;

                string logday = tm.ToString("yyyy-MM-dd");
                string recid = "";
                int workday = 1;
                string sql = "select * from KqjUserMonthPay where userid=@userid and jdzch=@jdzch and LogYear=@logyear and logmonth=@logmonth";
                IList<IDataParameter> parameters = new List<IDataParameter>
                {
                    new SqlParameter("@userid",log.UserId),
                    new SqlParameter("@jdzch",log.ProjectId),
                    new SqlParameter("@logyear",tm.Year),
                    new SqlParameter("@logmonth",tm.Month)
                };
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql, CommandType.Text, parameters);
                if (dt.Count == 0)
                {
                    sql = "INSERT INTO KqjUserMonthPay([UserId],[RYXM],[QYBH] ,[JDZCH] ,[GzId],[GzName],[GW],[BzzUserId],[BzzRYXM],[LogYear] ,[LogMonth] ,[workday],[totalSum],[" + fieldsum + "],[" + fieldnum + "]) VALUES ('" + log.UserId + "','"
                        + log.RealName + "','"
                        + log.CompanyId + "','"
                        + log.ProjectId + "','"
                        + log.GzId + "','"
                        + gzname + "','"
                        + log.GW + "','"
                        + log.Bzfzr + "','"
                        + log.BzfzrRealName + "','"
                        + tm.Year + "','"
                        + tm.Month + "','"
                        + workday.GetSafeInt() + "','"
                        + log.RealSum + "','"
                        + log.RealSum + "','1')";
                    CommonDao.ExecSql(sql);
                }
                else //该年该月该人员已考勤过
                {
                    recid = dt[0]["recid"].ToString();
                    string field_num = "0";
                    string field_sum = "";
                    dt[0].TryGetValue(fieldnum, out field_num);
                    dt[0].TryGetValue(fieldsum, out field_sum);
                    if (field_num == "") //当天没有考勤过
                    {
                        workday = Convert.ToInt32(dt[0]["workday"].GetSafeDecimal());
                        workday += 1;
                        decimal? totalSum = dt[0]["totalsum"].GetSafeDecimal();
                        totalSum += log.RealSum;

                        sql = "update KqjUserMonthPay set workday=" + workday.GetSafeString() + "," + fieldnum + "=1," + fieldsum + "=0 where Recid=" + recid;
                        CommonDao.ExecSql(sql); //更新考勤天数
                    }
                    if (log.RealSum != 0)
                    {
                        decimal? totalSum = dt[0]["totalsum"].GetSafeDecimal();//monthpay表中totalsum 月总工数
                        sql = "select sum(realsum) as worksum from kqjuserdaylog where userid=@userid and projectid=@projectid and datediff(dd,@logday,logday)=0";
                        IList<IDataParameter> param = new List<IDataParameter>
                        {
                            new SqlParameter("@userid",log.UserId),
                            new SqlParameter("@projectid",log.ProjectId),
                            new SqlParameter("@logday",logday)
                        };
                        IList<IDictionary<string, string>> list = CommonDao.GetDataTable(sql, CommandType.Text, param);
                        if (list.Count > 0)
                        {
                            decimal s_fieldsum = list[0]["worksum"].GetSafeDecimal(); //当天所有班次的工数之和
                            if (field_sum.GetSafeDecimal() != s_fieldsum) //monthpay表中field_sum 不等于 新计算出后的天工数和             
                            {
                                totalSum = totalSum - field_sum.GetSafeDecimal() + s_fieldsum;
                                sql = "update KqjUserMonthPay set totalSum='" + totalSum.ToString() + "'," + fieldsum + "= '" + s_fieldsum + "' where Recid=" + recid;
                                CommonDao.ExecSql(sql); //更新月工数
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            { }

        }

        /// <summary>
        /// 上班考勤时完成考勤情况
        /// </summary>
        /// <param name="daylog"></param>
        /// <param name="daylogdetail"></param>
        protected void SetUserDayLogIn(InfoSchedule schedule, KqjUserLog log,
           ref KqjUserDayLog daylog, ref KqjUserDayLogDetail daylogdetail, IList<InfoSchedule> schedules)
        {
            try
            {
                if (daylogdetail.OutTime.Value.Subtract(log.LogDate.Value).TotalHours > 18) //出工地时间-进工地时间>18小时            
                {
                    return;
                }
                daylogdetail.InTime = log.LogDate;
                daylog.RealMinutes += (int)(daylogdetail.OutTime.Value.Subtract(daylogdetail.InTime.Value).TotalMinutes);
                //daylog.RealSum = daylog.SetSum = DataFormat.GetSafeDecimal(schedule.ScheduleSum)*(daylog.RealMinutes >= daylog.ShouldMinutes ? 1 : ((decimal)daylog.RealMinutes.Value/daylog.ShouldMinutes.Value));

                daylog.RealSum += GetRealSumIn(schedule, log, daylog, daylogdetail, schedules, DataFormat.GetSafeInt(schedule.LjTime, 20));
                daylog.SetSum = daylog.RealSum;
                if (daylog.ScheduleId != "")
                {
                    InfoGzgz gzgz = InfoGzgzDao.Get(log.CompanyId, log.PlaceId, daylog.ScheduleId, daylog.GzId);
                    if (gzgz != null)
                        daylog.RealPay = daylog.SetPay = DataFormat.GetSafeDecimal(gzgz.PaySum) * daylog.RealSum;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
        }

        /// <summary>
        /// 上班考勤时完成考勤情况
        /// </summary>
        /// <param name="daylog"></param>
        /// <param name="daylogdetail"></param>
        protected void SetUserDayLogIn(InfoSchedule schedule, KqjUserLog log, KqjUserDayLog daylog)
        {
            try
            {
                daylog.RealSum = daylog.SetSum = DataFormat.GetSafeDecimal(schedule.ScheduleSum);
                if (schedule.ScheduleSum == "" || schedule.ScheduleSum == "0")
                    SysLog4.WriteError("schedule.ScheduleSum:" + schedule.ScheduleSum + "[schedule.recid]" + schedule.Recid);
                if (daylog.ScheduleId != "")
                {
                    InfoGzgz gzgz = InfoGzgzDao.Get(log.CompanyId, log.PlaceId, daylog.ScheduleId, daylog.GzId);
                    if (gzgz != null)
                        daylog.RealPay = daylog.SetPay = DataFormat.GetSafeDecimal(gzgz.PaySum) * daylog.RealSum;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.ToString());
            }
        }

        /// <summary>
        /// 从排班、工种、用户考勤日志，获取新的用户日考勤记录
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="gzid"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        protected KqjUserDayLog GetNewDayLog(InfoSchedule schedule, string ryxm, string gzid, string gw, string bzfzr, string bzfzrxm, KqjUserLog log)
        {
            return new KqjUserDayLog()
            {
                CompanyName = log.CompanyName,
                ProjectName = log.ProjectName,
                CompanyId = log.CompanyId,
                LogDay = log.LogDate.Value.Date,
                ProjectId = log.PlaceId,
                UserId = log.UserId,
                RealName = ryxm,
                GzId = gzid,
                GW = gw,
                Bzfzr = bzfzr,
                BzfzrRealName = bzfzrxm,
                ScheduleId = schedule.Recid,
                ShouldMinutes = (int)(TimeSpanOperation.GetEndTime(schedule.StartTime, schedule.EndTime).Subtract(TimeSpanOperation.GetTimeSpan(schedule.StartTime)).TotalMinutes) - schedule.FreeTime.Value,
                RealMinutes = 0,
                //RealPay = 0,
                RealSum = 0,
                //SetPay = 0,
                SetSum = 0
            };
        }

        /// <summary>
        /// 从考勤时间获取新的考勤详情
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected KqjUserDayLogDetail GetNewDayLogDetail(DateTime dt)
        {
            return new KqjUserDayLogDetail()
            {
                InTime = dt,
                OutTime = null,
                ParentId = 0
            };
        }

        protected KqjUserDayLogDetail GetNewDayLogOutDetail(DateTime dt)
        {
            return new KqjUserDayLogDetail()
            {
                InTime = null,
                OutTime = dt,
                ParentId = 0
            };
        }

        /// <summary>
        /// 下班考勤时完成考勤情况
        /// </summary>
        /// <param name="daylog"></param>
        /// <param name="daylogdetail"></param>
        protected void SetUserDayLogOut(InfoSchedule schedule, KqjUserLog log,
           ref KqjUserDayLog daylog, ref KqjUserDayLogDetail daylogdetail, IList<InfoSchedule> schedules, out  string msg)
        {
            msg = "";
            try
            {
                IList<KqjUserDayLogDetail> daylogdetails = KqjUserDayLogDetailDao.Gets(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date);

                //考勤时间大于最大时间，表示异常出工地
                //if (daylogdetails[0].InTime != daylogdetail.InTime && daylogdetails[0].OutTime < log.LogDate && daylogdetails[0].OutTime > daylogdetails[0].InTime)
                //{
                //    daylogdetail = null;
                //    return;
                //}

                for (int i = daylogdetails.Count - 1; i >= 0; i--)
                {
                    if (daylogdetails[i].InTime > daylogdetail.InTime && daylogdetails[i].InTime < log.LogDate && daylogdetails[i].OutTime > log.LogDate && daylogdetails[i].OutTime > daylogdetails[i].InTime)
                    {
                        daylogdetail = null;
                        msg = "无效的考勤";
                        return;
                    }
                    //后面的daylogdetail 进时间晚于找到的进时间
                    if (daylogdetails[i].InTime > daylogdetail.InTime && daylogdetails[i].OutTime < log.LogDate && daylogdetails[i].OutTime > daylogdetails[i].InTime)
                    {
                        daylogdetail = null;
                        msg = "无效的考勤,在有效考勤区间内";
                        return;
                    }
                }

                if (log.LogDate.Value.Subtract(daylogdetail.InTime.Value).TotalHours > 18) //出工地时间-进工地时间>18小时
                {
                    daylogdetail.OutTime = daylogdetail.InTime;
                    KqjUserDayLogDetailDao.Save(daylogdetail);
                    daylogdetail = null;
                    IList<KqjUserDayLog> daylogs = KqjUserDayLogDao.GetDayLogs(log.UserId, log.CompanyId, log.PlaceId, log.LogDate.Value.Date, schedule);
                    if (daylogs.Count == 0)
                        daylog = null;
                    else
                        daylog = daylogs.First();
                    return;
                }
                daylogdetail.OutTime = log.LogDate;
                daylog.RealMinutes += (int)(daylogdetail.OutTime.Value.Subtract(daylogdetail.InTime.Value).TotalMinutes);
                //daylog.RealSum = daylog.SetSum = DataFormat.GetSafeDecimal(schedule.ScheduleSum)*(daylog.RealMinutes >= daylog.ShouldMinutes ? 1 : ((decimal)daylog.RealMinutes.Value/daylog.ShouldMinutes.Value));

                daylog.RealSum += GetRealSum(schedule, log, daylog, daylogdetail, schedules, DataFormat.GetSafeInt(schedule.LjTime, 20));
                daylog.SetSum = daylog.RealSum;
                if (daylog.ScheduleId != "")
                {
                    InfoGzgz gzgz = InfoGzgzDao.Get(log.CompanyId, log.PlaceId, daylog.ScheduleId, daylog.GzId);
                    if (gzgz != null)
                        daylog.RealPay = daylog.SetPay = DataFormat.GetSafeDecimal(gzgz.PaySum) * daylog.RealSum;
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        protected decimal? GetRealSum(InfoSchedule schedule, KqjUserLog log,
          KqjUserDayLog daylog, KqjUserDayLogDetail daylogdetail, IList<InfoSchedule> schedules, int checkmins)
        {
            decimal ScheduleSum = DataFormat.GetSafeDecimal(schedule.ScheduleSum);
            int WorklMinutes1 = 0;
            int WorklMinutes2 = 0;
            DateTime? outTime = log.LogDate;
            DateTime endTime = DataFormat.GetSafeDate(log.LogDate.Value.ToShortDateString() + " " + schedule.EndTime);
            InfoSchedule nextSchedule = null;
            if (outTime.Value > endTime) //出工地考勤时间在上一个区间外
            {
                WorklMinutes1 += (int)(endTime.Subtract(daylogdetail.InTime.Value).TotalMinutes);
                WorklMinutes2 += (int)(outTime.Value.Subtract(endTime).TotalMinutes);
                var sch = schedules.Where(t => (DataFormat.GetSafeDate(t.StartTime) > DataFormat.GetSafeDate(schedule.StartTime))).OrderBy(t => t.StartTime);
                if (sch.Count() > 0)
                {
                    nextSchedule = sch.First();
                }
            }
            else
            {
                WorklMinutes1 += (int)(outTime.Value.Subtract(daylogdetail.InTime.Value).TotalMinutes);
            }
            int numpart1 = WorklMinutes1 / 60;
            decimal otherpart1 = WorklMinutes1 % 60;

            decimal result = 0;

            if (otherpart1 > checkmins)  //余数大于临界值
                result = ScheduleSum * (numpart1 + 1);
            else
                result = ScheduleSum * numpart1;

            int numpart2 = 0;
            decimal otherpart2 = 0;
            if (WorklMinutes2 != 0 && nextSchedule != null) //夸区间
            {
                numpart2 = WorklMinutes2 / 60;
                otherpart2 = WorklMinutes2 % 60;
                if (otherpart2 > checkmins)
                    result += DataFormat.GetSafeDecimal(nextSchedule.ScheduleSum) * (numpart2 + 1);
                else
                    result += DataFormat.GetSafeDecimal(nextSchedule.ScheduleSum) * (numpart2);
            }
            else //在同一区间内
            {

            }
            if (result < 0)
                result = 0;
            return result;
        }
        //通过进工地时间 设置工数
        protected decimal? GetRealSumIn(InfoSchedule schedule, KqjUserLog log,
            KqjUserDayLog daylog, KqjUserDayLogDetail daylogdetail, IList<InfoSchedule> schedules, int checkmins)
        {
            decimal ScheduleSum = DataFormat.GetSafeDecimal(schedule.ScheduleSum);
            int WorklMinutes1 = 0;
            int WorklMinutes2 = 0;
            DateTime? inTime = daylogdetail.InTime;
            DateTime StartTime = DataFormat.GetSafeDate(log.LogDate.Value.ToShortDateString() + " " + schedule.StartTime);
            InfoSchedule nextSchedule = null;
            if (inTime.Value < StartTime) //进工地考勤时间在上一个区间外
            {
                WorklMinutes1 += (int)(daylogdetail.OutTime.Value.Subtract(StartTime).TotalMinutes); //区间内
                WorklMinutes2 += (int)(StartTime.Subtract(inTime.Value).TotalMinutes); //外
                var sch = schedules.Where(t => (DataFormat.GetSafeDate(t.StartTime) > DataFormat.GetSafeDate(schedule.StartTime))).OrderBy(t => t.StartTime);
                if (sch.Count() > 0)
                {
                    nextSchedule = sch.First();
                }
            }
            else
            {
                WorklMinutes1 += (int)(daylogdetail.OutTime.Value.Subtract(inTime.Value).TotalMinutes);
            }
            int numpart1 = WorklMinutes1 / 60;
            decimal otherpart1 = WorklMinutes1 % 60;

            decimal result = 0;

            if (otherpart1 > checkmins)  //余数大于临界值
                result = ScheduleSum * (numpart1 + 1);
            else
                result = ScheduleSum * numpart1;

            int numpart2 = 0;
            decimal otherpart2 = 0;
            if (WorklMinutes2 != 0 && nextSchedule != null) //夸区间
            {
                numpart2 = WorklMinutes2 / 60;
                otherpart2 = WorklMinutes2 % 60;
                if (otherpart2 > checkmins)
                    result += DataFormat.GetSafeDecimal(nextSchedule.ScheduleSum) * (numpart2 + 1);
                else
                    result += DataFormat.GetSafeDecimal(nextSchedule.ScheduleSum) * (numpart2);
            }
            else //在同一区间内
            {

            }
            if (result < 0)
                result = 0;
            return result;
        }


        #endregion

        #endregion

        /// <summary>
        /// 当前工程人员统计
        /// </summary>
        /// <param name="gcid"></param>
        /// <returns>当前共××人，其中××多少人</returns>
        public IList<IDictionary<string, string>> GetCurrrentInWgryStatistic(string gcid, string gcbh_yc)
        {
            string where = "";
            where = " and projectid='" + gcid + "'";
            // 根据工程获取单位
            string dwid = "";
            string sql = "select qybh from i_s_gc_sgdw where gcbh='" + gcid + "'";
            IList<IDictionary<string, string>> sgdws = CommonDao.GetDataTable(sql);

            if (sgdws != null)
                dwid = sgdws[0]["qybh"].ToString();
            where += " and logday>=convert(datetime,'" + DateTime.Now.ToShortDateString() + "') ";

            ///获取当前属于哪个班次
            IList<InfoSchedule> schedules = InfoScheduleDao.Gets(dwid, gcid);
            InfoSchedule schedule = null;
            string scheduleid = "";
            var q = schedules.Where(t => TimeSpanOperation.IsTimeIn(t.StartTime, t.EndTime, DateTime.Now));
            if (q.Count() > 0)
            {
                schedule = q.First();
            }
            else
            {
                q = from e in schedules where TimeSpanOperation.GetTimeSpan(e.StartTime).TotalMinutes > DateTime.Now.TimeOfDay.TotalMinutes orderby TimeSpanOperation.GetTimeSpan(e.StartTime).Subtract(DateTime.Now.TimeOfDay) ascending select e;
                if (q.Count() > 0)
                    schedule = q.First();
            }
            string tablename = "";
            if (schedule != null)
            {
                scheduleid = schedule.Recid;
                // where += " and scheduleid ='" + scheduleid + "'";

                //if (schedule.KqTimes == 2) //双向考勤
                //{
                //    tablename = "ViewCurrentWorkerTwoTimes";
                //}
                //else
                tablename = "ViewWgryCurrentWorker"; //当前单、双向都是这视图

            }
            else
                tablename = "ViewWgryCurrentWorker"; //当前单、双向都是这视图

            //if (sgdws != null && sgdws.Count() > 0)
            //{
            //    sgdws[0].TryGetValue("dwbh", out dwid);
            //    if (dwid != "")
            //    {
            //        SysUserSetting setting = SysUserSettingDao.Get(dwid, SysUserSettingItem.KeyInUserSetting);
            //        if (setting != null)
            //        {
            //            int days = DataFormat.GetSafeInt(setting.Settingvalue);
            //            if (days != -1)
            //                where = " and a.logdate>=convert(datetime,'" + DateTime.Now.AddDays(days * -1).ToShortDateString() + "') ";
            //        }
            //    }
            //}
            where += " and gzname!='非实名制登记人员' ";
            sql = "select * from " + tablename + " where 1=1 " + where;
            sql += " ORDER BY XSSX ";
            //sql = "select b.gz,count(*) as sum1 from kqjuserlog a inner join infowgry b on a.userid=b.sfzhm where  a.projectid='" + gcid + "' and a.recid in (select max(recid) from KqjUserLog group by userid) and a.logtye='1' "+where+" group by b.gz";

            return CommonDao.GetDataTable(sql);

        }

        #region 工资册轮询，判断是否有未填写的人员
        /// <summary>
        /// 获取工程列表
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGClist()
        {
            string sql = "select gcbh,gcmc from i_m_gc";
            return CommonDao.GetDataTable(sql);
        }




        public IList<IDictionary<string, string>> GetYHGClist()
        {
            string sql = "select gcbh,gcmc from I_S_GC_YH";
            return CommonDao.GetDataTable(sql);
        }
        [Transaction(ReadOnly = false)]
        public bool SetGzcYJ(string gcbh, string gcmc, DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            string logyear, logmonth;
            if (month == 1)
            {
                logyear = (year - 1).ToString();
                logmonth = "12";
            }
            else
            {
                logyear = year.ToString();
                logmonth = (month - 1).ToString();
            }

            //把有考勤，没有设置工资的人员的tx_yjzt状态置1
            //string where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and (havepay is null or havepay =0) and totalsum>0";
            string where_sql = "select recid from kqjusermonthpay where  logyear='" + logyear + "' and logmonth='" + logmonth + "' and (havepay is null or havepay =0) and totalsum>0";
            string sql = "update kqjusermonthpay set tx_yjzt=1 where recid in (" + where_sql + ") ";
            //sqls.Add(sql);
            CommonDao.ExecSqlTran(sql);
            //把有考勤，设置工资的人员的tx_yjzt状态置0
            //where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and havepay is not null and havepay <>0 and totalsum>0";
            where_sql = "select recid from kqjusermonthpay where  logyear='" + logyear + "' and logmonth='" + logmonth + "' and havepay is not null and havepay <>0 and totalsum>0";
            sql = "update kqjusermonthpay set tx_yjzt=0 where recid in (" + where_sql + ")";
            //sqls.Add(sql);
            CommonDao.ExecSqlTran(sql);

            ////把应发和实发不同的人员的ff_yjzt状态置1,tx_yjzt=0
            // where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and bankpay is not null and bankpay <>0 and bankpay!=havepay ";
            where_sql = "select recid from kqjusermonthpay where  logyear='" + logyear + "' and logmonth='" + logmonth + "' and bankpay is not null and bankpay <>0 and bankpay!=havepay ";
            sql = "update kqjusermonthpay set ff_yjzt=1,tx_yjzt=0 where recid in (" + where_sql + ")";
            //sqls.Add(sql);
            CommonDao.ExecSqlTran(sql);
            ////把应发和实发相同同的人员的ff_yjzt状态置0,tx_yjzt=0
            //where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and bankpay is not null and bankpay <>0 and bankpay=havepay ";
            where_sql = "select recid from kqjusermonthpay where logyear='" + logyear + "' and logmonth='" + logmonth + "' and bankpay is not null and bankpay <>0 and bankpay=havepay ";
            sql = "update kqjusermonthpay set ff_yjzt=0,tx_yjzt=0 where recid in (" + where_sql + ")";
            //sqls.Add(sql);
            CommonDao.ExecSqlTran(sql);
            /*
            string msg = "";
            //ExecTrans(sqls,out msg);

            sql = "select TX_YJZT, FF_YJZT from ViewKqjUserMonthPay where TX_YJZT=1";
            IList<IDictionary<string,string>> yjdatas=CommonDao.GetDataTable(sql);
            if(yjdatas.Count>0)
            {
                msg += "工程人员进行了考勤，没有填写工资册;";
            }

            sql = "select TX_YJZT, FF_YJZT from ViewKqjUserMonthPay where FF_YJZT=1";
            yjdatas = CommonDao.GetDataTable(sql);
            if (yjdatas.Count > 0)
            {
                msg += "工程人员应发工资与实发工资不符合;";
            }
            if(msg!="")
            {
                sql="select * from H_SMS_SJHM where RYLX='1' and XQDM='TZ'";
                IList<IDictionary<string,string>> sjdatas=CommonDao.GetDataTable(sql);
                string sjhms="";
                if(sjdatas.Count>0)
                {
                    sjhms=sjdatas[0]["sjhm"];                
                    SaveSMSMsg(sjhms,"有预警信息:"+msg,"ryyj");
                }
             
            }*/
            return true;
        }

        public bool SetGzcYJErrInfo(string gcbh, string gcmc, DateTime dt)
        {
            string msg = "";

            string sql = "select TX_YJZT, FF_YJZT from ViewKqjUserMonthPay where TX_YJZT=1";
            IList<IDictionary<string, string>> yjdatas = CommonDao.GetDataTable(sql);
            if (yjdatas.Count > 0)
            {
                msg += "工程人员进行了考勤，没有填写工资册;";
            }

            sql = "select TX_YJZT, FF_YJZT from ViewKqjUserMonthPay where FF_YJZT=1";
            yjdatas = CommonDao.GetDataTable(sql);
            if (yjdatas.Count > 0)
            {
                msg += "工程人员应发工资与实发工资不符合;";
            }
            if (msg != "")
            {
                sql = "select * from H_SMS_SJHM where RYLX='1' and XQDM='TZ'";
                IList<IDictionary<string, string>> sjdatas = CommonDao.GetDataTable(sql);
                string sjhms = "";
                if (sjdatas.Count > 0)
                {
                    sjhms = sjdatas[0]["sjhm"];
                    SaveSMSMsg(sjhms, "有预警信息:" + msg, "ryyj");
                }

            }
            return true;
        }
        public bool SaveSMSMsg(string sjhms, string msg, string LX)
        {
            bool code = false;
            try
            {
                string guid = Guid.NewGuid().ToString();
                string sql = "INSERT INTO info_sms   ([Guid],[Phone] ,[Message],[HasDeal] ,[LX]) values ('" + guid + "','" +
                    sjhms + "','" + msg + "',0,'" + LX + "')";
                code = CommonDao.ExecSql(sql);
            }
            catch (Exception e)
            {
                code = false;
            }
            return code;
        }

        public bool SetGzcYJ2(string gcbh, string gcmc, DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            string logyear, logmonth;
            if (month == 1)
            {
                logyear = (year - 1).ToString();
                logmonth = "12";
            }
            else
            {
                logyear = year.ToString();
                logmonth = (month - 1).ToString();
            }

            //把没有设置工资的人员的tx_yjzt状态置1
            string where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and (havepay is null or havepay =0) and totalsum>0 ";
            string sql = "update kqjusermonthpay set tx_yjzt=1 where recid in (" + where_sql + ") ";
            CommonDao.ExecSql(sql);
            //把设置工资的人员的tx_yjzt状态置0
            where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and havepay is not null and havepay <>0";
            sql = "update kqjusermonthpay set tx_yjzt=0 where recid in (" + where_sql + ")";
            CommonDao.ExecSql(sql);

            ////把应发和实发不同的人员的ff_yjzt状态置1,tx_yjzt=0
            where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and bankpay is not null and bankpay <>0 and bankpay!=havepay ";
            sql = "update kqjusermonthpay set ff_yjzt=1,tx_yjzt=0 where recid in (" + where_sql + ")";
            CommonDao.ExecSql(sql);
            ////把应发和实发相同同的人员的ff_yjzt状态置0,tx_yjzt=0
            where_sql = "select recid from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + logyear + "' and logmonth='" + logmonth + "' and bankpay is not null and bankpay <>0 and bankpay=havepay ";
            sql = "update kqjusermonthpay set ff_yjzt=0,tx_yjzt=0 where recid in (" + where_sql + ")";
            CommonDao.ExecSql(sql);

            return true;

        }
        //[Transaction(ReadOnly=false)] select和update都有，会锁表
        public bool SetYHYEYJ(string gcbh, string gcmc, DateTime dt)
        {
            bool ret = false;
            string msg = "";
            try
            {
                string zhye = "";
                string gxsj = "";
                bool xzdw = true;
                bool xzze = true;
                IDictionary<string, string> yhrow = CommonDao.GetRowValue("zhye,gxsj", "I_S_GC_YH", "gcbh='" + gcbh + "'");
                if (yhrow == null)
                    return false;
                yhrow.TryGetValue("zhye", out zhye);
                yhrow.TryGetValue("gxsj", out gxsj);
                DateTime yh_gxsj = gxsj.GetSafeDate();
                if (dt.Year > yh_gxsj.Year) //更新时间为去年
                {
                    xzdw = false;
                    msg = gcmc + "的薪资发放存款账户本月没有转入存款";
                }
                else if (dt.Year == yh_gxsj.Year && dt.Month > yh_gxsj.Month) //更新时间为今年且月份小于当月
                {
                    xzdw = false;
                    msg = gcmc + "的薪资发放存款账户本月没有转入存款";
                }

                string sql = "select sum(havepay) as totalpay from kqjusermonthpay where jdzch='" + gcbh + "' and logyear='" + dt.Year.ToString() + "' and logmonth='" + dt.Month.ToString() + "' and bankpay is null ";
                IList<IDictionary<string, string>> mpaydatas = CommonDao.GetDataTable(sql);
                if (mpaydatas.Count > 0)
                {
                    string totalpay = "1000000000";// mpaydatas[0]["totalpay"];
                    if (zhye.GetSafeDecimal() < totalpay.GetSafeDecimal()) //银行卡余额小于应发的工资数
                    {
                        xzze = false;
                        msg = gcmc + "的薪资发放存款不足以发放当月工资";
                    }
                }

                IList<IDictionary<string, string>> yjrow = CommonDao.GetDataTable("select xzdw,xzze from info_yj_xz where gcbh='" + gcbh + "' and logyear='" + dt.Year + "' and logmonth='" + dt.Month + "'");
                if (yjrow.Count > 0)
                {
                    bool s_xzdw = yjrow[0]["xzdw"] == "True" ? true : false;
                    bool s_xzze = yjrow[0]["xzze"] == "True" ? true : false;
                    if (xzdw != s_xzdw || xzze != s_xzze)
                    {
                        sql = "update info_yj_xz set  xzdw='" + xzdw + "',xzze='" + xzze + "' where gcbh='" + gcbh + "' and logyear='" + dt.Year + "' and logmonth='" + dt.Month + "'";
                        ret = CommonDao.ExecSql(sql);
                    }
                }
                else if (!xzdw || !xzze) //有异常
                {
                    sql = "insert into info_yj_xz (yjlx,gcbh,gcmc,xzdw,xzze,logyear,logmonth) values('1','" + gcbh + "','" + gcmc + "','" + xzdw + "','" + xzze + "','" + dt.Year + "','" + dt.Month + "')";
                    ret = CommonDao.ExecSql(sql);

                    sql = "select * from H_SMS_SJHM where RYLX='1' and XQDM='TZ'";
                    //IList<IDictionary<string,string>> sjdatas=CommonDao.GetDataTable(sql);
                    //string sjhms="";
                    //if(sjdatas.Count>0)
                    //{
                    //    sjhms=sjdatas[0]["sjhm"];                
                    //    SaveSMSMsg(sjhms,"有预警信息:"+msg,"xzyj");
                    //}
                }
                sql = "select * from H_SMS_SJHM where RYLX='1' and XQDM='TZ'";
                IList<IDictionary<string, string>> sjdatas = CommonDao.GetDataTable(sql);

                if (sjdatas.Count > 0)
                {
                    string sjhms = sjdatas[0]["sjhm"];
                    SaveSMSMsg(sjhms, "有预警信息:" + msg, "xzyj");
                }

            }
            catch (Exception e)
            {

            }
            return ret;
        }
        #endregion

        #region 自动设置出工地
        public void UpdateSgryOutSchedule(string checkTime)
        {
            //string sql="Update KqjUserDayLogDetail SET OutTime=InTime where Recid='"+recid+"'";

            string sql = "Update KqjUserDayLogDetail SET OutTime=InTime where outtime is null and Recid in ( ";
            sql += "SELECT detail_recid FROM ViewKqjUserLogDetail WHERE OutTime IS NULL AND kqtimes = 2 ";
            sql += "AND InTime < '" + checkTime + "' ";
            sql += ") ";
            bool code = CommonDao.ExecSql(sql);
            //  if (code)
            SetGCryInNum();
        }
        /// <summary>
        /// 重置在场人数为0
        /// </summary>
        private void SetGCryInNum()
        {
            // string sql = "update i_m_gc set inrynum=0"; //所有工程在场人数清零
            //CommonDao.ExecSql(sql);
            string sql = "select sum(num) as rynum,projectid from ViewWgryCurrentWorker where (DATEDIFF(dd,logday,GETDATE())=0) group by projectid "; //or DATEDIFF(dd,logday,GETDATE())=1
            IList<IDictionary<string, string>> sumlist = CommonDao.GetDataTable(sql);
            string wheregc = "";
            for (int i = 0; i < sumlist.Count; i++)
            {
                string gcbh = sumlist[i]["projectid"];
                wheregc += gcbh + ",";
                string rynum = sumlist[i]["rynum"];
                sql = "update i_m_gc set inrynum='" + rynum + "' where gcbh='" + gcbh + "'";
                CommonDao.ExecSql(sql);
            }
            wheregc = wheregc.FormatSQLInStr();

            sql = "update i_m_gc set inrynum=0 where gcbh not in (" + wheregc + ")";
            CommonDao.ExecSql(sql);

            ////////////////////
            //sql = "select gcbh from i_m_gc";
            //IList<IDictionary<string, string>> gclist = CommonDao.GetDataTable(sql);
            //for (int i = 0; i < gclist.Count; i++)
            //{
            //    string gcbh = gclist[i]["gcbh"];
            //    sql = "select count(1) from ViewKqjUserLogDetail where projectid='" + gcbh + "' and OutTime is null";

            //}
        }
        #endregion

        /// <summary>
        /// 获取工程人员考勤机
        /// </summary>
        /// <param name="sfzhm"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetRyGcKqj(string sfzhm)
        {
            if (sfzhm.Length == 0)
                return null;
            // string sql = "select * from i_m_kqj where jdzch in (select jdzch from i_m_wgry where sfzhm='" + sfzhm + "')";
            string sql = "select * from i_m_kqj where jdzch = '" + CurrentUser.Jdzch + "'";
            IList<IDictionary<string, string>> list = CommonDao.GetDataTable(sql);
            return list;

        }

        public IList<IDictionary<string, string>> GetGcKqj(string gcbh)
        {
            string sql = "select * from i_m_kqj where jdzch = '" + gcbh + "'";
            IList<IDictionary<string, string>> list = CommonDao.GetDataTable(sql);
            return list;

        }

        public IList<IDictionary<string, string>> GetYCGcKqj(string gcbh)
        {
            string sql = "select * from View_I_M_KQJ where gcbh_yc = '" + gcbh + "'";
            IList<IDictionary<string, string>> list = CommonDao.GetDataTable(sql);
            return list;

        }

        /// <summary>
        /// 人员自动退出
        /// </summary>
        /// <param name="gcbh"></param>
        public void UpdateSgryOutDay(string gcbh)
        {
            try
            {
                string sql = "select max(logdate)as logday,userid from kqjuserlog where placeid=@placeid group by userid";
                List<IDataParameter> parameters = new List<IDataParameter>
                {               
                    new SqlParameter("@placeid",gcbh)
                };
                List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                int checkday = Convert.ToInt32(Configs.GetConfigItem("ryoutday"));
                for (int i = 0; i < list.Count; i++)
                {
                    DateTime logday = DataFormat.GetSafeDate(list[i]["logday"]);
                    string sfzhm = list[i]["userid"];
                    if (JudgeDay(logday, checkday)) //距离上次考勤已过15天,设置退场
                    {
                        DateTime outday = logday.AddDays(1);
                        if (SaveInfoWgryHistoryOutDay(gcbh, sfzhm, true, outday))
                        {
                            if (logday > DataFormat.GetSafeDate("2000-01-01")) //有考勤
                                UpdateSgryLeave(sfzhm, gcbh);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            finally
            {

            }
        }

        public bool JudgeDay(DateTime logday, int day)
        {
            try
            {
                TimeSpan span = DateTime.Now - logday;
                if (span.TotalDays > day)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 保存务工人员历史记录(自动退出)
        /// </summary>
        /// <param name="itm"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveInfoWgryHistoryOutDay(string gcbh, string sfz, bool isOut, DateTime? logdate = null)
        {
            // 获取人员工程记录
            IList<InfoWgryHistory> historys = InfoWgryHistoryDao.GetsOut(sfz, "", gcbh);
            // 没有进工地记录，保存出工地记录，抛弃
            if ((historys == null || historys.Count == 0) && isOut)
                return false;
            // 获取人员信息

            bool ret = false;
            try
            {
                // 如果是进工地，查找未登记退出工地的记录，并新增入工地记录
                // 如果是出工地，查找未登记退出工地并且工地信息相同的记录
                if (isOut)
                {
                    //  var q = from e in historys where e.OutTime == null && e.ProjectId == gcbh select e;
                    foreach (InfoWgryHistory hist in historys)
                    {
                        if (logdate == null)
                            hist.OutTime = DateTime.Now;
                        else
                        {
                            if (logdate > DataFormat.GetSafeDate("2000-01-01"))
                            {
                                hist.OutTime = logdate;
                                if (hist.InTime > logdate)
                                    hist.OutTime = hist.InTime.Value.AddDays(1);
                            }
                            else//没有考勤记录
                            {
                                TimeSpan span = DateTime.Now - hist.InTime.Value;
                                if (span.TotalDays > Convert.ToInt32(Configs.GetConfigItem("ryoutday")))
                                {
                                    hist.OutTime = hist.InTime.Value.AddDays(1);
                                }
                                else
                                    hist.OutTime = null;
                            }

                        }

                        InfoWgryHistoryDao.Save(hist);
                        ret = true;
                    }
                }
            }
            catch (Exception e)
            {
                ret = false;
                SysLog4.WriteError(e.Message);
            }
            return ret;

        }

        public void UpdateSgryLeave(string sfzhm, string gcbh)
        {
            try
            {
                // string sql1 = "INSERT into InfoWgry_leave select * from infowgry a WHERE a.SFZHM='" + sfzhm + "' and a.sfzhm not in (select b.sfzhm from InfoWgry_leave b where b.gcbh='" + gcbh + "' and b.sfzhm='" + sfzhm + "')";
                string sql2 = "UPDATE I_M_WGRY set HasDelete='1' WHERE SFZHM='" + sfzhm + "' and JDZCH='" + gcbh + "' ";
                IList<string> sqls = new List<string>();

                sqls.Add(sql2);
                CommonDao.ExecSql(sql2);
            }
            catch (Exception e)
            { }
        }

        #region 工资
       /// <summary>
       /// 获取需推送的工资册命令
       /// </summary>
       /// <returns></returns>
        public IList<IDictionary<string, string>> GetWgryPaylist()
        {
            string sql = "select * from INFO_M_XZFF where sptg=1 and TSZT=0";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }

        public IList<IDictionary<string, string>> GetWgryYHBGlist()
        {
            string sql = "select * from I_S_GC_WGRYYHBG where hasdeal=0 ";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }
        /// <summary>
        /// 推送工资册
        /// </summary>
        public bool SetPayroll(string rguid)
        {
            bool code = true;
            string msg = "";
            Payroll payroll = new Payroll();
            string key = MD5Util.StringToMD5Hash(WGRYPAY_KEY);
            try
            {
                string sql = "select * from View_I_M_XZFF where rguid='" + rguid + "'";
                IList<IDictionary<string, string>> dt1 = CommonDao.GetDataTable(sql);
                if (dt1.Count > 0)
                {
                    //保存pdf工资册
                    byte[] filebyte;
                   // code = SaveWGRYReportFDF(rguid, out filebyte);
                    string attach = "";// filebyte.EncodeBase64();

                    string recids = dt1[0]["payrecids"];
                    string gcbh = dt1[0]["jdzch"];
                    string sgdwbh = dt1[0]["sgdwbh"];
                    string lwgsbh = dt1[0]["lwgsbh"];
                    string year = dt1[0]["payyear"];
                    string month = dt1[0]["paymonth"];
                    string paytype = "";
                    string remark1 = "";
                    string remark2 = "";
                    payroll.paycode = rguid;
                    payroll.projectcode = gcbh;
                    payroll.companycode = sgdwbh;
                    payroll.paycompanycode = lwgsbh == "" ? sgdwbh : lwgsbh;
                    payroll.payyear = year;
                    payroll.paymonth = month;
                    payroll.paytype = paytype;
                    payroll.remark1 = remark1;
                    payroll.remark2 = remark2;
                    payroll.Attach = attach;
                    List<Payrollrows> payrowslist = new List<Payrollrows>();
                    sql = "select * from View_KQJUSERMONTHPAY where recid in(" + recids + ")";
                    IList<IDictionary<string, string>> rydetails = CommonDao.GetDataTable(sql);
                    for (int i = 0; i < rydetails.Count; i++)
                    {
                        Payrollrows payrows = new Payrollrows();
                        string Name = rydetails[i]["ryxm"];
                        string phone = rydetails[i]["sjhm"];
                        string IdNumber = rydetails[i]["userid"];
                        string CardNumber = rydetails[i]["yhkh"];
                        string BankNumber = rydetails[i]["yhhh"];
                        string Paysum = rydetails[i]["havepay"];
                        string Remark1 = "";
                        payrows.Name = Name;
                        payrows.Phone = phone;
                        payrows.IdNumber = IdNumber;
                        payrows.CardNumber = CardNumber;
                        payrows.BankNumber = BankNumber;
                        payrows.Paysum = Paysum;
                        payrows.Remark1 = Remark1;
                        payrowslist.Add(payrows);
                    }
                    payroll.rows = payrowslist;

                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string url = "http://pay.jzyglxt.com/api/apipay/SetPayroll";
                IDictionary<string, string> datas = new Dictionary<string, string>();
                datas.Add("key", key);
                datas.Add("data",jss.Serialize(payroll));

                if (code && MyHttp.Post(url, datas, out msg))
                {
                    PayMsg paymsg = jss.Deserialize<PayMsg>(msg); //反序列化.   
                    string resmsg = paymsg.message;
                    string success = paymsg.success;
                    if(success=="0000")
                    {
                        string sql = "update info_m_xzff set tszt=1 where rguid='" + rguid + "'";
                        CommonDao.ExecSql(sql);
                        code = true;              
                    }
                    else
                    {
                        string sql = "update info_m_xzff set message='" + resmsg + "' where rguid='" + rguid + "'";
                        CommonDao.ExecSql(sql);
                        code = false;
                    }

                }
                else
                {
                    if(msg!="")
                    {
                        PayMsg paymsg = jss.Deserialize<PayMsg>(msg); //反序列化.   
                        string resmsg = paymsg.message;
                        string sql = "update info_m_xzff set message='" + resmsg + "' where rguid='" + rguid + "'";
                        CommonDao.ExecSql(sql);
                    }                   
                    code = false;
                    SysLog4.WriteError("推送工资册失败:" + rguid);
                    msg = "推送工资册失败";                  
                }

                
            }
            return code;
        }

        /// <summary>
        /// 该接口用于务工人员平台把人员卡号或手机号变更信息推送给支付平台
        /// </summary>
        public void SetPersonCard(IDictionary<string, string> row)
        {
            bool code = true;
            string msg = "";
            string recid = row["recid"];
            PersonCard pcard = new PersonCard();
            string key = MD5Util.StringToMD5Hash(WGRYPAY_KEY);
            try
            {
                string ryxm = row["ryxm"].GetSafeString();
                string sfzhm = row["sfzhm"].GetSafeString();
                string Fromcard = row["ordcardnumber"].GetSafeString(); //原银行卡号
                string Tocard = row["newcardnumber"].GetSafeString(); //新银行卡号
                string ToBankCode = row["newyhhh"].GetSafeString(); //新银行行号
                string Formphone = row["oldphone"].GetSafeString(); //原手机号码
                string Tophone = row["newphone"].GetSafeString();  //新手机号码
                List<PersonCardrows> pcardrowslist = new List<PersonCardrows>();
                PersonCardrows pcardrows = new PersonCardrows();

                pcardrows.Name = ryxm;

                pcardrows.IdNumber = sfzhm;
                pcardrows.Fromcard = Fromcard;
                pcardrows.Tocard = Tocard;
                pcardrows.ToBankCode = ToBankCode;
                pcardrows.Fromephone = Formphone;
                pcardrows.Tophone = Tophone;
                pcardrowslist.Add(pcardrows);

           
                pcard.success = "0000";
                pcard.message = "";
                pcard.rows = pcardrowslist;

            }
            catch (Exception e)
            {
                msg = e.Message;
                pcard.success = "1111";
                pcard.message = msg;
                code = false;
            }
            finally
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string url = "http://pay.jzyglxt.com/api/apipay/SetPersonCard";
                //string json = jss.Serialize(pcard);
                string json = string.Format("{{\"key\":\"{0}\",\"data\":{1}}}", key, jss.Serialize(pcard));
                if (code&&MyHttp.Post(url, json, out msg))
                {
                    string sql = "update I_S_GC_WGRYYHBG set hasdeal=1 where recid='" + recid + "'";
                    CommonDao.ExecSql(sql);
                    code = true;
                }
                else
                {
                    SysLog4.WriteError("更换银行卡信息失败:"+row["sfzhm"]);
                }

            }
        }

        /// <summary>
        /// 保存工资册pdf
        /// </summary>
        /// <returns></returns>
        public bool SaveWGRYReportFDF(string rguid, out  byte[] fileBytes)
        {
            bool code = true;
            string msg = "";
            try
            {
                string url = "";
                string reportFile = "工资发放表";
                string tablename = "ViewKqjUserMonthLog";// Request["tablename"].GetSafeString();

                string where = " recid in (select payrecids from View_I_M_XZFF where rguid='" + rguid + "')";

                var g = new ReportPrint.GenerateGuid();
                var c = g.Get();

                c.type = ReportPrint.EnumType.Excel;
                //c.field = reportFile;
                c.fileindex = "0";

                c.filename = reportFile;
                c.table = tablename;
                c.where = where;

                c.openType = ReportPrint.OpenType.PDF;
                c.libType = ReportPrint.LibType.OpenXmlSdk;
                c.signindex = 0;
                c.customtools = "1,|2,|3,|4,|5,|6,|12,";
                c.AllowVisitNum = 1;

                //var guid = g.Add(c);

                string err = "";
                code = g.GetFile(c, out fileBytes, out err);
                if (code)
                {
                    code = SaveFileFun.saveExcelthread(rguid + ".pdf", fileBytes, "wgrypdf");
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
                fileBytes = null;
            }
            return code;
           
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
        /// <summary>
        /// 支付平台des加密的key
        /// </summary>
        private  string WGRYPAY_KEY
        {
            get
            {
                return GetSysSettingValue("WGRYPAY_KEY");
            }
        }

        #endregion   
    }
}
