using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;
using System.Reflection;


namespace BD.Jcbg.Bll
{
    public class FormAPIService:IFormAPIService
    {
        #region 数据库对象
        public ICommonDao CommonDao { get; set; }
        #endregion


        #region 服务
        [Transaction(ReadOnly = false)]
        public bool GetWebListData(HttpRequestBase Request, object controller, out int total, out List<Dictionary<string, object>> data, out string msg)
        {
            bool ret = true;
            msg = "";
            total = 0;
            data = new List<Dictionary<string, object>>();
            try
            {
                string lx = Request.QueryString["lx"].GetSafeString();
                if (lx != "")
                {

                    #region 查询配置信息
                    string sql = string.Format("select top 1 * from formapi where lx='{0}'", lx);
                    IList<IDictionary<string, object>> dt = CommonDao.GetBinaryDataTable(sql);
                    #endregion

                    // 无法获取信息，报错
                    if (dt.Count == 0)
                    {
                        ret = false;
                        msg = "无法获取配置信息！";
                    }
                    else
                    {
                        // 是否需要重新加载数据
                        // 主要用于过滤条件和查参数中有互相矛盾的情况
                        bool needtoload = true;

                        // JSON 序列化和反序列化类
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = int.MaxValue;

                        #region 获取配置信息
                        // 提取配置信息
                        IDictionary<string, object> config = dt[0];
                        // 数据配置
                        string urlDataConfig = config["urldataconfig"].GetSafeString();
                        string pageParamConfig = config["pageparamconfig"].GetSafeString();
                        string forgeFilterParamConfig = config["forgefilterparamconfig"].GetSafeString();
                        string retDataConfig = config["retdataconfig"].GetSafeString();
                        string orderbyConfig = config["orderbyconfig"].GetSafeString();
                        string ignoredParamConfig = config["ignoredparamconfig"].GetSafeString();
                        string requestMethod = config["requestmethod"].GetSafeString();
                        Dictionary<string, object> dtUrlDataConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtPageParamConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtForgeFilterParamConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtRetDataConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtOrderbyConfig = new Dictionary<string, object>();
                        Dictionary<string, object> dtIgnoredParamConfig = new Dictionary<string, object>();

                        if (urlDataConfig != "")
                        {
                            dtUrlDataConfig = jss.Deserialize<Dictionary<string, object>>(urlDataConfig);
                        }
                        if (pageParamConfig != "")
                        {
                            dtPageParamConfig = jss.Deserialize<Dictionary<string, object>>(pageParamConfig);
                        }
                        if (forgeFilterParamConfig != "")
                        {
                            dtForgeFilterParamConfig = jss.Deserialize<Dictionary<string, object>>(forgeFilterParamConfig);
                        }
                        if (retDataConfig != "")
                        {
                            dtRetDataConfig = jss.Deserialize<Dictionary<string, object>>(retDataConfig);
                        }
                        if (orderbyConfig != "")
                        {
                            dtOrderbyConfig = jss.Deserialize<Dictionary<string, object>>(orderbyConfig);
                        }
                        if (ignoredParamConfig != "")
                        {
                            dtIgnoredParamConfig = jss.Deserialize<Dictionary<string, object>>(ignoredParamConfig);
                        }
                        #endregion


                        if (dtUrlDataConfig == null || !dtUrlDataConfig.ContainsKey("url") || dtUrlDataConfig["url"].GetSafeString() == "")
                        {
                            ret = false;
                            msg = "请求地址配置不能为空！";
                        }
                        else
                        {
                            // 参数列表（来源：form、query、自定义）
                            List<KeyValuePair<string, string>> paramlist = new List<KeyValuePair<string, string>>();

                            #region 获取分页参数、过滤条件
                            // 获取分页参数
                            int page = Request.Form["page"].GetSafeInt(1);
                            int pageSize = Request.Form["pageSize"].GetSafeInt(20);
                            // 获取过滤条件
                            string filterRules = Request.Form["filterRules"].GetSafeString();

                            #endregion

                            #region 解析过滤条件
                            List<Dictionary<string, object>> filters = new List<Dictionary<string, object>>();
                            if (filterRules != "")
                            {
                                ArrayList al = jss.Deserialize<ArrayList>(filterRules);
                                if (al.Count > 0)
                                {
                                    foreach (var item in al)
                                    {
                                        filters.Add((Dictionary<string, object>)item);
                                    }

                                }
                            }
                            #endregion

                            #region  根据过滤条件生成参数列表
                            if (filters.Count > 0)
                            {
                                foreach (var f in filters)
                                {
                                    string fieldname = f["fieldname"].GetSafeString();
                                    string fieldvalue = f["fieldvalue"].GetSafeString();
                                    string fieldopt = f["fieldopt"].GetSafeString();
                                    if (fieldname != "" && fieldvalue != "")
                                    {
                                        paramlist.Add(new KeyValuePair<string, string>(fieldname.ToLower(), fieldvalue));

                                    }
                                }
                            }
                            #endregion

                            #region 将query参数合并到参数列表中,并去重
                            // 忽略固定的query参数:lx
                            List<string> keylist = Request.QueryString.AllKeys.Select(x => x.ToLower()).Where(x => x != "lx").ToList();

                            if (keylist.Count > 0)
                            {
                                foreach (var k in keylist)
                                {
                                    var p = paramlist.Where(x => x.Key == k);
                                    if (p.Count() > 0)
                                    {
                                        var fp = p.First();
                                        string fv = fp.Value;
                                        string qv = HttpUtility.UrlDecode(Request.QueryString[k]).Trim().GetSafeString();
                                        if (qv != "" && qv != fv)
                                        {
                                            needtoload = false; // url中的参数和filterRule中的查询条件有矛盾的地方
                                        }

                                    }
                                    else
                                    {
                                        paramlist.Add(new KeyValuePair<string, string>(k, Request.QueryString[k].GetSafeString()));
                                    }

                                }
                            }
                            #endregion

                            #region 获取字段映射配置
                            IList<IDictionary<string, string>> mpc = new List<IDictionary<string, string>>();
                            sql = string.Format("select * from FORMAPICONFIG where sfyx=1 and lx='{0}'", lx);
                            mpc = CommonDao.GetDataTable(sql);
                            #endregion

                            #region 生成排序字段
                            Dictionary<string, string> dtOrderbyInfo = new Dictionary<string, string>();
                            string sort = "";
                            string order = "";
                            sort = Request.Form["sort"].GetSafeString();
                            order = Request.Form["order"].GetSafeString();
                            if (sort != "" && order != "")
                            {
                                var q = mpc.Select(x => new { src = x["sourcefield"].GetSafeString(), dest = x["destfield"].GetSafeString() }).Where(x => x.dest.ToLower() == sort.ToLower());
                                if (q.Count() > 0)
                                {
                                    sort = q.First().src;
                                }
                                dtOrderbyInfo.Add("sort", sort);
                                dtOrderbyInfo.Add("order", order);
                            }
                            #endregion

                            #region 查询数据
                            if (needtoload)
                            {

                                #region 在发送请求之前，修改paramlist
                                if (dtForgeFilterParamConfig != null)
                                {
                                    if (dtForgeFilterParamConfig.ContainsKey("method"))
                                    {
                                        string fm = dtForgeFilterParamConfig["method"].GetSafeString();
                                        if (fm != "")
                                        {
                                            paramlist = (List<KeyValuePair<string, string>>)InvokeMethod(controller, fm, new object[] { paramlist });
                                        }
                                    }
                                }

                                #endregion

                                #region 根据paramlist和配置项生成请求参数字符串

                                string urldata = "";

                                #region 获取配置的分页参数
                                string pageField = "page";
                                string pageSizeField = "pageSize";
                                if (dtPageParamConfig != null)
                                {
                                    if (dtPageParamConfig.ContainsKey("page"))
                                    {
                                        var v = dtPageParamConfig["page"].GetSafeString();
                                        if (v != "")
                                        {
                                            pageField = v;
                                        }
                                    }
                                    if (dtPageParamConfig.ContainsKey("pageSize"))
                                    {
                                        var v = dtPageParamConfig["pageSize"].GetSafeString();
                                        if (v != "")
                                        {
                                            pageSizeField = v;
                                        }
                                    }
                                }
                                urldata += string.Format("{0}={1}&{2}={3}", pageField, page.ToString(), pageSizeField, pageSize.ToString());
                                #endregion

                                #region 获取配置的固定参数
                                if (dtUrlDataConfig.ContainsKey("fixedParams"))
                                {
                                    var fixedParams = dtUrlDataConfig["fixedParams"].GetSafeString();
                                    if (fixedParams != "")
                                    {
                                        urldata += "&" + fixedParams;
                                    }
                                }
                                #endregion

                                #region 获取配置的额外参数
                                if (dtUrlDataConfig.ContainsKey("extraParamsMethod"))
                                {
                                    var extraParamsMethod = dtUrlDataConfig["extraParamsMethod"].GetSafeString();
                                    if (extraParamsMethod != "")
                                    {
                                        Dictionary<string, object> extraParamInfo = (Dictionary<string, object>)InvokeMethod(controller,extraParamsMethod, new object[] { paramlist });
                                        bool issuccess = extraParamInfo["code"].GetSafeBool();
                                        if (!issuccess)
                                        {
                                            throw new Exception(extraParamInfo["msg"].GetSafeString());
                                        }
                                        else
                                        {
                                            string extraParam = extraParamInfo["data"].GetSafeString();
                                            if (extraParam != "")
                                            {
                                                urldata += "&" + extraParam;
                                            }
                                        }

                                    }
                                }
                                #endregion                                

                                #region 获取配置的orderby参数
                                if (dtOrderbyConfig != null && dtOrderbyConfig.Count > 0)
                                {
                                    bool enable = dtOrderbyConfig["enable"].GetSafeBool();
                                    string paramName = dtOrderbyConfig["paramName"].GetSafeString();
                                    string paramFormat = dtOrderbyConfig["paramFormat"].GetSafeString();
                                    // 启用orderby
                                    if (enable)
                                    {
                                        string orderby = "";
                                        string field = "";
                                        string fieldorder = "";
                                        if (dtOrderbyInfo != null && dtOrderbyInfo.Count > 0)
                                        {
                                            field = dtOrderbyInfo["sort"];
                                            fieldorder = dtOrderbyInfo["order"];
                                            orderby = string.Format("{0}={1}", paramName, HttpUtility.UrlEncode(paramFormat.Replace("{sort}", field).Replace("{order}", fieldorder)));
                                        }
                                        else
                                        {
                                            if (dtOrderbyConfig.ContainsKey("defaultParam"))
                                            {
                                                var v = dtOrderbyConfig["defaultParam"].GetSafeString();
                                                if (v != null)
                                                {
                                                    orderby = string.Format("{0}={1}", paramName, HttpUtility.UrlEncode(v));
                                                }
                                            }
                                        }

                                        if (orderby != "")
                                        {
                                            urldata += "&" + orderby;
                                        }
                                    }
                                }
                                #endregion

                                #region 获取paramlist参数
                                // 过滤掉需要忽略的参数
                                if (dtIgnoredParamConfig != null && dtIgnoredParamConfig.Count > 0)
                                {
                                    if (dtIgnoredParamConfig.ContainsKey("ignoredlist"))
                                    {
                                        ArrayList al = (ArrayList)dtIgnoredParamConfig["ignoredlist"];
                                        if (al != null && al.Count > 0)
                                        {
                                            List<string> ipms = al.ToArray().Select(x => x.GetSafeString().ToLower()).ToList();
                                            paramlist = paramlist.Where(x => !ipms.Contains(x.Key.ToLower())).ToList();
                                        }
                                    }
                                }
                                foreach (var item in paramlist)
                                {
                                    urldata += "&" + item.Key + "=" + HttpUtility.UrlEncode(item.Value);
                                }
                                #endregion

                                #endregion

                                #region 发送请求，获取数据
                                // 请求返回的字符串
                                string retstring = "";
                                // 获取配置的第三方请求URL
                                string url = dtUrlDataConfig["url"].GetSafeString();


                                // 默认请求方式为POST
                                if (requestMethod == "")
                                {
                                    requestMethod = "POST";
                                }
                                // POST请求方式
                                if (requestMethod == "POST")
                                {
                                    retstring = MyHttp.SendDataByPost(url, urldata);
                                    SysLog4.WriteError("formapi urldata:\r\n" + urldata);
                                }
                                else
                                {
                                    // GET 请求
                                    url += (url.Contains("?") ? "&" : "?") + urldata;
                                    retstring = MyHttp.SendDataByGET(url);
                                }
                                #endregion

                                #region 处理返回的数据
                                //SysLog4.WriteError(url);
                                Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                                //SysLog4.WriteError(retstring);
                                if (retdata != null)
                                {
                                    #region 获取返回数据的配置项
                                    string codeField = "code";
                                    string msgField = "msg";
                                    string totalField = "total";
                                    string dataField = "rows";
                                    string successMethod = "";
                                    string postMethod = "";
                                    string getDataMethod = "";
                                    if (dtRetDataConfig != null && dtRetDataConfig.Count > 0)
                                    {
                                        if (dtRetDataConfig.ContainsKey("code"))
                                        {
                                            var v = dtRetDataConfig["code"].GetSafeString();
                                            if (v != "")
                                            {
                                                codeField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("msg"))
                                        {
                                            var v = dtRetDataConfig["msg"].GetSafeString();
                                            if (v != "")
                                            {
                                                msgField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("total"))
                                        {
                                            var v = dtRetDataConfig["total"].GetSafeString();
                                            if (v != "")
                                            {
                                                totalField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("rows"))
                                        {
                                            var v = dtRetDataConfig["rows"].GetSafeString();
                                            if (v != "")
                                            {
                                                dataField = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("successMethod"))
                                        {
                                            var v = dtRetDataConfig["successMethod"].GetSafeString();
                                            if (v != "")
                                            {
                                                successMethod = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("postMethod"))
                                        {
                                            var v = dtRetDataConfig["postMethod"].GetSafeString();
                                            if (v != "")
                                            {
                                                postMethod = v;
                                            }
                                        }
                                        if (dtRetDataConfig.ContainsKey("getDataMethod"))
                                        {
                                            var v = dtRetDataConfig["getDataMethod"].GetSafeString();
                                            if (v != "")
                                            {
                                                getDataMethod = v;
                                            }
                                        }
                                    }
                                    #endregion

                                    object code = retdata[codeField];
                                    string retmsg = retdata[msgField].GetSafeString();
                                    bool issucess = false;
                                    if (successMethod != "")
                                    {
                                        issucess = (bool)InvokeMethod(controller,successMethod, new object[] { code });
                                    }
                                    else
                                    {
                                        msg = "必须设置successMethod！";
                                    }

                                    if (issucess)
                                    {
                                        ArrayList arr = new ArrayList();
                                        // 配置了自定义获取数据的方法
                                        if (getDataMethod != "")
                                        {
                                            //SysLog4.WriteError(getDataMethod);
                                            Dictionary<string, object> rdata = (Dictionary<string, object>)InvokeMethod(controller, getDataMethod, new object[] { retdata });
                                            if (rdata != null && rdata.Count > 0)
                                            {
                                                total = rdata["total"].GetSafeInt();
                                                arr = (ArrayList)rdata["rows"];
                                            }
                                            //SysLog4.WriteError("lllll");
                                        }
                                        // 没有配置自定义获取数据的方法
                                        else
                                        {
                                            // 数据集总数
                                            total = retdata[totalField].GetSafeInt();
                                            // 包装数据
                                            arr = (ArrayList)retdata[dataField];
                                        }
                                        
                                        if (arr != null && arr.Count > 0)
                                        {
                                            // 将JSON数据包 转成list对象
                                            List<Dictionary<string, object>> tmp = new List<Dictionary<string, object>>();
                                            foreach (var item in arr)
                                            {
                                                Dictionary<string, object> r = (Dictionary<string, object>)item;
                                                tmp.Add(r);
                                            }
                                            //数据映射
                                            var mp = mpc.Select(x => new { src = x["sourcefield"].GetSafeString(), dest = x["destfield"].GetSafeString() }).ToList();
                                            // 遍历每一行数据
                                            foreach (var item in tmp)
                                            {
                                                Dictionary<string, object> t = new Dictionary<string, object>();
                                                //遍历每一个字段
                                                foreach (var f in item)
                                                {
                                                    var q = mp.Where(x => x.src.Equals(f.Key, StringComparison.OrdinalIgnoreCase));
                                                    // 字段需要映射
                                                    if (q.Count() > 0)
                                                    {
                                                        t.Add(q.First().dest.ToLower(), f.Value);
                                                    }
                                                    else
                                                    {
                                                        // 不需要映射
                                                        t.Add(f.Key.ToLower(), f.Value);
                                                    }
                                                }
                                                data.Add(t);
                                            }
                                        }
                                        // 后处理
                                        if (postMethod != "")
                                        {
                                            //SysLog4.WriteError("postMethod:" + postMethod);
                                            data = (List<Dictionary<string, object>>)InvokeMethod(controller,postMethod, new object[] { data });

                                        }
                                    }
                                    else
                                    {
                                        ret = false;
                                        msg = retmsg;
                                    }
                                }
                                else
                                {
                                    ret = false;
                                    msg = "没有返回数据！";
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    ret = false;
                    msg = "参数不全！";
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            return ret;
        }


        #endregion

        #region 帮助函数
        private object InvokeMethod(object obj, string method, object[] parameters = null)
        {
            object ret = null;
            Type type = obj.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var m = type.GetMethod(method, flags);
            if (m != null)
            {
                ret = m.Invoke(obj, parameters);
            }
            return ret;
        }

        private object GetFieldValue(Dictionary<string, object>data, List<string> path)
        {
            object obj = data;
            if (path.Count > 0)
            {
                while (obj != null && path.Count > 0)
                {
                    if (data.ContainsKey(path[0]))
                    {
                        obj = data[path[0]];
                        if (obj != null)
                        {
                            path.RemoveAt(0);
                            obj = GetFieldValue((Dictionary <string,object >)obj, path);
                        }
                        
                    }
                }

            }
            return obj;
        }
        #endregion
    }
}
