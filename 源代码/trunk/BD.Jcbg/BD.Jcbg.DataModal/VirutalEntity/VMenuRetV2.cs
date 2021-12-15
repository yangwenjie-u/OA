using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VMenuRetV2
    {
        public string user_pic { get; set; }
        public string user_name { get; set; }

        public IList<VMenuRetV2Item1> one_caidan { get; set; }
        
    }

    public class VMenuRetV2Item1
    {
        public string one_caidan_pic_class { get; set; }
        public string one_caidan_name { get; set; }
        public string one_caidan_english { get; set; }
        public string topid { get; set; }

        public IList<VMenuRetV2Item2> two_caidan { get; set; }

 		#region 丁力  一级菜单url
        public string MenuUrl { get; set; }
        public string MenuId { get; set; }
        #endregion
    }

    public class VMenuRetV2Item2
    {
        public string two_caidan_pic_class { get; set; }

        public string two_caidan_name { get; set; }
        public string two_caidan_three { get; set; }

        public string MenuUrl { get; set; }
        public string MenuId { get; set; }

        public string IsOut { get; set; }
    }
}
