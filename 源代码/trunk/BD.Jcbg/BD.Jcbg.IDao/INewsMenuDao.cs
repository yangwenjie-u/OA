using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IDao
{
    public interface INewsMenuDao : IBaseDao<NewsMenu, int>
    {
        IList<NewsMenu> GetAllUseByDisp();
    }
}
