using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayRespGetPayList:VTransPayRespBase
    {
        public VTransPayRespGetPayListDataItem[] data { get; set; }
    }
    public class VTransPayRespGetPayListDataItem
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string OrderId { get; set; }
        public string PayCode { get; set; }        
        public string TransCodeId { get; set; }
        public string Amount { get; set; }      // 待发金额
        public string PayAmount { get; set; }   // 转账金额
        public string RealAmount { get; set; }  // 提现金额
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string SettleAccount { get; set; }
        public string PapersCode { get; set; }
        public string DrawTime { get; set; }        
        public string TradeComment { get; set; }
        public string TradeTime { get; set; }   // 转账或提现时间
        public string TradeState { get; set; }  // 0-失败，1-成功，2-进行中（审批后），3-撤销，4-提现请求已提交，交易进行中，5-待审批
        public string PayBankCode { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Remark4 { get; set; }
        public string Remark5 { get; set; }

        public string FaildCode { get; set; }

        public static string GetFaildDetail(string faildcode)
        {
            string errorInfo = "";
            switch (faildcode)
            {
                case "0":
                    errorInfo = "";
                    break;
                case "":
                    errorInfo = "";
                    break;
                case "1":
                    errorInfo = "转账失败";
                    break;
                case "2":
                    errorInfo = "提现失败";
                    break;
                case "3":
                    errorInfo = "二类卡限额";
                    break;
                case "5":
                    errorInfo = "绑卡失败";
                    break;
                default:
                    errorInfo = "";
                    break;
            }
            return errorInfo;
        }
    }
}
