using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespBasePage : VTransXcjcRespBase
    {
        public int totalcount { get; set; }
        public IList<IDictionary<string,string>> records { get; set; }
    }
}
