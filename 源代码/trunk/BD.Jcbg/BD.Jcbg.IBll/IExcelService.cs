using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using System.IO;

namespace BD.Jcbg.IBll
{
	public interface IExcelService
	{
		/// <summary>
		/// 导入工资
		/// </summary>
		/// <param name="filepath"></param>
		bool ImportWage(string filepath, int year, int month, string curusername, string currealname,
			IList<KeyValuePair<string, string>> users, out string msg);
		/// <summary>
		/// 导入加班费
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="extrainfo"></param>
		/// <param name="curusername"></param>
		/// <param name="currealname"></param>
		/// <param name="users"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool ImportExtraWage(string filepath, int year, int month, string extrainfo, string curusername, string currealname,
			IList<KeyValuePair<string, string>> users, out string msg);
        /// <summary>
        /// 把excel得第一个sheet解析成表格数据
        /// </summary>
        /// <param name="filecontent"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        IList<IDictionary<string, string>> ParseExcel(Stream content, out string msg);

    }
}
