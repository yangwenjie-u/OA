using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Service.XsXhWebService;
using System.Xml;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Service.Jc
{
    /// <summary>
    /// 2019-07-03
    /// 杨鑫钢
    /// 用于萧山协会服务接口
    /// </summary>
    public class XsXhService : IBaseService
    {
        #region 属性
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// 接口服务类
        /// </summary>
        private static QrpwsImpService service = new QrpwsImpService();

        //deviceService.Url = ConfigUtil.WebServiceUrl;
        #endregion

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="dic"></param>
        /// 机构ID：jgid
        /// 报告编号: bgbh
        /// 是否两块两材：type 0表示非两块两材 1表示两块两材
        /// <returns></returns>
        public ResultParam GetQrCode(IDictionary<string, string> dic)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //获取数据
                string jgid = dic["jgid"].GetSafeString();
                string bgbh = dic["bgbh"].GetSafeString();
                string type = dic["type"].GetSafeString();
                //判断是非两块两材还是两块两材
                string xml = "";
                switch (type)
                {
                    //非两块两材
                    case "0":
                        xml = service.preGetNotLklcQRCodeStr(jgid, bgbh);
                        break;
                    //两块两材
                    case "1":
                        xml = service.preGetQrCodeString(jgid, bgbh, 1);
                        break;
                    //其他
                    default:
                        ret.msg = String.Format("报告编号：{0}对应类别无效！", bgbh);
                        return ret;
                }
                //分析XML
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                //获取点值值
                XmlNode codeNode = doc.SelectSingleNode("/xml/code");
                if (codeNode == null)
                {
                    ret.msg = "XML格式不正确！";
                    return ret;
                }
                //判断是否正确
                if (codeNode.InnerText != "ok")
                {
                    codeNode = doc.SelectSingleNode("/xml/msg");
                    ret.msg = codeNode.InnerText;
                    return ret;
                }
                //具体二维码值
                XmlNode qrNode = doc.SelectSingleNode("/xml/qrcodestr");
                if (qrNode == null)
                {
                    ret.msg = "XML格式不正确！";
                    return ret;
                }
                ret.data = qrNode.InnerText;
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
                SysLog4.WriteError(String.Format("获取二维码出错，原因：{0}，参数：{1}", ex.Message, jss.Serialize(dic)));
            }        
            return ret;
        }

        /// <summary>
        /// 上传非两块两材数据
        /// </summary>
        /// <param name="jgid">机构ID</param>
        /// <param name="jcqy">机构区域</param>
        /// <param name="xml">XML数据包</param>
        /// <returns></returns>
        public ResultParam UploadFeiLiangKuai(IDictionary<string, string> dic)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //获取参数
                string jgid = dic["jgid"];
                string jgqy = dic["jgqy"];
                string basic = dic["basic"];
                string xml = service.afterUpNotLklcData(jgid, jgqy, basic, "", 1);
                //分析XML
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                //获取点值值
                XmlNode codeNode = doc.SelectSingleNode("/xml/code");
                if (codeNode == null)
                {
                    SysLog4.WriteError(String.Format("XML格式不正确！{0}", xml));
                    ret.msg = "XML格式不正确！";
                    return ret;
                }
                //判断是否正确
                if (codeNode.InnerText != "ok")
                {
                    codeNode = doc.SelectSingleNode("/xml/msg");
                    ret.msg = codeNode.InnerText;
                    return ret;
                }

                ret.msg = "上传成功！";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
                SysLog4.WriteError(String.Format("上传非两块两材出错，原因：{0}，参数：{1}", ex.Message, jss.Serialize(dic)));
            }
            return ret;
        }

        /// <summary>
        /// 上传两块两材
        /// </summary>
        /// <param name="jgid">机构ID</param>
        /// <param name="jgqy">机构区域</param>
        /// <param name="code">编号</param>
        /// <param name="xml">数据XML</param>
        /// <param name="jpjcXml">样品XML</param>
        /// <returns></returns>
        public ResultParam UploadLiangKuai(IDictionary<string, string> dic)
        {
            ResultParam ret = new ResultParam();
            try
            {
                //获取参数
                string jgid = dic["jgid"];
                string jgqy = dic["jgqy"];
                string code = dic["code"];
                string basic = dic["basic"];
                string sample = dic["sample"];

                string xml = service.afterUploadJcDataForLklc(jgid, jgqy, code, basic, sample, "0");
                //分析XML
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                //获取点值值
                XmlNode codeNode = doc.SelectSingleNode("/xml/code");
                if (codeNode == null)
                {
                    SysLog4.WriteError(String.Format("XML格式不正确！{0}", xml));
                    ret.msg = "XML格式不正确！";
                    return ret;
                }
                //判断是否正确
                if (codeNode.InnerText != "ok")
                {
                    codeNode = doc.SelectSingleNode("/xml/msg");
                    ret.msg = codeNode.InnerText;
                    return ret;
                }

                ret.msg = "上传成功！";
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
                SysLog4.WriteError(String.Format("上传两块两材出错，原因：{0}，参数：{1}", ex.Message, jss.Serialize(dic)));
            }
            return ret;
        }
    }
}
