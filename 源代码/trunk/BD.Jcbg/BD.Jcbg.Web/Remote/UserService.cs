using BD.Jcbg.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace BD.Jcbg.Web.Remote
{
    public static class UserService
    {
        #region 登录验证操作
        public static IList<RemoteUserService.VUser> m_Users = null;
        public static IList<RemoteUserService.VUser> Users
        {
            get
            {
                if (m_Users == null || m_Users.Count() == 0)
                {
                    try
                    {
                        RemoteUserService.Services srv = new RemoteUserService.Services();
                        srv.CookieContainer = CurrentUser.CurContainer;
                        RemoteUserService.VUser[] users = srv.UserTableList(Configs.AppId);
                        m_Users = users.ToList();

                    }
                    catch (Exception e)
                    {
                        SysLog4.WriteLog(e);
                    }
                }
                return m_Users;
            }
        }

        public static IList<RemoteUserService.VUser> FileShareUsers
        {
            get
            {
                string execludeDeps = Configs.FileShareExecudeDeps;
                var q = from e in Users where execludeDeps == "" || execludeDeps.IndexOf(e.DEPCODE) == -1 orderby e.REALNAME select e;
                return q.ToList<RemoteUserService.VUser>();
            }
        }

        /// <summary>
        /// 登录校验
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Login(string username, string password, out string err, System.Web.HttpSessionStateBase Session = null)
        {
            bool ret = false;
            err = "";
            try
            {
                ret = Login(username, "", password, out err, Session);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 登录校验，不传密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realname"></param>
        /// <param name="password"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool LoginWithOutPassWord(string username, string areacode, out string err)
        {
            bool ret = true;
            err = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;

                RemoteUserService.SUser user = srv.GetUserInfo(areacode, username);
                if (user == null)
                    return false;
                string companyname = GetUserCompany(user.CPCODE);
                string departmentname = GetUserDepartment(user.DEPCODE);
                OaSessionUser sessionuser = new OaSessionUser()
                {
                    CompanyId = user.CPCODE,
                    CompanyName = companyname,
                    DepartmentId = user.DEPCODE,
                    DepartmentName = departmentname,
                    DutyLevel = user.POSTDM,
                    Menus = GetMenus(),
                    RealName = user.REALNAME,
                    UserCode = user.USERCODE,
                    UserName = user.USERCODE,//user.USERNAME,
                    UserRights = GetRights(),
                    ManageDep = user.GLDEPCODE,
                    RealUserName = user.USERNAME
                };
                CurrentUser.SetLoginUser(sessionuser);
                //获取用户的角色菜单ID
                var roleList = srv.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                if (roleList.Length > 0)
                {
                    switch (roleList[0].ROLECODE)
                    {
                        case "CR201705000002"://项目报表查看
                            CurrentUser.Wdqy = "0";//只显示项目的
                            break;
                        case "CR201705000001"://涉外企业报表查看
                            CurrentUser.Wdqy = "1";//只显示涉外的
                            break;
                        default:
                            CurrentUser.Wdqy = "";//空表示所有的都显示
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// 登录校验
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Login(string username, string realname, string password, out string err, System.Web.HttpSessionStateBase Session = null)
        {
            bool ret = false;
            err = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.GetContainer(Session);
                ret = srv.CheckLogin(username, password,"");
                if (ret)
                {
                    RemoteUserService.SUser user = srv.GetCurrentUser();
                    if (user == null)
                        return false;
                    string companyname = GetUserCompany(user.CPCODE);
                    string departmentname = GetUserDepartment(user.DEPCODE);
                    OaSessionUser sessionuser = new OaSessionUser()
                    {
                        CompanyId = user.CPCODE,
                        CompanyName = companyname,
                        DepartmentId = user.DEPCODE,
                        DepartmentName = departmentname,
                        DutyLevel = user.POSTDM,
                        Menus = GetMenus(),
                        RealName = user.REALNAME,
                        UserCode = user.USERCODE,
                        UserName = user.USERCODE,//user.USERNAME,
                        UserRights = GetRights(),
                        ManageDep = user.GLDEPCODE,
                        RealUserName = user.USERNAME
                    };
                    if (realname != "")
                        sessionuser.RealName = realname;
                    CurrentUser.SetLoginUser(sessionuser, Session);
                    //获取用户的角色菜单ID
                    var roleList = srv.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                    if (roleList.Length > 0)
                    {
                        switch (roleList[0].ROLECODE)
                        {
                            case "CR201705000002"://项目报表查看
                                CurrentUser.Wdqy = "0";//只显示项目的
                                break;
                            case "CR201705000001"://涉外企业报表查看
                                CurrentUser.Wdqy = "1";//只显示涉外的
                                break;
                            default:
                                CurrentUser.Wdqy = "";//空表示所有的都显示
                                break;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                err = e.Message;
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 校验用户密码是否正确
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckLogin(string username, string password,
            out string usercode, out string realname,
            out string msg)
        {
            bool ret = false;
            usercode = "";
            realname = "";
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                ret = srv.CheckLogin(username, password, Configs.AppId);

                if (ret)
                {
                    RemoteUserService.SUser user = srv.GetUserInfo("", username);
                    if (user != null)
                    {
                        usercode = user.USERCODE;
                        realname = user.REALNAME;
                    }
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
            }
            return ret;
        }

        public static void Logout()
        {
            if (CurrentUser.IsLogin)
            {
                System.Web.HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
            }
        }
        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <returns></returns>
        public static List<MenuItem> GetMenus()
        {
            List<MenuItem> ret = new List<MenuItem>();

            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.VMenu[] menus = srv.GetUserVPower(Configs.AppId);
                var q = from e in menus where e.PMENUCODE == "" orderby e.PXH ascending select e;
                List<RemoteUserService.VMenu> groups = q.ToList();
                foreach (RemoteUserService.VMenu itm in groups)
                {
                    q = from e in menus where e.PMENUCODE == itm.MENUCODE && e.SFYW == "1" && e.ISMENU == "1" orderby e.PXH ascending select e;
                    List<RemoteUserService.VMenu> childs = q.ToList();
                    //子节点为1并备注为1
                    if (childs.Count == 1 && childs[0].MEMO == "1")
                    {
                        itm.FUNURL = childs[0].FUNURL;
                        itm.MEMO = childs[0].MEMO;
                    }
                    ret.Add(new MenuItem() { DisplayOrder = itm.PXH, ImageUrl = itm.IMGURL, IsGroup = true, IsMenu = true, MenuCode = itm.MENUCODE, MenuName = itm.MENUNAME, MenuUrl = itm.FUNURL, ParentCode = itm.PMENUCODE, Memo = itm.MEMO });
                    if (childs.Count == 0)
                        ret.RemoveAt(ret.Count - 1);
                    else if (childs.Count == 1 && childs[0].MEMO == "1") { }
                    else
                    {
                        foreach (RemoteUserService.VMenu menu in childs)
                        {
                            ret.Add(new MenuItem() { DisplayOrder = menu.PXH, ImageUrl = menu.IMGURL, IsGroup = false, IsMenu = true, MenuCode = menu.MENUCODE, MenuName = menu.MENUNAME, MenuUrl = menu.FUNURL, ParentCode = menu.PMENUCODE });
                        }
                    }
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        public static string GetRights()
        {
            StringBuilder ret = new StringBuilder();
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SMenu[] menus = srv.GetUserPower(Configs.AppId);

                foreach (RemoteUserService.SMenu itm in menus)
                {
                    ret.Append(itm.MENUCODE + ",");
                }

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret.ToString();
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldpass"></param>
        /// <param name="newpass"></param>
        /// <returns></returns>
        public static bool ChangePass(string username, string oldpass, string newpass)
        {
            bool ret = true;

            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                ret = srv.ChangePass("", username, oldpass, newpass);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ReSetPassWord(string username, string password)
        {
            string userjson = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();
                userjson = srv.UMS_ResetPasswordByName2(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), username, password); //重置指定密码

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return userjson;
        }



        public static bool ResetPass(string username, string userpwd, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {

                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SUser user = srv.GetUserInfo("", username);
                if (user == null)
                {
                    msg = "无效的用户名";
                    return ret;
                }
                string timestring = GetTimeStamp();
                string json = srv.UMS_ResetPasswordByName2(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERNAME, userpwd);
                JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                UserServiceRet usr = jds.DeSerializer(json, out msg);
                if (msg != "")
                    return ret;

                ret = usr.success;
                msg = usr.msg;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            return ret;

        }

        /// <summary>
        /// 根据用户代码更新用户名
        /// </summary>
        /// <param name="usercode">用户代码</param>
        /// <param name="username">用户名</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public static bool ChangeUsernameByUserCode(string usercode, string username, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {

                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SUser user = srv.GetUserInfoByUsercode("", usercode);
                if (user == null)
                {
                    msg = "无效的用户信息";
                    return ret;
                }
                string timestring = GetTimeStamp();

                string json = srv.UMS_USER_ChangeUsernameByUsercode(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, username);
                JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                UserServiceRet usr = jds.DeSerializer(json, out msg);
                if (msg != "")
                    return ret;

                ret = usr.success;
                msg = usr.msg;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            return ret;
        }
        /// <summary>
        /// 获取单位名称
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static string GetUserCompany(string companycode)
        {
            string ret = "";

            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                ret = srv.GetCompanyByCpcode(companycode).CPNAME;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 获取部门名称
        /// </summary>
        /// <param name="depcode"></param>
        /// <returns></returns>
        public static string GetUserDepartment(string depcode)
        {
            string ret = "";

            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                ret = srv.GetDepByCode(depcode).DEPNAME;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }
        /// <summary>
        /// 就获取用户姓名
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static string GetUserRealName(string users)
        {
            StringBuilder ret = new StringBuilder();
            try
            {
                string[] usernamearr = users.Split(new char[] { ',' });
                foreach (string username in usernamearr)
                {
                    var q = from e in Users where e.USERCODE.Equals(username, StringComparison.OrdinalIgnoreCase) select e.REALNAME;
                    if (ret.Length > 0)
                        ret.Append(",");
                    if (q.Count() == 0)
                        ret.Append(username);
                    else
                        ret.Append(q.First());
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 设置用户签名
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static bool SetUserSign(string username, string sign, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                //srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.ResultParam param = srv.SetSignByUsername(username, sign);
                ret = param.Successed;
                if (!ret)
                    msg = param.Msg;
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
        /// 设置用户签名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool GetUserSign(string username, out string msg)
        {
            bool ret = true;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                msg = srv.GetSignByUsername(username);
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
        /// 获取用户所属角色
        /// </summary>
        /// <param name="proCode"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetRoles(string proCode, string userName)
        {
            string ret = string.Empty;
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                var result = srv.GetRoleListByProcodeAndUsername(proCode, userName);

                if (result != null)
                    ret = string.Join(",", result.Select(x => x.ROLECODE).Distinct().ToList());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return ret;
        }
        #endregion

        #region 用户操作
        private static string KEY = "8e5sjd86";
        private static string IV = "fib85ede";
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
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                if (password == "")
                    password = Configs.DefaultPassword;
                string key = ("A01" + companycode + depcode + username + realname + password + roleid + Configs.AppId).EncodeDes(KEY, IV);
                string json = srv.UMS_AddUser("A01", companycode, depcode, username, realname, password, roleid, Configs.AppId, key, postcode);

                JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                UserServiceRet usr = jds.DeSerializer(json, out msg);
                if (msg != "")
                    return ret;
                if (!usr.success)
                {
                    //msg = usr.msg;
                    return ret;
                }
                RemoteUserService.SUser user = srv.GetUserInfo("", username);

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
        /// 判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool UserExists(string username)
        {
            bool ret = false;
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SUser user = srv.GetUserInfo("", username);
                ret = user != null;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
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
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.VUserrole[] roles = srv.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                IEnumerable<RemoteUserService.VUserrole> findRoles = roles.Where(role => role.ROLECODE == rolecode);
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
                        string json = srv.UMS_AddUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), usercode, rolecode);
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
        /// <summary>
        /// 用户删除角色
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="username"></param>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        public static bool DelUserRole(string username, string rolecode, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.VUserrole[] roles = srv.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                IEnumerable<RemoteUserService.VUserrole> findRoles = roles.Where(role => role.ROLECODE == rolecode);
                if (findRoles != null || findRoles.Count() != 0)
                {
                    string usercode = GetUserCode(username);
                    if (usercode == "")
                    {
                        msg = "获取用户代码失败";
                    }
                    else
                    {
                        string timestring = GetTimeStamp();
                        string json = srv.UMS_DelUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), usercode, rolecode);
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

        public static string GetUserCode(string username)
        {
            string ret = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                //srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SUser user = srv.GetUserInfo("", username);
                if (user != null)
                    ret = user.USERCODE;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public static string GetUserName(string usercode)
        {
            string ret = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                //srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SUser user = srv.GetUserInfoByUsercode("", usercode);
                if (user != null)
                    ret = user.USERNAME;
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }



        public static bool UpdateUserRole(string username, IList<string> newRoles, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string usercode = GetUserCode(username);
                if (usercode == "")
                {
                    msg = "获取用户代码失败";
                }
                else
                {

                    RemoteUserService.VUserrole[] oldRoles = srv.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                    // 删除不存在的
                    string timestring = GetTimeStamp();
                    foreach (RemoteUserService.VUserrole role in oldRoles)
                    {
                        var q = from e in newRoles where e.Equals(role.ROLECODE) select e;
                        if (q.Count() == 0)
                        {
                            string json = srv.UMS_DelUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), usercode, role.ROLECODE);
                            JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                            UserServiceRet usr = jds.DeSerializer(json, out msg);


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
                            srv.UMS_AddUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), usercode, str);
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


        public static bool UpdateUserRole(string username, string usercode, IList<string> newRoles, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;

                //string usercode = GetUserCode(username);
                RemoteUserService.SUser user = null;
                if (username != "")
                    user = srv.GetUserInfo("", username);
                else
                    user = srv.GetUserInfoByUsercode("", usercode);
                if (user == null)
                {
                    msg = "用户不存在";
                }
                else
                {

                    RemoteUserService.VUserrole[] oldRoles = srv.GetRoleListByProcodeAndUsername(Configs.AppId, user.USERNAME);
                    // 删除不存在的
                    string timestring = GetTimeStamp();
                    foreach (RemoteUserService.VUserrole role in oldRoles)
                    {
                        var q = from e in newRoles where e.Equals(role.ROLECODE) select e;
                        if (q.Count() == 0)
                        {
                            srv.UMS_DelUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, role.ROLECODE);

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
                            srv.UMS_AddUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, str);
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


        public static bool UpdateUserRoleNotDelet(string username, string usercode, IList<string> newRoles, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;

                //string usercode = GetUserCode(username);
                RemoteUserService.SUser user = null;
                if (username != "")
                    user = srv.GetUserInfo("", username);
                else
                    user = srv.GetUserInfoByUsercode("", usercode);
                if (user == null)
                {
                    msg = "用户不存在";
                }
                else
                {

                    RemoteUserService.VUserrole[] oldRoles = srv.GetRoleListByProcodeAndUsername(Configs.AppId, user.USERNAME);
                    string timestring = GetTimeStamp();
                    // 删除不存在的
                    /*
                    string timestring = GetTimeStamp();
                    foreach (RemoteUserService.VUserrole role in oldRoles)
                    {
                        var q = from e in newRoles where e.Equals(role.ROLECODE) select e;
                        if (q.Count() == 0)
                        {
                            srv.UMS_DelUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, role.ROLECODE);

                        }
                    }*/
                    // 添加新的
                    foreach (string str in newRoles)
                    {
                        if (str.Length == 0)
                            continue;
                        var q = from e in oldRoles where e.ROLECODE.Equals(str) select e;
                        if (q.Count() == 0)
                        {
                            srv.UMS_AddUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, str);
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



        public static List<RemoteUserService.VUser> GetUserListByRolecode(string rolecode)
        {
            List<RemoteUserService.VUser> users = new List<RemoteUserService.VUser>();
            string msg = "";

            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();
                string json = srv.UMS_GetUserListByRolecode(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), rolecode);
                IDictionary<string, object> info = JsonDynamicDeSerializer.DeSerializerObject(json);
                bool success = info["success"].GetSafeBool();
                msg = info["msg"].GetSafeString();
                if (success)
                {
                    users = JsonConvert.DeserializeObject<List<RemoteUserService.VUser>>(JsonConvert.SerializeObject(info["data"]));
                }

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return users;
        }

        /// <summary>
        /// 就获取用户单位
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static string GetUserCompanyCode(string users)
        {
            StringBuilder ret = new StringBuilder();
            try
            {
                string[] usernamearr = users.Split(new char[] { ',' });
                foreach (string username in usernamearr)
                {
                    var q = from e in Users where e.USERNAME.Equals(username, StringComparison.OrdinalIgnoreCase) select e.CPCODE;
                    if (ret.Length > 0)
                        ret.Append(",");
                    if (q.Count() == 0)
                        ret.Append(username);
                    else
                        ret.Append(q.First());
                }
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret.ToString();
        }


        /// <summary>
        /// 就获取用户权限
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static string GetUserRole(string username)
        {
            string userjson = "";
            try
            {

                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();
                RemoteUserService.VUserrole[] oldRoles = srv.GetRoleListByProcodeAndUsername(Configs.AppId, username);
                foreach (RemoteUserService.VUserrole role in oldRoles)
                {
                    if (userjson != "")
                        userjson += ",";
                    userjson += role.ROLECODE;
                }

                //userjson = srv.UMS_GetUserPower(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), Configs.AppId, users);
                //userjson = srv.UMS_ResetPasswordByName2(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), username, password); //重置指定密码
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return userjson;
        }


        /// <summary>
        /// 获取某角色的相关人员
        /// </summary>
        /// <param name="Rolecode"></param>
        /// <returns></returns>
        public static string GetRoleUser(string Rolecode)
        {
            string userjson = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();
                userjson = srv.UMS_GetUserListByRolecode(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), Rolecode); //获取角色的所有人员

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return userjson;
        }

        private const string KEY_UMS = "8e5sjd86";
        private const string IV_UMS = "fib85ede";
        public static bool DeleteUser(string username, out string msg)
        {
            bool ret = false;
            msg = "";

            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();

                ret = srv.DeleteUser("", username, username.EncodeDes(KEY_UMS, IV_UMS));

            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return ret;
        }


        public static bool DeleteUserRole(string username, string rolecode, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string usercode = GetUserCode(username);
                if (usercode == "")
                {
                    msg = "获取用户代码失败";
                }
                else
                {
                    string timestring = GetTimeStamp();
                    srv.UMS_DelUserRole(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), usercode, rolecode);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public static bool ForbidenUser(string username, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();
                srv.UMS_ChangeStateByName(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), username, "JY");
                ret = true;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        /// <summary>
        /// 根据用户代码更新用户名
        /// </summary>
        /// <param name="usercode">用户代码</param>
        /// <param name="username">用户名</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public static bool ChangeRealnameByUserCode(string usercode, string realname, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {

                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                RemoteUserService.SUser user = srv.GetUserInfoByUsercode("", usercode);
                if (user == null)
                {
                    msg = "无效的用户信息";
                    return ret;
                }
                string timestring = GetTimeStamp();

                string json = srv.UMS_USER_ChangeRealnameByUsercode(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), user.USERCODE, realname);
                JsonDeSerializer<UserServiceRet> jds = new JsonDeSerializer<UserServiceRet>();
                UserServiceRet usr = jds.DeSerializer(json, out msg);
                if (msg != "")
                    return ret;
                ret = usr.success;
                msg = usr.msg;

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                ret = false;
                msg = e.Message;
            }
            return ret;
        }
        public static bool GetMenuTopLevelList(string prcode, out string msg)
        {
            bool ret = false;
            msg = "";
            try
            {
                RemoteUserService.Services srv = new RemoteUserService.Services();
                srv.CookieContainer = CurrentUser.CurContainer;
                string timestring = GetTimeStamp();
                msg = srv.UMS_MENU_GetMenuTopLevelList(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=UMS", timestring)), prcode);
                ret = true;                
                

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                SysLog4.WriteLog(ex);
            }
            return ret;
        }

        public static List<RemoteUserService.VDep> GetDepListByCpCode(string cpcode)
        {
            RemoteUserService.Services srv = new RemoteUserService.Services();
            srv.CookieContainer = CurrentUser.CurContainer;
            List<RemoteUserService.VDep> deplist = new List<RemoteUserService.VDep>();
            RemoteUserService.VDep[] retlist = srv.GetDepListByCpCode(cpcode);
            if (retlist.Length > 0)
            {
                foreach (var item in retlist)
                {
                    deplist.Add(item);
                }
            }
            return deplist;

        }

        #endregion


        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }
    }

}