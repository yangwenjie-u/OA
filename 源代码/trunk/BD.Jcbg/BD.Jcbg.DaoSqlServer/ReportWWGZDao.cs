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
	class ReportWWGZDao : HibernateDaoSupport, IReportWWGZDao
    {
		public IList<ReportWWGZ> GetAll()
		{
			IList<ReportWWGZ> ret = new List<ReportWWGZ>();
			try
			{
				ret = HibernateTemplate.LoadAll<ReportWWGZ>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public ReportWWGZ Get(int id)
		{
			ReportWWGZ ret = new ReportWWGZ();
			try
			{
				ret = HibernateTemplate.Get<ReportWWGZ>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public ReportWWGZ Save(ReportWWGZ itm)
		{
			try
			{
                itm.RECID = DataFormat.GetSafeInt(HibernateTemplate.Save(itm));
				
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return itm;
		}

		public void Update(ReportWWGZ itm)
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

		public void Delete(ReportWWGZ itm)
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
				HibernateTemplate.Delete(string.Format("from ReportWWGZ where RECID={0}", id));
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
				HibernateTemplate.Delete("from ReportWWGZ");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}