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
    public class NewsArtcleDao : HibernateDaoSupport, INewsArtcleDao
    {
            public IList<NewsArtcle> GetAll()
        {
            IList<NewsArtcle> ret = new List<NewsArtcle>();
            try
            {
                ret = HibernateTemplate.LoadAll<NewsArtcle>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsArtcle Get(int id)
        {
            NewsArtcle ret = new NewsArtcle();
            try
            {
                ret = HibernateTemplate.Get<NewsArtcle>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public NewsArtcle Save(NewsArtcle itm)
        {
            try
            {
                itm.Articleid = DataFormat.GetSafeInt(HibernateTemplate.Save(itm));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return itm;
        }

        public void Update(NewsArtcle itm)
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

        public void Delete(NewsArtcle itm)
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
                HibernateTemplate.Delete(string.Format("from NewsArtcle where Articleid={0}", id));
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
                HibernateTemplate.Delete("from NewsArtcle");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    
    }
}
