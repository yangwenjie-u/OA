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
    public class SysycdyfieldDao : HibernateDaoSupport, ISysycdyfieldDao
    {
        public IList<SysYcdyField> GetAll()
        {
            IList<SysYcdyField> ret = new List<SysYcdyField>();
            try
            {
                ret = HibernateTemplate.LoadAll<SysYcdyField>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyField Get(int id)
        {
            SysYcdyField ret = new SysYcdyField();
            try
            {
                ret = HibernateTemplate.Get<SysYcdyField>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyField Save(SysYcdyField itm)
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

        public void Update(SysYcdyField itm)
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

        public void Delete(SysYcdyField itm)
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
                HibernateTemplate.Delete(string.Format("from SysYcdyField where RECID={0}", id));
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
                HibernateTemplate.Delete("from SysYcdyField");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public IList<SysYcdyField> Gets(string callid)
        {
            string hql = "from SysYcdyField where CallId='" + callid + "'";
            return HibernateTemplate.Find<SysYcdyField>(hql);
        }
    }
}
