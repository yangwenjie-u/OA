using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransReqSyncJcjgWtd : VTransXcjcReqBase
    {
        public string mjson { get; set; }

        public string sjson { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + mjson + sjson), StringComparison.OrdinalIgnoreCase);
        }
    }
}
