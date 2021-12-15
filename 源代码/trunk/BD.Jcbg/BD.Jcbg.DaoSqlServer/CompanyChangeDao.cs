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
    public class CompanyChangeDao : HibernateDaoSupport, ICompanyChangeDao
    {
        public IList<CompanyChange> GetAll()
        {
            IList<CompanyChange> ret = new List<CompanyChange>();
            try
            {
                ret = HibernateTemplate.LoadAll<CompanyChange>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChange Get(int id)
        {
            CompanyChange ret = new CompanyChange();
            try
            {
                ret = HibernateTemplate.Get<CompanyChange>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChange Save(CompanyChange itm)
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

        public void Update(CompanyChange itm)
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

        public void Delete(CompanyChange itm)
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
                HibernateTemplate.Delete(string.Format("from CompanyChange where ChangeTotalID={0}", id));
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
                HibernateTemplate.Delete("from CompanyChange");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public IList<CompanyChange> GetItems(string start,string  end)
        {
            IList<CompanyChange> ret = new List<CompanyChange>();
            try
            {
                ret = HibernateTemplate.Find<CompanyChange>("from CompanyChange where convert(datetime,CreatedOn)>='" + start + "' and convert(datetime,CreatedOn)<='" + end + "' order by DepartmentId");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
    }
}
