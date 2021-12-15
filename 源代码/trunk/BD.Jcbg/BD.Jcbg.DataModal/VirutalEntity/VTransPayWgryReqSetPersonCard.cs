using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryReqSetPersonCard: VTransPayWgryReqBase
    {
        public VTransPayWgryReqSetPersonCardItem[] RootData
        {
            get
            {
                if (data == null)
                    return null;
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                VTransPayWgryReqSetPersonCardItem[] ret = jsonSerializer.Deserialize<VTransPayWgryReqSetPersonCardItem[]>(data);
                return ret;
            }
        }


    }
    public class VTransPayWgryReqSetPersonCardItem
    {
        //支付id，多个逗号分隔
        public string payids { get; set; }
        public string name { get; set; }
        public string idnumber { get; set; }
        public string fromcard { get; set; }        
        public string tocard { get; set; }
        public string tobankcode { get; set; }
        public string fromphone { get; set; }
        public string tophone { get; set; }
    }
}
