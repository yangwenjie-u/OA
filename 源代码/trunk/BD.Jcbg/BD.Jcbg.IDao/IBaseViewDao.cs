using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IDao
{
	public interface IBaseViewDao<T, TId>
	{
		T Get(TId id);
		IList<T> GetAll();
	}
}
