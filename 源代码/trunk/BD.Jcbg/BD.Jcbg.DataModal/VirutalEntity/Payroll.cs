using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class Payroll
    {
        //发放编号（必须唯一，作为发放结果返回依据）
        public string paycode { get; set; }
        //工程编号
        public string projectcode { get; set; }
        //施工企业编号
        public string companycode { get; set; }
        //发放企业编号
        public string paycompanycode { get; set; }
        //工资时间年
        public string payyear { get; set; }
        //工资时间月
        public string paymonth { get; set; }
        //发放类型
        public string paytype { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }

        public string Attach { get; set; }
        //工资册详情详情未json数
        public List<Payrollrows> rows { get; set; }

    }

    public class Payrollrows
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string IdNumber { get; set; }
        public string CardNumber { get; set; }
        public string BankNumber { get; set; }
        public string Paysum { get; set; }
        public string Remark1 { get; set; }

    }
}
