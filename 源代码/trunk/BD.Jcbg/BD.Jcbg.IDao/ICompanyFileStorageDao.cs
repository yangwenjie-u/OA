using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	public interface ICompanyFileStorageDao : IBaseDao<CompanyFileStorage, int>
	{
		/// <summary>
		/// 根据多个id获取文件
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		IList<CompanyFileStorage> Gets(string ids);
	}
}
