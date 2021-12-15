using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespLogin2 : VTransXcjcRespBase
    {
        public bool upsimcode { get; set; }
        public string sessionid { get; set; }
        public string usertype { get; set; }

        public string jcrjzh { get; set; }
    }
}
