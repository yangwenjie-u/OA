using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface ISmsServiceWgry
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
        /// <summary>
        /// 获取需要发送的短信信息
        /// </summary>
        /// <returns></returns>
        IList<IDictionary<string, string>> GetSMSdata();


        bool DoSendMessage(string phone, string content, string guid, string lx);

        bool SaveSMSMsg(string msg,string sjhms,string LX);
        #endregion
    }
}
