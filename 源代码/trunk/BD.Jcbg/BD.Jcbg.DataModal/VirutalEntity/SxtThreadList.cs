using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SxtThreadList
    {
        //唯一号
        public string key { get; set; }

        //组号
        public string zh { get; set; }
        public Thread m_SxtThread { get; set; }
    }
}
