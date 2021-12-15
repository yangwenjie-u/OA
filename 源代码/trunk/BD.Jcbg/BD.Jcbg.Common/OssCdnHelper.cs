using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BD.Jcbg.Common
{
    public class FileOssCdnReturn
    {
        public bool Success { get; set; }

        public byte[] FileBytes { get; set; }

        public string FileName { get; set; }

        public string ErrorMsg { get; set; }
    }

    public class OssCdnHelper
    {
        public static FileOssCdnReturn GetByOssCdnUrl(string ossCdnUrl, string fileExtension)
         {
            var fileOssCdnReturn = new FileOssCdnReturn
            {
                Success = true 
            };

            try
            {
                if (string.IsNullOrEmpty(ossCdnUrl) || string.IsNullOrEmpty(fileExtension))
                {
                    fileOssCdnReturn.Success = false;
                    fileOssCdnReturn.ErrorMsg = "传入的参数出错:ossCdnUrl或fileExtension为空";
                    return fileOssCdnReturn;
                }

                Uri d = new Uri(ossCdnUrl);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(d);
                req.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
                req.KeepAlive = true;
                req.Method = "GET";
                req.Timeout = 60000;
                using (WebResponse wr = req.GetResponse())
                {
                    var sr = wr.GetResponseStream();
                    byte[] buffer = new byte[1024];
                    int actual = 0;
                    MemoryStream ms = new MemoryStream();
                    while ((actual = sr.Read(buffer, 0, 1024)) > 0)
                    {
                        ms.Write(buffer, 0, actual);
                    }

                    ms.Position = 0;
                    
                    byte[] fileBytes = ms.ToArray();
                    string filename = null;
                    if (wr.Headers["Content-Disposition"] != null)
                    {
                        filename = wr.Headers["Content-Disposition"];
                        filename = filename.Substring(filename.IndexOf("filename=") + 9);
                        if (String.IsNullOrEmpty(Path.GetExtension(filename)))
                        {
                            if (String.IsNullOrEmpty(filename))
                            {
                                filename = Guid.NewGuid().ToString("N") + "." + fileExtension;
                            }
                            else
                            {
                                filename = filename + "." + fileExtension;
                            }
                        }
                    }

                    fileOssCdnReturn.Success = true;
                    fileOssCdnReturn.FileBytes = fileBytes;
                    fileOssCdnReturn.FileName = filename;
                    fileOssCdnReturn.ErrorMsg = string.Empty;
                    return fileOssCdnReturn;
                }
            }
            catch (Exception ex)
            {
                fileOssCdnReturn.Success = false;
                fileOssCdnReturn.FileBytes = null;
                fileOssCdnReturn.FileName = null;
                fileOssCdnReturn.ErrorMsg = ex.Message;

                return fileOssCdnReturn;
            }
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
