using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class ZhwxUniversalMessage : SmsRequestBase
    {
        public UniversalMessage contentVar;
    }


    public class UniversalMessage
        {
        public string client;
        public string info;
       
     }
}
