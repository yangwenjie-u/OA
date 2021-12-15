using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestMessage:SmsRequestBase
    {
        public SmsVarMessage contentVar;
    }
    public class SmsVarMessage
    {
        public string client;
        public string info;
    }
}
