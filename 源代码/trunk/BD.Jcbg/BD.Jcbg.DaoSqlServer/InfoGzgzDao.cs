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
	/// 工种工作操作
	/// </summary>
	public class InfoGzgzDao : HibernateDaoSupport, IInfoGzgzDao
	{
		/// <summary>
		/// 获取一个单位一个工地的工种工资设置
		/// </summary>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		public IList<InfoGzgz> Gets(string companyid, string projectid)
		{
			return HibernateTemplate.Find<InfoGzgz>("from InfoGzgz where CompanyId='" + companyid + "' and ProjectId='" + projectid + "' and HasDelete=0");
		}
		/// <summary>
		/// 获取一个单位某个工地某个班的某个工种的工资设置
		/// </summary>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <param name="scheduleid"></param>
		/// <param name="gzid"></param>
		/// <returns></returns>
		public InfoGzgz Get(string companyid, string projectid, string scheduleid, string gzid)
		{
			IList<InfoGzgz> rows = HibernateTemplate.Find<InfoGzgz>("from InfoGzgz where CompanyId='" + companyid + "' and ProjectId='" + projectid + "' and ScheduleId='" + scheduleid + "' and GzId='" + gzid + "' and HasDelete=0");
			if (rows.Count == 0)
				return null;
			return rows[0];

		}
	}
}
