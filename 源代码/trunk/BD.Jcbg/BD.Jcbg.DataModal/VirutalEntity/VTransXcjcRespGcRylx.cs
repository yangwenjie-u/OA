﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcRespGcRylx : VTransXcjcRespBase
    {
        public string username { get; set; }

        public string realname { get; set; }

        public IList<IDictionary<string, string>> gcs { get; set; }
    }
}
