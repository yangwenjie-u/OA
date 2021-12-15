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
	public class SelfDesktopItemDao : HibernateDaoSupport, ISelfDesktopItemDao
	{
		public IList<SelfDesktopItem> GetAll()
		{
			IList<SelfDesktopItem> ret = new List<SelfDesktopItem>();
			try
			{
				ret = HibernateTemplate.LoadAll<SelfDesktopItem>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public SelfDesktopItem Get(int id)
		{
			SelfDesktopItem ret = new SelfDesktopItem();
			try
			{
				ret = HibernateTemplate.Get<SelfDesktopItem>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public SelfDesktopItem Save(SelfDesktopItem itm)
		{
			try
			{
				int recid = HibernateTemplate.Save(itm).GetSafeInt();
				itm.Recid = recid;
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return itm;
		}

		public void Update(SelfDesktopItem itm)
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

		public void Delete(SelfDesktopItem itm)
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
		public void Delete(int id)
		{
			try
			{
				HibernateTemplate.Delete(string.Format("from SelfDesktopItem where Recid={0}", id));
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
				HibernateTemplate.Delete("from SelfDesktopItem");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}

		/// <summary>
		/// 获取用户桌面项
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public IList<SelfDesktopItem> GetByUser(string username)
		{
			IList<SelfDesktopItem> ret = new List<SelfDesktopItem>();
			try
			{

				string hql = "from SelfDesktopItem where UserName='" + username + "' order by DisplayOrder";
				ret = HibernateTemplate.Find<SelfDesktopItem>(hql);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
