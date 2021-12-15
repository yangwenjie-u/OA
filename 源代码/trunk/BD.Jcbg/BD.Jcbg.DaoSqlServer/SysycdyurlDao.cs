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
    public class SysycdyurlDao:HibernateDaoSupport, ISysycdyurlDao
    {
        public IList<SysYcdyUrl> GetAll()
        {
            IList<SysYcdyUrl> ret = new List<SysYcdyUrl>();
            try
            {
                ret = HibernateTemplate.LoadAll<SysYcdyUrl>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyUrl Get(int id)
        {
            SysYcdyUrl ret = new SysYcdyUrl();
            try
            {
                ret = HibernateTemplate.Get<SysYcdyUrl>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyUrl Save(SysYcdyUrl itm)
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

        public void Update(SysYcdyUrl itm)
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

        public void Delete(SysYcdyUrl itm)
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
                HibernateTemplate.Delete(string.Format("from SysYcdyUrl where RECID={0}", id));
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
                HibernateTemplate.Delete("from SysYcdyUrl");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public IList<SysYcdyUrl> Gets(string callid)
        {
            string hql = "from SysYcdyUrl where CallId='" + callid + "'";
            return HibernateTemplate.Find<SysYcdyUrl>(hql);
        }
    }
}
