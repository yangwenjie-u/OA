using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace BD.Jcbg.Common
{
    public class FileOssConfig
    {
        private string mFilePath = "";

        public bool IsValid{get;set;}

        public IList<FileOssConfigUploadBase> UploadItems = null;

        public string Server { get; set; }

        public string OwnerCode { get; set; }
        public FileOssConfig()
        {
            IsValid = false;
            mFilePath = "";
            OwnerCode = "";
            Server = "";
        }

        public bool Load(out string msg)
        {
            IsValid = false;
            msg = "";
            mFilePath = string.Format(@"{0}\configs\fileoss.xml", SysEnvironment.CurPath);
            try
            {
                XDocument document = XDocument.Load(mFilePath);
                var query = from e in document.Elements("FileOss") select e;
                foreach (XElement ele in query)
                {
                    if (OwnerCode == "")
                        OwnerCode = ele.Attribute("OwnerCode").Value.GetSafeString();
                    if (Server == "")
                        Server = ele.Attribute("Server").Value.GetSafeString();
                    if (Server != "")
                        break;
                }
                if (Server == "")
                {
                    msg = "OSS服务地址未配置";
                    return IsValid;
                }

                query = from m in document.Elements("FileOss").Elements("Uploads").Elements("Upload")
                            select m;
                UploadItems = new List<FileOssConfigUploadBase>();

                foreach (XElement ele in query)
                {
                    FileOssConfigUploadBase uploadItem = null;
                    if (ele.Attribute("type").Value.GetSafeString() == "path")
                    {
                        uploadItem = new FileOssConfigUploadFile();
                        if (uploadItem.Load(ele, out msg))
                            UploadItems.Add(uploadItem);
                    }
                }
                IsValid = true;
                msg = "";
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return IsValid;
        }
    }
    /// <summary>
    /// 上传类型
    /// </summary>
    public enum FileOssConfigUploadType
    {
        File,
        Table
    }
    /// <summary>
    /// 上传配置基类
    /// </summary>
    public class FileOssConfigUploadBase
    {
        public FileOssConfigUploadType UploadType { get; set; }
        public IList<string> Params { get; set; }

        public virtual bool Load(XElement ele, out string msg)
        {
            msg = "";
            return true;
        }
    }

    public class FileOssConfigUploadFile : FileOssConfigUploadBase
    {
        public bool ScanSub { get; set; }
        public string PathName { get; set; }

        public override  bool Load(XElement ele, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                UploadType = FileOssConfigUploadType.File;

                ScanSub = false;
                var q = from e in ele.Elements("ScanSub") select e;
                if (q.Count() > 0)
                    ScanSub = q.First().Value.GetSafeBool();

                PathName = "";
                q = from e in ele.Elements("PathName") select e;
                if (q.Count() > 0)
                    PathName = q.First().Value.GetSafeString();
                if (!System.IO.Directory.Exists(PathName))
                {
                    msg = "文件对象存储配置路径:" + PathName + "，不存在。这条配置被抛弃。";
                    SysLog4.WriteError(msg);
                    return code;
                }

                Params = new List<string>();
                q = from e in ele.Elements("Params").Elements("Param") select e;
                foreach (XElement eleParam in q)
                {
                    Params.Add(eleParam.Value.GetSafeString());
                }
                code = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }

    }
}
