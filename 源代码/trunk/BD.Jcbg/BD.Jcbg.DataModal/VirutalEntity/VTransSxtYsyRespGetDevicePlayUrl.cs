using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransSxtYsyRespGetDevicePlayUrl : VTransSxtYsyRespBase
    {
        public VTransSxtYsyRespGetDevicePlayUrlData data { get; set; }
    }
    public class VTransSxtYsyRespGetDevicePlayUrlData
    {
        public string deviceSerial { get; set; }
        public int channelNo { get; set; }
        public string liveAddress { get; set; }
        public string hdAddress { get; set; }
        public string rtmp { get; set; }
        public string rtmpHd { get; set; }
        public int status { get; set; }
        public int exception { get; set; }
        public long beginTime { get; set; }
        public long endTime { get; set; }
    }
}
