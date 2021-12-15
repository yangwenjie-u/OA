using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Drawing;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Web.Func
{
    public static class GlobalVariable
    {
        #region 服务
        private static ICommonService _commonService = null;
        private static ICommonService CommonService
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

        #region 基础函数
        private static IList<IDictionary<string, string>> m_SysVariables = null;
        private static IList<IDictionary<string, string>> m_DnsVariables = null;

        public static string GetSysSettingValue(bool isGlobal, string key)
        {
            string ret = "";
            try
            {
                if (m_SysVariables == null || m_SysVariables.Count == 0)
                    LoadSysVariables();
                key = key.ToLower();
                if (isGlobal)
                {
                    var q = from e in m_SysVariables where e["settingcode"].Equals(key, StringComparison.OrdinalIgnoreCase) && e["istemplate"].Equals("False") && e["companycode"] == "" select e;
                    if (q.Count() > 0)
                        ret = q.First()["settingvalue"];
                }
                else
                {
                    var q = from e in m_SysVariables where e["settingcode"].Equals(key, StringComparison.OrdinalIgnoreCase) && e["istemplate"].Equals("False") && CurrentUser.IsLogin && e["companycode"].Equals(CurrentUser.CompanyCode) select e;
                    if (q.Count() > 0)
                        ret = q.First()["settingvalue"];
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                //ret = e.Message;
            }
            return ret;
        }

        public static string GetSysSettingValue(string key, HttpRequestBase request)
        {
            string ret = "";
            try
            {
                if (m_SysVariables == null || m_SysVariables.Count == 0)
                    LoadSysVariables();

                string dns = ClientInfo.GetCurDnsWithPort(request);
                var dnsSettings = m_DnsVariables.Where(e => e["settingcode"].Equals(key, StringComparison.OrdinalIgnoreCase) && e["dns"].Equals(dns, StringComparison.OrdinalIgnoreCase));
                if (dnsSettings.Count() > 0)
                    ret = dnsSettings.ElementAt(0)["settingvalue"].GetSafeString();
                else
                {

                    var q = from e in m_SysVariables where e["settingcode"].Equals(key, StringComparison.OrdinalIgnoreCase) && e["istemplate"].Equals("False") && e["companycode"] == "" select e;
                    if (q.Count() > 0)
                        ret = q.First()["settingvalue"];
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                //ret = e.Message;
            }
            return ret;
        }

        private static void LoadSysVariables()
        {
            try
            {
                string syssetting = "syssetting";
                try
                {
                    syssetting = Configs.GetConfigItem("syssetting");
                }
                catch (Exception e)
                {

                }
                if (syssetting == "")
                    syssetting = "syssetting";
                m_SysVariables = CommonService.GetDataTable("select * from " + syssetting);
                m_DnsVariables = CommonService.GetDataTable("select * from syssettingdns");
                //因全局参数是在Web项目中的，在业务层Service无法获取，现在Common中也引用同一个静态变量
                GlobalVariableConfig.m_SysVariables = m_SysVariables;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }

        public static void ReloadSysVariables()
        {
            LoadSysVariables();
        }
        #endregion

        #region 报告二维码设置
        /// <summary>
        /// 获取二维码左上角点
        /// </summary>
        /// <returns></returns>
        public static Point GetReportBarcodeLT()
        {
            Point ret = Point.Empty;
            try
            {
                string str = GetSysSettingValue(true, "GLOBAL_UPDATA_BARCODE_POS");
                string[] arr = str.Split(new char[] { ',', '，' });
                if (arr.Length > 0)
                    ret.X = arr[0].GetSafeInt();
                if (arr.Length > 1)
                    ret.Y = arr[1].GetSafeInt();
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 加二维码的页设置（1~N为第n页，0为所有页）
        /// </summary>
        /// <returns></returns>
        public static string GetReportBarcodePage()
        {
            string ret = "0";
            try
            {
                ret = GetSysSettingValue(true, "GLOBAL_UPDATA_BARCODE_PAGEMODEL");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取二维码宽度
        /// </summary>
        /// <returns></returns>
        public static int GetReportBarcodeWidth()
        {
            int ret = 80;
            try
            {
                ret = GetSysSettingValue(true, "GLOBAL_UPDATA_BARCODE_SIZE").GetSafeInt(80);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        
        #endregion

        #region 基础短信设置
        public static string GetSmsBaseAppId(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_BASE_SETTING_APP_ID");
            return GetSysSettingValue("SMS_BASE_SETTING_APP_ID", request);
        }
        public static string GetSmsBaseInvokeId(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_BASE_SETTING_INVOKE_ID");
            return GetSysSettingValue("SMS_BASE_SETTING_INVOKE_ID", request);
            
        }
        #endregion

        #region 注册短信设置
        public static string GetSmsRegisterTemplateCode(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_REGISTER_SETTING_TEMPLATE_CODE");
            return GetSysSettingValue("SMS_REGISTER_SETTING_TEMPLATE_CODE", request);
        }

        public static int GetSmsRegisterVerifyCodeLength(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_REGISTER_SETTING_VERIFY_CODE_LENGTH").GetSafeInt(60);
            return GetSysSettingValue("SMS_REGISTER_SETTING_VERIFY_CODE_LENGTH", request).GetSafeInt(60);
            
        }
        public static int GetSmsRegisterVerifyCodeSeconds(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_REGISTER_SETTING_VERIFY_CODE_SECONDS").GetSafeInt(60);
            return GetSysSettingValue("SMS_REGISTER_SETTING_VERIFY_CODE_SECONDS", request).GetSafeInt(60);
        }
        public static int GetSmsRegisterVerifyCodeMinSpan(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_REGISTER_SETTING_VERIFY_CODE_MIN_SPAN").GetSafeInt(60);
            return GetSysSettingValue("SMS_REGISTER_SETTING_VERIFY_CODE_MIN_SPAN", request).GetSafeInt(60);
        }
        public static string GetSmsRegisterTemplateCodeUserInfo(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "SMS_REGISTER_SETTING_TEMPLATE_CODE_USERINFO");
            return GetSysSettingValue("SMS_REGISTER_SETTING_TEMPLATE_CODE_USERINFO", request);
            
        }
        public static int GetSmsPayVerifyCodeLength(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "OTHER_SETTING_PAY_CODE_LENGTH").GetSafeInt(6);
            return GetSysSettingValue("OTHER_SETTING_PAY_CODE_LENGTH", request).GetSafeInt(6);
        }
        public static string GetSmsPayVerifyCodeTemplate(HttpRequestBase request = null)
        {
            if (request == null)
                return GetSysSettingValue(true, "OTHER_SETTING_PAY_TMPLATE");
            return GetSysSettingValue("OTHER_SETTING_PAY_TMPLATE", request);
        }
        #endregion

        #region 修改密码短信设置
        public static string GetSmsPasswordChangeTemplateCodeUserInfo()
        {
            return GetSysSettingValue(true, "SMS_PASSWORDCHANGE_SETTING_TEMPLATE_CODE_USERINFO");
        }
        #endregion

        #region 重置密码短信设置
        public static string GetSmsPasswordResetTemplateCodeUserInfo()
        {
            return GetSysSettingValue(true, "SMS_PASSWORDRESET_SETTING_TEMPLATE_CODE_USERINFO");
        }
        #endregion

        #region 通知短信
        public static string GetSmsMessageTemplateCode()
        {
            return GetSysSettingValue(true, "SMS_MESSAGE_SETTING_TEMPLATE_CODE");
        }
        public static string GetSmsMessageCompanyName()
        {
            return GetSysSettingValue(true, "SMS_MESSAGE_SETTING_COMPANY_NAME");
        }
        /// <summary>
        /// 见证取样通知短信模板
        /// </summary>
        /// <returns></returns>
        public static string GetSmsMessageTemplateCodeJzqy()
        {
            return GetSysSettingValue(true, "SMS_MESSAGE_SETTING_TEMPLATE_CODE_JZQY");
        }
        #endregion

        #region 用户设置
        public static int GetUserPasswordLength()
        {
            return GetSysSettingValue(true, "USER_SETTING_PASSWORD_LENGTH").GetSafeInt(6);
        }
        public static bool GetPermitRegister()
        {
            return GetSysSettingValue(true, "GLOBAL_PAGE_LOGIN_REGISTER").GetSafeInt() == 1 ? true : false;
        }
        public static string GetDefaultUserPass()
        {
            return GetSysSettingValue(true, "USER_SETTING_DEFAULTPASS");
        }

        /// <summary>
        /// 金成龙--2017-08-31
        /// 获取默认的短信发送模块ID
        /// 如果找不到配置项，返回百度短息模块ID（ID=0）
        /// ID=0 : 百度短信模块
        /// ID=1 : 温州市建设工程质量监督站短信模块
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultMessageSenderID(HttpRequestBase request = null)
        {
            int ID = 0;
            string senderID = "";
            if (request == null)
                senderID = GetSysSettingValue(true, "SMS_BASE_SETTING_DEFAULT_MESSAGESENDER");
            else
                senderID = GetSysSettingValue("SMS_BASE_SETTING_DEFAULT_MESSAGESENDER", request);
            if (senderID != null && senderID!="" )
            {
                ID = senderID.GetSafeInt();
            }
            return ID;
        }


        /// <summary>
        /// 判断config是否有这个节点，设置默认值
        /// </summary>
        /// <param name="itemvalue"></param>
        /// <returns></returns>
        public static string GetConfigValue(string itemvalue)
        {
            string value = "false";
            try
            {
                value = Configs.GetConfigItem(itemvalue);
            }
            catch(Exception e)
            {

            }
            return value;
        }
        #endregion

        #region 登录页面

        #endregion

        #region 短信模板

        #region 温州市建设工程监督站
        private static IList<IDictionary<string, string>> m_WzzjzSmsTemplates = null;
        private static void LoadWzzjzSmsTemplates()
        {
            try
            {
                m_WzzjzSmsTemplates = CommonService.GetDataTable("select * from sys_sms_template_wzzjz");
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
        }


        public static void ReloadWzzjzSmsTemplates()
        {
            LoadWzzjzSmsTemplates();
        }


        public static string GetWzzjzSmsTemplate(string key)
        {
            string ret = "";
            try
            {
                if (m_WzzjzSmsTemplates == null)
                    LoadWzzjzSmsTemplates();
                key = key.ToLower();
                var q = from e in m_WzzjzSmsTemplates where e["lx"].Equals(key, StringComparison.OrdinalIgnoreCase) select e;
                if (q.Count() > 0)
                    ret = q.First()["templatecontent"];
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        #endregion


        #endregion

        #region 远程设置
        public static string GetRemoteSettingBaDns()
        {
            return "http://" + GetSysSettingValue(true, "REMOTE_SETTING_BA_DNS") + "/";
        }

        #endregion

        #region 电子签章

        #endregion
		#region 其他设置
        /// <summary>
        /// 合同项目设置
        /// </summary>
        /// <returns></returns>
        public static bool GetHtxmIsXm()
        {
            return GetSysSettingValue(true, "OTHER_SETTING_HTXMLX").GetSafeInt() == 0;
        }

        public static string GetApiEncryptKey()
        {
            return GetSysSettingValue(true, "API_ENCRYPT_KEY").GetSafeString();
        }

        public static int GetApiSessionExpireHours()
        {
            return GetSysSettingValue(true, "API_SESSION_TIMEOUT").GetSafeInt();
        }
        /// <summary>
        /// 是否启用内部合同
        /// </summary>
        /// <returns></returns>
        public static bool UseNbht()
        {
            return GetSysSettingValue(true, "OTHER_SETTING_USE_NBHT").GetSafeInt() == 1;
        }
        /// <summary>
        /// 是否启用合同监管
        /// </summary>
        /// <returns></returns>
        public static bool UseHtjg()
        {
            return GetSysSettingValue(true, "OTHER_SETTING_USE_HT").GetSafeInt() == 1;
        }

        /// <summary>
        /// 默认地图标注抬头
        /// </summary>
        public static string DefaultMapTitle
        {
            get
            {
                return GetSysSettingValue(true, "MAP_SETTING_SELFTITLE");
            }
        }
        /// <summary>
        /// 默认地图标注点
        /// </summary>
        public static string DefaultMapPos
        {
            get
            {
                return GetSysSettingValue(true, "MAP_SETTING_SELFPOS");
            }
        }
        public static bool RySfzEncode
        {
            get
            {
                return GetSysSettingValue(true, "OTHER_SETTING_RY_SFZ_ENCODE") == "1";
            }
        }
        /// <summary>
        /// 支付平台des加密的key
        /// </summary>
        public static string WGRYPAY_KEY
        {
            get
            {
                return GetSysSettingValue(true, "WGRYPAY_KEY");
            }
        }

        /// <summary>
        /// 获取服务类型
        /// </summary>
        /// <returns></returns>
        public static string GLOBAL_SERVICE_GENERATE
        {
            get
            {
                return GetSysSettingValue(true, "GLOBAL_SERVICE_GENERATE");
            }
        }
        #endregion

        #region 登录短信验证
        public static bool LoginSmsVerify(HttpRequestBase request)
        {
            return GetSysSettingValue("GLOBAL_PAGE_LOGIN_SMSCODE", request) == "1";
        }
        public static int LoginSmsValidMinutes(HttpRequestBase request)
        {
            return GetSysSettingValue("SMS_LOGIN_SETTING_VALID_MINUTES", request).GetSafeInt(1);
        }
        public static int LoginSmsLength(HttpRequestBase request)
        {
            return GetSysSettingValue("SMS_LOGIN_SETTING_CODE_LENGTH", request).GetSafeInt(6);
        }
        public static string LoginSmsTemplate(HttpRequestBase request)
        {
            return GetSysSettingValue("SMS_LOGIN_SETTING_TEMPLATE_CODE", request);
        }
        #endregion
    }
}