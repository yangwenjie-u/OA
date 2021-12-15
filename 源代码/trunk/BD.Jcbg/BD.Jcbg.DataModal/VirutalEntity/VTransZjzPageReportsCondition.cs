using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransZjzPageReportsCondition
    {
        public string StationId { get; set; }
        public string Key { get; set; }
        public string Jcdwbh { get; set; }
        public string Jcdwmc { get; set; }
        public string Wtdbh { get; set; }
        public string Syxmbh { get; set; }
        public string Syxmmc { get; set; }
        public string Bgbh { get; set; }
        public string Zjdjh { get; set; }
        public string Gcbh { get; set; }
        public string Gcmc { get; set; }
        public string Khdwmc { get; set; }
        public string Jcjg { get; set; }
        public string Qfrq1 { get; set; }
        public string Qfrq2 { get; set; }
        public string Scsj1 { get; set; }
        public string Scsj2 { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        //排序 不传默认是：上传时间 desc
        public string OrderField { get; set; }
    }
}
