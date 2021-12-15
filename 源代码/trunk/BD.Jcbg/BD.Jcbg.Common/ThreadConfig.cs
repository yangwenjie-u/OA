using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace BD.Jcbg.Common
{
    public class ThreadConfig
    {
        private IList<ThreadConfigItem> mThreads = null;

        public IList<ThreadConfigItem> Load()
        {
            mThreads = new List<ThreadConfigItem>();
            string msg = "";
            try
            {
                string configPath = string.Format(@"{0}\configs\threads.xml", SysEnvironment.CurPath);
                XDocument document = XDocument.Load(configPath);
                var query = from e in document.Elements("Threads").Elements("Thread") select e;
                foreach (XElement ele in query)
                {
                    ThreadConfigItem item = new ThreadConfigItem();
                    if (item.Load(ele, out msg))
                        mThreads.Add(item);
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return mThreads;
        }

    }

    public class ThreadConfigItem
    {
        public string ItemClass { get; set; }
        public string ItemDesc { get; set; }
        public int ItemInterval { get; set; }
        public bool ItemStart { get; set; }

        public bool Load(XElement ele, out string msg)
        {
            bool code = false;
            msg = "";
            try
            {
                ItemClass = ele.Attribute("class").Value.GetSafeString();
                if (ele.Attributes("desc").Count()>0)
                    ItemDesc = ele.Attribute("desc").Value.GetSafeString();
                if (ele.Attributes("interval").Count() > 0)
                    ItemInterval = ele.Attribute("interval").Value.GetSafeInt(60);
                if (ele.Attributes("start").Count() > 0)
                    ItemStart = ele.Attribute("start").Value.GetSafeBool();

                if (ItemClass != "")
                {
                    ItemClass = "BD.Jcbg.Web.threads." + ItemClass;
                    code = true;
                }
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }
            return code;
        }
    }
}
