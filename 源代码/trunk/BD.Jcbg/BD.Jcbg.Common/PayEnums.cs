using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 提现状态
    /// </summary>
    public class PayStatus
    {
        public const int Disagree = -1;              //不同意提现
        public const int UnSubmit = 0;               // 待审批1
        public const int Submit = 1;                 // 已提交，未设置prepay
        public const int PrePay = 2;                 // 已设置prepay
        public const int Complete = 3;               // 提现完成
        public const int CompleteWithError = 4;      // 提现完成，至少有一笔错误
        public const int InWithdraw = 5;             // 提现中
        public const int Exception = 6;              // 异常
        public const int Cancel = 7;                 // 撤销
        public const int PrePayException = 8;        // prepay异常

        public static string GetDesc(int status)
        {
            var zt = "非正常";
            switch (status)
            {
                case Disagree:
                    zt = "不通过";
                    break;
                case UnSubmit:
                    zt = "待审批1";
                    break;
                case Submit:
                    zt = "审批1通过";
                    break;
                case PrePay:
                    zt = "待审批2";
                    break;
                case Complete:
                    zt = "发放完成";
                    break;
                case CompleteWithError:
                    zt = "提现异常";
                    break;
                case InWithdraw:
                    zt = "银行提现中";
                    break;
                case Exception:
                    zt = "异常";
                    break;
                case Cancel:
                    zt = "撤销";
                    break;
                case PrePayException:
                    zt = "预提现失败";
                    break;
            };
            return zt;
        }
        /// <summary>
        /// 获取当前状态的前一个可能的状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetPreStatus(int status)
        {
            string ret = "";
            switch (status)
            {
                case Disagree:
                    ret = PayStatus.PrePay.ToString() ;
                    break;
                case UnSubmit:
                    ret = "";
                    break;
                case Submit:
                    ret = PayStatus.UnSubmit.ToString() ;
                    break;
                case PrePay:
                    ret = PayStatus.Submit.ToString();
                    break;
                case Complete:
                    ret = PayStatus.InWithdraw.ToString();
                    break;
                case CompleteWithError:
                    ret = PayStatus.InWithdraw.ToString();
                    break;
                case InWithdraw:
                    ret = PayStatus.PrePay.ToString();
                    break;
                case Exception:
                    ret = PayStatus.InWithdraw.ToString();
                    break;
                case Cancel:
                    ret = PayStatus.InWithdraw.ToString();
                    break;
                case PrePayException:
                    ret = PayStatus.Submit.ToString();
                    break;
            };
            return ret;
        }

        public static string GetFinishString()
        {
            return Disagree + "," + Complete + "," + CompleteWithError + "," + Exception + "," + Cancel;// + "," + PrePayException;
        }

        public static string GetImportResultString()
        {
            return Complete + "," + CompleteWithError + "," + Exception ;
        }
    }

    public class BankPayDetialStaus
    {
        public const int Unsucceed = 0;              // 失败
        public const int Succeed = 1;                // 成功
        public const int Paying = 2;                 // 进行中（审批后）
        public const int Cancel = 3;                 // 撤销
        public const int Withdrawing = 4;            // 提现请求已提交，交易进行中
        public const int WaitforCheck = 5;           // 待审批
        public const int ErrorBindCard = 99;         // 绑卡错误
        public const int ErrorCancel = 98;           // 用户取消

        public static string GetDesc(int status)
        {
            var zt = "其他";
            switch (status)
            {
                case Unsucceed:
                    zt = "发放失败";
                    break;
                case Succeed:
                    zt = "发放成功";
                    break;
                case Paying:
                    zt = "发放中";
                    break;
                case Cancel:
                    zt = "已取消";
                    break;
                case Withdrawing:
                    zt = "银行提现中";
                    break;
                case WaitforCheck:
                    zt = "等待审批";
                    break;
                case ErrorBindCard:
                    zt = "绑卡失败";
                    break;
                case ErrorCancel:
                    zt = "用户取消";
                    break;
            };
            return zt;
        }

        public static bool IsFinish(int status)
        {
            return status == Unsucceed || status == Succeed || status == Cancel || status == ErrorCancel;
        }
        public static string GetFinishingString()
        {
            return Succeed+","+ Paying + "," + Withdrawing + "," + WaitforCheck + "," + ErrorBindCard+","+ErrorCancel;
        }
        public static string GetCanModifyStatus()
        {
            return Unsucceed + "," + Paying + "," + WaitforCheck + "," + ErrorBindCard;
        }
        public static bool CanModify(int zt)
        {
            return zt == Unsucceed || zt == Paying || zt == WaitforCheck || zt == ErrorBindCard;
        }

        public static bool CanCancel(int zt)
        {
            return zt == Paying;
        }
    }
    /// <summary>
    /// 绑卡状态
    /// </summary>
    public class BindCardStatus
    {
        public const int Binding = 0;   // 银行绑定中
        public const int Complete = 1;  // 绑卡成功
        public const int Error = 2;     // 绑卡失败
        
    }
}
