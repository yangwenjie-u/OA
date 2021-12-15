using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using System.Text;


namespace BD.Jcbg.Bll
{
    public class WdyKqjService : IWdyKqjService
    {

        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        #endregion

        #region 服务
        /// <summary>
        /// 下发人员模板
        /// </summary>
        /// <param name="rybh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DownRyIris(string rybh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string sql = "select qybh,ryxm,sfzhm,hm from i_m_ry where rybh='" + rybh + "' and jdzch='"+CurrentUser.Jdzch+"' ";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    msg = "找不到人员信息，下发虹膜失败。";
                    return false;
                }
                string qybh = dt[0]["qybh"].GetSafeString();
                string ryxm = dt[0]["ryxm"].GetSafeString();
                string sfzhm = dt[0]["sfzhm"].GetSafeString();
                string hm = dt[0]["hm"].GetSafeString();
                //if (qybh == "")
                //    msg = "企业信息为空。";
                if (ryxm == "")
                    msg = "人员姓名为空。";
                if (sfzhm == "")
                    msg = "身份证号码为空。";
                if (hm == "")
                    msg = "虹膜为空。";
                if (msg != "")
                {
                    msg += "下发虹膜失败。";
                    return false;
                }
                sql = "insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select kqjbh,21,'" + sfzhm + "','" + ryxm + "','','" + hm + "' from i_m_kqj where qybh='" + qybh + "' and jdzch='"+CurrentUser.Jdzch+"'";
                CommonDao.ExecCommand(sql, CommandType.Text);
               // CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select kqjbh,21,'" + sfzhm + "','" + ryxm + "','','" + hm + "' from i_m_kqj where gcbh in (select gcbh from view_gc_ry where rybh='" + rybh + "')", CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        [Transaction(ReadOnly = false)]
        public bool DownWGRyIris(string rybh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string sql = "select qybh,ryxm,sfzhm,hm from i_m_wgry where rybh='" + rybh + "' and jdzch='" + CurrentUser.Jdzch + "' ";
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count == 0)
                {
                    msg = "找不到人员信息，下发虹膜失败。";
                    return false;
                }
                string qybh = dt[0]["qybh"].GetSafeString();
                string ryxm = dt[0]["ryxm"].GetSafeString();
                string sfzhm = dt[0]["sfzhm"].GetSafeString();
                string hm = dt[0]["hm"].GetSafeString();
                //if (qybh == "")
                //    msg = "企业信息为空。";
                if (ryxm == "")
                    msg = "人员姓名为空。";
                if (sfzhm == "")
                    msg = "身份证号码为空。";
                if (hm == "")
                    msg = "虹膜为空。";
                if (msg != "")
                {
                    msg += "下发虹膜失败。";
                    return false;
                }
                sql = "insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select kqjbh,21,'" + sfzhm + "','" + ryxm + "','','" + hm + "' from i_m_kqj where qybh='" + qybh + "' and jdzch='" + CurrentUser.Jdzch + "'";
                CommonDao.ExecCommand(sql, CommandType.Text);
                // CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select kqjbh,21,'" + sfzhm + "','" + ryxm + "','','" + hm + "' from i_m_kqj where gcbh in (select gcbh from view_gc_ry where rybh='" + rybh + "')", CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        /// <summary>
        /// 下发考勤机所有模板
        /// </summary>
        /// <param name="kqjbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool DownKqjIris(string kqjbh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 下发人员
                //CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select '" + kqjbh + "',21,sfzhm,ryxm,'',hm from i_m_ry where qybh in (select qybh from i_m_kqj where kqjbh='" + kqjbh + "' and qybh<>'') and qybh<>'' and sfzhm<>'' and ryxm<>'' and hm<>''", CommandType.Text);
                CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select '" + kqjbh + "',21,sfzhm,ryxm,'',hm from i_m_wgry where jdzch in (select jdzch from i_m_kqj where kqjbh='" + kqjbh + "' and qybh<>'') and qybh<>'' and sfzhm<>'' and ryxm<>'' and hm<>''", CommandType.Text);
                //下面有问题
                //CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) select '" + kqjbh + "',21,sfzhm,ryxm,'',hm from i_m_ry where rybh in (select rybh from view_gc_ry where gcbh in (select gcbh from i_m_kqj where kqjbh='" + kqjbh + "' and gcbh<>'')) and  sfzhm<>'' and ryxm<>'' and hm<>''", CommandType.Text);
                // 重启设备
                //CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values ('" + kqjbh + "',3210,'','','','')", CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        /// <summary>
        /// 初始化考勤机
        /// </summary>
        /// <param name="kqjbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool InitKqj(string kqjbh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 设置设备时间
               // CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values('" + kqjbh + "',3120,'','','','')", CommandType.Text);

                // 清除人员信息，考勤信息
                CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values('" + kqjbh + "',22,'','','','')", CommandType.Text);
                // 设置logo
               // CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values('" + kqjbh + "',3181,'','','','')", CommandType.Text);
                // 设置背景
               // CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values('" + kqjbh + "',3182,'','','','')", CommandType.Text);
                // 重启设备
               // CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values('" + kqjbh + "',3210,'','','','')", CommandType.Text);

                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        /// <summary>
        /// 重启考勤机
        /// </summary>
        /// <param name="kqjbh"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool RestartKqj(string kqjbh, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                // 重启设备
                CommonDao.ExecCommand("insert into KqjDeviceCommand(Serial,Command,UserId,RealName,UserStation,IrisModule) values('" + kqjbh + "',3210,'','','','')", CommandType.Text);

                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        #endregion

        #region 考勤统计模块

        /// <summary>
        /// 外来企业考勤统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        [Transaction(ReadOnly = false)]
        public List<IDictionary<string, string>> GetQyKq(string startTime, string endTime)
        {
            List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            try
            {
                string sql = "SELECT newry.QYBH,newry.LXBH,newry.QYMC,newry.RYZS,CASE WHEN newkq.RYKQ IS NULL THEN 0 ELSE newkq.RYKQ END AS RYKQ FROM (SELECT qy.QYBH,qy.LXBH,qy.QYMC,COUNT(1) AS RYZS FROM dbo.I_M_QY AS qy LEFT JOIN dbo.I_M_RY AS ry ON ry.QYBH=qy.QYBH WHERE qy.WDQY='true' GROUP BY qy.QYBH,qy.LXBH,qy.QYMC) AS newry LEFT JOIN (SELECT new.CompanyId,COUNT(1) AS RYKQ FROM (SELECT CompanyId,UserId FROM dbo.KqjUserLog WHERE LogDate BETWEEN @StartTime AND @EndTime GROUP BY CompanyId,UserId) AS new GROUP BY new.CompanyId) AS newkq ON newkq.CompanyId=newry.QYBH";
                List<IDataParameter> parameters = new List<IDataParameter>
                {
                    new SqlParameter("@StartTime",startTime),
                    new SqlParameter("@EndTime",endTime)
                };
                list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return list;
        }

        /// <summary>
        /// 工程信息下拉框
        /// </summary>
        [Transaction(ReadOnly = false)]
        public List<IDictionary<string, string>> GetGcName()
        {
            List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            try
            {
                string sql = "SELECT GCBH,GCMC FROM dbo.I_M_GC";
                list = CommonDao.GetDataTable(sql).ToList();
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return list;
        }



        #endregion

        #region 地图展示

        /// <summary>
        /// 获取工程信息,工程企业考勤信息,工程经纬度信息
        /// </summary>
        public DataSet GetGcInfos(string gczt, string qymc)
        {
            ArrayList list = new ArrayList();
            DataSet ds = new DataSet();
            try
            {
                list.Add("SELECT gc.RECID,gc.JDZCH,gc.GCMC,qy.QYBH,qy.QYMC,gczt.GCZT FROM dbo.View_I_M_GC_KGBA AS gc LEFT JOIN dbo.I_M_GC_ZT AS gczt ON gczt.Recid=gc.GCZT LEFT JOIN dbo.H_GCQY AS qy ON qy.QYBH=gc.GCQYBH where qy.QYMC like '%" + qymc + "%' and gczt.GCZT like '%" + gczt + "%'");
                list.Add("SELECT *,CONVERT(DECIMAL(4,2),RYKQ*0.1/RYZS*10) AS KQL FROM View_GCKQ_Maps2 ORDER BY JDZCH,QYBH");
                list.Add("SELECT Recid,GCBH,JDZCH,InfoName,InfoImg,Lon,Lat,InfoUrl,OrderBy FROM dbo.I_M_GC_JWD");
                list.Add("SELECT kqj.RECID,kqj.QYBH,qy.QYMC,gc.GCBH,gc.JDZCH,gc.GCMC,kqj.KQJBH,kqj.Lon,kqj.Lat,dev.LastUpdate,kqj.KQJBZ FROM dbo.I_M_KQJ AS kqj LEFT JOIN dbo.I_M_QY AS qy ON qy.QYBH=kqj.QYBH LEFT JOIN dbo.I_M_GC AS gc ON gc.GCBH=kqj.GCBH LEFT JOIN dbo.KqjDeviceConnect AS dev ON dev.Serial=kqj.KQJBH");
                list.Add("SELECT SettingValue FROM dbo.SysSetting WHERE GroupId='KQJ_SETTING' AND SettingCode='KQJ_SETTING_DEFAULTTIME'");
                ds = CommonDao.GetDataSet(list);
            }
            catch(Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ds;
        }

        #endregion




        #region 考勤统计模块



        /// <summary>
        /// 工程企业考勤统计
        /// </summary>
        /// <param name="gcbh">工程编号</param>
        /// <param name="kqTime">考勤时间</param>
        [Transaction(ReadOnly = false)]
        public List<IDictionary<string, string>> GetGcKq(string gcbh, string kqTime)
        {
            List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            try
            {
                string sql = "SELECT * FROM dbo.View_GCKQ_Charts AS gckq WHERE gckq.GCBH=@GCBH AND gckq.LogDay=@LogDay";
                List<IDataParameter> parameters = new List<IDataParameter>
                {
                    new SqlParameter("@GCBH",gcbh),
                    new SqlParameter("@LogDay",kqTime)
                };
                list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return list;
        }

        #endregion

        #region 地图展示

        /// <summary>
        /// 获取工程信息,工程企业考勤信息,工程经纬度信息
        /// </summary>
        public DataSet GetGcInfos()
        {
            ArrayList list = new ArrayList();
            list.Add("SELECT gc.RECID,gc.GCBH,gc.GCMC,gczt.GCZT FROM dbo.I_M_GC AS gc LEFT JOIN dbo.I_M_GC_ZT AS gczt ON gczt.Recid=gc.GCZT");
            list.Add("SELECT *,case when RYZS=0 then 0 else CONVERT(DECIMAL(4,2),RYKQ*0.1/RYZS*10) end AS KQL  FROM View_GCKQ_Maps ORDER BY GCBH,QYBH");
            list.Add("SELECT Recid,GCBH,InfoName,InfoImg,Lon,Lat,InfoUrl,OrderBy FROM dbo.I_M_GC_JWD");
            return CommonDao.GetDataSet(list);
        }

        #endregion
        /// <summary>
        /// 涉外企业信息
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/28 14:07
        public DataSet GetSwqyInfos(string wdqy)
        {
            ArrayList list = new ArrayList();
            list.Add("SELECT * FROM view_qy_infos WHERE wdqy LIKE '%" + wdqy + "%'");
            list.Add("SELECT jwd.Recid,jwd.QYBH,jwd.InfoName,jwd.InfoImg,jwd.Lon,jwd.Lat,jwd.InfoUrl,jwd.OrderBy FROM dbo.I_M_QY_JWD AS jwd LEFT JOIN dbo.I_M_QY AS qy ON qy.QYBH=jwd.qybh WHERE qy.WDQY LIKE '%" + wdqy + "%'");
            return CommonDao.GetDataSet(list);
        }

        /// <summary>
        /// 获取考勤机信息,经纬度,断线间隔
        /// </summary>
        public DataSet GetKqjInfos(string wdqy)
        {
            ArrayList list = new ArrayList();
            list.Add("SELECT kqj.RECID,kqj.QYBH,qy.QYMC,ISNULL(qy.wdqy,0) as wdqy,gc.GCBH,gc.GCMC,kqj.KQJBH,kqj.Lon,kqj.Lat,dev.LastUpdate,kqj.KQJBZ FROM dbo.I_M_KQJ AS kqj LEFT JOIN dbo.I_M_QY AS qy ON qy.QYBH=kqj.QYBH LEFT JOIN dbo.I_M_GC AS gc ON gc.GCBH=kqj.GCBH LEFT JOIN dbo.KqjDeviceConnect AS dev ON dev.Serial=kqj.KQJBH where ISNULL(qy.wdqy,0) like '%" + wdqy + "%'");
            list.Add("SELECT SettingValue FROM dbo.SysSetting WHERE GroupId='KQJ_SETTING' AND SettingCode='KQJ_SETTING_DEFAULTTIME'");
            return CommonDao.GetDataSet(list);
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        public IList<IDictionary<string, string>> GetRyInfos(string qylx, string qybh, string ryxm, string wdqy)
        {
            string sql = "SELECT ry.QYBH,qy.QYMC,ry.RYBH,ry.RYXM,ry.XB,ry.zc,ry.GW,ry.Lon,ry.Lat FROM dbo.I_M_RY AS ry LEFT JOIN dbo.I_M_QY AS qy ON qy.QYBH=ry.QYBH WHERE qy.LXBH like @qylx and ry.QYBH like @qybh and ry.RYXM like @ryxm and qy.wdqy like @wdqy and ry.Lon is not null and ry.Lat is not null";
            IList<IDataParameter> parameters = new List<IDataParameter>
            {
                new SqlParameter("@qylx","%"+qylx+"%"),
                new SqlParameter("@qybh","%"+qybh+"%"),
                new SqlParameter("@ryxm","%"+ryxm+"%"),
                new SqlParameter("@wdqy","%"+wdqy+"%")
            };
            return CommonDao.GetDataTable(sql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 获取班组负责人
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetBzfzrs()
        {
            StringBuilder where = new StringBuilder();
            // ------单位用户过滤statr---------
            where.Append(" and qybh='" + CurrentUser.Qybh + "' ");
            // ----------- end ----------------

            string sql = "select sfzhm,ryxm from I_M_RY where sfbzfzr='是' " + where + " order by ryxm";
            return CommonDao.GetDataTable(sql);
        }
        /// <summary>
        /// 获取工种
        /// </summary>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetGzs()
        {
            StringBuilder where = new StringBuilder();
            // ------单位用户过滤statr---------
            where.Append(" and qybh='" + CurrentUser.Qybh + "' ");
            // ----------- end ----------------

            string sql = "select recid,gzname from H_RYGZ  order by xssx";
            return CommonDao.GetDataTable(sql);
        }

        public IList<IDictionary<string, string>> GetGzGws(string gz)
        {
            StringBuilder where = new StringBuilder();
            string sql="select 1=1";
             where.Append(" where 1=1 ");
            // ------单位用户过滤statr---------
          
            // ----------- end ----------------
            if(gz!="")
            {
                where.Append(" and gzname='" + gz + "' ");
                sql = "select recid,gwname from H_RYGZ_GW  " + where + " order by xssx";
            }
         
            return CommonDao.GetDataTable(sql);
        }
        
        
        #region 下拉框

        /// <summary>
        /// 单位类型
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 13:26
        public IList<IDictionary<string, string>> GetDwlx()
        {
            string sql = "SELECT qylx.LXBH,qylx.LXMC FROM dbo.H_QYLX AS qylx";
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 根据类型查找企业名称
        /// </summary>
        /// Author  : Napoleon
        /// Created : 2017/5/8 13:58
        public IList<IDictionary<string, string>> GetDwmc(string lxbh, string wdqy)
        {
            string sql = "SELECT qy.QYBH,qy.QYMC FROM dbo.I_M_QY AS qy WHERE qy.LXBH like @lxbh and qy.wdqy like @wdqy";
            IList<IDataParameter> parameters = new List<IDataParameter>
            {
                new SqlParameter("@lxbh","%"+lxbh+"%"),
                new SqlParameter("@wdqy","%"+wdqy+"%")
            };
            IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql, CommandType.Text, parameters);
            return dt;
        }

        #endregion

        #region 考勤机轮询
        public IList<IDictionary<string, string>> GetKqjUserLog()
        {
            string sql = "select * from kqjuserlog where HasDeal='False' and serial!='' order by LogDate ";
            return CommonDao.GetDataTable(sql); 
        }

        public bool updateKqjUserLog(
            string serial, string userid, string companyid, string jdzch, DateTime time, string kqjlx, string qymc, string gcmc)
        {
            string sql = "Update kqjuserlog Set HasDeal = 'True',companyid='" + companyid + "',placeid='" + jdzch + "',DealType=1,LogType='" + kqjlx + "',ExtraInfo1='wdy' ,companyname='" + qymc + "',projectname='" + gcmc + "' "; ;
            string where = "where serial='" + serial + "' and userid='" + userid + "' and logdate=convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "') and  HasDeal='False'";
            sql += where;
            return CommonDao.ExecSql(sql);
        }

        /// <summary>
		/// 保存一条考勤信息,五大员考勤
		/// </summary>
		/// <param name="log"></param>
		/// <returns></returns>
		public bool SaveUserLog(string serial, string userid, DateTime time)
        {
            string qybh, qymc, jdzch, gcmc, kqjlx="1", bzfzr, ryxm, bzfzrxm = "", kqtimes = "1";
            IDictionary<string, string> kqjrow = CommonDao.GetRowValue("qybh,qymc,gcmc,jdzch,kqjlx", "View_I_M_KQJ", "kqjbh='" + serial + "'");
            if (kqjrow == null)
                return false;
            //kqjrow.TryGetValue("qybh", out qybh);
           // kqjrow.TryGetValue("qymc", out qymc);
            //kqjrow.TryGetValue("gcmc", out gcmc);
            kqjrow.TryGetValue("jdzch", out jdzch);
            kqjrow.TryGetValue("kqjlx", out kqjlx);


            IDictionary<string, string> ryrow = null;
            string sfbzfzr = "";
            if (userid.Length == 16)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "View_I_M_RY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    ryrow.TryGetValue("sfzhm", out userid);
                }
            }
            ryrow = CommonDao.GetRowValue("bdrybh as rybh,ryxm,gw,rylx,qymc,r_qybh as qybh", "View_GC_RY_QYRYCK", "sfzhm='" + userid + "' and gcbh='" + jdzch + "' ");

            if (ryrow == null) //工程中没有该人员
                return false;

            //获取项目是单向还是双向          
            IDictionary<string, string> qykqrow = CommonDao.GetRowValue("kqtimes,gcmc", "View_I_M_GC_XM", "gcbh='" + jdzch + "'");
            if (qykqrow == null)
                return false;
            qykqrow.TryGetValue("gcmc", out gcmc);
            //qykqrow.TryGetValue("kqtimes", out kqtimes);
            //if (kqtimes == "")
            kqtimes = "1";
            string rybh, gw,rylx;
            ryrow.TryGetValue("rybh", out rybh);
            ryrow.TryGetValue("ryxm", out ryxm);
            ryrow.TryGetValue("gw", out gw);
            ryrow.TryGetValue("rylx", out rylx);
            ryrow.TryGetValue("qymc", out qymc);
            ryrow.TryGetValue("qybh", out qybh);

            //UpdateUserInTime(userid, jdzch, time);

            if (kqjlx.Equals("上班考勤"))
                kqjlx = UserLogType.In;
            else if (kqjlx.Equals("下班考勤"))
                kqjlx = UserLogType.Out;
            else if (kqjlx.Equals("门禁"))
                kqjlx = UserLogType.Check;
            else
                kqjlx = UserLogType.In;

            bool ret = updateKqjUserLog(serial, userid, qybh, jdzch, time, kqjlx, qymc,gcmc);
            //if(ret)
            //{
            //    WdySaveUserDayLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, gw,rylx,rybh);
            //}



            return ret;
        }
       
        /// <summary>
        /// 保存一条考勤信息---手动设置，不通过考勤机 目前只支持单项考勤
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool SaveUserLogWithOUTkqj(string userid, DateTime time,string qybh,string jdzch)
        {
            string qymc, gcmc, bzfzr, ryxm, bzfzrxm = "", kqtimes = "1";
            IDictionary<string, string> kqjrow = CommonDao.GetRowValue("sgdwmc,gcmc", "I_M_GC_kgba", "qybh='" + qybh + "' and jdzch='" + jdzch + "'");
            if (kqjrow == null)
                return false;

            kqjrow.TryGetValue("sgdwmc", out qymc);
            kqjrow.TryGetValue("gcmc", out gcmc);

            IDictionary<string, string> kqjlogtoday = CommonDao.GetRowValue("recid", "kqjuserlog", "userid='" + userid + "'and placeid !='" + jdzch + "' and placeid!='' and datediff(dd,logdate,'" + time + "')=0");
            if (kqjlogtoday != null)
            {
                string recid = "";
                kqjlogtoday.TryGetValue("recid", out recid);
                string sql = "update  kqjuserlog  set hasdeal=1,dealtype=0 where userid='" + userid + "'and companyid='" + qybh + "' and placeid='" + jdzch + "' and datediff(dd,logdate,'" + time + "')=0";  //表示今天已考勤过,dealtype=0 表示1天重复考勤
                CommonDao.ExecSql(sql);
                return false;
            }

            IDictionary<string, string> ryrow = null;
            string sfbzfzr = "";
            if (userid.Length == 16)
            {
                ryrow = CommonDao.GetRowValue("sfzhm", "View_I_M_RY", "sfzhm='" + userid + "'");
                if (ryrow != null)
                {
                    ryrow.TryGetValue("sfzhm", out userid);
                }

            }
            ryrow = CommonDao.GetRowValue("bzfzr,ryxm,bzfzrxm,sfbzfzr", "View_I_M_RY", "sfzhm='" + userid + "' and jdzch='" + jdzch + "'");

            if (ryrow == null) //工程中没有该人员
                return false;
            ryrow.TryGetValue("ryxm", out ryxm);
            ryrow.TryGetValue("bzfzr", out bzfzr);
            ryrow.TryGetValue("bzfzrxm", out bzfzrxm);
            ryrow.TryGetValue("sfbzfzr", out sfbzfzr);

            UpdateUserInTime(userid, jdzch, time);

            //bool ret = updateKqjUserLog(serial, userid, jdzch, time);
            //if (ret)
            //bool ret=SaveUserDayLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, bzfzr, bzfzrxm);
            bool ret = true;

            return ret;
        }

        /// <summary>
        /// 单向考勤
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <param name="qybh"></param>
        /// <param name="qymc"></param>
        /// <param name="jdzch"></param>
        /// <param name="gcmc"></param>
        /// <param name="ryxm"></param>
        /// <param name="bzfzr"></param>
        /// <param name="bzfzrxm"></param>
        /// <returns></returns>
        protected bool SaveUserDayLog(string userid,DateTime time,string qybh, string qymc, string jdzch, string gcmc, string ryxm, string bzfzr, string bzfzrxm)
        {
            string sql = "";
            try
            {
                //获取工种信息
                string gzid = "", gzname = "";
                IDictionary<string, string> gzs = CommonDao.GetRowValue("recid,gzname", "H_RYGZ", "gzname in (select gz from I_M_RY where sfzhm='" + userid + "' and jdzch='"+jdzch+"')");
                gzs.TryGetValue("recid", out gzid);
                gzs.TryGetValue("gzname", out gzname);
                IList<string> sqls = new List<string>();
                string logday = time.ToString("yyyy-MM-dd"); 
                

                //判断logday 这天，userid 人员有没有考勤过
                #region
                List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                sql = "select * from kqjuserdaylog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logday=@logday";
                List<IDataParameter> parameters = new List<IDataParameter>
                {
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@qybh",qybh),
                    new SqlParameter("@jdzch",jdzch),
                    new SqlParameter("@logday",logday)
                };

                list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                if(list.Count>0) //该日该人员已考勤过
                {
                    //string recid = list[0]["recid"]; //获取刚插入的recid
                    //sql = "Insert into KqjUserDayLogDetail (parentid,InTime) values ('" + recid + "','" + time + "')";
                    //CommonDao.ExecSql(sql);
                }
                else
                {
                   sql = "INSERT INTO kqjuserdaylog (userid,LogDay,QYBH,QYMC,JDZCH,GCMC,RYXM,GzId,Bzfzr,bzfzrxm) values ('" +
                   userid + "','" + logday + "','" +
                   qybh + "','" +
                   qymc + "','" +
                   jdzch + "','" +
                   gcmc + "','" +
                   ryxm + "','" +
                   gzid + "','" +
                   bzfzr + "','" +
                   bzfzrxm + "')";
                   CommonDao.ExecSql(sql);

                   #region 设置没天详细的进出
                   list = new List<IDictionary<string, string>>();
                   sql = "select * from kqjuserdaylog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logday=@logday";
                   parameters = new List<IDataParameter>
                    {
                        new SqlParameter("@userid",userid),
                        new SqlParameter("@qybh",qybh),
                        new SqlParameter("@jdzch",jdzch),
                        new SqlParameter("@logday",logday)
                    };                
                   list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                    if(list.Count>0)
                    {
                        string recid = list[0]["recid"]; //获取刚插入的recid
                        sql="Insert into KqjUserDayLogDetail (parentid,InTime) values ('"+recid+"','"+time+"')";
                        CommonDao.ExecSql(sql);
                    }
                 
                   #endregion

                   #region 保存到月考勤记录表
                  // SaveUserMonthLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, bzfzr, bzfzrxm, gzid);
                   #endregion
                }
                #endregion

            }
            catch(Exception e)
            {

            }
            return true;
        }
        /// <summary>
        /// 五大员考勤保存考勤记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <param name="qybh"></param>
        /// <param name="qymc"></param>
        /// <param name="jdzch"></param>
        /// <param name="gcmc"></param>
        /// <param name="ryxm"></param>
        /// <param name="bzfzr"></param>
        /// <param name="bzfzrxm"></param>
        /// <returns></returns>
        protected bool WdySaveUserDayLog(string userid, DateTime time, string qybh, string qymc, string jdzch, string gcmc, string ryxm, string gw, string rylx,string rybh)
        {
            string sql = "";
            try
            {
                IList<string> sqls = new List<string>();
                string logday = time.ToString("yyyy-MM-dd");


                //判断logday 这天，userid 人员有没有考勤过
                #region
                List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                sql = "select * from KqjWdyUserDayLog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logday=@logday";
                List<IDataParameter> parameters = new List<IDataParameter>
                {
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@qybh",qybh),
                    new SqlParameter("@jdzch",jdzch),
                    new SqlParameter("@logday",logday)
                };

                list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                if (list.Count > 0) //该日该人员已考勤过
                {
                    //string recid = list[0]["recid"]; //获取刚插入的recid
                    //sql = "Insert into KqjUserDayLogDetail (parentid,InTime) values ('" + recid + "','" + time + "')";
                    //CommonDao.ExecSql(sql);
                }
                else
                {
                    sql = "INSERT INTO KqjWdyUserDayLog (userid,LogDay,rybh,QYBH,QYMC,JDZCH,GCMC,RYXM,GW,RYLX) values ('" +
                    userid + "','" + logday + "','" +
                    rybh + "','" +
                    qybh + "','" +
                    qymc + "','" +
                    jdzch + "','" +
                    gcmc + "','" +
                    ryxm + "','" +
                    gw + "','" +
                    rylx + "')";
                    CommonDao.ExecSql(sql);

                    #region 设置每天详细的进出
                    list = new List<IDictionary<string, string>>();
                    sql = "select * from KqjWdyUserDayLog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logday=@logday";
                    parameters = new List<IDataParameter>
                    {
                        new SqlParameter("@userid",userid),
                        new SqlParameter("@qybh",qybh),
                        new SqlParameter("@jdzch",jdzch),
                        new SqlParameter("@logday",logday)
                    };
                    list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                    if (list.Count > 0)
                    {
                        string recid = list[0]["recid"]; //获取刚插入的recid
                        sql = "Insert into KqjWdyUserDayLogDetail (parentid,InTime) values ('" + recid + "','" + time + "')";
                        CommonDao.ExecSql(sql);
                    }

                    #endregion

                    #region 保存到月考勤记录表
                    SaveUserMonthLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm, gw,rylx,rybh);
                    #endregion
                }
                #endregion

            }
            catch (Exception e)
            {

            }
            return true;
        }

        protected bool SaveUserDayLogInAndOut(string userid, DateTime time, string qybh, string qymc, string jdzch, string gcmc, string ryxm, string gw,string rylx,string kqjlx)
        {
            string sql = "";
            try
            {
                //获取工种信息
                string gzid = "", gzname = "";
                IDictionary<string, string> gzs = CommonDao.GetRowValue("recid,gzname", "H_RYGZ", "gzname in (select gz from I_M_RY where sfzhm='" + userid + "' and jdzch='" + jdzch + "')");
                gzs.TryGetValue("recid", out gzid);
                gzs.TryGetValue("gzname", out gzname);
                IList<string> sqls = new List<string>();
                string logday = time.ToString("yyyy-MM-dd");


                //判断logday 这天，userid 人员有没有考勤过
                #region
                List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
                sql = "select * from kqjuserdaylog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logday=@logday";
                List<IDataParameter> parameters = new List<IDataParameter>
                {
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@qybh",qybh),
                    new SqlParameter("@jdzch",jdzch),
                    new SqlParameter("@logday",logday)
                };

                list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                if (list.Count > 0) //该日该人员已考勤过 ，为进的考勤
                {
                    string recid = list[0]["recid"]; //获取刚插入的recid
                    sql = "select * from KqjUserDayLogDetail where parentid='" + recid + "' and outtime is null and intime < '"+time+"' order by Intime";
                    IList<IDictionary<string, string>> detaillist = CommonDao.GetDataTable(sql);
                    if (detaillist.Count>0) //有未考勤出场的记录
                    {
                        for (int i = 0; i < detaillist.Count; i++) //有进的记录，没有出的记录
                        {
                            string detailRecid = detaillist[i]["recid"];
                            if (i != detaillist.Count-1)
                            {
                                sql = "update KqjUserDayLogDetail set OutTime=Intime where  recid='" + detailRecid + "'";  //设置原未进行出考勤的时间为进的时间
                                CommonDao.ExecSql(sql);
                            }
                        }
                        if (kqjlx == UserLogType.In) //当前为进的考勤
                        {
                            string detailRecid = detaillist[detaillist.Count - 1]["recid"].GetSafeString();
                            sql = "update KqjUserDayLogDetail set OutTime=Intime where  recid='" + detailRecid + "'";  //设置原未进行出考勤的时间为进的时间
                            CommonDao.ExecSql(sql);
                            sql = "Insert into KqjUserDayLogDetail (parentid,InTime) values ('" + recid + "','" + time + "')"; //插入新进的记录
                            CommonDao.ExecSql(sql);
                        }
                        else //出的考勤
                        {
                            string detailRecid = detaillist[detaillist.Count - 1]["recid"]; 
                            sql = "update KqjUserDayLogDetail set OutTime='" + time + "' where  recid='" + detailRecid+ "'"; //设置最近的一次进的时间相对应的出的时间
                            CommonDao.ExecSql(sql);
                        }
                       
                    }                
                    else  //没有未进行出考勤的记录，重新插一条进的记录
                    {
                        sql = "Insert into KqjUserDayLogDetail (parentid,InTime) values ('" + recid + "','" + time + "')";
                        CommonDao.ExecSql(sql);
                    }              
                }
                else
                {
                    sql = "INSERT INTO KqjWdyUserDayLog (userid,LogDay,QYBH,QYMC,JDZCH,GCMC,RYXM,gw,rylx) values ('" +
                    userid + "','" + logday + "','" +
                    qybh + "','" +
                    qymc + "','" +
                    jdzch + "','" +
                    gcmc + "','" +
                    ryxm + "','" +
                    gw + "','" +
                    rylx + "')";
                    CommonDao.ExecSql(sql);

                    #region 设置没天详细的进出
                    list = new List<IDictionary<string, string>>();
                    sql = "select * from kqjuserdaylog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logday=@logday";
                    parameters = new List<IDataParameter>
                    {
                        new SqlParameter("@userid",userid),
                        new SqlParameter("@qybh",qybh),
                        new SqlParameter("@jdzch",jdzch),
                        new SqlParameter("@logday",logday)
                    };
                    list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
                    if (list.Count > 0)
                    {
                        if (kqjlx == UserLogType.In)
                        {
                            string recid = list[0]["recid"]; //获取刚插入的recid
                            sql = "Insert into KqjUserDayLogDetail (parentid,InTime) values ('" + recid + "','" + time + "')";
                            CommonDao.ExecSql(sql);
                        }                 
                    }

                    #endregion

                    #region 保存到月考勤记录表
                    SaveUserMonthLog(userid, time, qybh, qymc, jdzch, gcmc, ryxm,gw,rylx,"");
                    #endregion
                }
                #endregion

            }
            catch (Exception e)
            {

            }
            return true;
        }
        protected bool SaveUserMonthLog(string userid, DateTime time, string qybh, string qymc, string jdzch, string gcmc, string ryxm, string gw,string rylx,string rybh)
        {
                string year=time.Year.ToString();
                string month=time.Month.ToString();
                string day=time.Day.ToString();
                int workday = 0;
                string recid = GetMonthlog_Recid(time, userid, qybh, jdzch, out workday);
                if (recid!="") //该年该月该人员已考勤过
                {
                    ///更新考勤day时间
                   SetMonthLog( recid,  day, "In", time,workday);
                }
                else
                {
                    ///插入月考勤相关数据
                    string sql = "INSERT INTO KqjWdyUserMonthLog (userid,rybh,Logyear,logmonth,QYBH,QYMC,JDZCH,GCMC,RYXM,GW,RYLX) values ('" +
                    userid + "','" +
                    rybh + "','" + 
                    year + "','" +
                    month + "','" +
                    qybh + "','" +
                    qymc + "','" +
                    jdzch + "','" +
                    gcmc + "','" +
                    ryxm + "','" +
                    gw + "','" +
                    rylx + "')";
                    CommonDao.ExecSql(sql);

                    ///更新考勤day时间
                    recid = GetMonthlog_Recid(time, userid, qybh, jdzch,out workday);
                    SetMonthLog(recid, day, "In", time, workday);
                }
            return true;
        }

        public string GetMonthlog_Recid(DateTime time, string userid, string qybh, string jdzch, out int workday)
        {
            string year = time.Year.ToString();
            string month = time.Month.ToString();
            string day = time.Day.ToString();
            string recid = "";
            List<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            string sql = "select * from KqjWdyUserMonthLog where userid=@userid and qybh=@qybh and jdzch=@jdzch and logyear=@logyear and logmonth=@logmonth";
            List<IDataParameter> parameters = new List<IDataParameter>
                {

                    new SqlParameter("@userid",userid),
                    new SqlParameter("@qybh",qybh),
                    new SqlParameter("@jdzch",jdzch),
                    new SqlParameter("@logyear",year),
                    new SqlParameter("@logmonth",month)
                };
            list = CommonDao.GetDataTable(sql, CommandType.Text, parameters).ToList();
            if (list.Count > 0) //该年该月该人员已考勤过
            {
                recid = list[0]["recid"].ToString();
                workday = Convert.ToInt32(list[0]["workday"].GetSafeDecimal());
            }
            else
            {
                workday = 0;
            }
            return recid;
        }
        /// <summary>
        /// 保存某天的月考勤记录
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="day"></param>
        /// <param name="logtype"></param>
        /// <returns></returns>
        public bool SetMonthLog(string recid, string day, string logtype, DateTime dt,int workday)
        {
            string field = "logday";
            string fieldnum="logdaynum";
            if (logtype == "In")
                field += "in";
            else
                field += "out";
            field += day;
            fieldnum += day;

            bool ret = true;  
            try
            {
                bool needupdate = true;
                string hql = "select " + field + " from KqjWdyUserMonthLog where Recid=" + recid;
                List<IDictionary<string, string>> list = CommonDao.GetDataTable(hql).ToList();
                if(list.Count>0)
                {
                    DateTime oldDt = DataFormat.GetSafeDate(list[0][field]);
                    if (logtype == "In")
                    {
                        if (oldDt.Year != 1900 && oldDt.TimeOfDay.CompareTo(dt.TimeOfDay) < 0)
                            needupdate = false;
                    }
                    if (logtype == "Out")
                    {
                        if (oldDt.Year != 1900 && oldDt.TimeOfDay.CompareTo(dt.TimeOfDay) > 0)
                            needupdate = false;
                    }
                }
                if (needupdate)
                {
                    workday += 1;
                    hql = "update KqjWdyUserMonthLog set " + field + "=convert(datetime,'" + dt.ToString() + "')," + fieldnum + "=1 ,workday=" + workday + " where Recid=" + recid;
                    CommonDao.ExecSql(hql);
                }
            }
            catch (Exception e)
            {
              
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 显示所有人某月的工资册
        /// </summary>
        /// <param name="sfz"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetUserMonthPay(string jdzch, string xm, string sfz, string dt1, string dt2, string gz, string gw, string bzfzr, int pageSize, int pageIndex, out int totalCount)
        {
            StringBuilder where = new StringBuilder();
            //// ------单位用户过滤statr---------
            //    where.Append(" and companyid='" ' ");
            // ----------- end ----------------
            if (jdzch != "")
                where.Append(" and jdzch= '" + jdzch + "' ");
            if (xm != "")
                where.Append(" and RYXM like '%" + xm + "%' ");
            if (sfz != "")
                where.Append(" and userid like '%" + sfz + "%' ");
            if (bzfzr != "")
                where.Append(" and bzfzr like '%" + bzfzr + "%' ");
            if (dt1 != "")
                where.Append(" and LogYear ='" + dt1 + "' ");
            if (dt2 != "")
                where.Append(" and LogMonth ='" + dt2 + "' ");
            if (gz != "")
                where.Append(" and GzName like '%" + gz + "%' ");
            if (gw != "")
                where.Append(" and gw like '%" + gw + "%' ");
            string sql = "SELECT   *  FROM ViewKqjUserMonthLog where 1=1 " + where + " order by RYXM;";

            return CommonDao.GetPageData(sql, pageSize, pageIndex, out totalCount);
        }

        [Transaction(ReadOnly = false)]
        /// <summary>
        /// 更新月工资册
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="realpay"></param>
        /// <param name="advance"></param>
        /// <param name="paid"></param>
        /// <returns></returns>
        public bool UpdateUserMonthPay(string jdzch, string userid, string realname, string year, string month, string gzgz, string shouldpay, string havepay,string yzpay)
        {
            bool ret = true;
            try{
                if (gzgz == "" && shouldpay == "" && havepay == "" ) return ret;

            shouldpay = shouldpay == "" ? "0" : shouldpay;
            havepay = havepay == "" ? "0" : havepay;
            yzpay = yzpay == "" ? "0" : yzpay;
            float notpay = float.Parse(shouldpay) - float.Parse(havepay)-float.Parse(yzpay);

            gzgz = gzgz == "" ? "0" : gzgz;
            string companyid = (string)CurrentUser.GetSession("INFODWBH");

            string sql = "Update KqjUserMonthPay  SET havepay='" + havepay
                //+ "',notpay='" + notpay.ToString()
                + "',shouldpay='" + shouldpay.ToString()
                + "',yzpay='" + yzpay.ToString()
                + "' where userid='" + userid + "'and logyear='" + year + "' and logmonth='" + month + "' and jdzch='" + jdzch + "';";
         
            //string sql = "Update KqjUserMonthLog  SET havepay='" + havepay
            //    + "',notpay='" + notpay.ToString()
            //    + "',shouldpay='" + shouldpay.ToString()
            //    + "' where userid='" + userid + "'and logyear='" + year + "' and logmonth='" + month + "' and jdzch='" + jdzch + "';";
         
            ret=CommonDao.ExecCommand(sql,CommandType.Text);
            
            }
            catch (Exception ex)
            {
                return false;
            }
            return ret;

        }
     

        #region 进退场时间更新
        public bool UpdateUserInTime(string sfzhm, string jdzch,DateTime dt)
        {
            bool ret = false;
            string sql = "select * from I_M_RY_History where jdzch=@jdzch and sfzhm=@sfzhm";
            IList<IDataParameter> arrParams = new List<IDataParameter>();
            arrParams.Add(new SqlParameter("@jdzch", jdzch));
            arrParams.Add(new SqlParameter("@sfzhm", sfzhm));

            IList<IDictionary<string, string>> historys = CommonDao.GetDataTable(sql, CommandType.Text, arrParams);
            // 没有进出工地记录
            if ((historys == null || historys.Count == 0))
            {
                SaveInfoWgryHistory(sfzhm,jdzch, false,dt);
            }
            else
            {
                var q = from e in historys where e["outtime"].GetSafeString() == "" select e;
              //  var q = from e in historys where e.OutTime == null select e;
                if (q.Count() == 0) //记录同时有进场记录和退场记录，表示该考勤为新进场
                    SaveInfoWgryHistory(sfzhm,jdzch, false, dt);
                else //有进场,无退场表示在该项目工作
                    ret = true;
            }
            return true;
        }

        /// <summary>
        /// 保存务工人员历史记录
        /// </summary>
        /// <param name="itm"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SaveInfoWgryHistory(string sfzhm,string jdzch, bool isOut, DateTime? logdate = null)
        {
          //  string sql="Insert Into I_M_RY_History SET SFZHM"

            //// 获取人员工程记录
            string sql = "select * from I_M_RY_History where sfzhm=@sfzhm";
            IList<IDataParameter> arrParams = new List<IDataParameter>();
            arrParams.Add(new SqlParameter("@sfzhm", sfzhm));

            IList<IDictionary<string, string>> historys = CommonDao.GetDataTable(sql, CommandType.Text, arrParams);
            // 没有进工地记录，保存出工地记录，抛弃
            if ((historys == null || historys.Count == 0) && isOut)
                return false;
            // 获取人员信息
            IDictionary<string, string> inforow = CommonDao.GetRowValue("sfzhm,ryxm,r_qybh,rybh,gw", "View_GC_RY_QYRYCK", "sfzhm='" + sfzhm + "'and gcbh='" + jdzch + "' ");
            if (inforow == null)
                return false;
            string ryxm, qybh, qymc , gcmc,gw,gz="1";

            inforow.TryGetValue("ryxm", out ryxm);
            inforow.TryGetValue("r_qybh", out qybh);
            inforow.TryGetValue("gw", out gw);

            qymc = CommonDao.GetFieldValue("qymc", "i_m_qy", "qybh_yc='" + qybh + "'");
            gcmc = CommonDao.GetFieldValue("gcmc", "i_m_gc", "gcbh='" + jdzch + "'");        
            bool ret = true;
            try
            {
                // 如果是进工地，查找未登记退出工地的记录，并新增入工地记录
                // 如果是出工地，查找未登记退出工地并且工地信息相同的记录
                if (isOut)
                {
                    var q = from e in historys where e["outtime"].GetSafeString() == "" && e["jdzch"] == jdzch && e["qybh"] == qybh select e;

                }
                else
                {

                    var q = from e in historys where e["outtime"].GetSafeString() == "" select e;
                    bool InCurrGC = false;
                    foreach (IDictionary<string, string> hist in q) //设置所有以前的工程的退场时间为今天
                    {
                        if (qybh == hist["qybh"].GetSafeString() && jdzch == hist["jdzch"].GetSafeString()) // 为目前所在工程
                        {
                            InCurrGC = true;
                        }
                        else
                        {
                           // string outtime = DateTime.Now.ToString();
                            //InfoWgryHistoryDao.Save(hist);
                        }
                    }
                    if (!InCurrGC) //如果没有退场记录的工程为下发模板的工程，则不用执行
                    {
                        string intime = logdate.ToString();// DateTime.Now.ToString();
                        sql = "Insert INTO i_m_ry_history (SFZHM,RYXM,QYBH,QYMC,JDZCH,GCMC,GW,intime) values (@sfzhm,@ryxm,@qybh,@qymc,@jdzch,@gcmc,@gw,@intime)";
                        IList<IDataParameter> Params = new List<IDataParameter>();
                        Params.Add(new SqlParameter("@sfzhm", sfzhm));
                        Params.Add(new SqlParameter("@ryxm", ryxm));
                        Params.Add(new SqlParameter("@qybh", qybh));
                        Params.Add(new SqlParameter("@qymc", qymc));
                        Params.Add(new SqlParameter("@jdzch", jdzch));
                        Params.Add(new SqlParameter("@gcmc", gcmc));
                        Params.Add(new SqlParameter("@gw", gw));
                        Params.Add(new SqlParameter("@intime", intime));
                        CommonDao.ExecCommandOpenSession(sql, CommandType.Text, Params);
                       // InfoWgryHistoryDao.Save(histnew);
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

        #endregion
        #endregion
    }
}
