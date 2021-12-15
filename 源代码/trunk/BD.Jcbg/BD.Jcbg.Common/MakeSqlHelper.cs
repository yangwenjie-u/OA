using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class MakeSqlHelper
    {
        public static string InsertSql(string tablename, Dictionary<string, string> dict)
        {
            string sql = string.Empty;
            string fileds = string.Empty;
            string values = string.Empty;
            foreach (var value in dict)
            {
                fileds += value.Key + ",";
                values += "'" + value.Value + "',";
            }
            sql += "insert into " + tablename + "(";
            sql += fileds.TrimEnd(',');
            sql += ") values(";
            sql += values.TrimEnd(',');
            sql += ") ";
            return sql;
        }

        public static string UpdateSql(string tablename, Dictionary<string, string> dict, string where)
        {
            string sql = string.Empty;
            sql = "update " + tablename + " set ";
            foreach (var value in dict)
            {
                sql += value.Key + "='" + value.Value + "',";
            }
            sql = sql.TrimEnd(',');
            sql += " where " + where;
            return sql;
        }

        public static string DeleteSql(string tablename, string where)
        {
            string sql = string.Empty;
            sql = string.Format("delete from {0} where {1}", tablename, where);
            return sql;
        }

        public static string PackageQuerySql(string field, string operation, string value)
        {
            value = value.GetSafeRequest();

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (operation.ToLower() == "like")
                return string.Format(" and {0} {1} '%{2}%'", field, operation, value);

            return string.Format(" and {0} {1} '{2}'", field, operation, value);
        }
    }
}
