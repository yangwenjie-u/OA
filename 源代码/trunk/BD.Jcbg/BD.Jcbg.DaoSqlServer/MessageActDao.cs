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
    public class MessageActDao : HibernateDaoSupport, IMessageActDao
    {

        public IList<MessageAct> GetAll()
        {
            IList<MessageAct> ret = new List<MessageAct>();
            try
            {
                ret = HibernateTemplate.LoadAll<MessageAct>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public MessageAct Get(string id)
        {
            MessageAct ret = new MessageAct();
            try
            {
                ret = HibernateTemplate.Get<MessageAct>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public MessageAct Save(MessageAct itm)
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

        public void Update(MessageAct itm)
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

        public void Delete(MessageAct itm)
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
        public void Delete(String id)
        {
            try
            {
                HibernateTemplate.Delete(string.Format("from MessageAct where Lsh='{0}'", id));
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
                HibernateTemplate.Delete("from MessageAct");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

    }
}
