using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class PersonCard
    {

        public string success { get; set; }
        public string message { get; set; }

        public List<PersonCardrows> rows { get; set; }
    }
    public class PersonCardrows
    {
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Fromcard { get; set; }
        public string Tocard { get; set; }
        public string ToBankCode { get; set; }
        public string Fromephone { get; set; }
        public string Tophone { get; set; }

    }
}
