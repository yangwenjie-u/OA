using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.IDao;
using NHibernate;
using Spring.Data.NHibernate.Generic.Support;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.DaoSqlServer
{
    public class BanHeZhanSectionDao : HibernateDaoSupport, IBanHeZhanSectionDao
    {
        public IList<BanHeZhanSection> GetAll()
        {
            IList<BanHeZhanSection> ret = new List<BanHeZhanSection>();
            try
            {
                ret = HibernateTemplate.LoadAll<BanHeZhanSection>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public BanHeZhanSection Get(int id)
        {
            BanHeZhanSection ret = new BanHeZhanSection();
            try
            {
                ret = HibernateTemplate.Get<BanHeZhanSection>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        
        public BanHeZhanSection Save(BanHeZhanSection itm)
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

        public void Update(BanHeZhanSection itm)
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

        public void Delete(BanHeZhanSection itm)
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
                HibernateTemplate.Delete(string.Format("from BanHeZhanSection where Recid={0}", id));
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
                HibernateTemplate.Delete("from BanHeZhanSection");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
