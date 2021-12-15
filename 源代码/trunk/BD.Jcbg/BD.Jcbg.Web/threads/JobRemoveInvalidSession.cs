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
using OssSDK;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 删除过期的api接口session
    /// </summary>
    public class JobRemoveInvalidSession : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
        #region 服务
        private static IApiSessionService _apiSessionService = null;
        private static IApiSessionService ApiSessionService
        {
            get
            {
                if (_apiSessionService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _apiSessionService = webApplicationContext.GetObject("ApiSessionService") as IApiSessionService;
                }
                return _apiSessionService;
            }
        }
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobRemoveInvalidSession");
            int expireHour = BD.Jcbg.Web.Func.GlobalVariable.GetApiSessionExpireHours();
            while (true)
            {
                try
                {
                    ApiSessionService.DeleteExpireSessions(expireHour);
                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }
            //SysLog4.WriteError("退出线程JobFileOssUpload");
        }
    }
}