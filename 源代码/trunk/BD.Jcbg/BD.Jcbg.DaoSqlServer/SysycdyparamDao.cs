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
    public class SysycdyparamDao : HibernateDaoSupport, ISysycdyparamDao
    {
        public IList<SysYcdyParam> GetAll()
        {
            IList<SysYcdyParam> ret = new List<SysYcdyParam>();
            try
            {
                ret = HibernateTemplate.LoadAll<SysYcdyParam>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyParam Get(int id)
        {
            SysYcdyParam ret = new SysYcdyParam();
            try
            {
                ret = HibernateTemplate.Get<SysYcdyParam>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public SysYcdyParam Save(SysYcdyParam itm)
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

        public void Update(SysYcdyParam itm)
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

        public void Delete(SysYcdyParam itm)
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
                HibernateTemplate.Delete(string.Format("from SysYcdyParam where RECID={0}", id));
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
                HibernateTemplate.Delete("from SysYcdyParam");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public IList<SysYcdyParam> Gets(string callid)
        {
            string hql = "from SysYcdyParam where CallId='" + callid + "'";
            return HibernateTemplate.Find<SysYcdyParam>(hql);
        }
    }
}
