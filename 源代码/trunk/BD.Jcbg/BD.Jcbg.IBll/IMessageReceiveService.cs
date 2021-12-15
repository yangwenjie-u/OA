using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface IMessageReceiveService
    {

        IList<MessageReceive> Gets();

        /// <summary>
        /// 根据Recid获取MessageReceive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MessageReceive Get(int Recid);

        /// <summary>
        /// 保存MessageReceive对象
        /// </summary>
        /// <param name="itm"></param>
        /// <returns></returns>
        int InsertData(List<MessageReceive> itms, string code, out string msg);
    }
}
