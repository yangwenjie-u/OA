using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransJzqyReqSyInfoByEwm : VTransXcjcReqBase
    {
        public string ewmbh { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + this.ewmbh), StringComparison.OrdinalIgnoreCase);
        }
    }
}
