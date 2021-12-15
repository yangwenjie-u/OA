using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
	public interface IDcLogRedoDao 
	{
		IList<DcLogRedo> Gets(string uniqcode);
		DcLogRedo Get(int recid);
	}
}
