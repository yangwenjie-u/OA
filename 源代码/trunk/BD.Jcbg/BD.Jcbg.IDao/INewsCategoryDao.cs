using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IDao
{
    public interface INewsCategoryDao : IBaseDao<NewsCategory, int>
    {


        IList<NewsCategory> GetByLeafTrue();
        IList<NewsCategory> GetByFatherId(string fatherId);

        IList<NewsCategory> GetByCategoryId(int categoryId);
        
    }
}
