using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VTransYcbaPersonCertificate
    {
        [DataMember]
        public string recid { get; set; }

        [DataMember]
        public string rybh { get; set; }
        [DataMember]
        public string zzzslx { get; set; }
        [DataMember]
        public string zzzsbh { get; set; }
        [DataMember]
        public string fzjg { get; set; }
        [DataMember]
        public string sptg { get; set; }
        [DataMember]
        public string sfyx { get; set; }
        [DataMember]
        public string rylxbh { get; set; }
        [DataMember]
        public string zszybh { get; set; }
        [DataMember]
        public string fzrq { get; set; }
        [DataMember]
        public string zch { get; set; }
        [DataMember]
        public string bz { get; set; }
        [DataMember]
        public string zswj { get; set; }
        [DataMember]
        public string yxrq { get; set; }
    }
}
