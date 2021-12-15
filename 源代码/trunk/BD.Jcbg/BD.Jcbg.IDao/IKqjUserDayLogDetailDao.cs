using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 日考勤日志详情表操作
	/// </summary>
	public interface IKqjUserDayLogDetailDao
	{
		/// <summary>
		/// 根据人员、工地、公司、日期获取考勤记录详情
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		IList<KqjUserDayLogDetail> Gets(string userid, string companyid, string projectid, DateTime dt);
        /// <summary>
        /// 获取没有出工地时间的记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="companyid"></param>
        /// <param name="projectid"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        IList<KqjUserDayLogDetail> GetsOut(string userid, string companyid, string projectid, DateTime dt);

        /// <summary>
        /// 获取没有进工地时间的记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="companyid"></param>
        /// <param name="projectid"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        IList<KqjUserDayLogDetail> GetsIn(string userid, string companyid, string projectid, DateTime dt);
		/// <summary>
		/// 保存考勤记录
		/// </summary>
		/// <param name="log"></param>
		/// <returns></returns>
		bool Save(KqjUserDayLogDetail log); 
	}
}
