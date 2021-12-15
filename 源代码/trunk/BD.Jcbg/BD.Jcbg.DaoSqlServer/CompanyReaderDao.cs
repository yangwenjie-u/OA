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
	public class CompanyReaderDao : HibernateDaoSupport, ICompanyReaderDao
	{
		public IList<CompanyReader> GetAll()
		{
			IList<CompanyReader> ret = new List<CompanyReader>();
			try
			{
				ret = HibernateTemplate.LoadAll<CompanyReader>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public CompanyReader Get(int id)
		{
			CompanyReader ret = new CompanyReader();
			try
			{
				ret = HibernateTemplate.Get<CompanyReader>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public CompanyReader Save(CompanyReader itm)
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

		public void Update(CompanyReader itm)
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

		public void Delete(CompanyReader itm)
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
				HibernateTemplate.Delete(string.Format("from CompanyReader where Recid={0}", id));
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
				HibernateTemplate.Delete("from CompanyReader");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
		/// <summary>
		/// 根据某个实体id删除
		/// </summary>
		/// <param name="entityname"></param>
		/// <param name="entityid"></param>
		public void Delete(string entityname, string entityid)
		{
			try
			{
				HibernateTemplate.Delete(string.Format("from CompanyReader where ParentEntity='{0}' and ParentId='{1}'", 
					entityname, entityid));
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}

		/// <summary>
		/// 获取某个实体的读者
		/// </summary>
		/// <param name="entityname"></param>
		/// <param name="entityid"></param>
		/// <returns></returns>
		public IList<CompanyReader> Gets(string entityname, string entityid)
		{
			IList<CompanyReader> ret = new List<CompanyReader>();
			try
			{
				ret = HibernateTemplate.Find<CompanyReader>(string.Format("from CompanyReader where ParentEntity='{0}' and ParentId='{1}'",
					entityname, entityid));
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}
