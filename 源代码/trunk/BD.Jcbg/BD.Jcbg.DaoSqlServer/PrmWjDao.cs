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
    public class PrmWjDao : HibernateDaoSupport, IPrmWjDao
    {
        public IList<PRMWJ> GetAll()
        {
            IList<PRMWJ> ret = new List<PRMWJ>();
            try
            {
                ret = HibernateTemplate.LoadAll<PRMWJ>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public PRMWJ Get(int id)
        {
            PRMWJ ret = new PRMWJ();
            try
            {
                ret = HibernateTemplate.Get<PRMWJ>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public PRMWJ Save(PRMWJ itm)
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

        public void Update(PRMWJ itm)
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

        public void Delete(PRMWJ itm)
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
                HibernateTemplate.Delete(string.Format("from PR_M_WJ where Recid={0}", id));
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
                HibernateTemplate.Delete("from PR_M_WJ");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
