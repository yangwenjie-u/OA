using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    /// <summary>
    /// 上报的报告详情
    /// </summary>
    [DataContract]
    public class VTransUpBgxqm
    {
        [DataMember]
        public string zdhy { get; set; }
        [DataMember]
        public string zdz { get; set; } 
    }
}
