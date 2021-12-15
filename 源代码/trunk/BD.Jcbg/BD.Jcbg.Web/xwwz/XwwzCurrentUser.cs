using BD.Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BD.Jcbg.Web.xwwz
{
 
    /*

    [Serializable]
    public class XwwzSessionUser
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public DateTime? UpDate { get; set; }
        public string Company { get; set; }
        public string Dep { get; set; }
        public bool? SupAdmin { get; set; }



    }
    public class XwwzCurrentUser
    {
        #region Session关键字
        private const string SessionCurUser = "XwwzCurrentUser_CurUser";				// 当前登录用户
        private const string SessionTaskUser = "XwwzCurrentUser_TaskUser";				// 托管用户
        #endregion
        #region Session操作
        public static void SetSession(string strName, object objVal)
        {
            HttpContext.Current.Session.Add(strName, objVal);
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

        public static void SetLoginUser(XwwzSessionUser user)
        {
            CurUser = user;
            FormsAuthentication.SetAuthCookie(user.UserCode, false);
        }
        #endregion
        #region 当前用户信息
        /// <summary>
        /// 当前用户
        /// </summary>
        public static XwwzSessionUser CurUser
        {
            get { return GetSession(SessionCurUser) as XwwzSessionUser; }
            set { SetSession(SessionCurUser, value); }
        }
        /// <summary>
        /// 托管用户
        /// </summary>
        public static XwwzSessionUser TaskUser
        {
            get
            {
                XwwzSessionUser user = GetSession(SessionTaskUser) as XwwzSessionUser;
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
 
        /// <summary>
        /// 用户id
        /// </summary>
        public static string UserCode
        {
            get
            {
                if (CurUser != null)
                    return CurUser.UserCode;
                return "";
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public static DateTime? UpDate
        {
            get
            {
                if (CurUser != null)
                    return CurUser.UpDate;
                return Convert.ToDateTime("1970-01-01");
            }
        }
        /// <summary>
        /// 用户单位名称
        /// </summary>
        public static string Company
        {
            get
            {
                if (CurUser != null)
                    return CurUser.Company;
                return "";
            }
        }
        /// <summary>
        /// 用户部门
        /// </summary>
        public static string Dep
        {
            get
            {
                if (CurUser != null)
                    return CurUser.Dep;
                return "";
            }
        }
        /// <summary>
        /// 超级管理员
        /// </summary>
        public static bool? SupAdmin
        {
            get
            {
                if (CurUser != null)
                    return CurUser.SupAdmin;
                return false;
            }
        }
 

        #endregion

        #region 其他扩展参数
 
        #endregion
    }*/
}