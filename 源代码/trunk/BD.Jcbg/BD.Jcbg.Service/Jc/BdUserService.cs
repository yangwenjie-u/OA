using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Service.UserWebService;
using System.Web.Script.Serialization;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;
using BD.Jcbg.Service.model.jc;
using BD.Jcbg.Service.config;

namespace BD.Jcbg.Service.Jc
{
    public class BdUserService
    {
        #region 属性
        private static string KEY = "8e5sjd86";
        private static string IV = "fib85ede";

        /// <summary>
        /// 序列化类
        /// </summary>
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// 接口服务类
        /// </summary>
        private static Services service = new Services();
        #endregion

        #region 构造函数
        static BdUserService()
        {
            //service.Url = UserServiceConfig.UmsUrl;
        }
        #endregion

        #region 函数
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="companycode"></param>
        /// <param name="depcode"></param>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="roleid"></param>
        /// <param name="postcode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool AddUser(string companycode, string depcode, string username, string realname, string roleid, string postcode, string password, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                //service.CookieContainer = CurrentUser.CurContainer;
                if (password == "")
                    password = Configs.DefaultPassword;
                string key = ("A01" + companycode + depcode + username + realname + password + roleid + Configs.AppId).EncodeDes(KEY, IV);
                string json = service.UMS_AddUser("A01", companycode, depcode, username, realname, password, roleid, Configs.AppId, key, postcode);

                JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                UserServiceRet usr = jds.DeSerializer(json, out msg);
                if (msg != "")
                    return ret;
                if (!usr.success)
                {
                    //msg = usr.msg;
                    return ret;
                }
                UserWebService.SUser user = service.GetUserInfo("", username);

                ret = usr.success;
                msg = user.USERCODE;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 用户添加角色
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="username"></param>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        public static bool AddUserRole(string username, string rolecode, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                service.CookieContainer = CurrentUser.CurContainer;
                UserWebService.VUserrole[] roles = service.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                IEnumerable<UserWebService.VUserrole> findRoles = roles.Where(role => role.ROLECODE == rolecode);
                if (findRoles == null || findRoles.Count() == 0)
                {
                    string usercode = GetUserCode(username);
                    if (usercode == "")
                    {
                        msg = "获取用户代码失败";
                    }
                    else
                    {
                        string timestring = GetTimeStamp();
                        string json = service.UMS_AddUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), usercode, rolecode);
                        JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                        UserServiceRet usr = jds.DeSerializer(json, out msg);
                        if (msg != "")
                            return ret;

                        ret = usr.success;
                        msg = usr.msg;
                    }
                }
                else
                    ret = true;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public static bool UpdateUserRole(string username, string usercode, IList<string> newRoles, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                //service.CookieContainer = CurrentUser.CurContainer;

                //string usercode = GetUserCode(username);
                UserWebService.SUser user = null;
                if (username != "")
                    user = service.GetUserInfo("", username);
                else
                    user = service.GetUserInfoByUsercode("", usercode);
                if (user == null)
                {
                    msg = "用户不存在";
                }
                else
                {

                    UserWebService.VUserrole[] oldRoles = service.GetRoleListByProcodeAndUsername(Configs.AppId, user.USERNAME);
                    // 删除不存在的
                    string timestring = GetTimeStamp();
                    foreach (UserWebService.VUserrole role in oldRoles)
                    {
                        var q = from e in newRoles where e.Equals(role.ROLECODE) select e;
                        if (q.Count() == 0)
                        {
                            service.UMS_DelUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, role.ROLECODE);

                        }
                    }
                    // 添加新的
                    foreach (string str in newRoles)
                    {
                        if (str.Length == 0)
                            continue;
                        var q = from e in oldRoles where e.ROLECODE.Equals(str) select e;
                        if (q.Count() == 0)
                        {
                            service.UMS_AddUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, str);
                        }
                    }
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        public static string GetUserCode(string username)
        {
            string ret = "";
            try
            {
                //srv.CookieContainer = CurrentUser.CurContainer;
                UserWebService.SUser user = service.GetUserInfo("", username);
                if (user != null)
                    ret = user.USERCODE;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        #endregion
        
    }
}
