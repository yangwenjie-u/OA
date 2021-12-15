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
	/// 人员单位工程历史
	/// </summary>
	public class InfoWgryHistoryDao : HibernateDaoSupport, IInfoWgryHistoryDao
	{
		/// <summary>
		/// 保存信息
		/// </summary>
		/// <param name="itm"></param>
		/// <returns></returns>
		public bool Save(InfoWgryHistory itm)
		{
			HibernateTemplate.SaveOrUpdate(itm);
			return true;
		}
		/// <summary>
		/// 获取某个人，在某个单位的历史的信息，如果没有单位，返回所有单位的
		/// </summary>
		/// <param name="sfzh"></param>
		/// <param name="companyid"></param>
		/// <returns></returns>
		public IList<InfoWgryHistory> Gets(string sfzh, string companyid,string projectid="")
		{
			StringBuilder hql = new StringBuilder("from InfoWgryHistory where Sfzhm='" + sfzh + "' ");
			if (companyid != "")
				hql.Append(" and CompanyId='" + companyid + "' ");
            if (projectid != "")
                hql.Append(" and ProjectId='" + projectid + "' ");
			hql.Append(" order by InTime asc");
			return HibernateTemplate.Find<InfoWgryHistory>(hql.ToString());
		}
        /// <summary>
        /// 获取没有出场纪录的记录
        /// </summary>
        /// <param name="sfzh"></param>
        /// <param name="companyid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public IList<InfoWgryHistory> GetsOut(string sfzh, string companyid, string projectid = "")
        {
            StringBuilder hql = new StringBuilder("from InfoWgryHistory where Sfzhm='" + sfzh + "' ");
            if (companyid != "")
                hql.Append(" and CompanyId='" + companyid + "' ");
            if (projectid != "")
                hql.Append(" and ProjectId='" + projectid + "' ");
            hql.Append(" and OutTime is null ");
            hql.Append(" order by InTime asc");
            return HibernateTemplate.Find<InfoWgryHistory>(hql.ToString());
        }
		/// <summary>
		/// 获取历史信息
		/// </summary>
		/// <param name="sfzh"></param>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <param name="companyname"></param>
		/// <param name="projectname"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindex"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public IList<InfoWgryHistory> Gets(string sfzh, string realname, string companyid, string projectid, string companyname, string projectname,
			int pagesize, int pageindex, out int totalCount)
		{
			StringBuilder where = new StringBuilder("");
			if (sfzh != "")
				where.Append(" and Sfzhm='" + sfzh + "' ");
			if (companyid != "")
				where.Append(" and CompanyId='" + companyid + "' ");
			if (projectid != "")
				where.Append(" and ProjectId='" + projectid + "' ");
			if (realname != "")
				where.Append(" and RealName like '%" + realname + "%' ");
			if (companyname != "")
				where.Append(" and CompanyName like '%" + companyname + "%' ");
			if (projectname != "")
				where.Append(" and ProjectName like '%" + projectname + "%' ");
			string hqlcount = "select count(*) from InfoWgryHistory where 1=1 " + where.ToString();
			string hql = "from InfoWgryHistory where 1=1 " + where.ToString() + " order by SInTime asc";
			totalCount = DataFormat.GetSafeInt(Session.CreateQuery(hqlcount).UniqueResult());

			return Session.CreateQuery(hql.ToString()).SetFirstResult(pagesize * (pageindex - 1)).SetMaxResults(pagesize).List<InfoWgryHistory>();
		}
	}
}
