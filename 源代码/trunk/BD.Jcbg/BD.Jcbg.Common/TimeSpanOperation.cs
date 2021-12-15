using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
	public static class TimeSpanOperation
	{
		/// <summary>
		/// 获取时间的TimeSpan结构，时间以 HH:mm表示
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static TimeSpan GetTimeSpan(string time)
		{
			TimeSpan ts = new TimeSpan();
			try
			{
				if (time.Length == 0)
					return ts;
				int dayindex = time.IndexOf(".");
				int day = 0;
				int h = 0;
				int m = 0;
				int s = 0;
				if (dayindex > -1)
				{
					day = DataFormat.GetSafeInt(time.Substring(0, dayindex));
					time = time.Substring(dayindex + 1);
				}
				string[] arr = time.Split(new char[] { ':' });
				if (arr.Length > 0)
					h = DataFormat.GetSafeInt(arr[0]);
				if (arr.Length > 1)
					m = DataFormat.GetSafeInt(arr[1]);
				if (arr.Length > 2)
					s = DataFormat.GetSafeInt(arr[2]);
				ts = new TimeSpan(day, h, m, s);
			}
			catch (Exception e)
			{
                SysLog4.WriteLog(e);
			}
			return ts;
		}
		/// <summary>
		/// 如果开始时间大于结束时间，返会结束时间加24小时，否则返回结束时间
		/// </summary>
		/// <param name="startTime"></param>
		/// <param name="endtime"></param>
		/// <returns></returns>
		public static TimeSpan GetEndTime(string startTime, string endtime)
		{
			TimeSpan ts = GetTimeSpan(startTime);
			TimeSpan te = GetTimeSpan(endtime);
			if (ts.CompareTo(te) == 1)
			{
				te = new TimeSpan(1, te.Hours, te.Minutes, te.Seconds);
			}
			return te;
		}
		/// <summary>
		/// 判断dt是否在时间区间
		/// </summary>
		/// <param name="starttime"></param>
		/// <param name="endtime"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static bool IsTimeIn(string starttime, string endtime, DateTime dt)
		{
			TimeSpan ts = GetTimeSpan(starttime);
			TimeSpan te = GetEndTime(starttime, endtime);
			TimeSpan tc1 = dt.TimeOfDay;
			return tc1.TotalMinutes >= ts.TotalMinutes && tc1.TotalMinutes <= te.TotalMinutes;
		}
	}
}
