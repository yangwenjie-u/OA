using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{
    public class NewsCategoryService : INewsCategoryService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        INewsCategoryDao NewsCategoryDao { get; set; }
        #endregion


        public IList<NewsCategory> GetByLeafTrue()
        {
            return NewsCategoryDao.GetByLeafTrue();
        }

        public IList<NewsCategory> GetByFatherId(string fatherId)
        {
            return NewsCategoryDao.GetByFatherId(fatherId);
        }

        public IList<NewsCategory> GetByCategoryId(int categoryId)
        {
            return NewsCategoryDao.GetByCategoryId(categoryId);
        }

       public NewsCategory Get(int id) {
           return NewsCategoryDao.Get(id);
       }


       public void SaveOrUpdate(NewsCategory itm)
       {
           NewsCategoryDao.Update(itm);
       }

       public void Delete(NewsCategory itm)
       {
           NewsCategoryDao.Delete(itm);
       }
    }
}
