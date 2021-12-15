using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Spring.Transaction.Interceptor;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Stereotype;
using NHibernate;
using NHibernate.Criterion;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IDao;
using BD.Jcbg.Common;

namespace BD.Jcbg.DaoSqlServer
{
	public class ViewSelfDesktopItemDao : HibernateDaoSupport, IViewSelfDesktopItemDao
	{
		public ViewSelfDesktopItem Get(int id)
		{
			ViewSelfDesktopItem ret = new ViewSelfDesktopItem();
			try
			{
				ret = HibernateTemplate.Get<ViewSelfDesktopItem>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public IList<ViewSelfDesktopItem> GetAll()
		{
			IList<ViewSelfDesktopItem> ret = new List<ViewSelfDesktopItem>();
			try
			{
				ret = HibernateTemplate.LoadAll<ViewSelfDesktopItem>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public IList<ViewSelfDesktopItem> GetByUser(string username)
		{
			IList<ViewSelfDesktopItem> ret = new List<ViewSelfDesktopItem>();
			try
			{
				string hql = "from ViewSelfDesktopItem where UserName='" + username + "' order by DisplayOrder";
				ret = HibernateTemplate.Find<ViewSelfDesktopItem>(hql);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
