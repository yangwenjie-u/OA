using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface INewsCategoryService
    {

        IList<NewsCategory> GetByLeafTrue();
        IList<NewsCategory> GetByFatherId(string fatherId);
        IList<NewsCategory> GetByCategoryId(int categoryId);
        NewsCategory Get(int id);

        void SaveOrUpdate(NewsCategory itm);

        void Delete(NewsCategory itm);
    }
}
