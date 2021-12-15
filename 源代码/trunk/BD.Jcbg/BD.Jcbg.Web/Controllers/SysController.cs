using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;


namespace BD.Jcbg.Web.Controllers
{
    public class SysController : Controller
    {
        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
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

        #region 页面
        /// <summary>
        /// 系统设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SysSetting()
        {
            ViewBag.IsGlobal = Request["isglobal"].GetSafeInt(1);
            return View();
        }
        /// <summary>
        /// 企业系统设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SysSettingQy()
        {
            return Redirect("/sys/syssetting?isglobal=0");
        }
        /// <summary>
        /// 帮助表设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult HelpSetting()
        {
            ViewBag.Blx = RouteData.Values["id"].GetSafeString();
            return View();
        }

        public ActionResult DownIe()
        {
            return View();
        }
        /// <summary>
        /// 分域名系统设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SysSettingDns()
        {
            return View();
        }
        /// <summary>
        /// 分域名系统配置编辑
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SysSettingDnsEdit()
        {
            ViewBag.IsCopy = Request["iscopy"].GetSafeInt();
            ViewBag.Dns = Request["dns"].GetSafeString();
            return View();
        }


        /// <summary>
        /// 地址跳转
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Action()
        {
            ViewBag.type = Request["type"].GetSafeString();
            return View();
        }
        #endregion

        #region 获取各种数据

        /// <summary>
        /// 获取系统设置分组
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public void GetSysSettings()
        {
            string ret = "{\"groups\":[],\"settings\":[]}";
            int isGlobal = Request["isglobal"].GetSafeInt();
            try
            {
                string syssettinggroup="";
                try
                {
                    syssettinggroup = Configs.GetConfigItem("syssettinggroup");
                }
                catch (Exception e)
                {

                }
                if (syssettinggroup == "")
                    syssettinggroup = "syssettinggroup";

                string syssetting = "";
                try
                {
                    syssetting = Configs.GetConfigItem("syssetting");
                }
                catch (Exception e)
                {

                }
                if (syssetting == "")
                    syssetting = "syssetting";
                string companyWhere = "";
                if (isGlobal == 0)
                    companyWhere = " and ((a.companycode = '' and a.istemplate=1 and not exists (select * from " + syssetting + " b where b.companycode='" + CurrentUser.CompanyCode + "' and b.settingcode=a.settingcode)) or a.companycode='" + CurrentUser.CompanyCode + "') ";
                else
                    companyWhere = " and a.companycode = '' and a.istemplate=0 ";
                IList<IDictionary<string, string>> groups = CommonService.GetDataTable("select * from " + syssettinggroup + " where isglobal=" + isGlobal + " and inuse=1 order by displayorder");
                IList<IDictionary<string, string>> settings = CommonService.GetDataTable("select * from " + syssetting + " a where a.groupid in (select groupid from " + syssettinggroup + " where inuse=1 and isglobal=" + isGlobal + ") " + companyWhere + " order by a.groupid, a.displayorder");

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ret = "{\"groups\":" + serializer.Serialize(groups) + ",\"settings\":" + serializer.Serialize(settings) + "}";
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            finally
            {
                Response.Write(ret);
            }
        }
        /// <summary>
        /// 获取DNS系统设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetSettingDns()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                ret = CommonService.GetDataTable("select * from syssettingdns order by dns");
            }catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(ret);
        }
        /// <summary>
        /// 获取某个dns配置项
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetSettingDnsItem(string dns)
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                if (!string.IsNullOrEmpty(dns))
                    ret = CommonService.GetDataTable("select settingcode,settingvalue from syssettingdns where dns='"+dns+"' order by dns");
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return Json(ret);
        }
        #endregion

        #region 各种处理函数
        /// <summary>
        /// 保存设置
        /// </summary>
        [Authorize]
        public void SaveSysSetting()
        {
            bool code = true;
            string msg = "";
            try
            {
                bool isGlobal = Request["isglobal"].GetSafeBool();

                
                string syssetting = "syssetting";
                /*
                try
                {
                    syssetting = Configs.GetConfigItem("syssetting");
                }
                catch (Exception e)
                {

                }
                if (syssetting == "")
                    syssetting = "syssettinggroup";*/

                IList<string> sqls = new List<string>();
                foreach (string key in Request.Form.AllKeys)
                {
                    string value = Request[key].GetSafeString();
                    if (isGlobal)
                        sqls.Add("update " + syssetting + " set settingvalue='" + value + "' where settingcode='" + key + "'");
                    else
                    {
                        sqls.Add("delete from " + syssetting + " where companycode='" + CurrentUser.CompanyCode + "' and settingcode='" + key + "'");
                        sqls.Add("insert into " + syssetting + "(companycode,groupid,settingcode,settingname,settingvalue,isupload,istemplate,displayorder) select '" + CurrentUser.CompanyCode + "',groupid,settingcode,settingname,'" + value + "',isupload,0,displayorder from  " + syssetting + " where companycode='' and  and settingcode='" + key + "' and istemplate=1");
                    }
                }
                code = CommonService.ExecTrans(sqls);
                // 重新加载内存的全局参数
                Func.GlobalVariable.ReloadSysVariables();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 保存域名设置
        /// </summary>
        [Authorize]
        public void SaveSysSettingDns()
        {
            bool code = true;
            string msg = "";
            try
            {
                string dns = Request["dns"].GetSafeString();
                if (string.IsNullOrEmpty(dns))
                {
                    code = false;
                    msg = "dns不能为空";
                }

                IList<string> sqls = new List<string>();
                sqls.Add("delete from syssettingdns where dns='" + dns + "'");
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.Equals("dns", StringComparison.OrdinalIgnoreCase))
                        continue;
                    sqls.Add("insert into syssettingdns(SettingId,CompanyCode,SettingCode,SettingValue,Dns) values('"+Guid.NewGuid().ToString()+"','','"+key+"','"+ Request[key].GetSafeString() + "','"+dns+"')");
                    
                }
                code = CommonService.ExecTrans(sqls);
                // 重新加载内存的全局参数
                Func.GlobalVariable.ReloadSysVariables();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        /// <summary>
        /// 删除域名设置
        /// </summary>
        [Authorize]
        public void DeleteSysSettingDns()
        {
            bool code = true;
            string msg = "";
            try
            {
                string dns = Request["dns"].GetSafeString();
                if (string.IsNullOrEmpty(dns))
                {
                    code = false;
                    msg = "dns不能为空";
                }

                IList<string> sqls = new List<string>();
                sqls.Add("delete from syssettingdns where dns='" + dns + "'");
                code = CommonService.ExecTrans(sqls);
                // 重新加载内存的全局参数
                Func.GlobalVariable.ReloadSysVariables();
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                code = false;
                msg = e.Message;
            }
            finally
            {
                Response.Write(JsonFormat.GetRetString(code, msg));
            }
        }
        #endregion
    }
}