using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface ISmsServiceWzzjz
    {
        #region 发送短消息
        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="receiver"></param>
        /// <param name="content"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SendMessage(string applicationid, string groupid, string receiver, string contents, out string msg);
        bool SendMessageV2(string applicationid, string groupid, string receiver, string contents, out string msg);
        #endregion
    }
}
