using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using Spring.Data.NHibernate.Generic.Support;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using System.Threading;
using System.Text.RegularExpressions;
using Spring.Transaction.Interceptor;
using NHibernate;

namespace BD.Jcbg.DaoSqlServer
{
    public class MessageReceiveDao : HibernateDaoSupport, IMessageReceiveDao
    {




        /// <summary>
        /// 某个用户的所有设置
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<MessageReceive> Gets()
        {
            IList<MessageReceive> ret = new List<MessageReceive>();
            try
            {
                ret = HibernateTemplate.Find<MessageReceive>("  from MessageReceive ");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }



        /// <summary>
        /// 保存MessageReceive对象(已经作废不用了，再commonDao中执行事务)
        /// </summary>
        /// <param name="itms"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
       
        public int InsertData(string sql, out string msg)
        {

            ISession session = this.SessionFactory.GetCurrentSession();
            ITransaction transaction = session.BeginTransaction();
            try
            {


                msg = "上传成功!";
                int ret = -1;



                ret = Session.CreateSQLQuery(sql.ToString()).ExecuteUpdate();

                transaction.Commit();
                return ret;
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);
                transaction.Rollback();
                msg = ex.Message.Substring(0, 48);
                return -1;
            }


        }


        public IList<MessageReceive> GetAll()
        {
            IList<MessageReceive> ret = new List<MessageReceive>();
            try
            {
                ret = HibernateTemplate.LoadAll<MessageReceive>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public MessageReceive Get(int id)
        {
            MessageReceive ret = new MessageReceive();
            try
            {
                ret = HibernateTemplate.Get<MessageReceive>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public MessageReceive Save(MessageReceive itm)
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

        public void Update(MessageReceive itm)
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

        public void Delete(MessageReceive itm)
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
                HibernateTemplate.Delete(string.Format("from MessageReceive where Recid={0}", id));
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
                HibernateTemplate.Delete("from MessageReceive");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
    }
}
