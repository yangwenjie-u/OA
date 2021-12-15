using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 文件存储方式(1-OSS, 2-数据库)
    /// </summary>
    public enum SysFileStorageEnum
    {
        [Description("上传到OSS")]
        OSS = 1,

        [Description("上传到数据库")]
        SqlData = 2
    }

    /// <summary>
    /// 温州监管建研新见证流程(0-不启用, 1-启用)
    /// </summary>
    public enum SysWzJgJyNewJzqyEnum
    { 
        [Description("不启用")]
        UnEnabled = 0,

        [Description("启用")]
        Enabled = 1
    }

    /// <summary>
    /// 人员录用申请(0-不启用, 1-启用)
    /// </summary>
    public enum SysRylysqEnum
    {
        [Description("不启用")]
        UnEnabled = 0,

        [Description("启用")]
        Enabled = 1
    }

    /// <summary>
    /// 一个见证人只允许一个工程
    /// </summary>
    public enum SysJzryZyxOneGcEnum
    {
        [Description("不启用")]
        UnEnabled = 0,

        [Description("启用")]
        Enabled = 1
    }

    /// <summary>
    /// 检测机构区域审批(0-不启用,1-启用)
    /// </summary>
    public enum SysJcjgQySpEnum
    {
        [Description("不启用")]
        UnEnabled = 0,

        [Description("启用")]
        Enabled = 1
    }
}
