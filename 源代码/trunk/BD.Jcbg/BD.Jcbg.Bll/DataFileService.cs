using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using BD.IDataInputDao;
using System.Xml.Linq;
using System.IO;
using System.Web;
using BD.DataInputModel.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Bll
{
    public class DataFileService: IDataFileService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }

        IDataFileDao DataFileDao { get; set; }
        #endregion

        #region 方法


        /// <summary>
        /// 保存datafile
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="filename"></param>
        /// <param name="filecontent"></param>
        /// <param name="fileext"></param>
        /// <param name="cjsj"></param>
        /// <param name="msg"></param>
        /// <param name="storgetype"></param>
        /// <returns></returns>
        public bool SaveDataFile(string fileid, string filename, byte[] filecontent, string fileext, string cjsj, out string msg, string storgetype = "")
        {
            bool ret = true;
            msg = "";
            try
            {
                if (fileid != "" && filename != "" && (filecontent != null && filecontent.Length > 0))
                {
                    byte[] smallcontent = null;
                    MyImage img = new MyImage(filecontent);
                    if (img.IsImage())
                    {
                        smallcontent = img.GetThumbnail();
                    }
                    bool isoss = false;
                    storgetype = storgetype.GetSafeString();
                    if (storgetype.Equals("oss", StringComparison.OrdinalIgnoreCase))
                    {
                        isoss = true;
                    }
                    else
                    {
                        if (GetOssConfig(out storgetype))
                        {
                            if (storgetype.Equals("oss", StringComparison.OrdinalIgnoreCase))
                            {
                                isoss = true;
                            }
                        }
                    }
                    DataFile file = new DataFile();
                    file.FILEID = fileid;
                    file.FILENAME = filename;
                    file.FILEEXT = fileext;
                    file.FILECONTENT = filecontent;
                    file.SMALLCONTENT = smallcontent;
                    file.CJSJ = cjsj;
                    if (isoss)
                    {
                        string ossCdnUrl = Configs.GetConfigItem("FileOssCdn");
                        string ossCdnOwnerCode = Configs.GetConfigItem("OssCdnCodeWj");
                        if (ossCdnUrl != "" && ossCdnOwnerCode != "")
                        {
                            string url = "";
                            ret = OssCdnUtil.UploadToOss(ossCdnUrl, ossCdnOwnerCode, filecontent, filename, out url, out msg);
                            if (ret)
                            {
                                file.FILECONTENT = null;
                                file.FILEURL = url;
                                file.STORAGETYPE = "OSS";

                                if (smallcontent!=null && smallcontent.Length > 0)
                                {
                                    ret = OssCdnUtil.UploadToOss(ossCdnUrl, ossCdnOwnerCode, filecontent, filename, out url, out msg);
                                    if (ret)
                                    {
                                        file.SMALLCONTENT = null;
                                        file.SMALLURL = url;
                                    }
                                    else
                                    {
                                        throw new Exception("OSS保存文件失败");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("OSS保存文件失败");
                            }

                        }
                        else
                        {
                            throw new Exception("保存文件失败 oss配置错误");
                        }
                    }
                    ret = DataFileDao.SaveFile(file);
                    if (!ret)
                    {
                        msg = "保存文件失败";
                    }
                }
                else
                {
                    ret = false;
                    msg = "缺少文件信息";
                }
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
            }
            
            return ret;
        }


        private bool GetOssConfig(out string storagetype)
        {
            bool ret = true;
            storagetype = "";
            try
            {
                string filepath = string.Format(@"{0}\configs\fileoss.xml", SysEnvironment.CurPath);
                XDocument document = XDocument.Load(filepath);
                var query = from e in document.Elements("FileOss") select e;
                foreach (XElement ele in query)
                {
                    storagetype = ele.Attribute("StorageType").Value.GetSafeString();
                }
            }
            catch (Exception e)
            {
                ret = false;
            }
            return ret;
        }

        #endregion

    }
}
