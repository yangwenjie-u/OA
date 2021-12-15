using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	public interface ICompanyReaderDao : IBaseDao<CompanyReader, int>
	{
		/// <summary>
		/// 根据某个实体id删除
		/// </summary>
		/// <param name="entityname"></param>
		/// <param name="entityid"></param>
		void Delete(string entityname, string entityid);
		/// <summary>
		/// 获取某个实体的读者
		/// </summary>
		/// <param name="entityname"></param>
		/// <param name="entityid"></param>
		/// <returns></returns>
		IList<CompanyReader> Gets(string entityname, string entityid);
	}
}
