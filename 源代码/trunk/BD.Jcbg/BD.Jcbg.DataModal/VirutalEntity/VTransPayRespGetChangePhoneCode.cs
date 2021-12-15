using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespGetChangePhoneCode: VTransPayRespBase
    {
        public VTransPayRespGetChangePhoneCodeItem data;
    }

    public class VTransPayRespGetChangePhoneCodeItem
    {
        public string CodeId { get; set; }
    }
}
