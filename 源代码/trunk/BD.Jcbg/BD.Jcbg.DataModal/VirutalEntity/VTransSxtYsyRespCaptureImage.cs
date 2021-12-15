using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransSxtYsyRespCaptureImage : VTransSxtYsyRespBase
    {
        public VTransSxtYsyRespCaptureImageData data { get; set; }
    }

    public class VTransSxtYsyRespCaptureImageData
    {
        public string picUrl { get; set; }
    }
}
