using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    /// <summary>
    /// 上报的重做数据
    /// </summary>
    public class VTransUpCzsj
    {
        [DataMember]
        public string syr { get; set; }
        [DataMember]
        public string sysb { get; set; }
        [DataMember]
        public string jh { get; set; }
        [DataMember]
        public string pzr { get; set; }
        [DataMember]
        public string czyy { get; set; }
        [DataMember]
        public string syqx { get; set; }
    }
}
