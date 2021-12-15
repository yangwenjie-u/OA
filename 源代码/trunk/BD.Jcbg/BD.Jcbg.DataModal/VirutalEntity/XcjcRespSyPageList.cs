using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class XcjcRespSyPageList : XcjcRespBase
    {
        public int totalcount { get; set; }

        public List<Dictionary<string, string>> records { get; set; }
    }
}
