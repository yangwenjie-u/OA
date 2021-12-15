using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BD.Jcbg.Common
{
	/// <summary>
	/// 客户端信息
	/// </summary>
	public static class ClientInfo
	{
		public static string Ip
		{
			get
			{
				HttpContext context = System.Web.HttpContext.Current;
				string result = String.Empty;
				if (context == null)
					return result;

				try
				{
					// 透过代理取真实IP
					result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
					if (null == result || result == String.Empty)
						result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

					if (null == result || result == String.Empty)
						result = HttpContext.Current.Request.UserHostAddress;
				}
				catch { }

				return result;
			}
		}

        public static string GetXcjcImageUrl(HttpRequest request, string func)
        {
            string Port = request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string sOut = Protocol + request.ServerVariables["SERVER_NAME"] + Port + "/api/apixcjc/" + func;
            return sOut;
        }
        public static string GetXcjcImageUrl(HttpRequestBase request, string func)
        {
            string Port = request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string sOut = Protocol + request.ServerVariables["SERVER_NAME"] + Port + "/api/apixcjc/" + func;
            return sOut;
        }

        public static string GetCurDns(HttpRequestBase request)
        {
            string Port = request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string sOut = Protocol + request.ServerVariables["SERVER_NAME"] + Port + "/";
            return sOut;
        }

        public static string GetCurDnsWithPort(HttpRequestBase request)
        {
            string Port = request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;
            
            string sOut = request.ServerVariables["SERVER_NAME"] + Port;
            return sOut;
        }
        public static string GetCurDnsWithPort(HttpRequest request)
        {
            string Port = request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;
            
            string sOut = request.ServerVariables["SERVER_NAME"] + Port;
            return sOut;
        }
    }
}
