using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.DaoSqlServer
{
    public  class MessageInfoListDao : HibernateDaoSupport, IMessageInfoListDao
    {


        public IList<MessageInfoList> GetAll()
        {
            IList<MessageInfoList> ret = new List<MessageInfoList>();
            try
            {
                ret = HibernateTemplate.LoadAll<MessageInfoList>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public MessageInfoList Get(int id)
        {
            MessageInfoList ret = new MessageInfoList();
            try
            {
                ret = HibernateTemplate.Get<MessageInfoList>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public MessageInfoList Save(MessageInfoList itm)
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

        public void Update(MessageInfoList itm)
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

        public void Delete(MessageInfoList itm)
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
                HibernateTemplate.Delete(string.Format("from MessageInfoList where Lsh='{0}'", id));
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
                HibernateTemplate.Delete("from MessageInfoList");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }


    }

}
