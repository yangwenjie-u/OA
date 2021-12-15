using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VBasicDataGetRy: BasicDataBase
    {
        [DataMember]
        public string rybh { get; set; }    // 人员编号
        [DataMember]
        public string ryxm { get; set; }    // 人员姓名
    }
}
