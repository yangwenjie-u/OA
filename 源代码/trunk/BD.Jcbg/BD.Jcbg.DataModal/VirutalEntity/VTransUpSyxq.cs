using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    /// <summary>
    /// 上报的采集数据
    /// </summary>
    [DataContract]
    public class VTransUpSyxq
    {
        [DataMember]
        public string zdmc { get; set; }

        [DataMember]
        public string zdhy { get; set; }
        [DataMember]
        public string zdz { get; set; }
    }
}
