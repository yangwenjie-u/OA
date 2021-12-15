using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class ZhwxWxBindBdMessage : SmsRequestBase
    {
        public BindBdMessage contentVar;
    }

    public class BindBdMessage
    {
        public string unitname;
        public string code;
        public string time;
       
     }
}
