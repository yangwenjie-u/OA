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
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.threads
{
    public class UpdateRyOutSchedule : ISchedulerJob
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
                    SysLog4.WriteError("出工地轮询判断---------------");

                    //人员自动轮询小时间隔
                    //int ryoutTime = 12;
                    //String checkTime = DateTime.Now.AddHours(-ryoutTime).ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime time = DateTime.Now;
                    int hour = time.Hour;
                    int minute = time.Minute;
                    if(hour==22)
                    {
                        SysLog4.WriteError("出工地轮询");

                        //出工地轮询
                        int ryoutTime = 18;
                        String checkTime = DateTime.Now.AddHours(-ryoutTime).ToString("yyyy-MM-dd HH:mm:ss");
                        WgryKqjService.UpdateSgryOutSchedule(checkTime);
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteError(e.Message);
                }


                Thread.Sleep(Interval);
            }
        }
    }
}