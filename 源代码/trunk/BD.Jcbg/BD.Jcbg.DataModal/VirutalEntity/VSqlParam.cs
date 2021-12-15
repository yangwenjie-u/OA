using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VSqlParam
    {
        public bool IsDynamic { get; set; }
        public string ParamName { get; set; }
        public object ParamValue { get; set; }
    }
}
