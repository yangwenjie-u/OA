using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SmsRequestBase
    {
        public string invokeId { get; set; }
        public string phoneNumber { get; set; }
        public string templateCode { get; set; }
    }
}
