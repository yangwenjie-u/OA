using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespBase
    {
        public string code { get; set; }
        public string message { get; set; }

        public bool IsSucceed { get { return code == "000000"; } }
    }
}
