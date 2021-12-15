using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.IDataInputBll;
using System.Text.RegularExpressions;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Web.Func
{
    public class SysYcdyApiObject
    {
        public SysYcdyUrl Url { get; set; }
        public IList<SysYcdyTable> Tables { get; set; }
        public IList<SysYcdyParam> Searchs { get; set; }
        public IList<SysYcdyTableRelation> Relations { get; set; }
        public IList<SysYcdyField> Fields { get; set; }
        public IList<SysYcdyPrimaryKey> PrimaryKeys { get; set; }
        public ICommonService CommonService { get; set; }
        public IDataInputService DataInputService { get; set; }
        public IYcbaService YcbaService { get; set; }
        public IList<IDictionary<string, string>> Zdzds { get; set; }

        public string Id { get; set; }    // callid,version
       
        public string RemoteUrl { get; set; }
        /// <summary>
        /// 远程表定义为空，对象是json数组格式，只允许有一个表
        /// </summary>

        public bool IsRemoteTableNull
        {
            get
            {
                var items = Tables.Where(e => String.IsNullOrEmpty(e.RemoteTable));
                return items.Count() > 0;
            }
        }
       
        public SysYcdyApiObject(SysYcdyUrl url, IList<SysYcdyTable> tables, IList<SysYcdyParam> searchs,
        IList<SysYcdyTableRelation> relations, IList<SysYcdyField> fields,
        ICommonService commonService, IDataInputService dataInputService, IYcbaService ycbaService, string remoteUrl, string id)
        {
            Url = url;
            Tables = tables;
            Searchs = searchs;
            Relations = relations;
            Fields = fields;
            CommonService = commonService;
            DataInputService = dataInputService;
            YcbaService = ycbaService;
            RemoteUrl = remoteUrl;
            Id = id;
            LoadZdzd();
        }

        public void LoadZdzd()
        {
            try
            {
                if (!IsValid())
                    return;
                Zdzds = new List<IDictionary<string, string>>();
                IDictionary<string, string> tableZdzd = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                StringBuilder sbTableName = new StringBuilder();
                foreach (SysYcdyTable table in Tables)
                    sbTableName.Append(table.LocalTableName + ",");
                IList<IDictionary<string, string>> sjbsms = CommonService.GetDataTable("select sjbmc,sszdzd from pr_m_sjbsm where sjbmc in (" + sbTableName.ToString().FormatSQLInStr() + ")");
                foreach (IDictionary<string, string> row in sjbsms)
                {
                    string zdzdTables = "";
                    if (tableZdzd.TryGetValue(row["sszdzd"], out zdzdTables))
                        tableZdzd[row["sszdzd"]] = zdzdTables + row["sjbmc"] + ",";
                    else
                        tableZdzd.Add(row["sszdzd"], row["sjbmc"] + ",");
                }
                foreach (string key in tableZdzd.Keys)
                {
                    IList<IDictionary<string,string>> oneZdzds = CommonService.GetDataTable("select sjbmc,zdmc,sy,sfbhzd,bhms,defaval from "+key+" where sjbmc in ("+tableZdzd[key].FormatSQLInStr()+")");
                    foreach (IDictionary<string, string> row in oneZdzds)
                    {
                        //人员没账号, 审批未通过
                        if (row["sjbmc"].ToUpper() == "I_M_RY" && row["zdmc"].ToUpper() == "SPTG")
                        {
                            row["defaval"] = "0";
                        }

                        row.Add("zdzdbm", key);
                    }
                    ((List<IDictionary<string, string>>)Zdzds).AddRange(oneZdzds);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }
        // 是否有效
        public bool IsValid()
        {
            bool ret = false;
            try
            {
                ret = Url != null && Tables.Count > 0 && Fields.Count() > 0;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        // 获取根表格，只有一个表就返回一个
        public bool GetRootTable(out SysYcdyTable table, out SysYcdyTableRelation relation)
        {
            bool ret = false;
            table = null;
            relation = null;
            try
            {
                if (!IsValid())
                    return ret;
                var relations = Relations.Where(r => (String.IsNullOrEmpty(r.LocalParentTable))).ToList();
                if (relations.Count == 0)
                    return ret;
                relation = relations[0];
                var rootTables = Tables.Where(e=>e.LocalTableName.Equals(e.LocalTableName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (rootTables.Count == 0)
                    return ret;
                table = rootTables[0];
                ret = true;

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 根据json对象的标签，获取表格，三重表结构，可能会获取多个
        /// </summary>
        /// <returns></returns>
        public IList<SysYcdyTable> GetTables(string tag)
        {
            IList<SysYcdyTable> ret = new List<SysYcdyTable>();
            try
            {
                if (IsDetailStruct(tag))
                    ret = Tables.Where(e => e.RemoteTable.IndexOf(tag + ".", StringComparison.OrdinalIgnoreCase) == 0).ToList();
                else
                    ret = Tables.Where(e => e.RemoteTable.Equals(tag, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        ///  根据json对象的标签，结合数据库定义，判断是否是三重结构。数据库中有 监理单位.单位的定义表示是三重
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool IsDetailStruct(string tag)
        {
            bool ret = false;
            try
            {
                var ts = Tables.Where(e => e.RemoteTable.IndexOf(tag+".", StringComparison.OrdinalIgnoreCase) == 0).ToList();
                if (ts.Count > 0)
                    ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 根据数据库字段定义，获取json数据主键属性
        /// </summary>
        /// <param name="rootTableName"></param>
        /// <param name="localPlatformKey"></param>
        /// <returns></returns>
        public string GetRemotePlatformKey(string rootTableName, string localPlatformKey)
        {
            string ret = "";
            try
            {
                var finds = Fields.Where(e => e.TableName.Equals(rootTableName, StringComparison.OrdinalIgnoreCase) && e.LocalField.Equals(localPlatformKey, StringComparison.OrdinalIgnoreCase));
                if (finds.Count() > 0)
                    ret = finds.ElementAt(0).RemoteField;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 根据数据库字段定义，获取json数据更新属性
        /// </summary>
        /// <param name="rootTAbleName"></param>
        /// <param name="localUpdateField"></param>
        /// <returns></returns>
        public string GetRemoteUpdateKey(string rootTableName, string localUpdateField)
        {
            string ret = "";
            try
            {
                var finds = Fields.Where(e => e.TableName.Equals(rootTableName, StringComparison.OrdinalIgnoreCase) && e.LocalField.Equals(localUpdateField, StringComparison.OrdinalIgnoreCase));
                if (finds.Count() > 0)
                    ret = finds.ElementAt(0).RemoteField;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取本地最后跟新时间sql
        /// </summary>
        /// <returns></returns>
        public string GetLastUpdateSql(string tablename, string keyfield, string updatefield, string keyvalue)
        {
            string ret = "";
            try
            {
                if (string.IsNullOrEmpty(keyfield))
                ret = string.Format("select {0} from {1} where {2}='{3}'", updatefield, tablename, keyfield, keyvalue);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取本地是否有平台记录sql
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="keyfield"></param>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public string GetHasRecordSql(string tablename, string keyfield, string keyvalue)
        {
            string ret = "";
            try
            {
                if (!string.IsNullOrEmpty(keyfield))
                    ret = string.Format("select count(*) from {0} where {1}='{2}'", tablename, keyfield, keyvalue);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 根据表格获取relation
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public SysYcdyTableRelation GetRelation(string tablename)
        {
            SysYcdyTableRelation ret = null;
            try
            {
                ret = Relations.Where(e => e.LocalTableName.Equals(tablename, StringComparison.OrdinalIgnoreCase)).ToList()[0];
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取表格的保存字段
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public IList<SysYcdyField> GetSaveFields(string tablename, bool isUpdate)
        {
            IList<SysYcdyField> fields = new List<SysYcdyField>();
            try
            {
                if (!isUpdate)
                    fields = Fields.Where(e => e.TableName.Equals(tablename, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(e.LocalField)).OrderBy(e=>e.DetailDisplayOrder).ToList();
                else
                    fields = Fields.Where(e => e.TableName.Equals(tablename, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(e.LocalField)&& !string.IsNullOrEmpty(e.RemoteField) && e.RemoteField.IndexOf("Local-",StringComparison.OrdinalIgnoreCase)!=0).OrderBy(e=>e.DetailDisplayOrder).ToList() ;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return fields;
        }
        /// <summary>
        /// 获取字段值，编号模式和从本地表取数的字段，保留变量
        /// </summary>
        /// <param name="field"></param>
        /// <param name="data"></param>
        /// <param name="allDatas"></param>
        /// <returns></returns>
        public object GetFieldValue(SysYcdyField field, IDictionary<string,object> data, IDictionary<string, object> allDatas, out bool isDynamic)
        {
            object ret = null;
            isDynamic = false;
            try
            {
                SysYcdyTable table = (Tables.Where(e => e.LocalTableName.Equals(field.TableName, StringComparison.OrdinalIgnoreCase)).ToList())[0];
                // 从本地数据表获取
                if (field.RemoteField.StartsWith("'") && field.RemoteField.EndsWith("'"))
                {
                    isDynamic = false;
                    string str = field.RemoteField;
                    str = str.Substring(1);
                    if (str.Length > 0)
                        str = str.Remove(str.Length - 1);
                    ret = str;
                }
                else if (field.RemoteField.StartsWith("Local-", StringComparison.OrdinalIgnoreCase))
                {
                    string str = field.RemoteField.Substring("Local-".Length);
                    string[] arr = str.Split(new char[] { '|' });
                    string fieldTable = "";
                    string fieldName = "";
                    string fieldWhere = "";
                    foreach (string strItem in arr)
                    {
                        str = strItem.Trim();
                        if (str.StartsWith("table-", StringComparison.OrdinalIgnoreCase))
                            fieldTable = str.Substring("table-".Length);
                        else if (str.StartsWith("fieldname-", StringComparison.OrdinalIgnoreCase))
                            fieldName = str.Substring("fieldname-".Length);
                        else if (str.StartsWith("where-", StringComparison.OrdinalIgnoreCase))
                            fieldWhere = str.Substring("where-".Length);
                    }
                    if (string.IsNullOrEmpty(fieldTable) || string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldWhere))
                        SysLog4.WriteError(field.CallId + "," + field.VersionNo + "," + field.TableName + "," + field.RemoteField);
                    else
                    {
                        Regex reg = new Regex(@"\{[^\}]*\}", RegexOptions.IgnoreCase);
                        MatchCollection matchCol = reg.Matches(fieldWhere);
                        IList<KeyValuePair<string, string>> matchValues = new List<KeyValuePair<string, string>>();
                        foreach (Match match in matchCol)
                        {
                            string replaceField = match.Value.Substring(1, match.Value.Length - 2);
                            matchValues.Add(new KeyValuePair<string, string>(match.Value, data[replaceField].GetSafeString()));
                        }
                        foreach (KeyValuePair<string, string> matchItem in matchValues)
                        {
                            fieldWhere = fieldWhere.Replace(matchItem.Key, "'" + matchItem.Value + "'");
                        }
                        ret = string.Format("select-select {0} as keyvalue from {1} where {2}", fieldName, fieldTable, fieldWhere);
                    }

                    isDynamic = true;

                }
                // 远程从表字段
                else if (field.RemoteField.IndexOf(".") > -1)
                {
                    string[] arr = field.RemoteField.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 2)
                    {
                        StringBuilder sb = new StringBuilder();
                        object[] subdatas = allDatas[arr[0]] as object[];
                        foreach (object row in subdatas){
                            IDictionary<string,object> realRow = row as IDictionary<string,object>;
                            if (field.SubTableFieldType == 1){
                                sb.Append(realRow[arr[1]]);
                                break;
                            }
                            else
                            {
                                if (sb.Length > 0)
                                    sb.Append(",");
                                sb.Append(realRow[arr[1]].GetSafeString());
                            }
                        }
                        ret = sb.ToString();
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        object[] subdatas = allDatas[arr[0]] as object[];
                        foreach (object row in subdatas)
                        {
                            IDictionary<string, object> realRow = row as IDictionary<string, object>;
                            if (field.SubTableFieldType == 1)
                            {
                                if (!table.IsJsonArray)
                                    sb.Append((realRow[arr[1]] as IDictionary<string,object>)[arr[2]]);
                                else
                                {
                                    if ((realRow[arr[1]] as object[]).Length>0)
                                        sb.Append((((realRow[arr[1]] as object[])[0]) as IDictionary<string,object> )[arr[2]]);
                                }

                                break;
                            }
                            else
                            {
                                if (sb.Length > 0)
                                    sb.Append(",");
                                if (!table.IsJsonArray)
                                    sb.Append((realRow[arr[1]] as IDictionary<string, object>)[arr[2]]);
                                else
                                {
                                    if ((realRow[arr[1]] as object[]).Length > 0)
                                        sb.Append((((realRow[arr[1]] as object[])[0]) as IDictionary<string, object>)[arr[2]]);
                                }

                            }
                        }
                        ret = sb.ToString();
                    }
                    isDynamic = false;
                }
                // 当前记录
                else if (!String.IsNullOrEmpty(field.RemoteField))
                {
                    ret = data[field.RemoteField];
                    isDynamic = false;
                }
                // 根据zdzd定义
                else
                {
                    IList<IDictionary<string, string>> zdzds = Zdzds.Where(e => e["sjbmc"].Equals(field.TableName) && e["zdmc"].Equals(field.LocalField)).ToList();
                    if (zdzds.Count > 0)
                    {
                        IDictionary<string, string> zdzd = zdzds[0];
                        if (zdzd["sfbhzd"].GetSafeBool())
                        {
                            ret = "bh-" + zdzd["sjbmc"] + ":" + zdzd["zdmc"] + "-" + zdzd["bhms"];
                            isDynamic = true;
                        }
                        else
                        {
                            ret = DataInputService.GetUserDefval(zdzd["zdzdbm"], zdzd["sjbmc"], zdzd["zdmc"], DataInputService.GetZdzdDefval(zdzd["defaval"]));
                            isDynamic = false;
                        }
                    }
                    else
                        SysLog4.WriteError(field.TableName + "," + field.LocalField + ",找不到zdzd");
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            if (ret == null)
                ret = "";
            return ret;
        }
        /// <summary>
        /// 根据主表的平台主键和更新字段，判断记录是否需要更新。如果平台编号一样，更新时间字段也一样，不需要处理
        /// </summary>
        /// <param name="rootData"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool NeedUpdate(IDictionary<string, object> rootData, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                if (rootData == null)
                    return false;
                SysYcdyTable rootTable = null;
                SysYcdyTableRelation rootRelation = null;
                ret = this.GetRootTable(out rootTable, out rootRelation);
                if (!ret)
                    msg = "找不到根表定义";
                else
                {
                    string rootLocalPlatformKey = rootRelation.RemotePrimaryKey;
                    string rootRemotePlatformKey = this.GetRemotePlatformKey(rootRelation.LocalTableName, rootLocalPlatformKey);
                    string rootLocalUpdateField = rootTable.UpdateField;
                    string rootRemoteUpdateField = this.GetRemoteUpdateKey(rootTable.LocalTableName, rootLocalUpdateField);

                    if (string.IsNullOrEmpty(rootLocalPlatformKey) || string.IsNullOrEmpty(rootRemotePlatformKey) ||
                        string.IsNullOrEmpty(rootLocalUpdateField) || string.IsNullOrEmpty(rootRemoteUpdateField))
                        msg = "找不到本地或远程的根表主键字段，或者根表更新字段";
                    else
                    {
                        IDictionary<string, object> primaryData = null;
                        if (string.IsNullOrEmpty(rootTable.RemoteTable))
                            primaryData = rootData;
                        else
                            primaryData = rootData[rootTable.RemoteTable] as IDictionary<string, object>;

                        string rootRemotePlatformValue = primaryData[rootRemotePlatformKey].GetSafeString();
                        string rootRemoteUpdateValue = primaryData[rootRemoteUpdateField].GetSafeString();
                        string sql = string.Format("select {0} as c1 from {1} where {2}='{3}'",
                                rootLocalUpdateField,
                                rootTable.LocalTableName,
                                rootLocalPlatformKey,
                                rootRemotePlatformValue);
                        IList<IDictionary<string, string>> findRows = CommonService.GetDataTable(sql);
                        if (findRows.Count > 0)
                            ret = findRows[0]["c1"] != rootRemoteUpdateValue;
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 某条记录是更新还是新建操作
        /// </summary>
        /// <param name="data"></param>
        /// <param name="table"></param>
        /// <param name="relation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsUpdate(IDictionary<string, object> data, SysYcdyTable table, SysYcdyTableRelation relation, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                string localPlatformKey = relation.RemotePrimaryKey;
                string remotePlatformKey = this.GetRemotePlatformKey(relation.LocalTableName, localPlatformKey);

                if (string.IsNullOrEmpty(localPlatformKey) || string.IsNullOrEmpty(remotePlatformKey))
                    ret = false;
                else
                {
                    string rootRemotePlatformValue = data[remotePlatformKey] as string;
                    string sql = string.Format("select count(*) from {0} where {1}='{2}'",
                            table.LocalTableName,
                            localPlatformKey,
                            rootRemotePlatformValue);

                    IList<IDictionary<string, string>> findRows = CommonService.GetDataTable(sql);
                    ret = findRows[0].ElementAt(0).Value.GetSafeInt() > 0;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取单条插入语句，如果有文件，下载文件并生成语句
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sql"></param>
        /// <param name="fileSql"></param>
        /// <returns></returns>
        public bool GetInsertSql(IDictionary<string,object> allDatas, IDictionary<string, object> data, SysYcdyTable table, 
            out string sql, out IList<VSqlParam> sqlParams, out IList<VDataFileItem> files, out string msg)
        {
            bool code = false;
            msg = "";
            sql = "";
            sqlParams = new List<VSqlParam>();
            files = new List<VDataFileItem>();
            try
            {
                StringBuilder fieldStr = new StringBuilder();
                StringBuilder valueStr = new StringBuilder();
                IList<SysYcdyField> fields = GetSaveFields(table.LocalTableName,false );
                IList<KeyValuePair<string, object>> valueFields = new List<KeyValuePair<string, object>>();
                foreach (SysYcdyField field in fields)
                {
                    if (fieldStr.Length > 0)
                        fieldStr.Append(",");
                    fieldStr.Append(field.LocalField);

                    if (valueStr.Length > 0)
                        valueStr.Append(",");
                    valueStr.Append("@" + field.LocalField + "");

                    bool isDynamic;
                    object fieldValue = GetFieldValue(field, data, allDatas, out isDynamic );
                    

                    if (field.IsFile)
                    {
                        string fieldFile = fieldValue.GetSafeString();
                        if (!String.IsNullOrEmpty(fieldFile))
                        {
                            IList<VDataFileItem> fieldFiles = GetFiles(ref fieldFile);
                            if (fieldFiles.Count > 0)
                                ((List<VDataFileItem>)files).AddRange(fieldFiles);
                            fieldValue = fieldFile;
                        }
                    }

                    sqlParams.Add(new VSqlParam() { IsDynamic = isDynamic, ParamName = field.LocalField, ParamValue = fieldValue });
                }
                if (fieldStr.Length>0)
                    sql = "insert into " + table.LocalTableName + "(" + fieldStr.ToString() + ") values(" + valueStr.ToString() + ")";
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
        /// <summary>
        /// 获取下载文件内容
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IList<VDataFileItem> GetFiles(ref string filename)
        {
            IList<VDataFileItem> ret = new List<VDataFileItem>();
            try
            {
                filename = filename.Trim();

                if (string.IsNullOrEmpty(filename))
                    return ret;
                string[] files = filename.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                filename = "";
                foreach (string fileitem in files)
                {
                    int idIndex = fileitem.IndexOf(",");
                    string curfileid = fileitem;
                    string curfilename = "";
                    if (idIndex > -1)
                    {
                        curfileid = fileitem.Substring(0, idIndex);
                        curfilename = fileitem.Substring(idIndex + 1);
                    }
                    VDataFileItem item = GetFile(ref curfileid, curfilename);
                    ret.Add(item);
                    filename += curfileid + ","+curfilename + "|";
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 获取远程单个文件
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public VDataFileItem GetFile(ref string fileid, string filename)
        {
            VDataFileItem item = new VDataFileItem();
            try
            {
                IDictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams.Add("id", fileid);
                string msg = "";
                byte[] filesmall = MyHttp.GetDownFile(RemoteUrl+ Url.DownFileUrl, queryParams, out msg);
                queryParams.Add("type", "big");
                byte[] filebig = MyHttp.GetDownFile(RemoteUrl + Url.DownFileUrl, queryParams, out msg);

                fileid = "P_" + fileid;
                item.FILEID = fileid;
                item.FILENAME = filename;
                item.FILECONTENT = filebig;
                item.FILEEXT = System.IO.Path.GetExtension(filename);
                item.CJSJ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.SMALLCONTENT = filesmall;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return item;
        }

        /// <summary>
        /// 获取单条更新语句，如果有文件，下载文件并生成语句
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sql"></param>
        /// <param name="fileSql"></param>
        /// <returns></returns>
        public bool GetUpdateSql(IDictionary<string, object> allDatas, IDictionary<string, object> data, SysYcdyTable table,
            out string sql, out IList<VSqlParam> sqlParams, out IList<VDataFileItem> files, out string msg)
        {
            bool code = false;
            msg = "";
            sql = "";
            sqlParams = new List<VSqlParam>();
            files = new List<VDataFileItem>();
            try
            {
                StringBuilder fieldStr = new StringBuilder();
                IList<SysYcdyField> fields = GetSaveFields(table.LocalTableName, true);
                IList<KeyValuePair<string, object>> valueFields = new List<KeyValuePair<string, object>>();
                foreach (SysYcdyField field in fields)
                {
                    bool isDynamic;
                    object fieldValue = GetFieldValue(field, data, allDatas, out isDynamic);
                    if (isDynamic)
                        continue;
                    if (fieldStr.Length > 0)
                        fieldStr.Append(",");
                    fieldStr.Append(field.LocalField + "=@" + field.LocalField+"");                   

                    if (field.IsFile)
                    {
                        string fieldFile = fieldValue.GetSafeString();
                        if (!String.IsNullOrEmpty(fieldFile))
                        {
                            IList<VDataFileItem> fieldFiles = GetFiles(ref fieldFile);
                            if (fieldFiles.Count > 0)
                                ((List<VDataFileItem>)files).AddRange(fieldFiles);
                            fieldValue = fieldFile;
                        }
                    }

                    sqlParams.Add(new VSqlParam() { IsDynamic = isDynamic, ParamName = field.LocalField, ParamValue = fieldValue });

                }
                SysYcdyTableRelation relation = GetRelation(table.LocalTableName);
                string localPlatformKey = relation.RemotePrimaryKey;
                string remotePlatformKey = GetRemotePlatformKey(table.LocalTableName, localPlatformKey);
                string rootRemotePlatformValue = data[remotePlatformKey].GetSafeString();
                sql = "update " + table.LocalTableName + " set " + fieldStr.ToString() + " where " + localPlatformKey + "='" + rootRemotePlatformValue + "'";
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }

        /// <summary>
        /// 根据表获取PrimaryKey
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public SysYcdyPrimaryKey GetYcdyPrimaryKey(string tablename)
        {
            SysYcdyPrimaryKey ret = null;
            try
            {
                ret = PrimaryKeys.FirstOrDefault(x => x.TableName.Equals(tablename, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 如果远程主键值为空,且配置了新的主键, 则替换为新的主键
        /// </summary>
        /// <param name="data"></param>
        /// <param name="localTableName"></param>
        /// <param name="localPlatformKey"></param>
        /// <param name="remotePlatformValue"></param>
        /// <returns></returns>
        public bool GetNewPrimaryKey(IDictionary<string, object> data, string localTableName, ref string localPlatformKey, ref string remotePlatformValue)
        { 
            //如果远程主键值为空,且配置了新的主键, 则替换为新的主键
            var info = GetYcdyPrimaryKey(localTableName);
            var newPlatformKey = string.Empty;

            if (info != null)
                newPlatformKey = info.PrimaryKey;

            if (string.IsNullOrEmpty(remotePlatformValue) && !string.IsNullOrEmpty(newPlatformKey))
            {
                var remotePlatformKey = GetRemotePlatformKey(localTableName, newPlatformKey);

                if (string.IsNullOrEmpty(remotePlatformKey))
                    return false;

                if (!data.ContainsKey(remotePlatformKey))
                    return false;

                localPlatformKey = newPlatformKey;
                remotePlatformValue = data[remotePlatformKey].GetSafeString();
            }

            return true;
        }
        
    }

}