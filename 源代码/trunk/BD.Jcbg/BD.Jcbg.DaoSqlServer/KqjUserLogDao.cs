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
	/// 考勤日志表操作
	/// </summary>
	public class KqjUserLogDao : HibernateDaoSupport, IKqjUserLogDao
	{
		/// <summary>
		/// 获取一条考勤日志
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public KqjUserLog Get(int id)
		{
			return HibernateTemplate.Get<KqjUserLog>(id);
		}
		/// <summary>
		/// 保存一条考勤日志
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool Save(KqjUserLog entity)
		{
			return HibernateTemplate.Save(entity) != null;
		}
		/// <summary>
		/// 删除一条考勤日志
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Delete(int id)
		{
			return HibernateTemplate.Delete(string.Format("from KqjUserLog where Recid={0}", id)) > 0;
		}
		/// <summary>
		/// 获取一个单位的考勤记录
		/// </summary>
		/// <param name="companyid"></param>
		/// <returns></returns>
		public IList<KqjUserLog> Gets(string companyid)
		{
			string hql = "from KqjUserLog";
			if (companyid.Length>0)
				hql += " where CompanyId='"+companyid+"' ";
			hql += " order by LogDate asc";
			return HibernateTemplate.Find<KqjUserLog>(hql);
		}

		/// <summary>
		/// 获取某个人在某个地方某天的考勤记录
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="projectid"></param>
		/// <param name="logday"></param>
		/// <returns></returns>
		public IList<KqjUserLog> Gets(string userid, string projectid, DateTime logday)
		{
            string hql = "from BD.Jcbg.DataModal.Entities.KqjUserLog where UserId='" + userid + "' and ProjectId='" + projectid + "' and LogDate>=convert(datetime,'" + logday.ToString("yyyy-MM-dd") + " 00:00:00') and LogDate<=convert(datetime,'" + logday.ToString("yyyy-MM-dd") + " 23:59:59') and LogType<>''";
			
			hql += " order by LogDate asc";
			return HibernateTemplate.Find<KqjUserLog>(hql);
		}
	}
}
