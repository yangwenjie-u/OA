using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 务工人员单位工程历史
	/// </summary>
	public interface IInfoWgryHistoryDao
	{
		/// <summary>
		/// 保存信息
		/// </summary>
		/// <param name="itm"></param>
		/// <returns></returns>
		bool Save(InfoWgryHistory itm);
		/// <summary>
		/// 获取某个人，在某个单位的历史的信息，如果没有单位，返回所有单位的
		/// </summary>
		/// <param name="sfzh"></param>
		/// <param name="companyid"></param>
		/// <returns></returns>
		IList<InfoWgryHistory> Gets(string sfzh, string companyid,string projectid="");

        IList<InfoWgryHistory> GetsOut(string sfzh, string companyid, string projectid = "");
		/// <summary>
		/// 获取历史信息
		/// </summary>
		/// <param name="sfzh"></param>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <param name="companyname"></param>
		/// <param name="projectname"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindex"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		IList<InfoWgryHistory> Gets(string sfzh,string realname, string companyid, string projectid, string companyname, string projectname,
			int pagesize, int pageindex, out int totalCount);
	}
}
