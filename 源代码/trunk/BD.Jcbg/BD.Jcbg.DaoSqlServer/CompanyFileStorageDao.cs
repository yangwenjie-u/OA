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
	public class CompanyFileStorageDao : HibernateDaoSupport, ICompanyFileStorageDao
	{
		public IList<CompanyFileStorage> GetAll()
		{
			IList<CompanyFileStorage> ret = new List<CompanyFileStorage>();
			try
			{
				ret = HibernateTemplate.LoadAll<CompanyFileStorage>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public CompanyFileStorage Get(int id)
		{
			CompanyFileStorage ret = new CompanyFileStorage();
			try
			{
				ret = HibernateTemplate.Get<CompanyFileStorage>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public CompanyFileStorage Save(CompanyFileStorage itm)
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

		public void Update(CompanyFileStorage itm)
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

		public void Delete(CompanyFileStorage itm)
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
				HibernateTemplate.Delete(string.Format("from CompanyFileStorage where Recid={0}", id));
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
				HibernateTemplate.Delete("from CompanyFileStorage");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
		/// <summary>
		/// 根据多个id获取文件
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		public IList<CompanyFileStorage> Gets(string ids)
		{
			IList<CompanyFileStorage> ret = new List<CompanyFileStorage>();
			try
			{
				ret = HibernateTemplate.Find<CompanyFileStorage>("from CompanyFileStorage where Recid in ("+ids.Trim(new char[]{','})+")");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
