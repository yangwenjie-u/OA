using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransJzqyReqSyrysyxqList : VTransXcjcReqBase
    {
        public string syxmbh { get; set; }
        public string gcbh { get;set; }
        public string zt { get; set; }
        public string tprq1 { get; set; }
        public string tprq2 { get; set; }
        public string key { get; set; }
        public string qybh { get; set; }


        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + syxmbh + gcbh+ this.key +zt+ tprq1+tprq2+qybh), StringComparison.OrdinalIgnoreCase);
        }
    }
}
