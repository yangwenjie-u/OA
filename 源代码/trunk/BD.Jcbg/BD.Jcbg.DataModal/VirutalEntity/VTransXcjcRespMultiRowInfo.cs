using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespMultiRowInfo:VTransXcjcRespBase
    {
        public IList<IDictionary<string, object>> records { get; set; }
    }
}
