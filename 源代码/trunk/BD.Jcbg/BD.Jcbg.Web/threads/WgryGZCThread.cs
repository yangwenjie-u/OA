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
    public class WgryGZCThread : ISchedulerJob
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
                    DateTime dt = DateTime.Now;
                    if (dt.Day ==14& dt.Hour>20) //每月15号前要填写好工资册
                    {
                        IList<IDictionary<string, string>> data = WgryKqjService.GetYHGClist();
                        foreach (IDictionary<string, string> row in data)
                        {
                            string gcbh = row["gcbh"];
                            string gcmc = row["gcmc"];

                            WgryKqjService.SetGzcYJ(gcbh, gcmc, dt);
                            WgryKqjService.SetGzcYJErrInfo(gcbh, gcmc, dt);
                            break;
                        }
                    }
                    Thread.Sleep(5000);
                    if (dt.Day == 15 & dt.Hour > 20)
                    {
                        IList<IDictionary<string, string>> yhgcdata = WgryKqjService.GetYHGClist();
                        foreach (IDictionary<string, string> row in yhgcdata)
                        {
                            string gcbh = row["gcbh"];
                            string gcmc = row["gcmc"];

                            WgryKqjService.SetYHYEYJ(gcbh, gcmc, dt);


                        }
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