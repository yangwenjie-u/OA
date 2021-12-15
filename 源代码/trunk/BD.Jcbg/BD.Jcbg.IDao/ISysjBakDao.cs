using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	public interface ISysjBakDao 
	{
		SysjBak Get(int recid);
	}
}
