using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{
    public class NewsAttachService : INewsAttachService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        INewsAttachDao NewsAttachDao { get; set; }
        #endregion

        public NewsAttach Get(int attachID) {

            return NewsAttachDao.Get(attachID);
        }

        public NewsAttach GetByArticleidAndSavename(int articleID, string saveName) {
            NewsAttach newsAttach = new NewsAttach();
            IList<NewsAttach> ret = NewsAttachDao.GetByArticleidAndSavename(articleID, saveName);
            if(ret.Count>0){
                newsAttach = ret[0];
            }
            return newsAttach;
        }
        public IList<NewsAttach> GetByArticleId(int articleID) {

            return NewsAttachDao.GetByArticleId(articleID);
        }



        public bool deleteNewsAttach(int attachID, out string msg)
        {
            msg = "删除成功!";
            try
            {
                NewsAttachDao.Delete(attachID);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;

        }


        public bool saveNewsAttach(NewsAttach newsAttach, out string msg)
        {
            msg = "上传成功!";
            try
            {
                NewsAttachDao.Save(newsAttach);

            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }

            return true;
        }


        public NewsAttach  Save(NewsAttach newsAttach)
        {
            return NewsAttachDao.Save(newsAttach);
        }
    }
}
