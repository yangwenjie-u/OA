using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 日考勤日志表操作
	/// </summary>
	public interface IKqjUserDayLogDao
	{
		/// <summary>
		/// 根据人员、工地、公司、日期获取考勤记录
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		IList<KqjUserDayLog> Gets(string userid, string companyid, string projectid, DateTime dt);
		/// <summary>
		/// 保存考勤记录
		/// </summary>
		/// <param name="log"></param>
		/// <returns></returns>
		bool Save(KqjUserDayLog log);
		/// <summary>
		/// 根据记录号获取记录
		/// </summary>
		/// <param name="recid"></param>
		/// <returns></returns>
		KqjUserDayLog Get(int recid);

        /// <summary>
        /// 根据人员、工地、公司、日期获取考勤记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="companyid"></param>
        /// <param name="projectid"></param>
        /// <param name="dt"></param>
        /// <param name="schedult"></param>
        /// <returns></returns>
        IList<KqjUserDayLog> GetDayLogs(string userid, string companyid, string projectid, DateTime dt, InfoSchedule schedult);
		/// <summary>
		/// 根据单位删除记录
		/// </summary>
		/// <param name="companyid"></param>
		/// <returns></returns>
		bool Delete(string companyid);
	}
}
