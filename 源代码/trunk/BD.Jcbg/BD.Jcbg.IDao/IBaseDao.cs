using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IDao
{
	public interface IBaseDao<T, TId>
	{
		T Get(TId id);
		IList<T> GetAll();
		T Save(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Clear();
		void Delete(TId id);
	}
}
