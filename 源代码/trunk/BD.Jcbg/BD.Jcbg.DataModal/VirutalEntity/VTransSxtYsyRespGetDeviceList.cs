using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransSxtYsyRespGetDeviceList : VTransSxtYsyRespBase
    {
        public VTransSxtYsyRespPage page;
        public VTransSxtYsyRespGetDeviceListItem[] data;
    }
    public class VTransSxtYsyRespGetDeviceListItem
    {
        public string deviceSerial { get; set; }
        public string deviceName { get; set; }
        public string deviceType { get; set; }
        public int status { get; set; }
        public int defence { get; set; }
        public string deviceVersion { get; set; }
    }
}
