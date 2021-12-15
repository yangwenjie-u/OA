using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Common
{
    public static class  JsonDynamicDeSerializer
    {
        public static IDictionary<string, object> DeSerializerObject(string jsonstr)
        {
            IDictionary<string, object> ret = null;
            try
            {
                JavaScriptSerializer s = new JavaScriptSerializer();
                ret = (IDictionary<string, object>)s.DeserializeObject(jsonstr);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
        public static IList<IDictionary<string, object>> DeSerializerArray(string jsonstr)
        {
            IList<IDictionary<string, object>> ret = new List<IDictionary<string,object>>();;
            try
            {
                JavaScriptSerializer s = new JavaScriptSerializer();
                object[] objs = (object[])s.DeserializeObject(jsonstr);
                for (int i = 0; i < objs.Length; i++)
                    ret.Add((IDictionary<string, object>)objs[i]);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
            return ret;
        }
    }
}
