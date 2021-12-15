using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.DataModal.VirutalEntity
{
	[Serializable]
	public class VUserCalendar
	{
		public string id { get; set; }
		public string title { get; set; }
		public string start { get; set; }
		public string end { get; set; }
		public string url { get; set; }
		public bool allDay { get; set; }
		public string color { get; set; }
		public string username { get; set; }
		public string realname { get; set; }
		public bool canEdit { get; set; }
	}
}
