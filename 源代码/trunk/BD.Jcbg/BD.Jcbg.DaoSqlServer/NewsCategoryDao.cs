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
    public class NewsCategoryDao : HibernateDaoSupport, INewsCategoryDao
    {


        public IList<NewsCategory> GetByLeafTrue()
        {
            IList<NewsCategory> ret = new List<NewsCategory>();
            try
            {
                
                string hql = " from NewsCategory where IsLeaf='True' ";
                ret = HibernateTemplate.Find<NewsCategory>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        public IList<NewsCategory> GetByFatherId(string fatherId)
        {
            IList<NewsCategory> ret = new List<NewsCategory>();
            try
            {
                string hql = "from NewsCategory where Fatherid='" + fatherId + "' ";
                ret = HibernateTemplate.Find<NewsCategory>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public IList<NewsCategory> GetByCategoryId(int categoryId) {
            IList<NewsCategory> ret = new List<NewsCategory>();
            try
            {
                string hql = "from NewsCategory where Categoryid='" + categoryId + "' ";
                ret = HibernateTemplate.Find<NewsCategory>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        public IList<NewsCategory> GetAll()
        {
            IList<NewsCategory> ret = new List<NewsCategory>();
            try
            {
                ret = HibernateTemplate.LoadAll<NewsCategory>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsCategory Get(int id)
        {
            NewsCategory ret = new NewsCategory();
            try
            {
                ret = HibernateTemplate.Get<NewsCategory>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsCategory Save(NewsCategory itm)
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

        public void Update(NewsCategory itm)
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

        public void Delete(NewsCategory itm)
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
                HibernateTemplate.Delete(string.Format("from NewsCategory where Recid={0}", id));
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
                HibernateTemplate.Delete("from NewsCategory");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
