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
	class UserMailFolderDao : HibernateDaoSupport, IUserMailFolderDao
	{
		public IList<UserMailFolder> GetAll()
		{
			IList<UserMailFolder> ret = new List<UserMailFolder>();
			try
			{
				ret = HibernateTemplate.LoadAll<UserMailFolder>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public UserMailFolder Get(int id)
		{
			UserMailFolder ret = new UserMailFolder();
			try
			{
				ret = HibernateTemplate.Get<UserMailFolder>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public UserMailFolder Save(UserMailFolder itm)
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

		public void Update(UserMailFolder itm)
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

		public void Delete(UserMailFolder itm)
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
				HibernateTemplate.Delete(string.Format("from UserMailFolder where Recid={0}", id));
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
				HibernateTemplate.Delete("from UserMailFolder");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}