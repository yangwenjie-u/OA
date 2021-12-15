using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Spring.Transaction.Interceptor;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Stereotype;
using NHibernate;
using NHibernate.Criterion;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IDao;
using BD.Jcbg.Common;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BD.Jcbg.DaoSqlServer
{
    public class MySqlDao:IMySqlDao
    {
        /// <summary>
        /// 给定连接的数据库用假设参数执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }


        }

        /// <summary>
        /// 用现有的数据库连接执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connection">一个现有的数据库连接</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public int ExecuteNonQuery(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns></returns>
        public DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            //创建一个MySqlCommand对象
            MySqlCommand cmd = new MySqlCommand();
            //创建一个MySqlConnection对象
            MySqlConnection conn = new MySqlConnection(connectionString);
            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，
            //因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
            try
            {
                //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 MySqlCommand 的 ExecuteReader 方法
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //清除参数
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 用执行的数据库连接执行一个返回数据集的sql命令
        /// </summary>
        /// <remarks>
        /// 举例:
        /// MySqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>包含结果的读取器</returns>
        public MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            //创建一个MySqlCommand对象
            MySqlCommand cmd = new MySqlCommand();
            //创建一个MySqlConnection对象
            MySqlConnection conn = new MySqlConnection(connectionString);
            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
            //因此commandBehaviour.CloseConnection 就不会执行
            try
            {
                //调用 PrepareCommand 方法，对 MySqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 MySqlCommand 的 ExecuteReader 方法
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //清除参数
                cmd.Parameters.Clear();
                conn.Close();
                return reader;
            }
            catch
            {
                //关闭连接，抛出异常
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        ///例如:
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        ///<param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                connection.Close();
                return val;
            }
        }

        /// <summary>
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        /// 例如:
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个存在的数据库连接</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public object ExecuteScalar(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }



        public DataTable ExecuteDataTable(string connectionString, string sql, MySqlParameter[] cmdParams)
        {
            try
            {
                //创建一个MySqlConnection对象
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    //创建一个MySqlCommand对象
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, CommandType.Text, sql, cmdParams);

                            MySqlDataAdapter adapter = new MySqlDataAdapter();
                            adapter.SelectCommand = cmd;
                            DataTable dt = new DataTable();

                            adapter.Fill(dt);

                            //清除参数
                            cmd.Parameters.Clear();
                            connection.Close();
                            return dt;

                        }
                        catch (Exception e)
                        {
                            connection.Close();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable ExecuteDataTable(string connectionString, string sql)
        {
            try
            {
                //创建一个MySqlConnection对象
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    //创建一个MySqlCommand对象
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, CommandType.Text, sql, null);

                            MySqlDataAdapter adapter = new MySqlDataAdapter();
                            adapter.SelectCommand = cmd;
                            DataTable dt = new DataTable();

                            adapter.Fill(dt);

                            //清除参数
                            cmd.Parameters.Clear();
                            connection.Close();
                            return dt;

                        }
                        catch (Exception e)
                        {
                            connection.Close();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<Dictionary<string, object>> GetDataTable(string connectionString, string sql)
        {
            List<Dictionary<string, object>> ret = new List<Dictionary<string, object>>();
            DataTable dt = ExecuteDataTable(connectionString, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Dictionary<string, object> r = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    foreach (DataColumn col in dt.Columns)
                    {
                        r.Add(col.ColumnName.ToLower(), row[col]);
                    }
                    ret.Add(r);
                }
            }
            return ret;
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>        
        public void ExecuteSqlTran(string connectionString, List<string> SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 0)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    conn.Close();
                }
                catch(Exception e)
                {
                    tx.Rollback();
                    conn.Close();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="cmdParams"></param>
        public void ExecuteProc(string connectionString, string sql, MySqlParameter[] cmdParams)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, sql, cmdParams);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }
            }
        }
        /// <summary>
        /// 准备执行一个命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">OleDb连接</param>
        /// <param name="trans">OleDb事务</param>
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如:Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        public void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = 0;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = cmdType;
                if (cmdParms != null)
                {
                    foreach (MySqlParameter parm in cmdParms)
                        cmd.Parameters.Add(parm);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
    }
}
