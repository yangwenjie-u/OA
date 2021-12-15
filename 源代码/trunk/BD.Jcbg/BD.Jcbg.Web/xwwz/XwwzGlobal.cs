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
using BD.Jcbg.IBll;
using BD.Jcbg.Common;


namespace BD.Jcbg.Web.xwwz
{
    public static class XwwzGlobal
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

        public static string GetSysSettingValue(bool isGlobal, string key)
        {
            string ret = "";
            try
            {
                if (m_SysVariables == null)
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


        private static void LoadSysVariables()
        {
            try
            {
                m_SysVariables = CommonService.GetDataTable("select * from syssetting");
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

      
    }
}