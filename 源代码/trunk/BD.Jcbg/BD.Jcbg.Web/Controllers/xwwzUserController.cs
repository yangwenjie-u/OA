using BD.Jcbg.IBll;
using BD.Jcbg.Web.xwwz;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD.Jcbg.Common;
using BD.Jcbg.DataModal.Entities;
using System.Web.Script.Serialization;

namespace BD.Jcbg.Web.Controllers
{
    public class xwwzUserController : Controller
    {



        #region 服务
        private ICommonService _commonService = null;
        private ICommonService CommonService
        {
            get
            {
                if (_commonService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _commonService = webApplicationContext.GetObject("CommonService") as ICommonService;
                }
                return _commonService;
            }
        }

        private static INewsArtcleService _newsArtcleService = null;
        private static INewsArtcleService NewsArtcleService
        {
            get
            {
                if (_newsArtcleService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _newsArtcleService = webApplicationContext.GetObject("NewsArtcleService") as INewsArtcleService;
                }
                return _newsArtcleService;
            }
        }
        private static INewsMenuService _newsMenuService = null;
        private static INewsMenuService NewsMenuService
        {
            get
            {
                if (_newsMenuService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _newsMenuService = webApplicationContext.GetObject("NewsMenuService") as INewsMenuService;
                }
                return _newsMenuService;
            }
        }


        private static INewsCategoryService _newsCategoryService = null;
        private static INewsCategoryService NewsCategoryService
        {
            get
            {
                if (_newsCategoryService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _newsCategoryService = webApplicationContext.GetObject("NewsCategoryService") as INewsCategoryService;
                }
                return _newsCategoryService;
            }
        }


        private static INewsAttachService _newsAttachService = null;
        private static INewsAttachService NewsAttachService
        {
            get
            {
                if (_newsAttachService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _newsAttachService = webApplicationContext.GetObject("NewsAttachService") as INewsAttachService;
                }
                return _newsAttachService;
            }
        }


        #endregion





        public ActionResult Index()
        {
          
          
            return View();
        }



        public ActionResult searchnewlb() {

            string zncontent = Request["zncontent"].GetSafeString();
            int page = Request["page"].GetSafeInt();
            int size = Request["size"].GetSafeInt();

            if (page <= 0)
                page = 1;
            if (size <= 0)
                size = 20;
            int pageCount;

            string sql = "select t4.recid as newsmenuid,t3.fatherid,t1.* from  News_artcle t1  left join  "
                       + "  News_Category t3 on t1.categoryID = t3.categoryID  "
                       + "  left join   News_Menu t4 on t3.fatherid = t4.categoryID "
                       + " where (t1.articletitle like '%" + zncontent + "%' or  t1.articlekey like '%" + zncontent + "%' or  t1.articlecontent like '%" + zncontent + "%' ) "
                       +" and t1.isAudited=1 and t4.inuse=1 order by articleDate desc";

            IList<IDictionary<string, string>> newsArtcleList = CommonService.GetPageData(sql, size, page, out pageCount);

            ViewData["zncontent"] = zncontent;
            ViewData["newsArtcleList"] = newsArtcleList;
            ViewData["pagetitber"] = " <div id=\"pp\" class=\"easyui-pagination\" data-options=\" total :" + pageCount + " , pageNumber :" + page + " ,pageSize:" + size + " ,  layout:['list','sep','first','prev','links','next','last','sep','refresh','info'] ,displayMsg:'显示 {from} 到 {to} 条数据，总共 {total} 条数据'\"></div>";
           
            return View("newlbsearch");
        }

        public ActionResult news()
        {
            int id = Request["id"].GetSafeInt();
            int rid = Request["rid"].GetSafeInt();
           
            NewsMenu newsMenu = NewsMenuService.Get(rid);
            NewsArtcle newsArtcle = NewsArtcleService.Get(id);
            if (newsArtcle==null)
            {
                return View("newlose");
            }

            IList<IDictionary<string, string>> newsAttachList = CommonService.GetDataTable("select   attachID, articleID, docName, saveName FROM  News_Attach where articleID=" + newsArtcle.Articleid);

            if (newsArtcle == null)
                newsArtcle = new NewsArtcle();
            if (newsMenu == null)
                newsMenu = new NewsMenu();

            newsArtcle.Hits += 1;
            NewsArtcleService.Update(newsArtcle);

            ViewData["NewsArtcle"] = newsArtcle;
            ViewData["newsAttachList"] = newsAttachList;
            ViewData["wz"] = newsMenu.MenuName.GetSafeString();

            return View("news");
        }


        public ActionResult newsc()
        {
            bool flag = true;
            try {
                int id = Request["id"].GetSafeInt();
                int fid = Request["fid"].GetSafeInt();
                int rid = Request["rid"].GetSafeInt();
                
                NewsMenu newsMenu = NewsMenuService.Get(rid);
                NewsArtcle newsArtcle = NewsArtcleService.Get(id);
                if (newsArtcle == null)
                {
                    return View("newlose");
                }

                int cid = newsArtcle.Categoryid;
                IList<NewsCategory> newsCategoryList;
                if (newsMenu == null)
                {
                    flag = false;
                    newsCategoryList = NewsCategoryService.GetByCategoryId(cid);
                }
                else {
                    newsCategoryList = NewsCategoryService.GetByFatherId(newsMenu.CategoryId.Value.ToString());
                }
                 
                NewsCategory newsCategory = NewsCategoryService.Get(cid);

                IList<IDictionary<string, string>> newsAttachList = CommonService.GetDataTable("select   attachID, articleID, docName, saveName FROM  News_Attach where  attachid not in ('" + newsArtcle.ImageUrl + "') and  articleID=" + newsArtcle.Articleid);

                

                if (newsArtcle == null)
                    newsArtcle = new NewsArtcle();
                if (newsMenu == null)
                    newsMenu = new NewsMenu();

                newsArtcle.Hits += 1;
                NewsArtcleService.Update(newsArtcle);

                ViewData["NewsArtcle"] = newsArtcle;
                ViewData["newsAttachList"] = newsAttachList;
                ViewData["newsMenu"] = newsMenu;
                ViewData["newsCategoryList"] = newsCategoryList;
                //ViewData["newsCategory"] = newsCategory;
                ViewData["fid"] = fid;
                ViewData["cid"] = cid;
                ViewData["rid"] = rid;

                if (cid == 0 || newsCategory == null)
                {
                    ViewData["wz"] = newsMenu.MenuName.GetSafeString();
                    ViewData["dh"] = newsMenu.MenuName.GetSafeString();
                }
                else
                {
                    ViewData["wz"] = newsMenu.MenuName.GetSafeString() + ">>" + newsCategory.Name.GetSafeString();
                    ViewData["dh"] = newsCategory.Name.GetSafeString();
                }
            }catch(Exception e){
                SysLog4.WriteError(e.Message);
            }

            if (flag)
            {
                return View("newsc");
            }
            else {
                return View("news");
            }
            
        }


 
        
        public ActionResult newlb()
        {
            int fid = Request["fid"].GetSafeInt();
            int rid = Request["rid"].GetSafeInt();
            int cid = Request["cid"].GetSafeInt();
            int page = Request["page"].GetSafeInt();
            int size = Request["size"].GetSafeInt();
            NewsMenu newsMenu = NewsMenuService.Get(rid);
            IList<NewsCategory> newsCategoryList;
            if (newsMenu == null)
            {
                newsCategoryList = NewsCategoryService.GetByCategoryId(cid);
            }
            else
            {
                newsCategoryList = NewsCategoryService.GetByFatherId(newsMenu.CategoryId.Value.ToString());
            }
            NewsCategory newsCategory = NewsCategoryService.Get(cid);

            if (page <= 0)
                page = 1;
            if (size <= 0)
                size = 10;
            int pageCount;
            IList<IDictionary<string, string>> newsArtcleList = NewsArtcleService.getNewsArtcles(cid, rid, page, size, out pageCount);


            ViewData["newsArtcleList"] = newsArtcleList;
            ViewData["newsMenu"] = newsMenu;
            ViewData["newsCategoryList"] = newsCategoryList;
            //ViewData["newsCategory"] = newsCategory;
            ViewData["fid"] = fid;
            ViewData["cid"] = cid;
            ViewData["rid"] = rid;
            ViewData["pagetitber"] = " <div id=\"pp\" class=\"easyui-pagination\" data-options=\" total :" + pageCount + " , pageNumber :" + page + " ,pageSize:" + size + " ,  layout:['list','sep','first','prev','links','next','last','sep','refresh','info'] ,displayMsg:'显示 {from} 到 {to} 条数据，总共 {total} 条数据'\"></div>";


          
            if (cid == 0 || newsCategory == null)
            {
                ViewData["wz"] = newsMenu.MenuName.GetSafeString();
                ViewData["dh"] = newsMenu.MenuName.GetSafeString();
            }
            else {
                ViewData["wz"] = newsMenu.MenuName.GetSafeString()+">>"+newsCategory.Name.GetSafeString();
                ViewData["dh"] = newsCategory.Name.GetSafeString();
            }

            return View("newlb");
        }



        /// <summary>
        /// 图片列表
        /// </summary>
        /// <returns></returns>
        public string tplist()
        {

            IList<IDictionary<string, string>> newsArtcleList =  new List<IDictionary<string, string>>();
            try
            {
                int num = Request["num"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();
                string isimage = Request["isimage"].GetSafeString();

                string sql = " select top " + num + "  t1.articletitle,t1.isfile,t1.islink,t1.articlelink,t2.attachid as id,t2.savename as name,t4.recid as newsmenuid,t3.fatherid,t3.categoryID,t1.articleID as aid"
                           + " from  News_artcle t1 left join News_Attach t2 on t1.imageUrl = t2.attachid      "
                           + "  left join   News_Category t3 on t1.categoryID = t3.categoryID        "
                           + " left join   News_Menu t4 on t3.fatherid = t4.categoryID   "
                           + " where  " + (isimage.Equals("") ? "" : "t1.isImage = '" + isimage + "' and ") + "   t1.isAudited='True'   and  t1.isfile='False'  "// and t4.inuse='True'
                           + (idlist.Equals("") ? "" : " and t3.categoryID in (" + idlist + ")   ") 
                           + " order by t1.articleDate desc   ";

                newsArtcleList = CommonService.GetDataTable(sql);

            }
            catch (Exception ex) {
                SysLog4.WriteError(ex.Message);
            
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(newsArtcleList) + "}";
        }


        /// <summary>
        /// 新闻动态
        /// </summary>
        /// <returns></returns>
        public string xwdt() {
            IList<IDictionary<string, string>> newsArtcleList = new List<IDictionary<string, string>>();
            try
            {
                int num = Request["num"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();
                string sql = " select  top " + num + "  t1.isfile,t1.filename as name,  t1.islink,t1.articlelink,t4.recid as newsmenuid,t3.fatherid,t3.categoryID,t1.articleID as aid, "
                           + " t1.articleTitle,datename(weekday, t1.articleDate) as xq,    "
                           + " RIGHT('0'+ltrim(MONTH(t1.articleDate)),2)+'月'+RIGHT('0'+ltrim(DAY(t1.articleDate)),2)+'日' as rq  "
                           + " from  News_artcle t1  left join   News_Category t3 on t1.categoryID = t3.categoryID  "
                           + " left join   News_Menu t4 on t3.fatherid = t4.categoryID "
                           + (idlist.Equals("") ? "" : " and t3.categoryID in (" + idlist + ")   ")
                           + " where      t1.isAudited='True'  order by t1.articleDate desc  ";// and t4.inuse='True'

                newsArtcleList = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(newsArtcleList) + "}";
        }






        /// <summary>
        /// 通知公告
        /// </summary>
        /// <returns></returns>
        public string tzgg()
        {
            IList<IDictionary<string, string>> newsArtcleList = new List<IDictionary<string, string>>();
            try
            {
                int num = Request["num"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();
                string sql = " select top " + num + "  t1.isfile,t1.filename as name,  t1.islink,t1.articlelink,t4.recid as newsmenuid,t3.fatherid,t3.categoryID,t1.articleID as aid, "
                           + " t1.articleTitle,  CONVERT(varchar(100), t1.articleDate, 23)as rq  "
                           + " from  News_artcle t1  left join   News_Category t3 on t1.categoryID = t3.categoryID  "
                           + " left join   News_Menu t4 on t3.fatherid = t4.categoryID  "
                           + " where      t1.isAudited='True'  "// and t4.inuse='True'
                           + (idlist.Equals("") ? "" : " and t3.categoryID in (" + idlist + ")   ") 
                           + " order by t1.articleDate desc  ";

                newsArtcleList = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(newsArtcleList) + "}";
        }



        /// <summary>
        /// 重大危险源
        /// </summary>
        /// <returns></returns>
        public string zdwxy()
        {
            IList<IDictionary<string, string>> newsArtcleList = new List<IDictionary<string, string>>();
            try
            {
                int num = Request["num"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();
                string sql = " select top " + num + "  t1.isfile,t1.filename as name,  t1.islink,t1.articlelink, t4.recid as newsmenuid,t3.fatherid,t3.categoryID,t1.articleID as aid, "
                           + " t1.articleTitle,  CONVERT(varchar(100), t1.articleDate, 23)as rq  "
                           + " from  News_artcle t1  left join   News_Category t3 on t1.categoryID = t3.categoryID  "
                           + " left join   News_Menu t4 on t3.fatherid = t4.categoryID  "
                           + " where      t1.isAudited='True'   " //and t4.inuse='True'
                           + (idlist.Equals("") ? "" : " and t3.categoryID in (" + idlist + ")   ") 
                           + " and  t1.IsImportant = 'True' order by t1.articleDate desc  ";

                newsArtcleList = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(newsArtcleList) + "}";
        }



        /// <summary>
        /// 单项目查询
        /// </summary>
        /// <returns></returns>
        public string dxmcx()
        {
            IList<IDictionary<string, string>> newsArtcleList = new List<IDictionary<string, string>>();
            try
            {
                int num = Request["num"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();
                string sql = " select  top " + num + " t1.isfile,t1.filename as name,  t1.islink,t1.articlelink, t4.recid as newsmenuid, "
                           + " t3.fatherid,t3.categoryID,t1.articleID as aid,   "
                           + " t1.articleTitle,  CONVERT(varchar(100), t1.articleDate, 23)as rq    "
                           + " from  News_artcle t1  left join   News_Category t3 on t1.categoryID = t3.categoryID   "
                           + " left join   News_Menu t4 on t3.fatherid = t4.categoryID    " //and t4.inuse='True'
                           + " where      t1.isAudited='True'  and t3.categoryID in (" + idlist + ") "
                           + "   order by t1.articleDate desc  ";
                newsArtcleList = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(newsArtcleList) + "}";
        }


        /// <summary>
        /// 工程 差尊
        /// </summary>
        /// <returns></returns>
        public string gclist()
        {
            IList<IDictionary<string, string>> newsArtcleList = new List<IDictionary<string, string>>();
            try
            {
                int num = Request["num"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();


                string sql = "";
                if (idlist == "1")
                {
                    sql = "select top " + num + "  gcmc as articletitle, CONVERT(varchar(100), SLRQ, 23) as rq from i_M_GC where zt not in ('YT','LR') order by SLRQ desc";
                }
                else
                    sql = "select top " + num + "  gcmc as articletitle,CONVERT(varchar(100), SLRQ, 23) as rq from i_M_GC where SGXKZH!='' and zt not in ('YT','LR') order by SLRQ desc";
                
                newsArtcleList = CommonService.GetDataTable(sql);

            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return "{\"success\":true,\"data\":" + jss.Serialize(newsArtcleList) + "}";
        }



        /// <summary>
        /// 质量工作统计
        /// </summary>
        /// <returns></returns>
        public string zlgztj()
        {

            return "{\"success\":true,\"data1\":\"12\",\"data2\":\"122\",\"data3\":\"235576.81\",\"data4\":\"1513447.04\",\"data5\":\"22\",\"data6\":\"84909.8\"}";
        }


        /// <summary>
        /// 计分单
        /// </summary>
        /// <returns></returns>
        public string jfd()
        {

            return "{\"success\":true,\"data\":\"无限计分\"}";
        }


        /// <summary>
        /// 中间列表部分
        /// </summary>
        /// <returns></returns>
        public string middleleft()
        {

            IList<IDictionary<string, string>> newsArtcleList = new List<IDictionary<string, string>>();
            IList<IDictionary<string, string>> newsCategoryList = new List<IDictionary<string, string>>();
            string xlh = "";
            string ret = "";
            try
            {
                int num1 = Request["num1"].GetSafeInt();
                int num2 = Request["num2"].GetSafeInt();
                string idlist = Request["idlist"].GetSafeString();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string sql = " select  top " + num1 + "  t2.fatherid,t2.categoryID,t2.name,t1.recid as newsmenuid from news_menu t1 left join News_Category t2 "
                           + " on t2.fatherid = t1.categoryID where menutype=2 "//inuse=1 and 
                           + (idlist.Equals("") ? "" : " and t2.categoryID in (" + idlist + ")   ") 
                           + " order by disporder ";
                newsCategoryList = CommonService.GetDataTable(sql);
                sql = "";
                for (int i = 0; i < newsCategoryList.Count;i++ )
                {
                    sql = " select top " + num2 + " t3.isfile,t3.filename as name, t3.islink,t3.articlelink,t3.articleID as aid, t3.articleTitle,  CONVERT(varchar(100), t3.articleDate, 23)as rq "
                        + " from news_menu t1 left join News_Category t2 on t2.fatherid = t1.categoryID "
                        + " left join News_artcle t3 on t3.categoryID = t2.categoryID "
                        +" where inuse=1 and menutype=2  "
                        + " and t2.categoryID = " + newsCategoryList[i]["categoryid"].GetSafeInt() + " and t3.isAudited='True' "
                        + (idlist.Equals("") ? "" : " and t2.categoryID in (" + idlist + ")   ") 
                        +" order by t3.articledate desc ";
                 
                    newsArtcleList = CommonService.GetDataTable(sql);

                    xlh += ",\"fid" + i + "\":\"" + newsCategoryList[i]["fatherid"].GetSafeInt() + "\",\"cid" + i + "\":\"" + newsCategoryList[i]["categoryid"].GetSafeInt() + "\",\"rid" + i + "\":\"" + newsCategoryList[i]["newsmenuid"].GetSafeInt() + "\",\"name" + i + "\":\"" + newsCategoryList[i]["name"].GetSafeString() + "\"," + "\"data" + i + "\":" + jss.Serialize(newsArtcleList);
                }
                if (xlh.Length > 0)
                    ret = "{\"success\":true,\"length\":\"" + newsCategoryList.Count + "\"" + xlh + "}";
                else
                    ret = "{\"success\":false}";
                
            }
            catch (Exception ex)
            {
                SysLog4.WriteError(ex.Message);

            }

            return ret;
        }

        

        /// <summary>
        /// 获取附件
        /// </summary>
        public void getAttachFile()
        {
            byte[] ret = null;
           
            string err = "";
            string attachname = "";
            int id = Request["id"].GetSafeInt();
            string name = Request["name"].GetSafeString();
            string attachID = Request["aid"].GetSafeString();
            try
            {
                NewsAttach newsAttach = new NewsAttach();
               // string sql = "";
                if (attachID != null & !attachID.Equals(""))
                    newsAttach = NewsAttachService.Get(attachID.GetSafeInt()); //sql = "SELECT    docname, filecontent FROM     News_Attach WHERE   attachID = " + attachID;
                else
                    newsAttach = NewsAttachService.Get(name.GetSafeInt());//newsAttach = NewsAttachService.GetByArticleidAndSavename(id, name); //sql = "SELECT    docname, filecontent FROM     News_Attach WHERE   saveName='" + name + "' and  articleID = " + id;

                //IList<IDictionary<string, object>> dt = CommonService.GetDataTable2(sql);
                if (newsAttach.Filecontent!=null)//dt.Count > 0
                {
                   // ret = dt[0]["filecontent"] as byte[];
                    //attachname = dt[0]["docname"] as string;
                    ret = newsAttach.Filecontent;
                    attachname =  newsAttach.DocName;

                    /*
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(attachname));
                    //Response.AddHeader("Content-Length", filesize.ToString());
                    Response.BinaryWrite(ret);
                    Response.Flush();
                    Response.End();
                    */


                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(attachname));
                    System.IO.Stream fs = this.Response.OutputStream;
                    fs.Write(ret, 0, ret.Length);
                    fs.Close();
                    Response.End();

                }
            }
            catch (Exception e)
            {
                err = e.Message;
                SysLog4.WriteLog(e);
            }

        }
    }
}