using System;
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
using System.Collections.Generic;
using System.Collections;

namespace BD.Jcbg.Bll
{
    public class JcjgBgService: IJcjgBgService
    {
        #region 数据库对象
        public ICommonDao CommonDao { get; set; }
        #endregion

        #region 服务
        [Transaction(ReadOnly =false)]
        /// <summary>
        /// 获取报告数量
        /// </summary>
        /// <param name="zjdjh">质监登记号</param>
        /// <param name="jcjg">检测结果</param>
        /// <param name="ptbh">平台编号</param>
        /// <returns></returns>
        public int GetBgsl(string zjdjh, string jcjg, string ptbh, string lszjdjh="")
        {
            int count = 0;
            try
            {
                // JSON 序列化和反序列化类
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string sql = string.Format("select * from h_jcbg_config where ptbh='{0}'", ptbh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string url = dt[0]["url"].GetSafeString();
                    string postdata = dt[0]["postdata"].GetSafeString();
                    Dictionary<string, string> info = new Dictionary<string, string>()
                    {
                        { "zjdjh", zjdjh.GetSafeString()},
                        { "lszjdjh", lszjdjh.GetSafeString()},
                        { "jcjg", jcjg.GetSafeString()},
                        { "page", "1"},
                        { "pagesize", "1"}
                    };
                    foreach (var item in info)
                    {
                        postdata = postdata.Replace("{" + item.Key + "}", item.Value);
                    }
                    string retstring = MyHttp.SendDataByPost(url, postdata);
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                    if (retdata != null)
                    {
                        string code = retdata["code"].GetSafeString();
                        string retmsg = retdata["msg"].GetSafeString();
                        int totalcount = retdata["totalcount"].GetSafeInt();
                        if (code == "true")
                        {
                            count = totalcount;
                        }

                    }
                }
                
            }
            catch (Exception e)
            {
                SysLog4.WriteError(e.Message);
            }


            return count;
        }
        /// <summary>
        /// 获取不合格报告记录
        /// </summary>
        /// <param name="ptbh"></param>
        /// <param name="param"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly =false)]
        public bool GetBHGBG(string ptbh, Dictionary<string, string> param, out List<Dictionary<string, object>> data, out int total, out string msg)
        {
            bool ret = true;
            msg = "";
            data = new List<Dictionary<string, object>>();
            total = 0;
            try
            {
                // JSON 序列化和反序列化类
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string sql = string.Format("select * from h_jcbg_config where ptbh='{0}' and lx='JCBG'", ptbh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string url = dt[0]["url"].GetSafeString();
                    string postdata = dt[0]["postdata"].GetSafeString();
                    foreach (var item in param)
                    {
                        postdata = postdata.Replace("{" + item.Key + "}", item.Value);
                    }
                    string retstring = MyHttp.SendDataByPost(url, postdata);
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                    if (retdata != null)
                    {
                        string code = retdata["code"].GetSafeString();
                        string retmsg = retdata["msg"].GetSafeString();
                        int totalcount = retdata["totalcount"].GetSafeInt();
                        if (code == "true")
                        {
                            total = totalcount;
                            ArrayList arr = (ArrayList)retdata["records"];
                            if (arr.Count > 0)
                            {
                                // 将JSON数据包 转成list对象                                    
                                foreach (var item in arr)
                                {
                                    Dictionary<string, object> r = (Dictionary<string, object>)item;
                                    data.Add(r);
                                }
                            }
                        }
                        else
                        {
                            ret = false;
                            msg = retmsg;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据委托单唯一号获取检测报告列表
        /// </summary>
        /// <param name="ptbh"></param>
        /// <param name="param"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetBGList(string ptbh, Dictionary<string, string> param, out List<Dictionary<string, object>> data, out string msg)
        {
            bool ret = true;
            msg = "";
            data = new List<Dictionary<string, object>>();
            try
            {
                // JSON 序列化和反序列化类
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = int.MaxValue;
                string sql = string.Format("select * from h_jcbg_config where ptbh='{0}' and lx='BGLIST'", ptbh);
                IList<IDictionary<string, string>> dt = CommonDao.GetDataTable(sql);
                if (dt.Count > 0)
                {
                    string url = dt[0]["url"].GetSafeString();
                    string postdata = dt[0]["postdata"].GetSafeString();
                    foreach (var item in param)
                    {
                        postdata = postdata.Replace("{" + item.Key + "}", item.Value);
                    }
                    string retstring = MyHttp.SendDataByPost(url, postdata);
                    Dictionary<string, object> retdata = jss.Deserialize<Dictionary<string, object>>(retstring);
                    if (retdata != null)
                    {
                        string code = retdata["code"].GetSafeString();
                        string retmsg = retdata["msg"].GetSafeString();
                        if (code == "true")
                        {
                            ArrayList arr = (ArrayList)retdata["records"];
                            if (arr.Count > 0)
                            {
                                // 将JSON数据包 转成list对象                                    
                                foreach (var item in arr)
                                {
                                    Dictionary<string, object> r = (Dictionary<string, object>)item;
                                    data.Add(r);
                                }
                            }
                        }
                        else
                        {
                            ret = false;
                            msg = retmsg;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            return ret;
        }
        #endregion

    }
}
