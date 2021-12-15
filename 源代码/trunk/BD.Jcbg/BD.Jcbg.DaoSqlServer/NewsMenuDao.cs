using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IDao;
using Spring.Data.NHibernate.Generic.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DaoSqlServer
{
    public class NewsMenuDao : HibernateDaoSupport, INewsMenuDao
    {
        public IList<NewsMenu> GetAllUseByDisp()
        {

            IList<NewsMenu> ret = new List<NewsMenu>();
            try
            {
                string hql = "from NewsMenu where InUse='True' order by DispOrder";
                ret = HibernateTemplate.Find<NewsMenu>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        public IList<NewsMenu> GetAll()
        {
            IList<NewsMenu> ret = new List<NewsMenu>();
            try
            {
                ret = HibernateTemplate.LoadAll<NewsMenu>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsMenu Get(int id)
        {
            NewsMenu ret = new NewsMenu();
            try
            {
                ret = HibernateTemplate.Get<NewsMenu>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsMenu Save(NewsMenu itm)
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

        public void Update(NewsMenu itm)
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

        public void Delete(NewsMenu itm)
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
                HibernateTemplate.Delete(string.Format("from NewsMenu where Recid={0}", id));
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
                HibernateTemplate.Delete("from NewsMenu");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
