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
	public class CompanyAnnounceDao : HibernateDaoSupport, ICompanyAnnounceDao
	{
		public IList<CompanyAnnounce> GetAll()
		{
			IList<CompanyAnnounce> ret = new List<CompanyAnnounce>();
			try
			{
				ret = HibernateTemplate.LoadAll<CompanyAnnounce>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public CompanyAnnounce Get(int id)
		{
			CompanyAnnounce ret = new CompanyAnnounce();
			try
			{
				ret = HibernateTemplate.Get<CompanyAnnounce>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public CompanyAnnounce Save(CompanyAnnounce itm)
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

		public void Update(CompanyAnnounce itm)
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

		public void Delete(CompanyAnnounce itm)
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
				HibernateTemplate.Delete(string.Format("from CompanyAnnounce where Recid={0}", id));
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
				HibernateTemplate.Delete("from CompanyAnnounce");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}
