using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.IBll;
using BD.Jcbg.Common;
using BD.Jcbg.IDao;
using BD.Jcbg.DataModal.Entities;
using Spring.Transaction.Interceptor;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml;

namespace BD.Jcbg.Bll
{
    public class SmsServiceWzzjz : ISmsServiceWzzjz
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        #endregion

        public string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }

        #region 老的短信机
        [Transaction(ReadOnly = false)]
        public bool SendMessage(string applicationid, string groupid, string receiver, string contents, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                object[] args = new object[2];
                args[0] = "wz077951";
                args[1] = GetMsg(receiver, contents);
                object returnmsg = DynamicWebService.InvokeWebService("", "sendmsg", "sendmsg_add", args, "http://111.1.14.18/webservice/services/sendmsg");

                ret = GetReturnMessage(returnmsg.GetSafeString(), out msg);

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

        private string GetMsg(string phone, string msg)
        {
            string ret = "";
            XmlDocument doc = new XmlDocument();
            // 申明
            XmlNode decl = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(decl);
            // 跟节点
            XmlNode root = doc.CreateElement("infos");
            doc.AppendChild(root);
            // 信息节点
            XmlNode info = doc.CreateElement("info");
            root.AppendChild(info);
            // 其他节点
            string password = phone.Substring(7);
            int npwd = (Convert.ToInt32(password) * 3 + 1658);
            password = npwd.ToString();

            XmlNode node = doc.CreateNode(XmlNodeType.Element, "msg_id", null);
            //node.InnerXml = "<![CDATA[1658]]>";
            node.InnerXml = "<![CDATA[-1]]>";
            info.AppendChild(node);
            node = doc.CreateNode(XmlNodeType.Element, "password", null);
            node.InnerXml = "<![CDATA[" + password + "]]>";
            info.AppendChild(node);
            node = doc.CreateNode(XmlNodeType.Element, "src_tele_num", null);
            node.InnerXml = "<![CDATA[106575256729]]>";
            info.AppendChild(node);
            node = doc.CreateNode(XmlNodeType.Element, "dest_tele_num", null);
            node.InnerXml = "<![CDATA[" + phone + "]]>";
            info.AppendChild(node);
            node = doc.CreateNode(XmlNodeType.Element, "msg", null);
            node.InnerXml = "<![CDATA[" + msg + "]]>";
            info.AppendChild(node);

            ret = ConvertXmlToString(doc);
            return ret;
        }

        

        protected bool GetReturnMessage(string content, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                XDocument doc = XDocument.Parse(content);
                var q = from e in doc.Elements("infos").Elements("info") select e;
                XElement eleInfo = q.First();
                int state = eleInfo.Element("state").Value.GetSafeInt(99);
                code = state == 0 ? true : false;
                if (code)
                    msg = eleInfo.Element("msg_id").Value;
                else
                {
                    switch (state)
                    {
                        case -1:
                            msg = "企业帐号错误";
                            break;
                        case -2:
                            msg = "验证码格式错误";
                            break;
                        case -3:
                            msg = "接入号即服务代码错误";
                            break;
                        case -4:
                            msg = "手机号码错误";
                            break;
                        case -5:
                            msg = "消息为空";
                            break;
                        case -6:
                            msg = "消息太长";
                            break;
                        case -7:
                            msg = "验证码不匹配";
                            break;
                        default:
                            msg = state.ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = content;// ex.Message;
            }
            return code;

        }

        #endregion


        #region 新的短信机20190329
        [Transaction(ReadOnly = false)]
        public bool SendMessageV2(string applicationid, string groupid, string receiver, string contents, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                object[] args = new object[1];
                args[0] = GetMsgV2(receiver, contents); 
                object returnmsg = DynamicWebService.InvokeWebServiceWithSchemas("", "WsSmsServiceService", "sendSms", args, "http://112.35.10.201:1999/smsservice");

                ret = GetReturnMessageV2(returnmsg.GetSafeString(), out msg);

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

        private string GetMsgV2(string phone, string msg)
        {
            string ret = "";
            string ecName = "温州市建设工程质量监督站";
            string apId = "wzzjz";
            string secretKey = "bdwry!@#$%^";
            string content = msg;
            string sign = "lSv1VqGgk";
            string addSerial = "";
            string mac = "";
            XmlDocument doc = new XmlDocument();
            // 申明
            XmlNode decl = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(decl);
            // 根节点
            XmlNode root = doc.CreateElement("WsSubmitReq");
            doc.AppendChild(root);

            // apid
            XmlNode node = doc.CreateElement("apId");
            node.InnerXml = apId;
            root.AppendChild(node);
            // secretKey
            node = doc.CreateElement("secretKey");
            node.InnerXml = secretKey;
            root.AppendChild(node);
            // ecName
            node = doc.CreateElement("ecName");
            node.InnerXml = ecName;
            root.AppendChild(node);
            // mobiles
            node = doc.CreateElement("mobiles");
            node.InnerXml = "<string>" + phone + "</string>";
            root.AppendChild(node);
            // content
            node = doc.CreateElement("content");
            node.InnerXml = content;
            root.AppendChild(node);
            // sign
            node = doc.CreateElement("sign");
            node.InnerXml = sign;
            root.AppendChild(node);
            // addSerial
            node = doc.CreateElement("addSerial");
            node.InnerXml = addSerial;
            root.AppendChild(node);

            // mac
            mac = MD5Util.StringToMD5Hash(ecName + apId + secretKey + phone + content + sign + addSerial);
            node = doc.CreateElement("mac");
            node.InnerXml = mac;
            root.AppendChild(node);


            ret = ConvertXmlToString(doc);
            return ret;
        }

        protected bool GetReturnMessageV2(string content, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                XDocument doc = XDocument.Parse(content);
                var q = from e in doc.Elements("SendSmsResponse") select e;
                XElement eleInfo = q.First();
                string state = eleInfo.Element("success").Value.GetSafeString();
                string rspcod = eleInfo.Element("rspcod").Value.GetSafeString();
                code = state == "true" ? true : false;
                if (code)
                    msg = state;
                else
                {
                    switch (state)
                    {
                        case "InvalidMessage":
                            msg = "非法消息，缺少必要参数";
                            break;
                        case "InvalidUsrOrPwd":
                            msg = "非法用户名或密码";
                            break;
                        case "IllegalSignId":
                            msg = "无效的签名";
                            break;
                        case "TooManyMobiles":
                            msg = "手机号超出最大上限（5000）";
                            break;
                        default:
                            msg = state.ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = content;// ex.Message;
            }
            return code;

        }
        #endregion


    }
}
