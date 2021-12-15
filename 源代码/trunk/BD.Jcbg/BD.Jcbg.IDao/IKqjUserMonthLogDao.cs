using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 用户月考勤记录操作
	/// </summary>
	public interface IKqjUserMonthLogDao
	{
		/// <summary>
		/// 身份证+单位+工种+班组负责人+年+月 唯一确定一条记录
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="companyid"></param>
		/// <param name="gzid"></param>
		/// <param name="bzfzr"></param>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <returns></returns>
        KqjUserMonthLog Get(string userid, string companyid, string gzid, int year, int month, string ScheduleId);
		/// <summary>
		/// 保存某天的考勤记录
		/// </summary>
		/// <param name="recid"></param>
		/// <param name="day"></param>
		/// <param name="logtype"></param>
		/// <returns></returns>
        bool SetLog(int recid, int day, string logtype, DateTime dt);
		/// <summary>
		/// 插入一个考勤记录
		/// </summary>
		/// <param name="KqjUserDayLog"></param>
		/// <returns></returns>
		void SaveLog(KqjUserMonthLog log);
		/// <summary>
		/// 获取考勤列表
		/// </summary>
		/// <param name="sfzhm"></param>
		/// <param name="y1"></param>
		/// <param name="m1"></param>
		/// <param name="y2"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		IList<KqjUserMonthLog> Gets(string sfzhm, string y1, string m1, string y2, string m2);
        /// <summary>
        /// 更新月工资册
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="realpay"></param>
        /// <param name="advance"></param>
        /// <param name="paid"></param>
        /// <returns></returns>
        bool UpdateUserMonthPay(string gcbh, string userid, string realname, string year, string month, string gzgz, string shouldpay, string realpay, string advance, string worknum);

        bool JudgeMonthPayLog(string gcbh, string userid, string realname, string year, string month,out string msg);
        /// <summary>
        /// 写入工资明细
        /// </summary>
        /// <param name="username"></param>
        /// <param name="GzName"></param>
        /// <param name="logyear"></param>
        /// <param name="logmonth"></param>
        /// <param name="Bc"></param>
        /// <param name="workcontent"></param>
        /// <param name="classprice"></param>
        /// <param name="areanum"></param>
        /// <param name="unitprice"></param>
        /// <param name="summoney"></param>
        /// <returns></returns>
        bool SetMonthDetails(string recid,string gcbh,string realname, string GzName, string logyear, string logmonth, string Bc, string workcontent, string classprice, string areanum, string unitprice, string summoney);

        bool SetMonthDetailsPrice(string recid, string gcbh, string price, string userid, string realname, string logyear, string logmonth, string shouldpay);
    }
}
