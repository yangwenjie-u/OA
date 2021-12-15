using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
    /// <summary>
    /// 服务类型(QRCODE防伪二维码产生源)
    /// </summary>
    public enum ServiceEnum
    {
        ZJBD,                               //浙江标点接口
        XSXH                                //萧山协会接口
    }

    /// <summary>
    /// 接口返回字段类型(中文：CN，英文：EN）
    /// </summary>
    public enum InterfaceEnum
    {
        EN,                                 //英文
        CN                                  //中文
    }
}
