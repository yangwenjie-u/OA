using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    [Serializable]
    public class VCheckItem
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool open { get; set; }

        public bool ischeckecd{get;set;}        

        public bool chkDisabled { get; set; }

        public bool isParent { get; set; }

        public string cevent { get; set; }

        public string GetJsonStr()
        {
            return "{\"id\":\"" + id + "\",\"pId\":\"" + pId + "\",\"name\":\"" + name + "\",\"open\":" + open.ToString().ToLower() + ",\"checked\":" + ischeckecd.ToString().ToLower() + ",\"chkDisabled\":" + chkDisabled.ToString().ToLower() + "}";
        }

        public string GetNoCheckJsonStr()
        {
            return "{\"id\":\"" + id + "\",\"pId\":\"" + pId + "\",\"name\":\"" + name + "\",\"open\":" + open.ToString().ToLower() + ",\"isParent\":" + isParent.ToString().ToLower() + ",\"cevent\":\"" +cevent + "\"}";
        }
    }
}
