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
using ReportPrint.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BD.WorkFlow.IBll;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using ICommonService = BD.Jcbg.IBll.ICommonService;

namespace BD.Jcbg.Web.threads
{
    /// <summary>
    /// 将stfile表的文件上传到OSS
    /// </summary>
    public class JobUploadStfileToOss : ISchedulerJob
    {
        protected int Interval = 10000;	// 毫秒
        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
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

        private IWorkFlowService _workflowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workflowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workflowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workflowService;
            }
        }
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobUploadStfileToOss");
            while (true)
            {
                try
                {
                    bool ret = true;
                    string msg = "";
                    string sql = "select top 10 fileid from stfile where storagetype is null or storagetype='' order by fileid";
                    IList<IDictionary<string, string>> dt = CommonService.GetDataTable(sql);
                    if (dt.Count > 0)
                    {
                        foreach (var row in dt)
                        {
                            
                            int fileid = dt[0]["fileid"].GetSafeInt();
                            if (fileid > 0)
                            {
                                StFile file = WorkFlowService.GetFile(fileid);
                                WorkFlowService.SaveFile(file);
                            }
                            
                        }
                    }


                }
                catch (Exception ex)
                {
                    SysLog4.WriteLog(ex);

                }


                Thread.Sleep(Interval);
            }

        }
    }
}