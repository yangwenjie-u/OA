using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [DataContract]
    public class VBasicDataGetSb : BasicDataBase
    {
        public string ssdwbh { get; set; }          // 所属单位编号

        public string ssdwmc { get; set; }          // 所属单位名称

        public string sbbh { get; set; }            // 设备编号

        public string sbmc { get; set; }            // 设备名称

        public string sbxh { get; set; }            // 设备型号

        public string sccj { get; set; }            // 生产厂家

    }
}
