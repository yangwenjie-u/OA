using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VTransDownGetJcht
    {
        [DataMember]
        public string htlx { get; set; }        // 合同类型名称，全匹配，选项:企业合同,监督合同
        [DataMember]
        public string jchtbh { get; set; }        // 合同编号
        [DataMember]
        public string gcbh { get; set; }        // 工程编号
        [DataMember]
        public string gcmc { get; set; }        // 工程名称
        [DataMember]
        public string khdwmc { get; set; }        // 客户单位名称
        [DataMember]
        public string zjzmc { get; set; }        // 质监站名称
        [DataMember]
        public string zjdjh { get; set; }        // 质监登记号
        [DataMember]
        public string syrxm { get; set; }        // 送样人姓名
        [DataMember]
        public string sybmmc { get; set; }        // 送样部门名称
        [DataMember]
        public string gsbmmc { get; set; }        // 归属部门名称
        [DataMember]
        public string htqdr { get; set; }        // 合同签订人

    }
}
