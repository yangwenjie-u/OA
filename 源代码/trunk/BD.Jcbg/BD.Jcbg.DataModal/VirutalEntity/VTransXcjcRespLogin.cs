using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespLogin:VTransXcjcRespBase
    {
        public bool upsimcode { get; set; }

        public string sessionid { get; set; }

        public string usertype { get; set; }

        public string jcrjzh { get; set; }

        public string username { get; set; }

        public string realname { get; set; }

        public IList<IDictionary<string, string>> gcs { get; set; }
    }
}
