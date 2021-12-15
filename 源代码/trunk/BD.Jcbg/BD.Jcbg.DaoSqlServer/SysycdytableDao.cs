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
    public class SysycdytableDao : HibernateDaoSupport, ISysycdytableDao
    {
        public IList<SysYcdyTable> GetAll()
        {
            IList<SysYcdyTable> ret = new List<SysYcdyTable>();
            try
            {
                ret = HibernateTemplate.LoadAll<SysYcdyTable>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyTable Get(int id)
        {
            SysYcdyTable ret = new SysYcdyTable();
            try
            {
                ret = HibernateTemplate.Get<SysYcdyTable>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyTable Save(SysYcdyTable itm)
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

        public void Update(SysYcdyTable itm)
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

        public void Delete(SysYcdyTable itm)
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
                HibernateTemplate.Delete(string.Format("from SysYcdyTable where RECID={0}", id));
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
                HibernateTemplate.Delete("from SysYcdyTable");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public IList<SysYcdyTable> Gets(string callid)
        {
            string hql = "from SysYcdyTable where CallId='" + callid + "'";
            return HibernateTemplate.Find<SysYcdyTable>(hql);
        }
    }
}
