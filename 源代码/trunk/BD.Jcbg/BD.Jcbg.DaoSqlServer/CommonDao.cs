using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NHibernate;
using Spring.Data.NHibernate.Generic.Support;
using System.Data;
using BD.Jcbg.IDao;
using BD.Jcbg.Common;
using System.Data.SqlClient;

namespace BD.Jcbg.DaoSqlServer
{
    /// <summary>
    /// 没有实体的通用的数据处理类
    /// </summary>
    public class CommonDao : HibernateDaoSupport, ICommonDao
    {
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="cmdTxt"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParams"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool ExecCommand(string cmdTxt, CommandType cmdType = CommandType.Text, IList<IDataParameter> cmdParams = null, int timeout = -1)
        {
            bool ret = true;
            ISession session = this.SessionFactory.GetCurrentSession();
            IDbCommand cmd = null;
            try
            {
                cmd = session.Connection.CreateCommand();
                session.Transaction.Enlist(cmd);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdTxt;
                if (cmdParams != null)
                {
                    foreach (IDataParameter cmdParam in cmdParams)
                        cmd.Parameters.Add(cmdParam);
                }
                if (timeout > -1)
                    cmd.CommandTimeout = timeout;

                ret = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                string msg = "";
                if (cmd != null && cmd.CommandType == CommandType.StoredProcedure)
                {
                    string msg2 = "";
                    msg = "exec " + cmd.CommandText + " ";
                    foreach (IDataParameter param in cmd.Parameters)
                    {
                        msg2 += param.ParameterName + "(" + param.DbType + "),";
                        if (param.DbType == DbType.String)
                            msg += "'";
                        msg += param.Value;
                        if (param.DbType == DbType.String)
                            msg += "'";
                        msg += ",";
                    }
                    msg.Trim(new char[] { ',' });
                    msg += "\r\n" + msg2;
                }
                else
                {
                    msg = cmdTxt + "," + cmdType.ToString();
                    if (cmdParams != null)
                    {
                        msg += "。参数：";
                        foreach (IDataParameter param in cmdParams)
                        {
                            msg += param.ParameterName + "=" + param.Value + "。";
                        }
                    }
                }
                SysLog4.WriteLog(msg, e);
                ret = false;
                throw e;
            }
            finally
            {
                //session.Close();
            }
            return ret;
        }


        public bool ExecCommandOpenSession(string cmdTxt, CommandType cmdType, IList<IDataParameter> cmdParams = null, int timeout = -1)
        {
            bool ret = true;
            ISession session = this.SessionFactory.OpenSession();
            IDbCommand cmd = null;
            try
            {
                cmd = session.Connection.CreateCommand();
                session.Transaction.Enlist(cmd);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdTxt;
                if (cmdParams != null)
                {
                    foreach (IDataParameter cmdParam in cmdParams)
                        cmd.Parameters.Add(cmdParam);
                }
                if (timeout > -1)
                    cmd.CommandTimeout = timeout;

                ret = cmd.ExecuteNonQuery() > 0;

            }
            catch (Exception e)
            {
                
                SysLog4.WriteLog(e.Message);
                ret = false;
                throw e;
            }
            finally
            {
                session.Close();
            }
            return ret;
        }
        public IDataReader ExecuteReader(string sql)
        {
            IDataReader reader = null;
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;             
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                //if (reader != null)
                //    reader.Close();
                session.Close();
            }
            return reader;
        }
        /// <summary>
        /// 多sql返回dataset
        /// </summary>
        /// <param name="list"></param>
        public DataSet GetDataSet(ArrayList list)
        {
            DataSet set = new DataSet();
            ISession session = SessionFactory.OpenSession();
            try
            {
                string table = "table";
                SqlCommand cmd = (SqlCommand)session.Connection.CreateCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                for (int i = 0; i < list.Count; ++i)
                {
                    cmd.CommandText = list[i].ToString();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(set, table + i);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog("执行sql出错!", e);
            }
            finally
            {
                session.Close();
            }
            return set;
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecSql(string sql)
        {
            bool ret = false;
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                ret = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                session.Close();
            }
            return ret;

        }
        public bool ExecSqlTran(string sql)
        {
            bool ret = true;
            ISession session = this.SessionFactory.GetCurrentSession();
            IDbCommand cmd = null;
            try
            {
                cmd = session.Connection.CreateCommand();
                session.Transaction.Enlist(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                ret = cmd.ExecuteNonQuery() > 0;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
                ret = false;
                throw e;
            }
            finally
            {
                //session.Close();
            }
            return ret;

        }

        /// <summary>
        /// 当前事务查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetDataTableTran(string sql)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            ISession session = this.SessionFactory.GetCurrentSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                session.Transaction.Enlist(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                    }
                    ret.Add(row);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                //session.Close();
            }
            return ret;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetDataTable(string sql)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                    }
                    ret.Add(row);
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
            return ret;
        }

        public IList<IDictionary<string, string>> GetDataTable(string cmdTxt, CommandType cmdType, IList<IDataParameter> cmdParams = null, int timeout = -1)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdTxt;
                if (cmdParams != null)
                {
                    foreach (IDataParameter cmdParam in cmdParams)
                        cmd.Parameters.Add(cmdParam);
                }
                if (timeout > -1)
                    cmd.CommandTimeout = timeout;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), DataFormat.GetSafeString(reader[i]));
                    }
                    ret.Add(row);
                }
                reader.Close();


            }
            catch (Exception e)
            {
                string msg = cmdTxt + "," + cmdType.ToString();
                if (cmdParams != null)
                {
                    msg += "。参数：";
                    foreach (IDataParameter param in cmdParams)
                    {
                        msg += param.ParameterName + "=" + param.Value + "。";
                    }
                }
                SysLog4.WriteLog(msg, e);
            }
            finally
            {
                session.Close();
            }
            return ret;
        }

        public DataSet GetDataSet(string cmdTxt, CommandType cmdType, IList<IDataParameter> cmdParams = null, int timeout = -1)
        {
            DataSet ds = new DataSet();
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdTxt;
                if (cmdParams != null)
                {
                    foreach (IDataParameter cmdParam in cmdParams)
                        cmd.Parameters.Add(cmdParam);
                }
                if (timeout > -1)
                    cmd.CommandTimeout = timeout;
                IDbDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
            }
            catch (Exception e)
            {
                string msg = cmdTxt + "," + cmdType;
                if (cmdParams != null)
                {
                    msg += "。参数：";
                    foreach (IDataParameter param in cmdParams)
                    {
                        msg += param.ParameterName + "=" + param.Value + "。";
                    }
                }
                SysLog4.WriteLog(msg, e);
            }
            finally
            {
                session.Close();
            }
            return ds;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<IDictionary<string, object>> GetBinaryDataTable(string sql)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, object> row = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i]);
                    }
                    ret.Add(row);
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
            return ret;
        }
        /// <summary>
        /// 获取分页内容
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetPageData(string sql, int pagesize, int pageindex, out int totalCount)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

            IDataReader reader = null;
            totalCount = 0;
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();

                reader = GetPageDataReader(cmd, sql,
                    pagesize, pageindex);
                reader.NextResult();

                if (reader.Read())
                    totalCount = reader[0].GetSafeInt();

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                        }
                        ret.Add(row);
                    }
                }
                reader.Close();

                reader = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                session.Close();
            }
            return ret;
        }


		/// <summary>
        /// 获取fieldname中指定多个字段的一条记录，多个字段以逗号分隔,多个返回值也是逗号分隔
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetRowValue(string field, string tablename, string where)
        {
            IList<IDictionary<string, string>> tabledata = GetData(tablename, where, "", field);
            if (tabledata.Count == 0)
                return null;

            return tabledata[0];
        }

		public string GetFieldValue(string field, string tablename, string where)
        {
            IDictionary<string, string> row = GetRowValue(field, tablename, where);
            if (row == null)
                return null;
            string ret = "";
            if (row.TryGetValue(field, out ret))
                return ret;
            return null;
        }
		/// <summary>
        /// 获取某个表格或视图的数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetData(string tablename, string where, string order, string field = "")
        {
            if (field == "")
                field = "*";
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

            IDataReader reader = null;
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                if (where.Length > 0)
                    where = " and " + where;
                if (order.Length > 0)
                    order = " order by " + order;
                string sql = "select " + field + " from " + tablename + " where 1=1 " + where + " " + order;
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                    }
                    ret.Add(row);
                }
                reader.Close();

                reader = null;
            }
            catch { }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                session.Close();
            }
            return ret;
        }
        /// <summary>
        /// 获取按月按周统计内容
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetTbData(string bs, string date)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

            IDataReader reader = null;
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();

                reader = GetTbDataReader(cmd, bs, date);


                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                    }
                    ret.Add(row);
                }

                reader.Close();

                reader = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                session.Close();
            }
            return ret;
        }



        /// <summary>
        /// 获取按月按周统计存储过程reader
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        private IDataReader GetTbDataReader(IDbCommand cmd, string bs, string date)
        {
            IDataReader ret = null;
            try
            {

                cmd.CommandText = "exec_yandz_mlytj";
                cmd.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@bs";
                param.Value = bs;
                cmd.Parameters.Add(param);


                param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@date";
                param.Value = date;
                cmd.Parameters.Add(param);

                ret = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }



        public IList<IDictionary<string, object>> GetBinaryPageData(string sql, int pagesize, int pageindex, out int totalCount)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();

            IDataReader reader = null;
            totalCount = 0;
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();

                reader = GetPageDataReader(cmd, sql,
                    pagesize, pageindex);
                reader.NextResult();

                if (reader.Read())
                    totalCount = reader[0].GetSafeInt();

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        IDictionary<string, object> row = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i).ToLower(), reader[i]);
                        }
                        ret.Add(row);
                    }
                }
                reader.Close();

                reader = null;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                session.Close();
            }
            return ret;
        }
        /// <summary>
        /// 获取分页存储过程reader
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        private IDataReader GetPageDataReader(IDbCommand cmd, string sql, int pagesize, int pageindex)
        {
            IDataReader ret = null;
            try
            {

                cmd.CommandText = "SP_RecordPpage";
                cmd.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@sqlstr";
                param.Value = sql;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.DbType = DbType.Int32;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@pagecount";
                param.Value = pageindex;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.DbType = DbType.Int32;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@pagesize";
                param.Value = pagesize;
                cmd.Parameters.Add(param);

                ret = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            return ret;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procstr"></param>
        public bool ExecProc(string procstr, out string err)
        {
            bool ret = false;
            err = "";
            try
            {
                string procname;
                IList<IDataParameter> procparams;
                ret = GetProc(procstr, out procname, out procparams, out err);
                if (!ret)
                    return ret;
                ret = ExecCommand(procname, CommandType.StoredProcedure, procparams);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(procstr, e);
                err = e.Message;
                throw new Exception(e.Message);
            }
            return ret;
        }

        public IList<IDictionary<string, string>> ExecDataTableProc(string procstr, out string err)
        {
            IList<IDictionary<string, string>> datas = new List<IDictionary<string, string>>();
            err = "";
            try
            {
                string procname;
                IList<IDataParameter> procparams;
                if (GetProc(procstr, out procname, out procparams, out err))
                {
                    datas = GetDataTable(procname, CommandType.StoredProcedure, procparams);
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(procstr, e);
                err = e.Message;
                throw new Exception(e.Message);
            }
            return datas;
        }
        /// <summary>
        /// 从一个字符串解析存储过程
        /// </summary>
        /// <param name="procstr"></param>
        /// <param name="procname"></param>
        /// <param name="procparams"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool GetProc(string procstr, out string procname, out IList<IDataParameter> procparams, out string err)
        {
            bool ret = false;
            procname = "";
            procparams = new List<IDataParameter>();
            err = "";
            ISession session = this.SessionFactory.OpenSession();
            try
            {
                // 获取名称
                procname = GetProcName(procstr);
                if (procname == "")
                {
                    err = "解析存储过程名字失败(" + procstr + ")";
                    return ret;
                }
                // 获取参数值
                IList<ProcParam> paramvalues = GetProcParamValues(procstr);
                // 获取参数字段
                string sql = "select a.name as pramname,c.name as paramtype from syscolumns a inner join sysobjects b on a.id=b.id inner join systypes c on a.xusertype=c.xusertype where b.name='" + procname + "' order by  a.colid asc";
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string strname = DataFormat.GetSafeString(reader["pramname"]);
                    string strtype = DataFormat.GetSafeString(reader["paramtype"]);
                    if (paramvalues.Count == 0)
                    {
                        err = "获取不到参数:" + strname + "的值";
                        throw new Exception(err);
                    }
                    ProcParam pvalue = paramvalues[0];
                    paramvalues.RemoveAt(0);
                    IDataParameter param = new SqlParameter() { DbType = DataFormat.GetDbType(strtype), ParameterName = strname, Value = pvalue.Value };
                    procparams.Add(param);
                }
                if (err != "")
                    return ret;


                ret = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                throw e;
            }
            finally
            {
                session.Close();
            }
            return ret;
        }

        /// <summary>
        /// 根据存储过程字符串，获取各个参数值
        /// </summary>
        /// <param name="procstr"></param>
        /// <returns></returns>
        private IList<ProcParam> GetProcParamValues(string procstr)
        {
            IList<ProcParam> arrRet = new List<ProcParam>();
            try
            {
                int nIndex = procstr.IndexOf("(");
                if (nIndex == -1)
                    return arrRet;
                procstr = procstr.Substring(nIndex).Trim(new char[] { ' ', '(', ')' });


                while (procstr.Length > 0)
                {
                    nIndex = 0;
                    char c = procstr[nIndex];
                    if (c == '\'' || c == '\"')
                    {
                        int nIndex2 = procstr.IndexOf(c, 1);
                        if (nIndex2 == -1)
                            break;
                        ProcParam param = new ProcParam();
                        param.Value = procstr.Substring(1, nIndex2 - 1);
                        param.Type = DbType.String;
                        arrRet.Add(param);

                        procstr = procstr.Substring(nIndex2 + 1).TrimStart(new char[] { ',', ' ' });
                    }
                    else
                    {
                        int nIndex2 = procstr.IndexOf(',', 1);

                        ProcParam param = new ProcParam();
                        if (nIndex2 == -1)
                        {
                            param.Value = procstr;
                            procstr = "";
                        }
                        else
                        {
                            param.Value = procstr.Substring(0, nIndex2);
                            procstr = procstr.Substring(nIndex2 + 1).TrimStart(new char[] { ',', ' ' });
                        }
                        param.Type = DbType.Int32;
                        arrRet.Add(param);


                    }

                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return arrRet;
        }
        /// <summary>
        /// 存储过程字符串中获取存储过程名称
        /// </summary>
        /// <param name="procstr"></param>
        /// <returns></returns>
        private string GetProcName(string procstr)
        {
            string ret = "";
            try
            {
                Regex reg = new Regex(@"\w+");
                Match match = reg.Match(procstr);
                ret = match.Value;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
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
        public bool SetCommonAlert(string username, string realname, string mailcontent, string mailtitle)
        {
            int res = 0;

            ISession session = this.SessionFactory.OpenSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                res = FlowSetCommonAlert(cmd, username, realname, mailcontent, mailtitle);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                session.Close();
                return false;
            }
            session.Close();
            return true;
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<IDictionary<string, string>> GetDataTableSameTrans(string sql)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            ISession session = this.SessionFactory.GetCurrentSession();
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                session.Transaction.Enlist(cmd);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                    }
                    ret.Add(row);
                }

                reader.Close();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
            }
            return ret;
        }
		/// <summary>
        /// 执行发送邮件存储过程
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="mailcontent"></param>
        /// <param name="mailtitle"></param>
        /// <returns></returns>
        private int FlowSetCommonAlert(IDbCommand cmd, string username, string realname, string mailcontent, string mailtitle)
        {
            int ret = 0;
            try
            {

                cmd.CommandText = "FlowSetCommonAlert";
                cmd.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@UserName";
                param.Value = username;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@RealName";
                param.Value = realname;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@mailcontent";
                param.Value = mailcontent;
                cmd.Parameters.Add(param);

                param = cmd.CreateParameter();
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.ParameterName = "@mailtitle";
                param.Value = mailtitle;
                cmd.Parameters.Add(param);
                ret = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        /// <summary>
        /// 查询单个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetSingleData(string sql)
        {
            object ret = null;
            ISession session = this.SessionFactory.OpenSession();

            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                ret = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }
            finally
            {
                session.Close();
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
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDictionary<string, string> row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i].GetSafeString());
                    }
                    ret.Add(row);
                }

                reader.Close();

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }

            return ret;
        }

        /// <summary>
        /// 传入cmd执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecSqlByCmd(IDbCommand cmd, string sql)
        {
            bool ret = false;
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                ret = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(sql, e);
            }

            return ret;
        }
    }





    // 存储过程中的参数类型
    public class ProcParam
    {
        public object Value;
        public DbType Type;
    }
}
