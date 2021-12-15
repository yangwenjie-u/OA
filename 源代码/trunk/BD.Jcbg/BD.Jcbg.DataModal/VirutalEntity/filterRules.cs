using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class filterRuleslist
    {
        public IList<filterRules> filterlist { get; set; }
    }

    public class filterRules
    {
        public string fieldname { get; set; }
        public string fieldvalue { get; set; }
        public string fieldopt { get; set; }
        public string filtertype { get; set; }
    }
}
