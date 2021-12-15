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
    public class PrmDywjDao : HibernateDaoSupport, IPrmDywjDao
    {
        public IList<PRMDYWJ> GetAll()
        {
            IList<PRMDYWJ> ret = new List<PRMDYWJ>();
            try
            {
                ret = HibernateTemplate.LoadAll<PRMDYWJ>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public PRMDYWJ Get(int id)
        {
            PRMDYWJ ret = new PRMDYWJ();
            try
            {
                ret = HibernateTemplate.Get<PRMDYWJ>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public PRMDYWJ Save(PRMDYWJ itm)
        {
            try
            {
                HibernateTemplate.Save(itm);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return itm;
        }

        public void Update(PRMDYWJ itm)
        {
            try
            {
                HibernateTemplate.SaveOrUpdateCopy(itm);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
        }

        public void Delete(PRMDYWJ itm)
        {
            try
            {
                HibernateTemplate.Delete(itm);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
        }
        public void Delete(int id)
        {
            try
            {
                HibernateTemplate.Delete(string.Format("from PR_M_DYWJ where Recid={0}", id));
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
                HibernateTemplate.Delete("from PR_M_DYWJ");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
