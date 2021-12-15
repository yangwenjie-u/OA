using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class JzqyServiceResponseBase
    {
        public string Results { get; set; }

        public object Reason { get; set; }
    }

    public class JzqyUpLoadDataListPageResponse : JzqyServiceResponseBase 
    {
        public int Count { get; set; }
    }
}
