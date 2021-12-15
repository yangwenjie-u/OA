using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BD.Jcbg.DataModal.Entities;

namespace BD.Jcbg.IDao
{
    public interface IKqUserSignDao : IBaseDao<KqUserSign, int>
	{
        KqUserSign Get(string username, string signdate);
        void Updatesign(KqUserSign itm);
	}
}
