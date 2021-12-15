using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OssSDK;
using System.Net;
using System.IO;

namespace BD.Jcbg.Common
{
    public static  class OssCdnUtil
    {
        public static OSS_CDNResult UploadFileToOss(string ossCdnUrl, string ossCdnCode, byte[] data)
        {
            OSS_CDN oSS_CDN = new OSS_CDN(ossCdnUrl, "osscdn.jzyglxt.com");
            OSS_CDNResult oSS_CDNResult = oSS_CDN.UploadFile(ossCdnCode, data, new string[0]);
            return oSS_CDNResult;
        }

        public static OSS_CDNResult UploadFileToOss(string ossCdnUrl, string ossCdnCode, byte[] data, string filename)
        {
            OSS_CDN oSS_CDN = new OSS_CDN(ossCdnUrl, "osscdn.jzyglxt.com");
            OSS_CDNResult oSS_CDNResult = oSS_CDN.UploadFile(ossCdnCode, data, filename,new string[0]);
            return oSS_CDNResult;
        }

        public static bool UploadToOss(string ossCdnUrl, string ossCdnCode, byte[] data, string filename, out string url, out string msg)
        {
            bool ret = false;
            msg = "";
            url = "";
            try
            {
                if (ossCdnUrl !="" && ossCdnCode!="")
                {
                    OSS_CDNResult rs = UploadFileToOss(ossCdnUrl, ossCdnCode, data, filename);
                    if (rs.success)
                    {
                        ret = true;
                        msg = "";
                        url = rs.Url;
                    }
                    else
                    {
                        ret = false;
                        msg = rs.message;
                        url = "";
                    }
                }
                else
                {
                    ret = false;
                    msg = "OSS参数配置错误";
                    url = "";
                }

                
            }
            catch (Exception e)
            {
                ret = false;
                msg = e.Message;
                url = "";
            }
            return ret;
        }

        public static byte[] DownFile(string url)
        {
            byte[] ret = null;
            WebRequest req = WebRequest.Create(url);
            WebResponse response = req.GetResponse();
            Stream responseStream = response.GetResponseStream();
            byte[] buffer = new byte[1024];
            MemoryStream memoryStream = new MemoryStream();
            int count;
            while ((count = responseStream.Read(buffer, 0, 1024)) > 0)
            {
                memoryStream.Write(buffer, 0, count);
            }
            memoryStream.Position = 0L;
            ret = memoryStream.ToArray();
            memoryStream.Close();
            return ret;
        }
    }
}
