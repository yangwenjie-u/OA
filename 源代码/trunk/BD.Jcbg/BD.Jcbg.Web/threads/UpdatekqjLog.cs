using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading;
using Spring.Context;
using Spring.Context.Support;
using Newtonsoft.Json;
using System.Reflection;
using System.Data;
using BD.Jcbg.Bll;
using BD.Jcbg.IBll;

namespace BD.Jcbg.Web.threads
{
    public class UpdatekqjLog : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒

        #region 服务
        public IKqjCmdService _kqjService = null;
        public IKqjCmdService KqjService
        {
            get
            {
                if (_kqjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _kqjService = webApplicationContext.GetObject("KqjService") as IKqjCmdService;
                }
                return _kqjService;
            }
        }
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }
        public void Execute()
        {
            while (true)
            {
                try
                {
                    IList<IDictionary<string, string>> data = KqjService.GetKqjUserLog();
                    foreach (IDictionary<string, string> row in data)
                    {
                        DateTime dt = DateTime.Parse(row["logdate"]);
                        KqjService.SaveUserLog(row["serial"], row["userid"], dt);
                    }
                }
                catch (Exception e)
                {
                  //  SysLog.WriteLog(e);
                }


                Thread.Sleep(Interval);
            }
        }
    }
}