using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;

namespace BD.JC.JS.Common
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public class SqlHelper
    {
        #region 服务
        private static ICommonService _commonService = null;
        private static ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                }
                return _commonService;
            }
        }
        #endregion
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="table">表格名称</param>
        /// <param name="idfield">字段名称</param>
        /// <param name="fieldvalue">字段值</param>
        /// <param name="msg">错误信息</param>
        /// <returns>操作成功返回true，失败返回false</returns>
        public static bool Delete(string table, string idfield, string fieldvalue, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                ret = CommonService.Delete(table, idfield, fieldvalue);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="msg">错误信息</param>
        /// <returns>查询成功返回记录集，查询失败返回null</returns>
        public static IList<IDictionary<string, string>> GetDataTable(string sql, out string msg)
        {
            IList<IDictionary<string, string>> ret = null;
            msg = "";
            try
            {
                ret = CommonService.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码从1开始</param>
        /// <param name="totalCount">返回的总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns>查询成功返回记录集，查询失败返回null</returns>
        public static IList<IDictionary<string, string>> GetPageDataTable(string sql, int pageSize, int pageIndex, out int totalCount, out string msg)
        {
            IList<IDictionary<string, string>> ret = null;
            msg = "";
            totalCount = 0;
            try
            {
                ret = CommonService.GetPageData(sql, pageSize, pageIndex, out totalCount);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="sqls">sql列表</param>
        /// <param name="msg">错误信息</param>
        /// <returns>操作成功返回true，失败返回false</returns>
        public static bool ExecTrans(IList<string> sqls, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                ret = CommonService.ExecTrans(sqls, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="msg">错误信息</param>
        /// <returns>操作成功返回true，失败返回false</returns>
        public static bool Execute(string sql, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                IList<string> sqls = new List<string>();
                sqls.Add(sql);
                ret = CommonService.ExecTrans(sqls, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return ret;
        }
    }
}
