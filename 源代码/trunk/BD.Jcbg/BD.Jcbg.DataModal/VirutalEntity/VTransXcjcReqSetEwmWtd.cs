using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcReqSetEwmWtd:VTransXcjcReqBase
    {
        public string items { get; set; }   // 委托单编号,组号,二维码|
        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + items), StringComparison.OrdinalIgnoreCase);
        }
    }
}
