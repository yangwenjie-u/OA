using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcReqBasePage:VTransXcjcReqBase
    {
        public string key { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + this.key + pagesize.ToString() + pageindex.ToString()), StringComparison.OrdinalIgnoreCase);
        }
    }
}
