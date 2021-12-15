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
    public class SysycdytablerelationDao : HibernateDaoSupport, ISysycdytablerelationDao
    {
        public IList<SysYcdyTableRelation> GetAll()
        {
            IList<SysYcdyTableRelation> ret = new List<SysYcdyTableRelation>();
            try
            {
                ret = HibernateTemplate.LoadAll<SysYcdyTableRelation>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyTableRelation Get(int id)
        {
            SysYcdyTableRelation ret = new SysYcdyTableRelation();
            try
            {
                ret = HibernateTemplate.Get<SysYcdyTableRelation>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyTableRelation Save(SysYcdyTableRelation itm)
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

        public void Update(SysYcdyTableRelation itm)
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

        public void Delete(SysYcdyTableRelation itm)
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
                HibernateTemplate.Delete(string.Format("from SysYcdyTableRelation where RECID={0}", id));
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
                HibernateTemplate.Delete("from SysYcdyTableRelation");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public IList<SysYcdyTableRelation> Gets(string callid)
        {
            string hql = "from SysYcdyTableRelation where CallId='" + callid + "'";
            return HibernateTemplate.Find<SysYcdyTableRelation>(hql);
        }
    }
}
