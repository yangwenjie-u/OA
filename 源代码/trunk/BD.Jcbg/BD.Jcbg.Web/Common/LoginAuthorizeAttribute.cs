using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD.Jcbg.Web.Common
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>  
        /// 在  Action方法之前 调用  
        /// </summary>  
        /// <param name="filterContext"></param>  
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断是否登录，未登录的跳转到登录页
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                Hashtable singleOnline = (Hashtable)filterContext.HttpContext.Application["Online"];
                // 判断当前SessionID是否存在
                if (singleOnline != null && filterContext.HttpContext.User.Identity != null && singleOnline.ContainsKey(filterContext.HttpContext.User.Identity.Name))
                {
                    if (!singleOnline[filterContext.HttpContext.User.Identity.Name].Equals(filterContext.HttpContext.Session.SessionID))
                    {

                        //排除特殊路径，如登录、退出登录等
                        var controllerName = (filterContext.RouteData.Values["controller"] ?? "").ToString().ToLower();
                        var actionName = (filterContext.RouteData.Values["action"] ?? "").ToString().ToLower();
                        if ((controllerName == "user" && actionName.Contains("login")) ||
                            controllerName == "user" && actionName.Contains("main") ||
                            controllerName == "user" && actionName.Contains("dologout"))
                        {

                        }
                        else
                        {
                            // filterContext.HttpContext.Session.Abandon();
                            // filterContext.Result = new ContentResult() { Content = "<script>if(confirm('你的账号已在别处登陆，是否返回登陆页面重新登陆？')){if(window != window.top){window.top.location = '/User/Loginv3'; } else{window.location.href  = '/User/Loginv3';}}else{window.close();}</script>" };
                        }

                    }
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
}