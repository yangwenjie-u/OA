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
    public class CompanyChangeTotalItemDao : HibernateDaoSupport, ICompanyChangeTotalItemDao
    {
        public IList<CompanyChangeTotalItem> GetAll()
        {
            IList<CompanyChangeTotalItem> ret = new List<CompanyChangeTotalItem>();
            try
            {
                ret = HibernateTemplate.LoadAll<CompanyChangeTotalItem>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChangeTotalItem Get(int id)
        {
            CompanyChangeTotalItem ret = new CompanyChangeTotalItem();
            try
            {
                ret = HibernateTemplate.Get<CompanyChangeTotalItem>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChangeTotalItem Save(CompanyChangeTotalItem itm)
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

        public void Update(CompanyChangeTotalItem itm)
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

        public void Delete(CompanyChangeTotalItem itm)
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
                HibernateTemplate.Delete(string.Format("from CompanyChangeTotalItem where ChangeTotalItemID={0}", id));
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
                HibernateTemplate.Delete("from CompanyChangeTotalItem");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        public IList<CompanyChangeTotalItem> GetItems(int changetotalid)
        {
            IList<CompanyChangeTotalItem> ret = new List<CompanyChangeTotalItem>();
            try
            {
                ret = HibernateTemplate.Find<CompanyChangeTotalItem>(string.Format("from CompanyChangeTotalItem where ChangeTotalID={0} order by DepartmentId asc ", changetotalid));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
    }
}
