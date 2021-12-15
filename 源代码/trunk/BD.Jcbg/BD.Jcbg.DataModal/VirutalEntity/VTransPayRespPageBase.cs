using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespPageBase<T>:VTransPayRespBase
    {
        public VTransPayRespPageSummaryInfo<T> data { get; set; }
    }

    public class VTransPayRespPageSummaryInfo<T>
    {
        public int total { get; set; }
        public int pageindex { get; set; }

        public T[] data { get; set; }
    }
}
