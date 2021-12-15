using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 排班操作
	/// </summary>
	public interface IInfoScheduleDao
	{
		/// <summary>
		/// 获取一个单位一个工地的排班设置
		/// </summary>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		IList<InfoSchedule> Gets(string companyid, string projectid);
		/// <summary>
		/// 获取排班
		/// </summary>
		/// <param name="recid"></param>
		/// <returns></returns>
		InfoSchedule Get(string recid);
	}
}
