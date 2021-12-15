using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	public interface ISelfDesktopItemDao:IBaseDao<SelfDesktopItem, int>
	{
		/// <summary>
		/// 获取用户桌面项
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<SelfDesktopItem> GetByUser(string username);
	}
}
