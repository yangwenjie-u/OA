using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayAliRespBankInfo
    {
        public string cardType { get; set; }
        public string bank { get; set; }
        public string key { get; set; }
        public string[] messages { get; set; }
        public bool validated { get; set; }
        public string stat { get; set; }

    }
    public class VTransPayAliRespBankInfoErrorItem
    {
        public string errorCodes { get; set; }
        public string name { get; set; }
    }
    //var cardTypeMap = {
    //    DC: "储蓄卡",
    //    CC: "信用卡",
    //    SCC: "准贷记卡",
    //    PC: "预付费卡"
    //};
}
