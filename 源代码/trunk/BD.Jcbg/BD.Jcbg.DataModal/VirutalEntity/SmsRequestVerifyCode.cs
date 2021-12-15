using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestVerifyCode:SmsRequestBase
    {
        public SmsVarVerifyCode contentVar;
    }
    public class SmsVarVerifyCode
    {
        public string name;
        public string code;
        public string time;
    }
}
