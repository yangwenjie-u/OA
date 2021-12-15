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
    public class SmsSend
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

        private static ISmsServiceWzzjz _smsServiceWzzjz = null;
        private static ISmsServiceWzzjz SmsServiceWzzjz
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
                    string sql = "select * from OA_SMS_DXDSFS where fssj < getdate() ";
                    IList<string> lsql = new List<string>();
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    string msg = "";
                    // 发送每一条定时短信
                    foreach (var row in dt)
                    {
                        string sjhm = row["sjhm"].GetSafeString();
                        string contents = row["contents"].GetSafeString();
                        string recid = row["recid"].GetSafeString();
                        bool notencoded = row["notencoded"].GetSafeBool();

                        if (contents != "" && (!notencoded))
                        {
                            contents = DataFormat.DecodeBase64(contents);
                        }
                        if (sjhm !="" && contents!="")
                        {
                            sql = "delete from OA_SMS_DXDSFS where recid=" + recid;
                            lsql.Add(sql);
                            SmsServiceWzzjz.SendMessageV2(Func.GlobalVariable.GetSmsBaseAppId(), Guid.NewGuid().ToString(), sjhm, contents, out msg);
                        }
                    }
                    // 删除发过的短信
                    if (lsql.Count > 0)
                    {
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