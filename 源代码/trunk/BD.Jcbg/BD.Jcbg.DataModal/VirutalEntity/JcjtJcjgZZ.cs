using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class JcjtJcjgZZ
    {
        public string xsflbh { get; set; }
        public string xsflmc { get; set; }
        public string sfyx { get; set; }

        public List<JcjtJcjgZZFL> rows { get; set; }

        public JcjtJcjgZZ()
        {
            rows = new List<JcjtJcjgZZFL>();
        }
    }

    public class JcjtJcjgZZFL
    {
        public string sjxsflbh { get; set; }
        public string xsflbh { get; set; }
        public string xsflmc { get; set; }
        public string sfyx { get; set; }

        public List<JcjtJcjgXM> xmrows { get; set; }

        public JcjtJcjgZZFL()
        {
            xmrows = new List<JcjtJcjgXM>();
        }
    }

    public class JcjtJcjgXM
    {
        public string syxmbh { get; set; }
        public string syxmmc { get; set; }

        public string sfyx { get; set; }
    }

    public class JcjtJcryRoleSYXM
    {
        public string rolecode { get; set; }
        public string syxmbhs { get; set; }

    }

}
