using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Linq;
using System.Web;
using System.Text;
using BD.Jcbg.Common;
using System.Web.Security;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.Remote
{
    public class LwService
    {
        public static string getAllGC()
        {
            return "";
            /*
            lwbgService.jcbgService srv = new lwbgService.jcbgService();
            srv.CookieContainer = CurrentUser.CurContainer;
            string timestring = GetTimeStamp();
            return srv.GetAllInfoGC(timestring, MD5Util.StringToMD5Hash(string.Format("timestring={0}&secret=lwbg", timestring)));*/
        }
        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }
    }
}