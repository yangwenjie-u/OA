using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.Common;

namespace BD.Jcbg.DataModal.VirutalEntity
{
	[Serializable]
	public class VLedInfo
	{
		public bool Error { get; set; }
		public string ErrorMsg { get; set; }
		public string CurDate { get; set; }
		public string CurWeek { get; set; }
		public string Weather { get; set; }
		public string Person { get; set; }

		public VLedInfo()
		{
			ErrorMsg = "";
			Error = false;
			CurDate = DateTime.Now.ToString("yyyy年MM月dd日");
			CurWeek = DataFormat.GetCnWeekday(DateTime.Now.DayOfWeek);

		}
	}
}
