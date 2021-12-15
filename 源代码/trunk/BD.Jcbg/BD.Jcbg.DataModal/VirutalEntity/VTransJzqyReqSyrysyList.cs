using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransJzqyReqSyrysyList : VTransXcjcReqBasePage
    {
        public string syxmbh { get; set; }
        public string gcbh { get; set; }
        public string zt { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + syxmbh +  gcbh + this.key + zt + pagesize.ToString() + pageindex.ToString()), StringComparison.OrdinalIgnoreCase);
        }
    }
}
