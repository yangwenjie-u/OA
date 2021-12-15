using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BD.Jcbg.Common;
using Spring.Context;
using Spring.Context.Support;
using BD.Jcbg.IBll;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.Web.modules
{
    /// <summary>
    /// 多域名映射
    /// </summary>
    public class UrlModule:IHttpModule
    {
        private HttpApplication mHttpApp = null;
        private Hashtable mLoginMap = null;         // 登陆页面映射
        private Hashtable mMainMap = null;          // 首页面
        private Hashtable mFrameMap = null;         // 框架页路径
        private IList<VSysJumpPage> mJumpPages = null;       // sysjump里面的映射，可能包含用户类型

        private static ISystemService _syetemService = null;
        private static ISystemService SystemService
        {
            get
            {
                if (_syetemService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _syetemService = webApplicationContext.GetObject("SystemService") as ISystemService;
                }
                return _syetemService;
            }
        }

        /// <summary>
        /// 处置由实现 System.Web.IHttpModule 的模块使用的资源（内存除外）
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// 初始化模块，并使其为处理请求做好准备。
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            mHttpApp = context;
            context.BeginRequest += context_BeginRequest;//在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
            context.EndRequest += context_EndRequest;    //在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
            context.AcquireRequestState += Context_AcquireRequestState;
            // 加载dns映射
            try
            {
                IDictionary<string, string> loginUrls = SystemService.GetLoginUrls();
                mLoginMap = new Hashtable();
                foreach (string key in loginUrls.Keys)
                    mLoginMap.Add(key, loginUrls[key]);
                IDictionary<string, string> mainUrls = SystemService.GetMainUrls();
                mMainMap = new Hashtable();
                foreach (string key in mainUrls.Keys)
                    mMainMap.Add(key, mainUrls[key]);
                IDictionary<string, string> frameUrls = SystemService.GetMainFrameUrls();
                mFrameMap = new Hashtable();
                foreach (string key in frameUrls.Keys)
                    mFrameMap.Add(key, frameUrls[key]);

                mJumpPages = SystemService.GetJumpPages();
            }
            catch { }
        }

        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = application.Request;
            HttpResponse response = application.Response;

            try
            {
                string jumpurl = "";
                if (CurrentUser.IsLogin)
                {
                    string url = request.FilePath;
                    string dns = GetSrcDns(request);
                    // 跳转设置里面找
                    var finds = mJumpPages.Where(ele =>
                        ele.SrcUrl.Equals(url, StringComparison.OrdinalIgnoreCase) &&
                        (string.IsNullOrEmpty(ele.Dns) || ele.Dns.Equals(dns, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrEmpty(ele.UserType) || ele.UserType.Equals(CurrentUser.CurUser.UrlJumpType))
                        ).OrderByDescending(ele => ele.Dns).OrderByDescending(ele => ele.UserType);
                    if (finds.Count() > 0)
                        jumpurl = finds.ElementAt(0).JumpUrl;
                    // 域名系统设置里面找
                    else
                    {
                        if (IsMainUrl(request))
                            jumpurl = GetMainTransUrl(request);
                        else if (IsMainFrameUrl(request))
                            jumpurl = GetMainFrameTransUrl(request);
                    }
                }

                if (!string.IsNullOrEmpty(jumpurl) && !jumpurl.Equals(request.FilePath, StringComparison.OrdinalIgnoreCase))
                    response.Redirect(jumpurl);
            }
            catch { }
        }

        /// <summary>
        /// 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = application.Request;
            HttpResponse response = application.Response;
            //response.Write("context_EndRequest >> 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生");
        }

        /// <summary>
        /// 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = application.Request;
            HttpResponse response = application.Response;
            //mHttpApp.Context.RewritePath("/user/login");
            //response.Write(request.ServerVariables["SERVER_NAME"]);
            //response.Write(request.FilePath);
            try
            {
                string jumpurl = "";
                // 登陆页面
                if (IsLoginUrl(request))
                {
                    jumpurl = GetLoginTransUrl(request);
                }

                if (!string.IsNullOrEmpty(jumpurl) && !jumpurl.Equals(request.FilePath, StringComparison.OrdinalIgnoreCase))
                    mHttpApp.Context.RewritePath(jumpurl);
            }
            catch { }
        }

        bool IsLoginUrl(HttpRequest request)
        {
            return request.FilePath.Equals("/") || request.FilePath.Equals("/user/login");
        }
        string GetLoginTransUrl(HttpRequest request)
        {
            string ret = "";
            try
            {
                string dns = GetSrcDns(request);
                if (mLoginMap.ContainsKey(dns))
                    ret = mLoginMap[dns].GetSafeString();
                else if (mLoginMap.ContainsKey("____"))
                    ret = mLoginMap["____"].GetSafeString();
            }
            catch { }
            return ret;
        }
        string GetSrcDns(HttpRequest request)
        {
            string ret = "";
            try
            {
                string dns = request.ServerVariables["SERVER_NAME"];
                string port = request.ServerVariables["SERVER_PORT"];
                if (port == null || port == "80" || port == "443")
                    port = "";
                else
                    port = ":" + port;
                ret = dns + port;
            }
            catch { }
            return ret;
        }
        bool IsMainUrl(HttpRequest request)
        {
            return request.FilePath.Equals("/user/welcome");
        }
        string GetMainTransUrl(HttpRequest request)
        {
            string ret = "";
            try
            {
                string dns = GetSrcDns(request);
                if (mMainMap.ContainsKey(dns))
                    ret = mMainMap[dns].GetSafeString();
                else if (mMainMap.ContainsKey("____"))
                    ret = mMainMap["____"].GetSafeString();
            }
            catch { }
            return ret;
        }

        bool IsMainFrameUrl(HttpRequest request)
        {
            return request.FilePath.Equals("/user/main");
        }
        string GetMainFrameTransUrl(HttpRequest request)
        {
            string ret = "";
            try
            {
                string dns = GetSrcDns(request);
                if (mFrameMap.ContainsKey(dns))
                    ret = mFrameMap[dns].GetSafeString();
                else if (mFrameMap.ContainsKey("____"))
                    ret = mFrameMap["____"].GetSafeString();
            }
            catch { }
            return ret;
        }
    }
}