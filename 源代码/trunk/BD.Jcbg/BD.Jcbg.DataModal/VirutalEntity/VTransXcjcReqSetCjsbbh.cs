using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransXcjcReqSetCjsbbh
    {
        public string wtdwyh { get; set; }
        public string zhuanghao { get; set; }
        public string cjsybh { get; set; }
        public string checkcode { get; set; }
        

        public bool IsValid(string key)
        {
            return checkcode.Equals(MD5Util.GetCommonMD5(key + wtdwyh + cjsybh));
        }

        public bool NeedDeal
        {
            get
            {
                return !string.IsNullOrEmpty(wtdwyh) && !string.IsNullOrEmpty(cjsybh);
            }
        }
    }
}
