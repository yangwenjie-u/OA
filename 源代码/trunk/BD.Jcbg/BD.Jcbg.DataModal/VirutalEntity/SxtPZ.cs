using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class SxtPZ
    {
        public IList<VTransXcjcReqStartItem> useSxts { get; set; }

        public string username { get; set; }
        public string realname { get; set; }
        public string wtdwyh { get; set; }

        /// <summary>
        /// 组号
        /// </summary>
        public string zh { get; set; }
    }
}
