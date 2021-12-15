using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{
    public class NewsMenuService : INewsMenuService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        INewsMenuDao NewsMenuDao { get; set; }
        #endregion

        public IList<NewsMenu> GetAllUseByDisp()
        {
            return NewsMenuDao.GetAllUseByDisp();
        }

        public NewsMenu Get(int id) {
            return NewsMenuDao.Get(id);
        }


        public void SaveOrUpdate(NewsMenu itm) {
             NewsMenuDao.Update(itm);
        }

        public void Delete(NewsMenu itm)
        {
            NewsMenuDao.Delete(itm);
        }
    }
}
