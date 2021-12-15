using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using BD.Jcbg.IBll;
using BD.Jcbg.IDao;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BD.Jcbg.Bll
{
    public class NewsArtcleService : INewsArtcleService
    {
        #region 用到的Dao
        ICommonDao CommonDao { get; set; }
        INewsArtcleDao NewsArtcleDao { get; set; }

        INewsAttachDao NewsAttachDao { get; set; }
        #endregion

        public NewsArtcle Get(int id) {
            NewsArtcle newsArtcle = NewsArtcleDao.Get(id);
            if (newsArtcle!=null)
            {
                newsArtcle.ArticleContent = newsArtcle.ArticleContent.Replace("UserFiles/File", "dyc_1_0/1").Replace("UserFiles/newsfiles", "dyc_1_0/2").Replace("UserFiles/newsimages", "dyc_1_0/3");
            }
            
            return newsArtcle;
        }

        public void Update(NewsArtcle itm)
        {
             NewsArtcleDao.Update(itm);
        }

        public IList<IDictionary<string, string>> getNewsArtcles(int cid, int rid, int page, int size, out int totalCount) {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            totalCount = 0;
            try
            {
                string sql = "";
                if (cid == 0)
                {
                    sql = "select * from   News_artcle where  isAudited=1  and categoryID in ("
                        + " select t1.categoryID from  News_Category t1 left join "
                        + " News_Menu t2 on t1.fatherID = t2.CategoryId where t2.recid= " + rid
                        + ") order by articleDate desc";
                }
                else 
                {
                    sql = "select * from   News_artcle where categoryID=" + cid + " and isAudited=1 order by articleDate desc";
                }
                ret = CommonDao.GetPageData(sql, size, page, out totalCount);
            }
            catch (Exception e)
            {
                SysLog4.WriteLog(e);
            }
            return ret;
        }



        [Transaction(ReadOnly = false)]
        public bool saveNewsArtcle(NewsArtcle newsArtcle, IList<NewsAttach> newsAttachList, string type, bool delimageUrl, bool delfileName, out string msg)
        {
           
            
            
            msg = "保存成功!";
            try {
               
                NewsArtcle newsArtcle_ = new NewsArtcle();


                if (type.Equals("edit"))
                {

                  //  string sql = "SELECT   t2.attachid FROM    News_artcle t1 left join   News_Attach t2 "
                //               + " on t1.articleID = t2.articleID  where t1.articleID = " + newsArtcle.Articleid;


                   // IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();

                    if (delimageUrl && !newsArtcle.ImageUrl.GetSafeString().Equals(""))//删除图片新闻附件
                    {
                      //  ret = CommonDao.GetDataTable(sql + " and t1.imageUrl = t2.savename");
                      //  for (int i = 0; i < ret.Count; i++)
                      //  {
                          NewsAttachDao.Delete(newsArtcle.ImageUrl.GetSafeInt());
                       // }

                    }
                    if (delfileName && !newsArtcle.FileName.GetSafeString().Equals(""))//删除文件新闻附件
                    {
                       // ret = CommonDao.GetDataTable(sql + " and t1.fileName = t2.savename");
                      //  for (int i = 0; i < ret.Count; i++)
                      //  {
                           NewsAttachDao.Delete(newsArtcle.FileName.GetSafeInt());
                     //   }

                    }

                    newsArtcle_ = newsArtcle;
                    NewsArtcleDao.Update(newsArtcle_);
                }
                else
                {
                    newsArtcle_ = NewsArtcleDao.Save(newsArtcle);
                    if (newsArtcle_.Articleid<=0)
                    {
                        msg = "新增失败!";
                        return false;
                    }

                }

                bool imageUrlflag = false;
                bool fileNameflag = false;


                for (int i = 0; i < newsAttachList.Count; i++)
                {

                    NewsAttach newsAttach_ = new NewsAttach();
                    newsAttachList[i].Articleid = newsArtcle_.Articleid;

                    if (newsAttachList[i].SaveName.StartsWith("Fa80cfdec-9dd3-4b33-b3a4-377d53229ee9_xwtp_"))
                    {
                        imageUrlflag = true;
                    }
                    if (newsAttachList[i].SaveName.StartsWith("Fa80cfdec-9dd3-4b33-b3a4-377d53229ee9_xwwj_"))
                    {
                        fileNameflag = true;
                    }
                    newsAttachList[i].SaveName = newsAttachList[i].DocName;
                    newsAttach_ = NewsAttachDao.Save(newsAttachList[i]);
                    if (imageUrlflag)
                    {
                        newsArtcle_.ImageUrl = newsAttach_.Attachid.GetSafeString();
                        imageUrlflag = false;
                    }
                    if (fileNameflag)
                    {
                        newsArtcle_.FileName = newsAttach_.Attachid.GetSafeString();
                        fileNameflag = false;
                    }
                    /**
                     string sqlstr = "INSERT INTO   News_Attach (articleID,docName,saveName,filecontent)VALUES( @articleID, @docName, @saveName, @filecontent )";
                     IList<IDataParameter> sqlparams = new List<IDataParameter>();
                     IDataParameter sqlparam = new SqlParameter("@articleID", newsAttachList[i].Articleid);
                     sqlparams.Add(sqlparam);
                     sqlparam = new SqlParameter("@docName", newsAttachList[i].DocName);
                     sqlparams.Add(sqlparam);
                     sqlparam = new SqlParameter("@saveName", newsAttachList[i].SaveName);
                     sqlparams.Add(sqlparam);
                     sqlparam = new SqlParameter("@filecontent", newsAttachList[i].Filecontent);
                     sqlparams.Add(sqlparam);
                     CommonDao.ExecCommand(sqlstr, CommandType.Text, sqlparams);
                       */
                }


                NewsArtcleDao.Update(newsArtcle_);

            }catch(Exception e){

                SysLog4.WriteLog(e);
                msg = e.Message;
                return false;
            }


            return true;
        }




       public  bool deleteNewsArtcle(NewsArtcle newsArtcle,out string msg) {
           msg = "删除成功!";
           try{

               NewsArtcleDao.Delete(newsArtcle.Articleid);
               NewsAttachDao.DeleteFromArticleId(newsArtcle.Articleid);
               

           }catch(Exception e){
               SysLog4.WriteLog(e);
               msg = e.Message;
               return false;
           }

           return true;

        }


        [Transaction(ReadOnly = false)]
        public NewsArtcle Save(NewsArtcle newsArtcle)
        {

            return NewsArtcleDao.Save(newsArtcle);
        }


    }
}
