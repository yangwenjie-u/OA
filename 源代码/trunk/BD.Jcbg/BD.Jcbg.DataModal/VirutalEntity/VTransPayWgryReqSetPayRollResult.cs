using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VTransPayWgryReqSetPayRollResult: VTransPayWgryReqBase
    {
        //发放编号（必须唯一，作为发放结果返回依据）
        public string Paycode { get; set; }
        //应付总额
        public string Shouldpay { get; set; }
        //实发总额
        public string Realpay { get; set; }
        //错误信息
        public string Message { get; set; }
        public string Code { get; set; }

        // 工程编号
        public string ProjectCode { get; set; }
        // 施工单位编号
        public string CompanyCodeSg { get; set; }
        // 劳务公司编号
        public string CompanyCodePay { get; set; }
        // 方法平台流水号
        public string SerialCode { get; set; }
        // 工资年份
        public int PayYear { get; set; }
        // 工资月份
        public int PayMonth { get; set; }
        // 支付平台工程编号
        public string PayProjectCode { get; set; }
        // 支付平台工程名称
        public string PayProjectName { get; set; }
        // 省
        public string Province { get; set; }
        // 市
        public string City { get; set; }
        // 区（县）
        public string Area { get; set; }
        

        //工资册详情详情未json数
        public List<VTransPayWgryReqSetPayRollResultItem> rows { get; set; }
    }

    public class VTransPayWgryReqSetPayRollResultItem
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
        // 错误代码
        public string Code { get; set; }
        // 是否二类卡，1-二类卡，其他-不是
        public string SubCard { get; set; }
    }
}
