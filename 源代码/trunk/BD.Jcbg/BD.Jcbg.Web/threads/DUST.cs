using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Spring.Context;
using Spring.Context.Support;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using Bd.jcbg.Common;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using System.Threading;
using ReportPrintService.OpenXmlSdk;

namespace BD.Jcbg.Web.threads
{
    public class DUST : ISchedulerJob
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
        protected int Interval = 10000;	// 毫秒
        private static Thread thread;                                   //定时器
        private readonly static int threadTime = 1 * 60 * 1000;         //间隔发送时间
        private static bool threadflag = true;                         //线程循环标记


        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

         public void Execute()
        {
            while (threadflag)
            {

                try
                {
                    string sql = "select recid,bodytext from I_S_GC_DUST_Alert where issend=0";
                    IList<IDictionary<string, string>> table = CommonService.GetDataTable(sql);
                    for (int i = 0; i < table.Count; i++)
                    {
                        IDictionary<string, string> rowData = table[i];
                        string text = rowData["bodytext"].GetSafeString();
                        Dictionary<string, object> param = null;
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        try
                        {
                            param = jss.Deserialize<Dictionary<string, object>>(text);

                            string DeviceCode = param["DeviceCode"].GetSafeString();
                            string SensorCode = param["SensorCode"].GetSafeString();
                            string SensorValue = param["SensorValue"].GetSafeString();


                            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select recid from I_S_GC_DUST_Alert_Deal where IsSend=1 and DeviceCode='" + DeviceCode + "' and CreateTime > DATEADD(hh,-2,getdate())");
                            if (dt.Count > 0)
                            {
                                string smsmsg = "";
                                string setsql = "INSERT INTO [I_S_GC_DUST_Alert_Deal]([BodyText] ,[GCBH] ,[GCMC] ,[DeviceCode] ,[CreateTime] ,[IsSend],[IsDeal]) select '" + text + "',gcbh,gcmc,'" + DeviceCode + "',getdate(),0,0 from i_S_GC_DUST where [DeviceCode]='" + DeviceCode + "'";
                                CommonService.Execsql(setsql);
                            }
                            else
                            {

                                IList<IDictionary<string, string>> dt1 = CommonService.GetDataTable("select gcmc from i_S_GC_DUST where DeviceCode='" + DeviceCode + "'");
                                if (dt1.Count > 0)
                                {
                                    string smsmsg = "";
                                    string setsql = "";
                                    

                                    IList<IDictionary<string, string>> dt2 = CommonService.GetDataTable("select recid from I_S_GC_DUST_Alert_Deal where DeviceCode='" + DeviceCode + "' and CreateTime > DATEADD(mi,-30,getdate())");
                                    if (dt2.Count >= 1)
                                    {
                                        setsql = "INSERT INTO [I_S_GC_DUST_Alert_Deal]([BodyText] ,[GCBH] ,[GCMC] ,[DeviceCode] ,[CreateTime] ,[IsSend],[IsDeal]) select '" + text + "',gcbh,gcmc,'" + DeviceCode + "',getdate(),1,0 from i_S_GC_DUST where [DeviceCode]='" + DeviceCode + "'";
                                        CommonService.Execsql(setsql);

                                        TimeSpan nowDt = DateTime.Now.TimeOfDay;

                                        TimeSpan workStartDT = DateTime.Parse("6:00").TimeOfDay;
                                        TimeSpan workEndDT = DateTime.Parse("21:00").TimeOfDay;
                                        if (nowDt > workStartDT && nowDt < workEndDT)
                                        {
                                            smsmsg = dt1[0]["gcmc"].GetSafeString() + "工程扬尘设备PM10超标，当前值：" + SensorValue + "，请及时采取降尘措施。";
                                            smsmsg = Convert.ToBase64String(Encoding.UTF8.GetBytes(smsmsg));
                                            setsql = "Insert INTO OA_SMS_DXDSFS ([SJHM],[CONTENTS],FSSJ) select dh,'" + smsmsg + "',getdate() from I_S_GC_JLRY where gw='项目总监' and len(DH)=11 and gcbh in(select gcbh from i_S_GC_DUST where [DeviceCode]='" + DeviceCode + "')";
                                            CommonService.Execsql(setsql);
                                            setsql = "Insert INTO OA_SMS_DXDSFS ([SJHM],[CONTENTS],FSSJ) select dh,'" + smsmsg + "',getdate() from I_S_GC_JLRY where gw='项目经理'  and len(DH)=11 and gcbh in(select gcbh from i_S_GC_DUST where [DeviceCode]='" + DeviceCode + "' )";
                                            CommonService.Execsql(setsql);
                                        }
                                    }
                                    else
                                    {
                                        setsql = "INSERT INTO [I_S_GC_DUST_Alert_Deal]([BodyText] ,[GCBH] ,[GCMC] ,[DeviceCode] ,[CreateTime] ,[IsSend],[IsDeal]) select '" + text + "',gcbh,gcmc,'" + DeviceCode + "',getdate(),0,0 from i_S_GC_DUST where [DeviceCode]='" + DeviceCode + "'";
                                        CommonService.Execsql(setsql);
                                    }

                                }



                            }

                            


                        }
                        catch (Exception et)
                        {
                            param = null;
                        }
                        finally
                        {
                            //CommonService.Delete("I_S_GC_DUST_Alert", "RECID", rowData["recid"].GetSafeInt().ToString());
                            CommonService.Execsql("update I_S_GC_DUST_Alert set issend=1 where recid=" + rowData["recid"].GetSafeInt().ToString());
                        }
                        

                    }

                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                finally
                {
                    Thread.Sleep(Interval);
                }
            }
        }

    }
}