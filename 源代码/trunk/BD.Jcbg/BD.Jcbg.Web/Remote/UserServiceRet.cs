using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.Web.Remote
{
    [DataContract]
    public class UserServiceRet
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string data { get; set; }
        [DataMember]
        public string code { get; set; }

    }
}