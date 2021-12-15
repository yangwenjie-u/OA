using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    public enum BasicDataType
    {
        /// <summary>
        /// 工程
        /// </summary>
        GC,

        /// <summary>
        /// 企业
        /// </summary>
        QY,
        /// <summary>
        /// 人员
        /// </summary>
        RY,
        /// <summary>
        /// 设备
        /// </summary>
        SB
    }

    /// <summary>
    /// 委托单申请审核
    /// </summary>
    public enum WtdApplyAuditEnum
    {
        [Description("未审核")]
        NoAudit = 0,

        [Description("审核同意")]
        Agree = 1,

        [Description("审核未同意")]
        DisAgree = 2
    }

    /// <summary>
    /// 委托单申请审核
    /// </summary>
    public enum WtdXgsqEnum
    {
        [Description("未申请")]
        NoApply = 0,

        [Description("已申请")]
        Apply = 1,

        [Description("已同意修改")]
        Agree = 2,

        [Description("未同意修改")]
        DisAgree = 3
    }

    public enum WtdModifyTypeEnum
    {
        [Description("删除")]
        Delete = 1,

        [Description("修改")]
        Update = 2,

        [Description("添加")]
        Add = 3
    }

    public enum WtdModifyTableTypeEnum
    {
        [Description("主表")]
        First = 1,

        [Description("从表")]
        Second = 2,

        [Description("三级表")]
        Three = 3
    }
}
