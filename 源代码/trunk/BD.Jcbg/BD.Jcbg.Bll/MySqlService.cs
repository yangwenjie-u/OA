using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BD.Jcbg.Bll
{
    public class MySqlService : IMySqlService
    {
        #region 用到的Dao
        public IMySqlDao MySqlDao { get; set; }
        #endregion

        #region mysql连接字符串
        public string connectionString { get; set; }
        #endregion

        #region 服务
        public bool ExecSql(string sql,out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                ret = MySqlDao.ExecuteNonQuery(
                    connectionString, CommandType.Text,
                    sql, null
                    ) > 0;
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        public bool ExecTrans(List<string> lssql, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                MySqlDao.ExecuteSqlTran(connectionString, lssql);
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                
            }
            return ret;
        }

        public List<Dictionary<string,object>> GetDataTable(string connectionString, string sql)
        {
            List<Dictionary<string, object>> ret = new List<Dictionary<string, object>>();
            try
            {
                ret = MySqlDao.GetDataTable(connectionString, sql);
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }
            return ret;
        }
        #endregion


    }
}
