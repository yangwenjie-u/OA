using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	/// <summary>
	/// 工种工作操作
	/// </summary>
	public interface IInfoGzgzDao
	{
		/// <summary>
		/// 获取一个单位一个工地的工种工资设置
		/// </summary>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <returns></returns>
		IList<InfoGzgz> Gets(string companyid, string projectid);
		/// <summary>
		/// 获取一个单位某个工地某个班的某个工种的工资设置
		/// </summary>
		/// <param name="companyid"></param>
		/// <param name="projectid"></param>
		/// <param name="scheduleid"></param>
		/// <param name="gzid"></param>
		/// <returns></returns>
		InfoGzgz Get(string companyid, string projectid, string scheduleid, string gzid);
		
	}
}
