using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryRespSetPersonCard:VTransPayWgryRespBase
    {
        public VTransPayWgryRespSetPersonCardItem[] rows { get; set; }
    }

    public class VTransPayWgryRespSetPersonCardItem
    {
        public string name { get; set; }
        public string idnumber { get; set; }
        public string fromcard { get; set; }
        public string tocard { get; set; }
        public string tobankcode { get; set; }
        public string fromphone { get; set; }
        public string tophone { get; set; }
        public string success { get; set; }
        public string message { get; set; }

        public void Load(VTransPayWgryReqSetPersonCardItem req)
        {
            this.name = req.name;
            this.idnumber = req.idnumber;
            this.fromcard = req.fromcard;
            this.tocard = req.tocard;
            this.tobankcode = req.tobankcode;
            this.fromphone = req.fromphone;
            this.tophone = req.tophone;
            success = VTransPayWgryRespBase.ErrorSuccess;
            message = VTransPayWgryRespBase.GetErrorInfo(success);
        }
    }
}
