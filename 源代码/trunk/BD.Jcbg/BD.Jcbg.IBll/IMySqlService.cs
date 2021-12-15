using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BD.Jcbg.IBll
{
    public interface IMySqlService
    {
        bool ExecSql(string sql, out string msg);
        bool ExecTrans(List<string> lssql, out string msg);
        List<Dictionary<string, object>> GetDataTable(string connectionString, string sql);
    }
}
