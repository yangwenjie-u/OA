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
	class GclrWxyFileDao : HibernateDaoSupport, IGclrWxyFileDao
    {
		public IList<GclrWxyFile> GetAll()
		{
			IList<GclrWxyFile> ret = new List<GclrWxyFile>();
			try
			{
				ret = HibernateTemplate.LoadAll<GclrWxyFile>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public GclrWxyFile Get(int id)
		{
			GclrWxyFile ret = new GclrWxyFile();
			try
			{
				ret = HibernateTemplate.Get<GclrWxyFile>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public GclrWxyFile Save(GclrWxyFile itm)
		{
            try
            {
                itm.FileID = DataFormat.GetSafeInt(HibernateTemplate.Save(itm));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return itm;
        }

		public void Update(GclrWxyFile itm)
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

		public void Delete(GclrWxyFile itm)
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
				HibernateTemplate.Delete(string.Format("from GclrWxyFile where fileid={0}", id));
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
				HibernateTemplate.Delete("from GclrWxyFile");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}