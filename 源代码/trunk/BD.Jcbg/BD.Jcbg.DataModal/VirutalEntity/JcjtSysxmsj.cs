using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{

    public class JcjtSysxmsj
    {
        public string qybh { get; set; }
        public string syxmbh { get; set; }
        public List<JcjtSysxmcp> cprows { get; set; }
        public List<JcjtSysxmzb> zbrows { get; set; }
        public List<JcjtSysxmbz> bzrows { get; set; }
        public JcjtSysxmsj()
        {
            cprows = new List<JcjtSysxmcp>();
            zbrows = new List<JcjtSysxmzb>();
            bzrows = new List<JcjtSysxmbz>();
        }
    }

    public class JcjtSysxmcp
    {
        public string cpmc { get; set; }
    }
    public class JcjtSysxmzb
    {
        public string zbbh { get; set; }
        public string zbmc { get; set; }
    }
    public class JcjtSysxmbz
    {
        public string bzmc { get; set; }
        public string bzbh { get; set; }
    }
}
