using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Common
{
	/// <summary>
	/// Json格式转换
	/// </summary>
	public static class JsonFormat
	{
		public static string GetRetString(object status)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			Dictionary<string, string> row = new Dictionary<string, string>();
			row.Add("code", ((int)status).ToString());
			row.Add("msg", StringResource.GetEnumString(status));

			return jss.Serialize(row);
		}

		public static string GetRetString(object status, object record)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			return string.Format("{{\"code\":\"{0}\",\"record\":[{1}]}}", ((int)status).ToString(), jss.Serialize(record));
		}

        public static string GetRetString(bool ret, string msg, string data)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, string> row = new Dictionary<string, string>();
			row.Add("code", ret ? "0" : "1");
			row.Add("msg", msg);
            row.Add("data", data);
            return jss.Serialize(row);
        }


		public static string GetRetString()
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			Dictionary<string, string> row = new Dictionary<string, string>();
			row.Add("code", "0");
			row.Add("msg", "");

			return jss.Serialize(row);
		}
		public static string GetRetString(bool ret, string msg = "")
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			Dictionary<string, string> row = new Dictionary<string, string>();
			row.Add("code", ret ? "0" : "1");
			row.Add("msg", msg);

			return jss.Serialize(row);
		}
		public static string GetRetString(string msg)
		{
			JavaScriptSerializer jss = new JavaScriptSerializer();
			Dictionary<string, string> row = new Dictionary<string, string>();
			row.Add("code", "0");
			row.Add("msg", msg);

			return jss.Serialize(row);
		}

        public static string GetRetString(bool ret, string msg, int totalcount, IList<IDictionary<string, string>> datas)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> pagedata = new Dictionary<string, object>();
            pagedata.Add("code", ret ? "0" : "1");
            pagedata.Add("msg", msg);
            pagedata.Add("totalcount", totalcount);
            pagedata.Add("datas", datas);
            return jss.Serialize(pagedata);
        }

        public static string GetString(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static string GetRetString(bool ret, string msg, object datas)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> pagedata = new Dictionary<string, object>();
            pagedata.Add("code", ret ? "0" : "1");
            pagedata.Add("msg", msg);
            pagedata.Add("datas", datas);
            return jss.Serialize(pagedata);
        }

        public static string GetRetString(bool ret, string msg, object data, object datas)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> pagedata = new Dictionary<string, object>();
            pagedata.Add("code", ret ? "0" : "1");
            pagedata.Add("msg", msg);
            pagedata.Add("data", data);
            pagedata.Add("datas", datas);
            return jss.Serialize(pagedata);
        }
	}
}
