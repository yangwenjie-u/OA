using BD.Jcbg.DataModal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Jcbg.IBll
{
    public interface INewsArtcleService
    {
        NewsArtcle Get(int id);

        void Update(NewsArtcle itm);

        IList<IDictionary<string, string>> getNewsArtcles(int cid, int rid, int page, int size, out int totalCount);


        /// <summary>
        /// 保存新闻信息(增加和修改)
        /// </summary>
        /// <param name="newsArtcle"></param>
        /// <param name="newsAttachList"></param>
        /// <param name="type"></param>
        /// <param name="createby"></param>
        /// <returns></returns>
        bool saveNewsArtcle(NewsArtcle newsArtcle, IList<NewsAttach> newsAttachList, string type,bool delimageUrl,bool delfileName, out string msg);



        /// <summary>
        /// 删除新闻信息
        /// </summary>
        /// <param name="newsArtcle"></param>
        /// <returns></returns>
        bool deleteNewsArtcle(NewsArtcle newsArtcle, out string msg);


        NewsArtcle Save(NewsArtcle newsArtcle);

    }
}
