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
    /// 诸暨质监站用
    /// 向物联网平台推送扬尘设备数据
    /// </summary>
    public class JobSaveDustDevice : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobSaveDustDevice");
            while (true)
            {
                try
                {
                    string msg = "";
                    string sql = "select * from i_s_gc_dusty_device where sfyx=1 and (ptid is null or ptid='') or (bindsuccess is null or bindsuccess=0)";
                    IList<IDictionary<string, object>> devicelist = CommonService.GetDataTable2(sql);
                    if (devicelist.Count > 0)
                    {
                        sql = "select lx, url from h_dust_apiconfig where lx in ('SaveDevice','UpdateDevice') and sfyx=1 ";
                        IList<IDictionary<string, string>> urlConfigs = CommonService.GetDataTable(sql);
                        string saveUrl = "";
                        string updateUrl = "";
                        var q = urlConfigs.Where(x => x["lx"] == "SaveDevice");
                        if (q.Count() > 0)
                        {
                            saveUrl = q.First()["url"];
                        }
                        q = urlConfigs.Where(x => x["lx"] == "UpdateDevice");
                        if (q.Count() > 0)
                        {
                            updateUrl = q.First()["url"];
                        }
                        if (saveUrl !="" && updateUrl!="")
                        {
                            // JSON 序列化和反序列化类
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            jss.MaxJsonLength = int.MaxValue;
                            string url = "";
                            string postdata = "";
                            string retmsg = "";
                            Dictionary<string, object> retdata = null;
                            int code = -9999;

                            foreach (var device in devicelist)
                            {
                                string ptid = device["ptid"].GetSafeString();
                                bool bindsuccess = device["bindsuccess"].GetSafeBool();
                                
                                // 需要提交的数据
                                Dictionary<string, object> data = new Dictionary<string, object>() {
                                        {"DeviceCode", device["devicecode"] },
                                        {"Lon", device["lon"] },
                                        {"Lat", device["lat"] },
                                        {"Expires", device["expires"] },
                                        {"Custom", device["custom"] },
                                        {"TypeCode", device["typecode"] },
                                        {"Notes", device["notes"] }
                                    };
                                // 未绑定过
                                if (ptid == "")
                                {
                                    //先调用保存接口
                                    url = saveUrl;
                                    postdata = "Token=" + HttpUtility.UrlEncode(Dust.DustGetToken()) +
                                        "&Datas=" + HttpUtility.UrlEncode(jss.Serialize(data));
                                    retmsg = MyHttp.SendDataByPost(url, postdata);
                                    SysLog4.WriteError(retmsg);

                                    if (retmsg !="")
                                    {
                                        retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                        if (retdata != null)
                                        {
                                            code = retdata["Code"].GetSafeInt();
                                            // 保存成功,更新设备表
                                            if (code == 0)
                                            {
                                                ptid = retdata["Datas"].GetSafeString();
                                                sql = string.Format("update i_s_gc_dusty_device set ptid='{0}', bindsuccess=1 where devicecode='{1}' and recid={2}", ptid, device["devicecode"].GetSafeString(),device["recid"].GetSafeString());
                                                CommonService.Execsql(sql);
                                            }
                                            // 当前设备编号在物联网平台中已登记，需要调用更新接口
                                            else if (code == 1)
                                            {
                                                ptid = retdata["Datas"].GetSafeString();
                                                data.Add("Id", ptid);
                                                url = updateUrl;
                                                postdata = "Token=" + HttpUtility.UrlEncode(Dust.DustGetToken()) +
                                                        "&Datas=" + HttpUtility.UrlEncode(jss.Serialize(data));
                                                retmsg = MyHttp.SendDataByPost(url, postdata);
                                                retdata = jss.Deserialize<Dictionary<string, object>>(retmsg);
                                                if (retdata !=null )
                                                {
                                                    code = retdata["Code"].GetSafeInt();
                                                    // 更新成功
                                                    if (code == 0)
                                                    {
                                                        sql = string.Format("update i_s_gc_dusty_device set ptid='{0}', bindsuccess=1 where devicecode='{1}' and recid={2}", ptid, device["devicecode"].GetSafeString(), device["recid"].GetSafeString());
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
                                else if (ptid !="" && !bindsuccess)
                                {
                                    data.Add("Id", ptid);
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
                                            sql = string.Format("update i_s_gc_dusty_device set ptid='{0}', bindsuccess=1 where devicecode='{1}' and recid={2}", ptid, device["devicecode"].GetSafeString(), device["recid"].GetSafeString());
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