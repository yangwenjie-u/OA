using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestMessage3 : SmsRequestBase
    {
        public SmsVarMessage3 contentVar;
    }
    public class SmsVarMessage3
    {
        public string client;
        public string info1;
        public string info2;
        public string info3;
    }
}
