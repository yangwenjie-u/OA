using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BD.Jcbg.IBll
{
	public interface ICommonService
	{
		/// <summary>
		/// 通用删除函数
		/// </summary>
		/// <param name="table"></param>
		/// <param name="idfield"></param>
		/// <param name="fieldvalue"></param>
		/// <returns></returns>
		bool Delete(string table, string idfield, string fieldvalue);

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetDataTable(string sql);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<IDictionary<string, object>> GetDataTable2(string sql);
		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindex"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetPageData(string sql, int pagesize, int pageindex, out int totalCount);

        IList<IDictionary<string, object>> GetPageData2(string sql, int pagesize, int pageindex, out int totalCount);

        bool ExecTrans(IList<string> sqls);

		bool ExecSqls(IList<string> sqls);

        bool Execsql(string sql);

        bool ExecTrans(IList<string> sqls, out string msg);

        bool ExecTrans(string sql, IList<IDataParameter> sqlparams, out string msg);

        bool ExecProc(string procstr, out string err);

        IList<IDictionary<string, string>> ExecDataTableProc(string procstr, out string err);

        bool ExecSql(string sql,out string msg);

		bool sendmail(string username, string realname, string mailcontent, string mailtitle);

        object GetSingleData(string sql);

        IList<IDictionary<string, string>> GetDataTableByCmd(IDbCommand cmd, string sql);

        bool ExecSqlByCmd(IDbCommand cmd, string sql);
    }
}
