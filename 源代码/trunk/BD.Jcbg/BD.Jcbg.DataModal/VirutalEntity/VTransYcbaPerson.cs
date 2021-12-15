using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VTransYcbaPerson
    {
        [DataMember]
        public VTransYcbaPersonBasicInfo 人员信息 { get; set; }
        [DataMember]
        public VTransYcbaPersonCertificate[] 人员资质 { get; set; }
    }
}
