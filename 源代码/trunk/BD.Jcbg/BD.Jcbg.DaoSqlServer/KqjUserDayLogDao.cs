using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.IDao;
using NHibernate;
using Spring.Data.NHibernate.Generic.Support;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.DaoSqlServer
{
	/// <summary>
	/// 日考勤日志表操作
	/// </summary>
	public class KqjUserDayLogDao : HibernateDaoSupport, IKqjUserDayLogDao
	{
		/// <summary>
		/// 根据人员和工地，日期获取考勤记录，获取当天和前一天的
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		public IList<KqjUserDayLog> Gets(string userid, string companyid, string projectid, DateTime dt)
		{
			IList<KqjUserDayLog> ret = new List<KqjUserDayLog>();
			try
			{
               // string hql = "from BD.Jcbg.DataModal.Entities.KqjUserDayLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and CompanyId='" + companyid + "' and (LogDay=convert(datetime,'" + dt.ToString("yyyy-MM-dd") + "') or LogDay=convert(datetime,'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "')) order by LogDay desc";
                string hql = "from KqjUserDayLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and CompanyId='" + companyid + "' and (LogDay=convert(datetime,'" + dt.ToString("yyyy-MM-dd") + "') or LogDay=convert(datetime,'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "')) order by LogDay desc"; ; 
                ret = HibernateTemplate.Find<KqjUserDayLog>(hql);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog("", e);
			}
			return ret;
		}
        /// <summary>
        /// 根据人员和工地，日期获取考勤记录，获取当天的记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public IList<KqjUserDayLog> GetDayLogs(string userid, string companyid, string projectid, DateTime dt, InfoSchedule schedule)
        {
            IList<KqjUserDayLog> ret = new List<KqjUserDayLog>();
            try
            {
                string hql = "from KqjUserDayLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and CompanyId='" + companyid + "' and LogDay=convert(datetime,'" + dt.ToString("yyyy-MM-dd") + "') and ScheduleId='" + schedule.Recid + "' order by LogDay desc";
                ret = HibernateTemplate.Find<KqjUserDayLog>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog("", e);
            }
            return ret;
        }

		/// <summary>
		/// 保存考勤记录
		/// </summary>
		/// <param name="log"></param>
		/// <returns></returns>
		public bool Save(KqjUserDayLog log)
		{
			bool ret = true;
			try
			{
				HibernateTemplate.SaveOrUpdate(log);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			return ret;
		}
		/// <summary>
		/// 根据记录号获取记录
		/// </summary>
		/// <param name="recid"></param>
		/// <returns></returns>
		public KqjUserDayLog Get(int recid)
		{
			return HibernateTemplate.Get<KqjUserDayLog>(recid);
		}

		/// <summary>
		/// 根据单位删除记录
		/// </summary>
		/// <param name="companyid"></param>
		/// <returns></returns>
		public bool Delete(string companyid)
		{
			bool ret = true;
			try
			{
				string hql = "from KqjUserDayLog";
				if (companyid != "")
					hql += " where CompanyId='" + companyid + "' ";
				string delete1 = "from KqjUserDayLogDetail where ParentId in (select Recid " + hql + ")";
				string delete2 = "from KqjUserMonthLog where CompanyId='" + companyid + "'";
				HibernateTemplate.Delete(delete1);
				HibernateTemplate.Delete(delete2);
				HibernateTemplate.Delete(hql);
				
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			return ret;
		}
	}
}
