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
    public class NewsAttachDao : HibernateDaoSupport, INewsAttachDao
    {


        public void DeleteFromArticleId(int articleID) {
            try
            {
                HibernateTemplate.Delete(string.Format("from NewsAttach where Articleid={0}", articleID));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        public IList<NewsAttach> GetByArticleidAndSavename(int articleID, string saveName)
        {
           IList<NewsAttach> ret = new List<NewsAttach>();
            try
            {
                string hql = "from NewsAttach where Articleid=" + articleID + " and  SaveName = '"+saveName+"'";
                ret = HibernateTemplate.Find<NewsAttach>(hql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        public IList<NewsAttach> GetByArticleId(int articleID)
        {
            IList<NewsAttach> ret = new List<NewsAttach>();
            try
            {
              
                string hql = "  from NewsAttach where Articleid=" + articleID + " ";
                ret = HibernateTemplate.Find<NewsAttach>(hql);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }


        public IList<NewsAttach> GetAll()
        {
            IList<NewsAttach> ret = new List<NewsAttach>();
            try
            {
                ret = HibernateTemplate.LoadAll<NewsAttach>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsAttach Get(int id)
        {
            NewsAttach ret = new NewsAttach();
            try
            {
                ret = HibernateTemplate.Get<NewsAttach>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsAttach Save(NewsAttach itm)
        {
            try
            {
               itm.Attachid = DataFormat.GetSafeInt( HibernateTemplate.Save(itm));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return itm;
        }

        public void Update(NewsAttach itm)
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

        public void Delete(NewsAttach itm)
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
                HibernateTemplate.Delete(string.Format("from NewsAttach where Attachid={0}", id));
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
                HibernateTemplate.Delete("from NewsAttach");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    
    }
}

