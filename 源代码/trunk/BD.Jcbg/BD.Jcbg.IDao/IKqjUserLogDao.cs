using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 考勤日志表操作
	/// </summary>
	public interface IKqjUserLogDao
	{
		/// <summary>
		/// 获取一条考勤日志
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		KqjUserLog Get(int id);
		/// <summary>
		/// 保存一条考勤日志
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool Save(KqjUserLog entity);
		/// <summary>
		/// 删除一条考勤日志
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool Delete(int id);
		/// <summary>
		/// 获取一个单位的考勤记录
		/// </summary>
		/// <param name="companyid"></param>
		/// <returns></returns>
		IList<KqjUserLog> Gets(string companyid);
		/// <summary>
		/// 获取某个人在某个地方某天的考勤记录
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="projectid"></param>
		/// <param name="logday"></param>
		/// <returns></returns>
		IList<KqjUserLog> Gets(string userid, string projectid, DateTime logday);
	}
}
