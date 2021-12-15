using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using System.Text.RegularExpressions;
using Spring.Transaction.Interceptor;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;


namespace BD.Jcbg.Web.Controllers
{
    /// <summary>
    /// 用户各种帮助文档页面
    /// </summary>
    public class WebApiController : Controller
    {
        #region 服务
        private BD.Jcbg.IBll.ICommonService _commonService = null;
        private BD.Jcbg.IBll.ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as BD.Jcbg.IBll.ICommonService;
                }
                return _commonService;
            }
        }

        private IJdbgService _jdbgService = null;
        private IJdbgService JdbgService
        {
            get
            {
                if (_jdbgService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _jdbgService = webApplicationContext.GetObject("JdbgService") as IJdbgService;
                }
                return _jdbgService;
            }
        }

        private IWorkFlowService _workflowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workflowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workflowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workflowService;
            }
        }

        private IRemoteUserService _remoteUserService = null;
        private IRemoteUserService RemoteUserService
        {
            get
            {
                if (_remoteUserService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _remoteUserService = webApplicationContext.GetObject("RemoteUserService") as IRemoteUserService;
                }
                return _remoteUserService;
            }
        }

        private ISmsServiceWzzjz _smsServiceWzzjz = null;
        private ISmsServiceWzzjz SmsServiceWzzjz
        {
            get
            {
                if (_smsServiceWzzjz == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsServiceWzzjz = webApplicationContext.GetObject("SmsServiceWzzjz") as ISmsServiceWzzjz;
                }
                return _smsServiceWzzjz;
            }
        }
        #endregion

        #region 获取数据

        #region 获取CtrlString数据
        public void GetCtrlStringData()
        {
            bool success = true;
            string msg = "";
            List<object> datarows = new List<object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                string type = Request.Form["type"].GetSafeString();
                string jsondata = Request.Form["data"].GetSafeString();
                if (type == "" || jsondata == "")
                {
                    success = false;
                    msg = "非法参数！";
                }
                else
                {
                    string sql = string.Format("select * from h_webapi_ctrlstring where  type='{0}' ", type);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0) // ctrlstring存在
                    {
                        bool islocal = dt[0]["islocal"].GetSafeBool();
                        if (islocal) // 本地应用的ctrlstring
                        {
                            Dictionary<string, object> param = jss.Deserialize<Dictionary<string, object>>(jsondata);
                            // 解析参数
                            string table = "";
                            string fieldname = "";
                            string distinct = "";
                            string order = "";
                            string customwhere = "";
                            string wherefield = "";
                            string wherectrl = "";
                            if (param.ContainsKey("table"))
                            {
                                table = param["table"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("fieldname"))
                            {
                                fieldname = param["fieldname"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("distinct"))
                            {
                                distinct = param["distinct"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("order"))
                            {
                                order = param["order"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("customwhere"))
                            {
                                customwhere = param["customwhere"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("wherefield"))
                            {
                                wherefield = param["wherefield"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("wherectrl"))
                            {
                                wherectrl = param["wherectrl"].GetSafeString().Trim();
                            }

                            string[] fields = fieldname.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);

                            if (table == "" || fields.Length == 0)
                            {
                                success = false;
                                msg = "表名或者字段名不能为空！";
                            }
                            else
                            {
                                // 拼接SQL
                                StringBuilder sb = new StringBuilder();
                                sb.Append("select ");
                                if (distinct != "" && distinct == "1")
                                {
                                    sb.Append("distinct ");
                                }
                                sb.Append(string.Join(",", fields) + " ");
                                sb.Append(string.Format(" from {0} ", table));
                                sb.Append(" where 1=1 ");

                                if (customwhere != "")
                                {
                                    sb.Append(string.Format(" and {0} ", customwhere));
                                }
                                if (wherefield != "")
                                {
                                    string[] wherefieldList = wherefield.Split(',');
                                    string[] wherectrlList = wherectrl.Split(',');
                                    for (int i = 0; i < wherefieldList.Length; i++)
                                    {
                                        sb.AppendFormat(" and {0} = '{1}' ", wherefieldList[i], wherectrlList[i]);
                                    }
                                }

                                if (order != "")
                                {
                                    sb.Append(string.Format(" order by {0} ", order));
                                }
                                sql = sb.ToString();
                                IList<IDictionary<string, string>> dd = CommonService.GetDataTable(sql);
                                // 生成返回数据
                                if (dd.Count > 0)
                                {
                                    foreach (var row in dd)
                                    {
                                        List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
                                        foreach (var key in row.Keys)
                                        {
                                            string origKey = fields.Where(f => f.ToLower() == key.ToLower()).FirstOrDefault().GetSafeString();
                                            if (origKey !="")
                                            {
                                                Dictionary<string, string> info = new Dictionary<string, string>();
                                                info.Add("fieldname", origKey);
                                                info.Add("fieldvalue", row[key]);
                                                l.Add(info);
                                            } 
                                        }
                                        datarows.Add(l);
                                    }
                                }
                            }
                        }
                        else // 第三方应用的ctrlstring
                        {

                        }
                    }
                    else // ctrlstring不存在
                    {
                        success = false;
                        msg = "ctrlstring不存在！";
                    }

                }

            }
            catch (Exception e)
            {
                success = false;
                msg = e.Message;
                SysLog4.WriteLog(e);

            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("total", datarows.Count);
                data.Add("rows", datarows);
                Response.Write(string.Format("{{\"success\":{0},\"msg\":\"{1}\", \"data\": {2}}}", success.ToString().ToLower(), msg, jss.Serialize(data)));
            }


        }
        #endregion

        #region 获取HelpLink数据
        public void GetHelpLinkData()
        {
            bool success = true;
            string msg = "";
            List<object> datarows = new List<object>();
            int total = 0;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            try
            {
                string type = Request.Form["type"].GetSafeString();
                string jsondata = Request.Form["data"].GetSafeString();
                if (type == "" || jsondata == "")
                {
                    success = false;
                    msg = "非法参数！";
                }
                else
                {
                    string sql = string.Format("select * from h_webapi_helplink where  type='{0}' ", type);
                    IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                    if (dt.Count > 0) // helplink存在
                    {
                        bool islocal = dt[0]["islocal"].GetSafeBool();
                        if (islocal) // 本地应用的helplink
                        {
                            Dictionary<string, object> param = jss.Deserialize<Dictionary<string, object>>(jsondata);
                            // 解析参数
                            string table = "";
                            string fieldname = "";
                            string distinct = "";
                            string order = "";
                            string customwhere = "";
                            string wherefield = "";
                            string wherectrl = "";
                            string col = "";
                            string val = "";
                            int page = 1;
                            int rows = 10;
                            if (param.ContainsKey("table"))
                            {
                                table = param["table"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("fieldname"))
                            {
                                fieldname = param["fieldname"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("distinct"))
                            {
                                distinct = param["distinct"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("order"))
                            {
                                order = param["order"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("customwhere"))
                            {
                                customwhere = param["customwhere"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("wherefield"))
                            {
                                wherefield = param["wherefield"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("wherectrl"))
                            {
                                wherectrl = param["wherectrl"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("col"))
                            {
                                col = param["col"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("val"))
                            {
                                val = param["val"].GetSafeString().Trim();
                            }
                            if (param.ContainsKey("page"))
                            {
                                page = param["page"].GetSafeInt();
                            }
                            if (param.ContainsKey("rows"))
                            {
                                rows = param["rows"].GetSafeInt();
                            }

                            string[] fields = fieldname.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);

                            if (table == "" || fields.Length == 0)
                            {
                                success = false;
                                msg = "表名或者字段名不能为空！";
                            }
                            else
                            {
                                // 拼接SQL
                                StringBuilder sb = new StringBuilder();
                                sb.Append("select ");
                                if (distinct != "" && distinct == "1")
                                {
                                    sb.Append("distinct ");
                                }
                                sb.Append(string.Join(",", fields) + " ");
                                sb.Append(string.Format(" from {0} ", table));
                                sb.Append(" where 1=1 ");

                                if (customwhere != "")
                                {
                                    sb.Append(string.Format(" and {0} ", customwhere));
                                }
                                if (wherefield != "")
                                {
                                    string[] wherefieldList = wherefield.Split(',');
                                    string[] wherectrlList = wherectrl.Split(',');
                                    for (int i = 0; i < wherefieldList.Length; i++)
                                    {
                                        sb.AppendFormat(" and {0} = '{1}' ", wherefieldList[i], wherectrlList[i]);
                                    }
                                }

                                if (val != "")
                                {
                                    if (col != "")
                                    {
                                        sb.AppendFormat(" and {0} like '%{1}%' ", col, val);
                                    }
                                    else
                                    {
                                        sb.Append(" and ( ");
                                        sb.Append(string.Join("or", fields.Select(x => string.Format(" {0} like '%{1}%' ", x, val))));

                                        sb.Append(" ) ");
                                    }
                                }

                                if (order != "")
                                {
                                    sb.Append(string.Format(" order by {0} ", order));
                                }
                                sql = sb.ToString();
                                IList<IDictionary<string, string>> dd = CommonService.GetPageData(sql, rows, page, out total);
                                // 生成返回数据
                                if (dd.Count > 0)
                                {
                                    foreach (var row in dd)
                                    {
                                        List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();
                                        foreach (var key in row.Keys)
                                        {
                                            string origKey = fields.Where(f => f.ToLower() == key.ToLower()).FirstOrDefault().GetSafeString();
                                            if (origKey !="")
                                            {
                                                Dictionary<string, string> info = new Dictionary<string, string>();
                                                info.Add("fieldname", origKey);
                                                info.Add("fieldvalue", row[key]);
                                                l.Add(info);
                                            }   
                                        }
                                        datarows.Add(l);
                                    }
                                }
                            }
                        }
                        else // 第三方应用的helplink
                        {

                        }
                    }
                    else // helplink不存在
                    {
                        success = false;
                        msg = "helplink不存在！";
                    }

                }

            }
            catch (Exception e)
            {
                success = false;
                msg = e.Message;
                SysLog4.WriteLog(e);

            }
            finally
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("total", total);
                data.Add("rows", datarows);
                Response.Write(string.Format("{{\"success\":{0},\"msg\":\"{1}\", \"data\": {2}}}", success.ToString().ToLower(), msg, jss.Serialize(data)));
            }


        }
        #endregion


        #endregion
    }


}