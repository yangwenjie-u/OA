using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using BD.Jcbg.Web.Func;
using BD.IDataInputBll;

namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 调用远程备案数据
    /// </summary>
    public class YcbaController:Controller
    {
        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
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
        private IYcbaService _ycbaService = null;
        private IYcbaService YcbaService
        {
            get
            {
                if (_ycbaService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _ycbaService = webApplicationContext.GetObject("YcbaService") as IYcbaService;
                }
                return _ycbaService;
            }
        }

        private IDataInputService _dataInputService = null;
        private IDataInputService DataInputService
        {
            get
            {
                if (_dataInputService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _dataInputService = webApplicationContext.GetObject("DataInputService") as IDataInputService;
                }
                return _dataInputService;
            }
        }
        #endregion
        #region 页面
        /// <summary>
        /// webapi分页
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult ApiPageList()
        {
            ViewBag.callid = Request["callid"].GetSafeString();
            ViewBag.version = Request["version"].GetSafeString();
            ViewBag.url = Request["url"].GetSafeString();
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取ctrlstring数据
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetCtrlInfo(string ctrlstring)
        {
            bool code = false;
            string msg = "";
            IList<IDictionary<string,string>> records = new List<IDictionary<string,string>>();
            try
            {

                msg = DataInputService.GetCtrlString(ctrlstring,"");
                msg = msg.Replace("value--", "");
                if (msg != "")
                {
                    string[] rows = msg.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string row in rows)
                    {
                        string[] cells = row.Split(new char[] { ',' });
                        
                        if (cells.Length < 3)
                            continue;
                        IDictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("itemdesc", cells[0]);
                        dic.Add("itemvalue", cells[1]);
                        dic.Add("selected", cells[2]);
                        records.Add(dic);
                    }
                }
                msg = "";
                code = true;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {

            }
            return Json(new { code = (code ? "0" : "1"), msg = msg , records=records});
        }
        /// <summary>
        /// 获取远程调用定义
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetYcdyDefine(string callid, string version)
        {
            SysYcdyUrl url = null;
            IList<SysYcdyTable> tables = new List<SysYcdyTable>();
            IList<SysYcdyParam> searchparam = new List<SysYcdyParam>();
            IList<SysYcdyTableRelation> relations = new List<SysYcdyTableRelation>();
            IList<SysYcdyField> fields = new List<SysYcdyField>();
            string msg = "";
            try
            {
                url = YcbaService.GetUrl(callid, version);
                tables = YcbaService.GetTables(callid, version);
                searchparam = YcbaService.GetParams(callid, version);
                relations = YcbaService.GetTableRelations(callid, version);
                fields = YcbaService.GetFields(callid, version);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            finally
            {

            }
            return Json(new { code = (msg == "" ? "0" : "1"), msg = msg, url=url, tables=tables, searchs=searchparam,relations=relations, fields=fields });
        }

        [Authorize]
        public JsonResult GetStations()
        {
            IList<SysYcdyStation> stations = new List<SysYcdyStation>();
            try
            {
                stations = YcbaService.GetStations();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(stations);
        }
        #endregion

        #region 数据操作
        [Authorize]
        public JsonResult DoSaveApiItem(string callid, string version, string key)
        {
            bool code = false;
            string msg = "";
            try
            {
                IList<SysYcdyStation> stations = YcbaService.GetStations();
                stations = stations.Where(e => e.VersionNo.Equals(version, StringComparison.OrdinalIgnoreCase)).ToList();
                SysYcdyStation station = stations[0];
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                IDictionary<string, object> obj = jsonSerializer.Deserialize<IDictionary<string, object>>(key);
                IList<IDictionary<string, object>> sqls = new List<IDictionary<string, object>>();

                IList<SysYcdyTable> tables = YcbaService.GetTables(callid, version);
                SysYcdyApiObject apiObj = new SysYcdyApiObject(
                    YcbaService.GetUrl(callid, version),
                    tables,
                    YcbaService.GetParams(callid, version),
                    YcbaService.GetTableRelations(callid, version),
                    YcbaService.GetFields(callid, version),
                    CommonService, DataInputService, YcbaService, station.RootUrl, callid + "," + version);
                // 获取该调用相关调用
                IList<SysYcdyApiObject> relateObjects = GetRelateObjects(station, tables);

                if (!apiObj.IsValid())
                    msg = "无效的调用ID或者版本";
                else
                {
                    SysYcdyTable rootTable = null;
                    SysYcdyTableRelation rootRelation = null;
                    code = apiObj.GetRootTable(out rootTable, out rootRelation);
                    if (!code)
                        msg = "找不到根表定义";
                    else
                    {
                        bool needUpdate = apiObj.NeedUpdate(obj, out msg);
                        // 没错误并且需要更新
                        if (msg == "" && needUpdate)
                        {

                            IList<IDictionary<string, object>> rowsDatas = new List<IDictionary<string, object>>();
                            // json数组
                            if (apiObj.IsRemoteTableNull)
                            {
                                IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                rowsData.Add("findtable", apiObj.Tables[0]);
                                rowsData.Add("findrelation", apiObj.Relations[0]);
                                rowsData.Add("rootdata", obj);
                                rowsDatas.Add(rowsData);
                            }
                            else
                            {
                                foreach (string property in obj.Keys)
                                {
                                    object propertyvalue = obj[property];
                                    IList<SysYcdyTable> findTables = apiObj.GetTables(property);
                                    // json对象在数据库中没定义，抛弃
                                    if (findTables.Count == 0)
                                        continue;

                                    #region 非三重表结构                                
                                    if (!apiObj.IsDetailStruct(property))
                                    {
                                        SysYcdyTable findTable = findTables[0];
                                        SysYcdyTableRelation findRelation = apiObj.GetRelation(findTable.LocalTableName);
                                        if (findRelation == null)
                                        {
                                            msg = findTable.LocalTableName + "找不到relation记录";
                                            SysLog4.WriteError(msg);
                                            break;
                                        }

                                        // 主表单条数据
                                        if (!findTable.IsJsonArray)
                                        {
                                            IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                            rowsData.Add("findtable", findTable);
                                            rowsData.Add("findrelation", findRelation);
                                            rowsData.Add("rootdata", propertyvalue);
                                            rowsDatas.Add(rowsData);

                                            // 如果有相关备案下载，添加到sql语句
                                            AddRelateSql(station, findTable, findRelation, relateObjects, propertyvalue as IDictionary<string, object>, ref sqls);
                                        }
                                        // 从表多条记录
                                        else
                                        {
                                            object[] subDatas = propertyvalue as object[];
                                            foreach (object subrow in subDatas)
                                            {
                                                IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                                rowsData.Add("findtable", findTable);
                                                rowsData.Add("findrelation", findRelation);
                                                rowsData.Add("rootdata", subrow);
                                                rowsDatas.Add(rowsData);

                                                // 如果有相关备案下载，添加到sql语句
                                                AddRelateSql(station, findTable, findRelation, relateObjects, subrow as IDictionary<string, object>, ref sqls);
                                            }

                                        }
                                    }
                                    #endregion
                                    #region 三重结构
                                    else
                                    {

                                        object[] rootSubDatas = propertyvalue as object[];
                                        foreach (object tmpData in rootSubDatas)
                                        {
                                            bool tmpCode = true;
                                            IDictionary<string, object> rowData = tmpData as IDictionary<string, object>;
                                            foreach (string subproperty in rowData.Keys)
                                            {
                                                IList<SysYcdyTable> detailTables = findTables.Where(e => e.RemoteTable.EndsWith(subproperty)).ToList();
                                                if (detailTables.Count == 0)
                                                    continue;
                                                SysYcdyTable findTable = detailTables[0];
                                                SysYcdyTableRelation findRelation = apiObj.GetRelation(findTable.LocalTableName);
                                                if (findRelation == null)
                                                {
                                                    msg = findTable.LocalTableName + "找不到relation记录";
                                                    SysLog4.WriteError(msg);
                                                    tmpCode = false;
                                                    break;
                                                }

                                                // 添加自己的记录
                                                if (!findTable.IsJsonArray)
                                                {
                                                    IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                                    rowsData.Add("findtable", findTable);
                                                    rowsData.Add("findrelation", findRelation);
                                                    rowsData.Add("rootdata", rowData[subproperty] as IDictionary<string, object>);
                                                    rowsDatas.Add(rowsData);

                                                    // 如果有相关备案下载，添加到sql语句
                                                    AddRelateSql(station, findTable, findRelation, relateObjects, rowData[subproperty] as IDictionary<string, object>, ref sqls);
                                                }
                                                else
                                                {
                                                    object[] subDatas = rowData[subproperty] as object[];
                                                    foreach (object tempData in subDatas)
                                                    {
                                                        IDictionary<string, object> subrow = tempData as IDictionary<string, object>;
                                                        IDictionary<string, object> rowsData = new Dictionary<string, object>();
                                                        rowsData.Add("findtable", findTable);
                                                        rowsData.Add("findrelation", findRelation);
                                                        rowsData.Add("rootdata", subrow);
                                                        rowsDatas.Add(rowsData);

                                                        // 如果有相关备案下载，添加到sql语句
                                                        AddRelateSql(station, findTable, findRelation, relateObjects, subrow, ref sqls);
                                                    }
                                                }
                                            }
                                            if (!tmpCode)
                                                break;
                                        }
                                    }
                                    #endregion
                                }

                            }

                            foreach (IDictionary<string, object> rowdata in rowsDatas)
                            {
                                SysYcdyTable findTable = rowdata["findtable"] as SysYcdyTable;
                                SysYcdyTableRelation findRelation = rowdata["findrelation"] as SysYcdyTableRelation;
                                IDictionary<string, object> data = rowdata["rootdata"] as IDictionary<string, object>;

                                bool isUpdate = apiObj.IsUpdate(data, findTable, findRelation, out msg);
                                if (msg != "")
                                    break;

                                string sql = "";
                                IList<VSqlParam> sqlParams = null;
                                IList<VDataFileItem> sqlFiles = null;
                                if (isUpdate)
                                {
                                    bool isSuccess = apiObj.GetUpdateSql(obj, data, findTable, out sql, out sqlParams, out sqlFiles, out msg);
                                    if (!isSuccess)
                                        break;
                                }
                                else
                                {
                                    bool isSuccess = apiObj.GetInsertSql(obj, data, findTable, out sql, out sqlParams, out sqlFiles, out msg);
                                    if (!isSuccess)
                                        break;
                                }

                                IDictionary<string, object> rowSql = new Dictionary<string, object>();
                                rowSql.Add("sql", sql);
                                rowSql.Add("params", sqlParams);
                                rowSql.Add("files", sqlFiles);
                                sqls.Add(rowSql);
                            }

                            if (msg == "")
                                code = YcbaService.SaveData(sqls, out msg);
                        }


                    }
                }
                code = msg == "";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return Json(new { code = code ? "0" : "1", msg = msg });
        }

        #endregion

        #region 相关信息下载内部调用
        /// <summary>
        /// 获取表格相关的调用
        /// </summary>
        /// <param name="station"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        private IList<SysYcdyApiObject> GetRelateObjects(SysYcdyStation station, IList<SysYcdyTable> tables)
        {
            IList<SysYcdyApiObject> relateObjects = new List<SysYcdyApiObject>();
            try
            {
                foreach (SysYcdyTable table in tables)
                {
                    if (!String.IsNullOrEmpty(table.RelateCall.GetSafeString()))
                    {
                        string[] arr = table.RelateCall.Split(new char[] { ',', '，' });
                        if (arr.Length == 2)
                        {
                            string tmpcallid = arr[0], tmpcallversion = arr[1];
                            string tmpid = tmpcallid + "," + tmpcallversion;
                            if (relateObjects.Where(e => e.Id == (tmpid)).Count() == 0)
                            {
                                var tmpurl = YcbaService.GetUrl(tmpcallid, tmpcallversion);
                                if (tmpurl != null)
                                {
                                    SysYcdyApiObject tmpApiObj = new SysYcdyApiObject(
                                        tmpurl,
                                        YcbaService.GetTables(tmpcallid, tmpcallversion),
                                        YcbaService.GetParams(tmpcallid, tmpcallversion),
                                        YcbaService.GetTableRelations(tmpcallid, tmpcallversion),
                                        YcbaService.GetFields(tmpcallid, tmpcallversion),
                                        CommonService, DataInputService, YcbaService, station.RootUrl, tmpid
                                        );
                                    relateObjects.Add(tmpApiObj);
                                }
                            }
                        }
                    }
                }
                relateObjects = relateObjects.Where(e => e.IsValid()).ToList();
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return relateObjects;
        }
        /// <summary>
        /// 表格是否有关联表需要瞎子
        /// </summary>
        /// <param name="table"></param>
        /// <param name="objects"></param>
        /// <returns></returns>

        private bool NeedDownRetions(SysYcdyTable table, IList<SysYcdyApiObject> objects, out SysYcdyApiObject obj)
        {
            bool ret = false;
            obj = null;
            try
            {
                string id = table.RelateCall.GetSafeString();
                if (string.IsNullOrEmpty(id))
                    return ret;
                var q = objects.Where(e => e.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                if (q.Count() > 0)
                {
                    obj = q.First();
                    ret = true;
                }
            }catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        
        /// <summary>
        /// 下载相关记录sql
        /// </summary>
        /// <param name="findTable"></param>
        /// <param name="relateObjects"></param>
        /// <param name="sqls"></param>
        private void AddRelateSql(SysYcdyStation station, SysYcdyTable findTable, SysYcdyTableRelation findRelation, IList<SysYcdyApiObject> relateObjects, 
            IDictionary<string,object>  rowData, ref IList<IDictionary<string, object>> sqls)
        {
            // 判断相关记录下载
            SysYcdyApiObject childObj = null;
            if (NeedDownRetions(findTable, relateObjects, out childObj))
            {
                IDictionary<string, string> callParam = new Dictionary<string, string>();

                SysYcdyTable rootTable;
                SysYcdyTableRelation rootRelation;
                if (!childObj.GetRootTable(out rootTable, out rootRelation))
                    return;
                var tableField = childObj.Fields.Where(e => e.TableName.Equals(rootTable.LocalTableName, StringComparison.OrdinalIgnoreCase));
                string remoteIdField = tableField.Where(e => e.LocalField.Equals(rootRelation.RemotePrimaryKey)).ElementAt(0).RemoteField;
                SysYcdyData tmpdata = new SysYcdyData();
                var key = string.Empty;

                if (rowData.ContainsKey(remoteIdField))
                { 
                    key = rowData[remoteIdField].GetSafeString();
                }
                else
                {
                    //乐清质监站特殊
                    if (rootTable.LocalTableName.ToLower() == "i_m_qy")
                    {
                        key = rowData["qybh"].GetSafeString();
                    }
                    else if (rootTable.LocalTableName.ToLower() == "i_m_ry") 
                    {
                        key = rowData["rybh"].GetSafeString();
                    }
                }

                callParam.Add("key", key);
                SysYcdyData data = new SysYcdyData();
                string tmpRelationMsg = "";
                // if (data.GetData(station.RootUrl + childObj.Url.DownAllUrl, callParam, out tmpRelationMsg))
                if (data.GetData(station.RootUrl + childObj.Url.UrlPath, callParam, out tmpRelationMsg))
                {
                    if (childObj.NeedUpdate(data.OrgDatas, out tmpRelationMsg))
                    {
                        foreach (SysYcdyTable tmpTable in childObj.Tables)
                        {
                            SysYcdyTableRelation tmpRelation = childObj.GetRelation(tmpTable.LocalTableName);
                            var tableDatas = data.Datas.Where(e => e.AreaId.Equals(tmpTable.RemoteTable));
                            if (tableDatas.Count() == 0)
                                continue;
                            var tableData = tableDatas.First().RowOrgData;
                            string sql = "";
                            IList<VSqlParam> sqlParams = null;
                            IList<VDataFileItem> sqlFiles = null;
                            if (childObj.IsUpdate(tableData, tmpTable, tmpRelation, out tmpRelationMsg))
                            {
                                bool isSuccess = childObj.GetUpdateSql(data.OrgDatas, tableData, tmpTable, out sql, out sqlParams, out sqlFiles, out tmpRelationMsg);
                                if (!isSuccess)
                                    continue;
                            }
                            else
                            {
                                bool isSuccess = childObj.GetInsertSql(data.OrgDatas, tableData, tmpTable, out sql, out sqlParams, out sqlFiles, out tmpRelationMsg);
                                if (!isSuccess)
                                    break;
                            }

                            if (!string.IsNullOrEmpty(sql))
                            {
                                IDictionary<string, object> rowSql = new Dictionary<string, object>();
                                rowSql.Add("sql", sql);
                                rowSql.Add("params", sqlParams);
                                rowSql.Add("files", sqlFiles);
                                sqls.Add(rowSql);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}