using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestPayVerify: SmsRequestBase
    {
        public SmsVarPayVerify contentVar;
    }

    public class SmsVarPayVerify
    {
        public string time;
        public string code;
    }
}
