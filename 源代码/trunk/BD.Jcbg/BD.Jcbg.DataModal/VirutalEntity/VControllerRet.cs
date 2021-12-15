using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VContrllerRet
    {
        public string code { get; set; }
        public string msg { get; set; }
        public object[] record { get; set; }

        public string data { get; set; }

        public int totalcount { get; set; }

        public IList<IDictionary<string, string>> datas { get; set; }

        public VContrllerRet(bool ret, string msg)
        {
            code = ret ? "0" : "1";
            this.msg = msg;
        }

    }
}
