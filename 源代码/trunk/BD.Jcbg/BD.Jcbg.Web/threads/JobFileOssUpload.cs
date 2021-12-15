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
    /// 根据配置文件，把文件上传到对象存储
    /// </summary>
    public class JobFileOssUpload : ISchedulerJob
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
            SysLog4.WriteError("开始线程JobFileOssUpload");
            while (true)
            {
                try
                {
                    FileOssConfig config = new FileOssConfig();
                    string msg = "";
                    bool code = config.Load(out msg);
                    if (!code)
                    {
                        SysLog4.WriteError("上传文件配置加载错误，错误信息：" + msg);
                    }
                    else
                    {
                        foreach (FileOssConfigUploadBase uploadItem in config.UploadItems)
                        {
                            // 文件上传
                            if (uploadItem.UploadType == FileOssConfigUploadType.File)
                            {
                                FileOssConfigUploadFile fileUploadItem = (FileOssConfigUploadFile)uploadItem;
                                string rootPath = fileUploadItem.PathName;
                                IList<string> files = new List<string>();
                                GetFiles(fileUploadItem.PathName, files, fileUploadItem.ScanSub);
                                foreach (string filepath in files)
                                {
                                    UploadFile(config.Server, config.OwnerCode, filepath, fileUploadItem.Params);
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
            //SysLog4.WriteError("退出线程JobFileOssUpload");
        }
        /// <summary>
        /// 上传某个文件到云存储
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool UploadFile(string server, string ownercode, string filepath, 
            IList<string> extraParams)
        {
            string msg = "";
            bool code = false;
            try
            {
                if (!System.IO.File.Exists(filepath))
                    return true;
                if (FileInUse(filepath))
                    return true;
                
                OSS oss = new OSS(server);
                // 格式化查询参数
                string pathname = GetFolerName(filepath);

                IList<string> formatedParams = new List<string>();
                string logParams = "";
                if (extraParams != null)
                {
                    foreach (string strSrc in extraParams)
                    {
                        // 替换目录名称
                        string formated = strSrc.Replace("{FolderName}", pathname);
                        // sql查询
                        if (formated.IndexOf("select ") > -1)
                        {
                            IList<IDictionary<string, string>> dt = CommonService.GetDataTable(formated);
                            if (dt.Count > 0 && dt[0].Keys.Count>0)
                            {
                                formated = dt[0][dt[0].Keys.ElementAt(0)];
                            }
                        }
                        logParams+=formated+",";
                        formatedParams.Add(formated);
                    }
                }

                var ossRet = oss.UploadFile(ownercode, filepath, formatedParams == null ? null : formatedParams.ToArray());
                if (ossRet.success)
                {
                    string filename = System.IO.Path.GetFileName(filepath);
                    string sql = "INSERT INTO CompanyFileOss([FileId],[TableId],[SrcRecid],[SrcFileName],[UploadFileId],[UploadExtraMsg],[UploadTime]) values('"+Guid.NewGuid().ToString()+"','','','"+filename+"','"+ossRet.fileId+"','"+ossRet.message+"',getdate())";
                    IList<string> sqls = new List<string>();
                    sqls.Add(sql);
                    bool codeInsert = CommonService.ExecTrans(sqls);
                    if (codeInsert)
                        System.IO.File.Delete(filepath);
                    else
                        SysLog4.WriteError("添加记录到CompanyFileOss失败，SQL：" + sql);
                    //SysLog4.WriteError("上传文件成功，原文件：" + filepath + ",参数：" + logParams + "，返回id：" + ossRet.fileId + "，写入数据库：" + (codeInsert ? "成功" : "失败"));
                }
                else
                {
                    SysLog4.WriteError("上传文件："+filepath+"失败，返回错误信息："+ossRet.message);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(filepath, ex);
                msg = ex.Message;
            }
            return code;

        }
        /// <summary>
        /// 文件是否被占用
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private bool FileInUse(string filepath)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {

                fs = new FileStream(filepath, FileMode.Open, FileAccess.Read,

                FileShare.None);

                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }
        /// <summary>
        /// 获取文件所在的文件夹名称
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private string GetFolerName(string filepath)
        {
            string ret = "";
            try
            {
                string path = System.IO.Path.GetDirectoryName(filepath);
                int index = path.LastIndexOf("\\");
                if (index > -1)
                    ret = path.Substring(index + 1);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(filepath, ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取目录下的文件
        /// </summary>
        /// <param name="path">根目录</param>
        /// <param name="FileList">返回的文件列表</param>
        /// <param name="scanSub">是否扫描子目录</param>
        /// <returns></returns>
        public static IList<string> GetFiles(string path, IList<string> FileList, bool scanSub)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                FileList.Add(f.FullName);//添加文件路径到列表中
            }
            if (!scanSub)
                return FileList;
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFiles(d.FullName, FileList, scanSub);
            }
            return FileList;
        }
    }
}