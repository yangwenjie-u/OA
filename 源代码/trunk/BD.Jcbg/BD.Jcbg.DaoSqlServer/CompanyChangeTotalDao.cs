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
    public class CompanyChangeTotalDao : HibernateDaoSupport, ICompanyChangeTotalDao
    {
        public IList<CompanyChangeTotal> GetAll()
        {
            IList<CompanyChangeTotal> ret = new List<CompanyChangeTotal>();
            try
            {
                ret = HibernateTemplate.LoadAll<CompanyChangeTotal>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChangeTotal Get(int id)
        {
            CompanyChangeTotal ret = new CompanyChangeTotal();
            try
            {
                ret = HibernateTemplate.Get<CompanyChangeTotal>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChangeTotal Save(CompanyChangeTotal itm)
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

        public void Update(CompanyChangeTotal itm)
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

        public void Delete(CompanyChangeTotal itm)
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
                HibernateTemplate.Delete(string.Format("from CompanyChangeTotal where ChangeTotalID={0}", id));
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
                HibernateTemplate.Delete("from CompanyChangeTotal");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
