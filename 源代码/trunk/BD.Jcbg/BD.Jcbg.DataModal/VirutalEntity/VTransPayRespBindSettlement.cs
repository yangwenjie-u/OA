using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespBindSettlement : VTransPayRespBase
    {
        VTransPayRespBindSettlementData data { get; set; }
    }

    public class VTransPayRespBindSettlementData
    {
        public string UserId { get; set; }
    }
}
