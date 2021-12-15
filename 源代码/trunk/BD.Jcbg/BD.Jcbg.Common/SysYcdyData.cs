using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Common
{
    public class SysYcdyData
    {
        private const int TYPE_JSONOBJECT = 1;
        private const int TYPE_ARRAY = 2;
        private const int TYPE_COMPLICATED = 3;
        private const int TYPE_OTHER = 0;
        public int TotalCount { get; set; }
        public IList<SysYcdyDataItem> Datas { get; set; }
        public IDictionary<string, object> OrgDatas { get; set; }
        public object[] OrgArray { get; set; }
        public SysYcdyData()
        {
            Datas = new List<SysYcdyDataItem>();
            OrgDatas = new Dictionary<string, object>();
            OrgArray = null;
        }

        public bool GetData(string url, IDictionary<string, string> callparams, out string msg)
        {
            Datas = new List<SysYcdyDataItem>();
            OrgDatas = new Dictionary<string, object>();
            OrgArray = null;
            string data = "";
            bool code = MyHttp.Post(url, callparams, out data);
            if (!code)
                msg = data;
            else
            {
                code = ParseData(data, out msg);
            }
            return code;

        }
        public bool ParseData(string datas, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                Datas.Clear();
                TotalCount = 0;

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                IDictionary<string, object> rootData = jsonSerializer.Deserialize<IDictionary<string, object>>(datas);
                object tmpvalue = null;
                if (rootData.TryGetValue("total", out tmpvalue))
                    TotalCount = Convert.ToInt32(tmpvalue);
                if (rootData.TryGetValue("rows", out tmpvalue))
                {
                    // 把树状的内容转换成二维表格
                    object[] rows = tmpvalue as object[];
                    OrgArray = rows;
                    OrgDatas = tmpvalue as IDictionary<string, object>;
                    if (OrgDatas == null)
                    {
                        var tmpArr = tmpvalue as object[];
                        if (tmpArr.Length > 0)
                            OrgDatas = tmpArr[0] as IDictionary<string, object>;
                    }
                    IList<SysYcdyUnformatDataItem> buffer = new List<SysYcdyUnformatDataItem>();
                    foreach (object obj in rows)
                        buffer.Add(new SysYcdyUnformatDataItem() { Id = Guid.NewGuid().ToString(), DataItem = obj });

                    while (buffer.Count() > 0)
                    {

                        SysYcdyUnformatDataItem rowItem = buffer[0];
                        buffer.RemoveAt(0);
                        int objType = GetObjectType(rowItem.DataItem);
                        SysYcdyDataItem dataItem = new SysYcdyDataItem();
                        dataItem.ParentId = rowItem.ParentId;
                        dataItem.Id = rowItem.Id;
                        dataItem.AreaId = rowItem.AreaId;
                        dataItem.ParentAreaId = rowItem.ParentAreaId;
                        Datas.Add(dataItem);
                        if (objType == TYPE_JSONOBJECT)
                        {
                            dataItem.RowData = GetDictionaryString(rowItem.DataItem as Dictionary<string, object>);
                        }
                        else if (objType == TYPE_ARRAY)
                        {
                            object[] tmpObjs = rowItem.DataItem as object[];
                            foreach (object obj in tmpObjs)
                                buffer.Add(new SysYcdyUnformatDataItem() { AreaId = "", ParentAreaId = dataItem.AreaId, Id = Guid.NewGuid().ToString(), ParentId = dataItem.Id, DataItem = obj });
                        }
                        else if (objType == TYPE_COMPLICATED)
                        {
                            IDictionary<string, object> tmpDatas = rowItem.DataItem as Dictionary<string, object>;
                            foreach (string key in tmpDatas.Keys)
                                buffer.Add(new SysYcdyUnformatDataItem() { AreaId = key, ParentAreaId = dataItem.AreaId, Id = Guid.NewGuid().ToString(), ParentId = dataItem.Id, DataItem = tmpDatas[key] });
                        }
                    }
                }
                // 把二维表格areaid加上所有父级前缀，去掉中间没有数据环境，并更新对应id和parentid
                var leafNodes = Datas.Where(e => e.RowData != null);
                foreach (var leafNode in leafNodes)
                {
                    if (leafNode.AreaId != "" && leafNode.ParentAreaId != "")
                        leafNode.AreaId = leafNode.ParentAreaId + "." + leafNode.AreaId;
                    else if (leafNode.ParentAreaId != "")
                        leafNode.AreaId = leafNode.ParentAreaId;
                    string curParent = leafNode.ParentId;
                    while (true)
                    {
                        var parents = Datas.Where(e => e.Id == curParent);
                        if (parents.Count() == 0)
                            break;
                        var parent = parents.ElementAt(0);
                        if (parent.RowData == null && parent.ParentId != "" && parent.AreaId == "")
                            leafNode.ParentId = parent.ParentId;
                        curParent = parent.ParentId;
                        if (parent.ParentAreaId != "" && leafNode.AreaId != "")
                            leafNode.AreaId = parent.ParentAreaId + "." + leafNode.AreaId;
                        else if (parent.ParentAreaId != "")
                            leafNode.AreaId = parent.ParentAreaId;
                    }
                }
                //Datas = Datas.Where(e => e.RowData != null && e.AreaId!="").ToList();
                code = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return code;
        }
        public bool ParseRowData(IDictionary<string, object> rootData, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                Datas.Clear();
                TotalCount = 1;
                // 把树状的内容转换成二维表格
                IList<SysYcdyUnformatDataItem> buffer = new List<SysYcdyUnformatDataItem>();

                buffer.Add(new SysYcdyUnformatDataItem() { Id = Guid.NewGuid().ToString(), DataItem = rootData });

                while (buffer.Count() > 0)
                {

                    SysYcdyUnformatDataItem rowItem = buffer[0];
                    buffer.RemoveAt(0);
                    int objType = GetObjectType(rowItem.DataItem);
                    SysYcdyDataItem dataItem = new SysYcdyDataItem();
                    dataItem.ParentId = rowItem.ParentId;
                    dataItem.Id = rowItem.Id;
                    dataItem.AreaId = rowItem.AreaId;
                    dataItem.ParentAreaId = rowItem.ParentAreaId;
                    Datas.Add(dataItem);
                    if (objType == TYPE_JSONOBJECT)
                    {
                        dataItem.RowData = GetDictionaryString(rowItem.DataItem as Dictionary<string, object>);
                    }
                    else if (objType == TYPE_ARRAY)
                    {
                        object[] tmpObjs = rowItem.DataItem as object[];
                        foreach (object obj in tmpObjs)
                            buffer.Add(new SysYcdyUnformatDataItem() { AreaId = "", ParentAreaId = dataItem.AreaId, Id = Guid.NewGuid().ToString(), ParentId = dataItem.Id, DataItem = obj });
                    }
                    else if (objType == TYPE_COMPLICATED)
                    {
                        IDictionary<string, object> tmpDatas = rowItem.DataItem as Dictionary<string, object>;
                        foreach (string key in tmpDatas.Keys)
                            buffer.Add(new SysYcdyUnformatDataItem() { AreaId = key, ParentAreaId = dataItem.AreaId, Id = Guid.NewGuid().ToString(), ParentId = dataItem.Id, DataItem = tmpDatas[key] });
                    }
                }

                // 把二维表格areaid加上所有父级前缀，去掉中间没有数据环境，并更新对应id和parentid
                var leafNodes = Datas.Where(e => e.RowData != null);
                foreach (var leafNode in leafNodes)
                {
                    if (leafNode.AreaId != "" && leafNode.ParentAreaId != "")
                        leafNode.AreaId = leafNode.ParentAreaId + "." + leafNode.AreaId;
                    else if (leafNode.ParentAreaId != "")
                        leafNode.AreaId = leafNode.ParentAreaId;
                    string curParent = leafNode.ParentId;
                    while (true)
                    {
                        var parents = Datas.Where(e => e.Id == curParent);
                        if (parents.Count() == 0)
                            break;
                        var parent = parents.ElementAt(0);
                        if (parent.RowData == null && parent.ParentId != "" && parent.AreaId == "")
                            leafNode.ParentId = parent.ParentId;
                        curParent = parent.ParentId;
                        if (parent.ParentAreaId != "" && leafNode.AreaId != "")
                            leafNode.AreaId = parent.ParentAreaId + "." + leafNode.AreaId;
                        else if (parent.ParentAreaId != "")
                            leafNode.AreaId = parent.ParentAreaId;
                    }
                }
                //Datas = Datas.Where(e => e.RowData != null && e.AreaId!="").ToList();
                code = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return code;
        }
        public bool ParseRowData(string data, out string msg)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            IDictionary<string, object> rootData = jsonSerializer.Deserialize<IDictionary<string, object>>(data);
            return ParseRowData(rootData, out msg);
        }
        /// <summary>
        /// 获取对象类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int GetObjectType(object obj)
        {
            int ret = TYPE_OTHER;
            var objType = obj.GetType();
            if (objType == typeof(Dictionary<string, object>))
            {
                ret = TYPE_JSONOBJECT;
                Dictionary<string, object> formatObj = obj as Dictionary<string, object>;
                foreach (string key in formatObj.Keys)
                {
                    if (formatObj[key].GetType() != typeof(string))
                    {
                        ret = TYPE_COMPLICATED;
                        break;
                    }
                }
            }
            else if (objType == typeof(object[]))
            {
                ret = TYPE_ARRAY;
            }
            return ret;
        }

        private IDictionary<string, string> GetDictionaryString(IDictionary<string, object> orgs)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            foreach (string key in orgs.Keys)
            {
                ret.Add(key, orgs[key] as string);
            }
            return ret;
        }

    }



    public class SysYcdyDataItem
    {
        public string AreaId { get; set; }
        public string ParentAreaId { get; set; }
        public string ParentId { get; set; }
        public string Id { get; set; }
        public IDictionary<string, string> RowData { get; set; }

        public IDictionary<string, object> RowOrgData { get
            {
                if (RowData == null)
                    return null;
                IDictionary<string, object> ret = new Dictionary<string, object>();
                foreach (string key in RowData.Keys)
                    ret.Add(key, RowData[key]);
                return ret;
            } }

        public SysYcdyDataItem()
        {
            AreaId = "";
            ParentAreaId = "";
            ParentId = "";
            Id = "";
            RowData = null;
        }
    }

    public class SysYcdyUnformatDataItem
    {
        public string AreaId { get; set; }
        public string ParentAreaId { get; set; }

        public string ParentId { get; set; }
        public string Id { get; set; }
        public object DataItem { get; set; }

        public SysYcdyUnformatDataItem()
        {
            AreaId = "";
            ParentAreaId = "";
            ParentId = "";
            Id = "";
            DataItem = null;
        }
    }
}
