using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IDao
{
    public interface INewsAttachDao : IBaseDao<NewsAttach, int>
    {

        IList<NewsAttach> GetByArticleidAndSavename(int articleID, string saveName);

        IList<NewsAttach> GetByArticleId(int articleID);

        void DeleteFromArticleId(int articleID);
    }
}
