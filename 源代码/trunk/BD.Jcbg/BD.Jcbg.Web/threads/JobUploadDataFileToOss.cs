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
    /// 将datafile表的文件上传到OSS
    /// </summary>
    public class JobUploadDataFileToOss : ISchedulerJob
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
        #endregion

        public void SetInterval(int seconds)
        {
            Interval = seconds * 1000;
        }

        public void Execute()
        {
            SysLog4.WriteError("开始线程JobUploadDataFileToOss");
            while (true)
            {
                try
                {
                    bool ret = true;
                    string msg = "";
                    string ossCdnUrl = Configs.GetConfigItem("FileOssCdn");
                    string ossCdnOwnerCode = Configs.GetConfigItem("OssCdnCodeWj");
                    if (ossCdnUrl !="" && ossCdnOwnerCode!="")
                    {
                        string sql = "select top 1 fileid,filename,filecontent,smallcontent from datafile where storagetype is null or storagetype='' order by cjsj";
                        IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                        if (dt.Count > 0)
                        {
                            foreach (var row in dt)
                            {

                                string fileid = dt[0]["fileid"].GetSafeString();
                                string filename = dt[0]["filename"].GetSafeString();
                                byte[] filecontent = dt[0]["filecontent"] as byte[];
                                byte[] smallcontent = dt[0]["smallcontent"] as byte[];
                                if (fileid != "")
                                {
                                    string fileurl = "";
                                    string smallurl = "";
                                    if (filecontent != null && filecontent.Length > 0)
                                    {
                                        ret = OssCdnUtil.UploadToOss(ossCdnUrl, ossCdnOwnerCode, filecontent, filename, out fileurl, out msg);
                                        if (!ret)
                                        {
                                            throw new Exception(string.Format("上传filecontent到OSS失败，fileid:{0},错误信息：{1}", fileid, msg));
                                        }
                                    }
                                    if (smallcontent != null && smallcontent.Length > 0)
                                    {
                                        ret = OssCdnUtil.UploadToOss(ossCdnUrl, ossCdnOwnerCode, smallcontent, filename, out smallurl, out msg);
                                        if (!ret)
                                        {
                                            throw new Exception(string.Format("上传smallcontent到OSS失败，fileid:{0},错误信息：{1}", fileid, msg));
                                        }
                                    }
                                    sql = "update datafile set filecontent=null, smallcontent=null, storagetype='OSS', fileurl='{0}',smallurl='{1}' where fileid='{2}'";
                                    sql = string.Format(sql, fileurl, smallurl, fileid);
                                    ret = CommonService.ExecSql(sql, out msg);
                                    if (!ret)
                                    {
                                        SysLog4.WriteError(string.Format("上传文件到oss失败,fileid:{0}, 错误信息：{1}", fileid, msg));
                                    }
                                }

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