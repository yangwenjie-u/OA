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

namespace BD.Jcbg.Web.threads
{
    public class JobCleanTempReportFileWzzjz : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒

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

                    string dirpath = SysEnvironment.CurPath + @"\report\pdftemp";
                    if (Directory.Exists(dirpath))
                    {
                        var files = Directory.GetFiles(dirpath, "(*.pdf)").Where(s => s.StartsWith("T-"));
                        DirectoryInfo di = new DirectoryInfo(dirpath);
                        FileInfo[] flist = di.GetFiles("T-*.pdf");
                        DateTime nw = DateTime.Now;
                        foreach (var f in flist)
                        {
                            string cts = f.Name.Replace("T-", "").Replace(".pdf", "");
                            DateTime ct = DateTime.Now;
                            if (DateTime.TryParseExact(cts, "yyyyMMddHHmmssffff", null, System.Globalization.DateTimeStyles.None, out ct))
                            {
                                if (ct.AddDays(1) < nw )
                                {
                                    f.Delete();
                                }
                            }
                        }
                    }
                    
                        

                }
                catch (Exception e)
                {
                }


                Thread.Sleep(Interval);
            }
        }
    }
}