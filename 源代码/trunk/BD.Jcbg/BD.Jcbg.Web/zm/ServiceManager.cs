using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BD.Jcbg.Web.zm
{
    public class ServiceManager
    {
        public static object GetService(string serviceName)
        {
            try
            {
                IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                return webApplicationContext.GetObject(serviceName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}