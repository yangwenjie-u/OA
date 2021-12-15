using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.IBll;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using System.Threading;
using System.IO;
using ReportPrint.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BD.Jcbg.Web.Func;
namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 保存扬尘设备阈值信息
    /// </summary>
    public class JobSaveDustyElimit : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
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
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobSaveDustyElimit");
            while (true)
            {
                try
                {
                    string msg = "";
                    string sql = "select * from i_s_gc_dusty_elimit where (delimitid is null or delimitid='') or (lastupdatetime is null )";
                    IList<IDictionary<string, object>> sensorlist = CommonService.GetDataTable2(sql);
                    if (sensorlist.Count > 0)
                    {
                        sql = "select lx, url from h_dust_apiconfig where lx in ('SaveDeLimit','UpdateDeLimit') and sfyx=1 ";
                        IList<IDictionary<string, string>> urlConfigs = CommonService.GetDataTable(sql);
                        string saveUrl = "";
                        string updateUrl = "";
                        var q = urlConfigs.Where(x => x["lx"] == "SaveDeLimit");
                        if (q.Count() > 0)
                        {
                            saveUrl = q.First()["url"];
                        }
                        q = urlConfigs.Where(x => x["lx"] == "UpdateDeLimit");
                        if (q.Count() > 0)
                        {
                            updateUrl = q.First()["url"];
                        }
                        if (saveUrl != "" && updateUrl != "")
                        {
                            // JSON 序列化和反序列化类
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            jss.MaxJsonLength = int.MaxValue;
                            string url = "";
                            string postdata = "";
                            string retmsg = "";
                            Dictionary<string, object> retdata = null;
                            int code = -9999;

                            foreach (var sensor in sensorlist)
                            {
                                string delimitid = sensor["delimitid"].GetSafeString();                                

                                // 需要提交的数据
                                Dictionary<string, object> data = new Dictionary<string, object>() {
                                        {"DeviceCode", sensor["devicecode"] },
                                        {"SensorCode", sensor["sensorcode"] },
                                        {"StartTime", Dust.getUnixTimestamp(sensor["starttime"].GetSafeDate()) },
                                        {"EndTime", Dust.getUnixTimestamp(sensor["endtime"].GetSafeDate()) },
                                        {"MinLimit", sensor["minlimit"] },
                                        {"MaxLimit", sensor["maxlimit"] },
                                        {"OrderBy", sensor["orderby"] }
                                    };
                                // 未绑定过
                                if (delimitid == "")
                                {
                                    SysLog4.WriteError("保存数据");
                                    //先调用保存接口
                                    url = saveUrl;
                                    postdata = "Token=" + HttpUtility.UrlEncode(Dust.DustGetToken()) +
                                        "&Datas=" + HttpUtility.UrlEncode(jss.Serialize(data));
                                    retmsg = MyHttp.SendDataByPost(url, postdata);
                                    SysLog4.WriteError(retmsg);

                                    if (retmsg != "")
                                    {
                                        retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                        if (retdata != null)
                                        {
                                            code = retdata["Code"].GetSafeInt();
                                            // 保存成功,更新设备表
                                            if (code == 0)
                                            {
                                                delimitid = retdata["Datas"].GetSafeString();
                                                sql = string.Format("update i_s_gc_dusty_elimit set delimitid='{0}', lastupdatetime=getdate() where recid={1}", delimitid, sensor["recid"].GetSafeString());
                                                CommonService.Execsql(sql);
                                            }
                                            // 当前传感器阈值在物联网平台中已登记，需要调用更新接口
                                            else if (code == 1)
                                            {
                                                delimitid = retdata["Datas"].GetSafeString();
                                                data.Add("Id", delimitid);
                                                url = updateUrl;
                                                postdata = "Token=" + HttpUtility.UrlEncode(Dust.DustGetToken()) +
                                                        "&Datas=" + HttpUtility.UrlEncode(jss.Serialize(data));
                                                retmsg = MyHttp.SendDataByPost(url, postdata);
                                                retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                                if (retdata != null)
                                                {
                                                    code = retdata["Code"].GetSafeInt();
                                                    // 更新成功
                                                    if (code == 0)
                                                    {
                                                        sql = string.Format("update i_s_gc_dusty_elimit set lastupdatetime=getdate() where delimitid='{0}' and recid={1}", delimitid,  sensor["recid"].GetSafeString());
                                                        CommonService.Execsql(sql);

                                                    }
                                                    // 更新失败，记录错误
                                                    else
                                                    {
                                                        SysLog4.WriteError(retmsg);
                                                    }
                                                }
                                                else
                                                {
                                                    SysLog4.WriteError(retmsg);
                                                }

                                            }
                                            else
                                            {
                                                SysLog4.WriteError(retmsg);
                                            }
                                        }
                                    }
                                }
                                // 已绑定过，但是未成功
                                else if (delimitid != "" )
                                {
                                    SysLog4.WriteError("更新数据");
                                    SysLog4.WriteError("id: " + delimitid);
                                    SysLog4.WriteError("data: \r\n" + jss.Serialize(data));
                                    data.Add("Id", delimitid);
                                    url = updateUrl;
                                    postdata = "Token=" + HttpUtility.UrlEncode(Dust.DustGetToken()) +
                                            "&Datas=" + HttpUtility.UrlEncode(jss.Serialize(data));
                                    retmsg = MyHttp.SendDataByPost(url, postdata);
                                    retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                    if (retdata != null)
                                    {
                                        code = retdata["Code"].GetSafeInt();
                                        // 更新成功
                                        if (code == 0)
                                        {
                                            sql = string.Format("update i_s_gc_dusty_elimit set lastupdatetime=getdate() where delimitid='{0}' and recid={1}", delimitid, sensor["recid"].GetSafeString());
                                            CommonService.Execsql(sql);

                                        }
                                        // 更新失败，记录错误
                                        else
                                        {
                                            SysLog4.WriteError(retmsg);
                                        }
                                    }
                                    else
                                    {
                                        SysLog4.WriteError(retmsg);
                                    }
                                }

                            }
                        }
                        else
                        {
                            SysLog4.WriteError("配置的url为空！");
                        }
                    }

                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }

        }
    }
}