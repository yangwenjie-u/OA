using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VTransDownGetWtd
    {
        [DataMember]
        public string gcbh { get; set; }
        [DataMember]
        public string lrr { get; set; }
    }
}
