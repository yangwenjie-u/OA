using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public class GcStatus
    {
        public static bool CanDelete(string zt)
        {
            return zt == "" || zt.Equals("lr", StringComparison.OrdinalIgnoreCase) || zt.Equals("yt", StringComparison.OrdinalIgnoreCase);
        }
    }
}
