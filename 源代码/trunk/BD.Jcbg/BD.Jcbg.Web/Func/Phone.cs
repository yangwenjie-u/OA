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
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Web.Remote;
using System.Threading;


namespace BD.Jcbg.Web.Func
{
    public class Phone
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

        private static Thread thread;                                   //定时器
        private readonly static int threadTime = Configs.GetConfigItem("pushtime").GetSafeInt(30) * 1000;         //间隔发送时间
        private static bool threadflag = false;                         //线程循环标记
        public static void StopSend()
        {
            threadflag = false;         //停止线程
            thread.Abort();
        }
         /// <summary>
        /// 开始启动发送线程
        /// </summary>
        public static void StartSend()
        {
            threadflag = true;          //开始线程循环标记
            thread = new Thread(new ThreadStart(ThreadOpt));
            thread.Start();
        }

        /// <summary>
        /// 发送线程
        /// </summary>
        public static void ThreadOpt()
        {
            //循环
            while (threadflag)
            {
                
                try
                {
                    string sql = "select recid,reader,type,title,context,logid from PhoneAlert";
                    IList<IDictionary<string, string>> table = CommonService.GetDataTable(sql);
                    for (int i = 0; i < table.Count; i++)
                    {
                        IDictionary<string, string> rowData = table[i];
                        setnotic(rowData["title"].GetSafeString(), rowData["context"].GetSafeString(), rowData["reader"].GetSafeString(), rowData["logid"].GetSafeInt(), rowData["type"].GetSafeString());
                        CommonService.Delete("PhoneAlert", "Recid", rowData["recid"].GetSafeInt().ToString());
                    }

                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e);
                }
                finally
                {
                    Thread.Sleep(threadTime);
                }
            }
        }

        #region 推送消息1

        private static void setnotic(string title, string body, string reader, int recid, string notictype)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<req><auth><username>{0}</username><type>oa</type></auth>", reader));
            string msg = "<list>"
                     + "<item>"
                     + "<id>" + recid.ToString() + "</id>"
                     + "<lx>" + notictype + "</lx>"
                     + "<priority_level>一般</priority_level>"
                     + "<title>" + title + "</title>"
                     + "<detail>" + body.EncodeBase64() + "</detail>"
                     + "</item>"
                     + "</list>";
            sb.Append(msg);
            sb.Append("</req>");
            HttpPost(sb.ToString());
        }

        private static void HttpPost(string content)
        {
            string url = Configs.GetConfigItem("pushurl"); //"http://139.129.205.143:8001/process/notify/wzoa/itask";
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "text/xml;charset=UTF-8";
            byte[] datas = Encoding.GetEncoding("GBK").GetBytes(content);
            using (Stream stream = req.GetRequestStream())
            {
                stream.Write(datas, 0, datas.Length);
            }
            req.GetResponse();
        }

        #endregion


    }
}