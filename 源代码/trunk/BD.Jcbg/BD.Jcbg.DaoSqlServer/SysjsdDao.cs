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
	public class SysjsdDao : HibernateDaoSupport, ISysjsdDao
	{
		public IList<Sysjsd> Gets(string commsylb="")
		{
			string hql = "from Sysjsd ";
			if (commsylb != "")
				hql += " where Commsylb='"+commsylb+"' order by BlockNumber,DisplayOrder ";
			return HibernateTemplate.Find<Sysjsd>(hql);
		}
	}
}
