using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestUserInfo : SmsRequestBase
    {
        public SmsVarUserInfo contentVar;
    }

    public class SmsVarUserInfo
    {
        public string username;
        public string password;
    }
}
