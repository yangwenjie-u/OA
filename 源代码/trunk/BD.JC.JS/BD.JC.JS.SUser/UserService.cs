using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.JC.JS.Common;
using System.Web;

namespace BD.JC.JS.SUser
{
    public class UserService:ICalculate
    {
        public string Calculate(string inparam)
        {
            ReturnParam ret = new ReturnParam(false, "");
            try
            {
                string[] arr = inparam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length < 2)
                    ret.msg = "输入参数格式不正确，应该是：用户类型,用户账号";
                else
                {
                    string msg = "";
                    ret.code = AddUser(arr[0], arr[1], out msg);
                    ret.msg = msg;
                }
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return ret.GetJson();
        }

        private bool AddUser(string usertype, string usercode, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                string sql = "", username = "", realname = "", postcode = "", qybh = "";
                IList<IDictionary<string, string>> dt = null;
                // 创建人员账号
                if (usertype.Equals("u", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找人员信息，获取人员类型、代码、姓名
                        sql = "select lxbh,ryxm,qybh,zh from i_m_ry where rybh='" + usercode + "'";
                        dt = SqlHelper.GetDataTable(sql, out msg);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["ryxm"];
                        qybh = dt[0]["qybh"];
                        // 查找人员类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_rylx where lxbh='" + dt[0]["lxbh"] + "'";
                        dt = SqlHelper.GetDataTable(sql, out msg);
                        if (dt.Count == 0)
                        {
                            msg = "找不到人员类型记录";
                            break;
                        }
                        // 不用创建账号，返回
                        if (!dt[0]["sfcjzh"].GetSafeBool())
                        {
                            code = true;
                            break;
                        }
                        string companycode = dt[0]["zhdwbh"];
                        string depcode = dt[0]["zhbmbh"];
                        string rolecode = dt[0]["zhjsbh"];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = SqlHelper.GetDataTable(sql, out msg);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在";
                            break;
                        }

                        string password = RandomNumber.GetNew(RandomType.NumberAndChar, UserPasswordLength);
                        code = RemoteAddUser(Configs.UmsUrl, Configs.AppId, companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + qybh + "','" + username + "',0,'','',getdate())";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        code = SqlHelper.ExecTrans(sqls, out msg);
                        if (code)
                        {
                            System.Web.HttpContext.Current.Session["USER_INFO_USERNAME"] = username;
                            System.Web.HttpContext.Current.Session["USER_INFO_PASSWORD"] = password;
                        }
                    } while (false);


                }
                // 创建单位账号
                else if (usertype.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    do
                    {
                        // 查找单位信息，获取单位类型、代码、名称
                        sql = "select lxbh,qymc,zh from i_m_qy where qybh='" + usercode + "'";
                        dt = SqlHelper.GetDataTable(sql, out msg);
                        if (dt.Count == 0)
                        {
                            msg = "找不到企业记录";
                            break;
                        }
                        username = dt[0]["zh"].GetSafeString();
                        if (username == "")
                            username = usercode;
                        realname = dt[0]["qymc"];
                        // 查找企业类型信息，获取默认单位、部门、角色                        
                        sql = "select * from h_qylx where lxbh='" + dt[0]["lxbh"] + "'";
                        dt = SqlHelper.GetDataTable(sql, out msg);
                        if (dt.Count == 0)
                        {
                            msg = "找不到企业类型记录";
                            break;
                        }
                        // 不用创建账号，返回
                        if (!dt[0]["sfcjzh"].GetSafeBool())
                        {
                            code = true;
                            break;
                        }
                        string companycode = dt[0]["zhdwbh"];
                        string depcode = dt[0]["zhbmbh"];
                        string rolecode = dt[0]["zhjsbh"];
                        postcode = dt[0]["gwbh"];
                        // 判断账号是否已创建
                        sql = "select * from i_m_qyzh where yhzh='" + username + "'";
                        dt = SqlHelper.GetDataTable(sql, out msg);
                        if (dt.Count > 0)
                        {
                            msg = "账号已经存在";
                            break;
                        }

                        string password = RandomNumber.GetNew(RandomType.NumberAndChar, UserPasswordLength);
                        code = RemoteAddUser(Configs.UmsUrl, Configs.AppId, companycode, depcode, username, realname, rolecode, postcode, password, out msg);
                        if (!code)
                            break;
                        sql = "insert into i_m_qyzh(qybh,yhzh,sfqyzzh,lrrzh,lrrxm,lrsj) values('" + usercode + "','" + username + "',1,'','',getdate())";
                        IList<string> sqls = new List<string>();
                        sqls.Add(sql);
                        code = SqlHelper.ExecTrans(sqls, out msg);
                        if (code)
                        {
                            System.Web.HttpContext.Current.Session["USER_INFO_USERNAME"] = username;
                            System.Web.HttpContext.Current.Session["USER_INFO_PASSWORD"] = password;
                        }
                    } while (false);
                }
                else
                {
                    code = false;
                    msg = "无效的用户类型";
                }

            }
            catch (Exception e)
            {
                code = false;
                msg = e.Message;
            }
            return code;
        }

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
            catch { }
            return ret;
        }

        private void LoadSysVariables()
        {
            try
            {
                string msg = "";
                m_SysVariables = SqlHelper.GetDataTable("select * from syssetting", out msg);
            }
            catch { }
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

        public int UserPasswordLength
        {
            get { return GetSysSettingValue("USER_SETTING_PASSWORD_LENGTH").GetSafeInt(6); }
        }
        #endregion
        #region webservice调用
        private static string KEY = "8e5sjd86";
        private static string IV = "fib85ede";
        private bool RemoteAddUser(string serviceurl, string appid, string companycode, string depcode, 
            string username, string realname, string roleid, string postcode, string password, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                string key = ("A01" + companycode + depcode + username + realname + password + roleid + appid).EncodeDes(KEY, IV);
                string[] args = new string[] { "A01", companycode, depcode, username, realname, password, roleid, appid, key, postcode };
                string json = DynamicWebService.InvokeWebService(
                    "",
                    "Services",
                    "OAFlowInfo",
                    args,
                    serviceurl).ToString();
            }
            catch
            {

            }
            return ret;
        }
        #endregion
    }
}
