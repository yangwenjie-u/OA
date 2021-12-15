using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespGetWithDrawCode : VTransPayRespBase
    {
        public VTransPayRespGetWithDrawCodeItem data;
    }

    public class VTransPayRespGetWithDrawCodeItem
    {
        public string CodeId { get; set; }
    }
}
