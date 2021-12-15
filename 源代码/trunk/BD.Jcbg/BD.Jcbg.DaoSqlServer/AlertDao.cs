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
	class AlertDao : HibernateDaoSupport, IAlertDao
    {
		public IList<Alert> GetAll()
		{
			IList<Alert> ret = new List<Alert>();
			try
			{
				ret = HibernateTemplate.LoadAll<Alert>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public Alert Get(int id)
		{
			Alert ret = new Alert();
			try
			{
				ret = HibernateTemplate.Get<Alert>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public Alert Save(Alert itm)
		{
			try
			{
                itm.AlertID = DataFormat.GetSafeInt(HibernateTemplate.Save(itm));
				
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return itm;
		}

		public void Update(Alert itm)
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

		public void Delete(Alert itm)
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
				HibernateTemplate.Delete(string.Format("from Alert where alertid={0}", id));
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
				HibernateTemplate.Delete("from Alert");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}