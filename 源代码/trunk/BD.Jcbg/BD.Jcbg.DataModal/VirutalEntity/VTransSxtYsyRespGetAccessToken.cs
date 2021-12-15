using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransSxtYsyRespGetAccessToken:VTransSxtYsyRespBase
    {
        public VTransSxtYsyRespGetAccessTokenData data { get; set; }
    }
    public class VTransSxtYsyRespGetAccessTokenData
    {
        public string accessToken { get; set; }
        public string expireTime { get; set; }
    }
}
