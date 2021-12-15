using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading;
using BD.Jcbg.Common;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.IBll;
using Newtonsoft.Json;
using System.Reflection;
using BD.Jcbg.Bll;

namespace BD.Jcbg.Web.threads
{
    public class JobSmsWgry : ISchedulerJob
	{
		protected int Interval = 10000;	// 毫秒

		#region 服务
        private ISmsServiceWgry _smsServiceWgry = null;
        private ISmsServiceWgry SmsServiceWgry
        {
            get
            {
                if (_smsServiceWgry == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _smsServiceWgry = webApplicationContext.GetObject("SmsServiceWgry") as ISmsServiceWgry;
                }
                return _smsServiceWgry;
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
                    IList<IDictionary<string, string>> data = SmsServiceWgry.GetSMSdata();
                    foreach (IDictionary<string, string> row in data)
                    {                    
                        string ReceiveUser = row["receiveuser"].GetSafeString();
                        string phone = row["phone"].GetSafeString();
                        string message = row["message"].GetSafeString();
                        string guid = row["guid"].GetSafeString();
                        string lx = row["lx"].GetSafeString();
                        SmsServiceWgry.DoSendMessage(phone, message, guid, lx);
                    }
                }
                catch (Exception e)
                {
                    SysLog4.WriteLog(e.Message);
                }


				Thread.Sleep(Interval);
			}
		}

		
	}
}