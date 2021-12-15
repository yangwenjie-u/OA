using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class WXJSON
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public IList<IDictionary<string, string>> Datas { get; set; }
    }
}
