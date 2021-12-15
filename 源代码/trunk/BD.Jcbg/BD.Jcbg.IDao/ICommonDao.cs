using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 没有实体的通用的数据处理接口
	/// </summary>
	public interface ICommonDao
	{
		/// <summary>
		/// 执行sql
		/// </summary>
		/// <param name="cmdTxt"></param>
		/// <param name="cmdType"></param>
		/// <param name="cmdParams"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
        bool ExecCommand(string cmdTxt, CommandType cmdType = CommandType.Text, IList<IDataParameter> cmdParams = null, int timeout = -1);

        /// <summary>
        /// open Session 执行sql
        /// </summary>
        /// <param name="cmdTxt"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool ExecCommandOpenSession(string cmdTxt, CommandType cmdType, IList<IDataParameter> cmdParams = null, int timeout = -1);
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExecSql(string sql);
  		/// <summary>
        /// 执行sql 事务
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExecSqlTran(string sql);


        IDataReader ExecuteReader(string sql);

	    /// <summary>
	    /// 多sql返回dataset
	    /// </summary>
	    /// <param name="list"></param>
	    DataSet GetDataSet(ArrayList list);

        /// <summary>
        /// 当前事务查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetDataTableTran(string sql);

		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		IList<IDictionary<string,string>> GetDataTable(string sql);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="cmdTxt"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetDataTable(string cmdTxt, CommandType cmdType, IList<IDataParameter> cmdParams = null, int timeout = -1);

	    DataSet GetDataSet(string cmdTxt, CommandType cmdType, IList<IDataParameter> cmdParams = null,
	        int timeout = -1);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<IDictionary<string, object>> GetBinaryDataTable(string sql);
		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindex"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		IList<IDictionary<string, string>> GetPageData(string sql, int pagesize, int pageindex, out int totalCount);

        IList<IDictionary<string, object>> GetBinaryPageData(string sql, int pagesize, int pageindex, out int totalCount);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procstr"></param>
        bool ExecProc(string procstr, out string err);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procstr"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> ExecDataTableProc(string procstr, out string err);

        IList<IDictionary<string, string>> GetDataTableSameTrans(string sql);



        /// <summary>
        /// 按周按月统计
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetTbData(string bs, string date);
    	    /// <summary>
        /// 获取fieldname中指定多个字段的一条记录，多个字段以逗号分隔
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        IDictionary<string, string> GetRowValue(string fieldname, string tablename, string where);


        string GetFieldValue(string field, string tablename, string where);
        /// <summary>
        /// 获取某个表格或视图的数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetData(string tablename, string where, string order, string field = "");


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="mailcontent"></param>
        /// <param name="mailtitle"></param>
        /// <returns></returns>
        bool SetCommonAlert(string username, string realname, string mailcontent, string mailtitle);

        /// <summary>
        /// 查询单个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        object GetSingleData(string sql);

        /// <summary>
        /// 传入cmd查询列表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetDataTableByCmd(IDbCommand cmd, string sql);

        /// <summary>
        /// 传入cmd执行sql
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool ExecSqlByCmd(IDbCommand cmd, string sql);
	}
}
