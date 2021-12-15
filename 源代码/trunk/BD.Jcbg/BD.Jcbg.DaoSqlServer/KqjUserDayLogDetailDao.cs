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
	/// 日考勤日志详情表操作
	/// </summary>
	public class KqjUserDayLogDetailDao : HibernateDaoSupport, IKqjUserDayLogDetailDao
	{
		/// <summary>
		/// 根据人员、工地、公司、日期获取考勤记录详情,获取当天和前一天的
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		public IList<KqjUserDayLogDetail> Gets(string userid, string companyid, string projectid, DateTime dt)
		{
			IList<KqjUserDayLogDetail> ret = null;
			try
			{
                string hql = "from KqjUserDayLogDetail where ParentId in (select Recid from KqjUserDayLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and CompanyId='" + companyid + "' and (LogDay=convert(datetime,'" + dt.ToString("yyyy-MM-dd") + "') or LogDay=convert(datetime,'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'))) order by ParentId asc,InTime desc";
				ret = HibernateTemplate.Find<KqjUserDayLogDetail>(hql);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog("", e);
			}
			return ret;
		}

        public IList<KqjUserDayLogDetail> GetsOut(string userid, string companyid, string projectid, DateTime dt)
        {
            IList<KqjUserDayLogDetail> ret = null;
            try
            {
                string hql = "from KqjUserDayLogDetail where ParentId in (select Recid from KqjUserDayLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and CompanyId='" + companyid + "') and (OutTime is null or InTime=OutTime) order by ParentId asc,InTime desc";
                ret = HibernateTemplate.Find<KqjUserDayLogDetail>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog("", e);
            }
            return ret;
        }
        public IList<KqjUserDayLogDetail> GetsIn(string userid, string companyid, string projectid, DateTime dt)
        {
            IList<KqjUserDayLogDetail> ret = null;
            try
            {
                string hql = "from KqjUserDayLogDetail where  InTime is null and DATEDIFF(hh,'" + dt + "',OutTime)<18  and ParentId in (select Recid from KqjUserDayLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and CompanyId='" + companyid + "' and (LogDay=convert(datetime,'" + dt.ToString("yyyy-MM-dd") + "') or LogDay=convert(datetime,'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'))) order by ParentId asc,OutTime asc";
                ret = HibernateTemplate.Find<KqjUserDayLogDetail>(hql);
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
		public bool Save(KqjUserDayLogDetail log)
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
	}
}
