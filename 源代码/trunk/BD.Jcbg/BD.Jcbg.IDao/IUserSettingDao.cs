using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface IUserSettingDao : IBaseDao<UserSetting, int>
    {
        /// <summary>
        /// 某个用户的所有设置
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IList<UserSetting> Gets(string username);
        /// <summary>
        /// 某个用户的某个设置项
        /// </summary>
        /// <param name="username"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        UserSetting Get(string username, string key);
    }
}
