using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class ZhwxWxEquipStatusBdMessage : SmsRequestBase
    {
        public EquipStatusBdMessage contentVar;
    }

    public class EquipStatusBdMessage {
        public string unitname;
        public string shebei;
        public string status;
        public string act;
    }
}
