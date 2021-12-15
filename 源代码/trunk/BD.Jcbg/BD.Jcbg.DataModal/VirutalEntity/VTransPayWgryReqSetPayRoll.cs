using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryReqSetPayRoll:VTransPayWgryReqBase
    {
        public VTransPayWgryReqSetPayRollMain RootData
        {
            get
            {
                if (data == null)
                    return null;
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
                VTransPayWgryReqSetPayRollMain ret = jsonSerializer.Deserialize<VTransPayWgryReqSetPayRollMain>(data);
                return ret;
            }
        }
    }

    public class VTransPayWgryReqSetPayRollMain
    {
        public string paycode { get; set; }
        public string projectcode { get; set; }
        public string companycode { get; set; }
        public string paycompanycode { get; set; }
        public string payyear { get; set; }
        public string paymonth { get; set; }
        public string paytype { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string attach { get; set; }
        public string[] pictures { get; set; }
        public VTransPayWgryReqSetPayRollItem[] rows { get; set; }

    }

    public class VTransPayWgryReqSetPayRollItem
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string idnumber { get; set; }
        public string cardnumber { get; set; }
        public string banknumber { get; set; }
        public string paysum { get; set; }
        public string remark1 { get; set; }
    }
}
