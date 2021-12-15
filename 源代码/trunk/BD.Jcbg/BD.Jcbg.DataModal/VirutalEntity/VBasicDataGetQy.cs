using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VBasicDataGetQy: BasicDataBase
    {
        [DataMember]
        public string qybh { get; set; }    // 企业编号
        [DataMember]
        public string qymc { get; set; }    // 企业名称
    }
}
