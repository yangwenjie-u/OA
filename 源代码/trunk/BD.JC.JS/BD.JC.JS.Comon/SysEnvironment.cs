using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace BD.JC.JS.Common
{
	public class SysEnvironment
	{
		public static string CurPath
		{
			get
			{
				return CurWebPath;
			}
		}
		// 当前Web绝对路径
		protected static string CurWebPath
		{
			get
			{
				return HttpRuntime.AppDomainAppPath;
			}
		}
	}
}
