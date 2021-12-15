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
    public class UpdateWgrykqjLog : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒

        #region 服务
        public IWgryKqjService _wgrykqjService = null;
        public IWgryKqjService WgryKqjService
        {
            get
            {
                if (_wgrykqjService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _wgrykqjService = webApplicationContext.GetObject("WgryKqjService") as IWgryKqjService;
                }
                return _wgrykqjService;
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
                    IList<IDictionary<string, string>> data = WgryKqjService.GetKqjUserLog();
                    foreach (IDictionary<string, string> row in data)
                    {
                        DateTime dt = DateTime.Parse(row["logdate"]);
                        WgryKqjService.SaveUserLog(row["serial"], row["userid"], dt);

                        string kqjlx = row["logtype"];
                       // WgryKqjService.SaveUserLog_WX(row["serial"], row["userid"], dt, kqjlx);
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