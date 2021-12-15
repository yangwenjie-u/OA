using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryReqBase
    {
        public string key { get; set; }
        public string data { get; set; }

        public bool IsValid(string seed)
        {
            return key.Equals(MD5Util.GetCommonMD5(seed), StringComparison.OrdinalIgnoreCase);
        }
    }
}
