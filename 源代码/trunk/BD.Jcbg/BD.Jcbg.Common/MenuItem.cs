using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Common
{
	[Serializable]
	public class MenuItem
	{
		public string MenuCode { get; set; }		
		public string ParentCode { get; set; }
		public string MenuName { get; set; }
		public string MenuUrl { get; set; }
		public decimal DisplayOrder { get; set; }
		public string ImageUrl { get; set; }
		public bool IsMenu { get; set; }
		public bool IsGroup { get; set; }
        public string Memo { get; set; }
        public string Djcd { get; set; }

        public string Procode { get; set; }
	}
}
