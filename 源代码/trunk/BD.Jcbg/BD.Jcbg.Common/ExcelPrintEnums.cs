using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 输入输出类型
    /// </summary>
    public enum EnumExcelPrintDir
    {
        Out,        // 输出字段
        In,         // 输入字段
        InOut       // 输入输出字段
    };
    /// <summary>
    /// 重复方向
    /// </summary>
    public enum EnumExcelPrintRepeater
    {
        Horizontal, // 横向重复
        Vertical,   // 纵向重复
        NoRepeater  //不重复
    };
    /// <summary>
    /// 汇总
    /// </summary>
    public enum EnumExcelPrintCollection
    {
        Count,      // 计数
        Top,        // 第一条
        Distinct,   // 取不同记录
        All,        // 所有记录
        NoCollection    // 不汇总
    };
    /// <summary>
    /// 输出类型
    /// </summary>
    public enum EnumExcelPrintOutput
    {
        Text,       // 文本
        Image,      // 图片
        Barcode1,   // 条形码
        Barcode2,   // 二维码
        Date,       // 日期
        DateTime,   // 日期时间
        Time,       // 时间
        Signature   // 签名 
    };
    /// <summary>
    /// 显示的日期或者日期时间格式
    /// </summary>
    public enum EnumExcelPrintOutputDate
    {
        Long,      // 长型
        Short      // 短型
    };
}
