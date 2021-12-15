using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespPay : VTransPayRespBase
    {
        public VTransPayRespPayDataItem data;
    }

    public class VTransPayRespPayDataItem
    {
        public string OrderId { get; set; }
    }
}
