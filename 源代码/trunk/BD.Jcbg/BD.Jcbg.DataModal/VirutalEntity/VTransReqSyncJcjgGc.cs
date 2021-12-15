using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransReqSyncJcjgGc : VTransXcjcReqBase
    {
        public string zjzbh { get; set; }

        public string gcbhyc { get; set; }

        public string qybh { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + zjzbh + gcbhyc + qybh), StringComparison.OrdinalIgnoreCase);
        }
    }
}
