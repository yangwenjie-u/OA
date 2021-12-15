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
	public class UserMailDao : HibernateDaoSupport, IUserMailDao
	{
		public IList<UserMail> GetAll()
		{
			IList<UserMail> ret = new List<UserMail>();
			try
			{
				ret = HibernateTemplate.LoadAll<UserMail>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public UserMail Get(int id)
		{
			UserMail ret = new UserMail();
			try
			{
				ret = HibernateTemplate.Get<UserMail>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public UserMail Save(UserMail itm)
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

		public void Update(UserMail itm)
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

		public void Delete(UserMail itm)
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
				HibernateTemplate.Delete(string.Format("from UserMail where Recid={0}", id));
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
				HibernateTemplate.Delete("from UserMail");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}
