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
	public class HelpDesktopItemDao:HibernateDaoSupport, IHelpDesktopItemDao
	{
		public IList<HelpDesktopItem> GetAll()
		{
			IList<HelpDesktopItem> ret = new List<HelpDesktopItem>();
			try
			{
				ret = HibernateTemplate.LoadAll<HelpDesktopItem>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public HelpDesktopItem Get(string id)
		{
			HelpDesktopItem ret = new HelpDesktopItem();
			try
			{
				ret = HibernateTemplate.Get<HelpDesktopItem>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public HelpDesktopItem Save(HelpDesktopItem itm)
		{
			try
			{
				HibernateTemplate.Save(itm);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return itm;
		}

		public void Update(HelpDesktopItem itm)
		{
			try
			{
				HibernateTemplate.SaveOrUpdateCopy(itm);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}

		public void Delete(HelpDesktopItem itm)
		{
			try
			{
				HibernateTemplate.Delete(itm);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
		public void Delete(string id)
		{
			try
			{
				HibernateTemplate.Delete(string.Format("from HelpDesktopItem where Recid='{0}'", id));
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
		public void Clear()
		{
			try
			{
				HibernateTemplate.Delete("from HelpDesktopItem");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}
