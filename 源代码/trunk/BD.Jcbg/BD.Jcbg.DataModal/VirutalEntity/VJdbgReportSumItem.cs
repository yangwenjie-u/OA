using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
    public class VJdbgReportSumItem
    {
        public const string JDFA = "JDFA";  // 监督方案
        public const string JDJD = "JDJD";  // 监督交底
        public const string JDBG = "JDBG";      // 监督报告
        public const string GDZL = "GDZL";      // 归档资料
        public const string ZLXWJCJL = "ZLXWJCJL";// 质量行为检查记录
        public const string JDJL = "JDJL";// 监督记录
        public const string ZGD = "ZGD";    // 整改单
        public const string ZGD_SP = "ZGD_SP";// 审批过的整改单
        public const string BHGBG = "BHGBG";    // 不合格报告
        public const string RYLZJL = "RYLZJL";    // 人员离职记录
        public const string YSSQJL = "YSSQJL";    // 验收申请记录
        public const string YSAPJL = "YSAPJL";    // 验收安排记录
        public const string JGYSJL = "JGYSJL";      // 竣工验收记录
        public const string QYLZJL = "QYLZJL";    // 企业离职记录
        public const string JDYBZ = "JDYBZ";    // 监督员

        public int SumJDFA { get; set; }
        public int SumJDJD { get; set; }
        public int SumJDBG { get; set; }
        public int SumGDZL { get; set; }
        public int SumZLXWJCJL { get; set; }
        public int SumJDJL { get; set; }
        public int SumZgd { get; set; }

        public int SumZgdSp { get; set; }
        public int SumRYLZJL { get; set; }

        public int SumYSSQJL { get; set; }

        public int SumYSAPJL { get; set; }

        public int SumJGYSJL { get; set; }

        public int SumQYLZJL { get; set; }

        public int SumJDYBZ { get; set; }

        public void SetSum(string lx, int sum)
        {
            lx = lx.ToUpper();
            if (lx.Equals(JDFA, StringComparison.OrdinalIgnoreCase))
                SumJDFA = sum;
            else if (lx.Equals(JDJD, StringComparison.OrdinalIgnoreCase))
                SumJDJD = sum;
            else if (lx.Equals(JDBG, StringComparison.OrdinalIgnoreCase))
                SumJDBG = sum;
            else if (lx.Equals(GDZL, StringComparison.OrdinalIgnoreCase))
                SumGDZL = sum;
            else if (lx.Equals(ZLXWJCJL, StringComparison.OrdinalIgnoreCase))
                SumZLXWJCJL = sum;
            else if (lx.Equals(JDJL, StringComparison.OrdinalIgnoreCase))
                SumJDJL = sum;
            else if (lx.Equals(ZGD, StringComparison.OrdinalIgnoreCase))
                SumZgd = sum;
            else if (lx.Equals(RYLZJL, StringComparison.OrdinalIgnoreCase))
                SumRYLZJL = sum;
            else if (lx.Equals(YSSQJL, StringComparison.OrdinalIgnoreCase))
                SumYSSQJL = sum;
            else if (lx.Equals(YSAPJL, StringComparison.OrdinalIgnoreCase))
                SumYSAPJL = sum;
            else if (lx.Equals(JGYSJL, StringComparison.OrdinalIgnoreCase))
                SumJGYSJL = sum;
            else if (lx.Equals(ZGD_SP, StringComparison.OrdinalIgnoreCase))
                SumZgdSp = sum;
            else if (lx.Equals(QYLZJL, StringComparison.OrdinalIgnoreCase))
                SumQYLZJL = sum;
            else if (lx.Equals(JDYBZ, StringComparison.OrdinalIgnoreCase))
                SumJDYBZ = sum;

        }
    }
}
