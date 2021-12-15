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
	class KqUserSignDao : HibernateDaoSupport, IKqUserSignDao
	{
		public IList<KqUserSign> GetAll()
		{
			IList<KqUserSign> ret = new List<KqUserSign>();
			try
			{
				ret = HibernateTemplate.LoadAll<KqUserSign>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public KqUserSign Get(int id)
		{
			KqUserSign ret = new KqUserSign();
			try
			{
				ret = HibernateTemplate.Get<KqUserSign>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public KqUserSign Save(KqUserSign itm)
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

		public void Update(KqUserSign itm)
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

		public void Delete(KqUserSign itm)
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
				HibernateTemplate.Delete(string.Format("from KqUserSign where Recid={0}", id));
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
				HibernateTemplate.Delete("from KqUserSign");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}

        public KqUserSign Get(string username, string signdate)
        {
            KqUserSign ret = new KqUserSign();
            try
            {
                IList<KqUserSign> hosts = HibernateTemplate.Find<KqUserSign>("from KqUserSign where UserCode='" + username + "' and SignDate='" + signdate + "'");
                if (hosts.Count > 0)
                    ret= hosts[0];
                else
                    ret = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public void Updatesign(KqUserSign itm)
        {
            try
            {
                HibernateTemplate.SaveOrUpdate(itm);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

	}
}