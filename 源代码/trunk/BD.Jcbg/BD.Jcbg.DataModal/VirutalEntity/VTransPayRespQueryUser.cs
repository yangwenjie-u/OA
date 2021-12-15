using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespQueryUser : VTransPayRespBase
    {
        public VTransPayRespQueryUserData data { get; set; }
    }

    public class VTransPayRespQueryUserData
    {
        public int pageindex { get; set; }
        public int total { get; set; }

        public VTransPayRespQueryUserDataItem[] data { get; set; }
    }

    public class VTransPayRespQueryUserDataItem
    {
        public string UserId { get; set; }          // 银行用户id
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
        public string SettleAccount { get; set; }       // 银行卡号
        public int IsSecondAcc { get; set; }
        public string PayBank { get; set; }
        public string BankName { get; set; }
        public string CreatedTime { get; set; }
        public string ModifyTime { get; set; }
        public decimal AccountBalance { get; set; } // 账户余额
        public decimal FrozenBalance { get; set; }  // 冻结余额
        public string SettleAccountCode { get; set; } // 绑卡是否成功,000000-表示成功
        public string SettleAccountMessage { get; set; }// 错误信息

        public string OthBankPayeeSubAcc { get; set; }              // 虚拟账户

        public string OthBankPayeeSubAccName { get; set; }          // 虚拟账户名称

        public string OthBankPayeeSubAccSetteName { get; set; }     // 开户网点名称

        public bool BindSucceed { get { return SettleAccountCode == "000000"; } }

    }
}
