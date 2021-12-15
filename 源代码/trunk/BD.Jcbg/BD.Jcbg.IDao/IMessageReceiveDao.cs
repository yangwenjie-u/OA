using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface IMessageReceiveDao : IBaseDao<MessageReceive, int>
	{

        IList<MessageReceive> Gets();

        /// <summary>
        /// 保存MessageReceive对象
        /// </summary>
        /// <param name="itms"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        int InsertData(string sql, out string msg);
	}
}
