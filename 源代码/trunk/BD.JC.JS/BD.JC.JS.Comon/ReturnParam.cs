using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BD.JC.JS.Common
{
    public class ReturnParam
    {
        public bool code { get; set; }
        public string msg { get; set; }

        public ReturnParam()
        {

        }

        public ReturnParam(bool code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public string GetJson()
        {
            JavaScriptSerializer serial = new JavaScriptSerializer();
            return serial.Serialize(this);
        }
    }
}
