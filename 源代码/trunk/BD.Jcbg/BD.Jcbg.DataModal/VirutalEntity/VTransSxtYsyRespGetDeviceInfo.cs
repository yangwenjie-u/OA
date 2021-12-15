using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransSxtYsyRespGetDeviceInfo : VTransSxtYsyRespBase
    {
        public VTransSxtYsyRespGetDeviceInfoData data { get; set; }
    }
    public class VTransSxtYsyRespGetDeviceInfoData
    {
        public string deviceSerial { get; set; }
        public string deviceName { get; set; }
        public string model { get; set; }
        public int status { get; set; }
        public int defence { get; set; }
        public int isEncrypt { get; set; }
        public int alarmSoundMode { get; set; }
        public int offlineNotify { get; set; }
    }
}
