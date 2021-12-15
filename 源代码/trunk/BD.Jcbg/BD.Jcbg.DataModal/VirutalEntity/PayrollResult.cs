using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class PayrollResult
    {
        public string key { get; set; }
        //发放编号（必须唯一，作为发放结果返回依据）
        public string Paycode { get; set; }
        //应付总额
        public string Shouldpay { get; set; }
        //实发总额
        public string Realpay { get; set; }
        //错误信息
        public string Message { get; set; }

        //工资册详情详情未json数
        public List<PayrollResultrows> rows { get; set; }
    }
    public class PayrollResultrows
    {
        //姓名
        public string Name { get; set; }
        //电话
        public string Phone { get; set; }
        //身份证号
        public string IdNumber { get; set; }
        //银行卡号
        public string CardNumber { get; set; }
        public string Shouldpay { get; set; }
        public string Realpay { get; set; }
        //错误信息
        public string message { get; set; }

    }
}
