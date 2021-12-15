using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransZjzPageProjectCondition
    {
        public string StationId { get; set; }
        public string Key { get; set; }
        public string Gcbh { get; set; }
        public string Gcmc { get; set; }
        public string Gcqy { get; set; }
        public string Gclx { get; set; }
        public string Jsdw { get; set; }
        public string Sgdw { get; set; }
        public string Jldw { get; set; }
        public string Jzry { get; set; }
        public string Syry { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        //排序 不传默认是：上传时间 desc
        public string OrderField { get; set; }
    }
}
