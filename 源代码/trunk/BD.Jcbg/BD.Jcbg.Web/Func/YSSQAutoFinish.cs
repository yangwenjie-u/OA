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
    public class YSSQAutoFinish
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
        private readonly static int threadTime = 30 * 1000;             //轮询间隔
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
        /// 轮询线程
        /// </summary>
        public static void ThreadOpt()
        {
            //循环
            while (threadflag)
            {

                try
                {
                    // 获取验收申请中，监督工程师不参加，未验收，已超过验收时间的申请记录
                    string sql = "select workserial from jdbg_yssqjl where sfcj=0 and yszt=0 and yssj < getdate() ";
                    string serialnos = "";
                    IList<IDictionary<string, string>> table = CommonService.GetDataTable(sql);
                    for (int i = 0; i < table.Count; i++)
                    {
                        IDictionary<string, string> rowData = table[i];
                        if (serialnos != "")
                            serialnos += ",";
                        serialnos += rowData["workserial"].GetSafeString();
                    }
                    if (serialnos !="")
                    {
                        sql = "update jdbg_yssqjl set yszt=1 where workserial in ( " + DataFormat.FormatSQLInStr(serialnos) + " ) ";
                        IList<string> lsql = new List<string>();
                        lsql.Add(sql);
                        CommonService.ExecTrans(lsql);
                        
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
    }
}