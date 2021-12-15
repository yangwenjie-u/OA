using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    /// <summary>
    /// 上报的报告文件
    /// </summary>
    [DataContract]
    public class VTransUpBgwj
    {
        [DataMember]
        public string bgwj { get; set; }
    }
}
