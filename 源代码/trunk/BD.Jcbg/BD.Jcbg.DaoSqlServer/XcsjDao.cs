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
	public class XcsjDao : HibernateDaoSupport, IXcsjDao
	{
		public IList<Xcsj> Gets()
		{
			string hql = "from Xcsj order by Sy";
			return HibernateTemplate.Find<Xcsj>(hql);
		}
		public Xcsj Get(string commsylb)
		{
			string hql = "from Xcsj where Commsylb='"+commsylb+"'";
			IList<Xcsj> rows = HibernateTemplate.Find<Xcsj>(hql);
			if (rows.Count > 0)
				return rows[0];
			return null;
		}
	}
}
