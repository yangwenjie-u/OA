﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransJzqyReqGetWtdInfo : VTransXcjcReqBase
    {
        public string ptbh { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + this.ptbh), StringComparison.OrdinalIgnoreCase);
        }
    }
}
