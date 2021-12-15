using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespPrePay: VTransPayRespBase
    {
        public VTransPayRespPrePayItem data;
    }

    public class VTransPayRespPrePayItem
    {
        public string TradeCode { get; set; }
    }
}
