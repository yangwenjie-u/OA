using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BD.Jcbg.IDao
{
    public interface IMySqlDao
    {
        int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters);


        int ExecuteNonQuery(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters);

        MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters);

        DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters);


        object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters);

        object ExecuteScalar(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters);

        DataTable ExecuteDataTable(string connectionString, string sql, MySqlParameter[] cmdParams);

        DataTable ExecuteDataTable(string connectionString, string sql);

        List<Dictionary<string,object>> GetDataTable(string connectionString, string sql);

        void ExecuteSqlTran(string connectionString, List<string> SQLStringList);

        void ExecuteProc(string connectionString, string sql, MySqlParameter[] cmdParams);
        void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms);

    }
}
