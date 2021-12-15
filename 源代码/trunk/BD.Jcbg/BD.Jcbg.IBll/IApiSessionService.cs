using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.DataModal.VirutalEntity;

namespace BD.Jcbg.IBll
{
	public interface IApiSessionService
	{
        /// <summary>
        /// 添加session并缓存
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="usercode"></param>
        /// <param name="realname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool SetSession(string username, string password, string usercode, string realname, out string msg);
        /// <summary>
        /// 获取未过期的session
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        SysSession GetSessionUser(string sessionid, out string msg);
        /// <summary>
        /// 删除过期的session
        /// </summary>
        /// <param name="expirehours"></param>
        void DeleteExpireSessions(int expirehours);

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool DelSession(string sessionid, out string msg);
    }
}
