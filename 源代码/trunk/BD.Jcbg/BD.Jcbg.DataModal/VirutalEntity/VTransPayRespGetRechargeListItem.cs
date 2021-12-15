using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespGetRechargeListItem
    {
        /*
         * 			"PayAcc": "4367421451657056193",
			"PayName": "冯海夫",
			"UserId": "110018120026844",
			"Amount": "0.01",
			"AccountBalance": "0.01",
			"TradeComment": "跨行转出",
			"TradeTime": "2018-12-17 22:35:31"
            */
        public string PayAcc { get; set; }
        public string PayName { get; set; }
        public string UserId { get; set; }
        public string Amount { get; set; }
        public string AccountBalance { get; set; }
        public string TradeComment { get; set; }
        public string TradeTime { get; set; }
    }
}
