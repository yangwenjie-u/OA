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
    public class UserSettingDao : HibernateDaoSupport, IUserSettingDao
    {
        public IList<UserSetting> GetAll()
        {
            IList<UserSetting> ret = new List<UserSetting>();
            try
            {
                ret = HibernateTemplate.LoadAll<UserSetting>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public UserSetting Get(int id)
        {
            UserSetting ret = new UserSetting();
            try
            {
                ret = HibernateTemplate.Get<UserSetting>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        public UserSetting Save(UserSetting itm)
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

        public void Update(UserSetting itm)
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

        public void Delete(UserSetting itm)
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
        public void Delete(int id)
        {
            try
            {
                HibernateTemplate.Delete(string.Format("from UserSetting where Recid={0}", id));
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
                HibernateTemplate.Delete("from UserSetting");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        /// <summary>
        /// 某个用户的所有设置
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IList<UserSetting> Gets(string username)
        {
            IList<UserSetting> ret = new List<UserSetting>();
            try
            {
                ret = HibernateTemplate.Find<UserSetting>("from UserSetting where UserName='"+username+"'");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        /// <summary>
        /// 某个用户的某个设置项
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public UserSetting Get(string username, string key)
        {
            UserSetting ret = new UserSetting();
            try
            {
                IList<UserSetting> settings = HibernateTemplate.Find<UserSetting>("from UserSetting where UserName='" + username + "' and SettingId='"+key+"'");
                if (settings.Count > 0)
                    ret = settings[0];
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
