using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using Spring.Transaction.Interceptor;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace BD.Jcbg.Bll
{
    public class SmsServiceLczjz : ISmsServiceLczjz
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        #endregion

        #region 发送短消息
        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="receiver"></param>
        /// <param name="content"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool SendMessage(string applicationid, string groupid, string receiver, string contents, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                ret = BdSendMessage(receiver, contents, out msg);


                string sql = "INSERT INTO PR_M_SMS([MessageId],[ApplicationId],[GroupId],[Receiver],[Flag],[Content],[SendTime],[ErrorInfo]) VALUES('" + Guid.NewGuid().ToString() + "','" + applicationid + "','" + groupid + "','" + receiver + "','" + (ret ? "1" : "0") + "','" + contents + "',getdate(),'" + msg + "')";
                CommonDao.ExecCommand(sql, System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        #region 获取轮询需要发送的短信
        private string SmsMessageTemplateCodeV3
        {
            get
            {
                return GetSysSettingValue("SMS_MESSAGE_SETTING_TEMPLATE_CODE_V3");
            }

        }
        /// <summary>
        /// 获取鹿城站名
        /// </summary>
        /// <returns></returns>
        private string SmsMessageCompanyName
        {
            get
            {
                return GetSysSettingValue("SMS_MESSAGE_SETTING_COMPANY_NAME");
            }

        }
        public IList<IDictionary<string, string>> GetSMSdata()
        {
            string sql = "select * from Info_SMS where HasDeal=0";
            return CommonDao.GetDataTable(sql);
        }

        public bool DoSendMessage(string phones, string content, string guid, string lx)
        {
            bool code = false;
            string msg = "";
            try
            {
                if (phones == "")
                {
                    msg = "手机号码不能为空";
                }
                else
                {
                    string[] arrPhones = phones.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string invokeid = SmsBaseSettingInvokeId;
                    string templateid = SmsMessageTemplateCodeV3; //模板id 
                    string companyname = SmsMessageCompanyName;

                    List<string> tempPhones = new List<string>();
                    string[] contentlist = SplitByLen(content, 90);
                    foreach (string strPhone in arrPhones)
                    {
                        if (!strPhone.IsMobile() || tempPhones.Contains(strPhone))
                            continue;
                        SmsRequestMessage3 objMsg = new SmsRequestMessage3()
                        {
                            invokeId = invokeid,
                            phoneNumber = strPhone,
                            templateCode = templateid,
                            contentVar = new SmsVarMessage3()
                            {
                                client = companyname,
                                info1 = contentlist[0],
                                info2 = contentlist[1],
                                info3 = contentlist[2],
                            }
                        };
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string contents = jss.Serialize(objMsg);

                        code = Thread_SendMessage(guid, strPhone, contents, out msg);
                        if (code)
                        {
                            tempPhones.Add(strPhone);
                        }
                    }
                    if (msg == "")
                    {
                        msg = "";
                        code = true;
                    }

                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                code = false;
                msg = ex.Message;
            }
            return code;
        }
        /// <summary>
        /// 轮询发送短消息
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="receiver"></param>
        /// <param name="content"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = false)]
        public bool Thread_SendMessage(string groupid, string receiver, string contents, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                ret = BdSendMessage(receiver, contents, out msg);
                if(groupid!="")
                {
                    string sql = "update Info_SMS set hasdeal=1,sendtime='" + DateTime.Now.ToString() + "' where guid='" + groupid + "'";
                    CommonDao.ExecSql(sql);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }
        /// <summary>
        /// 切割字符串函数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separatorCharNum"></param>
        /// <returns></returns>
        public string[] SplitByLen(string str, int separatorCharNum)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= separatorCharNum)
            {
                return new string[] { str, "", "" };
            }
            string tempStr = str;
            List<string> strList = new List<string>();
            int iMax = Convert.ToInt32(Math.Ceiling(str.Length / (separatorCharNum * 1.0)));//获取循环次数    
            for (int i = 1; i <= iMax; i++)
            {
                string currMsg = tempStr.Substring(0, tempStr.Length > separatorCharNum ? separatorCharNum : tempStr.Length);
                strList.Add(currMsg);
                if (tempStr.Length > separatorCharNum)
                {
                    tempStr = tempStr.Substring(separatorCharNum, tempStr.Length - separatorCharNum);
                }
            }
            if (strList.Count == 1)
            {
                strList.Add("");
                strList.Add("");
            }
            if (strList.Count == 2)
                strList.Add("");
            return strList.ToArray();

        }

        #endregion
        /// <summary>
        /// 百度发短信
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="templateid"></param>
        /// <param name="content"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool BdSendMessage(string receiver, string contents, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {

                System.IO.Stream receiveStream = null;
                System.IO.StreamReader responseReader = null;
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string SecretAccessKey = SmsBaseSettingSecretKey;
                string AccessKeyId = SmsBaseSettingKeyId;
                string authStringPrefix = string.Format("bce-auth-v1/{1}/{0}/1800", timestamp, AccessKeyId);// '这里要改
                //  ' Dim CanonicalRequest As String = "HTTP Method + "\n" + CanonicalURI + "\n" + CanonicalQueryString + "\n" + CanonicalHeaders"
                string CanonicalRequest = string.Format("POST" + "\n" + SmsBaseSettingUrl + "\n" + "\n" + "host:" + SmsBaseSettingDns);
                string SigningKey = GetSigningKeyByHMACSHA256HEX(SecretAccessKey, authStringPrefix);
                string Signature = GetSignatureByHMACSHA256HEX(SigningKey, CanonicalRequest);

                byte[] ContentByte = Encoding.UTF8.GetBytes(contents);
                Uri uri = new Uri("http://" + SmsBaseSettingDns + SmsBaseSettingUrl);
                System.Net.HttpWebRequest HttpWReq = (WebRequest.Create(uri) as System.Net.HttpWebRequest);
                HttpWReq.ContentLength = ContentByte.Length;
                HttpWReq.ContentType = "application/json";
                //HttpWReq.Headers["x-bce-date"] = timestamp;
                string author = string.Format("bce-auth-v1/{2}/{0}/1800/host/{1}", timestamp, Signature, AccessKeyId);
                HttpWReq.Headers["Authorization"] = author;
                HttpWReq.Method = "POST";
                //string sha256 = GetSHA256hash(temp);
                //HttpWReq.Headers["x-bce-content-sha256"] = sha256;
                //HttpWReq.Host = SmsBaseSettingDns;

                //HttpWReq.KeepAlive = false;
                using (Stream StreamData = HttpWReq.GetRequestStream())
                {
                    StreamData.Write(ContentByte, 0, ContentByte.Length);
                }

                using (HttpWebResponse HttpWRes = (System.Net.HttpWebResponse)HttpWReq.GetResponse())
                {
                    if (HttpWRes.Headers.Get("Content-Encoding") == "gzip")
                    {
                        System.IO.Stream zipStream = HttpWRes.GetResponseStream();
                        receiveStream = new GZipStream(zipStream, CompressionMode.Decompress);
                    }
                    else
                    {
                        receiveStream = HttpWRes.GetResponseStream();
                    }
                    responseReader = new System.IO.StreamReader(receiveStream);
                    string responseString = responseReader.ReadToEnd();
                    responseReader.Close();
                    receiveStream.Close();

                    JsonDeSerializer<SmsRet> jds = new JsonDeSerializer<SmsRet>();
                    SmsRet objResp = jds.DeSerializer(responseString, out msg);
                    ret = objResp.code == "1000";
                    if (!ret)
                        msg = objResp.message;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        private string GetSigningKeyByHMACSHA256HEX(String SecretAccessKey, String authStringPrefix)
        {
            HMACSHA256 Livehmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(SecretAccessKey));
            byte[] LiveHash = Livehmacsha256.ComputeHash(Encoding.UTF8.GetBytes(authStringPrefix));
            string SigningKey = HashEncode(LiveHash);
            return SigningKey;
        }

        private string GetSignatureByHMACSHA256HEX(String SigningKey, String CanonicalRequest)
        {
            HMACSHA256 Livehmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(SigningKey));
            byte[] LiveHash = Livehmacsha256.ComputeHash(Encoding.UTF8.GetBytes(CanonicalRequest));
            string Signature = HashEncode(LiveHash);
            return Signature;
        }

        // '将字符串全部变成小写。
        private string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private string GetSHA256hash(string input)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(input);
            SHA256 sha256 = new SHA256Managed();
            sha256.ComputeHash(clearBytes);
            byte[] hashedBytes = sha256.Hash;
            sha256.Clear();
            string output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return output;
        }

        [DataContract]
        private class SmsRet
        {
            [DataMember]
            public string code { get; set; }
            [DataMember]
            public string message { get; set; }
            [DataMember]
            public string requestId { get; set; }

        }


        #endregion

        #region 系统变量

        private static IList<IDictionary<string, string>> m_SysVariables = null;
        private string GetSysSettingValue(string key)
        {
            string ret = "";
            try
            {
                if (m_SysVariables == null)
                    LoadSysVariables();
                key = key.ToLower();

                var q = from e in m_SysVariables where e["settingcode"].Equals(key, StringComparison.OrdinalIgnoreCase) && e["istemplate"].Equals("False") && e["companycode"] == "" select e;
                if (q.Count() > 0)
                    ret = q.First()["settingvalue"];

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                //ret = e.Message;
            }
            return ret;
        }

        private void LoadSysVariables()
        {
            try
            {
                m_SysVariables = CommonDao.GetDataTable("select * from syssetting");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        private string SmsBaseSettingDns
        {
            get
            {
                return GetSysSettingValue("SMS_BASE_SETTING_DNS");
            }
        }

        private string SmsBaseSettingUrl
        {
            get { return GetSysSettingValue("SMS_BASE_SETTING_URL"); }
        }

        private string SmsBaseSettingInvokeId
        {
            get { return GetSysSettingValue("SMS_BASE_SETTING_INVOKE_ID"); }
        }

        private string SmsBaseSettingKeyId
        {
            get { return GetSysSettingValue("SMS_BASE_SETTING_KEY_ID"); }
        }
        private string SmsBaseSettingSecretKey
        {
            get { return GetSysSettingValue("SMS_BASE_SETTING_SECRET_KEY"); }
        }
        #endregion
    }
}
