using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VBasicDataGetGc: BasicDataBase
    {
        [DataMember]
        public string gcbh { get; set; }    // 工程编号
        [DataMember]
        public string gcmc { get; set; }    // 工程名称

    }
}
