using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 全局变量配置类
    /// </summary>
    public class GlobalVariableConfig
    {
        #region 属性
        /// <summary>
        /// 系统变量
        /// </summary>
        public static IList<IDictionary<string, string>> m_SysVariables = null;
        #endregion

        #region 函数
        /// <summary>
        /// 现场项目APP开启试验,启动抓拍
        /// </summary>
        public static bool GLOBAL_CAMERASNAP_AUTO
        {
            get
            {
                return GetSysSettingValue(true, "GLOBAL_CAMERASNAP_AUTO").GetSafeString() == "1" ? true : false;
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

        /// <summary>
        /// 接口返回信息（英文：EN，中文：CN，默认为：中文）
        /// </summary>
        /// <returns></returns>
        public static string GLOBAL_INTERFACE_CNEN
        {
            get
            {
                return GetSysSettingValue(true, "GLOBAL_INTERFACE_CNEN");
            }
        }

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
        #endregion

        #region 通用
        /// <summary>
        /// 获取设置参数
        /// </summary>
        /// <param name="isGlobal"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSysSettingValue(bool isGlobal, string key)
        {
            string ret = "";
            try
            {
                if (m_SysVariables == null || m_SysVariables.Count == 0)
                    throw new Exception("系统参数未设置！");
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
        #endregion
    }
}
