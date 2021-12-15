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
    public class DcLogDao : HibernateDaoSupport, IDcLogDao
	{
        public DcLog Get(int recid)
		{
            string hql = "from DcLog where Recid=" + recid;
            IList<DcLog> rows = HibernateTemplate.Find<DcLog>(hql);
			if (rows.Count > 0)
				return rows[0];
			return null;
		}
	}
}
