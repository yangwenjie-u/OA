using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.JC.JS.Common
{
    /// <summary>
    /// 计算函数公共接口，所有计算都要实现该接口
    /// </summary>
    public interface ICalculate
    {
        /// <summary>
        /// 计算函数
        /// </summary>
        /// <param name="inparam">必有主表recid</param>
        /// <returns>Json格式：{"code":true,"msg":"错误内容"}</returns>
        string Calculate(string inparam);
    }
}
