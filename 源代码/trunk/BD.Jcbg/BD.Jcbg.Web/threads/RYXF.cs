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


namespace BD.Jcbg.Web.threads
{
    public class RYXF : ISchedulerJob
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

                    //下发虹膜
                    string sql = "select recid,rybh,gcbh,type from dbo.H_RY_XF";
                    IList<IDictionary<string, string>> table = CommonService.GetDataTable(sql);
                    for (int i = 0; i < table.Count; i++)
                    {
                        IDictionary<string, string> rowData = table[i];
                        bool isdone = false;//setnotic(rowData["title"].GetSafeString(), rowData["context"].GetSafeString(), rowData["reader"].GetSafeString(), rowData["logid"].GetSafeInt(), rowData["type"].GetSafeString());
                        isdone=HttpPost(rowData["rybh"].GetSafeString(), rowData["gcbh"].GetSafeString(), rowData["type"].GetSafeInt(1));
                        if (isdone)
                            CommonService.Delete("H_RY_XF", "RECID", rowData["recid"].GetSafeInt().ToString());
                    }

                    sql = "select recid,keyid,keydate,type from H_SB_ToPic";
                    table = CommonService.GetDataTable(sql);
                    for (int i = 0; i < table.Count; i++)
                    {

                        string dosql = "";
                        string reportFile="";
                        string recid="";
                        string ttable="";
                        string strwere="";
                        IDictionary<string, string> rowData = table[i];
                        recid=rowData["keyid"].GetSafeString();
                        if(rowData["type"].GetSafeString()=="az")
                        {
                            ttable="View_SB_Install|SB_SpecialPerson";
                            reportFile="安装告知表";
                            strwere="InstallID='" + recid + "'|FKID='" + recid + "'";
                            dosql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate],[ShowImgURL],[ShowRUL]) select RECID ,'设备安装',AZRQ,'/machineimg/" + recid + ".jpg','/machine/SBInstallReportDown?type=pic&recid=" + recid + "' from SB_ReportSBSY where InstallID='" + recid + "'";
                        }
                        else if (rowData["type"].GetSafeString() == "sy")
                        {
                            ttable = "View_SB_UseReg|View_SB_Install|SB_SpecialPerson";
                            reportFile = "使用登记表";
                            strwere = "UseID='" + recid + "'|InstallID=(select InstallID from View_SB_UseReg where UseID='" + recid + "')|FKID='" + recid + "'";
                            dosql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate],[ShowImgURL],[ShowRUL]) select RECID ,'设备使用',SYRQ,'/machineimg/" + recid + ".jpg','/machine/SBUseRegReportDown?type=pic&recid=" + recid + "' from SB_ReportSBSY where UseID='" + recid + "'";
                        }
                        else if (rowData["type"].GetSafeString() == "cx")
                        {
                            ttable = "View_SB_UnInstall|View_SB_Install|SB_SpecialPerson";
                            reportFile = "拆卸告知表";
                            strwere = "UnInstallID='" + recid + "'|InstallID=(select InstallID from View_SB_UnInstall where UnInstallID='" + recid + "')|FKID='" + recid + "'";
                            dosql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate],[ShowImgURL],[ShowRUL]) select RECID ,'设备拆卸',CXRQ,'/machineimg/" + recid + ".jpg','/machine/SBUnInstallReportDown?type=pic&recid=" + recid + "' from SB_ReportSBSY where UnInstallID='" + recid + "'";
                        }
                        else if (rowData["type"].GetSafeString() == "bg")
                        {

                            IList<IDictionary<string, string>> dt = CommonService.GetDataTable("select formid,sbbaid,gcbh,jyrq from View_ReportQZJ where workserial='" + recid + "'");

                            if (dt.Count > 0)
                            {
                                ttable = "stformitem|view_reportqzj|view_qzj_jybg_wz|view_qzj_jybg_tp";
                                reportFile = "起重机械检测报告";
                                strwere = "formid=" + dt[0]["formid"] + "|workserial=" + recid + "|formid=" + dt[0]["formid"] + "|formid=" + dt[0]["formid"] + "";
                                string jcbgurl = "/tz/FlowReportDown?" +
                                "filename=%e8%b5%b7%e9%87%8d%e6%9c%ba%e6%a2%b0%e6%a3%80%e6%b5%8b%e6%8a%a5%e5%91%8a" +
                                "&type=" + System.Web.HttpUtility.UrlDecode("pic") +
                                 "&where=" + System.Web.HttpUtility.UrlDecode(strwere) +
                                "&table=" + System.Web.HttpUtility.UrlDecode(ttable);

                                dosql = "INSERT INTO [SB_SBSY_Show]([SBSYID],[ShowText],[ShowDate],[ShowImgURL],[ShowRUL]) select RECID ,'检测报告','" + dt[0]["jyrq"].GetSafeDate().ToString("yyyy-MM-dd") + "','/machineimg/" + recid + ".jpg','" + jcbgurl + "' from SB_ReportSBSY where baid='" + dt[0]["sbbaid"] + "' and (jdzch='" + dt[0]["gcbh"] + "' or jdzch=(select SJGCBH from I_M_GC where gcbh='" + dt[0]["gcbh"] + "'))";
                            }
                        }
                        else
                        {
                            CommonService.Delete("H_SB_ToPic", "RECID", rowData["recid"].GetSafeInt().ToString());
                        }

                        var g = new ReportPrint.GenerateGuid();
                        var c = g.Get();
                        c.type = ReportPrint.EnumType.Word;
                        c.openType = ReportPrint.OpenType.PIC;

                        //c.field = reportFile;
                        c.fileindex = "0";
                        c.table = ttable;
                        c.filename = reportFile;
                        //c.field = "formid";
                        //c.where = "RECID=" + recid + "|FKID=(select InstallID from View_SB_Install where RECID=" + recid + ")";
                        c.libType = ReportPrint.LibType.OpenXmlSdk;
                        c.where = strwere;
                        c.signindex = 0;
                        //c.openType = ReportPrint.OpenType.Print ;

                        c.AllowVisitNum = 1;
                        c.customtools = "2,";
                        var guid = g.GetFiles(c, new System.Drawing.Size(800, 800));
                        if (guid.success)
                        {
                            //guid.fileBytes[0]

                            File.WriteAllBytes(NMapPath("/machineimg/") + recid + ".jpg", guid.fileBytes[0]);
                            if (dosql != "")
                            {
                                CommonService.Execsql(dosql);
                            }
                            CommonService.Delete("H_SB_ToPic", "RECID", rowData["recid"].GetSafeInt().ToString());
                            
                        }
                        
                    }

                    //生成图片
                   


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
       
        /// <summary>
       

        #region 调用接口

        
        public string NMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用 
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    //strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\'); 
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }


        private bool HttpPost(string rybh,string gcbh, int type)
        {
            bool ret = false;
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string url = "";
                string content="";
                string sfzhm="";
                string ryxm="";
                string hm = "";
                IList<IDictionary<string, string>> retdate = CommonService.GetDataTable("select ryxm,sfzhm,hm from i_m_ry where rybh='" + rybh + "'");
                if (retdate.Count > 0)
                {
                    sfzhm = CryptFun.Encode(retdate[0]["sfzhm"].GetSafeString());
                    ryxm = retdate[0]["ryxm"].GetSafeString();
                    hm = retdate[0]["hm"].GetSafeString();
                    gcbh = CryptFun.Encode(gcbh);
                }
                if (hm != "")
                {

                    if (type == 1)
                    {
                        url = "http://47.97.22.69:10003/wzwgryfun/DownWdyiris";
                        content = "sfzhm=" + HttpUtility.UrlEncode(sfzhm, System.Text.Encoding.UTF8) + "&ryxm=" + HttpUtility.UrlEncode(ryxm, System.Text.Encoding.UTF8) + "&hm=" + HttpUtility.UrlEncode(hm, System.Text.Encoding.UTF8) + "&gcbh=" + HttpUtility.UrlEncode(gcbh, System.Text.Encoding.UTF8);

                        string rettext = SendDataByGET(url + "?" + content);
                        RetJson retjson = jss.Deserialize<RetJson>(rettext);

                        if (retjson.code == "0")
                            ret = true;
                        else if (retjson.msg == "没有找到相应考勤机！")
                        {
                            ret = true;
                        }

                    }
                    if (type == 2)
                    {
                        url = "http://47.97.22.69:10003/wzwgryfun/DelWdyiris";
                        content = "sfzhm=" + HttpUtility.UrlEncode(sfzhm, System.Text.Encoding.UTF8) + "&gcbh=" + HttpUtility.UrlEncode(gcbh, System.Text.Encoding.UTF8);

                        string rettext = SendDataByGET(url + "?" + content);
                        RetJson retjson = jss.Deserialize<RetJson>(rettext);

                        if (retjson.code == "0")
                            ret = true;
                        else if (retjson.msg == "没有找到相应考勤机！")
                        {
                            ret = true;
                        }

                    }
                }
                else
                {
                    ret = true;
                }
                /*
                //Configs.GetConfigItem("pushurl"); //"http://139.129.205.143:8001/process/notify/wzoa/itask";
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "POST";
                req.ContentType = "text/xml;charset=UTF-8";
                byte[] datas = Encoding.GetEncoding("GBK").GetBytes(content);
                using (Stream stream = req.GetRequestStream())
                {
                    stream.Write(datas, 0, datas.Length);
                }
                req.GetResponse();*/
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
            }
            finally
            {
                //Thread.Sleep(threadTime);
            }
            return ret;
        }


        public string SendDataByGET(string Url)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }

        public string SendDataByPost(string Url)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Method = "POST";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }



        /// <summary>
        /// 标准上传json实例化
        /// </summary>
        public class RetJson
        {
            /// <summary>
            /// 二维码号码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 顺序号（数字，整型）
            /// </summary>
            public string msg { get; set; }
            
        }

        #endregion


    }
}