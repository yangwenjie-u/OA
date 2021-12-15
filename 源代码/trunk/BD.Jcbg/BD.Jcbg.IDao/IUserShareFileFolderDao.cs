using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	public interface IUserShareFileFolderDao : IBaseDao<UserShareFileFolder, int>
	{
		/// <summary>
		/// 获取用户的文件夹
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		IList<UserShareFileFolder> Gets(string username);
	}
}
