using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespGetUserInfo: VTransPayRespBase
    {
        public VTransPayRespGetUserInfoDataItem[] data;
    }

    public class VTransPayRespGetUserInfoDataItem
    {
            public string UserId { get; set; }
            public string MemBerCode { get; set; }
            public string TradeMemBerName { get; set; }
            public string Currency { get; set; }
            public string SubAcc { get; set; }
            public string BoothNo { get; set; }
            public int TradeMemberProperty { get; set; }
            public string Contact { get; set; }
            public string ContactPhone { get; set; }
            public string Phone { get; set; }
            public string ContactAddr { get; set; }
            public string BusinessName { get; set; }
            public string PapersType { get; set; }
            public string PapersCode { get; set; }
            public int IsMessager { get; set; }
            public string MessagePhone { get; set; }
            public string Email { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string Remark3 { get; set; }
            public string Remark4 { get; set; }
            public string Remark5 { get; set; }
            public string SettleAccountName { get; set; }
            public string SettleAccount { get; set; }
            public int IsSecondAcc { get; set; }
            public string PayBank { get; set; }
            public string BankName { get; set; }
            public string CreatedTime { get; set; }
            public string ModifyTime { get; set; }
            public decimal AccountBalance { get; set; } // 账户余额
            public decimal FrozenBalance { get; set; }  // 冻结余额
    }

}
