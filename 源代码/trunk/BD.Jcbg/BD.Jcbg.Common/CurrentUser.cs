using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace BD.Jcbg.Common
{
    [Serializable]
    public class OaSessionUser
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DutyLevel { get; set; }           // 岗位等级
        public string ManageDep { get; set; }           // 管理部门
        public List<MenuItem> Menus { get; set; }
        public string UserRights { get; set; }

        public string Qybh { get; set; }
        public string Qymc { get; set; }

        public string Jdzch { get; set; }

        public string GCBH { get; set; }

        public string RealUserName { get; set; }

        public string UrlJumpType { get; set; }  // 页面跳转类型

        public DateTime LastLoginTime { get; set; } // 上次登陆时间
    }
    public class CurrentUser
    {
        #region Session关键字
        private const string CookieContainerName = "CurrentUser_CookieContiner";    // 用户系统对应的session容器
        private const string SessionCurUser = "CurrentUser_CurUser";                // 当前登录用户
        private const string SessionTaskUser = "CurrentUser_TaskUser";              // 托管用户
        private const string SessionWdqy = "Wdqy";//是否为涉外企业
        #endregion
        #region Session操作
        public static void SetSession(string strName, object objVal)
        {
            HttpContext.Current.Session.Add(strName, objVal);
        }

        public static void SetSession(System.Web.HttpSessionStateBase Session, string strName, object objVal)
        {
            Session.Add(strName, objVal);
        }

        public static object GetSession(string strName)
        {
            object objRet = null;
            if (System.Web.HttpContext.Current.Session == null || System.Web.HttpContext.Current.Session.Count == 0)
                objRet = null;
            else
                objRet = System.Web.HttpContext.Current.Session[strName];
            return objRet;
        }

        public static object GetSession(System.Web.HttpSessionStateBase Session, string strName)
        {
            object objRet = null;
            if (Session == null || Session.Count == 0)
                objRet = null;
            else
                objRet = Session[strName];
            return objRet;
        }

        public static void SetLoginUser(OaSessionUser user, System.Web.HttpSessionStateBase Session = null)
        {
            if (Session == null)
                CurUser = user;
            else
                SetSession(Session, SessionCurUser, user);
            FormsAuthentication.SetAuthCookie(user.UserName, false);
        }
        #endregion
        #region 当前用户信息
        /// <summary>
        /// 当前用户
        /// </summary>
        public static OaSessionUser CurUser
        {
            get { return GetSession(SessionCurUser) as OaSessionUser; }
            set { SetSession(SessionCurUser, value); }
        }

        public static string Wdqy
        {
            get { return GetSession(SessionWdqy).ToString(); }
            set { SetSession(SessionWdqy, value); }
        }

        /// <summary>
        /// 托管用户
        /// </summary>
        public static OaSessionUser TaskUser
        {
            get
            {
                OaSessionUser user = GetSession(SessionTaskUser) as OaSessionUser;
                if (user == null)
                    return CurUser;
                return user;
            }
            set { SetSession(SessionTaskUser, value); }
        }
        /// <summary>
        /// 用户是否已经验证
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.IsAuthenticated && CurUser != null && UserName != "";
            }
        }
        /// <summary>
        /// 用户系统对于的Session容器
        /// </summary>
        public static System.Net.CookieContainer GetContainer(System.Web.HttpSessionStateBase Session = null)
        {
            object ret = null;
            if (Session == null)
            {
                ret = GetSession(CookieContainerName);
                if (ret == null)
                    SetSession(CookieContainerName, new System.Net.CookieContainer());
                ret = GetSession(CookieContainerName);
            }
            else
            {
                ret = GetSession(Session, CookieContainerName);
                if (ret == null)
                    SetSession(Session, CookieContainerName, new System.Net.CookieContainer());
                ret = GetSession(Session, CookieContainerName);
            }
            return ret as System.Net.CookieContainer;
        }
        public static System.Net.CookieContainer CurContainer
        {
            get
            {
                object ret = null;
                    ret = GetSession(CookieContainerName);
                    if (ret == null)
                        SetSession(CookieContainerName, new System.Net.CookieContainer());
                    ret = GetSession(CookieContainerName);
                return ret as System.Net.CookieContainer;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                if (CurUser != null)
                    return CurUser.UserName;
                return "";
            }
        }
        public static string RealUserName
        {
            get
            {
                if (CurUser != null)
                    return CurUser.RealUserName;
                return "";
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string RealName
        {
            get
            {
                if (CurUser != null)
                    return CurUser.RealName;
                return "";
            }
        }
        /// <summary>
        /// 企业编号
        /// </summary>
        public static string Qybh
        {
            get
            {
                if (CurUser != null)
                    return CurUser.Qybh;
                return "JCQ007001";
            }
        }
        /// <summary>
        /// 所属企业
        /// </summary>
        public static string Qymc
        {
            get
            {
                if (CurUser != null)
                    return CurUser.Qymc;
                return "";
            }
        }

        /// <summary>
        /// 用户id
        /// </summary>
        public static string UserCode
        {
            get
            {
                if (CurUser != null)
                    return CurUser.UserCode;
                return "URCQ1uHnsuOjIN";
                //return "";
            }
        }
        // 页面跳转类型

        public static string UrlJumpType
        {
            get
            {
                if (CurUser != null)
                    return CurUser.UrlJumpType;
                return "";
            }
        }
        // 上次登陆时间
        public static string LastLoginTime {
            get
            {
                if (CurUser != null)
                    return CurUser.LastLoginTime.ToString("yyyy-MM-dd HH:mm");
                return "";
            }
        } 
        /// <summary>
        /// 用户菜单
        /// </summary>
        public static List<MenuItem> Menus
        {
            get
            {
                if (TaskUser != null)
                    return TaskUser.Menus;
                return new List<MenuItem>();
            }
        }
        /// <summary>
        /// 用户单位代码
        /// </summary>
        public static string CompanyCode
        {
            get
            {
                if (CurUser != null)
                    return CurUser.CompanyId;
                return "";
            }
        }
        /// <summary>
        /// 用户单位名称
        /// </summary>
        public static string CompanyName
        {
            get
            {
                if (CurUser != null)
                    return CurUser.CompanyName;
                return "";
            }
        }
		/// <summary>
        /// jdzch
        /// </summary>
        public static string Jdzch
        {
            get
            {
                if (CurUser != null)
                    return CurUser.Jdzch;
                return "";
            }
        }

        /// <summary>
        /// gcbh
        /// </summary>
        public static string GCBH
        {
            get
            {
                if (CurUser != null)
                    return CurUser.GCBH;
                return "";
            }
        }
        /// <summary>
        /// 用户权限，逗号分隔
        /// </summary>
        public static string UserRights
        {
            get
            {
                if (TaskUser != null)
                    return TaskUser.UserRights;
                return "";
            }
        }
        /// <summary>
        /// 用户是否有某个权限
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool HasRight(string right)
        {
            bool ret = false;
            try
            {

                ret = ("," + UserRights + ",").IndexOf("," + right + ",") > -1;
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }

        #endregion

        #region 其他扩展参数
        public static string Wtdytqy
        {
            get { return GetSession("QYBH") as string; }
            set { SetSession("QYBH", value); }
        }         // 委托单预填企业
        #endregion
    }
}
