using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface INewsAttachService 
    {


        NewsAttach Get(int attachID);

        NewsAttach GetByArticleidAndSavename(int articleID, string saveName);
        IList<NewsAttach> GetByArticleId(int articleID);


        bool deleteNewsAttach(int attachID, out string msg);

        bool saveNewsAttach(NewsAttach newsAttach, out string msg);

        NewsAttach Save(NewsAttach newsAttach);

    }
}
