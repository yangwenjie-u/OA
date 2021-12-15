using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace BD.Jcbg.Service.model.jc
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
