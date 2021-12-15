using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestRegister:SmsRequestBase
    {
        public SmsVarRegister contentVar;
    }
    public class SmsVarRegister
    {
        public string minute;
        public string code;
    }
    
}
