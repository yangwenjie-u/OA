using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VPayCreateBankAccountReturn
    {
        public bool Succeed { get; set; }
        public string ErrorMessage { get; set; }
        public string AccountUser { get; set; }  // 银行用户id
        public string AccountCode { get; set; }    // 银行帐号
        public string AccountName { get; set; }  // 银行账户名称
        public string BankPointName { get; set; }    // 银行网点名称
        public bool CreateAccountSucceed { get; set; }// 创建银行账户成功
        public bool BindCardSucceed { get; set; }          // 绑卡成功        
        public string BankErrorMessage { get; set; }    //银行错误信息
        public bool NeedCreateAccount { get; set; }     // 需要创建帐号
        public bool NeedBindCard { get; set; }          // 需要绑定卡号

        public VPayCreateBankAccountReturn()
        {
            Succeed = true;
            ErrorMessage = "";
            AccountUser = "";
            AccountCode = "";
            AccountName = "";
            BankPointName = "";
            CreateAccountSucceed = false;
            BindCardSucceed = false;
            BankErrorMessage = "";
            NeedCreateAccount = false;
            NeedBindCard = false;

        }
    }
}
