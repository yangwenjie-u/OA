using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespGetSyxms : VTransXcjcRespBase
    {
        public IList<IDictionary<string, object>> syxms { get; set; }
    }
}
