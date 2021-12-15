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
    public class CompanyChangeItemDao : HibernateDaoSupport, ICompanyChangeItemDao
    {
        public IList<CompanyChangeItem> GetAll()
        {
            IList<CompanyChangeItem> ret = new List<CompanyChangeItem>();
            try
            {
                ret = HibernateTemplate.LoadAll<CompanyChangeItem>();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChangeItem Get(int id)
        {
            CompanyChangeItem ret = new CompanyChangeItem();
            try
            {
                ret = HibernateTemplate.Get<CompanyChangeItem>(id);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public CompanyChangeItem Save(CompanyChangeItem itm)
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

        public void Update(CompanyChangeItem itm)
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

        public void Delete(CompanyChangeItem itm)
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
                HibernateTemplate.Delete(string.Format("from CompanyChangeItem where ChangeItemID={0}", id));
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
                HibernateTemplate.Delete("from CompanyChangeItem");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }
        public IList<CompanyChangeItem> GetItems(int changeid)
        {
            IList<CompanyChangeItem> ret = new List<CompanyChangeItem>();
            try
            {
                ret = HibernateTemplate.Find<CompanyChangeItem>(string.Format("from CompanyChangeItem where ChangeID={0} order by ChangeType asc ", changeid));
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public void GetSum(int changeid, int type, out int no, out decimal money)
        {
            no = 0;
            money = 0;
            string sql = "select count(ChangeItemID),sum(ChangeMoney) from CompanyChangeItem where ChangeID=" + changeid.ToString();
            if (type > -1)
            {
                sql += " and ChangeType=" + type.ToString();
            }
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    no = reader[0].GetSafeInt(0);
                    money = reader[1].GetSafeDecimal(0);
                }

                reader.Close();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                session.Close();
            }
        }

        public void GetAllSum(string depid, int type, out int no, out decimal money)
        {
            no = 0;
            money = 0;
            string sql = "select count(ChangeItemID),sum(ChangeMoney) from CompanyChangeItem where ChangeID in (select ChangeID from CompanyChange where DepartmentId='" + depid + "')";
            if (type > -1)
            {
                sql += " and ChangeType=" + type.ToString();
            }
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    no = reader[0].GetSafeInt(0);
                    money = reader[1].GetSafeDecimal(0);
                }

                reader.Close();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                session.Close();
            }
        }
    }
}
