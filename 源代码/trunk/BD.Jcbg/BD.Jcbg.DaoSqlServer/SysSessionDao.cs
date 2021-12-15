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
    public class SysSessionDao : HibernateDaoSupport, ISysSessionDao
    {
        public IList<SysSession> GetAll()
        {
            IList<SysSession> ret = new List<SysSession>();
            try
            {
                ret = HibernateTemplate.LoadAll<SysSession>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public SysSession Get(string id)
        {
            SysSession ret = null;
            try
            {
                string hql = "from SysSession where SessionId='"+id+"'";
                IList<SysSession> dt = HibernateTemplate.Find<SysSession>(hql);
                if (dt.Count > 0)
                    ret = dt[0];
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public SysSession Save(SysSession itm)
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

        public void Update(SysSession itm)
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

        public void Delete(SysSession itm)
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
        public void Delete(string id)
        {
            try
            {
                HibernateTemplate.Delete(string.Format("from SysSession where SessionId='{0}'", id));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
        }
        public void Clear()
        {
            try
            {
                HibernateTemplate.Delete("from SysSession");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
        }

        public void DeleteExpire(int expirehours)
        {
            try
            {
                string lastTime = "";
                lastTime = DateTime.Now.Subtract(new TimeSpan(expirehours, 0, 0)).ToString("yyyy-MM-dd HH:mm:ss");
                HibernateTemplate.Delete(string.Format("from SysSession where LoginTime<=convert(datetime,'{0}')", lastTime));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
        }

        public SysSession GetByUser(string username)
        {
            SysSession ret = null;
            try
            {
                IList <SysSession> dt = HibernateTemplate.Find<SysSession>(string.Format("from SysSession where UserName='{0}'", username));
                if (dt.Count > 0)
                    ret = dt[0];
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
    }
}
