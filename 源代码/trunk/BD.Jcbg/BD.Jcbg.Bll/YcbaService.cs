using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.IDataInputDao;

namespace BD.Jcbg.Bll
{
    /// <summary>
    /// 从四库平台获取数据
    /// </summary>
    public class YcbaService:IYcbaService
    {
        #region 数据库对象
        public ISysycdyfieldDao SysycdyfieldDao { get; set; }
        public ISysycdyparamDao SysycdyparamDao { get; set; }
        public ISysycdytableDao SysycdytableDao { get; set; }
        public ISysycdytablerelationDao SysycdytablerelationDao { get; set; }
        public ISysycdyurlDao SysycdyurlDao { get; set; }

        public IWebDataInputDao WebDataInputDao { get; set; }

        public ICommonDao CommonDao { get; set; }
        public ISysYcdyStationDao SysYcdyStationDao { get; set; }
        #endregion

        public IList<SysYcdyStation> GetStations()
        {
            return SysYcdyStationDao.GetAll();
        }
        public SysYcdyUrl GetUrl(string callid, string version)
        {
            SysYcdyUrl ret = null;
            try
            {
                IList<SysYcdyUrl> urls = SysycdyurlDao.Gets(callid);

                var q = from e in urls where e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase) select e;
                if (q.Count() > 0)
                    ret = q.First();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<SysYcdyParam> GetParams(string callid, string version)
        {
            IList<SysYcdyParam> ret = new List<SysYcdyParam>();
            try
            {
                ret = SysycdyparamDao.Gets(callid);
                var q = from e in ret where e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase) select e;
                ret = q.ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<SysYcdyTable> GetTables(string callid, string version)
        {
            IList<SysYcdyTable> ret = new List<SysYcdyTable>();
            try
            {
                ret = SysycdytableDao.Gets(callid);
                var q = from e in ret where e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase) select e;
                ret = q.ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<SysYcdyTableRelation> GetTableRelations(string callid, string version)
        {
            IList<SysYcdyTableRelation> ret = new List<SysYcdyTableRelation>();
            try
            {
                ret = SysycdytablerelationDao.Gets(callid);
                var q = from e in ret where e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase) select e;
                ret = q.ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<SysYcdyField> GetFields(string callid, string version)
        {
            IList<SysYcdyField> ret = new List<SysYcdyField>();
            try
            {
                ret = SysycdyfieldDao.Gets(callid);
                var q = from e in ret where e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase) select e;
                ret = q.ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public IList<SysYcdyPrimaryKey> GetPrimaryKeys(string callid, string version)
        {
            IList<SysYcdyPrimaryKey> ret = new List<SysYcdyPrimaryKey>();
            try
            {
                string sql = string.Format("select * from SysYcdyPrimaryKey where callid = '{0}' and version = '{1}'", callid, version);
                var datas = CommonDao.GetDataTable(sql);
                SysYcdyPrimaryKey info = null;

                foreach (var data in datas)
                {
                    info = new SysYcdyPrimaryKey();
                    info.RecId = data["recid"].GetSafeInt();
                    info.CallId = data["callid"].GetSafeString();
                    info.VersionNo = data["versionno"].GetSafeString();
                    info.TableName = data["tablename"].GetSafeString();
                    info.PrimaryKey = data["primarykey"].GetSafeString();
                    ret.Add(info);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public string GetBH(string bhms, string tablename, string columnname, DataRow dr, IDbCommand cmd, ref bool firstOpt)
        {
            string ret = "";
            try{
                //ret = WebDataInputDao.GetBH(bhms)
            }
            catch(Exception ex){
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        [Transaction(ReadOnly=false)]
        public bool SaveData(IList<IDictionary<string, object>> sqls, out string msg)
        {
            msg = "";
            bool code = false;
            try
            {
                foreach (IDictionary<string, object> sqlObj in sqls)
                {
                    
                    // 表格记录
                    string sql = sqlObj["sql"].GetSafeString();
                    IList<VSqlParam> sqlParams = sqlObj["params"] as IList<VSqlParam>;
                    IList<VDataFileItem> sqlFiles = sqlObj["files"] as IList<VDataFileItem>;
                    
                    IList<IDataParameter> realParams = new List<IDataParameter>();
                    foreach (VSqlParam vParam in sqlParams)
                    {
                        object paramValue = vParam.ParamValue;
                        if (vParam.IsDynamic)
                        {
                            string vValue = paramValue.GetSafeString();
                            if (vValue.IndexOf("select-", StringComparison.OrdinalIgnoreCase)>-1)
                            {
                                string tmpSql = vValue.Substring("select-".Length);
                                IList<IDictionary<string, string>> tmpDatas = CommonDao.GetDataTableSameTrans(tmpSql);
                                if (tmpDatas.Count > 0)
                                    paramValue = tmpDatas[0].ElementAt(0).Value;
                                else
                                    paramValue = "";
                            }
                            else if (vValue.IndexOf("bh-", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                // 生成datarow,编号字段用到
                                DataTable dt = new DataTable();
                                foreach (IDataParameter rp in realParams)
                                {
                                    dt.Columns.Add(rp.ParameterName.Substring(1));
                                }
                                DataRow dr = dt.NewRow();
                                foreach (IDataParameter rp in realParams)
                                {
                                    dr[rp.ParameterName.Substring(1)] = rp.Value;
                                }
                                string tmpBh = vValue.Substring("bh-".Length);
                                string tmpSjbmc = tmpBh.Substring(0,tmpBh.IndexOf(':'));
                                tmpBh = tmpBh.Substring(tmpBh.IndexOf(':')+1);
                                string tmpZdmc = tmpBh.Substring(0, tmpBh.IndexOf('-'));
                                tmpBh = tmpBh.Substring(tmpBh.IndexOf('-')+1);
                                bool firstOpt = false;
                                
                                paramValue = WebDataInputDao.GetBH(tmpBh, tmpSjbmc, tmpZdmc, dr, null, ref firstOpt);
                            }
                        }
                        IDataParameter param = new SqlParameter("@" + vParam.ParamName, paramValue);
                        realParams.Add(param);
                    }
                    CommonDao.ExecCommand(sql, CommandType.Text, realParams);
                    // 文件记录
                    foreach (VDataFileItem file in sqlFiles)
                    {
                        realParams.Clear();
                        //if (CommonDao.GetDataTable("select count(*) from DATAFILE where FILEID='" + file.FILEID + "'")[0].ElementAt(0).Value.GetSafeInt() > 0)
                        //    continue;
                        CommonDao.ExecCommand("delete from DATAFILE where FILEID='" + file.FILEID + "'", CommandType.Text);
                        sql = "insert into DATAFILE(FILEID,FILENAME,FILECONTENT,FILEEXT,CJSJ,SMALLCONTENT) values(@FILEID,@FILENAME,@FILECONTENT,@FILEEXT,@CJSJ,@SMALLCONTENT)";
                        IDataParameter param = new SqlParameter("@FILEID", file.FILEID);
                        realParams.Add(param);
                        param = new SqlParameter("@FILENAME", file.FILENAME);
                        realParams.Add(param);
                        param = new SqlParameter("@FILECONTENT", SqlDbType.Image) { Value = file.FILECONTENT };
                        realParams.Add(param);
                        param = new SqlParameter("@FILEEXT", file.FILEEXT);
                        realParams.Add(param);
                        param = new SqlParameter("@CJSJ", file.CJSJ);
                        realParams.Add(param);
                        param = new SqlParameter("@SMALLCONTENT", SqlDbType.Image) { Value = file.SMALLCONTENT };
                        realParams.Add(param);
                        CommonDao.ExecCommand(sql, CommandType.Text, realParams);
                    }
                }
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
                throw new Exception(ex.Message);
            }
            return code;
        }

        public IList<SysYcdyUrl> GetAllUrl()
        {
            return SysycdyurlDao.GetAll();
        }
        public IList<SysYcdyTable> GetAllTable()
        {
            return SysycdytableDao.GetAll();
        }
        public IList<SysYcdyTableRelation> GetAllRelations()
        {
            return SysycdytablerelationDao.GetAll();
        }
        public IList<SysYcdyParam> GetAllParams()
        {
            return SysycdyparamDao.GetAll();
        }
        public IList<SysYcdyField> GetAllFields()
        {
            return SysycdyfieldDao.GetAll();
        }
    }
}
