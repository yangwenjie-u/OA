using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace BD.Jcbg.Common
{
    public static class MyHttp
    {
        public static string NULL_KEY = "__NULL__";
        public static bool Post(string url, IDictionary<string, string> datas, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";
                // 参数经过URL编码
                StringBuilder sb = new StringBuilder();
                foreach (string key in datas.Keys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    if (key.Equals(NULL_KEY, StringComparison.OrdinalIgnoreCase))
                        sb.Append(System.Web.HttpUtility.UrlEncode(datas[key]));
                    else
                        sb.Append(System.Web.HttpUtility.UrlEncode(key) + "=" + System.Web.HttpUtility.UrlEncode(datas[key]));
                }
                byte[] arrParams;
                //将URL编码后的字符串转化为字节
                arrParams = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                //设置请求的 ContentLength 
                request.ContentLength = arrParams.Length;
                //获得请 求流
                System.IO.Stream writer = request.GetRequestStream();
                //将请求参数写入流
                writer.Write(arrParams, 0, arrParams.Length);
                // 关闭请求流
                writer.Close();
                System.Net.HttpWebResponse response;
                // 获得响应流
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                msg = myreader.ReadToEnd();
                myreader.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        public static bool Post(string url, string jsonbody, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                SysLog4.WriteLog("地址：" + url + ",参数:" + jsonbody);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                Encoding encoding = Encoding.UTF8;
                byte[] RequestBytes = encoding.GetBytes(jsonbody);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = RequestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(RequestBytes, 0, RequestBytes.Length);
                requestStream.Close();

                DateTime dt1 = DateTime.Now;
                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                {
                    StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    msg = sr.ReadToEnd();
                    //SysLog.WriteError(msg);
                    sr.Close();
                }
                SysLog4.WriteLog("返回："+msg);
                ret = true;

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }

        public static byte[] GetDownFile(string url, IDictionary<string, string> datas, out string msg)
        {
            msg = "";
            byte[] ret = null;
            try
            {

                // 参数经过URL编码
                StringBuilder sb = new StringBuilder();
                foreach (string key in datas.Keys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    if (key.Equals(NULL_KEY, StringComparison.OrdinalIgnoreCase))
                        sb.Append(System.Web.HttpUtility.UrlEncode(datas[key]));
                    else
                        sb.Append(System.Web.HttpUtility.UrlEncode(key) + "=" + System.Web.HttpUtility.UrlEncode(datas[key]));
                }
                if (sb.Length > 0)
                    url = url + "?" + sb.ToString();
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();

                List<byte> filecontent = new List<byte>();
                byte[] buff = new byte[1024];
                int size = stream.Read(buff, 0, (int)buff.Length);
                while (size > 0)
                {
                    foreach (byte by in buff)
                        filecontent.Add(by);
                    size = stream.Read(buff, 0, (int)buff.Length);
                }

                ret = filecontent.ToArray();
                stream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 向指定的url发送post请求
        /// </summary>
        /// <param name="url">指定的url</param>
        /// <param name="datas">
        /// 传递的参数，键值对，
        /// 键：参数名
        /// 值：参数值
        /// 如：key=value
        /// </param>
        /// <param name="files">
        /// 传递的文件数据，键值对
        /// 键：表单控件名称
        /// 值：文件信息（支持多个文件）
        /// 每个文件的信息是一个{文件名：文件二进制数据}键值对
        /// </param>
        /// <param name="msg">
        /// 调用Post之后，返回的数据包字符串（post请求对应的响应）
        /// </param>
        /// <returns>是否调用成功</returns>
        public static bool Post(string url, Dictionary<string,string> datas, IDictionary<string, Dictionary<string,byte[]>> files, out string msg)
        {
            bool ret = false;
            msg = "";                       
            try
            {
                MemoryStream ms = new MemoryStream();
                // 1.分界线
                string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x"));       // 分界线可以自定义参数
                string beginBoundary = string.Format("--{0}", boundary);
                string middleBoundary = string.Format("\r\n--{0}", boundary);
                string endBoundary = string.Format("\r\n--{0}--", boundary);
                byte[] beginBoundaryBytes = Encoding.UTF8.GetBytes(beginBoundary);
                byte[] middleBoundaryBytes = Encoding.UTF8.GetBytes(middleBoundary);
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes(endBoundary);
                List<byte[]> bl = new List<byte[]>();

                // 2.组装开始分界线数据体 到内存流中
                ms.Write(beginBoundaryBytes, 0, beginBoundaryBytes.Length);

                // 3.组装参数
                if (datas != null && datas.Count > 0)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in datas)
                    {
                        string parameterHeaderTemplate = string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", keyValuePair.Key, keyValuePair.Value);
                        byte[] parameterHeaderBytes = Encoding.UTF8.GetBytes(parameterHeaderTemplate);
                        bl.Add(parameterHeaderBytes);
                        //ms.Write(parameterHeaderBytes, 0, parameterHeaderBytes.Length);
                    }
                    
                }
                // 4.组装文件数据体
                if (files != null && files.Count > 0)
                {
                    //SysLog4.WriteError("文件field数量：" + files.Count.ToString());
                    foreach (KeyValuePair<string, Dictionary<string,byte[]>> f in files)
                    {
                        string fieldname = f.Key;
                        Dictionary<string, byte[]> filelist = f.Value;
                        if (filelist !=null && filelist.Count > 0)
                        {
                            //SysLog4.WriteError("每个field的文件数量：" +filelist.Count.ToString());
                            foreach (var file in filelist)
                            {
                                if (file.Value.Length > 0)
                                {
                                    // 组装文件头
                                    string fileHeaderTemplate = string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", fieldname, file.Key);
                                    byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeaderTemplate);
                                    byte[] c = new byte[fileHeaderBytes.Length + file.Value.Length];
                                    fileHeaderBytes.CopyTo(c, 0);
                                    file.Value.CopyTo(c, fileHeaderBytes.Length);
                                    bl.Add(c);                                                                    
                                }                                
                            }

                        }
                          
                    }
                }
                // 写入每个参数和文件数据
                if (bl.Count > 0)
                {
                    for (int i = 0; i < bl.Count; i++)
                    {
                        ms.Write(bl[i], 0, bl[i].Length);
                        if (i < bl.Count -1)
                        {
                            ms.Write(middleBoundaryBytes, 0, middleBoundaryBytes.Length);
                        }
                    }
                }

                // 组装结束分界线数据体 到内存流中
                ms.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                // 5.获取二进制数据
                byte[] postBytes = ms.ToArray();
                // 6.HttpWebRequest 组装
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url, UriKind.RelativeOrAbsolute));
                webRequest.Method = "POST";
                webRequest.Timeout = 1000 * 3600;
                webRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                webRequest.ContentLength = postBytes.Length;
                // 7.写入上传请求数据
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();
                //SysLog4.WriteError(Encoding.UTF8.GetString(postBytes));
                // 8.获取响应
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                string body = reader.ReadToEnd();
                reader.Close();
                msg = body;
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }


        public static CookieContainer cookie = new CookieContainer();
        public static bool PostAli(string url, IDictionary<string, string> datas, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookie;
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";
                // 参数经过URL编码
                StringBuilder sb = new StringBuilder();
                foreach (string key in datas.Keys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    if (key.Equals(NULL_KEY, StringComparison.OrdinalIgnoreCase))
                        sb.Append(System.Web.HttpUtility.UrlEncode(datas[key]));
                    else
                        sb.Append(System.Web.HttpUtility.UrlEncode(key) + "=" + System.Web.HttpUtility.UrlEncode(datas[key]));
                }
                byte[] arrParams;
                //将URL编码后的字符串转化为字节
                arrParams = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                //设置请求的 ContentLength 
                request.ContentLength = arrParams.Length;
                //获得请 求流
                System.IO.Stream writer = request.GetRequestStream();
                //将请求参数写入流
                writer.Write(arrParams, 0, arrParams.Length);
                // 关闭请求流
                writer.Close();
                System.Net.HttpWebResponse response;
                // 获得响应流
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                msg = myreader.ReadToEnd();
                myreader.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }


        public static string SendDataByPost(string Url, string datas)
        {
            string retString = "";
            try
            {
                // https请求
                if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.UTF8.GetBytes(datas);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    retString = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }


        public static string SendDataByGET(string Url)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);

            }
            return retString;
        }

        /// <summary>
        /// 模拟发送POST请求
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="data"></param>
        /// <param name="postHeaders"></param>
        /// <param name="contentType"></param>
        public static string SendPOST(string apiUrl, string data, Dictionary<string, string> postHeaders)
        {
            string result = string.Empty;
            int http_StatusCode = 0;
            string http_ResponseMessage = null;
            try
            {
                //注意提交的编码 这边是需要改变的 这边默认的是Default：系统当前编码
                byte[] postData = Encoding.UTF8.GetBytes(data);
                // 忽略SSL证书，防止“未能为 SSL/TLS 安全通道建立信任关系”的报错
                ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;
                // 设置提交的相关参数 
                HttpWebRequest request = WebRequest.Create(apiUrl) as HttpWebRequest;
                //Encoding myEncoding = Encoding.UTF8;
                request.Method = "POST";
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                //遍历字典
                foreach (KeyValuePair<string, string> header in postHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.ContentType = "application/json";

                // 提交请求数据 
                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                HttpWebResponse response = null;
                Stream responseStream = null;
                StreamReader reader = null;
                string srcString;
                response = request.GetResponse() as HttpWebResponse;
                // Statuscode 为枚举类型，200为正常，其他输出异常，需要转为int型才会输出状态码
                http_StatusCode = Convert.ToInt32(response.StatusCode);
                http_ResponseMessage = response.StatusCode.ToString();

                if (200 == http_StatusCode)
                {
                    responseStream = response.GetResponseStream();
                    reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                    srcString = reader.ReadToEnd();
                    result = srcString;   //返回值赋值
                    reader.Close();
                }
                else
                {
                    throw new Exception("网络请求异常：HTTP响应码： " + http_StatusCode + ", HTTP响应信息： " + http_ResponseMessage);
                }
            }
            catch (WebException ex)
            {
                throw new Exception("网络请求时发生异常：" + ex.Message);
            }
            return result;
        }
    }
}
