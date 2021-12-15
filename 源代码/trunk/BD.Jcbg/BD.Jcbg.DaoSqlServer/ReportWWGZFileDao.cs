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
	class ReportWWGZfileDao : HibernateDaoSupport, IReportWWGZfileDao
    {
		public IList<ReportWWGZfile> GetAll()
		{
			IList<ReportWWGZfile> ret = new List<ReportWWGZfile>();
			try
			{
				ret = HibernateTemplate.LoadAll<ReportWWGZfile>();
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public ReportWWGZfile Get(int id)
		{
			ReportWWGZfile ret = new ReportWWGZfile();
			try
			{
				ret = HibernateTemplate.Get<ReportWWGZfile>(id);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

		public ReportWWGZfile Save(ReportWWGZfile itm)
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

		public void Update(ReportWWGZfile itm)
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

		public void Delete(ReportWWGZfile itm)
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
				HibernateTemplate.Delete(string.Format("from ReportWWGZ_file where fileid={0}", id));
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
        // 根据外网告知ID删除文件
        public void DeleteFromWWGZId(int wwgzid)
        {
            try
            {
                HibernateTemplate.Delete(string.Format("from ReportWWGZ_FILE where WWGZid={0}", wwgzid));
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
				HibernateTemplate.Delete("from ReportWWGZ_file");
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
		}
	}
}