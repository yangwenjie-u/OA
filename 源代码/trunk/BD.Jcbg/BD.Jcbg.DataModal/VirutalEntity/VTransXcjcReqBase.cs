using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcReqBase
    {
        public string sessionid { get; set; }
        public string checkcode { get; set; }

        public virtual bool IsValid(string key)
        {
            return false;
        }
    }
}
