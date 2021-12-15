using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcReqVideoInfo:VTransXcjcReqBase
    {
        public string ptbh { get; set; }
        public string videotype { get; set; }   // XC01:单兵视频
        public string datajson { get; set; }

        public override bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + sessionid + ptbh + videotype+datajson), StringComparison.OrdinalIgnoreCase);
        }
    }

    public class VTransXcjcReqVideoInfoItem
    {
        public string kssj { get; set; }    // 开始时间
        public string jssj { get; set; }    // 结束时间
        public string spwjm { get; set; }   // 视频文件名，带路径
        public string longitude { get; set; }
        public string latitude { get; set; }
    }
}
