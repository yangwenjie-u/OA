using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;

namespace BD.Jcbg.Bll
{
	public class CommonService:ICommonService
	{
		#region 用到的Dao
		ICommonDao CommonDao { get; set; }
		#endregion
		#region 服务
		/// <summary>
		/// 通用删除函数
		/// </summary>
		/// <param name="table"></param>
		/// <param name="idfield"></param>
		/// <param name="fieldvalue"></param>
		/// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool Delete(string table, string idfield, string fieldvalue)
		{
			bool ret = true;
			try
			{
				string sql = "delete from " + table + " where " + idfield + "=@idvalue";
				IDataParameter sqlparam = new SqlParameter("@idvalue", fieldvalue);
				IList<IDataParameter> sqlparams = new List<IDataParameter>();
				sqlparams.Add(sqlparam);
				ret = CommonDao.ExecCommand(sql, CommandType.Text, sqlparams);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
				ret = false;
			}
			return ret;
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public IList<IDictionary<string, string>> GetDataTable(string sql)
		{
			IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
			try
			{
				ret = CommonDao.GetDataTable(sql);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<IDictionary<string, object>> GetDataTable2(string sql)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            try
            {
                ret = CommonDao.GetBinaryDataTable(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        
		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindex"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public IList<IDictionary<string, string>> GetPageData(string sql, int pagesize, int pageindex, out int totalCount)
		{
			IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
			totalCount = 0;
			try
			{
				ret = CommonDao.GetPageData(sql, pagesize, pageindex, out totalCount);
			}
			catch (Exception e)
			{
				SysLog4.WriteLog(e);
			}
			return ret;
		}

        

        [Transaction(ReadOnly=false)]
        public bool ExecTrans(IList<string> sqls)
        {
            bool ret = false;
            try
            {
                foreach (string str in sqls)
                    CommonDao.ExecCommand(str, CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        
		[Transaction(ReadOnly = false)]
        public bool ExecSqls(IList<string> sqls)
        {
            bool ret = false;
            try
            {
                foreach (string str in sqls)
                    CommonDao.ExecSql(str);
                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
		public bool Execsql(string sql)
        {
            bool ret = false;
            try
            {
                CommonDao.ExecSql(sql);
                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        [Transaction(ReadOnly = false)]
        public bool ExecSql(string sql,out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                ret = CommonDao.ExecCommand(sql, CommandType.Text);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
		[Transaction(ReadOnly = false)]
        public bool ExecTrans(IList<string> sqls, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                foreach (string str in sqls)
                    CommonDao.ExecCommand(str, CommandType.Text);
                ret = true;
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }
        [Transaction(ReadOnly = false)]
        public bool ExecTrans(string sql, IList<IDataParameter> sqlparams, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                
                ret = CommonDao.ExecCommand(sql, CommandType.Text, sqlparams);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public bool ExecProc(string procstr, out string err)
        {
            bool ret = false;
            err = "";
            try
            {

                ret = CommonDao.ExecProc(procstr, out err);
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

        [Transaction(ReadOnly = false)]
        public IList<IDictionary<string, string>> ExecDataTableProc(string procstr, out string err)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            err = "";
            try
            {

                ret = CommonDao.ExecDataTableProc(procstr, out err);
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

		/// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="mailcontent"></param>
        /// <param name="mailtitle"></param>
        /// <returns></returns>
        public bool sendmail(string username, string realname, string mailcontent, string mailtitle)
        {
            bool ret = false;
            string msg = "";
            try
            {
                ret = CommonDao.SetCommonAlert(username, realname, mailcontent, mailtitle);
            }
            catch (Exception e)
            {
                msg = e.Message;
                SysLog4.WriteLog(e);
                throw e;
            }
            return ret;
        }

		public IList<IDictionary<string, object>> GetPageData2(string sql, int pagesize, int pageindex, out int totalCount)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            totalCount = 0;
            try
            {
                ret = CommonDao.GetBinaryPageData(sql, pagesize, pageindex, out totalCount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetSingleData(string sql)
        {
            object ret = null;

            try
            {
                ret = CommonDao.GetSingleData(sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        /// <summary>
        /// 传入cmd查询列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetDataTableByCmd(IDbCommand cmd, string sql)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = CommonDao.GetDataTableByCmd(cmd, sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return ret;
        }

        /// <summary>
        /// 传入cmd执行Sql
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecSqlByCmd(IDbCommand cmd, string sql)
        {
            var ret = false;
            try
            {
                ret = CommonDao.ExecSqlByCmd(cmd, sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }

            return ret;
        }
        #endregion
    }
}
