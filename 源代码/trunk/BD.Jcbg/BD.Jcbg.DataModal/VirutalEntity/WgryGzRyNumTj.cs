using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class WgryGzTj
    {
        public string Gz { get; set; }

        public WgryGzRynumTj Datas;
    }

    public class WgryGzRynumTj
    {
        public string data1 { get; set; } //登记人员
        public string data2 { get; set; } //在职人员
        public string data3 { get; set; } //当前人员
        public string data4 { get; set; } //当天考勤人员

        public string shouldpaynum { get; set; } //应付

        public string bankpaynum { get; set; } //实付

        public string allcw { get; set; } //总床位

        public string usecw { get; set; } //占用床位
    }

}
