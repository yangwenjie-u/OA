using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface INewsMenuService
    {
        IList<NewsMenu> GetAllUseByDisp();

        NewsMenu Get(int id);

        void SaveOrUpdate(NewsMenu itm);

        void Delete(NewsMenu itm);
    }
}
