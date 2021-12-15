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
	class UserShareFileFolderDao : HibernateDaoSupport, IUserShareFileFolderDao
	{
		public IList<UserShareFileFolder> GetAll()
		{
			IList<UserShareFileFolder> ret = new List<UserShareFileFolder>();
			try
			{
				ret = HibernateTemplate.LoadAll<UserShareFileFolder>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public UserShareFileFolder Get(int id)
		{
			UserShareFileFolder ret = new UserShareFileFolder();
			try
			{
				ret = HibernateTemplate.Get<UserShareFileFolder>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public UserShareFileFolder Save(UserShareFileFolder itm)
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

		public void Update(UserShareFileFolder itm)
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

		public void Delete(UserShareFileFolder itm)
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
				HibernateTemplate.Delete(string.Format("from UserShareFileFolder where Recid={0}", id));
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
				HibernateTemplate.Delete("from UserShareFileFolder");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}

		/// <summary>
		/// 获取用户的文件夹
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public IList<UserShareFileFolder> Gets(string username)
		{
			IList<UserShareFileFolder> ret = new List<UserShareFileFolder>();
			try
			{
				ret = HibernateTemplate.Find<UserShareFileFolder>("from UserShareFileFolder where UserName='" + username + "' order by ParentId asc,FolderName asc");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
	}
}