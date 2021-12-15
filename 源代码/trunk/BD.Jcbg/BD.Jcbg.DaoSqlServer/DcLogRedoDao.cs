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
	public class DcLogRedoDao : HibernateDaoSupport,  IDcLogRedoDao
	{
		public IList<DcLogRedo> Gets(string uniqcode)
		{
			string hql = "from DcLogRedo where UniqCode='" + uniqcode + "' order by Recid";
			return HibernateTemplate.Find<DcLogRedo>(hql);
		}
		public DcLogRedo Get(int recid)
		{
			string hql = "from DcLogRedo where Recid=" + recid ;
			IList<DcLogRedo> rows = HibernateTemplate.Find<DcLogRedo>(hql);
			if (rows.Count > 0)
				return rows[0];
			return null;
		}
	}
}
