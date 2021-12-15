using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VTransDownGetGc
    {
        [DataMember]
        public string gcbh { get; set; }    // 工程编号
        [DataMember]
        public string gcmc { get; set; }    // 工程名称
        [DataMember]
        public string gcqy { get; set; }    // 工程区域
        [DataMember]
        public string gclx { get; set; }    // 工程类型
        [DataMember]
        public string jsdw { get; set; }    // 建设单位
        [DataMember]
        public string sgdw { get; set; }    // 施工单位
        [DataMember]
        public string jldw { get; set; }    // 监理单位
        [DataMember]
        public string jzry { get; set; }    // 见证人员
        [DataMember]
        public string syry { get; set; }    // 送样人员
    }
}
